using MemoryGame.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel();
        }
        private void Card_Clicked(object sender, RoutedEventArgs e)
        {
            var game = DataContext as GameViewModel;
            var button = sender as Button;
            game.ClickedCard(button.DataContext);
        }

        private void PlayAgain(object sender, RoutedEventArgs e)
        {
            var game = DataContext as GameViewModel;
            game.Restart();
        }
    }
}
