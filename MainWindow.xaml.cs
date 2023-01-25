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
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock lastTextBlockClicked;
        bool findinfMatch = false;

        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondElpased; // to keep track of the time elapsed
        int MatchesFound; // number of matches the player has found

        public MainWindow()
        {
            InitializeComponent();

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
            Random random= new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondElpased = 0;
            MatchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if(findinfMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked= textBlock;
                findinfMatch= true;
            }
            else if(textBlock.Text == lastTextBlockClicked.Text) 
            {
                MatchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findinfMatch = false;
            }
            else
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
