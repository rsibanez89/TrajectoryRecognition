using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RG
{
    public class Esqueleto
    {
        private Hashtable partesCuerpo;

        public Esqueleto()
        {
            partesCuerpo = new Hashtable();

            foreach (EnumeradorPartesCuerpo joint in Enum.GetValues(typeof(EnumeradorPartesCuerpo)))
                this.agregarParteCuerpo(new ParteCuerpo(joint.ToString())); // Agrega las partes definidas en el enumerador
        }

        public void agregarParteCuerpo(ParteCuerpo pc)
        {
            this.partesCuerpo.Add(pc.nombre, pc);
        }

        public ParteCuerpo obtenerParteCuerpo(string nombre)
        {
            if (partesCuerpo.ContainsKey(nombre))
                return (ParteCuerpo)partesCuerpo[nombre];
            return null;
        }

        public ParteCuerpo obtenerParteCuerpo(EnumeradorPartesCuerpo parteCuerpo)
        {
            return obtenerParteCuerpo(parteCuerpo.ToString());
        }

        public Hashtable obtenerPartesCuerpo()
        {
            return partesCuerpo;
        }

        public Esqueleto clon()
        {
            Esqueleto retorno = new Esqueleto();
            foreach (DictionaryEntry dE in obtenerPartesCuerpo())
                retorno.obtenerParteCuerpo((string)dE.Key).setPosicion(this.obtenerParteCuerpo((string)dE.Key).posicion);
            return retorno;
        }

        public bool igual(Esqueleto e)
        {
            if (e == null)
                return false;
            foreach (EnumeradorPartesCuerpo joint in Enum.GetValues(typeof(EnumeradorPartesCuerpo)))
                if (!(((ParteCuerpo)partesCuerpo[joint.ToString()]).igual((ParteCuerpo)e.partesCuerpo[joint.ToString()])))
                    return false;
            return true;
        }

        public void Clear()
        {
            partesCuerpo.Clear();
        }
    }
}
