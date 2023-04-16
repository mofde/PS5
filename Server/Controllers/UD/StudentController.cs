using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using DOOR.Server.Controllers.Common;

namespace CSBA6.Server.Controllers.app
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        public StudentController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {
            List<StudentDTO> lst = await _context.Students
                .Select(sp => new StudentDTO
                {
                    StudentId = sp.StudentId,
                    Salutation = sp.Salutation,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip,
                    Phone = sp.Phone,
                    Employer = sp.Employer,
                    RegistrationDate = sp.RegistrationDate,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetStudent/{_StudentId}")]
        [Route("GetStudent/{_SchoolId")]
        public async Task<IActionResult> GetStudent(int _StudentId, int _SchoolId)
        {
            var lst = await _context.Students
                .Where(x => x.StudentId == _StudentId)
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new StudentDTO
                {
                    StudentId = sp.StudentId,
                    Salutation = sp.Salutation,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip,
                    Phone = sp.Phone,
                    Employer = sp.Employer,
                    RegistrationDate = sp.RegistrationDate,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostStudent")]
        public async Task<IActionResult> PostStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student? student = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId)
                                                         .Where(x => x.SchoolId == _StudentDTO.SchoolId).FirstOrDefaultAsync();

                if (student == null)
                {
                    student = new Student
                    {
                        StudentId = _StudentDTO.StudentId,
                        Salutation = _StudentDTO.Salutation,
                        FirstName = _StudentDTO.FirstName,
                        LastName = _StudentDTO.LastName,
                        StreetAddress = _StudentDTO.StreetAddress,
                        Zip = _StudentDTO.Zip,
                        Phone = _StudentDTO.Phone,
                        Employer = _StudentDTO.Employer,
                        RegistrationDate = _StudentDTO.RegistrationDate,
                        SchoolId = _StudentDTO.SchoolId
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }








        [HttpPut]
        [Route("PutStudent")]
        public async Task<IActionResult> PutStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student? student = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId)
                                                         .Where(x => x.SchoolId == _StudentDTO.SchoolId).FirstOrDefaultAsync();

                if (student != null)
                {
                    student.StudentId = _StudentDTO.StudentId;
                    student.Salutation = _StudentDTO.Salutation;
                    student.FirstName = _StudentDTO.FirstName;
                    student.LastName = _StudentDTO.LastName;
                    student.StreetAddress = _StudentDTO.StreetAddress;
                    student.Zip = _StudentDTO.Zip;
                    student.Phone = _StudentDTO.Phone;
                    student.Employer = _StudentDTO.Employer;
                    student.RegistrationDate = _StudentDTO.RegistrationDate;
                    student.SchoolId = _StudentDTO.SchoolId;

                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("DeleteStudent/{_StudentId}")]
        [Route("DeleteStudent/{_SchoolId}")]
        public async Task<IActionResult> DeleteStudent(int _StudentId, int _SchoolId)
        {
            try
            {
                Student? student = await _context.Students.Where(x => x.StudentId == _StudentId)
                                                         .Where(x => x.SchoolId == _SchoolId).FirstOrDefaultAsync();

                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }



    }
}