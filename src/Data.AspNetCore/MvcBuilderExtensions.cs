﻿using DevZest.Data.AspNetCore.ClientValidation;
using DevZest.Data.AspNetCore.Primitives;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DevZest.Data.AspNetCore
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddDataSetMvc(this IMvcBuilder mvcBuilder, Action<DataSetMvcConfiguration> configurationExpression = null)
        {
            var expr = configurationExpression ?? delegate { };
            var config = new DataSetMvcConfiguration();

            mvcBuilder.AddMvcOptions(options => {
                options.ModelValidatorProviders.Add(new DataSetValidatorProvider());
                options.ModelBinderProviders.Insert(0, new DataSetModelBinderProvider());

                config.AddClientValidators(options.ModelBindingMessageProvider);
                expr(config);
            });

            var services = mvcBuilder.Services;
            services.AddSingleton(config);
            services.TryAddSingleton<DataSetValidationHtmlAttributeProvider, DefaultDataSetValidationHtmlAttributeProvider>();

            return mvcBuilder;
        }

        internal static void AddClientValidators(this DataSetMvcConfiguration config, ModelBindingMessageProvider messageProvider)
        {
            AddClientValidators(config,
                new MaxLengthClientValidator(),
                new NumericClientValidator(messageProvider),
                new RegularExpressionClientValidator(),
                new RequiredClientValidator(),
                new StringLengthClientValidator());
        }

        private static void AddClientValidators(DataSetMvcConfiguration config, params IDataSetClientValidator[] clientValidators)
        {
            for (int i = 0; i < clientValidators.Length; i++)
                config.DataSetClientValidators.Add(clientValidators[i]);
        }
    }
}