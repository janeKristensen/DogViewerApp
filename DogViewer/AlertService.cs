using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer
{
    public interface IAlertService
    {
        public Task DisplayAlert(string title, string message, string cancel = "OK");
        public Task<bool> DisplayAlertConfirmation(string title, string message, string cancel = "Cancel", string accept="OK");

    }

    public class AlertService : IAlertService
    {
        public Task DisplayAlert(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<bool> DisplayAlertConfirmation(string title, string message, string cancel = "Cancel", string accept = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);  
        }
    }
}
