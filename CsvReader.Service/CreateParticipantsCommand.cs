using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public class CreateParticipantsCommand : ICommand
    {
        private readonly IEnumerable<Participant> _participants;

        public CreateParticipantsCommand(IEnumerable<ParticipantDTO> participants)
        {
            _participants = participants
                .Select(
                    p => new Participant
                    {
                        Birthdate = p.Birthdate,
                        Email = p.Email,
                        FIO = p.FIO,
                        Phone = p.Phone,
                        Id = p.Id
                    });
        }
        public int Execute()
        {
            using (var dbContext = new DataModel())
            {
                dbContext.Participants.AddRange(_participants);
                return dbContext.SaveChanges();
            }
        }

        public async Task<int> ExecuteAsync()
        {
            using (var dbContext = new DataModel())
            {
                dbContext.Participants.AddRange(_participants);
                return await  dbContext.SaveChangesAsync();
            }
        }
    }
}