using AFTViewer.Utils;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AFTViewer.ViewModel
{
    public class MainViewModel : Observable
    {
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
                //if (runResultModel.FailureCount != 0)
                Runs.Add(new RunViewModel(runResultModel) { MainViewModel = this });
            }
            if (Runs.Count != 0)
                SelectedRun = Runs[^1];
        }

        #region Properties
        public ObservableCollection<RunViewModel> Runs { get; set; }

        private RunViewModel selectedRun;
        public RunViewModel SelectedRun
        {
            get => selectedRun;
            set => SetProperty(ref selectedRun, value);
        }
        #endregion

        /// <summary>
        /// Recharge la totalité des runs.
        /// </summary>
        public void ReloadRuns()
        {
            Runs.Clear();
            Init();
        }

        /// <summary>
        /// Supprime le FailureCaptureViewModel dans toutes les runs, supprime les tests, suite ou runs s'ils sont vides et actualise la SelectedCapture.
        /// </summary>
        /// <param name="failureCapture"></param>
        public void OverrideSpecCapture(FailureCaptureViewModel failureCapture)
        {
            foreach(var run in Runs)
            {
                run.OverrideSpecCapture(failureCapture);
            }
        }

        ///// <summary>
        ///// Effectue un rechargement des captures de specifications pour les FailureCaptureViewModel ayant le nom passé en paramètre (de toutes les runs).
        ///// </summary>
        ///// <param name="failureCaptureName"></param>
        //public void RefreshSpecCaptureSources(string failureCaptureName)
        //{
        //    foreach (var run in Runs)
        //    {
        //        foreach (var suite in run.TestSuiteViewModels)
        //        {
        //            foreach (var test in suite.TestViewModels)
        //            {
        //                foreach (var failure in test.FailureViewModels)
        //                {
        //                    if(failure is FailureCaptureViewModel c)
        //                    {
        //                        if (c.Name == failureCaptureName)
        //                        {
        //                            var specPath = string.Format(Globals.RUN_PATH, failure.RunName) + c.Model.SpecCapturePath;
        //                            c.SpecCaptureSource = Helper.LoadImage(specPath);
        //                            c.State = FailureBaseViewModel.FailureState.UnVerified;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public void SetPrevRun()
        {
            if (Runs.Count > 0)
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

        public void DeleteSelectedRun()
        {
            var runName = SelectedRun.RunName;
            Runs.Remove(SelectedRun);
            if (Runs.Count != 0)
                SelectedRun = Runs[^1];
            Directory.Delete(string.Format(Globals.RUN_PATH, runName), true);
        }
    }
}
