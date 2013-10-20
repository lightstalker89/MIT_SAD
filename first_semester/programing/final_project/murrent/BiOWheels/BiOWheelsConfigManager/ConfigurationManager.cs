using System.Xml.Serialization;

namespace BiOWheelsConfigManager
{
    using System;
    using System.IO;

    public class ConfigurationManager : IConfigurationManager
    {
        public ConfigurationManager() { }

        /// <summary>
        /// Loads the configuration
        /// </summary>
        public T Load<T>(string configFileName)
        {
            object deserializedObject = null;

            if (File.Exists(configFileName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (StreamReader str = new StreamReader(configFileName))
                    {
                        deserializedObject = serializer.Deserialize(str.BaseStream);
                    }
                }
                catch (InvalidOperationException)
                {
                    
                }
                catch(IOException)
                {
                    
                }
                catch (ArgumentNullException)
                {
                    
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (ArgumentException)
                {

                }
            }

            return (T)deserializedObject;
        }

        public WriterStatus Write<T>(string configFilename, T configurationObject)
        {
            if (typeof(T) == configurationObject.GetType())
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (StreamWriter writer = new StreamWriter(configFilename))
                    {
                        serializer.Serialize(writer, configurationObject);
                    }
                }
                catch (InvalidOperationException)
                {
                    return WriterStatus.FAILED;
                }
                catch(IOException)
                {
                    return WriterStatus.FAILED;
                }
                catch (ArgumentNullException)
                {
                    return WriterStatus.PATHISNULL;
                }
                catch(UnauthorizedAccessException)
                {
                    return WriterStatus.ACCESSERROR;
                }
                catch (ArgumentException)
                {
                    return WriterStatus.PATHERROR;
                }
            }
            else
            {
                return WriterStatus.TYPEMISSMATCH;
            }

            return WriterStatus.SUCCESSFULL;
        }
    }
}
