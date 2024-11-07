using MAC_B.Models;
using MAC_B.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace MAC_B.Views
{
    public partial class LoginPage : ContentPage
    {
        private QRCodeGeneratorService _qrCodeGeneratorService; 

        public LoginPage()
        {
            InitializeComponent();
            _qrCodeGeneratorService = new QRCodeGeneratorService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var loginRequest = new LoginRequest
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient(new CustomHttpClientHandler()))
            {
                try
                {
                    var response = await httpClient.PostAsync("https://192.168.1.44:7225/api/Auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var userJson = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(userJson);

                        if (user.IsAdmin)
                        {
                            // Navigate to AdminPage if user is an admin
                            await Navigation.PushAsync(new AdminPage());

                        }
                        else
                        {
                            // Generate QR code and navigate to QRCodePage for regular users
                            var qrCodeImageSource = _qrCodeGeneratorService.GenerateQRCode(
                                user.Id.ToString(),
                                user.Username,
                                user.Name,
                                user.IDNumber
                            );

                            await Navigation.PushAsync(new QRCodePage(user, qrCodeImageSource));
                        }
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        resultLabel.Text = $"Error: {result}, Status Code: {response.StatusCode}";
                    }
                }
                catch (HttpRequestException ex)
                {
                    await DisplayAlert("Error", $"Request error: {ex.Message}", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
                }
            }
        }



        // Navigate to the SignUpPage when the label is tapped
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}
