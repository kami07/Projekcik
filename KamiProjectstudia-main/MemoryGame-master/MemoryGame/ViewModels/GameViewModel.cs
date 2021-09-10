namespace MemoryGame.ViewModels
{
    public class GameViewModel : ConverterViewModel
    {
   
        public CardsViewModel Cards { get; private set; }
        
        public GameInfoModel GameInfo { get; private set; }

        public GameViewModel()
        {
            SetupGame();
        }

        private void SetupGame()
        {

            Cards = new CardsViewModel();
            GameInfo = new GameInfoModel();

            GameInfo.ClearInfo();

            Cards.CreateCards("../../Graphics/Owoce");
            Cards.Memorize();

            OnPropertyChanged("Cards");
            OnPropertyChanged("GameInfo");
        }

        public void ClickedCard(object card)
        {
            if(Cards.canSelect)
            {
                var selected = card as PictureViewModel;
                Cards.SelectCard(selected);
            }

            if(!Cards.areCardsActive)
            {
                if (!Cards.CheckIfMatched())
                {
                    GameInfo.Incorrect();
                }
            }
            GameStatus();
        }

        private void GameStatus()
        {
            if(GameInfo.Matches < 1)
            {
                GameInfo.GameStatus(false);
                Cards.RevealUnmatched();
                return;
            }

            if(Cards.AllCardsMatched)
            {
                GameInfo.GameStatus(true);
            }
        }

        public void Restart()
        {
            SetupGame();
        }
    }
}
