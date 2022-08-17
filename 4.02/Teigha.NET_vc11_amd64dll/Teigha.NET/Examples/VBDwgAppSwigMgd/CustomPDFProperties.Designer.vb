<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomPDFProperties
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.hatchResolution = New System.Windows.Forms.TextBox()
        Me.vectorResolution = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.monochromeResolution = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.imagesResolution = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.imageQuality = New System.Windows.Forms.TrackBar()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chbImageCompression = New System.Windows.Forms.CheckBox()
        Me.chbCropInvisible = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.pdfControl = New System.Windows.Forms.ComboBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.mergeControl = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.imageQuality, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 273)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(510, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(65, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(125, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(320, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(125, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.hatchResolution)
        Me.GroupBox1.Controls.Add(Me.vectorResolution)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(236, 80)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vector and hatch resolution"
        '
        'hatchResolution
        '
        Me.hatchResolution.Location = New System.Drawing.Point(121, 52)
        Me.hatchResolution.Name = "hatchResolution"
        Me.hatchResolution.Size = New System.Drawing.Size(100, 20)
        Me.hatchResolution.TabIndex = 3
        Me.hatchResolution.Text = "150"
        '
        'vectorResolution
        '
        Me.vectorResolution.Location = New System.Drawing.Point(121, 26)
        Me.vectorResolution.Name = "vectorResolution"
        Me.vectorResolution.Size = New System.Drawing.Size(100, 20)
        Me.vectorResolution.TabIndex = 2
        Me.vectorResolution.Text = "600"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Hatch to BMP, dpi:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Vector resolution, dpi:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.monochromeResolution)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.imagesResolution)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(263, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(257, 80)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Image resolution"
        '
        'monochromeResolution
        '
        Me.monochromeResolution.Location = New System.Drawing.Point(144, 52)
        Me.monochromeResolution.Name = "monochromeResolution"
        Me.monochromeResolution.Size = New System.Drawing.Size(100, 20)
        Me.monochromeResolution.TabIndex = 7
        Me.monochromeResolution.Text = "400"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Color images, dpi:"
        '
        'imagesResolution
        '
        Me.imagesResolution.Location = New System.Drawing.Point(144, 26)
        Me.imagesResolution.Name = "imagesResolution"
        Me.imagesResolution.Size = New System.Drawing.Size(100, 20)
        Me.imagesResolution.TabIndex = 6
        Me.imagesResolution.Text = "400"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Monochrome images, dpi:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.imageQuality)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.chbImageCompression)
        Me.GroupBox3.Controls.Add(Me.chbCropInvisible)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 108)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(508, 80)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Other image properties"
        '
        'imageQuality
        '
        Me.imageQuality.LargeChange = 10
        Me.imageQuality.Location = New System.Drawing.Point(260, 35)
        Me.imageQuality.Maximum = 100
        Me.imageQuality.Minimum = 10
        Me.imageQuality.Name = "imageQuality"
        Me.imageQuality.Size = New System.Drawing.Size(235, 45)
        Me.imageQuality.TabIndex = 7
        Me.imageQuality.Value = 50
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(346, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Image quality:"
        '
        'chbImageCompression
        '
        Me.chbImageCompression.AutoSize = True
        Me.chbImageCompression.Checked = True
        Me.chbImageCompression.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chbImageCompression.Location = New System.Drawing.Point(9, 54)
        Me.chbImageCompression.Name = "chbImageCompression"
        Me.chbImageCompression.Size = New System.Drawing.Size(170, 17)
        Me.chbImageCompression.TabIndex = 5
        Me.chbImageCompression.Text = "Additional images compression"
        Me.chbImageCompression.UseVisualStyleBackColor = True
        '
        'chbCropInvisible
        '
        Me.chbCropInvisible.AutoSize = True
        Me.chbCropInvisible.Checked = True
        Me.chbCropInvisible.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chbCropInvisible.Location = New System.Drawing.Point(9, 28)
        Me.chbCropInvisible.Name = "chbCropInvisible"
        Me.chbCropInvisible.Size = New System.Drawing.Size(162, 17)
        Me.chbCropInvisible.TabIndex = 4
        Me.chbCropInvisible.Text = "Crop invisible parts of images"
        Me.chbCropInvisible.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.pdfControl)
        Me.GroupBox4.Location = New System.Drawing.Point(263, 203)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(257, 52)
        Me.GroupBox4.TabIndex = 5
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "PDF/A"
        '
        'pdfControl
        '
        Me.pdfControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.pdfControl.FormattingEnabled = True
        Me.pdfControl.Items.AddRange(New Object() {"None", "PDF/A-1b"})
        Me.pdfControl.Location = New System.Drawing.Point(9, 19)
        Me.pdfControl.Name = "pdfControl"
        Me.pdfControl.Size = New System.Drawing.Size(235, 21)
        Me.pdfControl.TabIndex = 0
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.mergeControl)
        Me.GroupBox5.Location = New System.Drawing.Point(12, 203)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(236, 52)
        Me.GroupBox5.TabIndex = 4
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Merge Control"
        '
        'mergeControl
        '
        Me.mergeControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mergeControl.FormattingEnabled = True
        Me.mergeControl.Items.AddRange(New Object() {"Lines Merge", "Lines Overwrite"})
        Me.mergeControl.Location = New System.Drawing.Point(9, 19)
        Me.mergeControl.Name = "mergeControl"
        Me.mergeControl.Size = New System.Drawing.Size(212, 21)
        Me.mergeControl.TabIndex = 0
        '
        'CustomPDFProperties
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(534, 314)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomPDFProperties"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Custom PDF Properties"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.imageQuality, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents hatchResolution As System.Windows.Forms.TextBox
    Friend WithEvents vectorResolution As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents monochromeResolution As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents imagesResolution As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents imageQuality As System.Windows.Forms.TrackBar
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chbImageCompression As System.Windows.Forms.CheckBox
    Friend WithEvents chbCropInvisible As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents pdfControl As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents mergeControl As System.Windows.Forms.ComboBox

End Class
