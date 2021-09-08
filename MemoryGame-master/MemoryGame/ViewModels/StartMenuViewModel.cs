using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModels
{
    public class StartMenuViewModel
    {
        private MainWindow _mainWindow;
        public StartMenuViewModel(MainWindow main)
        {
            _mainWindow = main;
        }

        public void StartNewGame()
        {
            var category = Category.Owoce;
            GameViewModel newGame = new GameViewModel(category);
            _mainWindow.DataContext = newGame;
        }
    }
}
