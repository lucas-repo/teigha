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
using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace OdViewExMgd
{
  public partial class Form1 : Form
  {
    enum Mode
    {
      Quiescent,
      Selection,
      DragDrop,
      ZoomWindow,
      Zoom,
      Orbit,
      Dolly
    }

    Teigha.Runtime.Services dd;
    Graphics graphics;
    Teigha.GraphicsSystem.LayoutHelperDevice helperDevice;
    Database database = null;
    ObjectIdCollection selected = new ObjectIdCollection();
    Point2d startSelPoint;

    LayoutManager lm;
    RectFram selRect;
    ExGripManager gripManager;
    Mode mouseMode;
    OrbitCtrl orbCtrl;
    OrbitTracker orbTracker;
    bool bDolly = false;
    UserSettings userSt;
    LayoutPaper lp;

    [DllImport("dwmapi.dll", EntryPoint = "DwmEnableComposition")]
    protected extern static uint Win32DwmEnableComposition(uint uCompositionAction);

    void DisableAero()
    {
      Win32DwmEnableComposition((uint)0);
    }

    public Form1()
    {
      dd = new Teigha.Runtime.Services();
      SystemObjects.DynamicLinker.LoadApp("GripPoints", false, false);
      SystemObjects.DynamicLinker.LoadApp("PlotSettingsValidator", false, false);
      SystemObjects.DynamicLinker.LoadApp("ExDynamicBlocks", false, false);
      SystemObjects.DynamicLinker.LoadApp("ExEvalWatchers", false, false);
      //SystemObjects.DynamicLinker.LoadApp("PdfModuleVINet", false, false);
      //SystemObjects.DynamicLinker.LoadApp("PdfModuleVI", false, false);
      InitializeComponent();
      this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

      HostApplicationServices.Current = new HostAppServ(this.progressBar);
      Environment.SetEnvironmentVariable("DDPLOTSTYLEPATHS", ((HostAppServ)HostApplicationServices.Current).FindConfigPath(String.Format("PrinterStyleSheetDir")));

      gripManager = new ExGripManager();
      mouseMode = Mode.Quiescent;
      orbTracker = new OrbitTracker();
      userSt = new UserSettings();
      UpdateMenuFile();

      lp = new LayoutPaper();
      RXClass nLt = RXObject.GetClass(typeof(Layout));
      nLt.AddX(LayoutPaperPE.GetClass(), lp);
      //DisableAero();
    }

    // helper function transforming parameters from screen to world coordinates
    void dolly(Teigha.GraphicsSystem.View pView, int x, int y) 
    {
      Vector3d vec = new Vector3d(-x, -y, 0.0);
      vec = vec.TransformBy((pView.ScreenMatrix * pView.ProjectionMatrix).Inverse());
      pView.Dolly(vec);
    }
    void Form1_MouseWheel(object sender, MouseEventArgs e)
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

    private void fileOpen(String sFilePath, bool bPartialMode, AuditInfo aiAppAudit)
    {
      if (lm != null)
      {
        lm.LayoutSwitched -= new Teigha.DatabaseServices.LayoutEventHandler(reinitGraphDevice);
        HostApplicationServices.WorkingDatabase = null;
        lm = null;
      }

      bool bLoaded = true;
      database = new Database(false, false);

      try
      {
        String sExt = sFilePath.Substring(sFilePath.Length - 4);
        sExt = sExt.ToUpper();

        if (aiAppAudit != null)
        {
          FileStreamBuf strBuf = new FileStreamBuf(openFileDialog.FileName);
          database.ReadDwgFile(strBuf.UnmanagedObject, false, "", aiAppAudit, bPartialMode);
        }
        else
        {
          if (sExt.EndsWith(".DWG"))
          {
            database.ReadDwgFile(sFilePath, FileOpenMode.OpenForReadAndAllShare, false, "", bPartialMode);
          }
          else if (sExt.EndsWith(".DXF"))
          {
            database.DxfIn(sFilePath, "");
          }
        }
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message);
        bLoaded = false;
      }



      if (bLoaded)
      {
        userSt.LastOpenedFile = sFilePath;
        UpdateMenuFile();
        userSt.Save();
        HostApplicationServices.WorkingDatabase = database;
        lm = LayoutManager.Current;
        lm.LayoutSwitched += new Teigha.DatabaseServices.LayoutEventHandler(reinitGraphDevice);
        String str = HostApplicationServices.Current.FontMapFileName;

        //menuStrip.
        exportToolStripMenuItem.Enabled = true;
        zoomToExtentsToolStripMenuItem.Enabled = true;
        zoomWindowToolStripMenuItem.Enabled = true;
        setAvtiveLayoutToolStripMenuItem.Enabled = true;
        fileDependencyToolStripMenuItem.Enabled = true;
        layersToolStripMenuItem.Enabled = true;
        panel1.Enabled = true;
        pageSetupToolStripMenuItem.Enabled = true;
        printPreviewToolStripMenuItem.Enabled = true;
        printToolStripMenuItem.Enabled = true;
        this.Text = String.Format("OdViewExMgd - [{0}]", sFilePath);

        initializeGraphics();
        Invalidate();
      }
    }


    private void openPartialModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == openFileDialog.ShowDialog(this))
      {
        fileOpen(openFileDialog.FileName, true, null);
      }
    }

    private void file_openByPath(object sender, EventArgs e)
    {
      fileOpen(((ToolStripItem)sender).Text, false, null);
    }

    private void file_open_handler(object sender, EventArgs e)
    {
      if (DialogResult.OK == openFileDialog.ShowDialog(this))
      {
        fileOpen(openFileDialog.FileName, false, null);
      }
    }

    private void repairToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == openFileDialog.ShowDialog(this))
      {
        using (AuditInfo aiAppAudit = new CustomAuditInfo("AuditReport.txt"))
        {
          aiAppAudit.FixErrors = true;
          fileOpen(openFileDialog.FileName, false, aiAppAudit);
        }
      }
    }

    void initializeGraphics()
    {
      try
      {
        graphics = Graphics.FromHwnd(panel1.Handle);
        // load some predefined rendering module (may be also "WinDirectX" or "WinOpenGL")
        using (GsModule gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("WinOpenGL.txv", false, true))
        {
          // create graphics device
          using (Teigha.GraphicsSystem.Device graphichsDevice = gsModule.CreateDevice())
          {
            // setup device properties
            using (Dictionary props = graphichsDevice.Properties)
            {
              if (props.Contains("WindowHWND")) // Check if property is supported
                props.AtPut("WindowHWND", new RxVariant(panel1.Handle)); // hWnd necessary for DirectX device
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
            gripManager.init(helperDevice, helperDevice.Model, database);
          }
        }
        // set palette
        helperDevice.SetLogicalPalette(Device.DarkPalette);
        // set output extents
        resize();
        changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.Optimized2D);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      if (helperDevice != null)
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
    }

    private void InterruptSelection()
    {
      if (selRect != null)
        Aux.activeTopView(database, helperDevice).Erase(selRect);
      selRect = null;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (ObjectOverrule.Overruling)
        ObjectOverrule.RemoveOverrule(RXClass.GetClass(typeof(Teigha.DatabaseServices.Line)), LineOverrule.overrule);
      InterruptSelection();
      gripManager.uninit();
      gripManager = null;

      RXObject.GetClass(typeof(Layout)).DelX(LayoutPaperPE.GetClass());

      if (graphics != null)
        graphics.Dispose();
      if (helperDevice != null)
        helperDevice.Dispose();
      if (database != null)
        database.Dispose();
      dd.Dispose();
    }
    void resize()
    {
      if (helperDevice != null)
      {
        //Rectangle r = panel1.Bounds;
        //r.Offset(-panel1.Location.X, -panel1.Location.Y);
        // HDC assigned to the device corresponds to the whole client area of the panel
        //helperDevice.OnSize(r);
        helperDevice.OnSize(panel1.Size);
        Invalidate();
      }
    }
    private void panel1_Resize(object sender, EventArgs e)
    {
      resize();
    }
    bool get_layout_extents(Database db, Teigha.GraphicsSystem.View pView, ref BoundBlock3d bbox)
    {
      BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
      BlockTableRecord pSpace = (BlockTableRecord)bt[BlockTableRecord.PaperSpace].GetObject(OpenMode.ForRead);
      Layout pLayout = (Layout)pSpace.LayoutId.GetObject(OpenMode.ForRead);
      Extents3d ext = new Extents3d();
      if (pLayout.GetViewports().Count > 0)
      {
        bool bOverall = true;
        foreach (ObjectId id in pLayout.GetViewports())
        {
          if (bOverall)
          {
            bOverall = false;
            continue;
          }
          Teigha.DatabaseServices.Viewport pVp = (Teigha.DatabaseServices.Viewport)id.GetObject(OpenMode.ForRead);
        }
        ext.TransformBy(pView.ViewingMatrix);
        bbox.Set(ext.MinPoint, ext.MaxPoint);
      }
      else
      {
        ext = pLayout.Extents;
      }
      bbox.Set(ext.MinPoint, ext.MaxPoint);
      return ext.MinPoint != ext.MaxPoint;
    }
    void zoom_extents(Teigha.GraphicsSystem.View pView, DBObject pVpObj)
    {
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
            bBboxValid = get_layout_extents(database, pView, ref bbox);
          }
        }
        else if (!bBboxValid) // model space viewport
        {
          bBboxValid = get_layout_extents(database, pView, ref bbox);
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
    }
    // the same as Editor.ActiveViewportId if ApplicationServices are available

    private void zoom_extents_handler(object sender, EventArgs e)
    {
      using (DBObject pVpObj = Aux.active_viewport_id(database).GetObject(OpenMode.ForWrite))
      {
        // using protocol extensions we handle PS and MS viewports in the same manner
        AbstractViewportData pAVD = new AbstractViewportData(pVpObj);
        Teigha.GraphicsSystem.View pView = pAVD.GsView;
        // do actual zooming - change GS view
        zoom_extents(pView, pVpObj);
        // save changes to database
        pAVD.SetView(pView);
        pAVD.Dispose();
        pVpObj.Dispose();
        Invalidate();
      }
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
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
          if (orbCtrl != null)
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              toolStripButtonOrbit.Checked = false;
              pView.Erase(orbCtrl);
              orbTracker.reset();
              orbCtrl = null;
            }
          }
          break;
      }
      mouseMode = Mode.Quiescent;

      foreach (ObjectId id in selected)
      {
        gripManager.removeEntityGrips(id, true);
      }
      selected.Clear();
    }

    private void reinitGraphDevice(object sender, Teigha.DatabaseServices.LayoutEventArgs e)
    {
      disableCommand();
      helperDevice.Dispose();
      graphics.Dispose();
      initializeGraphics();
    }

    private void setActiveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (helperDevice != null)
      {
        SelectLayouts selLayoutForm = new SelectLayouts(database);
        selLayoutForm.Show();
      }
    }

    private void fileDependencyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (helperDevice != null)
      {
        File_Dependency fileDependencyForm = new File_Dependency(database);
        fileDependencyForm.Show();
      }
    }


    private void panel1_MouseMove(object sender, MouseEventArgs e)
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
            gripManager.setValue(toEyeToWorld(e.X, e.Y));
            Invalidate();
            break;
          }
        case Mode.ZoomWindow:
        case Mode.Selection:
          {
            selRect.setValue(toEyeToWorld(e.X, e.Y));
            Invalidate();
            break;
          }
        case Mode.Orbit:
          {
            orbTracker.setValue(toEyeToWorld(e.X, e.Y));
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

    private void panel1_MouseUp(object sender, MouseEventArgs e)
    {
      switch (mouseMode)
      {
        case Mode.Orbit:
          {
            orbTracker.reset();
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

    ObjectId blockToSearch(ObjectId vpId)
    {
      using (DBObject pDbObj = vpId.GetObject(OpenMode.ForRead))
      {
        if (pDbObj is Teigha.DatabaseServices.Viewport)
        {
          Teigha.DatabaseServices.Viewport pVp = (Teigha.DatabaseServices.Viewport)pDbObj;
          return pVp.Number == 1 ? SymbolUtilityServices.GetBlockPaperSpaceId(database) : SymbolUtilityServices.GetBlockModelSpaceId(database);
        }
        return SymbolUtilityServices.GetBlockModelSpaceId(database); 
      }
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
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
                  selRect = new RectFram(toEyeToWorld(e.X, e.Y));
                  selRect.Add(Aux.activeTopView(database, helperDevice));
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
                new SR(selected, blockToSearch(new ObjectId(new IntPtr(pView.ClientViewInfo.ViewportObjectId)))), startSelPoint.X < e.X ? Teigha.GraphicsSystem.SelectionMode.Window : Teigha.GraphicsSystem.SelectionMode.Crossing);

              updateSelection(selected);
            }
            mouseMode = Mode.Quiescent;
            break;
          }
        case Mode.DragDrop:
          {
            using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
            {
              gripManager.DragFinal(toEyeToWorld(e.X, e.Y), true);
              Regenerate();
            }
            mouseMode = Mode.Quiescent;
            break;
          }
        case Mode.Orbit:
          {
            orbTracker.init(helperDevice.ActiveView, toEyeToWorld(e.X, e.Y));
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

    private Point3d toEyeToWorld(int x, int y)
    {
      using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
      {
        Point3d wcsPt = new Point3d(x, y, 0.0);
        wcsPt = wcsPt.TransformBy((pView.ScreenMatrix * pView.ProjectionMatrix).Inverse());
        wcsPt = new Point3d(wcsPt.X, wcsPt.Y, 0.0);
        using (AbstractViewPE pVpPE = new AbstractViewPE(pView))
        {
          return wcsPt.TransformBy(pVpPE.EyeToWorld);
        }
      }
    }

    private void ZoomWindow(Point3d pt1, Point3d pt2)
    {
      using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
      {
        using (AbstractViewPE pVpPE = new AbstractViewPE(pView))
        {
          pt1 = pt1.TransformBy(pVpPE.WorldToEye);
          pt2 = pt2.TransformBy(pVpPE.WorldToEye);
          Vector3d eyeVec = pt2 - pt1;

          if (((eyeVec.X < -1E-10) || (eyeVec.X > 1E-10)) && ((eyeVec.Y < -1E-10) || (eyeVec.Y > 1E-10)))
          {
            Point3d newPos = pt1 + eyeVec / 2.0;
            pView.Dolly(newPos.GetAsVector());
            double wf = pView.FieldWidth / Math.Abs(eyeVec.X);
            double hf = pView.FieldHeight / Math.Abs(eyeVec.Y);
            pView.Zoom(wf < hf ? wf : hf);
            Invalidate();
          }
        }
      }
    }

    private void zoomWindowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      InterruptSelection();
      mouseMode = Mode.Zoom;
    }

    private void panel1_MouseClick(object sender, MouseEventArgs e)
    {
      if (mouseMode == Mode.ZoomWindow)
      {
        using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
        {
          if (Aux.SelectWindowPrint)
          {
            using (Teigha.GraphicsSystem.View pViewTmp = helperDevice.ActiveView)
            {
              using (AbstractViewPE pVpPE = new AbstractViewPE(pViewTmp))
              {
                Point3d pt1 = selRect.BasePoint.TransformBy(pVpPE.WorldToEye);
                Point3d pt2 = toEyeToWorld(e.X, e.Y).TransformBy(pVpPE.WorldToEye);
                SetWindowPrint(pt1, pt2);
                Invalidate();
              }
            }
            Aux.SelectWindowPrint = false;

            pageSetupToolStripMenuItem_Click(null, null);
          }
          else
            ZoomWindow(selRect.BasePoint, toEyeToWorld(e.X, e.Y));
          InterruptSelection();
          mouseMode = Mode.Quiescent;
        }
      }
      if (mouseMode == Mode.Zoom)
      {
        mouseMode = Mode.ZoomWindow;
        selRect = new RectFram(toEyeToWorld(e.X, e.Y));
        selRect.Add(Aux.activeTopView(database, helperDevice));
      }
    }

    private void exportToPDFToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PDFExport PDFExportForm = new PDFExport(database);
      PDFExportForm.Show();
    }

    private void saveBitmapToolStripMenuItem_Click(object sender, EventArgs e)
    {
      BMPExport bmpExport = new BMPExport(database);
      bmpExport.Show();
    }

    private void exportToDWFToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.DWF_export(database, helperDevice);
    }

    private void publish3dToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.Publish3d(database, helperDevice);
    }

    private void exportToSVGToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.SVG_export(database);
    }

    private void publishToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.Publish(database, helperDevice);
    }

    private void importDwfToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.Dwf_import(database);
    }
    bool newRegApp(Database db, string regAppName)
    {
      using (RegAppTable pRegAppTable = (RegAppTable)db.RegAppTableId.GetObject(OpenMode.ForWrite))
      {
        if (!pRegAppTable.Has(regAppName))
        {
          using (RegAppTableRecord pRegApp = new RegAppTableRecord())
          {
            pRegApp.Name = regAppName;
            pRegAppTable.Add(pRegApp);
          }
          return true;
        }
      }
      return false;
    }

    private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (DBObject pVpObj = Aux.active_viewport_id(database).GetObject(OpenMode.ForWrite))
      {
        AbstractViewportData pAVD = new AbstractViewportData(pVpObj);
        pAVD.SetView(helperDevice.ActiveView);
      }

      TransactionManager tm = database.TransactionManager;
      using (Transaction ta = tm.StartTransaction())
      {
        using (BlockTableRecord blTableRecord = (BlockTableRecord)database.CurrentSpaceId.GetObject(OpenMode.ForRead))
        {
          using (Layout pLayObj = (Layout)blTableRecord.LayoutId.GetObject(OpenMode.ForWrite))
          {
            PlotSettings ps = (PlotSettings)pLayObj;
            Print.PageSetup pageSetupDlg = new Print.PageSetup(ps);
            if (pageSetupDlg.ShowDialog() == DialogResult.OK)
            {        
              ta.Commit();
              if (Aux.SelectWindowPrint)
                zoomWindowToolStripMenuItem_Click(null, null);
            }
            else
            {
              ta.Abort();
            }
          }
        }
      }
    }

    private void SetWindowPrint(Point3d pt1, Point3d pt2)
    {
      TransactionManager tm = database.TransactionManager;
      using (Transaction ta = tm.StartTransaction())
      {
        using (BlockTableRecord blTableRecord = (BlockTableRecord)database.CurrentSpaceId.GetObject(OpenMode.ForRead))
        {
          using (Layout pLayObj = (Layout)blTableRecord.LayoutId.GetObject(OpenMode.ForWrite))
          {
            PlotSettings ps = (PlotSettings)pLayObj;
            PlotSettingsValidator plotSettingVal = PlotSettingsValidator.Current;
            Extents2d ext = new Extents2d(pt1.X, pt1.Y, pt2.X, pt2.Y);
            plotSettingVal.SetPlotWindowArea(ps, ext);
          }
        }
        ta.Commit();
      }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (database != null)
      {
        if (DialogResult.OK == saveAsFileDialog.ShowDialog(this))
        {
          database.SaveAs(saveAsFileDialog.FileName, DwgVersion.Current, true);
        }
      }
    }

    private void printToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Print.Printing pr = new Print.Printing();
      pr.Print(database, helperDevice.ActiveView, false);
    }

    private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Print.Printing pr = new Print.Printing();
      pr.Print(database, helperDevice.ActiveView, true);
    }

    void changeActiveViewMode(Teigha.GraphicsSystem.RenderMode rndMode)
    {
      helperDevice.ActiveView.Mode = rndMode;
      helperDevice.Model.Invalidate(InvalidationHint.kInvalidateAll);
    }

    private void toolStripDrawStyle_Click(object sender, EventArgs e)
    {
      ToolStripButton bt = (ToolStripButton)sender;
      toolStripWire2d.Checked           = false;
      toolStripWire3d.Checked           = false;
      toolStripHidden.Checked           = false;
      toolStripFlatShaded.Checked       = false;
      toolStripFlatShadedEd.Checked     = false;
      toolStripGroundedShaded.Checked   = false;
      toolStripGroundedShadedEd.Checked = false;
      switch (bt.Name)
      {
        case "toolStripWire2d":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.Optimized2D);
          toolStripWire2d.Checked = true;
          break;
        case "toolStripWire3d":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.Wireframe);
          toolStripWire3d.Checked = true;
          break;
        case "toolStripHidden":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.HiddenLine);
          toolStripHidden.Checked = true;
          break;
        case "toolStripFlatShaded":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.FlatShaded);
          toolStripFlatShaded.Checked = true;
          break;
        case "toolStripFlatShadedEd":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.FlatShadedWithWireframe);
          toolStripFlatShadedEd.Checked = true;
          break;
        case "toolStripGroundedShaded":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.GouraudShaded);
          toolStripGroundedShaded.Checked = true;
          break;
        case "toolStripGroundedShadedEd":
          changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.GouraudShadedWithWireframe);
          toolStripGroundedShadedEd.Checked = true;
          break;
      }
      Invalidate();
    }

    private void toolStripButtonOrbit_Click(object sender, EventArgs e)
    {
      toolStripButtonOrbit.Checked = !toolStripButtonOrbit.Checked;
      if (toolStripButtonOrbit.Checked)
      {
        if (mouseMode == Mode.Quiescent && database.TileMode)
        {
          mouseMode = Mode.Orbit;
          orbCtrl = new OrbitCtrl();
          using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
          {
            orbCtrl.Add(pView);
          }
        }
        else
        {
          toolStripButtonOrbit.Checked = false;
        }
      }
      else
      {
        if (orbCtrl != null)
        {
          using (Teigha.GraphicsSystem.View pView = helperDevice.ActiveView)
          {
            pView.Erase(orbCtrl);
            orbCtrl = null;
            orbTracker.reset();
            mouseMode = Mode.Quiescent;
          }
        }
      }
      Invalidate();
    }

    private void toolStripButtonDolly_Click(object sender, EventArgs e)
    {
      toolStripButtonDolly.Checked = !toolStripButtonDolly.Checked;
      if (toolStripButtonDolly.Checked)
      {
        if (mouseMode == Mode.Quiescent)
        {
          mouseMode = Mode.Dolly;
        }
        else
        {
          toolStripButtonDolly.Checked = false;
        }
      }
      else
      {
         mouseMode = Mode.Quiescent;
      }
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

    private void blockToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (helperDevice != null)
      {
        using(BlockTable bt = (BlockTable)database.BlockTableId.GetObject(OpenMode.ForRead))
        {
          BlockInsert blIns = new BlockInsert(bt);
          if (DialogResult.OK == blIns.ShowDialog(this))
          {
            Regenerate();
          }
        }
      }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (database != null)
      {
        database.Save();
      }
    }

    private void Regenerate()
    {
      helperDevice.Model.Invalidate(InvalidationHint.kInvalidateAll);
      Invalidate();
    }

    private void regenerateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Regenerate();
    }

    private void overrulingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem mi = (ToolStripMenuItem)sender;
      if (mi.Checked)
      {
        ObjectOverrule.RemoveOverrule(RXClass.GetClass(typeof(Teigha.DatabaseServices.Line)), LineOverrule.overrule);
        ObjectOverrule.RemoveOverrule(RXClass.GetClass(typeof(Teigha.DatabaseServices.RasterImage)), RasterImageOverrule.overrule);
      }
      else
      {
        ObjectOverrule.AddOverrule(RXClass.GetClass(typeof(Teigha.DatabaseServices.Line)), LineOverrule.overrule, true);
        ObjectOverrule.AddOverrule(RXClass.GetClass(typeof(Teigha.DatabaseServices.RasterImage)), RasterImageOverrule.overrule, true);
      }
      ObjectOverrule.Overruling = mi.Checked = !mi.Checked;
    }

    private void UpdateMenuFile()
    {
      StringCollection sLastOpenedFile = userSt.LastOpenedFiles;
      if (sLastOpenedFile.Count > 0)
      {
        int iStartIndex = fileToolStripMenuItem.DropDownItems.IndexOf(toolStripSeparator5);
        int iEndIndex = fileToolStripMenuItem.DropDownItems.IndexOf(toolStripSeparator6);
        foreach (String str in sLastOpenedFile)
        {
          iStartIndex++;
          if (iStartIndex < iEndIndex)
          {
            fileToolStripMenuItem.DropDownItems[iStartIndex].Text = str;
          }
          else
          {
            ToolStripButton ts = new ToolStripButton(str);
            ts.Click += new System.EventHandler(this.file_openByPath);
            fileToolStripMenuItem.DropDownItems.Insert(iStartIndex, ts);
          }
        }
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImExport.Image_export(database, helperDevice, panel1.Width, panel1.Height);
    }

    private void layersToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (helperDevice != null)
      {
        Layers layersForm = new Layers(database, helperDevice);
        layersForm.Show();
      }
    }
  }
}