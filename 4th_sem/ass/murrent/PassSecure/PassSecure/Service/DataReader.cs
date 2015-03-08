namespace PassSecure.Service
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;

    using PassSecure.Models;

    public class DataReader
    {
        private const string FileName = "data.pss";

        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public List<UserTraining> Read()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                return javaScriptSerializer.Deserialize<List<UserTraining>>(json);
            }
            return null;
        }

        public void Write(List<UserTraining> userTrainings)
        {
            string json = javaScriptSerializer.Serialize(userTrainings);

            File.WriteAllText(FileName, json);
        }
    }
}
