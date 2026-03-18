using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelApp.Models
{
    public class TravelPreDetails
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public int ExchangeRate { get; set; }
        public int AmountINR { get; set; }
        public string Description { get; set; }
        public int TravelId { get; set; }
    }
}