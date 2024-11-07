using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace MAC_B.Views
{
    public partial class TestLoginPage : ContentPage
    {
        public TestLoginPage()
        {
            InitializeComponent();
        }

        private async void OnTestLoginClicked(object sender, EventArgs e)
        {
            var loginUser = new
            {
                username = "logintest",
                passwordHash = "logintest"
            };

            var json = JsonConvert.SerializeObject(loginUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient(new CustomHttpClientHandler()))
            {
                try
                {
                    var response = await httpClient.PostAsync("https://10.0.2.2:7225/api/Auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        resultLabel.Text = "Login successful!";
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        resultLabel.Text = $"Error: {result}, Status Code: {response.StatusCode}";
                    }
                }
                catch (HttpRequestException ex)
                {
                    resultLabel.Text = $"Request error: {ex.Message}";
                }
                catch (Exception ex)
                {
                    resultLabel.Text = $"Unexpected error: {ex.Message}";
                }
            }
        }
    }
}
