using System.Collections.Generic;
using System.Linq;

namespace XmindReader.Tree
{
    class TreeElement
    {
        public string Value { get; }

        public TreeElement Parent { get; set; }

        public IReadOnlyList<TreeElement> Children => this._children;
        private readonly List<TreeElement> _children = new List<TreeElement>();

        public TreeElement(string value)
        {
            this.Value = value;
        }

        public void Append(TreeElement child)
        {
            this._children.Add(child);
            child.Parent = this;
        }

        public IEnumerable<Hierarchy> GetHierarchies()
        {
            if (this.Children.Any())
            {
                foreach (var hie in this.Children.SelectMany(x => x.GetHierarchies()))
                {
                    hie.InsertHead(this);
                    yield return hie;
                }
            }
            else
            {
                yield return new Hierarchy(this);
            }
        }
    }
}
