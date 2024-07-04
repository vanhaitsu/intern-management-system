namespace IMS.Repositories.QueryModels
{
    public class QueryResultModel<TEntity> where TEntity : class
    {
        public int TotalCount { get; set; }
        public TEntity? Data { get; set; }
    }
}
