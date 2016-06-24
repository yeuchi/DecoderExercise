using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        protected ArrayList listTriangles;

        public STL_ASCIIParser(byte[] data) : base(data)
        {
            parseLines();
        }

/////////////////////////////////////////////////////////////////////
// Public

        public void clear()
        {
            if (null!=listTriangles)
            {
                listTriangles.Clear();
                listTriangles = null;
            }
        }

        /// <summary>
        /// Is Valid ?
        /// - evaluate if ascii STL file is valid
        /// (1) must be at least MINIMUM_LEN long 
        /// (2) must contain the header 'solid'
        /// (3) must have at least 1 triangle successfully decoded (actually more)
        /// </summary>
        /// <returns>
        /// true if mimimum requirements are met
        /// </returns>
        override public bool isValid()
        {
            if (null != _data && _data.Length > MINIMUM_LEN)
            {
                // (1) must be at least MINIMUM_LEN long
                byte[] chunck = new byte[MINIMUM_LEN];
                Buffer.BlockCopy(_data, 0, chunck, 0, MINIMUM_LEN);
                string hdr = System.Text.Encoding.Default.GetString(chunck);

                bool ret1 = hdr.Contains(SOLID_NAME);   // (2) must contain the header 'solid'
                bool ret2 = listTriangles.Count > 0;    // (3) success decode of 1 triangle ?
                
                if(ret1 && ret2)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Inquire the number of triangles decoded
        /// </summary>
        /// <returns>
        /// Number of triangles
        /// </returns>
        override public int numTriangles
        {
            get
            {
                return listTriangles.Count;
            }
        }

        /// <summary>
        /// Request a triangle within the collection by index
        /// </summary>
        /// <param name="value">triangle index</param>
        /// <returns>
        /// An ArrayList: v0, v1, v2, n
        /// </returns>
        override public ArrayList index(int value)
        {
            if (listTriangles.Count > 0 &&    // we have list of triangles
                value < listTriangles.Count)  // a valid index
            {
                ArrayList collection = (ArrayList)listTriangles[value];
                return collection;
            }
            return null;
        }

        /// <summary>
        /// Parse each line in ascii STL file
        /// </summary>
        /// <returns>
        /// true if success
        /// </returns>
        private bool parseLines()
        {
            try
            {
                clear();
                listTriangles = new ArrayList();

                string[] lines;
                string s = System.Text.UTF8Encoding.UTF8.GetString(_data);
                lines = s.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    /*
                     * found line with facet, next 3 are vertex
                     */
                    if (lines[i].Contains("facet"))
                    {
                        // decode normal
                        Vector3D n;
                        if(parseNormal(lines[i], out n))
                        {
                            ArrayList collection = new ArrayList();
                            collection.Add(n);

                            i += 2;
                            int j = i + 3;
                            int index = 0;
                            for (; i < j; i++)
                            {
                                // decode vertex
                                if (lines[i].Contains("vertex"))
                                {
                                    Point3D v;
                                    if(parseVertex(lines[i], out v))
                                        collection.Insert(index++, v);
                                }
                            }

                            if (index != 3)
                                throw new Exception("Lines at: " + i.ToString() + "Invalid index: " + index.ToString());

                            // aggregate a triangle collection
                            listTriangles.Add(collection);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // log error here
                Trace.WriteLine("STL_ASCIIParser::parseLine() -" + ex);
                return false;
            }
        }

        /// <summary>
        /// Parse a line of normal data into vector3D
        /// </summary>
        /// <param name="lineNormal">line string of normal</param>
        /// <param name="vector3D">decoded vector data</param>
        /// <returns>
        /// - true if successful
        /// - false if failed OR 'endfacet'
        /// </returns>
        private bool parseNormal(string lineNormal, out Vector3D vector3D)
        {
            try
            {
                string pattern = @"(\s+[\S]+)";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(lineNormal);

                // should be 5 but may be 4 if no space prior facet
                int count = matches.Count;
                vector3D = new Vector3D();

                if (matches.Count >= 4)
                {
                    vector3D.Z = double.Parse(matches[count - 1].Value);
                    vector3D.Y = double.Parse(matches[count - 2].Value);
                    vector3D.X = double.Parse(matches[count - 3].Value);
                    return true;
                }
                else
                {
                    // endfacet
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("STL_ASCIIParser::parseNormal() unexpected error - " + lineNormal + ex);
            }
        }

        /// <summary>
        /// Parse a line of Vertex from ascii STL file
        /// </summary>
        /// <param name="lineVertex">one line of text</param>
        /// <param name="point3D">decoded point</param>
        /// <returns>true if successful</returns>
        private bool parseVertex(string lineVertex, out Point3D point3D)
        {
            try
            {
                string pattern = @"(\s+[\S]+)";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(lineVertex);

                // should be 4, but maybe 3 if there is no space prior vertex
                int count = matches.Count;
                point3D = new Point3D();

                if (matches.Count >= 3)
                {
                    point3D.Z = double.Parse(matches[count - 1].Value);
                    point3D.Y = double.Parse(matches[count - 2].Value);
                    point3D.X = double.Parse(matches[count - 3].Value);
                    return true;
                }
                else
                {
                    throw new Exception("STL_ASCIIParser::parseVertex() invalid regex parse - " + lineVertex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("STL_ASCIIParser::parseVertex() unexpected error - " + lineVertex);
            }
            return false;
        }
    }
}
