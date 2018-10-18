using JT.TechCases.Threading;
using System;
using System.Diagnostics;

namespace ThrededPrimeNumbers
{
    class Program
    {
        static void Main ( string [] args )
        {
            Stopwatch sw = Stopwatch.StartNew ();
            long valmax = 1000;
            ThreadPool tp = RangeSplitter.LaunchSplitedTasks ( 1, valmax, 2, ( SubRange sr, long BoundIndex ) =>
            {
                bool isprime = true;
                for ( int a = 2; a <= BoundIndex / 2; a++ )
                {
                    if ( BoundIndex % a == 0 )
                    {
                        isprime = false;
                        break;
                    }
                }
                Console.WriteLine ( BoundIndex + " is prime = " + isprime );
            } );
            tp.WaitAllFinished ();
            sw.Stop ();
            Console.WriteLine ( sw.Elapsed.TotalMilliseconds.ToString ( "0.0###" ) );
        }
    }
}
