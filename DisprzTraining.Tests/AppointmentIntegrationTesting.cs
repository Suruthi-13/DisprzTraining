using DisprzTraining.Controllers;
using DisprzTraining.DisprzTraining.Business;
using DisprzTraining.DisprzTraining.DataAccess;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace DisprzTrainingIntegration.Tests
{
    public class AppoinmentIntegrationTest
    {
         private readonly HttpClient _client;
         static IAppointmentDAL appoinmentDAL = new AppointmentDAL();
        static IAppointmentBL appoinmentBL = new AppointmentBL(appoinmentDAL);
        AppointmentsController appoinment = new(appoinmentBL);
          public AppoinmentIntegrationTest()
        {
            var integrationAppoinment = new WebApplicationFactory<AppointmentsController>();
            _client = integrationAppoinment.CreateClient();

        }
        [Fact]
        public async Task Integration_Testing_Get_All()
        {
            var response = await _client.GetAsync("http://localhost:5169/v1/appointments/bulk?offset=0&fetchCount=500");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}