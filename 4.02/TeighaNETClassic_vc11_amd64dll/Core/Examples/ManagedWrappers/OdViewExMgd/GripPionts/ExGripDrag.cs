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

namespace OdViewExMgd
{
  public class ExGripDrag : DrawableImp
  {
    FullSubentityPath subentPath;
    Entity m_pCloneEnt;
    ExGripManager m_pOwner;


    public ExGripDrag()
    {
      m_pOwner = null;
    }

    public void uninit()
    {
      if (m_pCloneEnt != null)
      {
        m_pCloneEnt.Dispose();
        m_pCloneEnt = null;
      }     
      m_pOwner   = null;
    }

    public ExGripDrag(ObjectId id, ExGripManager pOwner)
    {
      ObjectId[] arrId = new ObjectId[1];
      arrId[0] = id;
      subentPath = new FullSubentityPath(arrId, SubentityId.Null);
      m_pOwner = pOwner;
    }

    public ExGripDrag(FullSubentityPath entPath, ExGripManager pOwner)
    {
      subentPath = entPath;
      m_pOwner = pOwner;
    }

    protected override int SubSetAttributes(DrawableTraits traits)
    {
      if (null == m_pCloneEnt)
      {
        return AttributesFlags.DrawableIsInvisible.GetHashCode();
      }
      else
      {
        int iRet = m_pCloneEnt.SetAttributes(traits);
        if (traits is SubEntityTraits)
        {
          SubEntityTraits pEntityTraits = (SubEntityTraits)traits;
          pEntityTraits.FillType = FillType.FillNever;
          return iRet;
        }
      }
      return AttributesFlags.DrawableNone.GetHashCode();
    }

    protected override void SubViewportDraw(ViewportDraw vd)
    {
      if (null != m_pCloneEnt)
      {
        m_pCloneEnt.ViewportDraw(vd);
      }
    }

    protected override bool SubWorldDraw(WorldDraw pWorldDraw)
    {
      if (null != m_pCloneEnt)
      {
        return m_pCloneEnt.WorldDraw(pWorldDraw);
      }
      return true;
    }

    public ObjectId entityId()
    { 
      ObjectId[] Ids = subentPath.GetObjectIds();
      return Ids[Ids.Length-1];
    }

    public void cloneEntity()
    {
      if (m_pCloneEnt != null)
      {
        m_pCloneEnt.Dispose();
        m_pCloneEnt = null;
      }

      using(DBObject dbObj = entityId().GetObject(OpenMode.ForRead))
      {
        if (dbObj is Entity && null != m_pOwner)
        {
          Entity ent = (Entity)dbObj;
          if (ent.CloneMeForDragging)
          {
            m_pCloneEnt = (Entity)ent.Clone();
          }

          if (null != m_pCloneEnt)
          {
            m_pCloneEnt.DisableUndoRecording(true);
            m_pCloneEnt.SetPropertiesFrom(ent);
          }
        }
      }
    }

    bool locateActiveGrips(out IntegerCollection aIndices )
    {
      ExGripDataCollection grDataCol = entPath() ? m_pOwner.GetSubentGripData(m_pOwner.GripDataDict[entityId()], subentPath).SubData :
                                                   m_pOwner.GripDataDict[entityId()].DataArray;

      bool bExMethod = true;
      aIndices = new IntegerCollection();
      int i = 0;
      foreach (ExGripData dat in grDataCol)
      {
        if (null == dat.Data)
          bExMethod = false;

        if (GripData.DrawType.DragImageGrip == dat.Status)
          aIndices.Add(i);
        i++;
      }
      return bExMethod;
    }

    bool entPath()
    {
      return (subentPath.SubentId.IndexPtr != (IntPtr)0 && subentPath.SubentId.Type != SubentityType.Null);
    }

    bool entPath(out FullSubentityPath pPath)
    {
      pPath = subentPath;
      return entPath();
    }

