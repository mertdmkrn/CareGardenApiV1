using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Handler.Concrete
{
    public class SmsHandler : ISmsHandler
    {
        public async Task<bool> SendSmsAsync(string message, string telephoneNumber, string senderType = "GetConfirmationCode")
        {
            HttpClient client = new HttpClient();
            var configuration = HelperMethods.GetConfiguration();

            SmsRequest smsRequest = new SmsRequest();
            SmsRequestBody smsRequestBody = new SmsRequestBody();
            smsRequestBody.authentication = new SmsAuthentication(configuration["SmsSettings:Username"], configuration["SmsSettings:Password"]);

            SmsOrder smsOrder = new SmsOrder();

            smsOrder.sender = senderType;
            smsOrder.iys = "1";
            smsOrder.iysList = "INVIDUAL";
            smsOrder.sendDateTime = new List<string>() { DateTime.Now.ToString() };

            SmsMessage smsMessage = new SmsMessage();
            smsMessage.text = message;
            
            SmsReceipts smsReceipts = new SmsReceipts();
            smsReceipts.number = new List<string>();
            smsReceipts.number.Add(telephoneNumber);

            smsMessage.receipts = smsReceipts;
            smsOrder.message = smsMessage;
            smsRequestBody.order = smsOrder;

            smsRequest.request = smsRequestBody;

            HttpResponseMessage response = await client.PostAsJsonAsync("https://api.iletimerkezi.com/v1/send-sms/json", smsRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;

        }
    }
}
