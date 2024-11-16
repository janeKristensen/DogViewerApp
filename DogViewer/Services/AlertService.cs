using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer.Services
{
    public delegate Task AlertOperation(string title, string message);
    public delegate Task<bool> AlertOperationConfirmation(string title, string message);

    public class AlertService
    {
        public event AlertOperation alert;
        public event AlertOperationConfirmation alertConfirm;

        public void Alert(string title, string message)
        {
            alert.Invoke(title, message);
        }

        public void AlertConfirmation(string title, string message)
        {
            alertConfirm.Invoke(title, message);
        }
    }
}
