using System.Collections.Generic;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public class ParticipantService : ServiceBase<ParticipantDTO>
    {
        private readonly IRepository _repository;
        public ParticipantService()
        {
            _repository = new ParticipantRepository();
        }

        public ParticipantService(IRepository repository)
        {
            _repository = repository;
        }
        public override IEnumerable<ParticipantDTO> GetAll()
        {
            return _repository.GetAll();
        }
        public override async Task<IEnumerable<ParticipantDTO>> GetAsync(int skip = 0, int take = -1)
        {
            return await _repository.GetAsync(skip, take);
        }
        public override IEnumerable<ParticipantDTO> Get(int skip = 0, int take = -1)
        {
            return _repository.Get(skip, take);
        }

        public ParticipantDTO GetById(int itemId)
        {
            return _repository.GetById(itemId);
        }

        public async Task<double> GetCountAsync()
        {
            return await _repository.GetCountAsync();
        }
    }
}