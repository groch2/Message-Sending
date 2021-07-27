namespace MessageSending
{
    using System.Threading.Tasks;

    public interface IMessageSendingRequestVerifiyer
    {
        Task<bool> VerifiyMessageSendingRequest(string token, string remoteIPAddress);
    }
}