using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UC
{
    public class Trayectoria3D
    {
        private List<Vector3D> trayectoria;

        #region Metodos base de la lista
        public Trayectoria3D()
        {
            this.trayectoria = new List<Vector3D>();
        }

        public Trayectoria3D(List<Vector3D> trayectoria)
        {
            this.trayectoria = trayectoria;
        }

        public int Count
        {
            get { return trayectoria.Count; }
        }

        public void Add(Vector3D value)
        {
            trayectoria.Add(value);
        }

        public void Add(Trayectoria3D trayectoria)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                this.trayectoria.Add(trayectoria[i]);
        }

        public void Clear()
        {
            trayectoria.Clear();
        }

        public void Insert(int index, Vector3D value)
        {
            trayectoria.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            trayectoria.RemoveAt(index);
        }

        public Vector3D this[int index]
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

        public Trayectoria3D GetRange(int index, int count)
        {
            return new Trayectoria3D(trayectoria.GetRange(index, count));
        }
        #endregion

        #region Metodos de una Trayectoria3D
        public Trayectoria3D clon()
        {
            List<Vector3D> copia = new List<Vector3D>();
            for (int i = 0; i < trayectoria.Count; i++)
                copia.Add(trayectoria[i].clon());
            return new Trayectoria3D(copia);
        }

        //agrega n cantidad de puntos a la Trayectoria3D en posiciones aleatorias
        public void agregarPuntos(int n)
        {
            System.Random r = new System.Random();
            for (int punto = 0; punto < n; punto++)
            {
                int posicion = r.Next(trayectoria.Count - 1);
                agregarPuntoPromedio(posicion, posicion + 1);
            }
        }

        private void agregarPuntoPromedio(int anterior, int siguiente)
        {
            Vector3D va = trayectoria[anterior];
            Vector3D vs = trayectoria[siguiente];
            Vector3D desplazamiento = (vs - va) / 2;
            desplazamiento = va + desplazamiento;
            trayectoria.Insert(siguiente, desplazamiento);
        }

        public void trasladar(Vector3D posicion)
        {
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = trayectoria[i] - posicion;
        }

        // Traslada toda la Trayectoria3D restandole la media
        public Vector3D centrar()
        {
            Vector3D media = getMedia();
            trasladar(media);
            return media;
        }

        // Devuelve la media de una Trayectoria3D
        public Vector3D getMedia()
        {
            Vector3D media = trayectoria[0].clon();
            for (int i = 1; i < trayectoria.Count; i++)
                media.mas(trayectoria[i]);
            media = media / trayectoria.Count;
            return media;
        }

        // Escala toda la Trayectoria3D, restandole la media y dividiendolo por la escala
        public float normalizar()
        {
            Vector3D media = getMedia();
            float s = getEscala();
            for (int i = 0; i < trayectoria.Count; i++)
                trayectoria[i] = (trayectoria[i] - media) / s;
            return s;
        }

        // Calcula la escala como la raiz cuadrada, de la suma de las desviaciones cuadradas sobre la cantidad de elementos
        public float getEscala()
        {
            Vector3D media = getMedia();
            float sumaDeCuadrados = 0;
            for (int i = 0; i < trayectoria.Count; i++)
                sumaDeCuadrados += trayectoria[i].desviacionCuadrada(media);
            return (float)Math.Sqrt(sumaDeCuadrados / trayectoria.Count);
        }

        public double[][] getArray()
        {
            double[][] ret = new double[trayectoria.Count][];
            for (int i = 0; i < trayectoria.Count; i++)
                ret[i] = new double[] { trayectoria[i].X, trayectoria[i].Y, trayectoria[i].Z };
            return ret;
        }

        // Es la distancia del recorrido de toda la trayectoria
        public float distanciaDelCamino()
        {
            float distancia = 0;
            for (int i = 1; i < trayectoria.Count; i++)
                distancia += Vector3D.getDistancia(trayectoria[i - 1], trayectoria[i]);
            return distancia;
        }

        // Remuestrea devuelve una nueva trayectoria remuestreada de N puntos
        public Trayectoria3D remuestrear(int N)
        {
            float incremento = distanciaDelCamino() / N;
            float D = 0;
            Trayectoria3D trayectoriaRemuestreada = new Trayectoria3D();
            trayectoriaRemuestreada.Add(trayectoria[0]);
            for (int i = 1; i < trayectoria.Count; i++)
            {
                float distancia = Vector3D.getDistancia(trayectoria[i - 1], trayectoria[i]);
                if ((D + distancia) > incremento)
                {
                    Vector3D nuevo = new Vector3D();
                    nuevo.X = trayectoria[i - 1].X + ((incremento - D) / distancia) * (trayectoria[i].X - trayectoria[i - 1].X);
                    nuevo.Y = trayectoria[i - 1].Y + ((incremento - D) / distancia) * (trayectoria[i].Y - trayectoria[i - 1].Y);
                    nuevo.Z = trayectoria[i - 1].Z + ((incremento - D) / distancia) * (trayectoria[i].Z - trayectoria[i - 1].Z);
                    trayectoriaRemuestreada.Add(nuevo);
                    trayectoria.Insert(i, nuevo);
                    D = 0;
                }
                else
                    D += distancia;
            }
            return trayectoriaRemuestreada;
        }


        // Divide la trayectoria en N grupos de tamaños iguales, si sobran puntos son agregados al ultimo grupo
        public List<Trayectoria3D> Split(int N)
        {
            List<Trayectoria3D> grupos = new List<Trayectoria3D>();
            int tamanioGrupo = this.Count / N;
            int index = 0;
            while (index < this.Count)
            {
                Trayectoria3D nuevo;
                if (index + tamanioGrupo > this.Count)
                {
                    tamanioGrupo = this.Count - index;
                    nuevo = this.GetRange(index, tamanioGrupo);
                    grupos[grupos.Count - 1].Add(nuevo);
                }
                else
                {
                    nuevo = this.GetRange(index, tamanioGrupo);
                    grupos.Add(nuevo);
                }
                
                index += tamanioGrupo;
            }
            return grupos;
        }
        #endregion

        #region LecturaEscritura
        private static readonly char SEPARADOR = ' ';
        public void salvar(string path)
        {
            StreamWriter mySW = new StreamWriter(path);
            for (int i = 0; i < this.Count; i++)
            {
                string linea = this[i].X.ToString() + SEPARADOR + this[i].Y.ToString() + SEPARADOR + this[i].Z.ToString();
                mySW.WriteLine(linea);
            }
            mySW.Close();
        }

        public static Trayectoria3D getTrayectoria(string path)
        {
            Trayectoria3D t = new Trayectoria3D();
            StreamReader mySR = new StreamReader(path);
            string line;
            char[] delimiterChars = { SEPARADOR };
            while ((line = mySR.ReadLine()) != null)
            {
                string[] words = line.Split(delimiterChars);
                Vector3D v = new Vector3D(System.Convert.ToSingle(words[0]), System.Convert.ToSingle(words[1]), System.Convert.ToSingle(words[2]));
                t.Add(v);
            }
            mySR.Close();
            return t;
        }
        #endregion
    }
}
