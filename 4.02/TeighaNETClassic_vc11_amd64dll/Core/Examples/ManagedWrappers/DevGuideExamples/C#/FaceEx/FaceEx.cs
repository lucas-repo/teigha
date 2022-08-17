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

namespace CDevGuideExamplesProject.FaceEx
{
  public class FaceEx
  {
    public FaceEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            Face face1 = new Face(new Point3d(0, 0, -1), new Point3d(0, 2, 0), new Point3d(0, 0, 1), new Point3d(2 * Math.Cos(Math.PI / 10), 2 * Math.Sin(Math.PI / 10), 0), true, true, true, true);
            btr.AppendEntity(face1);
            face1.Color = Color.FromRgb(250, 0, 0);
            Console.WriteLine("face1:");
            printVertices(face1);
            

            Face face2 = new Face(new Point3d(0, 0, -1), new Point3d(-2 * Math.Cos(Math.PI / 10), 2 * Math.Sin(Math.PI / 10), 0), new Point3d(0, 0, 1), true, true, false, false);
            btr.AppendEntity(face2);
            face2.Color = Color.FromRgb(0, 250, 0);
            Console.WriteLine("face2:");
            printVertices(face2);

            Face face3 = new Face();
            btr.AppendEntity(face3);
            face3.SetVertexAt(0, new Point3d(2 * Math.Cos(3 * Math.PI / 10), -2 * Math.Sin(3 * Math.PI / 10), 0));
            face3.SetVertexAt(1, new Point3d(0,0,1));
            face3.SetVertexAt(2, new Point3d(0, 0, -1));
            face3.SetVertexAt(3, new Point3d(0, 0, -1));
            face3.Color = Color.FromRgb(0, 0, 250);
            Console.WriteLine("face3:");
            printVertices(face3);

            Face face4 = new Face();
            btr.AppendEntity(face4);
            face4.SetVertexAt(0, new Point3d(2 * Math.Cos(13*Math.PI / 10), 2 * Math.Sin(13*Math.PI / 10), 0));
            face4.SetVertexAt(1, new Point3d(0, 0, 1));
            face4.SetVertexAt(2, new Point3d(0, 0, -1));
            face4.SetVertexAt(3, new Point3d(0, 0, -1));
            face4.MakeEdgeInvisibleAt(1);
            face4.Color = Color.FromRgb(250, 0, 250);
            Console.WriteLine("face4:");
            printVertices(face4);
          }
          ta.Commit();
        }
        db.SaveAs(path + "FaceEx.dwg", DwgVersion.Current);
      }
    }
    void printVertices(Face face)
    {
      for (short i = 0; i < 4; i++)
        Console.WriteLine("vertex " + i + ": " + face.GetVertexAt(i) + ", edge visibility: " + face.IsEdgeVisibleAt(i));
      Console.WriteLine(System.Environment.NewLine);
    }
  }
}
