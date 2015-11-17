namespace Leaderboard.DataObjects
{
    public class PlayerRankDto
    {
        public string Id { get; set; } // primary key 1-1
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Streak { get; set; }
        public int Wins { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}