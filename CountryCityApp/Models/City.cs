using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CountryCityApp.Models
{
    public class City
    {
        [Key]
        public int Id { set; get; }
        [Required]
        [DisplayName("City")]
        [Remote("CheckName", "City", ErrorMessage = "Name must be unique")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string About { set; get; }
        [Required]
        [DisplayName("No Of Dwellers")]
        public int NoOfDwellers { set; get; }
          [Required]
        public string Location { set; get; }
        [Required]
        public string Weather { set; get; }
        public int CountryId { set; get; }
        public virtual Country aCountry { set; get; }
    }
}