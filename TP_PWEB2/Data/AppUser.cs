using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP_PWEB2.Models;

namespace TP_PWEB2.Data
{
    public class AppUser : IdentityUser
    {
        public AppUser() {
            Reservas = new HashSet<Reserva>();
            Alojamentos = new HashSet<Alojamento>();
            //Avaliacoes = new HashSet<Avaliacao>();
        }
        public string pNome { get; set; }
        public string uNome { get; set; }
        public string? Empresa { get; set; }
        public Boolean dono { get; set; }


        public virtual ICollection<Reserva> Reservas { get; set; }

        public virtual ICollection<Alojamento> Alojamentos { get; set; }

        //public virtual ICollection<Avaliacao> Avaliacoes { get; set; }
    }
}
