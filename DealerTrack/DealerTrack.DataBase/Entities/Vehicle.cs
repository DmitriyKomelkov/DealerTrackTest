using System.Collections.Generic;

namespace DealerTrack.DataBase.Entities
{
    public class Vehicle: BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Deal> Deals { get; set; }
    }
}