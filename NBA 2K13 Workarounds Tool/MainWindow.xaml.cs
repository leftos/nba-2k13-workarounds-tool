using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using LeftosCommonLibrary;

namespace NBA_2K13_Workarounds_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow mw;
        public static string DocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\NBA 2K13 Workarounds Tool";
        public static string NBA2K13SavesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\2K Sports\NBA 2K13\Saves";
        DispatcherTimer timer;
        Memory oMemory = new Memory();

        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists(DocsPath))
                Directory.CreateDirectory(DocsPath);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Elapsed;

            btnTightJerseys.Visibility = Visibility.Hidden;
            
            mw = this;
        }

        private void btnScoreboard_Click(object sender, RoutedEventArgs e)
        {
            ScoreboardWindow sw = new ScoreboardWindow();
            sw.Show();
        }

        private void btnAutosave_Click(object sender, RoutedEventArgs e)
        {
            string cur = btnAutosave.Content.ToString();
            if (cur.EndsWith("OFF"))
            {
                btnAutosave.Content = "Autosave ON";
                timer.Start();
            }
            else
            {
                btnAutosave.Content = "Autosave OFF";
                timer.Start();
            }
            /*
            var files = Directory.GetFiles(NBA2K13SavesFolder);
            if (btnAutosave.Content == "Autosave ON")
            {
                foreach (string file in files)
                {
                    File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
                }
                btnAutosave.Content = "Autosave OFF";
            }
            else
            {
                foreach (string file in files)
                {
                    File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);
                }
                btnAutosave.Content = "Autosave ON";
            }
            */
        }

        private void btnSliders_Click(object sender, RoutedEventArgs e)
        {
            bool reset = false;
            if (btnAutosave.Content == "Autosave OFF")
            {
                btnAutosave_Click(null, null);
                reset = true;
            }

            SlidersWindow sw = new SlidersWindow();
            sw.ShowDialog();

            if (reset)
            {
                btnAutosave_Click(null, null);
            }
        }

        private void btnMPPlayVision_Click(object sender, RoutedEventArgs e)
        {
            string cur = btnMPPlayVision.Content.ToString();
            if (cur.EndsWith("?") || cur.EndsWith("OFF"))
            {
                btnMPPlayVision.Content = "MyPlayer PlayVision ON";
                timer.Start();
            }
            else
            {
                btnMPPlayVision.Content = "MyPlayer PlayVision OFF";
            }
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            Process[] aProcesses = Process.GetProcessesByName("nba2k13"); //Find Tutorial-i386.exe
            if (aProcesses.Length != 0) //If the process exists
            {
                oMemory.ReadProcess = aProcesses[0]; //Sets the Process to Read/Write From/To
                oMemory.Open(); //Open Process
                try
                {
                    // Playvision
                    int PVPtr = Addr.ToDec("01ff70f0");
                    int PVdispPtr = Addr.ToDec("01ff70f4");
                    int PlayMsgPtr = Addr.ToDec("01ff70f8");
                    int OffPCPtr = Addr.ToDec("01ff70fc");
                    int DefPCPtr = Addr.ToDec("01ff7100");
                    int MCPVPtr = Addr.ToDec("01c4ef94");
                    int MCPVdispPtr = Addr.ToDec("01c4ef9c");

                    int bytesWritten;

                    bool shouldStop = true;

                    string cur = btnMPPlayVision.Content.ToString();
                    if (cur.EndsWith("ON"))
                    {
                        oMemory.Write((IntPtr) PVPtr, new byte[] {0}, out bytesWritten);
                        oMemory.Write((IntPtr) PVdispPtr, new byte[] {1}, out bytesWritten);
                        oMemory.Write((IntPtr) PlayMsgPtr, new byte[] {0}, out bytesWritten);
                        oMemory.Write((IntPtr) OffPCPtr, new byte[] {0}, out bytesWritten);
                        oMemory.Write((IntPtr) DefPCPtr, new byte[] {0}, out bytesWritten);
                        oMemory.Write((IntPtr) MCPVPtr, new byte[] {0}, out bytesWritten);
                        oMemory.Write((IntPtr) MCPVdispPtr, new byte[] {1}, out bytesWritten);
                        shouldStop = false;
                    }

                    // Tight jerseys
                    int JerseyPtr = Addr.ToDec("6D182532");

                    cur = btnTightJerseys.Content.ToString();
                    if (cur.EndsWith("ON"))
                    {
                        oMemory.Write((IntPtr) JerseyPtr, new byte[] {90, 59, 81, 80, 90, 59, 81, 80, 90, 59, 81, 80, 90}, out bytesWritten);
                        shouldStop = false;
                    }

                    // Autosave
                    int AutosavePtr = Addr.ToDec("01FF67B0");
                    
                    cur = btnAutosave.Content.ToString();
                    if (cur.EndsWith("OFF"))
                    {
                        oMemory.Write((IntPtr) AutosavePtr, new byte[] {0}, out bytesWritten);
                        shouldStop = false;
                    }
                    else if (cur.EndsWith("ON"))
                    {
                        oMemory.Write((IntPtr)AutosavePtr, new byte[] { 1 }, out bytesWritten);
                    }

                    if (shouldStop)
                        timer.Stop();
                }
                catch (Exception)
                {
                }
                finally
                {
                    oMemory.CloseHandle();
                }
            }
            else
            {
                //txbStatus.Text = "NBA 2K13 isn't running.";
            }
        }

        private void btnTightJerseys_Click(object sender, RoutedEventArgs e)
        {
            string cur = btnTightJerseys.Content.ToString();
            if (cur.EndsWith("?") || cur.EndsWith("OFF"))
            {
                btnTightJerseys.Content = "Tight Jerseys ON";
                timer.Start();
            }
            else
            {
                btnTightJerseys.Content = "Tight Jerseys OFF";
            }
        }
    }
}
