using System;
using System.CodeDom;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CsvReader.Model;
using EntityFramework.Extensions;

namespace CsvReader.Service
{
    public class DeleteParticipantsCommand : ICommand
    {
        private Expression<Func<Participant, bool>> _predicate;
        public DeleteParticipantsCommand(Expression<Func<Participant, bool>> predicate)
        {
            _predicate = predicate;

        }

        public int Execute()
        {
            var task = ExecuteAsync();
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<int> ExecuteAsync()
        {
            using (var dbContext = new DataModel())
            {
                return await dbContext.Participants.Where(_predicate).DeleteAsync();
            }
        }
    }
}


