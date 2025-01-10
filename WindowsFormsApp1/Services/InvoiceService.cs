using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using LiteDB;
using MimeKit;
using MailKit.Net.Smtp;
using InvoiceApp.Services.InvoiceApp.Services;
using System.IO;
using System.Windows.Forms;


namespace InvoiceApp.Services
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientNIP { get; set; }
        public string ClientPhone { get; set; }
        public string ClientStreet { get; set; }
        public string ClientCity { get; set; }
        public string ClientPostalCode { get; set; }
        public double Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class InvoiceService
    {
        private const string DatabasePath = "InvoiceApp.db";
        private string currentUserEmail;

        private readonly EmailService emailService;

        public InvoiceService(User user)
        {
            currentUserEmail = user.Email;

            // Konfiguracja EmailService
            emailService = new EmailService(
                smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                senderEmail: "your-email@gmail.com",
                senderPassword: "your-password" // Możesz przechowywać to w zmiennych środowiskowych
            );
        }


        private string GetSafeCollectionName(string email)
        {
            // Zamień znaki specjalne na podkreślenia
            return email.Replace(".", "_").Replace("@", "_");
        }

        public void AddInvoice(Invoice invoice)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var collectionName = GetSafeCollectionName(currentUserEmail) + "_invoices";
                var collection = db.GetCollection<Invoice>(collectionName);
                collection.Insert(invoice);
            }
        }

        public List<Invoice> GetInvoices()
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var collectionName = GetSafeCollectionName(currentUserEmail) + "_invoices";
                var collection = db.GetCollection<Invoice>(collectionName);
                return collection.FindAll().ToList();
            }
        }

        public Invoice GetInvoice(int invoiceId)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var collectionName = GetSafeCollectionName(currentUserEmail) + "_invoices";
                var collection = db.GetCollection<Invoice>(collectionName);
                return collection.FindOne(i => i.InvoiceId == invoiceId);
            }
        }

        // Inne metody (AddInvoice, GetInvoices, itp.)

        public void GenerateInvoicePdf(Invoice invoice)
        {
            try
            {
  

                // Utwórz folder, jeśli nie istnieje
                System.IO.Directory.CreateDirectory("Invoices");

                var pdfGenerator = new InvoicePdfGenerator();
                pdfGenerator.GeneratePdf(invoice);

                MessageBox.Show($"Invoice PDF generated at: ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateInvoice(Invoice invoice)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var collectionName = GetSafeCollectionName(currentUserEmail) + "_invoices";
                var collection = db.GetCollection<Invoice>(collectionName);

                collection.Update(invoice);
            }
        }


        public List<Invoice> SortInvoices(string sortBy, bool ascending = true)
        {
            var invoices = GetInvoices();

            if (sortBy.ToLower() == "clientname")
            {
                return ascending
                    ? invoices.OrderBy(i => i.ClientName).ToList()
                    : invoices.OrderByDescending(i => i.ClientName).ToList();
            }
            else if (sortBy.ToLower() == "amount")
            {
                return ascending
                    ? invoices.OrderBy(i => i.Amount).ToList()
                    : invoices.OrderByDescending(i => i.Amount).ToList();
            }
            else if (sortBy.ToLower() == "issuedate")
            {
                return ascending
                    ? invoices.OrderBy(i => i.IssueDate).ToList()
                    : invoices.OrderByDescending(i => i.IssueDate).ToList();
            }
            else if (sortBy.ToLower() == "duedate")
            {
                return ascending
                    ? invoices.OrderBy(i => i.DueDate).ToList()
                    : invoices.OrderByDescending(i => i.DueDate).ToList();
            }
            else
            {
                return invoices; // Jeśli sortBy jest nieprawidłowe, zwracamy niesortowaną listę
            }
        }


    }

    namespace InvoiceApp.Services
    {
        public class EmailService
        {
            private readonly string smtpServer;
            private readonly int smtpPort;
            private readonly string senderEmail;
            private readonly string senderPassword;

            public EmailService(string smtpServer, int smtpPort, string senderEmail, string senderPassword)
            {
                this.smtpServer = smtpServer;
                this.smtpPort = smtpPort;
                this.senderEmail = senderEmail;
                this.senderPassword = senderPassword;
            }

            public void SendEmail(string recipientEmail, string subject, string body, string attachmentPath = null)
            {
                try
                {
                    // Tworzenie wiadomości e-mail
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("InvoiceApp", senderEmail));
                    message.To.Add(new MailboxAddress("", recipientEmail));
                    message.Subject = subject;

                    var bodyBuilder = new BodyBuilder { TextBody = body };

                    // Dodanie załącznika, jeśli istnieje
                    if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
                    {
                        bodyBuilder.Attachments.Add(attachmentPath);
                    }

                    message.Body = bodyBuilder.ToMessageBody();

                    // Wysyłanie wiadomości przez MailKit
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate(senderEmail, senderPassword);
                        client.Send(message);
                        client.Disconnect(true);
                    }

                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
            }
        }
    }



}
