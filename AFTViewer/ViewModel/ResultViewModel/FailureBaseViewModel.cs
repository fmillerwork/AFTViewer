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
                OnPropertyChanged(nameof(ValidateButtonContent));
                OnPropertyChanged(nameof(FalsePositiveButtonContent));
                OnPropertyChanged(nameof(ValidateButtonBackground));
                OnPropertyChanged(nameof(FalsePositiveButtonBackground));
                OnPropertyChanged(nameof(ValidateButtonToolTip));
                OnPropertyChanged(nameof(FalsePositiveButtonToolTip));
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
                _ => "Orange",
            };
        }

        #region Buttons attributs
        public string ValidateButtonContent
        {
            get => State switch
            {
                FailureState.Recognized => "Non verifiée",
                _ => "Valider échec",
            };
        }

        public string FalsePositiveButtonContent
        {
            get => State switch
            {
                FailureState.FalsePositive => "Non verifiée",
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

        public string ValidateButtonToolTip
        {
            get => State switch
            {
                FailureState.Recognized => "Passe l'état de l'échec à \"Non vérifiée\"",
                _ => "Passe l'état de l'échec à \"Avérée\"",
            };
        }

        public string FalsePositiveButtonToolTip
        {
            get => State switch
            {
                FailureState.FalsePositive => "Passe l'état de l'échec à \"Non vérifiée\"",
                _ => "Passe l'état de l'échec à \"Faux positif\"",
            };
        }
        public abstract Visibility CaptureComparerVisibility{ get;}

        public abstract Visibility AssertPreviewVisibility { get;  }

        public abstract Visibility OverrideButtonVisibility { get;}

        #endregion

        public abstract string IconeFailureImageSource { get; }
    }
}
