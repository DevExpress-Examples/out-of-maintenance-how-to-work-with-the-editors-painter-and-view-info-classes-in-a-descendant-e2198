Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils
<UserRepositoryItem("RegisterMyProgressBar")> _
Public Class MyRepositoryItemProgressBar
	Inherits RepositoryItem
		Private _barColor As Color
	Public Property BarColor() As Color
		Get
			Return _barColor
		End Get
		Set(ByVal value As Color)
			If _barColor = value Then
				Return
			End If
			_barColor = value
			Me.OnPropertiesChanged()
		End Set
	End Property
	Shared Sub New()
		RegisterMyProgressBar()
	End Sub
	Public Shared Sub RegisterMyProgressBar()
		EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo("MyProgressBarControl", GetType(MyProgressBarControl), GetType(MyRepositoryItemProgressBar), GetType(MyProgressBarViewInfo), New MyProgressBarPainter(), True,EditImageIndexes.ProgressBarControl, GetType(DevExpress.Accessibility.PopupEditAccessible)))
	End Sub

	Public Shadows ReadOnly Property Properties() As MyRepositoryItemProgressBar
		Get
			Return Me
		End Get
	End Property
    Private Shared fPositionChanged As Object = New Object()
	<Browsable(False)> _
	Public Overrides ReadOnly Property EditorTypeName() As String
		Get
			Return "MyProgressBarControl"
		End Get
	End Property
	Private _minimum, _maximum As Integer
	Public Sub New()

		Me.fAutoHeight = False
		Me._barColor = Color.YellowGreen
		Me._minimum = 0
		Me._maximum = 100
	End Sub
	Public Overrides Sub Assign(ByVal item As RepositoryItem)
		Dim source As MyRepositoryItemProgressBar = TryCast(item, MyRepositoryItemProgressBar)
		BeginUpdate()
		Try
			MyBase.Assign(item)
			If source Is Nothing Then
				Return
			End If
			Me._maximum = source.Maximum
			Me._minimum = source.Minimum
			Me.BarColor = source.BarColor
		Finally
			EndUpdate()
		End Try
        Events.AddHandler(fPositionChanged, source.Events(fPositionChanged))
	End Sub
	Protected Shadows ReadOnly Property OwnerEdit() As MyProgressBarControl
		Get
			Return TryCast(MyBase.OwnerEdit, MyProgressBarControl)
		End Get
	End Property
	<Category(CategoryName.Behavior), Description("Gets or sets the control's minimum value."), RefreshProperties(RefreshProperties.All), DefaultValue(0)> _
	Public Property Minimum() As Integer
		Get
			Return _minimum
		End Get
		Set(ByVal value As Integer)
			If (Not IsLoading) Then
				If value > Maximum Then
					value = Maximum
				End If
			End If
			If Minimum = value Then
				Return
			End If
			_minimum = value
			OnMinMaxChanged()
			OnPropertiesChanged()
		End Set
	End Property
	<Category(CategoryName.Behavior), Description("Gets or sets the control's maximum value."), RefreshProperties(RefreshProperties.All), DefaultValue(100)> _
	Public Property Maximum() As Integer
		Get
			Return _maximum
		End Get
		Set(ByVal value As Integer)
			If (Not IsLoading) Then
				If value < Minimum Then
					value = Minimum
				End If
			End If
			If Maximum = value Then
				Return
			End If
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
		If IsLoading Then
			Return val
		End If
		val = Math.Max(val, Minimum)
		val = Math.Min(val, Maximum)
		Return val
	End Function
	Public Overrides Function GetBrick(ByVal info As PrintCellHelperInfo) As IVisualBrick
		Dim brick As IProgressBarBrick = CType(MyBase.GetBrick(info), IProgressBarBrick)
		brick.Position = ConvertValue(info.EditValue)
		Return brick
	End Function
	Protected Overridable Sub OnMinMaxChanged()
		If OwnerEdit IsNot Nothing Then
			OwnerEdit.OnMinMaxChanged()
		End If
	End Sub
	<Description("Occurs after the value of the ProgressBarControl.Position property has been changed."), Category(CategoryName.Events)> _
	Public Custom Event PositionChanged As EventHandler
		AddHandler(ByVal value As EventHandler)
            Me.Events.AddHandler(fPositionChanged, value)
		End AddHandler
		RemoveHandler(ByVal value As EventHandler)
            Me.Events.RemoveHandler(fPositionChanged, value)
		End RemoveHandler
		RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
		End RaiseEvent
	End Event
	Protected Overrides Sub RaiseEditValueChangedCore(ByVal e As EventArgs)
		MyBase.RaiseEditValueChangedCore(e)
		RaisePositionChanged(e)
	End Sub
	Protected Friend Overridable Sub RaisePositionChanged(ByVal e As EventArgs)
        Dim handler As EventHandler = CType(Me.Events(fPositionChanged), EventHandler)
		If handler IsNot Nothing Then
			handler(GetEventSender(), e)
		End If
	End Sub
