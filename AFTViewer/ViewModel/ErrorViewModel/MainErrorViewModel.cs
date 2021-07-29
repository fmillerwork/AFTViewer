using AFTViewer.Utils;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AFTViewer.ViewModel
{
    public class MainErrorViewModel : Observable
    {
        public MainErrorViewModel()
        {
            RunErrors = new ObservableCollection<RunErrorViewModel>();
            Init();
        }

        private void Init()
        {
            var runErrorModels = Helper.LoadRunErrors();
            foreach (var runErrorModel in runErrorModels)
            {
                RunErrors.Add(new RunErrorViewModel(runErrorModel));
            }
            if (RunErrors.Count != 0)
                SelectedRunError = RunErrors[0];
        }

        public ObservableCollection<RunErrorViewModel> RunErrors { get; set; }

        private RunErrorViewModel selectedRunError;
        public RunErrorViewModel SelectedRunError
        {
            get => selectedRunError;
            set => SetProperty(ref selectedRunError, value);
        }

        public void DeleteRun()
        {
            Directory.Delete(string.Format(Globals.RUN_PATH, SelectedRunError.TimeStamp), true);
            RunErrors.Remove(SelectedRunError);
            if (RunErrors.Count != 0)
                SelectedRunError = RunErrors[0];
        }

        public void ReloadRuns()
        {
            RunErrors.Clear();
            Init();
        }

        public void SetPrevRun()
        {
            if (RunErrors.Count > 0)
            {
                var runIndex = RunErrors.IndexOf(SelectedRunError);
                if (runIndex != 0)
                    SelectedRunError = RunErrors.ElementAt(runIndex - 1);
            }
        }

        public void SetNextRun()
        {
            if (RunErrors.Count > 0)
            {
                var runIndex = RunErrors.IndexOf(SelectedRunError);
                if (runIndex != RunErrors.Count - 1)
                    SelectedRunError = RunErrors.ElementAt(runIndex + 1);
            }
        }

    }
}
