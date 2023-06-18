using System.Net;
using System.Text;
using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Backend.Controllers;

public class UsersController : Controller
{

    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public UsersController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }
    
    public IActionResult Create()
    {
        return View();
    }
    public async Task<IActionResult> Registar([FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password, [FromForm] int TipoUtilizador)
    {
        var userModel = new UsersModel()
        {
            nomeUtilizador = NomeUtilizador,
            username = Username,
            password = Password,
            tipoUtilizador = TipoUtilizador
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

    /*public async Task<IActionResult> Index()
    {
        var myDbContext = _context.Utilizadors;
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
    }*/
    
    public async Task<IActionResult> Index()
    {
        // Make an API request
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/users/ListarTodosUtilizadores");

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

    /*public IActionResult Edit(Guid id)
    {
        var db = new MyDbContext();
        //var u = new Talento { Id = id };
        
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == id);
        ViewBag.Id = result.IdUtilizador;
        ViewBag.Nome = result.NomeUtilizador;
        //ViewBag.teste = result.Nome;
        //return View("teste");
        ViewBag.Username = result.Username;
        ViewBag.Password = result.Password;
        
        return View();
    }*/
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Invalid ID");
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/users/VerDadosEditUtilizador/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UsersModel>(apiResponse);
                ViewBag.Id = result.idUtilizador;
                ViewBag.Nome = result.nomeUtilizador;
                ViewBag.Username = result.username;
                ViewBag.Password = result.password;

                return View();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.erro = errorMessage;
                return View("ErrorView");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.erro = errorMessage;
                return View("ErrorView");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    /*public async Task<IActionResult> Edits([FromForm] Guid Id,[FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password)
    {
        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == Id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeUtilizador = NomeUtilizador;
        result.Username = Username;
        result.Password = Password;
        db.SaveChanges();
        return RedirectToAction("Index");
    }*/
    
    public async Task<IActionResult> Edits([FromForm] Guid Id,[FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password)
    {
        var userModel = new UsersModel()
        {
            idUtilizador = Id,
            nomeUtilizador = NomeUtilizador,
            username = Username,
            password = Password
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(userModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/users/EditarUtilizador", content);

        // Check if the request was successful
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Entity updated successfully");
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }
        
        return RedirectToAction("Index");
    }
  

    /*public IActionResult Delete(Guid IdUtilizador)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var db5 = new MyDbContext();
        var db6 = new MyDbContext();
        db2.Clientes.RemoveRange(db2.Clientes.Where(u => u.IdCliente == IdUtilizador));
        db2.SaveChanges();

        db3..RemoveRange(db3.TalentClientes.Where(u => u.IdCliente == id));
        db3.SaveChanges();

        var props = db6.Propostas.Where(u => u.IdCliente == IdUtilizador).ToList();
        foreach (var prop in props)
        {
            db5.Skillsproposta.RemoveRange(db5.Skillsproposta.Where(u => u.IdPropostas == prop.IdPropostas));
            db5.SaveChanges();
        }
        
        db4.Propostas.RemoveRange(db4.Propostas.Where(u => u.IdCliente == IdUtilizador));
        db4.SaveChanges();
        
        
        var db10 = new MyDbContext();
        var result = new Utilizador() { IdUtilizador = IdUtilizador };
        db10.Utilizadors.Attach(result);
        db10.Utilizadors.Remove(result);
        db10.SaveChanges();
        return RedirectToAction("Index");
    }*/
}