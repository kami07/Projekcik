using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public class CardsViewModel : ConverterViewModel
    {
        public ObservableCollection<PictureViewModel> MemoryCards { get; private set; }

        private PictureViewModel SelectedCard1;
        private PictureViewModel SelectedCard2;

        private DispatcherTimer peekTimer;
        private DispatcherTimer startTimer;

        private const int peekSeconds = 1;
        private const int startSeconds = 4;

        public bool areCardsActive
        {
            get => ((SelectedCard1 == null || SelectedCard2 == null)) ? true : false;
        }

        public bool AllCardsMatched
        {
            get
            {
                foreach(var card in MemoryCards)
                {
                    if (!card.isMatched)
                        return false;
                }
                return true;
            }
        }

        public bool canSelect { get; private set; }

        public CardsViewModel()
        {
            peekTimer = new DispatcherTimer();
            peekTimer.Interval = new TimeSpan(0, 0, peekSeconds);
            peekTimer.Tick += PeekTimer_Tick;

            startTimer = new DispatcherTimer();
            startTimer.Interval = new TimeSpan(0, 0, startSeconds);
            startTimer.Tick += OpeningTimer_Tick;
        }

        public void CreateCards(string imagesPath)
        {
            MemoryCards = new ObservableCollection<PictureViewModel>();
            var models = GetModelsFrom(@imagesPath);

            for (int i = 0; i < 10; i++)
            {
                var newCard = new PictureViewModel(models[i]);
                var newCardMatch = new PictureViewModel(models[i]);
                MemoryCards.Add(newCard);
                MemoryCards.Add(newCardMatch);
                newCard.PeekAtImage();
                newCardMatch.PeekAtImage();
            }

            ShuffleCards();
            OnPropertyChanged("MemoryCards");
        }

        public void SelectCard(PictureViewModel card)
        {
            card.PeekAtImage();

            if (SelectedCard1 == null )
            {
                SelectedCard1 = card;
            }
            else if (SelectedCard2 == null)
            {
                SelectedCard2 = card;
                HideUnmatched();
            }
            OnPropertyChanged("areCardsActive");
        }

        public bool CheckIfMatched()
        {
            if (SelectedCard1.Id == SelectedCard2.Id)
            {
                MatchCorrect();
                return true;
            }

            MatchFailed();
            return false;
        }

        private void MatchFailed()
        {
            SelectedCard1.MarkFailed();
            SelectedCard2.MarkFailed();
            ClearSelected();
        }

        private void MatchCorrect()
        {
            SelectedCard1.MarkMatched();
            SelectedCard2.MarkMatched();
            ClearSelected();
        }

        private void ClearSelected()
        {
            SelectedCard1 = null;
            SelectedCard2 = null;
            canSelect = false;
        }

        public void RevealUnmatched()
        {
            foreach(var card in MemoryCards)
            {
                if(!card.isMatched)
                {
                    peekTimer.Stop();
                    card.MarkFailed();
                    card.PeekAtImage();
                }
            }
        }

        public void HideUnmatched()
        {
            peekTimer.Start();
        }

        public void Memorize()
        {
            startTimer.Start();
        }

        private List<PictureModel> GetModelsFrom(string relativePath)
        {
            var models = new List<PictureModel>();
            var images = Directory.GetFiles(@relativePath, "*.jpg", SearchOption.AllDirectories);
            var id = 0;

            foreach (string i in images)
            {
                models.Add(new PictureModel() { Id = id, ImageSource = "/MemoryGame;component/" + i });
                id++;
            }

            return models;
        }

        private void ShuffleCards()
        {
            var rnd = new Random();

            for (int i = MemoryCards.Count-1; i >= 0; i--)
            {
                int k = rnd.Next(i);
                var temp = MemoryCards[i];
                MemoryCards[i] = MemoryCards[k];
                MemoryCards[k] = temp;
            }
        }

        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var card in MemoryCards)
            {
                card.ClosePeek();
                canSelect = true;
            }
            OnPropertyChanged("areCardsActive");
            startTimer.Stop();
        }

        private void PeekTimer_Tick(object sender, EventArgs e)
        {
            foreach(var card in MemoryCards)
            {
                if(!card.isMatched)
                {
                    card.ClosePeek();
                    canSelect = true;
                }
            }
            OnPropertyChanged("areCardsActive");
            peekTimer.Stop();
        }
    }
}
