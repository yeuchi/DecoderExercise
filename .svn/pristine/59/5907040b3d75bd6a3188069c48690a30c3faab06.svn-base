using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Collections;

namespace DecoderExercise
{
    public class STL_BinaryParser : STLParser
    {
        public const string BINARY_FILE = "binary file";

        public const int BIN_HEADER_LEN = 80;               // binary file header length
        public const int NUM_TRIANGLE_BYTE_COUNT = 4;       // 4 bytes counter of how many triangles in file
        public const int NUM_VERTICIES = 3;                 // 3 verticies make up 1 triangle
        public const int NUM_POINTS = 3;                    // 3 points per vertex
        public const int NORMAL_BYTE_COUNT = 4;             // 4 bytes for normal vector
        public const int VERTEX_BYTE_COUNT = 4;             // 4 bytes for each vertex
        public const int ATTRIBUTE_BYTE_COUNT = 2;          // 2 bytes for each attribute
        public const int TRIANGLE_BYTE_COUNT = 3 * NORMAL_BYTE_COUNT + 3 * 3 * VERTEX_BYTE_COUNT + ATTRIBUTE_BYTE_COUNT;

        public STL_BinaryParser(byte[] data) : base(data)
        {
        }

        override public bool isValid()
        {
            // validate file size with header info
            long totalBytes = numTriangles * TRIANGLE_BYTE_COUNT
                                + NUM_TRIANGLE_BYTE_COUNT
                                + BIN_HEADER_LEN;

            if (_data.Length == totalBytes)
                return true;

            return false;
        }

        public byte[] header
        {
            get
            {
                if (null!=_data && _data.Length > BIN_HEADER_LEN)
                {
                    byte[] hdr = new byte[BIN_HEADER_LEN];
                    Buffer.BlockCopy(_data, 0, hdr, 0, BIN_HEADER_LEN);
                    return hdr;
                }
                return null;
            }
        }

        override public int numTriangles
        {
            get
            {
                if(0<_numTriangles)
                    return _numTriangles;

                if (null!=_data && _data.Length > BIN_HEADER_LEN)
                {
                    byte[] bCount = new byte[NUM_TRIANGLE_BYTE_COUNT];
                    Buffer.BlockCopy(_data, BIN_HEADER_LEN, bCount, 0, NUM_TRIANGLE_BYTE_COUNT);
                    _numTriangles = System.BitConverter.ToInt32(bCount, 0);
                    return _numTriangles;
                }
                return -1;
            }
        }

        override public ArrayList index(int value)
        {
            if(value>numTriangles)
                return null;

            byte[] bytes = new byte[TRIANGLE_BYTE_COUNT];
            int offset = value * TRIANGLE_BYTE_COUNT
                                + NUM_TRIANGLE_BYTE_COUNT
                                + BIN_HEADER_LEN;
            Buffer.BlockCopy(_data, offset, bytes, 0, TRIANGLE_BYTE_COUNT);

            Vector3D n = parseNormal(bytes);
            Point3D v0 = parseVertex(bytes, 0);
            Point3D v1 = parseVertex(bytes, 1);
            Point3D v2 = parseVertex(bytes, 2);
            byte[] attr = parseAttribute(bytes);
            ArrayList collection = new ArrayList();

            // only make sense that vertex data comes first
            collection.Add(v0);
            collection.Add(v1);
            collection.Add(v2);

            // then normal
            collection.Add(n);

            // what about attribute ?

            return collection;
        }

        protected Vector3D parseNormal(byte[] dataChunk)
        {   // normal is packed the identical as vertex
            Point3D p = parseVertex(dataChunk, 0);
            Vector3D normal = new Vector3D();
            normal.X = p.X;
            normal.Y = p.Y;
            normal.Z = p.Z;
            return normal;
        }

        protected Point3D parseVertex(byte[] dataChunk, int index)
        {
            int offset = index*VERTEX_BYTE_COUNT*NUM_POINTS + NORMAL_BYTE_COUNT*NUM_POINTS;
            float[] array = new float[NUM_VERTICIES];
            byte[] bytes = new byte[VERTEX_BYTE_COUNT];
            for (int j = 0; j < NUM_POINTS; j++)
            {
                for( int k = 0; k < VERTEX_BYTE_COUNT; k++) {
                    bytes[k] = dataChunk[offset + 
                                         j * VERTEX_BYTE_COUNT +
                                         k];
                }
                array[j] = System.BitConverter.ToSingle(bytes, 0);
            }
            Point3D p3D = new Point3D();
            p3D.X = array[0];
            p3D.Y = array[1];
            p3D.Z = array[2];
            return p3D;
        }

        protected byte[] parseAttribute(byte[] dataChunk) {
            byte[] bytes = new byte[4];
            for(int i=0; i<4; i++)
                bytes[i] = dataChunk[dataChunk.Length-4+i];
            return bytes;
        }
    }
}
