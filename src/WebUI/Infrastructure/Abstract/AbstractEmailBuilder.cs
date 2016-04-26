﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public abstract class AbstractEmailBuilder
    {
        protected MailMessage MailMessage { get; private set; }

        public void CreateNewMessage()
        {
            MailMessage = new MailMessage();
        }

        public MailMessage GetMailMessage()
        {
            return MailMessage;
        }

        public abstract void SetSubject(string subject);
        public abstract void SetBody(string body);
        public abstract void AddAttachments(AttachmentCollection attachments);
        public abstract void SetAddressees(params string[] to);
    }
}
