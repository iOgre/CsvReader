using System;

namespace CsvReader.Service
{
    public class ParticipantDTO
    {
      
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}