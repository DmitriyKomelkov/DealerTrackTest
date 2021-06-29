using System.Collections.Generic;

namespace DealerTrack.DataBase.Entities
{
    public class Dealership: BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Deal> Deals { get; set; }
    }
}