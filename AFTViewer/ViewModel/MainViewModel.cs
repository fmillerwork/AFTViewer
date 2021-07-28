using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AFTViewer.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace AFTViewer.ViewModel
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<RunViewModel> Runs { get; set; }

        private RunViewModel selectedRun;
        public RunViewModel SelectedRun
        {
            get => selectedRun;
            set => SetProperty(ref selectedRun, value);
        }

        public MainViewModel()
        {
            Runs = new ObservableCollection<RunViewModel>();
            Init();
        }

        private void Init()
        {
            var runResultModels = Helper.LoadRunResults();

            foreach (var runResultModel in runResultModels)
            {
                // On ajoute uniquement les Run non vides (avec des failures)
                if (runResultModel.FailureCount != 0)
                    Runs.Add(new RunViewModel(runResultModel) { MainViewModel = this });
            }
            if (Runs.Count != 0)
                SelectedRun = Runs[^1];
        }

        /// <summary>
        /// Recharge la totalité des runs.
        /// </summary>
        public void ReloadRuns()
        {
            Runs.Clear();
            Init();
        }

        /// <summary>
        /// Effectue un rechargement des captures de specifications pour les FailureCaptureViewModel ayant le nom passé en paramètre (de toutes les runs).
        /// </summary>
        /// <param name="failureCaptureName"></param>
        public void RefreshSpecCaptureSources(string failureCaptureName)
        {
            foreach (var run in Runs)
            {
                foreach (var suite in run.TestSuiteViewModels)
                {
                    foreach (var test in suite.TestViewModels)
                    {
                        foreach (var capture in test.FailureCaptureViewModels)
                        {
                            if (capture.CaptureName == failureCaptureName)
                            {
                                var specPath = string.Format(Globals.RUN_PATH, capture.RunName) + capture.Model.SpecCapturePath;
                                capture.SpecCaptureSource = Helper.LoadImage(specPath);
                            }
                        }
                    }
                }
            }
        }

        public void SetPrevRun()
        {
            if(Runs.Count > 0)
            {
                var runIndex = Runs.IndexOf(SelectedRun);
                if (runIndex != 0)
                    SelectedRun = Runs.ElementAt(runIndex - 1);
            }
        }

        public void SetNextRun()
        {
            if (Runs.Count > 0)
            {
                var runIndex = Runs.IndexOf(SelectedRun);
                if (runIndex != Runs.Count - 1)
                    SelectedRun = Runs.ElementAt(runIndex + 1);
            }
        }

        public void DeleteRun()
        {
            Directory.Delete(string.Format(Globals.RUN_PATH, SelectedRun.RunName), true);
            Runs.Remove(SelectedRun);
            if (Runs.Count != 0)
                SelectedRun = Runs[^1];
        }
    }
}
