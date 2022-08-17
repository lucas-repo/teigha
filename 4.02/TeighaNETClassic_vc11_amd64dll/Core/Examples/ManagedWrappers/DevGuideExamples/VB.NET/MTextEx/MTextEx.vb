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

Public Class MTextEx
  Private Function textFragmentHandler(ByVal frag As MTextFragment, ByVal data As Object) As MTextFragmentCallbackStatus
    CType(data, List(Of MTextFragment)).Add(frag)
    Console.WriteLine("Text content is: " + frag.Text)
    Console.WriteLine("Text color is: " + frag.Color.ColorIndex.ToString())
    Console.WriteLine("Text height is: " + frag.CapsHeight.ToString())
    Console.WriteLine("Oblique angle is: " + frag.ObliqueAngle.ToString())
    Console.WriteLine("Font name is: " + frag.TrueTypeFont.ToString())
    Console.WriteLine()
    Return MTextFragmentCallbackStatus.Continue
  End Function


  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)

      db.Pdmode = 2
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)

          'Creates an MText entity contains text with different formatting options applied to single words or characters 
          'within a paragraph or a text section.
          Using mtext1 As New MText()
            btr.AppendEntity(mtext1)
            mtext1.TextHeight = 0.2
            mtext1.Contents = "This is a multiline text with" + MText.ParagraphBreak + "paragraph breaks, " + MText.ParagraphBreak + MText.BlockBegin + MText.ObliqueChange + "12;an oblique text, " + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + MText.OverlineOn + "an overlined text, " + MText.OverlineOff + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + MText.UnderlineOn + "an underlined text, " + MText.UnderlineOff + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + MText.WidthChange + "2;a text with the changed width" + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + MText.HeightChange + "0.5;a text with the changed height" + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + "a text " + MText.ColorChange + "10;in " + MText.ColorChange + "100;different " + MText.ColorChange + "200;colors, " + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + "a text " + MText.FontChange + "Times New Roman;in different " + MText.FontChange + "Tahoma;fonts" + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + "a stacked text with different allignments: " + MText.AlignChange + "0;1" + MText.StackStart + "1/2;  " + MText.AlignChange + "1;1" + MText.StackStart + "1/2;  " + MText.AlignChange + "2;1" + MText.StackStart + "1/2;" + MText.BlockEnd + MText.ParagraphBreak + MText.BlockBegin + "a text " + MText.TrackChange + "2;with the changed space" + MText.TrackChange + "0.4;between characters" + MText.BlockEnd + MText.ParagraphBreak
            mtext1.Location = New Point3d(0, 0, 0)

            'Explodes mtext1 entity into a sequence of text fragments, passing each fragment to the MTextDelegateFunct function 
            'and stores each fragment in a list.
            Dim aFrgm As New List(Of MTextFragment)
            mtext1.ExplodeFragments(New MTextFragmentCallback(AddressOf textFragmentHandler), aFrgm)
          End Using

          'Creates an MText entity with text flowing from top to bottom.
          Using mtext2 As New MText()
            btr.AppendEntity(mtext2)
            mtext2.TextHeight = 0.2
            mtext2.Location = New Point3d(-2.0, 0.0, 0.0)
            mtext2.FlowDirection = FlowDirection.TopToBottom
            mtext2.Contents = "This is a multiline text with FlowDirection of " + mtext2.FlowDirection.ToString()
          End Using

          'Creates an MText entity with LineSpacingFactor of 2.
          Using mtext3 As New MText()
            btr.AppendEntity(mtext3)
            mtext3.TextHeight = 0.2
            mtext3.Width = 2
            mtext3.Location = New Point3d(16.0, 0.0, 0.0)
            mtext3.LineSpacingFactor = 2
            mtext3.Contents = "This is a multiline text with LineSpacingFactor of " + mtext3.LineSpacingFactor.ToString()
          End Using

          'Creates an MText entity with LineSpacingFactor of 0.8.
          Using mtext4 As New MText()
            btr.AppendEntity(mtext4)
            mtext4.TextHeight = 0.2
            mtext4.Width = 2
            mtext4.Location = New Point3d(22.0, 0.0, 0.0)
            mtext4.LineSpacingFactor = 0.8
            mtext4.Contents = "This is a multiline text with LineSpacingFactor of " + mtext4.LineSpacingFactor.ToString()
          End Using

          'Creates an MText entity with AtLeast value of LineSpacingStyle.
          Using mtext5 As New MText()
            btr.AppendEntity(mtext5)
            mtext5.Location = New Point3d(28, 0.0, 0.0)
            mtext5.TextHeight = 0.2
            mtext5.Width = 2
            mtext5.LineSpacingStyle = LineSpacingStyle.AtLeast
            mtext5.Contents = "This is a multiline text with LineSpacingStyle of " + MText.HeightChange + "0.6;" + mtext5.LineSpacingStyle.ToString()
          End Using

          'Creates an MText entity with Exactly value of LineSpacingStyle.
          Using mtext6 As New MText()
            btr.AppendEntity(mtext6)
            mtext6.TextHeight = 0.2
            mtext6.Width = 2
            mtext6.Location = New Point3d(34.0, 0.0, 0.0)
            mtext6.LineSpacingStyle = LineSpacingStyle.Exactly
            mtext6.Contents = "This is a multiline text with LineSpacingStyle of " + MText.HeightChange + "0.6;" + mtext6.LineSpacingStyle.ToString()
          End Using

          'Creates an MText entity with default BackgroundScaleFactor.
          Using mtext7 As New MText()
            btr.AppendEntity(mtext7)
            mtext7.TextHeight = 0.2
            mtext7.Width = 4
            mtext7.Location = New Point3d(40, 0.0, 0.0)
            mtext7.BackgroundFill = True
            mtext7.BackgroundFillColor = Color.FromRgb(250, 200, 250)
            mtext7.Contents = "This is a multiline text with BackgroundScaleFactor of " + mtext7.BackgroundScaleFactor.ToString()
          End Using

          'Creates an MText entity with BackgroundScaleFactor of 5.
          Using mtext8 As New MText()
            btr.AppendEntity(mtext8)
            mtext8.TextHeight = 0.2
            mtext8.Width = 4
            mtext8.Location = New Point3d(46, 0.0, 0.0)
            mtext8.BackgroundFill = True
            mtext8.BackgroundFillColor = Color.FromRgb(250, 200, 250)
            mtext8.BackgroundScaleFactor = 5
            mtext8.Contents = "This is a multiline text with BackgroundScaleFactor of " + mtext8.BackgroundScaleFactor.ToString()
          End Using

          'Creates an MText entity with TopLeft attachment point.
          Using mtext9 As New MText()
            btr.AppendEntity(mtext9)
            mtext9.TextHeight = 0.2
            mtext9.Width = 3
            mtext9.Location = New Point3d(0, -10, 0)
            mtext9.Attachment = AttachmentPoint.TopLeft
            mtext9.Contents = "This is a multiline text with " + mtext9.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint9 As New DBPoint(mtext9.Location)
              btr.AppendEntity(attPoint9)
            End Using
          End Using

          'Creates an MText entity with TopCenter attachment point.
          Using mtext10 As New MText()
            btr.AppendEntity(mtext10)
            mtext10.TextHeight = 0.2
            mtext10.Width = 3
            mtext10.Location = New Point3d(5, -10, 0)
            mtext10.Attachment = AttachmentPoint.TopCenter
            mtext10.Contents = "This is a multiline text with " + mtext10.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint10 As New DBPoint(mtext10.Location)
              btr.AppendEntity(attPoint10)
            End Using
          End Using

          'Creates an MText entity with TopRight attachment point.
          Using mtext11 As New MText()
            btr.AppendEntity(mtext11)
            mtext11.TextHeight = 0.2
            mtext11.Width = 3
            mtext11.Location = New Point3d(10, -10, 0)
            mtext11.Attachment = AttachmentPoint.TopRight
            mtext11.Contents = "This is a multiline text with " + mtext11.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint11 As New DBPoint(mtext11.Location)
              btr.AppendEntity(attPoint11)
            End Using
          End Using

          'Creates an MText entity with MiddleLeft attachment point.
          Using mtext12 As New MText()
            btr.AppendEntity(mtext12)
            mtext12.TextHeight = 0.2
            mtext12.Width = 3
            mtext12.Location = New Point3d(0, -15, 0)
            mtext12.Attachment = AttachmentPoint.MiddleLeft
            mtext12.Contents = "This is a multiline text with " + mtext12.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint12 As New DBPoint(mtext12.Location)
              btr.AppendEntity(attPoint12)
            End Using
          End Using

          'Creates an MText entity with MiddleCenter attachment point.
          Using mtext13 As New MText()
            btr.AppendEntity(mtext13)
            mtext13.TextHeight = 0.2
            mtext13.Width = 3
            mtext13.Location = New Point3d(5, -15, 0)
            mtext13.Attachment = AttachmentPoint.MiddleCenter
            mtext13.Contents = "This is a multiline text with " + mtext13.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint13 As New DBPoint(mtext13.Location)
              btr.AppendEntity(attPoint13)
            End Using
          End Using

          'Creates an MText entity with MiddleRight attachment point.
          Using mtext14 As New MText()
            btr.AppendEntity(mtext14)
            mtext14.TextHeight = 0.2
            mtext14.Width = 3
            mtext14.Location = New Point3d(10, -15, 0)
            mtext14.Attachment = AttachmentPoint.MiddleRight
            mtext14.Contents = "This is a multiline text with " + mtext14.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint14 As New DBPoint(mtext14.Location)
              btr.AppendEntity(attPoint14)
            End Using
          End Using

          'Creates an MText entity with BottomLeft attachment point.
          Using mtext15 As New MText()
            btr.AppendEntity(mtext15)
            mtext15.TextHeight = 0.2
            mtext15.Width = 3
            mtext15.Location = New Point3d(0, -20, 0)
            mtext15.Attachment = AttachmentPoint.BottomLeft
            mtext15.Contents = "This is a multiline text with " + mtext15.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint15 As New DBPoint(mtext15.Location)
              btr.AppendEntity(attPoint15)
            End Using
          End Using

          'Creates an MText entity with BottomCenter attachment point.
          Using mtext16 As New MText()
            btr.AppendEntity(mtext16)
            mtext16.TextHeight = 0.2
            mtext16.Width = 3
            mtext16.Location = New Point3d(5, -20, 0)
            mtext16.Attachment = AttachmentPoint.BottomCenter
            mtext16.Contents = "This is a multiline text with " + mtext16.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint16 As New DBPoint(mtext16.Location)
              btr.AppendEntity(attPoint16)
            End Using
          End Using

          'Creates an MText entity with BottomRight attachment point.
          Using mtext17 As New MText()
            btr.AppendEntity(mtext17)
            mtext17.TextHeight = 0.2
            mtext17.Width = 3
            mtext17.Location = New Point3d(10, -20, 0)
            mtext17.Attachment = AttachmentPoint.BottomRight
            mtext17.Contents = "This is a multiline text with " + mtext17.Attachment.ToString() + " attachment point"
            'Creates an DBPoint entity indicating the attachment point.
            Using attPoint17 As New DBPoint(mtext17.Location)
              btr.AppendEntity(attPoint17)
            End Using
          End Using

          'Creates an MText entity and sets dynamic columns.
          Using mtext18 As New MText()
            btr.AppendEntity(mtext18)
            mtext18.Location = New Point3d(15, -10, 0)
            mtext18.TextHeight = 0.2
            mtext18.Height = 3
            mtext18.SetDynamicColumns(5, 0.4, True)
            mtext18.Contents = "This is a multiline text with " + mtext18.ColumnType.ToString() + " type of columns. " + MText.ParagraphBreak + "ColumnWidth is: " + mtext18.ColumnWidth.ToString() + MText.ParagraphBreak + "ColumnColumnGutterWidth is: " + mtext18.ColumnGutterWidth.ToString() + MText.ParagraphBreak + "ColumnAutoHeight is: " + mtext18.ColumnAutoHeight.ToString() + MText.ParagraphBreak + "ColumnFlowReversed is: " + mtext18.ColumnFlowReversed.ToString() + MText.ParagraphBreak + MText.ParagraphBreak + "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic)."
          End Using

          'Creates an MText entity and sets dynamic columns in flow reversed mode.
          Using mtext19 As New MText()
            btr.AppendEntity(mtext19)
            mtext19.Location = New Point3d(15, -15, 0)
            mtext19.TextHeight = 0.2
            mtext19.Height = 3
            mtext19.SetDynamicColumns(5, 0.4, True)
            mtext19.ColumnFlowReversed = True
            mtext19.Contents = "This is a multiline text with " + mtext19.ColumnType.ToString() + " type of columns. " + MText.ParagraphBreak + "ColumnWidth is: " + mtext19.ColumnWidth.ToString() + MText.ParagraphBreak + "ColumnColumnGutterWidth is: " + mtext19.ColumnGutterWidth.ToString() + MText.ParagraphBreak + "ColumnAutoHeight is: " + mtext19.ColumnAutoHeight.ToString() + MText.ParagraphBreak + "ColumnFlowReversed is: " + mtext19.ColumnFlowReversed.ToString() + MText.ParagraphBreak + MText.ParagraphBreak + "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic)."
          End Using

          'Creates an MText entity and sets static columns.
          Using mtext20 As New MText()
            btr.AppendEntity(mtext20)
            mtext20.Location = New Point3d(35, -10, 0)
            mtext20.TextHeight = 0.2
            mtext20.Height = 2
            mtext20.SetStaticColumns(5, 0.5, 2)
            mtext20.Contents = "This is a multiline text with " + mtext20.ColumnType.ToString() + " type of columns. " + MText.ParagraphBreak + "ColumnWidth is: " + mtext20.ColumnWidth.ToString() + MText.ParagraphBreak + "ColumnColumnGutterWidth is: " + mtext20.ColumnGutterWidth.ToString() + MText.ParagraphBreak + "ColumnCount is: " + mtext20.ColumnCount.ToString() + MText.ParagraphBreak + "ColumnFlowReversed is: " + mtext20.ColumnFlowReversed.ToString() + MText.ParagraphBreak + MText.ParagraphBreak + "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic)."
          End Using

          'Creates an MText entity and sets the rotation angle.
          Using mtext21 As New MText()
            btr.AppendEntity(mtext21)
            mtext21.Location = New Point3d(0, -30, 0)
            mtext21.TextHeight = 0.4
            mtext21.Rotation = 0.7
            mtext21.Contents = "Rotation angle is: " + mtext21.Rotation.ToString()
          End Using

        End Using
        ta.Commit()
      End Using
      db.SaveAs(path + "MTextEx.dwg", DwgVersion.Current)
    End Using
  End Sub
End Class


