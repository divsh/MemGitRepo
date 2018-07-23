Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Linq

Public Class clsBusinessObjectCollection(Of T As {IBO})
    Implements IBOs, Generic.IEnumerable(Of T), INotifyCollectionChanged, Generic.ICollection(Of T), Generic.IList(Of T)

    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Protected mList As New Generic.List(Of T)
    Protected mSortField As String 'the field the col is sorted on
    Protected mComparer As Generic.IComparer(Of T)
    Protected mBusinessClassName As String
    Protected mRaiseChangeNotification As Boolean = False
    Protected mMonitor As SimpleMonitor
    Protected mSupportsMarkedAsDeleted As Nullable(Of Boolean)
    'Indicate if an insert command is in effect


    Public ReadOnly Property itemZerobase(ByVal index As Integer) As T
        Get
            Return GetLiveObjects().ElementAt(index)
        End Get
    End Property

    Public ReadOnly Property item(ByVal index As Integer) As T
        Get
            Return GetLiveObjects().ElementAt(index - 1)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer Implements IBOs.count, Generic.ICollection(Of T).Count
        Get
            Return GetLiveObjects().Count()
        End Get
    End Property

    'Public Property sortField() As String
    '    Get
    '        Return mSortField
    '    End Get
    '    Set(ByVal Value As String)
    '        mSortField = Value
    '        mComparer = clsBusinessObjectUtilities.GetFieldComparer(Of T)(Value)
    '    End Set
    'End Property

    ''' <summary>
    ''' Does this collection support marking objects as deleted? True if it is typed with a class that implements IChildComponent.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable ReadOnly Property SupportsMarkedAsDeleted As Boolean
        Get
            If Not mSupportsMarkedAsDeleted.HasValue Then
                mSupportsMarkedAsDeleted = GetType(IChildComponent).IsAssignableFrom(GetType(T))
            End If

            Return mSupportsMarkedAsDeleted.Value
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of T) Implements System.Collections.Generic.IEnumerable(Of T).GetEnumerator
        Return GetLiveObjects().GetEnumerator()
    End Function

    ''' <summary>
    ''' Get all of the live objects in the underlying list.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function GetLiveObjects() As Generic.IEnumerable(Of T)
        If SupportsMarkedAsDeleted Then
            Return mList.Where(Function(b) Not DirectCast(b, IChildComponent).isToBeDeleted)
        Else
            Return mList
        End If
    End Function

    ''' <summary>
    ''' Get all of the objects that are marked for deletion in the underlying list.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetObjectsToBeDeleted() As Generic.IEnumerable(Of T)
        If SupportsMarkedAsDeleted Then
            Return mList.Where(Function(b) DirectCast(b, IChildComponent).isToBeDeleted)
        End If

        Return Enumerable.Empty(Of T)()
    End Function

    Protected Overridable Function IEnumerable_GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

    ''' <summary>
    ''' Add to collection
    ''' </summary>
    ''' <param name="businessObject"></param>
    ''' <remarks></remarks>
    Public Overridable Sub add(ByVal businessObject As T) Implements Generic.ICollection(Of T).Add
        internalAdd(businessObject)
    End Sub

    ''' <summary>
    ''' Add the elements of the specified collection to the end of the collection
    ''' </summary>
    ''' <param name="businessObjects"></param>
    ''' <remarks>SCM 206397 P5</remarks>
    Public Sub addRange(ByVal businessObjects As Generic.IEnumerable(Of T))
        For Each item As T In businessObjects
            add(item)
        Next
    End Sub

    Friend Overridable Sub internalAdd(ByVal businessObject As T)
        If mComparer Is Nothing Then
            Me.CheckReentrancy()
            mList.Add(businessObject)
            If mRaiseChangeNotification Then
                Me.OnCollectionChanged(NotifyCollectionChangedAction.Add, businessObject, GetLiveObjects().Count - 1)
            End If
        Else
            Dim index As Integer = mList.BinarySearch(businessObject, mComparer)

            If index < 0 Then
                index = index Xor -1
            Else
                ' SCM 72004 P1 R8  - Colours grid behaves abnormally
                ' The procedure below looks for the LAST "found" index and inserts the new item
                ' after last index found
                While index < mList.Count AndAlso mComparer.Compare(businessObject, mList.Item(index)) = 0
                    index += 1
                End While
            End If
            Me.CheckReentrancy()
            mList.Insert(index, businessObject)
            If mRaiseChangeNotification Then
                Me.OnCollectionChanged(NotifyCollectionChangedAction.Add, businessObject, GetLiveObjects().ToList().IndexOf(businessObject) + 1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Insert at index.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <param name="businessObject"></param>
    ''' <remarks></remarks>
    Public Overridable Sub insert(ByVal index As Integer, ByVal businessObject As T)
        internalInsert(index, businessObject)
    End Sub

    Friend Overridable Sub internalInsert(ByVal index As Integer, ByVal businessObject As T)
        Me.CheckReentrancy()
        Dim fullListInsertionIndex As Integer = 0

        If index > 1 Then
            Dim currentObjectAtInsertionIndex As T = GetLiveObjects().ElementAt(index - 1)
            fullListInsertionIndex = mList.IndexOf(currentObjectAtInsertionIndex)
        End If

        mList.Insert(fullListInsertionIndex, businessObject)
        If mRaiseChangeNotification Then
            Me.OnCollectionChanged(NotifyCollectionChangedAction.Add, businessObject, index)
        End If
    End Sub

    ''' <summary>
    ''' Remove from collection.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Overridable Sub remove(ByVal index As Integer) Implements IBOs.remove
        internalRemove(index)
    End Sub

    ''' <summary>
    ''' Removes the business object if it can be found in the underlying collection
    ''' </summary>
    ''' <param name="businessObject">Object to delete</param>
    ''' <remarks>This method may not work if the object in the parameter belongs to a different cache.</remarks>
    Public Sub remove(ByVal businessObject As T)
        remove(mList.IndexOf(DirectCast(businessObject, T)) + 1)
    End Sub

    ''' <summary>
    ''' Removes all the elements that match the conditions defined by the specified predicate.
    ''' </summary>
    ''' <param name="match">The System.Predicate(Of T) delegate that defines the conditions of the elements to remove.</param>
    ''' <returns>The number of elements removed from the underlying List</returns>
    ''' <remarks>Example of a predicate:
    ''' removeAll(Function(x) x.IBO_id = someID) - removes all object with IBO_id = someID
    ''' </remarks>
    Public Function removeAll(match As System.Predicate(Of T)) As Integer
        Return mList.RemoveAll(match)
    End Function

    Friend Overridable Sub internalRemove(ByVal businessObject As IBO)
        internalRemove(mList.IndexOf(DirectCast(businessObject, T)) + 1)
    End Sub

    Protected Sub internalRemove(ByVal index As Integer)
        Me.CheckReentrancy()

        Dim objectAtRemovalIndex As T = GetLiveObjects().ElementAt(index - 1)
        mList.Remove(objectAtRemovalIndex)
        If mRaiseChangeNotification Then
            Me.OnCollectionChanged(NotifyCollectionChangedAction.Remove, objectAtRemovalIndex, index)
        End If
    End Sub

    ''' <summary>
    ''' Clear all items out of collection.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub Clear() Implements Generic.ICollection(Of T).Clear
        internalClear()
    End Sub

    ''' <summary>
    ''' Clears the business object collection and notifies that the collection has changed
    ''' </summary>
    ''' <remarks>188819 P6 - Now passes the old items before clearing to the collection changed notification</remarks>
    Friend Sub internalClear()
        Dim items As New Generic.List(Of T)(mList)
        Me.CheckReentrancy()
        mList.Clear()
        If mRaiseChangeNotification Then
            Me.OnCollectionReset(items)
        End If
    End Sub

    Public ReadOnly Property GenericList() As Generic.List(Of T)
        Get
            Return mList
        End Get
    End Property

    Private Sub add(ByVal businessObject As IBO) Implements IBOs.add
        add(DirectCast(businessObject, T))
    End Sub

    ''' <summary>
    ''' Handles the inserting of the business object in the collection
    ''' </summary>
    ''' <param name="index"></param>
    ''' <param name="businessObject"></param>
    ''' <remarks></remarks>
    Private Sub insert(ByVal index As Integer, ByVal businessObject As IBO) Implements IBOs.insert
        insert(index, DirectCast(businessObject, T))
    End Sub

    Private ReadOnly Property IBOs_Item(ByVal index As Integer) As IBO Implements IBOs.item
        Get
            Return item(index)
        End Get
    End Property

    Private ReadOnly Property IBOs_ItemZeroBase(ByVal index As Integer) As IBO Implements IBOs.itemZeroBase
        Get
            Return itemZerobase(index)
        End Get
    End Property

    Public ReadOnly Property IBOs_BusinessClassName() As String Implements IBOs.BusinessClassName
        Get
            Return mBusinessClassName
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        'Dim info As clsBusinessClassAttribute = getBusinessClassAttribute()

        'If info IsNot Nothing Then
        '    mBusinessClassName = info.ClassName
        mBusinessClassName = GetType(T).Name.Substring(3)
        'End If

        mMonitor = New SimpleMonitor()
    End Sub

    Public Sub New(ByVal _list As Generic.List(Of T))
        Me.New()
        mList = _list
    End Sub

    ' ''' <summary>
    ' ''' Get the business class attribute for the type of objects that this business object collection will hold.
    ' ''' </summary>
    ' ''' <returns>The business class attribute if found. Nothing if not.</returns>
    ' ''' <remarks>Does not throw.</remarks>
    'Protected Function getBusinessClassAttribute() As clsBusinessClassAttribute
    '    Try
    '        If GetType(T).IsAbstract Then
    '            'If this collection is for an abstract type, e.g. an interface, then we do not know the concrete type to retrieve the business class attribute from. Return nothing.
    '            Return Nothing
    '        ElseIf gInfController IsNot Nothing Then
    '            'If we have an inf controller then try getting the business class attribute from there.
    '            Return gInfController.getBusinessClassAttribute(GetType(T))
    '        Else
    '            'Otherwise try getting the business class attribute directly from the type using reflection.
    '            Return DirectCast(Attribute.GetCustomAttribute(GetType(T), GetType(clsBusinessClassAttribute)), clsBusinessClassAttribute)
    '        End If
    '    Catch ex As Exception
    '        Logger.Log(ex, "Failed to retrieve a business class attribute for type {0}.", GetType(T).Name)
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' Return string containing comma delimited descriptions of objects in collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ToStringList() As String Implements IBOs.ToStringList
        Get

            Dim result As String = String.Empty
            Dim delimiter As String = ","
            Dim liveObjects As Generic.IEnumerable(Of T) = GetLiveObjects()
            If IsNothing(liveObjects) OrElse liveObjects.Count = 0 Then
                Return result
            End If

            For Each item As IBO In liveObjects
                result += delimiter & item.description
            Next

            Return result.Substring(1)
        End Get
    End Property

    ''' <summary>
    ''' Reverses the order of the items in the list.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub reverse()
        mList.Reverse()
    End Sub

    ''' <summary>
    ''' Gets or sets whether change notifications will be raised. Default is false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property raiseChangeNotification() As Boolean
        Get
            Return mRaiseChangeNotification
        End Get
        Set(ByVal value As Boolean)
            mRaiseChangeNotification = value
        End Set
    End Property
    ''' <summary>
    ''' Test if this collection contains the same IBO but not necessary to same instance of ibo object
    ''' </summary>
    ''' <param name="businessObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function contains(ByVal businessObject As IBO) As Boolean Implements IBOs.contains
        If businessObject IsNot Nothing Then
            For Each item As IBO In GetLiveObjects()
                If clsBusinessObjectUtilities.areBusinessObjectsEqual(item, businessObject) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    ''Sorts IBOs by fieldname.
    ''Choose to sort ascending/descending.
    ''Datatype of field determines how it will sort, if numeric is TRUE then forces numeric
    ''sorting.
    ''Now can be sorted cumulatively i.e. an IBOs can be sorted multiple times while
    ''retaining the order of previous sorts E.g. sort by invoice number then company name will
    ''give you invoice numbers sorted within company and companies sorted in ascending order
    'Public Sub sort(ByVal fieldName As String, Optional ByVal ascending As Boolean = True, Optional ByVal numeric As Boolean = False) Implements IBOs.sort
    '    Dim ix As Integer
    '    Dim working As New List(Of T)
    '    Dim thisIBO As T
    '    Dim midIX As Integer
    '    Dim minIX As Integer
    '    Dim maxIX As Integer
    '    Dim unsortedValue As Object

    '    'Sort IBOs to working collection
    '    For ix = 1 To mList.Count
    '        thisIBO = mList.Item(ix - 1)
    '        'Get value to be sorted
    '        If numeric Then
    '            unsortedValue = Val(thisIBO.field(fieldName))
    '        Else
    '            unsortedValue = thisIBO.field(fieldName)
    '        End If
    '        'Initialise variables
    '        minIX = 1
    '        maxIX = working.Count()
    '        'Determine where to insert thisIBO in the sorted working collection
    '        'To do this we use a binary search method

    '        Do While maxIX >= minIX
    '            midIX = CInt((maxIX - minIX) / 2 + minIX)
    '            If numeric Or IsNumeric(unsortedValue) Then 'sort numbers
    '                Dim numSorted As Double = Val(working.Item(midIX - 1).field(fieldName))
    '                Dim numUnsorted As Double = Val(unsortedValue)

    '                If (ascending And (numUnsorted >= numSorted)) Or ((Not ascending) And (numUnsorted <= numSorted)) Then
    '                    minIX = midIX + 1
    '                ElseIf (ascending And (numUnsorted < numSorted)) Or ((Not ascending) And (numUnsorted > numSorted)) Then
    '                    maxIX = midIX - 1
    '                End If

    '            ElseIf TypeOf unsortedValue Is Date Then 'sort dates
    '                Dim dateSorted As Date = CDate(working.Item(midIX - 1).field(fieldName))
    '                Dim dateUnsorted As Date = CDate(unsortedValue)

    '                If (ascending And (dateUnsorted >= dateSorted)) Or ((Not ascending) And (dateUnsorted <= dateSorted)) Then
    '                    minIX = midIX + 1
    '                ElseIf (ascending And (dateUnsorted < dateSorted)) Or ((Not ascending) And (dateUnsorted > dateSorted)) Then
    '                    maxIX = midIX - 1
    '                End If

    '            Else 'sort strings
    '                Dim strSorted As String = CStr(working.Item(midIX - 1).field(fieldName))
    '                Dim strUnsorted As String = CStr(unsortedValue)
    '                If String.IsNullOrEmpty(strSorted) Then strSorted = ""
    '                If String.IsNullOrEmpty(strUnsorted) Then strSorted = ""

    '                If (ascending And (strUnsorted >= strSorted)) Or ((Not ascending) And (strUnsorted <= strSorted)) Then
    '                    minIX = midIX + 1
    '                ElseIf (ascending And (strUnsorted < strSorted)) Or ((Not ascending) And (strUnsorted > strSorted)) Then
    '                    maxIX = midIX - 1
    '                End If
    '            End If

    '        Loop
    '        'Found position now insert
    '        If minIX > working.Count() Then
    '            'insert after
    '            working.Add(thisIBO)
    '        Else
    '            'insert before
    '            working.Insert(minIX - 1, thisIBO)
    '        End If
    '    Next
    '    'Remove existing items from IBOs
    '    mList.Clear()

    '    'Add items to empty IBOs
    '    'With comparer considered if exists - SCM168422
    '    'Calling internaladd would make the individual items dirty 
    '    For ix = 1 To working.Count()
    '        Dim businessObject As T = working.Item(ix - 1)

    '        If mComparer Is Nothing Then
    '            mList.Add(businessObject)
    '        Else
    '            Dim index As Integer = mList.BinarySearch(working.Item(ix - 1), mComparer)

    '            If index < 0 Then
    '                index = index Xor -1
    '            Else
    '                While index < mList.Count AndAlso mComparer.Compare(businessObject, mList.Item(index)) = 0
    '                    index += 1
    '                End While
    '            End If
    '            mList.Insert(index, businessObject)
    '        End If
    '    Next
    'End Sub

#Region "IList"
    ''' <summary>
    ''' Insert into collection.
    ''' </summary>
    ''' <param name="indexZeroBase">Zero-based index.</param>
    ''' <param name="item">The item to insert.</param>
    ''' <remarks></remarks>
    Private Sub IList_insert(indexZeroBase As Integer, item As T) Implements Generic.IList(Of T).Insert
        insert(indexZeroBase + 1, item)
    End Sub

    ''' <summary>
    ''' Copy all items in collection to array.
    ''' </summary>
    ''' <param name="array">The array to copy items to.</param>
    ''' <param name="index">The starting index for copying items into.</param>
    ''' <remarks></remarks>
    Private Sub IList_CopyTo(array() As T, index As Integer) Implements Generic.IList(Of T).CopyTo
        mList.CopyTo(array, index)
    End Sub

    ''' <summary>
    ''' Remove item at index.
    ''' </summary>
    ''' <param name="indexZeroBase">Zero-based index.</param>
    ''' <remarks></remarks>
    Private Sub IList_RemoveAt(indexZeroBase As Integer) Implements Generic.IList(Of T).RemoveAt
        internalRemove(indexZeroBase + 1)
    End Sub

    ''' <summary>
    ''' Gets/sets an item at index.
    ''' </summary>
    ''' <param name="indexZeroBase">Zero-based index.</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property IList_Item(indexZeroBase As Integer) As T Implements Generic.IList(Of T).Item
        Get
            Return itemZerobase(indexZeroBase)
        End Get
        Set(value As T)
            remove(indexZeroBase + 1)
            insert(indexZeroBase + 1, value)
        End Set
    End Property

    ''' <summary>
    ''' Gets the zero-based index of a particular item.
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IList_IndexOf(item As T) As Integer Implements Generic.IList(Of T).IndexOf
        Return mList.IndexOf(item)
    End Function
#End Region

#Region "ICollection"
    ''' <summary>
    ''' Checks if the collection contains the item. 
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ICollection_Contains(item As T) As Boolean Implements Generic.ICollection(Of T).Contains
        contains(item)
    End Function

    ''' <summary>
    ''' Checks if the collection is read-only. Always returns false as read-only clsBusinessObjectCollection has not been implemented.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property ICollection_IsReadOnly As Boolean Implements Generic.ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Removes an item from the collection.
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns>True if removed, false if not.</returns>
    ''' <remarks></remarks>
    Private Function ICollection_Remove(item As T) As Boolean Implements Generic.ICollection(Of T).Remove
        Try
            remove(item)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "INotifyCollectionChanged"

    Protected Function BlockReentrancy() As IDisposable
        Me.mMonitor.Enter()
        Return Me.mMonitor
    End Function

    Protected Sub CheckReentrancy()
        If (Me.mMonitor.Busy) Then
            Throw New InvalidOperationException("ObservableCollectionReentrancyNotAllowed")
        End If
    End Sub

    Protected Overridable Sub OnCollectionChanged(ByVal e As NotifyCollectionChangedEventArgs)
        Using (Me.BlockReentrancy())
            RaiseEvent CollectionChanged(Me, e)
        End Using
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal item As Object, ByVal index As Integer)
        Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, item, index))
    End Sub

    Private Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal item As Object, ByVal index As Integer, ByVal oldIndex As Integer)
        Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, item, index, oldIndex))
    End Sub

    Private Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal oldItem As Object, ByVal newItem As Object, ByVal index As Integer)
        Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, newItem, oldItem, index))
    End Sub

    ''' <summary>
    ''' Called when a collection has been reset(e.g. cleared), prepares the notifition event args
    ''' </summary>
    ''' <param name="oldItems"></param>
    ''' <remarks>188819 P6 - Now passes the old items before clearing to the collection changed notification</remarks>
    Private Sub OnCollectionReset(ByVal oldItems As IList)
        Me.OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, oldItems))
    End Sub

    'Private Property IBOs_sortField() As String Implements IBOs.sortField
    '    Get
    '        Return sortField()
    '    End Get
    '    Set(ByVal value As String)
    '        sortField = value
    '    End Set
    'End Property
    'Nested Types
    Protected Class SimpleMonitor
        Implements IDisposable
        'Fields
        Private mBusyCount As Integer

        'Methods
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.mBusyCount -= 1
        End Sub

        Public Sub Enter()
            Me.mBusyCount += 1
        End Sub

        'Properties
        Public ReadOnly Property Busy() As Boolean
            Get
                Return (Me.mBusyCount > 0)
            End Get
        End Property

    End Class
#End Region

End Class
