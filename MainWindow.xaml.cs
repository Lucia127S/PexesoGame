using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PexesoGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Simple game using List, conditions, loop, mouse event
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock lastTextBlockClicked;
        bool findinfMatch = false;

        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondElpased; // to keep track of the time elapsed
        int MatchesFound; // number of matches the player has found in game

        public MainWindow()
        {
            InitializeComponent();

            //setting timer
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondElpased++;
            timeTextBlock.Text = (tenthsOfSecondElpased / 10F).ToString("0.0s");
            if(MatchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            //setting a list with emoji
            List<string> animalEmoji = new List<string>()
            {
                "🐶","🐶",
                "🐺","🐺",
                "🐱","🐱",
                "🦁","🦁",
                "🐯","🐯",
                "🦒","🦒",
                "🦊","🦊",
                "🦝","🦝",
            };

            //random number generator
            Random random= new Random();

            //setting visual of TextBoxes for game
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count); //index - number between 0 and the number of emoji left in the list
                    string nextEmoji = animalEmoji[index];      // using random number to get a random emoji from list
                    textBlock.Text = nextEmoji;                 //updating the text with random emoji
                    animalEmoji.RemoveAt(index);                //remove used emoji 
                    textBlock.Visibility= Visibility.Visible;
                }
            }

            timer.Start();
            tenthsOfSecondElpased = 0;
            MatchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //steps for not/finding pair
            TextBlock textBlock = sender as TextBlock;
            if(findinfMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked= textBlock; // keeps track of TextBlock in case it needs to make it visible again
                findinfMatch= true;
            }
            else if(textBlock.Text == lastTextBlockClicked.Text) 
            {
                MatchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findinfMatch = false;
            }
            else // not found - reset visibility
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;   
                findinfMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MatchesFound == 8) //2x4 animals
            {
                SetUpGame();
            }
        }
    }
}
