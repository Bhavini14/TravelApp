using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelApp.Models
{
    public class TravelHeader
    {
        public TravelHeader()
        {
            TravelReasonList = new List<SelectListItem>();
            CurrencyList = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Display(Name = "Travel Reason*")]
        [Required]
        public string TravelReson { get; set; }

        [Display(Name = "Travelling From*")]
        [Required]
        public string TravellingFrom { get; set; }

        [Display(Name = "Travelling To Date*")]
        [Required]
        public string TravellingToDate { get; set; }

        [Display(Name = "Explaination Of Travel*")]
        [Required]
        public string ExplainationTravel { get; set; }
        [Display(Name = "Travelling Details(Plaese mention the Origin and Destination)*")]   
        [Required(ErrorMessage ="Enter Travelling Details")]
        public string TravellingDetails { get; set; }
        public List<SelectListItem> TravelReasonList{ get; set; }
        public List<SelectListItem> CurrencyList { get;  set;}
        public virtual List<TravelPreDetails> TravelPreDetailsdata { get; set; }
    }
}