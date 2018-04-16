using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MeetingSchema.Data
{
    public class MeetingSchemaContextFactory
    {
        public MeetingSchemaContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MeetingSchemaContext>();

            var ConnectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(ConnectionString);

            return new MeetingSchemaContext(builder.Options);
        }
    }
}
