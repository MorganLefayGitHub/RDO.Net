﻿using System.Diagnostics;
using System.Threading;

namespace DevZest.Data
{
    public abstract class Key<T> : Projection, IKey<T>
        where T : PrimaryKey
    {
        private sealed class ContainerModel : Model<T>
        {
            public ContainerModel(Key<T> key)
            {
                Debug.Assert(key != null);
                Debug.Assert(key.ParentModel == null);
                key.Construct(this, GetType(), string.Empty);
                Add(key);
                _key = key;
            }

            private readonly Key<T> _key;

            protected override T CreatePrimaryKey()
            {
                return _key.PrimaryKey;
            }

            internal override bool IsProjectionContainer
            {
                get { return true; }
            }
        }


        protected abstract T GetPrimaryKey();

        private T _primaryKey;
        public T PrimaryKey
        {
            get { return LazyInitializer.EnsureInitialized(ref _primaryKey, () => GetPrimaryKey()); }
        }

        internal override void EnsureConstructed()
        {
            if (ParentModel == null)
            {
                var containerModel = new ContainerModel(this);
                Debug.Assert(ParentModel == containerModel);
            }
        }
    }
}
