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
using System.IO;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.GraphicsInterface;
using Teigha.Colors;
using Teigha;
// note that GetObject doesn't work in Acad 2009, so we use "obsolete" Open instead
#pragma warning disable 618

namespace OdReadExMgd
{
  class DbDumper
  {
    public DbDumper() { }

    static string toDegreeString(double val)
    {
      return (val * 180.0 / Math.PI) + "d";
    }
    static string toHexString(int val)
    {
      return string.Format("0{0:X}", val);
    }
    static string toArcSymbolTypeString(int val)
    {
      switch (val)
      {
        case 0: return "Precedes text";
        case 1: return "Above text";
        case 2: return "None";
      }
      return "???";
    }
    /************************************************************************/
    /* Shorten a path with ellipses.                                        */
    /************************************************************************/
    static string shortenPath(string Inpath, int maxPath)
    {
      string path = Inpath;
      /**********************************************************************/
      /* If the path fits, just return it                                   */
      /**********************************************************************/
      if (path.Length <= maxPath)
      {
        return path;
      }
      /**********************************************************************/
      /* If there's no backslash, just truncate the path                    */
      /**********************************************************************/
      int lastBackslash = path.LastIndexOf('\\');
      if (lastBackslash < 0)
      {
        return path.Substring(0, maxPath - 3) + "...";
      }

      /**********************************************************************/
      /* Shorten the front of the path                                      */
      /**********************************************************************/
      int fromLeft = (lastBackslash - 3) - (path.Length - maxPath);
      // (12 - 3) - (19 - 10) = 9 - 9 = 0 
      if ((lastBackslash <= 3) || (fromLeft < 1))
      {
        path = "..." + path.Substring(lastBackslash);
      }
      else
      {
        path = path.Substring(0, fromLeft) + "..." + path.Substring(lastBackslash);
      }

      /**********************************************************************/
      /* Truncate the path                                                  */
      /**********************************************************************/
      if (path.Length > maxPath)
      {
        path = path.Substring(0, maxPath - 3) + "...";
      }

      return path;
    }
    static string shortenPath(string Inpath)
    {
      return shortenPath(Inpath, 40);
    }

    /************************************************************************/
    /* Output a string in the form                                          */
    /*   leftString:. . . . . . . . . . . .rightString                      */
    /************************************************************************/
    static void writeLine(int indent, object leftString, object rightString, int colWidth)
    {
      string spaces = "                                                            ";
      string leader = ". . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ";

      const int tabSize = 2;

      /**********************************************************************/
      /* Indent leftString with spaces characters                           */
      /**********************************************************************/
      string newleftString = spaces.Substring(0, tabSize * indent) + leftString.ToString();

      /**********************************************************************/
      /* If rightString is not specified, just output the indented          */
      /* leftString. Otherwise, fill the space between leftString and       */
      /* rightString with leader characters.                                */
      /**********************************************************************/
      if (rightString == null || ((rightString is string) && ((string)rightString) == ""))
      {
        Console.WriteLine(newleftString);
      }
      else
      {
        int leaders = colWidth - newleftString.Length;
        if (leaders > 0)
        {
          Console.WriteLine(newleftString + leader.Substring(newleftString.Length, leaders) + rightString.ToString());
        }
        else
        {
          Console.WriteLine(newleftString + ' ' + rightString.ToString());
        }
      }
    }
    static void writeLine(int indent, object leftString, object rightString)
    {
      writeLine(indent, leftString, rightString, 38);
    }
    static void writeLine(int indent, object leftString)
    {
      writeLine(indent, leftString, null, 38);
    }
    static void writeLine()
    {
      Console.WriteLine();
    }
    static void dumpEntityData(Entity pEnt, int indent)
    {
      try
      {
        Extents3d ext = pEnt.GeometricExtents;
        writeLine(indent, "Min Extents", ext.MinPoint);
        writeLine(indent, "Max Extents", ext.MaxPoint);
      }
      catch (System.Exception)
      {
      }
      writeLine(indent, "Layer", pEnt.Layer);
      writeLine(indent, "Color Index", pEnt.ColorIndex);
      writeLine(indent, "Color", pEnt.Color);
      writeLine(indent, "Linetype", pEnt.Linetype);
      writeLine(indent, "LTscale", pEnt.LinetypeScale);
      writeLine(indent, "Lineweight", pEnt.LineWeight);
      writeLine(indent, "Plot Style", pEnt.PlotStyleName);
      writeLine(indent, "Transparency Method", pEnt.Transparency);
      writeLine(indent, "Visibility", pEnt.Visible);
      writeLine(indent, "Planar", pEnt.IsPlanar);

      if (pEnt.IsPlanar)
      {
        try
        {
          CoordinateSystem3d cs = (CoordinateSystem3d)pEnt.GetPlane().GetCoordinateSystem();
          writeLine(indent + 1, "Origin", cs.Origin);
          writeLine(indent + 1, "u-Axis", cs.Xaxis);
          writeLine(indent + 1, "v-Axis", cs.Yaxis);
        }
        catch (System.Exception ex)
        {
          writeLine(indent + 1, "pEnt.GetPlane().GetCoordinateSystem() failed", ex.Message);
        }
      }
    }
    /************************************************************************/
    /* Dump Text data                                                       */
    /************************************************************************/
    static void dumpTextData(DBText pText, int indent)
    {
      writeLine(indent, "Text String", pText.TextString);
      writeLine(indent, "Text Position", pText.Position);
      writeLine(indent, "Default Alignment", pText.IsDefaultAlignment);
      writeLine(indent, "Alignment Point", pText.AlignmentPoint);
      writeLine(indent, "Height", pText.Height);
      writeLine(indent, "Rotation", toDegreeString(pText.Rotation));
      writeLine(indent, "Horizontal Mode", pText.HorizontalMode);
      writeLine(indent, "Vertical Mode", pText.VerticalMode);
      writeLine(indent, "Mirrored in X", pText.IsMirroredInX);
      writeLine(indent, "Mirrored in Y", pText.IsMirroredInY);
      writeLine(indent, "Oblique", toDegreeString(pText.Oblique));
      writeLine(indent, "Text Style", pText.TextStyleName);
      writeLine(indent, "Width Factor", pText.WidthFactor);

      writeLine(indent, "Normal", pText.Normal);
      writeLine(indent, "Thickness", pText.Thickness);
      dumpEntityData(pText, indent);
    }

    /************************************************************************/
    /* Dump Attribute data                                                  */
    /************************************************************************/
    static void dumpAttributeData(int indent, AttributeReference pAttr, int i)
    {
      writeLine(indent++, pAttr.GetRXClass().Name, i);
      writeLine(indent, "Handle", pAttr.Handle);
      writeLine(indent, "Tag", pAttr.Tag);
      writeLine(indent, "Field Length", pAttr.FieldLength);
      writeLine(indent, "Invisible", pAttr.Invisible);
      writeLine(indent, "Preset", pAttr.IsPreset);
      writeLine(indent, "Verifiable", pAttr.IsVerifiable);
      writeLine(indent, "Locked in Position", pAttr.LockPositionInBlock);
      writeLine(indent, "Constant", pAttr.IsConstant);
      dumpTextData(pAttr, indent);
    }

    /************************************************************************/
    /* Dump Block Reference Data                                             */
    /************************************************************************/
    static void dumpBlockRefData(BlockReference pBlkRef, int indent)
    {
      writeLine(indent, "Position", pBlkRef.Position);
      writeLine(indent, "Rotation", toDegreeString(pBlkRef.Rotation));
      writeLine(indent, "Scale Factors", pBlkRef.ScaleFactors);
      writeLine(indent, "Normal", pBlkRef.Normal);
      dumpEntityData(pBlkRef, indent);

      /**********************************************************************/
      /* Dump the attributes                                                */
      /**********************************************************************/
      int i = 0;
      AttributeCollection attCol = pBlkRef.AttributeCollection;
      foreach (ObjectId id in attCol)
      {
        try
        {
          using(AttributeReference pAttr = (AttributeReference)id.Open(OpenMode.ForRead))
            dumpAttributeData(indent, pAttr, i++);
        }
        catch (System.Exception)
        {

        }
      }
    }
    /************************************************************************/
    /* Dump data common to all OdDbCurves                                   */
    /************************************************************************/
    static void dumpCurveData(Entity pEnt, int indent)
    {
      Curve pEntity = (Curve)pEnt;
      try
      {
        writeLine(indent, "Start Point", pEntity.StartPoint);
        writeLine(indent, "End Point", pEntity.EndPoint);
      }
      catch (System.Exception)
      {
      }
      writeLine(indent, "Closed", pEntity.Closed);
      writeLine(indent, "Periodic", pEntity.IsPeriodic);

      try
      {
        writeLine(indent, "Area", pEntity.Area);
      }
      catch (System.Exception)
      {
      }
      dumpEntityData(pEntity, indent);
    }
    /************************************************************************/
    /* Dump Dimension data                                                  */
    /************************************************************************/
    static void dumpDimData(Dimension pDim, int indent)
    {
      writeLine(indent, "Measurement", pDim.CurrentMeasurement);
      writeLine(indent, "Dimension Text", pDim.DimensionText);

      if (pDim.CurrentMeasurement >= 0.0)
      {
        writeLine(indent, "Formatted Measurement", pDim.FormatMeasurement(pDim.CurrentMeasurement, pDim.DimensionText));
      }
      if (pDim.DimBlockId.IsNull)
      {
        writeLine(indent, "Dimension Block NULL");
      }
      else
      {
        using(BlockTableRecord btr = (BlockTableRecord)pDim.DimBlockId.Open(OpenMode.ForRead))
          writeLine(indent, "Dimension Block Name", btr.Name);
      }
      writeLine(indent + 1, "Position", pDim.DimBlockPosition);
      writeLine(indent, "Text Position", pDim.TextPosition);
      writeLine(indent, "Text Rotation", toDegreeString(pDim.TextRotation));
      writeLine(indent, "Dimension Style", pDim.DimensionStyleName);
      writeLine(indent, "Background Text Color", pDim.Dimtfillclr);
      writeLine(indent, "BackgroundText Flags", pDim.Dimtfill);
      writeLine(indent, "Extension Line 1 Linetype", pDim.Dimltex1);
      writeLine(indent, "Extension Line 2 Linetype", pDim.Dimltex2);
      writeLine(indent, "Dimension Line Linetype", pDim.Dimltype);
      writeLine(indent, "Horizontal Rotation", toDegreeString(pDim.HorizontalRotation));
      writeLine(indent, "Elevation", pDim.Elevation);
      writeLine(indent, "Normal", pDim.Normal);
      dumpEntityData(pDim, indent);
    }
    /************************************************************************/
    /* 2 Line Angular Dimension Dumper                                      */
    /************************************************************************/
    static void dump(LineAngularDimension2 pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Arc Point", pDim.ArcPoint);
      writeLine(indent, "Extension Line 1 Start", pDim.XLine1Start);
      writeLine(indent, "Extension Line 1 End", pDim.XLine1End);
      writeLine(indent, "Extension Line 2 Start", pDim.XLine2Start);
      writeLine(indent, "Extension Line 2 End", pDim.XLine2End);
      dumpDimData(pDim, indent);
    }
    /************************************************************************/
    /* Dump 2D Vertex data                                                  */
    /************************************************************************/
    static void dump2dVertex(int indent, Vertex2d pVertex, int i)
    {
      writeLine(indent++, pVertex.GetRXClass().Name, i);
      writeLine(indent, "Handle", pVertex.Handle);
      writeLine(indent, "Vertex Type", pVertex.VertexType);
      writeLine(indent, "Position", pVertex.Position);
      writeLine(indent, "Start Width", pVertex.StartWidth);
      writeLine(indent, "End Width", pVertex.EndWidth);
      writeLine(indent, "Bulge", pVertex.Bulge);

      if (pVertex.Bulge != 0)
      {
        writeLine(indent, "Bulge Angle", toDegreeString(4 * Math.Atan(pVertex.Bulge)));
      }

      writeLine(indent, "Tangent Used", pVertex.TangentUsed);
      if (pVertex.TangentUsed)
      {
        writeLine(indent, "Tangent", pVertex.Tangent);
      }
    }
    /************************************************************************/
    /* 2D Polyline Dumper                                                   */
    /************************************************************************/
    static void dump(Polyline2d pPolyline, int indent)
    {
      writeLine(indent++, pPolyline.GetRXClass().Name, pPolyline.Handle);
      writeLine(indent, "Elevation", pPolyline.Elevation);
      writeLine(indent, "Normal", pPolyline.Normal);
      writeLine(indent, "Thickness", pPolyline.Thickness);
      /********************************************************************/
      /* Dump the vertices                                                */
      /********************************************************************/
      int i = 0;
      foreach (ObjectId obj in pPolyline)
      {
        using (DBObject dbObj = (DBObject)obj.GetObject(OpenMode.ForRead))
        {
          if (dbObj is Vertex2d)
          {
            dump2dVertex(indent, (Vertex2d)dbObj, i++);
          }
        }
      }
      dumpCurveData(pPolyline, indent);
    }


    /************************************************************************/
    /* Dump 3D Polyline Vertex data                                         */
    /************************************************************************/
    void dump3dPolylineVertex(int indent, PolylineVertex3d pVertex, int i)
    {
      writeLine(indent++, pVertex.GetRXClass().Name, i);
      writeLine(indent, "Handle", pVertex.Handle);
      writeLine(indent, "Type", pVertex.VertexType);
      writeLine(indent, "Position", pVertex.Position);
    }

    /************************************************************************/
    /* 3D Polyline Dumper                                                   */
    /************************************************************************/
    void dump(Polyline3d pPolyline, int indent)
    {
      writeLine(indent++, pPolyline.GetRXClass().Name, pPolyline.Handle);
      /********************************************************************/
      /* Dump the vertices                                                */
      /********************************************************************/
      int i = 0;
      foreach (ObjectId obj in pPolyline)
      {
        using (DBObject dbObj = (DBObject)obj.GetObject(OpenMode.ForRead))
        {
          if (dbObj is PolylineVertex3d)
          {
            dump3dPolylineVertex(indent, (PolylineVertex3d)dbObj, i++);
          }
        }
      }
      dumpCurveData(pPolyline, indent);
    }


    /************************************************************************/
    /* 3DSolid Dumper                                                       */
    /************************************************************************/
    void dump(Solid3d pSolid, int indent)
    {
      writeLine(indent++, pSolid.GetRXClass().Name, pSolid.Handle);
      dumpEntityData(pSolid, indent);
    }


    /************************************************************************/
    /* 3 Point Angular Dimension Dumper                                     */
    /************************************************************************/
    void dump(Point3AngularDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Arc Point", pDim.ArcPoint);
      writeLine(indent, "Center Point", pDim.CenterPoint);
      writeLine(indent, "Extension Line 1 Point", pDim.XLine1Point);
      writeLine(indent, "Extension Line 2 Point", pDim.XLine2Point);
      dumpDimData(pDim, indent);
    }

    /************************************************************************/
    /* Aligned Dimension Dumper                                             */
    /************************************************************************/
    void dump(AlignedDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Dimension line Point", pDim.DimLinePoint);
      writeLine(indent, "Oblique", toDegreeString(pDim.Oblique));
      writeLine(indent, "Extension Line 1 Point", pDim.XLine1Point);
      writeLine(indent, "Extension Line 2 Point", pDim.XLine2Point);
      dumpDimData(pDim, indent);
    }

