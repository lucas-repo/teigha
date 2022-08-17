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
  public class DBTextEx
  {
    public DBTextEx(String path)
    {
      using(Database db = new Database(true, true))
      {
        // Changes point marker to "cross" instead of default "dot".
        db.Pdmode = 2;
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
          {
            // Creates a DBText entity with default settings.
            using (DBText dbText1 = new DBText())
            {
              btr.AppendEntity(dbText1);
              dbText1.TextString = "This is a text with default settings";
            }

            // Creates a DBText entity with height of 1.
            using (DBText dbText2 = new DBText())
            {
              btr.AppendEntity(dbText2);
              dbText2.Position = new Point3d(0, -2, 0);
              dbText2.Height = 1;
              dbText2.TextString = "This is a text with Height of " + dbText2.Height;
            }

            // Creates a DBText entity with WidthFactor of 2.
            using (DBText dbText3 = new DBText())
            {
              btr.AppendEntity(dbText3);
              dbText3.Position = new Point3d(0, -4, 0);
              dbText3.Height = 0.5;
              dbText3.WidthFactor = 2;
              dbText3.TextString = "This is a text with Height of " + dbText3.Height + " and WidthFactor of " + dbText3.WidthFactor;
            }

            // Creates a DBText entity with AlignmentPoint at (10, -6, 0) and TopLeft justify mode.
            using (DBText dbText4 = new DBText())
            {
              btr.AppendEntity(dbText4);
              dbText4.Position = new Point3d(0, -6, 0);
              dbText4.Height = 0.5;
              dbText4.AlignmentPoint = new Point3d(10, -6, 0);
              using (DBPoint alignmentPoint4 = new DBPoint(dbText4.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint4);
                alignmentPoint4.Color = Color.FromRgb(255, 0, 0);
              }
              dbText4.Justify = AttachmentPoint.TopLeft;
              dbText4.TextString = "This is a text with Position at " + dbText4.Position + ", AlignmentPoint at " + dbText4.AlignmentPoint + " and " + dbText4.Justify + "  justify mode";
            }

            // Creates a DBText entity with AlignmentPoint at (10, -8, 0) and TopCenter justify mode.
            using (DBText dbText5 = new DBText())
            {
              btr.AppendEntity(dbText5);
              dbText5.Position = new Point3d(0, -8, 0);
              dbText5.Height = 0.5;
              dbText5.AlignmentPoint = new Point3d(10, -8, 0);
              using (DBPoint alignmentPoint5 = new DBPoint(dbText5.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint5);
                alignmentPoint5.Color = Color.FromRgb(255, 0, 0);
              }
              dbText5.Justify = AttachmentPoint.TopCenter;
              dbText5.TextString = "This is a text with Position at " + dbText5.Position + ", AlignmentPoint at " + dbText5.AlignmentPoint + " and " + dbText5.Justify + " mode";
            }

            // Creates a DBText entity with AlignmentPoint at (10, -10, 0) and TopRight justify mode.
            using (DBText dbText6 = new DBText())
            {
              btr.AppendEntity(dbText6);
              dbText6.Position = new Point3d(0, -10, 0);
              dbText6.Height = 0.5;
              dbText6.AlignmentPoint = new Point3d(10, -10, 0);
              using (DBPoint alignmentPoint6 = new DBPoint(dbText6.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint6);
                alignmentPoint6.Color = Color.FromRgb(255, 0, 0);
              }
              dbText6.Justify = AttachmentPoint.TopRight;
              dbText6.TextString = "This is a text with Position at " + dbText6.Position + ", AlignmentPoint at " + dbText6.AlignmentPoint + " and " + dbText6.Justify + " mode";
            }

            // Creates a DBText entity with AlignmentPoint at (10, -12, 0) and BaseCenter justify mode.
            using (DBText dbText7 = new DBText())
            {
              btr.AppendEntity(dbText7);
              dbText7.Position = new Point3d(0, -12, 0);
              dbText7.Height = 0.5;
              dbText7.AlignmentPoint = new Point3d(10, -12, 0);
              using (DBPoint alignmentPoint7 = new DBPoint(dbText7.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint7);
                alignmentPoint7.Color = Color.FromRgb(255, 0, 0);
              }
              dbText7.Justify = AttachmentPoint.BaseCenter;
              dbText7.TextString = "This is a text with Position at " + dbText7.Position + ", AlignmentPoint at " + dbText7.AlignmentPoint + " and " + dbText7.Justify + " mode";
            }

            // Creates a DBText entity with AlignmentPoint at (10, -14, 0) and BaseAlign justify mode.
            using (DBText dbText8 = new DBText())
            {
              btr.AppendEntity(dbText8);
              dbText8.Position = new Point3d(0, -14, 0);
              dbText8.Height = 0.5;
              dbText8.AlignmentPoint = new Point3d(10, -14, 0);
              using (DBPoint alignmentPoint8 = new DBPoint(dbText8.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint8);
                alignmentPoint8.Color = Color.FromRgb(255, 0, 0);
              }
              dbText8.Justify = AttachmentPoint.BaseAlign;
              dbText8.TextString = "This is a text with Position at " + dbText8.Position + ", AlignmentPoint at " + dbText8.AlignmentPoint + " and " + dbText8.Justify + " mode";
            }

            // Creates a DBText entity with AlignmentPoint at (10, -16, 0) and BaseFit justify mode.
            using (DBText dbText9 = new DBText())
            {
              btr.AppendEntity(dbText9);
              dbText9.Position = new Point3d(0, -16, 0);
              dbText9.Height = 0.5;
              dbText9.AlignmentPoint = new Point3d(10, -16, 0);
              using (DBPoint alignmentPoint9 = new DBPoint(dbText9.AlignmentPoint))
              {
                btr.AppendEntity(alignmentPoint9);
                alignmentPoint9.Color = Color.FromRgb(255, 0, 0);
              }
              dbText9.Justify = AttachmentPoint.BaseFit;
              dbText9.TextString = "This is a text with Position at " + dbText9.Position + ", AlignmentPoint at " + dbText9.AlignmentPoint + " and " + dbText9.Justify + " mode";
            }

            // Creates a DBText entity with Oblique of -0.7.
            using (DBText dbText10 = new DBText())
            {
              btr.AppendEntity(dbText10);
              dbText10.Position = new Point3d(0, -18, 0);
              dbText10.Height = 0.5;
              dbText10.Oblique = -0.7;
              dbText10.TextString = "This is a text with oblique angle of " + dbText10.Oblique;
            }

            // Creates a DBText entity with Oblique of 0.
            using (DBText dbText11 = new DBText())
            {
              btr.AppendEntity(dbText11);
              dbText11.Position = new Point3d(0, -20, 0);
              dbText11.Height = 0.5;
              dbText11.TextString = "This is a text with oblique angle of " + dbText11.Oblique;
            }

            // Creates a DBText entity with Oblique of 0.7.
            using (DBText dbText12 = new DBText())
            {
              btr.AppendEntity(dbText12);
              dbText12.Position = new Point3d(0, -22, 0);
              dbText12.Height = 0.5;
              dbText12.Oblique = 0.7;
              dbText12.TextString = "This is a text with oblique angle of " + dbText12.Oblique;
            }

            // Creates a DBText entity with Rotation of -0.7.
            using (DBText dbText13 = new DBText())
            {
              btr.AppendEntity(dbText13);
              dbText13.Position = new Point3d(0, -24, 0);
              dbText13.Height = 0.5;
              dbText13.Rotation = -0.7;
              dbText13.TextString = "This is a text with rotation angle of " + dbText13.Rotation;
            }

            // Creates a DBText entity with Rotation of 0.0.
            using (DBText dbText14 = new DBText())
            {
              btr.AppendEntity(dbText14);
              dbText14.Position = new Point3d(0, -26, 0);
              dbText14.Height = 0.5;
              dbText14.TextString = "This is a text with rotation angle of " + dbText14.Rotation;
            }

            // Creates a DBText entity with Rotation of 0.7.
            using (DBText dbText15 = new DBText())
            {
              btr.AppendEntity(dbText15);
              dbText15.Position = new Point3d(0, -28, 0);
              dbText15.Height = 0.5;
              dbText15.Rotation = 0.7;
              dbText15.TextString = "This is a text with rotation angle of " + dbText15.Rotation;
            }

            // Creates a DBText entity mirrored in X and Y directions.
            using (DBText dbText16 = new DBText())
            {
              btr.AppendEntity(dbText16);
              dbText16.Position = new Point3d(0, -30, 0);
              dbText16.Height = 0.5;
              dbText16.IsMirroredInX = true;
              dbText16.IsMirroredInY = true;
              dbText16.TextString = "This is a text mirrored in X and Y";
            }

            // Creates a DBText entity with thickness of 5.
            using (DBText dbText17 = new DBText())
            {
              btr.AppendEntity(dbText17);
              dbText17.Position = new Point3d(0, -32, 0);
              dbText17.Height = 0.5;
              dbText17.Thickness = 5;
              dbText17.TextString = "This is a text with thickness of " + dbText17.Thickness;
            }
          }
          ta.Commit();
        }
        db.SaveAs(path+"DBTextEx.dwg", DwgVersion.Current);
      }
    }
  }
}
