public class CustomHttpClientHandler : HttpClientHandler
{
    public CustomHttpClientHandler()
    {
        // Allow untrusted certificates
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    }
}
