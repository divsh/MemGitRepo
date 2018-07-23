Option Strict On
Option Explicit On

''' <summary>
''' Interface to be implemented by classes that are the child component within a composition association i.e. where the child is dependant on the parent and if the parent
''' is deleted, the child must also be deleted.
''' </summary>
''' <remarks></remarks>
Public Interface IChildComponent
    ''' <summary>
    ''' Flag indicating if the object is to be deleted.
    ''' </summary>
    ''' <value></value>
    ''' <returns>True if the object is to be deleted. False if not.</returns>
    ''' <remarks></remarks>
    Property isToBeDeleted As Boolean
End Interface
