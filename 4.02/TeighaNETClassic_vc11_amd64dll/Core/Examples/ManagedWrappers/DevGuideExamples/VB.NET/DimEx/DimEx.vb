'/////////////////////////////////////////////////////////////////////////////// 
'// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
'// All rights reserved. 
'// 
'// This software and its documentation and related materials are owned by 
'// the Alliance. The software may only be incorporated into application 
'// programs owned by members of the Alliance, subject to a signed 
'// Membership Agreement and Supplemental Software License Agreement with the
'// Alliance. The structure and organization of this software are the valuable  
'// trade secrets of the Alliance and its suppliers. The software is also 
'// protected by copyright law and international treaty provisions. Application  
'// programs incorporating this software must include the following statement 
'// with their copyright notices:
'//   
'//   This application incorporates Teigha(R) software pursuant to a license 
'//   agreement with Open Design Alliance.
'//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
'//   All rights reserved.
'//
'// By use of this software, its documentation or related materials, you 
'// acknowledge and accept the above terms.
'///////////////////////////////////////////////////////////////////////////////
Public Class DimEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)

      Dim tm As TransactionManager
      tm = db.TransactionManager
      Dim ltr As LinetypeTableRecord

      Using ta As Transaction = tm.StartTransaction()
        'Create a new linetype
        Using lineTable As LinetypeTable = CType(db.LinetypeTableId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), LinetypeTable)
          ltr = New LinetypeTableRecord()
          ltr.Name = "TestLineType"
          lineTable.Add(ltr)
          ltr.PatternLength = 0.1
          ltr.NumDashes = 9
          ltr.SetDashLengthAt(0, 0.2)
          ltr.SetDashLengthAt(1, -0.2)
        End Using

        'Get the current space block
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)

          'Aligned dimensions
          Dim alDim1 As New AlignedDimension(New Point3d(0, 0, 0), New Point3d(3, 1, 0), New Point3d(1, 2, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(alDim1)
          alDim1.SetDatabaseDefaults()

          Dim alDim2 As New AlignedDimension()
          btr.AppendEntity(alDim2)
          alDim2.CopyFrom(alDim1)
          alDim2.Oblique = 4 * Math.PI / 9
          alDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          alDim2.UsingDefaultTextPosition = False
          alDim2.Dimtmove = 1
          alDim2.TextPosition = New Point3d(2, 4, 0)

          Dim alDim3 As New AlignedDimension
          btr.AppendEntity(alDim3)
          alDim3.CopyFrom(alDim1)
          alDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          alDim3.UsingDefaultTextPosition = False
          alDim3.TextPosition = New Point3d(4.5, 9, 0)
          alDim3.Dimtoh = False
          alDim3.Dimtad = 1
          alDim3.DimensionText = "Distance is <> units"

          Dim alDim4 As New AlignedDimension
          btr.AppendEntity(alDim4)
          alDim4.CopyFrom(alDim1)
          alDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          alDim4.Dimtmove = 0
          alDim4.Dimtih = False
          alDim4.Dimtad = 1
          alDim4.Dimjust = 4

          Dim alDim5 As New AlignedDimension
          btr.AppendEntity(alDim5)
          alDim5.CopyFrom(alDim1)
          alDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          alDim5.Dimgap = -0.2
          alDim5.Dimsah = True
          alDim5.Dimblk1s = "Dot"
          alDim5.Dimblk2s = "Oblique"
          alDim5.Dimdle = 0.1
          alDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          alDim5.Dimltex1 = ltr.ObjectId

          Dim alDim6 As New AlignedDimension()
          btr.AppendEntity(alDim6)
          alDim6.CopyFrom(alDim1)
          alDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          alDim6.Dimtol = True
          alDim6.Dimtp = 0.02
          alDim6.Dimtm = 0.02

          'Rotated dimensions
          Dim rotDim1 As New RotatedDimension(-0.2, New Point3d(7, 0, 0), New Point3d(10, 1, 0), New Point3d(8, 2, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(rotDim1)
          alDim1.SetDatabaseDefaults()

          Dim rotDim2 As New RotatedDimension
          btr.AppendEntity(rotDim2)
          rotDim2.CopyFrom(rotDim1)
          rotDim2.Oblique = 4 * Math.PI / 9
          rotDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          rotDim2.UsingDefaultTextPosition = False
          rotDim2.Dimtmove = 1
          rotDim2.TextPosition = New Point3d(9, 4, 0)

          Dim rotDim3 As New RotatedDimension
          btr.AppendEntity(rotDim3)
          rotDim3.CopyFrom(rotDim1)
          rotDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          rotDim3.UsingDefaultTextPosition = False
          rotDim3.TextPosition = New Point3d(11.5, 9, 0)
          rotDim3.Dimtoh = False
          rotDim3.Dimtad = 1
          rotDim3.DimensionText = "Distance is <> units"

          Dim rotDim4 As New RotatedDimension
          btr.AppendEntity(rotDim4)
          rotDim4.CopyFrom(rotDim1)
          rotDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          rotDim4.Dimtmove = 0
          rotDim4.Dimtih = False
          rotDim4.Dimtad = 1
          rotDim4.Dimjust = 4

          Dim rotDim5 As New RotatedDimension
          btr.AppendEntity(rotDim5)
          rotDim5.CopyFrom(rotDim1)
          rotDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          rotDim5.Dimgap = -0.2
          rotDim5.Dimsah = True
          rotDim5.Dimblk1s = "Dot"
          rotDim5.Dimblk2s = "Oblique"
          rotDim5.Dimdle = 0.1
          rotDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          rotDim5.Dimltex1 = ltr.ObjectId
          rotDim5.Dimlwe = LineWeight.LineWeight080

          Dim rotDim6 As New RotatedDimension
          btr.AppendEntity(rotDim6)
          rotDim6.CopyFrom(rotDim1)
          rotDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          rotDim6.Dimtol = True
          rotDim6.Dimtp = 0.02
          rotDim6.Dimtm = 0.02

          'Two-Line Angular dimensions
          Dim lineAngularDim1 As New LineAngularDimension2(New Point3d(16, 0, 0), New Point3d(16.5, 1, 0), New Point3d(15, 0, 0), New Point3d(14.5, 1, 0), New Point3d(15.5, 2, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(lineAngularDim1)
          lineAngularDim1.SetDatabaseDefaults()

          Dim lineAngularDim2 As New LineAngularDimension2
          btr.AppendEntity(lineAngularDim2)
          lineAngularDim2.CopyFrom(lineAngularDim1)
          lineAngularDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          lineAngularDim2.UsingDefaultTextPosition = False
          lineAngularDim2.Dimtmove = 1
          lineAngularDim2.TextPosition = New Point3d(16, 4, 0)

          Dim lineAngularDim3 As New LineAngularDimension2
          btr.AppendEntity(lineAngularDim3)
          lineAngularDim3.CopyFrom(lineAngularDim1)
          lineAngularDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          lineAngularDim3.UsingDefaultTextPosition = False
          lineAngularDim3.TextPosition = New Point3d(18.5, 9, 0)
          lineAngularDim3.Dimtoh = False
          lineAngularDim3.Dimtad = 1
          lineAngularDim3.DimensionText = "Angle is <> units"

          Dim lineAngularDim4 As New LineAngularDimension2
          btr.AppendEntity(lineAngularDim4)
          lineAngularDim4.CopyFrom(lineAngularDim1)
          lineAngularDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          lineAngularDim4.Dimtmove = 0
          lineAngularDim4.Dimtih = False
          lineAngularDim4.Dimtad = 1
          lineAngularDim4.Dimjust = 4

          Dim lineAngularDim5 As New LineAngularDimension2
          btr.AppendEntity(lineAngularDim5)
          lineAngularDim5.CopyFrom(lineAngularDim1)
          lineAngularDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          lineAngularDim5.Dimgap = -0.2
          lineAngularDim5.Dimsah = True
          lineAngularDim5.Dimblk1s = "Dot"
          lineAngularDim5.Dimblk2s = "Oblique"
          lineAngularDim5.Dimdle = 0.1
          lineAngularDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          lineAngularDim5.Dimltex1 = ltr.ObjectId
          lineAngularDim5.Dimlwe = LineWeight.LineWeight080

          Dim lineAngularDim6 As New LineAngularDimension2
          btr.AppendEntity(lineAngularDim6)
          lineAngularDim6.CopyFrom(lineAngularDim1)
          lineAngularDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          lineAngularDim6.Dimtol = True
          lineAngularDim6.Dimtp = 0.02
          lineAngularDim6.Dimtm = 0.02

          'Three-Point Angular dimensions
          Dim pointAngularDim1 As New Point3AngularDimension(New Point3d(22.5, -1, 0), New Point3d(23.5, 1, 0), New Point3d(21.5, 1, 0), New Point3d(22.5, 2, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(pointAngularDim1)
          pointAngularDim1.SetDatabaseDefaults()

          Dim pointAngularDim2 As New Point3AngularDimension
          btr.AppendEntity(pointAngularDim2)
          pointAngularDim2.CopyFrom(pointAngularDim1)
          pointAngularDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          pointAngularDim2.UsingDefaultTextPosition = False
          pointAngularDim2.Dimtmove = 1
          pointAngularDim2.TextPosition = New Point3d(23, 4, 0)

          Dim pointAngularDim3 As New Point3AngularDimension
          btr.AppendEntity(pointAngularDim3)
          pointAngularDim3.CopyFrom(pointAngularDim1)
          pointAngularDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          pointAngularDim3.UsingDefaultTextPosition = False
          pointAngularDim3.TextPosition = New Point3d(25.5, 9, 0)
          pointAngularDim3.Dimtoh = False
          pointAngularDim3.Dimtad = 1
          pointAngularDim3.DimensionText = "Angle is <> units"

          Dim pointAngularDim4 As New Point3AngularDimension
          btr.AppendEntity(pointAngularDim4)
          pointAngularDim4.CopyFrom(pointAngularDim1)
          pointAngularDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          pointAngularDim4.Dimtmove = 0
          pointAngularDim4.Dimtih = False
          pointAngularDim4.Dimtad = 1
          pointAngularDim4.Dimjust = 4

          Dim pointAngularDim5 As New Point3AngularDimension
          btr.AppendEntity(pointAngularDim5)
          pointAngularDim5.CopyFrom(pointAngularDim1)
          pointAngularDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          pointAngularDim5.Dimgap = -0.2
          pointAngularDim5.Dimsah = True
          pointAngularDim5.Dimblk1s = "Dot"
          pointAngularDim5.Dimblk2s = "Oblique"
          pointAngularDim5.Dimdle = 0.1
          pointAngularDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          pointAngularDim5.Dimltex1 = ltr.ObjectId
          pointAngularDim5.Dimlwe = LineWeight.LineWeight080

          Dim pointAngularDim6 As New Point3AngularDimension
          btr.AppendEntity(pointAngularDim6)
          pointAngularDim6.CopyFrom(pointAngularDim1)
          pointAngularDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          pointAngularDim6.Dimtol = True
          pointAngularDim6.Dimtp = 0.02
          pointAngularDim6.Dimtm = 0.02

          ' Arc Length dimensions
          Dim arcDim1 As New ArcDimension(New Point3d(29.5, -1, 0), New Point3d(30.5, -0.5, 0), New Point3d(28.5, -0.5, 0), New Point3d(29.5, 2, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim1)
          arcDim1.SetDatabaseDefaults()
          arcDim1.Dimarcsym = 2

          Dim arcDim2 As New ArcDimension(New Point3d(29.5, 2, 0), New Point3d(30.5, 2.5, 0), New Point3d(28.5, 2.5, 0), New Point3d(29.5, 5, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim2)
          arcDim1.SetDatabaseDefaults()
          arcDim2.UsingDefaultTextPosition = False
          arcDim2.Dimtmove = 1
          arcDim2.TextPosition = New Point3d(30, 4, 0)

          Dim arcDim3 As New ArcDimension(New Point3d(29.5, 5, 0), New Point3d(30.5, 5.5, 0), New Point3d(28.5, 5.5, 0), New Point3d(29.5, 8, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim3)
          arcDim3.SetDatabaseDefaults()
          arcDim3.UsingDefaultTextPosition = False
          arcDim3.TextPosition = New Point3d(32, 9, 0)
          arcDim3.Dimtoh = False
          arcDim3.Dimtad = 1
          arcDim3.DimensionText = "Arc length is <> units"

          Dim arcDim4 As New ArcDimension(New Point3d(29.5, 8, 0), New Point3d(30.5, 8.5, 0), New Point3d(28.5, 8.5, 0), New Point3d(29.5, 11, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim4)
          arcDim4.SetDatabaseDefaults()
          arcDim4.Dimtmove = 0
          arcDim4.Dimtih = False
          arcDim4.Dimtad = 1
          arcDim4.Dimjust = 4

          Dim arcDim5 As New ArcDimension(New Point3d(29.5, 11, 0), New Point3d(30.5, 11.5, 0), New Point3d(28.5, 11.5, 0), New Point3d(29.5, 14, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim5)
          arcDim5.SetDatabaseDefaults()
          arcDim5.Dimgap = -0.2
          arcDim5.Dimsah = True
          arcDim5.Dimblk1s = "Dot"
          arcDim5.Dimblk2s = "Oblique"
          arcDim5.Dimdle = 0.1
          arcDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          arcDim5.Dimltex1 = ltr.ObjectId
          arcDim5.Dimlwe = LineWeight.LineWeight080

          Dim arcDim6 As New ArcDimension(New Point3d(29.5, 14, 0), New Point3d(30.5, 14.5, 0), New Point3d(28.5, 14.5, 0), New Point3d(29.5, 17, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(arcDim6)
          arcDim6.SetDatabaseDefaults()
          arcDim6.Dimtol = True
          arcDim6.Dimtp = 0.02
          arcDim6.Dimtm = 0.02

          'Radial dimensions
          Dim radDim1 As New RadialDimension(New Point3d(37, 0, 0), New Point3d(40, 1, 0), 1, String.Empty, db.Dimstyle)
          btr.AppendEntity(radDim1)
          radDim1.SetDatabaseDefaults()

          Dim radDim2 As New RadialDimension
          btr.AppendEntity(radDim2)
          radDim2.CopyFrom(radDim1)
          radDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          radDim2.UsingDefaultTextPosition = False
          radDim2.Dimtmove = 1
          radDim2.TextPosition = New Point3d(37, 4, 0)

          Dim radDim3 As New RadialDimension
          btr.AppendEntity(radDim3)
          radDim3.CopyFrom(radDim1)
          radDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          radDim3.UsingDefaultTextPosition = False
          radDim3.TextPosition = New Point3d(39, 9, 0)
          radDim3.Dimtoh = False
          radDim3.Dimtad = 1
          radDim3.DimensionText = "Radius is <> units"

          Dim radDim4 As New RadialDimension
          btr.AppendEntity(radDim4)
          radDim4.CopyFrom(radDim1)
          radDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          radDim4.Dimtmove = 0
          radDim4.Dimtad = 1

          Dim radDim5 As New RadialDimension
          btr.AppendEntity(radDim5)
          radDim5.CopyFrom(radDim1)
          radDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          radDim5.Dimgap = -0.2
          radDim5.Dimblks = "Dot"
          radDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          radDim5.Dimltype = ltr.ObjectId
          radDim5.Dimlwd = LineWeight.LineWeight020
          radDim5.Dimtofl = True

          Dim radDim6 As New RadialDimension
          btr.AppendEntity(radDim6)
          radDim6.CopyFrom(radDim1)
          radDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          radDim6.Dimtol = True
          radDim6.Dimtp = 0.02
          radDim6.Dimtm = 0.02

          'Diametric dimensions
          Dim diamDim1 As New DiametricDimension(New Point3d(50, 1, 0), New Point3d(44, -1, 0), 1, String.Empty, db.Dimstyle)
          btr.AppendEntity(diamDim1)
          diamDim1.SetDatabaseDefaults()

          Dim diamDim2 As New DiametricDimension()
          btr.AppendEntity(diamDim2)
          diamDim2.CopyFrom(diamDim1)
          diamDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          diamDim2.UsingDefaultTextPosition = False
          diamDim2.Dimtmove = 1
          diamDim2.TextPosition = New Point3d(47, 4, 0)

          Dim diamDim3 As New DiametricDimension()
          btr.AppendEntity(diamDim3)
          diamDim3.CopyFrom(diamDim1)
          diamDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          diamDim3.UsingDefaultTextPosition = False
          diamDim3.TextPosition = New Point3d(52, 9, 0)
          diamDim3.Dimtoh = False
          diamDim3.Dimtad = 1
          diamDim3.DimensionText = "Diameter is <> units"

          Dim diamDim4 As New DiametricDimension()
          btr.AppendEntity(diamDim4)
          diamDim4.CopyFrom(diamDim1)
          diamDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          diamDim4.Dimtmove = 0
          diamDim4.Dimtad = 1

          Dim diamDim5 As New DiametricDimension()
          btr.AppendEntity(diamDim5)
          diamDim5.CopyFrom(diamDim1)
          diamDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          diamDim5.Dimgap = -0.2
          diamDim5.Dimblks = "Dot"
          diamDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          diamDim5.Dimltype = ltr.ObjectId
          diamDim5.Dimlwd = LineWeight.LineWeight020
          diamDim5.Dimtofl = True

          Dim diamDim6 As New DiametricDimension()
          btr.AppendEntity(diamDim6)
          diamDim6.CopyFrom(diamDim1)
          diamDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          diamDim6.Dimtol = True
          diamDim6.Dimtp = 0.02
          diamDim6.Dimtm = 0.02

          'Radial large dimensions
          Dim radLargeDim1 As New RadialDimensionLarge(New Point3d(57, 0, 0), New Point3d(60, 0, 0), New Point3d(62, 2, 0), New Point3d(61, 0.5, 0), Math.PI / 4, String.Empty, db.Dimstyle)
          btr.AppendEntity(radLargeDim1)
          radLargeDim1.SetDatabaseDefaults()

          Dim radLargeDim2 As New RadialDimensionLarge
          btr.AppendEntity(radLargeDim2)
          radLargeDim2.CopyFrom(radLargeDim1)
          radLargeDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          radLargeDim2.UsingDefaultTextPosition = False
          radLargeDim2.TextPosition = New Point3d(59, 4, 0)
          radLargeDim2.Dimtmove = 0
          radLargeDim2.Dimtih = True

          Dim radLargeDim3 As New RadialDimensionLarge
          btr.AppendEntity(radLargeDim3)
          radLargeDim3.CopyFrom(radLargeDim1)
          radLargeDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          radLargeDim3.ChordPoint = New Point3d(58, 7, 0)
          radLargeDim3.JogPoint = New Point3d(60, 7, 0)
          radLargeDim3.Dimtad = 1
          radLargeDim3.Dimtih = False
          radLargeDim3.DimensionText = "Radius is <> units"

          Dim radLargeDim4 As New RadialDimensionLarge
          btr.AppendEntity(radLargeDim4)
          radLargeDim4.CopyFrom(radLargeDim1)
          radLargeDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          radLargeDim4.Dimtmove = 0
          radLargeDim4.Dimtad = 1

          Dim radLargeDim5 As New RadialDimensionLarge
          btr.AppendEntity(radLargeDim5)
          radLargeDim5.CopyFrom(radLargeDim1)
          radLargeDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          radLargeDim5.Dimgap = -0.2
          radLargeDim5.Dimblks = "Dot"
          radLargeDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          radLargeDim5.Dimltype = ltr.ObjectId
          radLargeDim5.Dimlwd = LineWeight.LineWeight020
          radLargeDim5.Dimtofl = False
          radLargeDim5.JogAngle = Math.PI / 10

          Dim radLargeDim6 As New RadialDimensionLarge
          btr.AppendEntity(radLargeDim6)
          radLargeDim6.CopyFrom(radLargeDim1)
          radLargeDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          radLargeDim6.Dimtol = True
          radLargeDim6.Dimtp = 0.02
          radLargeDim6.Dimtm = 0.02

          'Ordinate dimensions
          Dim ordDim1 As New OrdinateDimension(True, New Point3d(65, 0, 0), New Point3d(66, 3, 0), String.Empty, db.Dimstyle)
          btr.AppendEntity(ordDim1)

          Dim ordDim2 As New OrdinateDimension
          btr.AppendEntity(ordDim2)
          ordDim2.CopyFrom(ordDim1)
          ordDim2.UsingXAxis = False
          ordDim2.DefiningPoint = New Point3d(65, 3, 0)
          ordDim2.TransformBy(Matrix3d.Displacement(New Vector3d(0, 3, 0)))
          ordDim2.UsingDefaultTextPosition = False
          ordDim2.Dimtmove = 1
          ordDim2.TextPosition = New Point3d(68, 4, 0)

          Dim ordDim3 As New OrdinateDimension
          btr.AppendEntity(ordDim3)
          ordDim3.CopyFrom(ordDim1)
          ordDim3.TransformBy(Matrix3d.Displacement(New Vector3d(0, 6, 0)))
          ordDim3.UsingDefaultTextPosition = False
          ordDim3.TextPosition = New Point3d(70, 9, 0)
          ordDim3.Dimtoh = False
          ordDim3.Dimtad = 1
          ordDim3.DimensionText = "Distance is <> units"

          Dim ordDim4 As New OrdinateDimension
          btr.AppendEntity(ordDim4)
          ordDim4.CopyFrom(ordDim1)
          ordDim4.TransformBy(Matrix3d.Displacement(New Vector3d(0, 9, 0)))
          ordDim4.Dimtmove = 0
          ordDim4.Dimtih = False
          ordDim4.Dimtad = 1
          ordDim4.Dimjust = 4

          Dim ordDim5 As New OrdinateDimension
          btr.AppendEntity(ordDim5)
          ordDim5.CopyFrom(ordDim1)
          ordDim5.TransformBy(Matrix3d.Displacement(New Vector3d(0, 12, 0)))
          ordDim5.Dimgap = -0.2
          ordDim5.Dimsah = True
          ordDim5.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 1)
          ordDim5.Dimltex2 = ltr.ObjectId

          Dim ordDim6 As New OrdinateDimension
          btr.AppendEntity(ordDim6)
          ordDim6.CopyFrom(ordDim1)
          ordDim6.TransformBy(Matrix3d.Displacement(New Vector3d(0, 15, 0)))
          ordDim6.Dimtol = True
          ordDim6.Dimtp = 0.02
          ordDim6.Dimtm = 0.02
        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "DimEx.dwg", DwgVersion.Current)
    End Using
  End Sub
End Class

