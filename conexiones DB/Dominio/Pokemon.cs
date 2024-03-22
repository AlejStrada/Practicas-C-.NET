using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pokemon
    {
        public int Id { get; set; }

        [DisplayName ("NUMERO")]  
        public int Numero { get; set; }

        [DisplayName("NOMBRE")]
        public string Nombre { get; set; }

        [DisplayName("DESCRIPCION")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }

        [DisplayName("TIPO")]
        public Elemento Tipo { get; set; }

        [DisplayName("DEBILIDAD")]
        public Elemento Debilidad { get; set; }

    }
}
