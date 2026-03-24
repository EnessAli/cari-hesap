using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class InvoiceForm : Form
    {
        public InvoiceForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadInvoices();
        }

        private void InitializeUI()
        {
            this.Text = "Fatura Yönetimi";
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create DataGridView
            DataGridView dgvInvoices = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Add columns
            dgvInvoices.Columns.Add("Id", "ID");
            dgvInvoices.Columns.Add("InvoiceNumber", "Fatura No");
            dgvInvoices.Columns.Add("InvoiceDate", "Fatura Tarihi");
            dgvInvoices.Columns.Add("CustomerName", "Müşteri");
            dgvInvoices.Columns.Add("SubTotal", "Ara Toplam");
            dgvInvoices.Columns.Add("VAT", "KDV");
            dgvInvoices.Columns.Add("Discount", "İndirim");
            dgvInvoices.Columns.Add("Total", "Toplam");

            // Create buttons panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            Button btnAdd = new Button
            {
                Text = "Yeni Fatura",
                Location = new System.Drawing.Point(10, 10),
                Width = 100
            };
            btnAdd.Click += BtnAdd_Click;

            Button btnEdit = new Button
            {
                Text = "Düzenle",
                Location = new System.Drawing.Point(120, 10),
                Width = 100
            };
            btnEdit.Click += BtnEdit_Click;

            Button btnDelete = new Button
            {
                Text = "Sil",
                Location = new System.Drawing.Point(230, 10),
                Width = 100
            };
            btnDelete.Click += BtnDelete_Click;

            Button btnPrint = new Button
            {
                Text = "Yazdır",
                Location = new System.Drawing.Point(340, 10),
                Width = 100
            };
            btnPrint.Click += BtnPrint_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnPrint });

            // Add controls to form
            this.Controls.Add(dgvInvoices);
            this.Controls.Add(buttonPanel);
        }

        private void LoadInvoices()
        {
            var invoices = Program.DbContext.Invoices
                .Where(i => !i.IsDeleted)
                .Include(i => i.Customer)
                .OrderByDescending(i => i.InvoiceDate)
                .ToList();

            DataGridView dgv = (DataGridView)this.Controls.OfType<DataGridView>().First();
            dgv.Rows.Clear();

            foreach (var invoice in invoices)
            {
                dgv.Rows.Add(
                    invoice.Id,
                    invoice.InvoiceNumber,
                    invoice.InvoiceDate.ToShortDateString(),
                    invoice.Customer?.Name,
                    invoice.SubTotal.ToString("N2"),
                    invoice.VAT.ToString("N2"),
                    invoice.Discount.ToString("N2"),
                    invoice.Total.ToString("N2")
                );
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // TODO: Implement add invoice functionality
            MessageBox.Show("Fatura ekleme özelliği yakında eklenecek.");
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // TODO: Implement edit invoice functionality
            MessageBox.Show("Fatura düzenleme özelliği yakında eklenecek.");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // TODO: Implement delete invoice functionality
            MessageBox.Show("Fatura silme özelliği yakında eklenecek.");
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            // TODO: Implement print invoice functionality
            MessageBox.Show("Fatura yazdırma özelliği yakında eklenecek.");
        }
    }
} 