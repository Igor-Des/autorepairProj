using System.Collections.Generic;

namespace autorepairProj.Services
{
    public interface ICached<T>
    {
        public IEnumerable<T> GetList();
        public void AddList(string key);
        public IEnumerable<T> GetList(string key);
    }
}
