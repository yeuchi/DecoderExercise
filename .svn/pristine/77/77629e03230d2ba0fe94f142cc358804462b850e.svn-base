using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Media3D;

namespace DecoderExercise
{
    public class STLDecoder
    {
        byte[] data;
        public string fileType = "";
        STLParser parser;

        public STLDecoder() { }

        public bool read(string sFile)
        {
            parser = null;

            data = File.ReadAllBytes(sFile);
            if(null==data||data.Length==0)
                return false;

            // validate ascii file header
            parser = new STL_ASCIIParser(data);
            if (parser.isValid())
            {
                fileType = STL_ASCIIParser.ASCII_FILE;
                return true;
            }

            // validate binary file header
            parser = new STL_BinaryParser(data);
            if (parser.isValid())
            {
                fileType = STL_BinaryParser.BINARY_FILE;
                return true;
            }
            return false;
        }

        public int numTriangles
        {
            get
            {
                return (null==parser)?-1:parser.numTriangles;
            }
        }

        public Point3DCollection index(int value)
        {
            return (null == parser) ? null : parser.index(value);
        }

        public Point3DCollection getAllVerticies()
        {
            Point3DCollection mesh = new Point3DCollection();
            for (int i = 0; i < parser.numTriangles; i++)
            {
                Point3DCollection p3DCollection = parser.index(i);
                mesh.Add(p3DCollection[0]);
                mesh.Add(p3DCollection[1]);
                mesh.Add(p3DCollection[2]);
            }
            return mesh;
        }
    }
}
