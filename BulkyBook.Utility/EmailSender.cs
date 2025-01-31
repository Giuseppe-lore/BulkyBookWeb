﻿using System;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var emailToSend = new MimeMessage();

            //emailToSend.From.Add(MailboxAddress.Parse("hello@mail.com"));

            //emailToSend.To.Add(MailboxAddress.Parse(email));

            //emailToSend.Subject = subject;
            //emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage};

            // Send email
            //using (var emailClient = new SmtpClient())
            //{
            //    emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            //    // NO TESTED because Gmail don't authorize third part app since may 2022
            //    //emailClient.Authenticate("instantgods@gmail.com", "password"); 

            //    emailClient.Send(emailToSend);

            //    emailClient.Disconnect(true);
            //}

            return Task.CompletedTask;
        }
    }
}

