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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;
using Teigha.Colors;


namespace OdaMgdMViewApp
{
  public class ExGripDataSubent
  {
    FullSubentityPath m_entPath;
    ExGripDataCollection m_pSubData;

    public ExGripDataSubent()
    {
      m_entPath = new FullSubentityPath();
      m_pSubData = new ExGripDataCollection();
    }

    public FullSubentityPath EntPath
    {
      get { return m_entPath; }
      set { m_entPath = value; }
    }

    public ExGripDataCollection SubData
    {
      get { return m_pSubData; }
      set { m_pSubData = value; }
    }
  };

  public class ExGripDataSubentCollection : CollectionBase
  {
    //strongly typed accessor
    public ExGripDataSubent this[int index]
    {
      get
      {
        return (ExGripDataSubent)this.List[index];
      }
      set
      {
        this.List[index] = value;
      }
    }

    public void Add(ExGripDataSubent grData)
    {
      this.List.Add(grData);
    }
  }

  public class ExGripDataExt
  {
    ExGripDataCollection m_pDataArray;
    ExGripDataSubentCollection m_pDataSub;
    public ExGripDataExt()
    {
      m_pDataArray = new ExGripDataCollection();
      m_pDataSub   = new ExGripDataSubentCollection();
    }

    public ExGripDataCollection DataArray
    {
      get { return m_pDataArray; }
      set { m_pDataArray = value; }
    }

    public ExGripDataSubentCollection DataSub
    {
      get { return m_pDataSub; }
      set { m_pDataSub = value; }
    }
  };

  public class ExGripManager
  {
    LayoutHelperDevice m_pDevice;
    Model m_pGsModel;
    Database m_pDb;

    int m_GRIPSIZE;
    int m_GRIPOBJLIMIT;
    EntityColor m_GRIPCOLOR;
    EntityColor m_GRIPHOVER;
    EntityColor m_GRIPHOT;
    ExGripDataCollection m_hoverGripsColl;
    ExGripDragCollection m_aDrags;

    Point3d m_ptBasePoint;
    Point3d m_ptLastPoint;
    Dictionary<ObjectId, ExGripDataExt> m_gripDataDict;

    ExGripData pGripTest;
    ExGripDataCollection aActiveKeys;

    public ExGripManager()
    {
      m_hoverGripsColl = new ExGripDataCollection();
      m_ptBasePoint = Point3d.Origin;
      m_ptLastPoint = Point3d.Origin;
      m_gripDataDict = new Dictionary<ObjectId, ExGripDataExt>();
      m_aDrags = new ExGripDragCollection();
    }

    public void uninit()
    {
      m_pDevice = null;
      m_pGsModel = null;
      m_hoverGripsColl = null;
      m_gripDataDict = null;
      m_aDrags = null;
      m_pDb = null;
    }

    public void init(LayoutHelperDevice pDevice, Model gsModule, Database pDb/* OdDbCommandContext ctx*/ )
    {
      m_pDevice = pDevice;
      m_pGsModel = gsModule;
      m_pDb = pDb;


      //pDb->addReactor(&m_cDbReactor);

      HostApplicationServices pAppSvcs = HostApplicationServices.Current;
      m_GRIPSIZE     = pAppSvcs.GRIPSIZE;
      m_GRIPOBJLIMIT = pAppSvcs.GRIPOBJLIMIT;
      m_GRIPCOLOR    = new EntityColor(ColorMethod.ByAci, pAppSvcs.GRIPCOLOR);
      m_GRIPHOVER    = new EntityColor(ColorMethod.ByAci, pAppSvcs.GRIPHOVER);
      m_GRIPHOT      = new EntityColor(ColorMethod.ByAci, pAppSvcs.GRIPHOT);

      //m_pGetSelectionSetPtr = pGetSSet;

    }

    public ExGripDataSubent GetSubentGripData(ExGripDataExt ext, FullSubentityPath entPath)
    {
      int i = 0;
      foreach (ExGripDataSubent subData in ext.DataSub)
      {
        if (subData.EntPath == entPath)
          return ext.DataSub[i];
        ++i;
      }
      return ext.DataSub[0];
    }

