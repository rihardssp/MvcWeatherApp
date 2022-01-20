using DataAccessLayer.MigrationExtensions.Operations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace DataAccessLayer.MigrationExtensions
{
    public static class MigrationBuilderExtensions
    {
        public static void SetIdentity<Model>(
            this MigrationBuilder migrationBuilder,
            bool enabled) where Model : class
        {
            var tableName = typeof(Model).Name;
            var operation = new SetIdentityOperation { Enabled = enabled, TableName = tableName };
            migrationBuilder.Operations.Add(operation);
        }

        public static void CreateWeatherType(
            this MigrationBuilder migrationBuilder,
            int id, 
            string name)
        {
            var operation = new CreateWeatherAttributeTypeOperation { Id = id, Name = name };
            migrationBuilder.Operations.Add(operation);
        }
    }
    
}
