using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.FileOperations;
using MCSTask.ViewModels;
using System.IO;
using System.Xml.Linq;

namespace MCSExam.FileOperations
{
    public class EmployeeCrudOperations : IEmployeeCrudOperations
    {
         
        private readonly IWebHostEnvironment _environment;
        private readonly string _filePath;

        
        public EmployeeCrudOperations(IWebHostEnvironment environment)
        {
            _environment = environment;
            _filePath = $"{_environment.WebRootPath}/Files/Employees.txt";
            
        }

        //Get data of the file using a stream readre instance to return a list of Employees
        public  async Task<List<Employee>> LoadEmployeesFromFile()
        {
            
            List<Employee> employees = new List<Employee>();
            string line1 = "";
            int i =0;
            try
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        //Extracting data of single employees that's represented by a single line
                        line1 = line;
                        string[] parts = line.Split('\t');
                        var isRightSalary = decimal.TryParse(parts[6], out decimal empSalary);
                        var EmploymentType = new EmploymentType()
                        {
                            Name = parts[4],
                            Id = parts[4],
                        };
                        Employee employee = new Employee
                        {
                            Name = parts[0],
                            Address = parts[1],
                            BirthDate = (parts[2]),
                            Graduation = parts[3],
                            EmploymentType = EmploymentType,                           
                            Salary = isRightSalary ? empSalary : 0,
                        };

                        if (employee.EmploymentType.Name != "Free lancer")
                        {
                            
                            employee.Assurance = parts[5];
                        }
                        i++;
                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{ex.Message}   {line1}   {i}");
            }
            return employees;
        }

        //Method to retrieve employees based on their employment type
        public async Task<IEnumerable<Employee>> GetTargetEmps(string empType)
        {
            var employees = await LoadEmployeesFromFile();
            var targetEmps = new List<Employee>();
            foreach(var emp in employees)
            {
                if(emp.EmploymentType.Id == empType)
                    targetEmps.Add(emp);
            }
            return targetEmps;
        }


        //method to add new employees to the file as a text line
        public async Task<string> AddEmployee(EmployeeViewModel employee)
        {
            //Converting employees data to a test line with tab delimited
            //fields, so that it matches the format of the file
            //string address = employee.Address.Replace("\"", "\"\"");
            string extractedValue = employee.Address.Trim('"').Replace("\"\"", "\"");
            var newEmployee = 
                $"{employee.Name}\t{employee.Address}{employee.BirthDate}\t{employee.Graduation}\t{employee.EmplomentTypeId}\t{employee.Assurance}\t{employee.Salary}";
            try
            {
                using (StreamWriter writer = File.AppendText(_filePath))
                {
                    await writer.WriteLineAsync(newEmployee);
                }
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException($"{ex.Message}");
            }
            
            return newEmployee;
        }


        public async Task WriteEmployeesToFile(IEnumerable<Employee> employees)
        {
            try
            {
                //overwrites all of the content of an existing file creates a new file
                //var exists = File.Exists(_filePath);
                //var fileMode = exists ? FileMode.Truncate : FileMode.Create;
                //var file = new FileStream(_filePath, fileMode);

                using (StreamWriter stream =  new StreamWriter(_filePath,false))
                {
                    foreach (var employee in employees)
                    {
                        var newEmployee =
                            $"{employee.Name}\t\"{employee.Address}\"\t\t{employee.BirthDate}\t{employee.Graduation}\t" +
                            $"{employee.EmploymentType.Id}\t{employee.Assurance}\t{employee.Salary}\t";
                        await stream.WriteLineAsync(newEmployee);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error writing to file: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Employee>> DeleteEmployee(string name)
        {
            var employees = await LoadEmployeesFromFile();
            employees = employees.Where(emp => emp.Name != name).ToList(); // Filter out employees with the specified name

            return employees; // Return the modified list of employees
        }

        public async Task<IEnumerable<Employee>> UpdateEmployee(Employee employee)
        {
            var employees = await LoadEmployeesFromFile();
            var emp = employees.Find(e => e.Name == employee.Name);
            var newEmp = new Employee();
            newEmp.Name = employee.Name;
            newEmp.Assurance = employee.Assurance;
            newEmp.Address = employee.Address;
            newEmp.BirthDate = employee.BirthDate;
            newEmp.Salary = employee.Salary;
            newEmp.EmplomentTypeId = employee.EmplomentTypeId;
            newEmp.EmploymentType = employee.EmploymentType;

            employees.Remove(emp);
            employees.Add(newEmp);
            return employees;
            
        }

        public IEnumerable<EmploymentType> GetEmploymentTypes()
        {
            var EmploymentTypes = new List<EmploymentType>()
            {
                new EmploymentType()
                {
                    Name = "Hourly Payroll",
                    Id = "Hourly Payroll"
                },
                new EmploymentType()
                {
                    Name = "Monthly payroll",
                    Id = "Monthly payroll"
                },
                new EmploymentType()
                 {
                    Name = "Free lancer",
                    Id = "Free lancer"
                }
            };
            return EmploymentTypes;
        }

        public async Task<Employee> GetEmployeeByName(string name)
        {
            var employees = await LoadEmployeesFromFile();
            Employee targetEmp = null;
            foreach(var emp in employees)
            {
                if(emp.Name == name)
                    targetEmp = emp;
            }
            return targetEmp;
        }

}
}
