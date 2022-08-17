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
using Teigha.Core;
using Teigha.TD;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace OdaDwgAppMgd
{
  public partial class VectorizeForm : Form
  {
    Tree _treeView;
    OdDbDatabase _db;
    OdDbObjectId _vectorizedViewId;
    OdDbObjectId _vectorizedModelId;
    public VectorizeForm(Tree t, OdDbObjectId vectorizedViewId, OdDbObjectId vectorizedModelId)
    {
      _treeView = t;
      _db = t.Database;
      _vectorizedViewId = vectorizedViewId;
      _vectorizedModelId = vectorizedModelId;
      InitializeComponent();
    }
    OdGsLayoutHelper _hDevice = null;
    Graphics _graphics;
    OdGiContextForDbDatabase _ctx;
    private void VectorizeForm_Load(object sender, EventArgs e)
    {
      _graphics = Graphics.FromHwnd(Handle);
      _ctx = OdGiContextForDbDatabase.createObject();
      try
      {
        // Load the vectorization module
        OdGsModule pGs = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false);
        // Create a new OdGsDevice object, and associate with the vectorization GsDevice
        OdGsDevice pDevice = pGs.createDevice();
        // Return a pointer to the dictionary entity containing the device properties
        OdRxDictionary Properties = pDevice.properties();
        if (Properties != null)
        {
            if (Properties.has("WindowHWND")) // Check if property is supported
                Properties.putAt("WindowHWND", new OdRxVariantValue(Handle.ToInt32())); // hWnd necessary for DirectX device
            if (Properties.has("WindowHWND")) // Check if property is supported
                Properties.putAt("WindowHWND", new OdRxVariantValue(Handle.ToInt32())); // hWnd necessary for DirectX device
            if (Properties.has("WindowHDC")) // Check if property is supported
                Properties.putAt("WindowHDC", new OdRxVariantValue(_graphics.GetHdc().ToInt32())); // hWindowDC necessary for Bitmap device
            if (Properties.has("DoubleBufferEnabled")) // Check if property is supported
                Properties.putAt("DoubleBufferEnabled", new OdRxVariantValue(true));
            if (Properties.has("EnableSoftwareHLR")) // Check if property is supported
                Properties.putAt("EnableSoftwareHLR", new OdRxVariantValue(true));
            if (Properties.has("DiscardBackFaces")) // Check if property is supported
                Properties.putAt("DiscardBackFaces", new OdRxVariantValue(true));
        }
        _ctx.enableGsModel(true);

        // Define a device coordinate rectangle equal to the client rectangle
        OdGsDCRect gsRect = new OdGsDCRect(ClientRectangle.Left, ClientRectangle.Right, ClientRectangle.Bottom, ClientRectangle.Top);

        _ctx.setDatabase(_db);
        //_hDevice.setUserGiContext(_ctx);
        _hDevice = TD_Db.setupActiveLayoutViews(pDevice, _ctx);
        
        // Set the device background color and palette
        uint[] CurPalette = Teigha.Core.AllPalettes.getLightPalette();
        _hDevice.setBackgroundColor(CurPalette[0]);
        _hDevice.setLogicalPalette(CurPalette, 256);

        // Return true if and only the current layout is a paper space layout
        bool bModelSpace = _db.getTILEMODE();

          // Set the viewport border properties
        SetViewportBorderProperties(_hDevice, !bModelSpace, CurPalette[7]);

        //OdGsView pView = _hDevice.activeView();//.viewAt(0);
        OdGsView pView = _hDevice.viewAt(0);
        OdAbstractViewPE pViewPE = OdAbstractViewPE.cast(pView);
        pViewPE.zoomExtents(pView);

        _hDevice.onSize(gsRect);
      }
      catch (OdError)
      {
        _hDevice = null;
        //theApp.reportError(_T("Graphic System Initialization Error"), e);
      }
    }

    public void SetViewportBorderProperties(OdGsLayoutHelper pDevice, bool bModel, uint color7)
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
          pView.setViewportBorderProperties(color7, 1);
        }
      }
    }

