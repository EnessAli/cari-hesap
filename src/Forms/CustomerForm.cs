using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class CustomerForm : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private Button btnAdd, btnEdit, btnDelete, btnAddDebt, btnAddPayment, btnRefresh, btnDetails;
        private System.Windows.Forms.Timer searchTimer;

        public CustomerForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadCustomers();
        }

        private void InitializeUI()
        {
            this.Text = "Müşteri Yönetimi";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Ana panel
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10),
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 50), // Üst araç çubuğu
                    new RowStyle(SizeType.Absolute, 70), // Butonlar
                    new RowStyle(SizeType.Percent, 100) // DataGridView
                }
            };

            // Üst araç çubuğu - Başlık ve Arama
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitle = new Label
            {
                Text = "MÜŞTERİ YÖNETİMİ",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(10, 12)
            };

            Panel searchPanel = new Panel
            {
                Width = 250,
                Height = 36,
                Location = new Point(topPanel.Width - 260, 7),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            txtSearch = new TextBox
            {
                Width = 220,
                Height = 30,
                Location = new Point(5, 4),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Müşteri Ara..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Label searchIcon = new Label
            {
                Text = "🔍",
                Location = new Point(225, 6),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(searchIcon);
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(searchPanel);

            // Buton Paneli
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            // Stil tanımları
            Color primaryButtonColor = Color.FromArgb(25, 118, 210);
            Color secondaryButtonColor = Color.FromArgb(66, 66, 66);
            Color dangerButtonColor = Color.FromArgb(211, 47, 47);
            Color successButtonColor = Color.FromArgb(56, 142, 60);
            Color warningButtonColor = Color.FromArgb(245, 124, 0);

            Font buttonFont = new Font("Segoe UI", 9F, FontStyle.Regular);

            // İşlem Butonları
            btnAdd = CreateButton("Yeni Müşteri", "➕", primaryButtonColor, buttonFont);
            btnEdit = CreateButton("Düzenle", "✏️", secondaryButtonColor, buttonFont);
            btnDelete = CreateButton("Sil", "🗑️", dangerButtonColor, buttonFont);
            btnAddDebt = CreateButton("Borç Ekle", "💰", warningButtonColor, buttonFont);
            btnAddPayment = CreateButton("Tahsilat Yap", "💸", successButtonColor, buttonFont);
            btnDetails = CreateButton("Detaylar", "📋", Color.FromArgb(121, 85, 72), buttonFont);
            btnRefresh = CreateButton("Yenile", "🔄", Color.FromArgb(0, 137, 123), buttonFont);

            // Butonların konumları
            btnAdd.Location = new Point(10, 10);
            btnEdit.Location = new Point(130, 10);
            btnDelete.Location = new Point(250, 10);
            btnAddDebt.Location = new Point(370, 10);
            btnAddPayment.Location = new Point(490, 10);
            btnDetails.Location = new Point(610, 10);
            btnRefresh.Location = new Point(730, 10);

            // Click olayları
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnAddDebt.Click += BtnAddDebt_Click;
            btnAddPayment.Click += BtnAddPayment_Click;
            btnDetails.Click += BtnDetails_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Butonları panele ekle
            buttonPanel.Controls.AddRange(new Control[] { 
                btnAdd, btnEdit, btnDelete, btnAddDebt, btnAddPayment, btnDetails, btnRefresh 
            });

            // DataGridView
            dgvCustomers = new DataGridView
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

            // DataGridView stil ayarları
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 255);
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvCustomers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.ColumnHeadersHeight = 40;
            dgvCustomers.RowTemplate.Height = 35;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Sütunlar
            dgvCustomers.Columns.Add("Id", "ID");
            dgvCustomers.Columns.Add("Name", "Müşteri Adı");
            dgvCustomers.Columns.Add("TaxNumber", "Vergi No");
            dgvCustomers.Columns.Add("Phone", "Telefon");
            dgvCustomers.Columns.Add("Email", "E-posta");
            dgvCustomers.Columns.Add("Balance", "Bakiye Durumu");

            // Sütun genişlikleri
            dgvCustomers.Columns["Id"].Width = 50;
            dgvCustomers.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvCustomers.Columns["Name"].FillWeight = 25;
            dgvCustomers.Columns["TaxNumber"].FillWeight = 15;
            dgvCustomers.Columns["Phone"].FillWeight = 15;
            dgvCustomers.Columns["Email"].FillWeight = 20;
            dgvCustomers.Columns["Balance"].FillWeight = 15;
            dgvCustomers.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Hücre stil olayı
            dgvCustomers.CellFormatting += DgvCustomers_CellFormatting;

            // Tüm panelleri ana layout'a ekle
            mainPanel.Controls.Add(topPanel, 0, 0);
            mainPanel.Controls.Add(buttonPanel, 0, 1);
            mainPanel.Controls.Add(dgvCustomers, 0, 2);

            // Ana layout'u forma ekle
            this.Controls.Add(mainPanel);

            // Arama timer'ı (performans için debounce)
            searchTimer = new System.Windows.Forms.Timer();
            searchTimer.Interval = 300;
            searchTimer.Tick += SearchTimer_Tick;
        }

        private Button CreateButton(string text, string icon, Color backColor, Font font)
        {
            Button btn = new Button
            {
                Text = $"{icon} {text}",
                Width = 120,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = font,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            return btn;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Debounce için timer'ı yeniden başlat
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            LoadCustomers(txtSearch.Text);
        }

        private void DgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvCustomers.Columns["Balance"].Index && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString().Replace(".", ","), out decimal balance))
                {
                    // Bakiye negatifse (müşteri borçluysa) kırmızı, pozitifse (müşteri alacaklıysa) yeşil göster
                    if (balance < 0)
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(211, 47, 47); // Kırmızı
                        e.Value = $"{balance:N2} ₺ (Borç)";
                    }
                    else if (balance > 0)
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(56, 142, 60); // Yeşil
                        e.Value = $"{balance:N2} ₺ (Alacak)";
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(66, 66, 66); // Gri
                        e.Value = $"{balance:N2} ₺";
                    }
                }
            }
        }

        private void LoadCustomers(string searchText = "")
        {
            try
            {
                var query = Program.DbContext.Customers
                    .Where(c => !c.IsDeleted);

                // Arama filtresini uygula
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLower();
                    query = query.Where(c => 
                        c.Name.ToLower().Contains(searchText) || 
                        (c.TaxNumber != null && c.TaxNumber.ToLower().Contains(searchText)) ||
                        (c.Phone != null && c.Phone.ToLower().Contains(searchText)) ||
                        (c.Email != null && c.Email.ToLower().Contains(searchText))
                    );
                }

                // Müşterileri adlarına göre sırala
                var customers = query.OrderBy(c => c.Name).ToList();

                dgvCustomers.Rows.Clear();

                foreach (var customer in customers)
                {
                    dgvCustomers.Rows.Add(
                        customer.Id,
                        customer.Name,
                        customer.TaxNumber,
                        customer.Phone,
                        customer.Email,
                        customer.Balance
                    );
                }

                // Buton durumlarını güncelle
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteri bilgileri yüklenirken bir hata oluştu: {ex.Message}", "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = dgvCustomers.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnAddDebt.Enabled = hasSelection;
            btnAddPayment.Enabled = hasSelection;
            btnDetails.Enabled = hasSelection;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new CustomerEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlenecek müşteriyi seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
            var customer = Program.DbContext.Customers.Find(customerId);

            if (customer != null)
            {
                using (var form = new CustomerEditForm(customer))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadCustomers();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek müşteriyi seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
            var customer = Program.DbContext.Customers.Find(customerId);

            if (customer != null)
            {
                var result = MessageBox.Show(
                    $"{customer.Name} müşterisini silmek istediğinizden emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        customer.IsDeleted = true;
                        customer.UpdatedAt = DateTime.Now;
                        Program.DbContext.SaveChanges();
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Silme işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAddDebt_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen borç eklenecek müşteriyi seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
            var customer = Program.DbContext.Customers.Find(customerId);

            if (customer != null)
            {
                using (var form = new TransactionEditForm(customer, TransactionType.Payment))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadCustomers();
                    }
                }
            }
        }

        private void BtnAddPayment_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen tahsilat yapılacak müşteriyi seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
            var customer = Program.DbContext.Customers.Find(customerId);

            if (customer != null)
            {
                using (var form = new TransactionEditForm(customer, TransactionType.Receipt))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadCustomers();
                    }
                }
            }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen detayları görüntülenecek müşteriyi seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
            var customer = Program.DbContext.Customers
                .Include(c => c.Invoices)
                .Include(c => c.Transactions)
                .FirstOrDefault(c => c.Id == customerId);

            if (customer != null)
            {
                using (var form = new CustomerDetailForm(customer))
                {
                    form.ShowDialog();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomers();
        }
    }
} 