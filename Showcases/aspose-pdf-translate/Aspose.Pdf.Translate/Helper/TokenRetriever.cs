﻿using System.Text.Json;

namespace Aspose.Pdf.Translate.Helper
{
    public class TokenRetriever
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tokenUrl = "https://id.groupdocs.cloud/connect/token";

        public TokenRetriever(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public async Task<string> GetTokenAsync()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });

                var response = await client.PostAsync(tokenUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponseStr = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var res = JsonSerializer.Deserialize<TokenResponse>(tokenResponseStr).AccessToken;

                        return res;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
                }
                else
                {
                    throw new Exception($"Failed to retrieve token: {response.StatusCode}");
                }
            }
        }
    }
}
