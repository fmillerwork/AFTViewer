using AFTViewer.Model;
using AFTViewer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFTViewer.ViewModel
{
    public class FailedAssertViewModel : FailureBaseViewModel
    {
        protected override FailureModel model { get; }

        public FailedAssertViewModel(FailedAssertResult model, RunViewModel runViewModel, string testSuiteName, string testName) : base(runViewModel, testSuiteName, testName)
        {   
            this.model = model;
        }

        public FailedAssertResult Model
        {
            get => (FailedAssertResult)model;
        }

        public override string Name
        {
            get => ((FailedAssertResult)model).Name;
        }
        public string Description
        {
            get => ((FailedAssertResult)model).Description;
        }
        public override Visibility CaptureComparerVisibility
        {
            get => Visibility.Collapsed;
        }

        public override Visibility AssertPreviewVisibility
        {
            get => Visibility.Visible;
        }

        public override Visibility OverrideButtonVisibility
        {
            get => Visibility.Collapsed;
        }

        public override string IconeFailureImageSource => "../Images/instruction.png";
    }
}