    public bool startHover( int x, int y )
    {
      bool bRet = endHover();
      ExGripDataCollection aKeys = new ExGripDataCollection();
      locateGripsAt(x, y, ref aKeys);

      if(0 != aKeys.Count)
      {
        m_hoverGripsColl = aKeys;
        foreach (ExGripData gr in m_hoverGripsColl)
        {
          if(GripData.DrawType.WarmGrip == gr.Status) 
          {
            gr.Status = GripData.DrawType.HoverGrip;

            if (m_pGsModel != null)
              m_pGsModel.OnModified(gr, null);
            else
              m_pDevice.Invalidate();
          }
        }
        bRet = true;
      }
      return bRet;
    }

    public bool endHover()
    {
      if (0 == m_hoverGripsColl.Count)
        return false;
      foreach (ExGripData gr in m_hoverGripsColl)
      {
        if (GripData.DrawType.HoverGrip == gr.Status)
        {
          gr.Status = GripData.DrawType.WarmGrip;
          if (m_pGsModel != null)
            m_pGsModel.OnModified(gr, null);
          else
            m_pDevice.Invalidate();
        }
      }
      m_hoverGripsColl.Clear();
      return true;
    }

    public void setValue( Point3d ptValue )
    {
      foreach (ExGripDrag grDrag in m_aDrags)
      {
        grDrag.cloneEntity(ptValue);
      }
      m_ptLastPoint = ptValue;
    }

    public bool onMouseMove(int x, int y)
    {
        // Restarts hover operation.
        return startHover( x, y );
    }

    void locateGripsAtInt(int x, int y, ref ExGripDataCollection aRes, ExGripDataCollection coll, Point3d ptFirst, Tolerance tol, bool bFfindGripPoint)
    {
      foreach (ExGripData exgrData in coll)
      {
        if (bFfindGripPoint)
        {
          Point3d ptCurrent = exgrData.Point;
          if (aRes.Count == 0)
          {
            // First grip is obtained by comparing
            // grip point device position with cursor position.
            using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
            {
              Point3d ptDC = ptCurrent.TransformBy(pView.WorldToDeviceMatrix);

              double dDeltaX = Math.Abs(x - ptDC.X);
              double dDeltaY = Math.Abs(y - ptDC.Y);
              bool bOk = (dDeltaX <= m_GRIPSIZE) && (dDeltaY <= m_GRIPSIZE);
              if (bOk)
              {
                ptFirst = ptCurrent;
                aRes.Add(exgrData);
              }
            }
          }
          else
          {
            if (ptCurrent.IsEqualTo(ptFirst, tol))
            {
              aRes.Add(exgrData);
            }
          }
        }
        else
        {
          aRes.Add(exgrData);
        }
      }
    }

    public void locateGripsAt(int x, int y, ref ExGripDataCollection aRes)
    {
      locateGripsAt(x, y, ref aRes, true);
    }

    public void locateGripsAt(int x, int y, ref ExGripDataCollection aRes, bool bFindGripPoint)
    {
      Point3d ptFirst = new Point3d();
      Tolerance tol = new Tolerance(1E-4, 1E-4);
      foreach (KeyValuePair<ObjectId, ExGripDataExt> grData in m_gripDataDict)
      {
        locateGripsAtInt(x, y, ref aRes, grData.Value.DataArray, ptFirst, tol, bFindGripPoint);
        foreach (ExGripDataSubent exgrData in grData.Value.DataSub)
        {
          locateGripsAtInt(x, y, ref aRes, exgrData.SubData, ptFirst, tol, bFindGripPoint);
        }
      }
    }

    public double activeViewUnitSize()
    {
      Vector3d v;
      using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
      {
        Point2d ptDim; // getNumPixelsInUnitSquare
        {
          Extents2d ext = pView.Viewport;
          ptDim = new Point2d(Math.Abs(((double)ext.MaxPoint.X - ext.MinPoint.X) / pView.FieldWidth),
                              Math.Abs(((double)ext.MaxPoint.Y - ext.MinPoint.Y) / pView.FieldHeight));
        }
        v = new Vector3d(GRIPSIZE / ptDim.X, 0, 0);
        v = v.TransformBy(pView.ViewingMatrix);
      }      
      return v.Length / GRIPSIZE;
    }

    public Vector3d activeViewDirection()
    {
      using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
      {
        Vector3d vec = pView.Position - pView.Target;
        return vec.GetNormal();
      }  
    }

