using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ConsoleSnake
{
    class Vector2
    {
        public int X;
        public int Y;
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        static List<Vector2> snakePos = new List<Vector2>();
        static int direction = 1;
        static Random r = new Random();
        static Vector2 foodPos;
        static double lastT = 0;
        static int sN = 0;
        static Vector2 screenBounds = new Vector2(80, 24);
        static ConsoleColor[] rainbowColor = new ConsoleColor[] {ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue};
        static bool canControl = true;

        static void reset()
        {
            snakePos.Clear();
            foodPos = new Vector2(r.Next(1, screenBounds.X), r.Next(1, screenBounds.Y));
            for (int i = 0; i < 6; i++)
                snakePos.Add(new Vector2(i + 1, 6));
            draw(true);
        }

        static void getInput()
        {
            while (!canControl)
            {
            }
                ConsoleKey ck = Console.ReadKey().Key;
                if (ck == ConsoleKey.UpArrow && direction != 1)
                    direction = 0;
                else
                    if (ck == ConsoleKey.DownArrow && direction != 0)
                        direction = 1;
                    else
                        if (ck == ConsoleKey.LeftArrow && direction != 3)
                            direction = 2;
                        else
                            if (ck == ConsoleKey.RightArrow && direction != 2)
                                direction = 3;
                            else
                                if (ck == ConsoleKey.Spacebar)
                                    draw(true);
                canControl = false;
            getInput();
        }

        static void Main(string[] args)
        {
            reset();

            new Thread(getInput).Start();
            draw(true);

            while (true)
            {
                for (int i = snakePos.Count - 1; i > 0; i--)
                    snakePos[i] = snakePos[i - 1];

                if (direction == 0)
                    snakePos[0] = new Vector2(snakePos[0].X, snakePos[0].Y - 1);
                if (direction == 1)
                    snakePos[0] = new Vector2(snakePos[0].X, snakePos[0].Y + 1);
                if (direction == 2)
                    snakePos[0] = new Vector2(snakePos[0].X - 1, snakePos[0].Y);
                if (direction == 3)
                    snakePos[0] = new Vector2(snakePos[0].X + 1, snakePos[0].Y);

                foreach (Vector2 sp in snakePos)
                    if (sp.X == foodPos.X && sp.Y == foodPos.Y)
                    {
                        foodPos = new Vector2(r.Next(1, screenBounds.X), r.Next(1, screenBounds.Y));
                        snakePos.Add(snakePos[snakePos.Count - 1]);
                        break;
                    }

                if (snakePos[0].Y < 0)
                    snakePos[0].Y = screenBounds.Y - 1;

                if (snakePos[0].X < 0)
                    snakePos[0].X = screenBounds.X - 1;

                if (snakePos[0].Y > screenBounds.Y - 1)
                    snakePos[0].Y = 0;

                if (snakePos[0].X > screenBounds.X - 1)
                    snakePos[0].X = 0;

                for (int i = snakePos.Count - 2; i > 0; i--)
                    if (snakePos[i].X == snakePos[0].X && snakePos[i].Y == snakePos[0].Y)
                    {
                        reset();
                        break;
                    }
                canControl = true;
                draw(true);
                Thread.Sleep(50);
            }
        }

        static void draw(bool clear)
        {
            if (!clear)
            {
                foreach (Vector2 pos in snakePos)
                {
                    Console.SetCursorPosition(pos.X, pos.Y);
                    Console.Write('O');
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(foodPos.X, foodPos.Y);
                Console.Write('*');
                Console.ForegroundColor = ConsoleColor.Gray;
                
                Console.SetCursorPosition(snakePos[snakePos.Count - 1].X, snakePos[snakePos.Count - 1].Y);
                Console.Write(' ');
            }
            else
            {
                Console.Clear();
                char[,] drawChars = new char[screenBounds.X, screenBounds.Y];

                drawChars[0, 0] = 'a';
                for (int x = 0; x < drawChars.GetLength(0); x++)
                    for (int y = 0; y < drawChars.GetLength(1); y++)
                        drawChars[x, y] = ' ';

                for (int i = snakePos.Count - 2; i > 0; i--)
                    drawChars[snakePos[i].X, snakePos[i].Y] = 'O';
                drawChars[foodPos.X, foodPos.Y] = '*';

                for (int y = 0; y < drawChars.GetLength(1); y++)
                    for (int x = 0; x < drawChars.GetLength(0); x++)
                    {
                        if (drawChars[x, y] == '*')
                            Console.ForegroundColor = ConsoleColor.Yellow;

                        for (int i = 0; i < snakePos.Count - 2; i++)
                            if (x == snakePos[i].X && y == snakePos[i].Y && false)
                            {
                                int ii = i;
                                while (ii > rainbowColor.Length - 1)
                                    ii -= rainbowColor.Length;
                                Console.ForegroundColor = rainbowColor[ii];
                            }

                        Console.Write(drawChars[x, y]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
            }
        }
    }
}