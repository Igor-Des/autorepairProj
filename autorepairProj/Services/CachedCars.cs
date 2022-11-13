using autorepairProj.Data;
using autorepairProj.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace autorepairProj.Services
{
    public class CachedCars : ICached<Car>
    {
        private readonly AutorepairContext _context;
        private readonly IMemoryCache _memoryCache;
        public CachedCars(AutorepairContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public void AddList(string key)
        {
            IEnumerable<Car> cars = _context.Cars.ToList();
            if (cars != null)
            {
                _memoryCache.Set(key, cars, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(258)
                });
            }
        }

        public IEnumerable<Car> GetList(string key)
        {
            IEnumerable<Car> cars;
            if (!_memoryCache.TryGetValue(key, out cars))
            {
                cars = _context.Cars.ToList();
                if (cars != null)
                {
                    _memoryCache.Set(key, cars, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(258)));
                }
            }
            return cars;
        }

        public IEnumerable<Car> GetList()
        {
            {
                return _context.Cars.ToList();
            }
        }
    }
}
