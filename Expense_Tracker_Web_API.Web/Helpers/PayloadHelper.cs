using Expense_Tracker_Web_API.Web.Utils;
using Newtonsoft.Json;

namespace Expense_Tracker_Web_API.Web.Helpers;

public static class PayloadHelper
{
    private static string? _secretKey;

    public static void Initialize(string secretKey)
    {
        _secretKey = secretKey;
    }
    public static T? DecryptAndDeserialize<T>(string encryptedPayload)
    {
        if (string.IsNullOrEmpty(_secretKey))
            throw new InvalidOperationException("PayloadHelper is not initialized with a secret key.");

        Payload payload = new();
        string decryptedJson = payload.DecryptPayload(encryptedPayload, _secretKey);
        return JsonConvert.DeserializeObject<T>(decryptedJson); 
    }
}
