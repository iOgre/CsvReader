using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvReader.Service;

namespace CsvReader.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
          //  await StoreParticipantsAsync("input.csv");
            Console.WriteLine("before");
            await ShowParticipantsAsync().ContinueWith(async k =>
                 {
                     await UpdateParticipantNo3Async()
                         .ContinueWith(async res =>
                         {
                             Console.WriteLine("after");
                             await ShowParticipantsAsync();
                         });
                 }).ContinueWith(
                t =>
                {
                    Console.ReadLine();
                }
                );
           
            
           

        }

        static void MainSync(string[] args)
        {

          //  StoreParticipants("input.csv", 5);
            
            UpdateParticipantNo3();
            ShowParticipants();
            Console.ReadLine();
        }

        private static async Task UpdateParticipantNo3Async()
        {
            var participantNew = new ParticipantDTO
            {
                Id = 3,
                FIO = "Rabindranat Tagor",
                Birthdate = new DateTime(1890, 12, 11),
                Email = "qqeq",
                Phone = "1000 00 000"
            };
            var command = new CreateOrUpdateParticipantCommand(participantNew);
           await  command.ExecuteAsync();
        }

        private static void UpdateParticipantNo3()
        {
            var participantNew = new ParticipantDTO
            {
                Id = 3,
                FIO = Guid.NewGuid().ToString(),
                Birthdate = new DateTime(1991, 11, 11),
                Email = "xxxxxx",
                Phone = "1000 00 000"
            };
            var command = new CreateOrUpdateParticipantCommand(participantNew);
            command.Execute();
        }

        private static async Task ShowParticipantsAsync()
        {
           var service = new ParticipantService(new ParticipantRepository());
            foreach (ParticipantDTO item in await service.GetAsync(0, 5))
            {
                Console.WriteLine($"{item.Id};{item.FIO};{item.Birthdate.ToShortDateString()};{item.Email};{item.Phone}");
            }
        }

        private static void ShowParticipants()
        {
            var service = new ParticipantService(new ParticipantRepository());
            foreach (var item in service.Get(0, 5))
            {
                Console.WriteLine($"{item.Id};{item.FIO};{item.Birthdate.ToShortDateString()};{item.Email};{item.Phone}");
            }
        }

    }
}
