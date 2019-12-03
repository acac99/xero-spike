using Xero.NetStandard.OAuth2.Config;

namespace Invoice
{
    public class Xero
    {
        public void GetClient()
        {
            var config = new XeroConfiguration();
            config.ClientId = "yourClientId";
            config.ClientSecret = "secret";
            
        } 
    }
}