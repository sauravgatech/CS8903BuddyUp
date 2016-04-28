using System;
using System.Data.Entity;

namespace GT.BuddyUp.Platform.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        DbContext Context { get; }
    }
}
