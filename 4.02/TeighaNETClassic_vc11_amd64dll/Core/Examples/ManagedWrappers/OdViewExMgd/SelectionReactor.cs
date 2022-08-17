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
using System.Text;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;

namespace OdViewExMgd
{
  public class DrawableImp : Drawable
  {
    public DrawableImp()
    {
    }

    protected override void SubViewportDraw(ViewportDraw vd)
    {
    }

    protected override int SubSetAttributes(DrawableTraits traits)
    {
      return 0;
    }

    protected override int SubViewportDrawLogicalFlags(ViewportDraw vd)
    {
      return 0;
    }

    protected override bool SubWorldDraw(WorldDraw wd)
    {
      return false;
    }

    public override bool IsPersistent
    {
      get
      {
        return false;
      }
    }

    public override ObjectId Id
    {
      get
      {
        return new ObjectId();
      }
    }
  }


  class RectFram : DrawableImp
  {
    Model mod;
    Point3dCollection m_pts;
    public RectFram()
    {
      m_pts = new Point3dCollection();
      m_pts.Add(new Point3d());
      m_pts.Add(new Point3d());
      m_pts.Add(new Point3d());
      m_pts.Add(new Point3d());
    }

    public RectFram(Point3d pt)
    {
      m_pts = new Point3dCollection();
      m_pts.Add(pt);
      m_pts.Add(pt);
      m_pts.Add(pt);
      m_pts.Add(pt);
    }

    protected override void SubViewportDraw(ViewportDraw vd)
    {
      using (Teigha.GraphicsInterface.Viewport vp = vd.Viewport)
      {
        using (ViewportGeometry vpg = vd.Geometry)
        {
          vd.SubEntityTraits.FillType = FillType.FillNever;
          Matrix3d mat = vp.WorldToEyeTransform;
          Point3d p0 = mat * m_pts[0];
          Point3d p2 = mat * m_pts[2];
          m_pts[1] = new Point3d(p0.X, p2.Y, p2.Z);
          m_pts[3] = new Point3d(p2.X, p0.Y, p2.Z);

          mat = vp.EyeToWorldTransform;
          m_pts[1] = m_pts[1].TransformBy(mat);
          m_pts[3] = m_pts[3].TransformBy(mat);
          vpg.Polygon(m_pts);
        }
      }
    }

    public void setValue(Point3d pt)
    {
      m_pts[2] = pt;
      if (mod != null)
        mod.OnModified(this, null);
    }

    public Point3d BasePoint
    {
      get 
      {
        return m_pts[0];
      }
    }

    public void Add(Teigha.GraphicsSystem.View pView)
    {
      if (mod == null)
      {
        mod = pView.Device.CreateModel();
        mod.RenderType = RenderType.Direct;
      }
      pView.Add(this, mod);
    }
  }

  class SR : SelectionReactor
  {
    ObjectIdCollection selected;
    ObjectId spaceId;
    public SR(ObjectIdCollection s, ObjectId id)
    {
      spaceId = id;
      selected = s;
    }
    public override bool Selected(DrawableDesc pDrawableDesc)
    {
      DrawableDesc pDesc = pDrawableDesc;
      if (pDesc.Parent != null)
      {
        // we walk up the GS node path to the root container primitive
        // to avoid e.g. selection of individual lines in a dimension 
        while (((DrawableDesc)pDesc.Parent).Parent != null)
          pDesc = (DrawableDesc)pDesc.Parent;
        if (pDesc.PersistId != IntPtr.Zero && ((DrawableDesc)pDesc.Parent).PersistId == spaceId.OldIdPtr)
        {
          pDesc.MarkedToSkip = true; // regen abort for selected drawable, to avoid duplicates
          ObjectId id = new ObjectId(pDesc.PersistId);
          if (!selected.Contains(id))
          {
            selected.Add(id);
          }
        }
        return true;
      }
      return false;
    }
    // this more informative callback may be used to implement subentities selection
    public override SelectionReactorResult Selected(PathNode pthNode, Teigha.GraphicsInterface.Viewport viewInfo)
    {
      return SelectionReactorResult.NotImplemented;
    }
  };
}
