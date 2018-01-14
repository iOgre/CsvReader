using System.Threading.Tasks;

namespace CsvReader.Service
{
    public interface ICommand
    {
        Task<int> ExecuteAsync();
        int  Execute();
    }
}