using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class ProviderUser
    {
        [Key]
        public int Id { get; set; }
        public string? ProviderId { get; set; }
        public string? Image { get; set; }
        public string ProviderType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string LicensedStates { get; set; }
        public string YearOfExperience { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public string OfficeFaxNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string TaxonomyNumber { get; set; }
        public string NPINumber { get; set; }
        public string GroupNPINumber { get; set; }
        public string InsurancesAccepted { get; set; }
        public string WorkLocations { get; set; }

        //Basic Account Profile Data
        public BasicAccountProfileData BasicAccountProfile { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool Status { get; set; }
    }
    public class BasicAccountProfileData
    {
        [Key]
        public int Id { get; set; }
        public string AreaOfFocus { get; set; }
        public string HospitalAffilation { get; set; }
        public string AgeGroupSeen { get; set; }
        public string LanguagesSpoken { get; set; }
        public string ProviderEmploymentReferralNumber { get; set; }
        public string AcceptNewPatients { get; set; }
        public string AcceptCashPay { get; set; }
        public string InsuranceVerification { get; set; }
        public string ProviderBio { get; set; }
        public string ExpertiseIn { get; set; }
        public string WorkExperience { get; set; }

    }
}
