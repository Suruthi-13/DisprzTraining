using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.DisprzTraining.Business;
using DisprzTraining.DisprzTraining.DataAccess;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace DisprzTraining.Tests
{

public class MeetingServiceTest
{
  Appointment appointment= new Appointment()
  {
    AppointmentID= Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2025,01,18,10,30,0,0),
    AppointmentDateEndTime= new DateTime(2025,01,18,11,0,0,0),
    Description="Stand Up Meet"
  };
  Appointment appointmentInvalidtimeUpdate= new Appointment()
  {
    AppointmentID=  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),
    EventTitle="StandUpCall",
   AppointmentDateStartTime=new DateTime(2021,01,18,10,30,0,0),
   AppointmentDateEndTime= new DateTime(2021,01,18,09,0,0,0),
   Description="Stand Up Meet"
  };
  Appointment appointmentInvalidTime=new Appointment()
  {
    AppointmentID= Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2023,01,18,10,30,0,0),
    AppointmentDateEndTime= new DateTime(2023,01,18,10,20,0,0),
    Description="Stand Up Meet"
  };
  Appointment appointmentPastTimeUpdate=new Appointment()
  {
    AppointmentID= Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2021,01,18,10,30,0,0),
    AppointmentDateEndTime= new DateTime(2023,01,18,10,20,0,0),
    Description="Stand Up Meet"
  };
  // Appointment appointmentdummy= new Appointment()
  // {
  //   AppointmentID= new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),
  //   EventTitle="StandUp",
  //   AppointmentDateStartTime=new DateTime(2024,01,18,10,30,0,0),
  //   AppointmentDateEndTime= new DateTime(2024,01,18,11,0,0,0),
  //   Description="Stand Up Meet"
  // };
  Appointment appointmentforUpdate= new Appointment()
  {
    AppointmentID=  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),
   EventTitle="StandUpCall",
   AppointmentDateStartTime=new DateTime(2024,01,18,10,30,0,0),
   AppointmentDateEndTime= new DateTime(2024,01,18,11,0,0,0),
   Description="Stand Up Meet"
  };
  Appointment appointmentforTimeUpdate = new Appointment()
  {
    AppointmentID=  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),
   EventTitle="StandUp",
   AppointmentDateStartTime=new DateTime(2024,01,18,10,40,0,0),
   AppointmentDateEndTime= new DateTime(2024,01,18,10,50,0,0),
   Description="Stand Up Meet"
  };
  Appointment appointmentforTimeUpdate_endTime= new Appointment()
  {
    AppointmentID=  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a"),
   EventTitle="StandUp",
   AppointmentDateStartTime=new DateTime(2024,01,18,10,30,0,0),
   AppointmentDateEndTime= new DateTime(2024,01,18,10,50,0,0),
   Description="Stand Up Meet"
  };
  Appointment appointmentforConflictUpdate=new Appointment()
  {
    AppointmentID= Guid.NewGuid(),
    EventTitle="StandUpCall",
   AppointmentDateStartTime=new DateTime(2023,07,18,09,35,0,0),
   AppointmentDateEndTime= new DateTime(2023,07,18,09,40,0,0),
   Description="Stand Up Meet"
  };
  Appointment appointmentCurrentDatePastTime= new Appointment()
  {
    AppointmentID=Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2023,01,13,10,30,0,0),
    AppointmentDateEndTime= new DateTime(2023,01,13,11,0,0,0),
    Description="Stand Up Meet"
  };
  Appointment appointmentForPast= new Appointment()
  {
    AppointmentID=Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2022,01,18,10,30,0,0),
    AppointmentDateEndTime= new DateTime(2022,01,18,11,0,0,0),
    Description="Stand Up Meet"
  };
   Appointment appointmentForConflictTimeInbetween= new Appointment()
  {
    AppointmentID=Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2023,07,18,09,40,0,0),
    AppointmentDateEndTime= new DateTime(2023,07,18,09,45,0,0),
    Description="Stand Up Meet"
  };
  
  Appointment appointmentForConflictTimeStartTimeInBetween= new Appointment()
  {
    AppointmentID=Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2024,01,18,10,45,0,0),
    AppointmentDateEndTime= new DateTime(2024,01,18,11,00,0,0),
    Description="Stand Up Meet"
  };
  Appointment appointmentForConflictTimeEndTimeInBetween= new Appointment()
  {
    AppointmentID=Guid.NewGuid(),
    EventTitle="StandUp",
    AppointmentDateStartTime=new DateTime(2023,07,18,09,45,0,0),
    AppointmentDateEndTime= new DateTime(2023,07,18,10,45,0,0),
    Description="Stand Up Meet"
  };
   [Fact]
  public async Task AddNewAppointment_Returns_201_Created()
  {
            IAppointmentDAL appointmentDAL= new AppointmentDAL();
            IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
            AppointmentsController _appointment= new(appointmentBL);
          var result= await _appointment.AddNewAppointment(appointment) as CreatedResult;
          Assert.IsType<CreatedResult>(result);
          Assert.Equal(result?.StatusCode,201);
  }
  [Fact]
  public async Task AddNewAppointment_Returns_409_Conflicts_400_BadRequest()
  {
            IAppointmentDAL appointmentDAL= new AppointmentDAL();
            IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
            AppointmentsController _appointment= new(appointmentBL);
            //  var result_For_Past_Date= await _appointment.AddNewAppointment(appointmentForPast) as BadRequestObjectResult;
            //  var result_For_current_Date_Past_time= await _appointment.AddNewAppointment(appointmentCurrentDatePastTime) as BadRequestObjectResult;
             var result_For_Invalid_Time= await _appointment.AddNewAppointment(appointmentInvalidTime) as BadRequestObjectResult;
            var result_Time_InBetween_ExcistingTime= await _appointment.AddNewAppointment(appointmentForConflictTimeInbetween) as ConflictObjectResult ;
             var result_StartTime_Inbetween_ExcistingTime= await _appointment.AddNewAppointment(appointmentForConflictTimeStartTimeInBetween) as ConflictObjectResult;
             var result_EndTime_Inbetween_ExcistingTime= await _appointment.AddNewAppointment(appointmentForConflictTimeEndTimeInBetween) as ConflictObjectResult;
            //  Assert.IsType<BadRequestObjectResult>( result_For_Past_Date);
            //  Assert.Equal( result_For_Past_Date?.StatusCode,400);
              // Assert.IsType<BadRequestObjectResult>( result_For_Past_Date);
            //  Assert.Equal(result_For_current_Date_Past_time?.StatusCode,400);
              Assert.IsType<BadRequestObjectResult>( result_For_Invalid_Time);
             Assert.Equal( result_For_Invalid_Time?.StatusCode,400);
              Assert.IsType<ConflictObjectResult>(result_Time_InBetween_ExcistingTime);
             Assert.Equal(result_Time_InBetween_ExcistingTime?.StatusCode,409);
              Assert.IsType<ConflictObjectResult>( result_StartTime_Inbetween_ExcistingTime);
             Assert.Equal(  result_StartTime_Inbetween_ExcistingTime?.StatusCode,409);
              Assert.IsType<ConflictObjectResult>( result_EndTime_Inbetween_ExcistingTime);
             Assert.Equal(result_EndTime_Inbetween_ExcistingTime?.StatusCode,409);
  }
  
  [Fact]
  public async Task GetAppointmentByDate_Returns_200_Sucess()
  {
    var testid=  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a");
            IAppointmentDAL appointmentDAL= new AppointmentDAL();
            IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
            AppointmentsController _appointment= new(appointmentBL);
          var result= await _appointment.GetAppointmentByDate(new DateTime(2024,01,18,10,30,0,0)) as OkObjectResult;
          Assert.Equal(result?.StatusCode,200);
          var appointments=Assert.IsType<List<Appointment>>(result.Value);
          Assert.Equal(appointments.Count,1);
        
  }
  [Fact]
  public async Task GetAppointmentByDate_For_Date_Having_No_Appointment_Returns_200_Sucess()
  {
            IAppointmentDAL appointmentDAL= new AppointmentDAL();
            IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
            AppointmentsController _appointment= new(appointmentBL);
          var result= await _appointment.GetAppointmentByDate(new DateTime(2023,04,08,10,30,0,0)) as OkObjectResult;
          Assert.Equal(result?.StatusCode,200);
          var events=Assert.IsType<List<Appointment>>(result.Value);
          Assert.Equal(events.Count,0);
  }
  // [Fact]
  // public async Task GetAppointmentById_returns_Ok_Success()
  // {
  //   IAppointmentDAL appointmentDAL= new AppointmentDAL();
  //   IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
  //   AppointmentsController _appointment= new(appointmentBL);
  //   var result= await _appointment.GetAppointmentByID(new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a")) as OkObjectResult;
  //   Assert.IsType<OkObjectResult>(result);
  //   Assert.Equal(result?.StatusCode,200);
  // }
  // [Fact]
  // public async Task GetAppointmentById_returns_NotFound_404()
  // {
  //   IAppointmentDAL appointmentDAL= new AppointmentDAL();
  //   IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
  //   AppointmentsController _appointment= new(appointmentBL);
  //   var result= await _appointment.GetAppointmentByID(Guid.NewGuid()) as NotFoundObjectResult;
  //   Assert.IsType<NotFoundObjectResult>(result);
  //   Assert.Equal(result?.StatusCode,404);
  // }
    [Fact]
  public async Task DeleteAppointmentByID_return_201_NoContent()
  {
        IAppointmentDAL appointmentDAL= new AppointmentDAL();
        IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
        AppointmentsController _appointment= new(appointmentBL);
        var result = await _appointment.DeleteAppointmentById( new Guid("3457a0fa-8406-4282-bbbc-132eee6637f7")) as NoContentResult;
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(result.StatusCode,204);
  }
  [Fact]
  public async Task DeleteAppointmentByID_return_404_NotFound()
  {
        IAppointmentDAL appointmentDAL= new AppointmentDAL();
        IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
        AppointmentsController _appointment= new(appointmentBL);
        var result = await _appointment.DeleteAppointmentById(Guid.NewGuid()) as NotFoundObjectResult;
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(result?.StatusCode,404);
  }
  [Fact]
  public async Task UpdateAppointment_return_204_Nocontent()
  {
    var id =  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a");
        IAppointmentDAL appointmentDAL= new AppointmentDAL();
        IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
        AppointmentsController _appointment= new(appointmentBL);
        var result = await _appointment.UpdateAppointment(appointmentforUpdate,id) as NoContentResult;
        var BadrequestResult= await _appointment.UpdateAppointment(appointmentInvalidtimeUpdate,id) as BadRequestObjectResult;
        var newresult= await _appointment.UpdateAppointment(appointmentforTimeUpdate,id) as NoContentResult;
        var newresulttime= await _appointment.UpdateAppointment(appointmentforTimeUpdate_endTime,id) as NoContentResult; 
        // var updateforPastTime= await _appointment.UpdateAppointment( appointmentPastTimeUpdate,id) as BadRequestObjectResult;
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(result.StatusCode,204);
         Assert.IsType<NoContentResult>(newresulttime);
        Assert.Equal(newresulttime.StatusCode,204);
         Assert.IsType<NoContentResult>(newresult);
        Assert.Equal(newresult.StatusCode,204);
        Assert.IsType<BadRequestObjectResult>(BadrequestResult);
        Assert.Equal(BadrequestResult.StatusCode,400);
        //  Assert.IsType<BadRequestObjectResult>(updateforPastTime);
        // Assert.Equal(updateforPastTime.StatusCode,400);
  }
  [Fact]
  public async Task UpdateAppointment_conflict_Time_return_409_Conflict()
  {
    var id =  new Guid("1784ae43-8e4f-48ed-82f0-b0b31c2f971a");
        IAppointmentDAL appointmentDAL= new AppointmentDAL();
        IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
        AppointmentsController _appointment= new(appointmentBL);
        var result = await _appointment.UpdateAppointment(appointmentforConflictUpdate,id) as ConflictObjectResult;
        Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(result.StatusCode,409);
  }
  [Fact]
  public async Task GetAllAppointmentPagination_returns_200_OkResult()
  {
    
       IAppointmentDAL appointmentDAL= new AppointmentDAL();
        IAppointmentBL appointmentBL = new AppointmentBL(appointmentDAL);
        AppointmentsController _appointment= new(appointmentBL);
        var result= await _appointment.GetAllAppointments(0,10,new DateTime(2023,01,08,10,30,0,0)," ") as OkObjectResult;
        var Emptyresult= await _appointment.GetAllAppointments(0,10,new DateTime(2025,01,08,10,30,0,0)," ") as OkObjectResult;
        var emptynewresult= await _appointment.GetAllAppointments(0,10,null,"") as OkObjectResult;
        var searchappointments= await _appointment.GetAllAppointments(0,10,null,"stand") as OkObjectResult;
        Assert.IsType<OkObjectResult>(searchappointments);
        Assert.Equal(searchappointments.StatusCode,200);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(result.StatusCode,200);
        Assert.IsType<OkObjectResult>(Emptyresult);

  }



















            // IHelloWorldBL helloWorldBL = new HelloWorldBL(helloWorldDAL);
            // HelloWorldController helloWorld = new(helloWorldBL);
