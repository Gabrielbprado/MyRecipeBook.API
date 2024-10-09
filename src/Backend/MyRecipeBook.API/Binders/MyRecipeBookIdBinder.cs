using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace MyRecipeBook.API.Binders;

public class MyRecipeBookIdBinder(SqidsEncoder<long> idEncoder) : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
            return Task.CompletedTask;

        var id = idEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
