﻿using DOOR.EF.Data;
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
    public class ZipcodeController : BaseController
    {
        public ZipcodeController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetZipcode")]
        public async Task<IActionResult> GetZipcode()
        {
            List<ZipcodeDTO> lst = await _context.Zipcodes
                .Select(sp => new ZipcodeDTO
                {
                    Zip = sp.Zip,
                    City = sp.City,
                    State = sp.State,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetZipcode/{_Zip}")]
        public async Task<IActionResult> GetZipcode(string _Zip)
        {
            var lst = await _context.Zipcodes
                .Where(x => x.Zip == _Zip)
                .Select(sp => new Zipcode
                {
                    Zip = sp.Zip,
                    City = sp.City,
                    State = sp.State,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostZipcode")]
        public async Task<IActionResult> PostZipcode([FromBody] ZipcodeDTO _ZipcodeDTO)
        {
            try
            {
                Zipcode? zipcode = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstOrDefaultAsync();

                if (zipcode == null)
                {
                    zipcode = new Zipcode
                    {
                        Zip = _ZipcodeDTO.Zip,
                        City = _ZipcodeDTO.City,
                        State = _ZipcodeDTO.State
                    };
                    _context.Zipcodes.Add(zipcode);
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
        [Route("PutZipcode")]
        public async Task<IActionResult> PutZipcode([FromBody] ZipcodeDTO _ZipcodeDTO)
        {
            try
            {
                Zipcode? zipcode = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstOrDefaultAsync();

                if (zipcode != null)
                {
                    zipcode.Zip = _ZipcodeDTO.Zip;
                    zipcode.City = _ZipcodeDTO.City;
                    zipcode.State = _ZipcodeDTO.State;

                    _context.Zipcodes.Update(zipcode);
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
        [Route("DeleteZip/{_Zip}")]
        public async Task<IActionResult> DeleteZip(string _Zip)
        {
            try
            {
                Zipcode? zipcode = await _context.Zipcodes.Where(x => x.Zip == _Zip).FirstOrDefaultAsync();

                if (zipcode != null)
                {
                    _context.Zipcodes.Remove(zipcode);
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