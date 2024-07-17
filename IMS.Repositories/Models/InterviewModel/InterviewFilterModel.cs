namespace IMS.Repositories.Models.InterviewModel
{
    public class InterviewFilterModel
    {
        public string Order { get; set; } = "date";
        public bool OrderByAscending { get; set; } = true;
        public string? Search { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
