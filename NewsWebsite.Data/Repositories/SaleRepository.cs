using AutoMapper;
using NewsWebsite.Common;
using NewsWebsite.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Data.Repositories
{
    public class SaleRepository: ISaleRepository
    {
        private readonly NewsDBContext _context;
        private readonly IMapper _mapper;
        public SaleRepository(NewsDBContext context, IMapper mapper)
        {
            _context = context;
            _context.CheckArgumentIsNull(nameof(_context));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }

    }
}
