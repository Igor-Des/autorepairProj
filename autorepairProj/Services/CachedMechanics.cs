using autorepairProj.Data;
using autorepairProj.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace autorepairProj.Services
{
    public class CachedMechanics : ICached<Mechanic>
    {
        private readonly AutorepairContext _context;
        private readonly IMemoryCache _memoryCache;
        public CachedMechanics(AutorepairContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public void AddList(string key)
        {
            IEnumerable<Mechanic> mechanics = _context.Mechanics.ToList();
            if (mechanics != null)
            {
                _memoryCache.Set(key, mechanics, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(258)
                });
            }
        }

        public IEnumerable<Mechanic> GetList(string key)
        {
            IEnumerable<Mechanic> mechanics;
            if (!_memoryCache.TryGetValue(key, out mechanics))
            {
                mechanics = _context.Mechanics.ToList();
                if (mechanics != null)
                {
                    _memoryCache.Set(key, mechanics, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(264)));
                }
            }
            return mechanics;
        }

        public IEnumerable<Mechanic> GetList()
        {
            {
                return _context.Mechanics.ToList();
            }
        }
    }
}
