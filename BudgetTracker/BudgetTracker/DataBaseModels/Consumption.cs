using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.DataBaseModels
{
    [Table("Consumptions")]
    public class Consumption
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public string ConsumptionName { get; set; }

        public double ConsumptionPrice { get; set; }

        [Indexed]
        public int DayId { get; set; }
    }
}
