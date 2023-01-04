using System.Xml.Serialization;

namespace Finaviaapi.Serializer
{
    static public class MyObjectSerializer
    {
        /// <summary>
        /// Seralizes a class to a .xml file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        static public void SaveObject(string path, object value)
        {
            XmlSerializer xmlWriter = new(value.GetType());

            using FileStream file = File.Create(path);
            xmlWriter.Serialize(file, value);

        }

        /// <summary>
        /// Deserializes a .xml file to a class
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeOf"></param>
        /// <returns></returns>
        static public object ReadObject(string path, Type typeOf)
        {
            XmlSerializer xmlReader = new(typeOf);

            using FileStream file = File.OpenRead(path);
            Object tempObj = xmlReader.Deserialize(file)!;

            return tempObj;
        }
    }
}

