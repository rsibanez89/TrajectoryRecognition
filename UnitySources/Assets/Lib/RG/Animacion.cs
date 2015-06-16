using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;
using System.IO;

namespace RG
{
    public class Animacion
    {
        private static readonly char SEPARADOR = ' ';
        private List<Esqueleto> animacion;

        #region Metodos base de la lista
        public Animacion()
        {
            this.animacion = new List<Esqueleto>();
        }

        public Animacion(List<Esqueleto> animacion)
        {
            this.animacion = animacion;
        }

        public int Count
        {
            get { return animacion.Count; }
        }

        public void Add(Esqueleto value)
        {
            animacion.Add(value);
        }

        public void Clear()
        {
            animacion.Clear();
        }

        public void Insert(int index, Esqueleto value)
        {
            animacion.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            animacion.RemoveAt(index);
        }

        public Esqueleto this[int index]
        {
            get
            {
                return animacion[index];
            }
            set
            {
                animacion[index] = value;
            }
        }

        public Animacion GetRange(int index, int count)
        {
            return new Animacion(animacion.GetRange(index, count));
        }
        #endregion

        #region Metodos de una animacion
        public Animacion clon()
        {
            List<Esqueleto> retorno = new List<Esqueleto>();
            for (int i = 0; i < animacion.Count; i++)
                retorno.Add(animacion[i].clon());
            return new Animacion(retorno);
        }

        public static Animacion obtenerAnimacion(string ruta)
        {
            Animacion grabacion = new Animacion();
            StreamReader mySR = new StreamReader(ruta);
            string line;
            char[] delimiterChars = { SEPARADOR };
            while ((line = mySR.ReadLine()) != null)
            {
                string[] words = line.Split(delimiterChars);
                int index = 0;
                Esqueleto esqueletoFrame = new Esqueleto();
                foreach (EnumeradorPartesCuerpo joint in Enum.GetValues(typeof(EnumeradorPartesCuerpo)))
                {
                    ParteCuerpo bp = esqueletoFrame.obtenerParteCuerpo(joint);
                    bp.setPosicion(System.Convert.ToSingle(words[index]), System.Convert.ToSingle(words[index + 1]), System.Convert.ToSingle(words[index + 2]));
                    index += 3;
                }
                grabacion.Add(esqueletoFrame);
            }
            mySR.Close();
            return grabacion;
        }

        public Trayectoria3D obtenerTrayectoria3D(EnumeradorPartesCuerpo parteCuerpo, bool crearCopia)
        {
            Trayectoria3D retorno = new Trayectoria3D();
            if (crearCopia)
                for (int i = 0; i < animacion.Count; i++)
                    retorno.Add(animacion[i].obtenerParteCuerpo(parteCuerpo).posicion);
            else
                for (int i = 0; i < animacion.Count; i++)
                    retorno.Add(animacion[i].obtenerParteCuerpo(parteCuerpo).posicion.clon());
            return retorno;
        }
        #endregion

    }
}
