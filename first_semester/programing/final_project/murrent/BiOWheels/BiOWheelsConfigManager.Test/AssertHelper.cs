using System.IO;
using System.Xml.Serialization;

namespace BiOWheelsConfigManager.Test
{
    /// <summary>
    /// AssertHelper is used to compare two objects in depth
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Serialize an object to xml
        /// </summary>
        /// <param name="o">Object to serialize</param>
        /// <returns>The stream ob the object</returns>
        public static string SerialilzeToXML(this object o)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer =
                    new XmlSerializer(o.GetType());
                xmlSerializer.Serialize(memoryStream, o);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader streamReader =
                    new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
