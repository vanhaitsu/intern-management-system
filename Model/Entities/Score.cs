namespace IMS.Models.Entities
{
    public class Score : BaseEntity
    {
        public float? Quiz1 { get; set; }
        public float? Quiz2 { get; set; }
        public float? Quiz3 { get; set; }
        public float? Quiz4 { get; set; }
        public float? Quiz5 { get; set; }
        public float? Quiz6 { get; set; }
        public float? QuizAvg { get; set; }
        public float? QuizFinal { get; set; }
        public float? Asm1 { get; set; }
        public float? Asm2 { get; set;}
        public float? Asm3 { get; set;}
        public float? Asm4 { get; set;}
        public float? Asm5 { get; set; }
        public float? AsmAvg { get; set;}
        public float? PraticeFinal { get; set;}
        public float? Audit {  get; set; }
        public float? GPAModule { get; set; }
        public string? LevelModule { get; set; }
        public string? Status { get; set; }

        //Relationship
        public Guid? TraineeId {  get; set; }
        public virtual Trainee? Trainee { get; set; }
    }
}
