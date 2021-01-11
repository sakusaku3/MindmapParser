using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using XmindReader.Tree;

namespace XmindReader.Infrastructure
{
    class XmindRepository : ITreeRepository
    {
        private readonly string filepath;

        public XmindRepository(string filepath)
        {
            this.filepath = filepath;
        }

        public TreeElement Load()
        {
            var content = File.ReadAllText(filepath);
            var topSheet = this.GetSheets(content)[0];
            var xml = XDocument.Parse(topSheet);
            var sheet = xml.Element("sheet");
            var topTopic = sheet.Element("topic");
            return this.GetTopicElement(topTopic);
        }

        private TreeElement GetTopicElement(XElement topic)
        {
            var topicTitle = topic.Element("title");
            var topicElement = new TreeElement(topicTitle.Value);

            var children = topic.Element("children");
            if (children is null) return topicElement;

            var childTopics = children
                .Element("topics")
                .Elements("topic");

            foreach (var childTopic in childTopics)
            {
                topicElement.Append(this.GetTopicElement(childTopic));
            }

            return topicElement;
        }

        private IReadOnlyList<string> GetSheets(string xmindContetns)
        {
            var expression = "<sheet.*>(?<content>.*)</sheet>";
            var regex = new Regex(expression);

            var matchs = regex.Matches(xmindContetns);

            return matchs.Select(x => x.Value).ToArray();
        }
    }
}
