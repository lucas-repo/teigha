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
Public Class FaceEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          Dim face1 As Face = New Face(New Point3d(0, 0, -1), New Point3d(0, 2, 0), New Point3d(0, 0, 1), New Point3d(2 * Math.Cos(Math.PI / 10), 2 * Math.Sin(Math.PI / 10), 0), True, True, True, True)
          btr.AppendEntity(face1)
          face1.Color = Color.FromRgb(250, 0, 0)
          Console.WriteLine("face1:")
          printVertices(face1)

          Dim face2 As Face = New Face(New Point3d(0, 0, -1), New Point3d(-2 * Math.Cos(Math.PI / 10), 2 * Math.Sin(Math.PI / 10), 0), New Point3d(0, 0, 1), True, True, False, False)
          btr.AppendEntity(face2)
          face2.Color = Color.FromRgb(0, 250, 0)
          Console.WriteLine("face2:")
          printVertices(face2)

          Dim face3 As Face = New Face()
          btr.AppendEntity(face3)
          face3.SetVertexAt(0, New Point3d(2 * Math.Cos(3 * Math.PI / 10), -2 * Math.Sin(3 * Math.PI / 10), 0))
          face3.SetVertexAt(1, New Point3d(0, 0, 1))
          face3.SetVertexAt(2, New Point3d(0, 0, -1))
          face3.SetVertexAt(3, New Point3d(0, 0, -1))
          face3.Color = Color.FromRgb(0, 0, 250)
          Console.WriteLine("face3:")
          printVertices(face3)

          Dim face4 As Face = New Face()
          btr.AppendEntity(face4)
          face4.SetVertexAt(0, New Point3d(2 * Math.Cos(13 * Math.PI / 10), 2 * Math.Sin(13 * Math.PI / 10), 0))
          face4.SetVertexAt(1, New Point3d(0, 0, 1))
          face4.SetVertexAt(2, New Point3d(0, 0, -1))
          face4.SetVertexAt(3, New Point3d(0, 0, -1))
          face4.MakeEdgeInvisibleAt(1)
          face4.Color = Color.FromRgb(250, 0, 250)
          Console.WriteLine("face4:")
          printVertices(face4)
        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "FaceEx.dwg", DwgVersion.Current)
    End Using
  End Sub

  REM Prints Face entity info 
  Sub printVertices(ByVal face As Face)
    Dim i As Short
    For i = 0 To 3
      Console.WriteLine("vertex " + i + ": " + face.GetVertexAt(i).ToString() + ", edge visibility: " + face.IsEdgeVisibleAt(i).ToString())
      Console.WriteLine(System.Environment.NewLine)
    Next i
  End Sub
End Class
