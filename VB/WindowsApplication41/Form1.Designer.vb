Namespace WindowsApplication41

    Partial Class Form1

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (Me.components IsNot Nothing) Then
                Me.components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.simpleButton1 = New DevExpress.XtraEditors.SimpleButton()
            Me.myProgressBarControl1 = New MyProgressBarControl()
            Me.gridControl1 = New DevExpress.XtraGrid.GridControl()
            Me.dataSet1 = New System.Data.DataSet()
            Me.dataTable1 = New System.Data.DataTable()
            Me.dataColumn1 = New System.Data.DataColumn()
            Me.gridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.colColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.myRepositoryItemProgressBar1 = New MyRepositoryItemProgressBar()
            CType((Me.myProgressBarControl1.Properties), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.gridControl1), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.dataSet1), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.dataTable1), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.gridView1), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.myRepositoryItemProgressBar1), System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' simpleButton1
            ' 
            Me.simpleButton1.Location = New System.Drawing.Point(468, 254)
            Me.simpleButton1.Name = "simpleButton1"
            Me.simpleButton1.Size = New System.Drawing.Size(106, 23)
            Me.simpleButton1.TabIndex = 0
            Me.simpleButton1.Text = "ChangeEditValue"
            AddHandler Me.simpleButton1.Click, New System.EventHandler(AddressOf Me.simpleButton1_Click)
            ' 
            ' myProgressBarControl1
            ' 
            Me.myProgressBarControl1.Location = New System.Drawing.Point(474, 177)
            Me.myProgressBarControl1.Name = "myProgressBarControl1"
            Me.myProgressBarControl1.Properties.AutoHeight = False
            Me.myProgressBarControl1.Properties.BarColor = System.Drawing.Color.YellowGreen
            Me.myProgressBarControl1.Size = New System.Drawing.Size(100, 18)
            Me.myProgressBarControl1.TabIndex = 1
            Me.myProgressBarControl1.TabStop = False
            ' 
            ' gridControl1
            ' 
            Me.gridControl1.DataMember = "Table1"
            Me.gridControl1.DataSource = Me.dataSet1
            Me.gridControl1.Location = New System.Drawing.Point(28, 21)
            Me.gridControl1.MainView = Me.gridView1
            Me.gridControl1.Name = "gridControl1"
            Me.gridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.myRepositoryItemProgressBar1})
            Me.gridControl1.Size = New System.Drawing.Size(402, 304)
            Me.gridControl1.TabIndex = 2
            Me.gridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridView1})
            ' 
            ' dataSet1
            ' 
            Me.dataSet1.DataSetName = "NewDataSet"
            Me.dataSet1.Tables.AddRange(New System.Data.DataTable() {Me.dataTable1})
            ' 
            ' dataTable1
            ' 
            Me.dataTable1.Columns.AddRange(New System.Data.DataColumn() {Me.dataColumn1})
            Me.dataTable1.TableName = "Table1"
            ' 
            ' dataColumn1
            ' 
            Me.dataColumn1.ColumnName = "Column1"
            Me.dataColumn1.DataType = GetType(Integer)
            ' 
            ' gridView1
            ' 
            Me.gridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colColumn1})
            Me.gridView1.GridControl = Me.gridControl1
            Me.gridView1.Name = "gridView1"
            ' 
            ' colColumn1
            ' 
            Me.colColumn1.Caption = "Column1"
            Me.colColumn1.ColumnEdit = Me.myRepositoryItemProgressBar1
            Me.colColumn1.FieldName = "Column1"
            Me.colColumn1.Name = "colColumn1"
            Me.colColumn1.Visible = True
            Me.colColumn1.VisibleIndex = 0
            ' 
            ' myRepositoryItemProgressBar1
            ' 
            Me.myRepositoryItemProgressBar1.AutoHeight = False
            Me.myRepositoryItemProgressBar1.BarColor = System.Drawing.Color.YellowGreen
            Me.myRepositoryItemProgressBar1.Name = "myRepositoryItemProgressBar1"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(661, 381)
            Me.Controls.Add(Me.gridControl1)
            Me.Controls.Add(Me.myProgressBarControl1)
            Me.Controls.Add(Me.simpleButton1)
            Me.Name = "Form1"
            Me.Text = "Form1"
            AddHandler Me.Load, New System.EventHandler(AddressOf Me.Form1_Load)
            CType((Me.myProgressBarControl1.Properties), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.gridControl1), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.dataSet1), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.dataTable1), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.gridView1), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.myRepositoryItemProgressBar1), System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

#End Region
        Private simpleButton1 As DevExpress.XtraEditors.SimpleButton

        Private myProgressBarControl1 As MyProgressBarControl

        Private gridControl1 As DevExpress.XtraGrid.GridControl

        Private dataSet1 As System.Data.DataSet

        Private dataTable1 As System.Data.DataTable

        Private dataColumn1 As System.Data.DataColumn

        Private gridView1 As DevExpress.XtraGrid.Views.Grid.GridView

        Private colColumn1 As DevExpress.XtraGrid.Columns.GridColumn

        Private myRepositoryItemProgressBar1 As MyRepositoryItemProgressBar
    End Class
End Namespace
