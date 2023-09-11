using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.Entities.identity;
using NewsWebsite.ViewModels.Comments;
using NewsWebsite.ViewModels.ContactUs;
using NewsWebsite.ViewModels.DynamicAccess;

namespace NewsWebsite.Areas.Admin.Controllers
{
    [DisplayName( "ارتباط با ما")]

    public class ContactUsController : BaseController
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        private const string CommentNotFound = "پیام یافت نشد.";

        public ContactUsController(IUnitOfWork uw, IMapper mapper)
        {
            _uw = uw;
            _uw.CheckArgumentIsNull(nameof(_uw));
            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }

        [HttpGet, DisplayName("مشاهده")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetMessages(string search, string order, int offset, int limit, string sort)
        {
            List <MessageViewModel> comments;
            int total = _uw.BaseRepository<ContactUs>().CountEntities();
            if (!search.HasValue())
                search = "";

            if (limit == 0)
                limit = total;

            if (sort == "نام")
            {
                if (order == "asc")
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit,item=>item.Name , item=>"", search);
                else
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit, item => "", item => item.Name, search);
            }


            else if (sort == "ایمیل")
            {
                if (order == "asc")
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit, item => item.Email, item=>"", search);
                else
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit,item=>"", item => item.Email, search);
            }

            else if (sort == "تاریخ ارسال")
            {
                if (order == "asc")
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit, item => item.PersianPostageDateTime, item => "", search);
                else
                    comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit, item => "", item => item.PersianPostageDateTime, search);
            }

            else
                comments = await _uw.ContactUsRepository.GetPaginateMessages(offset, limit,item=>"",item=>item.PersianPostageDateTime, search);

            if (search != "")
                total = comments.Count();

            return Json(new { total = total, rows = comments });
        }

    }
}