using Microsoft.WindowsAzure.Mobile.Service;
using System.Data;

namespace AirHockeyMobileService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}