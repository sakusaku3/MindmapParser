using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmindReader.Tree
{
    class Hierarchy
    {
        public IReadOnlyList<TreeElement> Children => this._children;
        private readonly List<TreeElement> _children = new List<TreeElement>();

        public Hierarchy() { }

        public Hierarchy(TreeElement child) 
        {
            this._children.Add(child);
        }

        public void InsertHead(TreeElement child)
        {
            this._children.Insert(0, child);
        }
    }
}
