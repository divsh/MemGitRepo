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
        ElseIf DirectCast(businessObject1, Object).GetType Is DirectCast(businessObject2, Object).GetType AndAlso _
               businessObject1.ID > 0 AndAlso businessObject2.ID > 0 AndAlso _
               businessObject1.ID = businessObject2.ID Then
            Return True
        End If

        Return False
    End Function
End Class
