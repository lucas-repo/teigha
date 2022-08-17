/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;

using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Colors;
using Teigha.GraphicsInterface;

namespace CDevGuideExamplesProject
{
  public class MTextEx
  {
    // Special fragment elaboration callback function. Adds extracted text fragment to list and prints some info. 
    MTextFragmentCallbackStatus MTextDelegateFunct(MTextFragment frag, Object data)
    {
      ((List<MTextFragment>)data).Add(frag);
      Console.WriteLine("Text content is: " + frag.Text);
      Console.WriteLine("Text color is: " + frag.Color.ColorIndex);
      Console.WriteLine("Text height is: " + frag.CapsHeight);
      Console.WriteLine("Oblique angle is: " + frag.ObliqueAngle);
      Console.WriteLine("Font name is: " + frag.TrueTypeFont);
      Console.WriteLine();
      return MTextFragmentCallbackStatus.Continue;
    }

    public MTextEx(String path)
    {
      Database db = new Database(true, true);
      // Changes point marker to "cross" instead of default "dot".
      db.Pdmode = 2;

      TransactionManager tm = db.TransactionManager;
      using (Transaction ta = tm.StartTransaction())
      {
        using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
        {
          // Creates an MText entity contains text with different formatting options applied to single words or characters 
          // within a paragraph or a text section.                
          using (MText mtext1 = new MText())
          {
            btr.AppendEntity(mtext1);
            mtext1.TextHeight = 0.2;
            mtext1.Contents = "This is a multiline text with" + MText.ParagraphBreak + "paragraph breaks, " + MText.ParagraphBreak +
            MText.BlockBegin + MText.ObliqueChange + "12;an oblique text, " + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + MText.OverlineOn + "an overlined text, " + MText.OverlineOff + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + MText.UnderlineOn + "an underlined text, " + MText.UnderlineOff + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + MText.WidthChange + "2;a text with the changed width" + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + MText.HeightChange + "0.5;a text with the changed height" + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + "a text " + MText.ColorChange + "10;in " + MText.ColorChange + "100;different " + MText.ColorChange + "200;colors, " + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + "a text " + MText.FontChange + "Times New Roman;in different " + MText.FontChange + "Tahoma;fonts" + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + "a stacked text with different allignments: " + MText.AlignChange + "0;1" + MText.StackStart + "1/2;  " + MText.AlignChange + "1;1" + MText.StackStart + "1/2;  " + MText.AlignChange + "2;1" + MText.StackStart + "1/2;" + MText.BlockEnd + MText.ParagraphBreak +
            MText.BlockBegin + "a text " + MText.TrackChange + "2;with the changed space" + MText.TrackChange + "0.4;between characters" + MText.BlockEnd + MText.ParagraphBreak;
            mtext1.Location = new Point3d(0, 0, 0);

            // Explodes mtext1 entity into a sequence of text fragments, passing each fragment to the MTextDelegateFunct function 
            // and stores each fragment in a list.
            List<MTextFragment> aFrgm = new List<MTextFragment>();
            mtext1.ExplodeFragments(new MTextFragmentCallback(MTextDelegateFunct), aFrgm);
          }

          // Creates an MText entity with text flowing from top to bottom.
          using (MText mtext2 = new MText())
          {
            btr.AppendEntity(mtext2);
            mtext2.TextHeight = 0.2;
            mtext2.Location = new Point3d(-2.0, 0.0, 0.0);
            mtext2.FlowDirection = FlowDirection.TopToBottom;
            mtext2.Contents = "This is a multiline text with FlowDirection of " + mtext2.FlowDirection;
          }

          // Creates an MText entity with LineSpacingFactor of 2.
          using (MText mtext3 = new MText())
          {
            btr.AppendEntity(mtext3);
            mtext3.TextHeight = 0.2;
            mtext3.Width = 2;
            mtext3.Location = new Point3d(16.0, 0.0, 0.0);
            mtext3.LineSpacingFactor = 2;
            mtext3.Contents = "This is a multiline text with LineSpacingFactor of " + mtext3.LineSpacingFactor;
          }

          // Creates an MText entity with LineSpacingFactor of 0.8.
          using (MText mtext4 = new MText())
          {
            btr.AppendEntity(mtext4);
            mtext4.TextHeight = 0.2;
            mtext4.Width = 2;
            mtext4.Location = new Point3d(22.0, 0.0, 0.0);
            mtext4.LineSpacingFactor = 0.8;
            mtext4.Contents = "This is a multiline text with LineSpacingFactor of " + mtext4.LineSpacingFactor;
          }

          // Creates an MText entity with AtLeast value of LineSpacingStyle.
          using (MText mtext5 = new MText())
          {
            btr.AppendEntity(mtext5);
            mtext5.Location = new Point3d(28, 0.0, 0.0);
            mtext5.TextHeight = 0.2;
            mtext5.Width = 2;
            mtext5.LineSpacingStyle = LineSpacingStyle.AtLeast;
            mtext5.Contents = "This is a multiline text with LineSpacingStyle of " + MText.HeightChange + "0.6;" + mtext5.LineSpacingStyle;
          }

          // Creates an MText entity with Exactly value of LineSpacingStyle.
          using (MText mtext6 = new MText())
          {
            btr.AppendEntity(mtext6);
            mtext6.TextHeight = 0.2;
            mtext6.Width = 2;
            mtext6.Location = new Point3d(34.0, 0.0, 0.0);
            mtext6.LineSpacingStyle = LineSpacingStyle.Exactly;
            mtext6.Contents = "This is a multiline text with LineSpacingStyle of " + MText.HeightChange + "0.6;" + mtext6.LineSpacingStyle;
          }

          // Creates an MText entity with default BackgroundScaleFactor.
          using (MText mtext7 = new MText())
          {
            btr.AppendEntity(mtext7);
            mtext7.TextHeight = 0.2;
            mtext7.Width = 4;
            mtext7.Location = new Point3d(40, 0.0, 0.0);
            mtext7.BackgroundFill = true;
            mtext7.BackgroundFillColor = Color.FromRgb(250, 200, 250);
            mtext7.Contents = "This is a multiline text with BackgroundScaleFactor of " + mtext7.BackgroundScaleFactor;
          }

          // Creates an MText entity with BackgroundScaleFactor of 5.
          using (MText mtext8 = new MText())
          {
            btr.AppendEntity(mtext8);
            mtext8.TextHeight = 0.2;
            mtext8.Width = 4;
            mtext8.Location = new Point3d(46, 0.0, 0.0);
            mtext8.BackgroundFill = true;
            mtext8.BackgroundFillColor = Color.FromRgb(250, 200, 250);
            mtext8.BackgroundScaleFactor = 5;
            mtext8.Contents = "This is a multiline text with BackgroundScaleFactor of " + mtext8.BackgroundScaleFactor;
          }

          // Creates an MText entity with TopLeft attachment point.
          using (MText mtext9 = new MText())
          {
            btr.AppendEntity(mtext9);
            mtext9.TextHeight = 0.2;
            mtext9.Width = 3;
            mtext9.Location = new Point3d(0, -10, 0);
            mtext9.Attachment = AttachmentPoint.TopLeft;
            mtext9.Contents = "This is a multiline text with " + mtext9.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint9 = new DBPoint(mtext9.Location))
            {
              btr.AppendEntity(attPoint9);
            }
          }

          // Creates an MText entity with TopCenter attachment point.
          using (MText mtext10 = new MText())
          {
            btr.AppendEntity(mtext10);
            mtext10.TextHeight = 0.2;
            mtext10.Width = 3;
            mtext10.Location = new Point3d(5, -10, 0);
            mtext10.Attachment = AttachmentPoint.TopCenter;
            mtext10.Contents = "This is a multiline text with " + mtext10.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint10 = new DBPoint(mtext10.Location))
            {
              btr.AppendEntity(attPoint10);
            }
          }

          // Creates an MText entity with TopRight attachment point.
          using (MText mtext11 = new MText())
          {
            btr.AppendEntity(mtext11);
            mtext11.TextHeight = 0.2;
            mtext11.Width = 3;
            mtext11.Location = new Point3d(10, -10, 0);
            mtext11.Attachment = AttachmentPoint.TopRight;
            mtext11.Contents = "This is a multiline text with " + mtext11.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint11 = new DBPoint(mtext11.Location))
            {
              btr.AppendEntity(attPoint11);
            }
          }

