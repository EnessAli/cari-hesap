using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using CariHesap.Data;
using CariHesap.Forms;

namespace CariHesap
{
    static class Program
    {
        public static ApplicationDbContext DbContext { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Initialize database
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CariHesap;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options;

                DbContext = new ApplicationDbContext(options);
                DbContext.Database.EnsureCreated();

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı bağlantısı sırasında bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
} 