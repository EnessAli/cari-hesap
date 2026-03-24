using System;
using System.Windows.Forms;
using CariHesap.Forms;

namespace CariHesap
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Cari Hesap - İşletme Yönetim Sistemi";
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create main menu
            MenuStrip mainMenu = new MenuStrip();
            this.MainMenuStrip = mainMenu;
            this.Controls.Add(mainMenu);

            // File Menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Dosya");
            fileMenu.DropDownItems.Add("Yedekle", null, (s, e) => BackupDatabase());
            fileMenu.DropDownItems.Add("Geri Yükle", null, (s, e) => RestoreDatabase());
            fileMenu.DropDownItems.Add("-");
            fileMenu.DropDownItems.Add("Çıkış", null, (s, e) => Application.Exit());
            mainMenu.Items.Add(fileMenu);

            // Create navigation buttons
            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 200,
                Padding = new Padding(10)
            };

            string[] buttonTexts = new[]
            {
                "Müşteriler",
                "Faturalar",
                "Ödemeler",
                "Raporlar",
                "Ayarlar"
            };

            foreach (string text in buttonTexts)
            {
                Button btn = new Button
                {
                    Text = text,
                    Dock = DockStyle.Top,
                    Height = 40,
                    Margin = new Padding(0, 5, 0, 5)
                };
                btn.Click += NavigationButton_Click;
                buttonPanel.Controls.Add(btn);
            }

            this.Controls.Add(buttonPanel);

            // Create content panel
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            this.Controls.Add(contentPanel);
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Form formToShow = null;

            switch (clickedButton.Text)
            {
                case "Müşteriler":
                    formToShow = new CustomerForm();
                    break;
                case "Faturalar":
                    formToShow = new InvoiceForm();
                    break;
                case "Ödemeler":
                    formToShow = new TransactionForm();
                    break;
                case "Raporlar":
                    formToShow = new ReportForm();
                    break;
                case "Ayarlar":
                    formToShow = new SettingsForm();
                    break;
            }

            if (formToShow != null)
            {
                formToShow.ShowDialog();
            }
        }

        private void BackupDatabase()
        {
            // TODO: Implement database backup functionality
            MessageBox.Show("Veritabanı yedekleme özelliği yakında eklenecek.");
        }

        private void RestoreDatabase()
        {
            // TODO: Implement database restore functionality
            MessageBox.Show("Veritabanı geri yükleme özelliği yakında eklenecek.");
        }
    }
} 