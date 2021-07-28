using AFTViewer.Helpers;
using AFTViewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static AFTViewer.ViewModel.FailureCaptureViewModel;

namespace AFTViewer.View
{
    /// <summary>
    /// Logique d'interaction pour DecisionView.xaml
    /// </summary>
    public partial class DecisionView : UserControl
    {
        public DecisionView()
        {
            InitializeComponent();
        }

        #region Choice events
        private void ValidateFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (FailureCaptureViewModel)DataContext;
            if (dataContext != null && dataContext.RunViewModel.SelectedCapture != null)
            {

                if (dataContext.State != FailureState.Recognized)
                {
                    var runViewModel = dataContext.RunViewModel;
                    if (dataContext != null)
                    {
                        var initialState = dataContext.State;
                        runViewModel.UpdateState(FailureState.Recognized);
                        Helper.SaveChanges(runViewModel.Model);

                        // Changement de capture ssi l'ancien l'état initial est Unverified
                        if (initialState == FailureState.UnVerified)
                            runViewModel.SetNextSelectedCapture(true);
                    }
                }
                else
                {
                    UnVerified_Click();
                }
            }
        }

        private void FalsePositive_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (FailureCaptureViewModel)DataContext;
            if(dataContext != null && dataContext.RunViewModel.SelectedCapture != null)
            {
                // Si capture est non vérifiée ou faux positif
                if (dataContext.State != FailureState.FalsePositive)
                {
                    var runViewModel = dataContext.RunViewModel;
                    if (dataContext != null)
                    {
                        var initialState = dataContext.State;
                        runViewModel.UpdateState(FailureState.FalsePositive);
                        Helper.SaveChanges(runViewModel.Model);

                        // Changement de capture ssi l'ancien l'état initial est Unverified
                        if (initialState == FailureState.UnVerified)
                            runViewModel.SetNextSelectedCapture(true);
                    }
                }
                else
                {
                    UnVerified_Click();
                }
            }
        }

        private void UnVerified_Click()
        {
            var dataContext = (FailureCaptureViewModel)DataContext;
            var runViewModel = dataContext.RunViewModel;
            if (dataContext != null)
            {
                runViewModel.UpdateState(FailureState.UnVerified);
                Helper.SaveChanges(runViewModel.Model);

                //runViewModel.SetNextSelectedCapture(true);
            }
        }

        private void OverrideSpec_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (FailureCaptureViewModel)DataContext;
            if(dataContext!= null && dataContext.RunViewModel.SelectedCapture != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de remplacement de spécification", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var runViewModel = dataContext.RunViewModel;
                    if (dataContext != null)
                    {
                        runViewModel.SelectedCapture.SwitchSpecCapture();

                        var selectedCaptureName = runViewModel.SelectedCapture.CaptureName;

                        runViewModel.DeleteFailureCapture(runViewModel.SelectedCapture);

                        runViewModel.MainViewModel.RefreshSpecCaptureSources(selectedCaptureName);

                        runViewModel.SetNextSelectedCapture(true, runViewModel.SelectedCaptureIndex - 1);

                        Helper.SaveChanges(dataContext.RunViewModel.Model);

                        if (runViewModel.SelectedCaptureIndex < 0)
                            runViewModel.MainViewModel.DeleteRun();
                    }
                }
            }
        }

        #endregion
    }
}
