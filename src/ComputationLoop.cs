using System.Diagnostics;

namespace AsteroidsServer.Src;

public class ComputationLoop
{
    private readonly Spawner spawner;
    private readonly GameState gameState;
    private readonly Stopwatch stopwatch = new();
    // Minimum time between starting a processing tick in milliseconds
    private const int tickRate = 10;
    private int roughSpinCyclesPerMillisecond = 0;
    private double lastTimestamp = 0;
    private double lastTimestampWithActiveSocket = 0;
    private readonly double stopLoopAfterMilliseconds = 2 * 60 * 1000;
    private bool running = false;
    public bool Running
    {
        get => running;
    }
    private Func<int>? getActiveSocketCount;

    public ComputationLoop(Spawner spawner, GameState gameState)
    {
        this.spawner = spawner;
        this.gameState = gameState;
        TuneSpinCyclesPerMillisecond();
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
            // Basically we want to monopolize a thread since Thread.Sleep() uses low-resolution timing (~15ms).
            // If we just busy wait in a while(true){} loop CPU temperature shoots up, but if we use Thread.SpinWait() instead the
            // processing loop is much tighter and temperatures stay lower.
            Thread.SpinWait(spinFor);
            double elapsedTime = stopwatch.Elapsed.TotalMicroseconds - lastTimestamp;
            spinCycleCountMeasurements[index++] = elapsedTime;
        }
        // Divide by 1000 to convert from micro to milli
        roughSpinCyclesPerMillisecond = (int)(spinFor / (spinCycleCountMeasurements.Sum() / spinCycleCountMeasurements.Length / 1000));
    }

    public void Start(Func<int> getActiveSocketCount)
    {
        this.getActiveSocketCount = getActiveSocketCount;
        running = true;
        new Thread(ServerLoop).Start();
    }

    private void ServerLoop()
    {
        while (running)
        {
            lastTimestamp = stopwatch.ElapsedMilliseconds;
            SetActiveSocketTimestamp();
            if (lastTimestamp - lastTimestampWithActiveSocket > stopLoopAfterMilliseconds)
            {
                running = false;
            }

            DoWork();
            double elapsedTime = stopwatch.ElapsedMilliseconds - lastTimestamp;

            if (elapsedTime < tickRate)
            {
                int waitCycles = (int)((tickRate - elapsedTime) * roughSpinCyclesPerMillisecond);
                Thread.SpinWait(waitCycles);
            }
        }
    }

    private void SetActiveSocketTimestamp()
    {
        if (getActiveSocketCount != null && getActiveSocketCount() > 0)
        {
            lastTimestampWithActiveSocket = lastTimestamp;
        }
    }

    private void DoWork()
    {
    }
}
