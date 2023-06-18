using System.Net;
using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Controllers;

public class UtilizadorController : Controller
{
    
    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public UtilizadorController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }

    /*public async Task<IActionResult> IndexUser()
    {
        var Id = SharedVariables.SharedVariables.userID;
        var myDbContext = _context.Utilizadors.Where(f => f.IdUtilizador == Id);
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
       
    }*/
    public async Task<IActionResult> IndexUser()
    {
        var id = SharedVariables.SharedVariables.userID;
        
        try
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync($"http://localhost:5052/users/ListarUtilizadorEspecifico/{id}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<UsersModel>>(apiResponse);

                return View(result);
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
    
    /*public async Task<IActionResult> ListarTalentos()
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }*/
    
    public async Task<IActionResult> ListarTalentos()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/perfil/ListarPerfis");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            //Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }

    /*public IActionResult RegistarTalento([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = NomePerfil;
        perfil.Pais = Pais;
        perfil.Email = Email;
        perfil.Precohora = Precohora;
        perfil.Publico = true;
        
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }*/

    public async Task<IActionResult> RegistarTalento([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora)
    {
        var perfilModel = new PerfisModel()
        {
            NomePerfil = NomePerfil,
            Pais = Pais,
            Email = Email,
            Precohora = Precohora
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(perfilModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/perfil/RegistarPerfil", content);

        // Check if the request was successful
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Entity updated successfully");
            return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }
        
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }
    
    public IActionResult CriarTalento()
    {
        return View();
    }

    /*public IActionResult EditarPerfil(Guid id)
    {
        //Console.WriteLine(id);
        var db = new MyDbContext();
        var result= db.Perfils.SingleOrDefault(p => p.IdPerfil == id);
        
        ViewBag.Id = result.IdPerfil;
        ViewBag.Nome = result.NomePerfil;
        ViewBag.Pais = result.Pais;
        ViewBag.Email = result.Email;
        ViewBag.Preco = result.Precohora;
        ViewBag.Publico = result.Publico;

        if (result.Publico)
        {
            ViewBag.PublicoTxt = "checked";
        }
        
        return View();
    }*/
    
    public async Task<IActionResult> EditarPerfil(Guid? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Invalid ID");
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/VerDadosEditPerfil/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PerfisModel>(apiResponse);
                ViewBag.Id = result.IdPerfil;
                ViewBag.Nome = result.NomePerfil;
                ViewBag.Pais = result.Pais;
                ViewBag.Email = result.Email;
                ViewBag.Preco = result.Precohora;
                ViewBag.Publico = result.Publico;

                if (result.Publico)
                {
                    ViewBag.PublicoTxt = "checked";
                }
                
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

    /*public RedirectToActionResult EditarTalentos([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora, [FromForm] bool Publico, [FromForm] Guid Id)
    {
        Console.WriteLine(NomePerfil);
        Console.WriteLine(Pais);
        Console.WriteLine(Email);
        Console.WriteLine(Precohora);
        Console.WriteLine(Publico);
        Console.WriteLine(Id);
        
        var db = new MyDbContext();
        var result = db.Perfils.SingleOrDefault(p => p.IdPerfil == Id);
        if (result != null)
        {
            result.NomePerfil = NomePerfil;
            result.Pais = Pais;
            result.Email = Email;
            result.Precohora = Precohora;
            result.Publico = Publico;
        }

        db.SaveChanges();
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }*/
    
    public async Task<RedirectToActionResult> EditarTalentos([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora, [FromForm] bool Publico, [FromForm] Guid Id)
    {
        var perfilModel = new PerfisModel()
        {
            IdPerfil = Id,
            NomePerfil = NomePerfil,
            Pais = Pais,
            Email = Email,
            Precohora = Precohora,
            Publico = Publico
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(perfilModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/perfil/EditarPerfil", content);

        // Check if the request was successful
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Entity updated successfully");
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
        }
        
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }
    
    /*public async Task<IActionResult> ListarSkills(int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Skills;
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }*/
    
    public async Task<IActionResult> ListarSkills(int id)
    {
        ViewBag.Id = id;
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/skill/ListarSkills");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SkillsModel>>(apiResponse);
            
            //Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }

    public async Task<IActionResult> CriarSkill()
    {
        var item = new List<SelectListItem>();

        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/admin/AreaProfissional");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Areaprofissional>>(apiResponse);
            
            foreach ( Areaprofissional a in _context.Areaprofissionals )
            {
                item.Add(new SelectListItem(text: a.NomeAreaPrfossional, value:a.IdAreaProfissional.ToString()));
            }

            ViewData["IdProf"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");

        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
        
        return View();
    }

    /*public IActionResult RegistarSkill([FromForm] string NomeSkills)
    {
        var db = new MyDbContext();
        Skill skill = new Skill();
        skill.NomeSkills = NomeSkills;
        skill.IdAreaProfissional = new Guid("58a2aa6a-bb71-4040-8535-53ffcbecd19c");
        
        db.Skills.Add(skill);
        db.SaveChanges();
        
        //Areas profissionais necessitam já estar criadas
        return RedirectToAction("ListarSkills");
    }*/
    
    public async Task<IActionResult> RegistarSkill([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
    {
        var skillModel = new SkillsModel()
        {
            NomeSkills = NomeSkills,
            IdAreaProfissional = IdAreaProfissional
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/RegistarSkill", content);

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
        return RedirectToAction("ListarSkills");
    }

    /*public  async Task<IActionResult> ElimininarSkill(Guid id)
    {
        
        //Falta implementar a verificação se está encontra associada a algum perfil de talento
        var db = new MyDbContext();
        var result = new Skill() { IdSkills = id };
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        await db.SaveChangesAsync();
        return RedirectToAction("ListarSkills");
    }*/
    
    public  async Task<IActionResult> ElimininarSkill(Guid id)
    {
        
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/skill/EliminarSkill/{id}");
            
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Skill Assoc deleted successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {result.StatusCode}");
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }

        return RedirectToAction("ListarSkills");
    }
}