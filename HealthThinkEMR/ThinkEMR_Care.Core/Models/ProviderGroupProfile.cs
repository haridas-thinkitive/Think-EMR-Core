using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinkEMR_Care.Core.Models
{
    public class ProviderGroupProfile
    {
        [Key]
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string ProviderGroupName { get; set; }
        public string ContactNumber { get; set; }
        public string GroupNPINumber { get; set; }
        public string Website { get; set; }
        public string Information { get; set; }
        public string SpecialityTypes { get; set; }
        public string EmailId { get; set; }
        public string FaxId { get; set; }
        public int? PhysicalAddressId { get; set; } // Foreign key property
        
        [ForeignKey("PhysicalAddressId")]
        public PhysicalAddress PhysicalAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PracticeOfficeHours PracticeOfficeHours { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