    public void cloneEntity(Point3d ptMoveAt)
    {
      cloneEntity();
      if (null == m_pCloneEnt)
        return;

      IntegerCollection aIndices;
      bool bExMethod = locateActiveGrips(out aIndices);
      Vector3d vOffset = ptMoveAt - m_pOwner.BasePoint;
      if (bExMethod)
      {
        using (GripDataCollection cloneDataColl = new GripDataCollection())
        {
          if (entPath())
          {
            m_pCloneEnt.GetGripPointsAtSubentityPath(subentPath, cloneDataColl, m_pOwner.activeViewUnitSize(), m_pOwner.GRIPSIZE, m_pOwner.activeViewDirection(), GetGripPointsFlags.GripPointsOnly);
          }
          else
          {
            m_pCloneEnt.GetGripPoints(cloneDataColl, m_pOwner.activeViewUnitSize(), m_pOwner.GRIPSIZE, m_pOwner.activeViewDirection(), GetGripPointsFlags.GripPointsOnly);
          }

          IntPtr[] aIds = new IntPtr[aIndices.Count];
          int index = 0;

          foreach (int i in aIndices)
          {
            if (i < cloneDataColl.Count)
            {
              aIds[index++] = cloneDataColl[i].AppData;
            }
          }
          if (entPath())
          {
            FullSubentityPath[] aPaths = new FullSubentityPath[1];
            aPaths[0] = subentPath;
            m_pCloneEnt.MoveGripPointsAtSubentityPaths(aPaths, aIds, vOffset, 0);
            m_pCloneEnt.SetSubentityGripStatus(GripStatus.GripsToBeDeleted, subentPath);
          }
          else
          {
            m_pCloneEnt.MoveGripPointsAt(aIds, vOffset, MoveGripPointsFlags.Osnapped);
            m_pCloneEnt.SetGripStatus(GripStatus.GripsToBeDeleted);
          }
        }
      }
      else
      {
        m_pCloneEnt.MoveGripPointsAt(aIndices, vOffset);
        m_pCloneEnt.SetGripStatus(GripStatus.GripsToBeDeleted);
      }
      if (m_pOwner.Model != null)
      {
        m_pOwner.Model.OnModified(this, null);
      }
      else
        m_pOwner.Device.Invalidate();
    }


    public void moveEntity(Point3d ptMoveAt)
    {
      IntegerCollection aIndices;
      bool bExMethod = locateActiveGrips(out aIndices);
      Vector3d vOffset = ptMoveAt - m_pOwner.BasePoint;

      using (DBObject dbObj = entityId().GetObject(OpenMode.ForWrite))
      {
        if (dbObj is Entity)
        {
          Entity ent = (Entity)dbObj;
          ExGripDataCollection grDataCol = entPath() ? m_pOwner.GetSubentGripData(m_pOwner.GripDataDict[entityId()], subentPath).SubData :
                                                       m_pOwner.GripDataDict[entityId()].DataArray;

          if (bExMethod)
          {
            IntPtr[] aIds = new IntPtr[aIndices.Count];
            int index = 0;
            foreach (int i in aIndices)
            {
              if (i < grDataCol.Count)
              {
                aIds[index++] = grDataCol[i].Data.AppData;
              }
            }

            if (entPath())
            {
              FullSubentityPath[] aPaths = new FullSubentityPath[1];
              aPaths[0] = subentPath;
              ent.MoveGripPointsAtSubentityPaths(aPaths, aIds, vOffset, 0);
            }
            else
            {
              ent.MoveGripPointsAt(aIds, vOffset, MoveGripPointsFlags.Osnapped);
            }
          }
          else
          {
            ent.MoveGripPointsAt(aIndices, vOffset);
          }
        }
      }
    }


    public void notifyDragStarted()
    {
      using(DBObject dbObj = entityId().GetObject(OpenMode.ForRead))
      {
        if (dbObj is Entity)
        {
          Entity ent = (Entity)dbObj;
          ent.SetDragStatus(DragStatus.DragStart);
        }
      }
    }

    public void notifyDragEnded()
    {
      using (DBObject dbObj = entityId().GetObject(OpenMode.ForRead))
      {
        if (dbObj is Entity)
        {
          Entity ent = (Entity)dbObj;
          ent.SetDragStatus(DragStatus.DragEnd);
        }
      }
    }

    public void notifyDragAborted()
    {
      using (DBObject dbObj = entityId().GetObject(OpenMode.ForRead))
      {
        if (dbObj is Entity)
        {
          Entity ent = (Entity)dbObj;
          ent.SetDragStatus(DragStatus.DragAbort);
        }
      }
    }
  };

  public class ExGripDragCollection : CollectionBase
  {
    //strongly typed accessor
    public ExGripDrag this[int index]
    {
      get
      {
        return (ExGripDrag)this.List[index];
      }
      set
      {
        this.List[index] = value;
      }
    }

    public void Add(ExGripDrag grData)
    {
      this.List.Add(grData);
    }
  }
}


