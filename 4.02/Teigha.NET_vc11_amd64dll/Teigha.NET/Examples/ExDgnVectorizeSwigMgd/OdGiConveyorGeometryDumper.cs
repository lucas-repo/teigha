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
//using System.Linq;
using System.Text;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnVectorizeMgd
{
  class OdGiConveyorGeometryDumper : OdGiGeometrySimplifier
  {
    public enum Simplification
    {
      Maximal_Simplification,
      Minimal_Simplification
    }
    OdGiDumper m_pDumper;
    Simplification m_dumpLevel;
    public OdGiConveyorGeometryDumper(OdGiDumper d)
    {
      m_pDumper = d;
      m_dumpLevel = Simplification.Maximal_Simplification;
    }
    public Simplification DumpLevel
    {
      set { m_dumpLevel = value; }
      get { return m_dumpLevel; }
    }

    /************************************************************************/
    /* Process OdGiPolyline data                                            */
    /************************************************************************/
    public override void plineProc(OdGiPolyline lwBuf,
                                     OdGeMatrix3d pXform,
                                     UInt32 fromIndex,
                                     UInt32 numSegs)
    {
      m_pDumper.output("Start plineProc");
      m_pDumper.pushIndent();
      m_pDumper.output("Xform", pXform);
      m_pDumper.output("fromIndex", fromIndex.ToString());
      m_pDumper.output("numSegs", numSegs.ToString());
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced pline data");
        m_pDumper.pushIndent();
        base.plineProc(lwBuf, pXform, fromIndex, numSegs);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End plineProc");

    }
    /************************************************************************/
    /* Process polyline data                                                */
    /************************************************************************/
    public override void polylineOut(OdGePoint3d[] vertexList)
    {
      m_pDumper.output("Start polylineOut");
      m_pDumper.pushIndent();
      m_pDumper.output("numPoints", vertexList.Length.ToString());
      m_pDumper.output(vertexList);
      m_pDumper.popIndent();
      m_pDumper.output("End polylineOut");
    }
    /************************************************************************/
    /* Process polygon data                                                 */
    /************************************************************************/
    public override void polygonOut(OdGePoint3d[] vertexList, OdGeVector3d pNormal)
    {
      m_pDumper.output("Start polygonOut");
      m_pDumper.pushIndent();
      m_pDumper.output("numPoints", vertexList.Length.ToString());
      m_pDumper.output(vertexList);
      m_pDumper.popIndent();
      m_pDumper.output("End polygonOut");
    }

    internal static String toString(OdGeVector3d pVal)
    {
      if (pVal != null && !pVal.isZeroLength())
        return string.Format("[{0} {1} {2}]", pVal.x, pVal.y, pVal.z);
      else
        return "Null";
    }

    /************************************************************************/
    /* Process simple polyline data                                         */
    /************************************************************************/
    public override void polylineProc(OdGePoint3d[] vertexList,
                                    OdGeVector3d pNormal,
                                    OdGeVector3d pExtrusion,
                                    IntPtr baseSubEntMarker)
    {
      m_pDumper.output("Start polylineProc");
      m_pDumper.pushIndent();
      m_pDumper.output("normal", toString(pNormal));
      m_pDumper.output("extrusion", toString(pExtrusion));
      m_pDumper.output("baseSubEntMarker", baseSubEntMarker.ToString());
      m_pDumper.output("numPoints", vertexList.Length.ToString());
      m_pDumper.output(vertexList);
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced polyline data");
        m_pDumper.pushIndent();
        base.polylineProc(vertexList, pNormal, pExtrusion, baseSubEntMarker);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End polylineProc");
    }
    /************************************************************************/
    /* Process polygon data                                                 */
    /************************************************************************/
    public override void polygonProc(OdGePoint3d[] vertexList, OdGeVector3d pNormal, OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start polygonProc");
      m_pDumper.pushIndent();
      m_pDumper.output("normal", toString(pNormal));
      m_pDumper.output("extrusion", toString(pExtrusion));
      m_pDumper.output("numPoints", vertexList.Length.ToString());
      m_pDumper.output(vertexList);
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced polygon data");
        m_pDumper.pushIndent();
        base.polygonProc(vertexList, pNormal, pExtrusion);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End polygonProc");
    }
    internal static String toString(OdGePoint3d val)
    {
      return String.Format("[{0} {1} {2}]", val.x, val.y, val.z);
    }
    String toString(OdGePoint2d val)
    {
      return String.Format("[{0} {1}]", val.x, val.y);
    }

    /************************************************************************/
    /* Process center-radius circle data                                    */
    /************************************************************************/
    public override void circleProc(OdGePoint3d center,
                                  double radius,
                                  OdGeVector3d normal,
                                  OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start circleProc");
      m_pDumper.pushIndent();
      m_pDumper.output("center", toString(center));
      m_pDumper.output("radius", radius.ToString());
      m_pDumper.output("normal", toString(normal));
      m_pDumper.output("extrusion", toString(pExtrusion));
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced circle data");
        m_pDumper.pushIndent();
        base.circleProc(center, radius, normal, pExtrusion);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End circleProc");
    }
    /************************************************************************/
    /* Process three-point circle data                                      */
    /************************************************************************/
    public override void circleProc(OdGePoint3d firstPoint,
                                  OdGePoint3d secondPoint,
                                  OdGePoint3d thirdPoint,
                                  OdGeVector3d pExtrusion)
    {
      // Convert 3 point circle to center/radius circle.
      OdGeCircArc3d circ = new OdGeCircArc3d(firstPoint, secondPoint, thirdPoint);
      base.circleProc(circ.center(), circ.radius(),
                                   circ.normal(), pExtrusion);
    }
    /************************************************************************/
    /* Process center-radius circular arc                                  */
    /************************************************************************/
    public override void circularArcProc(OdGePoint3d center,
                                       double radius,
                                       OdGeVector3d normal,
                                       OdGeVector3d startVector,
                                       double sweepAngle,
                                       OdGiArcType arcType,
                                       OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start circularArcProc");
      m_pDumper.pushIndent();
      m_pDumper.output("center", toString(center));
      m_pDumper.output("radius", radius.ToString());
      m_pDumper.output("normal", toString(normal));
      m_pDumper.output("startVector", toString(startVector));
      m_pDumper.output("sweepAngle", sweepAngle.ToString());
      m_pDumper.output("arcType", arcType.ToString());
      m_pDumper.output("extrusion", toString(pExtrusion));
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced circularArc data");
        m_pDumper.pushIndent();
        base.circularArcProc(center, radius, normal,
          startVector, sweepAngle, arcType, pExtrusion);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End circularArcProc");
    }

    /************************************************************************/
    /* Process three-point circular arc                                     */
    /************************************************************************/
    public override void circularArcProc(OdGePoint3d firstPoint,
                                       OdGePoint3d secondPoint,
                                       OdGePoint3d thirdPoint,
                                       OdGiArcType arcType,
                                       OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start circularArcProc");
      m_pDumper.pushIndent();
      m_pDumper.output("firstPoint", toString(firstPoint));
      m_pDumper.output("secondPoint", toString(secondPoint));
      m_pDumper.output("thirdPoint", toString(thirdPoint));
      m_pDumper.output("arcType", arcType.ToString());
      m_pDumper.output("extrusion", toString(pExtrusion));
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced circularArc data");
        m_pDumper.pushIndent();
        base.circularArcProc(firstPoint, secondPoint, thirdPoint,
          arcType, pExtrusion);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End circularArcProc");
    }

    /************************************************************************/
    /* Process mesh data                                                    */
    /************************************************************************/
    public override void meshProc(MeshData mesh)
    {
      m_pDumper.output("Start meshProc");
      m_pDumper.pushIndent();

      m_pDumper.output("Rows", mesh.NumRows.ToString());
      m_pDumper.output("Columns", mesh.NumColumns.ToString());

      m_pDumper.pushIndent();
      int k = 0;
      for (Int32 row = 0; row < mesh.NumRows; row++)
      {
        for (Int32 col = 0; col < mesh.NumColumns; col++)
        {
          m_pDumper.output(string.Format("Vertex[{0}, {1}]", col, row), toString(mesh.VertexList[k++]));
        }
      }

      m_pDumper.outputEdgeData(mesh.EdgeData);
      m_pDumper.outputFaceData(mesh.FaceData);
      m_pDumper.outputVertexData(mesh.VertexData);

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced shell data");
        m_pDumper.pushIndent();
        base.meshProc(mesh);
        m_pDumper.popIndent();
      }

      m_pDumper.popIndent();
      m_pDumper.output("End meshProc");
    }
    public override void shellProc(ShellData shell)
    {
      m_pDumper.output("Start shellProc");
      m_pDumper.pushIndent();
      m_pDumper.output("numVertices", shell.Points.Length.ToString());
      m_pDumper.output(shell.Points);

      /**********************************************************************/
      /* Count and dump faces, count edges                                  */
      /**********************************************************************/
      Int32 i = 0;
      Int32 numFaces = 0;
      Int32 numEdges = 0;
      m_pDumper.output("Faces");
      m_pDumper.pushIndent();
      while (i < shell.Faces.Length)
      {
        Int32 count = shell.Faces[i++];
        if (count < 0) count *= -1;
        numEdges += count;
        String face = "{";
        for (Int32 j = 0; j < count; j++, i++)
        {
          if (j != 0)
          {
            face += " ";
          }
          face += shell.Faces[i].ToString();
        }
        face += "}";
        m_pDumper.output(String.Format("Face[{0}]", numFaces++), face);
      }
      m_pDumper.popIndent();

      m_pDumper.outputEdgeData(shell.EdgeData);
      m_pDumper.outputFaceData(shell.FaceData);
      m_pDumper.outputVertexData(shell.VertexData);

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced shell data");
        m_pDumper.pushIndent();
        base.shellProc(shell);
        m_pDumper.popIndent();
      }

      m_pDumper.popIndent();
      m_pDumper.output("End shellProc");
    }

    /************************************************************************/
    /* Process text                                                         */
    /************************************************************************/
    public override void textProc(OdGePoint3d position,
                                OdGeVector3d direction,
                                OdGeVector3d upVector,
                                string msg, bool raw,
                                OdGiTextStyle pTextStyle,
                                OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start textProc");
      m_pDumper.pushIndent();
      m_pDumper.output("position", toString(position));
      m_pDumper.output("direction", toString(direction));
      m_pDumper.output("upVector", toString(upVector));
      m_pDumper.output("msg", msg);
      m_pDumper.output("raw", raw.ToString());
      m_pDumper.output("Extrusion vector", toString(pExtrusion));
      m_pDumper.output(pTextStyle);

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced text data");
        m_pDumper.pushIndent();
        base.textProc(position, direction, upVector, msg, raw, pTextStyle, pExtrusion);
        m_pDumper.popIndent();
      }

      m_pDumper.popIndent();
      m_pDumper.output("End textProc");
    }
    /************************************************************************/
    /* Process shape data                                                   */
    /************************************************************************/
    public override void shapeProc(OdGePoint3d position,
                                 OdGeVector3d direction,
                                 OdGeVector3d upVector,
                                 int shapeNumber,
                                 OdGiTextStyle pTextStyle,
                                 OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start shapeProc");
      m_pDumper.pushIndent();
      m_pDumper.output("position", toString(position));
      m_pDumper.output("direction", toString(direction));
      m_pDumper.output("upVector", toString(upVector));
      m_pDumper.output("shapeNumber", shapeNumber.ToString());
      m_pDumper.output("Extrusion vector", toString(pExtrusion));
      m_pDumper.output(pTextStyle);

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced shape data");
        m_pDumper.pushIndent();
        base.shapeProc(position, direction, upVector, shapeNumber, pTextStyle, pExtrusion);
        m_pDumper.popIndent();
      }

      m_pDumper.popIndent();
      m_pDumper.output("End shapeProc");
    }

    /************************************************************************/
    /* Process xline data                                                   */
    /************************************************************************/
    public override void xlineProc(OdGePoint3d firstPoint, OdGePoint3d secondPoint)
    {
      m_pDumper.output("Start xlineProc");
      m_pDumper.pushIndent();
      m_pDumper.output("firstPoint", toString(firstPoint));
      m_pDumper.output("secondPoint", toString(secondPoint));
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced xline data");
        m_pDumper.pushIndent();
        base.xlineProc(firstPoint, secondPoint);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End xlineProc");
    }

    /************************************************************************/
    /* Process ray data                                                     */
    /************************************************************************/
    public override void rayProc(OdGePoint3d basePoint, OdGePoint3d throughPoint)
    {
      m_pDumper.output("Start rayProc");
      m_pDumper.pushIndent();
      m_pDumper.output("basePoint", toString(basePoint));
      m_pDumper.output("throughPoint", toString(throughPoint));
      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced ray data");
        m_pDumper.pushIndent();
        base.rayProc(basePoint, throughPoint);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End rayProc");
    }

    /************************************************************************/
    /* Process nurbs data                                                      */
    /************************************************************************/
    public override void nurbsProc(OdGeNurbCurve3d nurbs)
    {

      m_pDumper.output("Start nurbsProc");
      m_pDumper.pushIndent();

      m_pDumper.output("degree", nurbs.degree().ToString());
      m_pDumper.output("order", nurbs.order().ToString());
      m_pDumper.output("isClosed", nurbs.isClosed().ToString());
      m_pDumper.output("isRational", nurbs.isRational().ToString());
      m_pDumper.output("startParam", nurbs.startParam().ToString());
      m_pDumper.output("endParam", nurbs.endParam().ToString());


      m_pDumper.output("Knots", nurbs.numKnots().ToString());
      m_pDumper.pushIndent();
      for (int i = 0; i < nurbs.numKnots(); i++)
      {
        m_pDumper.output(string.Format("Knot[{0}]", i), nurbs.knotAt(i).ToString());
      }
      m_pDumper.popIndent();

      m_pDumper.output("Control points", nurbs.numControlPoints().ToString());
      m_pDumper.pushIndent();
      for (int i = 0; i < nurbs.numControlPoints(); i++)
      {
        m_pDumper.output(string.Format("Control point[{0}]", i), toString(nurbs.controlPointAt(i)));
      }
      m_pDumper.popIndent();

      m_pDumper.output("Fit points", nurbs.numFitPoints().ToString());
      m_pDumper.pushIndent();
      for (int i = 0; i < nurbs.numFitPoints(); i++)
      {
        OdGePoint3d point = new OdGePoint3d();
        nurbs.getFitPointAt(i, point);
        m_pDumper.output(String.Format("Fit point[{0}]", i), toString(point));
      }

      if (nurbs.isRational())
      {
        m_pDumper.output("Weights", nurbs.numWeights().ToString());
        m_pDumper.pushIndent();
        for (int i = 0; i < nurbs.numWeights(); i++)
        {
          m_pDumper.output(String.Format("Weight[{0}]", i), nurbs.weightAt(i).ToString());
        }
        m_pDumper.popIndent();
      }

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced nurbs data");
        m_pDumper.pushIndent();
        base.nurbsProc(nurbs);
        m_pDumper.popIndent();
      }

      m_pDumper.popIndent();
      m_pDumper.output("End nurbsProc");
    }
    string toDegreeString(double val)
    {
      return (val * 180.0 / Math.PI).ToString() + "d";
    }

    /************************************************************************/
    /* Process ellipArc data                                                */
    /************************************************************************/
    public override void ellipArcProc(OdGeEllipArc3d ellipArc,
                                        OdGePoint3d[] endPointsOverrides,
                                        OdGiArcType arcType,
                                        OdGeVector3d pExtrusion)
    {
      m_pDumper.output("Start ellipArcProc");
      m_pDumper.pushIndent();
      m_pDumper.output("Center", toString(ellipArc.center()));
      m_pDumper.output("Normal", toString(ellipArc.normal()));
      m_pDumper.output("Major Radius", ellipArc.majorRadius().ToString());
      m_pDumper.output("Minor Radius", ellipArc.minorRadius().ToString());
      m_pDumper.output("Major Axis", toString(ellipArc.majorAxis()));
      m_pDumper.output("Minor Axis", toString(ellipArc.minorAxis()));
      m_pDumper.output("Start Angle", toDegreeString(ellipArc.startAng()));
      m_pDumper.output("End Angle", toDegreeString(ellipArc.endAng()));

      if (endPointsOverrides != null)
      {
        m_pDumper.output("Start point override", toString(endPointsOverrides[0]));
        m_pDumper.output("End point override", toString(endPointsOverrides[1]));
      }

      m_pDumper.output("arcType", arcType.ToString());
      m_pDumper.output("Extrusion vector", toString(pExtrusion));

      if (m_dumpLevel == Simplification.Maximal_Simplification)
      {
        m_pDumper.output("Reduced ellipArc data");
        m_pDumper.pushIndent();
        base.ellipArcProc(ellipArc, endPointsOverrides, arcType, pExtrusion);
        m_pDumper.popIndent();
      }
      m_pDumper.popIndent();
      m_pDumper.output("End ellipArcProc");
    }
    /************************************************************************/
    /* Process rasterImage data                                             */
    /************************************************************************/
    public override void rasterImageProc(OdGePoint3d origin,
                                           OdGeVector3d u,
                                           OdGeVector3d v,
                                           OdGiRasterImage pImg, // image object
                                           OdGePoint2d[] uvBoundary, // may not be null
                                           bool transparency,
                                           double brightness,
                                           double contrast,
                                           double fade)
    {
      m_pDumper.output("Start rasterImageProc");
      m_pDumper.pushIndent();

      m_pDumper.output("origin", toString(origin));
      m_pDumper.output("u", toString(u));
      m_pDumper.output("v", toString(v));
      m_pDumper.output("transparency", transparency.ToString());
      m_pDumper.output("brightness", brightness.ToString());
      m_pDumper.output("contrast", contrast.ToString());
      m_pDumper.output("fade", fade.ToString());

      m_pDumper.output("uvBoundary");
      m_pDumper.pushIndent();
      for (UInt32 i = 0; i < uvBoundary.Length; i++)
      {
        m_pDumper.output(String.Format("Vertex[{0}]", (int)i), toString(uvBoundary[i]));
      }
      m_pDumper.popIndent();

      m_pDumper.popIndent();
      m_pDumper.output("End rasterImageProc");
    }

    /************************************************************************/
    /* Process metafile data                                                */
    /************************************************************************/
    public override void metafileProc(OdGePoint3d origin,
                                        OdGeVector3d u,
                                        OdGeVector3d v,
                                        OdGiMetafile pMetafile,
                                        bool dcAligned,
                                        bool allowClipping)
    {
      m_pDumper.output("Start metafileProc");
      m_pDumper.pushIndent();
      m_pDumper.output("origin", toString(origin));
      m_pDumper.output("u", toString(u));
      m_pDumper.output("v", toString(v));
      m_pDumper.output("dcAligned", dcAligned.ToString());
      m_pDumper.output("allowClipping", allowClipping.ToString());
      m_pDumper.popIndent();
      m_pDumper.output("End metafileProc");
    }
  }
}
