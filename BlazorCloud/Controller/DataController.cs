using BlazorCloud.Areas.Authorization;
using BlazorCloud.Areas.Identity.Data;
using BlazorCloudCore.Logic.Services;
using BlazorCloudCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloud.Controller
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public DataController(FileAndDirectoryService fileAndDirectoryService, IBasicAuthorization basicAuthorization)
        {
            _fileAndDirectoryService = fileAndDirectoryService;
            _basicAuthorization = basicAuthorization;
        }
        public FileAndDirectoryService _fileAndDirectoryService { get; set; }

        private readonly IBasicAuthorization _basicAuthorization;


        /// <summary>
        /// Returns a list of FileBase objects which contain the name and their corresponding path
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("fileNames/{path}")]
        public async Task<ActionResult<List<FileBase>>> GetFileNamesAsync(string path)
        {
            if (!(await _basicAuthorization.BasicAuthIsValid(HttpContext)))
            {
                return Unauthorized();
            }

            try
            {
                var fileNames = await _fileAndDirectoryService.GetListOfFilesFromPathAsync(path);
                return Ok(fileNames);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
