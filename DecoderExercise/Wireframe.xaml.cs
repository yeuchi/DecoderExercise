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
using System.Windows.Media.Animation;

namespace DecoderExercise
{
    /// <summary>
    /// Interaction logic for Wireframe.xaml
    /// </summary>
    public partial class Wireframe : Page
    {
        public Wireframe()
        {
            InitializeComponent();
        }

        public void updateMesh( Point3DCollection verticies,
                                Vector3DCollection normals)
        {
            Int32Collection triangleIndices = new Int32Collection();
            for (int i = 0; i < verticies.Count/3; i++)
            {
                triangleIndices.Add(i*3);
                triangleIndices.Add(i*3+1);
                triangleIndices.Add(i*3+2);
            }
            
            this.mesh.TriangleIndices = triangleIndices; 
            this.mesh.Positions = verticies;
            this.mesh.Normals = normals;
            this.InvalidateVisual();
        }
    }
}
