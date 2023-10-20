using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP_PWEB2.Models
{
    public class Categoria
    {
        public Categoria(){
            CategoriaCheck_List = new HashSet<CategoriaCheck_List>();
            alojamentos = new HashSet<Alojamento>();
        }

        [Key]
        public int CategoriaId { get; set; }
        public string nome { get; set; }

        public Boolean Ativo { get; set; }

        public ICollection<Alojamento> alojamentos { get; set; }
        public string AlojamentoId { get; set; }

        //lista Checklist
        public ICollection<CategoriaCheck_List> CategoriaCheck_List { get; set; }

    }


    public class CategoriaCheck_List
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }


        public int Check_ListId { get; set; }
        public Check_List Check { get; set; }
    }
}
