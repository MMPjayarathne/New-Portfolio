using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace backend.Controllers
{
    [Route("api/personalinfo")]
    [ApiController]
    public class PersonalInfoController : ControllerBase
    {
        private readonly ILogger<PersonalInfoController> _logger;
        private readonly SqlDbContext _context;

        public PersonalInfoController(ILogger<PersonalInfoController> logger, SqlDbContext sqlDbContext)
        {
            _logger = logger;
            _context = sqlDbContext;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                await _context.Database.OpenConnectionAsync();
                return Ok("Database connection successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database connection failed: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IEnumerable<PersonalInfo>> GetPersonalInfo() {
            _logger.LogInformation("Request to get personal infos");
            try
            {
                return await _context.PersonalInfos.ToListAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"There is an error when trying to retreive personal infos {ex}");
                return Enumerable.Empty<PersonalInfo>();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetPersonalInfo(PersonalInfo personalInfo) {
            _logger.LogInformation("Request to add a new personal info");

            if (personalInfo.Identifier == null || personalInfo.Content == null) {
                _logger.LogWarning("Empty body! Canceling the request");
                return BadRequest("No attribute in the body cannot be null");
            }

            //chaeck whether the identifier is already exisitng
            var isExisting = await _context.PersonalInfos.Where(e => e.Identifier == personalInfo.Identifier).ToListAsync();
            if (isExisting.Any()) {
                _logger.LogWarning("Confict to add the personalInfo");
                return Conflict("The given Identifier is already existing. Try update!");
            }
            try
            {
                _context.Add(personalInfo);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added the personalInfo");
                return Ok(personalInfo);
            }
            catch (Exception ex) {
                _logger.LogError($"There is an error when trying to retreive personal infos {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"There is an error from the application side : {ex.Message}");
            }
        }

        [HttpGet("{identifier}")]
        public async Task<IEnumerable<PersonalInfo>> GetPersonalInfoByIdentifier(String identifier) {
            _logger.LogInformation($"Request to get {identifier}");
            return await _context.PersonalInfos.Where(x => x.Identifier == identifier ).ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> PutPersonalInfo(PersonalInfo personalInfo) {
            //check whether the body is empty
            if (personalInfo == null || personalInfo.Identifier == null || personalInfo.Content == null) {
                _logger.LogWarning("The replacing request cannot be done due to empty attributes.");
                return BadRequest("No attribute in the body cannot be null");

            }

            //check whether the identifier is existing
            var existingRecord = await _context.PersonalInfos.SingleOrDefaultAsync(x => x.Identifier == personalInfo.Identifier);
            _logger.LogInformation($"Request to replace a {personalInfo.Identifier}");
            if (existingRecord == null) {
                _logger.LogWarning("There is no any matching record for the given Identifier");
                return NotFound("There is no any matching record for the given Identifier");
            }
            try
            {
                // Update the existing record with the new values
                existingRecord.Content = personalInfo.Content;
                existingRecord.Identifier = personalInfo.Identifier;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully replace the {personalInfo.Identifier}");
                return Ok(personalInfo);
            }
            catch (DbUpdateConcurrencyException ex) {
                _logger.LogError($"There is an error when trying to replace the personal info {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"There is an error from the application side : {ex.Message}");
            }
             
        }
        [HttpDelete("{identifier}/{id?}")]
        public async Task<IActionResult> DeletePersonalInfo(string identifier, int? id = null) {
            _logger.LogInformation($"Request to delete the identifire: {identifier}");

            if (string.IsNullOrEmpty(identifier)) {
                _logger.LogWarning("Canceling the delete request due to empty identifier! ");
                return BadRequest("The identidfier cannot be null!");
                   
            }
            try
            {
               
                PersonalInfo recordToDelete;

                //check whether the identifier is existing
                var matchingRecords = await _context.PersonalInfos.Where(x => x.Identifier == identifier).ToListAsync();
                if (matchingRecords.Count == 0)
                {
                    _logger.LogWarning($"There is no any matching identifier for {identifier}. Canceling the request!");
                    return BadRequest($"There is no any matching identifier for {identifier}");
                }
                else if (matchingRecords.Count > 1 && id.HasValue)
                {
                    //if an Id is provided
                    recordToDelete = matchingRecords.SingleOrDefault(x => x.Id == id.Value);
                    if (recordToDelete == null)
                    {
                        _logger.LogWarning($"No record found with the provided id: {id} for the identifier: {identifier}. Canceling the request.");
                        return NotFound(new
                        {
                            Message = $"No record found with the provided id: {id} for the identifier: {identifier}. Refer below records and request!",
                            Records = matchingRecords.Select(record => new { record.Id, record.Identifier, record.Content })

                        });
                    }

                }
                else if (matchingRecords.Count > 1 && id == null)
                {
                    _logger.LogWarning($"Multiple records found for the Identifier:{identifier}");
                    return Conflict(new
                    {
                        Message = $"Multiple records found for the Identifier:{identifier} Please request with the id as a path parameter to identify.",
                        Records = matchingRecords.Select(record => new { record.Id, record.Identifier, record.Content })
                    }
                     );
                }
                else {
                    recordToDelete = matchingRecords.Single();
                }
                

                _context.PersonalInfos.Remove(recordToDelete);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully removed the record!");
                return Ok("Successfully removed!");
            }
            catch (Exception ex) {
                _logger.LogError($"There is an error when trying to delete the personal info {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"There is an error from the application side : {ex.Message}");

            }

        }

    }
}
