using System;
using static System.Console;
using System.Threading;

namespace Snake
{
    class Snake {
        public static int L = 4;//длина змеи
        public static int[] crdX = new int[L];//координаты частей змеи
        public static int[] crdY = new int[L];
        int napr = 2;

        public Snake() {//КОНСТРУКТОР
            crdY[0] = 5;
            crdX[0] = 5;
        }

        public void Head(ConsoleKeyInfo Inf)//точка входа при нажатие клавиши
        {
            int X=crdX[0], Y=crdY[0];
            switch (Inf.Key)
            {
                case ConsoleKey.UpArrow:
                    if (napr == 2)
                    {
                        Head();
                        return;
                    }
                    Y--;
                    napr = 0;
                    break;
                case ConsoleKey.DownArrow:
                    if (napr == 0)
                    {
                        Head();
                        return;
                    }
                    Y++;
                    napr = 2;
                    break;
                case ConsoleKey.LeftArrow:
                    if (napr == 1) {
                        Head();
                        return;
                    }
                    X--;
                    napr = 3;
                    break;
                case ConsoleKey.RightArrow:
                    if (napr == 3)
                    {
                        Head();
                        return;
                    }
                    X++;
                    napr = 1;
                    break;
            }
            StHead(X, Y);
        }

        public void Head()//точка входа при простое
        {
            int X = crdX[0], Y = crdY[0];
            X += (2-napr)*(napr%2);
            Y += (napr-1) * (1 - (napr % 2));
            StHead(X, Y);
        }

        public void StHead(int X,int Y)//продолжение HEAD()
        {

            SetCursorPosition(X*2, Y);
            Write("▀");
            SetCursorPosition(crdX[L-1]* 2, crdY[L-1]);
            Write(" ");
            for (int i = L - 1; i > 0; i--)
            {
                crdX[i] = crdX[i - 1];
                crdY[i] = crdY[i - 1];
            }
            crdX[0] = X;
            crdY[0] = Y;
            Test();
        }

        void Test()//ПРОВЕРКА НА СТОЛКНОВЕНИЯ
        {
            if((crdX[0]<1) || (crdX[0]>29) || (crdY[0] < 1) || (crdY[0] > 28))
            {
                Defolt();
            }
            for (int i = 1; i < L; i++)
            {
                if ((crdX[0] == crdX[i]) &&(crdY[0] == crdY[i]))
                {
                    Defolt();
                }
            }
        }
        void Defolt()//ВЕРНУТЬ К ИСХОДНЫМ ЗНАЧЕНИЯМ
        {
            Ball.score = 0;
            Clear();
            Program.Board();
            L = 2;
            Array.Resize(ref crdX, L);
            Array.Resize(ref crdY, L);
            crdX[0] = 5;
            crdY[0] = 5;
            napr = 2;
            CursorVisible = true;
            SetCursorPosition(28, 14);
            Write("PLAY");
            ReadKey();
            SetCursorPosition(12, 14);
            Write("                                 ");
            CursorVisible = false;
            Program.dead = true;
        }
    }

