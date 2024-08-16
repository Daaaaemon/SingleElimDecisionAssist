using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SingleElimDecisionAssist.View
{
    /// <summary>
    /// Interaction logic for Winner.xaml
    /// </summary>
    public partial class Winner : Window
    {
        private readonly string text;
        public Winner(BitmapImage image, string _text)
        {
            InitializeComponent();
            WinnerImage.Source = image;
            WinnerTextBlock.Text = text = _text;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WinnerTextBlock_TextInput(object sender, TextCompositionEventArgs e)
        {
            WinnerTextBlock.Text = text;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(text);
            CopyButton.Content = "Copied!";
        }
    }
}
