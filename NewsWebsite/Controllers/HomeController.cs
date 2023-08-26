using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(IUnitOfWork uw, IHttpContextAccessor accessor)
        {
            _uw = uw;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index(string duration, string TypeOfNews)
        {
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