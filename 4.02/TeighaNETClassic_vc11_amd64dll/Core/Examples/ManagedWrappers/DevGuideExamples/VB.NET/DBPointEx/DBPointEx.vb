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
Public Class DBPointEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      db.Pdmode = 2

      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)

          Const interval = 1
          Dim i As Integer
          Dim j As Integer

          For i = 0 To 3
            For j = 0 To 3
              'Creates DBPoint entity and adds it into the Block Table Record.
              Using dbPoint = New DBPoint()
                btr.AppendEntity(dbPoint)
                'Sets position for every DBPoint entity.
                dbPoint.Position = New Point3d(interval * j, interval * i, 0)
                'Sets color for every DBPoint entity.
                dbPoint.Color = Color.FromRgb(CType((i * 60), Byte), CType((j * 60), Byte), 200)
                'Sets thickness for every DBPoint entity.
                dbPoint.Thickness = j
                'Sets angle between the OCS x-axis and the x-axis for displaying the point entity for every DBPoint entity.
                dbPoint.EcsRotation = i * 0.314
                ''s current DBPoint entity.
                dbPoint.Dispose()
              End Using

            Next j
          Next i

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "DBPointEx.dwg", DwgVersion.Current)
    End Using
  End Sub
End Class
