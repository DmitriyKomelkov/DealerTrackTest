using System;

namespace DealerTrack.Models.Deals
{
    public class DealCsvDto
    {
        public int DealNumber { get; set; }
        public string CustomerName { get; set; }
        public string DealershipName { get; set; }
        public string Vehicle { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}