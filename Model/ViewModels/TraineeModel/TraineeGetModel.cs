using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.TraineeModel
{
    public class TraineeGetModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string University { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public Guid? ProgramId { get; set; }
        public string ProgramName { get; set; }
        public bool? IsDelete { get; set; }
    }
}
