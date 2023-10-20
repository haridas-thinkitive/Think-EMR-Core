using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinkEMR_Care.Core.Models
{
    public class Locations
    {
        [Key]
        public int Id { get; set; }
        public byte[]? AddLocationLogo { get; set; }
        public string LocationName { get; set; }
        public int? LocationId { get; set; }
        public string SpecialityType { get; set; }
        public string ContactNumber { get; set; }
        public string EmailId { get; set; }
        public string FaxId { get; set; }
        public string Information { get; set; }
        public int? LocationsPhysicalAddressId { get; set; } // Foreign key property

        [ForeignKey("LocationsPhysicalAddressId")]
        public LocationsPhysicalAddress PhysicalAddress { get; set; }
        public LocationsBillingAddress BillingAddress { get; set; }
        public PracticeOfficeHours PracticeOfficeHours { get; set; }
        public bool Status { get; set; }

        /*public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }*/
    }
}
