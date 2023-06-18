using System.Diagnostics;
using System.Text;
using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class RegistoController : Controller
{
    
    private readonly HttpClient _httpClient;
    public RegistoController()
    {
        _httpClient = new HttpClient();
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    //Handling errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    /*public IActionResult Registar([FromForm] string NomeUtilizador, [FromForm] string username,[FromForm] string password)
    {
        //User user = new User();
        var db = new MyDbContext();
        Utilizador user = new Utilizador();
        user.Username = username;
        user.Password = password;
        user.TipoUtilizador = 3;
        user.NomeUtilizador = NomeUtilizador;
        db.Utilizadors.Add(user);
        db.SaveChanges();
        return RedirectToAction(controllerName:"Home", actionName: "Index");
    }*/
    
    public async Task<IActionResult> Registar([FromForm] string NomeUtilizador, [FromForm] string username,[FromForm] string password)
    {
        var userModel = new UsersModel()
        {
            nomeUtilizador = NomeUtilizador,
            username = username,
            password = password,
            tipoUtilizador = 3
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(userModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/registo/RegistarUtilizador", content);

        // Check if the request was successful
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Entity updated successfully");
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
        }
        
        return RedirectToAction(controllerName:"Home", actionName: "Index");
    }
}