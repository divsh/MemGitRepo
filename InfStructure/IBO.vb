Public Interface IBO
    Event changeDirty()
    Event change(ByVal source As Object, ByVal e As System.EventArgs)

    Property description As String

    Sub setID(ByVal thisID As Integer)
    Function loadFromStorage() As Boolean
    ReadOnly Property ID() As Integer
    ReadOnly Property MaintTime As DateTime
    ReadOnly Property Version As Long
    Property IsNew() As Boolean
    Property isDirty() As Boolean
    Property values() As Object
    ReadOnly Property FieldCount As Integer

    Function Save() As Boolean
    Function Delete() As Boolean
    Sub Initialize()

End Interface
