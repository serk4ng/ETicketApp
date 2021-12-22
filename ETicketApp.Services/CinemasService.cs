using ETicketApp.Core.Models;
using ETicketApp.Core.Services;
using ETicketApp.Data;
using ETicketApp.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketApp.Services
{
    public class CinemasService:EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(ApplicationContext context) : base(context)
        {
        }
    }
}