    /************************************************************************/
    /* Arc Dumper                                                           */
    /************************************************************************/
    void dump(Arc pArc, int indent)
    {
      writeLine(indent++, pArc.GetRXClass().Name, pArc.Handle);
      writeLine(indent, "Center", pArc.Center);
      writeLine(indent, "Radius", pArc.Radius);
      writeLine(indent, "Start Angle", toDegreeString(pArc.StartAngle));
      writeLine(indent, "End Angle", toDegreeString(pArc.EndAngle));
      writeLine(indent, "Normal", pArc.Normal);
      writeLine(indent, "Thickness", pArc.Thickness);
      dumpCurveData(pArc, indent);
    }

    /************************************************************************/
    /* Arc Dimension Dumper                                                 */
    /************************************************************************/
    void dump(ArcDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Arc Point", pDim.ArcPoint);
      writeLine(indent, "Center Point", pDim.CenterPoint);
      writeLine(indent, "Arc symbol", toArcSymbolTypeString(pDim.ArcSymbolType));
      writeLine(indent, "Partial", pDim.IsPartial);
      writeLine(indent, "Has leader", pDim.HasLeader);

      if (pDim.HasLeader)
      {
        writeLine(indent, "Leader 1 Point", pDim.Leader1Point);
        writeLine(indent, "Leader 2 Point", pDim.Leader2Point);
      }
      writeLine(indent, "Extension Line 1 Point", pDim.XLine1Point);
      writeLine(indent, "Extension Line 2 Point", pDim.XLine2Point);
      dumpDimData(pDim, indent);
    }


    /************************************************************************/
    /* Block Reference Dumper                                                */
    /************************************************************************/
    void dump(BlockReference pBlkRef, int indent)
    {
      writeLine(indent++, pBlkRef.GetRXClass().Name, pBlkRef.Handle);

      using (BlockTableRecord pRecord = (BlockTableRecord)pBlkRef.BlockTableRecord.Open(OpenMode.ForRead))
      {
        writeLine(indent, "Name", pRecord.Name);
        dumpBlockRefData(pBlkRef, indent);
      /********************************************************************/
      /* Dump the Spatial Filter  (Xref Clip)                             */
      /********************************************************************/
      // TODO:
      /*
      Filters.SpatialFilter pFilt = 
      {
        writeLine(indent++, pFilt.GetRXClass().Name,   pFilt.Handle);
        writeLine(indent, "Normal",                    pFilt.Definition.Normal);
        writeLine(indent, "Elevation",                 pFilt.Definition.Elevation);
        writeLine(indent, "Front Clip Distance",       pFilt.Definition.FrontClip);
        writeLine(indent, "Back Clip Distance",        pFilt.Definition.BackClip);
        writeLine(indent, "Enabled",                   pFilt.Definition.Enabled);
        foreach (Point2d p in pFilt.Definition.GetPoints())
        {
          writeLine(indent, string.Format("Clip point %d",i), p);
        }
      }
       */
      }
    }

    /************************************************************************/
    /* Body Dumper                                                          */
    /************************************************************************/
    void dump(Body pBody, int indent)
    {
      writeLine(indent++, pBody.GetRXClass().Name, pBody.Handle);
      dumpEntityData(pBody, indent);
    }


    /************************************************************************/
    /* Circle Dumper                                                        */
    /************************************************************************/
    void dump(Circle pCircle, int indent)
    {
      writeLine(indent++, pCircle.GetRXClass().Name, pCircle.Handle);
      writeLine(indent, "Center", pCircle.Center);
      writeLine(indent, "Radius", pCircle.Radius);
      writeLine(indent, "Normal", pCircle.Normal);
      writeLine(indent, "Thickness", pCircle.Thickness);
      dumpCurveData(pCircle, indent);
    }

    /************************************************************************/
    /* Diametric Dimension Dumper                                           */
    /************************************************************************/
    void dump(DiametricDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Chord Point", pDim.ChordPoint);
      writeLine(indent, "Far chord Point", pDim.FarChordPoint);
      writeLine(indent, "Leader Length", pDim.LeaderLength);
      dumpDimData(pDim, indent);
    }

    /************************************************************************/
    /* Ellipse Dumper                                                       */
    /************************************************************************/
    void dump(Ellipse pEllipse, int indent)
    {
      writeLine(indent++, pEllipse.GetRXClass().Name, pEllipse.Handle);
      writeLine(indent, "Center", pEllipse.Center);
      writeLine(indent, "Major Axis", pEllipse.MajorAxis);
      writeLine(indent, "Minor Axis", pEllipse.MinorAxis);
      writeLine(indent, "Major Radius", pEllipse.MajorAxis.Length);
      writeLine(indent, "Minor Radius", pEllipse.MinorAxis.Length);
      writeLine(indent, "Radius Ratio", pEllipse.RadiusRatio);
      writeLine(indent, "Start Angle", toDegreeString(pEllipse.StartAngle));
      writeLine(indent, "End Angle", toDegreeString(pEllipse.EndAngle));
      writeLine(indent, "Normal", pEllipse.Normal);
      dumpCurveData(pEllipse, indent);
    }
    /************************************************************************/
    /* Face Dumper                                                       */
    /************************************************************************/
    void dump(Face pFace, int indent)
    {
      writeLine(indent++, pFace.GetRXClass().Name, pFace.Handle);
      for (short i = 0; i < 4; i++)
      {
        writeLine(indent, string.Format("Vertex {0} ", i), pFace.GetVertexAt(i));
      }
      for (short i = 0; i < 4; i++)
      {
        writeLine(indent, string.Format("Edge {0} visible", i), pFace.IsEdgeVisibleAt(i));
      }
      dumpEntityData(pFace, indent);
    }

    /************************************************************************/
    /* FCF Dumper                                                           */
    /************************************************************************/
    void dump(FeatureControlFrame pFcf, int indent)
    {
      writeLine(indent++, pFcf.GetRXClass().Name, pFcf.Handle);
      writeLine(indent, "Location", pFcf.Location);
      writeLine(indent, "Text", pFcf.Text);
      writeLine(indent, "Dimension Style", pFcf.DimensionStyleName);
      writeLine(indent, "Dimension Gap", pFcf.Dimgap);
      writeLine(indent, "Dimension Scale", pFcf.Dimscale);
      writeLine(indent, "Text Height", pFcf.Dimtxt);
      writeLine(indent, "Frame Color", pFcf.Dimclrd);
      writeLine(indent, "Text Style", pFcf.TextStyleName);
      writeLine(indent, "Text Color", pFcf.Dimclrd);
      writeLine(indent, "X-Direction", pFcf.Direction);
      writeLine(indent, "Normal", pFcf.Normal);
      dumpEntityData(pFcf, indent);
    }

    /************************************************************************/
    /* Hatch Dumper                                                         */
    /************************************************************************/
    /***********************************************************************/
    /* Dump Polyline Loop                                                  */
    /***********************************************************************/
    static void dumpPolylineType(int loopIndex, Hatch pHatch, int indent)
    {
      HatchLoop hl = pHatch.GetLoopAt(loopIndex);
      for (int i = 0; i < hl.Polyline.Count; i++)
      {
        BulgeVertex bv = hl.Polyline[i];
        writeLine(indent, "Vertex " + i.ToString(), bv.Vertex.ToString());
        writeLine(indent + 1, "Bulge " + i.ToString(), bv.Bulge);
        writeLine(indent + 1, "Bulge angle " + i.ToString(), toDegreeString(4 * Math.Atan(bv.Bulge)));
      }
    }

    /**********************************************************************/
    /* Dump Circular Arc Edge                                             */
    /**********************************************************************/
    static void dumpCircularArcEdge(int indent, CircularArc2d pCircArc)
    {
      writeLine(indent, "Center", pCircArc.Center);
      writeLine(indent, "Radius", pCircArc.Radius);
      writeLine(indent, "Start Angle", toDegreeString(pCircArc.StartAngle));
      writeLine(indent, "End Angle", toDegreeString(pCircArc.EndAngle));
      writeLine(indent, "Clockwise", pCircArc.IsClockWise);
    }

    /**********************************************************************/
    /* Dump Elliptical Arc Edge                                           */
    /**********************************************************************/
    static void dumpEllipticalArcEdge(int indent, EllipticalArc2d pEllipArc)
    {
      writeLine(indent, "Center", pEllipArc.Center);
      writeLine(indent, "Major Radius", pEllipArc.MajorRadius);
      writeLine(indent, "Minor Radius", pEllipArc.MinorRadius);
      writeLine(indent, "Major Axis", pEllipArc.MajorAxis);
      writeLine(indent, "Minor Axis", pEllipArc.MinorAxis);
      writeLine(indent, "Start Angle", toDegreeString(pEllipArc.StartAngle));
      writeLine(indent, "End Angle", toDegreeString(pEllipArc.EndAngle));
      writeLine(indent, "Clockwise", pEllipArc.IsClockWise);
    }
    /**********************************************************************/
    /* Dump NurbCurve Edge                                           */
    /**********************************************************************/
    static void dumpNurbCurveEdge(int indent, NurbCurve2d pNurbCurve)
    {
      NurbCurve2dData d = pNurbCurve.DefinitionData;
      writeLine(indent, "Degree", d.Degree);
      writeLine(indent, "Rational", d.Rational);
      writeLine(indent, "Periodic", d.Periodic);

      writeLine(indent, "Number of Control Points", d.ControlPoints.Count);
      for (int i = 0; i < d.ControlPoints.Count; i++)
      {
        writeLine(indent, "Control Point " + i.ToString(), d.ControlPoints[i]);
      }
      writeLine(indent, "Number of Knots", d.Knots.Count);
      for (int i = 0; i < d.Knots.Count; i++)
      {
        writeLine(indent, "Knot " + i.ToString(), d.Knots[i]);
      }

      if (d.Rational)
      {
        writeLine(indent, "Number of Weights", d.Weights.Count);
        for (int i = 0; i < d.Weights.Count; i++)
        {
          writeLine(indent, "Weight " + i.ToString(), d.Weights[i]);
        }
      }
    }

    /***********************************************************************/
    /* Dump Edge Loop                                                      */
    /***********************************************************************/
    static void dumpEdgesType(int loopIndex, Hatch pHatch, int indent)
    {
      Curve2dCollection edges = pHatch.GetLoopAt(loopIndex).Curves;
      for (int i = 0; i < (int)edges.Count; i++)
      {
        using (Curve2d pEdge = edges[i])
        {
          writeLine(indent, string.Format("Edge {0}", i), pEdge.GetType().Name);
          switch (pEdge.GetType().Name)
          {
            case "LineSegment2d":
              break;
            case "CircularArc2d":
              dumpCircularArcEdge(indent + 1, (CircularArc2d)pEdge);
              break;
            case "EllipticalArc2d":
              dumpEllipticalArcEdge(indent + 1, (EllipticalArc2d)pEdge);
              break;
            case "NurbCurve2d":
              dumpNurbCurveEdge(indent + 1, (NurbCurve2d)pEdge);
              break;
          }

          /******************************************************************/
          /* Common Edge Properties                                         */
          /******************************************************************/
          Interval interval = pEdge.GetInterval();
          writeLine(indent + 1, "Start Point", pEdge.EvaluatePoint(interval.LowerBound));
          writeLine(indent + 1, "End Point", pEdge.EvaluatePoint(interval.UpperBound));
          writeLine(indent + 1, "Closed", pEdge.IsClosed());
        }
      }
    }
    /************************************************************************/
    /* Convert the specified value to a LoopType string                     */
    /************************************************************************/
    string toLooptypeString(HatchLoopTypes loopType)
    {
      string retVal = "";
      if ((loopType & HatchLoopTypes.External) != 0)
        retVal = retVal + " | kExternal";

      if ((loopType & HatchLoopTypes.Polyline) != 0)
        retVal = retVal + " | kPolyline";

      if ((loopType & HatchLoopTypes.Derived) != 0)
        retVal = retVal + " | kDerived";

      if ((loopType & HatchLoopTypes.Textbox) != 0)
        retVal = retVal + " | kTextbox";

      if ((loopType & HatchLoopTypes.Outermost) != 0)
        retVal = retVal + " | kOutermost";

      if ((loopType & HatchLoopTypes.NotClosed) != 0)
        retVal = retVal + " | kNotClosed";

      if ((loopType & HatchLoopTypes.SelfIntersecting) != 0)
        retVal = retVal + " | kSelfIntersecting";

      if ((loopType & HatchLoopTypes.TextIsland) != 0)
        retVal = retVal + " | kTextIsland";

      if ((loopType & HatchLoopTypes.Duplicate) != 0)
        retVal = retVal + " | kDuplicate";

      return retVal == "" ? "kDefault" : retVal.Substring(3);
    }
    void dump(Hatch pHatch, int indent)
    {
      writeLine(indent++, pHatch.GetRXClass().Name, pHatch.Handle);
      writeLine(indent, "Hatch Style", pHatch.HatchStyle);
      writeLine(indent, "Hatch Object Type", pHatch.HatchObjectType);
      writeLine(indent, "Is Hatch", pHatch.IsHatch);
      writeLine(indent, "Is Gradient", !pHatch.IsGradient);
      if (pHatch.IsHatch)
      {
        /******************************************************************/
        /* Dump Hatch Parameters                                          */
        /******************************************************************/
        writeLine(indent, "Pattern Type", pHatch.PatternType);
        switch (pHatch.PatternType)
        {
          case HatchPatternType.PreDefined:
          case HatchPatternType.CustomDefined:
            writeLine(indent, "Pattern Name", pHatch.PatternName);
            writeLine(indent, "Solid Fill", pHatch.IsSolidFill);
            if (!pHatch.IsSolidFill)
            {
              writeLine(indent, "Pattern Angle", toDegreeString(pHatch.PatternAngle));
              writeLine(indent, "Pattern Scale", pHatch.PatternScale);
            }
            break;
          case HatchPatternType.UserDefined:
            writeLine(indent, "Pattern Angle", toDegreeString(pHatch.PatternAngle));
            writeLine(indent, "Pattern Double", pHatch.PatternDouble);
            writeLine(indent, "Pattern Space", pHatch.PatternSpace);
            break;
        }
        DBObjectCollection entitySet = new DBObjectCollection();
        Handle hhh = pHatch.Handle;
        if (hhh.Value == 1692) //69C)
        {
          pHatch.Explode(entitySet);
          return;
        }
        if (hhh.Value == 1693) //69D)
        {
          try
          {
            pHatch.Explode(entitySet);
          }
          catch (System.Exception e)
          {
            if (e.Message == "eCannotExplodeEntity")
            {
              writeLine(indent, "Hatch " + e.Message + ": ", pHatch.Handle);
              return;
            }
          }
        }
      }
      if (pHatch.IsGradient)
      {
        /******************************************************************/
        /* Dump Gradient Parameters                                       */
        /******************************************************************/
        writeLine(indent, "Gradient Type", pHatch.GradientType);
        writeLine(indent, "Gradient Name", pHatch.GradientName);
        writeLine(indent, "Gradient Angle", toDegreeString(pHatch.GradientAngle));
        writeLine(indent, "Gradient Shift", pHatch.GradientShift);
        writeLine(indent, "Gradient One-Color Mode", pHatch.GradientOneColorMode);
        if (pHatch.GradientOneColorMode)
        {
          writeLine(indent, "ShadeTintValue", pHatch.ShadeTintValue);
        }
        GradientColor[] colors = pHatch.GetGradientColors();
        for (int i = 0; i < colors.Length; i++)
        {
          writeLine(indent, string.Format("Color         {0}", i), colors[i].get_Color());
          writeLine(indent, string.Format("Interpolation {0}", i), colors[i].get_Value());
        }
      }

      /********************************************************************/
      /* Dump Associated Objects                                          */
      /********************************************************************/
      writeLine(indent, "Associated objects", pHatch.Associative);
      foreach (ObjectId id in pHatch.GetAssociatedObjectIds())
      {
        writeLine(indent + 1, id.ObjectClass.Name, id.Handle);
      }

      /********************************************************************/
      /* Dump Loops                                                       */
      /********************************************************************/
      writeLine(indent, "Loops", pHatch.NumberOfLoops);
      for (int i = 0; i < pHatch.NumberOfLoops; i++)
      {
        writeLine(indent + 1, "Loop " + i.ToString(), toLooptypeString(pHatch.LoopTypeAt(i)));

        /******************************************************************/
        /* Dump Loop                                                      */
        /******************************************************************/
        if ((pHatch.LoopTypeAt(i) & HatchLoopTypes.Polyline) != 0)
        {
          dumpPolylineType(i, pHatch, indent + 2);
        }
        else
        {
          dumpEdgesType(i, pHatch, indent + 2);
        }
        /******************************************************************/
        /* Dump Associated Objects                                        */
        /******************************************************************/
        if (pHatch.Associative)
        {
          writeLine(indent + 2, "Associated objects");
          foreach (ObjectId id in pHatch.GetAssociatedObjectIdsAt(i))
          {
            writeLine(indent + 3, id.ObjectClass.Name, id.Handle);
          }
        }
      }

      writeLine(indent, "Elevation", pHatch.Elevation);
      writeLine(indent, "Normal", pHatch.Normal);
      dumpEntityData(pHatch, indent);
    }

