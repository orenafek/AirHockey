using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyMobileService.DataObjects
{
    public class Game : EntityData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Rank { get; set; }
    }
}