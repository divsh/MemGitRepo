Option Strict On
Option Explicit On

Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Expressions

Public Class clsBusinessObjectUtilities

        ''' <summary>
        ''' Determines if two business objects are equal
        ''' </summary>
        ''' <param name="businessObject1">first business object to compare</param>
        ''' <param name="businessObject2">second business object to compare</param>
        ''' <returns>True if the business objects are equal otherwise false</returns>
        ''' <remarks></remarks>
        Public Shared Function areBusinessObjectsEqual(ByVal businessObject1 As IBO, ByVal businessObject2 As IBO) As Boolean
            If businessObject1 Is businessObject2 Then
                Return True
            ElseIf (businessObject1 Is Nothing Or businessObject2 Is Nothing) Then
                Return False
            ElseIf DirectCast(businessObject1, Object).GetType Is DirectCast(businessObject2, Object).GetType AndAlso
                   businessObject1.ID > 0 AndAlso businessObject2.ID > 0 AndAlso
                   businessObject1.ID = businessObject2.ID Then
                Return True
            End If

            Return False
        End Function

        Public Shared Function getSafeBusObjID(ByVal value As Object) As Integer
            'SCM 35318 - The routing board form assigns a temporary negative IBO id to a new route when created before
            'the route is saved to the database. getSafeLong needs to be used rather than getSafeNonNegativeLong
            'until the routing board form is refactored.
            Return getSafeLong(value)
        End Function

    ''' <summary>
    ''' Compare the control value with the attribute value to see if it has changed.
    ''' </summary>
    ''' <param name="busObj"></param>
    ''' <param name="attributeName"></param>
    ''' <param name="attributeValue"></param>
    ''' <param name="controlValue"></param>
    ''' <param name="arrayIdx"></param>
    ''' <returns></returns>
    ''' <remarks>Craig 13/6/02: If controlValue and attributeValue are both zero length strings then
    ''' isFieldNull returns true for the attributeValue and hasValueChanged gets
    ''' returned as true even though nothing has changed. To fix this I have
    '''  commented out the code in isStringFieldNull that returns true for zero length string.
    ''' It seems that zero length strings ought NOT be treated as nulls.</remarks>
    Public Shared Function hasValueChanged(ByVal busObj As IBO, ByVal attributeName As String, ByVal attributeValue As Object, ByVal controlValue As Object, Optional ByVal arrayIdx As Integer = -1) As Boolean
        Dim isAttributeNull As Boolean = busObj.isFieldNull(attributeName, arrayIdx)
        'check for a null date value and if found replace with an actual null value
        If TypeOf (controlValue) Is Date Then
            If CDate(controlValue) = InfServer.clsSession.NULL_DATE Then controlValue = System.DBNull.Value
        End If
        If (Convert.IsDBNull(controlValue) Or controlValue Is Nothing) And Not isAttributeNull Then
            hasValueChanged = True
        ElseIf Not (Convert.IsDBNull(controlValue) Or controlValue Is Nothing) And isAttributeNull Then
            hasValueChanged = True
        ElseIf (Convert.IsDBNull(controlValue) Or controlValue Is Nothing) And isAttributeNull Then
            hasValueChanged = False
        Else
            If TypeOf (attributeValue) Is Boolean Or TypeOf (controlValue) Is Boolean Then
                ' Note that:  True = -1  but True <> 1
                hasValueChanged = Convert.ToBoolean(attributeValue) <> Convert.ToBoolean(controlValue)
            ElseIf (TypeOf (attributeValue) Is Date) And (TypeOf (controlValue) Is String) Then
                hasValueChanged = hasDateValueChanged(CStr(controlValue), CDate(attributeValue))
            ElseIf (TypeOf (controlValue) Is Date) And (TypeOf (attributeValue) Is String) Then
                hasValueChanged = hasDateValueChanged(CStr(attributeValue), CDate(controlValue))
            ElseIf TypeOf (attributeValue) Is Decimal Or TypeOf (controlValue) Is Decimal Then
                hasValueChanged = Convert.ToDecimal(attributeValue) <> Convert.ToDecimal(controlValue)
            Else
                hasValueChanged = CStr(controlValue) <> CStr(attributeValue)
            End If
        End If

    End Function


#Region "GetSafeDateTypes"
    ''' <summary>
    ''' getSafeString
    ''' If the value is not null, nothing or a single blank space return the value 
    ''' coerced to a string, otherwise return an empty string
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>Either an empty string or the supplied value as a string</returns>
    ''' <remarks>Why are we treating a single blank space as an invalid string?
    ''' </remarks>
    Public Shared Function getSafeString(ByVal value As Object) As String
        Dim safeString As String = ""
        If Not value Is Nothing AndAlso Not Convert.IsDBNull(value) Then
            'Exclude single space strings???
            If Not (TypeOf value Is String AndAlso CStr(value) = " ") Then
                safeString = CType(value, String)
            End If
        End If
        Return safeString
    End Function

    Public Shared Function getSafeDate(ByVal value As Object) As Date
        If value Is Nothing Or Convert.IsDBNull(value) Then
            Return Utilities.CommonMethods.NULL_DATE
        ElseIf String.IsNullOrEmpty(CStr(value)) Then
            Return Utilities.CommonMethods.NULL_DATE
        End If
            Return CDate(value)
        End Function

    Public Shared Function getSafeBoolean(ByVal value As Object) As Boolean
        If value Is Nothing Or Convert.IsDBNull(value) Then
            Return False
        ElseIf String.IsNullOrEmpty(CStr(value)) Then
            Return False
        End If
        Return CBool(value)
    End Function

    ''' <summary>
    ''' Given a value of any type, return the value as an integer if it is a representation of an integer,
    ''' Return zero otherwise.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultValue"></param>
    ''' <returns>Returns the integer value or zero if the supplied value cannot be converted.</returns>
    ''' <remarks></remarks>
    Public Shared Function getSafeLong(ByVal value As Object, Optional ByVal defaultValue As Integer = 0) As Integer
        Dim safeLong As Integer = 0
        If TypeOf value Is Boolean Then
            safeLong = CType(value, Integer)
        Else
            'check that we don't have an object that is nothing or a null value
            'as these will cause an exception on the CStr in the tryparse
            If value Is Nothing OrElse value.Equals(DBNull.Value) _
                    OrElse Not Integer.TryParse(CStr(value), safeLong) Then
                safeLong = defaultValue
            End If
        End If
        Return safeLong
    End Function

    ''' <summary>
    ''' Return the "double" representation of a supplied value if it can be coerced to a double.
    ''' Return zero otherwise.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>Returns zero if the supplied value cannot be converted to a double.</returns>
    ''' <remarks></remarks>
    Public Shared Function getSafeNum(ByVal value As Object) As Double
        Dim safeNumber As Double
        If IsNothing(value) OrElse IsDBNull(value) Then
            safeNumber = 0
        Else
            safeNumber = CDbl(value)
        End If
        Return safeNumber
    End Function

#End Region

End Class

