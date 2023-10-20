using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TP_PWEB2.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TP_PWEB2.Models
{
    public class Reserva
    {
        public Reserva(){
            img_dados = new HashSet<Imagens_Alojamento>();
        }

        [Key]
        public string ReservaId { get; set; }  //Id reserva

        //quem fez reserva
        public string UserId { get; set; }
        public AppUser User { get; set; }

        //Id da avaliacao
        public string avaliacaoId { get; set; }
        public virtual Avaliacao avaliacao { get; set; }


        //qual o alojamento
        public string AlojamentoId { get; set; }
        public Alojamento Alojamento { get; set; }

        //data entrada e saida
        public DateTime check_in{ get; set;}

        public DateTime check_out{ get; set;}

        public Boolean Reserva_Confirmada { get; set; }
        public Boolean Entregue { get; set; }
        public Boolean Recebida { get; set; }

        public virtual ICollection<Imagens_Alojamento> img_dados { get; set; }

        [NotMapped]
        public IList<Avaliacao> Avaliacoes_aos_clientes { get; set; }
    }
}


//Posso collocar quem entregou o imovel
