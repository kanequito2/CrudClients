using System;
using System.ComponentModel.DataAnnotations;

namespace CrudClients.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool? Active { get; set; }
        public int Age { get; set; }
    }
}
