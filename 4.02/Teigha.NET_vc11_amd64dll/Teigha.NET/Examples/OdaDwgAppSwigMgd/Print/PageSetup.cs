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
using System.Diagnostics;

using Teigha.Core;
using Teigha.TD;


namespace OdViewExMgd.Print
{
  struct PlotScaleSetting
  {
    public double m_dRealWorldUnits;  // numerator    142(dxf)
    public double m_dDrawingUnits;    // denominator  143(dxf)
    public int m_ScaleType;        //              75 (dxf)
    public double m_dScaleFactor;     //              147(dxf)

    public PlotScaleSetting(double dRealWorldUnits, double dDrawingUnits, int iScaleType, double dScaleFactor)
    {
      m_dRealWorldUnits = dRealWorldUnits;
      m_dDrawingUnits   = dDrawingUnits;
      m_ScaleType       = iScaleType;
      m_dScaleFactor    = dScaleFactor;
    }
  };

  struct unitsInfo
  {
    public double m_dScale;
    public String m_sName1;
    public String m_sName2;

    public static String getTextByValue(double dValue, unitsInfo pInfo)
    {
      if (Math.Abs(dValue) <= 1.0)
      {
        return pInfo.m_sName1;
      }
      return pInfo.m_sName2;
    }

    public unitsInfo(double dScale, String sName1, String sName2)
    {
      m_dScale = dScale;
      m_sName1 = sName1;
      m_sName2 = sName2;
    }
  };

  public partial class PageSetup : Form
  {
    static PlotScaleSetting[] plotScaleSetting = {
      new PlotScaleSetting(1,     1,    0,  1),                   //    _T("Scaled to Fit"),  //    kScaleToFit
      new PlotScaleSetting(1,     1536, 1,  0.0006510416666667),  //    _T("1/128\" = 1'"),   //    k1_128in_1ft
      new PlotScaleSetting(1,     768,  2,  0.0013020833333333),  //    _T("1/64\" = 1'"),    //    k1_64in_1ft
      new PlotScaleSetting(1,     384,  3,  0.0026041666666667),  //    _T("1/32\" = 1'"),    //    k1_32in_1ft
      new PlotScaleSetting(1,     192,  4,  0.0052083333333333),  //    _T("1/16\" = 1'"),    //    k1_16in_1ft
      new PlotScaleSetting(1,     128,  5,  0.0078125),           //    _T("3/32\" = 1'"),    //    k3_32in_1ft
      new PlotScaleSetting(1,     96,   6,  0.0104166666666667),  //    _T("1/8\" = 1'"),     //    k1_8in_1ft,
      new PlotScaleSetting(1,     64,   7,  0.015625),            //    _T("3/16\" = 1'"),    //    k3_16in_1ft
      new PlotScaleSetting(1,     48,   8,  0.0208333333333333),   //   _T("1/4\" = 1'"),     //    k1_4in_1ft,
      new PlotScaleSetting(1,     32,   9,  0.03125),             //    _T("3/8\" = 1'"),     //    k3_8in_1ft,
      new PlotScaleSetting(1,     24,   10, 0.0416666666666667),  //    _T("1/2\" = 1'"),     //    k1_2in_1ft,
      new PlotScaleSetting(1,     16,   11, 0.0625),              //    _T("3/4\" = 1'"),     //    k3_4in_1ft,
      new PlotScaleSetting(1,     12,   12, 0.0833333333333333),  //    _T("1\" = 1'"),       //    k1in_1ft,
      new PlotScaleSetting(1,     4,    13, 0.25),                //    _T("3\" = 1'"),       //    k3in_1ft,
      new PlotScaleSetting(1,     2,    14, 0.5),                 //    _T("6\" = 1'"),       //    k6in_1ft,
      new PlotScaleSetting(1,     1,    15, 1),                   //    _T("1' = 1'"),        //    k1ft_1ft,
      new PlotScaleSetting(1,     1,    16, 1),                   //    _T("1:1"),            //    k1_1,
      new PlotScaleSetting(1,     2,    17, 0.5),                 //    _T("1:2"),            //    k1_2,
      new PlotScaleSetting(1,     4,    18, 0.25),                //    _T("1:4"),            //    k1_4,
      new PlotScaleSetting(1,     8,    19, 0.125),               //    _T("1:8"),            //    k1_8,
      new PlotScaleSetting(1,     10,   20, 0.1),                 //    _T("1:10"),           //    k1_10, 
      new PlotScaleSetting(1,     16,   21, 0.0625),              //    _T("1:16"),           //    k1_16, 
      new PlotScaleSetting(1,     20,   22, 0.05),                //    _T("1:20"),           //    k1_20, 
      new PlotScaleSetting(1,     30,   23, 0.03333333333333),    //    _T("1:30"),           //    k1_30, 
      new PlotScaleSetting(1,     40,   24, 0.025),               //    _T("1:40"),           //    k1_40, 
      new PlotScaleSetting(1,     50,   25, 0.2),                 //    _T("1:50"),           //    k1_50, 
      new PlotScaleSetting(1,     100,  26, 0.01),                //    _T("1:100"),          //    k1_100,
      new PlotScaleSetting(2,     1,    27, 2),                   //    _T("2:1"),            //    k2_1,
      new PlotScaleSetting(4,     1,    28, 4),                   //    _T("4:1"),            //    k4_1,
      new PlotScaleSetting(8,     1,    29, 8),                   //    _T("8:1"),            //    k8_1,
      new PlotScaleSetting(10,    1,    30, 10),                  //    _T("10:1"),           //    k10_1, 
      new PlotScaleSetting(100,   1,    31, 100),                 //    _T("100:1"),          //    k100_1,
      new PlotScaleSetting(1000,  1,    32, 1000)                 //    _T("1000:1")          //    k1000_1};
    };

