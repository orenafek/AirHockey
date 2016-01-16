using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AirHockeyMobileService.DataObjects;
using AirHockeyMobileService.Models;
using System.Web.Http.Description;

namespace AirHockeyMobileService.Controllers
{
    public class ScoreLimitedGameController : TableController<ScoreLimitedGame>
    {
        MobileServiceContext context = new MobileServiceContext();

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new EntityDomainManager<ScoreLimitedGame>(context, Request, Services);
        }

        // GET tables/ScoreLimitedGame
        public IQueryable<ScoreLimitedGame> GetAllScoreLimitedGame()
        {
            return Query(); 
        }

        // GET tables/ScoreLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ScoreLimitedGame> GetScoreLimitedGame(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ScoreLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ScoreLimitedGame> PatchScoreLimitedGame(string id, Delta<ScoreLimitedGame> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ScoreLimitedGame
        [ResponseType(typeof(ScoreLimitedGame))]
        public async Task<IHttpActionResult> PostScoreLimitedGame(ScoreLimitedGame item)
        {
            ScoreLimitedGame current = await InsertAsync(item);
            
            // Update rankings
            // See http://stackoverflow.com/a/575799
            const string updateCommand =
                "UPDATE r SET Rank = ((SELECT COUNT(*)+1 from {0}.ScoreLimitedGames " +
                "where Duration < (select duration from {0}.ScoreLimitedGames where Id = r.Id)))" +
                "FROM {0}.ScoreLimitedGames as r";

            string command = System.String.Format(updateCommand, ServiceSettingsDictionary.GetSchemaName());
            await context.Database.ExecuteSqlCommandAsync(command);

            return Ok(current);
        }

        // DELETE tables/ScoreLimitedGame/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteScoreLimitedGame(string id)
        {
             return DeleteAsync(id);
        }
    }
}