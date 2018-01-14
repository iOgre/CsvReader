using System.Collections.Generic;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public abstract class ServiceBase<T>
    {
        public abstract IEnumerable<T> GetAll();
        public abstract Task<IEnumerable<T>> GetAsync(int skip = 0, int take = -1);
        public abstract IEnumerable<T> Get(int skip = 0, int take = -1);
    }
}