    static unitsInfo[] unitsScale = {
      new unitsInfo(25.4, "inch", "inches"), 
      new unitsInfo(1.0, "mm", "mm") ,
      new unitsInfo(1.0, "pixel", "pixels"),
      new unitsInfo(1.0, "unit", "units")
    };

    static String[] pPlotScaleValues = {
      "Custom",         //"Scaled to Fit",
      "1/128\" = 1'",   //    k1_128in_1ft
      "1/64\" = 1'",    //    k1_64in_1ft
      "1/32\" = 1'",    //    k1_32in_1ft
      "1/16\" = 1'",    //    k1_16in_1ft
      "3/32\" = 1'",    //    k3_32in_1ft
      "1/8\" = 1'",     //    k1_8in_1ft,
      "3/16\" = 1'",    //    k3_16in_1ft
      "1/4\" = 1'",     //    k1_4in_1ft,
      "3/8\" = 1'",     //    k3_8in_1ft,
      "1/2\" = 1'",     //    k1_2in_1ft,
      "3/4\" = 1'",     //    k3_4in_1ft,
      "1\" = 1'",       //    k1in_1ft,
      "3\" = 1'",       //    k3in_1ft,
      "6\" = 1'",       //    k6in_1ft,
      "1' = 1'",        //    k1ft_1ft,
      "1:1",            //    k1_1,
      "1:2",            //    k1_2,
      "1:4",            //    k1_4,
      "1:8",            //    k1_8,
      "1:10",           //    k1_10, 
      "1:16",           //    k1_16, 
      "1:20",           //    k1_20, 
      "1:30",           //    k1_30, 
      "1:40",           //    k1_40, 
      "1:50",           //    k1_50, 
      "1:100",          //    k1_100,
      "2:1",            //    k2_1,
      "4:1",            //    k4_1,
      "8:1",            //    k8_1,
      "10:1",           //    k10_1, 
      "100:1",          //    k100_1,
      "1000:1"          //    k1000_1
    };


    OdDbPlotSettings m_plotStg;
    OdDbPlotSettingsValidator m_plotSettingVal;

    ~PageSetup()
    {
      textBoxPlotScaleDrawingUint.LostFocus -= new EventHandler(textBoxPlotScalePaperUint_TextChanged);
      textBoxPlotScalePaperUint.LostFocus   -= new EventHandler(textBoxPlotScalePaperUint_TextChanged);
      textBoxPlotOffsetX.LostFocus          -= new EventHandler(textBoxPlotOffsetX_TextChanged);
      textBoxPlotOffsetY.LostFocus          -= new EventHandler(textBoxPlotOffsetX_TextChanged);
      textBoxDPI.LostFocus                  -= new EventHandler(textBoxDPI_TextChanged);
    }


    public PageSetup(OdDbPlotSettings ps)
    {
      InitializeComponent();
      textBoxPlotScaleDrawingUint.LostFocus += new EventHandler(textBoxPlotScalePaperUint_TextChanged);
      textBoxPlotScalePaperUint.LostFocus   += new EventHandler(textBoxPlotScalePaperUint_TextChanged);
      textBoxPlotOffsetX.LostFocus          += new EventHandler(textBoxPlotOffsetX_TextChanged);
      textBoxPlotOffsetY.LostFocus          += new EventHandler(textBoxPlotOffsetX_TextChanged);
      textBoxDPI.LostFocus                  += new EventHandler(textBoxDPI_TextChanged);
      
      m_plotStg = ps;
      m_plotSettingVal = m_plotStg.database().appServices().plotSettingsValidator();

      // fill device list
      m_plotSettingVal.refreshLists(m_plotStg);

      // is stored device name available in system ?
      String deviceName = m_plotStg.getPlotCfgName();
      String mediaName = m_plotStg.getCanonicalMediaName();

      setPlotCfgName2Validator(deviceName, mediaName, false);

      FillDeviceCombo();

      FillPaperOrientation();
      comboBoxDevices.SelectedItem = deviceName;

      FillPlotAreaCombo(true, true);
      FillPlotOffset();

      FillScaleValues(true);
      FillPlotStyles();
      FillShadePlotQualityDPI(true);
      FillPlotStyleCombo(true);
      FillViewCombo(true);
      FillWindowArea();
    }


