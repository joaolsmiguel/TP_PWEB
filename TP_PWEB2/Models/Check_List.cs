using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP_PWEB2.Models
{
    public class Check_List
    {
        public Check_List(){
            CategoriaCheck_List = new HashSet<CategoriaCheck_List>();
        }

        [Key]
        public int id { get; set; }
        public string texto { get; set; }
        public Boolean Confirmado { get; set;  }

        public ICollection<CategoriaCheck_List> CategoriaCheck_List { get; set; }
    }
}
