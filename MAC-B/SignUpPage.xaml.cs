using MAC_B.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MAC_B.Views
{
    public partial class SignUpPage : ContentPage
    {
        private byte[] _selectedImageData; // Store the image data as a byte array

        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            _selectedImageData = memoryStream.ToArray(); // Read the image into a byte array
                        }
                    }
                    imagePathLabel.Text = $"Selected: {result.FileName}";

                    // Load the image into the Image control
                    selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(_selectedImageData));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Image selection failed: {ex.Message}", "OK");
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Id = 0,
                Username = usernameEntry.Text,
                PasswordHash = passwordEntry.Text,
                Name = nameEntry.Text,
                IDNumber = idEntry.Text,
                ImageData = _selectedImageData // Use the selected image data
            };

            if (string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.PasswordHash) ||
                string.IsNullOrEmpty(user.IDNumber) ||
                _selectedImageData == null) // Check if the image data is selected
            {
                await DisplayAlert("Error", "Please fill in all fields and select an image.", "OK");
                return;
            }

            var json = JsonConvert.SerializeObject(new { user.Id, user.Username, user.Name, user.IDNumber, user.PasswordHash, ImageData = Convert.ToBase64String(user.ImageData) });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient(new CustomHttpClientHandler()))
            {
                try
                {
                    var response = await httpClient.PostAsync("https://192.168.1.44:7225/api/Auth/signup", content);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Success", "Account created successfully!", "OK");
                        await Navigation.PushAsync(new LoginPage());
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
