using System.Windows;

namespace MemoryGame.ViewModels
{
    public class GameInfoModel : ConverterViewModel
    {
        private const int maximumAttempts = 10;

        private int matches { get; set; }

        private bool lose { get; set; }
        private bool win { get; set; }

        public int Matches
        {
            get => matches;

            private set
            {
                matches = value;
                OnPropertyChanged("Matches");
            }
        }

        public Visibility LostMessage
        {
            get => lose ? Visibility.Visible : Visibility.Hidden;
        }

        public Visibility WinMessage
        {
            get => win ?  Visibility.Visible : Visibility.Hidden;
        }

        public void GameStatus(bool win)
        {
            if (win)
            {
                this.win = true;
                OnPropertyChanged("WinMessage");

                return;
            }

            lose = true;
            OnPropertyChanged("LostMessage");
        }

        public void ClearInfo()
        {
            Matches = maximumAttempts;
            lose = false;
            win = false;
            OnPropertyChanged("LostMessage");
            OnPropertyChanged("WinMessage");
        }
        public void Incorrect()
        {
            Matches--;
        }
    }
}
