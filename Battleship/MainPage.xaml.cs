using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Battleship
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Board));

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private async void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog($"This feature is not implemented", "Not implemented").ShowAsync();
            //var b = new Board();
            //b.LoadGameButton_Click();
            //this.Frame.Navigate(typeof(Board));
        }
    }
}