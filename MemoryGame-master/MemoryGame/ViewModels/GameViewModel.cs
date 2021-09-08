using MemoryGame.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public enum Category { Owoce }
    public class GameViewModel : ObservableObject
    {
   
        public SlideCollectionViewModel Slides { get; private set; }
        
        public GameInfoViewModel GameInfo { get; private set; }
        
        public Category Category { get; private set; }

        public GameViewModel(Category category)
        {
            Category = category;
            SetupGame(category);
        }

        //Initialize game essentials
        private void SetupGame(Category category)
        {

            Slides = new SlideCollectionViewModel();
            GameInfo = new GameInfoViewModel();

            //Set attempts to the maximum allowed
            GameInfo.ClearInfo();

            //Create slides from image folder then display to be memorized
            Slides.CreateSlides("Assets/" + category.ToString());
            Slides.Memorize();

            //Slides have been updated
            OnPropertyChanged("Slides");
            OnPropertyChanged("GameInfo");
        }

        //Slide has been clicked
        public void ClickedSlide(object slide)
        {
            if(Slides.canSelect)
            {
                var selected = slide as PictureViewModel;
                Slides.SelectSlide(selected);
            }

            if(!Slides.areSlidesActive)
            {
                if (Slides.CheckIfMatched())
                    GameInfo.Correct(); //Correct match
                else
                    GameInfo.Incorrect();//Incorrect match
            }

            GameStatus();
        }

        //Status of the current game
        private void GameStatus()
        {
            if(GameInfo.MatchAttempts < 0)
            {
                GameInfo.GameStatus(false);
                Slides.RevealUnmatched();
            }

            if(Slides.AllSlidesMatched)
            {
                GameInfo.GameStatus(true);
            }
        }

        //Restart game
        public void Restart()
        {
            SetupGame(Category);
        }
    }
}
