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
using System.Text;
using System.Windows.Forms;
using Teigha.Core;
using Teigha.TG;

namespace OdaDgnAppMgd
{
  public partial class Form1 : Form
  {
    OdDgHostAppServices _hostApp;
    OdRxSystemServices _sysSrv;
    OdDgDumper m_dumper;
    OdDgDatabase CurDb;
    MemoryManager mMan = null;
    MemoryTransaction mStartTrans = null;
    public Form1()
    {
        mMan = MemoryManager.GetMemoryManager();
        mStartTrans = mMan.StartTransaction();
      #region initialization
      InitializeComponent();
      _sysSrv = new OdExDgnSystemServices();
      Teigha.Core.Globals.odrxInitialize(_sysSrv);
      Teigha.Core.Globals.odgsInitialize();
      Teigha.Core.Globals.odrxDynamicLinker().loadModule("TG_Db");
      Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_Db.dll", false);
      
      // mind derived Srv is used as we need to override gsBitmapDevice method
      //_hostApp = new OdExDgnHostAppServices();
      _hostApp = new Srv();
      m_dumper = new OdDgDumper();
      #endregion
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        mMan.StopTransaction(mStartTrans);
        mMan.StopAll();
      this.Close();
    }

    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (openDgnDialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          OdDgDatabase db = _hostApp.readFile(openDgnDialog.FileName);
          Tree t = new Tree(db);
          t.FormClosed += new FormClosedEventHandler(TreeFormClosed);
          t.MdiParent = this;
          t.Show();
          t.FillTree();
          CurDb = db;
          //exportSVGToolStripMenuItem.Enabled = true;
          UpdateMainMenu(true);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
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
      ExportSTL.Visible = flag;
    }

    public void setStatus(string s)
    {
      this.toolStripStatusLabel1.Text = s;
    }
    public VectorizeForm createVectorizationWindow(Tree t, OdDgElementId vectorizedViewId, OdDgElementId vectorizedModelId)
    {
      VectorizeForm v = new VectorizeForm(t, vectorizedViewId, vectorizedModelId);
      v.MdiParent = this;
      v.Show();
      return v;
    }

    // It is simply an example of deriving classes. Nothing more.
    public class OdGiContextForDgDatabaseToExportMine : OdGiContextForDgDatabaseToExport
    {
      public override bool getWeight( uint weight, out uint resultWeight, OdDgLevelTableRecord level)
      {
          resultWeight = 2;
          return false;
      }
    }
        
    private void ExportSVG_Click(object sender, EventArgs e)
    {
        MemoryTransaction mTr = mMan.StartTransaction();
      OdDgDatabase db = CurDb;
      OdDgViewGroupTable table = db.getViewGroupTable();
      OdDgElementIterator it = table.createIterator();
      SaveFileDialog saveSvgDialog = new SaveFileDialog();
      saveSvgDialog.Filter = "SVG files|*.svg";

      for (; !it.done(); it.step())
      {
        OdDgViewGroup viewGroup = OdDgViewGroup.cast(it.item().openObject(OpenMode.kForWrite));
        {
          OdDgElementIterator it1 = viewGroup.createIterator();
          for (; !it1.done(); it1.step())
          {
            OdDgView view = OdDgView.cast(it1.item().openObject(OpenMode.kForWrite));
            bool flag = view.getVisibleFlag();
            if (flag)
            {
              if (saveSvgDialog.ShowDialog() != DialogResult.OK)
              {
                return;
              }
              OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
              if (null == pModule)
              {
                MessageBox.Show("TD_SvgExport.tx is missing");
              }
              else
              {
                String fileName = saveSvgDialog.FileName;
                OdStreamBuf file = Globals.odrxSystemServices().createFile(fileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways);
                if (null != file)
                {
                  OdGsDevice dev = pModule.createDevice();
                  if (null != dev)
                  {
                    dev.properties().putAt("Output", file);
                    // size of pixel in device units
                    dev.properties().putAt("LineWeightScale", new OdRxVariantValue(1.0));//1.
                    // size of pixel in device units
                    dev.properties().putAt("Precision", new OdRxVariantValue(9));
                    // where copy images
                    String s_base = saveSvgDialog.FileName;
                    s_base = s_base.Substring(0, s_base.LastIndexOf("\\"));
                    dev.properties().putAt("ImageBase", new OdRxVariantValue(s_base));
                    // prefix to prepend to image name
                    dev.properties().putAt("ImageUrl", new OdRxVariantValue("./"));
                    // default image format
                    dev.properties().putAt("DefaultImageExt", new OdRxVariantValue(".png"));
                    OdDgModel pModel = (OdDgModel)db.getActiveModelId().safeOpenObject();
                    dev.properties().putAt("BackGround", new OdRxVariantValue(pModel.getBackground()));
                    ////////////////////
                    // Set active palette
                    uint[] refClr = OdDgColorTable.currentPalette(db);
                    //bool bCorrected = OdDgColorTable.correctPaletteForWhiteBackground(refClr);
                    refClr[255] = pModel.getBackground();
                    dev.setLogicalPalette(refClr, 256);
                    dev.setBackgroundColor(refClr[255]);
                    
                    // Prepare database context for device
                    OdGiContextForDgDatabase pDgnContext = OdGiContextForDgDatabaseToExport.createObject();
                    pDgnContext.setDatabase(db);
                    // do not render paper
                    pDgnContext.setPlotGeneration(true);
                    // Prepare the device to render the active layout in this database.
                    OdDbBaseDatabasePE pDbPE = OdDbBaseDatabasePE.cast(db);
                    OdGsDevice wrapper = pDbPE.setupActiveLayoutViews(dev, pDgnContext);
                    // Setup device coordinate space
                    wrapper.onSize(new OdGsDCRect(0, 1024, 768, 0));
                    // Initiate rendering.
                    wrapper.update();
                  }
                }
              }
            }
          }
        }
      }
      mMan.StopTransaction(mTr);
    }

