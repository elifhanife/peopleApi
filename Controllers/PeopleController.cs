using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using peopleApi.Models;
using peopleApi.Services;


namespace peopleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public PeopleController(MongoDBService mongoDBService) =>
        _mongoDBService = mongoDBService;


    [HttpGet]
    public async Task<List<People>> Get() =>
        await _mongoDBService.GetAsync();


    [HttpGet("{id:length(11)}")]
    public async Task<ActionResult<People>> Get(string id)
    {
        var person = await _mongoDBService.GetAsync(id);

        if (person is null)
        {
            return NotFound();
        }

        return person;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] People newPerson) {

        await _mongoDBService.CreateAsync(newPerson);

        return CreatedAtAction(nameof(Get), new { id = newPerson.Id }, newPerson);
    }

    [HttpPut("{id:length(11)}")]
    public async Task<IActionResult> Put(string id, People person) {

        var currentPerson = await _mongoDBService.GetAsync(id);

        if (currentPerson is null)
        {
            return NotFound();
        }

        person.Id = currentPerson.Id;

        var updatedPerson = await _mongoDBService.UpdateAsync(id, person);

        if (updatedPerson != null)
        {
            return Ok(updatedPerson);
        }
        else
        {
            return NoContent();
        }
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteAll()
    {
        await _mongoDBService.DeleteAsync();
        return NoContent();
    }

    [HttpDelete("{id:length(11)}")]
    public async Task<IActionResult> Delete(string id) {
        
        var person = await _mongoDBService.GetAsync(id);

        if (person is null)
        {
            return NotFound();
        }

        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:length(11)}")]
    public async Task<IActionResult> PatchAsync(string id, [FromBody] JsonPatchDocument<People> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var existingItem = await _mongoDBService.GetAsync(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(existingItem, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _mongoDBService.UpdateAsync(id,existingItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }
}



