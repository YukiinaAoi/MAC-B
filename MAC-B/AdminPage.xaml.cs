using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace MAC_B.Views
{
    public partial class AdminPage : ContentPage
    {
        public ObservableCollection<QrHistory> QrHistoryList { get; set; }

        public AdminPage()
        {
            InitializeComponent();
            QrHistoryList = new ObservableCollection<QrHistory>();
            qrHistoryListView.ItemsSource = QrHistoryList;
        }

        private async void OnQrScannerClicked(object sender, EventArgs e)
        {
            // Navigate to the QR scanner page
            await Navigation.PushAsync(new QrScannerPage(this));
        }

        // This method is to add QR data from scanner
        public void AddToHistory(string qrData)
        {
            QrHistoryList.Insert(0, new QrHistory
            {
                QRCodeData = qrData,
                Timestamp = DateTime.Now.ToString("g") // "g" for a general date/time pattern
            });
        }
    }

    public class QrHistory
    {
        public string QRCodeData { get; set; }
        public string Timestamp { get; set; }
    }
}

