using System.Net;
using System.Text;
using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Backend.Controllers;

public class SkillsController : Controller
{
    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;
    public  SkillsController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }
    
    /*public async  Task<IActionResult> IndexSkills()
    {
        var myDbContext = _context.Skills.Include(u=>u.IdAreaProfissionalNavigation);
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }*/
    
    public async  Task<IActionResult> IndexSkills()
    {
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
    
    
    /* public async  Task<IActionResult> IndexSkillsUtilizador()
    {
        var myDbContext = _context.Skills.Include(u=>u.IdAreaProfissionalNavigation);
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }*/
    
    public async  Task<IActionResult> IndexSkillsUtilizador()
    {
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
           return View("ErrorViewUtilizador");
       }
    }
    
    /*public IActionResult Edit(Guid id)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        ViewBag.Id = result.IdSkills;
        ViewBag.Nome = result.NomeSkills;
      //ViewBag.FkIdareaProfNavigation.Nome = result.FkIdareaProfNavigation;
       
        return View();
    }
    */
    
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Invalid ID");
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/skill/VerDadosEditSkill/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SkillsModel>(apiResponse);
                ViewBag.Id = result.IdSkills;
                ViewBag.Nome = result.NomeSkills;

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
    
    /*public IActionResult EditUtilizador(Guid id)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        ViewBag.Id = result.IdSkills;
        ViewBag.Nome = result.NomeSkills;
       
        return View();
    }*/
    
    public async Task<IActionResult> EditUtilizador(Guid? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Invalid ID");
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/skill/VerDadosEditSkill/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SkillsModel>(apiResponse);
                ViewBag.Id = result.IdSkills;
                ViewBag.Nome = result.NomeSkills;

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
    
    /*public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string NomeSkills, [FromForm] int fk_idareaProf, [FromForm] int FkIdareaProfNavigation, [FromForm] string NomeArea)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeSkills = NomeSkills;
       // result.FkIdareaProfNavigation.Nome = NomeArea ;
      
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
    }*/

    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string NomeSkills, [FromForm] int fk_idareaProf, [FromForm] int FkIdareaProfNavigation, [FromForm] string NomeArea)
    {
        var skillModel = new SkillsModel()
        {
           IdSkills = id,
           NomeSkills = NomeSkills
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/EditarSkill", content);

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
        return RedirectToAction("IndexSkills");
    }
    
    /*public async Task<IActionResult> EditsUtilizador ([FromForm] Guid id,[FromForm] string NomeSkills)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        result.NomeSkills = NomeSkills;
      
        db.SaveChanges();
        return RedirectToAction("IndexSkillsUtilizador");
    }*/
    
    public async Task<IActionResult> EditsUtilizador ([FromForm] Guid id,[FromForm] string NomeSkills)
    {
        var skillModel = new SkillsModel()
        {
            IdSkills = id,
            NomeSkills = NomeSkills
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(skillModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/skill/EditarSkill", content);

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
        
        return RedirectToAction("IndexSkillsUtilizador");
    }
    
    /*public IActionResult Create()
    
    {
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdProf"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");

        return View();

    }*/
    
    public async Task<IActionResult> Create()
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

    /*public IActionResult CreateUtilizador()
    
    {
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdProf"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");

        return View();

    }*/
    
    public async Task<IActionResult> CreateUtilizador()
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
    
   /* public IActionResult Registar([FromForm] string nome, [FromForm] Guid fkIdareaProf)
    {
      
        var db = new MyDbContext();
        var skil2 = new Skill();
          //var categoryList = skil2.FkIdareaProfNavigation.Select(c => c.Nome).ToList();
       skil2.NomeSkills = nome;
       skil2.IdAreaProfissional = fkIdareaProf;
      // ViewBag.skil2 = ToSelectList(_context, "FkIdareaProf", "fk");
        db.Skills.Add(skil2);
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
       
        
    }*/
    
    /*public IActionResult Registar([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
    {
        var db = new MyDbContext();
        Skill skill = new Skill();
        skill.NomeSkills = NomeSkills;
        skill.IdAreaProfissional = IdAreaProfissional;
        
        db.Skills.Add(skill);
        db.SaveChanges();
        
        //Areas profissionais necessitam já estar criadas
        return RedirectToAction("IndexSkills");
    }*/
    
    public async Task<IActionResult> Registar([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
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
        
        return RedirectToAction("IndexSkills");
    }
    
    /*public IActionResult RegistarUtilizador([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
    {
        var db = new MyDbContext();
        Skill skill = new Skill();
        skill.NomeSkills = NomeSkills;
        skill.IdAreaProfissional = IdAreaProfissional;
        
        db.Skills.Add(skill);
        db.SaveChanges();
        
        //Areas profissionais necessitam já estar criadas
        return RedirectToAction("IndexSkillsUtilizador");
    }*/
    
    public async Task<IActionResult> RegistarUtilizador([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
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
            return View("ErrorViewUtilizador");
        }
        
        return RedirectToAction("IndexSkillsUtilizador");
    }
    

    /*public async Task<IActionResult> Eliminar(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var dSkillprof = db.Skillprofs.Where(p => p.IdSkills == id).FirstOrDefault();
        if (dSkillprof != null)
        {
            return View("erro");
        }
        var result = new Skill() { IdSkills = id };
     
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
    }*/
    /*public  async Task<IActionResult> Eliminar(Guid id)
    {
        //Falta implementar a verificação se está encontra associada a algum perfil de talento
        var db = new MyDbContext();
        var result = new Skill() { IdSkills = id };
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        await db.SaveChangesAsync();
        return RedirectToAction("IndexSkills");
    }*/
    
    public  async Task<IActionResult> Eliminar(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/skill/EliminarSkill/{id}");
            
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
            return View("ErrorView");
        }

        return RedirectToAction("IndexSkills");
    }

    /*public  async Task<IActionResult> EliminarUtilizador(Guid id)
    {
        //Falta implementar a verificação se está encontra associada a algum perfil de talento
        var db = new MyDbContext();
        var result = new Skill() { IdSkills = id };
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        await db.SaveChangesAsync();
        return RedirectToAction("IndexSkillsUtilizador");
    }*/
    
    public  async Task<IActionResult> EliminarUtilizador(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"http://localhost:5052/skill/EliminarSkill/{id}");
            
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
        
        return RedirectToAction("IndexSkillsUtilizador");
    }

    
   
    /*public async  Task<IActionResult> IndexSkillsManager()
    {
        var myDbContext = _context.Skills.Include(u=>u.IdAreaProfissionalNavigation);
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }*/
    public async  Task<IActionResult> IndexSkillsManager()
    {
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
            return View("ErrorViewUtilizador");
        }
    }

    /*public IActionResult CreateUm()
    {  
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdAreaProfissional"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");
        return View();
        
    }*/
    
    public async Task<IActionResult> CreateUm()
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

            ViewData["IdAreaProfissional"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorViewUM");
        }

        return View();
        
    }

    /*public IActionResult RegistarUM([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
    {
      
        var db = new MyDbContext();

        Skill skill = new Skill();
        skill.NomeSkills = NomeSkills;
        skill.IdAreaProfissional = IdAreaProfissional;
        db.Skills.Add(skill);
        db.SaveChanges();
        return RedirectToAction("IndexSkillsManager");
    }*/
    
    public async Task<IActionResult> RegistarUM([FromForm] string NomeSkills, [FromForm] Guid IdAreaProfissional)
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
            return View("ErrorViewUM");
        }
        return RedirectToAction("IndexSkillsManager");
        }
}