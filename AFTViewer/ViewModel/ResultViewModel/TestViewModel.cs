using AFTViewer.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using AFTViewer.Model;

namespace AFTViewer.ViewModel
{
    public class TestViewModel : Observable
    {
        private readonly TestResultModel model;

        public TestViewModel(TestResultModel model, RunViewModel runViewModel, string testSuiteName)
        {
            this.model = model;
            FailureCaptureViewModels = new ObservableCollection<FailureCaptureViewModel>();
            foreach (var failureCapture in model.FailureCaptures)
            {
                FailureCaptureViewModels.Add(new FailureCaptureViewModel(failureCapture, runViewModel, testSuiteName, TestName));
            }
        }

        public ObservableCollection<FailureCaptureViewModel> FailureCaptureViewModels { get; set; }
        public string TestName
        {
            get => model.TestName;
            set { model.TestName = value; OnPropertyChanged(); }
        }

        public int DeleteFailureCapture(string failureCaptureName)
        {
            int captureVMIndex = -1;
            int resolvedUnVerifiedFailuresCount = 0;
            foreach (var captureModel in model.FailureCaptures)
            {
                captureVMIndex++;
                if (captureModel.FailureCaptureName.Split('.')[0] == failureCaptureName)
                {
                    model.FailureCaptures.Remove(captureModel);
                    if (FailureCaptureViewModels.ElementAt(captureVMIndex).State == FailureCaptureViewModel.FailureState.UnVerified)
                        resolvedUnVerifiedFailuresCount++;
                    FailureCaptureViewModels.RemoveAt(captureVMIndex);
                    return resolvedUnVerifiedFailuresCount;
                }
            }
            return resolvedUnVerifiedFailuresCount;
        }
    }
}
