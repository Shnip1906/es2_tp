using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Controllers;

public class UserManagerController : Controller{


    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public UserManagerController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }
        
    public IActionResult Create()
    {
        return View();
    }
    
    public IActionResult IndexUManager()
    {
        return View();
    }
        
    /*public async Task<IActionResult> Index()
    {
        var myDbContext = _context.Utilizadors.Where(f => f.TipoUtilizador == 3 );
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
    }*/
    
    public async Task<IActionResult> Index()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/users/ListarUtilizadores");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<UsersModel>>(apiResponse);
            
            //Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }
    
    /*public IActionResult Registar([FromForm] string nome, [FromForm] string username,[FromForm] string password)
    {
        //User user = new User();
        var db2 = new MyDbContext();
        var exist = db2.Utilizadors.Where(p => p.Username == username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.teste = "O username já existe no sistema!!!";
            return View("userExist");
        }
        var db = new MyDbContext();
        Utilizador users = new Utilizador();
        users.Username = username;
        users.Password = password;
        users.TipoUtilizador = 3;
        users.NomeUtilizador = nome;
        db.Utilizadors.Add(users);
        db.SaveChanges();
        return RedirectToAction("Index");
    }*/
    
    public async Task<IActionResult> Registar([FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password)
    {
        var userModel = new UsersModel()
        {
            nomeUtilizador = NomeUtilizador,
            username = Username,
            password = Password,
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
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
        }
        
        return RedirectToAction("Index");
    }
}