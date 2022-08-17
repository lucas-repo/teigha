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
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Teigha;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.DatabaseServices;
using Teigha.Export_Import;


namespace OdViewExMgd
{
  class ImExport
  {
    public static void SVG_export(Database database)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.AddExtension = true;
      sfd.DefaultExt = "svg";
      sfd.Filter = "SVG files (*.svg)|*.svg";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        // SVG export is a rendering device too
        using (GsModule gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("TD_SvgExport", false, true))
        {
          if (gsModule == null) // if SVG export module is not found
            return;
          // create graphics device
          using (Teigha.GraphicsSystem.Device dev = gsModule.CreateDevice())
          {
            using(FileStreamBuf outStr = new FileStreamBuf(sfd.FileName, false, FileShareMode.DenyWrite, FileCreationDisposition.CreateAlways))
            {
              // setup device properties
              using (Dictionary props = dev.Properties)
              {

                // SVG export uses stream output
                props.AtPut("Output", outStr);
                // lines may look thin - may be scaled
                props.AtPut("LineWeightScale", new RxVariant(1.0));
                // number of digits after decimal point in the output file
                props.AtPut("Precision", new RxVariant(6));
                // place to put images (may be relative path)
                props.AtPut("ImageBase", new RxVariant(System.IO.Path.GetDirectoryName(sfd.FileName)));
                // missing font
                props.AtPut("GenericFontFamily", new RxVariant("sans-serif"));
                // software HLR engine enabled
                props.AtPut("UseHLR", new RxVariant(true));
                // whether to use blended gradients for complex gradient fill (may increase size)
                props.AtPut("EnableGouraudShading", new RxVariant(true));
              }
              dev.SetLogicalPalette(Device.LightPalette); // light palette with white background
              ContextForDbDatabase ctx = new ContextForDbDatabase(database);
              ctx.SetPlotGeneration(true);
              LayoutHelperDevice hd = LayoutHelperDevice.SetupActiveLayoutViews(dev, ctx);
              // size is SVG viewbox
              hd.OnSize(new System.Drawing.Rectangle(0, 0, 1024, 768));
              hd.Update();

              hd.Dispose();
            }
          }
        }
      }
    }

    public static void DWF_export(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.AddExtension = true;
      sfd.DefaultExt = "dwf";
      sfd.Title = "Export To Dwf";
      sfd.Filter = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
        "Binary DWF (Desgin Web Format) v6.0 (*.dwf)|*.dwf",
        "Zipped Ascii Encoded 2D Stream DWF (Design Web Format) v6.0 (*.dwf)|*.dwf",
        "XPS DWF (*.dwfx)|*.dwfx",
        "Compressed DWF (Design Web Format) v5.5 (*.dwf)|*.dwf",
        "Binary DWF (Design Web Format) v5.5 (*.dwf)|*.dwf",
        "Ascii DWF (Design Web Format) v5.5 (*.dwf)|*.dwf",
        "Compressed DWF (Design Web Format) v4.2 (*.dwf)|*.dwf",
        "Binary DWF (Design Web Format) v4.2 (*.dwf)|*.dwf",
        "Ascii DWF (Design Web Format) v4.2 (*.dwf)|*.dwf");
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        String strFileName = sfd.FileName;
        using (mDwfExportParams param = new mDwfExportParams())
        {
          param.Database            = database;
          param.FileName            = strFileName;
          param.BackgroundColor     = helperDevice.BackgroundColor;
          param.InkedArea           = false;
          param.ColorMapOptimize    = false;
          param.ExportInvisibleText = true;
	  param.EmbedAllFonts = true;
          DwfPageDataCollection pageDataColl = new DwfPageDataCollection();
          DWFPageData pageData;
          switch(sfd.FilterIndex)
          {
            case 1:
              param.Format  = DwfFormat.UNCOMPRESSED_BINARY;
              param.Version = DwfVersion.Dwf_v60;
              
              pageData = new DWFPageData();
              pageData.Layout = LayoutManager.Current.CurrentLayout;
              pageDataColl.Add(pageData);
              param.PageData = pageDataColl;
              break;
            case 2:
              param.Format    = DwfFormat.ASCII;
              param.Version   = DwfVersion.Dwf_v60;

              pageData = new DWFPageData();
              pageData.Layout = LayoutManager.Current.CurrentLayout;
              pageDataColl.Add(pageData);
              param.PageData = pageDataColl;
              break;
            case 3:
              param.Format = DwfFormat.XPS;
              param.Version = DwfVersion.Dwf_v60;
              break;
            case 4:
              param.Format = DwfFormat.COMPRESSED_BINARY;
              param.Version = DwfVersion.Dwf_v55;
              break;
            case 5:
              param.Format = DwfFormat.UNCOMPRESSED_BINARY;
              param.Version = DwfVersion.Dwf_v55;
              break;
            case 6:
              param.Format = DwfFormat.ASCII;
              param.Version = DwfVersion.Dwf_v55;
              break;
            case 7:
              param.Format = DwfFormat.COMPRESSED_BINARY;
              param.Version = DwfVersion.Dwf_v42;
              break;
            case 8:
              param.Format = DwfFormat.UNCOMPRESSED_BINARY;
              param.Version = DwfVersion.Dwf_v42;
              break;
            case 9:
              param.Format = DwfFormat.ASCII;
              param.Version = DwfVersion.Dwf_v42;
              break;
            default:
              return;
          }
          Export_Import.ExportDwf(param);
        }
      }
    }

    public static void Publish3d(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.AddExtension = true;
      sfd.DefaultExt = "dwf";
      sfd.Title = "3D DWF Publish";
      sfd.Filter = String.Format("3D DWF (Desgin Web Format) v6.01 (*.dwf)|*.dwf");
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        using (mDwf3dExportParams param = new mDwf3dExportParams())
        {
          param.Database = database;
          param.FileName = sfd.FileName;
          param.BackgroundColor = helperDevice.BackgroundColor;
          Export_Import.Publish3d(param);
        }
      }
    }

    public static void Publish(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice)
    {
      using (mDwfExportParams param = new mDwfExportParams())
      {
        param.Database           = database;
        param.BackgroundColor    = helperDevice.BackgroundColor;
        param.InkedArea          = false;
        param.ColorMapOptimize   = false;
        PublishDrawingSheets pds = new PublishDrawingSheets(param);
        if(DialogResult.OK == pds.ShowDialog())
        {
          Export_Import.ExportDwf(param);
        }
      }
    }

    public static void Dwf_import(Database database)
    {
      OpenFileDialog saveDialog = new OpenFileDialog();
      saveDialog.Filter = String.Format("DWF (Desgin Web Format) (*.dwf)|*.dwf");
      if (DialogResult.OK == saveDialog.ShowDialog())
      {
        SystemObjects.DynamicLinker.LoadApp("TD_Dwf7Import", false, false);
        using (DWFImport dwfimp = new DWFImport())
        {
          using (Dictionary props = dwfimp.Properties)
          {
            if (props.Contains("Database")) 
              props.AtPut("Database", database);
            if (props.Contains("DwfPath")) // Check if property is supported
              props.AtPut("DwfPath", new RxVariant(saveDialog.FileName));
            if (props.Contains("Password")) // Check if property is supported
              props.AtPut("Password", new RxVariant(String.Format("")));
            if (props.Contains("PaperWidth")) // Check if property is supported
              props.AtPut("PaperWidth", new RxVariant(297.0));
            if (props.Contains("PaperHeight")) // Check if property is supported
              props.AtPut("PaperHeight", new RxVariant(210.0));
            if (props.Contains("PreserveColorIndices")) // Check if property is supported
              props.AtPut("PreserveColorIndices", new RxVariant(true));
            if (props.Contains("LayoutNumber")) // Check if property is supported
              props.AtPut("LayoutNumber", new RxVariant(-1));
            if (props.Contains("ImportW3d")) // Check if property is supported
              props.AtPut("ImportW3d", new RxVariant(true));
          }
          dwfimp.Import();
        }
      }
    }

    public static void Image_export(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice, int pWidth, int pHeight)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.AddExtension = true;
      saveFileDialog.Filter = "BMP files (*.bmp)|*.bmp|JPG files (*.jpg)|*.jpg|TIFF files (*.tiff)|*.tiff";

      saveFileDialog.DefaultExt = "bmp";

      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        using (GsModule gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("WinDirectX.txv", false, true))
        {
          // create graphics device
          using (Teigha.GraphicsSystem.Device dev = gsModule.CreateBitmapDevice())
          {
            // setup device properties
            using (ContextForDbDatabase ctx = new ContextForDbDatabase(database))
            {
              using (LayoutHelperDevice helperDeviceImg = LayoutHelperDevice.SetupActiveLayoutViews(dev, ctx))
              {
                helperDeviceImg.SetLogicalPalette(Device.DarkPalette); // DarkPalette palette. The same like in Form1
                Rectangle rect = new Rectangle(0, 0, pWidth, pHeight);
                helperDeviceImg.OnSize(rect);

                int iNViews = helperDeviceImg.NumViews;
                for (int iTmp = 0; iTmp < iNViews; ++iTmp)
                {
                  using (AbstractViewPE viewPe = new AbstractViewPE(helperDeviceImg.ViewAt(iTmp)))
                  {
                    viewPe.SetView(helperDevice.ViewAt(iTmp));
                  }
                }
                helperDeviceImg.Update();

                using (Dictionary props = dev.Properties)
                {
                  RXObject obj = props.At("RasterImage");
                  if (obj is GIRasterImage)
                  {
                    GIRasterImage rasImg = obj as GIRasterImage;

                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pWidth, pHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    byte[] rgbValues = rasImg.ScanLines();
                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pWidth, pHeight), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
                    bitmap.UnlockBits(bmpData);
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    int iIndex = saveFileDialog.FilterIndex;
                    if (1 == iIndex)
                      bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    if (2 == iIndex)
                      bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    if (3 == iIndex)
                      bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                  }
                  obj.Dispose();
                }
              }
            }
          }
        }
      }
    }
  }
}
