using System;
using System.Threading;
using System.Windows.Forms;

namespace GFZ
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // We make sure that no other instances of that application is not running.
            bool mutexIsAvailable;
            Mutex m = null;
            try
            {
                m = new Mutex(true, "Singleton");
                // We are waiting for 1 ms.
                mutexIsAvailable = m.WaitOne(1, false);
            }
            catch (AbandonedMutexException)
            {
                // Do not worry about AbandonedMutexException.
                // Mutex only "protects" the application of it's copies.
                mutexIsAvailable = true;
            }
            if (!mutexIsAvailable) return;
            try
            {
                Application.Run(new Control());
            }
            finally
            {
                m?.ReleaseMutex();
            }
        }
    }
}