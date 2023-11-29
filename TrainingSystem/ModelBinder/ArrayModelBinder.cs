using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace TrainingSystem.ModelBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        //Attempts to bind a model.
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //we extracted the value(a comma separated strings of GUIDS) by the ValueProvider.GetValue()
            string providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            //providedValue = "16431a59-0881-4d10-dade-08db26d0827e,818bd8cf-c047-41ea-dadf-08db26d0827e"
            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;

            }


            //with the reflection help we store the generic type that IEnumerable hold
            //in our case it is GUID
            Type genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            //genericType={Name = "Guid" FullName = "System.Guid"}


            //here we convert genericType to a GUID Type
            var converter = TypeDescriptor.GetConverter(genericType);
            //converter={System.ComponentModel.GuidConverter}


            //craete array of object type which consist all the string GUID values
            object[] objectArray = providedValue
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => converter.ConvertFromString(str.Trim()))
                .ToArray();

            //create arrray of GUIDS by copy the object array
            Array GuidArray = Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(GuidArray, 0);

            //assign the guid array to bindingContext
            bindingContext.Model = GuidArray;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
