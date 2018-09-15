using Google.Apis.Auth.OAuth2;

namespace LeadsPlus.GoogleApis
{
    public interface IGoogleApiConnector
    {
        GoogleCredential CreateCredential(string connectionConfigFilePath, string[] scopes);      
    }
}
