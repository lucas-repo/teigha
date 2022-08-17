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

namespace CDevGuideExamplesProject.MlineEx
{
  public class MlineEx
  {
    public MlineEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Multiline style
            MlineStyle mlStyle = new MlineStyle();
            
            Ray ray = new Ray();
            btr.AppendEntity(ray);
            ray.Color = Color.FromRgb(100, 100, 100);
            ray.UnitDir = new Vector3d(1, 0, 0);

            Mline mline1 = new Mline();
            btr.AppendEntity(mline1);
            mline1.AppendSegment(Point3d.Origin);
            mline1.Justification = MlineJustification.Bottom;
            mline1.AppendSegment(new Point3d(2, 2, 0));
            mline1.AppendSegment(new Point3d(4, 0, 0));
            mline1.AppendSegment(new Point3d(6, 6, 0));
            mline1.AppendSegment(new Point3d(16, 16, 0));
          
            mline1.MoveVertexAt(3, new Point3d(6, 2, 0));
            Point3d removedVertex = new Point3d();
            mline1.RemoveLastSegment(out removedVertex);
            Console.WriteLine("mline1:");
            printMline(mline1);
            
            Mline mline2 = new Mline();
            btr.AppendEntity(mline2);
            mline2.Justification = MlineJustification.Top;
            mline2.AppendSegment(new Point3d(8, 0, 0));
            mline2.AppendSegment(new Point3d(10, 2, 0));
            mline2.AppendSegment(new Point3d(12, 0, 0));
            mline2.AppendSegment(new Point3d(14, 2, 0));
            Console.WriteLine("mline2:");
            printMline(mline2);

            // Sets multiline style
            using (DBDictionary dic = (DBDictionary)tm.GetObject(db.MLStyleDictionaryId, OpenMode.ForWrite))
            {
              dic["ODA"] = mlStyle;
              mlStyle.Name = "ODAstyle";
              mlStyle.Description = "123";
              mlStyle.ShowMiters = true;
              mlStyle.StartInnerArcs = true;
              // Defines caps shapes
              mlStyle.StartRoundCap = true;
              mlStyle.EndSquareCap = true;

              // Set multiline as filled by color
              mlStyle.Filled = true;
              mlStyle.FillColor = Color.FromRgb(255, 200, 255);
              Teigha.Colors.Color color = Teigha.Colors.Color.FromRgb(255, 0, 0);

              // Adds multiline elements of specific color and offset
              mlStyle.Elements.Add(new MlineStyleElement(0, color, db.ByLayerLinetype), true);
              mlStyle.Elements.Add(new MlineStyleElement(0.5, color, db.ByLayerLinetype), true);
              mlStyle.Elements.Add(new MlineStyleElement(0.7, color, db.ByLayerLinetype), true);
              mlStyle.Elements.Add(new MlineStyleElement(2, color, db.ByLayerLinetype), true);
            }
                        
            Mline mline3 = new Mline();
            btr.AppendEntity(mline3);
            // Applies created style to the Mline entity
            mline3.Style = mlStyle.Id;
            // Sets caps to be drawn
            mline3.SupressStartCaps = false;
            mline3.SupressEndCaps = false;
            // Appends multiline segments
            mline3.AppendSegment(new Point3d(16, 0, 0));
            mline3.AppendSegment(new Point3d(18, 2, 0));
            mline3.AppendSegment(new Point3d(20, 0, 0));
            mline3.AppendSegment(new Point3d(22, 2, 0));

            Console.WriteLine("mline3:");
            printMline(mline3);
              
            Mline mline4 = new Mline();
            btr.AppendEntity(mline4);
            mline4.AppendSegment(new Point3d(24, 0, 0));
            mline4.AppendSegment(new Point3d(26, 2, 0));
            mline4.AppendSegment(new Point3d(28, 0, 0));
            mline4.AppendSegment(new Point3d(30, 2, 0));
            mline4.Scale = 0.1;
            mline4.IsClosed = true;

            Console.WriteLine("mline4:");
            printMline(mline4);
          }
          ta.Commit();
        }
        db.SaveAs(path + "MlineEx.dwg", DwgVersion.Current);
      }
    }

    // Prints Mline entity info 
    void printMline(Mline mline)
    {
      for (int i = 0; i < mline.NumberOfVertices; i++)
      {
        Console.WriteLine("\tvertex" + i + ": " + mline.VertexAt(i));
      }
      Console.WriteLine("\tJustification: " + mline.Justification);
      Console.WriteLine("\tIsClosed: " + mline.IsClosed);
      Console.WriteLine("\tStyle: " + ((MlineStyle)mline.Style.GetObject(OpenMode.ForRead)).Name + System.Environment.NewLine);
    }
  }
}
