using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessLayer.MigrationExtensions.Operations
{
    public class CreateWeatherAttributeTypeOperation : MigrationOperation
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}