using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class UpdateProfileCardRequest : BaseAuthRequest
    {
        [JsonProperty("CardNumber")]
        public string CardNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Format : DD/MM/YYYY
        /// </summary>
        public string DateOfBirth { get; set; }

        public string Email1 { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        /// <summary>
        /// Mandatory 
        /// </summary>
        public string Region { get; set; }

        public string PostCode { get; set; }

        /// <summary>
        /// See country list: ISO 3166-1
        /// LVA (LV, 428), USA (US, 840), THA (TH, 764), CAN (CA, 124)
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// not mandatory
        /// Family status. (S-M-D-W) Single, married, divorced, widower.
        /// </summary>
        public string FamilyStatus { get; set; }

        /// <summary>
        /// not madatory
        /// (M-F)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// not madatory
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// not madatory
        ///  (Default:+11111111)
        /// </summary>
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// not madatory
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// not madatory
        /// External transaction ID.
        /// </summary>
        public string ExternalTransaction { get; set; }

        /// <summary>
        /// See nationality list: ISO 3166-1
        /// </summary>
        public string NationalityCode { get; set; }

        /// <summary>
        /// not madatory
        /// MemberID of the client’s program.
        /// </summary>
        [JsonProperty("MemberID")]
        public string MemberID { get; set; }
    }
}