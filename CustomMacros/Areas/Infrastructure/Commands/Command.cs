using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CustomMacros.Areas.Infrastructure.Commands
{
    public class Command
    {
        public string Name { get; private set; }

        protected IDictionary<string, string> props { get; private set; }

        protected Command()
        {
            Name = this.GetType().ToString();
            props = new Dictionary<string, string>();
        }

        public string GetProperty(string propKey)
        {
            return props[propKey];
        }

        public void SetProperty(string propKey, string propValue)
        {
            if (props.ContainsKey(propKey)) props[propKey] = propValue;
            else props.Add(propKey, propValue);
        }

        public IDictionary<string, string> GetProperties()
        {
            return props;
        }

        public void SetProperties(IDictionary<string, object> properties)
        {
            foreach (KeyValuePair<string, object> p in properties)
                props[p.Key] = "'" + p.Value.ToString() + "'";
        }

        public void SetProperties(string propJSON)
        {
            if (!string.IsNullOrEmpty(propJSON))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                IDictionary<string, string> jSonProps = ser.Deserialize<IDictionary<string, string>>(propJSON);
                foreach (KeyValuePair<string, string> p in jSonProps)
                    props[p.Key] = p.Value;
            }
        }
    }
}