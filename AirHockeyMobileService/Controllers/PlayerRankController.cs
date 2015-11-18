using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AirHockeyMobileService.Models;
using Leaderboard.DataObjects;
using System;

namespace AirHockeyMobileService.Controllers
{
    public class PlayerRankController : TableController<PlayerRank>
    {
        MobileServiceContext context = new MobileServiceContext();


        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new EntityDomainManager<PlayerRank>(context, Request, Services);
        }

        // GET tables/PlayerRank
        public IQueryable<PlayerRankDto> GetAllPlayerRank()
        {
            return Query().Select(x => new PlayerRankDto()
            {
                Id = x.Id,
                FirstName = x.Player.FirstName,
                LastName = x.Player.LastName,
                Streak = x.Streak,
                Wins = x.Wins,
                Score = x.Score,
                Rank = x.Rank
            });
        }

        // GET tables/PlayerRank/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PlayerRankDto> GetPlayerRank(string id)
        {
            var result = Lookup(id).Queryable.Select(x => new PlayerRankDto()
            {
                Id = x.Id,
                FirstName = x.Player.FirstName,
                LastName = x.Player.LastName,
                Streak = x.Streak,
                Wins = x.Wins,
                Score = x.Score,
                Rank = x.Rank
            });
            return SingleResult.Create(result);
        }

        [Route("api/result")]
        public async Task<IHttpActionResult> PostPlayerResult(PlayerResult result)
        {
            // Does this player exist?
            var count = context.Players.Where(x => x.Id == result.PlayerId).Count();
            if (count < 1)
            {
                return BadRequest();
            }

            // Try to find the PlayerRank entity for this player. If not found, create a new one.
            PlayerRank rank = await context.PlayerRanks.FindAsync(result.PlayerId);
            if (rank == null)
            {
                rank = new PlayerRank { Id = result.PlayerId, Wins = 0 };
                if (result.RobotScore < result.PlayerScore)
                    ++rank.Wins;
                context.PlayerRanks.Add(rank);
            }
            else
            {
                if (result.RobotScore < result.PlayerScore)
                    ++rank.Wins;
            }

            await context.SaveChangesAsync();

            // Update rankings
            // See http://stackoverflow.com/a/575799
            const string updateCommand =
                "UPDATE r SET Rank = ((SELECT COUNT(*)+1 from {0}.PlayerRanks " +
                "where Wins > (select wins from {0}.PlayerRanks where Id = r.Id)))" +
                "FROM {0}.PlayerRanks as r";

            string command = String.Format(updateCommand, ServiceSettingsDictionary.GetSchemaName());
            await context.Database.ExecuteSqlCommandAsync(command);

            return Ok();
        }
    }
}