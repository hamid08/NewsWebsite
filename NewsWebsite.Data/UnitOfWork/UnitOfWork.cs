﻿using AutoMapper;
using NewsWebsite.Data.Contracts;
using NewsWebsite.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebsite.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public NewsDBContext _Context { get; }
        private IMapper _mapper;
        private ICategoryRepository _categoryRepository;
        private ISaleRepository _saleRepository;
        private ITagRepository _tagRepository;
        private IVideoRepository _videoRepository;
        private INewsRepository _newsRepository;
        private INewsletterRepository _newsletterRepository;
        private ICommentRepository _commentRepository;
        private IContactUsRepository _contactUsRepository;

        public UnitOfWork(NewsDBContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            IBaseRepository<TEntity> repository = new BaseRepository<TEntity,NewsDBContext>(_Context);
            return repository;
        }


        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_Context,_mapper);

                return _categoryRepository;
            }
        }
          public ISaleRepository SaleRepository
        {
            get
            {
                if (_saleRepository == null)
                    _saleRepository = new SaleRepository(_Context,_mapper);

                return _saleRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new TagRepository(_Context);

                return _tagRepository;
            }
        }


        public IVideoRepository VideoRepository
        {
            get
            {
                if (_videoRepository == null)
                    _videoRepository = new VideoRepository(_Context);

                return _videoRepository;
            }
        }

        public INewsRepository NewsRepository
        {
            get
            {
                if (_newsRepository == null)
                    _newsRepository = new NewsRepository(_Context, _mapper);

                return _newsRepository;
            }
        }

        public INewsletterRepository NewsletterRepository
        {
            get
            {
                if (_newsletterRepository == null)
                    _newsletterRepository = new NewsletterRepository(_Context);

                return _newsletterRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_Context);

                return _commentRepository;
            }
        }
        
        public IContactUsRepository ContactUsRepository
        {
            get
            {
                if (_contactUsRepository == null)
                    _contactUsRepository = new ContactUsRepository(_Context);

                return _contactUsRepository;
            }
        }

        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
