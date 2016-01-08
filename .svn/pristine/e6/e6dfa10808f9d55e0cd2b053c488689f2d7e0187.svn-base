using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace DecoderExercise
{
    public class STLParser
    {
        protected byte[] _data;
        protected int _numTriangles = 0;

        public STLParser(byte[] data)
        {
            _data = data;
        }

        virtual public bool isValid()
        {
            return false;
        }

        virtual public int numTriangles
        {
            get
            {
                return -1;
            }
        }

        virtual public Point3DCollection index(int value)
        {
            return null;
        }
    }
}
