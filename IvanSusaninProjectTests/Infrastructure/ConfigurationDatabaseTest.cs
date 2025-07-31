using IvanSusaninProject_Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProjectTests.Infrastructure
{
    public class ConfigurationDatabaseTest : IConfigurationDatabase
    {
        public string ConnectionString => "Host=localhost;Database=IvanSusanin_Test;Username=postgres;Password=postgres;";
    }
}