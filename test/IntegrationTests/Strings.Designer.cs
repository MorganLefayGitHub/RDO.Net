﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevZest.Data {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DevZest.Data.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ProductCategoryID&quot; : 1,
        ///      &quot;ParentProductCategoryID&quot; : null,
        ///      &quot;Name&quot; : &quot;Bikes&quot;,
        ///      &quot;RowGuid&quot; : &quot;cfbda25c-df71-47a7-b81b-64ee161aa37c&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : [
        ///         {
        ///            &quot;ProductCategoryID&quot; : 5,
        ///            &quot;Name&quot; : &quot;Mountain Bikes&quot;,
        ///            &quot;RowGuid&quot; : &quot;2d364ade-264a-433c-b092-4fcbf3804e01&quot;,
        ///            &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///            &quot;SubCategories&quot; : []
        ///         },
        ///          [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedJSON_ProductCategories {
            get {
                return ResourceManager.GetString("ExpectedJSON_ProductCategories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 71774,
        ///      &quot;RevisionNumber&quot; : 2,
        ///      &quot;OrderDate&quot; : &quot;2008-06-01T00:00:00.000&quot;,
        ///      &quot;DueDate&quot; : &quot;2008-06-13T00:00:00.000&quot;,
        ///      &quot;ShipDate&quot; : &quot;2008-06-08T00:00:00.000&quot;,
        ///      &quot;Status&quot; : 5,
        ///      &quot;OnlineOrderFlag&quot; : false,
        ///      &quot;PurchaseOrderNumber&quot; : &quot;PO348186287&quot;,
        ///      &quot;AccountNumber&quot; : &quot;10-4020-000609&quot;,
        ///      &quot;CustomerID&quot; : 29847,
        ///      &quot;ShipToAddressID&quot; : 1092,
        ///      &quot;BillToAddressID&quot; : 1092,
        ///      &quot;ShipMethod&quot; : &quot;CARGO TRANSPORT 5&quot;,
        ///      &quot;CreditCardApp [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedJSON_SalesOrder_71774 {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrder_71774", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 71774,
        ///      &quot;RevisionNumber&quot; : 2,
        ///      &quot;OrderDate&quot; : &quot;2008-06-01T00:00:00.000&quot;,
        ///      &quot;DueDate&quot; : &quot;2008-06-13T00:00:00.000&quot;,
        ///      &quot;ShipDate&quot; : &quot;2008-06-08T00:00:00.000&quot;,
        ///      &quot;Status&quot; : 5,
        ///      &quot;OnlineOrderFlag&quot; : false,
        ///      &quot;PurchaseOrderNumber&quot; : &quot;PO348186287&quot;,
        ///      &quot;AccountNumber&quot; : &quot;10-4020-000609&quot;,
        ///      &quot;CustomerID&quot; : 29847,
        ///      &quot;ShipToAddressID&quot; : 1092,
        ///      &quot;BillToAddressID&quot; : 1092,
        ///      &quot;ShipMethod&quot; : &quot;CARGO TRANSPORT 5&quot;,
        ///      &quot;CreditCardApp [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedJSON_SalesOrder_71774_with_ext {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrder_71774_with_ext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 71774,
        ///      &quot;SalesOrderDetailID&quot; : 110562,
        ///      &quot;OrderQty&quot; : 1,
        ///      &quot;ProductID&quot; : 836,
        ///      &quot;UnitPrice&quot; : 356.8980,
        ///      &quot;UnitPriceDiscount&quot; : 0,
        ///      &quot;RowGuid&quot; : &quot;e3a1994c-7a68-4ce8-96a3-77fdd3bbd730&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2008-06-01T00:00:00.000&quot;,
        ///      &quot;Extender&quot; : {
        ///         &quot;Product&quot; : {
        ///            &quot;Name&quot; : &quot;ML Road Frame-W - Yellow, 48&quot;,
        ///            &quot;ProductNumber&quot; : &quot;FR-R72Y-48&quot;
        ///         }
        ///      }
        ///   },
        ///   {
        ///      &quot;SalesOrderID&quot; : 71774,
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedJSON_SalesOrderDetail_71774_with_ext {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrderDetail_71774_with_ext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;Extender&quot; : {
        ///         &quot;Product&quot; : {
        ///            &quot;Name&quot; : &quot;ML Road Frame-W - Yellow, 48&quot;,
        ///            &quot;ProductNumber&quot; : &quot;FR-R72Y-48&quot;
        ///         }
        ///      }
        ///   },
        ///   {
        ///      &quot;Extender&quot; : {
        ///         &quot;Product&quot; : {
        ///            &quot;Name&quot; : &quot;ML Road Frame-W - Yellow, 38&quot;,
        ///            &quot;ProductNumber&quot; : &quot;FR-R72Y-38&quot;
        ///         }
        ///      }
        ///   },
        ///   {
        ///      &quot;Extender&quot; : {
        ///         &quot;Product&quot; : {
        ///            &quot;Name&quot; : null,
        ///            &quot;ProductNumber&quot; : null
        ///         }
        ///      }
        ///   }
        ///].
        /// </summary>
        internal static string ExpectedJSON_SalesOrderDetail_Lookup {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrderDetail_Lookup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ColumnList&quot; : [
        ///         71774,
        ///         &quot;SO71774&quot;
        ///      ]
        ///   },
        ///   {
        ///      &quot;ColumnList&quot; : [
        ///         71776,
        ///         &quot;SO71776&quot;
        ///      ]
        ///   }
        ///].
        /// </summary>
        internal static string ExpectedJSON_SalesOrderDynamicModel {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrderDynamicModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 71774,
        ///      &quot;RevisionNumber&quot; : 2,
        ///      &quot;OrderDate&quot; : &quot;2008-06-01T00:00:00.000&quot;,
        ///      &quot;DueDate&quot; : &quot;2008-06-13T00:00:00.000&quot;,
        ///      &quot;ShipDate&quot; : &quot;2008-06-08T00:00:00.000&quot;,
        ///      &quot;Status&quot; : 5,
        ///      &quot;OnlineOrderFlag&quot; : false,
        ///      &quot;PurchaseOrderNumber&quot; : &quot;PO348186287&quot;,
        ///      &quot;AccountNumber&quot; : &quot;10-4020-000609&quot;,
        ///      &quot;CustomerID&quot; : 29847,
        ///      &quot;ShipToAddressID&quot; : 1092,
        ///      &quot;BillToAddressID&quot; : 1092,
        ///      &quot;ShipMethod&quot; : &quot;CARGO TRANSPORT 5&quot;,
        ///      &quot;CreditCardApp [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExpectedJSON_SalesOrderEdit_71774 {
            get {
                return ResourceManager.GetString("ExpectedJSON_SalesOrderEdit_71774", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ProductID&quot; : 836
        ///   },
        ///   {
        ///      &quot;ProductID&quot; : 822
        ///   },
        ///   {
        ///      &quot;ProductID&quot; : -1
        ///   }
        ///].
        /// </summary>
        internal static string JSON_SalesOrderDetail_ForeignKeys {
            get {
                return ResourceManager.GetString("JSON_SalesOrderDetail_ForeignKeys", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ProductCategoryID&quot; : 1,
        ///      &quot;ParentProductCategoryID&quot; : null,
        ///      &quot;Name&quot; : &quot;Bikes&quot;,
        ///      &quot;RowGuid&quot; : &quot;cfbda25c-df71-47a7-b81b-64ee161aa37c&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   },
        ///   {
        ///      &quot;ProductCategoryID&quot; : 2,
        ///      &quot;ParentProductCategoryID&quot; : null,
        ///      &quot;Name&quot; : &quot;Other&quot;,
        ///      &quot;RowGuid&quot; : &quot;09e91437-ba4f-4b1a-8215-74184fd95db8&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   }
        ///].
        /// </summary>
        internal static string Mock_ProductCategories_Level1 {
            get {
                return ResourceManager.GetString("Mock_ProductCategories_Level1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ProductCategoryID&quot; : 3,
        ///      &quot;ParentProductCategoryID&quot; : 1,
        ///      &quot;Name&quot; : &quot;Mountain Bikes&quot;,
        ///      &quot;RowGuid&quot; : &quot;2d364ade-264a-433c-b092-4fcbf3804e01&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   },
        ///   {
        ///      &quot;ProductCategoryID&quot; : 4,
        ///      &quot;ParentProductCategoryID&quot; : 1,
        ///      &quot;Name&quot; : &quot;Road Bikes&quot;,
        ///      &quot;RowGuid&quot; : &quot;000310c0-bcc8-42c4-b0c3-45ae611af06b&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   },
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Mock_ProductCategories_Level2 {
            get {
                return ResourceManager.GetString("Mock_ProductCategories_Level2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;ProductCategoryID&quot; : 8,
        ///      &quot;ParentProductCategoryID&quot; : 6,
        ///      &quot;Name&quot; : &quot;Handlebars&quot;,
        ///      &quot;RowGuid&quot; : &quot;3ef2c725-7135-4c85-9ae6-ae9a3bdd9283&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   },
        ///   {
        ///      &quot;ProductCategoryID&quot; : 9,
        ///      &quot;ParentProductCategoryID&quot; : 6,
        ///      &quot;Name&quot; : &quot;Bottom Brackets&quot;,
        ///      &quot;RowGuid&quot; : &quot;a9e54089-8a1e-4cf5-8646-e3801f685934&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2002-06-01T00:00:00.000&quot;,
        ///      &quot;SubCategories&quot; : []
        ///   },
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Mock_ProductCategories_Level3 {
            get {
                return ResourceManager.GetString("Mock_ProductCategories_Level3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 1,
        ///      &quot;SalesOrderDetailID&quot; : 1,
        ///      &quot;OrderQty&quot; : 1,
        ///      &quot;ProductID&quot; : 836,
        ///      &quot;UnitPrice&quot; : 356.8980,
        ///      &quot;UnitPriceDiscount&quot; : 0.0000,
        ///      &quot;LineTotal&quot; : 356.8980,
        ///      &quot;RowGuid&quot; : &quot;e3a1994c-7a68-4ce8-96a3-77fdd3bbd730&quot;,
        ///      &quot;ModifiedDate&quot; : &quot;2008-06-01T00:00:00.000&quot;
        ///   },
        ///   {
        ///      &quot;SalesOrderID&quot; : 1,
        ///      &quot;SalesOrderDetailID&quot; : 2,
        ///      &quot;OrderQty&quot; : 1,
        ///      &quot;ProductID&quot; : 822,
        ///      &quot;UnitPrice&quot; : 356.8980,
        ///      &quot;UnitPriceDiscount&quot; : 0.000 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Mock_SalesOrderDetail {
            get {
                return ResourceManager.GetString("Mock_SalesOrderDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///   {
        ///      &quot;SalesOrderID&quot; : 1,
        ///      &quot;RevisionNumber&quot; : 2,
        ///      &quot;OrderDate&quot; : &quot;2008-06-01T00:00:00.000&quot;,
        ///      &quot;DueDate&quot; : &quot;2008-06-13T00:00:00.000&quot;,
        ///      &quot;ShipDate&quot; : &quot;2008-06-08T00:00:00.000&quot;,
        ///      &quot;Status&quot; : 5,
        ///      &quot;OnlineOrderFlag&quot; : false,
        ///      &quot;SalesOrderNumber&quot; : &quot;SO71774&quot;,
        ///      &quot;PurchaseOrderNumber&quot; : &quot;PO348186287&quot;,
        ///      &quot;AccountNumber&quot; : &quot;10-4020-000609&quot;,
        ///      &quot;CustomerID&quot; : 29847,
        ///      &quot;ShipToAddressID&quot; : 1092,
        ///      &quot;BillToAddressID&quot; : 1092,
        ///      &quot;ShipMethod&quot; : &quot;CARGO  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Mock_SalesOrderHeader {
            get {
                return ResourceManager.GetString("Mock_SalesOrderHeader", resourceCulture);
            }
        }
    }
}
