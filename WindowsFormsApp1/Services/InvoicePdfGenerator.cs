using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceApp.Services
{
    public class InvoicePdfGenerator
    {
        public void GeneratePdf(Invoice invoice)
        {
            // Ustawienie licencji QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            // Ścieżka do pulpitu
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(desktopPath, $"Faktura{invoice.InvoiceId}.pdf");

            // Generowanie dokumentu
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    // Nagłówek faktury
                    page.Header().Row(row =>
                    {
                        row.RelativeColumn().Stack(stack =>
                        {
                            stack.Item().Text("Faktura Vat").FontSize(20).Bold();
                            stack.Item().Text($"Miasto: {invoice.ClientCity}");
                            stack.Item().Text($"Ulica: {invoice.ClientStreet}");
                            stack.Item().Text($"Numer Telefonu: {invoice.ClientPhone}");
                        });

                        row.ConstantColumn(100).Image(Placeholders.Image(100, 100)); // Logo (możesz podmienić na plik)
                    });

                    page.Content().Stack(stack =>
                    {
                        // Sekcja informacji o kliencie i fakturze
                        stack.Item().PaddingBottom(10).Row(row =>
                        {
                            row.RelativeColumn().Stack(innerStack =>
                            {
                                innerStack.Item().Text("Wystawiono dla:").Bold();
                                innerStack.Item().Text(invoice.ClientName);
                                innerStack.Item().Text(invoice.ClientStreet);
                                innerStack.Item().Text($"{invoice.ClientPostalCode}, {invoice.ClientCity}");
                                innerStack.Item().Text($"Numer Telefonu: {invoice.ClientPhone}");
                                innerStack.Item().Text($"NIP: {invoice.ClientNIP}");
                            });

                            row.RelativeColumn().Stack(innerStack =>
                            {
                                innerStack.Item().Text("Szczegóły faktury:").Bold();
                                innerStack.Item().Text($"ID Faktury: {invoice.InvoiceId}");
                                innerStack.Item().Text($"Data wydania faktury: {invoice.IssueDate:yyyy-MM-dd}");
                                innerStack.Item().Text($"Data zapłaty: {invoice.DueDate:yyyy-MM-dd}");
                            });
                        });

                        // Sekcja tabeli z podsumowaniem
                        stack.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Opis
                                columns.RelativeColumn(1); // Ilość
                                columns.RelativeColumn(2); // Cena jednostkowa
                                columns.RelativeColumn(2); // Razem
                            });

                            // Nagłówki tabeli
                            table.Header(header =>
                            {
                                header.Cell().Element(x => CellStyle(x)).Text("Description").Bold();
                                header.Cell().Element(x => CellStyle(x)).Text("Quantity").Bold();
                                header.Cell().Element(x => CellStyle(x)).Text("Unit Price").Bold();
                                header.Cell().Element(x => CellStyle(x)).Text("Total").Bold();
                            });

                            // Dane faktury (przykład)
                            table.Cell().Element(x => CellStyle(x)).Text("Service/Product");
                            table.Cell().Element(x => CellStyle(x)).Text("1");
                            table.Cell().Element(x => CellStyle(x)).Text($"{invoice.Amount:C}");
                            table.Cell().Element(x => CellStyle(x)).Text($"{invoice.Amount:C}");
                        });

                        // Sekcja podsumowania
                        stack.Item().PaddingTop(10).AlignRight().Stack(innerStack =>
                        {
                            innerStack.Item().Text($"Netto: {invoice.Amount:C}").FontSize(12);
                            innerStack.Item().Text($"VAT (23%): {invoice.Amount * 0.23:C}").FontSize(12);
                            innerStack.Item().Text($"Brutto: {invoice.Amount * 1.23:C}").FontSize(14).Bold();
                        });
                    });

                    page.Footer().Stack(footerStack =>
                    {
                        footerStack.Item().PaddingTop(1).Row(row =>
                        {
                            // Pusta przestrzeń z lewej strony
                            row.RelativeColumn(0.05f); // 30% pustego miejsca po lewej

                            // Sekcja "Podpis odbiorcy"
                            row.RelativeColumn().AlignLeft().Stack(innerStack =>
                            {
                                innerStack.Item().Text("Podpis odbiorcy").FontSize(12).Italic();
                                innerStack.Item().Element(innerContainer =>
                                {
                                    innerContainer.PaddingTop(10).AlignCenter().Text("________________________");
                                });
                            });

                            row.RelativeColumn(0.1f);

                            row.RelativeColumn().AlignRight().Stack(innerStack =>
                            {
                                innerStack.Item().Text("Podpis wystawiającego").FontSize(12).Italic();
                                innerStack.Item().Element(innerContainer =>
                                {
                                    innerContainer.PaddingTop(10).AlignCenter().Text("________________________");
                                });
                            });

                            row.RelativeColumn(0.05f);
                        });

                        footerStack.Item().AlignCenter().Text("Dziękujemy za współpracę!").FontSize(10);
                    });





                });
            });

            document.GeneratePdf(filePath);

            Console.WriteLine($"Invoice saved as: Faktura{invoice.InvoiceId}.pdf");
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold())
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black);
        }
    }
}
