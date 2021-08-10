namespace MessageSending
{
    using System.Threading.Tasks;

    public interface IMessageSendingRequestChecker
    {
        Task<RecaptchaVerifyResponse> CheckMessageSendingRequest(string token, string remoteIPAddress);
    }
}