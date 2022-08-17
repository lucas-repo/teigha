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
using System.Linq;
using System.Text;
using System.Windows.Interop;
using Teigha.Core;
using Teigha.TD;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;
//using Microsoft.Win32;

namespace WpfSample2
{
  internal class TeighaD3DImage : D3DImage
  {
    private Size NewSize = new Size(0,0);
    private OdDbDatabase TDDatabase = null;

    private delegate void LoadFileDelegate(String fPath);
    private delegate void RunTestDelegate();

    private RunTestDelegate[] testArray;
    private LoadFileDelegate loadFile = null;

    private OdRxObject odRxObjectSurface = null;
    private OdRxVariantValue surfaceVariantValue = null;

    public String FilePath
    {
      get{return pFilePath;}
      set
      {
        pFilePath = value;
        if (null != loadFile)
        {
          loadFile(pFilePath);
        }
      }
    }
    private String pFilePath = String.Empty;

    private ExSystemServices Serv = null;
    private ExHostAppServices HostApp = null;
    uint[] curPalette;
    OdGsView pView = null;

    OdGsModule pGs = null;
    private OdGsDevice mDevice = null;

    private BackgroundWorker bWrk = new BackgroundWorker();

    public TeighaD3DImage()
    {
      Serv = new ExSystemServices();
      HostApp = new CustomServices();
      TD_Db.odInitialize(Serv);
      Globals.odgsInitialize(); 
      InitializeGDIModule();
      loadFile = new LoadFileDelegate(LoadDWGFile);
      if (String.Empty != pFilePath)
      {
        loadFile(pFilePath);
        //InitializeSurface();
      }
      testArray = new RunTestDelegate[3];
      testArray[0] = new RunTestDelegate(Update1);
      testArray[1] = new RunTestDelegate(Update2);
      testArray[2] = new RunTestDelegate(Update3);

      bWrk = new BackgroundWorker();
      bWrk.WorkerReportsProgress = true;
      bWrk.WorkerSupportsCancellation = true;

      bWrk.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWrk_RunWorkerCompleted);
    }

    private void bWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        Lock();
        SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
        Unlock();

        OdRxObject.ReleasePointer(odRxObjectSurface);
        mDevice.onSize(new OdGsDCRect(0, (int)NewSize.Width, (int)NewSize.Height, 0));
        mDevice.update();
        odRxObjectSurface = mDevice.properties().getAt("D3DSurface");
        surfaceVariantValue = new OdRxVariantValue(odRxObjectSurface);
        IntPtr surface = IntPtr.Zero;
        if (IntPtr.Size == 8)
        {
            surface = (IntPtr)surfaceVariantValue.AsInt64();
        }
        else
        {
            surface = (IntPtr)surfaceVariantValue.AsInt32();
        }

