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
using System.Text;
using System.Windows.Forms;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;


namespace OdaMgdMViewApp
{
  public partial class ChildForm : Form
  {
    LayoutManager lm;
    Database database = null;
    Graphics graphics;
    ExGripManager gripManager;
    Mode mouseMode;
    Teigha.GraphicsSystem.LayoutHelperDevice helperDevice;

    ObjectIdCollection selected = new ObjectIdCollection();
    Point2d startSelPoint;
    RectFram selRect;
    bool bDolly = false;

    public ChildForm(String sFile, bool isPartialLoad)
    {
      InitializeComponent();
      gripManager = new ExGripManager();
      mouseMode = Mode.Quiescent;

      FileOpen(sFile, isPartialLoad);

      //orbTracker = new OrbitTracker();
    }

    private void FileOpen(String sFile, bool isPartialLoad)
    {
      if (sFile != "")
      {
        if (lm != null)
        {
          HostApplicationServices.WorkingDatabase = null;
          lm = null;
        }

        bool bLoaded = true;
        database = new Database(false, false);
        
        try
        {
          String sExt = sFile.Substring(sFile.Length - 4);
          sExt = sExt.ToUpper();
          if (sExt.EndsWith(".DWG"))
          {
            database.ReadDwgFile(sFile, FileOpenMode.OpenForReadAndAllShare, false, "", isPartialLoad);
          }
          else if (sFile.EndsWith(".DXF"))
          {
            database.DxfIn(sFile, "");
          }
        }
        catch (System.Exception ex)
        {
          MessageBox.Show(ex.Message);
          bLoaded = false;
        }

        if (bLoaded)
        {
          HostApplicationServices.WorkingDatabase = database;
          lm = LayoutManager.Current;
          this.Text = sFile;
 
          initializeGraphics();
          Invalidate();
        }
      }
    }

    void initializeGraphics()
    {
      try
      {
        graphics = Graphics.FromHwnd(panel.Handle);
        // load some predefined rendering module (may be also "WinDirectX" or "WinOpenGL")
        using (GsModule gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("WinDirectX.txv", false, true))
        {
          if (null != gsModule)
          {
            // create graphics device
            using (Teigha.GraphicsSystem.Device graphichsDevice = gsModule.CreateDevice())
            {
              if (null != graphichsDevice)
              {
                // setup device properties
                using (Dictionary props = graphichsDevice.Properties)
                {
                  if (props.Contains("WindowHWND")) // Check if property is supported
                    props.AtPut("WindowHWND", new RxVariant(panel.Handle)); // hWnd necessary for DirectX device
                  if (props.Contains("WindowHDC")) // Check if property is supported
                    props.AtPut("WindowHDC", new RxVariant(graphics.GetHdc())); // hWindowDC necessary for Bitmap device
                  if (props.Contains("DoubleBufferEnabled")) // Check if property is supported
                    props.AtPut("DoubleBufferEnabled", new RxVariant(true));
                  if (props.Contains("EnableSoftwareHLR")) // Check if property is supported
                    props.AtPut("EnableSoftwareHLR", new RxVariant(true));
                  if (props.Contains("DiscardBackFaces")) // Check if property is supported
                    props.AtPut("DiscardBackFaces", new RxVariant(true));
                }
                // setup paperspace viewports or tiles
                ContextForDbDatabase ctx = new ContextForDbDatabase(database);
                ctx.UseGsModel = true;

                helperDevice = LayoutHelperDevice.SetupActiveLayoutViews(graphichsDevice, ctx);
                Aux.preparePlotstyles(database, ctx);

                this.Paint += new System.Windows.Forms.PaintEventHandler(this.ChildForm_Paint);
                this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.ChildForm_Paint);
                this.MouseWheel += new MouseEventHandler(ChildForm_MouseWheel);

                // set palette
                helperDevice.SetLogicalPalette(Device.DarkPalette);

                // set output extents
                resize();
              }
            }
          }
        }
        gripManager.init(helperDevice, helperDevice.Model, database);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }


