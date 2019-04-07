using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebBackCurrencyConverter.API.Entities
{
    [XmlRoot(ElementName = "currency")]
    public class Currency
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "rate")]
        public string Rate { get; set; }
    }

    [XmlRoot(ElementName = "dailyrates")]
    public class Dailyrates
    {
        [XmlElement(ElementName = "currency")]
        public List<Currency> Currency { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "exchangerates")]
    public class Exchangerates
    {
        [XmlElement(ElementName = "dailyrates")]
        public Dailyrates Dailyrates { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "author")]
        public string Author { get; set; }
        [XmlAttribute(AttributeName = "refcur")]
        public string Refcur { get; set; }
        [XmlAttribute(AttributeName = "refamt")]
        public string Refamt { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}