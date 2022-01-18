using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using Quartz;
using Services.HttpServices.Abstractions.Weather;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BatchQueue.Jobs
{
    /// <summary>
    /// This job is dedicated to seeding DB with WeatherEntries from APIs.
    /// </summary>
    [DisallowConcurrentExecution]
    public class UpdateWeatherDataJob : IJob
    {
        private ILogger<UpdateWeatherDataJob> _logger;
        private IWeatherRepositoryWrapper _repository;
        private IWeatherApiConsumer _openWeatherConsumer;
        public UpdateWeatherDataJob(ILogger<UpdateWeatherDataJob> logger, IWeatherRepositoryWrapper repository, IWeatherApiConsumer apiConsumer)
        {
            _logger = logger;
            _repository = repository;
            _openWeatherConsumer = apiConsumer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                var locationsAndLastUpdates = (from weather in _repository.WeatherEntry.FindAll()
                                              group weather by weather.LocationId into weatherGroup
                                              select new
                                              {
                                                  weatherGroup.Key,
                                                  lastUpdate = weatherGroup.Max(e => e.Date)
                                              } into lastUpdate
                                              join location in _repository.Location.FindAll()
                                                  on lastUpdate.Key equals location.Id
                                              select new
                                              {
                                                  Id = lastUpdate.Key,
                                                  lastUpdate.lastUpdate,
                                                  location.ApiId
                                              })
                                              .ToList();

                var result = await _openWeatherConsumer.GetRequest(locationsAndLastUpdates.Select(e => e.ApiId).ToArray());
                foreach(var item in result)
                {
                    // Do not put in duplicates or entries in the past. This job only updates current (latest) entries.
                    var location = locationsAndLastUpdates.FirstOrDefault(e => e.ApiId == item.LocationApiId);
                    if (location == null || location.lastUpdate >= item.Date)
                    {
                        var reason = location == null ? "location is null" : $"last update was '{location.lastUpdate}', however received '{item.Date}'. New entry should be newer than the latest.";
                        _logger.LogInformation($"Skipping entry returned by api, because {reason}");
                        continue;
                    }

                    _repository.WeatherEntry.Add(new WeatherEntryModel
                    {
                        LocationId = location.Id,
                        Date = item.Date,
                        Temperature = item.Temperature,
                        WindSpeed = item.WindSpeed
                    });

                    _logger.LogInformation($"Api result sent to DB: {item.Date} {item.LocationApiId}, {item.Temperature}, {item.WindSpeed}");
                }
                
                await _repository.SaveChangesAsync();
            } catch(Exception e)
            {
                throw new JobExecutionException(e);
            }
        }

        public string SomeValue { private get; set; }
    }
}
