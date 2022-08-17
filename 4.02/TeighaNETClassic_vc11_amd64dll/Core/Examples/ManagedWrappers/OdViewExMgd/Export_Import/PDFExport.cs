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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Export_Import;

namespace OdViewExMgd
{
  public partial class PDFExport : Form
  {
    Database database;
    public PDFExport(Database db)
    {
      InitializeComponent();

      database = db;
      PageParams pp = new PageParams();
      PapWidth.Text = pp.PaperWidth.ToString();
      PapHeight.Text = pp.PaperHeight.ToString();
      using(mPDFExportParams param = new mPDFExportParams())
      {
        HatchToBMP.Text = param.hatchDPI.ToString();
      }
    }

    private void btBrowse_Click(object sender, EventArgs e)
    {
      if(DialogResult.OK == saveFileDialog.ShowDialog())
      {
        outputFile.Text = saveFileDialog.FileName;
      }
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void Export_Click(object sender, EventArgs e)
    {
      if(outputFile.Text.Length > 0)
      {
        using (mPDFExportParams param = new mPDFExportParams())
        {
          param.Database = database;

          TransactionManager tm = database.TransactionManager;
          using (Transaction ta = tm.StartTransaction())
          {
            using (FileStreamBuf fileStrem = new FileStreamBuf(outputFile.Text, false, FileShareMode.DenyNo, FileCreationDisposition.CreateAlways))
            {
              param.OutputStream = fileStrem;

              param.Flags = (embedded_fonts.Checked ? PDFExportFlags.EmbededTTF : 0) |
                            (SHXTextAsGeometry.Checked ? PDFExportFlags.SHXTextAsGeometry : 0) |
                            (TTGeometry.Checked ? PDFExportFlags.TTFTextAsGeometry : 0) |
                            (ESimpGeometryOpt.Checked ? PDFExportFlags.SimpleGeomOptimization : 0) |
                            (ZoomExtents.Checked ? PDFExportFlags.ZoomToExtentsMode : 0) |
                            (EnableLayerSup_pdfv1_5.Checked ? PDFExportFlags.EnableLayers : 0) |
                            (ExportOffLay.Checked ? PDFExportFlags.IncludeOffLayers : 0);

              param.Title = textBox_title.Text;
              param.Author = textBox_author.Text;
              param.Subject = textBox_subject.Text;
              param.Keywords = textBox_keywords.Text;
              param.Creator = textBox_creator.Text;
              param.Producer = textBox_producer.Text;
              param.UseHLR = UseHidLRAlgorithm.Checked;
              param.FlateCompression = EncodedSZ.Checked;
              param.ASCIIHEXEncodeStream = checkBoxASCIIHEXencoded.Checked;
              param.hatchDPI = uint.Parse(HatchToBMP.Text);

              bool bV15 = EnableLayerSup_pdfv1_5.Checked || ExportOffLay.Checked;
              param.Versions = bV15 ? PDFExportVersions.PDFv1_5 : PDFExportVersions.PDFv1_4;

              PlotSettingsValidator plotSettingVal = PlotSettingsValidator.Current;

              StringCollection styleCol = plotSettingVal.GetPlotStyleSheetList();
              int iIndexStyle = checkBoxMonochrome.Checked?styleCol.IndexOf(String.Format("monochrome.ctb")):-1;

              StringCollection strColl = new StringCollection();
              if (radioButton_All.Checked)
              {
                using (DBDictionary layouts = (DBDictionary)database.LayoutDictionaryId.GetObject(OpenMode.ForRead))
                {
                  foreach (DBDictionaryEntry entry in layouts)
                  {
                    if ("Model" == entry.Key)
                      strColl.Insert(0, entry.Key);
                    else
                      strColl.Add(entry.Key);
                    if (-1 != iIndexStyle)
                    {
                      PlotSettings ps = (PlotSettings)ta.GetObject(entry.Value, OpenMode.ForWrite);
                      plotSettingVal.SetCurrentStyleSheet(ps, styleCol[iIndexStyle]);
                    }
                  }
                }
              }
              else if (-1 != iIndexStyle)
              {
                using (BlockTableRecord paperBTR = (BlockTableRecord)database.CurrentSpaceId.GetObject(OpenMode.ForRead))
                {
                  using (PlotSettings pLayout = (PlotSettings)paperBTR.LayoutId.GetObject(OpenMode.ForWrite))
                  {
                    plotSettingVal.SetCurrentStyleSheet(pLayout, styleCol[iIndexStyle]);
                  }
                }
              }
              param.Layouts = strColl;

              int nPages = Math.Max(1, strColl.Count);
              PageParamsCollection pParCol = new PageParamsCollection();
              Double width = Double.Parse(PapWidth.Text);
              Double height = Double.Parse(PapHeight.Text);
              for (int i = 0; i < nPages; ++i)
              {
                PageParams pp = new PageParams();
                pp.setParams(width, height);
                pParCol.Add(pp);
              }
              param.PageParams = pParCol;
              Export_Import.ExportPDF(param);
            }
            ta.Abort();
          }
        }
        Close();
      }
    }

    private void EnableLayerSup_pdfv1_5_CheckedChanged(object sender, EventArgs e)
    {
      ExportOffLay.Enabled = EnableLayerSup_pdfv1_5.Checked;
      if (!EnableLayerSup_pdfv1_5.Checked)
        ExportOffLay.Checked = EnableLayerSup_pdfv1_5.Checked;
    }
  }
}