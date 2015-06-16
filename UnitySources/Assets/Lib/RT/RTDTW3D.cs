using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RT
{
    public class RTDTW3D
    {
        private List<Entrenamiento3D> entrenamientos;
        private ModeloDTW modeloDTW;

        public RTDTW3D()
        {
            entrenamientos = new List<Entrenamiento3D>();
        }

        public void addEntrenamiento(Entrenamiento3D e)
        {
            e.centrar();
            entrenamientos.Add(e);
        }

        public void entrenar()
        {
            modeloDTW = new ModeloDTW(entrenamientos.Count);
            for (int e = 0; e < entrenamientos.Count; e++)
                modeloDTW.entrenar(e, entrenamientos[e]);
        }

        public int evaluar(Trayectoria3D trayectoria, out float distancia)
        {
            trayectoria.centrar();
            return modeloDTW.evaluar(trayectoria, out distancia);
        }

        public float[] getDistanciaMaximaPorGesto()
        {
            return modeloDTW.getDistanciaMaximaPorGesto();
        }
    }
}
