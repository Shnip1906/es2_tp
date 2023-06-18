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

public class RelatorioController : Controller
{
    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public  RelatorioController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }
    public async  Task<IActionResult> Relatorio()
    {
        return View();
    }
    public async  Task<IActionResult> RelSkill()
    {
        return View();
    }
    /*public async  Task<IActionResult> RelSkillVer([FromForm] string NomeSkills)
    {
        var value = 0.0;
        var count = 0;
        var ski = _context.Skills.Where(p => p.NomeSkills == NomeSkills).SingleOrDefault();
        
        var talent = _context.Skillprofs.Where(p => p.IdSkills == ski.IdSkills).Include(p => p.IdPerfilNavigation).ToList();
        foreach (var tal in talent)
        {
            
            count = count + 1;
            value = value + tal.IdPerfilNavigation!.Precohora;
        }
        var media = value / count;
        var final = media * 176;
        ViewBag.teste = final;
        return View("RelatorioS");
        //return View();
    }*/
    
    public async  Task<IActionResult> RelSkillVer([FromForm] string NomeSkills)
    {

        var skillModel = new SkillsModel { NomeSkills = NomeSkills };
        var jsonPayload = JsonConvert.SerializeObject(skillModel);

        var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5052/relatorio/rels", requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var finalValue = double.Parse(responseBody);

            ViewBag.teste = finalValue;
            return View("RelatorioS");
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine("Skill not found");
            ViewBag.erro = "Skill not found";
            return View("ErroView");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            ViewBag.erro = $"Error: {response.StatusCode}";
            return View("ErroView");
        }
    }
    
    public async  Task<IActionResult> RelTalentCountry()
    {
        return View("RelPerfilC");
    }
    
    /*public async  Task<IActionResult> RelT([FromForm] string pais)
    {
        var value = 0.0;
        var count = 0;
        var talento = _context.Perfils.Where(p => p.Pais == pais).ToList();
        foreach (var tal in talento)
        {
            count = count + 1;
            value = value + tal.Precohora;
        }
        
        var media = value / count;
        var final = media * 176;
        
        ViewBag.teste = final;
        return View("RelatorioS");
        //return View();
    }*/
    
    public async  Task<IActionResult> RelTVer([FromForm] string pais)
    {
        var perfilModel = new PerfisModel() { Pais = pais };
        var jsonPayload = JsonConvert.SerializeObject(perfilModel);

        var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5052/relatorio/RelT", requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var finalValue = double.Parse(responseBody);

            ViewBag.teste = finalValue;
            return View("RelatorioS");
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine("Perfil not found");
            ViewBag.erro = "Perfil not found";
            return View("ErroView");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            ViewBag.erro = $"Error: {response.StatusCode}";
            return View("ErroView");
        }
    }
    
    /*public async  Task<IActionResult> PesquisaSkills()
    {
        var myDbContext = _context.Skills;
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }*/
    
    public async  Task<IActionResult> PesquisaSkills()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/skill/ListarSkills");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SkillsModel>>(apiResponse);
            
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErroView");
        }
    }
    /*public async  Task<IActionResult> ListarTalentosSkill(Guid id)
    {
        var myDbContext = _context.Skillprofs.Where(f => f.IdSkills == id).Include(u => u.IdPerfilNavigation);
        
        return View(await myDbContext.OrderBy(u => u.IdPerfilNavigation!.NomePerfil).ToListAsync());
    }*/
    
    public async  Task<IActionResult> ListarTalentosSkill(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/perfil/ListarPerfilSkill/{id}");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PerfisModel>>(apiResponse);
            
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErroView");
        }
    }
    
}