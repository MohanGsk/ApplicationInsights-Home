using System;

namespace PingMeshWeb.Controllers
{
    public class Stock
    {

            //[Required]
            //[ScaffoldColumn(false)]
            public string StockID { get; set; }
            public DateTime Date { get; set; }
            public double ValueOpen { get; set; }
            public double ValueHigh { get; set; }
            public double ValueLow { get; set; }
            public double ValueClose { get; set; }
            public double TradeVolume { get; set; }
            public double adjClose { get; set; }
        }
}