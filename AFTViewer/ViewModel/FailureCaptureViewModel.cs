using AFTViewer.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TFAScriptTool.Models;
using Brush = System.Windows.Media.Brush;

namespace AFTViewer.ViewModel
{
    public class FailureCaptureViewModel : Observable
    {
        private readonly FailureCaptureResultModel model;
        private readonly string specPath;
        private readonly string failurePath;
        private readonly string capturePath;
        public FailureCaptureViewModel(FailureCaptureResultModel model, RunViewModel runViewModel, string testSuiteName, string testName)
        {
            failingCaptureCount++;
            ID = failingCaptureCount;
            this.model = model;
            RunName = runViewModel.RunName;
            specPath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.SpecCapturePath;
            failurePath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.FailurePath;
            capturePath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.CapturePath;

            SpecCaptureSource = Helper.LoadImage(specPath);
            FailureCaptureSource = Helper.LoadImage(failurePath);
            RunViewModel = runViewModel;
            TestSuiteName = testSuiteName;
            TestName = testName;
        }

        public FailureCaptureResultModel Model
        {
            get => model;
        }

        public RunViewModel RunViewModel { get; set; }

        public FailureState State
        {
            get => model.State;
            set 
            { 
                model.State = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(StringState)); 
                OnPropertyChanged(nameof(StateTextColor)); 
                OnPropertyChanged(nameof(ValidateButtonContent));
                OnPropertyChanged(nameof(FalsePositiveButtonContent)); 
                OnPropertyChanged(nameof(ValidateButtonBackground)); 
                OnPropertyChanged(nameof(FalsePositiveButtonBackground)); 
            }
        }

        public string StringState
        {
            get => State switch
            {
                FailureState.Recognized => "Avérée",
                FailureState.FalsePositive => "Faux positif",
                FailureState.UnVerified => "Non vérifiée",
                _ => "Non défini",
            };
        }

        public string StateTextColor
        {
            get => State switch
            {
                FailureState.Recognized => "Red",
                FailureState.FalsePositive => "Green",
                FailureState.UnVerified => "Orange",
                _ => "Yellow",
            };
        }

        #region Buttons attributs
        public string ValidateButtonContent
        {
            get => State switch
            {
                FailureState.Recognized => "Non verifié",
                _ => "Valider échec",
            };
        }

        public string FalsePositiveButtonContent
        {
            get => State switch
            {
                FailureState.FalsePositive => "Non verifié",
                _ => "Faux positif",
            };
        }

        public string ValidateButtonBackground
        {
            get => State switch
            {
                FailureState.Recognized => "Orange",
                _ => "Red",
            };
        }

        public string FalsePositiveButtonBackground
        {
            get => State switch
            {
                FailureState.FalsePositive => "Orange",
                _ => "Green",
            };
        }

        #endregion
        private string runName;
        public string RunName
        {
            get => runName;
            set => SetProperty(ref runName, value);
        }

        private string testSuiteName;
        public string TestSuiteName
        {
            get => testSuiteName;
            set => SetProperty(ref testSuiteName, value);
        }

        private string testName;
        public string TestName
        {
            get => testName;
            set => SetProperty(ref testName, value);
        }
        public string CaptureName
        {
            get => model.FailureCaptureName.Split('.')[0];
        }

        private ImageSource failureCaptureSource;
        public ImageSource FailureCaptureSource
        {
            get => failureCaptureSource;
            set => SetProperty(ref failureCaptureSource, value);
        }

        private ImageSource specCaptureSource;
        public ImageSource SpecCaptureSource
        {
            get => specCaptureSource;
            set => SetProperty(ref specCaptureSource, value);
        }

        public enum FailureState
        {
            Recognized,
            FalsePositive,
            UnVerified
        }

        private Brush backgroundColor;
        public Brush BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

        private int id;
        public int ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private static int failingCaptureCount;

        //private bool isSelected;
        ///// <summary>
        ///// Indique au treeview que l'item est sélectionné
        ///// </summary>
        //public bool IsSelected
        //{
        //    get => isSelected;
        //    set => SetProperty(ref isSelected, value);
        //}

        /// <summary>
        /// Supprime la specification actuelle et la remplace par la capture actuelle.
        /// </summary>
        public void SwitchSpecCapture()
        {
            try
            {
                var newSpec = new Bitmap(capturePath);

                if (File.Exists(specPath))
                    File.Delete(specPath);
                newSpec.Save(specPath);

                SpecCaptureSource = Helper.LoadImage(specPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
