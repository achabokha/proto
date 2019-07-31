using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers.Api.v2.Models
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<Embily.Models.Document, Document>()
                .ForMember(dest => dest.ImageBase64, opts => opts.MapFrom(src => "[removed]"))
                ;
            CreateMap<Document, Embily.Models.Document>()
                 .ForMember(dest => dest.Image, opts => opts.MapFrom(src => Convert.FromBase64String(src.ImageBase64)))
                ;
        }
    }

    public class Document
    {
        public string DocumentId { get; set; }

        public string ApplicationId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Embily.Models.DocumentTypes DocumentType { get; set; }

        public string FileType { get; set; }

        public string ImageBase64 { get; set; }
    }

}
