using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        TextBlock actualTextBlockClicked;
        TextBlock lastTextBlockClicked;

        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondElpased; // to keep track of the time elapsed
        int MatchesFound; // number of matches the player has found in game
        int FieldsClick = 0;
        List<string> actualEmoji = new List<string>(16);


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
                    int position = int.Parse( textBlock.Name.Substring(textBlock.Name.Length - 2, 2));
                    
                    int index = random.Next(animalEmoji.Count); // index - number between 0 and the number of emoji left in the list
                    string nextEmoji = animalEmoji[index];      // using random number to get a random emoji from list
                    //textBlock.Text = nextEmoji;               // not updating the text with random emoji - is "?" for all
                    animalEmoji.RemoveAt(index);                // remove used emoji 
                    textBlock.IsEnabled = true;
                    textBlock.Text = "?";
                    actualEmoji.Add(nextEmoji);                 //updating the list with random emoji
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
            FieldsClick++;
            int position = int.Parse(textBlock.Name.Substring(textBlock.Name.Length - 2, 2));

            if (FieldsClick == 1)
            {
                lastTextBlockClicked = textBlock;
                lastTextBlockClicked.Text = actualEmoji.ElementAt(position);
                lastTextBlockClicked.IsEnabled = false;
            }
            if (FieldsClick == 2)
            {
                actualTextBlockClicked = textBlock;
                actualTextBlockClicked.Text = actualEmoji.ElementAt(position);
                actualTextBlockClicked.IsEnabled = false;
            }
            
            if(lastTextBlockClicked != null && actualTextBlockClicked != null && 
                lastTextBlockClicked.Text == actualTextBlockClicked.Text)
            {
                lastTextBlockClicked.IsEnabled = false;
                actualTextBlockClicked.IsEnabled = false;
                FieldsClick = 0;
                MatchesFound++;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MatchesFound == 8) //2x4 animals
            {
                actualEmoji.Clear();
                SetUpGame();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //refresh clicked fields
            if (FieldsClick > 2)
            {
                actualTextBlockClicked.Text = "?";
                actualTextBlockClicked.IsEnabled = true;
                lastTextBlockClicked.Text = "?";
                lastTextBlockClicked.IsEnabled = true;
                FieldsClick = 0;
            }
        }
    }
}
