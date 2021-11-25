using System;
using System.Threading;

namespace AnalogClock
{
    class MyProgram
    {
        private bool _isRunning = true;

        private float hourPointerScale = 0.5f;
        private float minutePointerScale = 0.7f;
        private float secondPointerScale = 0.95f;

        private int FPS = 3;

        public void Run()
        {
            while (_isRunning)
            {
                // Clear screen, also assuming that it starts at 0,0 since while loop should end with setting cursor pos to 0
                // Why not use Console.Clear()? because it's dogshit and causes flickering :anger:
                for (int y = 0; y < Console.WindowHeight; y++)
                {
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }
                Console.SetCursorPosition(0, 0);

                
                int maxSize = (Console.WindowHeight < Console.WindowWidth / 2 ? Console.WindowHeight : Console.WindowWidth / 2) / 2;
                DateTime timeNow = DateTime.Now;

                #if DEBUG // We only want to write this as debug information
                Console.WriteLine($"{timeNow.Hour}:{timeNow.Minute}:{timeNow.Second}");
                #endif

                #region Draw Sphere
                int perimiter = (int)Math.Round(Math.PI * maxSize * 2);
                float degreesPerUnit = (float)360 / (float)perimiter;
                for (int i = 0; i < perimiter; i++)
                {
                    // Calculation for X and Y source from here: https://stackoverflow.com/a/43642346
                    double x = maxSize * Math.Sin(Math.PI * 2 * (degreesPerUnit * i) / 360);
                    double y = maxSize * Math.Cos(Math.PI * 2 * (degreesPerUnit * i) / 360);

                    Console.SetCursorPosition((maxSize*2)+(int)Math.Round(x*2), maxSize+(int)Math.Round(y));
                    Console.Write("#");
                }
                #endregion

                #region Draw Pointers
                int secondLength = (int)Math.Round(maxSize * secondPointerScale);
                for (int i = 0; i < secondLength; i++)
                {
                    double secondsToDegrees = 180 - ((360 / 60) * (timeNow.Second + (float)timeNow.Millisecond / 1000f));

                    double x = i * Math.Sin(Math.PI * 2 * secondsToDegrees / 360);
                    double y = i * Math.Cos(Math.PI * 2 * secondsToDegrees / 360);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(x * 2), maxSize + (int)Math.Round(y));
                    Console.Write(".");
                }

                int minuteLength = (int)Math.Round(maxSize * minutePointerScale);
                for (int i = 0; i < minuteLength; i++)
                {
                    double minutesToDegrees = 180 - ((360 / 60) * (timeNow.Minute + (float)timeNow.Second / 60f));

                    double x = i * Math.Sin(Math.PI * 2 * minutesToDegrees / 360);
                    double y = i * Math.Cos(Math.PI * 2 * minutesToDegrees / 360);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(x * 2), maxSize + (int)Math.Round(y));
                    Console.Write("*");
                }

                int hourLength = (int)Math.Round(maxSize * hourPointerScale);
                for (int i = 0; i < hourLength; i++)
                {
                    double hoursToDegrees = 180 - ((360 / 12) * (timeNow.Hour + (float)timeNow.Minute / 60f));
                    

                    double x = i * Math.Sin(Math.PI * 2 * hoursToDegrees / 360);
                    double y = i * Math.Cos(Math.PI * 2 * hoursToDegrees / 360);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(x * 2), maxSize + (int)Math.Round(y));
                    Console.Write("@");
                }
                #endregion

                #if DEBUG // Write numbers on the clock
                for (int i = 1; i < 13; i++)
                {
                    double degreesPerNum = 360 / 12;

                    double x = maxSize * Math.Sin(Math.PI * 2 * (180 - (degreesPerNum * i)) / 360);
                    double y = maxSize * Math.Cos(Math.PI * 2 * (180 - (degreesPerNum * i)) / 360);

                    Console.SetCursorPosition((maxSize * 2) + (int)Math.Round(x * 2), maxSize + (int)Math.Round(y));
                    Console.Write(i);
                }
                #endif

                Console.SetCursorPosition(maxSize*2, maxSize);
                Console.Write("O"); // Write knob in centre
                Console.SetCursorPosition(0, 0); // Reset position so that ugly thing isn't right where I'm looking...

                Thread.Sleep(1000/FPS); // FPS conversion
            }
        }
    }
}
