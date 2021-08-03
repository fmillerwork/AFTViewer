﻿using AFTViewer.Utils;
using AFTViewer.ViewModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using static AFTViewer.ViewModel.FailureBaseViewModel;

namespace AFTViewer.View
{
    /// <summary>
    /// Logique d'interaction pour RunExplorer.xaml
    /// </summary>
    public partial class RunExplorer : UserControl
    {
        private Timer timer;
        public RunExplorer()
        {
            InitializeComponent();
            timer = new Timer(500);

        }
        private void ResultTreeView_ItemClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            dataContext.SelectedFailure = (FailureBaseViewModel)ResultTreeView.SelectedItem;
            if(dataContext.SelectedFailure is FailureCaptureViewModel failureCapture)
            {

            }
            else if(dataContext.SelectedFailure is FailedAssertViewModel failedAssert)
            {
                //OverrideSpecButton
            }
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

        #region Navigation buttons
        private void PreviousFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext == null)
                return;
            dataContext.SetPreviousSelectedCapture(false);
        }

        private void NextFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext == null)
                return;
            dataContext.SetNextSelectedCapture(false);
        }

        private void PreviousVerif_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext == null)
                return;
            dataContext.SetPreviousSelectedCapture(true);
        }

        private void NextVerif_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext == null)
                return;
            dataContext.SetNextSelectedCapture(true);
        }
        #endregion

        #region Choice events
        private void ValidateFailure_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext != null && dataContext.SelectedFailure != null)
            {

                if (dataContext.SelectedFailure.State != FailureState.Recognized)
                {
                    if (dataContext != null)
                    {
                        var initialState = dataContext.SelectedFailure.State;
                        dataContext.UpdateState(FailureState.Recognized);
                        Helper.SaveChanges(dataContext.Model);

                        // Changement de capture ssi l'ancien l'état initial est Unverified
                        if (initialState == FailureState.UnVerified)
                            dataContext.SetSelectedCaptureOnFirstUnverifiedCapture();
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
            var dataContext = (RunViewModel)DataContext;
            if (dataContext != null && dataContext.SelectedFailure != null)
            {
                // Si capture est non vérifiée ou faux positif
                if (dataContext.SelectedFailure.State != FailureState.FalsePositive)
                {
                    if (dataContext != null)
                    {
                        var initialState = dataContext.SelectedFailure.State;
                        dataContext.UpdateState(FailureState.FalsePositive);
                        Helper.SaveChanges(dataContext.Model);

                        // Changement de capture ssi l'ancien l'état initial est Unverified
                        if (initialState == FailureState.UnVerified)
                            dataContext.SetSelectedCaptureOnFirstUnverifiedCapture();
                    }
                }
                else
                {
                    UnVerified_Click();
                }
            }
        }

        private void OverrideSpec_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunViewModel)DataContext;
            if(dataContext.SelectedFailure is FailureCaptureViewModel capture)
            {
                if (dataContext != null && capture != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de remplacement de spécification", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        if (dataContext != null)
                        {
                            dataContext.OverrideSpecCapture(capture);
                            Helper.SaveChanges(dataContext.Model);
                        }
                    }
                }
            }
        }

        #region Methods
        private void UnVerified_Click()
        {
            var dataContext = (RunViewModel)DataContext;
            if (dataContext != null)
            {
                dataContext.UpdateState(FailureState.UnVerified);
                Helper.SaveChanges(dataContext.Model);

            }
        }

        private void SetSaveTimer()
        {
            timer = new Timer(500);

            timer.Elapsed += SaveChanges;
            timer.AutoReset = false;
            timer.Enabled = true;
        }
        private void SaveChanges(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Helper.SaveChanges(((RunViewModel)DataContext).Model);
            });
        }
        #endregion

        #endregion

        private void CommentTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
            SetSaveTimer();
        }
    }
}
