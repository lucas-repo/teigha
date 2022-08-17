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


namespace OdViewExMgd
{
  public class LineOverrule : GripOverrule
  {
    static public LineOverrule overrule = new LineOverrule();

    public override void MoveStretchPointsAt(Entity entity, IntegerCollection indices, Vector3d offset) 
    {
      int size = indices.Count;
      if (size > 0)
      {
        Line ln = (Line)entity;

        if (size > 1 || indices[0] == 2)
        {
          ln.StartPoint += offset/2;
          ln.EndPoint += offset/2;
        }
        else if (indices[0] == 0)
        {
          ln.StartPoint += offset;
        }
        else if (indices[0] == 1)
        {
          ln.EndPoint += offset;
        }
      }
    }

    public override void GetGripPoints(Entity entity, Point3dCollection gripPoints, IntegerCollection snapModes, IntegerCollection geometryIds)
    {
      int size = gripPoints.Count;
      Line ln = (Line)entity;
      gripPoints.Add(ln.StartPoint);
      gripPoints.Add(ln.EndPoint);
      gripPoints.Add(gripPoints[size] + (gripPoints[size + 1] - gripPoints[size])/2);
    }
  }
}