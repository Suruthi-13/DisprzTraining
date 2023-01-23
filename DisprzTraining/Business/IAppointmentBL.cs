using DisprzTraining.Models;
namespace DisprzTraining.DisprzTraining.Business
{
    public interface IAppointmentBL
    {
      
       public Task<List<Appointment>> GetAppointmentByDate(DateTime date);
       public Task<Appointment> AddNewAppointment(Appointment appointment);
       public Task<bool> DeleteAppointmentById(Guid id);
       public Task<Appointment> UpdateAppointment(Appointment appointment,Guid meetingId);
       public Task <paginatedResponse> GetAllAppointments(int offset,int limit,DateTime? date,string? searchAppointments);

    }
}