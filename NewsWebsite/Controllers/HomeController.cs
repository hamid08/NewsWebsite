using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using NewsWebsite.ViewModels.ContactUs;
using NewsWebsite.ViewModels.Home;
using NewsWebsite.ViewModels.News;
using NewsWebsite.ViewModels.Video;

namespace NewsWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uw;
        private readonly IHttpContextAccessor _accessor;
        private DateTime Start;
        private TimeSpan TimeSpan;

        public HomeController(IUnitOfWork uw, IHttpContextAccessor accessor)
        {
            _uw = uw;
            _accessor = accessor;
        }


        public class SaleViewModel : Sale
        {
            public int SaleCount { get; set; }
        }


        public async Task<IActionResult> Index(string duration, string TypeOfNews)
        {
            //Random rnd = new Random();

            //var sales = new List<Sale>();

            //for (int i = 0; i < 4; i++)
            //{
            //    var terminal = new TransportTerminal()
            //    {
            //        Caption = "ترمینال" + i
            //    };

            //    await _uw._Context.TransportTerminals.AddAsync(terminal);
            //    await _uw._Context.SaveChangesAsync();
            //}



            //for (int i = 0; i < 20; i++)
            //{

            //    var saleNew = new Sale
            //    {
            //        EndMissionDate = DateTime.Now.AddDays(i),
            //        TripStatus = (TripStatus)rnd.Next(0, 3),
            //        TransportTerminalId = rnd.Next(1, 4),
            //        TripType = (TripType)rnd.Next(0, 3),
            //        TotalFare = rnd.Next(0, 18200),


            //    };

            //    await _uw._Context.Sales.AddAsync(saleNew);
            //    await _uw._Context.SaveChangesAsync();

            //    var setlRandNum = rnd.Next(1, 5);

            //    var setlList = new List<SettlementDetail>();

            //    for (var j = 0; j < setlRandNum; j++)
            //    {

            //        setlList.Add(new SettlementDetail
            //        {
            //            SaleId = saleNew.Id,
            //            BenefisheryType = (BenefisheryType)rnd.Next(0, 3),
            //            Value = rnd.Next(1, 11500),

            //        });
            //    }

            //    await _uw._Context.SettlementDetails.AddRangeAsync(setlList);
            //    await _uw._Context.SaveChangesAsync();
            //};



            //int? terminalFilter = null;

            //var sale = _uw._Context.Sales.AsNoTracking();
            //var settlementDetails = _uw._Context.SettlementDetails;




            //var saleData_Db = await (from Setl in settlementDetails

            //                         group Setl by Setl.SaleId into SetlGroup

            //                         select new
            //                         {
            //                             DriverValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Driver).Sum(c => c.Value),

            //                             UnitValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Unit).Sum(c => c.Value),

            //                             CompanyValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Company).Sum(c => c.Value),

            //                             TotalValue = SetlGroup.Sum(c => c.Value),

            //                             SaleId = SetlGroup.Key

            //                         } into SetlGroupSelect

            //                         join SaleDb in sale
            //                       on SetlGroupSelect.SaleId equals SaleDb.Id into bc
            //                         from bct in bc.DefaultIfEmpty()

            //                         where (bct.TripStatus == TripStatus.EndMission)
            //                         where (terminalFilter != null && terminalFilter.Value > 0 ?
            //                         bct.TransportTerminalId == terminalFilter.Value : true)

            //                         orderby bct.Id descending

            //                         select new
            //                         {
            //                             TotalFare = bct.TotalFare,
            //                             DriverValue = SetlGroupSelect.DriverValue,
            //                             UnitValue = SetlGroupSelect.UnitValue,
            //                             CompanyValue = SetlGroupSelect.CompanyValue,
            //                             TotalValue = SetlGroupSelect.TotalValue,

            //                             bct.EndMissionDate

            //                         } into SaleSelect

            //                         group SaleSelect by new
            //                         {
            //                             SaleSelect.EndMissionDate.Value.Date

            //                         } into SaleSelectGroup

            //                         select new
            //                         {
            //                             Date = SaleSelectGroup.Key.Date,
            //                             TotalFare = SaleSelectGroup.Sum(c => c.TotalFare),
            //                             DriverValue = SaleSelectGroup.Sum(c => c.DriverValue),
            //                             UnitValue = SaleSelectGroup.Sum(c => c.UnitValue),
            //                             CompanyValue = SaleSelectGroup.Sum(c => c.CompanyValue),
            //                             TotalValue = SaleSelectGroup.Sum(c => c.TotalValue),
            //                             SaleCount = SaleSelectGroup.Count()
            //                         }

            //                 ).ToListAsync();


            //var saleData_Db = await (from Setl in settlementDetails

            //                         group Setl by Setl.SaleId into SetlGroup

            //                         select new
            //                         {
            //                             DriverValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Driver).Sum(c => c.Value),

            //                             UnitValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Unit).Sum(c => c.Value),

            //                             CompanyValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Company).Sum(c => c.Value),

            //                             TotalValue = SetlGroup.Sum(c => c.Value),

            //                             SaleId = SetlGroup.Key

            //                         } into SetlGroupSelect

            //                         join SaleDb in sale
            //                       on SetlGroupSelect.SaleId equals SaleDb.Id into bc
            //                         from bct in bc.DefaultIfEmpty()

            //                         where (bct.TripStatus == TripStatus.EndMission)
            //                         where (terminalFilter != null && terminalFilter.Value > 0 ?
            //                         bct.TransportTerminalId == terminalFilter.Value : true)

            //                         orderby bct.Id descending

            //                         select new
            //                         {
            //                             TotalFare = bct.TotalFare,
            //                             DriverValue = SetlGroupSelect.DriverValue,
            //                             UnitValue = SetlGroupSelect.UnitValue,
            //                             CompanyValue = SetlGroupSelect.CompanyValue,
            //                             TotalValue = SetlGroupSelect.TotalValue,

            //                             TripType = bct.TripType,
            //                             TerminalId = bct.TransportTerminalId,
            //                             TerminalCaption = bct.TransportTerminal.Caption,

            //                         } into SaleSelect

            //                         group SaleSelect by new
            //                         {
            //                             SaleSelect.TripType,
            //                             SaleSelect.TerminalId,
            //                             SaleSelect.TerminalCaption

            //                         } into SaleSelectGroup

            //                         select new
            //                         {
            //                             TripType = SaleSelectGroup.Key.TripType,
            //                             TerminalId = SaleSelectGroup.Key.TerminalId,
            //                             TransportTerminalCaption = SaleSelectGroup.Key.TerminalCaption,
            //                             TotalFare = SaleSelectGroup.Sum(c => c.TotalFare),
            //                             DriverValue = SaleSelectGroup.Sum(c => c.DriverValue),
            //                             UnitValue = SaleSelectGroup.Sum(c => c.UnitValue),
            //                             CompanyValue = SaleSelectGroup.Sum(c => c.CompanyValue),
            //                             TotalValue = SaleSelectGroup.Sum(c => c.TotalValue),
            //                             SaleCount = SaleSelectGroup.Count()
            //                         }

            //                  ).ToListAsync();


            //var saleData_Db = await (from Setl in settlementDetails

            //                         group Setl by Setl.SaleId into SetlGroup

            //                         select new
            //                         {
            //                             DriverValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Driver).Sum(c => c.Value),

            //                             UnitValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Unit).Sum(c => c.Value),

            //                             CompanyValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Company).Sum(c => c.Value),

            //                             TotalValue = SetlGroup.Sum(c => c.Value),

            //                             SaleId = SetlGroup.Key

            //                         } into SetlGroupSelect

            //                         join SaleDb in sale
            //                         on SetlGroupSelect.SaleId equals SaleDb.Id into bc
            //                         from bct in bc.DefaultIfEmpty()

            //                         where (bct.TripStatus == TripStatus.EndMission)
            //                         where (terminalFilter != null && terminalFilter.Value > 0 ?
            //                         bct.TransportTerminalId == terminalFilter.Value : true)

            //                         orderby bct.Id descending

            //                         select new
            //                         {
            //                             TotalFare = bct.TotalFare,
            //                             DriverValue = SetlGroupSelect.DriverValue,
            //                             UnitValue = SetlGroupSelect.UnitValue,
            //                             CompanyValue = SetlGroupSelect.CompanyValue,
            //                             TotalValue = SetlGroupSelect.TotalValue,

            //                             bct.TransportTerminalId,
            //                             TerminalCaption = bct.TransportTerminal.Caption

            //                         } into SaleSelect

            //                         group SaleSelect by new
            //                         {
            //                             SaleSelect.TransportTerminalId,
            //                             SaleSelect.TerminalCaption

            //                         } into SaleSelectGroup

            //                         select new
            //                         {
            //                             TerminalCaption = SaleSelectGroup.Key.TerminalCaption,
            //                             TransportTerminalId = SaleSelectGroup.Key.TransportTerminalId,
            //                             TotalFare = SaleSelectGroup.Sum(c => c.TotalFare),
            //                             DriverValue = SaleSelectGroup.Sum(c => c.DriverValue),
            //                             UnitValue = SaleSelectGroup.Sum(c => c.UnitValue),
            //                             CompanyValue = SaleSelectGroup.Sum(c => c.CompanyValue),
            //                             TotalValue = SaleSelectGroup.Sum(c => c.TotalValue),
            //                             SaleCount = SaleSelectGroup.Count()
            //                         }

            //                ).ToListAsync();


            //var saleData_Db = await (from Setl in settlementDetails

            //                         group Setl by Setl.SaleId into SetlGroup

            //                         select new
            //                         {
            //                             DriverValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Driver).Sum(c => c.Value),

            //                             UnitValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Unit).Sum(c => c.Value),

            //                             CompanyValue = SetlGroup
            //                            .Where(c => c.BenefisheryType == BenefisheryType.Company).Sum(c => c.Value),

            //                             TotalValue = SetlGroup.Sum(c => c.Value),

            //                             SaleId = SetlGroup.Key

            //                         } into SetlGroupSelect

            //                         join SaleDb in sale
            //                         on SetlGroupSelect.SaleId equals SaleDb.Id into bc
            //                         from bct in bc.DefaultIfEmpty()


            //                         where (bct.TripStatus == TripStatus.EndMission)
            //                         where (terminalFilter != null && terminalFilter.Value > 0 ?
            //                         bct.TransportTerminalId == terminalFilter.Value : true)

            //                         orderby bct.Id descending

            //                         select new
            //                         {
            //                             TotalFare = bct.TotalFare,
            //                             DriverValue = SetlGroupSelect.DriverValue,
            //                             UnitValue = SetlGroupSelect.UnitValue,
            //                             CompanyValue = SetlGroupSelect.CompanyValue,
            //                             TotalValue = SetlGroupSelect.TotalValue,

            //                             bct.TransportTerminalId,
            //                             TerminalCaption = bct.TransportTerminal.Caption,
            //                             bct.EndMissionDate

            //                         } into SaleSelect

            //                         group SaleSelect by new
            //                         {
            //                             SaleSelect.TransportTerminalId,
            //                             SaleSelect.TerminalCaption,
            //                             SaleSelect.EndMissionDate.Value.Date

            //                         } into SaleSelectGroup

            //                         select new
            //                         {
            //                             TerminalCaption = SaleSelectGroup.Key.TerminalCaption,
            //                             TransportTerminalId = SaleSelectGroup.Key.TransportTerminalId,
            //                             Date = SaleSelectGroup.Key.Date,
            //                             TotalFare = SaleSelectGroup.Sum(c => c.TotalFare),
            //                             DriverValue = SaleSelectGroup.Sum(c => c.DriverValue),
            //                             UnitValue = SaleSelectGroup.Sum(c => c.UnitValue),
            //                             CompanyValue = SaleSelectGroup.Sum(c => c.CompanyValue),
            //                             TotalValue = SaleSelectGroup.Sum(c => c.TotalValue),
            //                             SaleCount = SaleSelectGroup.Count()
            //                         }

            //               ).ToListAsync();



            //var TotalFare = saleData_Db.Sum(c => c.TotalFare);
            //var DriverValue = saleData_Db.Sum(c => c.DriverValue);
            //var UnitValue = saleData_Db.Sum(c => c.UnitValue);
            //var CompanyValue = saleData_Db.Sum(c => c.CompanyValue);
            //var TotalValue = saleData_Db.Sum(c => c.TotalValue);
            //var SaleCount = saleData_Db.Sum(c => c.SaleCount);

            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var siteVisitDb = await _uw._Context.SiteVisits.FirstOrDefaultAsync(c =>
            c.IpAddress == ipAddress && c.VisitDateTime.Date == DateTime.UtcNow.Date);

            if (siteVisitDb == null)
            {
                var siteVisit = new SiteVisit()
                {
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    VisitDateTime = DateTime.UtcNow,

                };
                await _uw._Context.SiteVisits.AddAsync(siteVisit);
                await _uw.Commit();
            }


            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax && TypeOfNews == "MostViewedNews")
                return PartialView("_MostViewedNews", await _uw.NewsRepository.MostViewedNews(0, 3, duration));


            else if (isAjax && TypeOfNews == "MostTalkNews")
                return PartialView("_MostTalkNews", await _uw.NewsRepository.MostTalkNews(0, 5, duration));

            else
            {
                int countNewsPublished = _uw.NewsRepository.CountNewsPublished();
                var news = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, null);
                news = news.Where(c => c.IsConfirm).ToList();
                
                var mostViewedNews = await _uw.NewsRepository.MostViewedNews(0, 3, "day");
                var mostTalkNews = await _uw.NewsRepository.MostTalkNews(0, 5, "day");
                var mostPopulerNews = await _uw.NewsRepository.MostPopularNews(0, 5);
                var internalNews = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, true);
                internalNews = internalNews.Where(c => c.IsConfirm).ToList();

                 var foreignNews = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, false);
                foreignNews = foreignNews.Where(c => c.IsConfirm).ToList();


                var videos = _uw.VideoRepository.GetPaginateVideos(0, 10, item => "", item => item.PersianPublishDateTime, "");
                videos = videos.Where(c => c.IsConfirm).ToList();


                var homePageViewModel = new HomePageViewModel(news, mostViewedNews, mostTalkNews, mostPopulerNews, internalNews, foreignNews, videos, countNewsPublished);
                return View(homePageViewModel);
            }


        }

        [Route("News/{newsId}/{url}")]
        public async Task<IActionResult> NewsDetails(string newsId, string url)
        {
            var cNews = await _uw._Context.News.FirstOrDefaultAsync(c=> c.NewsId == newsId && c.IsConfirm);

            if (cNews == null) return NotFound();

            string ipAddress = _accessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
            Visit visit = _uw.BaseRepository<Visit>().FindByConditionAsync(n => n.NewsId == newsId && n.IpAddress == ipAddress).Result.FirstOrDefault();
            if (visit != null && visit.LastVisitDateTime.Date != DateTime.Now.Date)
            {
                visit.NumberOfVisit = visit.NumberOfVisit + 1;
                visit.LastVisitDateTime = DateTime.Now;
                await _uw.Commit();
            }
            else if (visit == null)
            {
                visit = new Visit { IpAddress = ipAddress, LastVisitDateTime = DateTime.Now, NewsId = newsId, NumberOfVisit = 1 };
                await _uw.BaseRepository<Visit>().CreateAsync(visit);
                await _uw.Commit();
            }

            var news = await _uw.NewsRepository.GetNewsById(newsId);
            var newsComments = await _uw.NewsRepository.GetNewsCommentsAsync(newsId);
            var nextAndPreviousNews = await _uw.NewsRepository.GetNextAndPreviousNews(news.PublishDateTime);
            var newsRelated = await _uw.NewsRepository.GetRelatedNews(2, news.TagIdsList, newsId);
            var newsDetailsViewModel = new NewsDetailsViewModel(news, newsComments, newsRelated, nextAndPreviousNews);
            return View(newsDetailsViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetNewsPaginate(int limit, int offset)
        {
            int countNewsPublished = _uw.NewsRepository.CountNewsPublished();
            var news = await _uw.NewsRepository.GetPaginateNews(offset, limit, item => "", item => item.First().PersianPublishDate, "", true, null);
            news = news.Where(c => c.IsConfirm).ToList();



            return PartialView("_NewsPaginate", new NewsPaginateViewModel(countNewsPublished, news));
        }


        public async Task<IActionResult> NewsInSearch(string search)
        {
            var news = await _uw.NewsRepository.GetNewsBySearch(search);

            ViewBag.Search = search;

            var mostTalkNews = await _uw.NewsRepository.MostTalkNews(0, 5, "day");
            var mostPopulerNews = await _uw.NewsRepository.MostPopularNews(0, 5);

            var homePageViewModel = new HomePageViewModel(news, null, mostTalkNews, mostPopulerNews, null, null, null, 0);
            return View("NewsInSearch", homePageViewModel);


        }



        [Route("Category/{categoryId}/{url}")]
        public async Task<IActionResult> NewsInCategory(string categoryId, string url)
        {
            if (!categoryId.HasValue())
                return NotFound();
            else
            {
                var category = await _uw.BaseRepository<Category>().FindByIdAsync(categoryId);
                if (category == null)
                    return NotFound();
                else
                {
                    ViewBag.Category = category.CategoryName;
                    //return View("NewsInCategoryAndTag", await _uw.NewsRepository.GetNewsInCategoryAndTag(categoryId, ""));
                    var news = await _uw.NewsRepository.GetNewsInCategoryAndTag(categoryId, "");

                    var mostTalkNews = await _uw.NewsRepository.MostTalkNews(0, 5, "day");
                    var mostPopulerNews = await _uw.NewsRepository.MostPopularNews(0, 5);

                    var homePageViewModel = new HomePageViewModel(news, null, mostTalkNews, mostPopulerNews, null, null, null, 0);
                    return View("NewsInCategoryAndTag", homePageViewModel);

                }



            }
        }

        [Route("Tag/{tagId}")]
        public async Task<IActionResult> NewsInTag(string tagId)
        {
            if (!tagId.HasValue())
                return NotFound();
            else
            {
                var tag = await _uw.BaseRepository<Tag>().FindByIdAsync(tagId);
                if (tag == null)
                    return NotFound();
                else
                {
                    ViewBag.Tag = tag.TagName;
                    //return View("NewsInCategoryAndTag", await _uw.NewsRepository.GetNewsInCategoryAndTag("", tagId));

                    var news = await _uw.NewsRepository.GetNewsInCategoryAndTag("", tagId);

                    var mostTalkNews = await _uw.NewsRepository.MostTalkNews(0, 5, "day");
                    var mostPopulerNews = await _uw.NewsRepository.MostPopularNews(0, 5);

                    var homePageViewModel = new HomePageViewModel(news, null, mostTalkNews, mostPopulerNews, null, null, null, 0);
                    return View("NewsInCategoryAndTag", homePageViewModel);
                }
            }
        }

        [Route("Videos")]
        public async Task<IActionResult> Videos()
        {
            var vidoes = await _uw.BaseRepository<Video>().FindAllAsync();
            vidoes = vidoes.Where(c => c.IsConfirm).ToList();

            return View(vidoes);
        }

        [Route("Video/{videoId}")]
        public async Task<IActionResult> VideoDetails(string videoId)
        {
            if (!videoId.HasValue())
                return NotFound();
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(videoId);
                if (video == null || !video.IsConfirm)
                    return NotFound();
                else
                    return View(video);
            }
        }


        public async Task<IActionResult> ContactUs()
        {
            return View();

        }



        public async Task<IActionResult> SendMessage(MessageViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    viewModel.PostageDateTime = DateTime.Now;

                    var contactUs = new ContactUs()
                    {
                        Desription = viewModel.Desription,
                        Email = viewModel.Email,
                        Name = viewModel.Name,
                        PostageDateTime = viewModel.PostageDateTime.Value
                    };

                    await _uw.BaseRepository<ContactUs>().CreateAsync(contactUs);
                    await _uw.Commit();
                    TempData["notification"] = "پیام شما با موفقیت ارسال شد.";
                }
                return PartialView("_SendMessage", viewModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}