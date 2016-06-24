using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Media3D;
using System.Collections;

namespace DecoderExercise
{
    public class STLDecoder
    {
        protected const int INDEX_VERTEX0 = 0;
        protected const int INDEX_VERTEX1 = 1;
        protected const int INDEX_VERTEX2 = 2;
        protected const int INDEX_NORMAL = 3;
        
        public string fileType = "";

        protected byte[] _data;
        protected STLParser _parser;
        protected Point3DCollection _mesh;
        protected Vector3DCollection _normals;

        public STLDecoder() { }

        public bool read(string sFile)
        {
            _parser = null;

            _data = File.ReadAllBytes(sFile);
            if(null==_data||_data.Length==0)
                return false;

            // validate ascii file header
            _parser = new STL_ASCIIParser(_data);
            if (_parser.isValid())
            {
                fileType = STL_ASCIIParser.ASCII_FILE;
                return true;
            }

            // validate binary file header
            _parser = new STL_BinaryParser(_data);
            if (_parser.isValid())
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
                return (null==_parser)?-1:_parser.numTriangles;
            }
        }

        public ArrayList index(int value)
        {
            return (null == _parser) ? null : _parser.index(value);
        }

        public ArrayList decode()
        {
            ArrayList array = new ArrayList();
            _mesh = new Point3DCollection();
            _normals = new Vector3DCollection();
            array.Add(_mesh);
            array.Add(_normals);

            for (int i = 0; i < _parser.numTriangles; i++)
            {
                ArrayList collection = _parser.index(i);
                _mesh.Add((Point3D)collection[INDEX_VERTEX0]);
                _mesh.Add((Point3D)collection[INDEX_VERTEX1]);
                _mesh.Add((Point3D)collection[INDEX_VERTEX2]);
                _normals.Add((Vector3D)collection[INDEX_NORMAL]);
            }
            return array;
        }
    }
}
