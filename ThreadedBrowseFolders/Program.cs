using JT.TechCases.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.TestCases.Threading.TestConsole
{
    struct threadinfo
    {
        public DirectoryInfo Di;
    }
    class Program
    {
        static void Main ( string [] args )
        {
            DirectoryInfo diroot = new DirectoryInfo ( "c:\\" );
            JT.TechCases.Threading.ThreadPool tp = JT.TechCases.Threading.ThreadPool.CreateNewAndUnique ( "title", 20 );
            tp.OnThreadsTerminatedHandler += ( ThreadPool source ) => { Console.WriteLine ( "OnThreadsTerminatedHandler => " + source.Title ); };
            tp.OnUnhandledThreadExceptionEventHandler += ( Thread source, TechCases.Threading.Exceptions.Base exc ) => { Console.WriteLine ( "OnUnhandledThreadExceptionEventHandler => " + exc.Message ); };
            JT.TechCases.Threading.Thread th = tp.CreateThread ( "root",
                                                                 new JT.TechCases.Threading.Thread.DoWorkEventHandler ( AfficheRepertoiresThreaded ),
                                                                 null,
                                                                 null,
                                                                 new threadinfo () { Di = diroot } 
                                                               );
            th.Start ();
            System.Threading.Thread.Sleep ( 1000 );
            tp.RequestCancellationAll ();
            tp.WaitAllFinished ();
        }
        static void AfficheRepertoiresThreaded ( JT.TechCases.Threading.Thread source )
        {
            if ( source.CheckStatus () == TechCases.Threading.Thread.Status.CancellationRequested ) return;
            threadinfo ti = ( threadinfo ) source.UserData;
            foreach ( DirectoryInfo dienfant in ti.Di.GetDirectories () )
            {
                if ( source.CheckStatus () == TechCases.Threading.Thread.Status.CancellationRequested ) return;
                Console.WriteLine ( dienfant.FullName );
                JT.TechCases.Threading.Thread th = source.OwnerPool.CreateThread ( dienfant.FullName,
                                                                                   AfficheRepertoiresThreaded,
                                                                                   null,
                                                                                   null,
                                                                                   new threadinfo () { Di = dienfant }
                                                                                 );
                th.StartLater ();
            }
        }
    }
}
