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
Public Class LineEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          'Number of lines.
          Const lineNum = 10
          'Line color from color index.
          Dim color As Short = 1

          Dim i As Integer
          For i = 0 To lineNum - 1
            'Creates Line entity and adds it into the Block Table Record.
            Using line As New Line()
              btr.AppendEntity(line)
              If (color = 7) Then
                color = 1
              End If
              'Sets the same start point for all Line entity.
              line.StartPoint = New Point3d(0, 0, 0)
              'Sets end point for Line entity depending on its number.
              line.EndPoint = New Point3d((Math.Cos(2 * Math.PI / lineNum * i) * 10), (Math.Sin(2 * Math.PI / lineNum * i) * 10), 0)
              'Sets color from color index for Line entity depending on its number.
              line.Color = Teigha.Colors.Color.FromColorIndex(ColorMethod.ByAci, color)
              color = color + 1
              'Sets Thickness for Line entity depending on its number.
              line.Thickness = CType((i / 10), Double)
              'Prints values of Angle, Delta and Thickness properties.
              Console.WriteLine("Angle is: " + line.Angle.ToString())
              Console.WriteLine("Delta is: " + line.Delta.ToString())
              Console.WriteLine("Thickness is: " + line.Thickness.ToString() + Environment.NewLine)
            End Using
          Next i
          ta.Commit()
        End Using
        db.SaveAs(path + "LineEx.dwg", DwgVersion.Current)
      End Using
    End Using
  End Sub
End Class

