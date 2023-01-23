using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using DisprzTraining.DisprzTraining.Business;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;


namespace DisprzTraining.Controllers
{
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _controller;
        private readonly paginatedResponse obj = new paginatedResponse();
        public AppointmentsController(IAppointmentBL controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Get Appointment By Date
        /// </summary>
        ///  <param name="date"></param>
        /// <response code="200">Returns appointments in the searched date</response>
        [HttpGet("v1/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        public async Task<IActionResult> GetAppointmentByDate(DateTime date)
        {

            var result = await _controller.GetAppointmentByDate(date);
            return Ok(result);

        }

        /// <summary>
        /// Add new Appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST v1/meetings
        ///     {        
        ///        "appointmentID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "eventTitle": "Catch Up",
        ///        "appointmentDateStartTime": "2023-01-20T06:40:48.590Z",
        ///        "appointmentDateEndTime": "2023-01-20T06:40:48.590Z",
        ///        "description": "Meet with mentor"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created appointment</response>
        /// <response code="409">If the appointment time is conflict with other appointments</response>  
        [HttpPost("v1/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictResponse))]
        public async Task<IActionResult> AddNewAppointment(Appointment appointment)
        {
            if (appointment.AppointmentDateStartTime >= appointment.AppointmentDateEndTime)
                return BadRequest(new InvalidTimeIntervalresponse());
            var result = await _controller.AddNewAppointment(appointment);
            return result == null ? Conflict(new ConflictResponse()) : Created(nameof(GetAllAppointments), new { MeetingId = result.AppointmentID });


        }

        /// <summary>
        /// Delete Appointment By AppointmentID
        /// </summary>
        ///<param name="meetingId"></param>
        /// <response code="204">The appointment will be deleted</response>
        /// <response code="404">If the Id is not present</response>   
        [HttpDelete("v1/appointments/{meetingId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResponse))]
        public async Task<IActionResult> DeleteAppointmentById(Guid meetingId)
        {
            var result = await _controller.DeleteAppointmentById(meetingId);
            return (result == true) ? NoContent() : NotFound(new NotFoundResponse());
        }

        /// <summary>
        /// Update an appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT v1/meetings
        ///     {        
        ///        "appointmentID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "eventTitle": "Catch Up",
        ///        "appointmentDateStartTime": "2023-01-20T06:40:48.590Z",
        ///        "appointmentDateEndTime": "2023-01-20T06:40:48.590Z",
        ///        "description": "Meet with mentor"
        ///     }
        /// </remarks>
        /// <response code="204">Returns the updated appointment</response>
        /// <response code="409">If the updated appointment time is conflict with other appointments</response>  
        /// <response code="400">If the appoinment time interval is Invalid</response> 
        /// <param name="meetingId"></param>
        [HttpPut("v1/appointments/{meetingId}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(updatedConflictResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidTimeIntervalresponse))]
        public async Task<IActionResult> UpdateAppointment(Appointment appointment, Guid meetingId)
        {

            if (appointment.AppointmentDateStartTime >= appointment.AppointmentDateEndTime)
                return BadRequest(new InvalidTimeIntervalresponse());
            var result = await _controller.UpdateAppointment(appointment, meetingId);
            return (result == null) ? Conflict(new updatedConflictResponse()) : NoContent();
        }

        ///<summary>
        /// Get All appointments using pagination concept
        ///</summary>
        /// <response code="200">Returns the List of appointments</response>  
        [HttpGet("v1/appointments/bulk")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(paginatedResponse))]
        public async Task<IActionResult> GetAllAppointments([Required] int offset, [Required] int fetchCount, DateTime? searchDate, string? searchAppointments)
        {
            var result = await _controller.GetAllAppointments(offset, fetchCount, searchDate, searchAppointments);
            return Ok(result);

        }
    }
}
