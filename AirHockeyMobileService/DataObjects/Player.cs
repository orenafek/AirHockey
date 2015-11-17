using Microsoft.WindowsAzure.Mobile.Service;

namespace Leaderboard.DataObjects
{
    public class Player : EntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}