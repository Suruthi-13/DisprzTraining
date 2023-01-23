using System.ComponentModel.DataAnnotations;
namespace DisprzTraining.Models
{
    public class Appointment
    {
        public  Guid AppointmentID{get;set;}
        public string EventTitle{get;set;}
        public DateTime AppointmentDateStartTime{get;set;}
        public DateTime AppointmentDateEndTime{get;set;}
        public string Description{get;set;}=string.Empty;
    }
    public class paginatedResponse
    {
        public bool isTruncated {get;set;}
        public List<Appointment> results {get;set;}
    }
}
