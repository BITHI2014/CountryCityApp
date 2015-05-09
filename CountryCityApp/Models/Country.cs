using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CountryCityApp.Models
{
    public class Country
    {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
        public string Images { set; get; }

        [UIHint("tinymce_full_compressed"), AllowHtml]
        public string About { set; get; }
        [Required]
        [DisplayName("No Of Dwellers")]
        public int NoOfDwellers { set; get; }
        [Required]
        [DisplayName("No Of Cities")]
        public int NoOfCities { set; get; }

    }
}