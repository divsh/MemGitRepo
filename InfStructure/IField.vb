Option Strict On
Option Explicit On
Public Interface IField
    ReadOnly Property name() As String
    ReadOnly Property sequence() As Integer
    ReadOnly Property datatype() As WhereClauseBuilder.IColumn
    ReadOnly Property size() As Integer
    ReadOnly Property isEnumeration() As Boolean
    ReadOnly Property isForeignKey() As Boolean
    ReadOnly Property hasDBColumn() As Boolean
End Interface