    void removeEntityGrips(Entity ent, ObjectId id,  bool bFireDone)
    {
      ExGripDataExt grDataExt;
      if (m_gripDataDict.TryGetValue(id, out grDataExt))
      {
        ent.SetGripStatus(GripStatus.GripsToBeDeleted);
        foreach (ExGripData gr in grDataExt.DataArray)
        {
          hideGrip(gr);
        }
        grDataExt.DataArray = null;
        int sz = grDataExt.DataSub.Count;
        for (int iSz = 0; iSz < sz; iSz++)
        {
          foreach (ExGripData grData in grDataExt.DataSub[iSz].SubData)
          {
            hideGrip(grData);
          }
        }
        m_gripDataDict.Remove(id);
      }
      ent.Unhighlight();

      if (bFireDone)
      {
        ent.SetGripStatus(GripStatus.GripsDone);
      }
    }

    public void removeEntityGrips( ObjectId id, bool bFireDone )
    {
      using (Entity ent = (Entity)id.GetObject(OpenMode.ForRead, false, true))
      {
        removeEntityGrips(ent, id, bFireDone);
      }
    }

    void updateEntityGrips( ObjectId id )
    {
      try
      {
        using (Entity ent = (Entity)id.GetObject(OpenMode.ForRead))
        {
          try // grip points are implemented not for all entities
          {
            removeEntityGrips(ent, id, false);
            ExGripDataCollection exGrDataColl;
            GripDataCollection grips = new GripDataCollection();
            if (ent.GetGripPoints(grips, activeViewUnitSize(), GRIPSIZE, activeViewDirection(), GetGripPointsFlags.GripPointsOnly))
            {
              exGrDataColl = new ExGripDataCollection();
              foreach (GripData gr in grips)
              {
                exGrDataColl.Add(new ExGripData(id, gr, gr.GripPoint, this));
              }
            }
            else
            {
              exGrDataColl = new ExGripDataCollection();
              Point3dCollection gripsPts = new Point3dCollection();
              ent.GetGripPoints(gripsPts, null, null);
              foreach (Point3d pt in gripsPts)
              {
                exGrDataColl.Add(new ExGripData(id, pt, this));
              }
            }

            if (null != exGrDataColl)
            {
              ExGripDataExt dExt = new ExGripDataExt();
              foreach (ExGripData grDat in exGrDataColl)
              {
                FullSubentityPath entPath;
                if (grDat.entPath(out entPath))
                {
                  bool bFound = false;
                  ExGripDataSubentCollection grSubColl = dExt.DataSub;
                  for (int j = 0; j < grSubColl.Count; j++)
                  {
                    if (grSubColl[j].EntPath == entPath)
                    {
                      bFound = true;
                      grSubColl[j].SubData.Add(grDat);
                      break;
                    }
                  }
                  if (!bFound)
                  {
                    ExGripDataSubent se = new ExGripDataSubent();
                    se.EntPath = entPath;
                    se.SubData.Add(grDat);
                    dExt.DataSub.Add(se);
                  }
                }
                else
                {
                  if (dExt.DataArray == null)
                    dExt.DataArray = new ExGripDataCollection();
                  dExt.DataArray.Add(grDat);
                }
              }

              m_gripDataDict.Add(id, dExt);

              foreach (ExGripData grData in exGrDataColl)
              {
                showGrip(grData);
              }
            }
          }
          catch (System.Exception)
          {
            // just skip non-supported entities
          }
          ent.Highlight();
        }
      }
      catch (System.Exception )
      {
        using (Entity ent = (Entity)id.GetObject(OpenMode.ForRead, false, true))//looked entity
        {
          ent.Highlight();
        }
      }

    }

    void showGrip( ExGripData pGrip)
    {
      int nViews = m_pDevice.NumViews;
      pGripTest = pGrip;
      for (int i = 0; i < nViews; i++)
        m_pDevice.ViewAt( i ).Add(pGrip);
    }

    void hideGrip(ExGripData pGrip)
    {
      int nViews = m_pDevice.NumViews;
      for (int i = 0; i < nViews; i++)
        m_pDevice.ViewAt( i ).Erase(pGrip);
    }

    public void updateSelection(ObjectIdCollection selected)
    {
      ObjectIdCollection aNew = new ObjectIdCollection();
      foreach (ObjectId id in selected)
      {
        if(!m_gripDataDict.ContainsKey(id))
        {
          updateEntityGrips(id);
        }
      }

      updateInvisibleGrips();
    }