//     private readonly Mock<IAppointmentBL> mock=new();
//     Guid id=Guid.NewGuid();
//     [Fact]
//     public async Task AddNewAppointment_Return_Created()
//     {
//        var obj= new Appointment(){
//        AppointmentDateStartTime= new DateTime(2022,10,08,11,20,0,0),
//         AppointmentDateEndTime= new DateTime(2022,10,08,12,20,0,0),
//         Description="stand up"
//         };
//         var obj1= new Appointment(){
//        AppointmentDateStartTime= new DateTime(2023,10,08,11,20,0,0),
//         AppointmentDateEndTime= new DateTime(2023,10,08,12,20,0,0),
//         Description="stand up"
//         };
//       mock.Setup(S=>S.AddNewAppointment(It.IsAny<Appointment>()))
//                 .ReturnsAsync(obj);
//       var _controller= new AppointmentsController(mock.Object);
      
//       var result=( CreatedResult) await _controller.AddNewAppointment(obj1) ;
    
//       result.Should().BeOfType<CreatedResult>();
//     }
//     [Fact]
//     public async Task AddNewMeeting_In_the_Already_Excisted_Time_Return_Conflict()
//     {
//        var obj= new Appointment(){
//         AppointmentDateStartTime= new DateTime(2022,10,08,11,20,0,0),
//         AppointmentDateEndTime= new DateTime(2022,10,08,12,20,0,0),
//         Description="stand up"
//         };
//         mock.Setup(S=>S.AddNewAppointment(It.IsAny<Appointment>()))
//                 .ReturnsAsync(()=>null);
//          var _controller= new AppointmentsController(mock.Object);
//          var result= await _controller.AddNewAppointment(obj) as ConflictObjectResult;
//         //  Assert.Equal(409,result?.StatusCode);
//          result.Should().BeOfType<ConflictObjectResult>();
//     }
//     [Fact]
//     public async Task GetAllAppointments_Return_Success()
//     {
//        mock.Setup(S=>S.GetAllAppointments(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<DateTime>(),It.IsAny<string>()))
//                 .ReturnsAsync(new paginatedResponse());
//       var _controller= new AppointmentsController(mock.Object);
//       var result=await _controller.GetAllAppointments(2,3,new DateTime(),"meet") as OkObjectResult;
//       result.Should().BeOfType<OkObjectResult>();
//     }
//     [Fact]
//     public async Task GetAppointmentByDate_For_Excisting_date_return_Success()
//     {
//         mock.Setup(s=>s.GetAppointmentByDate(It.IsAny<DateTime>()))
//         .ReturnsAsync(new List<Appointment>());
//         var _controller= new  AppointmentsController(mock.Object);
//         var result= await _controller.GetAppointmentByDate(new DateTime()) as OkObjectResult;
//         result.Should().BeOfType<OkObjectResult>();
//     }
//     [Fact]
//     public async Task GetAppointmentByDate_For_NonExcistingDate_return_NotFound()
//     {
//       mock.Setup(s=>s.GetAppointmentByDate(It.IsAny<DateTime>()))
//         .ReturnsAsync(()=>null);
//           var _controller= new  AppointmentsController(mock.Object);
//          var result= await _controller.GetAppointmentByDate(new DateTime()) as NotFoundObjectResult;
//         result.Should().BeOfType<NotFoundObjectResult>();

