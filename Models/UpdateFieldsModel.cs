using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Models
{
    public class UpdateFieldsModel
    {
        public string NameUpdate { get; set; }
        public int AgeUpdate { get; set; }
        public bool? ActiveUpdate { get; set; }
    }
}