    /************************************************************************/
    /* Leader Dumper                                                          */
    /************************************************************************/
    void dump(Leader pLeader, int indent)
    {
      writeLine(indent++, pLeader.GetRXClass().Name, pLeader.Handle);
      writeLine(indent, "Dimension Style", pLeader.DimensionStyleName);

      writeLine(indent, "Annotation");
      if (!pLeader.Annotation.IsNull)
      {
        writeLine(indent++, pLeader.Annotation.ObjectClass.Name, pLeader.Annotation.Handle);
      }
      writeLine(indent + 1, "Type", pLeader.AnnoType);
      writeLine(indent + 1, "Height", pLeader.AnnoHeight);
      writeLine(indent + 1, "Width", pLeader.AnnoWidth);
      writeLine(indent + 1, "Offset", pLeader.AnnotationOffset);
      writeLine(indent, "Has Arrowhead", pLeader.HasArrowHead);
      writeLine(indent, "Has Hook Line", pLeader.HasHookLine);
      writeLine(indent, "Splined", pLeader.IsSplined);

      for (int i = 0; i < pLeader.NumVertices; i++)
      {
        writeLine(indent, string.Format("Vertex {0}", i), pLeader.VertexAt(i));
      }
      writeLine(indent, "Normal", pLeader.Normal);
      dumpCurveData(pLeader, indent);
    }

    /************************************************************************/
    /* Line Dumper                                                          */
    /************************************************************************/
    void dump(Line pLine, int indent)
    {
      writeLine(indent++, pLine.GetRXClass().Name, pLine.Handle);
      writeLine(indent, "Normal", pLine.Normal);
      writeLine(indent, "Thickness", pLine.Thickness);
      dumpEntityData(pLine, indent);
    }
    /************************************************************************/
    /* MInsertBlock Dumper                                                  */
    /************************************************************************/
    void dump(MInsertBlock pMInsert, int indent)
    {
      writeLine(indent++, pMInsert.GetRXClass().Name, pMInsert.Handle);

      using (BlockTableRecord pRecord = (BlockTableRecord)pMInsert.BlockTableRecord.Open(OpenMode.ForRead))
      {
        writeLine(indent, "Name", pRecord.Name);
        writeLine(indent, "Rows", pMInsert.Rows);
        writeLine(indent, "Columns", pMInsert.Columns);
        writeLine(indent, "Row Spacing", pMInsert.RowSpacing);
        writeLine(indent, "Column Spacing", pMInsert.ColumnSpacing);
        dumpBlockRefData(pMInsert, indent);
      }
    }

    /************************************************************************/
    /* Mline Dumper                                                         */
    /************************************************************************/
    void dump(Mline pMline, int indent)
    {
      writeLine(indent++, pMline.GetRXClass().Name, pMline.Handle);
      writeLine(indent, "Style", pMline.Style);
      writeLine(indent, "Closed", pMline.IsClosed);
      writeLine(indent, "Scale", pMline.Scale);
      writeLine(indent, "Suppress Start Caps", pMline.SupressStartCaps);
      writeLine(indent, "Suppress End Caps", pMline.SupressEndCaps);
      writeLine(indent, "Normal", pMline.Normal);

      /********************************************************************/
      /* Dump the segment data                                            */
      /********************************************************************/
      for (int i = 0; i < pMline.NumberOfVertices; i++)
      {
        writeLine(indent, "Segment", i);
        writeLine(indent + 1, "Vertex", pMline.VertexAt(i));
      }
      dumpEntityData(pMline, indent);
    }
    /************************************************************************/
    /* MText Dumper                                                         */
    /************************************************************************/
    void dump(MText pMText, int indent)
    {
      writeLine(indent++, pMText.GetRXClass().Name, pMText.Handle);
      writeLine(indent, "Contents", pMText.Contents);
      writeLine(indent, "Location", pMText.Location);
      writeLine(indent, "Height", pMText.TextHeight);
      writeLine(indent, "Rotation", toDegreeString(pMText.Rotation));
      writeLine(indent, "Text Style Id", pMText.TextStyleId);
      writeLine(indent, "Attachment", pMText.Attachment);
      writeLine(indent, "Background Fill On", pMText.BackgroundFill);
      writeLine(indent, "Background Fill Color", pMText.BackgroundFillColor);
      writeLine(indent, "Background Scale Factor", pMText.BackgroundScaleFactor);
      writeLine(indent, "Background Transparency Method", pMText.BackgroundTransparency);
      writeLine(indent, "X-Direction", pMText.Direction);
      writeLine(indent, "Flow Direction", pMText.FlowDirection);
      writeLine(indent, "Width", pMText.Width);
      writeLine(indent, "Actual Height", pMText.ActualHeight);
      writeLine(indent, "Actual Width", pMText.ActualWidth);

      Point3dCollection points = pMText.GetBoundingPoints();
      writeLine(indent, "TL Bounding Point", points[0]);
      writeLine(indent, "TR Bounding Point", points[1]);
      writeLine(indent, "BL Bounding Point", points[2]);
      writeLine(indent, "BR Bounding Point", points[3]);
      writeLine(indent, "Normal", pMText.Normal);

      dumpEntityData(pMText, indent);
    }

    /************************************************************************/
    /* Ordinate Dimension Dumper                                            */
    /************************************************************************/
    void dump(OrdinateDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Defining Point", pDim.DefiningPoint);
      writeLine(indent, "Using x-Axis", pDim.UsingXAxis);
      writeLine(indent, "Using y-Axis", pDim.UsingYAxis);
      writeLine(indent, "Leader End Point", pDim.LeaderEndPoint);
      writeLine(indent, "Origin", pDim.Origin);
      dumpDimData(pDim, indent);
    }
    /************************************************************************/
    /* PolyFaceMesh Dumper                                                  */
    /************************************************************************/
    void dump(PolyFaceMesh pPoly, int indent)
    {
      writeLine(indent++, pPoly.GetRXClass().Name, pPoly.Handle);
      writeLine(indent, "Number of Vertices", pPoly.NumVertices);
      writeLine(indent, "Number of Faces", pPoly.NumFaces);

      /********************************************************************/
      /* dump vertices and faces                                          */
      /********************************************************************/
      int vertexCount = 0;
      int faceCount = 0;
      foreach (ObjectId objId in pPoly)
      {
        using (Entity ent = (Entity)objId.GetObject(OpenMode.ForRead))
        {
          if (ent is PolyFaceMeshVertex)
          {
            PolyFaceMeshVertex pVertex = (PolyFaceMeshVertex)ent;
            writeLine(indent, pVertex.GetRXClass().Name, ++vertexCount);
            writeLine(indent + 1, "Handle", pVertex.Handle);
            writeLine(indent + 1, "Position", pVertex.Position);
            dumpEntityData(pVertex, indent + 1);
          }
          else if (ent is FaceRecord)
          {
            FaceRecord pFace = (FaceRecord)ent;
            string face = "{";
            for (short i = 0; i < 4; i++)
            {
              if (i > 0)
              {
                face = face + " ";
              }
              face = face + pFace.GetVertexAt(i).ToString();
            }
            face += "}";
            writeLine(indent, pFace.GetRXClass().Name, ++faceCount);
            writeLine(indent + 1, "Handle", pFace.Handle);
            writeLine(indent + 1, "Vertices", face);
            dumpEntityData(pFace, indent + 1);
          }
          else
          { // Unknown entity type
            writeLine(indent, "Unexpected Entity");
          }
        }
      }
      dumpEntityData(pPoly, indent);
    }

    /************************************************************************/
    /* Ole2Frame                                                            */
    /************************************************************************/
    void dump(Ole2Frame pOle, int indent)
    {
      writeLine(indent++, pOle.GetRXClass().Name, pOle.Handle);

      Rectangle3d pos = (Rectangle3d)pOle.Position3d;
      writeLine(indent, "Lower Left", pos.LowerLeft);
      writeLine(indent, "Lower Right", pos.LowerRight);
      writeLine(indent, "Upper Left", pos.UpperLeft);
      writeLine(indent, "Upper Right", pos.UpperRight);
      writeLine(indent, "Type", pOle.Type);
      writeLine(indent, "User Type", pOle.UserType);
      if (pOle.Type == Ole2Frame.ItemType.Link)
      {
        writeLine(indent, "Link Name", pOle.LinkName);
        writeLine(indent, "Link Path", pOle.LinkPath);
      }
      writeLine(indent, "Output Quality", pOle.OutputQuality);
      dumpEntityData(pOle, indent);
    }

    /************************************************************************/
    /* Point Dumper                                                         */
    /************************************************************************/
    void dump(DBPoint pPoint, int indent)
    {
      writeLine(indent++, pPoint.GetRXClass().Name, pPoint.Handle);
      writeLine(indent, "Position", pPoint.Position);
      writeLine(indent, "ECS Rotation", toDegreeString(pPoint.EcsRotation));
      writeLine(indent, "Normal", pPoint.Normal);
      writeLine(indent, "Thickness", pPoint.Thickness);
      dumpEntityData(pPoint, indent);
    }

    /************************************************************************/
    /* Polygon Mesh Dumper                                                  */
    /************************************************************************/
    void dump(PolygonMesh pPoly, int indent)
    {
      writeLine(indent++, pPoly.GetRXClass().Name, pPoly.Handle);
      writeLine(indent, "m Size", pPoly.MSize);
      writeLine(indent, "m-Closed", pPoly.IsMClosed);
      writeLine(indent, "m Surface Density", pPoly.MSurfaceDensity);
      writeLine(indent, "n Size", pPoly.NSize);
      writeLine(indent, "n-Closed", pPoly.IsNClosed);
      writeLine(indent, "n Surface Density", pPoly.NSurfaceDensity);
      /********************************************************************/
      /* dump vertices                                                    */
      /********************************************************************/
      int vertexCount = 0;
      foreach (object o in pPoly)
      {
        PolygonMeshVertex pVertex = o as PolygonMeshVertex;
        if (pVertex != null)
        {
          writeLine(indent, pVertex.GetRXClass().Name, vertexCount++);
          writeLine(indent + 1, "Handle", pVertex.Handle);
          writeLine(indent + 1, "Position", pVertex.Position);
          writeLine(indent + 1, "Type", pVertex.VertexType);
        }
      }
      dumpEntityData(pPoly, indent);
    }

    /************************************************************************/
    /* Polyline Dumper                                                      */
    /************************************************************************/
    void dump(Teigha.DatabaseServices.Polyline pPoly, int indent)
    {
      writeLine(indent++, pPoly.GetRXClass().Name, pPoly.Handle);
      writeLine(indent, "Has Width", pPoly.HasWidth);
      if (!pPoly.HasWidth)
      {
        writeLine(indent, "Constant Width", pPoly.ConstantWidth);
      }
      writeLine(indent, "Has Bulges", pPoly.HasBulges);
      writeLine(indent, "Elevation", pPoly.Elevation);
      writeLine(indent, "Normal", pPoly.Normal);
      writeLine(indent, "Thickness", pPoly.Thickness);

      /********************************************************************/
      /* dump vertices                                                    */
      /********************************************************************/
      for (int i = 0; i < pPoly.NumberOfVertices; i++)
      {
        writeLine(indent, string.Format("Vertex {0}", i));
        writeLine(indent + 1, "Segment Type", pPoly.GetSegmentType(i));
        writeLine(indent + 1, "Point", pPoly.GetPoint3dAt(i));
        if (pPoly.HasWidth)
        {
          writeLine(indent, "Start Width", pPoly.GetStartWidthAt(i));
          writeLine(indent, "End Width", pPoly.GetEndWidthAt(i));
        }
        if (pPoly.HasBulges)
        {
          writeLine(indent, "Bulge", pPoly.GetBulgeAt(i));
          if (pPoly.GetSegmentType(i) == SegmentType.Arc)
          {
            writeLine(indent, "Bulge Angle", toDegreeString(4 * Math.Atan(pPoly.GetBulgeAt(i))));
          }
        }
      }
      dumpEntityData(pPoly, indent);
    }
    class DrawContextDumper : Context
    {
      Database _db;
      public DrawContextDumper(Database db)
      {
        _db = db;
      }
      public override Database Database
      {
        get { return _db; }
      }
      public override bool IsBoundaryClipping
      {
        get { return false; }
      }
      public override bool IsPlotGeneration
      {
        get { return false; }
      }
      public override bool IsPostScriptOut
      {
        get { return false; }
      }
    }
    class SubEntityTraitsDumper : SubEntityTraits
    {
      short _color;
      int _drawFlags;
      FillType _ft;
      ObjectId _layer;
      ObjectId _linetype;
      LineWeight _lineWeight;
      Mapper _mapper;
      double _lineTypeScale;
      ObjectId _material;
      PlotStyleDescriptor _plotStyleDescriptor;
      bool _sectionable;
      bool _selectionOnlyGeometry;
      ShadowFlags _shadowFlags;
      double _thickness;
      EntityColor _trueColor;
      Transparency _transparency;
      ObjectId _visualStyle;
      public SubEntityTraitsDumper(Database db)
      {
        _drawFlags = 0; // kNoDrawFlags 
        _color = 0;
        _ft = FillType.FillAlways;
        _layer = db.Clayer;
        _linetype = db.Celtype;
        _lineWeight = db.Celweight;
        _lineTypeScale = db.Celtscale;
        _material = db.Cmaterial;
        _shadowFlags = ShadowFlags.ShadowsIgnore;
        _thickness = 0;
        _trueColor = new EntityColor(ColorMethod.None);
        _transparency = new Transparency();
      }
      
