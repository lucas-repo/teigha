/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
//using System.Data;
using Microsoft.Win32;
using Teigha.Core;
using Teigha.TD;
using OdViewExMgd.Print;

namespace OdaDwgAppMgd
{
  public partial class Form1 : Form
  {
    CustomServices _hostApp;
    OdRxSystemServices _sysSrv;
    OdDbDumper m_dumper;
    OdDbDatabase CurDb = null;

    //OdGsLayoutHelper helperDevice;
    //OdDbPlotSettingsValidator validator;

    public Form1()
    {
      #region initialization
      InitializeComponent();
      _sysSrv = new ExSystemServices();
      _hostApp = new CustomServices();
      TD_Db.odInitialize(_sysSrv);
      Environment.SetEnvironmentVariable("DDPLOTSTYLEPATHS", _hostApp.FindConfigPath(String.Format("PrinterStyleSheetDir")));

      //Teigha.Core.Globals.odrxInitialize(_sysSrv);
      //Teigha.Core.Globals.odgsInitialize();
      //Teigha.Core.Globals.odrxDynamicLinker().loadModule("TG_Db");
      //Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_Db.dll", false);
      
      // mind derived Srv is used as we need to override gsBitmapDevice method
      //_hostApp = new OdExDgnHostAppServices();
      //_hostApp = new CustomServices();
      m_dumper = new OdDbDumper();

      #endregion
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (openDwgDialog.ShowDialog() == DialogResult.OK)
      {
        bool bLoaded = true;
        try
        {
          OdDbDatabase db = _hostApp.readFile(openDwgDialog.FileName);
          
          // get some mode settings
          short mtMode = _hostApp.getMtMode();
          LineWeight lw = _hostApp.getLWDEFAULT();
          string sFontAlt = _hostApp.getFONTALT();

          Tree t = new Tree(db);
          t.FormClosed += new FormClosedEventHandler(TreeFormClosed);
          t.MdiParent = this;
          t.Show();
          t.FillTree();
          CurDb = db;
          //exportSVGToolStripMenuItem.Enabled = true;
          //mku UpdateMainMenu(true);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
          bLoaded = false;
        }
        if (bLoaded)
        {
          UpdateMainMenu(true);
        }
      }
    }

    private void TreeFormClosed(object sender, FormClosedEventArgs e)
    {
      UpdateMainMenu(false);
    }

    private void UpdateMainMenu(bool flag)
    {
      toolStripSeparator1.Visible = flag;
      ExportSVG.Visible = flag;
      ExportPDF.Visible = flag;
      ExportDWF.Visible = flag;
      ImportDWF.Visible = flag;
      ImportDGN.Visible = flag;
      printToolStripMenuItem.Enabled = true;
      printToolStripMenuItem.Visible = flag;
      viewToolStripMenuItem.Visible = flag;
    }

    public void setStatus(string s)
    {
      this.toolStripStatusLabel1.Text = s;
    }
    public VectorizeForm createVectorizationWindow(Tree t, OdDbObjectId vectorizedViewId, OdDbObjectId vectorizedModelId)
    {
      VectorizeForm v = new VectorizeForm(t, vectorizedViewId, vectorizedModelId);
      v.MdiParent = this;
      v.Show();
      return v;
    }

    private void ExportSVG_Click(object sender, EventArgs e)
    {
        /*ExStringIO strIO = ExStringIO.create("svgout 1 6 \n\n.png sans-serif 768 1024 Yes Yes\n");
        OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, CurDb);
        //OdEdCommandContext pCont = OdEdCommandContext.cast(pCon);
        OdEdCommandStack pStack = Globals.odedRegCmds();
        //pStack.executeCommand("custom", pCont);
        OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
        if (null == pModule)
        {
            MessageBox.Show("TD_SvgExport.tx is missing");
            return;
        }
        pStack.executeCommand("SVGOUT", pCon);*/
        OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
        if (null == pModule)
        {
            MessageBox.Show("TD_SvgExport.tx is missing");
            return;
        }
        ExecuteCommand("svgout 1 6 \n\n.png sans-serif 768 1024 Yes Yes\n", false);
    }

    private void fillLayoutList(OdStringArray aLayouts, bool bAllLayouts)
    {
      if (bAllLayouts == true)
      {
        OdDbDictionary pLayoutDict = (OdDbDictionary)CurDb.getLayoutDictionaryId().safeOpenObject();
        OdDbDictionaryIterator pIter = pLayoutDict.newIterator();
        for (; !pIter.done(); pIter.next())
        {
          OdDbObjectId id = pIter.objectId();
          OdDbLayout pLayout = OdDbLayout.cast(id.safeOpenObject());
          if (pLayout == null)
          {
            continue;
          }
          string sName = pLayout.getLayoutName();
          if (pLayout.getBlockTableRecordId() == CurDb.getModelSpaceId())
          {
            aLayouts.Insert(0, sName);
          }
          else
          {
            OdDbBlockTableRecord pRec = (OdDbBlockTableRecord)pLayout.getBlockTableRecordId().safeOpenObject();
            OdDbObjectIterator pIt = pRec.newIterator();
            bool bEmpty = pIt.done();
            if (!bEmpty)
            {
              aLayouts.Add(sName);
            }
          }
        }
      }
      else
      {
        String layoutName = "";
        OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)CurDb.getActiveLayoutBTRId().safeOpenObject();
        OdDbLayout pLayout = (OdDbLayout)pBlock.getLayoutId().safeOpenObject();
        layoutName = pLayout.getLayoutName();
        aLayouts.Add(layoutName);
      }
    }

    private void ExportPDF_Click(object sender, EventArgs e)
    {

      OdRxModule modPlSt = Globals.odrxDynamicLinker().loadApp("PlotStyleServices", false);
      OdRxModule modPlValidator = Globals.odrxDynamicLinker().loadApp("PlotSettingsValidator", false);
      OdPsPlotStyleServices PsServices = new OdPsPlotStyleServices(OdRxModule.getCPtr(modPlSt).Handle, false);

      OdDbPlotSettingsValidator validator = _hostApp.plotSettingsValidator();
      int hashcode = validator.GetHashCode();

      string psFileName = "";
      string psFullFileName = _hostApp.findFile("IndexedColorTest.stb");
      if (psFullFileName.Length > 0)
      {
        OdStreamBuf pPsFile = TD_Db.odSystemServices().createFile(psFullFileName);
        OdPsPlotStyleTable pPlotStyleTable = PsServices.loadPlotStyleTable(pPsFile);

        bool bRes = false;
        OdPsPlotStyleData psData = new OdPsPlotStyleData();
        pPlotStyleTable.plotStyleAt("TestStyle").getData(psData);
        bool flag1 = psData.color().isByACI();
        short color_index = psData.color().colorIndex();
        if (color_index == 40)
        {
          bRes = true;
        }
        pPlotStyleTable.plotStyleAt("TestStyle1").getData(psData);
        bool flag2 = psData.color().isByACI();
        short color_index2 = psData.color().colorIndex();
        if (color_index2 == (ushort)OdCmEntityColor.ACIcolorMethod.kACIGreen)
        {
          bRes = true;
        }
      }
      //psFullFileName = _hostApp.findFile("Grayscale.ctb");
      psFullFileName = _hostApp.findFile("plotstyle.ctb");
      if (psFullFileName.Length > 0)
      {
        psFileName = "plotstyle.ctb";
        String layoutName = "";

        OdDbObjectId idLayoutCur = CurDb.currentLayoutId();
        OdDbLayout currentLayout = (OdDbLayout)idLayoutCur.safeOpenObject(Teigha.TD.OpenMode.kForWrite);
        layoutName = currentLayout.getLayoutName();

        OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)CurDb.getActiveLayoutBTRId().safeOpenObject();
        OdDbObjectId idLayoutAct = CurDb.getActiveLayoutBTRId();
        //OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)CurDb.getActiveLayoutBTRId().safeOpenObject();
        OdDbLayout pLayout = (OdDbLayout)pBlock.getLayoutId().safeOpenObject();
        layoutName = pLayout.getLayoutName();


		    OdDbPlotSettings plotSettings = (OdDbPlotSettings)currentLayout.objectId().safeOpenObject(Teigha.TD.OpenMode.kForWrite);

		    OdDbPlotSettingsValidator plotSettingsValidator = _hostApp.plotSettingsValidator();
        plotSettingsValidator.refreshLists(plotSettings);
        using (OdStringArray plotStyles = new OdStringArray())
        {
          plotSettingsValidator.plotStyleSheetList(plotStyles);
        }
        plotSettingsValidator.setCurrentStyleSheet(plotSettings, psFileName);
      }
      psFullFileName = _hostApp.findFile("plotstyle1.ctb");
      if (psFullFileName.Length > 0)
      {
        OdStreamBuf pPsFile = TD_Db.odSystemServices().createFile(psFullFileName);
        OdPsPlotStyleTable pPlotStyleTable = PsServices.loadPlotStyleTable(pPsFile);

        bool bRes = true;
      }

