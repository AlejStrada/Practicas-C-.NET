using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio

{
    public class Elemento
    {
        public int Id { get; set;}
        public string Descripcion { get; set;}

        public override string ToString() // sobreescribo el meto toString para que me muestre la descripcion y no el objeto
        {
            return Descripcion;
        }
    }
}
