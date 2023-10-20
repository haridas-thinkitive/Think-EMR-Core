using System.ComponentModel.DataAnnotations;

namespace ThinkEMR_Care.Core.Models
{
    public class WorkingTime
    {
        [Key]
        public int Id { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
    }


    public class PracticeOfficeHours
    {
        [Key]
        public int Id { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}


