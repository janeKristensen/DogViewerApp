using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer
{
    internal class Utils
    {
        #region Error notification delegate methods
        public static Task DisplayAlert(string title, string message)
        { 
            return App.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public static Task LogAlert(string title, string message)
        {
            string path = System.AppContext.BaseDirectory;
            string file = Path.Combine(path, "log.txt");
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine($"{System.DateTime.UtcNow} - Error: {title}, {message}");
            }
            return Task.CompletedTask;
        }

        public static Task<bool> DisplayAlertConfirmation(string title, string message)
        {
            LogAlert(title, message);
            return App.Current.MainPage.DisplayAlert(title, message, "OK", "Cancel");
        }

        public static void SetUpAlerts()
        {
            // Assign delegate methods
            App.AlertService.alert += DisplayAlert;
            App.AlertService.alert += LogAlert;
            App.AlertService.alertConfirm += DisplayAlertConfirmation;
        }
        #endregion
    }
}
