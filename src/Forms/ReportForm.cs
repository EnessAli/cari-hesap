using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using CariHesap.Models;
using Microsoft.EntityFrameworkCore;

namespace CariHesap.Forms
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Raporlar";
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create filter panel
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100
            };

            // Date range controls
            Label lblStartDate = new Label
            {
                Text = "Başlangıç Tarihi:",
                Location = new System.Drawing.Point(10, 15),
                AutoSize = true
            };

            DateTimePicker dtpStartDate = new DateTimePicker
            {
                Location = new System.Drawing.Point(120, 12),
                Width = 150
            };

            Label lblEndDate = new Label
            {
                Text = "Bitiş Tarihi:",
                Location = new System.Drawing.Point(290, 15),
                AutoSize = true
            };

            DateTimePicker dtpEndDate = new DateTimePicker
            {
                Location = new System.Drawing.Point(380, 12),
                Width = 150
            };

            // Report type selection
            Label lblReportType = new Label
            {
                Text = "Rapor Türü:",
                Location = new System.Drawing.Point(10, 50),
                AutoSize = true
            };

            ComboBox cmbReportType = new ComboBox
            {
                Location = new System.Drawing.Point(120, 47),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbReportType.Items.AddRange(new string[] {
                "Gelir/Gider Raporu",
                "Müşteri Bakiyeleri",
                "Vadesi Geçmiş Ödemeler",
                "KDV Raporu"
            });
            cmbReportType.SelectedIndex = 0;

            // Generate button
            Button btnGenerate = new Button
            {
                Text = "Rapor Oluştur",
                Location = new System.Drawing.Point(290, 45),
                Width = 100
            };
            btnGenerate.Click += (s, e) => GenerateReport(dtpStartDate.Value, dtpEndDate.Value, cmbReportType.SelectedItem.ToString());

            // Export button
            Button btnExport = new Button
            {
                Text = "Excel'e Aktar",
                Location = new System.Drawing.Point(400, 45),
                Width = 100
            };
            btnExport.Click += (s, e) => ExportToExcel();

            filterPanel.Controls.AddRange(new Control[] {
                lblStartDate, dtpStartDate,
                lblEndDate, dtpEndDate,
                lblReportType, cmbReportType,
                btnGenerate, btnExport
            });

            // Create report view panel
            Panel reportPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Add controls to form
            this.Controls.Add(reportPanel);
            this.Controls.Add(filterPanel);
        }

        private void GenerateReport(DateTime startDate, DateTime endDate, string reportType)
        {
            // TODO: Implement report generation functionality
            MessageBox.Show($"{reportType} raporu {startDate.ToShortDateString()} - {endDate.ToShortDateString()} tarihleri arasında oluşturulacak.");
        }

        private void ExportToExcel()
        {
            // TODO: Implement Excel export functionality
            MessageBox.Show("Excel'e aktarma özelliği yakında eklenecek.");
        }
    }
} 