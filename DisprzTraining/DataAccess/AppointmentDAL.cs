using DisprzTraining.Models;

namespace DisprzTraining.DisprzTraining.DataAccess
{
    public  class AppointmentDAL : IAppointmentDAL
    {
         private static List<Appointment> _appointmentListDAL = new List<Appointment>(){
             new Appointment(){AppointmentID=Guid.NewGuid(),EventTitle="StandUp", AppointmentDateStartTime=new DateTime(2023,01,08,10,30,0,0),AppointmentDateEndTime= new DateTime(2023,01,08,11,0,0,0),Description="Stand Up Meet"},
             new Appointment(){AppointmentID=Guid.NewGuid(),EventTitle="CatchUp",AppointmentDateStartTime=new DateTime(2023,07,18,09,30,0,0),AppointmentDateEndTime= new DateTime(2023,07,18,11,0,0,0),Description="Catch up Meet"},
              new Appointment(){AppointmentID=new Guid("3457a0fa-8406-4282-bbbc-132eee6637f7"),EventTitle="StandUp",AppointmentDateStartTime=new DateTime(2023,01,08,14,20,0,0),AppointmentDateEndTime= new DateTime(2023,01,08,15,0,0,0),Description="Stand Up Meet"},
              new Appointment(){AppointmentID= new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),EventTitle="StandUp",AppointmentDateStartTime=new DateTime(2024,01,18,10,30,0,0),AppointmentDateEndTime= new DateTime(2024,01,18,11,0,0,0),Description="Stand Up Meet"}
            };
        public List<Appointment> GetAllAppointmentsDAL()
        {
            return _appointmentListDAL;
        }

        // public bool UpdateAppointmentListDAL(List<Appointment> appointments)
        // {
        //       _appointmentListDAL=appointments;
        //       return true;
        // }
    }
}