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
Public Class CircleEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)

      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          REM Number of circles.
          Const CIRNUM = 10
          REM Circle color from color index.
          Dim color As Short = 1
          REM Circle thickness.
          Dim thickness As Short = 0
          REM Angle for calculating circle center.
          Dim alpha As Double = 0.0
          REM Circle radius.
          Dim radius As Double = 1.0

          Dim i As Integer
          For i = 0 To CIRNUM - 1
            REM Creates a Circle entity and adds it into the Block Table Record.
            Using cir As New Circle()
              btr.AppendEntity(cir)
              REM Sets Circle properties
              cir.Center = New Point3d(Math.Cos(alpha), Math.Sin(alpha), 0)
              cir.Radius = radius
              cir.Thickness = thickness
              cir.Color = Teigha.Colors.Color.FromColorIndex(ColorMethod.ByAci, color)

              thickness += 1
              radius += 0.5
              alpha += 2 * Math.PI / CIRNUM
              color += 1
              If (color = 7) Then
                color = 1
              End If

              REM Print Circle properties.
              Console.WriteLine("Center is: " + cir.Center.ToString())
              Console.WriteLine("Radius is: " + cir.Radius.ToString())
              Console.WriteLine("Circumference is: " + cir.Circumference.ToString())
              Console.WriteLine("Thickness is: " + cir.Thickness.ToString())
              Console.WriteLine("Normal is: " + cir.Normal.ToString() + System.Environment.NewLine)
            End Using
          Next i
          ta.Commit()
        End Using
        db.SaveAs(path + "CircleEx.dwg", DwgVersion.Current)
      End Using
    End Using
  End Sub
End Class
