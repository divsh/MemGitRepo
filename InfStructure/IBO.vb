Imports InfStructure

Public Interface IBO
    Event changeDirty()
    Event change(ByVal source As Object, ByVal e As System.EventArgs)

    ReadOnly Property description As String

    Sub setID(ByVal thisID As Integer)
    Function loadFromStorage() As Boolean
    ReadOnly Property ID() As Integer
    ReadOnly Property MaintTime As DateTime
    ReadOnly Property Version As Long
    Property isDirty() As Boolean
    Property values() As Object
    ReadOnly Property FieldCount As Integer
    Property tag As Object
    Property isReadOnly As Boolean
    Property isStored As Boolean
    Property parent As IBO
    Function Save() As Boolean
    Function Delete() As Boolean
    Sub Initialize()
    Function initialise() As Boolean
End Interface
