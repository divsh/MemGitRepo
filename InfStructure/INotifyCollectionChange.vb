Option Strict On
Option Explicit On

''' <summary>
''' Provides notification of changes to a collection object.  This is an implementation of the interface provided
''' in .NET 3.0. If using .NET 3.0 library, then replace this with the system library.
''' </summary>
''' <remarks>Converted to vb.net from c# implementation, 
''' see http://www.koders.com/csharp/fidE757D80B5D726238F1CE45CA2E6D61389BCD64A3.aspx?s=delegate#L6
''' </remarks>
''' 

Public Delegate Sub NotifyCollectionChangedEventHandler(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)

''' <summary>
''' Collection change notification interface
''' </summary>
''' <remarks></remarks>
Public Interface INotifyCollectionChanged
    'Events
    Event CollectionChanged As NotifyCollectionChangedEventHandler
End Interface

''' <summary>
''' The type of the change
''' </summary>
''' <remarks></remarks>
Public Enum NotifyCollectionChangedAction
    Add
    Remove
    Replace
    Move
    Reset
End Enum

''' <summary>
''' Notification change event args.
''' </summary>
''' <remarks></remarks>
Public Class NotifyCollectionChangedEventArgs
    Inherits EventArgs

    'Fields
    ''' <summary>
    ''' The action that is being performed on the collection e.g. Remove or Reset
    ''' </summary>
    ''' <remarks></remarks>
    Private mAction As NotifyCollectionChangedAction
    ''' <summary>
    ''' The new items for the collection
    ''' </summary>
    ''' <remarks></remarks>
    Private mNewItems As IList
    ''' <summary>
    ''' Starting index for new items the collection
    ''' </summary>
    ''' <remarks></remarks>
    Private mNewStartingIndex As Integer
    ''' <summary>
    ''' Old items that were in the collection
    ''' </summary>
    ''' <remarks></remarks>
    Private mOldItems As IList
    ''' <summary>
    ''' Starting index for old items
    ''' </summary>
    ''' <remarks></remarks>
    Private mOldStartingIndex As Integer

    'Methods
    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItems As IList)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (((action <> NotifyCollectionChangedAction.Add) AndAlso (action <> NotifyCollectionChangedAction.Remove)) AndAlso (action <> NotifyCollectionChangedAction.Reset)) Then
            Throw New ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action")
        End If
        If (changedItems Is Nothing) Then
            Throw New ArgumentNullException("changedItems")
        End If
        '188819 P6 - InitializeAddOrRemove now accepts resets
        Me.InitializeAddOrRemove(action, changedItems, -1)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItem As Object)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (((action <> NotifyCollectionChangedAction.Add) AndAlso (action <> NotifyCollectionChangedAction.Remove)) AndAlso (action <> NotifyCollectionChangedAction.Reset)) Then
            Throw New ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action")
        End If
        '188819 P6 - InitializeAddOrRemove now accepts resets
        Me.InitializeAddOrRemove(action, New Object() {changedItem}, -1)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal newItems As IList, ByVal oldItems As IList)
        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Replace) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        If (newItems Is Nothing) Then
            Throw New ArgumentNullException("newItems")
        End If
        If (oldItems Is Nothing) Then
            Throw New ArgumentNullException("oldItems")
        End If
        Me.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItems As IList, ByVal startingIndex As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (((action <> NotifyCollectionChangedAction.Add) AndAlso (action <> NotifyCollectionChangedAction.Remove)) AndAlso (action <> NotifyCollectionChangedAction.Reset)) Then
            Throw New ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action")
        End If
        If (changedItems Is Nothing) Then
            Throw New ArgumentNullException("changedItems")
        End If
        If (startingIndex < -1) Then
            Throw New ArgumentException("IndexCannotBeNegative", "startingIndex")
        End If
        '188819 P6 - InitializeAddOrRemove now accepts resets
        Me.InitializeAddOrRemove(action, changedItems, startingIndex)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItem As Object, ByVal index As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (((action <> NotifyCollectionChangedAction.Add) AndAlso (action <> NotifyCollectionChangedAction.Remove)) AndAlso (action <> NotifyCollectionChangedAction.Reset)) Then

            Throw New ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action")
        End If
        '188819 P6 - InitializeAddOrRemove now accepts resets
        Me.InitializeAddOrRemove(action, New Object() {changedItem}, index)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal newItem As Object, ByVal oldItem As Object)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Replace) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        Me.InitializeMoveOrReplace(action, New Object() {newItem}, New Object() {oldItem}, -1, -1)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal newItems As IList, ByVal oldItems As IList, ByVal startingIndex As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Replace) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        If (newItems Is Nothing) Then
            Throw New ArgumentNullException("newItems")
        End If
        If (oldItems Is Nothing) Then
            Throw New ArgumentNullException("oldItems")
        End If
        Me.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItems As IList, ByVal index As Integer, ByVal oldIndex As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Move) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        If (index < 0) Then
            Throw New ArgumentException("IndexCannotBeNegative", "index")
        End If
        Me.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal changedItem As Object, ByVal index As Integer, ByVal oldIndex As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Move) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        If (index < 0) Then
            Throw New ArgumentException("IndexCannotBeNegative", "index")
        End If
        Dim newItems As Object() = New Object() {changedItem}
        Me.InitializeMoveOrReplace(action, newItems, newItems, index, oldIndex)
    End Sub

    Public Sub New(ByVal action As NotifyCollectionChangedAction, ByVal newItem As Object, ByVal oldItem As Object, ByVal index As Integer)

        Me.mNewStartingIndex = -1
        Me.mOldStartingIndex = -1
        If (action <> NotifyCollectionChangedAction.Replace) Then
            Throw New ArgumentException("WrongActionForCtor", "action")
        End If
        Me.InitializeMoveOrReplace(action, New Object() {newItem}, New Object() {oldItem}, index, index)
    End Sub

    ''' <summary>
    ''' Sets newly added items to the new items list, sets the starting idex for new items and also set's the action
    ''' </summary>
    ''' <param name="action"></param>
    ''' <param name="newItems"></param>
    ''' <param name="newStartingIndex"></param>
    ''' <remarks></remarks>
    Private Sub InitializeAdd(ByVal action As NotifyCollectionChangedAction, ByVal newItems As IList, ByVal newStartingIndex As Integer)
        Me.mAction = action
        If newItems Is Nothing Then
            Me.mNewItems = Nothing
        Else
            Me.mNewItems = ArrayList.ReadOnly(newItems)
        End If
        Me.mNewStartingIndex = newStartingIndex
    End Sub

    ''' <summary>
    ''' Finds out what the action being performed is and sets the new and old item list depending on what action is being performed.
    ''' e.g. sets old item list when the action being performed on the collection is a Reset.
    ''' </summary>
    ''' <param name="action"></param>
    ''' <param name="changedItems"></param>
    ''' <param name="startingIndex"></param>
    ''' <remarks></remarks>
    Private Sub InitializeAddOrRemove(ByVal action As NotifyCollectionChangedAction, ByVal changedItems As IList, ByVal startingIndex As Integer)
        If (action = NotifyCollectionChangedAction.Add) Then
            Me.InitializeAdd(action, changedItems, startingIndex)
        ElseIf (action = NotifyCollectionChangedAction.Remove) Then
            Me.InitializeRemove(action, changedItems, startingIndex)
        ElseIf (action = NotifyCollectionChangedAction.Reset) Then
            '188819 P6 - InitializeAddOrRemove now accepts resets
            Me.InitializeRemove(action, changedItems, startingIndex)
        Else
            Throw New ArgumentException(String.Format("InvariantFailure, Unsupported action: {0}", action.ToString()))
        End If
    End Sub

    ''' <summary>
    ''' Sets the newly added removed items that were moved or replaced to the new and old items list.  Also sets the action.
    ''' </summary>
    ''' <param name="action"></param>
    ''' <param name="newItems"></param>
    ''' <param name="oldItems"></param>
    ''' <param name="startingIndex"></param>
    ''' <param name="oldStartingIndex"></param>
    ''' <remarks></remarks>
    Private Sub InitializeMoveOrReplace(ByVal action As NotifyCollectionChangedAction, ByVal newItems As IList, ByVal oldItems As IList, ByVal startingIndex As Integer, ByVal oldStartingIndex As Integer)
        Me.InitializeAdd(action, newItems, startingIndex)
        Me.InitializeRemove(action, oldItems, oldStartingIndex)
    End Sub

    ''' <summary>
    ''' Sets removed items to the old items list, sets the starting idex for old items and also set's the action
    ''' </summary>
    ''' <param name="action"></param>
    ''' <param name="oldItems"></param>
    ''' <param name="oldStartingIndex"></param>
    ''' <remarks></remarks>
    Private Sub InitializeRemove(ByVal action As NotifyCollectionChangedAction, ByVal oldItems As IList, ByVal oldStartingIndex As Integer)
        Me.mAction = action
        If oldItems Is Nothing Then
            Me.mOldItems = Nothing
        Else
            Me.mOldItems = ArrayList.ReadOnly(oldItems)
        End If
        Me.mOldStartingIndex = oldStartingIndex
    End Sub

    'Properties
    ''' <summary>
    ''' The action that is being performed on the collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property action() As NotifyCollectionChangedAction
        Get
            Return Me.mAction
        End Get
    End Property

    ''' <summary>
    ''' The new items for the collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NewItems() As IList
        Get
            Return Me.mNewItems
        End Get
    End Property

    ''' <summary>
    ''' The starting index for new items for the collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NewStartingIndex() As Integer
        Get
            Return Me.mNewStartingIndex
        End Get
    End Property

    ''' <summary>
    ''' The old items before the collection changed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OldItems() As IList
        Get
            Return Me.mOldItems
        End Get
    End Property

    ''' <summary>
    ''' The starting index for old items that were in the collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OldStartingIndex() As Integer
        Get
            Return Me.mOldStartingIndex
        End Get
    End Property

End Class




