using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyApp.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class PlayerRank
    {
        public string ID { get; set; } // primary key 1-1
        public int Streak { get; set; }
        public int Wins { get; set; }
        public int Rank { get; set; }
    }

    public class PlayerScore {
        public int ID { get; set; }
        public int Score { get; set; }
    }
}

    

