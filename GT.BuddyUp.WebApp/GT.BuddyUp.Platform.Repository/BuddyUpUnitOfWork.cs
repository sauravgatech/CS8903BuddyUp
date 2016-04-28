using System;
using System.Data.Entity;

namespace GT.BuddyUp.Platform.Repository
{
    public sealed class BuddyUpUnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        public BuddyUpUnitOfWork()
        {
            _context = new GT.BuddyUp.EntityModel.BuddyUpDb();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public DbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
