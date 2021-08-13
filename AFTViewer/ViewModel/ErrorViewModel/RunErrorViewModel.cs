using AFTViewer.Model;
using AFTViewer.Utils;
using System.Windows.Media;

namespace AFTViewer.ViewModel
{
    public class RunErrorViewModel : Observable
    {
        private readonly RunErrorModel model;

        public RunErrorViewModel(RunErrorModel model)
        {
            this.model = model;

            // Lecture image erreur
            ErrorCaptureSource = Helper.LoadImage(string.Format(Globals.RUN_PATH, model.TimeStamp) + "_error.png");
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

        private ImageSource errorCaptureSource;
        public ImageSource ErrorCaptureSource
        {
            get => errorCaptureSource;
            set => SetProperty(ref errorCaptureSource, value);
        }
    }
}
