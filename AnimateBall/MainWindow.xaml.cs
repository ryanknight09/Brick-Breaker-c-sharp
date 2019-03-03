using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AnimateBall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private double dx = 3;
        private double dy = 3;
        private double vertDirection = -1;
        private double horizDirection = 1;

        private double gameBallTop = 0;
        private double gameBallLeft = 0;

        private double gamePaddleTop = 0;
        private double gamePaddleLeft = 0;
        private double gamePaddleDy = 5;

        private int score = 0;
        private int _highScore = 0;
        private int LvL = 1;

        private bool moveUp = false;
        private bool moveDown = false;

        private TheBrick[] myBricks;
        private Rectangle[] tangles;
        private int[] points = new int[18];
        

        public MainWindow()
        {
            InitializeComponent();
           
            this.level.Content = "Level " + LvL.ToString();
            MainMenu.Visibility = Visibility.Visible;

            
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            gameTimer.Tick += GameTimer_Tick;
            
            gameBallTop = Canvas.GetTop(gameBall);
            gameBallLeft = Canvas.GetLeft(gameBall);

            gamePaddleTop = Canvas.GetTop(gamePaddle);
            gamePaddleLeft = Canvas.GetLeft(gamePaddle);
            this.start.IsEnabled = false;
        }

        private void FillTangles(int cnt)
        {
            tangles = new Rectangle[cnt];
            string temp = "";

            for (int i = 0; i < cnt; i++)
            {
                
                tangles[i] = (Rectangle)myGameCanvas.Children[i + 2];
   
                temp += tangles[i].Name + ", ";
            }
        }

        private void SetVectors(int cnt)
        {
            myBricks = new TheBrick[cnt];
            Rectangle temp;
            
            for (int i = 0; i < myBricks.Length; i++)
            {
                Random rnd = new Random();
                int val = rnd.Next(1, 3);
                temp = (Rectangle)tangles[i];
                TheBrick brick = new TheBrick(Canvas.GetTop(temp), Canvas.GetLeft(temp), temp.Name);
                myBricks[i] = brick;
            }
        }


        private void GameTimer_Tick(object sender, EventArgs e)
        {
            FillTangles(myGameCanvas.Children.Count - 2);
            SetVectors(myGameCanvas.Children.Count - 2);
            BallOffCanvas();
            MovePaddle();
            OffPaddle();
            BreakBrick();
            
            gameBallLeft += dx * horizDirection;
            gameBallTop += dy * vertDirection;
            
            Canvas.SetLeft(gamePaddle, gamePaddleLeft);
            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);
            
            if(this.myBricks.Length == 0)
            {
                gameTimer.Stop();
                LvL++;
                if (LvL == 4)
                {
                    this.level.Content = "Press start to restart lvl 1";
                    score = 0;
                    _highScore = 0;
                    LvL = 1;
                    MainMenu.IsEnabled = true;
                    MainMenu.Visibility = Visibility.Visible;
                }
                this.level.Content = "Level " + LvL.ToString();
                MainMenu.IsEnabled = true;
                MainMenu.Visibility = Visibility.Visible;
            }
        }


        private void BallOffCanvas()
        {
            //double xComponent = gameBall;
            //double yComponent = 0;
            if (gameBallTop <= 0)
            {
                vertDirection *= -1;
            }
            if (gameBallLeft <= 0 || gameBallLeft >= myGameCanvas.Width - gameBall.Width)
            {
                horizDirection *= -1;
            }

            if (gameBallTop >= (myGameCanvas.Height - gameBall.Height)) {
               gameTimer.Stop();
                PauseMenu.IsEnabled = true;
                this.Continue.IsEnabled = false;
                this.Continue.Content = "Game Over";
;                PauseMenu.Visibility = Visibility.Visible;
            }
        }

        private void OffPaddle()
        {
            if (gamePaddleLeft + gamePaddle.Width >= gameBallLeft
                && gameBallTop + gameBall.Height / 2 >= gamePaddleTop && gameBallTop + gameBall.Height / 2 <= gamePaddleTop + gamePaddle.Height)
            {
                if (gamePaddleTop + gamePaddle.Height >= gameBallTop
                     && gameBallLeft + gameBall.Width / 2 >= gamePaddleLeft && gameBallLeft + gameBall.Width / 2 <= gamePaddleLeft + gamePaddle.Width)
                {
                    vertDirection *= -1;
                }
                else
                {
                    horizDirection *= -1;
                }

            }

            if (gamePaddleLeft >= gameBallLeft + gameBall.Width
                && gameBallTop + gameBall.Height / 2 >= gamePaddleTop && gameBallTop + gameBall.Height / 2 <= gamePaddleTop + gamePaddle.Height)
            {

                horizDirection *= -1;
            }
        }

        private void BreakBrick()
        {
            Rectangle brick;
            double brickLeft = 0;
            double brickTop = 0;

            for (int i = 0; i < tangles.Length; i++)
            {                                                             
                brick = tangles[i];
                brickLeft = myBricks[i].Left;
                brickTop = myBricks[i].Top;

                Rect ball = new Rect(Canvas.GetLeft(gameBall), Canvas.GetTop(gameBall), gameBall.Width, gameBall.Height);
                Rect brickTemp = new Rect(Canvas.GetLeft(brick), Canvas.GetTop(brick), brick.Width, brick.Height);
                

                //MessageBox.Show(brick.Name);

                if (brickTemp.IntersectsWith(ball))
                {
                    if (brickLeft + brick.Width >= gameBallLeft && gameBallTop + gameBall.Height / 2 >= brickTop && gameBallTop + gameBall.Height / 2 <= brickTop + brick.Height)
                    {
                        //MessageBox.Show(brick.Name + " vertical change of ball logic layer 0");

                        if (brickTop + brick.Height >= gameBallTop
                             && gameBallLeft + gameBall.Width / 2 >= brickLeft && gameBallLeft + gameBall.Width / 2 <= brickLeft + brick.Width)
                        {
                            //MessageBox.Show(points[i].ToString());
                            if (points[i] == 1)
                            {
                                vertDirection *= -1;
                                scoreCard.Content = ++score;
                                highscore.Content = ++_highScore;
                                myGameCanvas.Children.Remove(brick);
                                myGameCanvas.UpdateLayout();
                            } else
                            {
                                vertDirection *= -1;
                                points[i]--;
                            }

                        }
                        else
                        {
                           // MessageBox.Show(points[i].ToString());
                            if (points[i] == 1)
                            {
                                horizDirection *= -1;
                                scoreCard.Content = ++score;
                                highscore.Content = ++_highScore;
                                myGameCanvas.Children.Remove(brick);
                                myGameCanvas.UpdateLayout();
                            } else
                            {
                                horizDirection *= -1;
                                points[i]--;
                            }
                            
                        }
                    }

                    if (brickLeft >= gameBallLeft + gameBall.Width
                        && gameBallTop + gameBall.Height / 2 >= brickTop && gameBallTop + gameBall.Height / 2 <= brickTop + brick.Height)
                    {
                        //MessageBox.Show(points[i].ToString());
                        if(points[i] == 1)
                        {
                            horizDirection *= -1;
                            scoreCard.Content = ++score;
                            highscore.Content = ++_highScore;
                            myGameCanvas.Children.Remove(brick);
                            myGameCanvas.UpdateLayout();
                        } else
                        {
                            horizDirection *= -1;
                            points[i]--;
                        }
                    }
                } 
            }
        }




        private void reset()
        {
            Random rnd = new Random();

            gameBallTop = rnd.Next(200, 400);
            gameBallLeft = rnd.Next(30, 570);

            gamePaddleLeft = 246;
            gamePaddleTop = 438;

            horizDirection = 1;
            
            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);
            
            this.start.IsEnabled = false;
            this.pause.IsEnabled = true;
        }

        private void MovePaddle()
        {
            if (moveUp) // left
            {
                if (gamePaddleLeft - gamePaddleDy >= 0) // gamePaddleTop - gamePaddleDy >= 0
                    gamePaddleLeft -= gamePaddleDy;
            }
            if (moveDown) //right
            {
                if (gamePaddleLeft + gamePaddleDy + gamePaddle.Width <= myGameCanvas.Width)
                    gamePaddleLeft += gamePaddleDy;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveUp = true;
            }
            else if (e.Key == Key.Right)
            {
                moveDown = true;
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveUp = false;
            }
            else if (e.Key == Key.Right)
            {
                moveDown = false;
            }

        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            pause.IsEnabled = false;
            gameTimer.Stop();
            PauseMenu.IsEnabled = true;
            PauseMenu.Visibility = Visibility.Visible;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Canvas.GetTop(gameBall).ToString());
            pause.IsEnabled = true;
            MainMenu.IsEnabled = false;
            MainMenu.Visibility = Visibility.Hidden;
            reset();
            gameTimer.Start();
            CreateBricks();
        }

        private void CreateBricks()
        {
            int row = 3;
            int col = 6;
            int width = 100;
            int height = 15;
            int top = 136;
            int left = 0;
            char name = 'a';
            int k = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Rectangle rec = new Rectangle()
                    {
                        Name = name.ToString(),
                        Width = width,
                        Height = height,
                        Opacity = 1,
                        Fill = Brushes.Black,
                        Stroke = Brushes.White,
                        StrokeThickness = 2,
                    };

                    LvlBuilder(name.ToString(), rec, k);
                    
                    myGameCanvas.Children.Add(rec);
                    Canvas.SetTop(rec, top);
                    Canvas.SetLeft(rec, left);
                    left += 100;
                    name++;
                    k++;
                }
                top -= 25;
                left = 0;
            }
        }

        private void LvlBuilder(string name, Rectangle rec, int k)
        {
            
            Match match1 = Regex.Match(name, @"(?i)^[a-f]+");
            Match match2 = Regex.Match(name, @"(?i)^[g-l]+");
            Match match3 = Regex.Match(name, @"(?i)^[m-r]+");

            if (match1.Success)
            {
                if (LvL == 2 || LvL == 3)
                {
                    rec.Fill = Brushes.DarkGray;
                    // brick.HitPoints = 2;
                    points[k] = 2;
                }
                else
                {
                    rec.Fill = Brushes.LightBlue;
                    //brick.HitPoints = 1;
                    points[k] = 1;
                }

            }
            else if (match2.Success)
            {
                if (LvL == 3)
                {
                    rec.Fill = Brushes.Black;
                    //brick.HitPoints = 3;
                    points[k] = 3;
                }
                else
                {
                    rec.Fill = Brushes.DarkGray;
                    // brick.HitPoints = 2;
                    points[k] = 2;
                }
            }
            else if (match3.Success)
            {
                rec.Fill = Brushes.Black;
                //brick.HitPoints = 3;
                points[k] = 3;
            }
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            pause.IsEnabled = true;
            gameTimer.Start();
            PauseMenu.IsEnabled = false;
            PauseMenu.Visibility = Visibility.Hidden;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
