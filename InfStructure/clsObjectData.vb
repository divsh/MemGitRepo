Option Strict On
Option Explicit On

''' <summary>
''' This class represents the data retrieved from the ESP data store that can be used for setting values on an IBO.
''' </summary>
''' <remarks></remarks>
Public Class clsObjectData
    Implements IObjectData

    Private mValues As Object()

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="columnCount">The number of columns this data has in the database.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal columnCount As Integer)
        ReDim mValues(columnCount - 1)
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="theseValues">The full set of mValues to create this instance with.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal theseValues As Object())
        mValues = theseValues
    End Sub

    ''' <summary>
    ''' Get the values of this data for all columns.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property values() As Object()
        Get
            If mValues Is Nothing Then
                mValues = New Object() {}
            End If

            Return mValues
        End Get
    End Property

    ''' <summary>
    ''' Default property for getting or setting a particular column in this data.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Overridable Property item(ByVal index As Integer) As Object
        Get
            Return values(index)
        End Get
        Set(ByVal value As Object)
            values(index) = value
        End Set
    End Property

    ''' <summary>
    ''' Gets a copy of this object data.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function Clone() As IObjectData
        Return New clsESPObjectData(DirectCast(mValues.Clone, Object()))
    End Function
End Class
