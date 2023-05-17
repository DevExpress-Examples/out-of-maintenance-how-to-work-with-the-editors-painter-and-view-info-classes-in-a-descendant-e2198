Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils

<UserRepositoryItem("RegisterMyProgressBar")>
Public Class MyRepositoryItemProgressBar
    Inherits RepositoryItem

    Private _barColor As Color

    Public Property BarColor As Color
        Get
            Return _barColor
        End Get

        Set(ByVal value As Color)
            If _barColor = value Then Return
            _barColor = value
            OnPropertiesChanged()
        End Set
    End Property

    Shared Sub New()
        RegisterMyProgressBar()
    End Sub

    Public Shared Sub RegisterMyProgressBar()
        Call EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo("MyProgressBarControl", GetType(MyProgressBarControl), GetType(MyRepositoryItemProgressBar), GetType(MyProgressBarViewInfo), New MyProgressBarPainter(), True, CType(Nothing, System.Drawing.Image), GetType(DevExpress.Accessibility.PopupEditAccessible)))
    End Sub

    Public Overloads ReadOnly Property Properties As MyRepositoryItemProgressBar
        Get
            Return Me
        End Get
    End Property

    Private Shared positionChangedField As Object = New Object()

    <Browsable(False)>
    Public Overrides ReadOnly Property EditorTypeName As String
        Get
            Return "MyProgressBarControl"
        End Get
    End Property

    Private _minimum, _maximum As Integer

    Public Sub New()
        fAutoHeight = False
        _barColor = Color.YellowGreen
        _minimum = 0
        _maximum = 100
    End Sub

    Public Overrides Sub Assign(ByVal item As RepositoryItem)
        Dim source As MyRepositoryItemProgressBar = TryCast(item, MyRepositoryItemProgressBar)
        BeginUpdate()
        Try
            MyBase.Assign(item)
            If source Is Nothing Then Return
            _maximum = source.Maximum
            _minimum = source.Minimum
            BarColor = source.BarColor
        Finally
            EndUpdate()
        End Try

        Events.AddHandler(positionChangedField, source.Events(positionChangedField))
    End Sub

    Protected Overloads ReadOnly Property OwnerEdit As MyProgressBarControl
        Get
            Return TryCast(MyBase.OwnerEdit, MyProgressBarControl)
        End Get
    End Property

    <Category(CategoryName.Behavior), Description("Gets or sets the control's minimum value."), RefreshProperties(RefreshProperties.All), DefaultValue(0)>
    Public Property Minimum As Integer
        Get
            Return _minimum
        End Get

        Set(ByVal value As Integer)
            If Not IsLoading Then
                If value > Maximum Then value = Maximum
            End If

            If Minimum = value Then Return
            _minimum = value
            OnMinMaxChanged()
            OnPropertiesChanged()
        End Set
    End Property

    <Category(CategoryName.Behavior), Description("Gets or sets the control's maximum value."), RefreshProperties(RefreshProperties.All), DefaultValue(100)>
    Public Property Maximum As Integer
        Get
            Return _maximum
        End Get

        Set(ByVal value As Integer)
            If Not IsLoading Then
                If value < Minimum Then value = Minimum
            End If

            If Maximum = value Then Return
            _maximum = value
            OnMinMaxChanged()
            OnPropertiesChanged()
        End Set
    End Property

    Protected Friend Overridable Function ConvertValue(ByVal val As Object) As Integer
        Try
            Return CheckValue(Convert.ToInt32(val))
        Catch
        End Try

        Return Minimum
    End Function

    Protected Friend Overridable Function CheckValue(ByVal val As Integer) As Integer
        If IsLoading Then Return val
        val = Math.Max(val, Minimum)
        val = Math.Min(val, Maximum)
        Return val
    End Function

    Public Overrides Function GetBrick(ByVal info As PrintCellHelperInfo) As VisualBrick
        Dim brick As ProgressBarBrick = CType(MyBase.GetBrick(info), ProgressBarBrick)
        brick.Position = ConvertValue(info.EditValue)
        Return brick
    End Function

    Protected Overridable Sub OnMinMaxChanged()
        If OwnerEdit IsNot Nothing Then OwnerEdit.OnMinMaxChanged()
    End Sub

    <Description("Occurs after the value of the ProgressBarControl.Position property has been changed."), Category(CategoryName.Events)>
    Public Custom Event PositionChanged As EventHandler
        AddHandler(ByVal value As EventHandler)
            Events.AddHandler(positionChangedField, value)
        End AddHandler

        RemoveHandler(ByVal value As EventHandler)
            Events.RemoveHandler(positionChangedField, value)
        End RemoveHandler

        <Description("Occurs after the value of the ProgressBarControl.Position property has been changed."), Category(CategoryName.Events)>
        RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
        End RaiseEvent
    End Event

    Protected Overrides Sub RaiseEditValueChangedCore(ByVal e As EventArgs)
        MyBase.RaiseEditValueChangedCore(e)
        RaisePositionChanged(e)
    End Sub

    Protected Friend Overridable Sub RaisePositionChanged(ByVal e As EventArgs)
        Dim handler As EventHandler = CType(Events(positionChangedField), EventHandler)
        If handler IsNot Nothing Then handler(GetEventSender(), e)
    End Sub
