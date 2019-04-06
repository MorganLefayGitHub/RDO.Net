﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DevZest.Data.AspNetCore.Primitives
{
    public class DataSetModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var modelType = context.Metadata.ModelType;
            if (modelType.IsDataSet())
            {
                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                return (IModelBinder)Activator.CreateInstance(
                    typeof(DataSetModelBinder<>).MakeGenericType(modelType.GetGenericArguments()[0]),
                    loggerFactory);
            }

            return null;
        }
    }
}