namespace ThinkEMR_Care.Core.Models
{
    public class AdminDashboard
    {
        public int GroupId { get; set; }

        public string ProviderGroupName { get; set; }

        public string Speciality { get; set; }

        public string ProvidersCount { get; set; }

        public string PatientsCount { get; set; }

        public string AppointmentCount { get; set; }

        public string EncounterCount { get; set; }

        public bool Status { get; set; }
    }
}