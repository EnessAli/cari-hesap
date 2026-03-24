using System;
using System.Windows.Forms;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class CustomerEditForm : Form
    {
        private readonly Customer _customer;
        private readonly bool _isNew;

        public CustomerEditForm(Customer customer = null)
        {
            InitializeComponent();
            _customer = customer ?? new Customer();
            _isNew = customer == null;
            InitializeUI();
            LoadCustomerData();
        }

        private void InitializeUI()
        {
            this.Text = _isNew ? "Yeni Müşteri" : "Müşteri Düzenle";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create main panel
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                ColumnCount = 2,
                RowCount = 7
            };

            // Customer Information
            Label lblName = new Label { Text = "Müşteri Adı:", Anchor = AnchorStyles.Left };
            TextBox txtName = new TextBox { Width = 300, Name = "txtName" };

            Label lblTaxNumber = new Label { Text = "Vergi Numarası:", Anchor = AnchorStyles.Left };
            TextBox txtTaxNumber = new TextBox { Width = 300, Name = "txtTaxNumber" };

            Label lblTaxOffice = new Label { Text = "Vergi Dairesi:", Anchor = AnchorStyles.Left };
            TextBox txtTaxOffice = new TextBox { Width = 300, Name = "txtTaxOffice" };

            Label lblPhone = new Label { Text = "Telefon:", Anchor = AnchorStyles.Left };
            TextBox txtPhone = new TextBox { Width = 300, Name = "txtPhone" };

            Label lblEmail = new Label { Text = "E-posta:", Anchor = AnchorStyles.Left };
            TextBox txtEmail = new TextBox { Width = 300, Name = "txtEmail" };

            Label lblAddress = new Label { Text = "Adres:", Anchor = AnchorStyles.Left };
            TextBox txtAddress = new TextBox { Width = 300, Height = 60, Multiline = true, Name = "txtAddress" };

            // Add controls to panel
            mainPanel.Controls.Add(lblName, 0, 0);
            mainPanel.Controls.Add(txtName, 1, 0);
            mainPanel.Controls.Add(lblTaxNumber, 0, 1);
            mainPanel.Controls.Add(txtTaxNumber, 1, 1);
            mainPanel.Controls.Add(lblTaxOffice, 0, 2);
            mainPanel.Controls.Add(txtTaxOffice, 1, 2);
            mainPanel.Controls.Add(lblPhone, 0, 3);
            mainPanel.Controls.Add(txtPhone, 1, 3);
            mainPanel.Controls.Add(lblEmail, 0, 4);
            mainPanel.Controls.Add(txtEmail, 1, 4);
            mainPanel.Controls.Add(lblAddress, 0, 5);
            mainPanel.Controls.Add(txtAddress, 1, 5);

            // Buttons panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            Button btnSave = new Button
            {
                Text = "Kaydet",
                Location = new System.Drawing.Point(10, 10),
                Width = 100,
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button
            {
                Text = "İptal",
                Location = new System.Drawing.Point(120, 10),
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnCancel });

            // Add panels to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);
        }

        private void LoadCustomerData()
        {
            if (!_isNew)
            {
                var txtName = (TextBox)this.Controls.Find("txtName", true)[0];
                var txtTaxNumber = (TextBox)this.Controls.Find("txtTaxNumber", true)[0];
                var txtTaxOffice = (TextBox)this.Controls.Find("txtTaxOffice", true)[0];
                var txtPhone = (TextBox)this.Controls.Find("txtPhone", true)[0];
                var txtEmail = (TextBox)this.Controls.Find("txtEmail", true)[0];
                var txtAddress = (TextBox)this.Controls.Find("txtAddress", true)[0];

                txtName.Text = _customer.Name;
                txtTaxNumber.Text = _customer.TaxNumber;
                txtTaxOffice.Text = _customer.TaxOffice;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
                txtAddress.Text = _customer.Address;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var txtName = (TextBox)this.Controls.Find("txtName", true)[0];
            var txtTaxNumber = (TextBox)this.Controls.Find("txtTaxNumber", true)[0];
            var txtTaxOffice = (TextBox)this.Controls.Find("txtTaxOffice", true)[0];
            var txtPhone = (TextBox)this.Controls.Find("txtPhone", true)[0];
            var txtEmail = (TextBox)this.Controls.Find("txtEmail", true)[0];
            var txtAddress = (TextBox)this.Controls.Find("txtAddress", true)[0];

            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Müşteri adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            // Update customer data
            _customer.Name = txtName.Text.Trim();
            _customer.TaxNumber = txtTaxNumber.Text.Trim();
            _customer.TaxOffice = txtTaxOffice.Text.Trim();
            _customer.Phone = txtPhone.Text.Trim();
            _customer.Email = txtEmail.Text.Trim();
            _customer.Address = txtAddress.Text.Trim();
            _customer.UpdatedAt = DateTime.Now;

            try
            {
                if (_isNew)
                {
                    Program.DbContext.Customers.Add(_customer);
                }
                Program.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
} 