    void setPlotCfgName2Validator(String DeviceName, String CanonicalMediaName, bool bValidNames)
    {
      String currentDeviceName = m_plotStg.getPlotCfgName();
      String currentMediaName = m_plotStg.getCanonicalMediaName();

      if (bValidNames && currentDeviceName == DeviceName && currentMediaName == CanonicalMediaName)
        return;

      try
      {
        m_plotSettingVal.setPlotCfgName(m_plotStg, DeviceName, CanonicalMediaName);
      }
      catch (System.Exception)
      {
        try
        {
          m_plotSettingVal.setPlotCfgName(m_plotStg, DeviceName, String.Format("")); // wrong device
        }
        catch (System.Exception)
        {
          try
          {
            m_plotSettingVal.setPlotCfgName(m_plotStg, String.Format("none"), String.Format("none_user_media")); // wrong device
          }
          catch (System.Exception ex2)
          {
            MessageBox.Show(ex2.Message);
          }
        }
      }
    }

    bool FillDeviceCombo()
    {
      return true;
      //not implemented yet
      //OdStringArray devices;
      //m_plotSettingVal.plotDeviceList(out devices);

      comboBoxDevices.Items.Clear();

      foreach (String strDevice in devices)
      {
        comboBoxDevices.Items.Add(strDevice);
      }
      return true;
    }

    void FillPlotStyles()
    {
      //checkBoxPlotObjectLineweights.Checked = m_plotStg.PrintLineweights;
      //checkBoxPlotWithPlotStyles.Checked = m_plotStg.PlotPlotStyles;
      //checkBoxPlotPaperspaceLast.Checked = m_plotStg.DrawViewportsFirst;
      //checkBoxHidePaperspaceObjects.Checked = m_plotStg.PlotHidden;
      //
      //checkBoxPlotPaperspaceLast.Enabled = !isModelSpacePageSetup();
      //checkBoxHidePaperspaceObjects.Enabled = !isModelSpacePageSetup();
      //checkBoxPlotObjectLineweights.Enabled = !m_plotStg.PlotPlotStyles;
      //if (m_plotStg.PlotPlotStyles)
      //{
      //  checkBoxPlotObjectLineweights.Checked = true;
      //}
    }

    bool isModelSpacePageSetup()
    {
      return m_plotStg.modelType();
    }

    void FillPlotStyleCombo(bool bFillCombo)
    {
      return;
      if (bFillCombo)
      {
        StringCollection PSSlist = m_plotSettingVal.GetPlotStyleSheetList();
        comboBoxPlotStyleTable.Items.Clear();

        comboBoxPlotStyleTable.Items.Add("None");
        foreach (String strDevice in PSSlist)
        {
          comboBoxPlotStyleTable.Items.Add(strDevice);
        }

        int indx = 0;
        String curSS = m_plotStg.CurrentStyleSheet;
        if (curSS.Length > 0)
        {
          indx = comboBoxPlotStyleTable.FindStringExact(curSS);
          if (indx == -1)
            indx = 0;
        }
        comboBoxPlotStyleTable.SelectedIndex = indx;

        checkBoxDisplayPlotStyles.Enabled = !isModelSpacePageSetup();
        checkBoxDisplayPlotStyles.Checked = m_plotStg.ShowPlotStyles;
      }
    }

    bool viewsExists()
    {
      return false;
      using (OdDbViewTable pViewTable = (OdDbViewTable)m_plotStg.database().getViewTableId().openObject(OpenMode.kForRead))
      {
        foreach (OdDbObjectId objView in pViewTable)
        {
          using (OdDbViewTableRecord pView = (OdDbViewTableRecord)objView.openObject(OpenMode.kForRead))
          {
            if (pView.IsPaperspaceView != isModelSpacePageSetup())
            {
              return true;
            }
          }
        }
      }
      return false;
    }

