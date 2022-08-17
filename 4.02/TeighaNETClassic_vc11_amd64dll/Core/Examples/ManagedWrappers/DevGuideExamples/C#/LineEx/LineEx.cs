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
  public class LineEx
  {
    public LineEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Number of lines.
            const int lineNum = 10;
            // Line color from color index.
            short color = 1;
            for (int i = 0; i < lineNum; i++)
              // Creates Line entity and adds it into the Block Table Record.
              using (Line line = new Line())
              {
                btr.AppendEntity(line);
                if (color == 7)
                  color = 1;
                // Sets the same start point for all Line entity.
                line.StartPoint = new Point3d(0, 0, 0);
                // Sets end point for Line entity depending on its number.
                line.EndPoint = new Point3d((Math.Cos(2 * Math.PI / lineNum * i) * 10), (Math.Sin(2 * Math.PI / lineNum * i) * 10), 0);
                // Sets color from color index for Line entity depending on its number.
                line.Color = Color.FromColorIndex(ColorMethod.ByAci, color++);
                // Sets Thickness for Line entity depending on its number.
                line.Thickness = (double)i / 10;
                // Prints values of Angle, Delta and Thickness properties.
                Console.WriteLine("Angle is: " + line.Angle);
                Console.WriteLine("Delta is: " + line.Delta);
                Console.WriteLine("Thickness is: " + line.Thickness + "\n");
              }
          }
          ta.Commit();
        }
        db.SaveAs(path + "LineEx.dwg", DwgVersion.Current);
      }
    }
  }
}
