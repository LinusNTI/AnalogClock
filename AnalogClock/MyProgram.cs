using System;
using System.Drawing;
using System.Threading;

namespace AnalogClock
{
    class MyProgram
    {
        private bool isRunning = true;

        private float hourPointerScale = 0.5f;
        private float minutePointerScale = 0.7f;
        private float secondPointerScale = 0.95f;

        private int fps = 5;

        private DateTime oldTime = DateTime.Now;
        private Size oldSize = new Size(Console.WindowWidth, Console.WindowHeight);

        bool hasRenderedSphere = false;

        Size GetWindowSize()
        {
            return new Size(Console.WindowWidth, Console.WindowHeight);
        }

        public PointF GetCirclePosition(float radius, float degrees) => new PointF((float)(radius * Math.Sin(Math.PI * 2 * degrees / 360)), (float)(radius * Math.Cos(Math.PI * 2 * degrees / 360)));

        public void Run()
        {
            while (isRunning)
            {
                // Clear screen, also assuming that it starts at 0,0 since while loop should end with setting cursor pos to 0
                // Why not use Console.Clear()? because it's dogshit and causes flickering :anger:
                if (GetWindowSize() != oldSize)
                {
                    for (int y = 0; y < Console.WindowHeight; y++)
                    {
                        Console.WriteLine(new string(' ', Console.WindowWidth));
                    }
                    Console.SetCursorPosition(0, 0);

                    hasRenderedSphere = false;
                }

                int maxSize = (Console.WindowHeight < Console.WindowWidth / 2 ? Console.WindowHeight : Console.WindowWidth / 2) / 2;
                DateTime timeNow = DateTime.Now;

                #region Draw Sphere
                if (!hasRenderedSphere)
                {
                    int perimiter = (int)Math.Round(Math.PI * maxSize * 2);
                    float degreesPerUnit = (float)360 / (float)perimiter;
                    for (int i = 0; i < perimiter; i++)
                    {
                        PointF positionOnCircle = GetCirclePosition(maxSize, degreesPerUnit * i);

                        Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                        Console.Write("#");
                    }
                    hasRenderedSphere = true;
                }
                #endregion

                #region Remove old pointers
                int secondLength2 = (int)Math.Round(maxSize * secondPointerScale);
                for (int i = 0; i < secondLength2; i++)
                {
                    double secondsToDegrees = 180 - ((360 / 60) * (oldTime.Second + (float)oldTime.Millisecond / 1000f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)secondsToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write(" ");
                }

                int minuteLength2 = (int)Math.Round(maxSize * minutePointerScale);
                for (int i = 0; i < minuteLength2; i++)
                {
                    double minutesToDegrees = 180 - ((360 / 60) * (oldTime.Minute + (float)oldTime.Second / 60f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)minutesToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write(" ");
                }

                int hourLength2 = (int)Math.Round(maxSize * hourPointerScale);
                for (int i = 0; i < hourLength2; i++)
                {
                    double hoursToDegrees = 180 - ((360 / 12) * (oldTime.Hour + (float)oldTime.Minute / 60f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)hoursToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write(" ");
                }
                #endregion

                #region Draw Pointers
                int secondLength = (int)Math.Round(maxSize * secondPointerScale);
                for (int i = 0; i < secondLength; i++)
                {
                    double secondsToDegrees = 180 - ((360 / 60) * (timeNow.Second + (float)timeNow.Millisecond / 1000f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)secondsToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write(".");
                }

                int minuteLength = (int)Math.Round(maxSize * minutePointerScale);
                for (int i = 0; i < minuteLength; i++)
                {
                    double minutesToDegrees = 180 - ((360 / 60) * (timeNow.Minute + (float)timeNow.Second / 60f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)minutesToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write("*");
                }

                int hourLength = (int)Math.Round(maxSize * hourPointerScale);
                for (int i = 0; i < hourLength; i++)
                {
                    double hoursToDegrees = 180 - ((360 / 12) * (timeNow.Hour + (float)timeNow.Minute / 60f));
                    PointF positionOnCircle = GetCirclePosition(i, (float)hoursToDegrees);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(positionOnCircle.X * 2), maxSize + (int)Math.Round(positionOnCircle.Y));
                    Console.Write("@");
                }
                #endregion

                oldTime = timeNow;
                oldSize = GetWindowSize();

                Console.SetCursorPosition(maxSize * 2, maxSize);
                Console.Write("O"); // Write knob in centre
                Console.SetCursorPosition(0, 0); // Reset position so that ugly thing isn't right where I'm looking...

                Thread.Sleep(1000 / fps); // FPS conversion
            }
        }
    }
}
