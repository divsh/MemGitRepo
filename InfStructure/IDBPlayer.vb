Public Interface IDBContext
    Function AddNewBusinessObject(ByVal businessObject As IBO) As Integer
    Function Save(ByVal businessObject As IBO) As Boolean
    Function Delete(ByVal businessObject As IBO) As Boolean
    Function LoadFromStorage(ByVal businessObject As IBO) As Boolean


    Sub updateBusinessObjectAssociations(ByVal businessObject As IBO)

    ''' <summary>
    ''' Updates all business object associations that use the near attribute for resolving their association.
    ''' </summary>
    ''' <param name="nearBusinessObject">The near business object.</param>
    ''' <param name="nearAttributeName">The name of the near attribute which has had its value changed.</param>
    ''' <param name="oldValue">The old value of the attribute/property.</param>
    ''' <param name="newValue">The new value of the attribute/property.</param>
    ''' <remarks></remarks>
    Sub updateBusinessObjectAssociations(ByVal nearBusinessObject As IBO,
                                        ByVal nearAttributeName As String,
                                        ByVal oldValue As Object,
                                        ByVal newValue As Object)


    Function GetBusinessObject(Of T As {IBO})(ByVal id As Integer, Optional ByVal refresh As Boolean = False) As T
    Function GetSomething(ByVal className As String, Optional ByVal refresh As Boolean = False) As IBOs
    Function GetSomething(ByVal className As String, ByVal whereClause As String) As IBOs

    Function GetBusinessCollection(Of T As {IBO})(Optional ByVal refresh As Boolean = False) As clsBusinessObjectCollection(Of T)
    Function GetBusinessCollection(Of T As {IBO})(ByVal whereClause As String) As clsBusinessObjectCollection(Of T)

    Function GetRelatedBUsinessObject(Of T As {IBO})(ByVal businessObject As IBO, ByVal associationName As String)
    Function GetRelated(ByVal parentBusinessObject As IBO, ByVal className As String, ByVal associationName As String, ByVal refresh As Boolean) As IBOs
    Function GetRelatedBusinessObjectCollection(Of TParent As {IBO}, TChild As {IBO})(ByVal parentBusinessObject As TParent, ByVal associationName As String) As clsBusinessObjectCollection(Of TChild)

End Interface
