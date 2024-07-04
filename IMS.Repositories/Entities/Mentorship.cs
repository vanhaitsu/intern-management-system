namespace IMS.Repositories.Entities
{
    public class Mentorship : BaseEntity
    {
        public Guid InternId { get; set; }
        public Guid AccountId { get; set; }

        //Relationship
        public virtual Account? Account { get; set; }
        public virtual Intern? Intern { get; set; }
    }
}
