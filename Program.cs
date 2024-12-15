using System;
using System.Threading;

public delegate void StopwatchEventHandler(string message);

public class Stopwatch
{
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    private int TimeElapsed = 0;
    private bool IsRunning = false;
    private Thread timerThread; 

    public void Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");

            timerThread = new Thread(() =>
            {
                while (IsRunning)
                {
                    Tick();
                    Thread.Sleep(1000);
                }
            });

            timerThread.Start();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            timerThread?.Join();
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }

    public void Reset()
    {
        if (IsRunning)
        {
            IsRunning = false; 
            timerThread?.Join();
        }
        TimeElapsed = 0;
        OnReset?.Invoke("Stopwatch Reset!");
    }

    public void Quit()
    {
        if (IsRunning)
        {
            IsRunning = false; 
            timerThread?.Join();
        }
    }

    private void Tick()
    {
        TimeElapsed++;
        Console.WriteLine($"Time Elapsed: {TimeElapsed} seconds");
    }
}






class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.OnStarted += DisplayMessage;
        stopwatch.OnStopped += DisplayMessage;
        stopwatch.OnReset += DisplayMessage;

        bool exit = false;

        Console.WriteLine("Console Stopwatch Application");
        Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit.");

        while (!exit)
        {
            string input = Console.ReadKey(true).Key.ToString().ToUpper();

            switch (input)
            {
                case "S":
                    stopwatch.Start();
                    break;
                case "T":
                    stopwatch.Stop();
                    break;
                case "R":
                    stopwatch.Reset();
                    break;
                case "Q":
                    stopwatch.Stop();
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Use S, T, R, or Q.");
                    break;
            }
        }
    }

    static void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
}
