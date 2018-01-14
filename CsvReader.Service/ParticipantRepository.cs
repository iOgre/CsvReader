using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CsvReader.Model;

namespace CsvReader.Service
{
    public class ParticipantRepository : IRepository
    {
      
        public IEnumerable<ParticipantDTO> GetAll()
        {
            using (var context = new DataModel())
            {
                var items = context.Participants.Select(
                    p => new ParticipantDTO
                    {
                        Birthdate = p.Birthdate,
                        Email = p.Email,
                        FIO = p.FIO,
                        Phone = p.Phone,
                        Id = p.Id
                    }
                ).ToList();
                return items;
            }
        }

        public async Task<IEnumerable<ParticipantDTO>> GetAsync(int skip = 0, int take = -1)
        {
            IEnumerable<ParticipantDTO> result = new List<ParticipantDTO>();
            using (var context = new DataModel())
            {
                var items = context.Participants.AsQueryable();
                if (skip != 0)
                {
                    items = items.Skip(skip);
                }

                if (take != -1)
                {
                    items = items.Take(take);
                }

                result = await items
                    .Select(
                        p => new ParticipantDTO
                        {
                            Birthdate = p.Birthdate,
                            Email = p.Email,
                            FIO = p.FIO,
                            Phone = p.Phone,
                            Id = p.Id
                        })
                    .ToListAsync();
            }

            return result;
        }

        public IEnumerable<ParticipantDTO> Get(int skip = 0, int take = -1)
        {
            IEnumerable<ParticipantDTO> result = new List<ParticipantDTO>();
            using (var context = new DataModel())
            {
                var items = context.Participants.AsQueryable();
                if (skip != 0)
                {
                    items = items.Skip(skip);
                }

                if (take != -1)
                {
                    items = items.Take(take);
                }

                result =  items
                    .Select(
                        p => new ParticipantDTO
                        {
                            Birthdate = p.Birthdate,
                            Email = p.Email,
                            FIO = p.FIO,
                            Phone = p.Phone,
                            Id = p.Id
                        })
                    .ToList();
            }

            return result;
        }

        public Participant GetEntity(int id)
        {
            using (var context = new DataModel())
            {
                return context.Participants.Find(id);
            }
        }

        public async Task<double> GetCountAsync()
        {
            using (var context = new DataModel())
            {
                return await context.Participants.CountAsync();
            }
        }

        public ParticipantDTO GetById(int participantId)
        {
            using (var context = new DataModel())
            {
                return context.Participants.Where(t => t.Id == participantId)
                    .Select(p => new ParticipantDTO
                    {
                        Birthdate = p.Birthdate,
                        Email = p.Email,
                        FIO = p.FIO,
                        Phone = p.Phone,
                        Id = p.Id
                    }).FirstOrDefault();
            }
        }
    }
}