    void FillPlotAreaCombo(bool bFillCombo, bool bStartInit)
    {
      if (bFillCombo)
      {
        comboBoxPlotAreaType.Items.Clear();

        comboBoxPlotAreaType.Items.Add("Display");
        comboBoxPlotAreaType.Items.Add("Extents");// TODO : depends on entities existance
        if (viewsExists())
          comboBoxPlotAreaType.Items.Add("View");
        comboBoxPlotAreaType.Items.Add("Window");

        if (isModelSpacePageSetup())
        {
          comboBoxPlotAreaType.Items.Insert(2, "Limits");
        }
        else
        {
          comboBoxPlotAreaType.Items.Insert(2, "Layout");
        }
      }

      PlotType pt = m_plotStg.PlotType;
      comboBoxPageSetupViews.Visible = (pt == PlotType.View);
      Aux.SelectWindowPrint = false;
      switch (pt)
      {
        case PlotType.Display:
          {
            comboBoxPlotAreaType.SelectedItem = "Display";
          }
          break;
        case PlotType.Extents:
          {
            comboBoxPlotAreaType.SelectedItem = "Extents";
          }
          break;
        case PlotType.Limits:
          {
            comboBoxPlotAreaType.SelectedItem = "Limits";
          }
          break;
        case PlotType.View:
          {
            comboBoxPlotAreaType.SelectedItem = "View";
          }
          break;
        case PlotType.Window:
          {
            comboBoxPlotAreaType.SelectedItem = "Window";
            if (!bStartInit)
            {
              Aux.SelectWindowPrint = true;
              this.DialogResult = DialogResult.OK;
              this.Hide();
            }
          }
          break;
        case PlotType.Layout:
          {
            comboBoxPlotAreaType.SelectedItem = "Layout";
          }
          break;
      }
    }

    bool isWHSwap()
    {
      PlotRotation pr = m_plotStg.PlotRotation;
      return pr == PlotRotation.Degrees090 || pr == PlotRotation.Degrees270;
    }

    void FillPlotOffset()
    {
      checkBoxCenterPlot.Enabled = m_plotStg.PlotCentered;
      textBoxPlotOffsetX.Enabled = !checkBoxCenterPlot.Enabled;
      textBoxPlotOffsetY.Enabled = !checkBoxCenterPlot.Enabled;

      Point2d plOrg = m_plotStg.PlotOrigin;
      if (isWHSwap())
      {
        textBoxPlotOffsetX.Text = plOrg.Y.ToString();
        textBoxPlotOffsetY.Text = plOrg.X.ToString();
      }
      else
      {
        textBoxPlotOffsetX.Text = plOrg.X.ToString();
        textBoxPlotOffsetY.Text = plOrg.Y.ToString();
      }

      PlotPaperUnit ppu = m_plotStg.PlotPaperUnits;
      if (ppu == PlotPaperUnit.Inches)
      {
        textBoxPlotOffsetX.Text = (double.Parse(textBoxPlotOffsetX.Text) / unitsScale[(int)ppu].m_dScale).ToString();
        textBoxPlotOffsetY.Text = (double.Parse(textBoxPlotOffsetY.Text) / unitsScale[(int)ppu].m_dScale).ToString();
      }

      Extents2d extMarg = m_plotStg.PlotPaperMargins;
      textBoxLeft.Text = extMarg.MinPoint.X.ToString();
      textBoxRight.Text = extMarg.MaxPoint.X.ToString();
      textBoxTop.Text = extMarg.MinPoint.Y.ToString();
      textBoxBottom.Text = extMarg.MaxPoint.Y.ToString();

      textBoxCanonicalPaperName.Text = m_plotStg.CanonicalMediaName;
      label9.Text = unitsInfo.getTextByValue(double.Parse(textBoxPlotOffsetX.Text), unitsScale[(int)ppu]);
      label8.Text = unitsInfo.getTextByValue(double.Parse(textBoxPlotOffsetY.Text), unitsScale[(int)ppu]);
    }

    bool isPaperWidthLessHeight()
    {
      Point2d ps = m_plotStg.PlotPaperSize;
      return ps.X < ps.Y;
    }

    void FillPaperOrientation()
    {
      PlotRotation pr = m_plotStg.PlotRotation;

      if (!isPaperWidthLessHeight())
      {        
        radioButtonPortrait.Checked  = Convert.ToBoolean((int)pr & 1);
        radioButtonLandscape.Checked = !radioButtonPortrait.Checked;
      }
      else
      {
        radioButtonLandscape.Checked = Convert.ToBoolean((int)pr & 1);
        radioButtonPortrait.Checked = !radioButtonLandscape.Checked;
      }
      checkBoxPlotUpsideDown.Checked = Convert.ToBoolean(((int)pr & 2) / 2);
    }

    void FillWindowArea()
    {
      Extents2d ex = m_plotStg.PlotWindowArea;
      textBoxMinX.Text = ex.MinPoint.X.ToString();
      textBoxMinY.Text = ex.MinPoint.Y.ToString();
      textBoxMaxX.Text = ex.MaxPoint.X.ToString();
      textBoxMaxY.Text = ex.MaxPoint.Y.ToString();
    }

    bool FillPaperSizes()
    {
      StringCollection canonicalMediaNames = m_plotSettingVal.GetCanonicalMediaNameList(m_plotStg);
      comboBoxPaperSize.Items.Clear();
      foreach (String strNames in canonicalMediaNames)
      {
        comboBoxPaperSize.Items.Add(m_plotSettingVal.GetLocaleMediaName(m_plotStg, strNames));
      }
      return true;
    }

