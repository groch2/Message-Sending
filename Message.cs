namespace MessageSending
{
    public class MessageSendingRequest
    {
        public Message Message { get; set; }
        public string RecaptchaToken { get; set; }

        public override string ToString()
        {
            return new { Message = Message.ToString(), RecaptchaToken }.ToString();
        }
    }

    public class Message
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return new { From, Subject, Body }.ToString();
        }
    }
}