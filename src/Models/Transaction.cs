using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CariHesap.Models
{
    public class Transaction : BaseEntity
    {
        [Required(ErrorMessage = "İşlem tarihi zorunludur.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice? Invoice { get; set; }

        [Required(ErrorMessage = "İşlem türü zorunludur.")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi zorunludur.")]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }
    }

    public enum TransactionType
    {
        Payment = 1,
        Receipt = 2
    }

    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        BankTransfer = 3,
        Check = 4
    }
} 