End Class

<ToolboxItem(True)>
Public Class MyProgressBarControl
    Inherits BaseEdit

    Shared Sub New()
        MyRepositoryItemProgressBar.RegisterMyProgressBar()
    End Sub

    Public Sub New()
        TabStop = False
        fEditValue = 0
        fOldEditValue = fEditValue
    End Sub

    <Browsable(False)>
    Public Overrides ReadOnly Property EditorTypeName As String
        Get
            Return "MyProgressBarControl"
        End Get
    End Property

    <Description("Gets an object containing properties, methods and events specific to progress bar controls."), Category(CategoryName.Properties), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public Overloads ReadOnly Property Properties As MyRepositoryItemProgressBar
        Get
            Return TryCast(MyBase.Properties, MyRepositoryItemProgressBar)
        End Get
    End Property

    <Browsable(False), DefaultValue(0)>
    Public Overrides Property EditValue As Object
        Get
            Return MyBase.EditValue
        End Get

        Set(ByVal value As Object)
            MyBase.EditValue = ConvertCheckValue(value)
        End Set
    End Property

    <Category(CategoryName.Appearance), Description("Gets or sets progress bar position."), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Bindable(ControlConstants.NonObjectBindable)>
    Public Overridable Property Position As Integer
        Get
            Return Properties.ConvertValue(EditValue)
        End Get

        Set(ByVal value As Integer)
            EditValue = Properties.CheckValue(value)
        End Set
    End Property

    Protected Overridable Function ConvertCheckValue(ByVal val As Object) As Object
        If val Is Nothing OrElse TypeOf val Is DBNull Then Return val
        Dim converted As Integer = Properties.ConvertValue(val)
        Try
            If converted = Convert.ToInt32(val) Then Return val
        Catch
        End Try

        Return converted
    End Function

    Protected Friend Overridable Sub OnMinMaxChanged()
        If IsLoading Then Return
        Position = Properties.CheckValue(Position)
    End Sub

    Protected Friend Overridable Sub UpdatePercent()
        Dim vi As MyProgressBarViewInfo = TryCast(ViewInfo, MyProgressBarViewInfo)
        If vi Is Nothing Then Return
        vi.UpdateProgressInfo(vi.ProgressInfo)
    End Sub

    Public Custom Event PositionChanged As EventHandler
        AddHandler(ByVal value As EventHandler)
            AddHandler Properties.PositionChanged, value
        End AddHandler

        RemoveHandler(ByVal value As EventHandler)
            RemoveHandler Properties.PositionChanged, value
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
        End RaiseEvent
    End Event
End Class

Public Class MyObjectInfoArgs
    Inherits ObjectInfoArgs

    Public Sub New()
    End Sub

    Public BackgroundColor As Color

    Public BarColor As Color

    Public Percent As Single = 0
End Class

