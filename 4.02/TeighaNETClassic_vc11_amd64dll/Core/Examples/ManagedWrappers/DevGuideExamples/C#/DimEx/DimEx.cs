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
  public class DimEx
  {
    public DimEx(string path)
    {
      using (Database db = new Database(true, true))
      {
        LinetypeTableRecord ltr;
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          // Create a new linetype
          using (LinetypeTable lineTable = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForWrite))
          {
            ltr = new LinetypeTableRecord();
            ltr.Name = "TestLineType";
            lineTable.Add(ltr);
            ltr.PatternLength = 0.1;
            ltr.NumDashes = 9;
            ltr.SetDashLengthAt(0, 0.2);
            ltr.SetDashLengthAt(1, -0.2);
          }

          // Get the current space block
          using (BlockTableRecord btr = (BlockTableRecord)ta.GetObject(db.CurrentSpaceId, OpenMode.ForWrite))
          {
            // Aligned dimensions
            #region Aligned dimensions
            AlignedDimension alDim1 = new AlignedDimension(new Point3d(0, 0, 0), new Point3d(3, 1, 0), new Point3d(1, 2, 0), null, db.Dimstyle);
            btr.AppendEntity(alDim1);
            alDim1.SetDatabaseDefaults();

            AlignedDimension alDim2 = new AlignedDimension();
            btr.AppendEntity(alDim2);
            alDim2.CopyFrom(alDim1);
            alDim2.Oblique = 4 * Math.PI / 9;
            alDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            alDim2.UsingDefaultTextPosition = false;
            alDim2.Dimtmove = 1;
            alDim2.TextPosition = new Point3d(2, 4, 0);

            AlignedDimension alDim3 = new AlignedDimension();
            btr.AppendEntity(alDim3);
            alDim3.CopyFrom(alDim1);
            alDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            alDim3.UsingDefaultTextPosition = false;
            alDim3.TextPosition = new Point3d(4.5, 9, 0);
            alDim3.Dimtoh = false;
            alDim3.Dimtad = 1;
            alDim3.DimensionText = "Distance is <> units";

            AlignedDimension alDim4 = new AlignedDimension();
            btr.AppendEntity(alDim4);
            alDim4.CopyFrom(alDim1);
            alDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            alDim4.Dimtmove = 0;
            alDim4.Dimtih = false;
            alDim4.Dimtad = 1;
            alDim4.Dimjust = 4;

            AlignedDimension alDim5 = new AlignedDimension();
            btr.AppendEntity(alDim5);
            alDim5.CopyFrom(alDim1);
            alDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            alDim5.Dimgap = -0.2;
            alDim5.Dimsah = true;
            alDim5.Dimblk1s = "Dot";
            alDim5.Dimblk2s = "Oblique";
            alDim5.Dimdle = 0.1;
            alDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            alDim5.Dimltex1 = ltr.ObjectId;

            AlignedDimension alDim6 = new AlignedDimension();
            btr.AppendEntity(alDim6);
            alDim6.CopyFrom(alDim1);
            alDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            alDim6.Dimtol = true;
            alDim6.Dimtp = 0.02;
            alDim6.Dimtm = 0.02;
            #endregion

            // Rotated dimensions
            #region Rotated dimensions
            RotatedDimension rotDim1 = new RotatedDimension(-0.2, new Point3d(7, 0, 0), new Point3d(10, 1, 0), new Point3d(8, 2, 0), null, db.Dimstyle);
            btr.AppendEntity(rotDim1);
            alDim1.SetDatabaseDefaults();

            RotatedDimension rotDim2 = new RotatedDimension();
            btr.AppendEntity(rotDim2);
            rotDim2.CopyFrom(rotDim1);
            rotDim2.Oblique = 4 * Math.PI / 9;
            rotDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            rotDim2.UsingDefaultTextPosition = false;
            rotDim2.Dimtmove = 1;
            rotDim2.TextPosition = new Point3d(9, 4, 0);

            RotatedDimension rotDim3 = new RotatedDimension();
            btr.AppendEntity(rotDim3);
            rotDim3.CopyFrom(rotDim1);
            rotDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            rotDim3.UsingDefaultTextPosition = false;
            rotDim3.TextPosition = new Point3d(11.5, 9, 0);
            rotDim3.Dimtoh = false;
            rotDim3.Dimtad = 1;
            rotDim3.DimensionText = "Distance is <> units";

            RotatedDimension rotDim4 = new RotatedDimension();
            btr.AppendEntity(rotDim4);
            rotDim4.CopyFrom(rotDim1);
            rotDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            rotDim4.Dimtmove = 0;
            rotDim4.Dimtih = false;
            rotDim4.Dimtad = 1;
            rotDim4.Dimjust = 4;

            RotatedDimension rotDim5 = new RotatedDimension();
            btr.AppendEntity(rotDim5);
            rotDim5.CopyFrom(rotDim1);
            rotDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            rotDim5.Dimgap = -0.2;
            rotDim5.Dimsah = true;
            rotDim5.Dimblk1s = "Dot";
            rotDim5.Dimblk2s = "Oblique";
            rotDim5.Dimdle = 0.1;
            rotDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            rotDim5.Dimltex1 = ltr.ObjectId;
            rotDim5.Dimlwe = LineWeight.LineWeight080;

            RotatedDimension rotDim6 = new RotatedDimension();
            btr.AppendEntity(rotDim6);
            rotDim6.CopyFrom(rotDim1);
            rotDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            rotDim6.Dimtol = true;
            rotDim6.Dimtp = 0.02;
            rotDim6.Dimtm = 0.02;
            #endregion

            // Two-Line Angular dimensions
            #region Two-Line Angular dimensions
            LineAngularDimension2 lineAngularDim1 = new LineAngularDimension2(new Point3d(16, 0, 0), new Point3d(16.5, 1, 0), new Point3d(15, 0, 0), new Point3d(14.5, 1, 0), new Point3d(15.5, 2, 0), null, db.Dimstyle);
            btr.AppendEntity(lineAngularDim1);
            lineAngularDim1.SetDatabaseDefaults();

            LineAngularDimension2 lineAngularDim2 = new LineAngularDimension2();
            btr.AppendEntity(lineAngularDim2);
            lineAngularDim2.CopyFrom(lineAngularDim1);
            lineAngularDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            lineAngularDim2.UsingDefaultTextPosition = false;
            lineAngularDim2.Dimtmove = 1;
            lineAngularDim2.TextPosition = new Point3d(16, 4, 0);

            LineAngularDimension2 lineAngularDim3 = new LineAngularDimension2();
            btr.AppendEntity(lineAngularDim3);
            lineAngularDim3.CopyFrom(lineAngularDim1);
            lineAngularDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            lineAngularDim3.UsingDefaultTextPosition = false;
            lineAngularDim3.TextPosition = new Point3d(18.5, 9, 0);
            lineAngularDim3.Dimtoh = false;
            lineAngularDim3.Dimtad = 1;
            lineAngularDim3.DimensionText = "Angle is <> units";

            LineAngularDimension2 lineAngularDim4 = new LineAngularDimension2();
            btr.AppendEntity(lineAngularDim4);
            lineAngularDim4.CopyFrom(lineAngularDim1);
            lineAngularDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            lineAngularDim4.Dimtmove = 0;
            lineAngularDim4.Dimtih = false;
            lineAngularDim4.Dimtad = 1;
            lineAngularDim4.Dimjust = 4;

            LineAngularDimension2 lineAngularDim5 = new LineAngularDimension2();
            btr.AppendEntity(lineAngularDim5);
            lineAngularDim5.CopyFrom(lineAngularDim1);
            lineAngularDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            lineAngularDim5.Dimgap = -0.2;
            lineAngularDim5.Dimsah = true;
            lineAngularDim5.Dimblk1s = "Dot";
            lineAngularDim5.Dimblk2s = "Oblique";
            lineAngularDim5.Dimdle = 0.1;
            lineAngularDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            lineAngularDim5.Dimltex1 = ltr.ObjectId;
            lineAngularDim5.Dimlwe = LineWeight.LineWeight080;

            LineAngularDimension2 lineAngularDim6 = new LineAngularDimension2();
            btr.AppendEntity(lineAngularDim6);
            lineAngularDim6.CopyFrom(lineAngularDim1);
            lineAngularDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            lineAngularDim6.Dimtol = true;
            lineAngularDim6.Dimtp = 0.02;
            lineAngularDim6.Dimtm = 0.02;
            #endregion

            #region Three-Point Angular dimensions
            // Three-Point Angular dimensions
            Point3AngularDimension pointAngularDim1 = new Point3AngularDimension(new Point3d(22.5, -1, 0), new Point3d(23.5, 1, 0), new Point3d(21.5, 1, 0), new Point3d(22.5, 2, 0), null, db.Dimstyle);
            btr.AppendEntity(pointAngularDim1);
            pointAngularDim1.SetDatabaseDefaults();

            Point3AngularDimension pointAngularDim2 = new Point3AngularDimension();
            btr.AppendEntity(pointAngularDim2);
            pointAngularDim2.CopyFrom(pointAngularDim1);
            pointAngularDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            pointAngularDim2.UsingDefaultTextPosition = false;
            pointAngularDim2.Dimtmove = 1;
            pointAngularDim2.TextPosition = new Point3d(23, 4, 0);

            Point3AngularDimension pointAngularDim3 = new Point3AngularDimension();
            btr.AppendEntity(pointAngularDim3);
            pointAngularDim3.CopyFrom(pointAngularDim1);
            pointAngularDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            pointAngularDim3.UsingDefaultTextPosition = false;
            pointAngularDim3.TextPosition = new Point3d(25.5, 9, 0);
            pointAngularDim3.Dimtoh = false;
            pointAngularDim3.Dimtad = 1;
            pointAngularDim3.DimensionText = "Angle is <> units";

            Point3AngularDimension pointAngularDim4 = new Point3AngularDimension();
            btr.AppendEntity(pointAngularDim4);
            pointAngularDim4.CopyFrom(pointAngularDim1);
            pointAngularDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            pointAngularDim4.Dimtmove = 0;
            pointAngularDim4.Dimtih = false;
            pointAngularDim4.Dimtad = 1;
            pointAngularDim4.Dimjust = 4;

            Point3AngularDimension pointAngularDim5 = new Point3AngularDimension();
            btr.AppendEntity(pointAngularDim5);
            pointAngularDim5.CopyFrom(pointAngularDim1);
            pointAngularDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            pointAngularDim5.Dimgap = -0.2;
            pointAngularDim5.Dimsah = true;
            pointAngularDim5.Dimblk1s = "Dot";
            pointAngularDim5.Dimblk2s = "Oblique";
            pointAngularDim5.Dimdle = 0.1;
            pointAngularDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            pointAngularDim5.Dimltex1 = ltr.ObjectId;
            pointAngularDim5.Dimlwe = LineWeight.LineWeight080;

            Point3AngularDimension pointAngularDim6 = new Point3AngularDimension();
            btr.AppendEntity(pointAngularDim6);
            pointAngularDim6.CopyFrom(pointAngularDim1);
            pointAngularDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            pointAngularDim6.Dimtol = true;
            pointAngularDim6.Dimtp = 0.02;
            pointAngularDim6.Dimtm = 0.02;
            #endregion

            // Arc Length dimensions
            #region Arc Length Dimension
            ArcDimension arcDim1 = new ArcDimension(new Point3d(29.5, -1, 0), new Point3d(30.5, -0.5, 0), new Point3d(28.5, -0.5, 0), new Point3d(29.5, 2, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim1);
            arcDim1.SetDatabaseDefaults();
            arcDim1.Dimarcsym = 2;

            ArcDimension arcDim2 = new ArcDimension(new Point3d(29.5, 2, 0), new Point3d(30.5, 2.5, 0), new Point3d(28.5, 2.5, 0), new Point3d(29.5, 5, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim2);
            arcDim1.SetDatabaseDefaults();
            arcDim2.UsingDefaultTextPosition = false;
            arcDim2.Dimtmove = 1;
            arcDim2.TextPosition = new Point3d(30, 4, 0);

            ArcDimension arcDim3 = new ArcDimension(new Point3d(29.5, 5, 0), new Point3d(30.5, 5.5, 0), new Point3d(28.5, 5.5, 0), new Point3d(29.5, 8, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim3);
            arcDim3.SetDatabaseDefaults();
            arcDim3.UsingDefaultTextPosition = false;
            arcDim3.TextPosition = new Point3d(32, 9, 0);
            arcDim3.Dimtoh = false;
            arcDim3.Dimtad = 1;
            arcDim3.DimensionText = "Arc length is <> units";

            ArcDimension arcDim4 = new ArcDimension(new Point3d(29.5, 8, 0), new Point3d(30.5, 8.5, 0), new Point3d(28.5, 8.5, 0), new Point3d(29.5, 11, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim4);
            arcDim4.SetDatabaseDefaults();
            arcDim4.Dimtmove = 0;
            arcDim4.Dimtih = false;
            arcDim4.Dimtad = 1;
            arcDim4.Dimjust = 4;

            ArcDimension arcDim5 = new ArcDimension(new Point3d(29.5, 11, 0), new Point3d(30.5, 11.5, 0), new Point3d(28.5, 11.5, 0), new Point3d(29.5, 14, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim5);
            arcDim5.SetDatabaseDefaults();
            arcDim5.Dimgap = -0.2;
            arcDim5.Dimsah = true;
            arcDim5.Dimblk1s = "Dot";
            arcDim5.Dimblk2s = "Oblique";
            arcDim5.Dimdle = 0.1;
            arcDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            arcDim5.Dimltex1 = ltr.ObjectId;
            arcDim5.Dimlwe = LineWeight.LineWeight080;

            ArcDimension arcDim6 = new ArcDimension(new Point3d(29.5, 14, 0), new Point3d(30.5, 14.5, 0), new Point3d(28.5, 14.5, 0), new Point3d(29.5, 17, 0), null, db.Dimstyle);
            btr.AppendEntity(arcDim6);
            arcDim6.SetDatabaseDefaults();
            arcDim6.Dimtol = true;
            arcDim6.Dimtp = 0.02;
            arcDim6.Dimtm = 0.02;
            #endregion

            // Radial dimensions
            #region Radial dimensions
            RadialDimension radDim1 = new RadialDimension(new Point3d(37, 0, 0), new Point3d(40, 1, 0), 1, null, db.Dimstyle);
            btr.AppendEntity(radDim1);
            radDim1.SetDatabaseDefaults();

            RadialDimension radDim2 = new RadialDimension();
            btr.AppendEntity(radDim2);
            radDim2.CopyFrom(radDim1);
            radDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            radDim2.UsingDefaultTextPosition = false;
            radDim2.Dimtmove = 1;
            radDim2.TextPosition = new Point3d(37, 4, 0);

            RadialDimension radDim3 = new RadialDimension();
            btr.AppendEntity(radDim3);
            radDim3.CopyFrom(radDim1);
            radDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            radDim3.UsingDefaultTextPosition = false;
            radDim3.TextPosition = new Point3d(39, 9, 0);
            radDim3.Dimtoh = false;
            radDim3.Dimtad = 1;
            radDim3.DimensionText = "Radius is <> units";

            RadialDimension radDim4 = new RadialDimension();
            btr.AppendEntity(radDim4);
            radDim4.CopyFrom(radDim1);
            radDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            radDim4.Dimtmove = 0;
            radDim4.Dimtad = 1;

            RadialDimension radDim5 = new RadialDimension();
            btr.AppendEntity(radDim5);
            radDim5.CopyFrom(radDim1);
            radDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            radDim5.Dimgap = -0.2;
            radDim5.Dimblks = "Dot";
            radDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            radDim5.Dimltype = ltr.ObjectId;
            radDim5.Dimlwd = LineWeight.LineWeight020;
            radDim5.Dimtofl = true;

            RadialDimension radDim6 = new RadialDimension();
            btr.AppendEntity(radDim6);
            radDim6.CopyFrom(radDim1);
            radDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            radDim6.Dimtol = true;
            radDim6.Dimtp = 0.02;
            radDim6.Dimtm = 0.02;
            #endregion

            // Diametric dimensions
            #region Diametric dimensions
            DiametricDimension diamDim1 = new DiametricDimension(new Point3d(50, 1, 0), new Point3d(44, -1, 0), 1, null, db.Dimstyle);
            btr.AppendEntity(diamDim1);
            diamDim1.SetDatabaseDefaults();

            DiametricDimension diamDim2 = new DiametricDimension();
            btr.AppendEntity(diamDim2);
            diamDim2.CopyFrom(diamDim1);
            diamDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            diamDim2.UsingDefaultTextPosition = false;
            diamDim2.Dimtmove = 1;
            diamDim2.TextPosition = new Point3d(47, 4, 0);

            DiametricDimension diamDim3 = new DiametricDimension();
            btr.AppendEntity(diamDim3);
            diamDim3.CopyFrom(diamDim1);
            diamDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            diamDim3.UsingDefaultTextPosition = false;
            diamDim3.TextPosition = new Point3d(52, 9, 0);
            diamDim3.Dimtoh = false;
            diamDim3.Dimtad = 1;
            diamDim3.DimensionText = "Diameter is <> units";

            DiametricDimension diamDim4 = new DiametricDimension();
            btr.AppendEntity(diamDim4);
            diamDim4.CopyFrom(diamDim1);
            diamDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            diamDim4.Dimtmove = 0;
            diamDim4.Dimtad = 1;

            DiametricDimension diamDim5 = new DiametricDimension();
            btr.AppendEntity(diamDim5);
            diamDim5.CopyFrom(diamDim1);
            diamDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            diamDim5.Dimgap = -0.2;
            diamDim5.Dimblks = "Dot";
            diamDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            diamDim5.Dimltype = ltr.ObjectId;
            diamDim5.Dimlwd = LineWeight.LineWeight020;
            diamDim5.Dimtofl = true;

            DiametricDimension diamDim6 = new DiametricDimension();
            btr.AppendEntity(diamDim6);
            diamDim6.CopyFrom(diamDim1);
            diamDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            diamDim6.Dimtol = true;
            diamDim6.Dimtp = 0.02;
            diamDim6.Dimtm = 0.02;
            #endregion

            // Radial large dimensions
            #region Radial large dimensions
            RadialDimensionLarge radLargeDim1 = new RadialDimensionLarge(new Point3d(57, 0, 0), new Point3d(60, 0, 0), new Point3d(62, 2, 0), new Point3d(61, 0.5, 0), Math.PI / 4, null, db.Dimstyle);
            btr.AppendEntity(radLargeDim1);
            radLargeDim1.SetDatabaseDefaults();

            RadialDimensionLarge radLargeDim2 = new RadialDimensionLarge();
            btr.AppendEntity(radLargeDim2);
            radLargeDim2.CopyFrom(radLargeDim1);
            radLargeDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            radLargeDim2.UsingDefaultTextPosition = false;
            radLargeDim2.TextPosition = new Point3d(59, 4, 0);
            radLargeDim2.Dimtmove = 0;
            radLargeDim2.Dimtih = true;

            RadialDimensionLarge radLargeDim3 = new RadialDimensionLarge();
            btr.AppendEntity(radLargeDim3);
            radLargeDim3.CopyFrom(radLargeDim1);
            radLargeDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            radLargeDim3.ChordPoint = new Point3d(58, 7, 0);
            radLargeDim3.JogPoint = new Point3d(60, 7, 0);
            radLargeDim3.Dimtad = 1;
            radLargeDim3.Dimtih = false;
            radLargeDim3.DimensionText = "Radius is <> units";

            RadialDimensionLarge radLargeDim4 = new RadialDimensionLarge();
            btr.AppendEntity(radLargeDim4);
            radLargeDim4.CopyFrom(radLargeDim1);
            radLargeDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            radLargeDim4.Dimtmove = 0;
            radLargeDim4.Dimtad = 1;

            RadialDimensionLarge radLargeDim5 = new RadialDimensionLarge();
            btr.AppendEntity(radLargeDim5);
            radLargeDim5.CopyFrom(radLargeDim1);
            radLargeDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            radLargeDim5.Dimgap = -0.2;
            radLargeDim5.Dimblks = "Dot";
            radLargeDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            radLargeDim5.Dimltype = ltr.ObjectId;
            radLargeDim5.Dimlwd = LineWeight.LineWeight020;
            radLargeDim5.Dimtofl = false;
            radLargeDim5.JogAngle = Math.PI / 10;

            RadialDimensionLarge radLargeDim6 = new RadialDimensionLarge();
            btr.AppendEntity(radLargeDim6);
            radLargeDim6.CopyFrom(radLargeDim1);
            radLargeDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            radLargeDim6.Dimtol = true;
            radLargeDim6.Dimtp = 0.02;
            radLargeDim6.Dimtm = 0.02;
            #endregion

            // Ordinate dimensions
            #region Ordinate dimensions
            OrdinateDimension ordDim1 = new OrdinateDimension(true, new Point3d(65, 0, 0), new Point3d(66, 3, 0), null, db.Dimstyle);
            btr.AppendEntity(ordDim1);

            OrdinateDimension ordDim2 = new OrdinateDimension();
            btr.AppendEntity(ordDim2);
            ordDim2.CopyFrom(ordDim1);
            ordDim2.UsingXAxis = false;
            ordDim2.DefiningPoint = new Point3d(65, 3, 0);
            ordDim2.TransformBy(Matrix3d.Displacement(new Vector3d(0, 3, 0)));
            ordDim2.UsingDefaultTextPosition = false;
            ordDim2.Dimtmove = 1;
            ordDim2.TextPosition = new Point3d(68, 4, 0);

            OrdinateDimension ordDim3 = new OrdinateDimension();
            btr.AppendEntity(ordDim3);
            ordDim3.CopyFrom(ordDim1);
            ordDim3.TransformBy(Matrix3d.Displacement(new Vector3d(0, 6, 0)));
            ordDim3.UsingDefaultTextPosition = false;
            ordDim3.TextPosition = new Point3d(70, 9, 0);
            ordDim3.Dimtoh = false;
            ordDim3.Dimtad = 1;
            ordDim3.DimensionText = "Distance is <> units";

            OrdinateDimension ordDim4 = new OrdinateDimension();
            btr.AppendEntity(ordDim4);
            ordDim4.CopyFrom(ordDim1);
            ordDim4.TransformBy(Matrix3d.Displacement(new Vector3d(0, 9, 0)));
            ordDim4.Dimtmove = 0;
            ordDim4.Dimtih = false;
            ordDim4.Dimtad = 1;
            ordDim4.Dimjust = 4;

            OrdinateDimension ordDim5 = new OrdinateDimension();
            btr.AppendEntity(ordDim5);
            ordDim5.CopyFrom(ordDim1);
            ordDim5.TransformBy(Matrix3d.Displacement(new Vector3d(0, 12, 0)));
            ordDim5.Dimgap = -0.2;
            ordDim5.Dimsah = true;
            ordDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1);
            ordDim5.Dimltex2 = ltr.ObjectId;

            OrdinateDimension ordDim6 = new OrdinateDimension();
            btr.AppendEntity(ordDim6);
            ordDim6.CopyFrom(ordDim1);
            ordDim6.TransformBy(Matrix3d.Displacement(new Vector3d(0, 15, 0)));
            ordDim6.Dimtol = true;
            ordDim6.Dimtp = 0.02;
            ordDim6.Dimtm = 0.02;
            #endregion
          }
          ta.Commit();
        }
        db.SaveAs(path + "DimEx.dwg", DwgVersion.Current);
      }
    }
  }
}
