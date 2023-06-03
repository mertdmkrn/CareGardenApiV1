using Org.BouncyCastle.Asn1.Ocsp;

namespace CareGardenApiV1.Handler.Model
{
    public class SmsRequest
    {
        public SmsRequestBody request { get; set; }
    }

    public class SmsRequestBody
    {
        public SmsAuthentication authentication { get; set; }
        public SmsOrder order { get; set; }
    }

    public class SmsAuthentication
    {
        public string username { get; set; }
        public string password { get; set; }

        public SmsAuthentication(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    public class SmsOrder
    {
        public string sender { get; set; }
        public List<string> sendDateTime { get; set; }
        public string iys { get; set; }
        public string iysList { get; set; }
        public SmsMessage message { get; set; }
    }

    public class SmsMessage
    {
        public string text { get; set; }
        public SmsReceipts receipts { get; set; }
    }

    public class SmsReceipts 
    {
        public List<string> number { get; set; }
    }
}
