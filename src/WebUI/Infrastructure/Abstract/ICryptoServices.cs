namespace WebUI.Infrastructure.Abstract
{
    public interface ICryptoServices
    {
        string GenerateRandomPassword();
        string GenerateRandomAlphanumericString(int length);
    }
}
