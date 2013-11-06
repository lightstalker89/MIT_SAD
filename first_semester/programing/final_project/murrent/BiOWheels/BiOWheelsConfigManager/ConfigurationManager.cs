// *******************************************************
// * <copyright file="ConfigurationManager.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsConfigManager
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing the <see cref="ConfigurationManager"/> and its interaction logic
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        /// <inheritdoc/>
        public object Load<T>(string configFileName)
        {
            object deserializedObject;

            if (File.Exists(configFileName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (StreamReader streamReader = new StreamReader(configFileName))
                    {
                        deserializedObject = serializer.Deserialize(streamReader.BaseStream);
                    }
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    return new LoaderException(invalidOperationException.Message, typeof(InvalidOperationException));
                }
                catch (IOException systemIOException)
                {
                    return new LoaderException(systemIOException.Message, typeof(IOException));
                }
                catch (ArgumentNullException argumentNullException)
                {
                    return new LoaderException(argumentNullException.Message, typeof(IOException));
                }
                catch (UnauthorizedAccessException unauthorizedAccessException)
                {
                    return new LoaderException(unauthorizedAccessException.Message, typeof(IOException));
                }
                catch (ArgumentException argumentException)
                {
                    return new LoaderException(argumentException.Message, typeof(IOException));
                }
            }
            else
            {
                return new LoaderException("File not found", typeof(FileNotFoundException));
            }

            return (T)deserializedObject;
        }

        /// <inheritdoc/>
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
                catch (IOException)
                {
                    return WriterStatus.FAILED;
                }
                catch (ArgumentNullException)
                {
                    return WriterStatus.PATHISNULL;
                }
                catch (UnauthorizedAccessException)
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