using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.BusinessLogic
{
    public class Hepler
    {
        public static string Serialize(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });
            return result;
        }
    }
    public enum Result
    {
        Passed,
        Failed
    }
    public enum Role
    {
        Buyer,
        Suppelier
    }
}