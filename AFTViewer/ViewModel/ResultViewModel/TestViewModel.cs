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
            FailureViewModels = new ObservableCollection<FailureBaseViewModel>();
            foreach (var failureCapture in model.FailureCaptures)
            {
                FailureViewModels.Add(new FailureCaptureViewModel(failureCapture, runViewModel, testSuiteName, TestName));
            }
            foreach (var failedAssert in model.FailureAsserts)
            {
                FailureViewModels.Add(new FailedAssertViewModel(failedAssert, runViewModel, testSuiteName, TestName));
            }
        }

        public ObservableCollection<FailureBaseViewModel> FailureViewModels { get; set; }
        public string TestName
        {
            get => model.TestName;
            set { model.TestName = value; OnPropertyChanged(); }
        }

        public void DeleteFailureCapture(string failureCaptureName)
        {
            int captureVMIndex = -1;
            foreach (var captureModel in model.FailureCaptures)
            {
                captureVMIndex++;
                if (captureModel.FailureCaptureName.Split('.')[0] == failureCaptureName)
                {
                    model.FailureCaptures.Remove(captureModel);
                    FailureViewModels.RemoveAt(captureVMIndex);
                    break;
                }
            }
        }
    }
}