      protected override void SetLayerFlags(LayerFlags flags)
      {
        writeLine(0, string.Format("SubEntityTraitsDumper.SetLayerFlags(flags = {0})", flags));
      }
      public override void AddLight(ObjectId lightId)
      {
        writeLine(0, string.Format("SubEntityTraitsDumper.AddLight(lightId = {0})", lightId.ToString()));
      }
      public override void SetupForEntity(Entity entity)
      {
        writeLine(0, string.Format("SubEntityTraitsDumper.SetupForEntity(entity = {0})", entity.ToString()));
      }

      public override short Color
      {
        get { return _color; }
        set { _color = value; }
      }
      public override int DrawFlags
      {
        get { return _drawFlags; }
        set { _drawFlags = value; }
      }
      public override FillType FillType
      {
        get { return _ft; }
        set { _ft = value; }
      }
      public override ObjectId Layer
      {
        get { return _layer; }
        set { _layer = value; }
      }
      public override ObjectId LineType
      {
        get { return _linetype; }
        set { _linetype = value; }
      }
      public override double LineTypeScale
      {
        get { return _lineTypeScale; }
        set { _lineTypeScale = value; }
      }
      public override LineWeight LineWeight
      {
        get { return _lineWeight; }
        set { _lineWeight = value; }
      }
      public override Mapper Mapper
      {
        get { return _mapper; }
        set { _mapper = value; }
      }
      public override ObjectId Material
      {
        get { return _material; }
        set { _material = value; }
      }
      public override PlotStyleDescriptor PlotStyleDescriptor
      {
        get { return _plotStyleDescriptor; }
        set { _plotStyleDescriptor = value; }
      }
      public override bool Sectionable
      {
        get { return _sectionable; }
        set { _sectionable = value; }
      }
      public override bool SelectionOnlyGeometry
      {
        get { return _selectionOnlyGeometry; }
        set { _selectionOnlyGeometry = value; }
      }
      public override ShadowFlags ShadowFlags
      {
        get { return _shadowFlags; }
        set { _shadowFlags = value; }
      }
      public override double Thickness
      {
        get { return _thickness; }
        set { _thickness = value; }
      }
      public override EntityColor TrueColor
      {
        get { return _trueColor; }
        set { _trueColor = value; }
      }
      public override Transparency Transparency
      {
        get { return _transparency; }
        set { _transparency = value; }
      }
      public override ObjectId VisualStyle
      {
        get { return _visualStyle; }
        set { _visualStyle = value; }
      }
      public override void SetSelectionMarker(IntPtr sm)
      {
      }
    }
    class WorldGeometryDumper : WorldGeometry
    {
      Stack<Matrix3d> modelMatrix;
      Stack<ClipBoundary> clips;
      int indent;
      public WorldGeometryDumper(int indent)
        : base()
      {
        this.indent = indent;
        modelMatrix = new Stack<Matrix3d>();
        clips = new Stack<ClipBoundary>();
        modelMatrix.Push(Matrix3d.Identity);
      }
      public override Matrix3d ModelToWorldTransform
      {
        get { return modelMatrix.Peek(); }
      }
      public override Matrix3d WorldToModelTransform
      {
        get { return modelMatrix.Peek().Inverse(); }
      }

      public override Matrix3d PushOrientationTransform(OrientationBehavior behavior)
      {
        writeLine(indent, string.Format("WorldGeometry.PushOrientationTransform(behavior = {0})", behavior));
        return new Matrix3d();
      }
      public override Matrix3d PushPositionTransform(PositionBehavior behavior, Point2d offset)
      {
        writeLine(indent, string.Format("WorldGeometry.PushPositionTransform(behavior = {0}, offset = {1})", behavior, offset));
        return new Matrix3d();
      }
      public override Matrix3d PushPositionTransform(PositionBehavior behavior, Point3d offset)
      {
        writeLine(indent, string.Format("WorldGeometry.PushPositionTransform(behavior = {0}, offset = {1})", behavior, offset));
        return new Matrix3d();
      }
      public override bool OwnerDraw(GdiDrawObject gdiDrawObject, Point3d position, Vector3d u, Vector3d v)
      {
        writeLine(indent, string.Format("WorldGeometry.OwnerDraw(gdiDrawObject = {0}, position = {1}, u = {2}, v = {3})", gdiDrawObject, position, u, v));
        return false;
      }
      public override bool Polyline(Teigha.GraphicsInterface.Polyline polylineObj)
      {
        writeLine(indent, string.Format("WorldGeometry.Polyline(value = {0}", polylineObj));
        return false;
      }
      public override bool Polypoint(Point3dCollection points, Vector3dCollection normals, IntPtrCollection subentityMarkers)
      {
        writeLine(indent, string.Format("WorldGeometry.Polypoint(points = {0}, normals = {1}, subentityMarkers = {2}", points, normals, subentityMarkers));
        return false;
      }
      public override bool Polypoint(Point3dCollection points, EntityColorCollection colors, Vector3dCollection normals, IntPtrCollection subentityMarkers)
      {
        writeLine(indent, string.Format("WorldGeometry.Polypoint(points = {0}, colors = {1}, normals = {2}, subentityMarkers = {3}", points, colors, normals, subentityMarkers));
        return false;
      }
      public override bool Polypoint(Point3dCollection points, EntityColorCollection colors, TransparencyCollection transparency, Vector3dCollection normals, IntPtrCollection subentityMarkers, int pointSize)
      {
        writeLine(indent, string.Format("WorldGeometry.Polypoint(points = {0}, colors = {1}, transparency = {2}, normals = {3}, subentityMarkers = {4}, pointSize = {5}", points, colors, transparency, normals, subentityMarkers, pointSize));
        return false;
      }
      public override bool PolyPolyline(Teigha.GraphicsInterface.PolylineCollection polylineCollection)
      {
        writeLine(indent, string.Format("WorldGeometry.PolyPolyline(polylineCollection = {0}", polylineCollection));
        return false;
      }
      public override bool PolyPolygon(UInt32Collection numPolygonPositions, Point3dCollection polygonPositions, UInt32Collection numPolygonPoints, Point3dCollection polygonPoints, EntityColorCollection outlineColors, LinetypeCollection outlineTypes, EntityColorCollection fillColors, Teigha.Colors.TransparencyCollection fillOpacities)
      {
        writeLine(indent, string.Format("WorldGeometry.PolyPolygon(numPolygonPositions = {0}, polygonPositions = {1}, numPolygonPoints = {2}, polygonPoints = {3}, outlineColors = {4}, outlineTypes = {5}, fillColors = {6}, fillOpacities = {7})", numPolygonPositions, polygonPositions, numPolygonPoints, polygonPoints, outlineColors, outlineTypes, fillColors, fillOpacities));
        return false;
      }
      public override Matrix3d PushScaleTransform(ScaleBehavior behavior, Point2d extents)
      {
        writeLine(indent, string.Format("WorldGeometry.PushScaleTransform(behavior = {0}, extents = {1})", behavior, extents));
        return new Matrix3d();
      }
      public override Matrix3d PushScaleTransform(ScaleBehavior behavior, Point3d extents)
      {
        writeLine(indent, string.Format("WorldGeometry.PushScaleTransform(behavior = {0}, extents = {1})", behavior, extents));
        return new Matrix3d();
      }
      public override bool EllipticalArc(Point3d center, Vector3d normal, double majorAxisLength, double minorAxisLength, double startDegreeInRads, double endDegreeInRads, double tiltDegreeInRads, ArcType arType)
      {
        writeLine(indent, string.Format("WorldGeometry.EllipticalArc(center = {0}, normal = {1}, majorAxisLength = {2}, minorAxisLength = {3}, startDegreeInRads = {4}, endDegreeInRads = {5}, tiltDegreeInRads = {6}, arType = {7}", center, normal, majorAxisLength, minorAxisLength, startDegreeInRads, endDegreeInRads, tiltDegreeInRads, arType));
        return false;
      }
      public override bool Circle(Point3d center, double radius, Vector3d normal)
      {
        writeLine(indent, string.Format("WorldGeometry.Circle(center = {0}, radius = {1}, normal = {2})", center, radius, normal));
        return false;
      }
      public override bool Circle(Point3d firstPoint, Point3d secondPoint, Point3d thirdPoint)
      {
        writeLine(indent, string.Format("WorldGeometry.Circle(firstPoint = {0}, secondPoint = {1}, thirdPoint = {2})", firstPoint, secondPoint, thirdPoint));
        return false;
      }
      public override bool CircularArc(Point3d start, Point3d point, Point3d endingPoint, ArcType arcType)
      {
        writeLine(indent, string.Format("WorldGeometry.CircularArc(start = {0}, point = {1}, endingPoint = {2}, arcType = {3})", start, point, endingPoint, arcType));
        return false;
      }
      public override bool CircularArc(Point3d center, double radius, Vector3d normal, Vector3d startVector, double sweepAngle, ArcType arcType)
      {
        writeLine(indent, string.Format("WorldGeometry.CircularArc(center = {0}, radius = {1}, normal = {2}, startVector = {3}, sweepAngle = {4}, arcType = {5}", center, radius, normal, startVector, sweepAngle, arcType));
        return false;
      }
      public override bool Draw(Drawable value)
      {
        writeLine(indent, string.Format("WorldGeometry.Draw(value = {0}", value));
        return false;
      }
      public override bool Image(ImageBGRA32 imageSource, Point3d position, Vector3d u, Vector3d v)
      {
        writeLine(indent, string.Format("WorldGeometry.Image(imageSource = , position = {1}, Vector3d = {2}, Vector3d = {3}", position, u, v));
        return false;
      }
      public override bool Image(ImageBGRA32 imageSource, Point3d position, Vector3d u, Vector3d v, TransparencyMode transparencyMode)
      {
        writeLine(indent, string.Format("WorldGeometry.Image(imageSource = , position = {1}, Vector3d = {2}, Vector3d = {3}, transparencyMode = {4}", position, u, v, transparencyMode));
        return false;
      }
      public override bool Mesh(int rows, int columns, Point3dCollection points, EdgeData edgeData, FaceData faceData, VertexData vertexData, bool bAutoGenerateNormals)
      {
        writeLine(indent, string.Format("WorldGeometry.Mesh(rows = {0}, columns = {1}, points = {2}, edgeData = {3}, faceData = {4}, vertexData = {5}, bAutoGenerateNormals = {6})", rows, columns, points, edgeData, faceData, vertexData, bAutoGenerateNormals));
        return false;
      }
      public override bool Polygon(Point3dCollection points)
      {
        writeLine(indent, string.Format("WorldGeometry.Polygon(points = {0})", points));
        return false;
      }
      public override bool Polyline(Teigha.DatabaseServices.Polyline value, int fromIndex, int segments)
      {
        writeLine(indent, string.Format("WorldGeometry.Polyline(value = {0}, fromIndex = {1}, segments = {2})", value, fromIndex, segments));
        return false;
      }
      public override bool Polyline(Point3dCollection points, Vector3d normal, IntPtr subEntityMarker)
      {
        writeLine(indent, string.Format("WorldGeometry.Polyline(points = {0}, normal = {1}, subEntityMarker = {2})", points, normal, subEntityMarker));
        return false;
      }
      public override void PopClipBoundary()
      {
        writeLine(indent, string.Format("WorldGeometry.PopClipBoundary"));
        clips.Pop();
      }
      public override bool PopModelTransform()
      {
        return true;
      }
      public override bool PushClipBoundary(ClipBoundary boundary)
      {
        writeLine(indent, string.Format("WorldGeometry.PushClipBoundary"));
        clips.Push(boundary);
        return true;
      }
      public override bool PushModelTransform(Matrix3d matrix)
      {
        writeLine(indent, "WorldGeometry.PushModelTransform(Matrix3d)");
        Matrix3d m = modelMatrix.Peek();
        modelMatrix.Push(m * matrix);
        return true;
      }
      public override bool PushModelTransform(Vector3d normal)
      {
        writeLine(indent, "WorldGeometry.PushModelTransform(Vector3d)");
        PushModelTransform(Matrix3d.PlaneToWorld(normal));
        return true;
      }
      public override bool RowOfDots(int count, Point3d start, Vector3d step)
      {
        writeLine(indent, string.Format("ViewportGeometry.RowOfDots(count = {0}, start = {1}, step = {1})", count, start, step));
        return false;
      }
      public override bool Ray(Point3d point1, Point3d point2)
      {
        writeLine(indent, string.Format("WorldGeometry.Ray(point1 = {0}, point2 = {1})", point1, point2));
        return false;
      }
      public override bool Shell(Point3dCollection points, IntegerCollection faces, EdgeData edgeData, FaceData faceData, VertexData vertexData, bool bAutoGenerateNormals)
      {
        writeLine(indent, string.Format("WorldGeometry.Shell(points = {0}, faces = {1}, edgeData = {2}, faceData = {3}, vertexData = {4}, bAutoGenerateNormals = {5})", points, faces, edgeData, faceData, vertexData, bAutoGenerateNormals));
        return false;
      }
      public override bool Text(Point3d position, Vector3d normal, Vector3d direction, string message, bool raw, TextStyle textStyle)
      {
        writeLine(indent, string.Format("WorldGeometry.Text(position = {0}, normal = {1}, direction = {2}, message = {3}, raw = {4}, textStyle = {5})", position, normal, direction, message, raw, textStyle));
        return false;
      }
      public override bool Text(Point3d position, Vector3d normal, Vector3d direction, double height, double width, double oblique, string message)
      {
        writeLine(indent, string.Format("WorldGeometry.Text(position = {0}, normal = {1}, direction = {2}, height = {3}, width = {4}, oblique = {5}, message = {6})", position, normal, direction, height, width, oblique, message));
        return false;
      }
      public override bool WorldLine(Point3d startPoint, Point3d endPoint)
      {
        writeLine(indent, string.Format("WorldGeometry.WorldLine(startPoint = {0}, endPoint = {1})", startPoint, endPoint));
        return false;
      }
      public override bool Xline(Point3d point1, Point3d point2)
      {
        writeLine(indent, string.Format("WorldGeometry.Xline(point1 = {0}, point2 = {1})", point1, point2));
        return false;
      }

      public override void SetExtents(Extents3d extents)
      {
        writeLine(indent, "WorldGeometry.SetExtents({0}) ", extents);
      }
      public override void StartAttributesSegment()
      {
        writeLine(indent, "WorldGeometry.StartAttributesSegment called");
      }
    }

    class WorldDrawDumper : WorldDraw
    {
      WorldGeometryDumper _geom;
      DrawContextDumper _ctx;
      SubEntityTraits _subents;
      RegenType _regenType;
      int indent;
      public WorldDrawDumper(Database db, int indent)
        : base()
      {
        _regenType = RegenType;
        this.indent = indent;
        _geom = new WorldGeometryDumper(indent);
        _ctx = new DrawContextDumper(db);
        _subents = new SubEntityTraitsDumper(db);
      }
      public override double Deviation(DeviationType deviationType, Point3d pointOnCurve)
      {
        return 1e-9;
      }
      public override WorldGeometry Geometry
      {
        get
        {
          return _geom;
        }
      }
      public override bool IsDragging
      {
        get
        {
          return false;
        }
      }
      public override Int32 NumberOfIsolines
      {
        get
        {
          return 10;
        }
      }
      public override Geometry RawGeometry
      {
        get
        {
          return _geom;
        }
      }
      public override bool RegenAbort
      {
        get
        {
          return false;
        }
      }
      public override RegenType RegenType
      {
        get
        {
          writeLine(indent, "RegenType is asked");
          return _regenType;
        }
      }
      public override SubEntityTraits SubEntityTraits
      {
        get
        {
          return _subents;
        }
      }
      public override Context Context
      {
        get
        {
          return _ctx;
        }
      }
    }

