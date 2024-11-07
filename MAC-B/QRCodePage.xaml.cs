using MAC_B.Models;
using Microsoft.Maui.Controls;

namespace MAC_B.Views
{
    public partial class QRCodePage : ContentPage
    {
        private User _user;

        public QRCodePage(User user, ImageSource qrCodeImageSource)
        {
            InitializeComponent();
            _user = user;

            // Set the QR code image source
            qrCodeImage.Source = qrCodeImageSource;

            // Display the user's image
            DisplayUserImage();
        }

        private void DisplayUserImage()
        {
            if (_user.ImageData != null)
            {
                var imageSource = ImageSource.FromStream(() => new MemoryStream(_user.ImageData));
                userImage.Source = imageSource; // Set the user image source
            }
        }
    }
}
