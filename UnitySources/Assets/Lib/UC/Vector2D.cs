using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UC
{
    public class Vector2D
    {
        private float x = 0;
        private float y = 0;

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

        public Vector2D()
        {
            this.set(0, 0);
        }

        public Vector2D(float x, float y)
        {
            this.set(x, y);
        }

        public Vector2D(Vector2D v)
        {
            this.set(v.x, v.y);
        }

        public Vector2D(string xy)
        {
            xy = xy.Replace('(', ' ').Replace(')', ' ');
            string[] trim = xy.Split(',');

            this.x = System.Convert.ToInt32(trim[0]);
            this.y = System.Convert.ToInt32(trim[1]);
        }

        public void set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static float getDistancia(Vector2D v1, Vector2D v2)
        {
            float determinante = (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y);
            return (float)Math.Sqrt(determinante);
        }

        public float productoEscalar(Vector2D v)
        {
            return x * v.x + y * v.y;
        }

        public float desviacionCuadrada(Vector2D v)
        {
            Vector2D desviacion = this - v;
            return desviacion.productoEscalar(desviacion);
        }

        public Vector2D clon()
        {
            return new Vector2D(this.x, this.y);
        }

        public float getAnguloEuler()
        {
            float angulo = radianToDegree(Math.Atan(y / x));
            if ((x < 0) && (y > 0))
                return angulo + 180;
            if ((x < 0) && (y < 0))
                return angulo + 180;
            if ((x > 0) && (y < 0))
                return angulo + 360;

            return angulo;
        }

        private float radianToDegree(double angle)
        {
            return (float)(angle * (180f / Math.PI));
        }

        #region Operadores
        public void mas(Vector2D v)
        {
            this.x += v.X;
            this.y += v.Y;
        }

        public void menos(Vector2D v)
        {
            this.x -= v.X;
            this.y -= v.Y;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2D operator /(Vector2D v1, float f)
        {
            return new Vector2D(v1.x / f, v1.y / f);
        }
        #endregion
    }
}
