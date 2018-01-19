using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.DataBaseModels
{
    [Table("Days")]
    public class Day
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public DateTime DayDate { get; set; }

        [Indexed]
        public int MonthId { get; set; }
    }
}
