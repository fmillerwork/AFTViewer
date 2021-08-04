using AFTViewer.Model;
using AFTViewer.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace AFTViewer.ViewModel
{
    public class FailureCaptureViewModel : FailureBaseViewModel
    {
        protected override FailureModel model { get; }

        private readonly string specPath;
        private readonly string failurePath;
        private readonly string capturePath;
        public FailureCaptureViewModel(FailureCaptureResultModel model, RunViewModel runViewModel, string testSuiteName, string testName) : base(runViewModel, testSuiteName, testName)
        {
            this.model = model;

            specPath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.SpecCapturePath;
            failurePath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.FailurePath;
            capturePath = string.Format(Globals.RUN_PATH, runViewModel.RunName) + model.CapturePath;

            SpecCaptureSource = Helper.LoadImage(specPath);
            FailureCaptureSource = Helper.LoadImage(failurePath);
        }

        #region Properties
        public FailureCaptureResultModel Model
        {
            get => (FailureCaptureResultModel)model;
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

        public override string Name
        {
            get => ((FailureCaptureResultModel)model).FailureCaptureName.Split('.')[0];
        }

        public override string IconeFailureImageSource => "../Images/image.png";

        public override Visibility CaptureComparerVisibility
        {
            get => Visibility.Visible;
        }

        public override Visibility AssertPreviewVisibility
        {
            get => Visibility.Collapsed;
        }

        public override Visibility OverrideButtonVisibility
        {
            get => Visibility.Visible;
        }

        #endregion

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
                newSpec.Dispose();
                SpecCaptureSource = Helper.LoadImage(specPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
