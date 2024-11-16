using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer.Models
{
    public interface ILabelCreator
    {
        public Label CreateLabel();
    }
}
