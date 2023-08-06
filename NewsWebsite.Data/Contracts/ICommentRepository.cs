using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Contracts
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);
        Task<List<CommentViewModel>> GetPaginateComments(int offset, int limit, Func<CommentViewModel, object> orderByAscFunc, Func<CommentViewModel, object> orderByDescFunc, string searchText);
    }
}
