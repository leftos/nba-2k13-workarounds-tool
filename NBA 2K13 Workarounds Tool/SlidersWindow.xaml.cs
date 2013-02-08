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

using System.IO;
using System.Windows;
using LeftosCommonLibrary;
using Microsoft.Win32;

#endregion

namespace NBA_2K13_Workarounds_Tool
{
    /// <summary>
    ///     Interaction logic for SlidersWindow.xaml
    /// </summary>
    public partial class SlidersWindow : Window
    {
        public SlidersWindow()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.cse";
            ofd.Filter = "Custom Sliders Export (*.cse)|*.cse";
            ofd.InitialDirectory = MainWindow.DocsPath;
            ofd.ShowDialog();

            if (ofd.FileName == "")
                return;

            var br = new BinaryReader(new FileStream(ofd.FileName, FileMode.Open));
            byte[] buf = br.ReadBytes(504);
            br.Close();

            var bw = new BinaryWriter(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Open));
            bw.BaseStream.Position = 69923;
            bw.Write((byte) 3);
            bw.Write(buf);
            bw.Close();

            br = new BinaryReader(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Open));
            bw = new BinaryWriter(new FileStream(MainWindow.DocsPath + @"\temp", FileMode.Create));
            br.BaseStream.Position = 4;
            byte[] buf2 = br.ReadBytes(4096);
            while (buf2.Length > 0)
            {
                bw.Write(buf2);
                buf2 = br.ReadBytes(4096);
            }
            br.Close();
            bw.Close();

            string crc = Crc32.CalculateCRC(MainWindow.DocsPath + @"\temp");

            br = new BinaryReader(new FileStream(MainWindow.DocsPath + @"\temp", FileMode.Open));
            bw = new BinaryWriter(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Create));
            bw.Write(Tools.HexStringToByteArray(crc));
            byte[] buf3 = br.ReadBytes(4096);
            while (buf3.Length > 0)
            {
                bw.Write(buf3);
                buf3 = br.ReadBytes(4096);
            }
            br.Close();
            bw.Close();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.cse";
            sfd.Filter = "Custom Sliders Export (*.cse)|*.cse";
            sfd.InitialDirectory = MainWindow.DocsPath;
            sfd.ShowDialog();

            if (sfd.FileName == "")
                return;

            var br = new BinaryReader(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Open));
            br.BaseStream.Position = 69924;
            byte[] buf = br.ReadBytes(504);
            br.Close();

            var bw = new BinaryWriter(new FileStream(sfd.FileName, FileMode.Create));
            bw.Write(buf);
            bw.Close();

            MessageBox.Show("Sliders saved to " + sfd.FileName);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}