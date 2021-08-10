namespace MessageSending
{
    public class RecaptchaVerifyResponse
    {
        public bool Success { get; set; }
        public float Score { get; set; }
        public string Action { get; set; }
        public string Challenge_ts { get; set; }
        public string Hostname { get; set; }
    }
}