    /************************************************************************/
    /* Dump the common data and WorldDraw information for all               */
    /* entities without explicit dumpers                                    */
    /************************************************************************/
    void dump(Entity pEnt, int indent)
    {
      writeLine(indent++, pEnt.GetRXClass().Name, pEnt.Handle);
      dumpEntityData(pEnt, indent);
      writeLine(indent, "WorldDraw()");
      using (Database db = pEnt.Database)
      {
        /**********************************************************************/
        /* Create an OdGiWorldDraw instance for the vectorization             */
        /**********************************************************************/
        WorldDrawDumper wd = new WorldDrawDumper(db, indent + 1);
        /**********************************************************************/
        /* Call worldDraw()                                                   */
        /**********************************************************************/
        pEnt.WorldDraw(wd);
      }
    }

    /************************************************************************/
    /* Proxy Entity Dumper                                                  */
    /************************************************************************/
    void dump(ProxyEntity pProxy, int indent)
    {
      // this will dump proxy entity graphics
      dump((Entity)pProxy, indent);
      writeLine(indent, "Proxy OriginalClassName", pProxy.OriginalClassName);

      DBObjectCollection collection = new DBObjectCollection(); ;
      try
      {
        pProxy.ExplodeGeometry(collection);
      }
      catch (System.Exception ex)
      {
        return;
      }
      foreach (Entity ent in collection)
      {
        if (ent is Polyline2d)
        {
          Polyline2d pline2d = (Polyline2d)ent;
          int i = 0;

          try
          {
            foreach (Entity ent1 in pline2d)
            {
              if (ent1 is Vertex2d)
              {
                Vertex2d vtx2d = (Vertex2d)ent1;
                dump2dVertex(indent, vtx2d, i++);
              }
            }
          }
          catch (System.Exception ex)
          {
            return;
          }
        }
      }
    }

    /************************************************************************/
    /* Radial Dimension Dumper                                              */
    /************************************************************************/
    void dump(RadialDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Center", pDim.Center);
      writeLine(indent, "Chord Point", pDim.ChordPoint);
      writeLine(indent, "Leader Length", pDim.LeaderLength);
      dumpDimData(pDim, indent);
    }
    /************************************************************************/
    /* Dump Raster Image Def                                               */
    /************************************************************************/
    void dumpRasterImageDef(ObjectId id, int indent)
    {
      if (!id.IsValid)
        return;
      using (RasterImageDef pDef = (RasterImageDef)id.Open(OpenMode.ForRead))
      {
        writeLine(indent++, pDef.GetRXClass().Name, pDef.Handle);
        writeLine(indent, "Source Filename", shortenPath(pDef.SourceFileName));
        writeLine(indent, "Loaded", pDef.IsLoaded);
        writeLine(indent, "mm per Pixel", pDef.ResolutionMMPerPixel);
        writeLine(indent, "Loaded", pDef.IsLoaded);
        writeLine(indent, "Resolution Units", pDef.ResolutionUnits);
        writeLine(indent, "Size", pDef.Size);
      }
    }
    /************************************************************************/
    /* Dump Raster Image Data                                               */
    /************************************************************************/
    void dumpRasterImageData(RasterImage pImage, int indent)
    {
      writeLine(indent, "Brightness", pImage.Brightness);
      writeLine(indent, "Clipped", pImage.IsClipped);
      writeLine(indent, "Contrast", pImage.Contrast);
      writeLine(indent, "Fade", pImage.Fade);
      writeLine(indent, "kClip", pImage.DisplayOptions & ImageDisplayOptions.Clip);
      writeLine(indent, "kShow", pImage.DisplayOptions & ImageDisplayOptions.Show);
      writeLine(indent, "kShowUnAligned", pImage.DisplayOptions & ImageDisplayOptions.ShowUnaligned);
      writeLine(indent, "kTransparent", pImage.DisplayOptions & ImageDisplayOptions.Transparent);
      writeLine(indent, "Scale", pImage.Scale);

      /********************************************************************/
      /* Dump clip boundary                                               */
      /********************************************************************/
      if (pImage.IsClipped)
      {
        writeLine(indent, "Clip Boundary Type", pImage.ClipBoundaryType);
        if (pImage.ClipBoundaryType != ClipBoundaryType.Invalid)
        {
          Point2dCollection pt = pImage.GetClipBoundary();
          for (int i = 0; i < pt.Count; i++)
          {
            writeLine(indent, string.Format("Clip Point {0}", i), pt[i]);
          }
        }
      }

      /********************************************************************/
      /* Dump frame                                                       */
      /********************************************************************/
      Point3dCollection vertices = pImage.GetVertices();
      for (int i = 0; i < vertices.Count; i++)
      {
        writeLine(indent, "Frame Vertex " + i.ToString(), vertices[i]);
      }

      /********************************************************************/
      /* Dump orientation                                                 */
      /********************************************************************/
      writeLine(indent, "Orientation");
      writeLine(indent + 1, "Origin", pImage.Orientation.Origin);
      writeLine(indent + 1, "uVector", pImage.Orientation.Xaxis);
      writeLine(indent + 1, "vVector", pImage.Orientation.Yaxis);
      dumpRasterImageDef(pImage.ImageDefId, indent);
      dumpEntityData(pImage, indent);
    }

    /************************************************************************/
    /* Raster Image Dumper                                                  */
    /************************************************************************/
    void dump(RasterImage pImage, int indent)
    {
      writeLine(indent++, pImage.GetRXClass().Name, pImage.Handle);
      writeLine(indent, "Image size", pImage.ImageSize(true));
      dumpRasterImageData(pImage, indent);
    }

    /************************************************************************/
    /* Ray Dumper                                                          */
    /************************************************************************/
    void dump(Ray pRay, int indent)
    {
      writeLine(indent++, pRay.GetRXClass().Name, pRay.Handle);
      writeLine(indent, "Base Point", pRay.BasePoint);
      writeLine(indent, "Unit Direction", pRay.UnitDir);
      dumpCurveData(pRay, indent);
    }

    /************************************************************************/
    /* Region Dumper                                                        */
    /************************************************************************/
    void dump(Region pRegion, int indent)
    {
      writeLine(indent++, pRegion.GetRXClass().Name, pRegion.Handle);
      dumpEntityData(pRegion, indent);
    }
    /************************************************************************/
    /* Rotated Dimension Dumper                                             */
    /************************************************************************/
    void dump(RotatedDimension pDim, int indent)
    {
      writeLine(indent++, pDim.GetRXClass().Name, pDim.Handle);
      writeLine(indent, "Dimension Line Point", pDim.DimLinePoint);
      writeLine(indent, "Oblique", toDegreeString(pDim.Oblique));
      writeLine(indent, "Rotation", toDegreeString(pDim.Rotation));
      writeLine(indent, "Extension Line 1 Point", pDim.XLine1Point);
      writeLine(indent, "Extension Line 2 Point", pDim.XLine2Point);
      dumpDimData(pDim, indent);
    }
    /************************************************************************/
    /* Shape Dumper                                                          */
    /************************************************************************/
    void dump(Shape pShape, int indent)
    {
      writeLine(indent++, pShape.GetRXClass().Name, pShape.Handle);

      if (!pShape.StyleId.IsNull)
      {
        using (TextStyleTableRecord pStyle = (TextStyleTableRecord)pShape.StyleId.Open(OpenMode.ForRead))
          writeLine(indent, "Filename", shortenPath(pStyle.FileName));
      }

      writeLine(indent, "Shape Number", pShape.ShapeNumber);
      writeLine(indent, "Shape Name", pShape.Name);
      writeLine(indent, "Position", pShape.Position);
      writeLine(indent, "Size", pShape.Size);
      writeLine(indent, "Rotation", toDegreeString(pShape.Rotation));
      writeLine(indent, "Oblique", toDegreeString(pShape.Oblique));
      writeLine(indent, "Normal", pShape.Normal);
      writeLine(indent, "Thickness", pShape.Thickness);
      dumpEntityData(pShape, indent);
    }

    /************************************************************************/
    /* Solid Dumper                                                         */
    /************************************************************************/
    // TODO:
    /*  void dump(Solid pSolid, int indent)
  {
    writeLine(indent++, pSolid.GetRXClass().Name, pSolid.Handle);

    for (int i = 0; i < 4; i++)
    {
      writeLine(indent, "Point " + i.ToString(),  pSolid .GetPointAt(i));
    }
    dumpEntityData(pSolid, indent);
  }
*/
    /************************************************************************/
    /* Spline Dumper                                                        */
    /************************************************************************/
    void dump(Spline pSpline, int indent)
    {
      writeLine(indent++, pSpline.GetRXClass().Name, pSpline.Handle);

      NurbsData data = pSpline.NurbsData;
      writeLine(indent, "Degree", data.Degree);
      writeLine(indent, "Rational", data.Rational);
      writeLine(indent, "Periodic", data.Periodic);
      writeLine(indent, "Control Point Tolerance", data.ControlPointTolerance);
      writeLine(indent, "Knot Tolerance", data.KnotTolerance);

      writeLine(indent, "Number of control points", data.GetControlPoints().Count);
      for (int i = 0; i < data.GetControlPoints().Count; i++)
      {
        writeLine(indent, "Control Point " + i.ToString(), data.GetControlPoints()[i]);
      }

      writeLine(indent, "Number of Knots", data.GetKnots().Count);
      for (int i = 0; i < data.GetKnots().Count; i++)
      {
        writeLine(indent, "Knot " + i.ToString(), data.GetKnots()[i]);
      }

      if (data.Rational)
      {
        writeLine(indent, "Number of Weights", data.GetWeights().Count);
        for (int i = 0; i < data.GetWeights().Count; i++)
        {
          writeLine(indent, "Weight " + i.ToString(), data.GetWeights()[i]);
        }
      }
      dumpCurveData(pSpline, indent);
    }
    /************************************************************************/
    /* Table Dumper                                                         */
    /************************************************************************/
    void dump(Table pTable, int indent)
    {
      writeLine(indent++, pTable.GetRXClass().Name, pTable.Handle);
      writeLine(indent, "Position", pTable.Position);
      writeLine(indent, "X-Direction", pTable.Direction);
      writeLine(indent, "Normal", pTable.Normal);
      writeLine(indent, "Height", (int)pTable.Height);
      writeLine(indent, "Width", (int)pTable.Width);
      writeLine(indent, "Rows", (int)pTable.NumRows);
      writeLine(indent, "Columns", (int)pTable.NumColumns);

      // TODO:
      //TableStyle pStyle = (TableStyle)pTable.TableStyle.Open(OpenMode.ForRead);
      //writeLine(indent, "Table Style",               pStyle.Name);
      dumpEntityData(pTable, indent);
    }
    /************************************************************************/
    /* Text Dumper                                                          */
    /************************************************************************/
    void dump(DBText pText, int indent)
    {
      writeLine();
      writeLine(indent++, pText.GetRXClass().Name, pText.Handle);
      dumpTextData(pText, indent);
    }
    /************************************************************************/
    /* Trace Dumper                                                         */
    /************************************************************************/
    void dump(Trace pTrace, int indent)
    {
      writeLine(indent++, pTrace.GetRXClass().Name, pTrace.Handle);

      for (short i = 0; i < 4; i++)
      {
        writeLine(indent, "Point " + i.ToString(), pTrace.GetPointAt(i));
      }
      dumpEntityData(pTrace, indent);
    }

    /************************************************************************/
    /* Trace UnderlayReference                                                         */
    /************************************************************************/
    void dump(UnderlayReference pEnt, int indent)
    {
      writeLine(indent++, pEnt.GetRXClass().Name, pEnt.Handle);
      writeLine(indent, "UnderlayReference Path ", pEnt.Path);
      writeLine(indent, "UnderlayReference Position ", pEnt.Position);
    }

    /************************************************************************/
    /* Viewport Dumper                                                       */
    /************************************************************************/
    void dump(Teigha.DatabaseServices.Viewport pVport, int indent)
    {
      writeLine(indent++, pVport.GetRXClass().Name, pVport.Handle);
      writeLine(indent, "Back Clip Distance", pVport.BackClipDistance);
      writeLine(indent, "Back Clip On", pVport.BackClipOn);
      writeLine(indent, "Center Point", pVport.CenterPoint);
      writeLine(indent, "Circle sides", pVport.CircleSides);
      writeLine(indent, "Custom Scale", pVport.CustomScale);
      writeLine(indent, "Elevation", pVport.Elevation);
      writeLine(indent, "Front Clip at Eye", pVport.FrontClipAtEyeOn);
      writeLine(indent, "Front Clip Distance", pVport.FrontClipDistance);
      writeLine(indent, "Front Clip On", pVport.FrontClipOn);
      writeLine(indent, "Plot style sheet", pVport.EffectivePlotStyleSheet);

      ObjectIdCollection layerIds = pVport.GetFrozenLayers();
      if (layerIds.Count > 0)
      {
        writeLine(indent, "Frozen Layers:");
        for (int i = 0; i < layerIds.Count; i++)
        {
          writeLine(indent + 1, i, layerIds[i]);
        }
      }
      else
      {
        writeLine(indent, "Frozen Layers", "None");
      }

      Point3d origin = new Point3d();
      Vector3d xAxis = new Vector3d();
      Vector3d yAxis = new Vector3d();
      pVport.GetUcs(ref origin, ref xAxis, ref yAxis);
      writeLine(indent, "UCS origin", origin);
      writeLine(indent, "UCS x-Axis", xAxis);
      writeLine(indent, "UCS y-Axis", yAxis);
      writeLine(indent, "Grid Increment", pVport.GridIncrement);
      writeLine(indent, "Grid On", pVport.GridOn);
      writeLine(indent, "Height", pVport.Height);
      writeLine(indent, "Lens Length", pVport.LensLength);
      writeLine(indent, "Locked", pVport.Locked);
      writeLine(indent, "Non-Rectangular Clip", pVport.NonRectClipOn);

      if (!pVport.NonRectClipEntityId.IsNull)
      {
        writeLine(indent, "Non-rectangular Clipper", pVport.NonRectClipEntityId.Handle);
      }
      writeLine(indent, "Render Mode", pVport.RenderMode);
      writeLine(indent, "Remove Hidden Lines", pVport.HiddenLinesRemoved);
      writeLine(indent, "Shade Plot", pVport.ShadePlot);
      writeLine(indent, "Snap Isometric", pVport.SnapIsometric);
      writeLine(indent, "Snap On", pVport.SnapOn);
      writeLine(indent, "Transparent", pVport.Transparent);
      writeLine(indent, "UCS Follow", pVport.UcsFollowModeOn);
      writeLine(indent, "UCS Icon at Origin", pVport.UcsIconAtOrigin);

      writeLine(indent, "UCS Orthographic", pVport.UcsOrthographic);
      writeLine(indent, "UCS Saved with VP", pVport.UcsPerViewport);

      if (!pVport.UcsName.IsNull)
      {
        using (UcsTableRecord pUCS = (UcsTableRecord)pVport.UcsName.Open(OpenMode.ForRead))
          writeLine(indent, "UCS Name", pUCS.Name);
      }
      else
      {
        writeLine(indent, "UCS Name", "Null");
      }

      writeLine(indent, "View Center", pVport.ViewCenter);
      writeLine(indent, "View Height", pVport.ViewHeight);
      writeLine(indent, "View Target", pVport.ViewTarget);
      writeLine(indent, "Width", pVport.Width);
      dumpEntityData(pVport, indent);

      {
          using (DBObjectCollection collection = new DBObjectCollection())
        {
          try
          {
            pVport.ExplodeGeometry(collection);

            foreach (Entity ent in collection)
            {
              if (ent is Polyline2d)
              {
                Polyline2d pline2d = (Polyline2d)ent;
                int i = 0;
                foreach (Entity ent1 in pline2d)
                {
                  if (ent1 is Vertex2d)
                  {
                    Vertex2d vtx2d = (Vertex2d)ent1;
                    dump2dVertex(indent, vtx2d, i++);
                  }
                }
              }
            }
          }
          catch (System.Exception e)
          {
          }
        }
      }      
    }

