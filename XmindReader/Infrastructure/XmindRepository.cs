using System;
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
        private static readonly Regex xmapContentRegex = 
            new Regex("<xmap-content.*>.*</xmap-content>");

        private static readonly string defaultNameSpace = 
            "urn:xmind:xmap:xmlns:content:2.0";

        private static readonly XName sheetTagName =
            XName.Get("sheet", defaultNameSpace);

        private static readonly XName topicTagName =
            XName.Get("topic", defaultNameSpace);

        private static readonly XName topicsTagName =
            XName.Get("topics", defaultNameSpace);

        private static readonly XName titleTagName =
            XName.Get("title", defaultNameSpace);

        private static readonly XName childrenTagName =
            XName.Get("children", defaultNameSpace);

        private readonly string filepath;

        public XmindRepository(string filepath)
        {
            this.filepath = filepath;
        }

        public TreeElement Load()
        {
            var content = File.ReadAllText(filepath);
            var xmapContent = this.GetXmapContent(content);
            var xml = XDocument.Parse(xmapContent);
            var sheet = xml.Descendants(sheetTagName).First();
            var topTopic = sheet.Element(topicTagName);
            return this.GetTopicElement(topTopic);
        }

        private TreeElement GetTopicElement(XElement topic)
        {
            var topicTitle = topic.Element(titleTagName);
            var topicElement = new TreeElement(topicTitle.Value);

            var children = topic.Element(childrenTagName);
            if (children is null) return topicElement;

            var childTopics = children
                .Element(topicsTagName)
                .Elements(topicTagName);

            foreach (var childTopic in childTopics)
            {
                topicElement.Append(this.GetTopicElement(childTopic));
            }

            return topicElement;
        }

        private string GetXmapContent(string xmindContetns)
        {
            var match = xmapContentRegex.Match(xmindContetns);

            if (match.Success) return match.Value;

            throw new ArgumentException("形式が異なるXmindファイルが指定されています。");
        }
    }
}
