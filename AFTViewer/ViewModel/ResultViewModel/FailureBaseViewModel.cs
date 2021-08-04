using AFTViewer.Model;
using AFTViewer.Utils;
using System.Windows;
using System.Windows.Media;

namespace AFTViewer.ViewModel
{
    public abstract class FailureBaseViewModel : Observable
    {
        protected abstract FailureModel model { get; }

        public FailureBaseViewModel(RunViewModel runViewModel, string testSuiteName, string testName)
        {
            RunName = runViewModel.RunName;

            RunViewModel = runViewModel;
            TestSuiteName = testSuiteName;
            TestName = testName;
        }

        public abstract string Name { get; }

        public string Comment
        {
            get => model.Comment;
            set { model.Comment = value; OnPropertyChanged(); }
        }

        private Brush backgroundColor;
        public Brush BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

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

        public enum FailureState
        {
            Recognized,
            FalsePositive,
            UnVerified
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
                OnPropertyChanged(nameof(ValidateButtonVisibility));
                OnPropertyChanged(nameof(FalsePositiveButtonVisibility));
                OnPropertyChanged(nameof(UnVerifiedButtonVisibility));
            }
        }

        public string StringState
        {
            get => State switch
            {
                FailureState.Recognized => "Avéré",
                FailureState.FalsePositive => "Faux positif",
                FailureState.UnVerified => "Non vérifié",
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
                _ => "Orange",
            };
        }

        #region Visibility

        public abstract Visibility CaptureComparerVisibility { get; }

        public abstract Visibility AssertPreviewVisibility { get; }

        public abstract Visibility OverrideButtonVisibility { get; }

        public Visibility ValidateButtonVisibility
        {
            get => State switch
            {
                FailureState.Recognized => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        public Visibility FalsePositiveButtonVisibility
        {
            get => State switch
            {
                FailureState.FalsePositive => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        public Visibility UnVerifiedButtonVisibility
        {
            get => State switch
            {
                FailureState.UnVerified => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        #endregion

        public abstract string IconeFailureImageSource { get; }
    }
}
