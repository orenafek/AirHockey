using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AirHockeyMobileService.DataObjects;
using AirHockeyMobileService.Models;
using System.Data;

namespace AirHockeyMobileService.Controllers
{
    public class DataController : ApiController
    {
        public ApiServices Services { get; set; }

        public DataViewWrapper getScoreBoardData()
        {
            return new DataViewWrapper(ADO_Data_Service.getView("ScoreBoard"));
        }
    }
}