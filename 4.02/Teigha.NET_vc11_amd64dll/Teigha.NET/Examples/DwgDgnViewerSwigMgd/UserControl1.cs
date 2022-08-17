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
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Teigha.Core;
using Teigha.TG;
using Teigha.TD;

namespace DwgViewer
{
  public partial class UserControl1 : UserControl
  {
    public OdGsDevice m_pDevice;

    uint[] CurPalette;

    public OdDgElementId TGVectorizedViewId;
    public OdDgElementId TGVectorizedModelId;


    public OdGiDefaultContext Context;
    public OdDbDatabase TDDatabase;
    public OdDgDatabase TGDatabase;
    private bool isDWG;
    
    public UserControl1()
    {
      InitializeComponent();
    }

    public void ResetDevice(bool DWG)
    {
      isDWG = DWG;
      {
        // Get the client rectangle
        Rectangle rc = Bounds;

        // Load the vectorization module
        OdGsModule pGs = null;
        try
        {
          pGs = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false);
        }
        catch (Exception err)
        {
          MessageBox.Show(err.Message);
          return;
        }
        // Create a new OdGsDevice object, and associate with the vectorization GsDevice
        m_pDevice = pGs.createDevice();

        // Return a pointer to the dictionary entity containing the device properties
        OdRxDictionary Properties = m_pDevice.properties();

        Properties.putAt("WindowHWND", new OdRxVariantValue(Handle.ToInt32())); // hWnd necessary for DirectX device
        // Define a device coordinate rectangle equal to the client rectangle
        OdGsDCRect gsRect = new OdGsDCRect(rc.Left, rc.Right, rc.Bottom, rc.Top);
        
        // Set the device background color and palette
        //CurPalette = Teigha.Core.AllPalettes.getDarkPalette();
        CurPalette = Teigha.Core.AllPalettes.getLightPalette();
        m_pDevice.setBackgroundColor(CurPalette[0]);
        m_pDevice.setLogicalPalette(CurPalette, 256);

        if (DWG)
        {
          if (null == TDDatabase)
            return;

          OdGiContextForDbDatabase Ctx1 = OdGiContextForDbDatabase.createObject();
          Ctx1.enableGsModel(true);
          Ctx1.setDatabase(TDDatabase);
          m_pDevice.setUserGiContext(Ctx1);
          //m_pDevice = OdGsDevice.cast(TD_Db.setupActiveLayoutViews(m_pDevice, Ctx1));
          m_pDevice = (OdGsDevice)TD_Db.setupActiveLayoutViews(m_pDevice, Ctx1);
          // Return true if and only the current layout is a paper space layout
          bool bModelSpace = TDDatabase.getTILEMODE();

          // Set the viewport border properties
          SetViewportBorderProperties(m_pDevice, !bModelSpace);

          OdGsView pView = m_pDevice.viewAt(0);
          OdAbstractViewPE pViewPE = OdAbstractViewPE.cast(pView);
          pViewPE.zoomExtents(pView);

        }
        else
        {
          if (null == TGDatabase)
            return;
          OdGiContextForDgDatabase Ctx1 = OdGiContextForDgDatabase.createObject(TGDatabase, (OdDgView)TGVectorizedViewId.openObject());
          Ctx1.setDatabase(TGDatabase);
          Ctx1.enableGsModel(true);

          m_pDevice = OdGsDeviceForDgModel.setupModelView(TGVectorizedModelId, TGVectorizedViewId, m_pDevice, Ctx1);
          CurPalette = OdDgColorTable.currentPalette(TGDatabase);
          OdDgModel pModel = (OdDgModel)TGVectorizedModelId.safeOpenObject();
          // Color with #255 always defines background.
          CurPalette[255] = pModel.getBackground();

          // Note: This method should be called to resolve "white background issue" before setting device palette
          bool bCorrected = OdDgColorTable.correctPaletteForWhiteBackground(CurPalette);

          m_pDevice.setLogicalPalette(CurPalette, 256);
          m_pDevice.setBackgroundColor(pModel.getBackground());
          Ctx1.setPaletteBackground(0);
        }

        // Update the client rectangle
        OnResize(EventArgs.Empty);
      }

    }

    public void SetViewportBorderProperties(OdGsDevice pDevice, bool bModel)
    {
      int n = pDevice.numViews();

      if (n > 1)
      {
        for (int i = bModel ? 0 : 1; i < n; ++i)
        {
          // Get the viewport
          OdGsView pView = pDevice.viewAt(i);

          // Make it visible
          pView.setViewportBorderVisibility(true);

          // Set the color and width
          pView.setViewportBorderProperties(CurPalette[7], 1);
        }
      }
    }

    public void DeleteContext()
    {
      if (m_pDevice != null)
      {
        m_pDevice.Dispose();
        m_pDevice = null;
      }
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      if (m_pDevice != null)
      {
        Rectangle r = Bounds;
        r.Offset(-Location.X, -Location.Y);
        m_pDevice.onSize(new OdGsDCRect(r.Left, r.Right, r.Bottom, r.Top));
      }

      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (m_pDevice != null)
        m_pDevice.update();
      else
        base.OnPaint(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
      OdGsView view = m_pDevice.viewAt(0);
      OdGePoint3d currentPosition = view.position();
      OdGePoint3d mousePosition = new OdGePoint3d(e.X, e.Y, 0);
      OdGeVector3d moveVector = currentPosition - mousePosition;
      moveVector *= -1;
      moveVector = moveVector.transformBy((view.screenMatrix() * view.projectionMatrix()).inverse());
      view.dolly(moveVector);
      currentPosition = view.position();

      using (Teigha.Core.OdAbstractViewPE pVpPE = Teigha.Core.OdAbstractViewPE.cast(view))
      {
          OdGeMatrix3d eyeToWorldMatrix = pVpPE.eyeToWorld(view); //cause OdError after moving mouse wheel few times 
          OdGePoint3d wcsPt = currentPosition.transformBy(eyeToWorldMatrix);
      }
      view.zoom(e.Delta > 0 ? 1.0 / 0.9 : 0.9); 
      moveVector *= -1;
      view.dolly(moveVector);
      Invalidate();
    }

    public void AddOrbit(bool DWG)
    {
      using (OdGsView pView = m_pDevice.viewAt(0))
      {
        using (OdGsModel pM = m_pDevice.createModel())
        {
          Orbit orb = new Orbit();
          pView.add(orb, pM);
          Invalidate();
        }
      }
    }
  }
}
