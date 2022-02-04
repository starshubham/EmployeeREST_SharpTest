using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace EmployeeRESTSharpTest
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private RestResponse getEmployeeList()
        {
            // Arrange
            // Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/employees", Method.Get);

            // Act
            // Execute the request
            RestResponse response = client.ExecuteAsync(request).Result;
            return response;
        }

        
        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = getEmployeeList();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);     // Comes from using System.Net namespace
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(8, dataResponse.Count);

            foreach (Employee e in dataResponse)
            {
                System.Console.WriteLine("id: " + e.id + ", Name: " + e.name + ", Salary: " + e.Salary);
            }
        }
    }
}
