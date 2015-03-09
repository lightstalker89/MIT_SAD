using System;
using System.Collections.Generic;

namespace PassSecure.Converter
{
    using System.Web.Script.Serialization;

    public class TimeSpantoJSONConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new[] { typeof(TimeSpan) };
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return new TimeSpan(
                this.GetValue(dictionary, "days"),
                this.GetValue(dictionary, "hours"),
                this.GetValue(dictionary, "minutes"),
                this.GetValue(dictionary, "seconds"),
                this.GetValue(dictionary, "milliseconds"));
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var timeSpan = (TimeSpan)obj;

            var result = new Dictionary<string, object>
                {
                    { "days", timeSpan.Days },
                    { "hours", timeSpan.Hours },
                    { "minutes", timeSpan.Minutes },
                    { "seconds", timeSpan.Seconds },
                    { "milliseconds", timeSpan.Milliseconds }
                };

            return result;
        }

        private int GetValue(IDictionary<string, object> dictionary, string key)
        {
            const int DefaultValue = 0;

            object value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return DefaultValue;
            }

            if (value is int)
            {
                return (int)value;
            }

            var valueString = value as string;
            if (valueString == null)
            {
                return DefaultValue;
            }

            int returnValue;
            return !int.TryParse(valueString, out returnValue) ? DefaultValue : returnValue;
        }
    }
}
