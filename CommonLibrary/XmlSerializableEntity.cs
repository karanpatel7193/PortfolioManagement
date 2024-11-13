using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CommonLibrary
{
    public class XmlSerializableEntity : IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return new XmlSchema();
            // Implementation omitted for clarity
        }

        public void ReadXml(XmlReader xmlReader)
        {
            // Implementation omitted for clarity
        }

        public void WriteXml(XmlWriter xmlWriter)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(this, null);
                if (value != null && MyConvert.ToString(value) != "1/1/0001 12:00:00 AM")
                    xmlWriter.WriteAttributeString(name, MyConvert.ToString(value).Trim());
            }
        }
    }
}
