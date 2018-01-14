using System.Data.Entity;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public class CreateOrUpdateParticipantCommand : ICommand
    {
        private readonly Participant _participant;
        public CreateOrUpdateParticipantCommand(ParticipantDTO participant)
        {
            IRepository repository = new ParticipantRepository();
            _participant =
                repository.GetEntity(participant.Id)
                ??
                new Participant();
            _participant.Email = participant.Email;
            _participant.FIO = participant.FIO;
            _participant.Birthdate = participant.Birthdate;
            _participant.Phone = participant.Phone;
           
        }
        public async Task<int> ExecuteAsync()
        {
            using (var dbContext = new DataModel())
            {
                
                var item = await dbContext.Participants.FirstOrDefaultAsync(t => t.Id ==_participant.Id);
                if (item != null)
                {
                    item.Email = _participant.Email;
                    item.FIO = _participant.FIO;
                    item.Birthdate = _participant.Birthdate;
                    item.Phone = _participant.Phone;
                }
                else
                {
                    dbContext.Participants.Add(_participant);
                }
                return await dbContext.SaveChangesAsync();
                
            }
        }

        public int  Execute()
        {
            using (var dbContext = new DataModel())
            {
                var item =  dbContext.Participants.Find(_participant.Id);
                if (item != null)
                {
                    item.Email = _participant.Email;
                    item.FIO = _participant.FIO;
                    item.Birthdate = _participant.Birthdate;
                    item.Phone = _participant.Phone;
                }
                else
                {
                    dbContext.Participants.Add(_participant);
                }
                return dbContext.SaveChanges();
            }
        }
    }
}


