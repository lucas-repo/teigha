<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PDFParams
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
        Me.Browse = New System.Windows.Forms.Button()
        Me.PdfFileName = New System.Windows.Forms.TextBox()
        Me.EmbeddedFonts = New System.Windows.Forms.CheckBox()
        Me.TTypeAsGeometry = New System.Windows.Forms.CheckBox()
        Me.SHXAsGeometry = New System.Windows.Forms.CheckBox()
        Me.SimGeometryOpt = New System.Windows.Forms.CheckBox()
        Me.ZoomToExtents = New System.Windows.Forms.CheckBox()
        Me.UseHLR = New System.Windows.Forms.CheckBox()
        Me.LayerSupport = New System.Windows.Forms.CheckBox()
        Me.OffLayers = New System.Windows.Forms.CheckBox()
        Me.Hyperlinks = New System.Windows.Forms.CheckBox()
        Me.PaperWidth = New System.Windows.Forms.TextBox()
        Me.PaperHeight = New System.Windows.Forms.TextBox()
        Me.ActiveLayout = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.Encoded = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Linearize = New System.Windows.Forms.CheckBox()
        Me.AsciiHex = New System.Windows.Forms.CheckBox()
        Me.EnablePRC = New System.Windows.Forms.CheckBox()
        Me.ExportAsBrep = New System.Windows.Forms.CheckBox()
        Me.EnableSingleViewMode = New System.Windows.Forms.CheckBox()
        Me.PRCCompressionLable = New System.Windows.Forms.Label()
        Me.prcCompressionList = New System.Windows.Forms.ComboBox()
        Me.chbOptimized = New System.Windows.Forms.CheckBox()
        Me.chbMakeSearchable = New System.Windows.Forms.CheckBox()
        Me.chbIgnoreInv = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.colorPolicyList = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.otherHatchesList = New System.Windows.Forms.ComboBox()
        Me.gradientHatchesList = New System.Windows.Forms.ComboBox()
        Me.solidHatchesList = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Title = New System.Windows.Forms.TextBox()
        Me.Author = New System.Windows.Forms.TextBox()
        Me.Subject = New System.Windows.Forms.TextBox()
        Me.Keywords = New System.Windows.Forms.TextBox()
        Me.Creator = New System.Windows.Forms.TextBox()
        Me.Producer = New System.Windows.Forms.TextBox()
        Me.CustomProperties = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 570)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(505, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(63, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(125, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Export"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(316, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(125, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Browse
        '
        Me.Browse.Location = New System.Drawing.Point(458, 4)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(59, 23)
        Me.Browse.TabIndex = 1
        Me.Browse.Text = "Browse"
        Me.Browse.UseVisualStyleBackColor = True
        '
        'PdfFileName
        '
        Me.PdfFileName.Location = New System.Drawing.Point(12, 6)
        Me.PdfFileName.Name = "PdfFileName"
        Me.PdfFileName.Size = New System.Drawing.Size(440, 20)
        Me.PdfFileName.TabIndex = 2
        '
        'EmbeddedFonts
        '
        Me.EmbeddedFonts.AutoSize = True
        Me.EmbeddedFonts.Location = New System.Drawing.Point(12, 32)
        Me.EmbeddedFonts.Name = "EmbeddedFonts"
        Me.EmbeddedFonts.Size = New System.Drawing.Size(103, 17)
        Me.EmbeddedFonts.TabIndex = 3
        Me.EmbeddedFonts.Text = "Embedded fonts"
        Me.EmbeddedFonts.UseVisualStyleBackColor = True
        '
        'TTypeAsGeometry
        '
        Me.TTypeAsGeometry.AutoSize = True
        Me.TTypeAsGeometry.Checked = True
        Me.TTypeAsGeometry.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TTypeAsGeometry.Location = New System.Drawing.Point(12, 55)
        Me.TTypeAsGeometry.Name = "TTypeAsGeometry"
        Me.TTypeAsGeometry.Size = New System.Drawing.Size(131, 17)
        Me.TTypeAsGeometry.TabIndex = 4
        Me.TTypeAsGeometry.Text = "True type as geometry"
        Me.TTypeAsGeometry.UseVisualStyleBackColor = True
        '
        'SHXAsGeometry
        '
        Me.SHXAsGeometry.AutoSize = True
        Me.SHXAsGeometry.Checked = True
        Me.SHXAsGeometry.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SHXAsGeometry.Location = New System.Drawing.Point(12, 78)
        Me.SHXAsGeometry.Name = "SHXAsGeometry"
        Me.SHXAsGeometry.Size = New System.Drawing.Size(128, 17)
        Me.SHXAsGeometry.TabIndex = 5
        Me.SHXAsGeometry.Text = "SHX text as geometry"
        Me.SHXAsGeometry.UseVisualStyleBackColor = True
        '
        'SimGeometryOpt
        '
        Me.SimGeometryOpt.AutoSize = True
        Me.SimGeometryOpt.Location = New System.Drawing.Point(12, 101)
        Me.SimGeometryOpt.Name = "SimGeometryOpt"
        Me.SimGeometryOpt.Size = New System.Drawing.Size(195, 17)
        Me.SimGeometryOpt.TabIndex = 6
        Me.SimGeometryOpt.Text = "Enable simple geometry optimization"
        Me.SimGeometryOpt.UseVisualStyleBackColor = True
        '
        'ZoomToExtents
        '
        Me.ZoomToExtents.AutoSize = True
        Me.ZoomToExtents.Checked = True
        Me.ZoomToExtents.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ZoomToExtents.Location = New System.Drawing.Point(12, 124)
        Me.ZoomToExtents.Name = "ZoomToExtents"
        Me.ZoomToExtents.Size = New System.Drawing.Size(131, 17)
        Me.ZoomToExtents.TabIndex = 7
        Me.ZoomToExtents.Text = "Zoom to extents mode"
        Me.ZoomToExtents.UseVisualStyleBackColor = True
        '
        'UseHLR
        '
        Me.UseHLR.AutoSize = True
        Me.UseHLR.Checked = True
        Me.UseHLR.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UseHLR.Location = New System.Drawing.Point(12, 147)
        Me.UseHLR.Name = "UseHLR"
        Me.UseHLR.Size = New System.Drawing.Size(184, 17)
        Me.UseHLR.TabIndex = 8
        Me.UseHLR.Text = "Use hidden line removal algorithm"
        Me.UseHLR.UseVisualStyleBackColor = True
        '
        'LayerSupport
        '
        Me.LayerSupport.AutoSize = True
        Me.LayerSupport.Location = New System.Drawing.Point(12, 170)
        Me.LayerSupport.Name = "LayerSupport"
        Me.LayerSupport.Size = New System.Drawing.Size(182, 17)
        Me.LayerSupport.TabIndex = 9
        Me.LayerSupport.Text = "Enable layer support (PDF v. 1.5)"
        Me.LayerSupport.UseVisualStyleBackColor = True
        '
        'OffLayers
        '
        Me.OffLayers.AutoSize = True
        Me.OffLayers.Enabled = False
        Me.OffLayers.Location = New System.Drawing.Point(32, 193)
        Me.OffLayers.Name = "OffLayers"
        Me.OffLayers.Size = New System.Drawing.Size(119, 17)
        Me.OffLayers.TabIndex = 10
        Me.OffLayers.Text = "Export off layers too"
        Me.OffLayers.UseVisualStyleBackColor = True
        '
        'Hyperlinks
        '
        Me.Hyperlinks.AutoSize = True
        Me.Hyperlinks.Location = New System.Drawing.Point(12, 285)
        Me.Hyperlinks.Name = "Hyperlinks"
        Me.Hyperlinks.Size = New System.Drawing.Size(108, 17)
        Me.Hyperlinks.TabIndex = 11
        Me.Hyperlinks.Text = "Export Hyperlinks"
        Me.Hyperlinks.UseVisualStyleBackColor = True
        '
        'PaperWidth
        '
        Me.PaperWidth.Location = New System.Drawing.Point(417, 30)
        Me.PaperWidth.MaxLength = 4
        Me.PaperWidth.Name = "PaperWidth"
        Me.PaperWidth.Size = New System.Drawing.Size(100, 20)
        Me.PaperWidth.TabIndex = 12
        Me.PaperWidth.Text = "210"
        '
        'PaperHeight
        '
        Me.PaperHeight.Location = New System.Drawing.Point(417, 56)
        Me.PaperHeight.Name = "PaperHeight"
        Me.PaperHeight.Size = New System.Drawing.Size(100, 20)
        Me.PaperHeight.TabIndex = 13
        Me.PaperHeight.Text = "297"
        '
        'ActiveLayout
        '
        Me.ActiveLayout.AutoSize = True
        Me.ActiveLayout.Checked = True
        Me.ActiveLayout.Location = New System.Drawing.Point(417, 169)
        Me.ActiveLayout.Name = "ActiveLayout"
        Me.ActiveLayout.Size = New System.Drawing.Size(55, 17)
        Me.ActiveLayout.TabIndex = 16
        Me.ActiveLayout.TabStop = True
        Me.ActiveLayout.Text = "Active"
        Me.ActiveLayout.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(481, 169)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(36, 17)
        Me.RadioButton2.TabIndex = 17
        Me.RadioButton2.Text = "All"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'Encoded
        '
        Me.Encoded.AutoSize = True
        Me.Encoded.Checked = True
        Me.Encoded.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Encoded.Location = New System.Drawing.Point(315, 216)
        Me.Encoded.Name = "Encoded"
        Me.Encoded.Size = New System.Drawing.Size(122, 17)
        Me.Encoded.TabIndex = 18
        Me.Encoded.Text = "Encoded (small size)"
        Me.Encoded.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(312, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Paper width, mm"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(312, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Paper height, mm"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(312, 171)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Layouts to export:"
        '
        'Linearize
        '
        Me.Linearize.AutoSize = True
        Me.Linearize.Location = New System.Drawing.Point(12, 308)
        Me.Linearize.Name = "Linearize"
        Me.Linearize.Size = New System.Drawing.Size(148, 17)
        Me.Linearize.TabIndex = 23
        Me.Linearize.Text = "Linearize PDF (Web PDF)"
        Me.Linearize.UseVisualStyleBackColor = True
        '
        'AsciiHex
        '
        Me.AsciiHex.AutoSize = True
        Me.AsciiHex.Location = New System.Drawing.Point(315, 239)
        Me.AsciiHex.Name = "AsciiHex"
        Me.AsciiHex.Size = New System.Drawing.Size(162, 17)
        Me.AsciiHex.TabIndex = 24
        Me.AsciiHex.Text = "ASCIIHEX Encoded (x2 size)"
        Me.AsciiHex.UseVisualStyleBackColor = True
        '
        'EnablePRC
        '
        Me.EnablePRC.AutoSize = True
        Me.EnablePRC.Enabled = False
        Me.EnablePRC.Location = New System.Drawing.Point(12, 216)
        Me.EnablePRC.Name = "EnablePRC"
        Me.EnablePRC.Size = New System.Drawing.Size(84, 17)
        Me.EnablePRC.TabIndex = 25
        Me.EnablePRC.Text = "Enable PRC"
        Me.EnablePRC.UseVisualStyleBackColor = True
        '
        'ExportAsBrep
        '
        Me.ExportAsBrep.AutoSize = True
        Me.ExportAsBrep.Enabled = False
        Me.ExportAsBrep.Location = New System.Drawing.Point(32, 239)
        Me.ExportAsBrep.Name = "ExportAsBrep"
        Me.ExportAsBrep.Size = New System.Drawing.Size(96, 17)
        Me.ExportAsBrep.TabIndex = 26
        Me.ExportAsBrep.Text = "Export As Brep"
        Me.ExportAsBrep.UseVisualStyleBackColor = True
        '
        'EnableSingleViewMode
        '
        Me.EnableSingleViewMode.AutoSize = True
        Me.EnableSingleViewMode.Enabled = False
        Me.EnableSingleViewMode.Location = New System.Drawing.Point(32, 262)
        Me.EnableSingleViewMode.Name = "EnableSingleViewMode"
        Me.EnableSingleViewMode.Size = New System.Drawing.Size(143, 17)
        Me.EnableSingleViewMode.TabIndex = 27
        Me.EnableSingleViewMode.Text = "Enable single view mode"
        Me.EnableSingleViewMode.UseVisualStyleBackColor = True
        '
        'PRCCompressionLable
        '
        Me.PRCCompressionLable.AutoSize = True
        Me.PRCCompressionLable.Location = New System.Drawing.Point(178, 217)
        Me.PRCCompressionLable.Name = "PRCCompressionLable"
        Me.PRCCompressionLable.Size = New System.Drawing.Size(91, 13)
        Me.PRCCompressionLable.TabIndex = 28
        Me.PRCCompressionLable.Text = "PRC compression"
        '
        'prcCompressionList
        '
        Me.prcCompressionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.prcCompressionList.Enabled = False
        Me.prcCompressionList.FormattingEnabled = True
        Me.prcCompressionList.Items.AddRange(New Object() {"No compression", "Loose compression", "Medium compression", "High compression"})
        Me.prcCompressionList.Location = New System.Drawing.Point(181, 235)
        Me.prcCompressionList.Name = "prcCompressionList"
        Me.prcCompressionList.Size = New System.Drawing.Size(99, 21)
        Me.prcCompressionList.TabIndex = 29
        '
        'chbOptimized
        '
        Me.chbOptimized.AutoSize = True
        Me.chbOptimized.Enabled = False
        Me.chbOptimized.Location = New System.Drawing.Point(121, 32)
        Me.chbOptimized.Name = "chbOptimized"
        Me.chbOptimized.Size = New System.Drawing.Size(70, 17)
        Me.chbOptimized.TabIndex = 30
        Me.chbOptimized.Text = "optimized"
        Me.chbOptimized.UseVisualStyleBackColor = True
        '
        'chbMakeSearchable
        '
        Me.chbMakeSearchable.AutoSize = True
        Me.chbMakeSearchable.Location = New System.Drawing.Point(146, 69)
        Me.chbMakeSearchable.Name = "chbMakeSearchable"
        Me.chbMakeSearchable.Size = New System.Drawing.Size(128, 17)
        Me.chbMakeSearchable.TabIndex = 31
        Me.chbMakeSearchable.Text = "Make text searchable"
        Me.chbMakeSearchable.UseVisualStyleBackColor = True
        '
        'chbIgnoreInv
        '
        Me.chbIgnoreInv.AutoSize = True
        Me.chbIgnoreInv.Location = New System.Drawing.Point(315, 262)
        Me.chbIgnoreInv.Name = "chbIgnoreInv"
        Me.chbIgnoreInv.Size = New System.Drawing.Size(177, 17)
        Me.chbIgnoreInv.TabIndex = 32
        Me.chbIgnoreInv.Text = "Ignore invisible viewport borders"
        Me.chbIgnoreInv.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(312, 289)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 13)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Color policy:"
        '
        'colorPolicyList
        '
        Me.colorPolicyList.FormattingEnabled = True
        Me.colorPolicyList.Items.AddRange(New Object() {"As Is", "Monochrome", "GrayScale", "Use mono plotstyle"})
        Me.colorPolicyList.Location = New System.Drawing.Point(393, 283)
        Me.colorPolicyList.Name = "colorPolicyList"
        Me.colorPolicyList.Size = New System.Drawing.Size(124, 21)
        Me.colorPolicyList.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 344)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(133, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Export solid hatches using:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(381, 344)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(136, 13)
        Me.Label8.TabIndex = 36
        Me.Label8.Text = "Export other hatches using:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(190, 344)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(150, 13)
        Me.Label9.TabIndex = 37
        Me.Label9.Text = "Export gradient hatches using:"
        '
        'otherHatchesList
        '
        Me.otherHatchesList.FormattingEnabled = True
        Me.otherHatchesList.Items.AddRange(New Object() {"bitmap export", "vectorizer"})
        Me.otherHatchesList.Location = New System.Drawing.Point(384, 360)
        Me.otherHatchesList.Name = "otherHatchesList"
        Me.otherHatchesList.Size = New System.Drawing.Size(133, 21)
        Me.otherHatchesList.TabIndex = 38
        '
        'gradientHatchesList
        '
        Me.gradientHatchesList.FormattingEnabled = True
        Me.gradientHatchesList.Items.AddRange(New Object() {"bitmap export", "vectorizer"})
        Me.gradientHatchesList.Location = New System.Drawing.Point(193, 360)
        Me.gradientHatchesList.Name = "gradientHatchesList"
        Me.gradientHatchesList.Size = New System.Drawing.Size(147, 21)
        Me.gradientHatchesList.TabIndex = 39
        '
        'solidHatchesList
        '
        Me.solidHatchesList.FormattingEnabled = True
        Me.solidHatchesList.Items.AddRange(New Object() {"bitmap export", "vectorizer", "PDF paths"})
        Me.solidHatchesList.Location = New System.Drawing.Point(12, 360)
        Me.solidHatchesList.Name = "solidHatchesList"
        Me.solidHatchesList.Size = New System.Drawing.Size(128, 21)
        Me.solidHatchesList.TabIndex = 40
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 397)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(30, 13)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Title:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(9, 423)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(41, 13)
        Me.Label11.TabIndex = 42
        Me.Label11.Text = "Author:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(9, 449)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 13)
        Me.Label12.TabIndex = 43
        Me.Label12.Text = "Subject:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(9, 475)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 13)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Keywords:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(9, 501)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(44, 13)
        Me.Label14.TabIndex = 45
        Me.Label14.Text = "Creator:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(9, 527)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(53, 13)
        Me.Label15.TabIndex = 46
        Me.Label15.Text = "Producer:"
        '
        'Title
        '
        Me.Title.Location = New System.Drawing.Point(73, 394)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(444, 20)
        Me.Title.TabIndex = 48
        '
        'Author
        '
        Me.Author.Location = New System.Drawing.Point(73, 420)
        Me.Author.Name = "Author"
        Me.Author.Size = New System.Drawing.Size(444, 20)
        Me.Author.TabIndex = 49
        '
        'Subject
        '
        Me.Subject.Location = New System.Drawing.Point(73, 446)
        Me.Subject.Name = "Subject"
        Me.Subject.Size = New System.Drawing.Size(444, 20)
        Me.Subject.TabIndex = 50
        '
        'Keywords
        '
        Me.Keywords.Location = New System.Drawing.Point(73, 472)
        Me.Keywords.Name = "Keywords"
        Me.Keywords.Size = New System.Drawing.Size(444, 20)
        Me.Keywords.TabIndex = 51
        '
        'Creator
        '
        Me.Creator.Location = New System.Drawing.Point(73, 498)
        Me.Creator.Name = "Creator"
        Me.Creator.Size = New System.Drawing.Size(444, 20)
        Me.Creator.TabIndex = 52
        '
        'Producer
        '
        Me.Producer.Location = New System.Drawing.Point(73, 524)
        Me.Producer.Name = "Producer"
        Me.Producer.Size = New System.Drawing.Size(444, 20)
        Me.Producer.TabIndex = 53
        '
        'CustomProperties
        '
        Me.CustomProperties.Location = New System.Drawing.Point(417, 118)
        Me.CustomProperties.Name = "CustomProperties"
        Me.CustomProperties.Size = New System.Drawing.Size(100, 23)
        Me.CustomProperties.TabIndex = 54
        Me.CustomProperties.Text = "Custom properties"
        Me.CustomProperties.UseVisualStyleBackColor = True
        '
        'PDFParams
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(529, 611)
        Me.Controls.Add(Me.CustomProperties)
        Me.Controls.Add(Me.Producer)
        Me.Controls.Add(Me.Creator)
        Me.Controls.Add(Me.Keywords)
        Me.Controls.Add(Me.Subject)
        Me.Controls.Add(Me.Author)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.solidHatchesList)
        Me.Controls.Add(Me.gradientHatchesList)
        Me.Controls.Add(Me.otherHatchesList)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.colorPolicyList)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.chbIgnoreInv)
        Me.Controls.Add(Me.chbMakeSearchable)
        Me.Controls.Add(Me.chbOptimized)
        Me.Controls.Add(Me.prcCompressionList)
        Me.Controls.Add(Me.PRCCompressionLable)
        Me.Controls.Add(Me.EnableSingleViewMode)
        Me.Controls.Add(Me.ExportAsBrep)
        Me.Controls.Add(Me.EnablePRC)
        Me.Controls.Add(Me.AsciiHex)
        Me.Controls.Add(Me.Linearize)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Encoded)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.ActiveLayout)
        Me.Controls.Add(Me.PaperHeight)
        Me.Controls.Add(Me.PaperWidth)
        Me.Controls.Add(Me.Hyperlinks)
        Me.Controls.Add(Me.OffLayers)
        Me.Controls.Add(Me.LayerSupport)
        Me.Controls.Add(Me.UseHLR)
        Me.Controls.Add(Me.ZoomToExtents)
        Me.Controls.Add(Me.SimGeometryOpt)
        Me.Controls.Add(Me.SHXAsGeometry)
        Me.Controls.Add(Me.TTypeAsGeometry)
        Me.Controls.Add(Me.EmbeddedFonts)
        Me.Controls.Add(Me.PdfFileName)
        Me.Controls.Add(Me.Browse)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PDFParams"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "PDFParams"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Browse As System.Windows.Forms.Button
    Friend WithEvents PdfFileName As System.Windows.Forms.TextBox
    Friend WithEvents EmbeddedFonts As System.Windows.Forms.CheckBox
    Friend WithEvents TTypeAsGeometry As System.Windows.Forms.CheckBox
    Friend WithEvents SHXAsGeometry As System.Windows.Forms.CheckBox
    Friend WithEvents SimGeometryOpt As System.Windows.Forms.CheckBox
    Friend WithEvents ZoomToExtents As System.Windows.Forms.CheckBox
    Friend WithEvents UseHLR As System.Windows.Forms.CheckBox
    Friend WithEvents LayerSupport As System.Windows.Forms.CheckBox
    Friend WithEvents OffLayers As System.Windows.Forms.CheckBox
    Friend WithEvents Hyperlinks As System.Windows.Forms.CheckBox
    Friend WithEvents PaperWidth As System.Windows.Forms.TextBox
    Friend WithEvents PaperHeight As System.Windows.Forms.TextBox
    Friend WithEvents ActiveLayout As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents Encoded As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Linearize As System.Windows.Forms.CheckBox
    Friend WithEvents AsciiHex As System.Windows.Forms.CheckBox
    Friend WithEvents EnablePRC As System.Windows.Forms.CheckBox
    Friend WithEvents ExportAsBrep As System.Windows.Forms.CheckBox
    Friend WithEvents EnableSingleViewMode As System.Windows.Forms.CheckBox
    Friend WithEvents PRCCompressionLable As System.Windows.Forms.Label
    Friend WithEvents prcCompressionList As System.Windows.Forms.ComboBox
    Friend WithEvents chbOptimized As System.Windows.Forms.CheckBox
    Friend WithEvents chbMakeSearchable As System.Windows.Forms.CheckBox
    Friend WithEvents chbIgnoreInv As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents colorPolicyList As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents otherHatchesList As System.Windows.Forms.ComboBox
    Friend WithEvents gradientHatchesList As System.Windows.Forms.ComboBox
    Friend WithEvents solidHatchesList As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Title As System.Windows.Forms.TextBox
    Friend WithEvents Author As System.Windows.Forms.TextBox
    Friend WithEvents Subject As System.Windows.Forms.TextBox
    Friend WithEvents Keywords As System.Windows.Forms.TextBox
    Friend WithEvents Creator As System.Windows.Forms.TextBox
    Friend WithEvents Producer As System.Windows.Forms.TextBox
    Friend WithEvents CustomProperties As System.Windows.Forms.Button

End Class
