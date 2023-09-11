using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using NewsWebsite.ViewModels.ContactUs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Contracts
{
    public interface IContactUsRepository
    {
        Task<List<MessageViewModel>> GetPaginateMessages(int offset, int limit, Func<MessageViewModel, object> orderByAscFunc, Func<MessageViewModel, object> orderByDescFunc, string searchText);
    }
}
