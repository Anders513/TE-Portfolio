using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkGeek.DAL.Models;

namespace ParkGeekMVC.Models
{
    public class SurveyViewModel
    {

        [Required]
        public string ParkCode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ActivityLevel { get; set; }

        public IList<SelectListItem> ParkCodeList { get; set; } = new List<SelectListItem>(); 

        public IList<SelectListItem> StateResidenceList { get; } = new List<SelectListItem>()
        {
        new SelectListItem() { Text="AL"},
        new SelectListItem() { Text="AK"},
        new SelectListItem() { Text="AZ"},
        new SelectListItem() { Text="AR"},
        new SelectListItem() { Text="CA"},
        new SelectListItem() { Text="CO"},
        new SelectListItem() { Text="CT"},
        new SelectListItem() { Text="DC"},
        new SelectListItem() { Text="DE"},
        new SelectListItem() { Text="FL"},
        new SelectListItem() { Text="GA"},
        new SelectListItem() { Text="HI"},
        new SelectListItem() { Text="ID"},
        new SelectListItem() { Text="IL"},
        new SelectListItem() { Text="IN"},
        new SelectListItem() { Text="IA"},
        new SelectListItem() { Text="KS"},
        new SelectListItem() { Text="KY"},
        new SelectListItem() { Text="LA"},
        new SelectListItem() { Text="ME"},
        new SelectListItem() { Text="MD"},
        new SelectListItem() { Text="MA"},
        new SelectListItem() { Text="MI"},
        new SelectListItem() { Text="MN"},
        new SelectListItem() { Text="MS"},
        new SelectListItem() { Text="MO"},
        new SelectListItem() { Text="MT"},
        new SelectListItem() { Text="NE"},
        new SelectListItem() { Text="NV"},
        new SelectListItem() { Text="NH"},
        new SelectListItem() { Text="NJ"},
        new SelectListItem() { Text="NM"},
        new SelectListItem() { Text="NY"},
        new SelectListItem() { Text="NC"},
        new SelectListItem() { Text="ND"},
        new SelectListItem() { Text="OH"},
        new SelectListItem() { Text="OK"},
        new SelectListItem() { Text="OR"},
        new SelectListItem() { Text="PA"},
        new SelectListItem() { Text="RI"},
        new SelectListItem() { Text="SC"},
        new SelectListItem() { Text="SD"},
        new SelectListItem() { Text="TN"},
        new SelectListItem() { Text="TX"},
        new SelectListItem() { Text="UT"},
        new SelectListItem() { Text="VT"},
        new SelectListItem() { Text="VA"},
        new SelectListItem() { Text="WA"},
        new SelectListItem() { Text="WV"},
        new SelectListItem() { Text="WI"},
        new SelectListItem() { Text="WY"}
    };
    }
}
