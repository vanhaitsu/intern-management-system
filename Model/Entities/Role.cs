using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        //Relationship
        public virtual ICollection<Account> Accounts { get; set;}
    }
}
