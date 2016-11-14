using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Contrib.SqlGenerators
{
    interface ISqlGenerator
    {
        string GenerateSqlStatement<T>(T person) where T : class;
    }
}
