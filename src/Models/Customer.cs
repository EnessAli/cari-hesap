using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CariHesap.Models
{
    public class Customer : BaseEntity
    {
        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Müşteri adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Vergi numarası en fazla 20 karakter olabilir.")]
        public string? TaxNumber { get; set; }

        [StringLength(50, ErrorMessage = "Vergi dairesi en fazla 50 karakter olabilir.")]
        public string? TaxOffice { get; set; }

        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir.")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "E-posta adresi en fazla 100 karakter olabilir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
        public string? Address { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 