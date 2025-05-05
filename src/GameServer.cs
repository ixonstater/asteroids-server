using System.Diagnostics;

namespace AsteroidsServer.Src
{
    public class GameServer
    {
        private readonly Spawner spawner;
        private readonly GameState gameState;
        private readonly Stopwatch stopwatch;
        // Minimum time between starting a processing tick in milliseconds
        private const int tickRate = 10;
        private int roughSpinCyclesPerMillisecond = 0;
        private double lastTimestamp = 0;
        private bool running = false;

        public GameServer(Spawner spawner, GameState gameState)
        {
            this.spawner = spawner;
            this.gameState = gameState;
            stopwatch = new Stopwatch();
        }

        public void Start()
        {
            running = true;
            TuneSpinCyclesPerMillisecond();
            Thread thread = new(ServerLoop);
            thread.Start();
        }

        // Figure out roughly how many spin cycles need to happen for a millisecond to elapse
        private void TuneSpinCyclesPerMillisecond()
        {
            double[] spinCycleCountMeasurements = new double[200];
            int spinFor = 100000;
            int index = 0;

            stopwatch.Start();

            while (index < spinCycleCountMeasurements.Length)
            {
                lastTimestamp = stopwatch.Elapsed.TotalMicroseconds;
                // Basically we want to monopolize a thread as much as possible since Thread.Sleep() uses low-resolution timing.
                // If we just let a while(true){} loop CPU usage shoots up, but if we use Thread.SpinWait() instead usage is more
                // manageable.
                Thread.SpinWait(spinFor);
                double elapsedTime = stopwatch.Elapsed.TotalMicroseconds - lastTimestamp;
                spinCycleCountMeasurements[index++] = elapsedTime;
            }
            // Divide by 1000 to convert from micro to milli
            roughSpinCyclesPerMillisecond = (int)(spinFor / (spinCycleCountMeasurements.Sum() / spinCycleCountMeasurements.Length / 1000));
        }

        private void ServerLoop()
        {
            while (running)
            {
                lastTimestamp = stopwatch.ElapsedMilliseconds;
                DoWork();
                double elapsedTime = stopwatch.ElapsedMilliseconds - lastTimestamp;

                if (elapsedTime < tickRate)
                {
                    int waitCycles = (int)((tickRate - elapsedTime) * roughSpinCyclesPerMillisecond);
                    Thread.SpinWait(waitCycles);
                }
            }
        }

        private void DoWork()
        {
        }
    }
}