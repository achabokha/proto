using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using EmbilyServices.Controllers.Api.v2.Models;
using Embily.Services;

namespace EmbilyServices.Controllers.Api.v2
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/v2/[controller]"), Produces("application/json"), ApiController, SwaggerTag]
    public class DocumentsController : ApiBaseController
    {
        private readonly Embily.Models.EmbilyDbContext _context;
        private readonly IMapper _mapper;

        public DocumentsController(Embily.Models.EmbilyDbContext context, IMapper mapper, IProgramService programService)
                        : base(programService)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Documents
        /// <summary>
        /// All Documents
        /// </summary>
        /// <remarks>Retrieves all documents for the program.</remarks>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Document> GetDocuments()
        {
            var programId = this.GetProgramId();

            // TODO: need to test it --
            var docs = _context.Documents.Include(d => d.Application).ThenInclude(a => a.User).Where(d => d.Application.User.ProgramId == programId); 
            //var docs = _context.Documents;
            var documents = _mapper.Map<IEnumerable<Document>>(docs);
            return documents;
        }

        // GET: api/Documents/5
        /// <summary>
        /// Document Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doc = await _context.Documents.FindAsync(id);

            if (doc == null)
            {
                return NotFound();
            }

            var document = _mapper.Map<Document>(doc);
            document.ImageBase64 = doc.Base64;

            return Ok(document);
        }

        // PUT: api/Documents/5
        /// <summary>
        /// Update Document
        /// </summary>
        /// <remarks>Documents for processed or in-process application cannot be updated.</remarks>
        /// <param name="id"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument([FromRoute] string id, [FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.DocumentId)
            {
                return BadRequest();
            }

            var doc = _mapper.Map<Embily.Models.Document>(document);
            
            _context.Entry(doc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/Documents
        /// <summary>
        /// Create Document
        /// </summary>
        /// <remarks>
        /// Use this API to add additional documents to an application
        /// </remarks>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostDocument([FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doc = _mapper.Map<Embily.Models.Document>(document);

            _context.Documents.Add(doc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.DocumentId }, document);
        }

        //// DELETE: api/Documents/5
        /// <summary>
        /// Delete Document
        /// </summary>
        /// <remarks>Documents for processed or in-process application cannot be deleted.</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(document);
        }

        private bool DocumentExists(string id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}