      ///////////////////

      OdRxModule mod = Globals.odrxDynamicLinker().loadApp("TD_PdfExport");
      if (null == mod)
      {
        MessageBox.Show("Failed to load PDF export module", "Error");
      }

      Export2PdfForm dlgPdf = new Export2PdfForm();
      using (PDFExportParams Params = new PDFExportParams())
      {
        OdGsPageParams pParams = new OdGsPageParams();
        dlgPdf.m_dPaperH = pParams.getPaperHeight();
        dlgPdf.m_dPaperW = pParams.getPaperWidth();
        if (DialogResult.OK != dlgPdf.ShowDialog())
        {
          return;
        }
        if (!dlgPdf.m_bEnableLayers)
          dlgPdf.m_bExportOffLayers = false;

        OdStringArray arrLayouts = new OdStringArray();
        fillLayoutList(arrLayouts, dlgPdf.m_Layouts);
        Params.setLayouts(arrLayouts);

        OdGsPageParamsArray ppArr = Params.pageParams();
        uint len = (uint)arrLayouts.Count;
        if (len == 0) len = 1;
        ppArr.resize(len);
        Params.setPageParams(ppArr);

        bool bV15 = dlgPdf.m_bEnableLayers || dlgPdf.m_bExportOffLayers;

        Params.setDatabase(CurDb);
        Params.setVersion((PDFExportParams.PDFExportVersions)(bV15 ? PDFExportParams.PDFExportVersions.kPDFv1_5 : PDFExportParams.PDFExportVersions.kPDFv1_4));

        using (OdStreamBuf file = Globals.odrxSystemServices().createFile(dlgPdf.m_FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways))
        {
            Params.setOutput(file);
            Params.setExportFlags(
                           (dlgPdf.m_bEmbedded ? PDFExportParams.PDFExportFlags.kEmbededTTF : 0) |
                           (dlgPdf.m_bSHXAsGeometry ? PDFExportParams.PDFExportFlags.kSHXTextAsGeometry : 0) |
                           (dlgPdf.m_bTTFAsGeometry ? PDFExportParams.PDFExportFlags.kTTFTextAsGeometry : 0) |
                           (dlgPdf.m_bSimpleGeomOpt ? PDFExportParams.PDFExportFlags.kSimpleGeomOptimization : 0) |
                           (dlgPdf.m_bZoomToExtents ? PDFExportParams.PDFExportFlags.kZoomToExtentsMode : 0) |
                           (dlgPdf.m_bEnableLayers ? PDFExportParams.PDFExportFlags.kEnableLayers : 0) |
                           (dlgPdf.m_bExportOffLayers ? PDFExportParams.PDFExportFlags.kIncludeOffLayers : 0) |
                              (dlgPdf.m_bUseHLR ? PDFExportParams.PDFExportFlags.kUseHLR : 0) |
                              (dlgPdf.m_bEncoded ? PDFExportParams.PDFExportFlags.kFlateCompression : 0) |
                              (dlgPdf.m_bEncoded ? PDFExportParams.PDFExportFlags.kASCIIHexEncoding : 0) |
                              (dlgPdf.m_bExportHyperlinks ? PDFExportParams.PDFExportFlags.kExportHyperlinks : 0));


            Params.setTitle(dlgPdf.m_Title);
            Params.setAuthor(dlgPdf.m_Author);
            Params.setSubject(dlgPdf.m_Subject);
            Params.setKeywords(dlgPdf.m_Keywords);
            Params.setCreator(dlgPdf.m_Creator);
            Params.setProducer(dlgPdf.m_Producer);

            UInt32[] CurPalette = Teigha.Core.AllPalettes.getLightPalette();         
          
          Params.Palette = CurPalette;
          Params.setBackground(CurPalette[0]);

          Params.setHatchDPI( dlgPdf.m_dHatchDPI );
          Params.setGeomDPI( dlgPdf.m_diGeomDPI );

          //////////// PRC test ////////////////
	      {
            Params.setPRCMode(PDFExportParams.PRCSupport.kAsBrep);//(m_bUsePRCAsBRep == TRUE ? PDFExportParams::kAsBrep : PDFExportParams::kAsMesh);
            OdRxModule pModule = Globals.odrxDynamicLinker().loadApp("OdPrcModule");
            if (null != pModule)
            {
              pModule = Globals.odrxDynamicLinker().loadApp("OdPrcExport");
            }
            if (null == pModule)
            {
                MessageBox.Show("PDF Export, PRC suppotr - exPdfExportServiceMissed");
            }

            OdRxObject pObj = null;
            bool m_bUsePRCSingleViewMode = true; // provide a corresponding checkbox in Export to PDF dialog similar to one in OdaMfcApp
            if (m_bUsePRCSingleViewMode)
            {
                pObj = Globals.odrxClassDictionary().getAt("OdPrcContextForPdfExport_AllInSingleView");
            }
            else 
            {
                pObj = Globals.odrxClassDictionary().getAt("OdPrcContextForPdfExport_Default");
            }
            if (null != pObj)
            {
                OdRxClass pCls = OdRxClass.cast(pObj);
                if (null != pCls)
                {
                    Params.setPRCContext(pCls.create());
                }
                else
                {
                    MessageBox.Show("PDF Export, PRC suppotr - OdRxClass failed");
                }
            }
            else
            {
                MessageBox.Show("PDF Export, PRC suppotr - getAt failed");
            }
          }
          //////////////////////////////////////

          PdfExportModule module = new PdfExportModule(OdRxModule.getCPtr(mod).Handle, false); //= PdfExportModule.cast();
          OdPdfExport exporter = module.create();
          UInt32 errCode = exporter.exportPdf(Params);

          if (errCode != 0)
          {
            String errMes = exporter.exportPdfErrorCode(errCode);
            String str;
            str = string.Format("Error number : {0}. \n {1}", errCode, errMes);

            if (errCode == 0x10008)
            {
              str += "\nPlease enable Zoom to extents check box or\ndefine page parameters for layout in page setup dialog.";
            }

            MessageBox.Show("PDF error", str);
          }
        }
      }
    }

    private void zoomExtents_Click(object sender, EventArgs e)
    {
      VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
      act.ZoomExtents();
    }
    public void UpdateZoomExtFlag(bool flag)
    {
      zoomExtents.Visible = flag;
    }

    public void UpdateRenderFlags(bool flag)
    {
      snapshotToolStripMenuItem.Enabled = flag;
    }

    private void ExportDWF_Click(object sender, EventArgs e)
    {
      OdRxModule mod = Globals.odrxDynamicLinker().loadApp("TD_Dwf7Export");
      if (null == mod)
      {
          MessageBox.Show("Failed to load DWF export module", "Error");
          return;
      }
      DwfExportModule dwf_mod = new DwfExportModule(OdRxModule.getCPtr(mod).Handle, false);
        
      SaveFileDialog dwfDlg = new SaveFileDialog();
      dwfDlg.Title = "Export to DWF";
      dwfDlg.Filter = "Binary DWF v6.0|*.dwf|Zipped Stream DWF v6.0|*.dwf|Compressed DWF v5.5|*.dwf|Binary DWF v5.5|*dwf|Ascii DWF v5.5|*.dwf|Compressed DWF v4.2|*.dwf|Binary DWF v4.2|*dwf|Ascii DWF v4.2|*.dwf|XPS|*.dwfx";
      if (DialogResult.OK != dwfDlg.ShowDialog())
      {
        return;
      }
///////////////////////////
    DwExportParams Params = new DwExportParams();
    OdGiContextForDbDatabase pCtx = OdGiContextForDbDatabase.createObject();
//    OdGiContextForDbDatabaseToExport pCtx = OdGiContextForDgDatabaseToExport.createObject();
    pCtx.setDatabase(CurDb);
    Params.setDatabase(pCtx.database());
    Params.setDwfFileName(dwfDlg.FileName);

    // MKU 03\01\02 - resolved "white background issue"
    UInt32[] refColors = Teigha.Core.AllPalettes.getLightPalette();
    //OdDgModel pModel = (OdDgModel)CurDb.getActiveModelId().safeOpenObject();
    UInt32 background = refColors[0];
    //refColors[255] = background;
    pCtx.setPaletteBackground(refColors[0]);

    // This method should be called to resolve "white background issue" before setting device palette
    //bool bCorrected = OdDgColorTable.correctPaletteForWhiteBackground(refColors);
    Params.Palette = refColors;
    Params.setBackground(background);

    Params.setInkedArea(false);                                                        // MKU 1/21/2004
    Params.setColorMapOptimize(false);                                                 // MKU 1/21/2004
    Params.setExportInvisibleText(true);
    //params.bForceInitialViewToExtents = true;

    DwfPageData pageData = new DwfPageData();
        //OdDbObjectId pId = m_pDb->getActiveModelId();
        //OdDgModelPtr pModel = OdDgModel::cast(pId.openObject());
    DwfPageDataArray pdArray = new DwfPageDataArray();
    switch(dwfDlg.FilterIndex)
    {
      case 1:
        Params.setFormat(DW_FORMAT.DW_UNCOMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v60);

        pageData.sLayout = CurDb.findActiveLayout(true);
        pdArray.Add(pageData);
        break;
      case 2:
        Params.setFormat(DW_FORMAT.DW_ASCII);
        Params.setVersion(DwfVersion.nDwf_v60);
        pageData.sLayout = CurDb.findActiveLayout(true);
        pdArray.Add(pageData);
        break;
      case 3:
        Params.setFormat(DW_FORMAT.DW_COMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v55);
        break;
      case 4:
        Params.setFormat(DW_FORMAT.DW_UNCOMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v55);
        break;
      case 5:
        Params.setFormat(DW_FORMAT.DW_ASCII);
        Params.setVersion(DwfVersion.nDwf_v55);
        break;
      case 6:
        Params.setFormat(DW_FORMAT.DW_COMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v42);
        break;
      case 7:
        Params.setFormat(DW_FORMAT.DW_UNCOMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v42);
        break;
      case 8:
        Params.setFormat(DW_FORMAT.DW_ASCII);
        Params.setVersion(DwfVersion.nDwf_v42);
        break;
      case 9:
        Params.setFormat(DW_FORMAT.DW_XPS);
        Params.setVersion((DwfVersion)602);
        pageData.sLayout = CurDb.findActiveLayout(true);
        pdArray.Add(pageData);
        break;
      default:
        //ASSERT(0);
        return;
    }
    Params.setPageData(pdArray);
    OdDwfExport dwf_exp = dwf_mod.create();
    dwf_exp.exportDwf(Params);
///////////////////////////
    }

    private void toolStripSeparator3_Click(object sender, EventArgs e)
    {

    }

    private void ImportDWF_Click(object sender, EventArgs e)
    {
        ExStringIO strIO = ExStringIO.create("dwfin ~\n\n297 210 No -1 Yes\n");
        OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, CurDb);
        OdEdCommandStack pStack = Globals.odedRegCmds();
        OdRxModule pModule = Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_Dwf7Import");
        if (null == pModule)
        {
            MessageBox.Show("TD_Dwf7Import.tx is missing");
            return;
        }
        pStack.executeCommand("DWFIN", pCon);
    }

    private void ImportDGN_Click(object sender, EventArgs e)
    {
        OpenFileDialog opnDgn = new OpenFileDialog();
        opnDgn.Filter = "DGN files|*.dgn";
        if (DialogResult.Cancel == opnDgn.ShowDialog())
        {
            return;
        }
        //
      OdRxModule module = Globals.odrxDynamicLinker().loadApp("TD_DgnImport", false);
      OdDgnImportModule pModule = new OdDgnImportModule(OdRxModule.getCPtr(module).Handle, false);

      OdDgnImport importer = pModule.create();
      OdRxDictionary Properties = importer.properties();
      Properties.putAt("Services", _hostApp);
      Properties.putAt("DgnPath", new OdRxVariantValue(opnDgn.FileName));
//      importer->properties()->putAt( L"MS_SYMBRSRC", OdRxVariantValue(arrRsrc) );
        // !!! be careful when using numbers as arguments for OdRxVariantalue
        // !!! there are several ctors: with byte, sbyte, (U)Int16,32,64
        // pay attention to legacy C++ code and use the proper one
      Properties.putAt("XRefImportMode", new OdRxVariantValue((byte)2));
      Properties.putAt("ImportActiveModelToModelSpace", new OdRxVariantValue(true));
      Properties.putAt("ImportPaperSpaceModels", new OdRxVariantValue(true));
      Properties.putAt("RecomputeDimensionsAfterImport", new OdRxVariantValue(false));
      Properties.putAt("ImportViewIndex", new OdRxVariantValue((byte)0));
      Properties.putAt("3dShapeImportMode", new OdRxVariantValue((byte)1));
      Properties.putAt("shxFontsPath", new OdRxVariantValue(""));//new OdRxVariantValue(shxPath));

      OdRxObject lw = Properties.getAt("LineWeightsMap");
      OdDgnImportLineWeightsMap pLWMap = new OdDgnImportLineWeightsMap(OdRxObject.getCPtr(lw).Handle, false);
      if (null != pLWMap)
      {
          for (UInt32 i = 0; i < 32; i++)
          {
              if (i % 2 == 0)
              {
                  pLWMap.setLineWeightForDgnIndex(i, LineWeight.kLnWt030);
              }
              else
              {
                  pLWMap.setLineWeightForDgnIndex(i, LineWeight.kLnWt100);
              }
          }
      }

      OdDgnImport.ImportResult res = importer.import();

      if (res == OdDgnImport.ImportResult.success)
      {
        OdDbDatabase db = (OdDbDatabase)Properties.getAt("Database");
        Tree t = new Tree(db);
        t.FormClosed += new FormClosedEventHandler(TreeFormClosed);
        t.MdiParent = this;
        t.Show();
        t.FillTree();
        CurDb = db;
        //exportSVGToolStripMenuItem.Enabled = true;
        UpdateMainMenu(true);
      }
      else
      {
        switch(res)
        {
        case OdDgnImport.ImportResult.bad_database:
          MessageBox.Show("Bad database", "DGN import");
          break;
        case OdDgnImport.ImportResult.bad_file:
          MessageBox.Show("Bad file", "DGN import");
          break;
        case OdDgnImport.ImportResult.encrypted_file:
        case OdDgnImport.ImportResult.bad_password:
          MessageBox.Show("The file is encrypted", "DGN import");
          break;
        case OdDgnImport.ImportResult.fail:
          MessageBox.Show("Unknown import error", "DGN import");
          break;
        }
      }
    }

    private void importDWF2_Click(object sender, EventArgs e)
    {
        OpenFileDialog opnDwf = new OpenFileDialog();
        opnDwf.Filter = "DWF files|*.dwf";
        if (DialogResult.Cancel == opnDwf.ShowDialog())
        {
            return;
        }

        OdRxModule module = Globals.odrxDynamicLinker().loadModule("TD_Dwf7Import");
        OdDwfImportModule pModule = new OdDwfImportModule(OdRxModule.getCPtr(module).Handle, false);
        OdDwfImport importer = pModule.create();
        OdRxDictionary Properties = importer.properties();

        OdDbDatabase pDb = _hostApp.createDatabase();
        Properties.putAt("Database", pDb);
        Properties.putAt("DwfPath", new OdRxVariantValue(opnDwf.FileName));
        Properties.putAt("PreserveColorIndices", new OdRxVariantValue(false));

        OdDwfImport.ImportResult res = importer.import();
        if (OdDwfImport.ImportResult.success != res)
        {
            switch (res)
            {
                case OdDwfImport.ImportResult.bad_database:
                    MessageBox.Show("DWF import error", "bad database");
                    break;
                case OdDwfImport.ImportResult.bad_file:
                    MessageBox.Show("DWF import error", "bad file");
                    break;
                case OdDwfImport.ImportResult.bad_password:
                    MessageBox.Show("DWF import error", "bad password");
                    break;
                case OdDwfImport.ImportResult.encrypted_file:
                    MessageBox.Show("DWF import error", "encrypted file");
                    break;
                case OdDwfImport.ImportResult.fail:
                    MessageBox.Show("DWF import error", "unknown failure");
                    break;
                default:
                    MessageBox.Show("DWF import error", "other failure");
                    break;
            }
            return;
        }
        pDb = (OdDbDatabase)Properties.getAt("Database");
        Tree t = new Tree(pDb);
        t.FormClosed += new FormClosedEventHandler(TreeFormClosed);
        t.MdiParent = this;
        t.Show();
        t.FillTree();
        CurDb = pDb;
        UpdateMainMenu(true);
    }

    private void Print2_Click(object sender, EventArgs e)
    {
      try
      {
        //streamToPrint = new StreamReader("C:\\My Documents\\MyFile.txt");
        //try
        //{
          //printFont = new Font("Arial", 10);
          PrintDocument pd = new PrintDocument();
          pd.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);
          pd.Print();
        //}
        //finally
        //{
        //  streamToPrint.Close();
        //}
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
    //void Print(Boolean showDialog)
    //{
    //  using (PrintDocument printDocument = new PrintDocument())
    //  {
    //    printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
    //
    //    if (showDialog)
    //    {
    //      using (PrintDialog dialog = new PrintDialog())
    //      {
    //        dialog.Document = printDocument;
    //        DialogResult result = dialog.ShowDialog();
    //        if (result == DialogResult.OK)
    //        {
    //          using (PrintPreviewDialog previewDialog = new PrintPreviewDialog())
    //          {
    //            previewDialog.Document = printDocument;
    //            if (previewDialog.ShowDialog() == DialogResult.OK)
    //            {
    //            }
    //          }
    //        }
    //        else if (result == DialogResult.Cancel)
    //        {
    //          return;
    //        }
    //      }
    //    }
    //
    //  }
    //}

    private void PrintDocument_PrintPage(Object sender, PrintPageEventArgs e)
    {
      return;
      #region Source posted on forum from sample
      ////OdGsDevice printDevice;
      //using (OdGsModule gsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false))
      //{
      //    OdGsDevice printDevice = gsModule.createDevice();
      //    //using (OdGsDevice printDevice = gsModule.createDevice())
      //    {
      //        using (OdGiContextForDbDatabase ctx = OdGiContextForDbDatabase.createObject())
      //        {
      //            ctx.enableGsModel(true);
      //            ctx.setPlotGeneration(true);
      //            ctx.setDatabase(_tdDatabase);
      //            printDevice.setUserGiContext(ctx);
      //            printDevice = (OdGsDevice)TD_Db.setupActiveLayoutViews(printDevice, ctx);

      //            Boolean modelSpace = _tdDatabase.getTILEMODE();
      //            SetViewportBorderProperties(_gsDevice, !modelSpace);

      //            printDevice.setBackgroundColor(_monoPalette[0]);
      //            printDevice.setLogicalPalette(_monoPalette, 256);
      //            if (printDevice != null)
      //            {
      //                Double factorX = e.Graphics.DpiX / 100;
      //                Double factorY = e.Graphics.DpiY / 100;

      //                OdGsView pView = printDevice.viewAt(0);
      //                OdAbstractViewPE view = OdAbstractViewPE.cast(pView);
      //                view.zoomExtents(pView);

      //                OdGsDCRect rect = new OdGsDCRect((Int32)(e.MarginBounds.X * factorX), (Int32)(e.MarginBounds.Y * factorY), (Int32)(e.MarginBounds.Width * factorX), (Int32)(e.MarginBounds.Height * factorY));
      //                //OdGsDCRect rect = new OdGsDCRect(0, 0, (Int32)(e.MarginBounds.Width * factorX) - (Int32)(e.MarginBounds.X * factorX), (Int32)(e.MarginBounds.Height * factorY) - (Int32)(e.MarginBounds.Y * factorY));
      //                printDevice.onSize(rect);

      //                if (printDevice.properties().has("WindowHDC"))
      //                {
      //                    printDevice.properties().putAt("WindowHDC", new OdRxVariantValue(e.Graphics.GetHdc().ToInt32()));
      //                }

      //                printDevice.update();
      //            }
      //        }
      //    }
      //}
      #endregion

      //try
      //{
      //  OdGsLayoutHelper printerDevice = null;
      //  using (OdGiContextForDbDatabase databaseContext = OdGiContextForDbDatabase.createObject())
      //  {
      //    databaseContext.setDatabase(CurDb);
      //    if (_gsDevice != null)
      //    {
      //      databaseContext.setPlotGeneration(true);
      //      databaseContext.enableGsModel(false);
      //      printerDevice = TD_Db.setupActiveLayoutViews(_gsDevice, databaseContext);
      //      databaseContext.setPlotGeneration(false);
      //    }
      //
      //    if (printerDevice != null)
      //    {
      //      Double printerLeftMargin = e.PageSettings.HardMarginX;
      //      Double printerTopMargin = e.PageSettings.HardMarginY;
      //      Double printerRightMargin = e.PageSettings.PaperSize.Width - e.PageSettings.Margins.Left - printerLeftMargin;
      //      Double printerBottomMargin = e.PageSettings.PaperSize.Height - e.PageSettings.Margins.Top - printerTopMargin;
      //      Double koeffX = e.PageSettings.PrinterResolution.X / AC.Shared.Utility.Length.InchToMillimeters;
      //      Double koeffY = e.PageSettings.PrinterResolution.Y / AC.Shared.Utility.Length.InchToMillimeters;
      //
      //      if (_printColor)
      //      {
      //        printerDevice.setLogicalPalette(Teigha.Core.AllPalettes.getLightPalette(), 256);
      //      }
      //      else
      //      {
      //        printerDevice.setLogicalPalette(_monoPalette, 256);
      //      }
      //      printerDevice.setBackgroundColor(_monoPalette[0]);
      //
      //      // Get Layout info
      //      OdDbLayout layout = (OdDbLayout)printerDevice.layoutId().safeOpenObject();
      //      Boolean scaledToFit = layout.useStandardScale() && (OdDbPlotSettings.StdScaleType.kScaleToFit == layout.stdScaleType());
      //      Boolean centered = layout.plotCentered();
      //      Boolean metric = (layout.plotPaperUnits() != OdDbPlotSettings.PlotPaperUnits.kInches) ? true : false;
      //      Boolean printLW = layout.printLineweights() || layout.showPlotStyles();
      //
      //      Double offsetX, offsetY;
      //      layout.getPlotOrigin(out offsetX, out offsetY); // in mm
      //      OdGePoint2d pio = layout.getPaperImageOrigin(); // in mm
      //      Double leftMargin = layout.getLeftMargin();  // in mm
      //      Double rightMargin = layout.getRightMargin(); // in mm
      //      Double topMargin = layout.getTopMargin();   // in mm
      //      Double bottomMargin = layout.getBottomMargin();// in mm
      //
      //      OdDbPlotSettings.PlotType plotType = layout.plotType();
      //      // Force the plot type to "extents" and switch-off lineweights
      //      scaledToFit = centered = true;
      //      plotType = OdDbPlotSettings.PlotType.kExtents;
      //      printLW = false;
      //
      //      Boolean model = printerDevice.isKindOf(OdGsModelLayoutHelper.desc());
      //      // Set LineWeight scale factor for model space
      //      if (printLW && model)
      //      {
      //        OdGsView pTo = printerDevice.viewAt(0);
      //        pTo.setLineweightToDcScale(Math.Max(e.PageSettings.PrinterResolution.X, e.PageSettings.PrinterResolution.Y) / AC.Shared.Utility.Length.InchToMillimeters * 0.01);
      //      }
      //
      //      Boolean print0 = false;
      //      Boolean print90 = false;
      //      Boolean print180 = false;
      //      Boolean print270 = false;
      //      switch (layout.plotRotation())
      //      {
      //        default:
      //        case OdDbPlotSettings.PlotRotation.k0degrees:
      //          {
      //            print0 = true;
      //          }
      //          break;
      //
      //        case OdDbPlotSettings.PlotRotation.k90degrees:
      //          print90 = true;
      //          Swap<Double>(ref topMargin, ref rightMargin);
      //          Swap<Double>(ref bottomMargin, ref leftMargin);
      //          Swap<Double>(ref bottomMargin, ref topMargin);
      //          Swap<Double>(ref offsetX, ref offsetY);
      //          offsetY = -offsetY;
      //          offsetX = -offsetX;
      //          break;
      //
      //        case OdDbPlotSettings.PlotRotation.k180degrees:
      //          print180 = true;
      //          Swap<Double>(ref rightMargin, ref leftMargin);
      //          offsetY = -offsetY;
      //          offsetX = -offsetX;
      //          break;
      //
      //        case OdDbPlotSettings.PlotRotation.k270degrees:
      //          print270 = true;
      //          Swap<Double>(ref topMargin, ref rightMargin);
      //          Swap<Double>(ref bottomMargin, ref leftMargin);
      //          Swap<Double>(ref offsetX, ref offsetY);
      //          break;
      //      }
      //
      //      // Get scale factor
      //      Double factor;
      //      if (layout.useStandardScale())
      //      {
      //        layout.getStdScale(out factor);
      //      }
      //      else
      //      {
      //        Double numerator, denominator;
      //        layout.getCustomPrintScale(out numerator, out denominator);
      //        factor = numerator / denominator;
      //      }
      //
      //      if (leftMargin < printerLeftMargin / koeffX)
      //      {
      //        leftMargin = printerLeftMargin / koeffX;
      //      }
      //      if (topMargin < printerTopMargin / koeffY)
      //      {
      //        topMargin = printerTopMargin / koeffY;
      //      }
      //
      //      // Also adjust Right and Bottom margins
      //      if (rightMargin < printerRightMargin / koeffX)
      //      {
      //        rightMargin = printerRightMargin / koeffX;
      //      }
      //      if (bottomMargin < printerBottomMargin / koeffY)
      //      {
      //        bottomMargin = printerBottomMargin / koeffY;
      //      }
      //
      //      // Calculate paper drawable area using margins from layout (in mm).
      //      Double rx1 = (-printerLeftMargin / koeffX + leftMargin);                // in mm
      //      Double rx2 = (rx1 + e.PageSettings.PaperSize.Width / koeffX - leftMargin - rightMargin); // in mm
      //      Double ry1 = (-printerTopMargin / koeffY + topMargin);                // in mm
      //      Double ry2 = (ry1 + e.PageSettings.PaperSize.Height / koeffY - topMargin - bottomMargin); // in mm
      //
      //      // Margin clip box calculation
      //      topMargin *= koeffY; // in printer units
      //      rightMargin *= koeffX;
      //      bottomMargin *= koeffY;
      //      leftMargin *= koeffX;
      //
      //      Double dScreenFactorH, dScreenFactorW;
      //      Double x = leftMargin - printerLeftMargin;
      //      Double y = topMargin - printerTopMargin;
      //      Rectangle marginsClipBox = new Rectangle((Int32)x, (Int32)y,
      //                                                  (Int32)(x + e.PageSettings.PaperSize.Width - leftMargin - rightMargin),
      //                                                  (Int32)(y + e.PageSettings.PaperSize.Height - topMargin - bottomMargin));
      //      dScreenFactorH = 1.0;
      //      dScreenFactorW = 1.0;
      //
      //      // MarginsClipBox is calculated
      //      Rectangle clipBox = marginsClipBox;
      //
      //      // Get view and viewport position, direction ...
      //      OdGsView pOverallView;
      //
      //      OdGePoint3d viewportCenter;
      //      OdGePoint3d viewTarget;
      //      OdGeVector3d upV, viewDir;
      //      Boolean isPerspective;
      //      Double viewportH, viewportW;
      //      OdGeMatrix3d eyeToWorld, WorldToeye;
      //      Boolean SkipClipping = false;
      //
      //      OdRxObject pVObject;
      //      OdAbstractViewPE pAbstractViewPE = null;
      //      pOverallView = model ? OdGsModelLayoutHelper.cast(printerDevice).activeView() : OdGsPaperLayoutHelper.cast(printerDevice).overallView();
      //      if (plotType == OdDbPlotSettings.PlotType.kView)
      //      {
      //        String sPlotViewName = layout.getPlotViewName();
      //        OdDbViewTableRecord pVtr = (OdDbViewTableRecord)((OdDbViewTable)(databaseContext.getDatabase().getViewTableId().safeOpenObject())).getAt(sPlotViewName).safeOpenObject();
      //        viewTarget = pVtr.target();     // in plotPaperUnits
      //        pVObject = pVtr;
      //      }
      //      else if (model)
      //      {
      //        OdDbViewportTable pVPT = (OdDbViewportTable)databaseContext.getDatabase().getViewportTableId().safeOpenObject();
      //        OdDbViewportTableRecord pActiveVP = (OdDbViewportTableRecord)pVPT.getActiveViewportId().safeOpenObject();
      //        viewTarget = pActiveVP.target();     // in plotPaperUnits
      //        pVObject = pActiveVP;
      //      }
      //      else
      //      {
      //        OdDbObjectId overallVpId = layout.overallVportId();
      //        OdDbViewport pActiveVP = (OdDbViewport)overallVpId.safeOpenObject();
      //        viewTarget = pActiveVP.viewTarget();   // in plotPaperUnits
      //        pVObject = pActiveVP;
      //      }
      //      pAbstractViewPE = OdAbstractViewPE.cast(pVObject);
      //
      //      // get info from view, viewport .... etc
      //      viewportCenter = pAbstractViewPE.target(pVObject);       // in plotPaperUnits
      //      isPerspective = pAbstractViewPE.isPerspective(pVObject);
      //      viewportH = pAbstractViewPE.fieldHeight(pVObject);  // in plotPaperUnits
      //      viewportW = pAbstractViewPE.fieldWidth(pVObject);   // in plotPaperUnits
      //      viewDir = pAbstractViewPE.direction(pVObject);    // in plotPaperUnits
      //      upV = pAbstractViewPE.upVector(pVObject);     // in plotPaperUnits
      //      eyeToWorld = pAbstractViewPE.eyeToWorld(pVObject);
      //      WorldToeye = pAbstractViewPE.worldToEye(pVObject);
      //
      //      Boolean isPlanView = viewTarget.isEqualTo(new OdGePoint3d(0, 0, 0)) && viewDir.normal().isEqualTo(OdGeVector3d.kZAxis);
      //
      //      // To set OverAll View using default settings
      //      // get rect of drawing to view (in plotPaperUnits)
      //      Double fieldWidth = viewportW;
      //      Double fieldHeight = viewportH;
      //      if (plotType == OdDbPlotSettings.PlotType.kWindow || (plotType == OdDbPlotSettings.PlotType.kLimits && isPlanView))
      //      {
      //        Double xmin, ymin, xmax, ymax;
      //        if (plotType == OdDbPlotSettings.PlotType.kWindow)
      //        {
      //          layout.getPlotWindowArea(out xmin, out ymin, out xmax, out ymax); // in plotPaperUnits
      //        }
      //        else
      //        {
      //          xmin = databaseContext.getDatabase().getLIMMIN().x;
      //          ymin = databaseContext.getDatabase().getLIMMIN().y;
      //          xmax = databaseContext.getDatabase().getLIMMAX().x;
      //          ymax = databaseContext.getDatabase().getLIMMAX().y;
      //        }
      //
      //        fieldWidth = xmax - xmin;
      //        fieldHeight = ymax - ymin;
      //
      //        OdGeVector3d tmp = viewportCenter - viewTarget;
      //        viewTarget.set((xmin + xmax) / 2.0, (ymin + ymax) / 2.0, 0);
      //        viewTarget.transformBy(eyeToWorld);
      //        viewTarget -= tmp;
      //      }
      //      else if (plotType == OdDbPlotSettings.PlotType.kDisplay)
      //      {
      //        viewTarget = viewportCenter;
      //        fieldWidth = viewportW;
      //        fieldHeight = viewportH;
      //      }
      //      else if (plotType == OdDbPlotSettings.PlotType.kExtents || (plotType == OdDbPlotSettings.PlotType.kLimits && !isPlanView))
      //      {
      //        OdGeBoundBlock3d extents = new OdGeBoundBlock3d();
      //        OdGsView pView = _gsDevice.viewAt(0);
      //        OdAbstractViewPE abstractView = OdAbstractViewPE.cast(pView);
      //        if (abstractView.viewExtents(pView, extents))
      //        {
      //          fieldWidth = Math.Abs(extents.maxPoint().x - extents.minPoint().x);
      //          fieldHeight = Math.Abs(extents.maxPoint().y - extents.minPoint().y);
      //
      //          extents.transformBy(abstractView.eyeToWorld(pView));
      //
      //          viewTarget = (extents.minPoint() + extents.maxPoint().asVector()).Div(2.0);
      //        }
      //      }
      //      else if (plotType == OdDbPlotSettings.PlotType.kView)
      //      {
      //        viewTarget = viewportCenter;
      //
      //        fieldWidth = viewportW;
      //        fieldHeight = viewportH;
      //      }
      //      else if (plotType == OdDbPlotSettings.PlotType.kLayout)
      //      {
      //        SkipClipping = true; // used full paper drawing area.
      //
      //        fieldWidth = (rx2 - rx1) / factor; // drx in mm -> fieldWidth in mm
      //        fieldHeight = (ry2 - ry1) / factor;
      //
      //        viewTarget.set(fieldWidth / 2.0 - pio.x - offsetX / factor, fieldHeight / 2.0 - pio.y - offsetY / factor, 0); // in mm
      //        if (!metric)
      //        {
      //          viewTarget = viewTarget.Div(AC.Shared.Utility.Length.InchToMillimeters); // must be in plotpaper units
      //          fieldWidth /= AC.Shared.Utility.Length.InchToMillimeters;
      //          fieldHeight /= AC.Shared.Utility.Length.InchToMillimeters;
      //        }
      //
      //        offsetX = 0.0;
      //        offsetY = 0.0;
      //        pio.x = 0.0;
      //        pio.y = 0.0; // it was applied to viewTarget, reset it.
      //        centered = scaledToFit = false;       // kLayout doesn't support it.
      //      }
      //
      //      // in plotpaper units
      //      pOverallView.setView(viewTarget, viewTarget + viewDir, upV, fieldWidth, fieldHeight, isPerspective ? OdGsView.Projection.kPerspective : OdGsView.Projection.kParallel);
      //
      //      if (!metric)
      //      {
      //        fieldWidth *= AC.Shared.Utility.Length.InchToMillimeters;
      //        fieldHeight *= AC.Shared.Utility.Length.InchToMillimeters;
      //      }
      //
      //      if (scaledToFit) // Scale factor can be stored in layout, but preview recalculate it if scaledToFit = true.
      //      {                 // Some times stored factor isn't correct, due to autocad isn't update it before saving.
      //        factor = Math.Min((rx2 - rx1) / fieldWidth, (ry2 - ry1) / fieldHeight);
      //      }
      //
      //      if (centered)    // Offset also can be incorectly saved.
      //      {
      //        offsetX = ((rx2 - rx1) - fieldWidth * factor) / 2.0;
      //        offsetY = ((ry2 - ry1) - fieldHeight * factor) / 2.0;
      //
      //        if (print90 || print180)
      //        {
      //          offsetY = -offsetY;
      //          offsetX = -offsetX;
      //        }
      //      }
      //
      //      if (print180 || print90)
      //      {
      //        rx1 = rx2 - fieldWidth * factor;
      //        ry2 = ry1 + fieldHeight * factor;
      //      }
      //      else if (print0 || print270)
      //      {
      //        rx2 = rx1 + fieldWidth * factor;
      //        ry1 = ry2 - fieldHeight * factor;
      //      }
      //      else // preview
      //      {
      //        rx2 = rx1 + fieldWidth * factor;
      //        ry1 = ry2 - fieldHeight * factor;
      //      }
      //
      //      if (!SkipClipping)
      //      {
      //        if (print180 || print90)
      //        {
      //          clipBox.X = (Int32)(clipBox.Right - (rx2 - rx1) * koeffX * dScreenFactorW);
      //          clipBox.Height = (Int32)(clipBox.Top + (ry2 - ry1) * koeffY * dScreenFactorH);
      //        }
      //        else if (print0 || print270)
      //        {
      //          clipBox.Width = (Int32)(clipBox.Left + (rx2 - rx1) * koeffX * dScreenFactorW);
      //          clipBox.Y = (Int32)(clipBox.Bottom - (ry2 - ry1) * koeffY * dScreenFactorH);
      //        }
      //        else // preview
      //        {
      //          clipBox.Width = (Int32)(clipBox.Left + (rx2 - rx1) * koeffX * dScreenFactorW);
      //          clipBox.Y = (Int32)(clipBox.Bottom - (ry2 - ry1) * koeffY * dScreenFactorH);
      //        }
      //        clipBox.Offset((Int32)(offsetX * koeffX * dScreenFactorW), (Int32)(-offsetY * koeffY * dScreenFactorH));
      //      }
      //
      //      pOverallView.setViewport(new OdGePoint2d(0, 0), new OdGePoint2d(1, 1));
      //
      //      Rectangle resultClipBox = Rectangle.Intersect(marginsClipBox, clipBox);
      //      // Apply clip region to screen
      //      e.Graphics.SetClip(resultClipBox);
      //      //Rectangle newClip;
      //      //newClip.CreateRectRgnIndirect(&ResultClipBox);
      //      //e.Graphics.SetClip(newClip);
      //
      //      // Calculate viewport rect in printer units
      //      Int32 x1 = (Int32)((offsetX + rx1) * koeffX);
      //      Int32 x2 = (Int32)((offsetX + rx2) * koeffX);
      //      Int32 y1 = (Int32)((-offsetY + ry1) * koeffY);
      //      Int32 y2 = (Int32)((-offsetY + ry2) * koeffY);
      //
      //      OdGsDCRect viewportRect;
      //      if (print180 || print90)
      //      {
      //        viewportRect = new OdGsDCRect(x1, x2, y1, y2);
      //      }
      //      else if (print0 || print270)
      //      {
      //        viewportRect = new OdGsDCRect(x2, x1, y2, y1);
      //      }
      //      else // preview
      //      {
      //        viewportRect = new OdGsDCRect(x2, x1, y2, y1);
      //      }
      //
      //      printerDevice.onSize(viewportRect);
      //      printerDevice.properties().putAt("WindowHDC", new OdRxVariantValue((Int32)e.Graphics.GetHdc()));
      //
      //      databaseContext.setPlotGeneration(true);
      //      printerDevice.update();
      //      databaseContext.setPlotGeneration(false);
      //
      //      // Release the printer device
      //      printerDevice.Dispose();
      //    }
      //  }
      //}
      //catch (Exception ex)
      //{
      //}
    }

    private void printToolStripMenuItem_Click(object sender, EventArgs e)
    {
      VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
      OdViewExMgd.Print.Printing pr = new OdViewExMgd.Print.Printing();
      pr.Print(CurDb, act.getGs().activeView(), false);
    }

    private void snapshotToolStripMenuItem_Click(object sender, EventArgs e)
    {
      VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
      act.snapShot();
    }


    //private void Print2_Click(object sender, EventArgs e)
    //{
    //
    //}
      //////////////////////////// Undo/Redo /////////////////////

    private void undovariant1ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // 2 lines were added - thus undo is called twice
        CurDb.undo();
        CurDb.undo();
        VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
        act.Invalidate();
        act.Update();
        redoToolStripMenuItem.Enabled = CurDb.hasRedo();
    }

    private void appendLinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // code provided as an illustration of user described issue
        try
        {
            CurDb.disableUndoRecording(false);
            CurDb.startUndoRecord(); //First Time
            {

                //b) We have put getBlockTableId().safeOpenObject() call in the undo record scope.
                using (OdDbBlockTableRecord pBlockTableRecord = (OdDbBlockTableRecord)CurDb.getModelSpaceId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
                {
                    OdDbLine pLine = OdDbLine.createObject();
                    OdGePoint3d spoint = new OdGePoint3d();
                    spoint.x = 66;
                    spoint.y = 86;
                    spoint.z = 0;
                    OdGePoint3d epoint = new OdGePoint3d();
                    epoint.x = 110;
                    epoint.y = 86;
                    epoint.z = 0;
                    pLine.setStartPoint(spoint);
                    pLine.setEndPoint(epoint);

                    pLine.setDatabaseDefaults(CurDb);
                    pBlockTableRecord.appendOdDbEntity(pLine);

                    // DownGrade Open
                    pBlockTableRecord.downgradeOpen();
                    pLine.Dispose();
                    spoint.Dispose();
                    epoint.Dispose();
                    pLine = null;
                    spoint = null;
                    epoint = null;
                }
            }

            CurDb.startUndoRecord();//Second Time
            {
                //b) We have put getBlockTableId().safeOpenObject() call in the undo record scope.
                using (OdDbBlockTableRecord pBlockTableRecord = (OdDbBlockTableRecord)CurDb.getModelSpaceId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
                {
                    OdDbLine pLine1 = OdDbLine.createObject();
                    OdGePoint3d spoint1 = new OdGePoint3d();
                    OdGePoint3d epoint1 = new OdGePoint3d();
                    spoint1.x = 14;
                    spoint1.y = 96;
                    spoint1.z = 0;

                    epoint1.x = 47;
                    epoint1.y = 125;
                    epoint1.z = 0;
                    pLine1.setStartPoint(spoint1);
                    pLine1.setEndPoint(epoint1);
                    pLine1.setDatabaseDefaults(CurDb);
                    pBlockTableRecord.appendOdDbEntity(pLine1);

                    //Downgrade Open
                    pBlockTableRecord.downgradeOpen();
                    pLine1.Dispose();
                    spoint1.Dispose();
                    epoint1.Dispose();
                    pLine1 = null;
                    spoint1 = null;
                    epoint1 = null;
                }
            }
            VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
            act.Invalidate();
            act.Update();
        }
        catch (OdError err)
        {
            MessageBox.Show(err.description());
        }
    }

    private void undovariant2ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            // we added 2 lines, thus we need to make 2 steps back
            ExStringIO strIO = ExStringIO.create("2");
            OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, CurDb);
            OdEdCommandStack pStack = Globals.odedRegCmds();
            Globals.odrxDynamicLinker().loadModule("DbCommands");

            pStack.executeCommand("UNDO", pCon);
            VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
            act.Invalidate();
            act.Update();
        }
        catch (OdError err)
        {
            MessageBox.Show(err.description());
        }
        redoToolStripMenuItem.Enabled = CurDb.hasRedo();
    }

    //////////////////////// Execute command //////////////////////
    //OdEdBaseIO cmd_IO = ExStringIO.create("");
    //OdDbCommandContext m_pCmdCtx = null;
    String rec_command = String.Empty;
    OdEdBaseIO cmdIO(String strIO)
    {
        return ExStringIO.create(strIO);
    }
    OdDbCommandContext cmdCtx(String strIO)
    {
      return ExDbCommandContext.createObject(cmdIO(strIO), CurDb);
    }

    public String recentCmdName()
    {
      return rec_command;
    }
    public void setRecentCmdName(String name)
    {
        rec_command = name;
    }

    private void ExecuteCommand(String sCmd, bool bEcho) 
    {
      //OdSaveState<int> save_m_nCmdActive(m_nCmdActive);
      //++m_nCmdActive;

        OdDbCommandContext pExCmdCtx = cmdCtx(sCmd);
        CmdReactor cr = new CmdReactor(pExCmdCtx); // cmd_reactor

      try
      {
        OdEdCommandStack pCommands = Globals.odedRegCmds();
        //ExDbCommandContext *pExCmdCtx = static_cast<ExDbCommandContext*>(pCmdCtx.get());
        //OdDbCommandContext pExCmdCtx = cmdCtx(sCmd);
        /*if(CurDb.appServices().getPICKFIRST())
        {
          pExCmdCtx.setPickfirst(selectionSet());
        }*/

        /*if (sCmd[0]=='(')
        {
          OdEdLispModulePtr lspMod = odrxDynamicLinker()->loadApp(OdLspModuleName);
          if (!lspMod.isNull())
            lspMod->createLispEngine()->execute(pExCmdCtx, sCmd);
        }
        else*/
        {
          String s = sCmd.Substring(0, sCmd.IndexOf(" "));//.spanExcluding(L" \t\r\n");
          if(s.Length == sCmd.Length)
          {
            s = s.ToUpper();
            cr.setLastInput(s); // cmd_reactor
            pCommands.executeCommand(s, pExCmdCtx);
          }
          else
          {
            ExStringIO m_pMacro = ExStringIO.create(sCmd);
            while(!m_pMacro.isEof())
            {
              try
              {
                  s = pExCmdCtx.userIO().getString("Command:");//(commandPrompt());
                s = s.ToUpper();
                cr.setLastInput(s); // cmd_reactor
              }
              catch(OdEdEmptyInput eEmptyInput)
              {
                s = recentCmdName();
              }
              pCommands.executeCommand(s, pExCmdCtx);
            }
          }
        }
/*
        if (getViewer())
        {
          getViewer()->Invalidate();

          getViewer()->propagateActiveViewChanges();
        }*/
      }
      catch(OdEdEmptyInput eEmptyInput)
      {
      }
      catch(OdEdCancel eCanc)
      {
      }
      catch(OdError err)
      {
        /*if(!m_bConsole)
        {
          theApp.reportError(commandMessageCaption(sCmd), err);
        }*/
        //cmdIO(sCmd).putString(err.description());
      }
      //if ((cr.isDatabaseModified() || selectionSet()->numEntities()) /*&& 0 != cr.lastInput().iCompare(_T("SELECT"))*/) // cmd_reactor
      if (cr.isDatabaseModified())
      {
        //selectionSet()->clear();
        // Call here OdExEditorObject::unselect() instead sset->clear(), if you want affect changes on grip points and etc.
        /*if (0 != cr.lastInput().iCompare(_T("SELECT")) || pCmdCtx->database()->appServices()->getPICKADD() != 2) // cmd_reactor
          OnEditClearselection();
        UpdateAllViews(0);*/
      }
      //static_cast<ExDbCommandContext*>(pCmdCtx.get())->setMacroIOPresent(false);
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CurDb.redo();
        VectorizeForm act = (VectorizeForm)this.ActiveMdiChild;
        act.Invalidate();
        act.Update();
        redoToolStripMenuItem.Enabled = CurDb.hasRedo();
    }
  };
