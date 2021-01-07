using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EBank.ApiServer.Infrastructure.ModelBinders
{
    /// <summary>
    /// 支持逗号(,)分隔字符串的模型绑定
    /// </summary>
    public class StringToListModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            var values = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim())).ToArray();

            var typedValues = Array.CreateInstance(elementType, values.Length);

            values.CopyTo(typedValues, 0);

            bindingContext.Model = typedValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
