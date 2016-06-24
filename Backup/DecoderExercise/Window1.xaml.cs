using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Collections;

namespace DecoderExercise
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        protected STLDecoder stl;
        public Window1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".stl"; // Default file extension
            dlg.Filter = "Stereo lithorgraphy documents (.stl)|*.stl"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                string filename = dlg.FileName;
                textSrc.Text = filename;
                frame.Source = new Uri("Wireframe.xaml", UriKind.RelativeOrAbsolute);
                frame.LoadCompleted += onLoadFrame;
            }
        }

        protected const int INDEX_MESH = 0;
        protected const int INDEX_NORMAL = 1;

        private void onLoadFrame(object sender, EventArgs args)
        {
            Frame f = (Frame)sender;
            Wireframe wireFrame = (Wireframe)f.Content;
            stl = new STLDecoder();

            if (stl.read(textSrc.Text))
            {
                ArrayList array = stl.decode();
                
                wireFrame.updateMesh((Point3DCollection)array[INDEX_MESH], 
                                     (Vector3DCollection)array[INDEX_NORMAL]);
            }
            else
                MessageBox.Show("Read error");
        }
    }
}
