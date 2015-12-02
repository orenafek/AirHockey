using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyMobileService.DataObjects
{
    public class TimeLimitedGame : Game
    {
        public int PlayerScore { get; set; }
        public int RobotScore { get; set; }
    }
}