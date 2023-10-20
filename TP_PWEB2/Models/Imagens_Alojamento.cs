using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP_PWEB2.Models
{
    public class Imagens_Alojamento
    {
        [Key]
        public int imgId { get; set; }
        public string path { get; set; }

        public string AlojamentoId { get; set; }
        public virtual Alojamento alojamento { get; set; }

        public string ReservaId { get; set; }
        public virtual Reserva Reserva { get; set; }

        
    }
}
