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
Public Class MlineEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          REM Multiline style
          Dim mlStyle As MlineStyle = New MlineStyle()

          Dim Ray As Ray = New Ray()
          btr.AppendEntity(Ray)
          Ray.Color = Color.FromRgb(100, 100, 100)
          Ray.UnitDir = New Vector3d(1, 0, 0)

          Dim mline1 As Mline = New Mline()
          btr.AppendEntity(mline1)
          mline1.AppendSegment(Point3d.Origin)
          mline1.Justification = MlineJustification.Bottom
          mline1.AppendSegment(New Point3d(2, 2, 0))
          mline1.AppendSegment(New Point3d(4, 0, 0))
          mline1.AppendSegment(New Point3d(6, 6, 0))
          REM mline1.AppendSegment(New Point3d(16, 16, 0))

          mline1.MoveVertexAt(3, New Point3d(6, 2, 0))
          Dim removedVertex As Point3d = New Point3d()

          Console.WriteLine("mline1:")
          printMline(mline1)

          Dim mline2 As Mline = New Mline()
          btr.AppendEntity(mline2)
          mline2.Justification = MlineJustification.Top
          mline2.AppendSegment(New Point3d(8, 0, 0))
          mline2.AppendSegment(New Point3d(10, 2, 0))
          mline2.AppendSegment(New Point3d(12, 0, 0))
          mline2.AppendSegment(New Point3d(14, 2, 0))
          Console.WriteLine("mline2:")
          printMline(mline2)

          REM Sets multiline style
          Using dic As DBDictionary = CType(tm.GetObject(db.MLStyleDictionaryId, Teigha.DatabaseServices.OpenMode.ForWrite), DBDictionary)
            dic("ODA") = mlStyle

            mlStyle.Name = "ODAstyle"
            mlStyle.Description = "123"
            mlStyle.ShowMiters = True
            mlStyle.StartInnerArcs = True
            REM Defines caps shapes
            mlStyle.StartRoundCap = True
            mlStyle.EndSquareCap = True

            REM Set multiline as filled by color
            mlStyle.Filled = True
            mlStyle.FillColor = Teigha.Colors.Color.FromRgb(255, 200, 255)
            Dim color As Teigha.Colors.Color = Teigha.Colors.Color.FromRgb(255, 0, 0)

            REM Adds multiline elements of specific color and offset
            mlStyle.Elements.Add(New MlineStyleElement(0, color, db.ByLayerLinetype), True)
            mlStyle.Elements.Add(New MlineStyleElement(0.5, color, db.ByLayerLinetype), True)
            mlStyle.Elements.Add(New MlineStyleElement(0.7, color, db.ByLayerLinetype), True)
            mlStyle.Elements.Add(New MlineStyleElement(2, color, db.ByLayerLinetype), True)
          End Using

          Dim mline3 As Mline = New Mline()
          btr.AppendEntity(mline3)
          REM Applies created style to the Mline entity
          mline3.Style = mlStyle.Id
          REM Sets caps to be drawn
          mline3.SupressStartCaps = False
          mline3.SupressEndCaps = False
          REM Appends multiline segments
          mline3.AppendSegment(New Point3d(16, 0, 0))
          mline3.AppendSegment(New Point3d(18, 2, 0))
          mline3.AppendSegment(New Point3d(20, 0, 0))
          mline3.AppendSegment(New Point3d(22, 2, 0))

          Console.WriteLine("mline3:")
          printMline(mline3)

          Dim mline4 As Mline = New Mline()
          btr.AppendEntity(mline4)
          mline4.AppendSegment(New Point3d(24, 0, 0))
          mline4.AppendSegment(New Point3d(26, 2, 0))
          mline4.AppendSegment(New Point3d(28, 0, 0))
          mline4.AppendSegment(New Point3d(30, 2, 0))
          mline4.Scale = 0.1
          mline4.IsClosed = True

          Console.WriteLine("mline4:")
          printMline(mline4)

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "MlineEx.dwg", DwgVersion.Current)
    End Using
  End Sub

  REM Prints Mline entity info 
  Sub printMline(ByVal mline As Mline)
    Dim i As Short
    For i = 0 To mline.NumberOfVertices - 1
      Console.WriteLine("vertex" + i.ToString() + ": " + mline.VertexAt(i).ToString())
    Next i
    Console.WriteLine("Justification: " + mline.Justification.ToString())
    Console.WriteLine("IsClosed: " + mline.IsClosed.ToString())
    Console.WriteLine("Style: " + CType(mline.Style.GetObject(Teigha.DatabaseServices.OpenMode.ForRead), MlineStyle).Name + System.Environment.NewLine)
  End Sub

End Class
