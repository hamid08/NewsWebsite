using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NewsWebsite.ViewModels.Video
{
    public class VideoViewModel
    {
        [JsonProperty("Id")]
        public string VideoId { get; set; }

        [JsonProperty("ردیف")]
        public int Row { get; set; }

        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [JsonProperty("فایل ویدیو")]
        [Display(Name ="فایل ویدیو")]
        public IFormFile VideoFile { get; set; }

        [JsonProperty("IsConfirm")]
        public bool IsConfirm { get; set; }

        [JsonProperty("عنوان ویدیو"),Display(Name ="عنوان ویدیو")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Title { get; set; }

        public string Url { get; set; }

        [Display(Name = "پوستر ویدیو"),JsonIgnore]
        //[Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        public IFormFile PosterFile { get; set; }
        
        public string Poster { get; set; }

        [JsonIgnore]
        public DateTime? PublishDateTime { get; set; }

        [JsonProperty("تاریخ انتشار")]
        public string PersianPublishDateTime { get; set; }
    }
}
