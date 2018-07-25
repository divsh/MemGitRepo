Option Strict On
Option Explicit On
Imports InfServer
Imports KiwiUtil
'*** Start Custom Code imports
'*** End Custom Code

<clsBusinessClass("Alt", "Question")> _
Public Class clsQuestion
    Implements IBO
    Implements IESPBO
'*** Start Custom Code class_definition
'*** End Custom Code

    'Class: AltServer.clsQuestion 
    ' Generated Class

    Public Event changeDirty() Implements IBO.changeDirty
    Public Event change(ByVal source As Object, ByVal e As System.EventArgs) Implements IBO.change

#Region "Columns for WhereClauses"
    Public Class ColumnList
        Public Shared ReadOnly ID As New WhereClauseBuilder.clsIntegerColumn("ID", 0)
        Public Shared ReadOnly Mainttime As New WhereClauseBuilder.clsDateTimeColumn("Mainttime", 1)
        Public Shared ReadOnly UserID As New WhereClauseBuilder.clsStringColumn("UserID", 2)
        Public Shared ReadOnly VersionTimeStamp As New WhereClauseBuilder.clsTimeStampColumn("VersionTimeStamp", 3)
        Public Shared ReadOnly Name As New WhereClauseBuilder.clsStringColumn("Name", 4)
        Public Shared ReadOnly AnswerRTF As New WhereClauseBuilder.clsStringColumn("AnswerRTF", 5)
        Public Shared ReadOnly AnswerText As New WhereClauseBuilder.clsStringColumn("AnswerText", 6)
        Public Shared ReadOnly MemorizedIndex As New WhereClauseBuilder.clsStringColumn("MemorizedIndex", 7)
        Public Shared ReadOnly DeferredTillDate As New WhereClauseBuilder.clsDateTimeColumn("DeferredTillDate", 8)
        Public Shared ReadOnly TopicID As New WhereClauseBuilder.clsIntegerColumn("TopicID", 9)
    End Class
#End Region

    Private Const FIELD_COUNT As Integer = 10

    Private mAppEnvironment As clsAppEnvironment
    Private mMetaData As clsDDController
    Private mClass As clsBusinessClass
    Private mdirty As Boolean
    Private mReadOnly As Boolean
    Private mIsStored As Boolean
    Private mInitialised As Boolean
    Private mParent As IBO
    Private mActions As clsBusinessObjectCollection(Of clsActivity)
    Private mValues As clsESPObjectData
    Private mCache As ICache
    Private mChangeStack As Integer
    Private mTag As Object
    Private mIBOKey As clsKey
    ''' <summary>
    ''' Allows the business object to define service parameters for an external service
    ''' </summary>
    ''' <remarks></remarks>
    Private mServiceParameters As IServiceParameters
    Private mClonedQuestion As clsQuestion

#If CACHETRACE Then
    Private ReadOnly mCreationTrace As StackTrace
#End If

#Region "Custom Declarations"
'*** Start Custom Code custom_declarations
'*** End Custom Code
#End Region

#Region "Shared"
    ''' <summary>
    ''' Make a key from raw data
    ''' </summary>
    ''' <param name="appEnvironment">The application environment</param>
    ''' <param name="data">The raw data</param>
    ''' <returns>The key created from the data</returns>
    Public Shared Function makeKey(ByVal appEnvironment As clsAppEnvironment, ByVal data As IObjectData) As clsKey
        Dim keyComponents As Generic.List(Of clsKeyComponent)
        Dim objectData As clsESPObjectData

        If data Is Nothing Then
            Throw New ArgumentNullException("Cannot call clsQuestion.makeKey when data is null.")
        End If

        If Not TypeOf (data) Is clsESPObjectData Then
            Throw New ArgumentException("clsQuestion.makeKey failed as it is only possible to create a key if data is of the type clsESPObjectData.")
        End If

        objectData = DirectCast(data, clsESPObjectData)
        keyComponents = New Generic.List(Of clsKeyComponent)
        keyComponents.Add(New clsKeyComponent(appEnvironment, objectData(ColumnList.ID.Ordinal)))
        Return New clsKey(appEnvironment, keyComponents)
    End Function
