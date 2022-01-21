using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessLayer.MigrationExtensions.Operations
{
    /// <summary>
    /// Sometimes necessary when making inserts with IDs
    /// </summary>
    public class SetIdentityOperation : MigrationOperation
    {
        public bool Enabled { get; set; }
        public string TableName { get; set; }
    }
}