    String getCanonicalByLocaleMediaName(String localeMediaName)
    {
      StringCollection mediaNames = m_plotSettingVal.GetCanonicalMediaNameList(m_plotStg);
      foreach (String strName in mediaNames)
      {
        if (m_plotSettingVal.GetLocaleMediaName(m_plotStg, strName) == localeMediaName)
          return strName;
      }
      return mediaNames[0];
    }

    void comboBoxDevices_SelectedChanged(object sender, EventArgs e)
    {
      int i = comboBoxDevices.SelectedIndex;
      String strDeviceName = comboBoxDevices.SelectedItem.ToString();

      String canonicalMediaName = m_plotStg.CanonicalMediaName;

      setPlotCfgName2Validator(strDeviceName, canonicalMediaName, true);

      // fill paper sizes combo box
      if (!FillPaperSizes())
      {
        return /*FALSE*/;
      }
      String localeMediaName = m_plotSettingVal.GetLocaleMediaName(m_plotStg, canonicalMediaName);

      // select active media
      comboBoxPaperSize.SelectedItem = localeMediaName;
      if (comboBoxPaperSize.SelectedIndex < 0)
      {
        String csLocaleMediaName = comboBoxPaperSize.Items[0].ToString();
        if (csLocaleMediaName.Length < 1)
        {
          return /*FALSE*/;
        }

        canonicalMediaName = getCanonicalByLocaleMediaName(csLocaleMediaName);
        m_plotSettingVal.SetCanonicalMediaName(m_plotStg, canonicalMediaName);
        comboBoxPaperSize.SelectedItem = csLocaleMediaName;
      }

      FillPlotAreaCombo(false, false);
      FillPlotOffset();
      FillScaleValues(false);
      FillPaperOrientation();
      FillPlotStyles();
    }

    void FillScaleValues(bool bFillCombo)
    {
      if (bFillCombo)
      {
        comboBoxPlotScale.Items.Clear();
        foreach(String strScaleVal in pPlotScaleValues)
        {
          comboBoxPlotScale.Items.Add(strScaleVal);
        }
      }

      StdScaleType sst= m_plotStg.StdScaleType;
      if (m_plotStg.UseStandardScale && sst != StdScaleType.ScaleToFit && sst >=0 && sst <=StdScaleType.StdScale1000To1)
      {
        comboBoxPlotScale.SelectedItem = pPlotScaleValues[(int)sst];
      }
      else
      {
        comboBoxPlotScale.SelectedItem = "Custom";
      }

      bool isModel = isModelSpacePageSetup();
      bool isLayoutMode = m_plotStg.PlotType == PlotType.Layout;

      bool bTmp = m_plotStg.UseStandardScale && !isLayoutMode && (sst == StdScaleType.ScaleToFit);
      if (checkBoxPlotScaleFitToPaper.Checked != bTmp)
        checkBoxPlotScaleFitToPaper.Checked = bTmp;
      checkBoxScaleLineweights.Checked    = m_plotStg.ScaleLineweights;

      if (isLayoutMode)
      {
        checkBoxPlotScaleFitToPaper.Checked = checkBoxCenterPlot.Checked = false;
      }

      checkBoxScaleLineweights.Enabled = !isModel;
      checkBoxPlotScaleFitToPaper.Enabled = !isLayoutMode;
      checkBoxCenterPlot.Enabled = !isLayoutMode;

      comboBoxPlotScale.Enabled = !checkBoxPlotScaleFitToPaper.Checked;
      textBoxPlotScalePaperUint.Enabled = !checkBoxPlotScaleFitToPaper.Checked;
      textBoxPlotScaleDrawingUint.Enabled = !checkBoxPlotScaleFitToPaper.Checked;

      if (m_plotStg.UseStandardScale && !checkBoxPlotScaleFitToPaper.Checked)
      {
        textBoxPlotScalePaperUint.Text   = (plotScaleSetting[(int)sst].m_dRealWorldUnits).ToString();
        textBoxPlotScaleDrawingUint.Text = (plotScaleSetting[(int)sst].m_dDrawingUnits).ToString();
      }
      else
      {
        CustomScale cs = m_plotStg.CustomPrintScale;
        textBoxPlotScalePaperUint.Text   = (cs.Numerator).ToString();
        textBoxPlotScaleDrawingUint.Text = (cs.Denominator).ToString();
      }

      FillMMInches();
      textBoxPlotScaleDrawingUintText.Text = unitsInfo.getTextByValue(double.Parse(textBoxPlotScaleDrawingUint.Text), unitsScale[3]);
    }

