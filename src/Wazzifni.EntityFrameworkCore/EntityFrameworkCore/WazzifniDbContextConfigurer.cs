using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Wazzifni.EntityFrameworkCore
{
    public static class WazzifniDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WazzifniDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WazzifniDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