#End Region

    ''' <summary>
    ''' The unique key used to resolve the business object
    ''' </summary>
    Public Overridable ReadOnly Property IBO_key() As clsKey Implements IBO.key
        Get
            If mIBOKey Is Nothing OrElse Not mIBOKey.doesKeyMatchIBO(Me) Then
                mIBOKey = clsKey.createKey(mAppEnvironment, Me)
            End If

            Return mIBOKey
        End Get
    End Property
	
	''' <summary>
    ''' Allows the business object to define service parameters for an external service
    ''' </summary>
    Public Overridable ReadOnly Property IBO_serviceParameters() As IServiceParameters Implements IBO.serviceParameters
        Get
            Return mServiceParameters
        End Get
    End Property

    ''' <summary>
    ''' Database id for the business object.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_id() As Integer Implements IBO.id
        Get
            Return clsBusinessObjectUtilities.getSafeBusObjID(mValues(ColumnList.ID.Ordinal))
        End Get
    End Property

    Public Overridable Sub IBO_setID(ByVal thisID As Integer) Implements IBO.setID
        mValues(ColumnList.ID.Ordinal) = thisID
    End Sub

    ''' <summary>
    ''' Number of fields on the business object.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_fieldCount() As Integer Implements IBO.fieldCount
        Get
            Return FIELD_COUNT
        End Get
    End Property



    ''' <summary>
    ''' Save the business object.
    ''' </summary>
    ''' <returns>True if successful</returns>
    Public Overridable Function IBO_save() As Boolean Implements IBO.save
        Dim validTrue As Boolean

'*** Start Custom Code pre-save
'*** End Custom Code
        If isValid() Then
            validTrue = True
            IBO_save = mCache.Save(Me)
        End If
'*** Start Custom Code post-save
'*** End Custom Code
    End Function

    ''' <summary>
    ''' Load the business object from persistent storage.
    ''' </summary>
    Public Overridable Function IBO_loadFromStorage(ByVal refresh As Boolean) As Boolean Implements IBO.loadFromStorage
        Dim settonothing As Boolean
'*** Start Custom Code pre-loadfromstorage
'*** End Custom Code
        If Not mValues Is Nothing Then
            'if this load fails, it sets the values variable to nothing
            IBO_loadFromStorage = mCache.loadFromStorage(Me, refresh)
        End If
        If Not IBO_loadFromStorage Then
            settonothing = True
        End If
        If settonothing Then
        End If
'*** Start Custom Code post-loadfromstorage
'*** End Custom Code
    End Function

    ''' <summary>
    ''' Delete the business object.
    ''' </summary>
    Public Overridable Function IBO_delete() As Boolean Implements IBO.delete
        Dim valid As Boolean
'*** Start Custom Code pre-delete
'*** End Custom Code
        If CInt(mValues(ColumnList.ID.Ordinal)) <> 0 Then
            valid = True
            IBO_delete = mCache.Delete(Me)
        End If
'*** Start Custom Code post-delete
'*** End Custom Code
    End Function

    ''' <summary>
    ''' Business object has changed.
    ''' </summary>
    Public Overridable Property IBO_dirty() As Boolean Implements IBO.dirty
        Get
            Return mdirty
        End Get
        Set(ByVal thisDirty As Boolean)
            If IBO_dirty <> thisDirty Then
                mdirty = thisDirty
                If mChangeStack = 0 Then
                    RaiseEvent changeDirty()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Business object is read only.
    ''' </summary>
    Public Overridable Property IBO_isReadOnly() As Boolean Implements IBO.isReadOnly
        Get
'*** Start Custom Code ibo_readonly
'*** End Custom Code
            Return mReadOnly
        End Get
        Set(ByVal thisReadOnly As Boolean)
            mReadOnly = thisReadOnly
        End Set
    End Property

    ''' <summary>
    ''' Business object has been persisted to storage.
    ''' </summary>
    Public Overridable Property IBO_isStored() As Boolean Implements IBO.isStored
        Get
            Return mIsStored
        End Get
        Set(ByVal thisIsStored As Boolean)
            mIsStored = thisIsStored
        End Set
    End Property

    ''' <summary>
    ''' Last datetime that the business object has been changed.
    ''' </summary>
    Public Overridable Property IBO_maintTime() As Date Implements IBO.maintTime
        Get
            Return CDate(mValues(ColumnList.Mainttime.Ordinal))
        End Get
        Set(ByVal thisMaintTime As Date)
            mValues(ColumnList.Mainttime.Ordinal) = thisMaintTime
        End Set
    End Property

    ''' <summary>
    ''' Last user to change the business object.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_user() As String Implements IBO.user
        Get
            Return Convert.ToString(mValues(ColumnList.UserID.Ordinal))
        End Get
    End Property

    ''' <summary>
    ''' Business object description.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_description(Optional ByVal viewPoint As String = "") As String Implements IBO.description
        Get
            Dim separator As String
            separator = " "
            IBO_description = ""
