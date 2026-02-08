namespace DM.Pagamentos.DodsPag;

public class DodsPagService
{
    public readonly string ApiKey;
    public readonly string EncryptionKey;

    public DodsPagService(string apiKey, string encryptionKey)
    {
        ApiKey = apiKey;
        EncryptionKey = encryptionKey;
    }
}
