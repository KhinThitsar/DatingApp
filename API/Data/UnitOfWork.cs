using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context , IMapper mapper)
        {
            _mapper=mapper;
            _context=context;
        }

        public IUserRespository UserRespository => new UserRespository(_context,_mapper);

        public ILikesRespository likesRespository =>new LikesRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync()>0; 
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}