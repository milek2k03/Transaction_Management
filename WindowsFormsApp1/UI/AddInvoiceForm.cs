using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using InvoiceApp.Services;

namespace InvoiceApp.UI
{
    public class AddInvoiceForm : Form
    {
        private InvoiceService invoiceService;

        private TextBox nameTextBox;
        private TextBox emailTextBox;
        private TextBox nipTextBox;
        private TextBox phoneTextBox;
        private TextBox streetTextBox;
        private TextBox cityTextBox;
        private TextBox postalCodeTextBox;
        private TextBox amountTextBox;
        private DateTimePicker issueDatePicker;
        private DateTimePicker dueDatePicker;
        private Button saveButton;

        public AddInvoiceForm(InvoiceService service)
        {
            invoiceService = service;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Add Invoice";
            Size = new System.Drawing.Size(400, 600);

            var nameLabel = new Label { Text = "Client Name", Dock = DockStyle.Top };
            nameTextBox = new TextBox { Dock = DockStyle.Top };

            var emailLabel = new Label { Text = "Client Email", Dock = DockStyle.Top };
            emailTextBox = new TextBox { Dock = DockStyle.Top };

            var nipLabel = new Label { Text = "NIP (numbers only)", Dock = DockStyle.Top };
            nipTextBox = new TextBox { Dock = DockStyle.Top };
            nipTextBox.KeyPress += (s, e) =>
            {
                // Tylko cyfry
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            var phoneLabel = new Label { Text = "Phone Number", Dock = DockStyle.Top };
            phoneTextBox = new TextBox { Dock = DockStyle.Top };

            var streetLabel = new Label { Text = "Street", Dock = DockStyle.Top };
            streetTextBox = new TextBox { Dock = DockStyle.Top };

            var cityLabel = new Label { Text = "City", Dock = DockStyle.Top };
            cityTextBox = new TextBox { Dock = DockStyle.Top };

            var postalCodeLabel = new Label { Text = "Postal Code", Dock = DockStyle.Top };
            postalCodeTextBox = new TextBox { Dock = DockStyle.Top };

            var amountLabel = new Label { Text = "Amount", Dock = DockStyle.Top };
            amountTextBox = new TextBox { Dock = DockStyle.Top };

            var issueDateLabel = new Label { Text = "Issue Date", Dock = DockStyle.Top };
            issueDatePicker = new DateTimePicker { Dock = DockStyle.Top };

            var dueDateLabel = new Label { Text = "Due Date", Dock = DockStyle.Top };
            dueDatePicker = new DateTimePicker { Dock = DockStyle.Top };

            saveButton = new Button { Text = "Save", Dock = DockStyle.Bottom };
            saveButton.Click += SaveButton_Click;

            Controls.Add(saveButton);
            Controls.Add(dueDatePicker);
            Controls.Add(dueDateLabel);
            Controls.Add(issueDatePicker);
            Controls.Add(issueDateLabel);
            Controls.Add(amountTextBox);
            Controls.Add(amountLabel);
            Controls.Add(postalCodeTextBox);
            Controls.Add(postalCodeLabel);
            Controls.Add(cityTextBox);
            Controls.Add(cityLabel);
            Controls.Add(streetTextBox);
            Controls.Add(streetLabel);
            Controls.Add(phoneTextBox);
            Controls.Add(phoneLabel);
            Controls.Add(nipTextBox);
            Controls.Add(nipLabel);
            Controls.Add(emailTextBox);
            Controls.Add(emailLabel);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Walidacja danych
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    throw new Exception("Client name is required.");
                }

                if (!Regex.IsMatch(nipTextBox.Text, @"^\d+$"))
                {
                    throw new Exception("NIP must contain only digits.");
                }

                // Tworzenie faktury
                Invoice invoice = new Invoice
                {
                    ClientName = nameTextBox.Text,
                    ClientEmail = emailTextBox.Text,
                    ClientNIP = nipTextBox.Text,
                    ClientCity = cityTextBox.Text,
                    ClientStreet = streetTextBox.Text,
                    ClientPostalCode = postalCodeTextBox.Text,
                    ClientPhone = phoneTextBox.Text,
                    Amount = double.Parse(amountTextBox.Text),
                    IssueDate = issueDatePicker.Value,
                    DueDate = dueDatePicker.Value
                };

                invoiceService.AddInvoice(invoice);

                MessageBox.Show("Invoice added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
