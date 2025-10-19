namespace Application.Utils
{
    public class VerificarConexao
    {
        public static async Task<bool> TemInternet()
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync("https://www.google.com", HttpCompletionOption.ResponseHeadersRead);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}