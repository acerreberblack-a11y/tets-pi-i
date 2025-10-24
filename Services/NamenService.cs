using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPWhiteListManager.Services
{
    public class NamenService
    {
        private readonly HttpClient _httpClient;

        public NamenService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<(bool Success, string RequestNumber)> RegisterIPInNamen(string ipAddress, string environment)
        {
            try
            {
                // Здесь нужно заменить на реальный URL API namen
                var url = $"https://namen-api.example.com/register";

                var requestData = new
                {
                    IP = ipAddress,
                    Environment = environment,
                    Timestamp = DateTime.Now
                };

                // Имитация работы с API
                await Task.Delay(1000);

                // В реальной реализации здесь будет:
                // var json = JsonSerializer.Serialize(requestData);
                // var content = new StringContent(json, Encoding.UTF8, "application/json");
                // var response = await _httpClient.PostAsync(url, content);

                // Имитация успешной регистрации с номером заявки
                bool success = true; // или response.IsSuccessStatusCode
                string requestNumber = success ? $"NAMEN-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}" : null;

                return (success, requestNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации в namen: {ex.Message}");
                return (false, null);
            }
        }
    }
}