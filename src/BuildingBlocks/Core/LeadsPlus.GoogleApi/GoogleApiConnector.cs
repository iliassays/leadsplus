namespace LeadsPlus.GoogleApis
{
    using Google.Apis.Auth.OAuth2;
    using System.IO;

    public class GoogleApiConnector : IGoogleApiConnector
    {
        public GoogleApiConnector()
        {
           
        }

        public GoogleCredential CreateCredential(string connectionConfigFilePath, string[] scopes)
        {
            GoogleCredential googleCredential;

            using (var stream = new FileStream(connectionConfigFilePath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(stream)
                    .CreateScoped(scopes);
            }

            return googleCredential;
        }
    }
}
