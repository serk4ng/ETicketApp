using ETicketApp.Core.Models;
using ETicketApp.Core.Services;
using ETicketApp.Data;
using ETicketApp.Data.Base;

namespace ETicketApp.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(ApplicationContext context) : base(context) { }
    }
}
