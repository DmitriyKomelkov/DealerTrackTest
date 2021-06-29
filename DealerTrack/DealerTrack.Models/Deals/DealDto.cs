using System;

namespace DealerTrack.Models.Deals
{
    public class DealDto: BaseDto
    {
        public int DealNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int DealershipId { get; set; }
        public string DealershipName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}