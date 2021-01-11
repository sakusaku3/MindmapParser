using System;
using System.Collections.Generic;
using System.Text;

namespace XmindReader
{
    interface ITreeRepository
    {
        Tree.TreeElement Load();
    }
}
