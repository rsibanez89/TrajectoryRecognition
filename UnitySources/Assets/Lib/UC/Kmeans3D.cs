using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UC
{
    public class Kmeans3D
    {
        private Trayectoria3D centroides;
        private Trayectoria3D trayectoria;
        private static readonly int ITERACIONES = 10;

        public Trayectoria3D Centroides
        {
            get { return centroides; }
        }

        public Kmeans3D()
        {
            this.centroides = new Trayectoria3D();
            this.trayectoria = new Trayectoria3D();
        }

        public Kmeans3D(Trayectoria3D trayectoria)
        {
            this.centroides = new Trayectoria3D();
            this.trayectoria = trayectoria;
        }

        public void ejecutar(int k)
        {
            ejecutar(k, ITERACIONES);
        }

        /// <summary>
        /// Ejecuta K-means:
        ///     - k es la cantidad de clusters generada
        ///     - iteraciones es la cantidad de iteraciones maximas con las que se ejecuta k-means
        /// </summary>
        public void ejecutar(int k, int iteraciones)
        {
            List<Trayectoria3D> grupos = trayectoria.Split(k);

            bool hayMovimiento = true;

            for (int h = 0; h < iteraciones && hayMovimiento; h++)
            {
                hayMovimiento = false;

                centroides.Clear();

                // Calcular los centroides
                for (int g = 0; g < grupos.Count; g++)
                    centroides.Add(grupos[g].getMedia());

                // Reagrupar puntos a su centroide mas cercano
                for (int g = 0; g < grupos.Count; g++)
                {
                    Trayectoria3D grupo = grupos[g];
                    int indice = 0;
                    while (indice < grupo.Count)
                    {
                        Vector3D v = grupo[indice];
                        int indiceCMS = indiceDelCentroideMasCercano(centroides, v);
                        if (indiceCMS == g)//Esta en el grupo que debe estar
                            indice++;
                        else
                            if (grupo.Count > 1) // No está en el grupo correcto y el grupo no queda vacio si lo saco
                            {
                                grupos[indiceCMS].Add(v);
                                grupo.RemoveAt(indice);
                                hayMovimiento = true;
                            }
                            else
                                indice++;
                    }
                }
            }
        }

        public static int indiceDelCentroideMasCercano(Trayectoria3D centroides, Vector3D v)
        {
            float distancia = Vector3D.getDistancia(v, centroides[0]);
            int indice = 0;
            for (int i = 1; i < centroides.Count; i++)
            {
                float distanciaAux = Vector3D.getDistancia(v, centroides[i]);
                if (distancia > distanciaAux)
                {
                    indice = i;
                    distancia = distanciaAux;
                }
            }
            return indice;
        }

        public int[] nearest(Trayectoria3D trayectoria3D)
        {
            return nearest(centroides, trayectoria3D);
        }

        public static int[] nearest(Trayectoria3D centroides, Trayectoria3D trayectoria3D)
        {
            int[] retorno = new int[trayectoria3D.Count];
            for (int i = 0; i < trayectoria3D.Count; i++)
                retorno[i] = indiceDelCentroideMasCercano(centroides, trayectoria3D[i]);
            return retorno;
        }

    }
}
