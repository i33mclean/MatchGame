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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text;
            }
        }

        private void SetUpGame()
        {
            // Uses a List to create 8 pairs of emojis
            List<string> animalEmoji = new List<string>()
            {
                "🐙","🐙",
                "🐡","🐡",
                "🐘","🐘",
                "🐳","🐳",
                "🐫","🐫",
                "🦕","🦕",
                "🦘","🦘",
                "🦔","🦔",
            };
            
            // Creates a new random number generator 
            Random random = new Random();
            
            // Finds each Textblock in the mainGrid and repeats the following statements for each of them
            foreach (TextBlock textblock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textblock.Name != "timeTextBlock")
                {
                    // Picks a random number between 0 and the number of emojis left in the list and calls it "index"
                    int index = random.Next(animalEmoji.Count);

                    // Uses the random number called "index" to get a random emoji from the list
                    string nextEmoji = animalEmoji[index];

                    //Updates the textblock with the random emoji from the list
                    textblock.Text = nextEmoji;

                    // Removes the random emoji from the list to avoid duplicate pairs
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        
        /* Boolean to keep track of whether or not a player just clicked 
         * on the first animal in a pair and is now trying to find a match */
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
            
        {
           
            //bool findingMatch = false;

            TextBlock textBlock = sender as TextBlock; 

            /* The player just clicked on the first animal in a pair,so it makes that animal visible 
             * and keeps track of its textblock in case it needs to make it visible again */
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            /* The player found a match, so it makes the second animal in the pair invisible and unclickable,
             *and resets findingMatch so the next animal clicked is the first one in a pair again */
            else if(textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            /*The player clicked on an animal that doesn't match, so it makes the first animal that was 
             * clicked visible again and resets findingMatch */
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }

        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
