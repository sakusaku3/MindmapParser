using System.Collections.Generic;

namespace XmindReader
{
    interface IArraysRepository
    {
        void Save(IEnumerable<string[]> stringArrays);
    }
}
