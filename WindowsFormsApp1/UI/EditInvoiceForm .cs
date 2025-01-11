using System;
using System.Windows.Forms;
using InvoiceApp.Services;

namespace InvoiceApp.UI
{
    public class EditInvoiceForm : Form
    {
        private Invoice invoice;
        private InvoiceService invoiceService;

        private TextBox companyNameTextBox;
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

        public EditInvoiceForm(Invoice invoice, InvoiceService service)
        {
            this.invoice = invoice;
            this.invoiceService = service;
            InitializeComponents();
            LoadInvoiceData();
        }

        private void InitializeComponents()
        {
            Text = "Edit Invoice";
            Size = new System.Drawing.Size(400, 600);

            var nipLabel = new Label { Text = "Client NIP", Dock = DockStyle.Top };
            nipTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false }; // Zablokowane pole

            var companyNameLabel = new Label { Text = "Company Name", Dock = DockStyle.Top };
            companyNameTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false }; // Zablokowane pole

            var nameLabel = new Label { Text = "Client Name", Dock = DockStyle.Top };
            nameTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false }; // Zablokowane pole

            var emailLabel = new Label { Text = "Client Email", Dock = DockStyle.Top };
            emailTextBox = new TextBox { Dock = DockStyle.Top };

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
            Controls.Add(companyNameTextBox);
            Controls.Add(companyNameLabel);
        }

        private void LoadInvoiceData()
        {
            companyNameTextBox.Text = invoice.CompanyName;
            nameTextBox.Text = invoice.ClientName;
            emailTextBox.Text = invoice.ClientEmail;
            nipTextBox.Text = invoice.ClientNIP;
            phoneTextBox.Text = invoice.ClientPhone;
            streetTextBox.Text = invoice.ClientStreet;
            cityTextBox.Text = invoice.ClientCity;
            postalCodeTextBox.Text = invoice.ClientPostalCode;
            amountTextBox.Text = invoice.Amount.ToString();
            issueDatePicker.Value = invoice.IssueDate;
            dueDatePicker.Value = invoice.DueDate;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                invoice.ClientEmail = emailTextBox.Text;
                invoice.ClientPhone = phoneTextBox.Text;
                invoice.ClientStreet = streetTextBox.Text;
                invoice.ClientCity = cityTextBox.Text;
                invoice.ClientPostalCode = postalCodeTextBox.Text;
                invoice.Amount = double.Parse(amountTextBox.Text);
                invoice.IssueDate = issueDatePicker.Value;
                invoice.DueDate = dueDatePicker.Value;

                invoiceService.UpdateInvoice(invoice); // Aktualizacja faktury w bazie danych

                MessageBox.Show("Invoice updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
