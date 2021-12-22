using ETicketApp.UI.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketApp.UI.ViewModels
{
    public class ShoppingCartVM
    {
        public ShoppingCart ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
    }
}
