namespace ThinkEMR_Care.Core.Models.MasterDashboard
{
    public class DataImport
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int TotalRecords { get; set; }
        public string SampleTemplate { get; set; }
        public string UploadFile { get; set; }
    }
}