    void locateGripsByStatus( GripData.DrawType eStatus, ref ExGripDataCollection aResult )
    {
      foreach (KeyValuePair<ObjectId, ExGripDataExt> grData in m_gripDataDict)
      {
        foreach (ExGripData exgrDat in grData.Value.DataArray)
        {
          if (exgrDat.Status == eStatus)
          {
            aResult.Add(exgrDat);
          }
        }

        foreach (ExGripDataSubent exgrData in grData.Value.DataSub)
        {
          foreach (ExGripData exgrDat in exgrData.SubData)
          {
            if (exgrDat.Status == eStatus)
            {
              aResult.Add(exgrDat);
            }
          }
        }
      }
    }

    public bool MoveAllSelected(int x, int y)
    {
      endHover();

      ExGripDataCollection aKeys = new ExGripDataCollection();
      locateGripsAt(x, y, ref aKeys, false);
      if (aKeys.Count > 0)
      {
        aActiveKeys = new ExGripDataCollection();
        locateGripsByStatus(GripData.DrawType.WarmGrip, ref aActiveKeys);
        if (aActiveKeys.Count == 0)
        {
          return false;
        }
        foreach (ExGripData exgrData in aActiveKeys)
        {
          exgrData.Status = GripData.DrawType.DragImageGrip;
        }


        foreach (KeyValuePair<ObjectId, ExGripDataExt> grData in m_gripDataDict)
        {
          bool bActive = false;
          foreach (ExGripData exgrData in grData.Value.DataArray)
          {
            bActive = true;
            m_aDrags.Add(new ExGripDrag(grData.Key, this));
          }
          foreach (ExGripDataSubent grDatSub in grData.Value.DataSub)
          {
            foreach (ExGripData exgrData in grDatSub.SubData)
            {
              bActive = true;
              m_aDrags.Add(new ExGripDrag(exgrData.SubentPath, this));
            }
            if (bActive)
              break;
          }
        }

        using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
        {
          foreach (ExGripDrag grDrag in m_aDrags)
          {
            grDrag.notifyDragStarted();
            grDrag.cloneEntity();
            pView.Add(grDrag);
          }
        }

        m_ptBasePoint = aKeys[0].Point;
        m_ptLastPoint = m_ptBasePoint;
      }
      else
      {
        return false;
      }
      return true;
    }

    public bool OnMouseDown(int x, int y)
    {
      endHover();

      ExGripDataCollection aKeys = new ExGripDataCollection();
      locateGripsAt(x, y, ref aKeys);
      if (aKeys.Count > 0)
      {
        //if ( bShift )  TODO
        {
          bool bMakeHot = true;
          foreach (KeyValuePair<ObjectId, ExGripDataExt> grData in m_gripDataDict)
          {
            foreach (ExGripData exgrData in grData.Value.DataArray)
            {
              if (GripData.DrawType.HotGrip == exgrData.Status)
              {
                bMakeHot = false;
                break;
              }
            }

            foreach (ExGripDataSubent grDatSub in grData.Value.DataSub)
            {
              foreach (ExGripData exgrData in grDatSub.SubData)
              {
                if (GripData.DrawType.HotGrip == exgrData.Status)
                {
                  bMakeHot = false;
                  break;
                }
              }
              if (bMakeHot == false)
                break;
            }

            if (bMakeHot == false)
              break;
          }
          if (bMakeHot)
          {
            foreach (ExGripData exgrData in aKeys)
            {
              exgrData.Status = GripData.DrawType.HotGrip;
            }
          }

          aActiveKeys = new ExGripDataCollection();
          locateGripsByStatus(GripData.DrawType.HotGrip, ref aActiveKeys);
          if (aActiveKeys.Count == 0)
          {
            // Valid situation.
            // If trigger grip performed entity modification and returned eGripHotToWarm
            // then nothing is to be done cause entity modification will cause reactor to regen grips.
            return false;
          }

          foreach (ExGripData exgrData in aActiveKeys)
          {
            exgrData.Status = GripData.DrawType.DragImageGrip;
          }

          foreach (KeyValuePair<ObjectId, ExGripDataExt> grData in m_gripDataDict)
          {
            bool bActive = false;
            foreach (ExGripData exgrData in grData.Value.DataArray)
            {
              if (GripData.DrawType.DragImageGrip == exgrData.Status)
              {
                bActive = true;
                m_aDrags.Add(new ExGripDrag(grData.Key, this));
              }
            }
            foreach (ExGripDataSubent grDatSub in grData.Value.DataSub)
            {
              foreach (ExGripData exgrData in grDatSub.SubData)
              {
                if (GripData.DrawType.DragImageGrip == exgrData.Status)
                {
                  bActive = true;
                  m_aDrags.Add(new ExGripDrag(exgrData.SubentPath, this));
                }
              }
              if (bActive == true)
              {
                break;
              }
            }
          }

          using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
          {
            foreach (ExGripDrag grDrag in m_aDrags)
            {
              grDrag.notifyDragStarted();
              grDrag.cloneEntity();
              pView.Add(grDrag);
            }
          }  

          m_ptBasePoint = aKeys[0].Point;
          m_ptLastPoint = m_ptBasePoint;
        }
      }
      else
      {
        return false;
      }
      return true;
    }

