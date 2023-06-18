using System.Net;
using System.Text;
using Backend.Models;
using BusinessLogic.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Backend.Controllers;

public class PerfilController : Controller
{
    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public PerfilController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }
    
    /*public async Task<IActionResult> ListarPerfis()
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }*/
    
    public async Task<IActionResult> ListarPerfisAdmin()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/perfil/ListarPerfis");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            //Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View("ListarPerfis", data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }
    
    
    public async Task<IActionResult> ListarPerfisUtilizador()
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
            return View("ErrorViewUtilizador");
        }
    }

    /*public async Task<IActionResult> EliminarPerfil(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        db2.Skillprofs.RemoveRange(db2.Skillprofs.Where(u => u.IdSkills == id));
        db2.SaveChanges();

        var result = new Perfil { IdPerfil = id };
        db.Perfils.Attach(result);
        db.Perfils.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }*/
    public async Task<IActionResult> EliminarPerfil(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"http://localhost:5052/perfil/EliminarPerfil/{id}");
            
            // Check the response status
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Client deleted successfully.");
            }
            else
            {
                Console.WriteLine($"An error occurred: {result.StatusCode}");
                string errorMessage = await result.Content.ReadAsStringAsync();
                ViewBag.erro = errorMessage;
                return View("ErrorView");
            }
                    
            return RedirectToAction("ListarPerfisAdmin");
        }

    public IActionResult CriarPerfil()
    {
        return View();
    }
    
    public IActionResult CriarPerfilUtilizador()
    {
        return View();
    }

    /*public IActionResult EditarPerfil(Guid id)
    {
        var db = new MyDbContext();
        var u = new Perfil { IdPerfil = id };

        var result = db.Perfils.SingleOrDefault(b => b.IdPerfil == id);
        if (result != null)
        {
            ViewBag.Id = result.IdPerfil;
            ViewBag.Nome = result.NomePerfil;
            ViewBag.Pais = result.Pais;
            ViewBag.Email = result.Email;
            ViewBag.Preco = result.Precohora;
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
    
    /*public IActionResult EditarPerfilUtilizador(Guid id)
    {
        var db = new MyDbContext();
        //var u = new Perfil { IdPerfil = id };

        var result = db.Perfils.SingleOrDefault(b => b.IdPerfil == id);
        if (result != null)
        {
            ViewBag.Id = result.IdPerfil;
            ViewBag.Nome = result.NomePerfil;
            ViewBag.Pais = result.Pais;
            ViewBag.Email = result.Email;
            ViewBag.Preco = result.Precohora;
            ViewBag.Publico = result.Publico;
        }

        return View();
    }*/
    
    public async Task<IActionResult> EditarPerfilUtilizador(Guid? id)
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

                return View();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.erro = errorMessage;
                return View("ErrorViewUtilizador");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.erro = errorMessage;
                return View("ErrorViewUtilizador");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    /*public async Task<IActionResult> EditarPerfis([FromForm] Guid Id, [FromForm] string NomePerfil, [FromForm] string Pais,
        [FromForm] string Email, [FromForm] double Precohora, [FromForm] bool Publico)
    {
               
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
        
        return RedirectToAction("ListarPerfisUtilizador");
    }*/
    
    public async Task<IActionResult> EditarPerfis([FromForm] Guid Id, [FromForm] string NomePerfil, [FromForm] string Pais,
        [FromForm] string Email, [FromForm] double Precohora, [FromForm] bool Publico)
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
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorViewUtilizador");
        }
        
        return RedirectToAction("ListarPerfisUtilizador");
    }
    
    /*public async Task<IActionResult> EditarPerfisAdmin([FromForm] Guid Id, [FromForm] string NomePerfil, [FromForm] string Pais,
        [FromForm] string Email, [FromForm] double Precohora, [FromForm] bool Publico)
    {
               
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
        
        return RedirectToAction("ListarPerfisAdmin");
    }*/
    
    public async Task<IActionResult> EditarPerfisAdmin([FromForm] Guid Id, [FromForm] string NomePerfil, [FromForm] string Pais,
        [FromForm] string Email, [FromForm] double Precohora, [FromForm] bool Publico)
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
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }
        
        return RedirectToAction("ListarPerfisAdmin");
    }

    /*public IActionResult RegistarPerfil([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = NomePerfil;
        perfil.Email = Email;
        perfil.Pais = Pais;
        perfil.Precohora = Precohora;
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }*/
    public async Task<IActionResult> RegistarPerfil([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
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
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }
        return RedirectToAction("ListarPerfisAdmin");
    }
    
    /*public IActionResult RegistarPerfilUtilizador([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
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
        return RedirectToAction("ListarPerfisUtilizador");
    }*/

    public async Task<IActionResult> RegistarPerfilUtilizador([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
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
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorViewUtilizador");
        }
        
        return RedirectToAction("ListarPerfisUtilizador");
    }
    
    /*public async Task<IActionResult> ExperienciaPerfil(Guid id)
    {
        var myDbContext = _context.Experiencia;
        ViewBag.id = id;
        //return View(await myDbContext.Where(u => u.IdExperiencia == id).OrderBy(u => u.IdExperiencia == id).ToListAsync());
        return View(await myDbContext.Where(u => u.IdPerfil == id).ToListAsync());
    }*/
    
    public async Task<IActionResult> ExperienciaPerfil(Guid? id)
    {
        ViewBag.Id = id;
        
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarExpPerfis/{id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ExperienciaModel>>(apiResponse);
            
            ViewBag.Id = id;
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }

    public async Task<IActionResult> ExperienciaPerfilUtilizador(Guid? id)
    {
        ViewBag.Id = id;
        
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarExpPerfis/{id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ExperienciaModel>>(apiResponse);
            
            ViewBag.Id = id;
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUtilizador");
        }
    }
    
    // Criar Experiencia Page
    public async Task<IActionResult> CriarExp(Guid id)
    {
        ViewBag.Id = id;
        return View();
    }
    public async Task<IActionResult> CriarExpUtilizador(Guid id)
    {
        ViewBag.Id = id;
        return View();
    }

    // asp-action form Criar Experiencia
    /*public async Task<IActionResult> CriarExps([FromForm] Guid Id,[FromForm] string NomeExperiencia, [FromForm] string NomeEmpresa,[FromForm] int Anoinicio, [FromForm] int Anofim)
    {
        if (Anoinicio > Anofim)
        {
            return View("ErroView");
        }
        var db = new MyDbContext();
        Experiencium exp = new Experiencium();
        exp.NomeExperiencia = NomeExperiencia;
        exp.NomeEmpresa = NomeEmpresa;
        exp.Anoinicio = Anoinicio;
        exp.Anofim = Anofim;
        exp.IdPerfil = Id;
        exp.Continuo = Anofim == 2023;
        db.Experiencia.Add(exp);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }
   
    */
    /*public async Task<IActionResult> CriarExps([FromForm] Guid Id,[FromForm] string NomeExperiencia, [FromForm] string NomeEmpresa,[FromForm] int Anoinicio, [FromForm] int Anofim)
    {
        if (Anoinicio > Anofim)
        {
            ViewBag.erro = "A data de inicio é maior que a data Final!!!";
            return View("ErrorView");
        }
        var db = new MyDbContext();
        Experiencium exp = new Experiencium();
        exp.NomeExperiencia = NomeExperiencia;
        exp.NomeEmpresa = NomeEmpresa;
        exp.Anoinicio = Anoinicio;
        exp.Anofim = Anofim;
        exp.IdPerfil = Id;
        exp.Continuo = Anofim == 2023;
        db.Experiencia.Add(exp);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }*/
    
    public async Task<IActionResult> CriarExps([FromForm] Guid Id,[FromForm] string NomeExperiencia, [FromForm] string NomeEmpresa,[FromForm] int Anoinicio, [FromForm] int Anofim)
    {
        if (Anoinicio > Anofim)
        {
            ViewBag.erro = "A data de inicio é maior que a data Final!!!";
            return View("ErrorView");
        }
        
        var expModel = new ExperienciaModel()
        {
            NomeExperiencia = NomeExperiencia,
            NomeEmpresa = NomeEmpresa,
            Anoinicio = Anoinicio,
            Anofim = Anofim,
            IdPerfil = Id,
            Continuo = Anofim == 2023
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/perfil/RegistarExperiencia", content);

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
        
        return RedirectToAction("ListarPerfisAdmin");
    }

    /*public async Task<IActionResult> CriarExpsUtilizador([FromForm] string NomeExperiencia, [FromForm] string NomeEmpresa,[FromForm] int Anoinicio, [FromForm] int Anofim, [FromForm] Guid Id)
    {
        if (Anoinicio > Anofim)
        {
            return View("ErroView");
        }
        var db = new MyDbContext();
        Experiencium exp = new Experiencium();
        //exp.id = id;
        exp.NomeExperiencia = NomeExperiencia;
        exp.NomeEmpresa = NomeEmpresa;
        exp.Anoinicio = Anoinicio;
        exp.Anofim = Anofim;
        exp.IdPerfil = Id;
        exp.Continuo = Anofim == 2023;
        db.Experiencia.Add(exp);
        db.SaveChanges();
        return RedirectToAction("ExperienciaPerfilUtilizador", new RouteValueDictionary { { "id", Id } });
    }*/
    
    public async Task<IActionResult> CriarExpsUtilizador([FromForm] string NomeExperiencia, [FromForm] string NomeEmpresa,[FromForm] int Anoinicio, [FromForm] int Anofim, [FromForm] Guid Id)
    {
        
        if (Anoinicio > Anofim)
        {
            ViewBag.erro = "A data de inicio é maior que a data Final!!!";
            return View("ErrorViewUtilizador");
        }
        
        var expModel = new ExperienciaModel()
        {
            NomeExperiencia = NomeExperiencia,
            NomeEmpresa = NomeEmpresa,
            Anoinicio = Anoinicio,
            Anofim = Anofim,
            IdPerfil = Id,
            Continuo = Anofim == 2023
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/perfil/RegistarExperiencia", content);

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
            return View("ErrorViewUtilizador");
        }
        
        return RedirectToAction("ExperienciaPerfilUtilizador", new RouteValueDictionary { { "id", Id } });
    }
    
    /*public async Task<IActionResult> EliminarExp(Guid id)
    {
        var db = new MyDbContext();
        var result = new Experiencium() { IdExperiencia = id };
        db.Experiencia.Attach(result);
        db.Experiencia.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }*/
    
    public async Task<IActionResult> EliminarExp(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/perfil/EliminarExperiencia/{id}");
            
        // Check the response status
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Client deleted successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {result.StatusCode}");
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }

        return RedirectToAction("ListarPerfisAdmin");
    }
    
    /*public async Task<IActionResult> EliminarExpUtilizador(Guid id)
    {
        var db = new MyDbContext();
        var result = new Experiencium() { IdExperiencia = id };
        db.Experiencia.Attach(result);
        db.Experiencia.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfisUtilizador");
    }*/
    
    public async Task<IActionResult> EliminarExpUtilizador(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/perfil/EliminarExperiencia/{id}");
            
        // Check the response status
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Client deleted successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {result.StatusCode}");
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorView");
        }
        
        return RedirectToAction("ListarPerfisUtilizador");
    }
    
    /*public async Task<IActionResult> ListarSkillTalento(Guid Id)
    {
        {
            var myDbContext = _context.Skillprofs.Where(t => t.IdPerfil == Id).Include(t=>t.IdSkillsNavigation);
            // ViewBag.id = id;
            return View(await myDbContext.OrderBy(u =>u.IdPerfil == Id).ToListAsync());
        }
    }*/
    
    public async Task<IActionResult> ListarSkillTalento(Guid Id)
    {
        ViewBag.Id = Id;
        
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarSkillPerfil/{Id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SkillsProfModel>>(apiResponse);
            
            ViewBag.Id = Id;
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }
    
    /*public async Task<IActionResult> ListarSkillPerfilUtilizador(Guid Id)
    {
        {
            var myDbContext = _context.Skillprofs.Where(t => t.IdPerfil == Id).Include(p => p.IdSkillsNavigation);
            return View(await myDbContext.OrderBy(u =>u.IdPerfil == Id).ToListAsync());
        }
    }*/
    
    public async Task<IActionResult> ListarSkillPerfilUtilizador(Guid Id)
    {
        ViewBag.Id = Id;
        
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarSkillPerfil/{Id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SkillsProfModel>>(apiResponse);
            
            ViewBag.Id = Id;
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUtilizador");
        }
    }
    
    /*public IActionResult CriarSkillPerfil()
    {
        var item = new List<SelectListItem>();

        foreach ( Skill skil in _context.Skills )
        {
            item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
        }

        ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        
        var item2 = new List<SelectListItem>();

        foreach ( Perfil p in _context.Perfils )
        {
            item2.Add(new SelectListItem(text: p.NomePerfil, value:p.IdPerfil.ToString()));
        }

        ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");

        return View();
    }*/
    
    public async Task<IActionResult> CriarSkillPerfil()
    {
        var item = new List<SelectListItem>();

        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/skill/ListarSkills");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Areaprofissional>>(apiResponse);
            
            foreach ( Skill skil in _context.Skills )
            {
                item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
            }

            ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUtilizador");
        }
        
        var item2 = new List<SelectListItem>();

        HttpResponseMessage result = await _httpClient.GetAsync("http://localhost:5052/perfil/ListarPerfis");

        // Check if the API request was successful
        if (result.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            foreach ( Perfil p in _context.Perfils )
            {
                item2.Add(new SelectListItem(text: p.NomePerfil, value:p.IdPerfil.ToString()));
            }

            ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");

        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
        
        return View();
    }
    
    /*public IActionResult CriarSkillPerfilUtilizador()
    {
        var item = new List<SelectListItem>();

        foreach ( Skill skil in _context.Skills )
        {
            item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
        }

        ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        
        var item2 = new List<SelectListItem>();

        foreach ( Perfil p in _context.Perfils )
        {
            item2.Add(new SelectListItem(text: p.NomePerfil, value:p.IdPerfil.ToString()));
        }

        ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");
        
        return View();
    }*/
    
    public async Task<IActionResult> CriarSkillPerfilUtilizador()
    {
        var item = new List<SelectListItem>();

        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/skill/ListarSkills");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Areaprofissional>>(apiResponse);
            
            foreach ( Skill skil in _context.Skills )
            {
                item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
            }

            ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUtilizador");
        }
        
        var item2 = new List<SelectListItem>();

        HttpResponseMessage result = await _httpClient.GetAsync("http://localhost:5052/perfil/ListarPerfis");

        // Check if the API request was successful
        if (result.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            foreach ( Perfil p in _context.Perfils )
            {
                item2.Add(new SelectListItem(text: p.NomePerfil, value:p.IdPerfil.ToString()));
            }

            ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");

        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
        
        return View();
    }
    
    /*public IActionResult CriaSKT([FromForm] int Nhoras, [FromForm] Guid IdSkills,[FromForm] Guid IdPerfil)
    {

        var db = new MyDbContext();
        
        Skillprof skillprof = new Skillprof();
        skillprof.Nhoras = Nhoras;
        skillprof.IdSkills= IdSkills;
        skillprof.IdPerfil = IdPerfil;
        db.Skillprofs.Add(skillprof);
        db.SaveChanges();

        return RedirectToAction("ListarPerfis");
    }*/
    
    public async Task<IActionResult> CriaSKT([FromForm] int Nhoras, [FromForm] Guid IdSkills,[FromForm] Guid IdPerfil)
    {
        var skillModel = new SkillsProfModel()
        {
            Nhoras = Nhoras,
            IdSkills = IdSkills,
            IdPerfil = IdPerfil
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/RegistarSkillProfissional", content);

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
        
        return RedirectToAction("ListarPerfisAdmin");
    }
    
    public async Task<IActionResult> CriaSKTUtilizador([FromForm] int Nhoras, [FromForm] Guid IdSkills,[FromForm] Guid IdPerfil)
    {

        var skillModel = new SkillsProfModel()
        {
            Nhoras = Nhoras,
            IdSkills = IdSkills,
            IdPerfil = IdPerfil
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/RegistarSkillProfissional", content);

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

        return RedirectToAction("ListarSkillPerfilUtilizador", new RouteValueDictionary { { "id", IdPerfil } });
    }
    /*public async Task<IActionResult> ListarPerfil2(string id, string searchTalento)
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }*/
    /*public async Task<IActionResult> ListarPerfilUserManager(Guid id, string searchTalento)
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }*/
    
    public async Task<IActionResult> ListarPerfilUserManager(Guid id, string searchTalento)
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
    
    /*
     * Listar skills dos Perfis
     */
    /*public async Task<IActionResult> ListarSkillPerfil2(Guid id)
    {
        {
            var myDbContext = _context.Skillprofs.Where(t => t.IdPerfil == id).Include(t => t.IdSkillsNavigation);
            // ViewBag.id = id;
            return View(await myDbContext.OrderBy(u => u.IdPerfil == id).ToListAsync());
        }
    }*/
    
    public async Task<IActionResult> ListarSkillPerfil2(Guid id)
    {
        ViewBag.Id = id;
        
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarSkillPerfil/{id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SkillsProfModel>>(apiResponse);
            
            ViewBag.Id = id;
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }
    
    /*public IActionResult RegistarPerfilUM([FromForm] string nome, [FromForm] string pais, [FromForm] string email,
        [FromForm] double precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = nome;
        perfil.Email = email;
        perfil.Pais = pais;
        //ViewBag.teste = precohora;
        //return View("teste");
        perfil.Precohora = precohora;
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction("ListarPerfilUserManager");
    }*/
    
    public async Task<IActionResult> RegistarPerfilUM([FromForm] string nome, [FromForm] string pais, [FromForm] string email,
        [FromForm] double precohora)
    {
        var perfilModel = new PerfisModel()
        {
            NomePerfil = nome,
            Pais = pais,
            Email = email,
            Precohora = precohora
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(perfilModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/perfil/RegistarPerfil", content);

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
            return View("ErrorViewUM");
        }
        return RedirectToAction("ListarPerfilUserManager");
    }

    public IActionResult CriarPerfilUm()
    {
        return View();
    }
/*
public IActionResult CriarSkillPerfilUm()
    {
        var item = new List<SelectListItem>();

        foreach ( Skill skil in _context.Skills )
        {
            item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
        }

        ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        
        var item2 = new List<SelectListItem>();

        foreach ( Perfil talento in _context.Perfils )
        {
            item.Add(new SelectListItem(text: talento.NomePerfil, value:talento.IdPerfil.ToString()));
        }

        ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");
        
        return View();
    }*/
    
    public async Task<IActionResult> CriarSkillPerfilUm()
    {
        var item = new List<SelectListItem>();
        
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/skill/ListarSkills");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Areaprofissional>>(apiResponse);
            
            foreach ( Skill skil in _context.Skills )
            {
                item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
            }

            ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUM");
        }
        
        var item2 = new List<SelectListItem>();

        HttpResponseMessage result = await _httpClient.GetAsync("http://localhost:5052/perfil/ListarPerfis");

        // Check if the API request was successful
        if (result.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            foreach ( Perfil p in _context.Perfils )
            {
                item2.Add(new SelectListItem(text: p.NomePerfil, value:p.IdPerfil.ToString()));
            }

            ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");

        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUM");
        }
        return View();
    }

    /*public IActionResult CriaSKTUM([FromForm] int Anos, [FromForm] Guid fkIdSkill,[FromForm] Guid fkIdTalento)
    {

        var db = new MyDbContext();
        
        Skillprof tSkillprof = new Skillprof();
        tSkillprof.Nhoras = Anos;
        tSkillprof.IdSkills= fkIdSkill;
        tSkillprof.IdPerfil = fkIdTalento;
        db.Skillprofs.Add(tSkillprof);
        db.SaveChanges();
        return RedirectToAction("ListarPerfilUserManager");
    }*/

    public async Task<IActionResult> CriaSKTUM([FromForm] int Anos, [FromForm] Guid fkIdSkill,[FromForm] Guid fkIdTalento)
    {

        var skillModel = new SkillsProfModel()
        {
            Nhoras = Anos,
            IdSkills = fkIdSkill,
            IdPerfil = fkIdTalento
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/RegistarSkillProfissional", content);

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

        return RedirectToAction("ListarPerfilUserManager");
    }

    /*public IActionResult EliminarSkillAssoc(Guid id)
    {
        var db = new MyDbContext();
        var result = new Skillprof() { IdSkillsprof = id };
        db.Skillprofs.Attach(result);
        db.Skillprofs.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfisUtilizador");
    }*/
    
    public async Task<IActionResult> EliminarSkillAssoc(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/skill/EliminarSkillAssoc/{id}");
            
        // Check the response status
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Skill Assoc deleted successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {result.StatusCode}");
            string errorMessage = await result.Content.ReadAsStringAsync();
            ViewBag.erro = errorMessage;
            return View("ErrorViewUtilizador");
        }
        
        return RedirectToAction("ListarPerfisUtilizador");
    }
}