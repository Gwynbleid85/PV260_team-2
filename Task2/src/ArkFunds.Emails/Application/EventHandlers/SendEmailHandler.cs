using ArkFunds.Emails.Core.Events;
using ArkFunds.Emails.Infrastructure.AntiCorruptionLayer.Reports;
using ArkFunds.Emails.Infrastructure.AntiCorruptionLayer.Users;
using ArkFunds.Emails.Mailables;
using Coravel.Mailer.Mail.Interfaces;
using Wolverine;

namespace ArkFunds.Emails.Application.EventHandlers;

public class SendEmailHandler
{
    public static async Task<EmailsSend> Handle(ReportGenerated reportGeneratedEvent, IMailer mailer, IMessageBus bus)
    {
        Console.WriteLine("=== Event ===");
        Console.WriteLine("Sending email...");
        Console.WriteLine("=============");
        // Get subscribed users
        var query = new GetSubscribedUsersQuery();
        var subscribedUsers = await bus.InvokeAsync<GetSubscribedUsersQuery.Response>(query);

        foreach (var user in subscribedUsers.Users)
        {
            var mail = new GenericMailable()
                .To(user.Email)
                .From("from@test.com")
                .Subject("Yo, new report just dropped!")
                .Html($"<html>" +
                      $"<body>" +
                      $"<h1>Hi {user.Name}!</h1>" +
                      $"<h3>New report just dropped!!!<h3>" +
                      $"Check it out by clicking this button:" +
                      $"<a href=\"www.asdf.com\" style=\"color:blue;font-size:36px\">" +
                      $"<button>Check new report</button>" +
                      $"</a>" +
                      $"</body>" +
                      $"</html>"
                );

            await mailer.SendAsync(mail);
        }

        return new EmailsSend(reportGeneratedEvent.Id);
    }
}