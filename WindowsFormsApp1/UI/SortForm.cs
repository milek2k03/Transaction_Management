// SortForm.cs
using System;
using System.Windows.Forms;
using InvoiceApp.Services;
//using WindowsFormsApp1.Services;

namespace InvoiceApp.UI
{
    public class SortForm : Form
    {
        private ComboBox sortByComboBox;
        private CheckBox ascendingCheckBox;
        private Button submitButton;
        private InvoiceService invoiceService;

        public SortForm(InvoiceService service)
        {
            invoiceService = service;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Sort Invoices";
            Size = new System.Drawing.Size(300, 200);

            var sortByLabel = new Label { Text = "Sort By", Dock = DockStyle.Top };
            sortByComboBox = new ComboBox { Dock = DockStyle.Top };
            sortByComboBox.Items.AddRange(new string[] { "ClientName", "Amount", "IssueDate" });

            ascendingCheckBox = new CheckBox { Text = "Ascending", Dock = DockStyle.Top };

            submitButton = new Button { Text = "Sort", Dock = DockStyle.Bottom };
            submitButton.Click += SubmitButton_Click;

            Controls.Add(submitButton);
            Controls.Add(ascendingCheckBox);
            Controls.Add(sortByComboBox);
            Controls.Add(sortByLabel);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var sortBy = sortByComboBox.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(sortBy))
            {
                var ascending = ascendingCheckBox.Checked;
                invoiceService.SortInvoices(sortBy.ToLower(), ascending);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please select a sort option.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
