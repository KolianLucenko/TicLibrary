﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

 
namespace CoolGameLibrary
{
    /// <summary>
    /// Перечисление типа игрока (X,Y)
    /// </summary>
    public enum PlayerType { User,Computer}
    /// <summary>
    /// "Тип" победы
    /// </summary>
    public enum WinType { Horizont_1, Horizont_2, Horizont_3, Vert_1, Vert_2, Vert_3,Diagon_1,Diagon_2,NULL }

    /// <summary>
    /// Класс для работы с игрой "Крестики-нолики" 
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Метод возвращающий путь к картинке 
        /// </summary>
        /// <param name="myUri">Двумерный массив путей к картинкам</param>
        /// <param name="pType">Тип игрока(крестики или нолики)</param>
        /// <returns>Путь к картинке в строковом виде</returns>
        static public string GetImage(string[,] myUri,PlayerType pType)
        {
            Random ran = new Random();
            int c;
            int i = ran.Next(0, myUri.GetLength(0));
            if (pType == PlayerType.User)
                c = 0;
            else
                c = 1;
            return myUri[c,i];
        }

        /// <summary>
        /// Заполнение двумерного массива (путя к картинкам)
        /// </summary>
        /// <param name="patch">Имя папки(ресурса) с картинками</param>
        /// <param name="UserImage">Двумерный массив который в который запишутся путя</param>
        static public void FillArray(string patch, out string[,] UserImage)
        {
            UserImage = new string[2, 3];
            string pat1 = Assembly.GetExecutingAssembly().Location.Replace('\\', '/');
            string[] pat = pat1.Split('/');
            List<string> l = new List<string>();
            foreach (string s in pat)
                l.Add(s);
            l.RemoveRange(l.Count - 1, 1);
            string p = string.Empty;
            foreach (string s in l)
                p += s + "/";

            int i = 0;
            int c = 0;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(p + patch);
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    c = 0;
                    foreach (FileInfo f in d.GetFiles())
                    {
                        UserImage[i, c] = string.Format("{0}", f.FullName.Replace('\\', '/'));
                        c++;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {

            }    
        }
        

        /// <summary>
        /// Расчёт правильных координат для добавления на экран рисунка
        /// </summary>
        /// <param name="pt">Точка нажатия на экран(Х,Y)</param>
        /// <param name="Arr">Индексатор картинки</param>
        /// <returns>Возврат "Коректных" координат для вставки картинки</returns>
        static public Point GetPoint(Point pt, out int[] ArrPoint)
        {
            Point po = new Point();
            // Индексатор для GameMap
            ArrPoint = new int[2];


            if (pt.X > 0 && pt.X < 105) { po.X = 0; ArrPoint[0] = 0; }
            else if (pt.X > 105 && pt.X < 210) { po.X = 110; ArrPoint[0] = 1; }
            else if (pt.X > 210 && pt.X < 320) { po.X = 220; ArrPoint[0] = 2; }

            if (pt.Y > 0 && pt.Y < 105) { po.Y = 0; ArrPoint[1] = 0; }
            else if (pt.Y > 105 && pt.Y < 210) { po.Y = 110; ArrPoint[1] = 1; }
            else if (pt.Y > 210 && pt.Y < 320) { po.Y = 220; ArrPoint[1] = 2; }

            return po;
        }
        /// <summary>
        /// Расчёт правильных координат для добавления на экран рисунка
        /// </summary>
        /// <param name="pt">Точка нажатия на экран(Х,Y)</param>
        /// <returns>Возврат "Коректных" координат для вставки картинки</returns>
        static public Point GetPoint(Point pt)
        {
            Point p = new Point();

            if (pt.X == 0 ) p.X = 0; 
            else if (pt.X == 1) p.X = 110;
            else if (pt.X == 2) p.X = 220;

            if (pt.Y == 0) p.Y = 0;
            else if (pt.Y == 1) p.Y = 110;
            else if (pt.Y == 2) p.Y = 220;

            return p;
        }

        /// <summary>
        /// Легкая сложность - Поиск случайно не занятой позиции на игровой карте(3 на 3)
        /// </summary>
        /// <param name="map">Игровая карта(3 на 3)</param>
        /// <returns>Координаты для вставки картинки</returns>
        static public Point EaseComputerRun(ref int[,] map)
        {
            List<Point> TempIn = new List<Point>();
            for (int a = 0; a < 3; a++)
                for (int b = 0; b < 3; b++)
                    if (map[a, b] == 0)
                        // Добавить во временной индексатор
                        TempIn.Add(new Point((double)a, (double)b));

            Random r = new Random();
            // Вернуть рандомное число в диапозоне пустых ячеек
            Point newPoint = TempIn[r.Next(0, TempIn.Count - 1)];
            map[(int)newPoint.X, (int)newPoint.Y] = -1;

            return GetPoint(newPoint);
        }

        /// <summary>
        /// Средняя сложность - Поиск случайной или НЕ случайно позиции на игровой карте (3 на 3)
        /// </summary>
        /// <param name="map">Игровая карта(3 на 3)</param>
        /// <returns>Координаты для вставки картинки</returns>
        static public Point NormalComputerRun(ref int[,] map)
        {
            /*  ВСЕ ПУСТЫЕ  */
            List<Point> TempRandIn = new List<Point>();

            for (int a = 0; a < map.GetLength(0); a++)
                for (int b = 0; b < map.GetLength(1); b++)
                    if (map[a, b] == 0)
                        // Добавить во временной индексатор
                        TempRandIn.Add(new Point((double)a, (double)b));

            /*  ЛОГИКА  */
            List<Point> TempCompLogIn = new List<Point>();
            List<Point> TempUserLogIn = new List<Point>();
            CheckGameMapOnWinTach(-1, ref TempCompLogIn, map);
            CheckGameMapOnWinTach(1, ref TempUserLogIn, map);
            

            /*  ВОЗВРАТ КООРДИНАТЫ */
            Point newPoint;
            Random r = new Random();
            if (TempUserLogIn.Count > 0 && r.Next(0,100)<=65)
            {
                newPoint = TempUserLogIn[r.Next(0, TempUserLogIn.Count - 1)];
                map[(int)newPoint.X, (int)newPoint.Y] = -1;
                return GetPoint(newPoint);
            }
            else if (TempCompLogIn.Count > 0 && r.Next(0, 100) <= 65)
            {
                newPoint = TempCompLogIn[r.Next(0, TempCompLogIn.Count - 1)];
                map[(int)newPoint.X, (int)newPoint.Y] = -1;
                return GetPoint(newPoint);
            }
            else
            {
                newPoint = TempRandIn[r.Next(0, TempRandIn.Count - 1)];
                map[(int)newPoint.X, (int)newPoint.Y] = -1;
                return GetPoint(newPoint);
            }
        }

        /// <summary>
        /// Трудная сложность - Поиск случайной или НЕ случайно позиции на игровой карте (3 на 3)
        /// </summary>
        /// <param name="map">Игровая карта(3 на 3)</param>
        /// <returns>Координаты для вставки картинки</returns>
        static public Point HardComputerRun(ref int[,] map)
        {
            /*  ВСЕ ПУСТЫЕ  */
            List<Point> TempRandIn = new List<Point>();

            for (int a = 0; a < map.GetLength(0); a++)
                for (int b = 0; b < map.GetLength(1); b++)
                    if (map[a, b] == 0)
                        // Добавить во временной индексатор
                        TempRandIn.Add(new Point((double)a, (double)b));

            /*  ЛОГИКА  */
            List<Point> TempCompLogIn = new List<Point>();
            List<Point> TempUserLogIn = new List<Point>();
            CheckGameMapOnWinTach(-1, ref TempCompLogIn, map);
            CheckGameMapOnWinTach(1, ref TempUserLogIn, map);


            /*  ВОЗВРАТ КООРДИНАТЫ */
            Point newPoint;
            Random r = new Random();
            if (TempUserLogIn.Count > 0)
            {
                newPoint = TempUserLogIn[r.Next(0, TempUserLogIn.Count - 1)];
                map[(int)newPoint.X, (int)newPoint.Y] = -1;
                return GetPoint(newPoint);
            }
            else if (TempCompLogIn.Count > 0)
            {
                if (r.Next(0, 100) <= 80)
                {
                    newPoint = TempCompLogIn[r.Next(0, TempCompLogIn.Count - 1)];
                    map[(int)newPoint.X, (int)newPoint.Y] = -1;
                    return GetPoint(newPoint);
                }
                else
                {
                    newPoint = TempRandIn[r.Next(0, TempRandIn.Count - 1)];
                    map[(int)newPoint.X, (int)newPoint.Y] = -1;
                    return GetPoint(newPoint);
                }
            }
            else
            {
                newPoint = TempRandIn[r.Next(0, TempRandIn.Count - 1)];
                map[(int)newPoint.X, (int)newPoint.Y] = -1;
                return GetPoint(newPoint);
            }
        }

        /// <summary>
        /// Сбор Point(ов) в случае двух совпадений на ряд
        /// </summary>
        /// <param name="num">Число для сравнения</param>
        /// <param name="LP">Лист Point(ов)</param>
        /// <param name="map">Игровая карта</param>
        static void CheckGameMapOnWinTach(int num,ref List<Point> LP,int[,] map){
            for(int i = 0;i<map.GetLength(0);i++){
                if ((map[i, 0] == num) && (map[i, 1] == num) && (map[i, 2] == 0))
                    LP.Add(new Point((double)i, (double)2));
                if ((map[i, 0] == num) && (map[i, 1] == 0) && (map[i, 2] == num))
                    LP.Add(new Point((double)i, (double)1));
                if ((map[i, 0] == 0) && (map[i, 1] == num) && (map[i, 2] == num))
                    LP.Add(new Point((double)i, (double)0));
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                if ((map[0, i] == num) && (map[1, i] == num) && (map[2, i] == 0))
                    LP.Add(new Point((double)2, (double)i));
                if ((map[0, i] == num) && (map[1, i] == 0) && (map[2, i] == num))
                    LP.Add(new Point((double)1, (double)i));
                if ((map[0, i] == 0) && (map[1, i] == num) && (map[2, i] == num))
                    LP.Add(new Point((double)0, (double)i));
            }

            if ((map[0, 0] == num) && (map[1, 1] == num) && (map[2, 2] == 0))
                LP.Add(new Point((double)2, (double)2));
            if ((map[0, 0] == num) && (map[1, 1] == 0) && (map[2, 2] == num))
                LP.Add(new Point((double)1, (double)1));
            if ((map[0, 0] == 0) && (map[1, 1] == num) && (map[2, 2] == num))
                LP.Add(new Point((double)0, (double)0));


            if ((map[0, 2] == num) && (map[1, 1] == num) && (map[2, 0] == 0))
                LP.Add(new Point((double)2, (double)0));
            if ((map[0, 2] == num) && (map[1, 1] == 0) && (map[2, 0] == num))
                LP.Add(new Point((double)1, (double)1));
            if ((map[0, 2] == 0) && (map[1, 1] == num) && (map[2, 0] == num))
                LP.Add(new Point((double)0, (double)0));
        }

        /// <summary>
        /// Добавить картинку на полотно
        /// </summary>
        /// <param name="p">"Коректные" координаты"</param>
        /// <param name="UserImage">Двумерный массив с картинками</param>
        /// <param name="pt">"Тип" картинки</param>
        /// <param name="myCanvas">Полотно для записи картинки</param>
        static public void AddImage(Point p, string[,] UserImage, PlayerType pt, Canvas myCanvas)
        {
            Image im = new Image();
            im.Source = new BitmapImage(new Uri(Game.GetImage(UserImage, pt), UriKind.Absolute));
            Canvas.SetLeft(im, p.X);
            Canvas.SetTop(im, p.Y);
            myCanvas.Children.Add(im);
        }

        /// <summary>
        /// Проверка игровой карты на выигрыш
        /// </summary>
        /// <param name="map">Игровая карта</param>
        /// <param name="i">Число обозначающее игрока(1 - человек, 2 - компьютер)</param>
        /// <returns>"Тип" победы</returns>
        static public WinType CheckWin(int[,] map, int i)
        {
            // горизонт
            if (map[0, 0] == i && map[1, 0] == i && map[2, 0] == i)  return WinType.Horizont_1; 
            if (map[0, 1] == i && map[1, 1] == i && map[2, 1] == i) return WinType.Horizont_2; 
            if (map[0, 2] == i && map[1, 2] == i && map[2, 2] == i) return WinType.Horizont_3; 
            // верт
            if (map[0, 0] == i && map[0, 1] == i && map[0, 2] == i) return WinType.Vert_1; 
            if (map[1, 0] == i && map[1, 1] == i && map[1, 2] == i) return WinType.Vert_2;
            if (map[2, 0] == i && map[2, 1] == i && map[2, 2] == i) return WinType.Vert_3;
            // оси
            if (map[0, 0] == i && map[1, 1] == i && map[2, 2] == i) return WinType.Diagon_1;
            if (map[0, 2] == i && map[1, 1] == i && map[2, 0] == i) return WinType.Diagon_2;

            return WinType.NULL;
        }

        /// <summary>
        /// Картинка победы (перечёркивание)
        /// </summary>
        /// <param name="c">Полотно для внесения картинки</param>
        /// <param name="wt">"Тип" победы</param>
        static public void GetImage(Canvas c, WinType wt)
        {
            Shape img;

            /*------------------- ЕСЛИ ГОРИЗОНТ----------------------------*/
            if (wt == WinType.Horizont_1)
            {
                img = new Line() { X1 = 0, X2 = 315, Y1 = 0, Y2 = 0 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 50);
                c.Children.Add(img);
            }
            if (wt == WinType.Horizont_2)
            {
                img = new Line() { X1 = 0, X2 = 315, Y1 = 0, Y2 = 0 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 160);
                c.Children.Add(img);
            }
            if (wt == WinType.Horizont_3)
            {
                img = new Line() { X1 = 0, X2 = 315, Y1 = 0, Y2 = 0 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 0);
                Canvas.SetTop(img, 270);
                c.Children.Add(img);
            }
            /*------------------- ЕСЛИ ВЕРТИКАЛЬ----------------------------*/
            if (wt == WinType.Vert_1)
            {
                img = new Line() { X1 = 0, X2 = 0, Y1 = 0, Y2 = 315 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 50);
                Canvas.SetTop(img, 0);
                c.Children.Add(img);
            }
            if (wt == WinType.Vert_2)
            {
                img = new Line() { X1 = 0, X2 = 0, Y1 = 0, Y2 = 315 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 160);
                Canvas.SetTop(img, 0);
                c.Children.Add(img);
            }
            if (wt == WinType.Vert_3)
            {
                img = new Line() { X1 = 0, X2 = 0, Y1 = 0, Y2 = 315 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 270);
                Canvas.SetTop(img, 0);
                c.Children.Add(img);
            }
            /*------------------ДИАГОНАЛЬ-----------------------------------*/
            if (wt == WinType.Diagon_1)
            {
                img = new Line() { X1 = 0, X2 = 315, Y1 = 0, Y2 = 315 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                Canvas.SetLeft(img, 0);
                Canvas.SetRight(img, 0);
                c.Children.Add(img);
            }
            if (wt == WinType.Diagon_2)
            {
                img = new Line() { X1 = 315, X2 = 0, Y1 = 0, Y2 = 315 };
                img.Stroke = Brushes.Blue;
                img.StrokeThickness = 4;
                //Canvas.SetLeft(img, X);
                //Canvas.SetRight(img, Y);
                c.Children.Add(img);
            }
        }

        /// <summary>
        /// Заполнить карту игры
        /// </summary>
        /// <param name="g">"Карта" игры</param>
        static public void FillMap(out int[,] g)
        {
            g = new int[3, 3];
            for (int a = 0; a < 3; a++)
                for (int b = 0; b < 3; b++)
                    g[a, b] = 0;
        }

        /// <summary>
        /// Проверка на ничью
        /// </summary>
        /// <param name="GameMap">Двумерный массив с картой игры</param>
        /// <returns></returns>
        static public bool ChekingDeadHeat(int[,] GameMap)
        {
            for (int i = 0; i < GameMap.GetLength(0); i++)
                for (int c = 0; c < GameMap.GetLength(1); c++)
                    if(GameMap[i,c]==0) 
                        return false;

            return true;
        }

        /// <summary>
        /// Сериализует объект статистики в файл
        /// </summary>
        /// <param name="obj">Объект статистики</param>
        static public void SaveStatistic(Statistic obj)
        {
            BinaryFormatter binForm = new BinaryFormatter();
            if (!File.Exists("SaveStatistic.dat"))
            {
                using (Stream fstr = new FileStream("SaveStatistic.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    binForm.Serialize(fstr, obj);
                }
            }
            else
            {
                using (Stream fstr = new FileStream("SaveStatistic.dat", FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    binForm.Serialize(fstr, obj);
                }
            }
            
        }

        /// <summary>
        /// Десериализация статистика в объект
        /// </summary>
        /// <returns>Объект статистики</returns>
        static public Statistic LoadStatistic()
        {
            if (File.Exists("SaveStatistic.dat"))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                using (Stream fstr = File.OpenRead("SaveStatistic.dat"))
                {
                    return (Statistic)binForm.Deserialize(fstr);
                }
            }
            else
                return new Statistic();
        }
    }
}
