Imports System

Imports Teigha.Core
Imports Teigha.TD


Public Class Form1
    Private sysServ As ExSystemServices
    Private hostApp As ExHostAppServices
    Private pDb As OdDbDatabase
    Private helperDevice As OdGsLayoutHelper

    Private Views As New List(Of DisplayControl)()
    Private viewCtrl As DisplayControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        sysServ = New ExSystemServices()
        hostApp = New ExHostAppServices()
        TD_Db.odInitialize(sysServ)
        Globals.odgsInitialize()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim openDlg As New OpenFileDialog()
        openDlg.Filter = "dwg files (*.dwg)|*.dwg"
        If openDlg.ShowDialog() = DialogResult.OK Then
            pDb = hostApp.readFile(openDlg.FileName)
            TableLayoutPanel1.RowCount = 1
            TableLayoutPanel1.ColumnCount = 1
            viewCtrl = New DisplayControl()
            viewCtrl.TDDatabase = pDb
            viewCtrl.ResetDevice()
            viewCtrl.Dock = DockStyle.Fill
            viewCtrl.Margin = New Padding(1)
            TableLayoutPanel1.Controls.Add(viewCtrl)
            ExportToPdf.Enabled = True
        End If

    End Sub
    Private Sub FillLayouts(ByRef aLayouts As OdStringArray, ByVal bAllLayouts As Boolean)
        If True = bAllLayouts Then
            Dim pLayoutDict As OdDbDictionary = pDb.getLayoutDictionaryId().safeOpenObject()
            Dim pIter As OdDbDictionaryIterator = pLayoutDict.newIterator()
            While False = pIter.done()
                Dim id As OdDbObjectId = pIter.objectId()
                Dim pLayout As OdDbLayout = id.safeOpenObject()
                If Nothing Is pLayout Then
                    Continue While
                End If

                pIter.next()
            End While
        End If
        If False = bAllLayouts Then

        End If
    End Sub

    Private Sub ExportToPdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToPdf.Click
        Dim dlgPdf As New PDFParams()
        Dim Params As New PDFExportParams()
        Dim pParams As New OdGsPageParams()
        'dlgPdf.m_dPaperH = pParams.getPaperHeight()
        'dlgPdf.m_dPaperW = pParams.getPaperWidth()
        If Not dlgPdf.ShowDialog() = DialogResult.OK Then
            Return
        End If

        If Not dlgPdf.m_bEnableLayers Then
            dlgPdf.m_bExportOffLayers = False
        End If

        Dim arrLayouts As New OdStringArray()
        FillLayouts(arrLayouts, dlgPdf.m_Layouts)
        Params.setLayouts(arrLayouts)

        Dim ppArr As OdGsPageParamsArray
        ppArr = Params.pageParams()
        Dim len As UInt32
        len = arrLayouts.Count
        If 0 = len Then
            len = 1
        End If
        ppArr.resize(len)
        Params.setPageParams(ppArr)

        Dim bV15 As Boolean
        bV15 = dlgPdf.m_bEnableLayers Or dlgPdf.m_bExportOffLayers

        Params.setDatabase(pDb)
        If bV15 = True Then
            Params.setVersion(PDFExportParams.PDFExportVersions.kPDFv1_5)
        End If
        If bV15 = False Then
            Params.setVersion(PDFExportParams.PDFExportVersions.kPDFv1_4)
        End If

        Dim file As OdStreamBuf
        file = Globals.odrxSystemServices().createFile(dlgPdf.FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways)
        Params.setOutput(file)
        Dim flags As PDFExportParams.PDFExportFlags
        flags = 0
        If dlgPdf.m_bEmbedded = True Then
            flags = PDFExportParams.PDFExportFlags.kEmbededTTF Or flags
        End If
        If dlgPdf.m_bSHXAsGeometry = True Then
            flags = PDFExportParams.PDFExportFlags.kSHXTextAsGeometry Or flags
        End If
        If dlgPdf.m_bTTFAsGeometry = True Then
            flags = PDFExportParams.PDFExportFlags.kTTFTextAsGeometry Or flags
        End If
        If dlgPdf.m_bSimpleGeomOpt = True Then
            flags = PDFExportParams.PDFExportFlags.kSimpleGeomOptimization Or flags
        End If
        If dlgPdf.m_bZoomToExtents = True Then
            flags = PDFExportParams.PDFExportFlags.kZoomToExtentsMode Or flags
        End If
        If dlgPdf.m_bEnableLayers = True Then
            flags = PDFExportParams.PDFExportFlags.kEnableLayers Or flags
        End If
        If dlgPdf.m_bExportOffLayers = True Then
            flags = PDFExportParams.PDFExportFlags.kIncludeOffLayers Or flags
        End If
        If dlgPdf.m_bUseHLR = True Then
            flags = PDFExportParams.PDFExportFlags.kUseHLR Or flags
        End If
        If dlgPdf.m_bEncoded = True Then
            flags = PDFExportParams.PDFExportFlags.kFlateCompression Or flags
        End If
        If dlgPdf.m_bEncoded = True Then
            flags = PDFExportParams.PDFExportFlags.kASCIIHexEncoding Or flags
        End If
        If dlgPdf.m_bExportHyperlinks = True Then
            flags = PDFExportParams.PDFExportFlags.kExportHyperlinks Or flags
        End If
        If dlgPdf.m_bOptimized = True Then
            flags = PDFExportParams.PDFExportFlags.kEmbededOptimizedTTF Or flags
        End If
        If dlgPdf.m_bAsciiHex = True Then
            flags = PDFExportParams.PDFExportFlags.kASCIIHexEncoding Or flags
        End If
        If dlgPdf.m_bLinearize = True Then
            flags = PDFExportParams.PDFExportFlags.kLinearized Or flags
        End If
        '(m_bMergeLinesCrossing && m_iPdfAMode == (unsigned int)PDFExportParams::kPDFA_None ? PDFExportParams::kMergeLines : 0) |
        If dlgPdf.m_bLinearize = True And dlgPdf.pdfaCtrl = 0 Then
            flags = PDFExportParams.PDFExportFlags.kMergeLines Or flags
        End If


        Params.setExportFlags(flags)


        Params.setTitle(dlgPdf.str_Title)
        Params.setAuthor(dlgPdf.str_Author)
        Params.setSubject(dlgPdf.str_Subject)
        Params.setKeywords(dlgPdf.str_Keywords)
        Params.setCreator(dlgPdf.str_Creator)
        Params.setProducer(dlgPdf.str_Producer)

        Params.setUseViewExtents(dlgPdf.m_bIgnoreInvisible)
        Params.setImageCropping(dlgPdf.bCropInvisible)
        Params.setDCTCompression(dlgPdf.bAdditionalCompression)
        Params.setDCTQuality(dlgPdf.imageQ)
        If dlgPdf.pdfaCtrl = 0 Then
            Params.setArchived(PDFExportParams.PDF_A_mode.kPDFA_None)
        End If
        If dlgPdf.pdfaCtrl = 1 Then
            Params.setArchived(PDFExportParams.PDF_A_mode.kPDFA_1b)
        End If

        'for some reason SearchableTextType enum isn't available for now, thus this parameter setting is skipped for now
        ' bug SWIG-472 opened to resolve this issue
        'params.setSearchableTextType(STType);

        If dlgPdf.m_bEnablePRC = True Then
            If dlgPdf.m_bAsBrep = True Then
                Params.setPRCMode(PDFExportParams.PRCSupport.kAsBrep)
            End If
            If dlgPdf.m_bAsBrep = False Then
                Params.setPRCMode(PDFExportParams.PRCSupport.kAsMesh)
            End If

            Dim pModule As OdRxModule = Globals.odrxDynamicLinker().loadApp("OdPrcModule")
            If pModule IsNot Nothing Then
                pModule = Globals.odrxDynamicLinker().loadApp("OdPrcExport")
            End If

            If pModule Is Nothing Then
                Return
            End If

            Dim compressLevel As PDFExportParams.PRCCompressionLevel = PDFExportParams.PRCCompressionLevel.kA3DLooseCompression
            Dim bCompress As Boolean = True
            Select Case dlgPdf.ui_Compression
                Case 0
                    bCompress = False
                Case 2
                    compressLevel = PDFExportParams.PRCCompressionLevel.kA3DMeddiumCompression
                Case 3
                    compressLevel = PDFExportParams.PRCCompressionLevel.kA3DHighCompression
            End Select
            If bCompress = True Then
                Params.setPRCCompression(compressLevel, True, True)
            End If
        End If

        Select Case dlgPdf.ui_SolidHatches
            Case 1 ' as a drawing (vectorizer)
                Params.setSolidHatchesExportType(PDFExportParams.ExportHatchesType.kDrawing)
            Case 2 ' as a pdf path
                Params.setSolidHatchesExportType(PDFExportParams.ExportHatchesType.kPdfPaths)
            Case 0 ' as a bitmap
            Case Else
                Params.setSolidHatchesExportType(PDFExportParams.ExportHatchesType.kBitmap)
        End Select

        Select Case dlgPdf.ui_GradientHatches
            Case 1 ' as a drawing (vectorizer)
                Params.setGradientHatchesExportType(PDFExportParams.ExportHatchesType.kDrawing)
            Case 0 ' as a bitmap
            Case Else
                Params.setGradientHatchesExportType(PDFExportParams.ExportHatchesType.kBitmap)
        End Select

        Select Case dlgPdf.ui_OtherHatches
            Case 1 ' as a drawing (vectorizer)
                Params.setOtherHatchesExportType(PDFExportParams.ExportHatchesType.kDrawing)
            Case 0 ' as a bitmap
            Case Else
                Params.setOtherHatchesExportType(PDFExportParams.ExportHatchesType.kBitmap)
        End Select

        Params.setGeomDPI(dlgPdf.vectorRes)
        Params.setBWImagesDPI(dlgPdf.imagesRes)
        Params.setColorImagesDPI(dlgPdf.imagesRes)

        Dim bMonoPalette As Boolean = False

        ' for some reason PDFExportParams.ColorPolicy considered ambiguous - bug swig 473 opened
        'Select dlgPdf.ui_ColorPolicy
        '    Case 1
        'Params.setColorPolicy(PDFExportParams.ColorPolicy.kMono)
        '    Case 2
        'Params.setColorPolicy(PDFExportParams.ColorPolicy.kGrayscale)
        '    Case 3
        'bMonoPalette = True
        '    Case 0
        '    Case Else
        'End Select

        Dim bMonochrome As Boolean = False
        Dim pValidator As OdDbPlotSettingsValidator

        Dim i As Boolean = pDb.getPSTYLEMODE()
        Dim strMono As String = "monochrome.stb"
        If i = True Then
            strMono = "monochrome.ctb"
        End If

        If bMonoPalette = True  Then
            pValidator = hostApp.plotSettingsValidator()
            Dim PSSlist As OdStringArray = New OdStringArray()
            pValidator.plotStyleSheetList(PSSlist)

            Dim iSize As Integer = PSSlist.Count
            Dim j As Integer = 0
            While j < iSize
                If strMono = PSSlist.Item(j) Then
                    bMonochrome = True
                    j = iSize
                End If
                j = j + 1
            End While
        End If

        If bMonochrome = True Then
            Dim pLayoutDict As OdDbDictionary = pDb.getLayoutDictionaryId().safeOpenObject()
            Dim f As Integer = 0
            Dim lSize As Integer = Params.layouts.Count
            While f < lSize
                Dim pLayout As OdDbLayout = pLayoutDict.getAt(Params.layouts()(f)).safeOpenObject(OpenMode.kForWrite)
                pValidator.setCurrentStyleSheet(OdDbPlotSettings.cast(pLayout), strMono)
                f = f + 1
            End While
        End If
        ''''''''''''''''''''''''''''''''''''
        Dim CurPalette() As UInteger
        CurPalette = Teigha.Core.AllPalettes.getLightPalette()

        Params.Palette = CurPalette
        Params.setBackground(CurPalette(0))

        Params.setHatchDPI(dlgPdf.hatchRes)
        'Params.setGeomDPI(dlgPdf.m_diGeomDPI)

        Dim pdf_mod As OdRxModule = Globals.odrxDynamicLinker().loadApp("TD_PdfExport")
        Dim pdf_module As PdfExportModule = New PdfExportModule(OdRxModule.getCPtr(pdf_mod).Handle, False)
        Dim exporter As OdPdfExport = pdf_module.create()
        Dim errCode As UInt32 = exporter.exportPdf(Params)
    End Sub
End Class
