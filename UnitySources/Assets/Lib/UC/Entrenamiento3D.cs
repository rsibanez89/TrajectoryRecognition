using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UC
{
    public class Entrenamiento3D
    {
        private List<Trayectoria3D> entrenamiento;

        #region Metodos base de la lista
        public Entrenamiento3D()
        {
            this.entrenamiento = new List<Trayectoria3D>();
        }

        public Entrenamiento3D(List<Trayectoria3D> trayectoria)
        {
            this.entrenamiento = trayectoria;
        }

        public int Count
        {
            get { return entrenamiento.Count; }
        }

        public void Add(Trayectoria3D value)
        {
            entrenamiento.Add(value);
        }

        public void Add(Entrenamiento3D entrenamiento)
        {
            for (int i = 0; i < entrenamiento.Count; i++)
                this.entrenamiento.Add(entrenamiento[i]);
        }

        public void Clear()
        {
            entrenamiento.Clear();
        }

        public void Insert(int index, Trayectoria3D value)
        {
            entrenamiento.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            entrenamiento.RemoveAt(index);
        }

        public Trayectoria3D this[int index]
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

        public Entrenamiento3D GetRange(int index, int count)
        {
            return new Entrenamiento3D(entrenamiento.GetRange(index, count));
        }
        #endregion

        #region Metodos de un Entrenamiento
        public Entrenamiento3D clon()
        {
            List<Trayectoria3D> copia = new List<Trayectoria3D>();
            for (int i = 0; i < entrenamiento.Count; i++)
                copia.Add(entrenamiento[i].clon());
            return new Entrenamiento3D(copia);
        }

        public void trasladar(Vector3D posicion)
        {
            for (int i = 0; i < entrenamiento.Count; i++)
                entrenamiento[i].trasladar(posicion);
        }

        // Centra individualmente cada trayectoria
        public void centrar()
        {
            for (int i = 0; i < entrenamiento.Count; i++)
                entrenamiento[i].centrar();
        }

        // Devuelve la media de una Trayectoria3D
        public Vector3D getMedia()
        {
            Trayectoria3D planchada = planchar();
            return planchada.getMedia();
        }

        public Trayectoria3D planchar()
        {
            Trayectoria3D retorno = new Trayectoria3D();
            for (int i = 0; i < entrenamiento.Count; i++)
                for (int j = 0; j < entrenamiento[i].Count; j++)
                    retorno.Add(entrenamiento[i][j]);
            return retorno;
        }
        #endregion

        #region LecturaEscritura
        public void salvar(string carpeta)
        {
            for (int i = 0; i < entrenamiento.Count; i++)
                entrenamiento[i].salvar(carpeta + "/T" + i + ".dat");
        }

        public static Entrenamiento3D getEntrenamiento(string carpeta)
        {
            Entrenamiento3D e = new Entrenamiento3D();
            string[] fileEntries = Directory.GetFiles(carpeta);
            foreach (string fileName in fileEntries)
                if (fileName.EndsWith(".dat"))
                    e.Add(Trayectoria3D.getTrayectoria(fileName));
            return e;
        }
        #endregion
    }
}