'*** Start Custom Code ibo_description_preassignment
'*** End Custom Code
        End Get
    End Property

    ''' <summary>
    ''' Business object is wanted.
    ''' </summary>
    Public Overridable Function IBO_isWanted() As Boolean Implements IBO.isWanted
        IBO_isWanted = True
'*** Start Custom Code ibo_iswanted_end
'*** End Custom Code
    End Function

    ''' <summary>
    ''' Set a field to null.
    ''' </summary>
    ''' <param name="fldName">name of field to set</param>
    ''' <param name="arrayIdx">*Optional* which element in a field array</param>
    Public Overridable Sub IBO_setFieldToNull(ByVal fldName As String, Optional ByVal arrayIdx As Integer = -1) Implements IBO.setFieldToNull
        Dim msg As String
'*** Start Custom Code ibo_setfieldtonull_precase
'*** End Custom Code
        Select Case clsStringUtilities.noSpaces(fldName).ToLower
'*** Start Custom Code ibo_setfieldtonull_start
'*** End Custom Code
            Case "name"
                mValues(ColumnList.Name.Ordinal) = System.DBNull.Value
            Case "answerrtf"
                mValues(ColumnList.AnswerRTF.Ordinal) = System.DBNull.Value
            Case "answertext"
                mValues(ColumnList.AnswerText.Ordinal) = System.DBNull.Value
            Case "memorizedindex"
                mValues(ColumnList.MemorizedIndex.Ordinal) = System.DBNull.Value
            Case "deferredtilldate"
                mValues(ColumnList.DeferredTillDate.Ordinal) = System.DBNull.Value
            Case "topicid"
                mValues(ColumnList.TopicID.Ordinal) = System.DBNull.Value
'*** Start Custom Code ibo_setfieldtonull_customcase
'*** End Custom Code
            Case Else
                msg = _
                  mAppEnvironment.LangController.translate("Unknown field: @fldname" _
                , "Messages.Error Messages.Global.Unknown Field.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage _
                , New clsInfoParam("fldname", fldName))

                Throw New ArgumentOutOfRangeException(fldName, msg)
        End Select
'*** Start Custom Code ibo_setfieldtonull_end
'*** End Custom Code
        IBO_dirty = True
    End Sub

    ''' <summary>
    ''' Returns true if the field is null.
    ''' </summary>
    ''' <param name="fldName">name of field</param>
    ''' <param name="arrayIdx">*Optional* which element in a field array</param>
    Public Overridable Function IBO_isFieldNull(ByVal fldName As String, Optional ByVal arrayIdx As Integer = -1) As Boolean Implements IBO.isFieldNull
        Dim errMsg As String
        IBO_isFieldNull = True
'*** Start Custom Code ibo_isfieldnull_precase
'*** End Custom Code
        Select Case clsStringUtilities.noSpaces(fldName).ToLower
'*** Start Custom Code ibo_isfieldnull_start
'*** End Custom Code
            Case "id"
                IBO_isFieldNull = clsBusinessObjectUtilities.isIDFieldNull(mValues(ColumnList.ID.Ordinal))
            Case "mainttime"
                IBO_isFieldNull = clsBusinessObjectUtilities.isDateFieldNull(mValues(ColumnList.Mainttime.Ordinal))
            Case "userid"
                IBO_isFieldNull = clsBusinessObjectUtilities.isStringFieldNull(mValues(ColumnList.UserID.Ordinal))
            Case "name"
                IBO_isFieldNull = clsBusinessObjectUtilities.isStringFieldNull(mValues(ColumnList.Name.Ordinal))
            Case "answerrtf"
                IBO_isFieldNull = clsBusinessObjectUtilities.isStringFieldNull(mValues(ColumnList.AnswerRTF.Ordinal))
            Case "answertext"
                IBO_isFieldNull = clsBusinessObjectUtilities.isStringFieldNull(mValues(ColumnList.AnswerText.Ordinal))
            Case "memorizedindex"
                IBO_isFieldNull = clsBusinessObjectUtilities.isStringFieldNull(mValues(ColumnList.MemorizedIndex.Ordinal))
            Case "deferredtilldate"
                IBO_isFieldNull = clsBusinessObjectUtilities.isDateFieldNull(mValues(ColumnList.DeferredTillDate.Ordinal))
            Case "topicid"
                IBO_isFieldNull = clsBusinessObjectUtilities.isIDFieldNull(mValues(ColumnList.TopicID.Ordinal))
'*** Start Custom Code ibo_isfieldnull_customcase
'*** End Custom Code
            Case Else
                errMsg = _
                  mAppEnvironment.LangController.translate("Unknown field: @fldname" _
                , "Messages.Error Messages.Global.Unknown Field.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage _
                , New clsInfoParam("fldname", fldName))

                Throw New ArgumentOutOfRangeException(fldName, errMsg)
        End Select
    End Function

    ''' <summary>
    ''' Get/Set a field value.
    ''' </summary>
    ''' <param name="fldName">name of field</param>
    ''' <param name="arrayIdx">*Optional* which element in a field array</param>
    Default Public Overridable Property IBO_field(ByVal fldName As String, Optional ByVal arrayIdx As Integer = -1) As Object Implements IBO.field
        Get
            Return getField(fldName, arrayIdx)
        End Get
        Set(ByVal fldValue As Object)
            letField(fldName, fldValue, arrayIdx)
        End Set
    End Property

    ''' <summary>
    ''' Set a field value.
    ''' </summary>
    ''' <param name="fldName">name of field</param>
    ''' <param name="fldValue">value</param>
    ''' <param name="arrayIdx">*Optional* which element in a field array</param>
    Public Overridable Sub letField(ByVal fldName As String, ByVal fldValue As Object, Optional ByVal arrayIdx As Integer = -1)
