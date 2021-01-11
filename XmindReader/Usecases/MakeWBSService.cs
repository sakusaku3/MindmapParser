using System.Collections.Generic;
using System.Linq;
using XmindReader.Tree;

namespace XmindReader.Usecases
{
    class MakeWBSService
    {
        public void Handle(
            ITreeRepository treeRepository,
            IArraysRepository arraysRepository)
        {
            var top = treeRepository.Load();
            var stringArrays = this.GetHierarchies(top)
                .Select(x => x.ToArray());
            arraysRepository.Save(stringArrays);
        }

        private IEnumerable<List<string>> GetHierarchies(
            TreeElement current,
            int indent = 0)
        {
            var baseList = this.GetBaseList(indent);
            baseList.Add(current.Value);
            yield return baseList;

            foreach (var child in current.Children)
                foreach (var item in this.GetHierarchies(child, indent + 1))
                    yield return item;
        }

        private List<string> GetBaseList(int indent)
        {
            return Enumerable.Range(0, indent)
                .Select(x => string.Empty)
                .ToList();
        }
    }
}
