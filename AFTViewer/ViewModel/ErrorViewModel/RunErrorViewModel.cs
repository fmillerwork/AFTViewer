using AFTViewer.Model;
using AFTViewer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTViewer.ViewModel
{
    public class RunErrorViewModel : Observable
    {
        private readonly RunErrorModel model;

        public RunErrorViewModel(RunErrorModel model)
        {
            this.model = model;
        }

        public string TimeStamp
        {
            get => model.TimeStamp;
            set { model.TimeStamp = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => model.ErrorMessage;
            set { model.ErrorMessage = value; OnPropertyChanged(); }
        }

    }
}
