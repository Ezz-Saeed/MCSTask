using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.FileOperations;
using MCSTask.Models;
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
        public async Task<List<Employee>> LoadData()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                var lines = await File.ReadAllLinesAsync(_filePath);
                foreach (var line in lines)
                {
                    //Extracting data from each line by splitting it's parts by tab.
                    string[] parts = line.Split('\t');
                    var isRightSalary = decimal.TryParse(parts[6], out decimal empSalary);                   
                    var isEightParts = parts.Length == 8;
                    //bool isRightOverTimeHourRate =false;
                    //decimal overTimeHourRate = 0.0m;
                    //if (isEightParts)

                        //var IsRightOverTimeHourRate = isEightParts ? decimal.TryParse(parts[7], out decimal overTimeHourRate): false;

                        var EmploymentType = new EmploymentType()
                    {
                        Name = parts[4],
                        Id = parts[4],
                    };
                    //The model to be added to the list for displaying in the view
                    Employee employee = new Employee
                    {
                        Name = parts[0],
                        Address = parts[1],
                        BirthDate = (parts[2]),
                        Graduation = parts[3],
                        EmploymentType = EmploymentType,
                        Salary = isRightSalary ? empSalary : 0,
                        OverTimeHourRate = isEightParts ? decimal.Parse(parts[7]) : 0,
                    };

                    if (employee.EmploymentType.Name != "Free lancer")
                    {

                        employee.Assurance = parts[5];
                    }
                    employees.Add(employee);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            List <Employee> sortedEmps = employees.OrderBy(e => e.Name).ToList();
            return sortedEmps;
        }

        //Method to filter employees based on their Employment Type.
        public async Task<IEnumerable<Employee>> GetTargetEmps(string empType)
        {
            var employees = await LoadData();
            var targetEmps = new List<Employee>();
            foreach(var emp in employees)
            {
                if(emp.EmploymentType.Id == empType)
                    targetEmps.Add(emp);
            }
            return targetEmps;
        }


        //method to add new employees to the file as a text line
        public async Task<string> AddEmployee(EmployeeViewModel model)
        {

            //Converting employees data to a text line with tab as delimiter.
            //preparing the line to be appended to the file.
            List<string> newEmployees = new List<string>();
            var assurance = model.EmplomentTypeId == "Free lancer" ? string.Empty : model.EmplomentAssuranceId;
            model.OverTimeHourRate = GetOverTimeHourRate(model.Salary, model.EmplomentTypeId);
            var newEmployee =
            $"{model.Name}\t{model.Address}\t{model.BirthDate}\t{model.Graduation}\t" +
            $"{model.EmplomentTypeId}\t{assurance}\t{model.Salary}\t{model.OverTimeHourRate}";
            newEmployees.Add(newEmployee);
            try
            {
                await File.AppendAllLinesAsync(_filePath, newEmployees);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{ex.Message}");
            }           
            return newEmployee;
        }

        //To delete an model with specified properties.
        public async Task<IEnumerable<Employee>> DeleteEmployee(EmployeeViewModel model)
        {
            var employees = await LoadData();
            //Get the model to be removed
            var removedEmployees = employees.Where(e  => e.Name == model.Name && e.Address == model.Address && 
            e.BirthDate==model.BirthDate && e.Graduation==model.Graduation && e.Salary==model.Salary).ToList();
            //excluding the model to be removed from the lsit.
            employees = employees.Except(removedEmployees).ToList(); // Filter out employees with the specified properties
            await ReWriteEmployeesToFile(employees);
            return employees; // Return the modified list of employees
        }

        //To rewrite data to the file after doing some modification.
        public async Task ReWriteEmployeesToFile(IEnumerable<Employee> employees)
        {
            using (StreamWriter stream = new StreamWriter(_filePath, false))
            {

                foreach (var employee in employees)
                {
                    var assurance = employee.EmplomentTypeId == "Free lancer" ? string.Empty : employee.Assurance;
                    var emp =
                        $"{employee.Name}\t{employee.Address}\t{employee.BirthDate}\t{employee.Graduation}\t" +
                        $"{employee.EmploymentType.Id}\t{assurance}\t{employee.Salary}";
                    await stream.WriteLineAsync(emp);

                }
            }
        }

        //Updating an existing model
        public async Task UpdateEmployee(EmployeeViewModel model)
        {
            //Prepare data of the updated model.
            var assurance = model.EmplomentTypeId == "Free lancer" ? string.Empty : model.EmplomentAssuranceId;
            model.OverTimeHourRate = GetOverTimeHourRate(model.Salary, model.EmplomentTypeId);
            var newEmp =$"{model.Name}\t{model.Address}\t{model.BirthDate}\t{model.Graduation}\t" +
                     $"{model.EmplomentTypeId}\t{assurance}\t{model.Salary}\t{model.OverTimeHourRate}";
            string [] lines =  File.ReadAllLines(_filePath);
            for(int i=0; i<lines.Length; i++)
            {
                //Splitting the line parts by tab
                var parts = lines[i].Split('\t');
                //Get the old data of the model
                if (parts[0] == model.Name )
                    //Replacing the old data of the model with the new one.
                    lines[i] = newEmp;
            }
            await File.WriteAllLinesAsync(_filePath, lines);
        }

        //Method to get OverTime Hour Rate for model based on it's salary and employemnt type.
        public decimal GetOverTimeHourRate(decimal salary, string employmentType)
        {
            var overTimeHourRate = employmentType == "Hourly Payroll" ? ((salary / 30.0m) * 3.0m) / 16.0m :
                (employmentType == "Monthly payroll" ?
            ((salary) / 160.0m) : (employmentType == "Free lancer" ? ((salary / (30.0m * 24.0m)) * 1.5m) : 0));
            return Math.Round(overTimeHourRate, 4);
        }

        //Method to retrieve list of Employment Types with it's Ids and Values so as to be
        //transformed to select list items to be viewed bu UIs
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

        //Method to retrieve list of Employment Assurance Types with it's Ids and Values so as to be
        //transformed to select list items to be viewed bu UIs
        public IEnumerable<EmploymentAssurance> GetEmploymentAssurance()
        {
            var employmentAssurance = new List<EmploymentAssurance>()
            {
                new EmploymentAssurance()
                {
                    Id = "Assurance",
                    Value = "Assurance",
                },
                new EmploymentAssurance()
                {
                    Id = "No Assurance",
                    Value = "No Assurance",
                },
                new EmploymentAssurance()
                {
                    Id = "None",
                    Value = "None",
                }
            };
            return employmentAssurance;
        }

        //Retrieving an model of a specific name
        public async Task<Employee> GetEmployee(string name)
        {
            var employees = await LoadData();
            Employee? targetEmp = employees.Find(e=> e.Name==name );
         
            return targetEmp;
        }

        //To get employees with time rate greater than a specific value.
        public async Task<List<Employee>> FilterOutEmployees(decimal xValue)
        {
            var employees = await LoadData();
            var targetEmps = employees.Where(e => (e.Salary/(30*24)) > xValue).ToList();
            return targetEmps;
        }

    }
}
