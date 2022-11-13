using autorepairProj.Data;
using autorepairProj.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace autorepairProj.Services
{
    public class CachedOwners : ICached<Owner>
    {
        private readonly AutorepairContext _context;
        private readonly IMemoryCache _memoryCache;
        public CachedOwners(AutorepairContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public void AddList(string key)
        {
            IEnumerable<Owner> owners = _context.Owners.ToList();
            if (owners != null)
            {
                _memoryCache.Set(key, owners, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(258)
                });
            }
        }

        public IEnumerable<Owner> GetList(string key)
        {
            IEnumerable<Owner> owners;
            if (!_memoryCache.TryGetValue(key, out owners))
            {
                owners = _context.Owners.ToList();
                if (owners != null)
                {
                    _memoryCache.Set(key, owners, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(258)));
                }
            }
            return owners;
        }

        public IEnumerable<Owner> GetList()
        {
            {
                return _context.Owners.ToList();
            }
        }
    }
}
