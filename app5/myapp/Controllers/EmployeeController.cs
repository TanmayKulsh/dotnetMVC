using Microsoft.AspNetCore.Mvc;
using myapp.Models;
using System.Text.Json;


namespace myapp.Controllers;

public class EmployeeController : Controller
{

    public readonly DBTESTContext _DBTESTContext;

    public EmployeeController(DBTESTContext DBTESTContext)
    {
        _DBTESTContext = DBTESTContext;
    }
    protected override void Dispose(bool disposing)
    {
        _DBTESTContext.Dispose();
    }

    public IActionResult GetEmployees()
    {
        List<Employee> list = _DBTESTContext.Employees.ToList();
        
        ViewData["allemps"] = list;

        string fileName = "formlist.json";
        string jsonString = JsonSerializer.Serialize(list);
        System.IO.File.WriteAllText(fileName, jsonString);
        Console.WriteLine(System.IO.File.ReadAllText(fileName));

        return View();
    }
    [HttpGet]
    public IActionResult Add()
    {

        return View();
    }

    [HttpPost]
    public IActionResult Add(Employee empformadd)
    {
        _DBTESTContext.Employees.Add(empformadd);
        _DBTESTContext.SaveChanges();
        return RedirectToAction("GetEmployees");
    }

    [HttpGet]
    public IActionResult Delete(Employee empformdel)
    {
        _DBTESTContext.Employees.Remove(empformdel);
        _DBTESTContext.SaveChanges();
        return RedirectToAction("GetEmployees");
    }
    public IActionResult Edit(int id)
    {
        var emp = _DBTESTContext.Employees.FirstOrDefault(x => x.Id == id);
        return View(emp);
    }


    [HttpPost]
    public IActionResult Edit(Employee empUpdate)
    {
        var emp = new Employee()
        {
            Id = empUpdate.Id,
            Name = empUpdate.Name,
            Email = empUpdate.Email,
            Address = empUpdate.Address,
            Phone = empUpdate.Phone,
        };

        _DBTESTContext.Employees.Update(emp);
        _DBTESTContext.SaveChanges();
        return RedirectToAction("GetEmployees");
    }

    [HttpGet]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    public IActionResult Login(Employee empform)

    {

        string fileName = "formlist.json";
        string jsonString = System.IO.File.ReadAllText(fileName);
        
        List<Employee> list = JsonSerializer.Deserialize<List<Employee>>(jsonString);

        foreach (Employee item in list)
        {
            if (empform.Email == item.Email && empform.Id == item.Id)
            {

                return RedirectToAction("GetEmployees");
            }
        }
        return RedirectToAction("Login");
    }

    public IActionResult ViewEmp(int id)
    {
        var emp = _DBTESTContext.Employees.FirstOrDefault(x => x.Id == id);
        List<Employee> list = _DBTESTContext.Employees.ToList();
        ViewData["emp"] = emp;

        return View();
    }

}