﻿using DevZest.Data.Annotations.Primitives;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DevZest.Data.Annotations
{
    [CrossReference(typeof(_CheckConstraintAttribute))]
    [ModelDeclarationSpec(true, typeof(_Boolean))]
    public sealed class CheckConstraintAttribute : ModelDeclarationAttribute
    {
        private sealed class Validator : IValidator
        {
            public Validator(CheckConstraintAttribute checkAttribute, _Boolean condition)
            {
                _checkAttribute = checkAttribute;
                _condition = condition;
            }

            private readonly CheckConstraintAttribute _checkAttribute;
            private readonly _Boolean _condition;

            DataValidationError IValidator.Validate(DataRow dataRow)
            {
                return IsValid(dataRow) ? null : new DataValidationError(_checkAttribute.GetMessage(), GetValidationSource());
            }

            private bool IsValid(DataRow dataRow)
            {
                return _condition[dataRow] != false;
            }

            private IColumns GetValidationSource()
            {
                var expression = _condition.Expression;
                return expression == null ? Columns.Empty : expression.BaseColumns;
            }
        }

        public CheckConstraintAttribute(string name, string message)
            : base(name)
        {
            message.VerifyNotEmpty(nameof(message));
            Message = message;
        }

        public CheckConstraintAttribute(string name, Type messageResourceType, string message)
            : this(name, message)
        {
            MessageResourceType = messageResourceType.VerifyNotNull(nameof(messageResourceType));
            _messageGetter = messageResourceType.ResolveStaticGetter<string>(message);
        }

        public string Message { get; }

        public Type MessageResourceType { get; }

        private readonly Func<string> _messageGetter;

        private string GetMessage()
        {
            return _messageGetter != null ? _messageGetter() : Message;
        }

        private Func<Model, _Boolean> _conditionGetter;
        protected override void Initialize()
        {
            var getMethod = GetPropertyGetter(typeof(_Boolean));
            _conditionGetter = BuildConditionGetter(ModelType, getMethod);
        }

        private static Func<Model, _Boolean> BuildConditionGetter(Type modelType, MethodInfo getMethod)
        {
            var paramModel = Expression.Parameter(typeof(Model));
            var model = Expression.Convert(paramModel, modelType);
            var call = Expression.Call(model, getMethod);
            return Expression.Lambda<Func<Model, _Boolean>>(call, paramModel).Compile();
        }

        protected override ModelWireupEvent WireupEvent
        {
            get { return ModelWireupEvent.Initializing; }
        }

        private _Boolean GetCondition(Model model)
        {
            return _conditionGetter(model);
        }

        protected override void Wireup(Model model)
        {
            var condition = GetCondition(model);
            model.DbCheck(Name, Description, condition);
            model.Validators.Add(new Validator(this, condition));
        }

    }
}