    void FillMMInches()
    {
      PlotPaperUnit ppu = m_plotStg.PlotPaperUnits;
      bool bTmp = (ppu == PlotPaperUnit.Pixels);
      if (comboBoxMMInches.Items.Count != (bTmp ? 1 : 2))
      {
        comboBoxMMInches.Items.Clear();
        if (bTmp)
        {
          comboBoxMMInches.Items.Add("pixels");
        }
        else
        {
          comboBoxMMInches.Items.Add("inches");
          comboBoxMMInches.Items.Add("mm");
        }
      }
      int iIndex = (ppu == PlotPaperUnit.Millimeters) ? 1 : 0;
      if (comboBoxMMInches.Enabled == bTmp)
      {
        comboBoxMMInches.Enabled = !bTmp;
      }
      if (comboBoxMMInches.SelectedIndex != iIndex)
      {
        comboBoxMMInches.SelectedIndex = iIndex;
      }
    }

    void FillViewCombo(bool bFillCombo)
    {
      if (bFillCombo)
      {
        comboBoxPageSetupViews.Items.Clear();
        using (ViewTable pViewTable = (ViewTable)m_plotStg.Database.ViewTableId.GetObject(OpenMode.ForRead))
        {
          foreach (ObjectId objView in pViewTable)
          {
            using (ViewTableRecord pView = (ViewTableRecord)objView.GetObject(OpenMode.ForRead))
            {
              if (pView.IsPaperspaceView != isModelSpacePageSetup())
              {
                comboBoxPageSetupViews.Items.Add(pView.Name);
              }
            }
          }
        }
      }
      comboBoxPageSetupViews.Enabled = (comboBoxPageSetupViews.Items.Count > 0);

      if (comboBoxPageSetupViews.Items.Count > 0)
      {
        comboBoxPageSetupViews.SelectedItem = m_plotStg.PlotViewName;
      }
    }

    void FillShadePlotQualityDPI(bool bFillCombo)
    {
      if (bFillCombo)
      {
        comboBoxQuality.Items.Clear();
        comboBoxQuality.Items.Add("Draft");
        comboBoxQuality.Items.Add("Preview");
        comboBoxQuality.Items.Add("Normal");
        comboBoxQuality.Items.Add("Presentation");
        comboBoxQuality.Items.Add("Maximum");
        comboBoxQuality.Items.Add("Custom");

        comboBoxShadePlot.Items.Clear();
        comboBoxShadePlot.Items.Add("As displayed");
        comboBoxShadePlot.Items.Add("Wireframe");
        comboBoxShadePlot.Items.Add("Hidden");
        comboBoxShadePlot.Items.Add("Rendered");
      }

      PlotSettingsShadePlotType pShadePlot = m_plotStg.ShadePlot;
      comboBoxShadePlot.SelectedIndex = (int)pShadePlot;

      ShadePlotResLevel spr = m_plotStg.ShadePlotResLevel;
      comboBoxQuality.SelectedIndex = (int)spr;

      if (spr == ShadePlotResLevel.Custom)
        textBoxDPI.Text = m_plotStg.ShadePlotCustomDpi.ToString();

      if (isModelSpacePageSetup())
      {
        bool bEnableWindows = pShadePlot == PlotSettingsShadePlotType.AsDisplayed || pShadePlot == PlotSettingsShadePlotType.Rendered;
        comboBoxQuality.Enabled = bEnableWindows;
        textBoxDPI.Enabled = (spr == ShadePlotResLevel.Custom && bEnableWindows);
      }
      else
      {
        comboBoxShadePlot.Enabled = false;
        comboBoxQuality.Enabled = true;
        textBoxDPI.Enabled = (spr == ShadePlotResLevel.Custom);
      }
    }

