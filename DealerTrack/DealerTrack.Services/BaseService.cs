using System.Collections.Generic;
using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models;

namespace DealerTrack.Services
{
    public class BaseService
    {
        protected readonly DealerTrackDbContext DealerTrackDbContext;

        public BaseService(DealerTrackDbContext dealerTrackDbContext)
        {
            DealerTrackDbContext = dealerTrackDbContext;
        }
    }
}