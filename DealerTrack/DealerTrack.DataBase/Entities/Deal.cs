using System;

namespace DealerTrack.DataBase.Entities
{
    public class Deal: BaseEntity
    {
        public int DealNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int DealershipId { get; set; }
        public Dealership Dealership { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}