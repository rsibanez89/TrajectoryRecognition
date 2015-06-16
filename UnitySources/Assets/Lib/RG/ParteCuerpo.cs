using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UC;

namespace RG
{
    public class ParteCuerpo
    {
        private static readonly char SEPARADOR = ' ';
        public string nombre { get; set; }
        public Vector3D posicion { get; set; }

        public ParteCuerpo(string nombre)
        {
            this.nombre = nombre;
            this.posicion = new Vector3D();
        }

        public void setPosicion(float x, float y, float z)
        {
            this.posicion.X = x;
            this.posicion.Y = y;
            this.posicion.Z = z;
        }

        public void setPosicion(Vector3D p)
        {
            this.posicion.X = p.X;
            this.posicion.Y = p.Y;
            this.posicion.Z = p.Z;
        }

        public bool igual(ParteCuerpo pc)
        {
            if (!this.nombre.Equals(pc.nombre))
                return false;
            if (!this.posicion.igual(pc.posicion))
                return false;
            return true;
        }

        public ParteCuerpo clon(ParteCuerpo pc)
        {
            ParteCuerpo retorno = new ParteCuerpo(pc.nombre);
            retorno.setPosicion(pc.posicion);
            return retorno;
        }

        public override string ToString()
        {
            return posicion.X.ToString() + SEPARADOR + posicion.Y.ToString() + SEPARADOR + posicion.Z.ToString() + SEPARADOR;
        }
    }
}