    public Point3d BasePoint
    {
      get { return m_ptBasePoint; }
    }

    public Point3d LastPoint
    {
      get { return m_ptLastPoint; }
    }

    public int GRIPSIZE
    {
      get { return m_GRIPSIZE; }
    }

    public int GRIPOBJLIMIT
    {
      get { return m_GRIPOBJLIMIT; }
    }

    public EntityColor GRIPCOLOR
    {
      get { return m_GRIPCOLOR; }
    }

    public EntityColor GRIPHOVER
    {
      get { return m_GRIPHOVER; }
    }

    public EntityColor GRIPHOT
    {
      get { return m_GRIPHOT; }
    }

    public Model Model
    {
      get { return m_pGsModel; }
    }

    public Device Device
    {
      get { return m_pDevice; }
    }

    public Dictionary<ObjectId, ExGripDataExt> GripDataDict
    {
      get { return m_gripDataDict; }
    }
    
    void updateInvisibleGrips()
    {
      ExGripDataCollection aOverall = new ExGripDataCollection();
      foreach (KeyValuePair<ObjectId, ExGripDataExt> grDataDc in m_gripDataDict)
      {        
        foreach (ExGripData grData in grDataDc.Value.DataArray)
        {
          aOverall.Add(grData);
        }
        foreach (ExGripDataSubent grDataSub in grDataDc.Value.DataSub)
        {
          foreach (ExGripData grData in grDataSub.SubData)
          {
            aOverall.Add(grData);
          }
        }
      }

      int iSize = aOverall.Count;
      for (int i = 0; i < iSize; i++)
      {
        ExGripData grData = aOverall[i];
        grData.Invisible = false;
        grData.Shared = false;

        IntegerCollection aEq = new IntegerCollection();
        aEq.Add( i );

        Point3d ptIni = grData.Point;

        int iNext = i + 1;
        Tolerance tc = new Tolerance(1E-6, 1E-6);
        while (iNext < iSize)
        {
          Point3d ptCur = aOverall[ iNext ].Point;
          if (ptIni.IsEqualTo(ptCur, tc))
          {
            aEq.Add( iNext );
            iNext++;
          }
          else
            break;
        }

        if (aEq.Count >= 2)
        {
          int iVisible = 0;
          int jSize = aEq.Count;
          for (int j = 0; j < jSize; j++)
          {
            ExGripData pGrip = aOverall[aEq[j]];

            bool bOk = true;
            if (pGrip.Data != null)
            {
              if (pGrip.Data.SkipWhenShared)
                bOk = false;
            }

            if (bOk)
            {
              iVisible = j;
              break;
            }
          }

          for (int j = 0; j < jSize; j++)
          {
            ExGripData pGrip = aOverall[aEq[j]];
            pGrip.Shared = true;
            pGrip.Invisible = (j != iVisible);
          }
        }
      }
    }



    public void DragFinal(Point3d ptFinal, bool bOk)
    {
      using (Teigha.GraphicsSystem.View pView = m_pDevice.ActiveView)
      {
        foreach (ExGripDrag grDrag in m_aDrags)
        {
          if (bOk)
          {
            m_pDb.StartUndoRecord();
            grDrag.moveEntity(ptFinal);
            grDrag.notifyDragEnded();
          }
          else
            grDrag.notifyDragAborted();
          pView.Erase(grDrag);
          updateEntityGrips(grDrag.entityId());
          grDrag.uninit();
        }
      }   

      m_aDrags.Clear();

      if (bOk)
        updateInvisibleGrips();

      if (aActiveKeys != null)
      {
        foreach (ExGripData exgrData in aActiveKeys)
        {
          exgrData.Status = GripData.DrawType.WarmGrip;
        }
      }
      aActiveKeys = null;
    }
  }
}
