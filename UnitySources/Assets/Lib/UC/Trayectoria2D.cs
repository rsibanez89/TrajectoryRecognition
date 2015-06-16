using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UC
{
    public class Trayectoria2D
    {
        private List<Vector2D> trayectoria;

        #region Metodos base de la lista
        public Trayectoria2D()
        {
            this.trayectoria = new List<Vector2D>();
        }

        public Trayectoria2D(List<Vector2D> trayectoria)
        {
            this.trayectoria = trayectoria;
        }

        public int Count
        {
            get { return trayectoria.Count; }
        }

        public void Add(Vector2D value)
        {
            trayectoria.Add(value);
        }

        public void Add(Trayectoria2D trayectoria)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                this.trayectoria.Add(trayectoria[i]);
        }

        public void Clear()
        {
            trayectoria.Clear();
        }

        public void Insert(int index, Vector2D value)
        {
            trayectoria.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            trayectoria.RemoveAt(index);
        }

        public Vector2D this[int index]
        {
            get
            {
                return trayectoria[index];
            }
            set
            {
                trayectoria[index] = value;
            }
        }

        public Trayectoria2D GetRange(int index, int count)
        {
            return new Trayectoria2D(trayectoria.GetRange(index, count));
        }
        #endregion

        #region Metodos de una trayectoria
        public Trayectoria2D clon()
        {
            List<Vector2D> copia = new List<Vector2D>();
            for (int i = 0; i < trayectoria.Count; i++)
                copia.Add(trayectoria[i].clon());
            return new Trayectoria2D(copia);
        }

        public void trasladar(Vector2D posicion)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = trayectoria[i] - posicion;
        }

        // Traslada toda la Trayectoria restandole la media
        public Vector2D centrar()
        {
            Vector2D media = getMedia();
            trasladar(media);
            return media;
        }

        // Devuelve la media de una Trayectoria
        public Vector2D getMedia()
        {
            Vector2D media = trayectoria[0].clon();
            for (int i = 1; i < trayectoria.Count; i++)
                media.mas(trayectoria[i]);
            media = media / trayectoria.Count;
            return media;
        }

        // Escala toda la Trayectoria, restandole la media y dividiendolo por la escala
        public float normalizar()
        {
            Vector2D media = getMedia();
            float s = getEscala();
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = (trayectoria[i] - media) / s;
            return s;
        }

        // Calcula la escala como la raiz cuadrada, de la suma de las desviaciones cuadradas sobre la cantidad de elementos
        public float getEscala()
        {
            Vector2D media = getMedia();
            float sumaDeCuadrados = 0;
            for (int i = 0; i < trayectoria.Count; i++)
                sumaDeCuadrados += trayectoria[i].desviacionCuadrada(media);
            return (float)Math.Sqrt(sumaDeCuadrados / trayectoria.Count);
        }

        public double[][] getArray()
        {
            double[][] ret = new double[trayectoria.Count][];
            for (int i = 0; i < trayectoria.Count; i++)
                ret[i] = new double[] { trayectoria[i].X, trayectoria[i].Y };
            return ret;
        }

        // Rota la trayectoria alrededor del centroide
        public Trayectoria2D rotar(float angulo, Vector2D centroide)
        {
            Trayectoria2D rotada = new Trayectoria2D();
            for (int i = 0; i < this.Count; i++)
            {
                Vector2D nuevo = new Vector2D();
                nuevo.X = (float)((this[i].X - centroide.X) * Math.Cos(angulo) - (this[i].Y - centroide.Y) * Math.Sin(angulo) + centroide.X);
                nuevo.Y = (float)((this[i].X - centroide.X) * Math.Sin(angulo) + (this[i].Y - centroide.Y) * Math.Cos(angulo) + centroide.Y);
                rotada.Add(nuevo);
            }
            return rotada;
        }

        // Rota la trayectoria alrededor de la media
        public Trayectoria2D rotar(float angulo)
        {
            Vector2D centroide = getMedia();
            Trayectoria2D rotada = new Trayectoria2D();
            for (int i = 0; i < this.Count; i++)
            {
                Vector2D nuevo = new Vector2D();
                nuevo.X = (float)((this[i].X - centroide.X) * Math.Cos(angulo) - (this[i].Y - centroide.Y) * Math.Sin(angulo) + centroide.X);
                nuevo.Y = (float)((this[i].X - centroide.X) * Math.Sin(angulo) + (this[i].Y - centroide.Y) * Math.Cos(angulo) + centroide.Y);
                rotada.Add(nuevo);
            }
            return rotada;
        }

        // Es la distancia del recorrido de toda la trayectoria
        public float distanciaDelCamino()
        {
            float distancia = 0;
            for (int i = 1; i < trayectoria.Count; i++)
                distancia += Vector2D.getDistancia(trayectoria[i - 1], trayectoria[i]);
            return distancia;
        }

        // Remuestrea devuelve una nueva trayectoria remuestreada de N puntos
        public Trayectoria2D remuestrear(int N)
        {
            float incremento = distanciaDelCamino() / N;
            float D = 0;
            Trayectoria2D trayectoriaRemuestreada = new Trayectoria2D();
            trayectoriaRemuestreada.Add(trayectoria[0]);
            for (int i = 1; i < trayectoria.Count; i++)
            {
                float distancia = Vector2D.getDistancia(trayectoria[i - 1], trayectoria[i]);
                if ((D + distancia) > incremento)
                {
                    Vector2D nuevo = new Vector2D();
                    nuevo.X = trayectoria[i - 1].X + ((incremento - D) / distancia) * (trayectoria[i].X - trayectoria[i - 1].X);
                    nuevo.Y = trayectoria[i - 1].Y + ((incremento - D) / distancia) * (trayectoria[i].Y - trayectoria[i - 1].Y);
                    trayectoriaRemuestreada.Add(nuevo);
                    trayectoria.Insert(i, nuevo);
                    D = 0;
                }
                else
                    D += distancia;
            }
            return trayectoriaRemuestreada;
        }
        #endregion
    }
}
