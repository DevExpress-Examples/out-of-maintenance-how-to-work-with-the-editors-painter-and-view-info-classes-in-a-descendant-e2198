Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace WindowsApplication41
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			For i As Integer = 0 To 5
				dataTable1.Rows.Add(New Object() { i*20})
			Next i
			myProgressBarControl1.BackColor = Color.Tomato
			myRepositoryItemProgressBar1.BarColor = Color.Thistle
			myRepositoryItemProgressBar1.Appearance.BackColor = Color.Teal
		End Sub

		Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
			myProgressBarControl1.EditValue = CInt(Fix(myProgressBarControl1.EditValue)) + 10
		End Sub
	End Class
End Namespace