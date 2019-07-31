using Embily.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Embily.Services
{
    public class ProgramService : IProgramService
    {
        readonly EmbilyDbContext _context;
        readonly IMemoryCache _cache;

        public ProgramService(EmbilyDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public Program GetProgramByDomain(string domain)
        {
            if (domain == "localhost")
            {
                //domain = "sandbox.embily.com";
                domain = "services.embily.com";
            }

            Program program;
            if (!_cache.TryGetValue($"program-{domain}", out program))
            {
                program = _context.Programs.SingleOrDefault(p => p.Domain == domain); ;
                _cache.Set($"program-{domain}", program);
            }
            return program;
        }

        public Program GetProgramByUserId(string userId)
        {
            Program program;
            if (!_cache.TryGetValue($"program-{userId}", out program))
            {
                var user = _context.Users.Find(userId);
                program = _context.Programs.Find(user.ProgramId); ;
                _cache.Set($"program-{userId}", program);
            }
            return program;
        }
    }
}