<ToolboxBitmap(GetType(MyProgressBarControl), "")>
Public Class MyProgressBarViewInfo
    Inherits BaseEditViewInfo

    Private positionField As Integer

    Private percentsField As Single

    Private progressInfoField As MyObjectInfoArgs

    Public Sub New(ByVal item As RepositoryItem)
        MyBase.New(item)
    End Sub

    Protected Overrides Sub Assign(ByVal info As BaseControlViewInfo)
        MyBase.Assign(info)
        Dim be As MyProgressBarViewInfo = TryCast(info, MyProgressBarViewInfo)
        If be Is Nothing Then Return
        percentsField = be.percentsField
        positionField = be.positionField
    End Sub

    Protected Overrides Sub OnEditValueChanged()
        positionField = Item.ConvertValue(EditValue)
        percentsField = 1
        If Item.Maximum <> Item.Minimum Then
            percentsField = Math.Abs(CSng(Position - Item.Minimum) / CSng(Item.Maximum - Item.Minimum))
        End If
    End Sub

    Public Overridable ReadOnly Property Position As Integer
        Get
            Return positionField
        End Get
    End Property

    Public Overridable ReadOnly Property Percents As Single
        Get
            Return percentsField
        End Get
    End Property

    Protected Overrides Sub ReCalcViewInfoCore(ByVal g As Graphics, ByVal buttons As MouseButtons, ByVal mousePosition As Point, ByVal bounds As Rectangle)
        MyBase.ReCalcViewInfoCore(g, buttons, mousePosition, bounds)
        UpdateProgressInfo(ProgressInfo)
    End Sub

    Public Overrides Sub Reset()
        MyBase.Reset()
        progressInfoField = CreateInfoArgs()
        positionField = 0
        percentsField = 0
    End Sub

    Public Overridable ReadOnly Property ProgressInfo As MyObjectInfoArgs
        Get
            Return progressInfoField
        End Get
    End Property

    Public Overloads ReadOnly Property Item As MyRepositoryItemProgressBar
        Get
            Return TryCast(MyBase.Item, MyRepositoryItemProgressBar)
        End Get
    End Property

    Protected Friend Overridable Sub UpdateProgressInfo(ByVal info As MyObjectInfoArgs)
        info.Bounds = ContentRect
        info.BarColor = Item.BarColor
        info.BackgroundColor = Item.Appearance.BackColor
        info.Percent = Percents
    End Sub

    Protected Overrides Function GetBorderArgs(ByVal bounds As Rectangle) As ObjectInfoArgs
        Dim pi As MyObjectInfoArgs = New MyObjectInfoArgs()
        pi.Bounds = bounds
        Return pi
    End Function

    Protected Overrides Function GetBorderPainterCore() As BorderPainter
        Dim bp As BorderPainter = MyBase.GetBorderPainterCore()
        If TypeOf bp Is WindowsXPTextBorderPainter Then bp = New WindowsXPProgressBarBorderPainter()
        Return bp
    End Function

    Public Overrides Sub Offset(ByVal x As Integer, ByVal y As Integer)
        MyBase.Offset(x, y)
        ProgressInfo.OffsetContent(x, y)
    End Sub

    Protected Overrides Sub CalcContentRect(ByVal bounds As Rectangle)
        MyBase.CalcContentRect(bounds)
        UpdateProgressInfo(ProgressInfo)
    End Sub

    Protected Overridable Function CreateInfoArgs() As MyObjectInfoArgs
        Return New MyObjectInfoArgs()
    End Function

    Public Overrides Property EditValue As Object
        Get
            Return MyBase.EditValue
        End Get

        Set(ByVal value As Object)
            fEditValue = value
            OnEditValueChanged()
        End Set
    End Property
End Class

Public Class MyProgressBarPainter
    Inherits BaseEditPainter

    Protected Overrides Sub DrawContent(ByVal info As ControlGraphicsInfoArgs)
        Dim vi As MyProgressBarViewInfo = TryCast(info.ViewInfo, MyProgressBarViewInfo)
        info.Graphics.FillRectangle(New SolidBrush(info.ViewInfo.PaintAppearance.BackColor), info.Bounds)
        DrawObject(info.Cache, vi.ProgressInfo)
    End Sub

    Public Sub DrawObject(ByVal cache As GraphicsCache, ByVal e As ObjectInfoArgs)
        If e Is Nothing Then Return
        Dim ee As MyObjectInfoArgs = TryCast(e, MyObjectInfoArgs)
        Dim prev As GraphicsCache = e.Cache
        e.Cache = cache
        Try
            e.Cache.Paint.FillRectangle(e.Graphics, New SolidBrush(ee.BackgroundColor), e.Bounds)
            DrawBar(ee)
        Finally
            e.Cache = prev
        End Try
    End Sub

    Protected Overridable Sub DrawBar(ByVal e As MyObjectInfoArgs)
        Dim brush As Brush = e.Cache.GetSolidBrush(e.BarColor)
        e.Cache.Paint.FillGradientRectangle(e.Graphics, brush, CalcProgressBounds(e))
    End Sub

    Protected Overridable Function CalcProgressBounds(ByVal e As MyObjectInfoArgs) As Rectangle
        Dim r As Rectangle = e.Bounds
        If e.Percent < 1 Then
            Dim size As Size = r.Size
            r.Width = Math.Min(Convert.ToInt32(e.Percent * CSng(size.Width)), size.Width)
        End If

        Return r
    End Function
End Class