//     }
//     [Fact]
//     public async Task DeleteAppointment_Using_meetingId_Return_Success()
//     {
//        mock.Setup(S=>S.DeleteAppointmentById(It.IsAny<Guid>()))
//           .ReturnsAsync(true);
//       var _controller= new AppointmentsController(mock.Object);
//       var result= await _controller.DeleteAppointmentById(Guid.NewGuid()) as NoContentResult;
//       // Assert.Equal(204,result?.StatusCode);
//       result.Should().BeOfType<NoContentResult>();
//     }
//     [Fact]
//     public async Task DeleteAppointment_Using_Non_Excisted_meetingId_Return_NotFound()
//     {
//       mock.Setup(S=>S.DeleteAppointmentById(It.IsAny<Guid>()))
//           .ReturnsAsync(false);
//       var _controller= new AppointmentsController(mock.Object);
//       var result= await _controller.DeleteAppointmentById(Guid.NewGuid()) as NotFoundObjectResult;
//       // Assert.Equal(404,result?.StatusCode);
//       result.Should().BeOfType<NotFoundObjectResult>();
//     }
//      private readonly Mock<IAppointmentBL> mock1=new();
//     [Fact]
//     public async Task UpdateAppointment_Return_Ok()
//     {
//       var obj= new Appointment(){
//             AppointmentID= id,
//              AppointmentDateStartTime= new DateTime(2022,10,08,11,20,0,0),
//             AppointmentDateEndTime= new DateTime(2022,10,08,12,20,0,0),
//             Description="stand up"
  
