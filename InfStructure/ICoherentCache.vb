Option Strict On
Option Explicit On

Public Interface ICoherentCache
    Sub addNewBusinessObject(ByVal businessObject As IBO)
    Sub updateBusinessObjectAssociations(ByVal businessObject As IBO)
    Function allowValuesOverwrite(ByVal businessObject As IBO) As Boolean
    ''' <summary>
    ''' Enable a one time IBO_values overwrite on an INO
    ''' </summary>
    ''' <param name="businessObject"></param>
    ''' <remarks>Should only be used for cloning and merging back into the same parent.</remarks>
    Sub enableValuesOverwrite(ByVal businessObject As IBO)

    ''' <summary>
    ''' Updates all business object associations that use the near attribute for resolving their association.
    ''' </summary>
    ''' <param name="nearBusinessObject">The near business object.</param>
    ''' <param name="nearAttributeName">The name of the near attribute which has had its value changed.</param>
    ''' <param name="oldValue">The old value of the attribute/property.</param>
    ''' <param name="newValue">The new value of the attribute/property.</param>
    ''' <remarks></remarks>
    Sub updateBusinessObjectAssociations(ByVal nearBusinessObject As IBO, _
                                        ByVal nearAttributeName As String, _
                                        ByVal oldValue As Object, _
                                        ByVal newValue As Object)

    ''' <summary>
    ''' Update the primary key in the cache when a key attribute's value has been changed.
    ''' </summary>
    ''' <param name="businessObject"></param>
    ''' <param name="newValue"></param>
    ''' <remarks></remarks>
    Sub updatePrimaryKey(ByVal businessObject As IBO, ByVal newValue As Object)
End Interface


