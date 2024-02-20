using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Cambio el namespace
namespace dominioD
{
    // La hago publica
    public class Discos
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        [DisplayName("Fecha de Lanzamiento")]
        public DateTime FechaLanzamiento { get; set; }

        [DisplayName("Cantidad de Canciones")]
        public int CantidadCanciones { get; set; }
        public string UrlImagenTapa { get; set; }

        [DisplayName("Estilo")]
        public Estilos IdEstilo { get; set; }

        [DisplayName("Tipo de Estilo")]
        public TiposEdicion IdTipoEdicion { get; set; }
    }
}
