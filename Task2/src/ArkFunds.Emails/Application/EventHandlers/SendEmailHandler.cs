using ArkFunds.Emails.Core.Events;
using ArkFunds.Emails.Infrastructure.AntiCorruptionLayer.Reports;
using ArkFunds.Emails.Mailables;
using Coravel.Mailer.Mail.Interfaces;

namespace ArkFunds.Emails.Application.EventHandlers;

public class SendEmailHandler
{
    public static async Task<EmailsSend> Handle(ReportGenerated reportGeneratedEvent, IMailer mailer)
    {
        Console.WriteLine("=== Event ===");
        Console.WriteLine("Sending email...");
        Console.WriteLine("=============");
        var mail = new GenericMailable()
            .To("to@test.com")
            .From("from@test.com")
            .Html("<html><body><h1>Hi!</h1></body></html>");

        await mailer.SendAsync(mail);

        return new EmailsSend(reportGeneratedEvent.Id);
    }
}