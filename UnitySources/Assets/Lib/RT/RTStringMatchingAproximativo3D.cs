using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RT
{
    public class RTStringMatchingAproximativo3D
    {
        private Trayectoria3D centroides;
        private List<Entrenamiento3D> entrenamientos;
        private ModeloStringMatchingAproximativo modeloStringMatchingAproximativo;

        public RTStringMatchingAproximativo3D()
        {
            centroides = new Trayectoria3D();
            entrenamientos = new List<Entrenamiento3D>();
        }

        public void agregarEntrenamiento(Entrenamiento3D e, int k)
        {
            e.centrar();
            Kmeans3D kmeanAux = new Kmeans3D(e.planchar());
            kmeanAux.ejecutar(k);
            centroides.Add(kmeanAux.Centroides);
            entrenamientos.Add(e);
        }

        public void entrenar()
        {
            modeloStringMatchingAproximativo = new ModeloStringMatchingAproximativo(entrenamientos.Count);
            for (int e = 0; e < entrenamientos.Count; e++)
            {
                List<int[]> secuenciasGestoN = new List<int[]>();
                for (int t = 0; t < entrenamientos[e].Count; t++)
                    secuenciasGestoN.Add(Kmeans3D.nearest(centroides, entrenamientos[e][t]));
                modeloStringMatchingAproximativo.entrenar(e, secuenciasGestoN, centroides);
            }
        }

        public bool evaluar(Trayectoria3D trayectoria, int clase, out float distancia)
        {
            trayectoria.centrar();
            int[] secuencia = Kmeans3D.nearest(centroides, trayectoria);
            return modeloStringMatchingAproximativo.evaluar(secuencia, centroides, clase, out distancia);
        }

        public int evaluar(Trayectoria3D trayectoria)
        {
            float distancia;
            return evaluar(trayectoria, out distancia);
        }

        public int evaluar(Trayectoria3D trayectoria, out float distancia)
        {
            trayectoria.centrar();
            int[] secuencia = Kmeans3D.nearest(centroides, trayectoria);
            return modeloStringMatchingAproximativo.evaluar(secuencia, centroides, out distancia);
        }

        public float[] getDistanciaMaximaPorGesto()
        {
            return modeloStringMatchingAproximativo.getDistanciaMaximaPorGesto();
        }
    }
}
