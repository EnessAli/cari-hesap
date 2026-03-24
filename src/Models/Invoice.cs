using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CariHesap.Models
{
    public class Invoice : BaseEntity
    {
        [Required(ErrorMessage = "Fatura numarası zorunludur.")]
        [StringLength(20, ErrorMessage = "Fatura numarası en fazla 20 karakter olabilir.")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fatura tarihi zorunludur.")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required(ErrorMessage = "Ara toplam zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Ara toplam 0'dan büyük olmalıdır.")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "KDV tutarı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "KDV tutarı 0'dan büyük olmalıdır.")]
        public decimal VAT { get; set; }

        [Required(ErrorMessage = "İndirim tutarı zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "İndirim tutarı 0'dan büyük olmalıdır.")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Toplam tutar zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Toplam tutar 0'dan büyük olmalıdır.")]
        public decimal Total { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 