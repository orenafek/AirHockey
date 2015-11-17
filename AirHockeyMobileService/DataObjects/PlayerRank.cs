﻿using Microsoft.WindowsAzure.Mobile.Service;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leaderboard.DataObjects
{
    public class PlayerRank : EntityData
    {
        public int Streak { get; set; }
        public int Wins { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }

        [ForeignKey("Id")]
        public virtual Player Player { get; set; }
    }
}