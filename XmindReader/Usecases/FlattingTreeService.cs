using System.Collections.Generic;
using System.Linq;
using XmindReader.Tree;

namespace XmindReader.Usecases
{
    class FlattingTreeService
    {
        public void Handle(
            ITreeRepository treeRepository,
            IArraysRepository arraysRepository)
        {
            var top = treeRepository.Load();
            var stringArrays = this.GetHierarchies(top)
                .Select(x => x.Children.Select(x => x.Value).ToArray());
            arraysRepository.Save(stringArrays);
        }

        private IEnumerable<Hierarchy> GetHierarchies(TreeElement current)
        {
            if (current.Children.Any())
            {
                foreach (var hie in current.Children.SelectMany(x => this.GetHierarchies(x)))
                {
                    hie.InsertHead(current);
                    yield return hie;
                }
            }
            else
            {
                yield return new Hierarchy(current);
            }
        }
    }
}
