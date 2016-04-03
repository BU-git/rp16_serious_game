namespace Domain
{
    /// <summary>
    ///     Email credentials to use with EmailSender
    /// </summary>
    public class EmailCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; }
    }
}