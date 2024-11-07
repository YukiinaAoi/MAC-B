using MAC_B.Services;
using Microsoft.Maui.Controls;
using System;

namespace MAC_B.Views
{
    public partial class QRCodeTestPage : ContentPage
    {
        private QRCodeGeneratorService _qrCodeGeneratorService;

        public QRCodeTestPage()
        {
            InitializeComponent();
            _qrCodeGeneratorService = new QRCodeGeneratorService();
        }

        private void OnGenerateQRCodeClicked(object sender, EventArgs e)
        {
            // Example user data
            string id = "1";
            string username = "testuser";
            string name = "Test User";
            string idNumber = "123456";

            try
            {
                var qrCodeImageSource = _qrCodeGeneratorService.GenerateQRCode(id, username, name, idNumber);
                GeneratedQRCodeImage.Source = qrCodeImageSource;
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully, for now, we'll just output to debug
                System.Diagnostics.Debug.WriteLine($"Error generating QR code: {ex.Message}");
            }
        }
    }
}
