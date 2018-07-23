Option Strict On
Option Explicit On

Public Interface ICache
#If CACHETRACE Then
    Inherits ICacheWithTracing
#End If
    Function Delete(ByVal businessObject As IBO) As Boolean
    '    Function Delete(ByVal businessObject As IBO, ByVal transactionToken As clsTransactionToken) As Boolean
    Function loadFromStorage(ByVal businessObject As IBO, Optional ByVal refresh As Boolean = False) As Boolean
    Function quickSave(ByVal businessObject As IBO) As Boolean
    '   Function quickSave(ByVal businessObject As IBO, ByVal transactionToken As clsTransactionToken) As Boolean
    Function Save(ByVal businessObject As IBO) As Boolean

    ' ''' <summary>
    ' ''' Save a business object to the database (can be new or pre-existing in data store).
    ' ''' </summary>
    ' ''' <typeparam name="T">The type of the business object (can be inferred by data store feedback concrete type).</typeparam>
    ' ''' <param name="businessObject">The business object to save.</param>
    ' ''' <param name="dataStoreFeedback">Data store feedback object used for interacting with data store e.g. transaction token.</param>
    ' ''' <returns>True if saved successfully. False if not.</returns>
    ' ''' <remarks></remarks>
    ''  Function Save(Of T As IBO)(ByVal businessObject As T, ByVal dataStoreFeedback As IDataStoreFeedback(Of T)) As Boolean

    Function makeBusObject(ByVal objectIdentifier As String, ByVal initialise As Boolean) As IBO
    Function MakeBusinessObject(Of T As {IBO})(ByVal initialise As Boolean) As T
    Function GetOne(ByVal className As String, ByVal id As Integer, Optional ByVal refresh As Boolean = False) As IBO
    Function GetBusinessObject(Of T As {IBO})(ByVal id As Integer, Optional ByVal refresh As Boolean = False) As T
    Function GetSomething(ByVal className As String, Optional ByVal refresh As Boolean = False) As IBOs
    Function GetSomething(ByVal className As String, ByVal whereClause As String) As IBOs
    Function getSomething(ByVal className As String, ByVal whereClause As String, ByVal orderBy As String) As IBOs
    'Function getSomething(ByVal className As String, ByVal whereClause As String, ByVal orderBy As String, ByVal transactionToken As clsTransactionToken) As IBOs
    Function GetSomethingUseArchiveFilters(ByVal className As String, ByVal whereClause As String) As IBOs
    Function GetBusinessCollection(Of T As {IBO})(Optional ByVal refresh As Boolean = False) As clsBusinessObjectCollection(Of T)
    Function GetBusinessCollection(Of T As {IBO})(ByVal whereClause As String, ByVal orderBy As String) As clsBusinessObjectCollection(Of T)
    'Function GetBusinessCollection(Of T As {IBO})(ByVal whereClause As String, ByVal orderBy As String, ByVal transactionToken As clsTransactionToken) As clsBusinessObjectCollection(Of T)
    Function GetBusinessCollection(Of T As {IBO})(ByVal whereClause As String) As clsBusinessObjectCollection(Of T)
    'Function GetBusinessCollection(Of T As {IBO})(ByVal whereClause As WhereClauseBuilder.clsBooleanExpression) As clsBusinessObjectCollection(Of T)
    'Function GetBusinessCollection(Of T As {IBO})(ByVal paramWhereClause As clsWhereClauseParam, ByVal orderBy As String) As clsBusinessObjectCollection(Of T)

    ' ''' <summary>
    ' ''' Get a business object collection.
    ' ''' </summary>
    ' ''' <typeparam name="T">The type of objects to retrieve.</typeparam>
    ' ''' <param name="whereClause">A where clause.</param>
    ' ''' <param name="dataStoreFeedback">Data store feedback object used for interacting with data store e.g. transaction token.</param>
    ' ''' <param name="serviceParameters">The class containing the delegate to populate the request</param>
    ' ''' <returns>The business object collection.</returns>
    ' ''' <remarks></remarks>
    'Function GetBusinessCollection(Of T As {IBO})(whereClause As WhereClauseBuilder.clsBooleanExpression,
    '                                              dataStoreFeedback As IDataStoreFeedback(Of T),
    '                                              serviceParameters As IServiceParameters) As clsBusinessObjectCollection(Of T)

    Function GetBusinessCollectionUseArchiveFilters(Of T As {IBO})(ByVal whereClause As String) As clsBusinessObjectCollection(Of T)
    Function GetBusinessObjects(Of T As {IBO})(Optional ByVal refresh As Boolean = False) As Generic.List(Of T)
    Function GetBusinessObjects(Of T As {IBO})(ByVal whereClause As String) As Generic.List(Of T)
    Function GetBusinessObjects(Of T As {IBO})(ByVal whereClause As String, ByVal orderBy As String) As Generic.List(Of T)
    '   Function GetBusinessObjects(Of T As {IBO})(ByVal whereClause As String, ByVal orderBy As String, ByVal transactionToken As clsTransactionToken) As Generic.List(Of T)
    'Function GetBusinessObjects(Of T As {IBO})(ByVal whereClause As WhereClauseBuilder.clsBooleanExpression) As Generic.List(Of T)

    ' ''' <summary>
    ' ''' Get a list of business objects.
    ' ''' </summary>
    ' ''' <typeparam name="T">The type of objects to retrieve.</typeparam>
    ' ''' <param name="whereClause">A where clause.</param>
    ' ''' <param name="dataStoreFeedback">Data store feedback object used for interacting with data store e.g. transaction token.</param>
    ' ''' <param name="serviceParameters">The class containing the delegate to populate the request</param>
    ' ''' <returns>The list of business objects.</returns>
    ' ''' <remarks></remarks>
    'Function GetBusinessObjects(Of T As {IBO})(whereClause As WhereClauseBuilder.clsBooleanExpression,
    '                                           dataStoreFeedback As IDataStoreFeedback(Of T),
    '                                           serviceParameters As IServiceParameters) As Generic.List(Of T)

    Function GetBusinessObjectsUseArchiveFilters(Of T As {IBO})(ByVal whereClause As String) As Generic.List(Of T)
    Function GetRelated(ByVal parentBusinessObject As IBO, ByVal className As String, ByVal associationName As String, ByVal refresh As Boolean) As IBOs
    '    Function GetRelated(ByVal parentBusinessObject As IBO, ByVal className As String, ByVal associationName As String, ByVal refresh As Boolean, ByVal transactionToken As clsTransactionToken) As IBOs
    Function GetRelatedBusinessCollection(Of TParent As {IBO}, TChild As {IBO})(ByVal parentBusinessObject As TParent, ByVal associationName As String, ByVal refresh As Boolean) As clsBusinessObjectCollection(Of TChild)

    ' ''' <summary>
    ' ''' Get a child collection (the many side of a one-to-many association)
    ' ''' </summary>
    ' ''' <typeparam name="TParent">The type of the parent object.</typeparam>
    ' ''' <typeparam name="TChild">The type of the children.</typeparam>
    ' ''' <param name="parentBusinessObject">The parent object.</param>
    ' ''' <param name="associationName">The name of the association (from the parent object to the children).</param>
    ' ''' <param name="refresh"></param>
    ' ''' <param name="dataStoreFeedback">Data store feedback object used for interacting with data store e.g. transaction token.</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Function GetRelatedBusinessCollection(Of TParent As {IBO}, TChild As {IBO})(ByVal parentBusinessObject As TParent,
    '                                                                            ByVal associationName As String,
    '                                                                            ByVal refresh As Boolean,
    '                                                                            ByVal dataStoreFeedback As IDataStoreFeedback(Of TChild)) As clsBusinessObjectCollection(Of TChild)

    Function findMatch(ByVal classname As String, ByVal columnName As String, ByVal value As Object) As IBOs
    ' ''' <summary>
    ' ''' Get a list of business objects that match a column's value
    ' ''' </summary>
    ' ''' <param name="classname">The class name</param>
    ' ''' <param name="columnName">The search column</param>
    ' ''' <param name="value">The match value</param>
    ' ''' <param name="transactionToken">The database transaction token</param>
    ' ''' <returns>List of business objects</returns>
    ' ''' <remarks></remarks>
    'Function findMatch(ByVal classname As String, ByVal columnName As String, ByVal value As Object, ByVal transactionToken As clsTransactionToken) As IBOs
    Function findMatch(Of T As {IBO})(ByVal columnName As String, ByVal value As Object) As clsBusinessObjectCollection(Of T)
    Function copyBusObject(Of T As {IBO})(ByVal sourceObject As T) As T
    Sub copyBusObject(Of T As {IBO})(ByVal sourceObject As T, ByVal destinationObject As T)
    Function copyBusinessObject(ByVal sourceObject As IBO, Optional ByVal DestinationObject As IBO = Nothing) As IBO
    'Function beginTransaction() As clsTransactionToken
    'Function beginNewTransaction() As clsTransactionToken
    'Function GetBusinessObject(Of T As {IBO})(ByVal key As clsKey, Optional ByVal refresh As Boolean = False) As T
    'Function GetBusinessObject(Of T As {IBO})(ByVal key As clsKey, ByVal serviceParameters As IServiceParameters, Optional ByVal refresh As Boolean = False) As T
    'Function GetOne(ByVal className As String, ByVal key As clsKey, Optional ByVal refresh As Boolean = False) As IBO
    Function GetRelatedBusinessObject(Of TFarClass As {IBO})(ByVal nearObject As IBO, ByVal associationName As String, ByVal refresh As Boolean) As TFarClass

    ' ''' <summary>
    ' ''' Get a related business object.
    ' ''' </summary>
    ' ''' <typeparam name="TFarClass">The far class' type.</typeparam>
    ' ''' <param name="nearObject">The near object.</param>
    ' ''' <param name="associationName">The name of the association (from the near object to the far object).</param>
    ' ''' <param name="refresh"></param>
    ' ''' <param name="dataStoreFeedback">Data store feedback object used for interacting with data store e.g. transaction token.</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ''    Function GetRelatedBusinessObject(Of TFarClass As {IBO})(ByVal nearObject As IBO, ByVal associationName As String, ByVal refresh As Boolean, ByVal dataStoreFeedback As IDataStoreFeedback(Of TFarClass)) As TFarClass

    Function GetRelatedBusinessObject(ByVal nearObject As IBO, ByVal associationName As String, ByVal refresh As Boolean) As IBO
End Interface

''' <summary>
''' Generic interface that enables consumers to view the actions available through the cache for objects that support cache actions. 
''' </summary>
''' <typeparam name="TActions"></typeparam>
''' <remarks></remarks>
Public Interface ICache(Of TActions)
    Inherits ICache

    ''' <summary>
    ''' The actions that can be performed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Actions As TActions
End Interface