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


            //for (int i = 0; i < 100; i++)
            //{
            //    var saleNew = new Sale
            //    {
            //        DriverValue = rnd.Next(0, 8),
            //        SumTotal = rnd.Next(0, 8),
            //        TotalFare = rnd.Next(0, 8),
            //        TransportationCompanyValue = rnd.Next(0, 8),
            //        TransportationUnitValue = rnd.Next(0, 8),

            //        TripType = (TripType)rnd.Next(0, 2),
            //        TerminalCaption = "Terminal" + i,
            //        TerminalId = 1,


            //    };

            //    await _uw._Context.Sales.AddAsync(saleNew);
            //    await _uw._Context.SaveChangesAsync();

            //    var setlRandNum = rnd.Next(1, 3);

            //    var setlList = new List<SettlementDetail>();

            //    for (var j = 0; j < setlRandNum; j++)
            //    {

            //        setlList.Add(new SettlementDetail
            //        {
            //            SaleId = saleNew.Id,
            //            BenefisheryType = (BenefisheryType)rnd.Next(0, 2),
            //            Value = rnd.Next(5)
            //        });
            //    }

            //    await _uw._Context.SettlementDetails.AddRangeAsync(setlList);
            //    await _uw._Context.SaveChangesAsync();

            //}


            //Start = DateTime.Now;
            //using (var transaction = _uw._Context.Database.BeginTransaction())
            //{
            //    await _uw._Context.BulkInsertAsync(sales);

            //    transaction.Commit();
            //}
            //TimeSpan = DateTime.Now - Start;

            //var sale = _uw._Context.Sales.AsNoTracking();
            //var settlementDetails = _uw._Context.SettlementDetails;

            //var sumDrivervalue = await _uw._Context.Sales.AsNoTracking()
            //    .Include(c => c.settlementDetails)
            //    .Select(c => new
            //    {
            //        //value = c.settlementDetails.Sum(c=> c.Value)

            //        //value = c.settlementDetails.Where(x => x.BenefisheryType == BenefisheryType.Driver).Sum(c=> c.Value)

            //        value = c.settlementDetails.Where(x => x.BenefisheryType == BenefisheryType.Driver)
            //        .FirstOrDefault() == null ? 0 : c.settlementDetails.Where(x => x.BenefisheryType == BenefisheryType.Driver)
            //        .FirstOrDefault().Value

            //    }).ToListAsync();

            //var gfg = sumDrivervalue.Sum(c => c.value);

            //var ts = await (from saleDb in sale

            //                join Setl in settlementDetails
            //                on saleDb.Id equals Setl.SaleId into bc
            //                from bct in bc.DefaultIfEmpty()

            //                select new
            //                {

            //                    DriverValue = (bct.BenefisheryType == BenefisheryType.Driver ? bct.Value : 0),

            //                    CompanyValue = (bct.BenefisheryType == BenefisheryType.Company ? bct.Value : 0),
            //                    UnitValue = (bct.BenefisheryType == BenefisheryType.Unit ? bct.Value : 0),
            //                    TripType = saleDb.TripType,
            //                    TerminalId = saleDb.TerminalId,
            //                }


            //                into CallData

            //                group CallData by new { CallData.TripType, CallData.TerminalId }

            //               into queryGroup
            //                select new SaleViewModel
            //                {

            //                    SaleCount = queryGroup.Count(),
            //                    TripType = queryGroup.Key.TripType,
            //                    TerminalId = queryGroup.Key.TerminalId,
            //                    DriverValue = queryGroup.Sum(c => c.DriverValue),
            //                    TransportationUnitValue = queryGroup.Sum(c => c.UnitValue),
            //                    TransportationCompanyValue = queryGroup.Sum(c => c.CompanyValue),

            //                }
            //        ).ToListAsync();




            //var model = query.Select(c => new SaleViewModel
            //{
            //    TerminalCaption = "Terminal" + c.TerminalId,
            //    DriverValue = c.DriverValue,
            //    SaleCount = c.SaleCount,
            //    SumTotal = c.SumTotal,
            //    TerminalId = c.TerminalId,
            //    TotalFare = c.TotalFare,
            //    TransportationCompanyValue = c.TransportationCompanyValue,
            //    TransportationUnitValue = c.TransportationCompanyValue,
            //    TripType = c.TripType

            //}).ToList();





            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax && TypeOfNews == "MostViewedNews")
                return PartialView("_MostViewedNews", await _uw.NewsRepository.MostViewedNews(0, 3, duration));


            else if (isAjax && TypeOfNews == "MostTalkNews")
                return PartialView("_MostTalkNews", await _uw.NewsRepository.MostTalkNews(0, 5, duration));

            else
            {
                int countNewsPublished = _uw.NewsRepository.CountNewsPublished();
                var news = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, null);
                var mostViewedNews = await _uw.NewsRepository.MostViewedNews(0, 3, "day");
                var mostTalkNews = await _uw.NewsRepository.MostTalkNews(0, 5, "day");
                var mostPopulerNews = await _uw.NewsRepository.MostPopularNews(0, 5);
                var internalNews = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, true);
                var foreignNews = await _uw.NewsRepository.GetPaginateNews(0, 10, item => "", item => item.First().PersianPublishDate, "", true, false);
                var videos = _uw.VideoRepository.GetPaginateVideos(0, 10, item => "", item => item.PersianPublishDateTime, "");
                var homePageViewModel = new HomePageViewModel(news, mostViewedNews, mostTalkNews, mostPopulerNews, internalNews, foreignNews, videos, countNewsPublished);
                return View(homePageViewModel);
            }


        }

        [Route("News/{newsId}/{url}")]
        public async Task<IActionResult> NewsDetails(string newsId, string url)
        {
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
                if (video == null)
                    return NotFound();
                else
                    return View(video);
            }
        }

    }
}