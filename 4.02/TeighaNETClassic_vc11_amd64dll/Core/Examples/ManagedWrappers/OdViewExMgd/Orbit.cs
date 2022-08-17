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
using Teigha.Colors;

namespace OdViewExMgd
{
  class OrbitCtrl : DrawableImp
  {
    Model mod;
    public OrbitCtrl()
    {
    }

    protected override int SubSetAttributes(DrawableTraits traits)
    {
      return AttributesFlags.DrawableIsAnEntity.GetHashCode() | AttributesFlags.DrawableRegenDraw.GetHashCode();
    }

    protected override void SubViewportDraw(ViewportDraw vd)
    {
      using (Teigha.GraphicsInterface.Viewport vp = vd.Viewport)
      {
        using (ViewportGeometry vpg = vd.Geometry)
        {
          vd.SubEntityTraits.FillType = FillType.FillNever;
          vd.SubEntityTraits.Color = Color.FromRgb(0, 255, 0).ColorIndex;
          Extents2d ext2d = vp.DeviceContextViewportCorners;
          Point3d lowerLeft = new Point3d(ext2d.MinPoint.X, ext2d.MinPoint.Y, 0);
          vpg.PushModelTransform(vp.EyeToModelTransform);

          Vector3d vecTmp = new Vector3d(ext2d.MaxPoint.X - lowerLeft.X, ext2d.MaxPoint.Y - lowerLeft.Y, 0);
          double r = Math.Max(vecTmp.X, vecTmp.Y) / 4;

          lowerLeft = lowerLeft + (vecTmp / 2);
          vpg.Circle(lowerLeft, r, Vector3d.ZAxis);

          vpg.Circle(lowerLeft + new Vector3d(0,  r, 0), r / 20, Vector3d.ZAxis);
          vpg.Circle(lowerLeft + new Vector3d(0, -r, 0), r / 20, Vector3d.ZAxis);
          vpg.Circle(lowerLeft + new Vector3d(r,  0, 0), r / 20, Vector3d.ZAxis);
          vpg.Circle(lowerLeft + new Vector3d(-r, 0, 0), r / 20, Vector3d.ZAxis);
          vpg.PopModelTransform();
        }
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

  class OrbitTracker
  {
    enum Axis
    {
      Horizontal,
      Vertical,
      PerpDir, // orbit around perpendicular to mouse direction
      Eye,
    }

    Axis m_axis;
    View m_pView;
    Point3d m_pt;
    Point3d m_pos;
    Point3d m_trg;
    Vector3d m_up;
    Vector3d m_x;
    double m_D; // diameter of orbit control in projected coordinates
    Point3d lastPt;
    Point3d m_viewCenter;


    void viewportDcCorners(out Point2d lower_left, out Point2d upper_right)
    {
      Point3d target = m_pView.ViewingMatrix * m_pView.Target;
      double halfFieldWidth = m_pView.FieldWidth / 2.0;
      double halfFieldHeight = m_pView.FieldHeight / 2.0;
      lower_left = new Point2d(target.X - halfFieldWidth, target.Y - halfFieldHeight);
      upper_right = new Point2d(target.X + halfFieldWidth, target.Y + halfFieldHeight);
    }

    public void init(View pView, Point3d pt)
    {
      m_pView = pView;
      m_pos = m_pView.Position;
      m_trg = m_pView.Target;
      m_up = m_pView.UpVector;
      m_x = m_up.CrossProduct(m_trg - m_pos).GetNormal();

      Matrix3d m_initViewingMatrixInv = m_pView.ViewingMatrix;
      lastPt = m_initViewingMatrixInv * pt;
      m_pt = m_initViewingMatrixInv * pt;
      m_pt = new Point3d(m_pt.X, m_pt.Y, 0);
      m_initViewingMatrixInv = m_initViewingMatrixInv.Inverse();

      Point2d lowerLeft2d, upper_right;
      viewportDcCorners(out lowerLeft2d, out upper_right);

      Point3d lowerLeft = new Point3d(lowerLeft2d.X, lowerLeft2d.Y, 0);
      Vector3d vecTmp = new Vector3d(upper_right.X - lowerLeft2d.X, upper_right.Y - lowerLeft2d.Y, 0);
      double r = Math.Max(vecTmp.X, vecTmp.Y) / 4;
      lowerLeft = lowerLeft + (vecTmp / 2);
      m_D = 2.0 * r;
      double r2sqrd = r * r / 400;


      vecTmp = new Vector3d(lowerLeft.X - m_pt.X, lowerLeft.Y - m_pt.Y + r, lowerLeft.Z - m_pt.Z);
      if (vecTmp.LengthSqrd <= r2sqrd)
      {
        m_axis = Axis.Horizontal;
      }
      else
      {
        vecTmp = new Vector3d(lowerLeft.X - m_pt.X, lowerLeft.Y - m_pt.Y - r, lowerLeft.Z - m_pt.Z);
        if (vecTmp.LengthSqrd <= r2sqrd)
        {
          m_axis = Axis.Horizontal;
        }
        else
        {
          vecTmp = new Vector3d(lowerLeft.X - m_pt.X + r, lowerLeft.Y - m_pt.Y, lowerLeft.Z - m_pt.Z);
          if (vecTmp.LengthSqrd <= r2sqrd)
          {
            m_axis = Axis.Vertical;
          }
          else
          {
            vecTmp = new Vector3d(lowerLeft.X - m_pt.X - r, lowerLeft.Y - m_pt.Y, lowerLeft.Z - m_pt.Z);
            if (vecTmp.LengthSqrd <= r2sqrd)
            {
              m_axis = Axis.Vertical;
            }
            else
            {
              vecTmp = new Vector3d(lowerLeft.X - m_pt.X, lowerLeft.Y - m_pt.Y, lowerLeft.Z - m_pt.Z);
              if (vecTmp.LengthSqrd <= r * r)
              {
                m_axis = Axis.PerpDir;
              }
              else
              {
                m_axis = Axis.Eye;
              }
            }
          }
        }
      }

      using (AbstractViewPE pVpPE = new AbstractViewPE(pView))
      {
        BoundBlock3d extents = new BoundBlock3d();
        bool bBboxValid = pVpPE.GetViewExtents(extents);
        Point3d maxPt = extents.GetMaximumPoint();
        Point3d minPt = extents.GetMinimumPoint();
        Point3d cnt = (maxPt + minPt.GetAsVector())/2;
        m_viewCenter = new Point3d(cnt.X, cnt.Y, cnt.Z);
        m_viewCenter = m_viewCenter.TransformBy(m_initViewingMatrixInv);
      }
    }

    public void reset()
    {
      m_pView = null;
    }
    
    void angle(Point3d value, out double xOrbit,  out double yOrbit)
    {
      Point3d pt1 = m_pView.ViewingMatrix * value;
      xOrbit = lastPt.Y - pt1.Y;
      yOrbit = pt1.X - lastPt.X;
      lastPt = pt1;
      xOrbit = xOrbit * Math.PI / m_D;
      yOrbit = yOrbit * Math.PI / m_D;
    }


    double angleZ(Point3d value)
    {
      Point3d pt1 = m_pView.ViewingMatrix * value;
      Point3d targ = m_pView.ViewingMatrix * m_viewCenter;
      pt1 = new Point3d(pt1.X, pt1.Y, 0);
      targ = new Point3d(targ.X, targ.Y, 0);
      Vector3d vec1 = pt1 - targ;
      Vector3d vec2 = m_pt - targ;
      return vec2.GetAngleTo(vec1, Vector3d.ZAxis);
    }

    public void setValue(Point3d value)
    {
      if (null != m_pView)
      {
        if (Axis.Eye == m_axis)
        {
          Matrix3d x = Matrix3d.Rotation(angleZ(value), m_trg - m_pos, m_viewCenter);
          Point3d newPos = x * m_pos, newTarget = x * m_trg;
          Vector3d newPosDir = newPos - newTarget;
          newPosDir = newPosDir.GetNormal();
          newPosDir *= m_pos.DistanceTo(m_trg);
          newPos = newTarget + newPosDir;
          m_pView.SetView( newPos,
                           newTarget,
                           x * m_up,
                           m_pView.FieldWidth,
                           m_pView.FieldHeight,
                           m_pView.IsPerspective ? Teigha.GraphicsSystem.Projection.Perspective : Teigha.GraphicsSystem.Projection.Parallel
                          );
        }
        else
        {
          double xOrbit, yOrbit;
          angle(value, out xOrbit, out yOrbit);
          switch (m_axis)
          {
            case Axis.Horizontal:
              m_pView.Orbit(xOrbit, 0);
              break;
            case Axis.Vertical:
              m_pView.Orbit(0, yOrbit);
              break;
            case Axis.PerpDir:
              m_pView.Orbit(xOrbit, yOrbit);
              break;
          }
        }
      }
    }
  }
}
