using AFTViewer.ViewModel;
using System.Windows;

namespace AFTViewer.View
{
    /// <summary>
    /// Logique d'interaction pour FailedRunsWindow.xaml
    /// </summary>
    public partial class FailedRunsWindow : Window
    {
        public FailedRunsWindow()
        {
            InitializeComponent();
        }
        private void RemoveRunError_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainErrorViewModel)DataContext;
            if (dataContext.RunErrors.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ((MainErrorViewModel)DataContext).DeleteRun();
                }
            }
        }

        private void RefreshRunErrors_Click(object sender, RoutedEventArgs e)
        {
            ((MainErrorViewModel)DataContext).ReloadRuns();
        }

        private void PrevRunError_Click(object sender, RoutedEventArgs e)
        {
            ((MainErrorViewModel)DataContext).SetPrevRun();
        }

        private void NextRunError_Click(object sender, RoutedEventArgs e)
        {
            ((MainErrorViewModel)DataContext).SetNextRun();
        }
        private void ResetLeftCapture_Click(object sender, RoutedEventArgs e)
        {
            ErrorCapture.Reset();
        }
    }
}
