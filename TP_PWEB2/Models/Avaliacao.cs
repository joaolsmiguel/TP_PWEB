using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TP_PWEB2.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_PWEB2.Models
{
    public class Avaliacao
    {
        [Key]
        public string AvaliacaoId { get; set; }


        public string avaliacao_do_cliente { get; set; }
        public string avalicacao_do_gestor { get; set; }

        public string id_cliente { get; set; }
        public string id_gestor { get; set; }

        public Alojamento alojamento { get; set; }
        public string AlojamentoId { get; set; }


        //Avaliou reserva em contexto
        [ForeignKey("Reserva")]
        public virtual Reserva reserva { get; set; }
        public string reservaId { get; set; }
    }
}
