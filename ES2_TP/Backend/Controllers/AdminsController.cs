using BusinessLogic.Context;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace Backend.Controllers;

public class AdminsController : Controller
{
    //  [HttpGet] //
    private readonly MyDbContext _context;
    private readonly HttpClient _httpClient;

    public AdminsController()
    {
        _context = new MyDbContext();
        _httpClient = new HttpClient();
    }

    public IActionResult IndexAdmin()
    {
        return View();
    }


//Listagem de Areas Profissionais com API
    public async Task<IActionResult>  AreaProfissional()
    {
        // Make an API request
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5052/admin/AreaProfissional");

        // Check if the API request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<AreaProfissionalModel>>(apiResponse);
            
            Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }

    /*public IActionResult Edit(Guid? id)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        ViewBag.Id = result.IdAreaProfissional;
        ViewBag.Nome = result.NomeAreaPrfossional;
        //ViewBag.teste = result.Nome;
        //return View("teste");

        
        return View();
    }*/
    public async Task Edit(Guid? id)
    {
        try
        {
            string endpointUrl = $"http://localhost:5052/admin/edit/'{id}'";
            HttpResponseMessage response = await _httpClient.GetAsync(endpointUrl);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(apiResponse);
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the endpoint: {ex.Message}");
            
        }
    }
    
    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string NomeAreaPrfossional)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeAreaPrfossional = NomeAreaPrfossional;
        db.SaveChanges();
        return RedirectToAction("AreaProfissional");
    }

    
}