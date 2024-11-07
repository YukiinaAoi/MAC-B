using Microsoft.Maui.Graphics.Platform;
using QRCoder;
using SkiaSharp;

namespace MAC_B.Services
{
    public class QRCodeGeneratorService
    {
        public ImageSource GenerateQRCode(string id, string username, string name, string idNumber)
        {
            try
            {
                string qrData = $"Id={id},Username={username},Name={name},IDNumber={idNumber}";

                using (var qrGenerator = new QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q))
                {
                    int pixelsPerModule = 10;
                    int qrSize = qrCodeData.ModuleMatrix.Count * pixelsPerModule;

                    using (var surface = SKSurface.Create(new SKImageInfo(qrSize, qrSize)))
                    {
                        var canvas = surface.Canvas;
                        canvas.Clear(SKColors.White);

                        for (int x = 0; x < qrCodeData.ModuleMatrix.Count; x++)
                        {
                            for (int y = 0; y < qrCodeData.ModuleMatrix.Count; y++)
                            {
                                if (qrCodeData.ModuleMatrix[x][y])
                                {
                                    var rect = new SKRect(x * pixelsPerModule, y * pixelsPerModule,
                                                          (x + 1) * pixelsPerModule, (y + 1) * pixelsPerModule);
                                    canvas.DrawRect(rect, new SKPaint { Color = SKColors.Black });
                                }
                            }
                        }

                        using (var image = surface.Snapshot())
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))

                        using (var stream = new MemoryStream())
                        {
                            data.SaveTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);

                            // Convert the stream to ImageSource
                            return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generating QR code: {ex.Message}");
                throw;
            }
        }
    }
}