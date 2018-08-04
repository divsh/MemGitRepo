Imports InfStructure

Public Class clsDBContext
    Implements IDBContext

    Dim mDataStore As Object

    Public Function addNewBusinessObject(businessObject As IBO) As Integer Implements IDBContext.AddNewBusinessObject
        Dim table As String = businessObject.GetType.ToString
        businessObject.values
        businessObject.FieldCount
        businessObject.field



        mDataStore()
        Throw New NotImplementedException()
    End Function

    Public Sub updateBusinessObjectAssociations(businessObject As IBO) Implements IDBContext.updateBusinessObjectAssociations
        Throw New NotImplementedException()
    End Sub

    Public Sub updateBusinessObjectAssociations(nearBusinessObject As IBO, nearAttributeName As String, oldValue As Object, newValue As Object) Implements IDBContext.updateBusinessObjectAssociations
        Throw New NotImplementedException()
    End Sub

    Public Function Save(businessObject As IBO) As Boolean Implements IDBContext.Save
        Throw New NotImplementedException()
    End Function

    Public Function Delete(businessObject As IBO) As Boolean Implements IDBContext.Delete
        Throw New NotImplementedException()
    End Function

    Public Function loadFromStorage(businessObject As IBO) As Boolean Implements IDBContext.loadFromStorage
        Throw New NotImplementedException()
    End Function

    Public Function GetBusinessObject(Of T As IBO)(id As Integer, Optional refresh As Boolean = False) As T Implements IDBContext.GetBusinessObject
        Throw New NotImplementedException()
    End Function

    Public Function GetSomething(className As String, Optional refresh As Boolean = False) As IBOs Implements IDBContext.GetSomething
        Throw New NotImplementedException()
    End Function

    Public Function GetSomething(className As String, whereClause As String) As IBOs Implements IDBContext.GetSomething
        Throw New NotImplementedException()
    End Function

    Public Function GetBusinessCollection(Of T As IBO)(Optional refresh As Boolean = False) As clsBusinessObjectCollection(Of T) Implements IDBContext.GetBusinessCollection
        Throw New NotImplementedException()
    End Function

    Public Function GetBusinessCollection(Of T As IBO)(whereClause As String) As clsBusinessObjectCollection(Of T) Implements IDBContext.GetBusinessCollection
        Throw New NotImplementedException()
    End Function

    Public Function GetRelatedBUsinessObject(Of T As IBO)(businessObject As IBO, associationName As String) As Object Implements IDBContext.GetRelatedBUsinessObject
        Throw New NotImplementedException()
    End Function

    Public Function GetRelated(parentBusinessObject As IBO, className As String, associationName As String, refresh As Boolean) As IBOs Implements IDBContext.GetRelated
        Throw New NotImplementedException()
    End Function

    Public Function GetRelatedBusinessObjectCollection(Of TParent As IBO, TChild As IBO)(parentBusinessObject As TParent, associationName As String) As clsBusinessObjectCollection(Of TChild) Implements IDBContext.GetRelatedBusinessObjectCollection
        Throw New NotImplementedException()
    End Function
End Class
