using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DataAccessLayer.MigrationExtensions.Operations
{
    public class CreateLocationOperation : MigrationOperation
    {
        public string City { get; set; }
        public string Country { get; set; }
        public int ApiId { get; set; }
    }
}