//  public class Srv : ExHostAppServices
//  {
//    // override this method in order to be able to perform DWF export of rasters
//    public override OdGsDevice gsBitmapDevice()
//    {
//      try
//      {
//        OdGsModule pGsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv");
//        return pGsModule.createBitmapDevice();
//      }
//      catch(OdError err)
//      {
//      }
//      return base.gsBitmapDevice();
//    }
//  }
  public class CustomServices : ExHostAppServices
  {
    // override this method for some purpose as
    //    to be able to perform DWF export of rasters
    //    to be able to export shaded viewports using OpenGL device
    //    and so on..
    //
    public override OdGsDevice gsBitmapDevice(OdRxObject pViewObj, OdRxObject pDb, uint flags)
      {
        OdGsModule pGsModule = null;
        OdGsDevice gsDevice = null;
        ExHostAppServices.GsBitmapDeviceFlags deviceFlags = (ExHostAppServices.GsBitmapDeviceFlags)flags;
        
        try
        {
          ExHostAppServices.GsBitmapDeviceFlags aaa = (deviceFlags & ExHostAppServices.GsBitmapDeviceFlags.kFor2dExportRender);
          ExHostAppServices.GsBitmapDeviceFlags bbb = (deviceFlags & ExHostAppServices.GsBitmapDeviceFlags.kFor2dExportRenderHLR);
          if ((deviceFlags & ExHostAppServices.GsBitmapDeviceFlags.kFor2dExportRender) != 0)
          {
            // Don't export HiddenLine viewports as bitmap in Pdf/Dwf/Svg exports.
            if ((deviceFlags & ExHostAppServices.GsBitmapDeviceFlags.kFor2dExportRenderHLR) == 0)
            {
              // Try to export shaded viewports using OpenGL device.
              pGsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinOpenGL.txv");
            }
          }
          if (pGsModule != null)
          {
            gsDevice = pGsModule.createBitmapDevice();
          }
        }
        catch (OdError err)
        {
        }
        return gsDevice;
      }

      public override string fileDialog(int flags, string dialogCaption, string defExt, string defFilename, string filter)
      {
          SaveFileDialog dlg = new SaveFileDialog();
          dlg.DefaultExt = defExt;
          dlg.Title = dialogCaption;
          dlg.FileName = defFilename;
          dlg.Filter = filter;
          if (dlg.ShowDialog() == DialogResult.OK)
          {
              return dlg.FileName;
          }
          //throw new OdEdCancel();
          return String.Empty;
      }

      bool GetRegistryString(RegistryKey rKey, String subkey, String name, out String value)
      {
          bool rv = false;
          object objData = null;

          RegistryKey regKey;
          regKey = rKey.OpenSubKey(subkey);
          if (regKey != null)
          {
              objData = regKey.GetValue(name);
              if (objData != null)
              {
                  rv = true;
              }
              regKey.Close();
          }
          if (rv)
              value = objData.ToString();
          else
              value = String.Format("");

          rKey.Close();
          return rv;
      }

      String GetRegistryAVEMAPSFromProfile()
      {
          String subkey = GetRegistryAcadProfilesKey();
          if (subkey.Length > 0)
          {
              subkey += String.Format("\\General");
              // get the value for the ACAD entry in the registry
              String tmp;
              if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("AVEMAPS"), out tmp))
                  return tmp;
          }
          return String.Format("");
      }

      String GetRegistryAcadProfilesKey()
      {
          String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
          String tmp;

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
              return String.Format("");
          subkey += String.Format("\\{0}", tmp);

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
              return String.Format("");
          subkey += String.Format("\\{0}\\Profiles", tmp);

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
              return String.Format("");
          subkey += String.Format("\\{0}", tmp);
          return subkey;
      }

      String GetRegistryAcadLocation()
      {
          String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
          String tmp;

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
              return String.Format("");
          subkey += String.Format("\\{0}", tmp);

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
              return String.Format("");
          subkey += String.Format("\\{0}", tmp);

          if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
              return String.Format("");
          return tmp;
      }

      public String FindConfigPath(String configType)
      {
          String subkey = GetRegistryAcadProfilesKey();
          if (subkey.Length > 0)
          {
              subkey += String.Format("\\General");
              String searchPath;
              if (GetRegistryString(Registry.CurrentUser, subkey, configType, out searchPath))
                  return searchPath;
          }
          return String.Format("");
      }

      private String FindConfigFile(String configType, String file)
      {
          String searchPath = FindConfigPath(configType);
          if (searchPath.Length > 0)
          {
              searchPath = String.Format("{0}\\{1}", searchPath, file);
              if (System.IO.File.Exists(searchPath))
                  return searchPath;
          }
          return String.Format("");
      }

      String GetRegistryACADFromProfile()
      {
          String subkey = GetRegistryAcadProfilesKey();
          if (subkey.Length > 0)
          {
              subkey += String.Format("\\General");
              // get the value for the ACAD entry in the registry
              String tmp;
              if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("ACAD"), out tmp))
                  return tmp;
          }
          return String.Format("");
      }

      public override String findFile(String fileName, OdRxObject pDb, OdDbBaseHostAppServices.FindFileHint hint)
      {
          String sFile = base.findFile(fileName, pDb, hint);
          if (sFile.Length > 0)
              return sFile;

          String strFileName = fileName;
          String ext;
          if (strFileName.Length > 3)
              ext = strFileName.Substring(strFileName.Length - 4, 4).ToUpper();
          else
              ext = fileName.ToUpper();
          if (ext == String.Format(".PC3"))
              return FindConfigFile(String.Format("PrinterConfigDir"), fileName);
          if (ext == String.Format(".STB") || ext == String.Format(".CTB"))
              return FindConfigFile(String.Format("PrinterStyleSheetDir"), fileName);
              //return System.IO.Path.Combine(Environment.GetEnvironmentVariable("DDPLOTSTYLEPATHS"), fileName);
          if (ext == String.Format(".PMP"))
              return FindConfigFile(String.Format("PrinterDescDir"), fileName);

          switch (hint)
          {
              case FindFileHint.kFontFile:
              case FindFileHint.kCompiledShapeFile:
              case FindFileHint.kTrueTypeFontFile:
              case FindFileHint.kPatternFile:
              case FindFileHint.kFontMapFile:
              case FindFileHint.kTextureMapFile:
                  break;
              default:
                  return sFile;
          }

          if (hint != FindFileHint.kTextureMapFile && ext != String.Format(".SHX") && ext != String.Format(".PAT") && ext != String.Format(".TTF") && ext != String.Format(".TTC"))
          {
              strFileName += String.Format(".shx");
          }
          else if (hint == FindFileHint.kTextureMapFile)
          {
              strFileName.Replace(String.Format("/"), String.Format("\\"));
              int last = strFileName.LastIndexOf("\\");
              if (last == -1)
                  strFileName = "";
              else
                  strFileName = strFileName.Substring(0, last);
          }


          sFile = (hint != FindFileHint.kTextureMapFile) ? GetRegistryACADFromProfile() : GetRegistryAVEMAPSFromProfile();
          while (sFile.Length > 0)
          {
              int nFindStr = sFile.IndexOf(";");
              String sPath;
              if (-1 == nFindStr)
              {
                  sPath = sFile;
                  sFile = String.Format("");
              }
              else
              {
                  sPath = String.Format("{0}\\{1}", sFile.Substring(0, nFindStr), strFileName);
                  if (System.IO.File.Exists(sPath))
                  {
                      return sPath;
                  }
                  sFile = sFile.Substring(nFindStr + 1, sFile.Length - nFindStr - 1);
              }
          }

          if (hint == FindFileHint.kTextureMapFile)
          {
              return sFile;
          }

          if (sFile.Length <= 0)
          {
              String sAcadLocation = GetRegistryAcadLocation();
              if (sAcadLocation.Length > 0)
              {
                  sFile = String.Format("{0}\\Fonts\\{1}", sAcadLocation, strFileName);
                  if (System.IO.File.Exists(sFile))
                  {
                      sFile = String.Format("{0}\\Support\\{1}", sAcadLocation, strFileName);
                      if (System.IO.File.Exists(sFile))
                      {
                          sFile = String.Format("");
                      }
                  }
              }
          }
          return sFile;
      }
      private OdStreamBuf undoStr = null;
      private bool undoType = false;
      public void setUndoType(bool value) { undoType = value; }
      public bool getUndoType() { return undoType; }
      public override OdDbUndoController newUndoController()
      {
          if (null == undoStr)
          {
              undoStr = Globals.newUndoStream(this);
          }
          return Globals.newUndoController(undoType, undoStr);
      }
      public override OdStreamBuf newUndoStream()
      {
          undoStr = Globals.newUndoStream(this);
          return undoStr;
      }
  }
}
