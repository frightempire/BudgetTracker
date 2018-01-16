using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.DataBaseModels
{
    [Table("Months")]
    public class Month
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public string MonthName { get; set; }

        public DateTime MonthDate { get; set; }
    }
}
