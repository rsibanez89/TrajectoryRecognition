using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RT
{
    class ModeloStringMatching
    {
        private int cantGestos;
        private List<string>[] secuenciasAceptadasPorGesto;

        public ModeloStringMatching(int cantGestos)
        {
            this.cantGestos = cantGestos;
            secuenciasAceptadasPorGesto = new List<string>[cantGestos];
            for (int i = 0; i < cantGestos; i++)
                secuenciasAceptadasPorGesto[i] = new List<string>();
        }

        public void addSecuenciaAceptada(int nGesto, int[] secuencia)
        {
            List<int> secuenciaSinCaracteresRepetidos = eliminarCaracteresRepetidos(secuencia);
            string secuenciaAceptada = convertToString(secuenciaSinCaracteresRepetidos);
            if (!secuenciasAceptadasPorGesto[nGesto].Contains(secuenciaAceptada))
                secuenciasAceptadasPorGesto[nGesto].Add(secuenciaAceptada);
        }

        // La funcion anda perfecto puede pasar que quede el caracter 11, pero es porque es el cluster numero 11.
        public static List<int> eliminarCaracteresRepetidos(int[] secuencia)
        {
            List<int> result = new List<int>();
            result.Add(secuencia[0]);
            for (int i = 1; i < secuencia.Length; i++)
                if (secuencia[i - 1] != secuencia[i])
                    result.Add(secuencia[i]);
            return result;
        }

        private static string convertToString(List<int> cadena)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < cadena.Count; i++)
                result.Append(cadena[i].ToString());
            return result.ToString();
        }

        public int evaluar(int[] secuencia)
        {
            List<int> secuenciaSinCaracteresRepetidos = eliminarCaracteresRepetidos(secuencia);
            string secuenciaAEvaluar = convertToString(secuenciaSinCaracteresRepetidos);
            for (int nGesto = 0; nGesto < secuenciasAceptadasPorGesto.Length; nGesto++)
                if (secuenciasAceptadasPorGesto[nGesto].Contains(secuenciaAEvaluar))
                    return nGesto;
            return -1;
        }

        public List<string>[] getSecuenciasAceptadasPorGesto()
        {
            return secuenciasAceptadasPorGesto;
        }
    }
}
