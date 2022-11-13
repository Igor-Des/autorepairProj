using autorepairProj.Data;
using autorepairProj.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace autorepairProj.Services
{
    public class CachedPayments : ICached<Payment>
    {
        private readonly AutorepairContext _context;
        private readonly IMemoryCache _memoryCache;
        public CachedPayments(AutorepairContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public void AddList(string key)
        {
            IEnumerable<Payment> payments = _context.Payments.ToList();
            if (payments != null)
            {
                _memoryCache.Set(key, payments, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(258)
                });
            }
        }

        public IEnumerable<Payment> GetList(string key)
        {
            IEnumerable<Payment> payments;
            if (!_memoryCache.TryGetValue(key, out payments))
            {
                payments = _context.Payments.ToList();
                if (payments != null)
                {
                    _memoryCache.Set(key, payments, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(258)));
                }
            }
            return payments;
        }

        public IEnumerable<Payment> GetList()
        {
            {
                return _context.Payments.ToList();
            }
        }
    }
}
