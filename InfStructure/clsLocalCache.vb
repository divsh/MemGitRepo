Imports InfStructure

Public Class clsLocalCache
    Implements ICache, ICoherentCache

    Public Sub addNewBusinessObject(businessObject As IBO) Implements ICoherentCache.addNewBusinessObject
        Throw New NotImplementedException()
    End Sub

    Public Sub updateBusinessObjectAssociations(businessObject As IBO) Implements ICoherentCache.updateBusinessObjectAssociations
        Throw New NotImplementedException()
    End Sub

    Public Sub updateBusinessObjectAssociations(nearBusinessObject As IBO, nearAttributeName As String, oldValue As Object, newValue As Object) Implements ICoherentCache.updateBusinessObjectAssociations
        Throw New NotImplementedException()
    End Sub

    Public Sub enableValuesOverwrite(businessObject As IBO) Implements ICoherentCache.enableValuesOverwrite
        Throw New NotImplementedException()
    End Sub

    Public Sub updatePrimaryKey(businessObject As IBO, newValue As Object) Implements ICoherentCache.updatePrimaryKey
        Throw New NotImplementedException()
    End Sub

    Public Function allowValuesOverwrite(businessObject As IBO) As Boolean Implements ICoherentCache.allowValuesOverwrite
        Throw New NotImplementedException()
    End Function
End Class
