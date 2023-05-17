Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace WindowsApplication41

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            For i As Integer = 0 To 6 - 1
                dataTable1.Rows.Add(New Object() {i * 20})
            Next

            myProgressBarControl1.BackColor = Color.Tomato
            myRepositoryItemProgressBar1.BarColor = Color.Thistle
            myRepositoryItemProgressBar1.Appearance.BackColor = Color.Teal
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
            myProgressBarControl1.EditValue = CInt(myProgressBarControl1.EditValue) + 10
        End Sub
    End Class
End Namespace
