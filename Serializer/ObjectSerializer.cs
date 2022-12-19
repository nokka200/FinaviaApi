using System.Xml.Serialization;

namespace Finaviaapi.Serializer
{
    static public class MyObjectSerializer
    {
        static public void SaveObject(string path, object value)
        {
            XmlSerializer xmlWriter = new(value.GetType());

            using FileStream file = File.Create(path);
            xmlWriter.Serialize(file, value);

        }

        static public object ReadObject(string path, Type typeOf)
        {
            XmlSerializer xmlReader = new(typeOf);

            using FileStream file = File.OpenRead(path);
            Object tempObj = xmlReader.Deserialize(file)!;

            return tempObj;
        }
    }
}

