using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using InvoiceApp.Services;

namespace InvoiceApp.UI
{
    public class AddInvoiceForm : Form
    {
        private InvoiceService invoiceService;

        private TextBox nipTextBox;
        private TextBox companyNameTextBox;
        private TextBox nameTextBox;
        private TextBox emailTextBox;
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

            var nipLabel = new Label { Text = "NIP (numbers only)", Dock = DockStyle.Top };
            nipTextBox = new TextBox { Dock = DockStyle.Top };
            nipTextBox.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            nipTextBox.TextChanged += NipTextBox_TextChanged;

            var companyNameLabel = new Label { Text = "Company Name", Dock = DockStyle.Top };
            companyNameTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var nameLabel = new Label { Text = "Client Name", Dock = DockStyle.Top };
            nameTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var emailLabel = new Label { Text = "Client Email", Dock = DockStyle.Top };
            emailTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var phoneLabel = new Label { Text = "Phone Number", Dock = DockStyle.Top };
            phoneTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var streetLabel = new Label { Text = "Street", Dock = DockStyle.Top };
            streetTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var cityLabel = new Label { Text = "City", Dock = DockStyle.Top };
            cityTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var postalCodeLabel = new Label { Text = "Postal Code", Dock = DockStyle.Top };
            postalCodeTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var amountLabel = new Label { Text = "Amount", Dock = DockStyle.Top };
            amountTextBox = new TextBox { Dock = DockStyle.Top, Enabled = false };

            var issueDateLabel = new Label { Text = "Issue Date", Dock = DockStyle.Top };
            issueDatePicker = new DateTimePicker { Dock = DockStyle.Top, Enabled = false };

            var dueDateLabel = new Label { Text = "Due Date", Dock = DockStyle.Top };
            dueDatePicker = new DateTimePicker { Dock = DockStyle.Top, Enabled = false };

            saveButton = new Button { Text = "Save", Dock = DockStyle.Bottom, Enabled = false };
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

        private async void NipTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nipTextBox.Text.Length == 10 && long.TryParse(nipTextBox.Text, out _))
            {
                try
                {
                    var nipService = new PublicNipService();
                    var companyData = await nipService.GetCompanyData(nipTextBox.Text);

                    companyNameTextBox.Text = companyData.Name;
                    emailTextBox.Text = companyData.Email;
                    nameTextBox.Text = companyData.Surname;
                    streetTextBox.Text = companyData.Street;
                    cityTextBox.Text = companyData.City;
                    postalCodeTextBox.Text = companyData.PostalCode;
                    phoneTextBox.Text = companyData.PhoneNumber ?? string.Empty;

                    EnableFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to fetch data for NIP: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DisableFields();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    throw new Exception("Client name is required.");
                }

                if (!Regex.IsMatch(nipTextBox.Text, @"^\d+$"))
                {
                    throw new Exception("NIP must contain only digits.");
                }

                Invoice invoice = new Invoice
                {
                    CompanyName = companyNameTextBox.Text,
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

        private void EnableFields()
        {
            nameTextBox.Enabled = true;
            emailTextBox.Enabled = true;
            phoneTextBox.Enabled = true;
            streetTextBox.Enabled = true;
            cityTextBox.Enabled = true;
            postalCodeTextBox.Enabled = true;
            amountTextBox.Enabled = true;
            issueDatePicker.Enabled = true;
            dueDatePicker.Enabled = true;
            saveButton.Enabled = true;
        }

        private void DisableFields()
        {
            nameTextBox.Enabled = false;
            emailTextBox.Enabled = false;
            phoneTextBox.Enabled = false;
            streetTextBox.Enabled = false;
            cityTextBox.Enabled = false;
            postalCodeTextBox.Enabled = false;
            amountTextBox.Enabled = false;
            issueDatePicker.Enabled = false;
            dueDatePicker.Enabled = false;
            saveButton.Enabled = false;
        }
    }
}