'*** Start Custom Code letfield_custom_start
'*** End Custom Code
        Dim errMsg As String = ""
        If (arrayIdx = -1) Then
            arrayIdx = clsBusinessObjectUtilities.getFieldIndex(fldName)
        End If
        Select Case clsBusinessObjectUtilities.getFieldName(clsStringUtilities.noSpaces(fldName).ToLower)
            Case "ibo_tag", "tag"
                Me.IBO_tag = fldValue
            Case "id"
                errMsg = mAppEnvironment.LangController.translate("Readonly field: @fldname", _
                "Messages.Error Messages.Global.Field Is Read Only.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage, New clsInfoParam("fldname", fldName))
                Throw New ArgumentOutOfRangeException("fldName", errMsg)
            Case "mainttime"
                errMsg = mAppEnvironment.LangController.translate("Readonly field: @fldname", _
                "Messages.Error Messages.Global.Field Is Read Only.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage, New clsInfoParam("fldname", fldName))
                Throw New ArgumentOutOfRangeException("fldName", errMsg)
            Case "userid"
                errMsg = mAppEnvironment.LangController.translate("Readonly field: @fldname", _
                "Messages.Error Messages.Global.Field Is Read Only.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage, New clsInfoParam("fldname", fldName))
                Throw New ArgumentOutOfRangeException("fldName", errMsg)
            Case "name"
                Me.name = CType(fldValue, String)
            Case "answerrtf"
                Me.answerRTF = CType(fldValue, String)
            Case "answertext"
                Me.answerText = CType(fldValue, String)
            Case "memorizedindex"
                Me.memorizedIndex = CType(fldValue, String)
            Case "deferredtilldate"
                Me.deferredTillDate = CType(fldValue, Date)
            Case "topicid"
                Me.topicID = CType(fldValue, Integer)
'*** Start Custom Code letfield_customcase
'*** End Custom Code
            Case Else
                errMsg = mAppEnvironment.LangController.translate("Unknown field: @fldname", "Messages.Error Messages.Global.Unknown Field.Text" _
                , clsInfoMessage.MessageSeverity.ErrorMessage, New clsInfoParam("fldname", fldName))
                Throw New ArgumentOutOfRangeException(fldName, errMsg)
        End Select
    End Sub

    ''' <summary>
    ''' Get a field value.
    ''' </summary>
    ''' <param name="fldName">name of field</param>
    ''' <param name="arrayIdx">*Optional* which element in a field array</param>
    Public Overridable Function getField(ByVal fldName As String, Optional ByVal arrayIdx As Integer = -1) As Object
'*** Start Custom Code getfield_precase
'*** End Custom Code

        Select Case clsStringUtilities.noSpaces(fldName).ToLower
            Case "ibo_description"
                Return Me.IBO_description
            Case "ibo_tag", "tag"
                Return Me.IBO_tag
'*** Start Custom Code getfield_start
'*** End Custom Code
            Case "id"
                Return Me.iD
            Case "mainttime"
                Return Me.mainttime
            Case "userid"
                Return Me.userID
            Case "name"
                Return Me.name
            Case "answerrtf"
                Return Me.answerRTF
            Case "answertext"
                Return Me.answerText
            Case "memorizedindex"
                Return Me.memorizedIndex
            Case "deferredtilldate"
                Return Me.deferredTillDate
            Case "topicid"
                Return Me.topicID
            Case "topic"
                Return Me.Topic()
            Case "reviews"
                If arrayIdx <> -1 Then
                    Return Me.Reviews.Item(arrayIdx)
                Else
                    Return Me.Reviews()
                End If
'*** Start Custom Code getfield_customcase
'*** End Custom Code
            Case Else
                Return clsBusinessObjectUtilities.walkBusObjs(Me, fldName)
        End Select
    End Function

    ''' <summary>
    ''' Get/Set the values collection
    ''' </summary>
    Public Overridable Property ibo_values() As IObjectData Implements IBO.values
        Get
            Return mValues
        End Get
        Set(ByVal otherValues As IObjectData)
            If DirectCast(mCache, ICoherentCache).allowValuesOverwrite(Me) Then
                mValues = DirectCast(otherValues, clsESPObjectData)
            End If
'*** Start Custom Code ibo_values_end
'*** End Custom Code
        End Set
    End Property

    ''' <summary>
    ''' Business object tag.
    ''' </summary>
    Public Overridable Property IBO_tag() As Object Implements IBO.tag
        Get
            Return mTag
        End Get
        Set(ByVal newTag As Object)
            mTag = newTag
        End Set
    End Property

    ''' <summary>
    ''' Business object parent IBO.
    ''' </summary>
    Public Overridable Property IBO_parent() As IBO Implements IBO.parent
        Get
            Return mParent
        End Get
        Set(ByVal busObj As IBO)
            mParent = busObj
        End Set
    End Property

    ''' <summary>
    ''' This is a collection of activities which are actions.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_actionList() As clsBusinessObjectCollection(Of clsActivity) Implements IBO.actionList
        Get
            Return IBO_businessClass.Actions
        End Get
    End Property

    ''' <summary>
    ''' Cache that this business object belongs to
    ''' </summary>
    Public Overridable ReadOnly Property cache() As ICache Implements IBO.cache
        Get
            Return mCache
        End Get
    End Property

    ''' <summary>
    ''' Execute a business object activity.
    ''' </summary>
    ''' <param name="activity">activity to execute</param>
    ''' <param name="parameters">*Optional* parameters for the activity</param>
    Public Overridable Function IBO_execute(ByVal activity As String, Optional ByVal parameters As clsList = Nothing) As Object Implements IBO.execute
'*** Start Custom Code ibo_execute_start
'*** End Custom Code

        Dim errMsg As String
        IBO_execute = Nothing
        Select Case clsStringUtilities.noSpaces(activity).ToLower
'*** Start Custom Code ibo_execute_customactions
'*** End Custom Code
            Case Else
                errMsg = mAppEnvironment.LangController.translate("Action @activity is unknown for class @className: ", _
                "Messages.Error Messages.Global.Unknown Activity.Text", clsInfoMessage.MessageSeverity.ErrorMessage _
                , New clsInfoParam("activity", activity), New clsInfoParam("className", Me.IBO_businessClass.displayname))
                Throw New InvalidOperationException(errMsg)
        End Select
    End Function

    ''' <summary>
    ''' *Private* Check if a business object activity can be executed.
    ''' </summary>
    ''' <param name="activity">activity to execute</param>
    Private Function checkCanExecute(ByVal activity As String) As Boolean
        If Not IBO_canExecute(activity) Then
            Dim errMsg As String = mAppEnvironment.LangController.translate("Cannot '@activity' for @className with status @Status", _
                "Messages.Error Message.Global.Invalid Activity.Text", clsInfoMessage.MessageSeverity.ErrorMessage _
                , New clsInfoParam("activity", activity), New clsInfoParam("className", Me.IBO_businessClass.displayname) _
                , New clsInfoParam("status", Me.IBO_statusText))
            Throw New InvalidOperationException(errMsg)
        End If
        checkCanExecute = mMetaData.validateRulesforActivity(Me, activity)
    End Function

    ''' <summary>
    ''' Determines if the business object activity can be executed.
    ''' </summary>
    ''' <param name="activity">activity to execute</param>
    ''' <returns>True if the business object activity can be executed</returns>
    Public Overridable Function IBO_canExecute(ByVal activity As String) As Boolean Implements IBO.canExecute
        Dim canExec As Boolean
        Dim hasPermission As Boolean
        Dim canExecuteForPlant As IPlant = Nothing

'*** Start Custom Code ibo_canexecute_setplant
'*** End Custom Code
        hasPermission = mAppEnvironment.CurrentUser.canAccess(activity, canExecuteForPlant)
        If hasPermission Then
            canExec = True
            Select Case clsStringUtilities.noSpaces(activity).ToLower
'*** Start Custom Code ibo_canexecute_customactions
'*** End Custom Code
            End Select
        End If
'*** Start Custom Code ibo_canexecute_customactionsend
'*** End Custom Code
        IBO_canExecute = canExec And hasPermission
    End Function

    ''' <summary>
    ''' This must be called to initialise a new business object.
    ''' </summary>
    Public Overridable Function IBO_initialise() As Boolean Implements IBO.initialise
        IBO_initialise = mMetaData.initialiseBusObj(Me)
'*** Start Custom Code initialise_busobj
'*** End Custom Code
        mInitialised = True
    End Function

    ''' <summary>
    ''' Clone the business object.
    ''' </summary>
    Public Overridable Function IBO_cloneMe() As IBO
'*** Start Custom Code ibo_cloneme_pre
'*** End Custom Code
        IBO_cloneMe = mCache.copyBusObject(Me)
'*** Start Custom Code ibo_cloneme_post
'*** End Custom Code
    End Function


    '*************************************************************
    ' Publish Attributes
    '*************************************************************

    'Property: ID 
    'A number that uniquely identifies this question in the database.
    Public Overridable ReadOnly Property ID() As Integer
        Get
            Try
'*** Start Custom Code get_id_start
'*** End Custom Code
                Return CType(clsBusinessObjectUtilities.getSafeNum(mValues(ColumnList.ID.Ordinal)), Integer)
            Catch ex As Exception
'*** Start Custom Code get_id_err
'*** End Custom Code
                Throw
            End Try
        End Get
    End Property


    'Property: mainttime 
    'The time that this question was last modified.
    Public Overridable ReadOnly Property mainttime() As Date
        Get
            Try
'*** Start Custom Code get_mainttime_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeDate(mValues(ColumnList.Mainttime.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_mainttime_err
'*** End Custom Code
                Throw
            End Try
        End Get
    End Property


    'Property: userID 
    'The last user to modify this question.
    Public Overridable ReadOnly Property userID() As String
        Get
            Try
'*** Start Custom Code get_userid_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeString(mValues(ColumnList.UserID.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_userid_err
'*** End Custom Code
                Throw
            End Try
        End Get
    End Property


    'Property: name 
    Public Overridable Property name() As String
        Get
            Try
'*** Start Custom Code get_name_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeString(mValues(ColumnList.Name.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_name_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisName As String)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_name_start
'*** End Custom Code
            isValueChanged = clsBusinessObjectUtilities.hasValueChanged(Me, "name", mValues(ColumnList.Name.Ordinal), thisName)
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValues = mValues.Clone()
                mValues(ColumnList.Name.Ordinal) = thisName
                mMetaData.validateAttribute(Me, mMetaData.businessAttribute(IBO_businessClass, "name"))
                IBO_dirty = True
'*** Start Custom Code let_name_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
        End Set
    End Property


    'Property: answerRTF 
    Public Overridable Property answerRTF() As String
        Get
            Try
'*** Start Custom Code get_answerrtf_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeString(mValues(ColumnList.AnswerRTF.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_answerrtf_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisAnswerRTF As String)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_answerrtf_start
'*** End Custom Code
            isValueChanged = clsBusinessObjectUtilities.hasValueChanged(Me, "answerRTF", mValues(ColumnList.AnswerRTF.Ordinal), thisAnswerRTF)
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValues = mValues.Clone()
                mValues(ColumnList.AnswerRTF.Ordinal) = thisAnswerRTF
                mMetaData.validateAttribute(Me, mMetaData.businessAttribute(IBO_businessClass, "answerRTF"))
                IBO_dirty = True
'*** Start Custom Code let_answerrtf_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
        End Set
    End Property


    'Property: answerText 
    Public Overridable Property answerText() As String
        Get
            Try
'*** Start Custom Code get_answertext_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeString(mValues(ColumnList.AnswerText.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_answertext_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisAnswerText As String)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_answertext_start
'*** End Custom Code
            isValueChanged = clsBusinessObjectUtilities.hasValueChanged(Me, "answerText", mValues(ColumnList.AnswerText.Ordinal), thisAnswerText)
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValues = mValues.Clone()
                mValues(ColumnList.AnswerText.Ordinal) = thisAnswerText
                mMetaData.validateAttribute(Me, mMetaData.businessAttribute(IBO_businessClass, "answerText"))
                IBO_dirty = True
'*** Start Custom Code let_answertext_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
        End Set
    End Property


    'Property: memorizedIndex 
    Public Overridable Property memorizedIndex() As String
        Get
            Try
'*** Start Custom Code get_memorizedindex_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeString(mValues(ColumnList.MemorizedIndex.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_memorizedindex_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisMemorizedIndex As String)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_memorizedindex_start
'*** End Custom Code
            isValueChanged = clsBusinessObjectUtilities.hasValueChanged(Me, "memorizedIndex", mValues(ColumnList.MemorizedIndex.Ordinal), thisMemorizedIndex)
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValues = mValues.Clone()
                mValues(ColumnList.MemorizedIndex.Ordinal) = thisMemorizedIndex
                mMetaData.validateAttribute(Me, mMetaData.businessAttribute(IBO_businessClass, "memorizedIndex"))
                IBO_dirty = True
'*** Start Custom Code let_memorizedindex_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
        End Set
    End Property


    'Property: deferredTillDate 
    Public Overridable Property deferredTillDate() As Date
        Get
            Try
'*** Start Custom Code get_deferredtilldate_start
'*** End Custom Code
                Return clsBusinessObjectUtilities.getSafeDate(mValues(ColumnList.DeferredTillDate.Ordinal))
            Catch ex As Exception
'*** Start Custom Code get_deferredtilldate_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisDeferredTillDate As Date)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_deferredtilldate_start
'*** End Custom Code
            isValueChanged = clsBusinessObjectUtilities.hasValueChanged(Me, "deferredTillDate", mValues(ColumnList.DeferredTillDate.Ordinal), thisDeferredTillDate)
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValues = mValues.Clone()
                mValues(ColumnList.DeferredTillDate.Ordinal) = thisDeferredTillDate
                mMetaData.validateAttribute(Me, mMetaData.businessAttribute(IBO_businessClass, "deferredTillDate"))
                IBO_dirty = True
'*** Start Custom Code let_deferredtilldate_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
        End Set
    End Property


    'Property: topicID 
    Public Overridable Property topicID() As Integer
        Get
            Try
'*** Start Custom Code get_topicid_start
'*** End Custom Code
                Return CType(clsBusinessObjectUtilities.getSafeNum(mValues(ColumnList.TopicID.Ordinal)), Integer)
            Catch ex As Exception
'*** Start Custom Code get_topicid_err
'*** End Custom Code
                Throw
            End Try
        End Get
        Set(ByVal thisTopicID As Integer)
            Dim isValueChanged As Boolean = False
            Dim oldDirty As Boolean = False
            Dim oldValue As Object = Nothing
            Dim oldValues As IObjectData = Nothing
'*** Start Custom Code let_topicid_start
'*** End Custom Code
            isValueChanged = thisTopicID <> topicID
            If Not (isValueChanged Or Not IBO_isStored) Then Exit Property
            Try
                mChangeStack += 1
                oldDirty = IBO_dirty
                oldValue = mValues(ColumnList.TopicID.Ordinal)
                oldValues = mValues.Clone()
'*** Start Custom Code let_topicid_beforechange
'*** End Custom Code
                If thisTopicID = 0 Then
                    mValues(ColumnList.TopicID.Ordinal) = System.DBNull.Value
                Else
                    mValues(ColumnList.TopicID.Ordinal) = thisTopicID
                End If
                IBO_dirty = True
                DirectCast(mCache, ICoherentCache).updateBusinessObjectAssociations(Me, "topicID", oldValue, mValues(ColumnList.TopicID.Ordinal))
'*** Start Custom Code let_topicid_valuehaschanged
'*** End Custom Code
            Catch
                handleChangeException(oldValues, oldDirty)
                Throw
            Finally
                mChangeStack -= 1
            End Try
            raiseChangedEvents(oldDirty)
'*** Start Custom Code let_topicid_end
'*** End Custom Code
        End Set
    End Property


    '*************************************************************
    ' Status
    '*************************************************************
    ''' <summary>
    ''' Numeric status of the object.
    ''' </summary>
    Public Overridable Property IBO_status() As Integer Implements IBO.status
        Get
        End Get
        Set(ByVal status As Integer)
        End Set
    End Property

    ''' <summary>
    ''' Status text of the object.
    ''' </summary>
    Public Overridable Property IBO_statusText() As String Implements IBO.statusText
        Get
            IBO_statusText = Nothing
        End Get
        Set(ByVal thisStatusText As String)
        End Set
    End Property

    ''' <summary>
    ''' Business class for this object.
    ''' </summary>
    Public Overridable ReadOnly Property IBO_businessClass() As clsBusinessClass Implements IBO.businessClass
        Get
            If mClass Is Nothing Then
                mClass = mMetaData.businessClass("question")
            End If
            Return mClass
        End Get
    End Property

    '*************************************************************
    ' Publish Associations
    '*************************************************************
    'Property: topic 
    Public Overridable ReadOnly Property topic(Optional ByVal refresh As Boolean = False) As clsTopic
        Get
            topic = Nothing
'*** Start Custom Code get_topic_start
'*** End Custom Code
            topic = mCache.GetRelatedBusinessObject(Of clsTopic)(Me, "topic", refresh)
        End Get
    End Property

    'Property: reviews 
    Public Overridable ReadOnly Property reviews(Optional ByVal refresh As Boolean = False) As clsBusinessObjectCollection(Of clsReview)
        Get
'*** Start Custom Code get_reviews_start
'*** End Custom Code
                reviews = mCache.GetRelatedBusinessCollection(Of clsQuestion, clsReview)(Me, "question", refresh)
'*** Start Custom Code get_reviews_end
'*** End Custom Code
        End Get
    End Property

    'Sub: setTopicID 
    'sets the foreign key
    Public Overridable Sub setTopicID(ByVal thisID As Integer)
        topicID = thisID
    End Sub

    'Function: getTopicID 
    'gets the foreign key
    Public Overridable Function getTopicID() As Integer
        Return topicID
    End Function


    Private Sub handleChangeException(ByVal oldValues As IObjectData, ByVal oldDirty As Boolean)
        mValues = DirectCast(oldValues, clsESPObjectData)
        IBO_dirty = oldDirty
        DirectCast(mCache, ICoherentCache).updateBusinessObjectAssociations(Me)
    End Sub

    Private Sub raiseChangedEvents(ByVal oldDirty As Boolean)
        If mChangeStack = 0 Then
            If oldDirty <> IBO_dirty Then
                RaiseEvent changeDirty()
            End If
            RaiseEvent change(Me, New System.EventArgs)
        End If
    End Sub

    Public Overridable Function getDataMemberName(ByVal fieldName As String) As String
        getDataMemberName = ""
'*** Start Custom Code get_data_member_name
'*** End Custom Code
    End Function

    Public Overridable Function getWidgetName(ByVal fieldName As String) As String
        getWidgetName = ""
        Select Case clsStringUtilities.noSpaces(fieldName).ToLower
            Case "name", "1"
                getWidgetName = "kiwidget.kwtextbox" & " " & "name"
            Case "answerrtf", "2"
                getWidgetName = "kiwidget.kwtextbox" & " " & "answerrtf"
            Case "answertext", "3"
                getWidgetName = "kiwidget.kwtextbox" & " " & "answertext"
            Case "memorizedindex", "4"
                getWidgetName = "kiwidget.kwtextbox" & " " & "memorizedindex"
            Case "deferredtilldate", "5"
                getWidgetName = "kiwidget.kwcalendar" & " " & "deferredtilldate"
            Case "topicid", "6"
                getWidgetName = "kiwidget.kwnumberbox" & " " & "topicid"
                ' associations
            Case "topic", "7"
                getWidgetName = "kiwidget.kwcombo" & " " & "topic"
            Case "reviews", "8"
                getWidgetName = "kiwidget.kwcombo" & " " & "reviews"
'*** Start Custom Code get_widget_name
'*** End Custom Code
        End Select
    End Function

    ''' <summary>
    ''' Is the business object valid?
    ''' </summary>
    ''' <returns>true if the business object is valid, false if it is invalid.</returns>
    ''' <remarks>This method is obsolete. Please use IBO_isValid.
    ''' For further information please see the Validation How To and SCM 239643.</remarks>
    Public Overridable Function isValid() As Boolean
'*** Start Custom Code isvalid_pre_validation
'*** End Custom Code
        isValid = mMetaData.validateBusObj(Me)
'*** Start Custom Code isvalid_post_validation
'*** End Custom Code
    End Function

    ''' <summary>
    ''' Is the business object valid?
    ''' </summary>
    ''' <param name="messages">warning or information messages</param>
    ''' <returns>true if the business object is valid, false if it is invalid.</returns>
    ''' <remarks></remarks>
    Public Function IBO_isValid(ByVal messages As clsInfoMessages) As Boolean Implements IBO.isValid
'*** Start Custom Code isvalid_custom_validation
'*** End Custom Code
        Return True
    End Function

    Public Overridable Function getClonedQuestion() As clsQuestion
        getClonedQuestion = mClonedQuestion
    End Function

    ''' <summary>
    ''' Injects the application environment to the module level appenvironment variable
    ''' </summary>
    Private Sub Setup(ByVal aCache As ICache)
        mValues = New clsESPObjectData(FIELD_COUNT)
        mCache = aCache
        mAppEnvironment = aCache.appEnvironment
        mMetaData = mAppEnvironment.MetaData
        DirectCast(mCache, ICoherentCache).addNewBusinessObject(Me)
        mChangeStack = 0
        mServiceParameters = Nothing
        'Initialise any enumerations
'*** Start Custom Code class_initialize_end
'*** End Custom Code
        mInitialised = False
    End Sub

    ''' <summary>
    ''' Initialize this object with a ICache
    ''' </summary>
    Public Sub New(ByVal aCache As ICache)
        If aCache Is Nothing Then
            Throw New ArgumentNullException()
        End If
#If CACHETRACE Then
        Dim traceCache As ITracedCache = TryCast(aCache, ITracedCache)
        If traceCache IsNot Nothing Then
            mCreationTrace = New StackTrace(True)
            traceCache.TraceObjectCreation(Me, mCreationTrace)
        End If
#End If
        Setup(aCache)
    End Sub
'*** Start Custom Code custom_operations
'*** End Custom Code

End Class