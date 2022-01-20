using DataAccessLayer.MigrationExtensions.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessLayer.MigrationExtensions
{
    public class SqlServerMigrationGeneratorExtension : SqlServerMigrationsSqlGenerator
    {
        public SqlServerMigrationGeneratorExtension(
            MigrationsSqlGeneratorDependencies dependencies,
            IRelationalAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations)
        {
        }

        protected override void Generate(
            MigrationOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            if (operation is SetIdentityOperation setIdentityOperation)
            {
                Generate(setIdentityOperation, builder);
            }
            else if (operation is CreateWeatherAttributeTypeOperation createWeatherType) 
            {
                Generate(createWeatherType, builder);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }

        private void Generate(
            SetIdentityOperation operation,
            MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;
            var enabled = operation.Enabled ? "ON" : "OFF";

            builder
                .Append("SET IDENTITY_INSERT ")
                .Append(sqlHelper.DelimitIdentifier(operation.TableName))
                .Append($" {enabled}")
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(
            CreateWeatherAttributeTypeOperation operation,
            MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;
            var stringMapping = Dependencies.TypeMappingSource.FindMapping(typeof(string));
            var stringMappingInt = Dependencies.TypeMappingSource.FindMapping(typeof(int));

            builder.Append("INSERT ")
                .Append(sqlHelper.DelimitIdentifier("WeatherAttributeTypeModel"))
                .Append(" (")
                .Append(sqlHelper.DelimitIdentifier("Id"))
                .Append(", ")
                .Append(sqlHelper.DelimitIdentifier("Name"))
                .Append(") VALUES (")
                .Append(stringMappingInt.GenerateSqlLiteral(operation.Id))
                .Append(",")
                .Append(stringMapping.GenerateSqlLiteral(operation.Name))
                .Append(")")
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }
    }
}
