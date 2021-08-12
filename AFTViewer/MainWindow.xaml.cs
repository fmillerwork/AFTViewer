using AFTViewer.Utils;
using AFTViewer.View;
using AFTViewer.ViewModel;
using System.Windows;

namespace AFTViewer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FailedRunsWindow failedRunsWindow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void RemoveRun_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainViewModel)DataContext;
            if (dataContext.Runs.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ((MainViewModel)DataContext).DeleteSelectedRun();
                }
            }
        }

        private void RefreshRun_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).ReloadRuns();
        }

        private void PrevRun_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).SetPrevRun();
        }

        private void NextRun_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).SetNextRun();
        }

        private void FailedRunsDisplay_Click(object sender, RoutedEventArgs e)
        {
            // Création
            if (failedRunsWindow == null || !failedRunsWindow.IsVisible)
            {
                failedRunsWindow = new FailedRunsWindow()
                {
                    DataContext = new MainErrorViewModel()
                };
                failedRunsWindow.Show();
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            failedRunsWindow?.Close();
        }
    }
}
