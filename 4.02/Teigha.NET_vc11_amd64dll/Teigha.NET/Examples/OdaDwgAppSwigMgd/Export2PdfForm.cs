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
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OdaDwgAppMgd
{
  public partial class Export2PdfForm : Form
  {
    #region private variables
    private double PaperW;
    private double PaperH;
    private ushort HatchDPI;
    private ushort iGeomDPI;
    private bool pEmbeddedFonts;
    private bool pTrueType;
    private bool pSHXText;
    private bool pEnableOptimization;
    private bool pZoomToExtents;
    private bool pHiddenAlgorithm;
    private bool pLayerSupport;
    private bool pOffLayers;
    private bool pEncoded;
    private String pPath;
    private String pTitle;
    private String pAuthor;
    private String pSubject;
    private String pKeywords;
    private String pCreator;
    private String pProducer;
    private bool bLayouts;
    private bool bExportHyperlinks;
    #endregion

    #region public variables
    public double m_dPaperW
    {
      get { return PaperW; }
      set { PaperW = value; }
    }
    public double m_dPaperH
    {
      get { return PaperH; }
      set { PaperH = value; }
    }
    public ushort m_dHatchDPI
    {
      get { return HatchDPI; }
      set { HatchDPI = value; }
    }
    public ushort m_diGeomDPI
    {
      get { return iGeomDPI; }
      set { iGeomDPI = value; }
    }
    public bool m_bEmbedded
    {
      get { return pEmbeddedFonts; }
    }
    public bool m_bSimpleGeomOpt
    {
      get { return pEnableOptimization; }
    }
    public bool m_bSHXAsGeometry
    {
      get { return pSHXText; }
    }
    public bool m_bTTFAsGeometry
    {
      get { return pTrueType; }
    }
    public bool m_bZoomToExtents
    {
      get { return pZoomToExtents; }
    }
    public bool m_bUseHLR
    {
      get { return pHiddenAlgorithm; }
    }
    public bool m_bEnableLayers
    {
      get { return pLayerSupport; }
    }
    public bool m_bExportOffLayers
    {
      get { return pOffLayers; }
      set { pOffLayers = value; }
    }
    public bool m_bEncoded
    {
      get { return pEncoded; }
    }
    public bool m_Layouts
    {
      get { return bLayouts; }
      set { bLayouts = value; }
    }
    public bool m_bExportHyperlinks
    {
      get { return bExportHyperlinks; }
    }
    public String m_FileName
    {
      get { return pPath; }
    }
    public String m_Title    
    {
      get { return pTitle; }
    }
    public String m_Author
    {
      get { return pAuthor; }
    }
    public String m_Subject
    {
      get { return pSubject; }
    }
    public String m_Keywords
    {
      get { return pKeywords; }
    }
    public String m_Creator
    {
      get { return pCreator; }
    }
    public String m_Producer
    {
      get { return pProducer; }
    }
    #endregion

    public Export2PdfForm()
    {
      InitializeComponent();
      PaperWidthString.Text = PaperW.ToString();
      PaperHeightString.Text = PaperH.ToString();
      HatchDPI = 150;
      iGeomDPI = 600;
      textBox1.Text = HatchDPI.ToString();
      textBox2.Text = iGeomDPI.ToString();
      Off_layers.Checked = pOffLayers;
      All.Checked = bLayouts;
      Active.Checked = !All.Checked;
    }

    private void Browse_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveSvgDialog = new SaveFileDialog();
      saveSvgDialog.Filter = "PDF files|*.pdf";
      if (saveSvgDialog.ShowDialog() != DialogResult.OK)
      {
        return;
      }
      PdfFilePathString.Text = saveSvgDialog.FileName;
      Export.Enabled = true;
    }

    private void Export_Click(object sender, EventArgs e)
    {
      PaperW = Convert.ToDouble(PaperWidthString.Text);
      PaperH = Convert.ToDouble(PaperHeightString.Text);
      HatchDPI = Convert.ToUInt16(textBox1.Text);
      iGeomDPI = Convert.ToUInt16(textBox2.Text);
      pEmbeddedFonts = Embedded_fonts.Checked; // 1
      pTrueType = True_type.Checked; // 2
      pSHXText = SHX_text.Checked; // 3
      pEnableOptimization = Enable_optimization.Checked; // 4
      pZoomToExtents = Zoom_extents.Checked; // 5
      pHiddenAlgorithm = Hidden_line.Checked; // 6
      pLayerSupport = Layer_support.Checked; // 7
      pOffLayers = Off_layers.Checked; // 8
      pEncoded = Encoded_small.Checked;
      pPath = PdfFilePathString.Text;
      pTitle = TitleString.Text;
      pAuthor = AuthorString.Text;
      pSubject = SubjectString.Text;
      pKeywords = KeywordsString.Text;
      pCreator = CreatorString.Text;
      pProducer = ProducerString.Text;
      bLayouts = All.Checked;
      bExportHyperlinks = ExportHyperlinks1.Checked;
    }

    private void Export2PdfForm_Shown(object sender, EventArgs e)
    {
      PaperWidthString.Text = PaperW.ToString();
      PaperHeightString.Text = PaperH.ToString();
      textBox1.Text = HatchDPI.ToString();
      textBox2.Text = iGeomDPI.ToString();
    }
  }
}