          // Creates an MText entity with MiddleLeft attachment point.
          using (MText mtext12 = new MText())
          {
            btr.AppendEntity(mtext12);
            mtext12.TextHeight = 0.2;
            mtext12.Width = 3;
            mtext12.Location = new Point3d(0, -15, 0);
            mtext12.Attachment = AttachmentPoint.MiddleLeft;
            mtext12.Contents = "This is a multiline text with " + mtext12.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint12 = new DBPoint(mtext12.Location))
            {
              btr.AppendEntity(attPoint12);
            }
          }

          // Creates an MText entity with MiddleCenter attachment point.
          using (MText mtext13 = new MText())
          {
            btr.AppendEntity(mtext13);
            mtext13.TextHeight = 0.2;
            mtext13.Width = 3;
            mtext13.Location = new Point3d(5, -15, 0);
            mtext13.Attachment = AttachmentPoint.MiddleCenter;
            mtext13.Contents = "This is a multiline text with " + mtext13.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint13 = new DBPoint(mtext13.Location))
            {
              btr.AppendEntity(attPoint13);
            }
          }

          // Creates an MText entity with MiddleRight attachment point.
          using (MText mtext14 = new MText())
          {
            btr.AppendEntity(mtext14);
            mtext14.TextHeight = 0.2;
            mtext14.Width = 3;
            mtext14.Location = new Point3d(10, -15, 0);
            mtext14.Attachment = AttachmentPoint.MiddleRight;
            mtext14.Contents = "This is a multiline text with " + mtext14.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint14 = new DBPoint(mtext14.Location))
            {
              btr.AppendEntity(attPoint14);
            }
          }

          // Creates an MText entity with BottomLeft attachment point.
          using (MText mtext15 = new MText())
          {
            btr.AppendEntity(mtext15);
            mtext15.TextHeight = 0.2;
            mtext15.Width = 3;
            mtext15.Location = new Point3d(0, -20, 0);
            mtext15.Attachment = AttachmentPoint.BottomLeft;
            mtext15.Contents = "This is a multiline text with " + mtext15.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint15 = new DBPoint(mtext15.Location))
            {
              btr.AppendEntity(attPoint15);
            }
          }

          // Creates an MText entity with BottomCenter attachment point.
          using (MText mtext16 = new MText())
          {
            btr.AppendEntity(mtext16);
            mtext16.TextHeight = 0.2;
            mtext16.Width = 3;
            mtext16.Location = new Point3d(5, -20, 0);
            mtext16.Attachment = AttachmentPoint.BottomCenter;
            mtext16.Contents = "This is a multiline text with " + mtext16.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint16 = new DBPoint(mtext16.Location))
            {
              btr.AppendEntity(attPoint16);
            }
          }

          // Creates an MText entity with BottomRight attachment point.
          using (MText mtext17 = new MText())
          {
            btr.AppendEntity(mtext17);
            mtext17.TextHeight = 0.2;
            mtext17.Width = 3;
            mtext17.Location = new Point3d(10, -20, 0);
            mtext17.Attachment = AttachmentPoint.BottomRight;
            mtext17.Contents = "This is a multiline text with " + mtext17.Attachment + " attachment point";
            // Creates an DBPoint entity indicating the attachment point.
            using (DBPoint attPoint17 = new DBPoint(mtext17.Location))
            {
              btr.AppendEntity(attPoint17);
            }
          }

          // Creates an MText entity and sets dynamic columns.
          using (MText mtext18 = new MText())
          {
            btr.AppendEntity(mtext18);
            mtext18.Location = new Point3d(15, -10, 0);
            mtext18.TextHeight = 0.2;
            mtext18.Height = 3;
            mtext18.SetDynamicColumns(5, 0.4, true);
            mtext18.Contents = "This is a multiline text with " + mtext18.ColumnType + " type of columns. " + MText.ParagraphBreak +
              "ColumnWidth is: " + mtext18.ColumnWidth + MText.ParagraphBreak +
              "ColumnColumnGutterWidth is: " + mtext18.ColumnGutterWidth + MText.ParagraphBreak +
              "ColumnAutoHeight is: " + mtext18.ColumnAutoHeight + MText.ParagraphBreak +
              "ColumnFlowReversed is: " + mtext18.ColumnFlowReversed + MText.ParagraphBreak + MText.ParagraphBreak +
              "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic).";
          }

          // Creates an MText entity and sets dynamic columns in flow reversed mode.
          using (MText mtext19 = new MText())
          {
            btr.AppendEntity(mtext19);
            mtext19.Location = new Point3d(15, -15, 0);
            mtext19.TextHeight = 0.2;
            mtext19.Height = 3;
            mtext19.SetDynamicColumns(5, 0.4, true);
            mtext19.ColumnFlowReversed = true;
            mtext19.Contents = "This is a multiline text with " + mtext19.ColumnType + " type of columns. " + MText.ParagraphBreak +
              "ColumnWidth is: " + mtext19.ColumnWidth + MText.ParagraphBreak +
              "ColumnColumnGutterWidth is: " + mtext19.ColumnGutterWidth + MText.ParagraphBreak +
              "ColumnAutoHeight is: " + mtext19.ColumnAutoHeight + MText.ParagraphBreak +
              "ColumnFlowReversed is: " + mtext19.ColumnFlowReversed + MText.ParagraphBreak + MText.ParagraphBreak +
              "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic).";
          }

          // Creates an MText entity and sets static columns.
          using (MText mtext20 = new MText())
          {
            btr.AppendEntity(mtext20);
            mtext20.Location = new Point3d(35, -10, 0);
            mtext20.TextHeight = 0.2;
            mtext20.Height = 2;
            mtext20.SetStaticColumns(5, 0.5, 2);
            mtext20.Contents = "This is a multiline text with " + mtext20.ColumnType + " type of columns. " + MText.ParagraphBreak +
              "ColumnWidth is: " + mtext20.ColumnWidth + MText.ParagraphBreak +
              "ColumnColumnGutterWidth is: " + mtext20.ColumnGutterWidth + MText.ParagraphBreak +
              "ColumnCount is: " + mtext20.ColumnCount + MText.ParagraphBreak +
              "ColumnFlowReversed is: " + mtext20.ColumnFlowReversed + MText.ParagraphBreak + MText.ParagraphBreak +
              "Teigha.NET for .dwg files is a managed .NET component containing tools for working with .dwg file data. Built on top of the Teigha for .dwg files C++ development environment, Teigha.NET for .dwg files exposes access to .dwg application development using the .NET framework using any .NET language (C#, Visual Basic).";
          }

          // Creates an MText entity and sets the rotation angle.
          using (MText mtext21 = new MText())
          {
            btr.AppendEntity(mtext21);
            mtext21.Location = new Point3d(0, -30, 0);
            mtext21.TextHeight = 0.4;
            mtext21.Rotation = 0.7;
            mtext21.Contents = "Rotation angle is: " + mtext21.Rotation;
          }
        }
        ta.Commit();
      }
      db.SaveAs(path + "MTextEx.dwg", DwgVersion.Current);
      db.Dispose();
    }
  }
}
