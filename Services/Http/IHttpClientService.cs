namespace Services.Http
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> PostAsync<T>(T model, string path) where T : class;
        Task<List<TResult>> PostAsync<TResult, TRequest>(TRequest request, string path) where TRequest : class where TResult : class;
        Task<T> PutAsync<T>(T model, string path) where T : class;
        Task<T> PutAsync<T>(string path);
        Task<HttpResponseMessage> DeleteAsync(string path);
        Task<List<T>> GetAsync<T>(string path);
        Task<T> GetByIdAsync<T>(string path);
        Task<T> SendAsync<T, TModel>(string path, TModel model, HttpMethod method);
        Task<T> DeserializeAsync<T>(HttpResponseMessage response);
        string Serialize<T>(T model);
    }
}