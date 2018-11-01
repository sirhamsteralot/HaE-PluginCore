using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace HaEPluginCore
{
    public static class HaEPluginConfigurationHandler
    {
        public static List<IHaESerializable> serializables = new List<IHaESerializable>();
        public static string StoragePath => Path.GetDirectoryName(typeof(HaEPluginConfigurationHandler).Assembly.Location);

        public interface IHaESerializable
        {
            string fileName { get; }
            object Configuration { get; set; }
        }

        public static void AddSerializable(IHaESerializable serializable)
        {
            serializables.Add(serializable);
        }

        public static void Serialize()
        {
            foreach (var serializable in serializables)
            {
                Serialize(serializable);
            }
        }

        public static void Serialize(IHaESerializable serializable)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream($"{StoragePath}\\{serializable.fileName}",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, serializable.Configuration);
            stream.Close();
        }

        public static void DeSerialize()
        {
            foreach (var serializable in serializables)
            {
                DeSerialize(serializable);
            }
        }

        public static void DeSerialize(IHaESerializable serializable)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream($"{StoragePath}\\{serializable.fileName}",
                                     FileMode.Create,
                                     FileAccess.Read, FileShare.Read);
            serializable.Configuration = (IHaESerializable)formatter.Deserialize(stream);
            stream.Close();
        }
    }
}
