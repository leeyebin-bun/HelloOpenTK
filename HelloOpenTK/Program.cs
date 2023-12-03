using System;
using OpenTK;
using OpenTK.Windowing.Desktop;
using HelloOpenTK;

public class Program
{
    static void Main(String[] args)
    {
        Console.WriteLine("Hello ! OpenTk!");

        using (MyGameWindow game= new MyGameWindow(800, 600, "LearnOpenTK"))
        {
            game.Run();
        }
    }

}