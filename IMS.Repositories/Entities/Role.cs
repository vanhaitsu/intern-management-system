namespace IMS.Repositories.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        //Relationship
        public virtual ICollection<Account> Accounts { get; set;}
    }
}
