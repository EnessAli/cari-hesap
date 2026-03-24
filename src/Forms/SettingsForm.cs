using System;
using System.Windows.Forms;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Ayarlar";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create settings panel
            TableLayoutPanel settingsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                ColumnCount = 2,
                RowCount = 5
            };

            // Company Information
            Label lblCompanyName = new Label { Text = "Firma Adı:" };
            TextBox txtCompanyName = new TextBox { Width = 300 };

            Label lblTaxNumber = new Label { Text = "Vergi Numarası:" };
            TextBox txtTaxNumber = new TextBox { Width = 300 };

            Label lblTaxOffice = new Label { Text = "Vergi Dairesi:" };
            TextBox txtTaxOffice = new TextBox { Width = 300 };

            Label lblAddress = new Label { Text = "Adres:" };
            TextBox txtAddress = new TextBox { Width = 300, Height = 60, Multiline = true };

            // Add controls to panel
            settingsPanel.Controls.Add(lblCompanyName, 0, 0);
            settingsPanel.Controls.Add(txtCompanyName, 1, 0);
            settingsPanel.Controls.Add(lblTaxNumber, 0, 1);
            settingsPanel.Controls.Add(txtTaxNumber, 1, 1);
            settingsPanel.Controls.Add(lblTaxOffice, 0, 2);
            settingsPanel.Controls.Add(txtTaxOffice, 1, 2);
            settingsPanel.Controls.Add(lblAddress, 0, 3);
            settingsPanel.Controls.Add(txtAddress, 1, 3);

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
                Width = 100
            };
            btnSave.Click += (s, e) => SaveSettings();

            Button btnCancel = new Button
            {
                Text = "İptal",
                Location = new System.Drawing.Point(120, 10),
                Width = 100
            };
            btnCancel.Click += (s, e) => this.Close();

            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnCancel });

            // Add panels to form
            this.Controls.Add(settingsPanel);
            this.Controls.Add(buttonPanel);
        }

        private void SaveSettings()
        {
            // TODO: Implement settings save functionality
            MessageBox.Show("Ayarlar kaydedildi.");
            this.Close();
        }
    }
} 