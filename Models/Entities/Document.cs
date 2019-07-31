using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Embily.Models
{
    public class Document : BaseEntity
    {
        public string DocumentId { get; set; }

        [Column("DocumentType")]
        public string DocumentTypeString
        {
            get { return DocumentType.ToString(); }
            set { DocumentType = value.ParseEnum<DocumentTypes>(); }
        }

        [NotMapped]
        public DocumentTypes DocumentType { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [NotMapped]
        public string ImageSrcBase64
        {
            get
            {
                //src="data: image / gif; base64,..."
                return this.Image != null ? $"data:{FileType};base64," + Convert.ToBase64String(this.Image) : null;
            }
        }

        [NotMapped]
        public string Base64
        {
            get
            {
                return this.Image != null ? Convert.ToBase64String(this.Image) : null;
            }
        }

        [Required]
        public string ApplicationId { get; set; }

        public Application Application { get; set; }
    }
}
