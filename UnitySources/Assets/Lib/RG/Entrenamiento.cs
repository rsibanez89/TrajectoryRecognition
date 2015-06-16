using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UC;

namespace RG
{
    public class Entrenamiento
    {
        private List<Animacion> entrenamiento;

        #region Metodos base de la lista
        public Entrenamiento()
        {
            this.entrenamiento = new List<Animacion>();
        }

        public Entrenamiento(List<Animacion> entrenamiento)
        {
            this.entrenamiento = entrenamiento;
        }

        public int Count
        {
            get { return entrenamiento.Count; }
        }

        public void Add(Animacion value)
        {
            entrenamiento.Add(value);
        }

        public void Add(Entrenamiento e)
        {            
            for (int i = 0; i < e.Count; i++)
                entrenamiento.Add(e[i]);
        }

        public void Clear()
        {
            entrenamiento.Clear();
        }

        public Animacion this[int index]
        {
            get
            {
                return entrenamiento[index];
            }
            set
            {
                entrenamiento[index] = value;
            }
        }
        #endregion

        #region Metodos del entrenamiento
        public Entrenamiento3D obtenerEntrenamiento3D(EnumeradorPartesCuerpo parteCuerpo, bool crearCopia)
        {
            Entrenamiento3D retorno = new Entrenamiento3D();
            for (int i = 0; i < entrenamiento.Count; i++)
                retorno.Add(entrenamiento[i].obtenerTrayectoria3D(parteCuerpo, crearCopia));
            return retorno;
        }

        public static Entrenamiento obtenerEntrenamiento(string rutaCarpeta)
        {
            string[] fileEntries = Directory.GetFiles(rutaCarpeta);
            Entrenamiento retorno = new Entrenamiento();
            foreach (string fileName in fileEntries)
            {
                string relativePath = fileName.Substring(rutaCarpeta.Length);
                if (relativePath.StartsWith("Entrenamiento") && relativePath.EndsWith("txt"))
                    retorno.Add(Animacion.obtenerAnimacion(fileName));
            }
            return retorno;
        }
        #endregion
    }
}