End Class
 <ToolboxItem(True)> _
 Public Class MyProgressBarControl
	 Inherits BaseEdit

	Shared Sub New()
		MyRepositoryItemProgressBar.RegisterMyProgressBar()
	End Sub
	 Public Sub New()
			Me.TabStop = False
			Me.fEditValue = 0
			Me.fOldEditValue = Me.fEditValue
	 End Sub
	<Browsable(False)> _
	Public Overrides ReadOnly Property EditorTypeName() As String
		Get
			Return "MyProgressBarControl"
		End Get
	End Property
	<Description("Gets an object containing properties, methods and events specific to progress bar controls."), Category(CategoryName.Properties), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
	Public Shadows ReadOnly Property Properties() As MyRepositoryItemProgressBar
		Get
			Return TryCast(MyBase.Properties, MyRepositoryItemProgressBar)
		End Get
	End Property
	<Browsable(False), DefaultValue(0)> _
	Public Overrides Property EditValue() As Object
		Get
			Return MyBase.EditValue
		End Get
		Set(ByVal value As Object)
			MyBase.EditValue = ConvertCheckValue(value)
		End Set
	End Property
	<Category(CategoryName.Appearance), Description("Gets or sets progress bar position."), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Bindable(ControlConstants.NonObjectBindable)> _
	Public Overridable Property Position() As Integer
		Get
			Return Properties.ConvertValue(EditValue)
		End Get
		Set(ByVal value As Integer)
			EditValue = Properties.CheckValue(value)
		End Set
	End Property
	Protected Overridable Function ConvertCheckValue(ByVal val As Object) As Object
		If val Is Nothing OrElse TypeOf val Is DBNull Then
			Return val
		End If
		Dim converted As Integer = Properties.ConvertValue(val)
		Try
			If converted = Convert.ToInt32(val) Then
				Return val
			End If
		Catch
		End Try
		Return converted
	End Function
	Protected Friend Overridable Sub OnMinMaxChanged()
		If IsLoading Then
			Return
		End If
		Position = Properties.CheckValue(Position)
	End Sub
	Protected Friend Overridable Sub UpdatePercent()
		Dim vi As MyProgressBarViewInfo = TryCast(ViewInfo, MyProgressBarViewInfo)
		If vi Is Nothing Then
			Return
		End If
		vi.UpdateProgressInfo(vi.ProgressInfo)
	End Sub
	Public Custom Event PositionChanged As EventHandler
		AddHandler(ByVal value As EventHandler)
			AddHandler Properties.PositionChanged, value
		End AddHandler
		RemoveHandler(ByVal value As EventHandler)
			RemoveHandler Properties.PositionChanged, value
		End RemoveHandler
		RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

<ToolboxBitmap(GetType(MyProgressBarControl), "")> _
Public Class MyProgressBarViewInfo
	Inherits BaseEditViewInfo
		Private position_Renamed As Integer
		Private percents_Renamed As Single
		Private progressInfo_Renamed As MyObjectInfoArgs
			Public Sub New(ByVal item As RepositoryItem)
				MyBase.New(item)

			End Sub
		Protected Overrides Sub Assign(ByVal info As BaseControlViewInfo)
			MyBase.Assign(info)
			Dim be As MyProgressBarViewInfo = TryCast(info, MyProgressBarViewInfo)
			If be Is Nothing Then
				Return
			End If
			Me.percents_Renamed = be.percents
			Me.position_Renamed = be.position
		End Sub
		Protected Overrides Sub OnEditValueChanged()
			Me.position_Renamed = Item.ConvertValue(EditValue)
			Me.percents_Renamed = 1
			If Item.Maximum <> Item.Minimum Then
				Me.percents_Renamed = Math.Abs(CSng(Position - Item.Minimum) / CSng(Item.Maximum - Item.Minimum))
			End If
		End Sub
		Public Overridable ReadOnly Property Position() As Integer
			Get
				Return position_Renamed
			End Get
		End Property
		Public Overridable ReadOnly Property Percents() As Single
			Get
				Return percents_Renamed
			End Get
		End Property
		Protected Overrides Sub ReCalcViewInfoCore(ByVal g As Graphics, ByVal buttons As MouseButtons, ByVal mousePosition As Point, ByVal bounds As Rectangle)
			MyBase.ReCalcViewInfoCore(g, buttons, mousePosition, bounds)
			UpdateProgressInfo(ProgressInfo)
		End Sub
		Public Overrides Sub Reset()
			MyBase.Reset()
			Me.progressInfo_Renamed = CreateInfoArgs()
			Me.position_Renamed = 0
			Me.percents_Renamed = 0
		End Sub
		Public Overridable ReadOnly Property ProgressInfo() As MyObjectInfoArgs
			Get
				Return progressInfo_Renamed
			End Get
		End Property
		Public Shadows ReadOnly Property Item() As MyRepositoryItemProgressBar
			Get
				Return TryCast(MyBase.Item, MyRepositoryItemProgressBar)
			End Get
		End Property
		Protected Friend Overridable Sub UpdateProgressInfo(ByVal info As MyObjectInfoArgs)
			info.Bounds = ContentRect
			info.BarColor = Item.BarColor
			info.BackgroundColor = Item.Appearance.BackColor
			info.Percent = Me.Percents
		End Sub
		Protected Overrides Function GetBorderArgs(ByVal bounds As Rectangle) As ObjectInfoArgs
			Dim pi As New MyObjectInfoArgs()
			pi.Bounds = bounds
			Return pi
		End Function
		Protected Overrides Function GetBorderPainterCore() As BorderPainter
			Dim bp As BorderPainter = MyBase.GetBorderPainterCore()
			If TypeOf bp Is WindowsXPTextBorderPainter Then
				bp = New WindowsXPProgressBarBorderPainter()
			End If
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
		Public Overrides Property EditValue() As Object
			Get
				Return MyBase.EditValue
			End Get
			Set(ByVal value As Object)
				Me.fEditValue = value
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
			If e Is Nothing Then
				Return
			End If
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

