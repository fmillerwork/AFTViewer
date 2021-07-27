using AFTViewer.Helpers;
using AFTViewer.ViewModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using static AFTViewer.ViewModel.FailureCaptureViewModel;

namespace AFTViewer.View
{
    /// <summary>
    /// Logique d'interaction pour RunExplorer.xaml
    /// </summary>
    public partial class RunExplorer : UserControl
    {
        public RunExplorer()
        {
            InitializeComponent();
        }

        #region Collapse/Expand treeview
        private void CollapseTreeView_Click(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapseTreeviewItems(false);
        }

        private void ExpandTreeView_Click(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapseTreeviewItems(true);
        }

        /// <summary>
        /// Développe ou réduit le ResultTreeView. Mettre "expand" à true pour développer et à false pour réduire.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="expand"></param>
        private void ExpandOrCollapseTreeviewItems(bool expand)
        {
            foreach (TestSuiteViewModel testSuite in ResultTreeView.Items)
            {
                testSuite.IsExpanded = expand;

                foreach (TestViewModel test in testSuite.TestViewModels)
                {
                    test.IsExpanded = expand;
                }
            }
        }
        #endregion


        private void ResultTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ResultTreeView.SelectedItem is FailureCaptureViewModel selectedFailureCapture)
            {
                var dataContext = (RunViewModel)DataContext;
                dataContext.SelectedCapture = selectedFailureCapture;
            }
        }

        #region Previous/Next Failure
        private void PreviousFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            dataContext.SetPreviousSelectedCapture(false);
        }

        private void NextFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            dataContext.SetNextSelectedCapture(false);
        }

        private void PreviousVerif_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            dataContext.SetPreviousSelectedCapture(true);
        }

        private void NextVerif_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            dataContext.SetNextSelectedCapture(true);
        }
        #endregion

        //#region Choice events
        //private void ValidateFailure_Click(object sender, RoutedEventArgs e)
        //{
        //    var dataContext = (RunViewModel)DataContext;
        //    if (dataContext.SelectedCapture != null)
        //    {
        //        var initialState = dataContext.SelectedCapture.State;
        //        dataContext.UpdateState(FailureState.Recognized);
        //        Helper.SaveChanges(dataContext.Model);

        //        // Changement de capture ssi l'ancien l'état initial est Unverified
        //        if (initialState == FailureState.UnVerified)
        //            dataContext.SetNextSelectedCapture(true);
        //    }
        //}

        //private void FalsePositive_Click(object sender, RoutedEventArgs e)
        //{
        //    var dataContext = (RunViewModel)DataContext;
        //    if (dataContext.SelectedCapture != null)
        //    {
        //        var initialState = dataContext.SelectedCapture.State;
        //        dataContext.UpdateState(FailureState.FalsePositive);
        //        Helper.SaveChanges(dataContext.Model);

        //        // Changement de capture ssi l'ancien l'état initial est Unverified
        //        if (initialState == FailureState.UnVerified)
        //            dataContext.SetNextSelectedCapture(true);
        //    }
        //}

        //private void UnVerified_Click(object sender, RoutedEventArgs e)
        //{
        //    var dataContext = (RunViewModel)DataContext;
        //    if (dataContext.SelectedCapture != null)
        //    {
        //        dataContext.UpdateState(FailureState.UnVerified);
        //        Helper.SaveChanges(dataContext.Model);

        //        dataContext.SetNextSelectedCapture(true);
        //    }
        //}

        //private void OverrideSpec_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de remplacement de spécification", MessageBoxButton.YesNo);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        var dataContext = (RunViewModel)DataContext;
        //        if (dataContext.SelectedCapture != null)
        //        {
        //            dataContext.SelectedCapture.SwitchSpecCapture();

        //            dataContext.DeleteFailureCapture(dataContext.SelectedCapture);

        //            dataContext.MainViewModel.RefreshSpecCaptureSources(dataContext.SelectedCapture);

        //            dataContext.SetNextSelectedCapture(true, dataContext.SelectedCaptureIndex - 1);

        //            Helper.SaveChanges(dataContext.Model);
        //        }
        //    }
        //}

        //#endregion
    }
}
