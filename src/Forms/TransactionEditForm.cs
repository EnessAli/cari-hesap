using System;
using System.Windows.Forms;
using System.Drawing;
using CariHesap.Models;
using System.Linq;

namespace CariHesap.Forms
{
    public partial class TransactionEditForm : Form
    {
        private readonly Customer _customer;
        private readonly TransactionType _transactionType;
        private readonly Transaction _transaction;
        private readonly bool _isNew;

        public TransactionEditForm(Customer customer, TransactionType transactionType, Transaction? transaction = null)
        {
            InitializeComponent();
            _customer = customer;
            _transactionType = transactionType;
            _transaction = transaction ?? new Transaction
            {
                CustomerId = customer.Id,
                TransactionDate = DateTime.Now,
                Type = transactionType,
                PaymentMethod = PaymentMethod.Cash
            };
            _isNew = transaction == null;
            InitializeUI();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TransactionEditForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Name = "TransactionEditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            string title = _transactionType == TransactionType.Payment 
                ? "Borç Ekle" 
                : "Tahsilat Yap";
            Color headerColor = _transactionType == TransactionType.Payment
                ? Color.FromArgb(245, 124, 0) // Turuncu (Borç)
                : Color.FromArgb(56, 142, 60); // Yeşil (Tahsilat)

            this.Text = $"{title} - {_customer.Name}";
            this.Size = new Size(450, 400);
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
                RowCount = 3,
                Padding = new Padding(15),
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 60),
                    new RowStyle(SizeType.Percent, 100),
                    new RowStyle(SizeType.Absolute, 50)
                }
            };

            // Başlık paneli
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = headerColor
            };

            Label lblTitle = new Label
            {
                Text = title.ToUpper(),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 15)
            };

            Label lblCustomer = new Label
            {
                Text = _customer.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 40)
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblCustomer);

            // Form paneli
            TableLayoutPanel formPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(0, 10, 0, 0),
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 30),
                    new ColumnStyle(SizeType.Percent, 70)
                }
            };

            // Tutar
            Label lblAmount = new Label
            {
                Text = "Tutar:",
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            NumericUpDown numAmount = new NumericUpDown
            {
                Name = "numAmount",
                Minimum = 0.01M,
                Maximum = 1000000M,
                DecimalPlaces = 2,
                Value = _transaction.Amount > 0 ? _transaction.Amount : 0.01M,
                Width = 200,
                TextAlign = HorizontalAlignment.Right,
                Margin = new Padding(0, 0, 0, 5)
            };

            // İşlem tarihi
            Label lblDate = new Label
            {
                Text = "Tarih:",
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            DateTimePicker dtpDate = new DateTimePicker
            {
                Name = "dtpDate",
                Format = DateTimePickerFormat.Short,
                Value = _transaction.TransactionDate,
                Width = 200,
                Margin = new Padding(0, 0, 0, 5)
            };

            // Ödeme yöntemi
            Label lblPaymentMethod = new Label
            {
                Text = "Ödeme Yöntemi:",
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            ComboBox cmbPaymentMethod = new ComboBox
            {
                Name = "cmbPaymentMethod",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Margin = new Padding(0, 0, 0, 5)
            };

            // Ödeme yöntemlerini ekle
            cmbPaymentMethod.Items.Add(new { Text = "Nakit", Value = PaymentMethod.Cash });
            cmbPaymentMethod.Items.Add(new { Text = "Kredi Kartı", Value = PaymentMethod.CreditCard });
            cmbPaymentMethod.Items.Add(new { Text = "Havale/EFT", Value = PaymentMethod.BankTransfer });
            cmbPaymentMethod.Items.Add(new { Text = "Çek", Value = PaymentMethod.Check });
            cmbPaymentMethod.DisplayMember = "Text";
            cmbPaymentMethod.ValueMember = "Value";
            cmbPaymentMethod.SelectedIndex = (int)_transaction.PaymentMethod - 1;

            // Açıklama
            Label lblDescription = new Label
            {
                Text = "Açıklama:",
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0)
            };

            TextBox txtDescription = new TextBox
            {
                Name = "txtDescription",
                Multiline = true,
                Height = 70,
                Width = 250,
                Text = _transaction.Description,
                Margin = new Padding(0, 5, 0, 0)
            };

            // Mevcut bakiye bilgisi
            Label lblCurrentBalance = new Label
            {
                Text = "Mevcut Bakiye:",
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            Label lblBalanceValue = new Label
            {
                Name = "lblBalanceValue",
                Text = $"{_customer.Balance:N2} ₺",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = _customer.Balance < 0 ? Color.FromArgb(211, 47, 47) : // Borç
                            _customer.Balance > 0 ? Color.FromArgb(56, 142, 60) : // Alacak
                            Color.FromArgb(66, 66, 66), // Sıfır bakiye
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            // Formları panel'e ekle
            formPanel.Controls.Add(lblAmount, 0, 0);
            formPanel.Controls.Add(numAmount, 1, 0);
            formPanel.Controls.Add(lblDate, 0, 1);
            formPanel.Controls.Add(dtpDate, 1, 1);
            formPanel.Controls.Add(lblPaymentMethod, 0, 2);
            formPanel.Controls.Add(cmbPaymentMethod, 1, 2);
            formPanel.Controls.Add(lblCurrentBalance, 0, 3);
            formPanel.Controls.Add(lblBalanceValue, 1, 3);
            formPanel.Controls.Add(lblDescription, 0, 4);
            formPanel.Controls.Add(txtDescription, 1, 4);

            // Buton paneli
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Button btnSave = new Button
            {
                Text = "Kaydet",
                Width = 100,
                Height = 35,
                Location = new Point(buttonPanel.Width - 220, 8),
                Anchor = AnchorStyles.Right,
                BackColor = headerColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button
            {
                Text = "İptal",
                Width = 100,
                Height = 35,
                Location = new Point(buttonPanel.Width - 110, 8),
                Anchor = AnchorStyles.Right,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };

            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);

            // Tüm panelleri ana layout'a ekle
            mainPanel.Controls.Add(headerPanel, 0, 0);
            mainPanel.Controls.Add(formPanel, 0, 1);
            mainPanel.Controls.Add(buttonPanel, 0, 2);

            this.Controls.Add(mainPanel);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void LoadData()
        {
            if (!_isNew)
            {
                var numAmount = (NumericUpDown)this.Controls.Find("numAmount", true)[0];
                var dtpDate = (DateTimePicker)this.Controls.Find("dtpDate", true)[0];
                var cmbPaymentMethod = (ComboBox)this.Controls.Find("cmbPaymentMethod", true)[0];
                var txtDescription = (TextBox)this.Controls.Find("txtDescription", true)[0];

                numAmount.Value = _transaction.Amount;
                dtpDate.Value = _transaction.TransactionDate;
                cmbPaymentMethod.SelectedIndex = (int)_transaction.PaymentMethod - 1;
                txtDescription.Text = _transaction.Description;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var numAmount = (NumericUpDown)this.Controls.Find("numAmount", true)[0];
                var dtpDate = (DateTimePicker)this.Controls.Find("dtpDate", true)[0];
                var cmbPaymentMethod = (ComboBox)this.Controls.Find("cmbPaymentMethod", true)[0];
                var txtDescription = (TextBox)this.Controls.Find("txtDescription", true)[0];

                // İşlem verilerini güncelle
                _transaction.Amount = numAmount.Value;
                _transaction.TransactionDate = dtpDate.Value;
                _transaction.PaymentMethod = (PaymentMethod)((dynamic)cmbPaymentMethod.SelectedItem).Value;
                _transaction.Description = txtDescription.Text;
                _transaction.UpdatedAt = DateTime.Now;

                if (_isNew)
                {
                    _transaction.CreatedAt = DateTime.Now;
                    Program.DbContext.Transactions.Add(_transaction);
                }

                // Müşteri bakiyesini güncelle
                // Ödeme (Borç) işlemi: Bakiyeyi azalt
                // Tahsilat işlemi: Bakiyeyi artır
                if (_transactionType == TransactionType.Payment)
                {
                    _customer.Balance -= _transaction.Amount;
                }
                else
                {
                    _customer.Balance += _transaction.Amount;
                }
                _customer.UpdatedAt = DateTime.Now;

                Program.DbContext.SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem kaydedilirken bir hata oluştu: {ex.Message}", "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
} 