using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RT
{
    class ModeloStringMatchingAproximativo
    {
        private int cantGestos;
        private List<int>[] gestoModeloPorGesto; // Hay un único gesto modelo
        private float[] distanciaMaximaPorGesto;
        private static readonly float INFINITO = 10000000;

        public ModeloStringMatchingAproximativo(int cantGestos)
        {
            this.cantGestos = cantGestos;
            gestoModeloPorGesto = new List<int>[cantGestos];
            for (int i = 0; i < cantGestos; i++)
                gestoModeloPorGesto[i] = new List<int>();
            distanciaMaximaPorGesto = new float[cantGestos];
        }

        public void entrenar(int nGesto, List<int[]> secuenciasGestoN, Trayectoria3D centroides)
        {
            float distanciaMayor = -INFINITO;
            float[][] distancias = new float[secuenciasGestoN.Count][];
            for (int s = 0; s < secuenciasGestoN.Count; s++)
            {
                List<int> secuenciaSinCaracteresRepetidos = ModeloStringMatching.eliminarCaracteresRepetidos(secuenciasGestoN[s]);
                distancias[s] = new float[secuenciasGestoN.Count];
                for (int s2 = s + 1; s2 < secuenciasGestoN.Count; s2++)
                {
                    List<int> secuenciaSinCaracteresRepetidos2 = ModeloStringMatching.eliminarCaracteresRepetidos(secuenciasGestoN[s2]);
                    distancias[s][s2] = distancia(secuenciaSinCaracteresRepetidos, secuenciaSinCaracteresRepetidos2, centroides);
                    if (distancias[s][s2] > distanciaMayor)
                        distanciaMayor = distancias[s][s2]; 
                }
            }
            distanciaMaximaPorGesto[nGesto] = distanciaMayor;
            int indice = indiceDelGestoModelo(distancias);
            int[] gestoModelo = secuenciasGestoN[indice];
            gestoModeloPorGesto[nGesto] = ModeloStringMatching.eliminarCaracteresRepetidos(gestoModelo);
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

        private float distancia(List<int> x, List<int> y, Trayectoria3D centroides)
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
                    D[i, j] = distancia(x[i], y[j], centroides)
                            + minimo(D[i - 1, j]
                                    , D[i - 1, j - 1]
                                    , D[i, j - 1]);
                }
            return D[x.Count - 1, y.Count - 1];
        }

        private float distancia(int x, int y, Trayectoria3D centroides)
        {
            return Vector3D.getDistancia(centroides[x], centroides[y]);
        }

        private static float minimo(float a, float b, float c)
        {
            return Math.Min(Math.Min(a, b), c);
        }

        public bool evaluar(int[] secuencia, Trayectoria3D centroides, int clase, out float costo)
        {
            List<int> secuenciaSinCaracteresRepetidos = ModeloStringMatching.eliminarCaracteresRepetidos(secuencia);
            costo = distancia(gestoModeloPorGesto[clase], secuenciaSinCaracteresRepetidos, centroides);
            if (costo <= distanciaMaximaPorGesto[clase])
                return true;
            
            return false;
        }

        public int evaluar(int[] secuencia, Trayectoria3D centroides, out float costo)
        {
            List<int> secuenciaSinCaracteresRepetidos = ModeloStringMatching.eliminarCaracteresRepetidos(secuencia);
            float[] distancias = new float[cantGestos];
            for (int gesto = 0; gesto < cantGestos; gesto++)
                distancias[gesto] = distancia(gestoModeloPorGesto[gesto], secuenciaSinCaracteresRepetidos, centroides);
            
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
