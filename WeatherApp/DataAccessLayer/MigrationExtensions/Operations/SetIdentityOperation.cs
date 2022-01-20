using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessLayer.MigrationExtensions.Operations
{
    public class SetIdentityOperation : MigrationOperation
    {
        public bool Enabled { get; set; }
        public string TableName { get; set; }
    }
}