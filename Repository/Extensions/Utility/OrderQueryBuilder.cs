using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
    public static class OrderQueryBuilder
    {
        //orderBy=name,age desc
        //here the url has name and age fields
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            //1-split fildes
            string[] propertiesFromQueryString = orderByQueryString.Trim().Split(','); // remove the comma and create array from qyuery string params["name","age"] 

            //2-check if the provided fields in the URL exist in our type T or not, by using reflection
            //get the instance and static properties
            PropertyInfo[] realproperPties  = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);//create array from all real["","",....] 



            StringBuilder orderQueryBuilder = new StringBuilder();

            //3-compare between the properties set in the url and real property get for the type by reflection
            foreach (string param in propertiesFromQueryString)
            {
                if(string.IsNullOrEmpty(param))  continue;


                string onePropertyFromQueryString = param.Split(' ')[0];//split the param if it contain white space and select first index => if it "na me" [0]=>"na" and if it "name" [0] => "name"

                PropertyInfo oneRealProperty = realproperPties.FirstOrDefault(PropertyInfo => PropertyInfo.Name.Equals(onePropertyFromQueryString, StringComparison.InvariantCultureIgnoreCase));//match "name" Qstring to Name real prop

                if (oneRealProperty == null) continue;


                var direction = param.EndsWith(" desc") ? "descending" : "ascending"; 

                //add the matched result to the string builder
                orderQueryBuilder.Append($"{oneRealProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' '); //remove the comma and space 
            return orderQuery;
        
        
        
        }
    }
}