//       };
      
//       mock.Setup(S=>S.UpdateAppointment(It.IsAny<Appointment>(),It.IsAny<Guid>()))
//           .ReturnsAsync(new Appointment());
//       var _controller= new AppointmentsController(mock.Object);
      
//       var result=( NoContentResult) await _controller.UpdateAppointment(obj,obj.AppointmentID) ;
//       result.Should().BeOfType<NoContentResult>();
//     }
    
//     [Fact]
//     public async Task UpdateAppointment_For_Excisting_Time_Return_Conflict()
//     {
//       var obj= new Appointment(){
//             AppointmentID= Guid.NewGuid(),
//              AppointmentDateStartTime= new DateTime(2022,10,08,11,20,0,0),
//             AppointmentDateEndTime= new DateTime(2022,10,08,12,20,0,0),
//             Description="stand up"
  
//       };
      
//       mock1.Setup(S=>S.UpdateAppointment(It.IsAny<Appointment>(),It.IsAny<Guid>()))
//           .ReturnsAsync(()=>null);
//       var _controller= new AppointmentsController(mock1.Object);
      
//       var result= (ConflictObjectResult)await _controller.UpdateAppointment(obj,obj.AppointmentID);
//       result.Should().BeOfType<ConflictObjectResult>();
//     }
    
}
}