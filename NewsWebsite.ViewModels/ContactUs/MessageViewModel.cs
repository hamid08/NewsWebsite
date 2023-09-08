using System;
using System.Collections.Generic;
using NewsWebsite.Entities;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.ViewModels.ContactUs
{
    public class MessageViewModel
    {
        public MessageViewModel()
        {

        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("ردیف")]
        public int Row { get; set; }

        [JsonProperty("نام"),Display(Name="نام")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Name { get; set; }


        [JsonProperty("ایمیل"),Display(Name = "ایمیل")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [JsonProperty("دیدگاه") , Display(Name = "دیدگاه")]
        public string Desription { get; set; }

        [JsonIgnore]
        public DateTime? PostageDateTime { get; set; }

        [JsonProperty("تاریخ ارسال")]
        public string PersianPostageDateTime { get; set; }

    }
}
