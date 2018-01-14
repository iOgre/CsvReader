using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsvReader.Service
{
    public static class CsvService
    {
        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        private static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }
        public static async Task<long> GetLinesAsync(string path)
        {
            var lengthInBytes = 0;
            var totalFileSize = new FileInfo(path).Length;
            var encoding = GetEncoding(path);
            var limit = 0;
            var currentString = "";
            using (var stream = new StreamReader(path))
            {
                
                while (limit < 10 && (currentString = stream.ReadLine()) != null)
                {
                    lengthInBytes += encoding.GetByteCount(currentString);
                    limit++;
                }
            }

            var averageLineInBytes = (lengthInBytes / limit);
            var approxLinesInFile = totalFileSize / averageLineInBytes;
            return approxLinesInFile;
        }

        public static async Task<int> StoreParticipantsAsync(string path, IProgress<int> progress, CancellationToken token, int blockSize = 200, int limit = 0)
        {
            var itemsToAdd = new List<ParticipantDTO>();
            using (var stream = new StreamReader(path))
            {
                string currentString;
                int totalItems = 0;
                while ((limit == 0 || totalItems < limit) && (currentString = await stream.ReadLineAsync()) != null)
                {
                   
                    token.ThrowIfCancellationRequested();
                    var participantFields = currentString.Split(';');
                    itemsToAdd.Add(new ParticipantDTO()
                    {
                        FIO = participantFields[0],
                        Birthdate = DateTime.Parse(participantFields[1]),
                        Email = participantFields[2],
                        Phone = participantFields[3]
                    });
                    if (itemsToAdd.Count >= blockSize)
                    {
                        var command = new CreateParticipantsCommand(itemsToAdd);
                        await command.ExecuteAsync();
                        progress?.Report(itemsToAdd.Count);
                        itemsToAdd.Clear();
                    }
                    totalItems++;
                }

            } 
            return await new CreateParticipantsCommand(itemsToAdd).ExecuteAsync();
        }

       
        public static async Task<int> DeleteParticipantsAsync()
        {
            var deleteCommand = new DeleteParticipantsCommand(p => true);
            return await deleteCommand.ExecuteAsync();
        }
    }
}