/*    OdGsView activeView(OdGsDevice pDevice)
    {
      OdGsView pActiveView = null;
      OdGsDeviceForDgModel pLHelper = OdGsDeviceForDgModel.cast(pDevice);
      if (pLHelper != null)
        pActiveView = pLHelper.activeView();
      return pActiveView;
    }
*/
    void setViewportBorderProperties()
    {
      int n = _hDevice.numViews();
      if (n > 1)
      {
        for (int i = 0; i < n; ++i)
        {
          OdGsView pView = _hDevice.viewAt(i);
          pView.setViewportBorderVisibility(true);
        }
      }
    }
    protected override void OnPaint(PaintEventArgs e)
    {
      if (_hDevice == null)
        return;
      setViewportBorderProperties();
      _hDevice.update();
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }
    OdGePoint3d getWorldCoordinateByScreen(OdGePoint2d screenPoint)
    {
      OdGsView view = _hDevice.viewAt(0);// .activeView();

      //directions, origin, etc
      OdGePoint3d target = view.target();
      OdGeVector3d direction = target - view.position();
      OdGeVector3d yVector = view.upVector();
      OdGeVector3d xVector = direction.crossProduct(yVector).normal();

      OdGePoint3d center, stepX, stepY;
      {
        center = new OdGePoint3d(target);
        stepX = target + xVector;
        stepY = target + yVector;

        OdGeMatrix3d matrix = view.worldToDeviceMatrix();
        stepX.transformBy(matrix);
        stepY.transformBy(matrix);
        center.transformBy(matrix);
      }

      //make the calculation
      OdGePoint3d result;
      {
        double x, y;
        x = (screenPoint.x - center.x) / (stepX.x - center.x);
        y = (screenPoint.y - center.y) / (stepY.y - center.y);

        result = target + xVector * x + yVector * y;
      }

      return result;
    }
    void zoom(double steps)
    {
      OdGsView view = _hDevice.activeView();

      if (view.isPerspective())
      {
        OdGePoint3d position = view.position(), target = view.target();
        OdGeVector3d direction = target - position;
        OdGeVector3d upVector = view.upVector();
        double width = view.fieldWidth(), height = view.fieldHeight();

        //calculate an appropriate offset using real size of the frame
        if (width < height)
        {
          steps *= width / 2.0;
        }
        else
        {
          steps *= height / 2.0;
        }
        direction *= steps;

        position += direction;
        target += direction;

        view.setView(position, target, upVector, width, height, OdGsView.Projection.kPerspective);
      }
      else
      {
        view.zoom(Math.Pow(1.11, steps));
      }
      Invalidate();
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
      if (_hDevice == null)
        return;
      OdGePoint2d correctScreenPoint = new OdGePoint2d(e.X, e.Y);

      OdGePoint3d worldPoint = getWorldCoordinateByScreen(correctScreenPoint);
      zoom(e.Delta / 120);

      //shift back
      OdGsView view = _hDevice.activeView();

      //get new screen point of the same world point
      OdGeMatrix3d worldToDevice = view.worldToDeviceMatrix();
      OdGePoint3d newScreenPoint = worldToDevice * worldPoint;

      //get world points on the focal plane
      OdGePoint3d movingPoint = getWorldCoordinateByScreen(new OdGePoint2d(newScreenPoint.x, newScreenPoint.y));
      OdGePoint3d destinationPoint = getWorldCoordinateByScreen(correctScreenPoint);

      OdGePoint3d position = view.position(), target = view.target();
      OdGeVector3d upVector = view.upVector();
      double width = view.fieldWidth(), height = view.fieldHeight();
      bool isPerspective = view.isPerspective();

      //shift the camera so points coincide
      OdGeVector3d offset = destinationPoint - movingPoint;
      position -= offset;
      target -= offset;

      view.setView(position, target, upVector, width, height, isPerspective ? OdGsView.Projection.kPerspective : OdGsView.Projection.kParallel);
    }
    public void ZoomExtents()
    {
      //  return;
      OdGsView view = _hDevice.activeView();

      //it is not calculated yet for perspective cameras
      if (view.isPerspective())
        return;
    }

