using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RT
{
    class ModeloDTW
    {
        private int cantGestos;
        private Trayectoria3D[] gestoModeloPorGesto; // Hay un único gesto modelo
        private float[] distanciaMaximaPorGesto;
        private static readonly float INFINITO = 10000000;

        public ModeloDTW(int cantGestos)
        {
            this.cantGestos = cantGestos;
            gestoModeloPorGesto = new Trayectoria3D[cantGestos];
            distanciaMaximaPorGesto = new float[cantGestos];
        }

        public void entrenar(int nGesto, Entrenamiento3D e)
        {
            float distanciaMayor = -INFINITO;
            float[][] distancias = new float[e.Count][];
            for (int i = 0; i < e.Count; i++)
            {
                distancias[i] = new float[e.Count];
                for (int j = i + 1; j < e.Count; j++)
                {
                    distancias[i][j] = distancia(e[i], e[j]);
                    if (distancias[i][j] > distanciaMayor)
                        distanciaMayor = distancias[i][j];
                }
            }
            distanciaMaximaPorGesto[nGesto] = distanciaMayor;
            int indice = indiceDelGestoModelo(distancias);
            gestoModeloPorGesto[nGesto] = e[indice];
        }

        // El gesto modelo es el gesto que tiene la menor distancia a todos los demas
        private static int indiceDelGestoModelo(float[][] distancias)
        {
            float[] distanciaTotal = new float[distancias.Length];
            for (int i = 0; i < distancias.Length; i++)
                distanciaTotal[i] = 0;

            for (int i = 0; i < distancias.Length; i++)
                for (int j = 0; j < distancias.Length; j++)
                {
                    float valor = 0;
                    if (i < j)
                        valor = distancias[i][j];
                    if (i > j)
                        valor = distancias[j][i];
                    distanciaTotal[i] += valor;
                }

            return indiceMenor(distanciaTotal);
        }

        private static int indiceMenor(float[] distancias)
        {
            int indiceDistanciaMenor = 0;
            for (int i = 1; i < distancias.Length; i++)
                if (distancias[indiceDistanciaMenor] > distancias[i])
                    indiceDistanciaMenor = i;
            return indiceDistanciaMenor;
        }

        private static float distancia(Trayectoria3D x, Trayectoria3D y)
        {
            float[,] D = new float[x.Count, y.Count];

            //inicializo la primera fila y primera columna de la matriz en infinito
            for (int i = 0; i < x.Count; i++)
                D[i, 0] = INFINITO;
            for (int i = 0; i < y.Count; i++)
                D[0, i] = INFINITO;
            D[0, 0] = 0;

            for (int i = 1; i < x.Count; i++)
                for (int j = 1; j < y.Count; j++)
                {
                    D[i, j] = Vector3D.getDistancia(x[i], y[j])
                            + minimo(D[i - 1, j]
                                    , D[i - 1, j - 1]
                                    , D[i, j - 1]);
                }
            return D[x.Count - 1, y.Count - 1];
        }

        private static float minimo(float a, float b, float c)
        {
            return Math.Min(Math.Min(a, b), c);
        }

        public int evaluar(Trayectoria3D aEvaluar, out float costo)
        {
            float[] distancias = new float[cantGestos];
            for (int gesto = 0; gesto < cantGestos; gesto++)
                distancias[gesto] = distancia(gestoModeloPorGesto[gesto], aEvaluar);

            int indiceCandidato = indiceMenor(distancias);
            costo = distancias[indiceCandidato];
            if (distancias[indiceCandidato] <= distanciaMaximaPorGesto[indiceCandidato])
                return indiceCandidato;

            return -1;
        }

        public float[] getDistanciaMaximaPorGesto()
        {
            return distanciaMaximaPorGesto;
        }


    }
}
