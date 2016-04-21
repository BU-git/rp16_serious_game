﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class EmailBuilder : AbstractEmailBuilder
    {
        public override void SetSubject(string subject)
        {
            MailMessage.Subject = subject;
        }

        public override void SetBody(string body)
        {
            MailMessage.Body = body;
        }

        public override void AddAttachments(AttachmentCollection attachments)
        {
            foreach (var attachment in attachments)
            {
                MailMessage.Attachments.Add(attachment);
            }
        }

        public override void SetAddressees(string from, params string[] to)
        {
            foreach (var s in to)
            {
                MailMessage.To.Add(s);
            }
            MailMessage.From = new MailAddress(from);
        }
    }
}