//mku
    public void ZoomExtentsTmp()
    {
      /*OdGsView view = _hDevice.activeView();

      //it is not calculated yet for perspective cameras
      if (view.isPerspective())
        return;
      */
      //OdCmColor colorByColor = new OdCmColor(OdCmEntityColor.ColorMethod.kByColor);
      OdCmColor colorByColor = new OdCmColor();
      colorByColor.setRGB(255, 0, 0);

      using (OdDbBlockTable blockTable = (OdDbBlockTable)_db.getBlockTableId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
      {
        using (OdDbBlockTableRecord blockTableRecord = (OdDbBlockTableRecord)blockTable.getModelSpaceId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
        {
          OdDbObjectIterator iter = blockTableRecord.newIterator();
          for (; !iter.done(); iter.step())
          {
            ChangeObjectColor(blockTableRecord, iter.entity().handle().ToUInt64(), colorByColor);
            break;
          }
        }
      }

      this.zoom(2);
      this.Invalidate();
      _hDevice.update();
    }
    private void ChangeObjectColor(OdDbBlockTableRecord bBTR, ulong handleId, OdCmColor color)
    {
      OdDbObjectId objectId = _db.getOdDbObjectId(new OdDbHandle(handleId));

      using (OdDbObject dbObject = objectId.safeOpenObject(Teigha.TD.OpenMode.kForWrite))
      {
        //((OdDbEntity)dbObject).setColor(color);
        OdDbCircle circle = (OdDbCircle)dbObject;
        if (circle != null)
        {
          OdCmColor ccc1 = circle.color();
          circle.setColorIndex((ushort)(OdCmEntityColor.ACIcolorMethod.kACIGreen));
          OdCmColor ccc = circle.color();

          OdDbCircle pCircle = OdDbCircle.createObject();
          pCircle.setDatabaseDefaults(bBTR.database());
          bBTR.appendOdDbEntity(pCircle);
          OdGePoint3d center = circle.center();
          pCircle.setCenter(center);
          pCircle.setRadius(circle.radius() / 2);
          OdCmColor color1 = new OdCmColor();
          color1.setRGB(255, 0, 0);
          pCircle.setColor(color1);

          //((OdDbEntity)dbObject).setColorIndex((uint)OdCmEntityColor.ACIcolorMethod.kACIGreen);
        }

        if (dbObject is OdDbBlockReference)
        {
          using (OdDbBlockTableRecord blockTableRecord = (OdDbBlockTableRecord)((OdDbBlockReference)dbObject).blockTableRecord().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
          {
            OdDbObjectIterator iter = blockTableRecord.newIterator();
            for (; !iter.done(); iter.step())
            {
              OdDbObjectId subObjectId = iter.entity().objectId();

              OdCmColor colorByBlock = new OdCmColor(color);
              colorByBlock.setColorMethod(OdCmEntityColor.Items_ColorMethod.kByBlock);

              ChangeObjectColor(bBTR, subObjectId.getHandle().ToUInt64(), colorByBlock);
            }
          }
        }
      }
    }
    //--

    public OdGsLayoutHelper getGs()
    {
      return _hDevice;
    }

    private void VectorizeForm_SizeChanged(object sender, EventArgs e)
    {
      if (_hDevice != null)
      {
        _hDevice.onSize(new OdGsDCRect(new OdGsDCPoint(ClientRectangle.Left, ClientRectangle.Bottom), new OdGsDCPoint(ClientRectangle.Right, ClientRectangle.Top)));
        Invalidate();
      }
    }

    private void VectorizeForm_Activated(object sender, EventArgs e)
    {
      ((Form1)Parent.Parent).UpdateZoomExtFlag(true);
      ((Form1)Parent.Parent).UpdateRenderFlags(true);
    }
    public void snapShot()
    {
      MemoryManager mMan = MemoryManager.GetMemoryManager();
      MemoryTransaction mStartTrans = mMan.StartTransaction();

      OdRxRasterServices RasSvcs = (OdRxRasterServices)Teigha.Core.Globals.odrxDynamicLinker().loadApp("RxRasterServices");
      if (RasSvcs == null)
      {
        MessageBox.Show("Failed to load RxRasterServices module", "Error");
        return;
      }
      OdGiRasterImage img = OdGiRasterImageDesc.createObject(1024, 650, OdGiRasterImage.Units.kInch);
      OdGsDCRect gsRect = new OdGsDCRect(ClientRectangle.Left, ClientRectangle.Right, ClientRectangle.Bottom, ClientRectangle.Top);
      _hDevice.getSnapShot(out img, gsRect);
      SaveFileDialog saveImageDialog = new SaveFileDialog();
      saveImageDialog.Filter = "Bmp files|*.bmp";
      if (saveImageDialog.ShowDialog() != DialogResult.OK)
      {
        return;
      }
      bool status = RasSvcs.saveRasterImage(img, saveImageDialog.FileName /* @"D:\tmp1\saveSnapShot.bmp" */ );

      //OdGsView pView = _hDevice.activeView();
      //pView.getSnapShot(out img, gsRect);
      //bool status1 = RasSvcs.saveRasterImage(img, @"D:\tmp1\saveSnapShot1.bmp");
      mMan.StopTransaction(mStartTrans);
      mMan.StopAll();
    }
  }
}
