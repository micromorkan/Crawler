namespace Crawler.Model
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; } = true;
        public T Dados { get; set; }
        public string ErroMessage { get; set; }
    }
}
