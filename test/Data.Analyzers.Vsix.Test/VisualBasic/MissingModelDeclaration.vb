﻿Imports DevZest.Data.Annotations

Public Class MissingModelDeclaration
    Inherits Model

    <_CheckConstraint>
    Private ReadOnly Property CK_AlwaysTrue As _Boolean
        Get
            Return _Boolean.Const(True)
        End Get
    End Property
End Class