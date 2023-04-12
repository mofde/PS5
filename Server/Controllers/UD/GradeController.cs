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
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : BaseController
    {
        public GradeController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }
        [HttpGet]
        [Route("GetGrade")]
        public async Task<IActionResult> GetGrade()
        {
            List<GradeDTO> lst = await _context.Grades
                .Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetGrade/{_SchoolId}")]
        public async Task<IActionResult> GetGrade(int _SchoolId)
        {
            var lst = await _context.Grades
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostGrades")]
        public async Task<IActionResult> PostGrades([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade grade = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId).FirstOrDefaultAsync();

                if (grade == null)
                {
                    grade = new Grade
                    {
                        SchoolId = _GradeDTO.SchoolId,
                        StudentId = _GradeDTO.StudentId,
                        SectionId = _GradeDTO.SectionId,
                        GradeTypeCode = _GradeDTO.GradeTypeCode,
                        GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                        NumericGrade = _GradeDTO.NumericGrade,
                        Comments = _GradeDTO.Comments
                    };
                    _context.Grades.Add(grade);
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
        [Route("PutGrades")]
        public async Task<IActionResult> PutGrades([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                Grade grade = await _context.Grades.Where(x => x.SchoolId == _GradeDTO.SchoolId).FirstOrDefaultAsync();

                if (grade != null)
                {
                    grade.SchoolId = _GradeDTO.SchoolId;
                    grade.StudentId = _GradeDTO.StudentId;
                    grade.SectionId = _GradeDTO.SectionId;
                    grade.GradeTypeCode = _GradeDTO.GradeTypeCode;
                    grade.GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence;
                    grade.NumericGrade = _GradeDTO.NumericGrade;
                    grade.Comments = _GradeDTO.Comments;

                    _context.Grades.Update(grade);
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
        [Route("DeleteGrade/{_SchoolId}")]
        public async Task<IActionResult> DeleteGrade(int _SchoolId)
        {
            try
            {
                Course c = await _context.Courses.Where(x => x.CourseNo == _SchoolId).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.Courses.Remove(c);
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