    class Ball
    {
        static public bool S = false;
        static public int score = 8;
        static public int crdXBALL;
        static public int crdYBALL;
        ConsoleColor Color =ConsoleColor.Green;
        public void Test()
        {
            if((Snake.crdX[0]==crdXBALL)&&(Snake.crdY[0] == crdYBALL)){
                Snake.L++;
                Array.Resize(ref Snake.crdX, Snake.L);
                Array.Resize(ref Snake.crdY, Snake.L);
                score++;
                if (score % 10 == 0)
                {
                    S = true;
                    return;
                }
                Draw();
            }
        }
        public void Draw()
        {
            
            Random rand = new Random();
            crdXBALL = rand.Next(1, 29);
            crdYBALL = rand.Next(1, 29);
            SetCursorPosition(crdXBALL * 2, crdYBALL);
            ForegroundColor = Color;
            Write("▀");
            ForegroundColor = ConsoleColor.Gray;
        }
    }
    class XBall : Ball
    {
        ConsoleColor Color = ConsoleColor.Red;
        public void Draw1()
        {
            Random rand = new Random();
            crdXBALL = rand.Next(2, 28);
            crdYBALL = rand.Next(2, 28);
            ForegroundColor = Color;
            SetCursorPosition((crdXBALL - 1) * 2, crdYBALL - 1);
            Write("▀ ▀ ▀");
            SetCursorPosition((crdXBALL - 1) * 2, crdYBALL);
            Write("▀ ▀ ▀");
            SetCursorPosition((crdXBALL - 1) * 2, crdYBALL + 1);
            Write("▀ ▀ ▀");
            ForegroundColor = Color;
        }
        public new void Test()
        {
            if (S == true) Draw1();S = false;
            if ((Snake.crdX[0] >= crdXBALL - 1) && (Snake.crdX[0] <= crdXBALL + 1) && (Snake.crdY[0] >= crdYBALL - 1) && (Snake.crdY[0] <= crdYBALL + 1))
            {
                SetCursorPosition((crdXBALL - 1) * 2, crdYBALL - 1);
                Write("     ");
                SetCursorPosition((crdXBALL - 1) * 2, crdYBALL);
                Write("     ");
                SetCursorPosition((crdXBALL - 1) * 2, crdYBALL + 1);
                Write("     ");
                Snake.L++;
                Array.Resize(ref Snake.crdX, Snake.L);
                Array.Resize(ref Snake.crdY, Snake.L);
                score+=3;
                Draw();
            }
        }
    }
    class Program
    {
        public static bool dead =false;
        static void Main()
        {
            WindowWidth = 70;
            ConsoleKeyInfo Inf;
            Board();
            SetCursorPosition(28, 14);
            Write("PLAY");
            Inf = ReadKey();
            SetCursorPosition(12, 14);
            Write("                                 ");
            CursorVisible= false;
            Ball SBall = new Ball();
            XBall XBall = new XBall();
            SBall.Draw();
            Snake Snake = new Snake();
            do
            {
                if (dead) {
                    SBall.Draw();
                    dead = false;
                }
                INF();
                if ((Ball.score % 10 == 0)&&(Ball.score>0))XBall.Test();
                else SBall.Test();
                if (KeyAvailable)
                {
                    Inf = ReadKey();
                    if(Inf.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    Snake.Head(Inf);
                }
                else
                {
                    Snake.Head();
                }
                Thread.Sleep(100);
            }
            while (Inf.Key != ConsoleKey.Escape); 
        }
        static void INF()//ВЫВОДИТ ВСЮ ИНФУ(КООРДИНАТЫ ВСЕХ ТОЧЕК И ОЧКИ)
        {
            SetCursorPosition(62, 3);
            Write("S");
            SetCursorPosition(62, 4);
            Write("C");
            SetCursorPosition(62, 5);
            Write("O");
            SetCursorPosition(62, 6);
            Write("R");
            SetCursorPosition(62, 7);
            Write("E");
            SetCursorPosition(62, 9);
            Write(Ball.score);
            /*SetCursorPosition(64, 5);
            Write("crdXBALL = {0}  ", Ball.crdXBALL);
            SetCursorPosition(64, 6);
            Write("crdYBALL = {0}  ", Ball.crdYBALL);
            for (int i = 0; i < Snake.L; i++)
            {
                SetCursorPosition(64, 8 + i);
                Write("crdX[{0}] = {1,3}", i, Snake.crdX[i]);
                Write("crdY[{0}] = {1,3}", i, Snake.crdY[i]);
            }*/
        }
        public static void Board()//ОТРИСОВКА СТОЛА
        {
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine("▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀");
            for (int i = 0; i < 28; i++)
            {
                Write("▀");
                Write("                                                           ");
                ForegroundColor = ConsoleColor.DarkGray;
                WriteLine("▀");
            }
            Write("▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀ ▀");
            ForegroundColor = ConsoleColor.Gray;
        }
    }

}
