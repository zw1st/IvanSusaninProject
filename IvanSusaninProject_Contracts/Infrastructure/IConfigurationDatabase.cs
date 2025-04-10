using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Infrastructure;

public interface IConfigurationDatabase
{
    string ConnectionString { get; }
}
