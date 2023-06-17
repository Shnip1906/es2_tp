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
using System.Net;
using System.Text;

namespace Backend.Controllers;

public class AdminsController : Controller
{
    //  [HttpGet] //
    private readonly HttpClient _httpClient;

    public AdminsController()
    {
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
            
            //Console.WriteLine(data[0].NomeAreaPrfossional);
            
            return View(data);
        }
        else
        {
            ViewBag.erro = "Erro ao receber dados da API!!!";
            return View("ErrorView");
        }
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Invalid ID"); 
        }

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5052/admin/Edit/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                ViewBag.Id = result.id;
                ViewBag.Nome = result.nome;

                Console.WriteLine(result.id);
                return View();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Entity not found");
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string NomeAreaPrfossional)
    {
        var areaProfissionalModel = new AreaProfissionalModel()
        {
            IdAreaProfissional = id,
            NomeAreaPrfossional = NomeAreaPrfossional
        };
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(areaProfissionalModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var result =  await _httpClient.PostAsync("http://localhost:5052/admin/Edits", content);

        // Check if the request was successful
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine("Entity updated successfully");
        }
        else
        {
            Console.WriteLine("Failed to update entity. Response status: " + result.StatusCode);
        }
        
        return RedirectToAction("AreaProfissional");
    }


}