using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    [Table("Role")]
    public class Role
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
