using Microsoft.Maui.Controls;
using ZXing.Net.Maui.Controls;
using System;
using ZXing.Net.Maui;

namespace MAC_B.Views
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        // Handle barcode detection with try-catch block for error handling
        private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
        {
            try
            {
                // Check if Results exists and count items
                var results = e.Results?.ToList(); // Ensure that Results is a collection (if it's a method, we call it)
                if (results != null && results.Count > 0)
                {
                    var qrValue = results[0].Value;
                    System.Diagnostics.Debug.WriteLine($"Detected QR Code: {qrValue}");
                    // You can add further handling logic here, such as displaying the value
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No QR code detected.");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                System.Diagnostics.Debug.WriteLine($"Error during barcode detection: {ex.Message}");
            }
        }

        // Override OnAppearing to ensure the barcode reader initializes correctly
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Ensure the barcode reader view is fully visible before starting scanning
            barcodeReaderView.IsVisible = true;
        }
    }
}
