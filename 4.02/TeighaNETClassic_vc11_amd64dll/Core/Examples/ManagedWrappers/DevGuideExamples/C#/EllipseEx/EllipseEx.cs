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
using System.Linq;
using System.Text;

using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Colors;

namespace CDevGuideExamplesProject
{
  public class EllipseEx
  {
    public EllipseEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            Ellipse el1 = new Ellipse(Point3d.Origin, new Vector3d(0, 0, 1), new Vector3d(2, 0, 0), 0.5, 0.0, 2 * Math.PI);
            btr.AppendEntity(el1);
            el1.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);
                        
            Ellipse el2 = new Ellipse(Point3d.Origin, new Vector3d(1, 0, 0), new Vector3d(0, 0, 2), 0.5, 0.0, 2 * Math.PI);
            btr.AppendEntity(el2);
            el2.Color = Color.FromColorIndex(ColorMethod.ByAci, 2);

            Ellipse el3 = new Ellipse();
            btr.AppendEntity(el3);
            el3.Set(Point3d.Origin, new Vector3d(1, 0, 1), new Vector3d(-2 * Math.Cos(Math.PI / 4), 0, 2 * Math.Sin(Math.PI / 4)), 0.5, 0.0, 2 * Math.PI);
            el3.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);

            Ellipse el4 = new Ellipse();
            btr.AppendEntity(el4);
            el4.Set(Point3d.Origin, new Vector3d(1, 0, -1), new Vector3d(2 * Math.Cos(Math.PI / 4), 0, 2 * Math.Sin(Math.PI / 4)), 0.5, 0.0, 2 * Math.PI);
            el4.Color = Color.FromColorIndex(ColorMethod.ByAci, 4);

            Ellipse elArc1 = new Ellipse();
            btr.AppendEntity(elArc1);
            elArc1.Set(new Point3d(5, 0, 0), new Vector3d(0, 0, 1), new Vector3d(1, 0, 0), 0.7, Math.PI / 4, 3 * Math.PI / 4);
            elArc1.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);

            Ellipse elArc2 = new Ellipse();
            btr.AppendEntity(elArc2);
            elArc2.Set(new Point3d(5, 0, 0), new Vector3d(0, 0, 1), new Vector3d(1, 0, 0), 0.7, -5*Math.PI / 4, Math.PI / 4);
            elArc2.Color = Color.FromRgb(20, 20, 255);
           }
          ta.Commit();
        }
        db.SaveAs(path + "EllipseEx.dwg", DwgVersion.Current);
      }
    }
  }
}
