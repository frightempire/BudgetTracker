using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker
{
    public interface IDbHelper
    {
        string GetDbPath(string dbName);
    }
}
