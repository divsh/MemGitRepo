Option Strict On
Option Explicit On

Public Interface IBOs
    Inherits IEnumerable
    Sub add(ByVal businessObject As IBO)
    ''' <summary>
    ''' Inserts the business object in the collection at the specified index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <param name="businessObject"></param>
    ''' <remarks></remarks>
    Sub insert(ByVal index As Integer, ByVal businessObject As IBO)
    ReadOnly Property item(ByVal index As Integer) As IBO
    ReadOnly Property itemZeroBase(ByVal index As Integer) As IBO
    ReadOnly Property count() As Integer
    Sub remove(ByVal index As Integer)
    ReadOnly Property BusinessClassName() As String
    ReadOnly Property ToStringList() As String
    'Property sortField() As String
    Function contains(ByVal businessObject As IBO) As Boolean
    'Sub sort(ByVal fieldName As String, Optional ByVal ascending As Boolean = True, Optional ByVal numeric As Boolean = False)
End Interface
