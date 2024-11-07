using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using System.Linq;
//
namespace MAC_B.Views
{
    public partial class QrScannerPage : ContentPage
    {
        private AdminPage _adminPage;

        public QrScannerPage(AdminPage adminPage)
        {
            InitializeComponent();
            _adminPage = adminPage;  // Pass the reference of AdminPage

            barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = true,
                Multiple = false
            };
        }

        private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first is null || !barcodeReader.IsEnabled)
                return;

            await Dispatcher.DispatchAsync(async () =>
            {
                // Disable further scanning
                barcodeReader.IsEnabled = false;  // Stop the scanner immediately after detecting

                // Display a message with the QR code
                await DisplayAlert("Barcode Detected", first.Value, "OK");

                // Add the scanned QR data to the AdminPage's history
                _adminPage.AddToHistory(first.Value);

                // Optionally, navigate back to the AdminPage after the alert
                await Navigation.PopAsync();
            });
        }
    }
}
