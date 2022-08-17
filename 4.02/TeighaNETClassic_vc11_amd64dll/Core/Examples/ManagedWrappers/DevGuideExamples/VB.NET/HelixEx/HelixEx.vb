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
Public Class HelixEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          REM Creates an expanding helix (BaseRadius < TopRadius)
          Dim helix1 As Helix = New Helix()
          btr.AppendEntity(helix1)
          helix1.AxisVector = Vector3d.YAxis
          helix1.StartPoint = New Point3d(0.5, 0.0, 0)
          helix1.SetAxisPoint(New Point3d(0.0, 0.0, 0), False)
          helix1.Constrain = ConstrainType.Height
          helix1.TopRadius = 2
          helix1.Height = 7.0
          helix1.Turns = 4.0
          helix1.CreateHelix()

          REM Creates a helix converging to a point (TopRadius == 0)
          Dim helix2 As Helix = New Helix()
          btr.AppendEntity(helix2)
          helix2.AxisVector = New Vector3d(0, 1, 0)
          helix2.StartPoint = New Point3d(-8.0, 0.0, 0)
          helix2.SetAxisPoint(New Point3d(-10.0, 0.0, 0), False)
          helix2.TopRadius = 0
          helix2.Constrain = ConstrainType.Height
          helix2.Height = 7.0
          helix2.Turns = 4.0
          helix2.CreateHelix()

          REM Creates a narrowing helix (BaseRadius > TopRadius)
          Dim helix3 As Helix = New Helix()
          btr.AppendEntity(helix3)
          helix3.CopyFrom(helix2)
          helix3.StartPoint = New Point3d(-18.0, 0.0, 0)
          helix3.SetAxisPoint(New Point3d(-20.0, 0.0, 0), False)
          helix3.TopRadius = 0.5
          helix3.CreateHelix()

          REM Creates a cylindrical helix (BaseRadius == TopRadius)
          Dim helix4 As Helix = New Helix()
          btr.AppendEntity(helix4)
          helix4.CopyFrom(helix1)
          helix4.StartPoint = New Point3d(12.0, 0.0, 0)
          helix4.SetAxisPoint(New Point3d(10.0, 0.0, 0), False)
          helix4.TopRadius = 2
          helix4.CreateHelix()

          REM Creates a helix diverging from a point (BaseRadius == 0)
          Dim helix5 As Helix = New Helix()
          btr.AppendEntity(helix5)
          helix5.CopyFrom(helix1)
          helix5.SetAxisPoint(New Point3d(20.0, 0.0, 0), False)
          helix5.BaseRadius = 0.0
          helix5.TopRadius = 2
          helix5.CreateHelix()

          REM Creates a helix with the Height of 4
          Dim helix6 As Helix = New Helix()
          btr.AppendEntity(helix6)
          helix6.AxisVector = Vector3d.YAxis
          helix6.TopRadius = 2
          helix6.StartPoint = New Point3d(-18.0, 10.0, 0)
          helix6.SetAxisPoint(New Point3d(-20.0, 10.0, 0), False)
          helix6.Turns = 3
          helix6.Constrain = ConstrainType.Height
          helix6.Height = 4
          helix6.CreateHelix()

          REM Creates a helix with the Height changed to 7 with ConstrainType.TurnHeight
          Dim helix7 As Helix = New Helix()
          btr.AppendEntity(helix7)
          helix7.CopyFrom(helix6)
          helix7.StartPoint = New Point3d(-12.0, 10.0, 0)
          helix7.SetAxisPoint(New Point3d(-14.0, 10.0, 0), False)
          helix7.Turns = 3
          helix7.Constrain = ConstrainType.TurnHeight
          helix7.Height = 7
          helix7.CreateHelix()

          REM Creates a helix with the Height changed to 7 with ConstrainType.Turns
          Dim helix8 As Helix = New Helix()
          btr.AppendEntity(helix8)
          helix8.CopyFrom(helix7)
          helix8.StartPoint = New Point3d(-6.0, 10.0, 0)
          helix8.SetAxisPoint(New Point3d(-8.0, 10.0, 0), False)
          helix8.Turns = 3
          helix8.Constrain = ConstrainType.Turns
          helix8.Height = 7
          helix8.CreateHelix()

          REM Creates a helix with the TurnHeight changed to 2 with ConstrainType.Turns
          Dim helix9 As Helix = New Helix()
          btr.AppendEntity(helix9)
          helix9.CopyFrom(helix6)
          helix9.StartPoint = New Point3d(-0.0, 10.0, 0)
          helix9.SetAxisPoint(New Point3d(-2.0, 10.0, 0), False)
          helix9.Constrain = ConstrainType.Turns
          helix9.TurnHeight = 2
          helix9.CreateHelix()

          REM Creates a helix with the TurnHeight changed to 2 with ConstrainType.TurnHeight
          Dim helix10 As Helix = New Helix()
          btr.AppendEntity(helix10)
          helix10.CopyFrom(helix6)
          helix10.StartPoint = New Point3d(6.0, 10.0, 0)
          helix10.SetAxisPoint(New Point3d(4.0, 10.0, 0), False)
          helix10.Constrain = ConstrainType.TurnHeight
          helix10.TurnHeight = 2
          helix10.CreateHelix()

          REM Creates a helix with clockwise turns direction 
          Dim helix11 As Helix = New Helix()
          btr.AppendEntity(helix11)
          helix11.CopyFrom(helix6)
          helix11.StartPoint = New Point3d(-18.0, 20.0, 0)
          helix11.SetAxisPoint(New Point3d(-20.0, 20.0, 0), False)
          helix11.Twist = False
          helix11.CreateHelix()

          REM Creates a helix with the Turns changed to 5 with ConstrainType.TurnHeight
          Dim helix12 As Helix = New Helix()
          btr.AppendEntity(helix12)
          helix12.CopyFrom(helix11)
          helix12.StartPoint = New Point3d(-12.0, 20.0, 0)
          helix12.SetAxisPoint(New Point3d(-14.0, 20.0, 0), False)
          helix12.Constrain = ConstrainType.TurnHeight
          helix12.Turns = 5
          helix12.CreateHelix()

          REM Creates a helix with the Turns changed to 5 with ConstrainType.Height
          Dim helix13 As Helix = New Helix()
          btr.AppendEntity(helix13)
          helix13.CopyFrom(helix11)
          helix13.StartPoint = New Point3d(-6.0, 20.0, 0)
          helix13.SetAxisPoint(New Point3d(-8.0, 20.0, 0), False)
          helix13.Constrain = ConstrainType.Height
          helix13.Turns = 5
          helix13.CreateHelix()

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "HelixEx.dwg", DwgVersion.Current)
    End Using
  End Sub
End Class
