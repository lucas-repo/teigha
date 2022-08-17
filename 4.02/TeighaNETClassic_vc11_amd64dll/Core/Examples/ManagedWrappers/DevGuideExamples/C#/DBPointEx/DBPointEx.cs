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
  public class DBPointEx
  {
    public DBPointEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        // Changes point marker to "cross" instead of default "dot".
        db.Pdmode = 2;

        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            const int interval = 1;

            for (int i = 0; i < 4; i++)
              for (int j = 0; j < 4; j++)
                // Creates DBPoint entity and adds it into the Block Table Record.
                using (DBPoint dbPoint = new DBPoint())
                {
                  btr.AppendEntity(dbPoint);
                  // Sets position for every DBPoint entity.
                  dbPoint.Position = new Point3d(interval * j, interval * i, 0);
                  // Sets color for every DBPoint entity.
                  dbPoint.Color = Color.FromRgb((byte)(i * 60), (byte)(j * 60), 200);
                  // Sets thickness for every DBPoint entity.
                  dbPoint.Thickness = j;
                  // Sets thickness for every DBPoint entity.
                  dbPoint.EcsRotation = i * 0.314;
                }
          }
          ta.Commit();
        }
        db.SaveAs(path + "DBPointEx.dwg", DwgVersion.Current);
      }
    }
  }
}
