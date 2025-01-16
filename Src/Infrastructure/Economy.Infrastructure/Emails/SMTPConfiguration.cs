﻿#nullable disable

namespace Economy.Infrastructure.Emails
{
    public class SMTPConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
