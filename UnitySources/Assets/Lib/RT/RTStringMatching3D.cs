using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RT
{
    public class RTStringMatching3D
    {
        private Trayectoria3D centroides;
        private List<Entrenamiento3D> entrenamientos;
        private ModeloStringMatching modeloStringMatching;

        public Trayectoria3D Centroides
        {
            get { return centroides; }

        }

        public RTStringMatching3D()
        {
            centroides = new Trayectoria3D();
            entrenamientos = new List<Entrenamiento3D>();
        }

        public void addEntrenamiento(Entrenamiento3D e, int k)
        {
            e.centrar();
            Kmeans3D kmeanAux = new Kmeans3D(e.planchar());
            kmeanAux.ejecutar(k);
            centroides.Add(kmeanAux.Centroides);
            entrenamientos.Add(e);
        }

        public void entrenar()
        {
            modeloStringMatching = new ModeloStringMatching(entrenamientos.Count);
            for (int e = 0; e < entrenamientos.Count; e++)
                for (int t = 0; t < entrenamientos[e].Count; t++)
                {
                    int[] secuencia = Kmeans3D.nearest(centroides, entrenamientos[e][t]);
                    modeloStringMatching.addSecuenciaAceptada(e, secuencia);
                }
        }

        public int evaluar(Trayectoria3D trayectoria)
        {
            trayectoria.centrar();
            int[] secuencia = Kmeans3D.nearest(centroides, trayectoria);
            return modeloStringMatching.evaluar(secuencia);
        }

        public List<string>[] getSecuenciasAceptadasPorGesto()
        {
            return modeloStringMatching.getSecuenciasAceptadasPorGesto();
        }
    }
}
