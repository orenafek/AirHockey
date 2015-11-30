using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HockeyApp
{
    public class Utils
    {
        public static async void Show(string text, List<UICommand> commands, uint defaultCommandIndex = 0, uint cancelCommandIndex = 0)
        {
            var message = new MessageDialog(text);
            foreach (UICommand command in commands)
            {
                message.Commands.Add(command);
            }
            message.DefaultCommandIndex = defaultCommandIndex;
            message.CancelCommandIndex = cancelCommandIndex;
            await message.ShowAsync();
        }
    }
}
