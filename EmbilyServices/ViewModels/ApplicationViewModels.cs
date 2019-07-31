using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.ViewModels
{
    public class PassportViewModel
    {
        [Required]
        public string ApplicationId { get; set; }

        // do we need this one? currency code known right away --
        //[Required] 
        //public string CurrencyCode { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public PassportDateOfBirth DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string MaritalStatus { get; set; }

        [Required]
        public string Nationality { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }

    public class PassportDateOfBirth
    {
        public PassportDateOfBirth() { }
        public PassportDateOfBirth(string dateOfBirth)
        {
            if (!string.IsNullOrEmpty(dateOfBirth))
            {
                string[] words = dateOfBirth.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                year = Convert.ToInt32(words[0]);
                month = Convert.ToInt32(words[1]);
                day = Convert.ToInt32(words[2]);
            }           
        }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
    }

    public class UploadFileViewModel
    {
        [Required]
        public string ApplicationId { get; set; }

        [Required]
        public string DocumentType { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string DocumentId { get; set; }
    }


    public class ShippingViewModel
    {
        [Required]
        public string ApplicationId { get; set; }

        public int SelectedShippingOption { get; set; }

        public ShippingDetail ShippingDetail { get; set; }
        
        public Address Address { get; set; }
    }

}
