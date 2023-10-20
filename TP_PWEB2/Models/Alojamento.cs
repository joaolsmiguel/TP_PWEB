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
    public class Alojamento
    {
        public Alojamento() {
            Reservas = new HashSet<Reserva>();
            imagens = new HashSet<Imagens_Alojamento>();
            Avaliacoes = new HashSet<Avaliacao>();
        }

        [Key]
        public string AlojamentoId { get; set; }

        public string titulo { get; set; }
        
        public string descricao { get; set; }
        public double preco { get; set; }

        public int? categoria_Id { get; set; }
        public Categoria Categoria { get; set; }


        //para saber quem e o dono do alojamento
        public string DonoId { get; set; }
        public AppUser Dono { get; set; }

        public bool pode_ser_alugado { get; set; }

        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }
        public virtual ICollection<Imagens_Alojamento> imagens { get; set; }


        [NotMapped]
        public IFormFile imagem { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime check_in { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime check_out { get; set; }

        [NotMapped]
        public virtual ICollection<Categoria> lista_categorias { get; set; }
    }
}
