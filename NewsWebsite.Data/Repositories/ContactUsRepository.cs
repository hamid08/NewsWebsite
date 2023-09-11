using Microsoft.EntityFrameworkCore;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Entities;
using NewsWebsite.ViewModels.Comments;
using NewsWebsite.ViewModels.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly NewsDBContext _context;
        public ContactUsRepository(NewsDBContext context)
        {
            _context = context;
        }


        public async Task<List<MessageViewModel>> GetPaginateMessages(int offset, int limit, Func<MessageViewModel, Object> orderByAscFunc, Func<MessageViewModel, Object> orderByDescFunc, string searchText)
        {
            var comments = await _context.ContactUs.Where(c => c.Name.Contains(searchText)
            || c.Email.Contains(searchText))
                .Select(l => new MessageViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    Email = l.Email,
                    PersianPostageDateTime = DateTimeExtensions.ConvertMiladiToShamsi(l.PostageDateTime,"yyyy/MM/dd ساعت hh:mm:ss"),
                    Desription = l.Desription
                })
                .Skip(offset).Take(limit)
                .ToListAsync();

            foreach (var item in comments)
                item.Row = ++offset;

            return comments;
        }

    }
}
