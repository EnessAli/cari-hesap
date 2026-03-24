using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class TransactionForm : Form
    {
        public TransactionForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadTransactions();
        }

        private void InitializeUI()
        {
            this.Text = "Ödeme Yönetimi";
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create DataGridView
            DataGridView dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Add columns
            dgvTransactions.Columns.Add("Id", "ID");
            dgvTransactions.Columns.Add("TransactionDate", "Tarih");
            dgvTransactions.Columns.Add("CustomerName", "Müşteri");
            dgvTransactions.Columns.Add("Type", "Tür");
            dgvTransactions.Columns.Add("Amount", "Tutar");
            dgvTransactions.Columns.Add("PaymentMethod", "Ödeme Yöntemi");
            dgvTransactions.Columns.Add("Description", "Açıklama");

            // Create buttons panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            Button btnAdd = new Button
            {
                Text = "Yeni Ödeme",
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

            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });

            // Add controls to form
            this.Controls.Add(dgvTransactions);
            this.Controls.Add(buttonPanel);
        }

        private void LoadTransactions()
        {
            var transactions = Program.DbContext.Transactions
                .Where(t => !t.IsDeleted)
                .Include(t => t.Customer)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            DataGridView dgv = (DataGridView)this.Controls.OfType<DataGridView>().First();
            dgv.Rows.Clear();

            foreach (var transaction in transactions)
            {
                dgv.Rows.Add(
                    transaction.Id,
                    transaction.TransactionDate.ToShortDateString(),
                    transaction.Customer?.Name,
                    transaction.Type.ToString(),
                    transaction.Amount.ToString("N2"),
                    transaction.PaymentMethod.ToString(),
                    transaction.Description
                );
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // TODO: Implement add transaction functionality
            MessageBox.Show("Ödeme ekleme özelliği yakında eklenecek.");
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // TODO: Implement edit transaction functionality
            MessageBox.Show("Ödeme düzenleme özelliği yakında eklenecek.");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // TODO: Implement delete transaction functionality
            MessageBox.Show("Ödeme silme özelliği yakında eklenecek.");
        }
    }
} 