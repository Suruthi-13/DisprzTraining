using System.Linq;
using DisprzTraining.DisprzTraining.DataAccess;
using DisprzTraining.Models;

namespace DisprzTraining.DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {
        
        private readonly IAppointmentDAL _appointmentDAL;
        private readonly  paginatedResponse obj= new paginatedResponse();
        List<Appointment> _appointmentList;
        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;

        }

        public IEnumerable<Appointment> ConflictTestingFunction(DateTime AppointmentDateStartTime, DateTime AppointmentDateEndTime, IEnumerable<Appointment> conflictAppointments)
        {
            var  conflictAppointment=conflictAppointments.Where(s=>(((s.AppointmentDateStartTime <= AppointmentDateStartTime)&&(AppointmentDateStartTime <= s.AppointmentDateEndTime))
                    ||((s.AppointmentDateStartTime <= AppointmentDateEndTime) && (AppointmentDateEndTime <= s.AppointmentDateEndTime)) 
                   || ((AppointmentDateStartTime <= s.AppointmentDateStartTime) && (s.AppointmentDateEndTime <= AppointmentDateEndTime))));

            return conflictAppointment;
        }
        public async Task<List<Appointment>> GetAppointmentByDate(DateTime date)
        {
            _appointmentList=  _appointmentDAL.GetAllAppointmentsDAL();
            var result=_appointmentList.Where(s=>s.AppointmentDateStartTime.Date ==date.Date).ToList();
            return result;
        }
         public async Task<Appointment> AddNewAppointment(Appointment appointment)
        {
            _appointmentList = _appointmentDAL.GetAllAppointmentsDAL();
            var conflictAppointments = _appointmentList.Where(s=>(appointment.AppointmentDateStartTime.Date == s.AppointmentDateStartTime.Date));
            conflictAppointments=ConflictTestingFunction(appointment.AppointmentDateStartTime,appointment.AppointmentDateEndTime,conflictAppointments);
            if (conflictAppointments.Count()<=0)
            {
                _appointmentList.Add(appointment);
                return await Task.FromResult(appointment);
            }
            return null;


        }
        public async Task<paginatedResponse> GetAllAppointments(int offset, int fetchCount, DateTime? date, string? searchAppointments)
        {
            _appointmentList =  _appointmentDAL.GetAllAppointmentsDAL();
            var result=_appointmentList.Where(s=>(date == null || s.AppointmentDateStartTime.Date==date )&&((searchAppointments== null) || (s.EventTitle.ToLower().Contains(searchAppointments.ToLower())||s.Description.ToLower().Contains(searchAppointments.ToLower()))));
             obj.isTruncated = result.Count()<=fetchCount ? false : true; 
             obj.results=result.Skip(offset).Take(fetchCount).ToList<Appointment>();
             return(obj);
        }
        public async Task<bool> DeleteAppointmentById(Guid id)
        {
             _appointmentList = _appointmentDAL.GetAllAppointmentsDAL();
            var result = _appointmentList.Find(s => s.AppointmentID == id);
            if (result != null)
            {
                _appointmentList.Remove(result);
                return await Task.FromResult(true);
            }
            return false;
        }
        public async Task<Appointment> UpdateAppointment(Appointment appointment, Guid id)
        {
            _appointmentList =  _appointmentDAL.GetAllAppointmentsDAL();
            var result =_appointmentList.Find(s => s.AppointmentID == id);
            var isConflict = false;
           if ((result != null) && ((appointment.AppointmentDateStartTime != result.AppointmentDateStartTime) || (appointment.AppointmentDateEndTime != result.AppointmentDateEndTime)))
            {
                var appointmentsOnSameDay=_appointmentList.Where((s=>((s.AppointmentDateStartTime.ToShortDateString()== appointment.AppointmentDateStartTime.ToShortDateString())&&(id!=s.AppointmentID))));
                
                if(appointmentsOnSameDay.Count()>0)
                {
                    var conflictItems = ConflictTestingFunction(appointment.AppointmentDateStartTime,appointment.AppointmentDateEndTime,appointmentsOnSameDay);
                   if(conflictItems.Count()>0)
                   {
                    isConflict=true;
                   }
                }
            }
            if (isConflict == false )
            {
                result.AppointmentDateStartTime = appointment.AppointmentDateStartTime;
                result.AppointmentDateEndTime = appointment.AppointmentDateEndTime;
                result.Description = appointment.Description;
                result.EventTitle=appointment.EventTitle;
                //  _appointmentDAL.UpdateAppointmentListDAL(_appointmentList);
                return await Task.FromResult(appointment);
            }
            return null;
        }


    }
}