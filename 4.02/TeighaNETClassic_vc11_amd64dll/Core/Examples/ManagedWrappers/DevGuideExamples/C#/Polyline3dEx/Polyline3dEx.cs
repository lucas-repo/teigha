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

namespace CDevGuideExamplesProject.Polyline3dEx
{
  public class Polyline3dEx
  {
    public Polyline3dEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Creates Polyline3d entity with specified parameters
            Polyline3d pLine1 = new Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(1), false);
            btr.AppendEntity(pLine1);
            printPlolylineParams(pLine1);

            // Creates empty Polyline3d entity then append vertices and set properties
            Polyline3d pLine2 = new Polyline3d();
            btr.AppendEntity(pLine2);
            pLine2.Closed = false;
            foreach (Point3d pt in createCollectionForPolyline(2))
              pLine2.AppendVertex(new PolylineVertex3d(pt));
            // Converts created SimplePoly polyline to QuadSplinePoly type
            pLine2.ConvertToPolyType(Poly3dType.QuadSplinePoly);
            printPlolylineParams(pLine2);

            // Creates Polyline3d entity with specified parameters
            Polyline3d pLine3 = new Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(3), false);
            btr.AppendEntity(pLine3);
            pLine3.Closed = true;
            // Converts created SimplePoly polyline to QuadSplinePoly type
            pLine3.ConvertToPolyType(Poly3dType.QuadSplinePoly);
            // Straightens QuadSplinePoly polyline
            pLine3.Straighten();
            // Converts created SimplePoly polyline to CubicSplinePoly type
            pLine3.ConvertToPolyType(Poly3dType.CubicSplinePoly);
            printPlolylineParams(pLine3);

            // Creates Polyline3d entity with specified parameters
            Polyline3d pLine4 = new Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(4), false);
            btr.AppendEntity(pLine4);
            
            // Vertices to be inserted to SimplePoly polyline
            PolylineVertex3d vrtx1 = new PolylineVertex3d(new Point3d(45, -1, -3));
            PolylineVertex3d vrtx2 = new PolylineVertex3d(new Point3d(45, 12, -3));
            
            ObjectId[] verticesID = new ObjectId[12];
            int j = 0;
            foreach (ObjectId obj in pLine4)
            {
              using (DBObject dbObj = (DBObject)tm.GetObject(obj, OpenMode.ForRead))
              {
                if (dbObj is PolylineVertex3d)
                {
                  // Gets all vertices IDs
                  verticesID[j] = obj;
                  j++;
                }
              }
            }
            // Insrets vertices
            pLine4.InsertVertexAt(ObjectId.Null, vrtx1);
            pLine4.InsertVertexAt(verticesID[11], vrtx2);

            // Creates spline fitted polyline of CubicSplinePoly type and segments number of 2
            pLine4.SplineFit(Poly3dType.CubicSplinePoly, 2);
            printPlolylineParams(pLine4);
          }
          ta.Commit();
        }
        db.SaveAs(path + "Polyline3dEx.dwg", DwgVersion.Current);
      }
    }

    // Creates collection of Point3ds as a polyline vertices
    Point3dCollection createCollectionForPolyline(int i)
    {
      Point3d[] p3d = new Point3d[12];
      
      p3d[0] = new Point3d(6 + 10 * i, 0, 0);
      p3d[1] = new Point3d(1 + 10 * i, 1, 0);
      p3d[2] = new Point3d(0 + 10 * i, 2, -3);
      p3d[3] = new Point3d(5 + 10 * i, 3, -3);

      p3d[4] = new Point3d(6 + 10 * i, 4, 0);
      p3d[5] = new Point3d(1 + 10 * i, 5, 0);
      p3d[6] = new Point3d(0 + 10 * i, 6, -3);
      p3d[7] = new Point3d(5 + 10 * i, 7, -3);

      p3d[8] = new Point3d(6 + 10 * i, 8, 0);
      p3d[9] = new Point3d(1 + 10 * i, 9, 0);
      p3d[10] = new Point3d(0 + 10 * i, 10, -3);
      p3d[11] = new Point3d(5 + 10 * i, 11, -3);

      return new Point3dCollection(p3d);
    }

    // Prints polyline parameters to the console
    void printPlolylineParams(Polyline3d pLine)
    {
      Console.WriteLine("Polytype is " + pLine.PolyType);
      Console.WriteLine("Closed is " + pLine.Closed);
      int i = 0;
      
      // Gets vertices of a polyline and prints parameters of control and simple vertices 
      foreach (ObjectId objId in pLine)
      {
        using (DBObject obj = (DBObject)objId.GetObject(OpenMode.ForRead))
        {
          if (obj is PolylineVertex3d)
          {
            PolylineVertex3d pt = (PolylineVertex3d)obj;
            if ((pt.VertexType == Vertex3dType.ControlVertex) || (pt.VertexType == Vertex3dType.SimpleVertex))
            {
              Console.WriteLine("Vertex #" + i + ": " + pt.Position);
              i++;
            }
          }
        }
      }
      Console.WriteLine("Length is " + pLine.Length + System.Environment.NewLine);
    }  
  }
}
