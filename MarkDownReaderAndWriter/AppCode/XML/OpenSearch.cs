
using System;
using System.Xml.Serialization;
using System.Collections.Generic;


// <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="Search jstree API">
namespace MarkDownReaderAndWriter.XML
{


    [XmlRoot(ElementName = "Url", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
    public class Url
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "method")]
        public string Method { get; set; }
        [XmlAttribute(AttributeName = "template")]
        public string Template { get; set; }
        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }
    }


    [XmlRoot(ElementName = "Image", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
    public class Image
    {
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "OpenSearchDescription", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
    public class OpenSearchDescription
    {
        [XmlElement(ElementName = "ShortName", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public string ShortName { get; set; }
        [XmlElement(ElementName = "Description", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Tags", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public string Tags { get; set; }
        [XmlElement(ElementName = "Url", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public List<Url> Url { get; set; }
        [XmlElement(ElementName = "Image", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public List<Image> Image { get; set; }
        [XmlElement(ElementName = "InputEncoding", Namespace = "http://a9.com/-/spec/opensearch/1.1/")]
        public string InputEncoding { get; set; }
        [XmlElement(ElementName = "SearchForm", Namespace = "http://www.mozilla.org/2006/browser/search/")]
        public string SearchForm { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "moz", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Moz { get; set; }
    }


}
