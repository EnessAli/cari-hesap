using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class CustomerDetailForm : Form
    {
        private readonly Customer _customer;
        private TabControl tabControl;
        private DataGridView dgvInvoices;
        private DataGridView dgvTransactions;

        public CustomerDetailForm(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            InitializeUI();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomerDetailForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Name = "CustomerDetailForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            this.Text = $"Müşteri Detayları - {_customer.Name}";
            this.Size = new Size(900, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9F);

            // Ana panel
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(15),
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 150), // Üst bilgi (müşteri detayları)
                    new RowStyle(SizeType.Percent, 100)   // Tab paneli (faturalar ve işlemler)
                }
            };

            // Müşteri detay paneli
            Panel customerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Başlık
            Label lblCustomerName = new Label
            {
                Text = _customer.Name,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            // Bakiye
            string balanceText = $"Bakiye: {_customer.Balance:N2} ₺";
            string balanceStatus = _customer.Balance < 0 ? " (Borçlu)" : _customer.Balance > 0 ? " (Alacaklı)" : "";
            Label lblBalance = new Label
            {
                Text = balanceText + balanceStatus,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = _customer.Balance < 0 ? Color.FromArgb(211, 47, 47) : // Borç
                           _customer.Balance > 0 ? Color.FromArgb(56, 142, 60) : // Alacak
                           Color.FromArgb(66, 66, 66), // Sıfır bakiye
                AutoSize = true,
                Location = new Point(customerPanel.Width - 200, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Müşteri detayları - Sol Panel
            FlowLayoutPanel leftDetailsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Width = 400,
                Height = 80,
                Location = new Point(15, 50),
                WrapContents = false
            };

            // Sağ Panel
            FlowLayoutPanel rightDetailsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Width = 400,
                Height = 80,
                Location = new Point(420, 50),
                WrapContents = false
            };

            // Sol detaylar
            AddDetailLabel(leftDetailsPanel, "Vergi No:", _customer.TaxNumber ?? "-");
            AddDetailLabel(leftDetailsPanel, "Vergi Dairesi:", _customer.TaxOffice ?? "-");
            AddDetailLabel(leftDetailsPanel, "Telefon:", _customer.Phone ?? "-");

            // Sağ detaylar
            AddDetailLabel(rightDetailsPanel, "E-posta:", _customer.Email ?? "-");
            AddDetailLabel(rightDetailsPanel, "Adres:", _customer.Address ?? "-");
            AddDetailLabel(rightDetailsPanel, "Kayıt Tarihi:", _customer.CreatedAt.ToShortDateString());

            // Panelleri ekle
            customerPanel.Controls.Add(lblCustomerName);
            customerPanel.Controls.Add(lblBalance);
            customerPanel.Controls.Add(leftDetailsPanel);
            customerPanel.Controls.Add(rightDetailsPanel);

            // Tab kontrolü
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            // Faturalar sekmesi
            TabPage tabInvoices = new TabPage("Faturalar");
            tabInvoices.BackColor = Color.White;
            tabInvoices.Padding = new Padding(10);

            // İşlemler sekmesi
            TabPage tabTransactions = new TabPage("İşlemler");
            tabTransactions.BackColor = Color.White;
            tabTransactions.Padding = new Padding(10);

            // Faturalar DataGridView
            dgvInvoices = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            // DataGridView stil ayarları - Faturalar
            SetDataGridViewStyle(dgvInvoices);

            // Faturalar sütunları
            dgvInvoices.Columns.Add("Id", "ID");
            dgvInvoices.Columns.Add("InvoiceNumber", "Fatura No");
            dgvInvoices.Columns.Add("InvoiceDate", "Tarih");
            dgvInvoices.Columns.Add("Total", "Tutar");
            dgvInvoices.Columns.Add("Status", "Durum");

            // Sütun genişlikleri
            dgvInvoices.Columns["Id"].Width = 50;
            dgvInvoices.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvInvoices.Columns["InvoiceNumber"].FillWeight = 20;
            dgvInvoices.Columns["InvoiceDate"].FillWeight = 20;
            dgvInvoices.Columns["Total"].FillWeight = 20;
            dgvInvoices.Columns["Status"].FillWeight = 20;

            // İşlemler DataGridView
            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            // DataGridView stil ayarları - İşlemler
            SetDataGridViewStyle(dgvTransactions);

            // İşlemler sütunları
            dgvTransactions.Columns.Add("Id", "ID");
            dgvTransactions.Columns.Add("TransactionDate", "Tarih");
            dgvTransactions.Columns.Add("Type", "Tür");
            dgvTransactions.Columns.Add("Amount", "Tutar");
            dgvTransactions.Columns.Add("PaymentMethod", "Ödeme Şekli");
            dgvTransactions.Columns.Add("Description", "Açıklama");

            // Sütun genişlikleri
            dgvTransactions.Columns["Id"].Width = 50;
            dgvTransactions.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTransactions.Columns["TransactionDate"].FillWeight = 15;
            dgvTransactions.Columns["Type"].FillWeight = 15;
            dgvTransactions.Columns["Amount"].FillWeight = 15;
            dgvTransactions.Columns["PaymentMethod"].FillWeight = 15;
            dgvTransactions.Columns["Description"].FillWeight = 25;

            // CellFormatting olayları
            dgvInvoices.CellFormatting += DgvInvoices_CellFormatting;
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;

            // DataGridView'leri sekmelere ekle
            tabInvoices.Controls.Add(dgvInvoices);
            tabTransactions.Controls.Add(dgvTransactions);

            // Sekmeleri tab kontrolüne ekle
            tabControl.TabPages.Add(tabInvoices);
            tabControl.TabPages.Add(tabTransactions);

            // Ana panele ekle
            mainPanel.Controls.Add(customerPanel, 0, 0);
            mainPanel.Controls.Add(tabControl, 0, 1);

            // Forma ekle
            this.Controls.Add(mainPanel);

            // Kapat butonu
            Button btnClose = new Button
            {
                Text = "Kapat",
                Width = 100,
                Height = 35,
                Location = new Point(this.ClientSize.Width - 115, this.ClientSize.Height - 50),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(66, 66, 66),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(btnClose);
            this.CancelButton = btnClose;
        }

        private void AddDetailLabel(FlowLayoutPanel panel, string title, string value)
        {
            Panel detailPanel = new Panel
            {
                Width = panel.Width - 10,
                Height = 25,
                Margin = new Padding(0, 0, 0, 3)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(66, 66, 66),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(66, 66, 66),
                AutoSize = true,
                Location = new Point(120, 0)
            };

            detailPanel.Controls.Add(lblTitle);
            detailPanel.Controls.Add(lblValue);
            panel.Controls.Add(detailPanel);
        }

        private void SetDataGridViewStyle(DataGridView dgv)
        {
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.RowTemplate.Height = 35;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void LoadData()
        {
            try
            {
                // Fatura verilerini yükle
                var invoices = Program.DbContext.Invoices
                    .Where(i => i.CustomerId == _customer.Id && !i.IsDeleted)
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToList();

                dgvInvoices.Rows.Clear();
                foreach (var invoice in invoices)
                {
                    // Fatura durumu: Ödendi mi, kısmen ödendi mi, ödenmedi mi?
                    string status = "Ödenmedi";
                    decimal totalPayments = Program.DbContext.Transactions
                        .Where(t => t.InvoiceId == invoice.Id && t.Type == TransactionType.Receipt)
                        .Sum(t => t.Amount);

                    if (totalPayments >= invoice.Total)
                        status = "Ödendi";
                    else if (totalPayments > 0)
                        status = "Kısmen Ödendi";

                    dgvInvoices.Rows.Add(
                        invoice.Id,
                        invoice.InvoiceNumber,
                        invoice.InvoiceDate.ToShortDateString(),
                        invoice.Total.ToString("N2") + " ₺",
                        status
                    );
                }

                // İşlem verilerini yükle
                var transactions = Program.DbContext.Transactions
                    .Where(t => t.CustomerId == _customer.Id && !t.IsDeleted)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToList();

                dgvTransactions.Rows.Clear();
                foreach (var transaction in transactions)
                {
                    string transactionType = transaction.Type == TransactionType.Payment ? "Borç" : "Tahsilat";
                    string paymentMethod = "";

                    switch (transaction.PaymentMethod)
                    {
                        case PaymentMethod.Cash:
                            paymentMethod = "Nakit";
                            break;
                        case PaymentMethod.CreditCard:
                            paymentMethod = "Kredi Kartı";
                            break;
                        case PaymentMethod.BankTransfer:
                            paymentMethod = "Havale/EFT";
                            break;
                        case PaymentMethod.Check:
                            paymentMethod = "Çek";
                            break;
                    }

                    dgvTransactions.Rows.Add(
                        transaction.Id,
                        transaction.TransactionDate.ToShortDateString(),
                        transactionType,
                        transaction.Amount.ToString("N2") + " ₺",
                        paymentMethod,
                        transaction.Description
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri yüklenirken bir hata oluştu: {ex.Message}", "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvInvoices_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvInvoices.Columns["Status"].Index && e.Value != null)
            {
                string status = e.Value.ToString();
                
                if (status == "Ödendi")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(56, 142, 60); // Yeşil
                }
                else if (status == "Kısmen Ödendi")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(245, 124, 0); // Turuncu
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(211, 47, 47); // Kırmızı
                }
            }
        }

        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvTransactions.Columns["Type"].Index && e.Value != null)
            {
                string type = e.Value.ToString();
                
                if (type == "Borç")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(211, 47, 47); // Kırmızı
                }
                else if (type == "Tahsilat")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(56, 142, 60); // Yeşil
                }
            }
        }
    }
} 