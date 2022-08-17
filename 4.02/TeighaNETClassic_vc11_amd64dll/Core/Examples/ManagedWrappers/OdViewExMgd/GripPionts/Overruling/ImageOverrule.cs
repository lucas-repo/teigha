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
using Teigha;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using System.Windows.Forms;

namespace OdViewExMgd
{
  public class RasterImageOverrule : GripOverrule
  {
    static public RasterImageOverrule overrule = new RasterImageOverrule();

    public override void GetGripPoints(Entity entity, Point3dCollection gripPoints, IntegerCollection snapModes, IntegerCollection geometryIds)
    {
      int size = gripPoints.Count;
      RasterImage img = (RasterImage)entity;
      Extents3d? ext = img.Bounds;
      
      if (ext != null)
      {
        //Fill gripPoints array by points
        Extents3d exts = ext.Value;
        Point3d centerPt = exts.MinPoint + (exts.MaxPoint - exts.MinPoint) / 2;
        gripPoints.Add(centerPt);
        gripPoints.Add(exts.MinPoint);
        gripPoints.Add(new Point3d(exts.MinPoint.X, exts.MaxPoint.Y, exts.MinPoint.Z));
        gripPoints.Add(exts.MaxPoint);
        gripPoints.Add(new Point3d(exts.MaxPoint.X, exts.MinPoint.Y, exts.MinPoint.Z));
        //middle of edges
        gripPoints.Add(new Point3d(centerPt.X, exts.MaxPoint.Y, exts.MinPoint.Z));
        gripPoints.Add(new Point3d(centerPt.X, exts.MinPoint.Y, exts.MinPoint.Z));
        gripPoints.Add(new Point3d(exts.MinPoint.X, centerPt.Y, exts.MinPoint.Z));
        gripPoints.Add(new Point3d(exts.MaxPoint.X, centerPt.Y, exts.MinPoint.Z));
      }
    }

    class Coeff
    {
      public int m_iUOrg;
      public int m_iVOrg;
      public int m_iUCf;
      public int m_iVCf;

      public Coeff(int iUOrg, int iVOrg, int iUCf, int iVCf)
      {
        m_iUOrg = iUOrg;
        m_iVOrg = iVOrg;
        m_iUCf = iUCf;
        m_iVCf = iVCf;
      }
    };

    public override void MoveGripPointsAt(Entity entity, IntegerCollection indices, Vector3d offset)
    {
      //Verify what there is a point for processing
      int size = indices.Count;
      if ( size == 0 )
        return;

      RasterImage img = (RasterImage)entity;

      //Modify entity by the point.
      int iIndex = indices[0];
      if(iIndex == 0)
      {
        img.TransformBy(Matrix3d.Displacement(offset));
      }
      else
      {
        CoordinateSystem3d cs = img.Orientation;
        Vector3d u = cs.Xaxis;
        Vector3d v = cs.Yaxis;

        double dX = offset.X;
        double dY = offset.Y;

        if (iIndex > 0 && iIndex < 5)
        {
          //Default bottom - left corner
          Coeff cfs = new Coeff(1, 1, 1, 1);

          double dKoeffXY = u.X / v.Y;

          switch (iIndex)
          {
            case 2:
              //top - left corner
              cfs = new Coeff(1, 0, 1, -1);
              break;
            case 3:
              //top - right corner
              cfs = new Coeff(0, 0, -1, -1);
              break;
            case 4:
              //bottom - right corner
              cfs = new Coeff(0, 1, -1, 1);
              break;
          }

          double dYMod = cfs.m_iVCf * dY * dKoeffXY;
          dX = cfs.m_iUCf * dX;
          if (dX > 0 && dYMod > 0)
          {
            if (dX > u.X)
              dX = 2 * u.X - dX;
            if (cfs.m_iVCf * dY > v.Y)
            {
              dY = 2 * v.Y - cfs.m_iVCf * dY;
            }
          }
          double dMov = dX < dYMod ? dX : dYMod;

          double dLenU = u.Length;
          Vector3d vecU = u.GetNormal();
          vecU = vecU * dMov;
          double dLenV = v.Length;
          Vector3d vecV = v.GetNormal();
          vecV = (vecV * dMov * dLenV) / dLenU;

          Point3d pOrg = cs.Origin + cfs.m_iUOrg * vecU + cfs.m_iVOrg * vecV;
          img.Orientation = new CoordinateSystem3d(pOrg, u - vecU, v - vecV);
        }
        else
        {
          Vector3d vMod = new Vector3d();
          Coeff cfs = new Coeff(0, 0, 0, 0);

          switch (iIndex)
          {
            case 5:
              //top - middle
              if (dY > 0 || dY > -v.Y)
                vMod = new Vector3d(0, dY, 0);
              cfs.m_iVCf = 1;
              break;
            case 6:
              //bottom - middle
              if (dY < 0 || dY < v.Y)
                vMod = new Vector3d(0, dY, 0);
              cfs.m_iUOrg = 1;
              cfs.m_iVCf = -1;
              break;
            case 7:
              //left - middle
              if (dX < 0 || dX < u.X)
                vMod = new Vector3d(dX, 0, 0);
              cfs.m_iUOrg = 1;
              cfs.m_iUCf  = -1;
              break;
            case 8:
              //right - middle
              if (dX > 0 || dX > -u.X)
                vMod = new Vector3d(dX, 0, 0);
              cfs.m_iUCf = 1;
              break;
            default:
              return;
          }
          Point3d pOrg = cs.Origin + cfs.m_iUOrg * vMod;
          img.Orientation = new CoordinateSystem3d(pOrg, u + cfs.m_iUCf * vMod, v + cfs.m_iVCf * vMod);
        }
      }
    }
  }
}