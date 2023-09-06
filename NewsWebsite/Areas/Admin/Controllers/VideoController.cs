using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsWebsite.Common;
using NewsWebsite.Common.Attributes;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.ViewModels.DynamicAccess;
using NewsWebsite.ViewModels.Video;

namespace NewsWebsite.Areas.Admin.Controllers
{
    [DisplayName("مدیریت ویدیوها")]

    public class VideoController : BaseController
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;
        private const string VideoNotFound = "ویدیو درخواستی یافت نشد.";

        public VideoController(IUnitOfWork uw, IMapper mapper,IHostingEnvironment env)
        {
            _uw = uw;
            _uw.CheckArgumentIsNull(nameof(_uw));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));

            _env = env;
            _env.CheckArgumentIsNull(nameof(_env));
        }


        [HttpGet, DisplayName("مشاهده")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetVideos(string search, string order, int offset, int limit, string sort)
        {
            List<VideoViewModel> videos;
            int total = _uw.BaseRepository<Video>().CountEntities();
            if (!search.HasValue())
                search = "";

            if (limit == 0)
                limit = total;

            if (sort == "عنوان ویدیو")
            {
                if (order == "asc")
                    videos = _uw.VideoRepository.GetPaginateVideos(offset, limit,item=>item.Title,item=>"", search);
                else
                    videos = _uw.VideoRepository.GetPaginateVideos(offset, limit, item => "", item => item.Title, search);
            }

            else if (sort == "تاریخ انتشار")
            {
                if (order == "asc")
                    videos = _uw.VideoRepository.GetPaginateVideos(offset, limit,item=>item.PersianPublishDateTime, item => "", search);
                else
                    videos = _uw.VideoRepository.GetPaginateVideos(offset, limit, item => "", item => item.PersianPublishDateTime, search);
            }

            else
                videos = _uw.VideoRepository.GetPaginateVideos(offset, limit, item => "", item => item.PersianPublishDateTime, search);


            if (search != "")
                total = videos.Count();

            return Json(new { total = total, rows = videos });
        }


        [AjaxOnly()]
        [HttpGet, DisplayName("درج و ویرایش")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> RenderVideo(string videoId)
        {
            var videoViewModel = new VideoViewModel();
            if (videoId.HasValue())
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(videoId);
                if (video != null)
                {
                    videoViewModel = _mapper.Map<VideoViewModel>(video);
                
                    string path = $"{_env.WebRootPath}/videos/{video.Url}";
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        videoViewModel.VideoFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                      
                    }


                }
                else
                    ModelState.AddModelError(string.Empty,VideoNotFound);
            }
            return PartialView("_RenderVideo", videoViewModel);
        }

        [HttpPost, AjaxOnly()]
        public async Task<IActionResult> CreateOrUpdate(VideoViewModel viewModel)
        {
            if (viewModel.VideoId.HasValue())
                ModelState.Remove("PosterFile");

            if (ModelState.IsValid)
            {
                var videoFileName = Guid.NewGuid().ToString().Substring(0,8) + viewModel.VideoFile.FileName;
                await viewModel.VideoFile.UploadFileAsync($"{_env.WebRootPath}/videos/{videoFileName}");


                if (viewModel.PosterFile!=null)
                {
                    viewModel.Poster = _uw.VideoRepository.CheckVideoFileName(viewModel.PosterFile.FileName);
                    await viewModel.PosterFile.UploadFileAsync($"{_env.WebRootPath}/posters/{viewModel.Poster}");
                }
              
                if (viewModel.VideoId.HasValue())
                {
                    var video = await _uw.BaseRepository<Video>().FindByIdAsync(viewModel.VideoId);
                   
                    if (video != null)
                    {
                        if(viewModel.PosterFile != null)
                        {
                            FileExtensions.DeleteFile($"{_env.WebRootPath}/posters/{video.Poster}");

                        }
                        else
                            viewModel.Poster = video.Poster;

                        FileExtensions.DeleteFile($"{_env.WebRootPath}/videos/{video.Url}");



                        viewModel.PublishDateTime = video.PublishDateTime;
                        var newVideo = _mapper.Map(viewModel, video);
                        newVideo.Url = videoFileName;

                        _uw.BaseRepository<Video>().Update(newVideo);
                        await _uw.Commit();
                        TempData["notification"] = EditSuccess;
                    }
                    else
                        ModelState.AddModelError(string.Empty,VideoNotFound);
                }

                else
                {
                    viewModel.VideoId = StringExtensions.GenerateId(10);
                    var newVideo = _mapper.Map<Video>(viewModel);
                    newVideo.Url = videoFileName;

                    newVideo.IsConfirm = false;

                    await _uw.BaseRepository<Video>().CreateAsync(newVideo);
                    await _uw.Commit();
                    TempData["notification"] = InsertSuccess;
                }
            }

            return PartialView("_RenderVideo", viewModel);
        }


        [AjaxOnly()]
        [HttpGet, DisplayName("حذف")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Delete(string videoId)
        {
            if (!videoId.HasValue())
                ModelState.AddModelError(string.Empty, VideoNotFound);
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(videoId);
                if (video == null)
                    ModelState.AddModelError(string.Empty, VideoNotFound);
                else
                    return PartialView("_DeleteConfirmation", video);
            }
            return PartialView("_DeleteConfirmation");
        }


        [HttpPost, ActionName("Delete"), AjaxOnly()]
       
        public async Task<IActionResult> DeleteConfirmed(Video model)
        {
            if (model.VideoId == null)
                ModelState.AddModelError(string.Empty, VideoNotFound);
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(model.VideoId);
                if (video == null)
                    ModelState.AddModelError(string.Empty, VideoNotFound);
                else
                {
                    FileExtensions.DeleteFile($"{_env.WebRootPath}/posters/{video.Poster}");
                    FileExtensions.DeleteFile($"{_env.WebRootPath}/videos/{video.Url}");


                    _uw.BaseRepository<Video>().Delete(video);
                    await _uw.Commit();
                    TempData["notification"] = DeleteSuccess;
                    return PartialView("_DeleteConfirmation", video);
                }
            }
            return PartialView("_DeleteConfirmation");
        }


        [HttpPost, ActionName("DeleteGroup")]
        [DisplayName("حذف گروهی")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> DeleteGroupConfirmed(string[] btSelectItem)
        {
            if (btSelectItem.Count() == 0)
                ModelState.AddModelError(string.Empty, "هیچ ویدیویی برای حذف انتخاب نشده است.");
            else
            {
                foreach (var item in btSelectItem)
                {
                    var video = await _uw.BaseRepository<Video>().FindByIdAsync(item);
                    _uw.BaseRepository<Video>().Delete(video);
                    await _uw.Commit();
                    FileExtensions.DeleteFile($"{_env.WebRootPath}/posters/{video.Poster}");
                    FileExtensions.DeleteFile($"{_env.WebRootPath}/videos/{video.Url}");

                }
                TempData["notification"] = "حذف گروهی اطلاعات با موفقیت انجام شد.";
            }

            return PartialView("_DeleteGroup");
        }


        [HttpGet, DisplayName("تایید ویدیو")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> ConfirmOrInconfirm(string videoId)
        {
            if (!videoId.HasValue())
                ModelState.AddModelError(string.Empty, VideoNotFound);
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(videoId);
                if (video == null)
                    ModelState.AddModelError(string.Empty, VideoNotFound);
                else
                    return PartialView("_ConfirmOrInconfirm", video);
            }
            return PartialView("_ConfirmOrInconfirm");
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrInconfirm(Video model)
        {
            if (model.VideoId == null)
                ModelState.AddModelError(string.Empty, VideoNotFound);
            else
            {
                var video = await _uw.BaseRepository<Video>().FindByIdAsync(model.VideoId);
                if (video == null)
                    ModelState.AddModelError(string.Empty, VideoNotFound);
                else
                {
                    if (video.IsConfirm)
                        video.IsConfirm = false;
                    else
                        video.IsConfirm = true;

                    _uw.BaseRepository<Video>().Update(video);
                    await _uw.Commit();
                    TempData["notification"] = OperationSuccess;
                    return PartialView("_ConfirmOrInconfirm", video);
                }
            }
            return PartialView("_ConfirmOrInconfirm");
        }


    }
}