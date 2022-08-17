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

namespace CDevGuideExamplesProject.HelixEx
{
  public class HelixEx
  {
    public HelixEx(String path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Creates an expanding helix (BaseRadius < TopRadius)
            Helix helix1 = new Helix();
            btr.AppendEntity(helix1);
            helix1.AxisVector = Vector3d.YAxis;
            helix1.StartPoint = new Point3d(0.5, 0.0, 0);
            helix1.SetAxisPoint(new Point3d(0.0, 0.0, 0), false);
            helix1.Constrain = ConstrainType.Height;
            helix1.TopRadius = 2;
            helix1.Height = 7.0;
            helix1.Turns = 4.0;
            helix1.CreateHelix();

            // Creates a helix converging to a point (TopRadius == 0)
            Helix helix2 = new Helix();
            btr.AppendEntity(helix2);
            helix2.AxisVector = new Vector3d(0, 1, 0);
            helix2.StartPoint = new Point3d(-8.0, 0.0, 0);
            helix2.SetAxisPoint(new Point3d(-10.0, 0.0, 0), false);
            helix2.TopRadius = 0;
            helix2.Constrain = ConstrainType.Height;
            helix2.Height = 7.0;
            helix2.Turns = 4.0;
            helix2.CreateHelix();

            // Creates a narrowing helix (BaseRadius > TopRadius)
            Helix helix3 = new Helix();
            btr.AppendEntity(helix3);
            helix3.CopyFrom(helix2);
            helix3.StartPoint = new Point3d(-18.0, 0.0, 0);
            helix3.SetAxisPoint(new Point3d(-20.0, 0.0, 0), false);
            helix3.TopRadius = 0.5;
            helix3.CreateHelix();

            // Creates a cylindrical helix (BaseRadius == TopRadius)
            Helix helix4 = new Helix();
            btr.AppendEntity(helix4);
            helix4.CopyFrom(helix1);
            helix4.StartPoint = new Point3d(12.0, 0.0, 0);
            helix4.SetAxisPoint(new Point3d(10.0, 0.0, 0), false);
            helix4.TopRadius = 2;
            helix4.CreateHelix();

            // Creates a helix diverging from a point (BaseRadius == 0)
            Helix helix5 = new Helix();
            btr.AppendEntity(helix5);
            helix5.CopyFrom(helix1);
            helix5.SetAxisPoint(new Point3d(20.0, 0.0, 0), false);
            helix5.BaseRadius = 0.0;
            helix5.TopRadius = 2;
            helix5.CreateHelix();
            
            // Creates a helix with the Height of 4
            Helix helix6 = new Helix();
            btr.AppendEntity(helix6);
            helix6.AxisVector = Vector3d.YAxis;
            helix6.TopRadius = 2;
            helix6.StartPoint = new Point3d(-18.0, 10.0, 0);
            helix6.SetAxisPoint(new Point3d(-20.0, 10.0, 0), false);
            helix6.Turns = 3;
            helix6.Constrain = ConstrainType.Height;
            helix6.Height = 4;
            helix6.CreateHelix();

            // Creates a helix with the Height changed to 7 with ConstrainType.TurnHeight
            Helix helix7 = new Helix();
            btr.AppendEntity(helix7);
            helix7.CopyFrom(helix6);
            helix7.StartPoint = new Point3d(-12.0, 10.0, 0);
            helix7.SetAxisPoint(new Point3d(-14.0, 10.0, 0), false);
            helix7.Turns = 3;
            helix7.Constrain = ConstrainType.TurnHeight;
            helix7.Height = 7;
            helix7.CreateHelix();

            // Creates a helix with the Height changed to 7 with ConstrainType.Turns
            Helix helix8 = new Helix();
            btr.AppendEntity(helix8);
            helix8.CopyFrom(helix7);
            helix8.StartPoint = new Point3d(-6.0, 10.0, 0);
            helix8.SetAxisPoint(new Point3d(-8.0, 10.0, 0), false);
            helix8.Turns = 3;
            helix8.Constrain = ConstrainType.Turns;
            helix8.Height = 7;
            helix8.CreateHelix();

            // Creates a helix with the TurnHeight changed to 2 with ConstrainType.Turns
            Helix helix9 = new Helix();
            btr.AppendEntity(helix9);
            helix9.CopyFrom(helix6);
            helix9.StartPoint = new Point3d(-0.0, 10.0, 0);
            helix9.SetAxisPoint(new Point3d(-2.0, 10.0, 0), false);
            helix9.Constrain = ConstrainType.Turns;
            helix9.TurnHeight = 2;
            helix9.CreateHelix();

            // Creates a helix with the TurnHeight changed to 2 with ConstrainType.TurnHeight
            Helix helix10 = new Helix();
            btr.AppendEntity(helix10);
            helix10.CopyFrom(helix6);
            helix10.StartPoint = new Point3d(6.0, 10.0, 0);
            helix10.SetAxisPoint(new Point3d(4.0, 10.0, 0), false);
            helix10.Constrain = ConstrainType.TurnHeight;
            helix10.TurnHeight = 2;
            helix10.CreateHelix();

            // Creates a helix with clockwise turns direction 
            Helix helix11 = new Helix();
            btr.AppendEntity(helix11);
            helix11.CopyFrom(helix6);
            helix11.StartPoint = new Point3d(-18.0, 20.0, 0);
            helix11.SetAxisPoint(new Point3d(-20.0, 20.0, 0), false);
            helix11.Twist = false;
            helix11.CreateHelix();

            // Creates a helix with the Turns changed to 5 with ConstrainType.TurnHeight
            Helix helix12 = new Helix();
            btr.AppendEntity(helix12);
            helix12.CopyFrom(helix11);
            helix12.StartPoint = new Point3d(-12.0, 20.0, 0);
            helix12.SetAxisPoint(new Point3d(-14.0, 20.0, 0), false);
            helix12.Constrain = ConstrainType.TurnHeight;
            helix12.Turns = 5;
            helix12.CreateHelix();

            // Creates a helix with the Turns changed to 5 with ConstrainType.Height
            Helix helix13 = new Helix();
            btr.AppendEntity(helix13);
            helix13.CopyFrom(helix11);
            helix13.StartPoint = new Point3d(-6.0, 20.0, 0);
            helix13.SetAxisPoint(new Point3d(-8.0, 20.0, 0), false);
            helix13.Constrain = ConstrainType.Height;
            helix13.Turns = 5;
            helix13.CreateHelix();
          }
          ta.Commit();
        }
        db.SaveAs(path + "HelixEx.dwg", DwgVersion.Current);
      }
    }
  }
}
