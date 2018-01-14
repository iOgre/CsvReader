using System.Collections.Generic;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public interface IRepository
    {
        IEnumerable<ParticipantDTO> GetAll();
        Task<IEnumerable<ParticipantDTO>> GetAsync(int skip = 0, int take = -1);
        IEnumerable<ParticipantDTO> Get(int skip = 0, int take = -1);
        ParticipantDTO GetById(int participantId);
        Participant GetEntity(int id);
        Task<double> GetCountAsync();
    }
}