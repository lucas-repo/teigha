Imports System.Windows.Forms

Public Class PDFParams
    Public m_bEmbedded As Boolean = False
    Public m_bOptimized As Boolean = False 'new
    Public m_bTTFAsGeometry As Boolean = True
    Public m_bSHXAsGeometry As Boolean = True
    Public m_bTextSearchable As Boolean = False 'new
    Public m_bSimpleGeomOpt As Boolean = False
    Public m_bZoomToExtents As Boolean = True
    Public m_bUseHLR As Boolean = True
    Public m_bEnableLayers As Boolean = False
    Public m_bExportOffLayers As Boolean = False
    Public m_bEnablePRC As Boolean = False ' new 
    Public m_bAsBrep As Boolean = False 'new
    Public m_bSingleView As Boolean = False 'new
    Public ui_Compression As UInt16 = 0 'new
    Public m_bExportHyperlinks As Boolean = False
    Public m_bLinearize As Boolean = False 'new


    Public m_dPaperW As Double = 210
    Public m_dPaperH As Double = 297

    Public m_Layouts As Boolean = False
    Public m_bEncoded As Boolean = True
    Public m_bAsciiHex As Boolean = False ' new
    Public m_bIgnoreInvisible As Boolean = False 'new
    Public ui_ColorPolicy As UInt16 = 0 'new
    Public ui_SolidHatches As UInt16 = 2 ' new
    Public ui_GradientHatches As UInt16 = 0 'new
    Public ui_OtherHatches As UInt16 = 0 'new

    Public FileName As String

    Public str_Title As String 'new
    Public str_Author As String 'new
    Public str_Subject As String 'new
    Public str_Keywords As String 'new
    Public str_Creator As String ' new
    Public str_Producer As String 'new

    ' custom pdf params
    Private custPropsDlg As CustomPDFProperties
    Public vectorRes As UInt16 = 600
    Public hatchRes As UInt16 = 150
    Public imagesRes As UInt16 = 400
    Public monochromeRes As UInt16 = 400

    Public bCropInvisible As Boolean = True
    Public bAdditionalCompression As Boolean = True

    Public imageQ As UInt16 = 50

    Public mergeCtrl As UInt16 = 0
    Public pdfaCtrl As UInt16 = 0

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EmbeddedFonts.Checked = m_bEmbedded

        TTypeAsGeometry.Checked = m_bTTFAsGeometry
        SHXAsGeometry.Checked = m_bSHXAsGeometry
        SimGeometryOpt.Checked = m_bSimpleGeomOpt
        ZoomToExtents.Checked = m_bZoomToExtents
        UseHLR.Checked = m_bUseHLR
        LayerSupport.Checked = m_bEnableLayers
        OffLayers.Checked = m_bExportOffLayers

        Hyperlinks.Checked = m_bExportHyperlinks

        PaperWidth.Text = m_dPaperW.ToString()
        PaperHeight.Text = m_dPaperH.ToString()

        ActiveLayout.Checked = Not m_Layouts
        Encoded.Checked = m_bEncoded

        chbOptimized.Checked = m_bOptimized 'new
        chbMakeSearchable.Checked = m_bTextSearchable 'new
        EnablePRC.Checked = m_bEnablePRC 'new
        ExportAsBrep.Checked = m_bAsBrep 'new
        EnableSingleViewMode.Checked = m_bSingleView 'new
        prcCompressionList.SelectedIndex = ui_Compression 'new
        Linearize.Checked = m_bLinearize 'new

        AsciiHex.Checked = m_bAsciiHex ' new
        chbIgnoreInv.Checked = m_bIgnoreInvisible ' new
        colorPolicyList.SelectedIndex = ui_ColorPolicy 'new
        solidHatchesList.SelectedIndex = ui_SolidHatches ' new
        gradientHatchesList.SelectedIndex = ui_GradientHatches 'new
        otherHatchesList.SelectedIndex = ui_OtherHatches 'new

        Title.Text = str_Title 'new
        Author.Text = str_Author 'new
        Subject.Text = str_Subject 'new
        Keywords.Text = str_Keywords 'new
        Creator.Text = str_Creator ' new
        Producer.Text = str_Producer 'new

        ' custom pdf properties
        custPropsDlg = New CustomPDFProperties()
        'custPropsDlg.vectorRes = vectorRes
        'custPropsDlg.hatchRes = hatchRes
        'custPropsDlg.imagesRes = imagesRes
        'custPropsDlg.monochromeRes = monochromeRes

        'custPropsDlg.bCropInvisible = bCropInvisible
        'custPropsDlg.bAdditionalCompression = bAdditionalCompression

        'custPropsDlg.imageQ = imageQ

        'custPropsDlg.mergeCtrl = mergeCtrl
        'custPropsDlg.pdfaCtrl = pdfaCtrl
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        m_bEmbedded = EmbeddedFonts.Checked
        m_bTTFAsGeometry = TTypeAsGeometry.Checked
        m_bSHXAsGeometry = SHXAsGeometry.Checked
        m_bSimpleGeomOpt = SimGeometryOpt.Checked
        m_bZoomToExtents = ZoomToExtents.Checked
        m_bUseHLR = UseHLR.Checked
        m_bEnableLayers = LayerSupport.Checked
        m_bExportOffLayers = OffLayers.Checked
        m_bExportHyperlinks = Hyperlinks.Checked

        m_dPaperW = Convert.ToDouble(PaperWidth.Text)
        m_dPaperH = Convert.ToDouble(PaperHeight.Text)

        m_Layouts = Not ActiveLayout.Checked
        m_bEncoded = Encoded.Checked

        m_bOptimized = chbOptimized.Checked 'new
        m_bTextSearchable = chbMakeSearchable.Checked 'new
        m_bEnablePRC = EnablePRC.Checked 'new
        m_bAsBrep = ExportAsBrep.Checked 'new
        m_bSingleView = EnableSingleViewMode.Checked 'new
        ui_Compression = prcCompressionList.SelectedIndex 'new
        m_bLinearize = Linearize.Checked 'new

        m_bAsciiHex = AsciiHex.Checked ' new
        m_bIgnoreInvisible = chbIgnoreInv.Checked ' new
        ui_ColorPolicy = colorPolicyList.SelectedIndex 'new
        ui_SolidHatches = solidHatchesList.SelectedIndex ' new
        ui_GradientHatches = gradientHatchesList.SelectedIndex 'new
        ui_OtherHatches = otherHatchesList.SelectedIndex 'new

        str_Title = Title.Text 'new
        str_Author = Author.Text 'new
        str_Subject = Subject.Text 'new
        str_Keywords = Keywords.Text 'new
        str_Creator = Creator.Text ' new
        str_Producer = Producer.Text 'new

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Browse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse.Click
        Dim saveDlg As New SaveFileDialog()
        saveDlg.Filter = "pdf files (*.pdf)|*pdf"
        If saveDlg.ShowDialog() = DialogResult.OK Then
            FileName = saveDlg.FileName
            PdfFileName.Text = saveDlg.FileName
        End If
    End Sub

    Private Sub CustomProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomProperties.Click
        If custPropsDlg.ShowDialog() = DialogResult.OK Then
            ' custom pdf properties
            vectorRes = custPropsDlg.vectorRes
            hatchRes = custPropsDlg.hatchRes
            imagesRes = custPropsDlg.imagesRes
            monochromeRes = custPropsDlg.monochromeRes

            bCropInvisible = custPropsDlg.bCropInvisible
            bAdditionalCompression = custPropsDlg.bAdditionalCompression

            imageQ = custPropsDlg.imageQ

            mergeCtrl = custPropsDlg.mergeCtrl
            pdfaCtrl = custPropsDlg.pdfaCtrl
        End If
    End Sub

    Private Sub EmbeddedFonts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmbeddedFonts.CheckedChanged
        chbOptimized.Enabled = EmbeddedFonts.Checked
    End Sub

    Private Sub UseHLR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseHLR.CheckedChanged
        EnablePRC.Enabled = Not UseHLR.Checked
    End Sub

    Private Sub LayerSupport_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LayerSupport.CheckedChanged
        OffLayers.Enabled = LayerSupport.Checked
    End Sub

    Private Sub EnablePRC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnablePRC.CheckedChanged
        UseHLR.Enabled = Not EnablePRC.Checked
        PRCCompressionLable.Enabled = EnablePRC.Checked
        prcCompressionList.Enabled = EnablePRC.Checked
        ExportAsBrep.Enabled = EnablePRC.Checked
        EnableSingleViewMode.Enabled = EnablePRC.Checked
    End Sub
End Class