    void resize()
    {
      if (helperDevice != null)
      {
        Rectangle r = panel.Bounds;
        r.Offset(-panel.Location.X, -panel.Location.Y);
        // HDC assigned to the device corresponds to the whole client area of the panel
        helperDevice.OnSize(r);
        Invalidate();
      }
    }

    private void ChildForm_Paint(object sender, PaintEventArgs e)
    {
      try
      {
        helperDevice.Update();
      }
      catch (System.Exception ex)
      {
        graphics.DrawString(ex.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), new PointF(150.0F, 150.0F));
      }
    }

    private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      InterruptSelection();
      gripManager.uninit();
      gripManager = null;
      if (helperDevice != null)
      {
        helperDevice.Dispose();
        helperDevice = null;
      }
      if (graphics != null)
      {
        graphics.Dispose();
        graphics = null;
      }
      if (database != null)
      {
        database.Dispose();
        database = null;
      }
    }

    // helper function transforming parameters from screen to world coordinates
    void dolly(Teigha.GraphicsSystem.View pView, int x, int y)
    {
      Vector3d vec = new Vector3d(-x, -y, 0.0);
      vec = vec.TransformBy((pView.ScreenMatrix * pView.ProjectionMatrix).Inverse());
      pView.Dolly(vec);
    }

    void ChildForm_MouseWheel(object sender, MouseEventArgs e)
    {
      using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
      {
        // camera position in world coordinates
        Point3d pos = pView.Position;
        // TransformBy() returns a transformed copy
        pos = pos.TransformBy(pView.WorldToDeviceMatrix);
        int vx = (int)pos.X;
        int vy = (int)pos.Y;
        vx = e.X - vx;
        vy = e.Y - vy;
        // we move point of view to the mouse location, to create an illusion of scrolling in/out there
        dolly(pView, -vx, -vy);
        // note that we essentially ignore delta value (sign is enough for illustrative purposes)
        pView.Zoom(e.Delta > 0 ? 1.0 / 0.9 : 0.9);
        dolly(pView, vx, vy);
        //
        Invalidate();
      }
    }

    private void ChildForm_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Escape:
          disableCommand();
          if (helperDevice != null)
            helperDevice.Invalidate();
          Invalidate();
          break;
        case Keys.Oemplus:
          break;
        case Keys.OemMinus:
          break;
        case Keys.Delete:
          erase();
          break;
      }
    }

    void disableCommand()
    {
      switch (mouseMode)
      {
        case Mode.DragDrop:
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              gripManager.DragFinal(new Point3d(), false);
              Regenerate();
            }
            break;
          }
        case Mode.Selection:
          InterruptSelection();
          break;
        case Mode.Orbit:
          /*if (orbCtrl != null)
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              //pView.Erase(orbCtrl);
              //orbTracker.reset();
             // orbCtrl = null;
            }
          }*/
          break;
      }
      mouseMode = Mode.Quiescent;

      foreach (ObjectId id in selected)
      {
        gripManager.removeEntityGrips(id, true);
      }
      selected.Clear();
    }

    void erase()
    {
      foreach (ObjectId id in selected)
      {
        using (Entity ent = (Entity)id.GetObject(OpenMode.ForWrite))
        {
          using (LayerTableRecord ltr = (LayerTableRecord)ent.LayerId.GetObject(OpenMode.ForRead))
          {
            if (!ltr.IsLocked)
            {
              gripManager.removeEntityGrips(id, true);
              ent.Erase();
            }
          }
        }
      }
      selected.Clear();
      Regenerate();
    }

    private void Regenerate()
    {
      helperDevice.Model.Invalidate(InvalidationHint.kInvalidateAll);
      Invalidate();
    }

    private void ChildForm_MouseMove(object sender, MouseEventArgs e)
    {
      switch (mouseMode)
      {
        case Mode.Quiescent:
          {
            if (gripManager.onMouseMove(e.X, e.Y))
              Invalidate();
            break;
          }
        case Mode.DragDrop:
          {
            gripManager.setValue(Aux.toEyeToWorld(helperDevice, e.X, e.Y));
            Invalidate();
            break;
          }
        case Mode.ZoomWindow:
        case Mode.Selection:
          {
            selRect.setValue(Aux.toEyeToWorld(helperDevice, e.X, e.Y));
            Invalidate();
            break;
          }
        case Mode.Orbit:
          {
            //orbTracker.setValue(Aux.toEyeToWorld(helperDevice, e.X, e.Y));
            Invalidate();
            break;
          }
        case Mode.Dolly:
          {
            if (bDolly)
            {
              using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
              {
                dolly(pView, (int)(e.X - startSelPoint.X), (int)(e.Y - startSelPoint.Y));
                startSelPoint = new Point2d(e.X, e.Y);
                Invalidate();
              }
            }
            break;
          }
        default:
          break;
      }
    }

    private void ChildForm_MouseUp(object sender, MouseEventArgs e)
    {
      switch (mouseMode)
      {
        case Mode.Orbit:
          {
            //orbTracker.reset();
            break;
          }
        case Mode.Dolly:
          {
            bDolly = false;
            break;
          }
        default:
          break;
      }
    }

    private void updateSelection(ObjectIdCollection selected)
    {
      InterruptSelection();
      gripManager.updateSelection(selected);
      helperDevice.Invalidate();
      Invalidate();
    }

    private void ChildForm_MouseDown(object sender, MouseEventArgs e)
    {
      switch (mouseMode)
      {
        case Mode.Quiescent:
          {
            if (gripManager.OnMouseDown(e.X, e.Y))
            {
              mouseMode = Mode.DragDrop;
            }
            else
            {
              //select one object
              using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
              {
                ObjectIdCollection tmpColl = new ObjectIdCollection();
                pView.Select(new Point2dCollection(new Point2d[] { new Point2d(e.X - 1, e.Y - 1), new Point2d(e.X + 1, e.Y + 1) }),
                  new SR(tmpColl, database.CurrentSpaceId), Teigha.GraphicsSystem.SelectionMode.Crossing);

                if (tmpColl.Count > 0)
                {
                  if (selected.Count > 0)
                  {
                    foreach (ObjectId objId in tmpColl)
                    {
                      if (selected.Contains(objId))
                      {
                        gripManager.MoveAllSelected(e.X, e.Y);
                        mouseMode = Mode.DragDrop;
                        return;
                      }
                    }
                    foreach (ObjectId objId in tmpColl)
                    {
                      selected.Add(objId);
                    }
                  }
                  else
                  {
                    selected = tmpColl;
                  }
                  updateSelection(selected);
                }
                else
                {
                  selRect = new RectFram(Aux.toEyeToWorld(helperDevice, e.X, e.Y));
                  pView.Add(selRect);
                  startSelPoint = new Point2d(e.X, e.Y);
                  Invalidate();
                  mouseMode = Mode.Selection;
                }
              }
            }
            break;
          }
        case Mode.Selection:
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              pView.Select(new Point2dCollection(new Point2d[] { startSelPoint, new Point2d(e.X, e.Y) }),
                new SR(selected, database.CurrentSpaceId), startSelPoint.X < e.X ? Teigha.GraphicsSystem.SelectionMode.Window : Teigha.GraphicsSystem.SelectionMode.Crossing);

              updateSelection(selected);
            }
            mouseMode = Mode.Quiescent;
            break;
          }
        case Mode.DragDrop:
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              gripManager.DragFinal(Aux.toEyeToWorld(helperDevice, e.X, e.Y), true);
              Regenerate();
            }
            mouseMode = Mode.Quiescent;
            break;
          }
        case Mode.Orbit:
          {
            //orbTracker.init(helperDevice.ActiveView, Aux.toEyeToWorld(helperDevice, e.X, e.Y));
            break;
          }
        case Mode.Dolly:
          {
            startSelPoint = new Point2d(e.X, e.Y);
            bDolly = true;
            break;
          }
        default:
          break;
      }
    }

    private void InterruptSelection()
    {
      if (selRect != null)
        helperDevice.ActiveView.Erase(selRect);
      selRect = null;
    }

    public void ZoomExtents()
    {
      using (DBObject pVpObj = Aux.active_viewport_id(database).GetObject(OpenMode.ForWrite))
      {
        // using protocol extensions we handle PS and MS viewports in the same manner
        AbstractViewportData pAVD = new AbstractViewportData(pVpObj);
        Teigha.GraphicsSystem.View pView = pAVD.GsView;
        // do actual zooming - change GS view
        // here protocol extension is used again, that provides some helpful functions
        using (AbstractViewPE pVpPE = new AbstractViewPE(pView))
        {
          BoundBlock3d bbox = new BoundBlock3d();
          bool bBboxValid = pVpPE.GetViewExtents(bbox);

          // paper space overall view
          if (pVpObj is Teigha.DatabaseServices.Viewport && ((Teigha.DatabaseServices.Viewport)pVpObj).Number == 1)
          {
            if (!bBboxValid || !(bbox.GetMinimumPoint().X < bbox.GetMaximumPoint().X && bbox.GetMinimumPoint().Y < bbox.GetMaximumPoint().Y))
            {
              bBboxValid = Aux.get_layout_extents(database, pView, ref bbox);
            }
          }
          else if (!bBboxValid) // model space viewport
          {
            bBboxValid = Aux.get_layout_extents(database, pView, ref bbox);
          }

          if (!bBboxValid)
          {
            // set to somewhat reasonable (e.g. paper size)
            if (database.Measurement == MeasurementValue.Metric)
            {
              bbox.Set(Point3d.Origin, new Point3d(297.0, 210.0, 0.0)); // set to papersize ISO A4 (portrait)
            }
            else
            {
              bbox.Set(Point3d.Origin, new Point3d(11.0, 8.5, 0.0)); // ANSI A (8.50 x 11.00) (landscape)
            }
            bbox.TransformBy(pView.ViewingMatrix);
          }

          pVpPE.ZoomExtents(bbox);
        }

        // save changes to database
        pAVD.SetView(pView);
        pAVD.Dispose();
        pVpObj.Dispose();
        Invalidate();
      }
    }

    private void ChildForm_ResizeEnd(object sender, EventArgs e)
    {
      resize();
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      database.Save();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == saveAsFileDialog.ShowDialog())
      {
        int version = saveAsFileDialog.FilterIndex % 7;
        if (database != null)
        {
            DwgVersion vers = DwgVersion.Current;
            if (0 == version)
              vers = DwgVersion.vAC12;
            else
            {
              if (1 == version)
                vers = DwgVersion.vAC24;
              else
              {
                if (2 == version)
                  vers = DwgVersion.vAC21;
                else
                {
                  if (3 == version)
                    vers = DwgVersion.vAC18;
                  else
                  {
                    if (4 == version)
                      vers = DwgVersion.vAC15;
                    else if (5 == version)
                      vers = DwgVersion.vAC14;
                  }
                }
              }
            }
            if (Math.Truncate((double)saveAsFileDialog.FilterIndex / 7) == 0)
              database.SaveAs(saveAsFileDialog.FileName, vers);
            else
              database.DxfOut(saveAsFileDialog.FileName, 16, vers);
        }
      }
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (database.HasUndo)
      {
        database.Undo();
        Invalidate();
      }
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (database.HasRedo)
      { 
        database.Redo();
        Invalidate();
      }
    }

    private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      redoToolStripMenuItem.Enabled = database.HasRedo;
      undoToolStripMenuItem.Enabled = database.HasUndo;
    }
  }
}