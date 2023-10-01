using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class HttpClientHelper
{
    private readonly HttpClient _httpClient;

    public HttpClientHelper(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<T> SendRequestAsync<T>(string url, HttpMethod method, string token = null, string model= null)
    {
        try
        {
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(token))
            {
                // Set the Authorization header with the JWT token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }     
            
            if (!string.IsNullOrEmpty(model))
            {
                // Set the Authorization header with the JWT token
                request.Content = new StringContent(model, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {

                string jsonResponse = await response.Content.ReadAsStringAsync();

                T responseData = JsonConvert.DeserializeObject<T>(jsonResponse);
                
                return responseData;
            }
            else
            {
                // Handle error responses here
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here
            throw new HttpRequestException($"HTTP request failed: {ex.Message}", ex);
        }
    }
    
    public async Task<HttpResponseMessage> SendRequestAsync(string url,string jsonRequestData, HttpMethod method, string token = null)
    {
        try
        {
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(token))
            {
                // Set the Authorization header with the JWT token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            request.Content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

            return await _httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            // Handle exceptions here
            throw new HttpRequestException($"HTTP request failed: {ex.Message}", ex);
        }
    }
}