    /************************************************************************/
    /* Wipeout Dumper                                                  */
    /************************************************************************/
    void dump(Wipeout pWipeout, int indent)
    {
      writeLine(indent++, pWipeout.GetRXClass().Name, pWipeout.Handle);
      dumpRasterImageData(pWipeout, indent);
    }

    /************************************************************************/
    /* Xline Dumper                                                         */
    /************************************************************************/
    void dump(Xline pXline, int indent)
    {
      writeLine(indent++, pXline.GetRXClass().Name, pXline.Handle);
      writeLine(indent, "Base Point", pXline.BasePoint);
      writeLine(indent, "Unit Direction", pXline.UnitDir);
      dumpCurveData(pXline, indent);
    }

    public void dump(Database pDb, int indent)
    {
      dumpHeader(pDb, indent);
      dumpLayers(pDb, indent);
      dumpLinetypes(pDb, indent);
      dumpTextStyles(pDb, indent);
      dumpDimStyles(pDb, indent);
      dumpRegApps(pDb, indent);
      dumpViewports(pDb, indent);
      dumpViews(pDb, indent);
      dumpMLineStyles(pDb, indent);
      dumpUCSTable(pDb, indent);
      dumpObject(pDb.NamedObjectsDictionaryId, "Named Objects Dictionary", indent);
      dumpBlocks(pDb, indent);
    }
    /************************************************************************/
    /* Dump the BlockTable                                                  */
    /************************************************************************/
    public void dumpBlocks(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the BlockTable                               */
      /**********************************************************************/
      using (BlockTable pTable = (BlockTable)pDb.BlockTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the BlockTable                                        */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /********************************************************************/
          /* Open the BlockTableRecord for Reading                            */
          /********************************************************************/
          using (BlockTableRecord pBlock = (BlockTableRecord)id.Open(OpenMode.ForRead))
          {
            /********************************************************************/
            /* Dump the BlockTableRecord                                        */
            /********************************************************************/
            writeLine();
            writeLine(indent, pBlock.GetRXClass().Name);
            writeLine(indent + 1, "Name", pBlock.Name);
            writeLine(indent + 1, "Anonymous", pBlock.IsAnonymous);
            writeLine(indent + 1, "Comments", pBlock.Comments);
            writeLine(indent + 1, "Origin", pBlock.Origin);
            writeLine(indent + 1, "Block Insert Units", pBlock.Units);
            writeLine(indent + 1, "Block Scaling", pBlock.BlockScaling);
            writeLine(indent + 1, "Explodable", pBlock.Explodable);
            writeLine(indent + 1, "IsDynamicBlock", pBlock.IsDynamicBlock);

            try
            {
              Extents3d extents = new Extents3d(new Point3d(1E+20, 1E+20, 1E+20), new Point3d(1E-20, 1E-20, 1E-20));
              extents.AddBlockExtents(pBlock);
              writeLine(indent + 1, "Min Extents", extents.MinPoint);
              writeLine(indent + 1, "Max Extents", extents.MaxPoint);
            }
            catch (System.Exception)
            {
            }
            writeLine(indent + 1, "Layout", pBlock.IsLayout);
            writeLine(indent + 1, "Has Attribute Definitions", pBlock.HasAttributeDefinitions);
            writeLine(indent + 1, "Xref Status", pBlock.XrefStatus);
            if (pBlock.XrefStatus != XrefStatus.NotAnXref)
            {
              writeLine(indent + 1, "Xref Path", pBlock.PathName);
              writeLine(indent + 1, "From Xref Attach", pBlock.IsFromExternalReference);
              writeLine(indent + 1, "From Xref Overlay", pBlock.IsFromOverlayReference);
              writeLine(indent + 1, "Xref Unloaded", pBlock.IsUnloaded);
            }

            /********************************************************************/
            /* Step through the BlockTableRecord                                */
            /********************************************************************/
            foreach (ObjectId entid in pBlock)
            {
              /********************************************************************/
              /* Dump the Entity                                                  */
              /********************************************************************/
              dumpEntity(entid, indent + 1);
            }
          }
        }
      }
    }
    public void dumpDimStyles(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a SmartPointer to the DimStyleTable                            */
      /**********************************************************************/
      using (DimStyleTable pTable = (DimStyleTable)pDb.DimStyleTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the DimStyleTable                                    */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the DimStyleTableRecord for Reading                         */
          /*********************************************************************/
          using (DimStyleTableRecord pRecord = (DimStyleTableRecord)id.Open(OpenMode.ForRead))
          {
            /*********************************************************************/
            /* Dump the DimStyleTableRecord                                      */
            /*********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "Arc Symbol", toArcSymbolTypeString(pRecord.Dimarcsym));

            writeLine(indent, "Background Text Color", pRecord.Dimtfillclr);
            writeLine(indent, "BackgroundText Flags", pRecord.Dimtfill);
            writeLine(indent, "Extension Line 1 Linetype", pRecord.Dimltex1);
            writeLine(indent, "Extension Line 2 Linetype", pRecord.Dimltex2);
            writeLine(indent, "Dimension Line Linetype", pRecord.Dimltype);
            writeLine(indent, "Extension Line Fixed Len", pRecord.Dimfxlen);
            writeLine(indent, "Extension Line Fixed Len Enable", pRecord.DimfxlenOn);
            writeLine(indent, "Jog Angle", toDegreeString(pRecord.Dimjogang));
            writeLine(indent, "Modified For Recompute", pRecord.IsModifiedForRecompute);
            writeLine(indent, "DIMADEC", pRecord.Dimadec);
            writeLine(indent, "DIMALT", pRecord.Dimalt);
            writeLine(indent, "DIMALTD", pRecord.Dimaltd);
            writeLine(indent, "DIMALTF", pRecord.Dimaltf);
            writeLine(indent, "DIMALTRND", pRecord.Dimaltrnd);
            writeLine(indent, "DIMALTTD", pRecord.Dimalttd);
            writeLine(indent, "DIMALTTZ", pRecord.Dimalttz);
            writeLine(indent, "DIMALTU", pRecord.Dimaltu);
            writeLine(indent, "DIMALTZ", pRecord.Dimaltz);
            writeLine(indent, "DIMAPOST", pRecord.Dimapost);
            writeLine(indent, "DIMASZ", pRecord.Dimasz);
            writeLine(indent, "DIMATFIT", pRecord.Dimatfit);
            writeLine(indent, "DIMAUNIT", pRecord.Dimaunit);
            writeLine(indent, "DIMAZIN", pRecord.Dimazin);
            writeLine(indent, "DIMBLK", pRecord.Dimblk);
            writeLine(indent, "DIMBLK1", pRecord.Dimblk1);
            writeLine(indent, "DIMBLK2", pRecord.Dimblk2);
            writeLine(indent, "DIMCEN", pRecord.Dimcen);
            writeLine(indent, "DIMCLRD", pRecord.Dimclrd);
            writeLine(indent, "DIMCLRE", pRecord.Dimclre);
            writeLine(indent, "DIMCLRT", pRecord.Dimclrt);
            writeLine(indent, "DIMDEC", pRecord.Dimdec);
            writeLine(indent, "DIMDLE", pRecord.Dimdle);
            writeLine(indent, "DIMDLI", pRecord.Dimdli);
            writeLine(indent, "DIMDSEP", pRecord.Dimdsep);
            writeLine(indent, "DIMEXE", pRecord.Dimexe);
            writeLine(indent, "DIMEXO", pRecord.Dimexo);
            writeLine(indent, "DIMFRAC", pRecord.Dimfrac);
            writeLine(indent, "DIMGAP", pRecord.Dimgap);
            writeLine(indent, "DIMJUST", pRecord.Dimjust);
            writeLine(indent, "DIMLDRBLK", pRecord.Dimldrblk);
            writeLine(indent, "DIMLFAC", pRecord.Dimlfac);
            writeLine(indent, "DIMLIM", pRecord.Dimlim);
            writeLine(indent, "DIMLUNIT", pRecord.Dimlunit);
            writeLine(indent, "DIMLWD", pRecord.Dimlwd);
            writeLine(indent, "DIMLWE", pRecord.Dimlwe);
            writeLine(indent, "DIMPOST", pRecord.Dimpost);
            writeLine(indent, "DIMRND", pRecord.Dimrnd);
            writeLine(indent, "DIMSAH", pRecord.Dimsah);
            writeLine(indent, "DIMSCALE", pRecord.Dimscale);
            writeLine(indent, "DIMSD1", pRecord.Dimsd1);
            writeLine(indent, "DIMSD2", pRecord.Dimsd2);
            writeLine(indent, "DIMSE1", pRecord.Dimse1);
            writeLine(indent, "DIMSE2", pRecord.Dimse2);
            writeLine(indent, "DIMSOXD", pRecord.Dimsoxd);
            writeLine(indent, "DIMTAD", pRecord.Dimtad);
            writeLine(indent, "DIMTDEC", pRecord.Dimtdec);
            writeLine(indent, "DIMTFAC", pRecord.Dimtfac);
            writeLine(indent, "DIMTIH", pRecord.Dimtih);
            writeLine(indent, "DIMTIX", pRecord.Dimtix);
            writeLine(indent, "DIMTM", pRecord.Dimtm);
            writeLine(indent, "DIMTOFL", pRecord.Dimtofl);
            writeLine(indent, "DIMTOH", pRecord.Dimtoh);
            writeLine(indent, "DIMTOL", pRecord.Dimtol);
            writeLine(indent, "DIMTOLJ", pRecord.Dimtolj);
            writeLine(indent, "DIMTP", pRecord.Dimtp);
            writeLine(indent, "DIMTSZ", pRecord.Dimtsz);
            writeLine(indent, "DIMTVP", pRecord.Dimtvp);
            writeLine(indent, "DIMTXSTY", pRecord.Dimtxsty);
            writeLine(indent, "DIMTXT", pRecord.Dimtxt);
            writeLine(indent, "DIMTZIN", pRecord.Dimtzin);
            writeLine(indent, "DIMUPT", pRecord.Dimupt);
            writeLine(indent, "DIMZIN", pRecord.Dimzin);

            dumpSymbolTableRecord(pRecord, indent);
          }
        }
      }
    }
    public void dumpEntity(ObjectId id, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the Entity                                   */
      /**********************************************************************/
      using (Entity pEnt = (Entity)id.Open(OpenMode.ForRead, false, true))
      {
        /**********************************************************************/
        /* Dump the entity                                                    */
        /**********************************************************************/
        writeLine();
        // Protocol extensions are not supported in DD.NET (as well as in ARX.NET)
        // so we just switch by entity type here
        // (maybe it makes sense to make a map: type -> delegate)
        switch (pEnt.GetRXClass().Name)
        {
          case "AcDbAlignedDimension":
            dump((AlignedDimension)pEnt, indent);
            break;
          case "AcDbArc":
            dump((Arc)pEnt, indent);
            break;
          case "AcDbArcDimension":
            dump((ArcDimension)pEnt, indent);
            break;
          case "AcDbBlockReference":
            dump((BlockReference)pEnt, indent);
            break;
          case "AcDbBody":
            dump((Body)pEnt, indent);
            break;
          case "AcDbCircle":
            dump((Circle)pEnt, indent);
            break;
          case "AcDbPoint":
            dump((DBPoint)pEnt, indent);
            break;
          case "AcDbText":
            dump((DBText)pEnt, indent);
            break;
          case "AcDbDiametricDimension":
            dump((DiametricDimension)pEnt, indent);
            break;
          case "AcDbViewport":
            dump((Teigha.DatabaseServices.Viewport)pEnt, indent);
            break;
          case "AcDbEllipse":
            dump((Ellipse)pEnt, indent);
            break;
          case "AcDbFace":
            dump((Face)pEnt, indent);
            break;
          case "AcDbFcf":
            dump((FeatureControlFrame)pEnt, indent);
            break;
          case "AcDbHatch":
            dump((Hatch)pEnt, indent);
            break;
          case "AcDbLeader":
            dump((Leader)pEnt, indent);
            break;
          case "AcDbLine":
            dump((Line)pEnt, indent);
            break;
          case "AcDb2LineAngularDimension":
            dump((LineAngularDimension2)pEnt, indent);
            break;
          case "AcDbMInsertBlock":
            dump((MInsertBlock)pEnt, indent);
            break;
          case "AcDbMline":
            dump((Mline)pEnt, indent);
            break;
          case "AcDbMText":
            dump((MText)pEnt, indent);
            break;
          case "AcDbOle2Frame":
            dump((Ole2Frame)pEnt, indent);
            break;
          case "AcDbOrdinateDimension":
            dump((OrdinateDimension)pEnt, indent);
            break;
          case "AcDb3PointAngularDimension":
            dump((Point3AngularDimension)pEnt, indent);
            break;
          case "AcDbPolyFaceMesh":
            dump((PolyFaceMesh)pEnt, indent);
            break;
          case "AcDbPolygonMesh":
            dump((PolygonMesh)pEnt, indent);
            break;
          case "AcDbPolyline":
            dump((Teigha.DatabaseServices.Polyline)pEnt, indent);
            break;
          case "AcDb2dPolyline":
            dump((Polyline2d)pEnt, indent);
            break;
          case "AcDb3dPolyline":
            dump((Polyline3d)pEnt, indent);
            break;
          case "AcDbProxyEntity":
            dump((ProxyEntity)pEnt, indent);
            break;
          case "AcDbRadialDimension":
            dump((RadialDimension)pEnt, indent);
            break;
          case "AcDbRasterImage":
            dump((RasterImage)pEnt, indent);
            break;
          case "AcDbRay":
            dump((Ray)pEnt, indent);
            break;
          case "AcDbRegion":
            dump((Region)pEnt, indent);
            break;
          case "AcDbRotatedDimension":
            dump((RotatedDimension)pEnt, indent);
            break;
          case "AcDbShape":
            dump((Shape)pEnt, indent);
            break;
          case "AcDb3dSolid":
            dump((Solid3d)pEnt, indent);
            break;
          case "AcDbSpline":
            dump((Spline)pEnt, indent);
            break;
          case "AcDbTable":
            dump((Table)pEnt, indent);
            break;
          case "AcDbTrace":
            dump((Trace)pEnt, indent);
            break;
          case "AcDbWipeout":
            dump((Wipeout)pEnt, indent);
            break;
          case "AcDbXline":
            dump((Xline)pEnt, indent);
            break;
          case "AcDbPdfReference":
          case "AcDbDwfReference":
          case "AcDbDgnReference":
            dump((UnderlayReference)pEnt, indent);
            break;
          default:
            dump(pEnt, indent);
            break;
        }

        /**********************************************************************/
        /* Dump the Xdata                                                     */
        /**********************************************************************/
        dumpXdata(pEnt.XData, indent);

        /**********************************************************************/
        /* Dump the Extension Dictionary                                      */
        /**********************************************************************/
        if (!pEnt.ExtensionDictionary.IsNull)
        {
          dumpObject(pEnt.ExtensionDictionary, "ACAD_XDICTIONARY", indent);
        }
      }
    }
    public void dumpHeader(Database pDb, int indent)
    {
      writeLine();
      writeLine(indent, "Filename:", shortenPath(pDb.Filename));
      writeLine(indent, ".dwg file version:", pDb.OriginalFileVersion);

      writeLine();
      writeLine(indent++, "Header Variables:");

      //writeLine();
      //writeLine(indent, "TDCREATE:", pDb.TDCREATE);
      //writeLine(indent, "TDUPDATE:", pDb.TDUPDATE);

      writeLine();
      writeLine(indent, "ANGBASE", pDb.Angbase);
      writeLine(indent, "ANGDIR", pDb.Angdir);
      writeLine(indent, "ATTMODE", pDb.Attmode);
      writeLine(indent, "AUNITS", pDb.Aunits);
      writeLine(indent, "AUPREC", pDb.Auprec);
      writeLine(indent, "CECOLOR", pDb.Cecolor);
      writeLine(indent, "CELTSCALE", pDb.Celtscale);
      writeLine(indent, "CHAMFERA", pDb.Chamfera);
      writeLine(indent, "CHAMFERB", pDb.Chamferb);
      writeLine(indent, "CHAMFERC", pDb.Chamferc);
      writeLine(indent, "CHAMFERD", pDb.Chamferd);
      writeLine(indent, "CMLJUST", pDb.Cmljust);
      writeLine(indent, "CMLSCALE", pDb.Cmljust);
      writeLine(indent, "DIMADEC", pDb.Dimadec);
      writeLine(indent, "DIMALT", pDb.Dimalt);
      writeLine(indent, "DIMALTD", pDb.Dimaltd);
      writeLine(indent, "DIMALTF", pDb.Dimaltf);
      writeLine(indent, "DIMALTRND", pDb.Dimaltrnd);
      writeLine(indent, "DIMALTTD", pDb.Dimalttd);
      writeLine(indent, "DIMALTTZ", pDb.Dimalttz);
      writeLine(indent, "DIMALTU", pDb.Dimaltu);
      writeLine(indent, "DIMALTZ", pDb.Dimaltz);
      writeLine(indent, "DIMAPOST", pDb.Dimapost);
      writeLine(indent, "DIMASZ", pDb.Dimasz);
      writeLine(indent, "DIMATFIT", pDb.Dimatfit);
      writeLine(indent, "DIMAUNIT", pDb.Dimaunit);
      writeLine(indent, "DIMAZIN", pDb.Dimazin);
      writeLine(indent, "DIMBLK", pDb.Dimblk);
      writeLine(indent, "DIMBLK1", pDb.Dimblk1);
      writeLine(indent, "DIMBLK2", pDb.Dimblk2);
      writeLine(indent, "DIMCEN", pDb.Dimcen);
      writeLine(indent, "DIMCLRD", pDb.Dimclrd);
      writeLine(indent, "DIMCLRE", pDb.Dimclre);
      writeLine(indent, "DIMCLRT", pDb.Dimclrt);
      writeLine(indent, "DIMDEC", pDb.Dimdec);
      writeLine(indent, "DIMDLE", pDb.Dimdle);
      writeLine(indent, "DIMDLI", pDb.Dimdli);
      writeLine(indent, "DIMDSEP", pDb.Dimdsep);
      writeLine(indent, "DIMEXE", pDb.Dimexe);
      writeLine(indent, "DIMEXO", pDb.Dimexo);
      writeLine(indent, "DIMFRAC", pDb.Dimfrac);
      writeLine(indent, "DIMGAP", pDb.Dimgap);
      writeLine(indent, "DIMJUST", pDb.Dimjust);
      writeLine(indent, "DIMLDRBLK", pDb.Dimldrblk);
      writeLine(indent, "DIMLFAC", pDb.Dimlfac);
      writeLine(indent, "DIMLIM", pDb.Dimlim);
      writeLine(indent, "DIMLUNIT", pDb.Dimlunit);
      writeLine(indent, "DIMLWD", pDb.Dimlwd);
      writeLine(indent, "DIMLWE", pDb.Dimlwe);
      writeLine(indent, "DIMPOST", pDb.Dimpost);
      writeLine(indent, "DIMRND", pDb.Dimrnd);
      writeLine(indent, "DIMSAH", pDb.Dimsah);
      writeLine(indent, "DIMSCALE", pDb.Dimscale);
      writeLine(indent, "DIMSD1", pDb.Dimsd1);
      writeLine(indent, "DIMSD2", pDb.Dimsd2);
      writeLine(indent, "DIMSE1", pDb.Dimse1);
      writeLine(indent, "DIMSE2", pDb.Dimse2);
      writeLine(indent, "DIMSOXD", pDb.Dimsoxd);
      writeLine(indent, "DIMTAD", pDb.Dimtad);
      writeLine(indent, "DIMTDEC", pDb.Dimtdec);
      writeLine(indent, "DIMTFAC", pDb.Dimtfac);
      writeLine(indent, "DIMTIH", pDb.Dimtih);
      writeLine(indent, "DIMTIX", pDb.Dimtix);
      writeLine(indent, "DIMTM", pDb.Dimtm);
      writeLine(indent, "DIMTOFL", pDb.Dimtofl);
      writeLine(indent, "DIMTOH", pDb.Dimtoh);
      writeLine(indent, "DIMTOL", pDb.Dimtol);
      writeLine(indent, "DIMTOLJ", pDb.Dimtolj);
      writeLine(indent, "DIMTP", pDb.Dimtp);
      writeLine(indent, "DIMTSZ", pDb.Dimtsz);
      writeLine(indent, "DIMTVP", pDb.Dimtvp);
      writeLine(indent, "DIMTXSTY", pDb.Dimtxsty);
      writeLine(indent, "DIMTXT", pDb.Dimtxt);
      writeLine(indent, "DIMTZIN", pDb.Dimtzin);
      writeLine(indent, "DIMUPT", pDb.Dimupt);
      writeLine(indent, "DIMZIN", pDb.Dimzin);
      writeLine(indent, "DISPSILH", pDb.DispSilh);
      writeLine(indent, "DRAWORDERCTL", pDb.DrawOrderCtl);
      writeLine(indent, "ELEVATION", pDb.Elevation);
      writeLine(indent, "EXTMAX", pDb.Extmax);
      writeLine(indent, "EXTMIN", pDb.Extmin);
      writeLine(indent, "FACETRES", pDb.Facetres);
      writeLine(indent, "FILLETRAD", pDb.Filletrad);
      writeLine(indent, "FILLMODE", pDb.Fillmode);
      writeLine(indent, "INSBASE", pDb.Insbase);
      writeLine(indent, "ISOLINES", pDb.Isolines);
      writeLine(indent, "LIMCHECK", pDb.Limcheck);
      writeLine(indent, "LIMMAX", pDb.Limmax);
      writeLine(indent, "LIMMIN", pDb.Limmin);
      writeLine(indent, "LTSCALE", pDb.Ltscale);
      writeLine(indent, "LUNITS", pDb.Lunits);
      writeLine(indent, "LUPREC", pDb.Luprec);
      writeLine(indent, "MAXACTVP", pDb.Maxactvp);
      writeLine(indent, "MIRRTEXT", pDb.Mirrtext);
      writeLine(indent, "ORTHOMODE", pDb.Orthomode);
      writeLine(indent, "PDMODE", pDb.Pdmode);
      writeLine(indent, "PDSIZE", pDb.Pdsize);
      writeLine(indent, "PELEVATION", pDb.Pelevation);
      writeLine(indent, "PELLIPSE", pDb.PlineEllipse);
      writeLine(indent, "PEXTMAX", pDb.Pextmax);
      writeLine(indent, "PEXTMIN", pDb.Pextmin);
      writeLine(indent, "PINSBASE", pDb.Pinsbase);
      writeLine(indent, "PLIMCHECK", pDb.Plimcheck);
      writeLine(indent, "PLIMMAX", pDb.Plimmax);
      writeLine(indent, "PLIMMIN", pDb.Plimmin);
      writeLine(indent, "PLINEGEN", pDb.Plinegen);
      writeLine(indent, "PLINEWID", pDb.Plinewid);
      writeLine(indent, "PROXYGRAPHICS", pDb.Saveproxygraphics);
      writeLine(indent, "PSLTSCALE", pDb.Psltscale);
      writeLine(indent, "PUCSNAME", pDb.Pucsname);
      writeLine(indent, "PUCSORG", pDb.Pucsorg);
      writeLine(indent, "PUCSXDIR", pDb.Pucsxdir);
      writeLine(indent, "PUCSYDIR", pDb.Pucsydir);
      writeLine(indent, "QTEXTMODE", pDb.Qtextmode);
      writeLine(indent, "REGENMODE", pDb.Regenmode);
      writeLine(indent, "SHADEDGE", pDb.Shadedge);
      writeLine(indent, "SHADEDIF", pDb.Shadedif);
      writeLine(indent, "SKETCHINC", pDb.Sketchinc);
      writeLine(indent, "SKPOLY", pDb.Skpoly);
      writeLine(indent, "SPLFRAME", pDb.Splframe);
      writeLine(indent, "SPLINESEGS", pDb.Splinesegs);
      writeLine(indent, "SPLINETYPE", pDb.Splinetype);
      writeLine(indent, "SURFTAB1", pDb.Surftab1);
      writeLine(indent, "SURFTAB2", pDb.Surftab2);
      writeLine(indent, "SURFTYPE", pDb.Surftype);
      writeLine(indent, "SURFU", pDb.Surfu);
      writeLine(indent, "SURFV", pDb.Surfv);
      //writeLine(indent, "TEXTQLTY", pDb.TEXTQLTY);
      writeLine(indent, "TEXTSIZE", pDb.Textsize);
      writeLine(indent, "THICKNESS", pDb.Thickness);
      writeLine(indent, "TILEMODE", pDb.TileMode);
      writeLine(indent, "TRACEWID", pDb.Tracewid);
      writeLine(indent, "TREEDEPTH", pDb.Treedepth);
      writeLine(indent, "UCSNAME", pDb.Ucsname);
      writeLine(indent, "UCSORG", pDb.Ucsorg);
      writeLine(indent, "UCSXDIR", pDb.Ucsxdir);
      writeLine(indent, "UCSYDIR", pDb.Ucsydir);
      writeLine(indent, "UNITMODE", pDb.Unitmode);
      writeLine(indent, "USERI1", pDb.Useri1);
      writeLine(indent, "USERI2", pDb.Useri2);
      writeLine(indent, "USERI3", pDb.Useri3);
      writeLine(indent, "USERI4", pDb.Useri4);
      writeLine(indent, "USERI5", pDb.Useri5);
      writeLine(indent, "USERR1", pDb.Userr1);
      writeLine(indent, "USERR2", pDb.Userr2);
      writeLine(indent, "USERR3", pDb.Userr3);
      writeLine(indent, "USERR4", pDb.Userr4);
      writeLine(indent, "USERR5", pDb.Userr5);
      writeLine(indent, "USRTIMER", pDb.Usrtimer);
      writeLine(indent, "VISRETAIN", pDb.Visretain);
      writeLine(indent, "WORLDVIEW", pDb.Worldview);
    }
    public void dumpLayers(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a SmartPointer to the LayerTable                               */
      /**********************************************************************/
      using (LayerTable pTable = (LayerTable)pDb.LayerTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Get a SmartPointer to a new SymbolTableIterator                    */
        /**********************************************************************/

        /**********************************************************************/
        /* Step through the LayerTable                                        */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /********************************************************************/
          /* Open the LayerTableRecord for Reading                            */
          /********************************************************************/
          using (LayerTableRecord pRecord = (LayerTableRecord)id.Open(OpenMode.ForRead))
          {
            /********************************************************************/
            /* Dump the LayerTableRecord                                        */
            /********************************************************************/
            writeLine();
            writeLine(indent, "<" + pRecord.GetRXClass().Name + ">");
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "In Use", pRecord.IsUsed);
            writeLine(indent, "On", (!pRecord.IsOff));
            writeLine(indent, "Frozen", pRecord.IsFrozen);
            writeLine(indent, "Locked", pRecord.IsLocked);
            writeLine(indent, "Color", pRecord.Color);
            writeLine(indent, "Linetype", pRecord.LinetypeObjectId);
            writeLine(indent, "Lineweight", pRecord.LineWeight);
            writeLine(indent, "Plotstyle", pRecord.PlotStyleName);
            writeLine(indent, "Plottable", pRecord.IsPlottable);
            writeLine(indent, "New VP Freeze", pRecord.ViewportVisibilityDefault);
            dumpSymbolTableRecord(pRecord, indent);
          }
        }
      }
    }
    public void dumpLinetypes(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the LinetypeTable                            */
      /**********************************************************************/
      using (LinetypeTable pTable = (LinetypeTable)pDb.LinetypeTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, "<" + pTable.GetRXClass().Name + ">");

        /**********************************************************************/
        /* Step through the LinetypeTable                                     */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the LinetypeTableRecord for Reading                          */
          /*********************************************************************/
          using (LinetypeTableRecord pRecord = (LinetypeTableRecord)id.Open(OpenMode.ForRead))
          {
            /********************************************************************/
            /* Dump the LinetypeTableRecord                                      */
            /********************************************************************/
            writeLine();
            writeLine(indent, "<" + pRecord.GetRXClass().Name + ">");
            /********************************************************************/
            /* Dump the first line of record as in ACAD.LIN                     */
            /********************************************************************/
            string buffer = "*" + pRecord.Name;
            if (pRecord.Comments != "")
            {
              buffer = buffer + "," + pRecord.Comments;
            }
            writeLine(indent, buffer);

            /********************************************************************/
            /* Dump the second line of record as in ACAD.LIN                    */
            /********************************************************************/
            if (pRecord.NumDashes > 0)
            {
              buffer = pRecord.IsScaledToFit ? "S" : "A";
              for (int i = 0; i < pRecord.NumDashes; i++)
              {
                buffer = buffer + "," + pRecord.DashLengthAt(i);
                int shapeNumber = pRecord.ShapeNumberAt(i);
                string text = pRecord.TextAt(i);

                /**************************************************************/
                /* Dump the Complex Line                                      */
                /**************************************************************/
                if (shapeNumber != 0 || text != "")
                {
                  using (TextStyleTableRecord pTextStyle = (TextStyleTableRecord)(pRecord.ShapeStyleAt(i) == ObjectId.Null ? null : pRecord.ShapeStyleAt(i).Open(OpenMode.ForRead)))
                  {
                    if (shapeNumber != 0)
                    {
                      buffer = buffer + ",[" + shapeNumber + ",";
                      if (pTextStyle != null)
                        buffer = buffer + pTextStyle.FileName;
                      else
                        buffer = buffer + "NULL style";
                    }
                    else
                    {
                      buffer = buffer + ",[" + text + ",";
                      if (pTextStyle != null)
                        buffer = buffer + pTextStyle.Name;
                      else
                        buffer = buffer + "NULL style";
                    }
                  }
                  if (pRecord.ShapeScaleAt(i) != 0.0)
                  {
                    buffer = buffer + ",S" + pRecord.ShapeScaleAt(i);
                  }
                  if (pRecord.ShapeRotationAt(i) != 0)
                  {
                    buffer = buffer + ",R" + toDegreeString(pRecord.ShapeRotationAt(i));
                  }
                  if (pRecord.ShapeOffsetAt(i).X != 0)
                  {
                    buffer = buffer + ",X" + pRecord.ShapeOffsetAt(i).X;
                  }
                  if (pRecord.ShapeOffsetAt(i).Y != 0)
                  {
                    buffer = buffer + ",Y" + pRecord.ShapeOffsetAt(i).Y;
                  }
                  buffer = buffer + "]";
                }
              }
              writeLine(indent, buffer);
            }
            dumpSymbolTableRecord(pRecord, indent);
          }
        }
      }
    }
    public void dumpRegApps(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the RegAppTable                            */
      /**********************************************************************/
      using (RegAppTable pTable = (RegAppTable)pDb.RegAppTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the RegAppTable                                    */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the RegAppTableRecord for Reading                         */
          /*********************************************************************/
          using (RegAppTableRecord pRecord = (RegAppTableRecord)id.Open(OpenMode.ForRead))
          {
            /*********************************************************************/
            /* Dump the RegAppTableRecord                                      */
            /*********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
          }
        }
      }
    }
    public void dumpSymbolTableRecord(SymbolTableRecord pRecord, int indent)
    {
      writeLine(indent, "Xref dependent", pRecord.IsDependent);
      if (pRecord.IsDependent)
      {
        writeLine(indent, "Resolved", pRecord.IsResolved);
      }
    }
    public void dumpTextStyles(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a SmartPointer to the TextStyleTable                            */
      /**********************************************************************/
      using (TextStyleTable pTable = (TextStyleTable)pDb.TextStyleTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the TextStyleTable                                    */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the TextStyleTableRecord for Reading                         */
          /*********************************************************************/
          using (TextStyleTableRecord pRecord = (TextStyleTableRecord)id.Open(OpenMode.ForRead))
          {
            /*********************************************************************/
            /* Dump the TextStyleTableRecord                                      */
            /*********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "Shape File", pRecord.IsShapeFile);
            writeLine(indent, "Text Height", pRecord.TextSize);
            writeLine(indent, "Width Factor", pRecord.XScale);
            writeLine(indent, "Obliquing Angle", toDegreeString(pRecord.ObliquingAngle));
            writeLine(indent, "Backwards", (pRecord.FlagBits & 2));
            writeLine(indent, "Vertical", pRecord.IsVertical);
            writeLine(indent, "Upside Down", (pRecord.FlagBits & 4));
            writeLine(indent, "Filename", shortenPath(pRecord.FileName));
            writeLine(indent, "BigFont Filename", shortenPath(pRecord.BigFontFileName));

            FontDescriptor fd = pRecord.Font;
            writeLine(indent, "Typeface", fd.TypeFace);
            writeLine(indent, "Character Set", fd.CharacterSet);
            writeLine(indent, "Bold", fd.Bold);
            writeLine(indent, "Italic", fd.Italic);
            writeLine(indent, "Font Pitch & Family", toHexString(fd.PitchAndFamily));
            dumpSymbolTableRecord(pRecord, indent);
          }
        }
      }
    }
    public void dumpAbstractViewTableRecord(AbstractViewTableRecord pView, int indent)
    {
      /*********************************************************************/
      /* Dump the AbstractViewTableRecord                                  */
      /*********************************************************************/
      writeLine(indent, "Back Clip Dist", pView.BackClipDistance);
      writeLine(indent, "Back Clip Enabled", pView.BackClipEnabled);
      writeLine(indent, "Front Clip Dist", pView.FrontClipDistance);
      writeLine(indent, "Front Clip Enabled", pView.FrontClipEnabled);
      writeLine(indent, "Front Clip at Eye", pView.FrontClipAtEye);
      writeLine(indent, "Elevation", pView.Elevation);
      writeLine(indent, "Height", pView.Height);
      writeLine(indent, "Width", pView.Width);
      writeLine(indent, "Lens Length", pView.LensLength);
      writeLine(indent, "Render Mode", pView.RenderMode);
      writeLine(indent, "Perspective", pView.PerspectiveEnabled);
      writeLine(indent, "UCS Name", pView.UcsName);

      //writeLine(indent, "UCS Orthographic", pView.IsUcsOrthographic(orthoUCS));
      //writeLine(indent, "Orthographic UCS", orthoUCS);

      writeLine(indent, "UCS Origin", pView.Ucs.Origin);
      writeLine(indent, "UCS x-Axis", pView.Ucs.Xaxis);
      writeLine(indent, "UCS y-Axis", pView.Ucs.Yaxis);
      writeLine(indent, "Target", pView.Target);
      writeLine(indent, "View Direction", pView.ViewDirection);
      writeLine(indent, "Twist Angle", toDegreeString(pView.ViewTwist));
      dumpSymbolTableRecord(pView, indent);
    }
    public void dumpDimAssoc(DBObject pObject, int indent)
    {

    }
    public void dumpMLineStyles(Database pDb, int indent)
    {
      using (DBDictionary pDictionary = (DBDictionary)pDb.MLStyleDictionaryId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pDictionary.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the MlineStyle dictionary                             */
        /**********************************************************************/
        DbDictionaryEnumerator e = pDictionary.GetEnumerator();
        while (e.MoveNext())
        {
          try
          {
            using (MlineStyle pEntry = (MlineStyle)e.Value.Open(OpenMode.ForRead))
            {
              /*********************************************************************/
              /* Dump the MLineStyle dictionary entry                              */
              /*********************************************************************/
              writeLine();
              writeLine(indent, pEntry.GetRXClass().Name);
              writeLine(indent, "Name", pEntry.Name);
              writeLine(indent, "Description", pEntry.Description);
              writeLine(indent, "Start Angle", toDegreeString(pEntry.StartAngle));
              writeLine(indent, "End Angle", toDegreeString(pEntry.EndAngle));
              writeLine(indent, "Start Inner Arcs", pEntry.StartInnerArcs);
              writeLine(indent, "End Inner Arcs", pEntry.EndInnerArcs);
              writeLine(indent, "Start Round Cap", pEntry.StartRoundCap);
              writeLine(indent, "End Round Cap", pEntry.EndRoundCap);
              writeLine(indent, "Start Square Cap", pEntry.StartRoundCap);
              writeLine(indent, "End Square Cap", pEntry.EndRoundCap);
              writeLine(indent, "Show Miters", pEntry.ShowMiters);
              /*********************************************************************/
              /* Dump the elements                                                 */
              /*********************************************************************/
              if (pEntry.Elements.Count > 0)
              {
                writeLine(indent, "Elements:");
              }
              int i = 0;
              foreach (MlineStyleElement el in pEntry.Elements)
              {
                writeLine(indent, "Index", (i++));
                writeLine(indent + 1, "Offset", el.Offset);
                writeLine(indent + 1, "Color", el.Color);
                writeLine(indent + 1, "Linetype", el.LinetypeId);
              }
            }
          }
          catch (System.Exception)
          {
          }
        }
      }
    }
    public void dumpObject(ObjectId id, string itemName, int indent)
    {
      using (DBObject pObject = id.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the item name and class name                                  */
        /**********************************************************************/
        if (pObject is DBDictionary)
        {
          writeLine();
        }
        writeLine(indent++, itemName, pObject.GetRXClass().Name);

        /**********************************************************************/
        /* Dispatch                                                           */
        /**********************************************************************/
        if (pObject is DBDictionary)
        {
          /********************************************************************/
          /* Dump the dictionary                                               */
          /********************************************************************/
          DBDictionary pDic = (DBDictionary)pObject;

          /********************************************************************/
          /* Get a pointer to a new DictionaryIterator                   */
          /********************************************************************/
          DbDictionaryEnumerator pIter = pDic.GetEnumerator();

          /********************************************************************/
          /* Step through the Dictionary                                      */
          /********************************************************************/
          while (pIter.MoveNext())
          {
            /******************************************************************/
            /* Dump the Dictionary object                                     */
            /******************************************************************/
            dumpObject(pIter.Value, pIter.Key, indent);
          }
        }
        else if (pObject is Xrecord)
        {
          /********************************************************************/
          /* Dump an Xrecord                                                  */
          /********************************************************************/
          Xrecord pXRec = (Xrecord)pObject;
          dumpXdata(pXRec.Data, indent);
        }
      }
    }

    public void dumpUCSTable(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the UCSTable                               */
      /**********************************************************************/
      using (UcsTable pTable = (UcsTable)pDb.UcsTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the UCSTable                                          */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /********************************************************************/
          /* Open the UCSTableRecord for Reading                            */
          /********************************************************************/
          using (UcsTableRecord pRecord = (UcsTableRecord)id.Open(OpenMode.ForRead))
          {
            /********************************************************************/
            /* Dump the UCSTableRecord                                        */
            /********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "UCS Origin", pRecord.Origin);
            writeLine(indent, "UCS x-Axis", pRecord.XAxis);
            writeLine(indent, "UCS y-Axis", pRecord.YAxis);
            dumpSymbolTableRecord(pRecord, indent);
          }
        }
      }
    }
    public void dumpViewports(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the ViewportTable                            */
      /**********************************************************************/
      using (ViewportTable pTable = (ViewportTable)pDb.ViewportTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the ViewportTable                                    */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the ViewportTableRecord for Reading                          */
          /*********************************************************************/
          using (ViewportTableRecord pRecord = (ViewportTableRecord)id.Open(OpenMode.ForRead))
          {
            /*********************************************************************/
            /* Dump the ViewportTableRecord                                      */
            /*********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "Circle Sides", pRecord.CircleSides);
            writeLine(indent, "Fast Zooms Enabled", pRecord.FastZoomsEnabled);
            writeLine(indent, "Grid Enabled", pRecord.GridEnabled);
            writeLine(indent, "Grid Increments", pRecord.GridIncrements);
            writeLine(indent, "Icon at Origin", pRecord.IconAtOrigin);
            writeLine(indent, "Icon Enabled", pRecord.IconEnabled);
            writeLine(indent, "Iso snap Enabled", pRecord.IsometricSnapEnabled);
            writeLine(indent, "Iso Snap Pair", pRecord.SnapPair);
            writeLine(indent, "UCS Saved w/Vport", pRecord.UcsSavedWithViewport);
            writeLine(indent, "UCS follow", pRecord.UcsFollowMode);
            writeLine(indent, "Lower-Left Corner", pRecord.LowerLeftCorner);
            writeLine(indent, "Upper-Right Corner", pRecord.UpperRightCorner);
            writeLine(indent, "Snap Angle", toDegreeString(pRecord.SnapAngle));
            writeLine(indent, "Snap Base", pRecord.SnapBase);
            writeLine(indent, "Snap Enabled", pRecord.SnapEnabled);
            writeLine(indent, "Snap Increments", pRecord.SnapIncrements);
            dumpAbstractViewTableRecord(pRecord, indent);
          }
        }
      }
    }

    /************************************************************************/
    /* Dump the ViewTable                                                   */
    /************************************************************************/
    public void dumpViews(Database pDb, int indent)
    {
      /**********************************************************************/
      /* Get a pointer to the ViewTable                                */
      /**********************************************************************/
      using (ViewTable pTable = (ViewTable)pDb.ViewTableId.Open(OpenMode.ForRead))
      {
        /**********************************************************************/
        /* Dump the Description                                               */
        /**********************************************************************/
        writeLine();
        writeLine(indent++, pTable.GetRXClass().Name);

        /**********************************************************************/
        /* Step through the ViewTable                                         */
        /**********************************************************************/
        foreach (ObjectId id in pTable)
        {
          /*********************************************************************/
          /* Open the ViewTableRecord for Reading                              */
          /*********************************************************************/
          using (ViewTableRecord pRecord = (ViewTableRecord)id.Open(OpenMode.ForRead))
          {
            /*********************************************************************/
            /* Dump the ViewTableRecord                                          */
            /*********************************************************************/
            writeLine();
            writeLine(indent, pRecord.GetRXClass().Name);
            writeLine(indent, "Name", pRecord.Name);
            writeLine(indent, "Category Name", pRecord.CategoryName);
            writeLine(indent, "Layer State", pRecord.LayerState);

            string layoutName = "";
            if (!pRecord.Layout.IsNull)
            {
              using (Layout pLayout = (Layout)pRecord.Layout.Open(OpenMode.ForRead))
                layoutName = pLayout.LayoutName;
            }
            writeLine(indent, "Layout Name", layoutName);
            writeLine(indent, "PaperSpace View", pRecord.IsPaperspaceView);
            writeLine(indent, "Associated UCS", pRecord.IsUcsAssociatedToView);
            writeLine(indent, "PaperSpace View", pRecord.ViewAssociatedToViewport);
            dumpAbstractViewTableRecord(pRecord, indent);
          }
        }
      }
    }
    /************************************************************************/
    /* Dump Xdata                                                           */
    /************************************************************************/
    public void dumpXdata(ResultBuffer xIter, int indent)
    {
      if (xIter == null)
        return;
      writeLine(indent++, "Xdata:");
      /**********************************************************************/
      /* Step through the ResBuf chain                                      */
      /**********************************************************************/
      foreach (TypedValue resbuf in xIter)
      {
        writeLine(indent, resbuf);
      }
    }
  }
  class ExProtocolExtension
  {
  }
  class Program
  {
    static void Main(string[] args)
    {
      /********************************************************************/
      /* Initialize Teigha.                                            */
      /********************************************************************/
      bool bSuccess = true;
      using (Teigha.Runtime.Services srv = new Teigha.Runtime.Services())
      {
        try
        {
          HostApplicationServices.Current = new OdaMgdMViewApp.HostAppServ();
          /**********************************************************************/
          /* Display the Product and Version that created the executable        */
          /**********************************************************************/
          Console.WriteLine("\nReadExMgd developed using {0} ver {1}", HostApplicationServices.Current.Product, HostApplicationServices.Current.VersionString);
          
          if (args.Length != 1) 
          {
            Console.WriteLine("\n\n\tusage: OdReadExMgd <filename>");
            Console.WriteLine("\nPress ENTER to continue...\n");
            Console.ReadLine();
            bSuccess = false;
          }
          else
          {
            /******************************************************************/
            /* Create a database and load the drawing into it.                
            /* first parameter means - do not initialize database- it will be read from file
             * second parameter is not used by Teigha.NET Classic - it is left for ARX compatibility.
             * Note the 'using' clause - generally, wrappers should disposed after use, 
             * to close underlying database objects
            /******************************************************************/
            using (Database pDb = new Database(false, false))
            {
              pDb.ReadDwgFile(args[0], FileShare.Read, true, "");
              HostApplicationServices.WorkingDatabase = pDb;
              /****************************************************************/
              /* Display the File Version                                     */
              /****************************************************************/
              Console.WriteLine("File Version: {0}", pDb.OriginalFileVersion);
              /****************************************************************/
              /* Dump the database                                            */
              /****************************************************************/
              DbDumper dumper = new DbDumper();
              dumper.dump(pDb, 0);
            }
          }
        }
        /********************************************************************/
        /* Display the error                                                */
        /********************************************************************/
        catch (System.Exception e)
        {
          bSuccess = false;
          Console.WriteLine("Teigha®.NET for .dwg files Error: " + e.Message);
        }

        if (bSuccess)
          Console.WriteLine("OdReadExMgd Finished Successfully");
      }
    }
  }
}
