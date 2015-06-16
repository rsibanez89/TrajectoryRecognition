using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UC
{
    public class Vector3D
    {
        private float x = 0;
        private float y = 0;
        private float z = 0;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public Vector3D()
        {
            this.set(0, 0, 0);
        }

        public Vector3D(float x, float y, float z)
        {
            this.set(x, y, z);
        }

        public Vector3D(Vector3D v)
        {
            this.set(v.x, v.y, v.z);
        }

        public Vector3D(string xyz)
        {
            xyz = xyz.Replace('(', ' ').Replace(')', ' ');
            string[] trim = xyz.Split(',');

            this.x = System.Convert.ToSingle(trim[0]);
            this.y = System.Convert.ToSingle(trim[1]);
            this.z = System.Convert.ToSingle(trim[2]);
        }

        private void set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float getModulo()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public float productoEscalar(Vector3D v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        public Vector3D clon()
        {
            return new Vector3D(x, y, z);
        }

        public bool igual(Vector3D v)
        {
            return ((x == v.x) && (y == v.y) && (z == v.z));
        }

        public float getAnguloEuler(Vector3D v)
        {
            float moduloV1 = getModulo();
            float moduloV2 = v.getModulo();
            float coseno = (productoEscalar(v) / (moduloV1 * moduloV2));
            float radianAngle = (float)Math.Acos(coseno);
            return (float)(radianAngle * 180 / Math.PI);
        }

        public float desviacionCuadrada(Vector3D v)
        {
            Vector3D desviacion = this - v;
            return desviacion.productoEscalar(desviacion);
        }

        public static float getDistancia(Vector3D v1, Vector3D v2)
        {
            float determinante = (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z);
            return (float)Math.Sqrt(determinante);
        }

        #region Operadores
        public void mas(Vector3D v)
        {
            this.x += v.X;
            this.y += v.Y;
            this.z += v.Z;
        }

        public void menos(Vector3D v)
        {
            this.x -= v.X;
            this.y -= v.Y;
            this.z -= v.Z;
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3D operator /(Vector3D v1, float f)
        {
            return new Vector3D(v1.x / f, v1.y / f, v1.z / f);
        }
        #endregion
    }
}
