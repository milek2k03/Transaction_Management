using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using InvoiceApp.Services;

namespace InvoiceApp.UI
{
    public class MainForm : Form
    {
        private User currentUser;
        private InvoiceService invoiceService;
        private ListView invoiceListView;
        private Button addInvoiceButton;
        private Button sendEmailButton;
        private Button sortButton;

        public MainForm(User user)
        {
            currentUser = user;
            invoiceService = new InvoiceService(user);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = $"Invoice Management - Logged in as {currentUser.Email}";
            Size = new System.Drawing.Size(800, 600);

            invoiceListView = new ListView
            {
                Dock = DockStyle.Top,
                View = View.Details,
                FullRowSelect = true,
                Height = 400
            };
            invoiceListView.Columns.Add("ID", 50);
            invoiceListView.Columns.Add("Client Name", 150);
            invoiceListView.Columns.Add("Amount", 100);
            invoiceListView.Columns.Add("Issue Date", 100);
            invoiceListView.Columns.Add("Due Date", 100);

            addInvoiceButton = new Button
            {
                Text = "Add Invoice",
                Dock = DockStyle.Bottom
            };
            addInvoiceButton.Click += AddInvoiceButton_Click;

            var editInvoiceButton = new Button
            {
                Text = "Edit Invoice",
                Dock = DockStyle.Bottom
            };
            editInvoiceButton.Click += EditInvoiceButton_Click;
            Controls.Add(editInvoiceButton);


            sortButton = new Button
            {
                Text = "Sort Invoices",
                Dock = DockStyle.Bottom
            };
            sortButton.Click += SortButton_Click;

            var downloadPdfButton = new Button
            {
                Text = "Download PDF",
                Dock = DockStyle.Bottom
            };
            downloadPdfButton.Click += DownloadPdfButton_Click;
            Controls.Add(downloadPdfButton);


            Controls.Add(invoiceListView);
            Controls.Add(addInvoiceButton);
            Controls.Add(sendEmailButton);
            Controls.Add(sortButton);
            RefreshInvoiceList();
        }

        private void AddInvoiceButton_Click(object sender, EventArgs e)
        {
            using (var addInvoiceForm = new AddInvoiceForm(invoiceService))
            {
                if (addInvoiceForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshInvoiceList();
                }
            }
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            using (var sortForm = new SortForm(invoiceService))
            {
                if (sortForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshInvoiceList();
                }
            }
        }

        private void RefreshInvoiceList()
        {
            invoiceListView.Items.Clear();
            var sortedInvoices = invoiceService.GetInvoices(); // Pobiera posortowane faktury

            foreach (var invoice in sortedInvoices)
            {
                var item = new ListViewItem(invoice.InvoiceId.ToString());
                item.SubItems.Add(invoice.ClientName);
                item.SubItems.Add(invoice.Amount.ToString("C"));
                item.SubItems.Add(invoice.IssueDate.ToString("yyyy-MM-dd"));
                item.SubItems.Add(invoice.DueDate.ToString("yyyy-MM-dd"));
                invoiceListView.Items.Add(item);
            }
        }


        private void EditInvoiceButton_Click(object sender, EventArgs e)
        {
            if (invoiceListView.SelectedItems.Count > 0)
            {
                var selectedItem = invoiceListView.SelectedItems[0];
                if (int.TryParse(selectedItem.SubItems[0].Text, out int invoiceId))
                {
                    var invoice = invoiceService.GetInvoice(invoiceId);
                    using (var editForm = new EditInvoiceForm(invoice, invoiceService))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            RefreshInvoiceList();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid invoice ID selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an invoice to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void DownloadPdfButton_Click(object sender, EventArgs e)
        {
            if (invoiceListView.SelectedItems.Count > 0)
            {
                var selectedItem = invoiceListView.SelectedItems[0];
                if (int.TryParse(selectedItem.SubItems[0].Text, out int invoiceId))
                {
                    var invoice = invoiceService.GetInvoice(invoiceId);
                    try
                    {
                        invoiceService.GenerateInvoicePdf(invoice);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error generating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid invoice ID selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an invoice to download.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
