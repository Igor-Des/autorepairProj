using autorepairProj.Data;
using autorepairProj.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace autorepairProj.Services
{
    public class CachedQualification : ICached<Qualification>
    {
        private readonly AutorepairContext _context;
        private readonly IMemoryCache _memoryCache;
        public CachedQualification(AutorepairContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public void AddList(string key)
        {
            IEnumerable<Qualification> qualifications = _context.Qualifications.ToList();
            if (qualifications != null)
            {
                _memoryCache.Set(key, qualifications, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(258)
                });
            }
        }

        public IEnumerable<Qualification> GetList(string key)
        {
            IEnumerable<Qualification> qualifications;
            if (!_memoryCache.TryGetValue(key, out qualifications))
            {
                qualifications = _context.Qualifications.ToList();
                if (qualifications != null)
                {
                    _memoryCache.Set(key, qualifications, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(258)));
                }
            }
            return qualifications;
        }

        public IEnumerable<Qualification> GetList()
        {
            {
                return _context.Qualifications.ToList();
            }
        }
    }
}
