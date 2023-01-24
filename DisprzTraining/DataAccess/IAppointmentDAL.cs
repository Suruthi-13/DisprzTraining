using DisprzTraining.Models;

namespace DisprzTraining.DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        public List<Appointment> GetAllAppointmentsDAL();
        // public bool UpdateAppointmentListDAL(List<Appointment> appointments);
       
    }
}