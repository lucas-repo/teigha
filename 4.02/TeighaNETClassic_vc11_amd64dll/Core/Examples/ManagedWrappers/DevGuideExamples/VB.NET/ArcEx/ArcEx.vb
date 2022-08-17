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
Public Class ArcEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          Dim arc1 As Arc = New Arc()
          btr.AppendEntity(arc1)
          arc1.Radius = 1
          arc1.StartAngle = 0.0
          arc1.EndAngle = Math.PI
          arc1.Thickness = 1

          Dim arc2 As Arc = New Arc(Point3d.Origin, 1.5, Math.PI / 4, 5 * Math.PI / 4)
          btr.AppendEntity(arc2)

          Dim arc3 As Arc = New Arc(Point3d.Origin, New Vector3d(0, 0, -1), 2.0, 0.0, Math.PI / 2)
          btr.AppendEntity(arc3)
          arc3.Thickness = 0.5

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "ArcEx.dwg", DwgVersion.Current)
    End Using
  End Sub
End Class
