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

namespace DOOR.Server.Controllers.UD
{
    public class EnrollmentController : BaseController
    {
        public EnrollmentController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetEnrollment")]
        public async Task<IActionResult> GetEnrollment()
        {
            List<EnrollmentDTO> lst = await _context.Enrollments
                .Select(sp => new EnrollmentDTO
                {
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    EnrollDate = sp.EnrollDate,
                    FinalGrade = sp.FinalGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetEnrollment/{_SchoolId}/{_SectionId}/{_StudentId}")]
        public async Task<IActionResult> GetEnrollment(int _SchoolId, int _SectionId, int _StudentId)
        {
            var lst = await _context.Enrollments
                .Where(x => x.SchoolId == _SchoolId)
                .Where(x => x.SectionId == _SectionId)
                .Where(x => x.StudentId == _StudentId)
                .Select(sp => new EnrollmentDTO
                {
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    EnrollDate = sp.EnrollDate,
                    FinalGrade = sp.FinalGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostEnrollment")]
        public async Task<IActionResult> PostCourse([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                Enrollment? enrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                if (enrollment == null)
                {
                    enrollment = new Enrollment
                    {
                        StudentId = _EnrollmentDTO.StudentId,
                        SectionId = _EnrollmentDTO.SectionId,
                        EnrollDate = _EnrollmentDTO.EnrollDate,
                        FinalGrade = _EnrollmentDTO.FinalGrade
                    };
                    _context.Enrollments.Add(enrollment);
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
        [Route("PutEnrollment")]
        public async Task<IActionResult> PutEnrollment([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                Enrollment enrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                if (enrollment != null)
                {
                    enrollment.StudentId = _EnrollmentDTO.StudentId;
                    enrollment.SectionId = _EnrollmentDTO.SectionId;
                    enrollment.EnrollDate = _EnrollmentDTO.EnrollDate;
                    enrollment.FinalGrade = _EnrollmentDTO.FinalGrade;

                    _context.Enrollments.Update(enrollment);
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
        [Route("DeleteEnrollment/{_StudentId}")]
        public async Task<IActionResult> DeleteEnrollment(int _StudentId)
        {
            try
            {
                Enrollment enrollment = await _context.Enrollments.Where(x => x.StudentId == _StudentId).FirstOrDefaultAsync();

                if (enrollment != null)
                {
                    _context.Enrollments.Remove(enrollment);
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
