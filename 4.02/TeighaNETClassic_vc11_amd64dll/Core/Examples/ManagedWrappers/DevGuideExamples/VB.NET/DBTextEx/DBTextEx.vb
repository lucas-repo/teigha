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
Public Class DBTextEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      db.Pdmode = 2

      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)

          'Creates an DBText entity with default settings.
          Using dbText1 As New DBText()
            btr.AppendEntity(dbText1)
            dbText1.TextString = "This is a text with default settings"
          End Using

          'Creates a DBText entity with height of 1.
          Using dbText2 As New DBText()
            btr.AppendEntity(dbText2)
            dbText2.Position = New Point3d(0, -2, 0)
            dbText2.Height = 1
            dbText2.TextString = "This is a text with Height of " + dbText2.Height.ToString()
          End Using

          'Creates a DBText entity with WidthFactor of 2.
          Using dbText3 As New DBText()
            btr.AppendEntity(dbText3)
            dbText3.Position = New Point3d(0, -4, 0)
            dbText3.Height = 0.5
            dbText3.WidthFactor = 2
            dbText3.TextString = "This is a text with Height of " + dbText3.Height.ToString() + " and WidthFactor of " + dbText3.WidthFactor.ToString()
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -6, 0) and TopLeft justify mode.
          Using dbText4 As New DBText()
            btr.AppendEntity(dbText4)
            dbText4.Position = New Point3d(0, -6, 0)
            dbText4.Height = 0.5
            dbText4.AlignmentPoint = New Point3d(10, -6, 0)
            Using alignmentPoint4 As New DBPoint(dbText4.AlignmentPoint)
              btr.AppendEntity(alignmentPoint4)
              alignmentPoint4.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText4.Justify = AttachmentPoint.TopLeft
            dbText4.TextString = "This is a text with Position at " + dbText4.Position.ToString() + ", AlignmentPoint at " + dbText4.AlignmentPoint.ToString() + " and " + dbText4.Justify.ToString() + "  justify mode"
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -8, 0) and TopCenter justify mode.
          Using dbText5 As New DBText()
            btr.AppendEntity(dbText5)
            dbText5.Position = New Point3d(0, -8, 0)
            dbText5.Height = 0.5
            dbText5.AlignmentPoint = New Point3d(10, -8, 0)
            Using alignmentPoint5 As New DBPoint(dbText5.AlignmentPoint)
              btr.AppendEntity(alignmentPoint5)
              alignmentPoint5.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText5.Justify = AttachmentPoint.TopCenter
            dbText5.TextString = "This is a text with Position at " + dbText5.Position.ToString() + ", AlignmentPoint at " + dbText5.AlignmentPoint.ToString() + " and " + dbText5.Justify.ToString() + " mode"
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -10, 0) and TopRight justify mode.
          Using dbText6 As New DBText()
            btr.AppendEntity(dbText6)
            dbText6.Position = New Point3d(0, -10, 0)
            dbText6.Height = 0.5
            dbText6.AlignmentPoint = New Point3d(10, -10, 0)
            Using alignmentPoint6 As New DBPoint(dbText6.AlignmentPoint)
              btr.AppendEntity(alignmentPoint6)
              alignmentPoint6.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText6.Justify = AttachmentPoint.TopRight
            dbText6.TextString = "This is a text with Position at " + dbText6.Position.ToString() + ", AlignmentPoint at " + dbText6.AlignmentPoint.ToString() + " and " + dbText6.Justify.ToString() + " mode"
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -12, 0) and BaseCenter justify mode.
          Using dbText7 As New DBText()
            btr.AppendEntity(dbText7)
            dbText7.Position = New Point3d(0, -12, 0)
            dbText7.Height = 0.5
            dbText7.AlignmentPoint = New Point3d(10, -12, 0)
            Using alignmentPoint7 As New DBPoint(dbText7.AlignmentPoint)
              btr.AppendEntity(alignmentPoint7)
              alignmentPoint7.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText7.Justify = AttachmentPoint.BaseCenter
            dbText7.TextString = "This is a text with Position at " + dbText7.Position.ToString() + ", AlignmentPoint at " + dbText7.AlignmentPoint.ToString() + " and " + dbText7.Justify.ToString() + " mode"
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -14, 0) and BaseAlign justify mode.
          Using dbText8 As New DBText()
            btr.AppendEntity(dbText8)
            dbText8.Position = New Point3d(0, -14, 0)
            dbText8.Height = 0.5
            dbText8.AlignmentPoint = New Point3d(10, -14, 0)
            Using alignmentPoint8 As New DBPoint(dbText8.AlignmentPoint)
              btr.AppendEntity(alignmentPoint8)
              alignmentPoint8.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText8.Justify = AttachmentPoint.BaseAlign
            dbText8.TextString = "This is a text with Position at " + dbText8.Position.ToString() + ", AlignmentPoint at " + dbText8.AlignmentPoint.ToString() + " and " + dbText8.Justify.ToString() + " mode"
          End Using

          'Creates a DBText entity with AlignmentPoint at (10, -16, 0) and BaseFit justify mode.
          Using dbText9 As New DBText()
            btr.AppendEntity(dbText9)
            dbText9.Position = New Point3d(0, -16, 0)
            dbText9.Height = 0.5
            dbText9.AlignmentPoint = New Point3d(10, -16, 0)
            Using alignmentPoint9 As New DBPoint(dbText9.AlignmentPoint)
              btr.AppendEntity(alignmentPoint9)
              alignmentPoint9.Color = Color.FromRgb(255, 0, 0)
            End Using
            dbText9.Justify = AttachmentPoint.BaseFit
            dbText9.TextString = "This is a text with Position at " + dbText9.Position.ToString() + ", AlignmentPoint at " + dbText9.AlignmentPoint.ToString() + " and " + dbText9.Justify.ToString() + " mode"
          End Using

          'Creates a DBText entity with Oblique of -0.7.
          Using dbText10 As New DBText()
            btr.AppendEntity(dbText10)
            dbText10.Position = New Point3d(0, -18, 0)
            dbText10.Height = 0.5
            dbText10.Oblique = -0.7
            dbText10.TextString = "This is a text with oblique angle of " + dbText10.Oblique.ToString()
          End Using

          'Creates a DBText entity with Oblique of 0.
          Using dbText11 As New DBText()
            btr.AppendEntity(dbText11)
            dbText11.Position = New Point3d(0, -20, 0)
            dbText11.Height = 0.5
            dbText11.TextString = "This is a text with oblique angle of " + dbText11.Oblique.ToString()
          End Using

          'Creates a DBText entity with Oblique of 0.7.
          Using dbText12 As New DBText()
            btr.AppendEntity(dbText12)
            dbText12.Position = New Point3d(0, -22, 0)
            dbText12.Height = 0.5
            dbText12.Oblique = 0.7
            dbText12.TextString = "This is a text with oblique angle of " + dbText12.Oblique.ToString()
          End Using

          'Creates a DBText entity with Rotation of -0.7.
          Using dbText13 As New DBText()
            btr.AppendEntity(dbText13)
            dbText13.Position = New Point3d(0, -24, 0)
            dbText13.Height = 0.5
            dbText13.Rotation = -0.7
            dbText13.TextString = "This is a text with rotation angle of " + dbText13.Rotation.ToString()
          End Using

          'Creates a DBText entity with Rotation of 0.0.
          Using dbText14 As New DBText()
            btr.AppendEntity(dbText14)
            dbText14.Position = New Point3d(0, -26, 0)
            dbText14.Height = 0.5
            dbText14.TextString = "This is a text with rotation angle of " + dbText14.Rotation.ToString()
          End Using

          'Creates a DBText entity with Rotation of 0.7.
          Using dbText15 As New DBText()
            btr.AppendEntity(dbText15)
            dbText15.Position = New Point3d(0, -28, 0)
            dbText15.Height = 0.5
            dbText15.Rotation = 0.7
            dbText15.TextString = "This is a text with rotation angle of " + dbText15.Rotation.ToString()
          End Using

          'Creates a DBText entity mirrored in X and Y directions.
          Using dbText16 As New DBText()
            btr.AppendEntity(dbText16)
            dbText16.Position = New Point3d(0, -30, 0)
            dbText16.Height = 0.5
            dbText16.IsMirroredInX = True
            dbText16.IsMirroredInY = True
            dbText16.TextString = "This is a text mirrored in X and Y"
          End Using

          'Creates a DBText entity with thickness of 5.
          Using dbText17 As New DBText()
            btr.AppendEntity(dbText17)
            dbText17.Position = New Point3d(0, -32, 0)
            dbText17.Height = 0.5
            dbText17.Thickness = 5
            dbText17.TextString = "This is a text with thickness of " + dbText17.Thickness.ToString()
          End Using

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "DBTextEx.dwg", DwgVersion.Current)
      db.Dispose()
    End Using
  End Sub
End Class