        if (IntPtr.Zero != surface)
        {
            Lock();
            SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface);
            UpdateDirtyRect();
            Unlock();
        }
    }


    public void LoadDWGFile(String filePath)
    {
      TDDatabase = HostApp.readFile(filePath);
      if (null == TDDatabase)
      {
        return;
      }
      // Create a new OdGsDevice object, and associate with the vectorization GsDevice
      mDevice = pGs.createBitmapDevice();
      mDevice.properties().putAt("WindowHWND", new OdRxVariantValue(0));

      // Set the device background color and palette
      curPalette = AllPalettes.getLightPalette();
      mDevice.setBackgroundColor(curPalette[0]);
      mDevice.setLogicalPalette(curPalette, 256);

      OdGiContextForDbDatabase ctx1 = OdGiContextForDbDatabase.createObject();
      ctx1.enableGsModel(true);
      ctx1.setDatabase(TDDatabase);
      mDevice.setUserGiContext(ctx1);
      mDevice = (OdGsDevice)TD_Db.setupActiveLayoutViews(mDevice, ctx1);

      bool bModelSpace = TDDatabase.getTILEMODE();

      pView = this.mDevice.viewAt(0);
      OdAbstractViewPE pViewPe = OdAbstractViewPE.cast(pView);
      //pViewPe.zoomExtents(pView);
      if ((NewSize.Width != 0) && (NewSize.Height != 0))
      {
        mDevice.onSize(new OdGsDCRect(0, (int)NewSize.Width, (int)NewSize.Height, 0));
      }
      else
      {
        mDevice.onSize(new OdGsDCRect(0, 300, 300, 0));
      }

      InitializeSurface();
    }

    private void InitializeGDIModule()
    {
      try
      {
        pGs = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinDirectX.txv", false);
      }
      catch (Exception err)
      {
        MessageBox.Show(err.Message, "Error1");
        return;
      }
    }

    public void OnRenderSizeChanged(Size size)
    {
        // background worker is used for the following purpose:
        // resize procedure may be time-consuming and cause an improper rendering
        // placing resize procedure in a bWrk_RunWorkerCompleted handler makes it a delayed procedure
        // thus the initial image will be first stretched to new size and then the image will be redrawn
        NewSize = size;
        if (null != mDevice)
        {
            if (!bWrk.IsBusy)
            {
                bWrk.RunWorkerAsync((object)size);
            }
        }
    }

    public void InitializeSurface()
    {
      if (IsFrontBufferAvailable)
      {
        if (null != mDevice)
        {
          mDevice.update();
          if (mDevice.properties().has("D3DSurface"))
          {
            odRxObjectSurface = mDevice.properties().getAt("D3DSurface");
            surfaceVariantValue = new OdRxVariantValue(odRxObjectSurface);
            IntPtr surface = IntPtr.Zero;
            if (IntPtr.Size == 8)
            {
                surface = (IntPtr)surfaceVariantValue.AsInt64();
            }
            else
            {
                surface = (IntPtr)surfaceVariantValue.AsInt32();
            }
            if (IntPtr.Zero != surface)
            {
              Lock();
              SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface);
              UpdateDirtyRect();
              Unlock();
            }
          }
        }
      }
    }

    void UpdateDirtyRect()
    {
      Int32Rect updateRect = new Int32Rect();
      updateRect.X = updateRect.Y = 0;
      updateRect.Width = this.PixelWidth;
      updateRect.Height = this.PixelHeight;
      this.AddDirtyRect(updateRect);
    }

    public void Zoom(bool IsZoomIn)
    {
      if (pView != null)
      {
        Lock();
        pView.zoom(IsZoomIn ? 1.0 / 0.9 : 0.9);
        mDevice.update();
        UpdateDirtyRect();
        Unlock();
      }
    }
    
    public void Dolly(double x, double y, double z)
    {
      if (pView != null)
      {
        Lock();
        pView.dolly(x/100, y/100, z);
        mDevice.update();
        UpdateDirtyRect();
        Unlock();
      }
    }
    
    public void DrawLine()
    {
      if (pView != null)
      {
        Lock();
        using (OdDbLine l = OdDbLine.createObject())
        {
          l.setStartPoint(new OdGePoint3d(0, 0, 0));
          l.setEndPoint(new OdGePoint3d(1000, 1000, 0));
          l.setLineWeight(LineWeight.kLnWt200);


          OdDbBlockTableRecord model = (OdDbBlockTableRecord)TDDatabase.getModelSpaceId().safeOpenObject(OpenMode.kForWrite);

          model.appendOdDbEntity(l);
        }
        mDevice.invalidate();
        mDevice.update();
        UpdateDirtyRect();
        Unlock();
      }
    }

    private void Update1() // export to pdf

    {
      OdRxModule mod = Globals.odrxDynamicLinker().loadApp("TD_PdfExport");
      if (null == mod)
      {
        MessageBox.Show("Failed to load PDF export module", "Error");
      }

      Microsoft.Win32.SaveFileDialog dlgPdf = new Microsoft.Win32.SaveFileDialog();
      dlgPdf.DefaultExt = ".pdf";
      dlgPdf.Title = "PDF Export default";
      dlgPdf.Filter = "PDF files|*.pdf";
      // Display SaveFileDialog by calling ShowDialog method 
      if (dlgPdf.ShowDialog() != true)
      {
        return;
      }

      using (PDFExportParams Params = new PDFExportParams())
      {
        OdGsPageParams pParams = new OdGsPageParams();

        Params.setDatabase(TDDatabase);
        Params.setVersion(PDFExportParams.PDFExportVersions.kPDFv1_5);

        using (OdStreamBuf file = Globals.odrxSystemServices().createFile(dlgPdf.FileName, FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways))
        {
            Params.setOutput(file);
            Params.setExportFlags(
                         (PDFExportParams.PDFExportFlags.kEmbededTTF) |
                         (PDFExportParams.PDFExportFlags.kSHXTextAsGeometry) | // no in sample
                         //(PDFExportParams.PDFExportFlags.kTTFTextAsGeometry) | // no in sample
                         (PDFExportParams.PDFExportFlags.kSimpleGeomOptimization) |
                         (PDFExportParams.PDFExportFlags.kZoomToExtentsMode) |
                         (PDFExportParams.PDFExportFlags.kEnableLayers) | // under condition
                         (PDFExportParams.PDFExportFlags.kIncludeOffLayers) | // under condition
                            (PDFExportParams.PDFExportFlags.kUseHLR) |
                            (PDFExportParams.PDFExportFlags.kFlateCompression) |
                            (PDFExportParams.PDFExportFlags.kASCIIHexEncoding) |
                            (PDFExportParams.PDFExportFlags.kExportHyperlinks));

          Params.setTitle(dlgPdf.Title);
          Params.setAuthor("WpfSample2");
          Params.setSubject("WpfSample2");
          Params.setKeywords("WpfSample2");
          Params.setCreator("WpfSample2");
          Params.setProducer("WpfSample2");

          UInt32[] CurPalette = AllPalettes.getLightPalette();// OdDgColorTable.currentPalette(CurDb);
          CurPalette[255] = 0x00ffffff; // the same as ODRGB(255, 255, 255); in the similar C++ code extract
          //OdDbColorTable.correctPaletteForWhiteBackground(CurPalette);

          Params.Palette = CurPalette;
          Params.setBackground(CurPalette[0]);

          /*if (dlgPdf.m_Layouts == 1) // all
          {
            OdDbModelTable pModelTable = TDDatabase.getModelTable();
            if (null != pModelTable)
            {
              OdDgElementIterator pIter = pModelTable.createIterator();
              for (; !pIter.done(); pIter.step())
              {
                OdDgModel pModel = OdDgModel.cast(pIter.item().openObject());
                if (null != pModel)
                {
                  Params.layouts.Add(pModel.getName());
                }
              }
            }
          }*/
    
          /*Params.layouts.Add(TDDatabase.findActiveLayout(true));

          UInt32 nPages = (UInt32)(1 > Params.layouts.Count ? 1 : Params.layouts.Count);

          OdGsPageParams pageParams = new OdGsPageParams();
          pageParams.set(210, 295);
          Params.pageParams.resize(nPages);*/
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
    
    private void Update2()
    {
      Lock();
      mDevice.update();
      UpdateDirtyRect();
      Unlock();
    }
    private void Update3()
    {
      Lock();
      mDevice.invalidate();
      UpdateDirtyRect();
      Unlock();
    }

    public void RunTest(int number)
    {
      testArray[number]();
    }

  }
  public class CustomServices : ExHostAppServices
  {
    public override string fileDialog(int flags, string dialogCaption, string defExt, string defFilename, string filter)
    {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

      dlg.DefaultExt = defExt;
      dlg.Title = dialogCaption;
      dlg.FileName = defFilename;
      dlg.Filter = filter;
      // Display SaveFileDialog by calling ShowDialog method 
      if (dlg.ShowDialog() == true)
      {
        return dlg.FileName;
      }
      throw new OdEdCancel();
    }
  }
}
