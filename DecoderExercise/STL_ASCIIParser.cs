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
        public const string SOLID_NAME = "solid ";         // require 1st string
        protected const string FACET_NORMAL = "facet normal";
        protected const string OUTER_LOOP = "outer loop";
        protected const string VERTEXT = "vertext";
        protected const string END_LOOP = "endloop";
        protected const string END_FACET = "endfacet";
        protected const string END_SOLID = "endsolid";
        protected ArrayList list;
        protected int count;

        public STL_ASCIIParser(byte[] data) : base(data)
        {
            parseLines();
        }

        private Vector3D parseNormal(string lineNormal)
        {
        }

        private Point3D parseVertex(string lineVertex)
        {
        }

        private bool parseLines()
        {
            list = new ArrayList();

            string[] lines;
            string s = System.Text.UTF8Encoding.UTF8.GetString(_data);
            lines = s.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains("facet")) {
                    count ++;
                    ArrayList collection = new ArrayList();
                    Vector3D n = parseNormal(lines[i]);
                    collection.Add(n);
                    
                    i+=2;
                    int j = i+3;
                    for (;  i < j; i++)
                    {
                        if (lines[i].Contains("vertex"))
                        {
                            Point3D v = parseVertex(lines[i]);
                            collection.Add(v);
                        }
                    }
                    list.Add(collection);
                }
            }
            return true;
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
                int count = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    string sPattern = "facet";
                    if (System.Text.RegularExpressions.Regex.IsMatch(lines[i], sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        count++;
                    }     
                }

                return count;
            }
        }

        override public ArrayList index(int value)
        {

            return null;
        }
    }
}
