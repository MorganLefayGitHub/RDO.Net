﻿using DevZest.Data;
using DevZest.Data.Annotations;
using DevZest.Data.SqlServer;

namespace DevZest.Samples.AdventureWorksLT
{
    public class Address : BaseModel<Address.Key>
    {
        public sealed class Key : PrimaryKey
        {
            public Key(_Int32 addressID)
            {
                AddressID = addressID;
            }

            public _Int32 AddressID { get; private set; }
        }

        public class Ref : Model<Address.Key>
        {
            static Ref()
            {
                RegisterColumn((Ref _) => _.AddressID, _AddressID);
            }

            private Key _primaryKey;
            public sealed override Key PrimaryKey
            {
                get
                {
                    if (_primaryKey == null)
                        _primaryKey = new Key(AddressID);
                    return _primaryKey;
                }
            }

            public _Int32 AddressID { get; private set; }
        }

        public class Lookup : ModelExtension
        {
            static Lookup()
            {
                RegisterColumn((Lookup _) => _.AddressLine1, _AddressLine1);
                RegisterColumn((Lookup _) => _.AddressLine2, _AddressLine2);
                RegisterColumn((Lookup _) => _.City, _City);
                RegisterColumn((Lookup _) => _.StateProvince, _StateProvince);
                RegisterColumn((Lookup _) => _.CountryRegion, _CountryRegion);
                RegisterColumn((Lookup _) => _.PostalCode, _PostalCode);
            }

            public _String AddressLine1 { get; private set; }
            public _String AddressLine2 { get; private set; }
            public _String City { get; private set; }
            public _String StateProvince { get; private set; }
            public _String CountryRegion { get; private set; }
            public _String PostalCode { get; private set; }
        }

        public static readonly Mounter<_Int32> _AddressID;
        public static readonly Mounter<_String> _AddressLine1;
        public static readonly Mounter<_String> _AddressLine2;
        public static readonly Mounter<_String> _City;
        public static readonly Mounter<_String> _StateProvince;
        public static readonly Mounter<_String> _CountryRegion;
        public static readonly Mounter<_String> _PostalCode;

        static Address()
        {
            _AddressID = RegisterColumn((Address _) => _.AddressID);
            _AddressLine1 = RegisterColumn((Address _) => _.AddressLine1);
            _AddressLine2 = RegisterColumn((Address _) => _.AddressLine2);
            _City = RegisterColumn((Address _) => _.City);
            _StateProvince = RegisterColumn((Address _) => _.StateProvince);
            _CountryRegion = RegisterColumn((Address _) => _.CountryRegion);
            _PostalCode = RegisterColumn((Address _) => _.PostalCode);
        }

        private Key _primaryKey;
        public sealed override Key PrimaryKey
        {
            get
            {
                if (_primaryKey == null)
                    _primaryKey = new Key(AddressID);
                return _primaryKey;
            }
        }

        [Identity(1, 1)]
        public _Int32 AddressID { get; private set; }

        [Required]
        [AsNVarChar(60)]
        public _String AddressLine1 { get; private set; }

        [AsNVarChar(60)]
        public _String AddressLine2 { get; private set; }

        [Required]
        [AsNVarChar(30)]
        public _String City { get; private set; }

        [UdtName]
        public _String StateProvince { get; private set; }

        [UdtName]
        public _String CountryRegion { get; private set; }

        [Required]
        [AsNVarChar(15)]
        public _String PostalCode { get; private set; }
    }
}
