using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WumpusG
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int direction = 1;

        public struct cell
        {
            public bool isPit; 
            public bool isGold;
            public bool isWumpus;
            public bool isSafe;
            public bool isFog;
        }

        public int score = 100;
        public Image[,] map = new Image[4, 4];
        public cell[,] field = new cell[4, 4];
        public int playerRow = 3, playerCol = 0;

        public MainWindow()
        {
            InitializeComponent();
            richTextBox1.Document.LineHeight = 0.1;
            label2.Content = score;
        }

        public void drawCell(int row, int col, string path)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@path, UriKind.Relative);
            logo.EndInit();
            map[row, col].Source = logo;
        }

        public void drawMap(cell[,] field, int plRow, int plCol)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (plRow == i && plCol == j)
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri(@"/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png", UriKind.Relative);
                        logo.EndInit();
                        map[i, j].Source = logo;
                        //RotateTransform angle = new RotateTransform(90);
                        //map[i, j].RenderTransformOrigin=new Point(0.5,0.5);
                        //map[i, j].RenderTransform = angle;
                    }
                }
            }
        }

        public void CreateMap(cell[,] map)
        {
            Random r = new Random();
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    map[row, col].isFog = true;
                }
            }
            int pitRow, pitCol;
            //for (int pitNum = 0; pitNum < 4; pitNum++)
            //{
            //    pitRow = r.Next(4);
            //    pitCol = r.Next(4);
            //    while (pitRow == 3 && pitCol == 0)
            //    {
            //        pitRow = r.Next(4);
            //        pitCol = r.Next(4);
            //    }
            //    map[pitRow, pitCol] = 1;
            //}
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int n = new Random().Next(1, 100);
                    if (n < 40 && row != 0 && col != 0)
                        map[row, col].isPit = true;
                }
            }
            pitRow = r.Next(4);
            pitCol = r.Next(4);
            while (pitRow == 3 && pitCol == 0)
            {
                pitRow = r.Next(4);
                pitCol = r.Next(4);
            }
            map[pitRow, pitCol].isWumpus = true;
            pitRow = r.Next(4);
            pitCol = r.Next(4);
            while (pitRow == 3 && pitCol == 0)
            {
                pitRow = r.Next(4);
                pitCol = r.Next(4);
            }
            map[pitRow, pitCol].isGold = true;
            //drawCell(pitRow, pitCol, "/WumpusG;component/resources/gold_trophy_trophy_prize_winner_gold_cup.png");
        }

        public int checkShoot(cell[,] mapL, int newRow, int newCol)
        {
            if (newRow < 0 || newRow > 3 || newCol < 0 || newCol > 3)
            {
                richTextBox1.AppendText("Arrow hit the wall\n");
                richTextBox1.ScrollToEnd();
                return 0;
            }
            else if (mapL[newRow, newCol].isWumpus)
            {
                richTextBox1.AppendText("You kill the Wumpus\n");
                richTextBox1.ScrollToEnd();
                drawCell(newRow, newCol, "/WumpusG;component/resources/monster_green.png");
                field[newRow, newCol].isWumpus = false;
                return 0;
            }
            return 1;
        }

        public int checkMove(cell[,] mapL, int newRow, int newCol)
        {
            if (newRow < 0 || newRow > 3 || newCol < 0 || newCol > 3)
            {
                richTextBox1.AppendText("You hit the wall\n");
                richTextBox1.ScrollToEnd();
                return 0;
            }
            else if (mapL[newRow, newCol].isPit)
            {
                richTextBox1.AppendText("You fall into a pit\n");
                richTextBox1.ScrollToEnd();
                drawCell(newRow, newCol, "/WumpusG;component/resources/blackhole.png");
                return 2;
            }
            else if (mapL[newRow, newCol].isWumpus)
            {
                richTextBox1.AppendText("You've eaten by the Wumpus\n");
                richTextBox1.ScrollToEnd();
                drawCell(newRow, newCol, "/WumpusG;component/resources/monster_green.png");
                return 2;
            }
            else if (mapL[newRow, newCol].isGold)
            {
                richTextBox1.AppendText("You found the Gold! \n");
                richTextBox1.ScrollToEnd();
                drawCell(newRow, newCol, "/WumpusG;component/resources/gold_trophy_trophy_prize_winner_gold_cup.png");
                field[newRow, newCol].isGold = false;
                label2.Content = score += 1000;
                return 3;
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1].isPit) ||
                (newCol < 3 && mapL[newRow, newCol + 1].isPit) ||
                (newRow > 0 && mapL[newRow - 1, newCol].isPit) ||
                (newRow < 3 && mapL[newRow + 1, newCol].isPit))
            {
                richTextBox1.AppendText("You fell a breeze\n");
                richTextBox1.ScrollToEnd();
                if (newCol > 0 && !field[newRow, newCol - 1].isSafe)
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newCol < 3 && !field[newRow, newCol + 1].isSafe)
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newRow > 0 && !field[newRow - 1, newCol].isSafe)
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newRow < 3 && !field[newRow + 1, newCol].isSafe)
                    drawCell(newRow + 1, newCol, "/WumpusG;component/resources/folder_ele_wind_weather.png");
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1].isGold) ||
                (newCol < 3 && mapL[newRow, newCol + 1].isGold) ||
                (newRow > 0 && mapL[newRow - 1, newCol].isGold) ||
                (newRow < 3 && mapL[newRow + 1, newCol].isGold))
            {
                richTextBox1.AppendText("You see shining\n");
                richTextBox1.ScrollToEnd();
            }

            if ((newCol > 0 && mapL[newRow, newCol - 1].isWumpus) ||
               (newCol < 3 && mapL[newRow, newCol + 1].isWumpus) ||
               (newRow > 0 && mapL[newRow - 1, newCol].isWumpus) ||
               (newRow < 3 && mapL[newRow + 1, newCol].isWumpus))
            {
                richTextBox1.AppendText("You smell Wumpus\n");
                richTextBox1.ScrollToEnd();
                if (newCol > 0 && !field[newRow, newCol - 1].isSafe &&
                    field[newRow, newCol - 1].isPit)
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newCol > 0 && !field[newRow, newCol - 1].isSafe)
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/bad_smelly.png");
                if (newCol < 3 && !field[newRow, newCol + 1].isSafe &&
                    field[newRow, newCol + 1].isPit)
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newCol < 3 && !field[newRow, newCol + 1].isSafe)
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/bad_smelly.png");
                if (newRow > 0 && !field[newRow - 1, newCol].isSafe &&
                    field[newRow - 1, newCol].isPit)
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newRow > 0 && !field[newRow - 1, newCol].isSafe)
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/bad_smelly.png");
                if (newRow < 3 && !field[newRow + 1, newCol].isSafe &&
                    field[newRow + 1, newCol].isPit)
                    drawCell(newRow + 1, newCol, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newRow < 3 && !field[newRow + 1, newCol].isSafe)
                    drawCell(newRow + 1, newCol, "/WumpusG;component/resources/bad_smelly.png");
            }
            return 1;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    map[row, col] = new Image();
                    var transform = map[row, col].RenderTransform as TranslateTransform;
                    if (transform == null)
                    {
                        transform = new TranslateTransform();
                        map[row, col].RenderTransform = transform;
                    }
                    transform.X = col * 64;
                    transform.Y = row * 64;
                    map[row, col].RenderTransform = transform;
                    map[row, col].Width = 64;
                    map[row, col].Height = 64;

                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"/WumpusG;component/resources/Fog.png", UriKind.Relative);
                    logo.EndInit();
                    map[row, col].Source = logo;
                    canvas1.Children.Add(map[row, col]);
                }
            }
            CreateMap(field);
            checkMove(field, playerRow, playerCol);
            drawMap(field, playerRow, playerCol);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (direction != 1)
            {
                direction = 1;
                richTextBox1.AppendText("You look Up\n");
                richTextBox1.ScrollToEnd();
                label2.Content = --score;
            }
            else
            {
                richTextBox1.AppendText("You step Up\n");
                richTextBox1.ScrollToEnd();
                int state = 0;
                state = checkMove(field, playerRow - 1, playerCol);
                if (state > 0)
                {
                    playerRow -= 1;
                }
                if (state != 2)
                    drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
                field[playerRow + 1, playerCol].isSafe = true;
                field[playerRow + 1, playerCol].isFog = false;
                drawCell(playerRow + 1, playerCol, "/WumpusG;component/resources/field.png");
                label2.Content = --score;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (direction != 2)
            {
                direction = 2;
                richTextBox1.AppendText("You look Down\n");
                richTextBox1.ScrollToEnd();
                label2.Content = --score;
            }
            else
            {
                direction = 2;
                richTextBox1.AppendText("You step Down\n");
                richTextBox1.ScrollToEnd();
                int state = 0;
                state = checkMove(field, playerRow + 1, playerCol);
                if (state > 0)
                {
                    playerRow += 1;
                }
                if (state != 2)
                    drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
                field[playerRow - 1, playerCol].isSafe = true;
                field[playerRow - 1, playerCol].isFog = false;
                drawCell(playerRow - 1, playerCol, "/WumpusG;component/resources/field.png");
                label2.Content = --score;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (direction != 4)
            {
                direction = 4;
                richTextBox1.AppendText("You loor Right\n");
                richTextBox1.ScrollToEnd();
                label2.Content = --score;
            }
            else
            {
                direction = 4;
                richTextBox1.AppendText("You step Right\n");
                richTextBox1.ScrollToEnd();
                int state = 0;
                state = checkMove(field, playerRow, playerCol + 1);
                if (state > 0)
                {
                    playerCol += 1;
                }
                if (state != 2)
                    drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
                field[playerRow, playerCol - 1].isSafe = true;
                field[playerRow, playerCol - 1].isFog = false;
                drawCell(playerRow, playerCol - 1, "/WumpusG;component/resources/field.png");
                label2.Content = --score;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (direction != 3)
            {
                direction = 3;
                richTextBox1.AppendText("You look Left\n");
                richTextBox1.ScrollToEnd();
                label2.Content = --score;
            }
            else
            {
                direction = 3;
                richTextBox1.AppendText("You step Right\n");
                richTextBox1.ScrollToEnd();
                int state = 0;
                state = checkMove(field, playerRow, playerCol - 1);
                if (state > 0)
                {
                    playerCol -= 1;
                }
                if (state != 2)
                    drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
                field[playerRow, playerCol + 1].isSafe = true;
                field[playerRow, playerCol + 1].isFog = false;
                drawCell(playerRow, playerCol + 1, "/WumpusG;component/resources/field.png");
                label2.Content = --score;
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            int state = -1;
            switch (direction)
            {
                case 1:
                    do
                    {
                        state = checkShoot(field, playerRow - 1, playerCol);
                    }
                    while (state != 0);
                    break;
                case 2:
                    do
                    {
                        state = checkShoot(field, playerRow + 1, playerCol);
                    }
                    while (state != 0) ;
                    break;
                case 3:
                    do
                    {
                        state = checkShoot(field, playerRow, playerCol - 1);
                    }
                    while (state != 0) ;
                    break;
                case 4:
                    do
                    {
                        state = checkShoot(field, playerRow, playerCol + 1);
                    }
                    while (state != 0) ;
                    break;
            }
            label2.Content = score-=10;
        }
    }
}
