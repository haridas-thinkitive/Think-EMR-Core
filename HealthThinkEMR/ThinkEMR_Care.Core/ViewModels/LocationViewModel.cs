namespace ThinkEMR_Care.Core.ViewModels
{
    public class LocationViewModel
    {
        public IEnumerable<ThinkEMR_Care.Core.Models.Locations> Locations { get; set; }
        public IEnumerable<ThinkEMR_Care.Core.Models.ProviderGroupProfile> ProviderGroupProfiles { get; set; }
        
    }
}
