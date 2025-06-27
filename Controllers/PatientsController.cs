using DapperWebAPI.Data;
using DapperWebAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize] 
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientRepository _patientRepo;
    private readonly IMealPlanRepository _mealPlanRepo;

    public PatientsController(IPatientRepository patientRepo, IMealPlanRepository mealPlanRepo)
    {
        _patientRepo = patientRepo;
        _mealPlanRepo = mealPlanRepo;
    }

    [HttpGet("{id}/mealplans/today")]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> GetTodayMealPlan(long id)
    {
        var plan = await _mealPlanRepo.GetTodayMealPlanAsync(id);
        if (plan == null)
            return NotFound("Plano alimentar não encontrado para hoje.");

        return Ok(plan);
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientRepo.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> Get(long id)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> Create([FromBody] Patients patient)
    {
        var id = await _patientRepo.InsertAsync(patient);
        return CreatedAtAction(nameof(Get), new { id }, patient);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> Update(long id, [FromBody] Patients patient)
    {
        patient.Id = id;
        return await _patientRepo.UpdateAsync(patient) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN,NUTRITIONIST")]
    public async Task<IActionResult> Delete(long id)
    {
        return await _patientRepo.DeleteAsync(id) ? NoContent() : NotFound();
    }

}
