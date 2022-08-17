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
using Teigha.TG;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace OdaDgnAppMgd
{
  public partial class VectorizeForm : Form
  {
    Tree _treeView;
    OdDgDatabase _db;
    OdDgElementId _vectorizedViewId;
    OdDgElementId _vectorizedModelId;
    public VectorizeForm(Tree t, OdDgElementId vectorizedViewId, OdDgElementId vectorizedModelId)
    {
      _treeView = t;
      _db = t.Database;
      _vectorizedViewId = vectorizedViewId;
      _vectorizedModelId = vectorizedModelId;
      InitializeComponent();
    }
    OdGsDeviceForDgModel _pDevice = null;
    Graphics _graphics;
    public OdRxObject selected;
    OdGiContextForDgDatabase _ctx;

    private void VectorizeForm_Load(object sender, EventArgs e)
    {
      _graphics = Graphics.FromHwnd(Handle);
      _ctx = OdGiContextForDgDatabase.createObject(_db, (OdDgView)_vectorizedViewId.openObject());
      try
      {
        OdGsModule pGs = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false);//(str, false);
        using (OdGsDevice pDevice = pGs.createDevice())
        {
          //OdRxDictionary props = pDevice.properties();
          using (OdRxDictionary props = pDevice.properties())
          {
            if (props != null)
            {
              if (props.has("WindowHWND")) // Check if property is supported
                props.putAt("WindowHWND", new OdRxVariantValue(Handle.ToInt32())); // hWnd necessary for DirectX device
              if (props.has("WindowHDC")) // Check if property is supported
                props.putAt("WindowHDC", new OdRxVariantValue(_graphics.GetHdc().ToInt32())); // hWindowDC necessary for Bitmap device
              if (props.has("DoubleBufferEnabled")) // Check if property is supported
                props.putAt("DoubleBufferEnabled", new OdRxVariantValue(true));
              if (props.has("EnableSoftwareHLR")) // Check if property is supported
                props.putAt("EnableSoftwareHLR", new OdRxVariantValue(true));
              if (props.has("DiscardBackFaces")) // Check if property is supported
                props.putAt("DiscardBackFaces", new OdRxVariantValue(true));
            }
          }
          _ctx.enableGsModel(true); // TODO: use theApp.useGsModel() setting

          _pDevice = OdGsDeviceForDgModel.setupModelView(_vectorizedModelId, _vectorizedViewId, pDevice, _ctx);
          UInt32[] refColors = OdDgColorTable.currentPalette(_db);
          OdDgModel pModel = (OdDgModel)_db.getActiveModelId().safeOpenObject();
          // Color with #255 always defines background.
          refColors[255] = pModel.getBackground();
          //theApp.settings().setActiveBackground( pModel.getBackground() );

          // Note: This method should be called to resolve "white background issue" before setting device palette
          bool bCorrected = OdDgColorTable.correctPaletteForWhiteBackground(refColors);

          _pDevice.setLogicalPalette(refColors, 256);
          _pDevice.setBackgroundColor(pModel.getBackground());//theApp.settings().getActiveBackground()); // ACAD's color for paper bg
          _ctx.setPaletteBackground(0);
          OdGsDCRect gsRect = new OdGsDCRect(ClientRectangle.Left, ClientRectangle.Right, ClientRectangle.Bottom, ClientRectangle.Top);
          _pDevice.onSize(gsRect);
          OdGePoint3d target = _pDevice.activeView().target();
          Debug.Print(target.ToString());
        }
      }
      catch (OdError)
      {
        _pDevice = null;
        //theApp.reportError(_T("Graphic System Initialization Error"), e);
      }
    }

    OdGsView activeView(OdGsDevice pDevice)
    {
      OdGsView pActiveView = null;
      OdGsDeviceForDgModel pLHelper = OdGsDeviceForDgModel.cast(pDevice);
      if (pLHelper != null)
        pActiveView = pLHelper.activeView();
      return pActiveView;
    }

    void setViewportBorderProperties()
    {
      int n = _pDevice.numViews();
      if (n > 1)
      {
        for (int i = 0; i < n; ++i)
        {
          OdGsView pView = _pDevice.viewAt(i);
          pView.setViewportBorderVisibility(true);
        }
      }
    }
    protected override void OnPaint(PaintEventArgs e)
    {
      if (_pDevice == null)
        return;
      setViewportBorderProperties();
      _pDevice.update();
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }
    OdGePoint3d getWorldCoordinateByScreen(OdGePoint2d screenPoint)
    {
      OdGsView view = _pDevice.activeView();

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
      OdGsView view = _pDevice.activeView();

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
      if (_pDevice == null)
        return;
      OdGePoint2d correctScreenPoint = new OdGePoint2d(e.X, e.Y);

      OdGePoint3d worldPoint = getWorldCoordinateByScreen(correctScreenPoint);
      zoom(e.Delta / 120);

      //shift back
      OdGsView view = _pDevice.activeView();

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
      OdGsView view = _pDevice.activeView();

      //it is not calculated yet for perspective cameras
      if (view.isPerspective())
        return;

      //get active pModel
      OdDgModel pModel = (OdDgModel)_db.getActiveModelId().safeOpenObject();

      //get the extents of that pModel
      OdGeExtents3d extents = new OdGeExtents3d();
      pModel.getGeomExtents(extents);
      if (!extents.isValidExtents())
        return;

      //get all points
      OdGePoint3d[] realPoints = new OdGePoint3d[8];
      {
        OdGePoint3d min1 = extents.minPoint(), max1 = extents.maxPoint();

        realPoints[0] = min1;
        realPoints[1] = max1;
        realPoints[2] = new OdGePoint3d(min1.x, max1.y, min1.z);
        realPoints[3] = new OdGePoint3d(max1.x, max1.y, min1.z);
        realPoints[4] = new OdGePoint3d(max1.x, min1.y, min1.z);
        realPoints[5] = new OdGePoint3d(min1.x, min1.y, max1.z);
        realPoints[6] = new OdGePoint3d(min1.x, max1.y, max1.z);
        realPoints[7] = new OdGePoint3d(max1.x, min1.y, max1.z);
      }

      //get 'relative' points
      OdGePoint2d[] relativePoints = new OdGePoint2d[8];
      OdGePoint3d position = view.position(), target = view.target();
      OdGeVector3d n = view.upVector(), m = (target - position).crossProduct(n).normal();
      {
        for (int i = 0; i < 8; i++)
        {
          relativePoints[i] = new OdGePoint2d(
            (realPoints[i] - position).dotProduct(m), (realPoints[i] - position).dotProduct(n));
        }
      }

      //get characteristic points
      OdGeVector2d min = new OdGeVector2d(), medium = new OdGeVector2d(), max = new OdGeVector2d();
      {
        max.x = relativePoints[0].x;
        max.y = relativePoints[0].y;
        min.x = relativePoints[0].x;
        min.y = relativePoints[0].y;

        for (int i = 0; i < 8; i++)
        {
          if (min.x > relativePoints[i].x)
          {
            min.x = relativePoints[i].x;
          }
          if (max.x < relativePoints[i].x)
          {
            max.x = relativePoints[i].x;
          }
          if (min.y > relativePoints[i].y)
          {
            min.y = relativePoints[i].y;
          }
          if (max.y < relativePoints[i].y)
          {
            max.y = relativePoints[i].y;
          }
        }

        medium = (max + min) / 2.0;
      }

      //shift the camera (if new size is not zero; it is prohibited by Ge library)
      if (min.x < max.x || min.y < max.y)
      {
        view.setView(position + m * medium.x + n * medium.y, target + m * medium.x + n * medium.y, n,
                        (max.x - min.x) * 1.1, (max.y - min.y) * 1.1);
        Invalidate();
      }
    }

    private void VectorizeForm_SizeChanged(object sender, EventArgs e)
    {
      if (_pDevice != null)
      {
        _pDevice.onSize(new OdGsDCRect(new OdGsDCPoint(ClientRectangle.Left, ClientRectangle.Bottom), new OdGsDCPoint(ClientRectangle.Right, ClientRectangle.Top)));
        Invalidate();
      }
    }

    private void VectorizeForm_Activated(object sender, EventArgs e)
    {
      ((Form1)Parent.Parent).UpdateZoomExtFlag(true);
    }

    public void selectGeometry(MouseEventArgs e)
    {
      selected = null;
      try
      {
        int size = 5;
        OdGePoint2d center = new OdGePoint2d(e.X, e.Y);
        OdGePoint2d lowerLeft = new OdGePoint2d(center.x - size, center.y - size);
        OdGePoint2d upperRight = new OdGePoint2d(center.x + size, center.y + size);
        _pDevice.activeView().getViewport(lowerLeft, upperRight);
        var viewportTable = (OdDgViewGroupTable)_db.getViewGroupTable();
        OdDgElementId id = viewportTable.elementId();
        OdGePoint3d[] result = new OdGePoint3d[1];
        result[0] = getWorldCoordinateByScreen(center);

        using (OdDgSelectionSetIterator iter1 = (OdDgSelectionSetIterator)OdDgSelectionSet.select(_db, _pDevice.activeView(), result, OdDbVisualSelection.Mode.kPoint).newIterator())
        {
          while (!iter1.done())
          {
            OdDgElementId curId = iter1.objectId();
            selected = curId.safeOpenObject();
            Console.WriteLine(selected.GetType());
            iter1.next();
            curId.Dispose();
          }
        }
        center.Dispose();
        lowerLeft.Dispose();
        upperRight.Dispose();
        id.Dispose();
        viewportTable.Dispose();
      }
      catch (OdError err)
      {
        System.Diagnostics.Debug.WriteLine(err.InnerException.ToString());
      }
    }

    private void VectorizeForm_MouseMove(object sender, MouseEventArgs e)
    {
      selectGeometry(e);
    }
  }
}
