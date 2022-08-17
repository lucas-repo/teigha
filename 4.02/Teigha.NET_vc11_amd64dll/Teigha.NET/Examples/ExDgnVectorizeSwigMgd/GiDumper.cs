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
using System.IO;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnVectorizeMgd
{
  public abstract class OdGiDumper
  {
    public abstract void output(string label, OdGeMatrix3d xfm);
    public abstract void output(string str);
    public abstract void output(string str1, string str2);
    public abstract void output(OdGePoint3d[] points);
    public abstract void output(OdGiTextStyle textStyle);
    public abstract void outputEdgeData(Teigha.Core.EdgeData pEdgeData);
    public abstract void outputFaceData(Teigha.Core.FaceData pFaceData);
    public abstract void outputVertexData(Teigha.Core.VertexData pVertexData);
    public abstract void outputColors(UInt16[] colors, string name);
    public abstract void outputTrueColors(OdCmEntityColor[] trueColors, string name);
    public abstract void outputIds(OdDbStub[] ids, string name, string table);
    public abstract void outputSelectionMarkers(IntPtr[] selectionMarkers, string name);
    public abstract void outputVisibility(byte[] visibility, string name);
    public abstract void pushIndent();
    public abstract void popIndent();
  }


  internal class OdGiDumperImpl : OdGiDumper
  {
    int m_indentLevel;
    public OdGiDumperImpl() { m_indentLevel = 0; }
    public override void output(string label, OdGeMatrix3d xfm)
    {
      if (xfm != null)
      {
        for (int i = 0; i < 4; i++)
        {
          string leftString = (i != 0) ? "" : label;
          string rightString = "[";
          for (int j = 0; j < 4; j++)
          {
            if (j != 0)
            {
              rightString = rightString + " ";
            }
            rightString += xfm[i, j].ToString();
          }
          rightString = rightString + "]";
          output(leftString, rightString);
        }
      }
      else
      {
        output(label, "[]");
      }
    }
    public override void output(string str)
    {
      System.Text.StringBuilder indent = new System.Text.StringBuilder();
      for (int i = 0; i < m_indentLevel; i++)
        indent.Append(' ');
      Console.WriteLine(indent + str);
    }
    public override void output(string str1, string str2)
    {
      string leader = ". . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ";
      string buffer = str1;
      int colWidth = 38;
      //*********************************************************************
      //* If rightString is not specified, just output the indented          
      //* leftString. Otherwise, fill the space between leftString and       
      //* rightString with leader characters.                                
      //*********************************************************************
      if (str2 != "")
      {
        int leaders = colWidth - (str1.Length + m_indentLevel);
        if (leaders > 0)
        {
          buffer = str1 + leader.Substring(str1.Length + m_indentLevel, leaders) + str2;
        }
        else
        {
          buffer = ">" + str1 + " " + str2;
        }
      }
      output(buffer);
    }
    public override void output(OdGePoint3d[] points)
    {
      pushIndent();
      for (Int32 i = 0; i < points.Length; ++i)
      {
        output("Vertex[" + i.ToString() + "] (" + points[i].x.ToString() + ", " + points[i].y.ToString() + ")");
      }
      popIndent();
    }
    string shortenPath(string path)
    {
      return shortenPath(path, 40);
    }
    string shortenPath(string path, int maxPath)
    {
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
        return path.Substring(maxPath - 3) + "...";
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
    string toDegreeString(double val)
    {
      return (val * 180.0 / Math.PI).ToString() + "d";
    }
    public override void output(OdGiTextStyle textStyle)
    {
      output("Text Style");
      pushIndent();
      if (textStyle.isTtfFont())
      {
        string typeface = "";
        bool bold;
        bool italic;
        int charset;
        int pitchAndFamily;
        textStyle.font(ref typeface, out bold, out italic, out charset, out pitchAndFamily);
        output("Typeface", typeface.ToString());
        output("Character Set", charset.ToString());
        output("Bold", bold.ToString());
        output("Italic", italic.ToString());
        output("Font Pitch and Family", pitchAndFamily.ToString("X"));
      }
      else
      {
        output("Filename", shortenPath(textStyle.ttfdescriptor().fileName()));
        output("BigFont Filename", shortenPath(textStyle.bigFontFileName()));
      }
      output("Shape File", textStyle.isShape().ToString());
      output("Text Height", textStyle.textSize().ToString());
      output("Width Factor", textStyle.xScale().ToString());
      output("Obliquing Angle", toDegreeString(textStyle.obliquingAngle()));
      output("Tracking Percentage", toDegreeString(textStyle.trackingPercent()));
      output("Backwards", textStyle.isBackward().ToString());
      output("Vertical", textStyle.isVertical().ToString());
      output("Upside Down", textStyle.isUpsideDown().ToString());
      output("Underlined", textStyle.isUnderlined().ToString());
      output("Overlined", textStyle.isOverlined().ToString());

      popIndent();
    }
    public override void outputEdgeData(EdgeData pEdgeData)
    {
      if (pEdgeData != null)
      {
        outputColors(pEdgeData.Colors, "Edge");
        outputTrueColors(pEdgeData.TrueColors, "Edge");
        outputIds(pEdgeData.LayerIds, "Edge", "Layers");
        outputIds(pEdgeData.LinetypeIds, "Edge", "Linetypes");
        outputSelectionMarkers(pEdgeData.SelectionMarkers, "Edge");
        outputVisibility(pEdgeData.Visibilities, "Edge");
      }
    }
    public override void outputFaceData(FaceData pFaceData)
    {
      if (pFaceData != null)
      {
        outputColors(pFaceData.Colors, "Face");
        outputTrueColors(pFaceData.TrueColors, "Face");
        outputIds(pFaceData.LayerIds, "Face", "Layers");
        outputSelectionMarkers(pFaceData.SelectionMarkers, "Face");
        outputVisibility(pFaceData.Visibilities, "Face");
      }
    }
    public override void outputVertexData(VertexData pVertexData)
    {
      if (pVertexData != null)
      {
        if (pVertexData.Normals != null)
        {
          String orientation = "???";
          switch (pVertexData.OrientationFlag)
          {
            case OdGiOrientationType.kOdGiCounterClockwise: orientation = "kOdGiCounterClockwise "; break;
            case OdGiOrientationType.kOdGiNoOrientation: orientation = "kOdGiNoOrientation"; break;
            case OdGiOrientationType.kOdGiClockwise: orientation = "kOdGiClockwise"; break;
          }
          output("Vertex Orientation Flag", orientation);

          output("Vertex Normals");
          pushIndent();
          for (Int32 i = 0; i < pVertexData.Normals.Length; ++i)
          {
            output(String.Format("Vertex[{0}]", i), OdGiConveyorGeometryDumper.toString(pVertexData.Normals[i]));
          }
          popIndent();

          outputTrueColors(pVertexData.TrueColors, "Vertex");
        }
      }
    }
    public override void outputColors(UInt16[] colors, string name)
    {
      if (colors != null)
      {
        output(name + " Colors");
        pushIndent();
        for (int i = 0; i < colors.Length; i++)
        {
          output(String.Format(name + "[{0}]", i), String.Format("ACI {0}", colors[i]));
        }
        popIndent();
      }
    }
    /************************************************************************/
    /* Convert the specified value to an OdDgCmEntityColor string           */
    /*                                                                      */
    /************************************************************************/
    String toString(OdCmEntityColor val)
    {
      String retVal = "???";
      if (val.isByLayer())
      {
        retVal = "ByLayer";
      }
      else if (val.isByBlock())
      {
        retVal = "ByBlock";
      }
      else if (val.isForeground())
      {
        retVal = "Foreground";
      }
      else if (val.isNone())
      {
        retVal = "None";
      }
      else if (val.isByACI()
               || val.isByDgnIndex())
      {
        retVal = String.Format("ACI {0}", val.colorIndex());
      }
      else if (val.isByColor())
      {
        retVal = "ByColor" + " r" + val.red().ToString() + ":g"
            + val.green().ToString() + ":b" + val.blue().ToString();
      }
      return retVal;
    }
    public override void outputTrueColors(OdCmEntityColor[] trueColors, string name)
    {
      if (trueColors != null)
      {
        output(name + " TrueColors");
        pushIndent();
        for (int i = 0; i < trueColors.Length; i++)
        {
          output(String.Format(name + "[{0}]", i), toString(trueColors[i]));
        }
        popIndent();
      }
    }
    /************************************************************************/
    /* Convert the specified value to an OdRxClass name string              */
    /************************************************************************/
    String toString(OdRxClass val)
    {
      return "<" + val.name() + ">";
    }

    /************************************************************************/
    /* Convert the specified value to an OdDgObjectId string                */
    /************************************************************************/
    String toString(OdDgElementId val)
    {
      if (val.isNull())
      {
        return "Null";
      }

      if (val.isErased())
      {
        return "Erased";
      }

      /**********************************************************************/
      /* Open the object                                                    */
      /**********************************************************************/
      //  OdRxObjectPtr pObject = val.safeOpenObject();
      OdDgElement pElm = (OdDgElement)val.safeOpenObject();

      /**********************************************************************/
      /* Return the name of an OdDgTableRecord                              */
      /**********************************************************************/
      if (pElm is OdDgTableRecord)
      {
        OdDgTableRecord pRec = (OdDgTableRecord)pElm;
        return pRec.getName();
      }

      /**********************************************************************/
      /* Return the name of an OdDgSymbolTableRecord                        */
      /**********************************************************************/
      //  if (pObject.isKindOf(OdDgSymbolTableRecord::desc()))
      //  {
      //    OdDgSymbolTableRecordPtr pSTR = pObject;
      //    return pSTR.getName(); 
      //  }

      /**********************************************************************/
      /* Return the name of an OdDgMlineStyle                               */
      /**********************************************************************/
      //  if (pObject.isKindOf(OdDgMlineStyle::desc()))
      //  {
      //    OdDgMlineStylePtr pStyle = pObject;
      //    return pStyle.name(); 
      //  }

      /**********************************************************************/
      /* Return the name of a PlotStyle                                      */
      /**********************************************************************/
      //  if (pObject.isKindOf(OdDgPlaceHolder::desc()))
      //  {
      //    OdDgDictionaryPtr pDictionary = val.database().getPlotStyleNameDictionaryId().safeOpenObject(); 
      //    OdString plotStyleName = pDictionary.nameAt(val);
      //    return plotStyleName; 
      //  }

      /**********************************************************************/
      /* Return the name of an OdDgMaterial                                 */
      /**********************************************************************/
      //  if (pObject.isKindOf(OdDgMaterial::desc()))
      //  {
      //    OdDgMaterialPtr pMaterial = pObject;
      //    return pMaterial.name(); 
      //  }

      /**********************************************************************/
      /* We don't know what it is, so return the description of the object  */
      /* object specified by the ObjectId                                   */
      /**********************************************************************/
      //  return toString(pObject.isA());
      return toString(pElm.isA());
    }
    public override void outputIds(OdDbStub[] ids, string name, string table)
    {
      if (ids != null)
      {
        output(name + " " + table);
        pushIndent();
        for (int i = 0; i < ids.Length; i++)
        {
          OdDgElementId id = new OdDgElementId(ids[i]);
          output(String.Format(name + "[{0}]", i), toString(id));
        }
        popIndent();
      }
    }
    public override void outputSelectionMarkers(IntPtr[] selectionMarkers, string name)
    {
      if (selectionMarkers != null)
      {
        output(name + " Selection Markers");
        pushIndent();
        for (int i = 0; i < selectionMarkers.Length; i++)
        {
          output(String.Format(name + "[{0}]", i), selectionMarkers[i].ToString());
        }
      }
    }
    public override void outputVisibility(byte[] visibility, string name)
    {
      if (visibility != null)
      {
        output(name + " Visibility");
        pushIndent();

        for (int i = 0; i < visibility.Length; i++)
        {
          String vis = "???";
          switch ((OdGiVisibility)visibility[i])
          {
            case OdGiVisibility.kOdGiInvisible: vis = "kOdGiInvisible"; break;
            case OdGiVisibility.kOdGiVisible: vis = "kOdGiVisible"; break;
            case OdGiVisibility.kOdGiSilhouette: vis = "kOdGiSilhouette"; break;
          }
          output(String.Format(name + "[{0}]", i), vis);
        }
        popIndent();
      }
    }
    public override void pushIndent() { m_indentLevel += 2; }
    public override void popIndent() { m_indentLevel -= 2; }
  };

}