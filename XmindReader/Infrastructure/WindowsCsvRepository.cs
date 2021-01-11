using System.Collections.Generic;

namespace XmindReader.Infrastructure
{
    class WindowsCsvRepository : IArraysRepository
    {
        private readonly string filepath;

        public WindowsCsvRepository(string filepath)
        {
            this.filepath = filepath;
        }

        public void Save(IEnumerable<string[]> stringArrays)
        {
            WindowsCsv.Write(stringArrays, this.filepath);
        }
    }
}
