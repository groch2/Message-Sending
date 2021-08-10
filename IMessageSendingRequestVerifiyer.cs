namespace MessageSending
{
    using System.Threading.Tasks;

    public interface IMessageSendingRequestVerifiyer
    {
        Task<RecaptchaVerifyResponse> VerifiyMessageSendingRequest(string token, string remoteIPAddress);
    }
}