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

using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Colors;

namespace CDevGuideExamplesProject
{
  public class CircleEx
  {
    public CircleEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Number of circles.
            const int cirNum = 10;
            // Circle color from color index.
            short color = 1;
            // Circle thickness.
            short thickness = 0;
            // Angle for calculating circle center.
            double alpha = 0.0;
            // Circle radius.
            double radius = 1;

            for (int i = 0; i < cirNum; i++)
              // Creates Circle entity and adds it into the Block Table Record.
              using (Circle cir = new Circle())
              {
                btr.AppendEntity(cir);
                // Sets Circle properties
                cir.Center = new Point3d(Math.Cos(alpha), Math.Sin(alpha), 0);
                cir.Radius = radius;
                cir.Thickness = thickness;
                // Sets color from color index for Circle entity.
                cir.Color = Color.FromColorIndex(ColorMethod.ByAci, color);

                thickness++;
                radius += 0.5;
                alpha += 2 * Math.PI / cirNum;
                color++;
                if (color == 7)
                  color = 1;

                // Prints Circle properties.
                Console.WriteLine("Center is: " + cir.Center);
                Console.WriteLine("Radius is: " + cir.Radius);
                Console.WriteLine("Circumference is: " + cir.Circumference);
                Console.WriteLine("Thickness is: " + cir.Thickness);
                Console.WriteLine("Normal is: " + cir.Normal + "\n");
              }
          }
          ta.Commit();
        }
        db.SaveAs(path + "CircleEx.dwg", DwgVersion.Current);
        db.Dispose();
      }
    }
  }
}
