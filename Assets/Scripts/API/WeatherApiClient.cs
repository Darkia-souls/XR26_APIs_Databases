using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "http://api.openweathermap.org/data/2.5/weather";
        [SerializeField] private string apiKey = "144b966c6258d859947df68b6f9a1497";
        
        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// TODO: Students will implement this method
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }
            
            // Check if API key is configured
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }
            
            // TODO: Build the complete URL with city and API key //DONE
            string url = $"{baseUrl}?q={city}&appid={apiKey}";
            
            // TODO: Create UnityWebRequest and use modern async pattern
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // TODO: Use async/await, send the request and wait for response //DONE
                await request.SendWebRequest();
                
                // TODO: Implement proper error handling for different result types //DONE
                // Check request.result for Success, ConnectionError, ProtocolError, DataProcessingError
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        return ParseWeatherData(request.downloadHandler.text);
                
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"Network connection failed: {request.error}");
                        break;
                
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"HTTP Error {request.responseCode}: {request.error}");
                        break;
                
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Data processing failed: {request.error}");
                        break;
                }
        
                return null;
                
                // TODO: Parse JSON response using Newtonsoft.Json
                
                // TODO: Return the parsed WeatherData object
                
                return null; // Placeholder - students will replace this
            }
        }
        
        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");
            
            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
        
        private WeatherData ParseWeatherData(string json)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.DeserializeObject<WeatherData>(json, settings);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON parsing failed: {ex.Message}");
                return null;
            }
        }
    }
    
    
}