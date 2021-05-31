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
namespace Bird
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timerGame = new DispatcherTimer();
        double score;
        int gravity = 8;
        bool gameover;
        Rect birdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            timerGame.Tick += MainEventTimer;
            timerGame.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            Score.Content = "Score: " + score;
            birdHitBox = new Rect(Canvas.GetLeft(Bird), Canvas.GetTop(Bird), Bird.Width - 5, Bird.Height);
            Canvas.SetTop(Bird, Canvas.GetTop(Bird) + gravity);

            if (Canvas.GetTop(Bird) <  -10 || Canvas.GetTop(Bird) > 458)
            {
                EndGame();
            }

            foreach (var x in CanvasName.Children.OfType<Image>())
            {
                if((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if(Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        score += 0.5;
                    }
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 

                    if (birdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }
                }

                if((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                Bird.RenderTransform = new RotateTransform(-20, Bird.Width /2, Bird.Height /2);
                gravity = -8;
            }

            if (e.Key == Key.R && gameover == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            Bird.RenderTransform = new RotateTransform(5, Bird.Width / 2, Bird.Height / 2);
            gravity = 8;
        }

        private void StartGame()
        {
            CanvasName.Focus();

            int temp = 300;
            score = 0;
            gameover = false;
            Canvas.SetTop(Bird, 190);

            foreach (var x in CanvasName.Children.OfType<Image>())
            {
                if((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }

                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }

                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }

                timerGame.Start();
            }
        }

        private void EndGame()
        {
            timerGame.Stop();
            gameover = true;
            Score.Content += "Игра окончена || Нажмите R чтобы начать заново";
        }
    }
}
