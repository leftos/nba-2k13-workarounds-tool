using System.Collections.Generic;
using System.IO;
using System.Windows;
using LeftosCommonLibrary;
using Microsoft.Win32;

namespace NBA_2K13_Workarounds_Tool
{
    /// <summary>
    /// Interaction logic for SlidersWindow.xaml
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

            BinaryReader br = new BinaryReader(new FileStream(ofd.FileName, FileMode.Open));
            byte[] buf = br.ReadBytes(504);
            br.Close();

            BinaryWriter bw = new BinaryWriter(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Open));
            bw.BaseStream.Position = 69923;
            bw.Write((byte)3);
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.cse";
            sfd.Filter = "Custom Sliders Export (*.cse)|*.cse";
            sfd.InitialDirectory = MainWindow.DocsPath;
            sfd.ShowDialog();

            if (sfd.FileName == "")
                return;

            BinaryReader br = new BinaryReader(new FileStream(MainWindow.NBA2K13SavesFolder + @"\Settings.STG", FileMode.Open));
            br.BaseStream.Position = 69924;
            byte[] buf = br.ReadBytes(504);
            br.Close();

            BinaryWriter bw = new BinaryWriter(new FileStream(sfd.FileName, FileMode.Create));
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
