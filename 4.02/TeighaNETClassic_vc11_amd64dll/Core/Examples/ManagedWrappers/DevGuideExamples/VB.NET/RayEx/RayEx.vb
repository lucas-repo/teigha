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
Public Class RayEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)

      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          'Number of rays.
          Const RAYNUM = 10
          'Ray color from color index.
          Dim color As Short = 1

          Dim i As Integer
          For i = 0 To RAYNUM - 1
            'Creates a Ray entity and adds it into the Block Table Record.
            Using ray As New Ray()
              btr.AppendEntity(ray)
              If (color = 7) Then
                color = 1
              End If
              'Sets the same start point for all Ray entities.
              ray.BasePoint = New Point3d(0, 0, 0)
              'Sets end point for a Ray entity depending on its number.
              ray.SecondPoint = New Point3d((Math.Cos(2 * Math.PI / RAYNUM * i) * 10), (Math.Sin(2 * Math.PI / RAYNUM * i) * 10), 0)
              'Sets color for a Ray entity from color index depending on a ray's number.
              ray.Color = Teigha.Colors.Color.FromColorIndex(ColorMethod.ByAci, color)
              color = color + 1
              'Prints values of UnitDir property.
              Console.WriteLine("UniDir is: " + ray.UnitDir.ToString() + System.Environment.NewLine)
            End Using
          Next i
          ta.Commit()
        End Using
        db.SaveAs(path + "RayEx.dwg", DwgVersion.Current)
      End Using
    End Using
  End Sub
End Class

