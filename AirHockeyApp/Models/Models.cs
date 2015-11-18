using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyApp.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class PlayerRank
    {
        public string Id { get; set; } // primary key 1-1
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Streak { get; set; }
        public int Wins { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    public class PlayerResult {
        public int Id { get; set; }
        public int PlayerScore { get; set; }
        public int RobotScore { get; set; }
    }
}

    

