using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EmbilyServices.Pages
{
    public class IndexModel : PageModel
    {
        readonly IHostingEnvironment _env;
        readonly EmbilyDbContext _ctx;
        readonly IMapper _mapper;
        readonly IMemoryCache _cache;
        readonly IProgramService _programService;
        readonly ILogger<IndexModel> _logger;

        public List<string> JSFiles
        {
            get
            {
                List<string> jsfiles;
                if (!_cache.TryGetValue("jsfiles", out jsfiles))
                {
                    jsfiles = CalcJSFiles();
                    _cache.Set("jsfiles", jsfiles);
                }
                return jsfiles;
            }
        }

        public string StylesFile
        {
            get
            {
                string stylesFile;
                if (!_cache.TryGetValue("stylesfile", out stylesFile))
                {
                    stylesFile = CalcStylesFile();
                    _cache.Set("stylesfile", stylesFile);
                }
                return stylesFile;
            }
        }

        public Embily.Models.Program Theme
        {
            get
            {
                string domain = Request.Host.Host.Replace("www.", "");
                return _programService.GetProgramByDomain(domain);
            }
        }

        public IndexModel(
            IHostingEnvironment env,
            IMapper mapper,
            EmbilyDbContext ctx,
            IMemoryCache cache,
            IProgramService programService,
            ILogger<IndexModel> logger
            )
        {
            _logger = logger;
            _env = env;
            _ctx = ctx;
            _mapper = mapper;
            _cache = cache;
            _programService = programService;

            _logger.LogInformation("constructor complete");
        }

        public void OnGet()
        {
        }

        List<string> CalcJSFiles()
        {
            var jsFiles = new List<string>();

            List<string> bundles = new List<string>
            {
                "runtime",
                "polyfills",
                "main",
            };

            if (_env.IsDevelopment())
            {
                bundles.Add("styles");
                bundles.Add("vendor");

                bundles.ForEach(b => jsFiles.Add(b + ".js"));

                return jsFiles;
            }

            var root = Path.Combine(_env.ContentRootPath, "ClientApp", "dist");
            var files = Directory.EnumerateFiles(root, "*.js");

            foreach (var bundleName in bundles)
            {
                foreach (var filename in files)
                {
                    var fileInfo = new FileInfo(filename);
                    if (fileInfo.Name.Contains(bundleName))
                    {
                        jsFiles.Add(fileInfo.Name);
                    }
                }
            }

            _logger.LogInformation("JSFiles bundle complete");

            return jsFiles;
        }

        string CalcStylesFile()
        {
            if (_env.IsDevelopment())
            {
                return String.Empty;
            }

            var root = Path.Combine(_env.ContentRootPath, "ClientApp", "dist");

            var files = Directory.EnumerateFiles(root, "styles.*.css");
            foreach (var filename in files)
            {
                var fileInfo = new FileInfo(filename);
                _logger.LogInformation("CalcStylesFile complete");
                return fileInfo.Name;
            }

            _logger.LogInformation("CalcStylesFile complete, styles not found!");
            return String.Empty;
        }
    }
}