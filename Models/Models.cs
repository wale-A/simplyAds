using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimplyAds.Models
{
    public abstract class ExtraProperties
    {
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime? DateDeleted { get; set; }
    }

    public class DurationPricing : ExtraProperties
    {
        public int ID { get; set; }
        public string Time { get; set; }
        public decimal Amount { get; set; }
        //public int NumberOfDays { get; set; }
    }      

    public class AdUpdate : ExtraProperties
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public int AdvertID { get; set; }
        public Advert Advert { get; set; }
    }

    public class Car : ExtraProperties
    {
        public int ID { get; set; }
        //public string Number { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Number of cars"), Range(1, 100, ErrorMessage = "selected value is invalid")]
        public int NumberOfCars { get; set; }
    }

    public class ContentPricing : ExtraProperties
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class MiscChargePricing : ExtraProperties
    {
        public int ID { get; set; }
        public string Property { get; set; }
        public decimal Amount { get; set; }
    }

    public class Advert
    {
        public Advert()
        {
            AdUpdates = new List<AdUpdate>();
        }

        public int ID { get; set; }
        [Display (Name = "Ref. No")]
        public string ReferenceNo { get; set; }
        [Display(Name = "Trx. Ref.")]
        public string TransactionReference { get; set; }
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Phone Number")]
        public string CustomerPhone { get; set; }
        [DataType(DataType.EmailAddress), Display(Name = "Email Add.")]
        public string CustomerEmail { get; set; }
        [DataType(DataType.Date), Display(Name = "Date Placed")]
        public DateTime DatePlaced { get; set; }
        public bool Urgent { get; set; }
        public decimal Amount { get; set; }
        public string Duration { get; set; }
        [Display(Name = "No. Of Cars")]
        public string NoOfCars { get; set; }
        [DataType(DataType.Date), Display (Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date), Display(Name = "Start Date")]
        public DateTime? EndDate { get; set; }
        public bool Treated { get; set; }
        public int AdContentID { get; set; }
        public virtual AdContent AdContent { get; set; }
        public bool Paid { get; set; }

        public DateTime? DateTreated { get; set; }
        public string TreatedBy { get; set; }

        public ICollection<AdUpdate> AdUpdates { get; set; }

        public string getBackgroundColor()
        {
            if (Urgent && !Treated)
                return "red";  //urgent and !treated
            else if (!Treated)
                return "#FF4500"; //not treated ORANGE RED
            else if (Treated && StartDate.Value.Date > DateTime.UtcNow.AddHours(1).Date)
                return "#FFae42";   //treated and abt to start      ORANGE YELLOW
            else if (EndDate >= DateTime.UtcNow.AddHours(1) && StartDate <= DateTime.UtcNow.AddHours(1))
                return "yellow";  //running
            else if (EndDate < DateTime.UtcNow.AddHours(1))
                return "green"; //done
            else return "";
        }
    }

    public class AdContent
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public DateTime? DateAdded { get; set; }
    }

    public class AdvertViewModel
    {
        [Required]
        public string CustomerName { get; set; }
        [Required, RegularExpression(@"\d{11,13}", ErrorMessage = "please enter valid phone number e.g 08081112222")]
        public string CustomerPhone { get; set; }
        //[Required]
        public bool Urgent { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string NoOfCars { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.EmailAddress), Required]
        public string CustomerEmail { get; set; }
    }

    /// <summary>
    /// outside the data context
    /// </summary>

    public class AjaxAdModel
    {
        public string Duration { get; set; }
        public string NoOfCars { get; set; }
        public bool Urgent { get; set; }
        public string ContentExtension { get; set; }
    }

    public class CostOfAdvert
    {
        public decimal CarCharge { get; set; }
        public decimal DurationCharge { get; set; }
        public decimal UrgentAdCharge { get; set; }
        public decimal ContentChage { get; set; }

        public decimal GetTotalCharge()
        {
            return CarCharge + DurationCharge + UrgentAdCharge + ContentChage;
        }
    }

    public class BreakDown
    {
        public int ID { get; set; }
        public int Counter { get; set; }
        
    }
}