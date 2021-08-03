using System.Windows;
using System.Windows.Controls;

namespace AFTViewer.View
{
    /// <summary>
    /// Logique d'interaction pour Comparer.xaml
    /// </summary>
    public partial class CaptureComparer : UserControl
    {
        public CaptureComparer()
        {
            InitializeComponent();
        }

        private void ResetLeftCapture_Click(object sender, RoutedEventArgs e)
        {
            LeftCapture.Reset();
        }

        private void ResetRightCapture_Click(object sender, RoutedEventArgs e)
        {
            RightCapture.Reset();
        }
    }
}
