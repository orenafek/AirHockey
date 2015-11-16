using AirHockeyApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace AirHockeyApp.ViewModel
{
    class ScoreBoardViewModel : INotifyPropertyChanged
    {
        MobileServiceClient _client;
        private MobileServiceCollection<Player, Player> _players;
        private MobileServiceCollection<PlayerRank, PlayerRank> _ranks;
        private bool _isPending;
        private string _errorMessege;

        public ScoreBoardViewModel(MobileServiceClient client)
        {
            _client = client;
        }

        public string ErrorMessege
        {
            get
            {
                return _errorMessege;
            }
            set
            {
                _errorMessege = value;
                NotifyPropertyChanged("ErrorMessege");
            }
        }
        public bool isPending
        {
            get
            {
                return _isPending;
            }
            set
            {
                _isPending = value;
                NotifyPropertyChanged("IsPending");
            }
        }
        public MobileServiceCollection<PlayerRank, PlayerRank> Ranks
        {
            get
            {
                return _ranks;
            }
            set
            {
                _ranks = value;
                NotifyPropertyChanged("Ranks");
            }
        }
        public MobileServiceCollection<Player,Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                NotifyPropertyChanged("Players");
            }
        }

      
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, 
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task GetAllPlayersAsync()
        {
            isPending = true;
            ErrorMessege = null;

            try
            {
                IMobileServiceTable<Player> Table = _client.GetTable<Player>();
                Players = await Table.OrderBy(X => (X.LastName + " " + X.FirstName)).ToCollectionAsync();
            }

            catch(MobileServiceInvalidOperationException ex) { ErrorMessege = ex.Message; }
            catch(HttpRequestException ex) { ErrorMessege = ex.Message; }
            finally { isPending = false; }
        }

        public async Task AddPlayerAsync(Player player)
        {
            isPending = true;
            ErrorMessege = null;

            try
            {
                IMobileServiceTable<Player> Table = _client.GetTable<Player>();
                await Table.InsertAsync(player);
                Players.Add(player);
            }

            catch (MobileServiceInvalidOperationException ex) { ErrorMessege = ex.Message; }
            catch (HttpRequestException ex) { ErrorMessege = ex.Message; }
            finally { isPending = false; }
        }

        public async Task SubmitScroeAsync(Player player, int score)
        {
            isPending = true;
            ErrorMessege = null;

            PlayerScore playerScore = new PlayerScore { ID = player.ID, Score = score };

            try
            {
                await _client.InvokeApiAsync<PlayerScore, object>("Score", playerScore);
                await GetAllRanksAsync();
            }

            catch (MobileServiceInvalidOperationException ex) { ErrorMessege = ex.Message; }
            catch (HttpRequestException ex) { ErrorMessege = ex.Message; }
            finally { isPending = false; }
        }

       public async Task GetAllRanksAsync()
        {
            isPending = true;
            ErrorMessege = null;

            try
            {
                IMobileServiceTable<PlayerRank> Table = _client.GetTable<PlayerRank>();
                Ranks = await Table.OrderBy(X => X.Rank).ToCollectionAsync();
            }

            catch (MobileServiceInvalidOperationException ex) { ErrorMessege = ex.Message; }
            catch (HttpRequestException ex) { ErrorMessege = ex.Message; }
            finally { isPending = false; }
        }
    }
}
