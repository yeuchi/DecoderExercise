using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Collections;

namespace DecoderExercise
{
    public class STL_ASCIIParser : STLParser
    {
        // ascii
        public const string ASCII_FILE = "ascii file";
        public const int MINIMUM_LEN = 20;                 // just guessing
        public const string SOLID_NAME = "solid name";     // 

        public STL_ASCIIParser(byte[] data) : base(data)
        {
        }

        override public bool isValid()
        {
            if (null!=_data && _data.Length > MINIMUM_LEN)
            {
                byte[] chunck = new byte[MINIMUM_LEN];
                Buffer.BlockCopy(_data, 0, chunck, 0, MINIMUM_LEN);
                string hdr = chunck.ToString();
                if (hdr.Contains(SOLID_NAME))
                    return true;
            }
            return false;
        }

        override public int numTriangles
        {
            get
            {
                return -1;
            }
        }

        override public ArrayList index(int value)
        {
            return null;
        }
    }
}
