using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AirHockeyMobileService.DataObjects
{
    public class ScoreLimitedGame : Game
    {
        public TimeSpan Duration { get; set; }
    }
}