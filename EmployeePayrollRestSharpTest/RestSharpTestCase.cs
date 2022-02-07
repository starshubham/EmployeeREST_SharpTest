using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
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

        /* UC1:- Ability to Retrieve all Employees in EmployeePayroll JSON Server.
                 - Use JSON Server and RESTSharp to save the EmployeePayroll Data of id, name, and salary.
                 - Retrieve in the MSTest Test and corresponding update the Memory with the Data.
        */
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
                System.Console.WriteLine("id: " + e.id + ", Name: " + e.name + ", Salary: " + e.salary);
            }
        }

        /* UC2:- Ability to add a new Employee to the EmployeePayroll JSON Server.
                 - Use JSON Server and RESTSharp to save the EmployeePayroll Data of id, name, and salary.
                 - Ability to add using RESTSharp to JSONServer in the MSTest Test Case and then on success add to Employee Payroll .
                 - Validate with the successful Count 
        */
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            // Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/employees", Method.Post);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(new Employee
            {
                id = 4,
                name = "Clark",
                salary = "15000"
            });

            //Act
            RestResponse response = client.ExecuteAsync(request).Result;

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }

        /*UC3:- Ability to add multiple Employee to  the EmployeePayroll JSON Server.
                - Use JSON Server and RESTSharp to add  multiple Employees to Payroll
                - Ability to add using RESTSharp to  JSONServer in the MSTest Test Case and  then on success add to  EmployeePayrollService
                - Validate with the successful Count
        */

        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ThenShouldReturnEmployeeList()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { name = "Vinaya", salary = "15000" });
            employeeList.Add(new Employee { name = "Ajaya kumar", salary = "7000" });
            employeeList.Add(new Employee { name = "Powan", salary = "9000" });
            employeeList.Add(new Employee { name = "Swathi", salary = "12000" });
            // Iterate the loop for each employee
            foreach (var emp in employeeList)
            {
                // Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("/employees", Method.Post);
                JObject jsonObj = new JObject();
                jsonObj.Add("name", emp.name);
                jsonObj.Add("salary", emp.salary);
                // Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                RestResponse response = client.ExecuteAsync(request).Result;

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.name, employee.name);
                Assert.AreEqual(emp.salary, employee.salary);
                System.Console.WriteLine(response.Content);
            }
        }

        /*UC4:- Ability to Update Salary in Employee Payroll JSON Server.
                - Firstly Update the Salary in Memory.
                - Post that Use JSON Server and RESTSharp to Update the salary.
        */
        [TestMethod]
        public void OnCallingPutAPI_ReturnEmployeeObject()
        {
            // Arrange
            // Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/employees/12", Method.Put);
            JObject jsonObj = new JObject();
            jsonObj.Add("Name", "Shubham");
            jsonObj.Add("Salary", "65000");
            // Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Shubham", employee.name);
            Assert.AreEqual("65000", employee.salary);
            Console.WriteLine(response.Content);
        }

        /*UC5:- Ability to Delete Employee from Employee Payroll JSON Server.
                - Use JSON Server and RESTSharp to then delete the employee by ID.
                - Delete the Employee from the Memory.
        */
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            // Arrange
            // Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/employees/4", Method.Delete);

            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
