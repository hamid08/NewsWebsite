﻿using Microsoft.EntityFrameworkCore;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly NewsDBContext _context;
        public CommentRepository(NewsDBContext context)
        {
            _context = context;
        }


        public async Task<List<CommentViewModel>> GetPaginateComments(int offset, int limit, Func<CommentViewModel, Object> orderByAscFunc, Func<CommentViewModel, Object> orderByDescFunc, string searchText)
        {
            var comments = await _context.Comments.Where(c => c.Name.Contains(searchText)
            || c.Email.Contains(searchText))
                .Select(l => new CommentViewModel
                {
                    CommentId = l.CommentId,
                    Name = l.Name,
                    Email = l.Email,
                    IsConfirm = l.IsConfirm,
                    PersianPostageDateTime = DateTimeExtensions.ConvertMiladiToShamsi(l.PostageDateTime,"yyyy/MM/dd ساعت hh:mm:ss"),
                    Desription = l.Desription
                })
                .Skip(offset).Take(limit)
                .ToListAsync();


            //List<CommentViewModel> comments = _context.Comments.Where(c => c.Name.Contains(searchText) || c.Email.Contains(searchText) || c.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss").Contains(searchText))
            //                       .Select(l => new CommentViewModel {CommentId=l.CommentId,Name=l.Name , Email = l.Email, IsConfirm = l.IsConfirm, PersianPostageDateTime = l.PostageDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت hh:mm:ss") , Desription=l.Desription })
            //                       .OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc)
            //                       .Skip(offset).Take(limit).ToList();

            foreach (var item in comments)
                item.Row = ++offset;

            return comments;
        }

        public async Task AddComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            
        }
    }
}