    private void comboBoxPlotScale_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxPlotScale.SelectedIndex != 0) // skip Custom
        m_plotSettingVal.SetStdScaleType(m_plotStg, (StdScaleType)plotScaleSetting[comboBoxPlotScale.SelectedIndex].m_ScaleType);

      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();
    }

    PlotPaperUnit getMediaNativePPU()
    { // This method uses the backdoor way to define PPU from Media.
      m_plotSettingVal.SetPlotConfigurationName(m_plotStg, m_plotStg.PlotConfigurationName, m_plotStg.CanonicalMediaName);
      return m_plotStg.PlotPaperUnits;
    }

    private void comboBoxPaperSize_SelectedIndexChanged(object sender, EventArgs e)
    {
      String newLocaleMediaName = comboBoxPaperSize.SelectedItem.ToString();
      String newCanonicalMediaName = getCanonicalByLocaleMediaName(newLocaleMediaName);
      m_plotSettingVal.SetCanonicalMediaName(m_plotStg, newCanonicalMediaName);

      PlotPaperUnit nativeUnits = getMediaNativePPU();

      // change paper orientation to dialog values
      PlotRotation pr;
      if (isPaperWidthLessHeight())
        pr = (PlotRotation)(Convert.ToInt32(radioButtonLandscape.Checked) + Convert.ToInt32(checkBoxPlotUpsideDown.Checked) * 2);
      else
        pr = (PlotRotation)(Convert.ToInt32(!radioButtonLandscape.Checked) + Convert.ToInt32(checkBoxPlotUpsideDown.Checked) * 2);
      m_plotSettingVal.SetPlotRotation(m_plotStg, pr);

      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();

      // and reset units to paper native
      if (nativeUnits == PlotPaperUnit.Inches || nativeUnits == PlotPaperUnit.Millimeters)
      {
        comboBoxMMInches.SelectedIndex = (nativeUnits == PlotPaperUnit.Millimeters)?1:0;
      }
    }

    void unitsConverted(PlotPaperUnit prevUnits, PlotPaperUnit plotPaperUnits)
    {
      double dCoeff = 0;
      if (plotPaperUnits == PlotPaperUnit.Millimeters && prevUnits == PlotPaperUnit.Inches)
      {
        dCoeff = 25.4;
      }
      else if (plotPaperUnits == PlotPaperUnit.Inches && prevUnits == PlotPaperUnit.Millimeters)
      {
        dCoeff = 1.0 / 25.4;
      }
      else
      {
        return;
      }

      bool bStandardScale = m_plotStg.UseStandardScale;
      if (bStandardScale)
      {
        double dStandardScale = m_plotStg.StdScale;
        if (dStandardScale != 0) // skip Fit to paper
          m_plotSettingVal.SetCustomPrintScale(m_plotStg, new CustomScale(dStandardScale, 1.0 / dCoeff));
      }
      else
      {
        CustomScale cs = m_plotStg.CustomPrintScale;
        m_plotSettingVal.SetCustomPrintScale(m_plotStg, new CustomScale(cs.Numerator, cs.Denominator / dCoeff));
      }
    }

    private void comboBoxMMInches_SelectedIndexChanged(object sender, EventArgs e)
    {
      String unitsStr = comboBoxMMInches.SelectedItem.ToString();

      PlotPaperUnit plotPaperUnits = PlotPaperUnit.Pixels;
      if (unitsStr == "mm") plotPaperUnits = PlotPaperUnit.Millimeters;
      else if (unitsStr == "inches") plotPaperUnits = PlotPaperUnit.Inches;

      PlotPaperUnit prevUnits = m_plotStg.PlotPaperUnits;
      m_plotSettingVal.SetPlotPaperUnits(m_plotStg, plotPaperUnits);
      unitsConverted( prevUnits, plotPaperUnits);

      FillScaleValues(false);
      FillPlotOffset();
    }

    private void comboBoxQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
      int idx = comboBoxQuality.SelectedIndex;
      m_plotStg.ShadePlotResLevel = (ShadePlotResLevel)idx;

      FillShadePlotQualityDPI(false);
    }

    private void comboBoxShadePlot_SelectedIndexChanged(object sender, EventArgs e)
    {
      int idx = comboBoxShadePlot.SelectedIndex;
      m_plotStg.ShadePlot = (PlotSettingsShadePlotType)idx;
      FillShadePlotQualityDPI(false);
    }

    private void comboBoxPageSetupViews_SelectedIndexChanged(object sender, EventArgs e)
    {
      String viewName = comboBoxPageSetupViews.SelectedItem.ToString();
      m_plotSettingVal.SetPlotViewName(m_plotStg, viewName);
      FillViewCombo(false);
    }

    private void comboBoxPlotAreaType_SelectedIndexChanged(object sender, EventArgs e)
    {
      String newViewType = comboBoxPlotAreaType.SelectedItem.ToString();

      PlotType pt = PlotType.Display;
      if (newViewType != "Display")
      {
        if (newViewType == "Limits")
        {
          pt = PlotType.Limits;
        }
        else if (newViewType == "View")
        {
          pt = PlotType.View;
        }
        else if (newViewType == "Window")
        {
          pt = PlotType.Window;
        }
        else if (newViewType == "Extents")
        {
          pt = PlotType.Extents;
        }
        else if (newViewType == "Layout")
        {
          pt = PlotType.Layout;
        }
      }
      
      m_plotSettingVal.SetPlotType(m_plotStg, pt);
      FillPlotAreaCombo(false, false);

      if (pt == PlotType.Layout)
      {
        // This is differences between dialog and validator. Validator doesn't
        // change UseStandardScale to false. Example is kExtents, kFit2Paper -> kLayout ->kExtents
        // Dialog has kFit2Paper disabled, but validator don't clear kFit2Paper flag.
        // Validator also don't change PlotOrigin to 0,0, if plotsenteres was true, but it change scale to 1:1 if fittopaper was true

        if (checkBoxCenterPlot.Enabled)
        {
          m_plotSettingVal.SetPlotOrigin(m_plotStg, new Point2d(0,0));
        }
        if (checkBoxPlotScaleFitToPaper.Enabled)
        {
          m_plotSettingVal.SetUseStandardScale(m_plotStg, false);
        }
      }
      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();
    }

    private void checkBoxCenterPlot_CheckedChanged(object sender, EventArgs e)
    {
      m_plotSettingVal.SetPlotCentered(m_plotStg, checkBoxCenterPlot.Checked);
      FillPaperOrientation();
      FillPlotOffset();
      FillScaleValues(false);
    }

    private void checkBoxPlotScaleFitToPaper_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBoxPlotScaleFitToPaper.Checked)
      {
        m_plotSettingVal.SetStdScaleType(m_plotStg, StdScaleType.ScaleToFit);
      }
      else
      {
        m_plotSettingVal.SetUseStandardScale(m_plotStg, false);
      }

      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();
    }

    private void checkBoxDisplayPlotStyles_CheckedChanged(object sender, EventArgs e)
    {
      m_plotStg.ShowPlotStyles = checkBoxDisplayPlotStyles.Checked;
      FillPlotStyleCombo(false);
    }

    private void checkBoxScaleLineweights_CheckedChanged(object sender, EventArgs e)
    {
      m_plotStg.ScaleLineweights = checkBoxScaleLineweights.Checked;
      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();
    }

    private void textBoxPlotOffsetX_TextChanged(object sender, EventArgs e)
    {
      if (textBoxPlotOffsetX.Text.Length > 0 && textBoxPlotOffsetY.Text.Length > 0)
      {
        PlotPaperUnit ppu = m_plotStg.PlotPaperUnits;
        if (ppu == PlotPaperUnit.Inches)
        {
          textBoxPlotOffsetX.Text = (double.Parse(textBoxPlotOffsetX.Text) * unitsScale[(int)ppu].m_dScale).ToString();
          textBoxPlotOffsetY.Text = (double.Parse(textBoxPlotOffsetY.Text) * unitsScale[(int)ppu].m_dScale).ToString();
        }

        if (isWHSwap())
          m_plotSettingVal.SetPlotOrigin(m_plotStg, new Point2d(double.Parse(textBoxPlotOffsetY.Text), double.Parse(textBoxPlotOffsetX.Text)));
        else
          m_plotSettingVal.SetPlotOrigin(m_plotStg, new Point2d(double.Parse(textBoxPlotOffsetX.Text), double.Parse(textBoxPlotOffsetY.Text)));

        FillPaperOrientation();
        FillPlotOffset(); // possibly offset was changed in validator
        FillScaleValues(false);
      }
    }

    private void textBoxDPI_TextChanged(object sender, EventArgs e)
    {
      m_plotStg.ShadePlotCustomDpi = short.Parse(textBoxDPI.Text);
      FillShadePlotQualityDPI(false);
    }

    private void textBoxPlotScalePaperUint_TextChanged(object sender, EventArgs e)
    {
      if (textBoxPlotScalePaperUint.Text.Length > 0 && textBoxPlotScaleDrawingUint.Text.Length > 0)
      {
        m_plotSettingVal.SetCustomPrintScale(m_plotStg, new CustomScale(double.Parse(textBoxPlotScalePaperUint.Text), double.Parse(textBoxPlotScaleDrawingUint.Text)));
        FillPaperOrientation();
        FillScaleValues(false);
        FillPlotOffset();
      }
    }

    private void checkBoxPlotUpsideDown_CheckedChanged(object sender, EventArgs e)
    {
      PlotRotation pr;
      if (isPaperWidthLessHeight())
        pr = (PlotRotation)(Convert.ToInt32(radioButtonLandscape.Checked) + Convert.ToInt32(checkBoxPlotUpsideDown.Checked) * 2);
      else
        pr = (PlotRotation)(Convert.ToInt32(!radioButtonLandscape.Checked) + Convert.ToInt32(checkBoxPlotUpsideDown.Checked) * 2);
      m_plotSettingVal.SetPlotRotation(m_plotStg, pr);

      FillPaperOrientation();
      FillScaleValues(false);
      FillPlotOffset();
    }

    private void checkBoxPlotWithPlotStyles_CheckedChanged(object sender, EventArgs e)
    {
      m_plotStg.PrintLineweights   = checkBoxPlotObjectLineweights.Checked;
      m_plotStg.PlotPlotStyles     = checkBoxPlotWithPlotStyles.Checked;
      m_plotStg.DrawViewportsFirst = checkBoxPlotPaperspaceLast.Checked;
      m_plotStg.PlotHidden         = checkBoxHidePaperspaceObjects.Checked;
      FillPlotStyles();
    }

    private void comboBoxPlotStyleTable_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxPlotStyleTable.SelectedIndex > 0)
      {
        m_plotSettingVal.SetCurrentStyleSheet(m_plotStg, comboBoxPlotStyleTable.SelectedItem.ToString());
      }
      else
      {
        m_plotSettingVal.SetCurrentStyleSheet(m_plotStg, String.Empty);
      }
    }
  }
}