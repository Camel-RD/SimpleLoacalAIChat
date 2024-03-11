using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleLoacalAIChat
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static string StartUpPresetName = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "SomeRandomTextdfdsjl4903fsdh894hkashgd", out createdNew))
            {
                if (createdNew)
                {
                    var args = Environment.GetCommandLineArgs();
                    if (args != null && args.Length > 1)
                        StartUpPresetName = args[1];
                    // To customize application configuration such as set high DPI settings or default font,
                    // see https://aka.ms/applicationconfiguration.
                    ApplicationConfiguration.Initialize();
                    Application.Run(new Form1());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }

        }
    }
}