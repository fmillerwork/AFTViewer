using AFTViewer.ViewModel;
using System.Windows;

namespace AFTViewer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void RemoveRun_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainViewModel)DataContext;
            if(dataContext.Runs.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Êtes-vous sûr(e) ?", "Confirmation de suppression", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ((MainViewModel)DataContext).DeleteRun();
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

    }
}