    private void fillLayoutList(OdStringArray aLayouts, bool bAllLayouts)
    {
      if (bAllLayouts == true)
      {
        OdDgModelTable pModelTable = CurDb.getModelTable();
        if (pModelTable != null)
        {
          OdDgElementIterator pIter = pModelTable.createIterator();
          for ( ; !pIter.done(); pIter.step() )
          {
            OdDgModel pModel = OdDgModel.cast( pIter.item().openObject() );
            if (null != pModel)
            {
              aLayouts.Add(pModel.getName());
            }
          }
        }
      }  
    }
    private void ExportPDF_Click(object sender, EventArgs e)
    {
        MemoryTransaction mTr = mMan.StartTransaction();
        OdRxModule mod = Globals.odrxDynamicLinker().loadApp("TD_PdfExport");
      if (null == mod)
      {
        MessageBox.Show("Failed to load PDF export module", "Error");
      }

      Export2PdfForm dlgPdf = new Export2PdfForm();
      //using (PDFExportParams Params = new PDFExportParams())
      PDFExportParams Params = new PDFExportParams();
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

        //using (OdStreamBuf file = Globals.odrxSystemServices().createFile(dlgPdf.m_FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways))
        OdStreamBuf file = Globals.odrxSystemServices().createFile(dlgPdf.m_FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways);
        {
          OdGiContextForDgDatabaseToExportMine pCtx = new OdGiContextForDgDatabaseToExportMine();
          pCtx.setDatabase(CurDb);
          Params.setTGSpecific(true, pCtx);
          OdGiDefaultContext ctx = Params.giContext();
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
                            (dlgPdf.m_bUseHLR ? PDFExportParams.PDFExportFlags.kExportHyperlinks : 0));


          Params.setTitle(dlgPdf.m_Title);
          Params.setAuthor(dlgPdf.m_Author);
          Params.setSubject(dlgPdf.m_Subject);
          Params.setKeywords(dlgPdf.m_Keywords);
          Params.setCreator(dlgPdf.m_Creator);
          Params.setProducer(dlgPdf.m_Producer);

          UInt32[] CurPalette = OdDgColorTable.currentPalette(CurDb);
          CurPalette[255] = 0x00ffffff; // the same as ODRGB(255, 255, 255); in the similar C++ code extract
          OdDgColorTable.correctPaletteForWhiteBackground(CurPalette);

          Params.Palette = CurPalette;

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
      mMan.StopTransaction(mTr);
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

    private void ExportDWF_Click(object sender, EventArgs e)
    {
      MemoryTransaction mTr = mMan.StartTransaction();
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
    OdGiContextForDgDatabaseToExportMine pCtx = new OdGiContextForDgDatabaseToExportMine();
    //OdGiContextForDgDatabaseToExport pCtx = OdGiContextForDgDatabaseToExport.createObject();
    uint iweight = 0;
    bool dRet = pCtx.getWeight(4, out iweight, null);

    pCtx.setDatabase(CurDb);
    Params.setDatabase(pCtx.database());
    Params.setDwfFileName(dwfDlg.FileName);

    // MKU 03\01\02 - resolved "white background issue"
    UInt32[] refColors = OdDgColorTable.currentPalette(CurDb);
    OdDgModel pModel = (OdDgModel)CurDb.getActiveModelId().safeOpenObject();
    UInt32 background = pModel.getBackground();
    refColors[255] = background;

    // This method should be called to resolve "white background issue" before setting device palette
    //bool bCorrected = OdDgColorTable.correctPaletteForWhiteBackground(refColors);
    Params.Palette = refColors;
    Params.setBackground(refColors[255]);

    Params.setInkedArea(false);                                                        // MKU 1/21/2004
    Params.setColorMapOptimize(false);                                                 // MKU 1/21/2004
    Params.setExportInvisibleText(true);
    //params.bForceInitialViewToExtents = true;

    DwfPageData pageData = new DwfPageData();
        //OdDgElementId pId = m_pDb->getActiveModelId();
        //OdDgModelPtr pModel = OdDgModel::cast(pId.openObject());
    DwfPageDataArray pdArray = new DwfPageDataArray();
    switch(dwfDlg.FilterIndex)
    {
      case 1:
        Params.setFormat(DW_FORMAT.DW_UNCOMPRESSED_BINARY);
        Params.setVersion(DwfVersion.nDwf_v60);
        
        pageData.sLayout = pModel.getName();//m_pDb->findActiveLayout(true);
        pdArray.Add(pageData);
        break;
      case 2:
        Params.setFormat(DW_FORMAT.DW_ASCII);
        Params.setVersion(DwfVersion.nDwf_v60);
        pageData.sLayout = pModel.getName();//m_pDb->findActiveLayout(true);
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
        pageData.sLayout = pModel.getName();//m_pDb->findActiveLayout(true);
        pdArray.Add(pageData);
        break;
      default:
        //ASSERT(0);
        return;
    }
    Params.setPageData(pdArray);
    OdDwfExport dwf_exp = dwf_mod.create();
    dwf_exp.exportDwf(Params);
    mMan.StopTransaction(mTr);
///////////////////////////
    }

    private void ExportSTL_Click(object sender, EventArgs e)
    {
        MemoryTransaction mTr = mMan.StartTransaction();
        Export2STLDlg dlg = new Export2STLDlg();
        if (DialogResult.OK == dlg.ShowDialog())
        {
            OdRxModule tmp_mod = Globals.odrxDynamicLinker().loadModule("TD_STLExport");
            STLModule module = new STLModule(OdRxModule.getCPtr(tmp_mod).Handle, false);
            OdDbHandle han = new OdDbHandle(dlg.HandleId);
            OdDgElementId targetId = CurDb.getElementId(han);
            if (targetId.isNull())
            {
                MessageBox.Show("Element not found", "Error");
            }
            else
            {
                OdRxObject pCurObj = targetId.safeOpenObject(OpenMode.kForRead);
                OdGiDrawable pDr = OdGiDrawable.cast(pCurObj);
                if (null == pDr)
                {
                    MessageBox.Show("Not drawable element", "Error");
                }
                else
                {
                    using (OdStreamBuf file = Globals.odrxSystemServices().createFile(dlg.FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways))
                    {
                        OdGeExtents3d extents = new OdGeExtents3d();
                        // !!! calling an OdGiDrawable method getGeomExrents will give a wrong result
                        // !!! we work with a DGN file, thus we should cast to OdDgElement
                        OdDgElement pElt = OdDgElement.cast(pCurObj);
                        pElt.getGeomExtents(extents);

                        double deviation = extents.minPoint().distanceTo(extents.maxPoint()) / 100;

                        OdResult res = module.exportSTL(CurDb, pDr, file, dlg.Binary == false, deviation);
                        if (res != OdResult.eOk)
                        {
                            OdError tmp_err = new OdError(res);
                            MessageBox.Show(tmp_err.description(), "Error");
                        }
                    }
                }
            }
            //han.Dispose();
            //module.Dispose();
        }
        mMan.StopTransaction(mTr);
    }
  };
  public class Srv : OdExDgnHostAppServices
  {
    // override this method in order to be able to perform DWF export of rasters
    public override OdGsDevice gsBitmapDevice()
    {
      try
      {
        OdGsModule pGsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv");
        return pGsModule.createBitmapDevice();
      }
      catch(OdError err)
      {
      }
      return base.gsBitmapDevice();
    }
  }
}
