using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using NewsWebsite.ViewModels.News;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsDBContext _context;
        private readonly IMapper _mapper;
        public NewsRepository(NewsDBContext context, IMapper mapper)
        {
            _context = context;
            _context.CheckArgumentIsNull(nameof(_context));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }

        public int CountNewsPublished() => _context.News.Where(n => n.IsConfirm && n.IsPublish == true && n.PublishDateTime <= DateTime.Now).Count();
        public async Task<List<NewsViewModel>> GetPaginateNews(int offset, int limit, Func<IGrouping<string, NewsViewModel>, Object> orderByAscFunc,
            Func<IGrouping<string, NewsViewModel>, Object> orderByDescFunc, string searchText,bool? isPublish,bool? isInternal)
        {
            string NameOfCategories = "";
            string NameOfTags = "";
            List<NewsViewModel> newsViewModel = new List<NewsViewModel>();

            newsViewModel = await _context.News
                .Include(c=> c.Visits)
                .Include(c=> c.Likes)
                .Include(c=> c.User)
                .Include(c=> c.Comments)
                .Include(c=> c.NewsCategories).ThenInclude(c=> c.Category)
                .Include(c=> c.NewsTags)
                .Where(c=> !string.IsNullOrWhiteSpace(searchText) ?
                
                c.Title.Contains(searchText) || c.Abstract.Contains(searchText)
                || c.Description.Contains(searchText)
                : true)

                .Where(c=> isInternal != null?
                (isInternal.Value? c.IsInternal : !c.IsInternal
                )
                : true)
                .Select(n=> new NewsViewModel
                {
                    NewsId = n.NewsId,
                    Title = n.Title,
                    Abstract = n.Abstract,
                    ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                    Url = n.Url,
                    ImageName = n.ImageName,
                    Description = n.Description,
                    NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                    NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                    NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                    NumberOfComments = n.Comments.Count(),
                    NameOfCategories = n.NewsCategories != null ? string.Join("-", n.NewsCategories.Select(v=> v.Category.CategoryName).ToList()):"",
                    NameOfTags = n.NewsTags != null ? string.Join("-",n.NewsTags.Select(v=> v.Tag.TagName).ToList()):"",
                    AuthorName = n.User.FirstName + " " + n.User.LastName,
                    IsPublish = n.IsPublish,
                    NewsType = n.IsInternal == true ? "داخلی" : "خارجی",
                    PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                    PersianPublishDate = n.PublishDateTime == null ? "-" : DateTimeExtensions.ConvertMiladiToShamsi(n.PublishDateTime, "yyyy/MM/dd ساعت HH:mm:ss"),
                    Status = n.IsPublish == false ? "پیش نویس" : n.PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده",
                    AccessCategoryIds = n.NewsCategories.Select(c=> c.CategoryId).ToList(),
                    IsConfirm = n.IsConfirm
                })
                .Skip(offset).Take(limit)
                .ToListAsync();



            foreach (var item in newsViewModel)
                item.Row = ++offset;

            return newsViewModel;

        }

        public string CheckNewsFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            int fileNameCount = _context.News.Where(f => f.ImageName == fileName).Count();
            int j = 1;
            while (fileNameCount != 0)
            {
                fileName = fileName.Replace(fileExtension, "") + j + fileExtension;
                fileNameCount = _context.Videos.Where(f => f.Poster == fileName).Count();
                j++;
            }

            return fileName;
        }


        public async Task<List<string>> GetChildernCategory(string categoryId)
        {
            var result = new List<string>();

           result = await _context.Categories.Where(c => c.CategoryId == categoryId || c.ParentCategoryId == categoryId)
                .Select(c => c.CategoryId).ToListAsync();


            return result;
        }

        public async Task<List<NewsViewModel>> MostViewedNews(int offset, int limit, string duration)
        {
            string NameOfCategories = "";
            List<NewsViewModel> newsViewModel = new List<NewsViewModel>();
            DateTime StartMiladiDate;
            DateTime EndMiladiDate = DateTime.Now;

            if (duration == "week")
            {
                int NumOfWeek = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dddd").GetNumOfWeek();
                StartMiladiDate = DateTime.Now.AddDays((-1) * NumOfWeek).Date + new TimeSpan(0, 0, 0);
            }

            else if (duration == "day")
                StartMiladiDate = DateTime.Now.Date + new TimeSpan(0, 0, 0);

            else
            {
                string DayOfMonth = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dd").Fa2En();
                StartMiladiDate = DateTime.Now.AddDays((-1) * (int.Parse(DayOfMonth) - 1)).Date + new TimeSpan(0, 0, 0);
            }

            var news = await _context.News
              .Include(c => c.Visits)
              .Include(c => c.Likes)
              .Include(c => c.User)
              .Include(c => c.Comments)
              .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
              .Include(c => c.NewsTags)
              //.Where(n => n.PublishDateTime <= EndMiladiDate && StartMiladiDate <= n.PublishDateTime)
           
              .Where(c=> c.IsConfirm)
              .Select(n => new NewsViewModel
              {
                  NewsId = n.NewsId,
                  ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                  Url = n.Url,
                  NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                  NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                  NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                  NumberOfComments = n.Comments.Count(),
                  ImageName = n.ImageName,
                  PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
              }).OrderByDescending(o => o.NumberOfVisit).Skip(offset).Take(limit).AsNoTracking().ToListAsync();


            return news;
        }


        public async Task<List<NewsViewModel>> MostTalkNews(int offset, int limit, string duration)
        {
            DateTime StartMiladiDate;
            DateTime EndMiladiDate = DateTime.Now;

            if (duration == "week")
            {
                int NumOfWeek = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dddd").GetNumOfWeek();
                StartMiladiDate = DateTime.Now.AddDays((-1) * NumOfWeek).Date + new TimeSpan(0, 0, 0);
            }

            else if (duration == "day")
                StartMiladiDate = DateTime.Now.Date + new TimeSpan(0, 0, 0);

            else
            {
                string DayOfMonth = ConvertDateTime.ConvertMiladiToShamsi(DateTime.Now, "dd").Fa2En();
                StartMiladiDate = DateTime.Now.AddDays((-1) * (int.Parse(DayOfMonth) - 1)).Date + new TimeSpan(0, 0, 0);
            }

            var news = await _context.News
               .Include(c => c.Visits)
               .Include(c => c.Likes)
               .Include(c => c.User)
               .Include(c => c.Comments)
               .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
               .Include(c => c.NewsTags)
               //.Where(n => n.PublishDateTime <= EndMiladiDate && StartMiladiDate <= n.PublishDateTime)
            
               .Where(c=> c.IsConfirm)
               .Select(n => new NewsViewModel
               {
                   NewsId = n.NewsId,
                   ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                   Url = n.Url,
                   NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                   NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                   NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                   NumberOfComments = n.Comments.Count(),
                   ImageName = n.ImageName,
                   PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
               }).OrderByDescending(o => o.NumberOfComments).Skip(offset).Take(limit).AsNoTracking().ToListAsync();

            return news;
        }

        public async Task<List<NewsViewModel>> MostPopularNews(int offset, int limit)
        {
            return await _context.News
             .Include(c => c.Visits)
             .Include(c => c.Likes)
             .Include(c => c.User)
             .Include(c => c.Comments)
             .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
             .Include(c => c.NewsTags)
             .Where(c=> c.IsConfirm)

             //.Where(n => n.IsPublish == true && n.PublishDateTime <= DateTime.Now)
             .Select(n => new NewsViewModel
             {
                 NewsId = n.NewsId,
                 ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                 Url = n.Url,
                 NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                 NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                 NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                 NumberOfComments = n.Comments.Count(),
                 ImageName = n.ImageName,
                 PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
             }).OrderByDescending(o => o.NumberOfLike).Skip(offset).Take(limit).AsNoTracking().ToListAsync();


            
        }

        public async Task<NewsViewModel> GetNewsById(string newsId)
        {
            string NameOfCategories = "";

            var news = await _context.News
                .Include(c => c.Visits)
                .Include(c => c.Likes)
                .Include(c => c.User)
                .Include(c => c.Comments)
                .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
                .Include(c => c.NewsTags).ThenInclude(c=> c.Tag)
                .Where(c=> c.NewsId == newsId)
                .Where(c=> c.IsConfirm)

                .Select(n => new NewsViewModel
                {
                    NewsId = n.NewsId,
                    Title = n.Title,
                    Abstract = n.Abstract,
                    ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                    Url = n.Url,
                    ImageName = n.ImageName,
                    Description = n.Description,
                    NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                    NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                    NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                    NumberOfComments = n.Comments.Count(),
                    NameOfCategories = n.NewsCategories != null ? string.Join("-", n.NewsCategories.Select(v => v.Category.CategoryName).ToList()) : "",
                    NameOfTags = n.NewsTags != null ? string.Join("-", n.NewsTags.Select(v => v.Tag.TagName).ToList()) : "",
                    AuthorName = n.User.FirstName + " " + n.User.LastName,
                    IsPublish = n.IsPublish,
                    NewsType = n.IsInternal == true ? "داخلی" : "خارجی",
                    PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                    PersianPublishDate = n.PublishDateTime == null ? "-" : DateTimeExtensions.ConvertMiladiToShamsi(n.PublishDateTime,"yyyy/MM/dd ساعت HH:mm:ss"),
                    Status = n.IsPublish == false ? "پیش نویس" : n.PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده",
                    AuthorInfo = n.User,
                    TagIdsList = n.NewsTags.Select(c=> c.TagId).ToList(),
                    TagNamesList = n.NewsTags.Select(c=> c.Tag.TagName).ToList()
                    
                })
                .FirstOrDefaultAsync();


            return news;
        }

        public async Task<List<NewsViewModel>> GetNextAndPreviousNews(DateTime? PublishDateTime)
        {
            var newsList = new List<NewsViewModel>();
            newsList.Add( await (from n in _context.News.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                          where (n.IsPublish == true && n.PublishDateTime <= DateTime.Now && n.PublishDateTime< PublishDateTime)
                          select (new NewsViewModel{NewsId = n.NewsId,ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,Url = n.Url,Title = n.Title,NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),NumberOfComments = n.Comments.Count(),ImageName = n.ImageName,PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                          })).OrderByDescending(o => o.PublishDateTime).AsNoTracking().FirstOrDefaultAsync());

            newsList.Add(await (from n in _context.News.Include(v => v.Visits).Include(l => l.Likes).Include(c => c.Comments)
                                where (n.IsPublish == true && n.PublishDateTime <= DateTime.Now && n.PublishDateTime > PublishDateTime)
                                select (new NewsViewModel{NewsId = n.NewsId,ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,Url = n.Url,Title = n.Title,NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),NumberOfComments = n.Comments.Count(),ImageName = n.ImageName,PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                                })).OrderBy(o => o.PublishDateTime).AsNoTracking().FirstOrDefaultAsync());

            return newsList;
        }

        public async Task<List<Comment>> GetNewsCommentsAsync(string newsId)
        {
            var comments = await _context.Comments
                .Where(c => c.ParentCommentId == null && c.NewsId == newsId && c.IsConfirm == true)
                .Select(c => new Comment
                {
                    CommentId = c.CommentId,
                    Desription = c.Desription,
                    Email = c.Email,
                    PostageDateTime = c.PostageDateTime,
                    Name = c.Name,
                    NewsId = c.NewsId
                }).ToListAsync();

            foreach (var item in comments)
                await BindSubComments(item);

            return comments;
        }

        public async Task BindSubComments(Comment comment)
        {
            var subComments = await _context.Comments
            .Where(c => c.ParentCommentId == comment.CommentId && c.IsConfirm == true)
            .Select(c => new Comment
            {
                CommentId = c.CommentId,
                Desription = c.Desription,
                Email = c.Email,
                PostageDateTime = c.PostageDateTime,
                Name = c.Name,
                NewsId = c.NewsId
            }).ToListAsync();
          
            foreach (var item in subComments)
            {
                await BindSubComments(item);
                comment.comments.Add(item);
            }
        }
        public async Task<List<NewsViewModel>> GetRelatedNews(int number, List<string> tagIdList,string newsId)
        {
            var newsList = new List<NewsViewModel>();
            int randomRow;

            int newsCount = await _context.News
                .Include(t => t.NewsTags)
                .Where(n => n.IsPublish == true )
                .Where(n=> n.PublishDateTime <= DateTime.Now )
                //.Where(n=> tagIdList.Any(y => n.NewsTags.Select(x => x.TagId).Contains(y)) 
                .Where(n=> n.NewsId != newsId).CountAsync();

            for (int i = 0; i < number && i< newsCount; i++)
            {
                randomRow = CustomMethods.RandomNumber(1, newsCount + 1);
                var news = await _context.News
                    .Include(t => t.NewsTags)
                    .Include(c=>c.Comments)
                    .Include(l=>l.Likes)
                    .Include(l => l.Visits)
                    .Where(n => n.IsPublish == true 
                    && n.PublishDateTime <= DateTime.Now 
                    //&& tagIdList.Any(y => n.NewsTags.Select(x => x.TagId).Contains(y)) 
                    && n.NewsId != newsId)

                    .Select(n => new NewsViewModel { Title = n.Title, Url = n.Url, NewsId = n.NewsId, ImageName = n.ImageName, PublishDateTime=n.PublishDateTime,
                        NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                        NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                        NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                        NumberOfComments = n.Comments.Count(),
                    })
                    .Skip(randomRow - 1).Take(1).FirstOrDefaultAsync();

                newsList.Add(news);
            }

            return newsList;
        }


        public async Task<List<NewsViewModel>> GetNewsInCategoryAndTag(string categoryId,string TagId)
        {
            string NameOfCategories = "";
            List<NewsViewModel> newsViewModel = new List<NewsViewModel>();

            var news = await _context.News
               .Include(c => c.Visits)
               .Include(c => c.Likes)
               .Include(c => c.User)
               .Include(c => c.Comments)
               .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
               .Include(c => c.NewsTags)
               .Where(c => c.NewsCategories.Any(c=> c.CategoryId == categoryId))
               .Where(c=> c.IsConfirm)
               .Select(n => new NewsViewModel
               {
                   NewsId = n.NewsId,
                   Title = n.Title,
                   Abstract = n.Abstract,
                   ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                   Url = n.Url,
                   ImageName = n.ImageName,
                   Description = n.Description,
                   NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                   NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                   NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                   NumberOfComments = n.Comments.Count(),
                   NameOfCategories = n.NewsCategories != null ? string.Join("-", n.NewsCategories.Select(v => v.Category.CategoryName).ToList()) : "",
                   NameOfTags = n.NewsTags != null ? string.Join("-", n.NewsTags.Select(v => v.Tag.TagName).ToList()) : "",
                   AuthorName = n.User.FirstName + " " + n.User.LastName,
                   IsPublish = n.IsPublish,
                   NewsType = n.IsInternal == true ? "داخلی" : "خارجی",
                   PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                   PersianPublishDate = n.PublishDateTime == null ? "-" : DateTimeExtensions.ConvertMiladiToShamsi(n.PublishDateTime, "yyyy/MM/dd ساعت HH:mm:ss"),
                   Status = n.IsPublish == false ? "پیش نویس" : n.PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده",
                   AuthorInfo = n.User
               })
               .ToListAsync();

            return news;
        }

        public async Task<List<NewsViewModel>> GetNewsBySearch(string search)
        {
            List<NewsViewModel> newsViewModel = new List<NewsViewModel>();

            var news = await _context.News
               .Include(c => c.Visits)
               .Include(c => c.Likes)
               .Include(c => c.User)
               .Include(c => c.Comments)
               .Include(c => c.NewsCategories).ThenInclude(c => c.Category)
               .Include(c => c.NewsTags)

                .Where(c => !string.IsNullOrWhiteSpace(search) ?

                c.Title.Contains(search) || c.Abstract.Contains(search)
                || c.Description.Contains(search)
                : true)

                .Where(c=> c.IsConfirm)
               .Select(n => new NewsViewModel
               {
                   NewsId = n.NewsId,
                   Title = n.Title,
                   Abstract = n.Abstract,
                   ShortTitle = n.Title.Length > 50 ? n.Title.Substring(0, 50) + "..." : n.Title,
                   Url = n.Url,
                   ImageName = n.ImageName,
                   Description = n.Description,
                   NumberOfVisit = n.Visits.Select(v => v.NumberOfVisit).Sum(),
                   NumberOfLike = n.Likes.Where(l => l.IsLiked == true).Count(),
                   NumberOfDisLike = n.Likes.Where(l => l.IsLiked == false).Count(),
                   NumberOfComments = n.Comments.Count(),
                   NameOfCategories = n.NewsCategories != null ? string.Join("-", n.NewsCategories.Select(v => v.Category.CategoryName).ToList()) : "",
                   NameOfTags = n.NewsTags != null ? string.Join("-", n.NewsTags.Select(v => v.Tag.TagName).ToList()) : "",
                   AuthorName = n.User.FirstName + " " + n.User.LastName,
                   IsPublish = n.IsPublish,
                   NewsType = n.IsInternal == true ? "داخلی" : "خارجی",
                   PublishDateTime = n.PublishDateTime == null ? new DateTime(01, 01, 01) : n.PublishDateTime,
                   PersianPublishDate = n.PublishDateTime == null ? "-" : DateTimeExtensions.ConvertMiladiToShamsi(n.PublishDateTime, "yyyy/MM/dd ساعت HH:mm:ss"),
                   Status = n.IsPublish == false ? "پیش نویس" : n.PublishDateTime > DateTime.Now ? "انتشار در آینده" : "منتشر شده",
                   AuthorInfo = n.User
               })
               .ToListAsync();

            return news;
        }
    }
}
