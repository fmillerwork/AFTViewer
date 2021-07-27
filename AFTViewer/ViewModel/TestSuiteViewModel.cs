using AFTViewer.Helpers;
using System.Collections.ObjectModel;
using TFAScriptTool.Models;

namespace AFTViewer.ViewModel
{
    public class TestSuiteViewModel : Observable
    {
        private readonly TestSuiteResultModel model;

        public TestSuiteViewModel(TestSuiteResultModel model, RunViewModel runViewModel)
        {
            this.model = model;
            TestViewModels = new ObservableCollection<TestViewModel>();
            foreach (var testResult in model.TestResults)
            {
                TestViewModels.Add(new TestViewModel(testResult, runViewModel, TestSuiteName));
            }
        }

        public string TestSuiteName
        {
            get => model.TestSuiteName;
        }

        public ObservableCollection<TestViewModel> TestViewModels { get; set; }

        public void DeleteTest(TestViewModel testVM)
        {
            int testVMindex = -1;
            foreach (var testModel in model.TestResults)
            {

                testVMindex++;
                if (testModel.TestName == testVM.TestName)
                {
                    model.TestResults.Remove(testModel);
                    break;
                }
            }
            TestViewModels.RemoveAt(testVMindex);
        }
    }
}
