﻿namespace DevZest.Data.CodeAnalysis
{
    static partial class KnownTypes
    {
        private static partial class Namespaces
        {
            // Must end with "."
            public const string System = "System.";
            public const string System_Threading = "System.Threading.";
            public const string System_Threading_Tasks = "System.Threading.Tasks.";
            public const string Data_Addons = Data + "Addons.";
            public const string Data = "DevZest.Data.";
            public const string Data_Primitives = Data + "Primitives.";
            public const string Data_Annotations = Data + "Annotations.";
            public const string Data_Annotations_Primitives = Data_Annotations + "Primitives.";
        }

        public const string AttributeUsageAttribute = Namespaces.System + nameof(AttributeUsageAttribute);

        public const string Model = Namespaces.Data + nameof(Model);
        public const string ModelOf = Namespaces.Data + "Model`1";
        public const string Column = Namespaces.Data + nameof(Column);
        public const string LocalColumnOf = Namespaces.Data + "LocalColumn`1";
        public const string ColumnList = Namespaces.Data + nameof(ColumnList);
        public const string Projection = Namespaces.Data + nameof(Projection);
        public const string CandidateKey = Namespaces.Data + nameof(CandidateKey);
        public const string AscAttribute = Namespaces.Data_Annotations + nameof(AscAttribute);
        public const string DescAttribute = Namespaces.Data_Annotations + nameof(DescAttribute);
        public const string PropertyRegistrationAttribute = Namespaces.Data_Annotations_Primitives + nameof(PropertyRegistrationAttribute);
        public const string CreateKeyAttribute = Namespaces.Data_Annotations_Primitives + nameof(CreateKeyAttribute);
        public const string ModelDeclarationAttribute = Namespaces.Data_Annotations_Primitives + nameof(ModelDeclarationAttribute);
        public const string ModelImplementationAttribute = Namespaces.Data_Annotations_Primitives + nameof(ModelImplementationAttribute);
        public const string CrossReferenceAttribute = Namespaces.Data_Annotations_Primitives + nameof(CrossReferenceAttribute);
        public const string ModelDeclarationSpecAttribute = Namespaces.Data_Annotations_Primitives + nameof(ModelDeclarationSpecAttribute);

        public const string ModelDesignerSpecAttribute = Namespaces.Data_Annotations_Primitives + nameof(ModelDesignerSpecAttribute);

        public const string DbSession = Namespaces.Data_Primitives + nameof(DbSession);
        public const string DbTableOf = Namespaces.Data + "DbTable`1";
        public const string RelationshipAttribute = Namespaces.Data_Annotations + nameof(RelationshipAttribute);
        public const string _RelationshipAttribute = Namespaces.Data_Annotations + nameof(_RelationshipAttribute);
        public const string KeyMapping = Namespaces.Data + nameof(KeyMapping);

        public const string DbMockOf = Namespaces.Data + "DbMock`1";
        public const string TaskOf = Namespaces.System_Threading_Tasks + "Task`1";
        public const string IProgressOf = Namespaces.System + "IProgress`1";
        public const string DbInitProgress = Namespaces.Data + nameof(DbInitProgress);
        public const string CancellationToken = Namespaces.System_Threading + nameof(CancellationToken);
    }
}
