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
        public Image[,] map = new Image[4, 4];
        public int[,] field = new int[4, 4];
        public int playerRow = 3, playerCol = 0;

        public MainWindow()
        {
            InitializeComponent();
            richTextBox1.Document.LineHeight = 0.1;
        }

        public void drawCell(int row, int col, string path)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@path, UriKind.Relative);
            logo.EndInit();
            map[row, col].Source = logo;
        }

        public void drawMap(int[,] field, int plRow, int plCol)
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
                    }
                }
            }
        }

        public void CreateMap(int[,] map)
        {
            Random r = new Random();
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    map[row, col] = 0;
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
                    if (n < 20 && row != 0 && col != 0)
                        map[row, col] = 1;
                }
            }
            pitRow = r.Next(4);
            pitCol = r.Next(4);
            while (pitRow == 3 && pitCol == 0)
            {
                pitRow = r.Next(4);
                pitCol = r.Next(4);
            }
            map[pitRow, pitCol] = 2;
            pitRow = r.Next(4);
            pitCol = r.Next(4);
            while (pitRow == 3 && pitCol == 0)
            {
                pitRow = r.Next(4);
                pitCol = r.Next(4);
            }
            map[pitRow, pitCol] = 3;
            //drawCell(pitRow, pitCol, "/WumpusG;component/resources/gold_trophy_trophy_prize_winner_gold_cup.png");
        }

        public int checkMove(int[,] mapL, int newRow, int newCol)
        {
            if (newRow < 0 || newRow > 3 || newCol < 0 || newCol > 3)
            {
                richTextBox1.AppendText("You hit the wall\n");
                return 0;
            }
            else if (mapL[newRow, newCol] == 1)
            {
                richTextBox1.AppendText("You fall into a pit\n");
                drawCell(newRow, newCol, "/WumpusG;component/resources/blackhole.png");
                return 2;
            }
            else if (mapL[newRow, newCol] == 2)
            {
                richTextBox1.AppendText("You've eaten by the Wumpus\n");
                drawCell(newRow, newCol, "/WumpusG;component/resources/monster_green.png");
                return 2;
            }
            else if (mapL[newRow, newCol] == 3)
            {
                richTextBox1.AppendText("You found the Gold! \n");
                drawCell(newRow, newCol, "/WumpusG;component/resources/gold_trophy_trophy_prize_winner_gold_cup.png");
                field[newRow, newCol] = 0;
                return 3;
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1] == 1) ||
                (newCol < 3 && mapL[newRow, newCol + 1] == 1) ||
                (newRow > 0 && mapL[newRow - 1, newCol] == 1) ||
                (newRow < 3 && mapL[newRow + 1, newCol] == 1))
            {
                richTextBox1.AppendText("You fell a breeze\n");
                if (newCol > 0 && field[newRow, newCol - 1] != 4)
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newCol < 3 && field[newRow, newCol + 1] != 4)
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newRow > 0 && field[newRow - 1, newCol] != 4)
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/folder_ele_wind_weather.png");
                if (newRow < 3 && field[newRow + 1, newCol] != 4)
                    drawCell(newRow + 1, newCol, "/WumpusG;component/resources/folder_ele_wind_weather.png");
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1] == 3) ||
                (newCol < 3 && mapL[newRow, newCol + 1] == 3) ||
                (newRow > 0 && mapL[newRow - 1, newCol] == 3) ||
                (newRow < 3 && mapL[newRow + 1, newCol] == 3))
            {
                richTextBox1.AppendText("You see shining\n");
            }

            if ((newCol > 0 && mapL[newRow, newCol - 1] == 2) ||
               (newCol < 3 && mapL[newRow, newCol + 1] == 2) ||
               (newRow > 0 && mapL[newRow - 1, newCol] == 2) ||
               (newRow < 3 && mapL[newRow + 1, newCol] == 2))
            {
                richTextBox1.AppendText("You smell Wumpus\n");
                if (newCol > 0 && field[newRow, newCol - 1] != 4 && 
                    map[newRow, newCol - 1].Source.Equals("/WumpusG;component/resources/folder_ele_wind_weather.png"))
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newCol > 0 && field[newRow, newCol - 1] != 4)
                    drawCell(newRow, newCol - 1, "/WumpusG;component/resources/bad_smelly.png");
                if (newCol < 3 && field[newRow, newCol + 1] != 4 &&
                    map[newRow, newCol + 1].Source.Equals("/WumpusG;component/resources/folder_ele_wind_weather.png"))
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newCol < 3 && field[newRow, newCol + 1] != 4)
                    drawCell(newRow, newCol + 1, "/WumpusG;component/resources/bad_smelly.png");
                if (newRow > 0 && field[newRow - 1, newCol] != 4 &&
                    map[newRow - 1, newCol].Source.Equals("/WumpusG;component/resources/folder_ele_wind_weather.png"))
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newRow > 0 && field[newRow - 1, newCol] != 4)
                    drawCell(newRow - 1, newCol, "/WumpusG;component/resources/bad_smelly.png");
                if (newRow < 3 && field[newRow + 1, newCol] != 4 &&
                    map[newRow + 1, newCol].Source.Equals("/WumpusG;component/resources/folder_ele_wind_weather.png"))
                    drawCell(newRow + 1, newCol, "/WumpusG;component/resources/bad_smelly_and_wind.png");
                else if (newRow < 3 && field[newRow + 1, newCol] != 4)
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
            richTextBox1.AppendText("You step Up\n");
            int state = 0;
            state = checkMove(field, playerRow - 1, playerCol);
            if (state > 0)
            {
                playerRow -= 1;
            }
            if(state !=2)
                drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
            field[playerRow + 1, playerCol] = 4;
            drawCell(playerRow + 1, playerCol, "/WumpusG;component/resources/field.png"); 
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            richTextBox1.AppendText("You step Down\n");
            int state = 0;
            state = checkMove(field, playerRow + 1, playerCol);
            if (state > 0)
            {
                playerRow += 1;
            }
            if (state != 2)
                drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
            field[playerRow - 1, playerCol] = 4;
            drawCell(playerRow - 1, playerCol, "/WumpusG;component/resources/field.png"); 
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            richTextBox1.AppendText("You step Right\n");
            int state = 0;
            state = checkMove(field, playerRow, playerCol + 1);
            if (state > 0)
            {
                playerCol += 1;
            }
            if (state != 2)
                drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
            field[playerRow, playerCol - 1] = 4;
            drawCell(playerRow, playerCol - 1, "/WumpusG;component/resources/field.png"); 
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            richTextBox1.AppendText("You step Right\n");
            int state = 0;
            state = checkMove(field, playerRow, playerCol - 1);
            if (state > 0)
            {
                playerCol -= 1;
            }
            if (state != 2)
                drawCell(playerRow, playerCol, "/WumpusG;component/resources/avatar_hero_superhero_loki_avengers.png");
            field[playerRow, playerCol + 1] = 4;
            drawCell(playerRow, playerCol + 1, "/WumpusG;component/resources/field.png"); 
        }
    }
}
