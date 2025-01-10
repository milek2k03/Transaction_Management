using System;
using System.Windows.Forms;
using InvoiceApp.Services;

namespace InvoiceApp.UI
{
    public class EditInvoiceForm : Form
    {
        private Invoice invoice;
        private InvoiceService invoiceService;

        private TextBox nameTextBox;
        private TextBox emailTextBox;
        private TextBox nipTextBox;
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
            Size = new System.Drawing.Size(400, 400);

            var nameLabel = new Label { Text = "Client Name", Dock = DockStyle.Top };
            nameTextBox = new TextBox { Dock = DockStyle.Top };

            var emailLabel = new Label { Text = "Client Email", Dock = DockStyle.Top };
            emailTextBox = new TextBox { Dock = DockStyle.Top };

            var nipLabel = new Label { Text = "Client NIP", Dock = DockStyle.Top };
            nipTextBox = new TextBox { Dock = DockStyle.Top };

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
            Controls.Add(nipTextBox);
            Controls.Add(nipLabel);
            Controls.Add(emailTextBox);
            Controls.Add(emailLabel);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
        }

        private void LoadInvoiceData()
        {
            nameTextBox.Text = invoice.ClientName;
            emailTextBox.Text = invoice.ClientEmail;
            nipTextBox.Text = invoice.ClientNIP;
            amountTextBox.Text = invoice.Amount.ToString();
            issueDatePicker.Value = invoice.IssueDate;
            dueDatePicker.Value = invoice.DueDate;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                invoice.ClientName = nameTextBox.Text;
                invoice.ClientEmail = emailTextBox.Text;
                invoice.ClientNIP = nipTextBox.Text;
                invoice.Amount = double.Parse(amountTextBox.Text);
                invoice.IssueDate = issueDatePicker.Value;
                invoice.DueDate = dueDatePicker.Value;

                invoiceService.UpdateInvoice(invoice); // Zaktualizowanie faktury w bazie danych

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
