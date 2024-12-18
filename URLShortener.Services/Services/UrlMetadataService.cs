﻿using Microsoft.Extensions.Configuration;
using System.Text.Json;
using URLShortener.Core.Models;

namespace URLShortener.Services;

public class UrlMetadataService
{
    private readonly HttpClient? _httpClient;
    private readonly IConfiguration? _configuration;

    public UrlMetadataService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public UrlMetadataService()
    {
    }


    public async Task<UrlMetadata> FetchMetadataAsync(string url)
    {
        try
        {
            var apiKey = _configuration["ApiSettings:LinkPreviewApiKey"];
            string apiUrl = $"https://api.linkpreview.net?key={apiKey}&q={Uri.EscapeDataString(url)}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch metadata from the API.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var metadata = JsonSerializer.Deserialize<UrlMetadata>(json);

            if (metadata == null)
            {
                throw new Exception("Failed to parse metadata response.");
            }

            return metadata;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching metadata: {ex.Message}");
            return new UrlMetadata
            {
                Title = "No Title Available",
                Description = "No Description Available",
                ImageUrl = "https://static.thenounproject.com/png/3674270-200.png"
            };
        }
    }
}
