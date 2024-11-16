using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogViewer.Models
{
    

    public class TitleLabelCreator : ILabelCreator
    {
        public Label CreateLabel()
        {
            return new TitleLabel();
        }
    }

    public class SubtitleLabelCreator : ILabelCreator
    {
        public Label CreateLabel()
        {
            return new SubtitleLabel();
        }
    }
}
