namespace MessageSending
{
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