using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AirHockeyMobileService.DataObjects;
using AirHockeyMobileService.Models;

namespace AirHockeyMobileService.Controllers
{
    public class TimeLimitedGameController : TableController<TimeLimitedGame>
    {
        MobileServiceContext context = new MobileServiceContext();

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new EntityDomainManager<TimeLimitedGame>(context, Request, Services);
        }

        // GET tables/TimeLimitedGame
        public IQueryable<TimeLimitedGame> GetAllTimeLimitedGame()
        {
            return Query(); 
        }

        // GET tables/TimeLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TimeLimitedGame> GetTimeLimitedGame(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TimeLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TimeLimitedGame> PatchTimeLimitedGame(string id, Delta<TimeLimitedGame> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TimeLimitedGame
        public async Task<IHttpActionResult> PostTimeLimitedGame(TimeLimitedGame item)
        {
            TimeLimitedGame current = await InsertAsync(item);

            // Update rankings
            // See http://stackoverflow.com/a/575799
            const string updateCommand =
                "UPDATE r SET Rank = ((SELECT COUNT(*)+1 from {0}.TimeLimitedGames " +
                "where (PlayerScore - RobotScore) > ((SELECT (playerscore - robotscore) AS diff FROM {0}.TimeLimitedGames WHERE Id = r.Id))))" +
                "FROM {0}.TimeLimitedGames as r";

            string command = System.String.Format(updateCommand, ServiceSettingsDictionary.GetSchemaName());
            await context.Database.ExecuteSqlCommandAsync(command);

            return Ok();
        }

        // DELETE tables/TimeLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTimeLimitedGame(string id)
        {
             return DeleteAsync(id);
        }

        public void DeleteAll()
        {
            string cmd = "TRUNCATE TABLE {0}.TimeLimitedGames";
            string command = string.Format(cmd, ServiceSettingsDictionary.GetSchemaName());
            context.Database.ExecuteSqlCommand(command);
        }

    }
}