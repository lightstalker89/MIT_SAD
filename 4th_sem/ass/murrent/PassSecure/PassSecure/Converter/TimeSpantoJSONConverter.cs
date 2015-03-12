#region File Header
// <copyright file="TimeSpantoJSONConverter.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion

namespace PassSecure.Converter
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    #endregion

    /// <summary>
    /// </summary>
    public class TimeSpantoJSONConverter : JavaScriptConverter
    {
        /// <summary>
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new[] { typeof(TimeSpan) };
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dictionary">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="serializer">
        /// </param>
        /// <returns>
        /// </returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return new TimeSpan(
                this.GetValue(dictionary, "days"), 
                this.GetValue(dictionary, "hours"), 
                this.GetValue(dictionary, "minutes"), 
                this.GetValue(dictionary, "seconds"), 
                this.GetValue(dictionary, "milliseconds"));
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <param name="serializer">
        /// </param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// </summary>
        /// <param name="dictionary">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
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
