#region Copyright Notice

//    Copyright 2011-2013 Eleftherios Aslanoglou
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

#region Using Directives

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace NBA_2K13_Workarounds_Tool
{
    /// <summary>
    ///     Interaction logic for ScoreboardWindow.xaml
    /// </summary>
    public partial class ScoreboardWindow : Window
    {
        private Memory oMemory = new Memory();

        private ScoreboardWindow sw;

        public ScoreboardWindow()
        {
            InitializeComponent();

            sw = this;

            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Elapsed;

            timer.Start();
        }

        private void timer_Elapsed(object sender, EventArgs eventArgs)
        {
            Process[] aProcesses = Process.GetProcessesByName("nba2k13"); //Find Tutorial-i386.exe
            if (aProcesses.Length != 0) //If the process exists
            {
                oMemory.ReadProcess = aProcesses[0]; //Sets the Process to Read/Write From/To
                oMemory.Open(); //Open Process
                try
                {
                    txbStatus.Text = "NBA 2K13 is running.";

                    int homeScorePointer = Addr.ToDec("01BC7A10"); //The static address of the pointer (#1)
                    int awayScorePointer = Addr.ToDec("01BC7E7C");

                    int bytesRead;
                    ushort homeScore = Convert.ToUInt16(oMemory.Read((IntPtr) homeScorePointer, 1, out bytesRead)[0]);
                    ushort awayScore = Convert.ToUInt16(oMemory.Read((IntPtr) awayScorePointer, 1, out bytesRead)[0]);
                    txbAway.Text = awayScore.ToString();
                    txbHome.Text = homeScore.ToString();
                }
                catch (Exception e)
                {
                }
                finally
                {
                    oMemory.CloseHandle();
                }
            }
            else
            {
                txbStatus.Text = "NBA 2K13 isn't running.";
            }
        }

        private void chkOnTop_Checked(object sender, RoutedEventArgs e)
        {
            sw.Topmost = true;
        }

        private void chkOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            sw.Topmost = false;
        }
    }
}