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
using Teigha.TD;
using System.IO;
using Microsoft.Win32;

namespace OdReadEx
{
  /************************************************************************/
  /*      Define custom OdDbGripPointsPE                                  */
  /************************************************************************/
    class OdDbCircleGripPointsPE : OdDbGripPointsPE
    {
        public OdGePoint3d getPlanePoint(OdDbCircle circle, OdGePoint3d Point)
        {
            OdGePlane       plane = new OdGePlane();                     // recalculated Point in plane of pCircle
            Planarity planarity = new Planarity();

            circle.getPlane(plane, out planarity);
            plane.intersectWith(new OdGeLine3d(Point, circle.normal()), Point);

            return Point;
        }

        public bool projectOffset(OdDbDatabase pDb, OdGeVector3d vNormal, OdGeVector3d vOffset)
        {
          OdDbObjectId idVp = pDb.activeViewportId();
          OdDbObject pVp = (OdDbObject)idVp.openObject();
          OdAbstractViewPE pAVp = OdAbstractViewPE.cast(pVp);
          if (null != pAVp)
          {
            OdGeVector3d vViewDir = pAVp.direction(pVp);
            if (!vViewDir.isPerpendicularTo(vNormal))
            {
              OdGePlane pln = new OdGePlane(OdGePoint3d.kOrigin, vNormal);
              OdGeMatrix3d mProj = new OdGeMatrix3d();
              mProj.setToProjection(pln, vViewDir);
              vOffset.transformBy(mProj);
              return true;
            }
          }
          return false;
        }

        public override OdResult getGripPoints(OdDbEntity ent, OdGePoint3dArray gripPoints)
        {
            Console.WriteLine("OdDbCircleGripPointsPE getGripPoints call");
            return base.getGripPoints(ent, gripPoints);
        }
        //----------------------------------------------------------------------------------------------------------------------------
        // Move circle or change radius
        public override OdResult moveGripPointsAt(OdDbEntity pEnt, OdIntArray indices, OdGeVector3d vOffset)
        {
            Console.WriteLine("OdDbCircleGripPointsPE moveGripPointsAt call");
            return base.moveGripPointsAt(pEnt, indices, vOffset);
        }
        //----------------------------------------------------------------------------------------------------------------------------
        // Cannot be stretched
        public override OdResult getStretchPoints(OdDbEntity pEnt, OdGePoint3dArray stretchPoints )
        {
            Console.WriteLine("OdDbCircleGripPointsPE getStretchPoints call");
            return base.getStretchPoints(pEnt, stretchPoints);
        }

        public override OdResult moveStretchPointsAt( OdDbEntity pEnt, OdIntArray indices_, OdGeVector3d offset )
        {
            Console.WriteLine("OdDbCircleGripPointsPE moveStretchPointsAt call");
            return base.moveStretchPointsAt(pEnt, indices_, offset);
        }
        //----------------------------------------------------------------------------------------------------------------------------
        // Return snap Points into snapPoints, depending on type sonapMode
//                 getOsnapPoints(OdDbEntity pEntity, OsnapMode osnapMode, IntPtr /*SIMPLETYPE*/ gsSelectionMark, OdGePoint3d pickPoint, OdGePoint3d lastPoint, OdGeMatrix3d xWorldToEye, OdGePoint3dArray snapPoints) {
        public override OdResult getOsnapPoints(OdDbEntity ent, OsnapMode sonapMode, IntPtr gsSelectionMark, OdGePoint3d pickPoint_, OdGePoint3d lastPoint_, OdGeMatrix3d xWorldToEye, OdGePoint3dArray snapPoints)
        {
            Console.WriteLine("OdDbCircleGripPointsPE getOsnapPoints call");
            return base.getOsnapPoints(ent, sonapMode, gsSelectionMark, pickPoint_, lastPoint_, xWorldToEye, snapPoints);
        }
    }

  /************************************************************************/
  /* Define an a Custom OdDbAuditInfo class                               */
  /************************************************************************/
  class AuditInfo : OdDbAuditInfo
  {
    OdStreamBuf m_ReportFile;
    bool m_isSomething;
    String m_ReportFileName;

    public AuditInfo(String reportFileName)
    {
      m_isSomething = false;
      m_ReportFileName = reportFileName;
      /********************************************************************/
      /* Create a report file                                             */
      /********************************************************************/
      m_ReportFile = Globals.odrxSystemServices().createFile(m_ReportFileName,
        FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways);
    }
    public override void Dispose()
    {
      /********************************************************************/
      /* Refer user to OD_T("Audit Report.txt") if error(s) have been output.   */
      /********************************************************************/
      if (m_isSomething)
      {
        Console.WriteLine("\n\nAudit error : Check \"{0}\" for errors.", m_ReportFileName);
      }
      else
      {
        Console.WriteLine("\n\nAudit : ok.\n");
      }
      base.Dispose();
    }

    /**********************************************************************/
    /* Print error to ReportFile                                          */
    /**********************************************************************/
    public override void printError(String strName, String strValue, String strValidation, String strDefaultValue)
    {
      m_isSomething = true;
      String str = String.Format("{0} {1} {2} {3}\n",
        strName, strValue, strValidation, strDefaultValue);
      m_ReportFile.putBytes(Encoding.Unicode.GetBytes(str));
    }

    /**********************************************************************/
    /* Print info to ReportFile                                           */
    /**********************************************************************/
    public override void printInfo(String strInfo)
    {
      m_ReportFile.putBytes(Encoding.Unicode.GetBytes(strInfo + "\n"));
    }
  }

  class Srv : ExHostAppServices
  {
    public override bool getPassword(string dwgName, bool isXref, ref string password)
    {
      Console.WriteLine("Enter password to open drawing: {0}", dwgName);
      password = Console.ReadLine().ToUpper();
      return password != "";
    }

    public override String product()
    {
      return String.Format("Teigha.NET");
    }
    bool GetRegistryString(RegistryKey rKey, String subkey, String name, out String value)
    {
      bool rv = false;
      object objData = null;

      RegistryKey regKey;
      regKey = rKey.OpenSubKey(subkey);
      if (regKey != null)
      {
        objData = regKey.GetValue(name);
        if (objData != null)
        {
          rv = true;
        }
        regKey.Close();
      }
      if (rv)
        value = objData.ToString();
      else
        value = String.Format("");

      rKey.Close();
      return rv;
    }

    String GetRegistryAVEMAPSFromProfile()
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        // get the value for the ACAD entry in the registry
        String tmp;
        if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("AVEMAPS"), out tmp))
          return tmp;
      }
      return String.Format("");
    }

    String GetRegistryAcadProfilesKey()
    {
      String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
      String tmp;

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}\\Profiles", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);
      return subkey;
    }

    String GetRegistryAcadLocation()
    {
      String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
      String tmp;

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
        return String.Format("");
      return tmp;
    }

    public String FindConfigPath(String configType)
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        String searchPath;
        if (GetRegistryString(Registry.CurrentUser, subkey, configType, out searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    private String FindConfigFile(String configType, String file)
    {
      String searchPath = FindConfigPath(configType);
      if (searchPath.Length > 0)
      {
        searchPath = String.Format("{0}\\{1}", searchPath, file);
        if (System.IO.File.Exists(searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    String GetRegistryACADFromProfile()
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        // get the value for the ACAD entry in the registry
        String tmp;
        if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("ACAD"), out tmp))
          return tmp;
      }
      return String.Format("");
    }

    public override String findFile(String fileName, OdRxObject pDb, OdDbBaseHostAppServices.FindFileHint hint)
    {
      Console.WriteLine("findFileName: {0}", fileName);

      String sFile = base.findFile(fileName, pDb, hint);
      if (sFile.Length > 0)
        return sFile;

      String strFileName = fileName;
      String ext;
      if (strFileName.Length > 3)
        ext = strFileName.Substring(strFileName.Length - 4, 4).ToUpper();
      else
        ext = fileName.ToUpper();
      if (ext == String.Format(".PC3"))
        return FindConfigFile(String.Format("PrinterConfigDir"), fileName);
      if (ext == String.Format(".STB") || ext == String.Format(".CTB"))
        return FindConfigFile(String.Format("PrinterStyleSheetDir"), fileName);
      //return System.IO.Path.Combine(Environment.GetEnvironmentVariable("DDPLOTSTYLEPATHS"), fileName);
      if (ext == String.Format(".PMP"))
        return FindConfigFile(String.Format("PrinterDescDir"), fileName);

      switch (hint)
      {
        case FindFileHint.kFontFile:
        case FindFileHint.kCompiledShapeFile:
        case FindFileHint.kTrueTypeFontFile:
        case FindFileHint.kPatternFile:
        case FindFileHint.kFontMapFile:
        case FindFileHint.kTextureMapFile:
          break;
        default:
          return sFile;
      }

      if (hint != FindFileHint.kTextureMapFile && ext != String.Format(".SHX") && ext != String.Format(".PAT") && ext != String.Format(".TTF") && ext != String.Format(".TTC"))
      {
        strFileName += String.Format(".shx");
      }
      else if (hint == FindFileHint.kTextureMapFile)
      {
        strFileName.Replace(String.Format("/"), String.Format("\\"));
        int last = strFileName.LastIndexOf("\\");
        if (last == -1)
          strFileName = "";
        else
          strFileName = strFileName.Substring(0, last);
      }


      sFile = (hint != FindFileHint.kTextureMapFile) ? GetRegistryACADFromProfile() : GetRegistryAVEMAPSFromProfile();
      while (sFile.Length > 0)
      {
        int nFindStr = sFile.IndexOf(";");
        String sPath;
        if (-1 == nFindStr)
        {
          sPath = sFile;
          sFile = String.Format("");
        }
        else
        {
          sPath = String.Format("{0}\\{1}", sFile.Substring(0, nFindStr), strFileName);
          if (System.IO.File.Exists(sPath))
          {
            return sPath;
          }
          sFile = sFile.Substring(nFindStr + 1, sFile.Length - nFindStr - 1);
        }
      }

      if (hint == FindFileHint.kTextureMapFile)
      {
        return sFile;
      }

      if (sFile.Length <= 0)
      {
        String sAcadLocation = GetRegistryAcadLocation();
        if (sAcadLocation.Length > 0)
        {
          sFile = String.Format("{0}\\Fonts\\{1}", sAcadLocation, strFileName);
          if (System.IO.File.Exists(sFile))
          {
            sFile = String.Format("{0}\\Support\\{1}", sAcadLocation, strFileName);
            if (System.IO.File.Exists(sFile))
            {
              sFile = String.Format("");
            }
          }
        }
      }
      return sFile;
    }
  }
  class Program
  {
    public static String toString(OdDbObjectId val)
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
      OdDbObject pObject = val.safeOpenObject();

      /**********************************************************************/
      /* Return the name of an OdDbSymbolTableRecord                        */
      /**********************************************************************/
      if (pObject is OdDbSymbolTableRecord)
      {
        OdDbSymbolTableRecord pSTR = (OdDbSymbolTableRecord)pObject;
        return pSTR.getName();
      }

      /**********************************************************************/
      /* Return the name of an OdDbMlineStyle                               */
      /**********************************************************************/
      if (pObject is OdDbMlineStyle)
      {
        OdDbMlineStyle pStyle = (OdDbMlineStyle)pObject;
        return pStyle.name();
      }

      /**********************************************************************/
      /* Return the name of a PlotStyle                                      */
      /**********************************************************************/
      if (pObject is OdDbPlaceHolder)
      {
        OdDbDictionary pDictionary = (OdDbDictionary)val.database().getPlotStyleNameDictionaryId().safeOpenObject();
        String plotStyleName = pDictionary.nameAt(val);
        return plotStyleName;
      }

      /**********************************************************************/
      /* Return the name of an OdDbMaterial                                 */
      /**********************************************************************/
      if (pObject is OdDbMaterial)
      {
        OdDbMaterial pMaterial = (OdDbMaterial)pObject;
        return pMaterial.name();
      }

      /**********************************************************************/
      /* We don't know what it is, so return the description of the object  */
      /* object specified by the ObjectId                                   */
      /**********************************************************************/
      return pObject.isA().name();
    }
    public static String toDegreeString(double val)
    {
      return (val * 180.0 / Math.PI).ToString() + "\u00B0";
    }

    /************************************************************************/
    /* Shorten a path with ellipses.                                        */
    /************************************************************************/
    public static String shortenPath(String path)
    {
      return shortenPath(path, 40);
    }
    static String shortenPath(String path, int maxPath)
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
    /************************************************************************/
    /* Convert the specified value to a LoopType string                     */
    /************************************************************************/
    static String toLooptypeString(OdDbHatch.HatchLoopType loopType)
    {
      String retVal = "";
      if ((loopType & OdDbHatch.HatchLoopType.kExternal) != 0)
        retVal += " | kExternal";

      if ((loopType & OdDbHatch.HatchLoopType.kPolyline) != 0)
        retVal += " | kPolyline";

      if ((loopType & OdDbHatch.HatchLoopType.kDerived) != 0)
        retVal += " | kDerived";

      if ((loopType & OdDbHatch.HatchLoopType.kTextbox) != 0)
        retVal += " | kTextbox";

      if ((loopType & OdDbHatch.HatchLoopType.kOutermost) != 0)
        retVal += " | kOutermost";

      if ((loopType & OdDbHatch.HatchLoopType.kNotClosed) != 0)
        retVal += " | kNotClosed";

      if ((loopType & OdDbHatch.HatchLoopType.kSelfIntersecting) != 0)
        retVal += " | kSelfIntersecting";

      if ((loopType & OdDbHatch.HatchLoopType.kTextIsland) != 0)
        retVal += " | kTextIsland";

      if ((loopType & OdDbHatch.HatchLoopType.kDuplicate) != 0)
        retVal += " | kDuplicate";

      return retVal == "" ? "kDefault" : retVal.Substring(3);
    }
    /************************************************************************/
    /* Convert the specified value to an ArcSymbolType string               */
    /************************************************************************/
    public static String toArcSymbolTypeString(int val)
    {
      String retVal = "???";
      switch (val)
      {
        case 0: retVal = "Precedes text"; break;
        case 1: retVal = "Above text"; break;
        case 2: retVal = "None"; break;
      }
      return retVal;
    }
    /************************************************************************/
    /* Dump data common to all entities                                     */
    /************************************************************************/
    static void dumpEntityData(OdDbEntity pEnt)
    {
      OdGeExtents3d extents = new OdGeExtents3d();
      if (OdResult.eOk == pEnt.getGeomExtents(extents))
      {
        Console.WriteLine("Min Extents = {0}", extents.minPoint().ToString());
        Console.WriteLine("Max Extents = {0}", extents.maxPoint().ToString());
      }
      Console.WriteLine("Layer = {0}", pEnt.layer());
      Console.WriteLine("Color Index = {0}", pEnt.colorIndex());
      Console.WriteLine("Color = {0}", pEnt.color().ToString());
      Console.WriteLine("Linetype = {0}", pEnt.linetype());
      Console.WriteLine("LTscale = {0}", pEnt.linetypeScale());
      Console.WriteLine("Lineweight = {0}", pEnt.lineWeight());
      Console.WriteLine("Plot Style = {0}", pEnt.plotStyleName());
      Console.WriteLine("Transparency Method = {0}", pEnt.transparency().method().ToString());
      Console.WriteLine("Visibility = {0}", pEnt.visibility().ToString());
      Console.WriteLine("Planar = {0}", pEnt.isPlanar());

      OdGePlane plane = new OdGePlane();
      Planarity planarity = Planarity.kNonPlanar;
      pEnt.getPlane(plane, out planarity);
      Console.WriteLine("Planarity = {0}", planarity);
      if (pEnt.isPlanar())
      {
        OdGePoint3d origin = new OdGePoint3d();
        OdGeVector3d uAxis = new OdGeVector3d();
        OdGeVector3d vAxis = new OdGeVector3d();
        plane.get(origin, uAxis, vAxis);
        Console.WriteLine("Origin = {0}", origin.ToString());
        Console.WriteLine("u-Axis = {0}", uAxis.ToString());
        Console.WriteLine("v-Axis = {0}", vAxis.ToString());
      }
    }
    /************************************************************************/
    /* Dump Text data                                                       */
    /************************************************************************/
    static void dumpTextData(OdDbText pText)
    {
      Console.WriteLine("Text String = {0}", pText.textString());
      Console.WriteLine("Text Position = {0}", pText.position().ToString());
      Console.WriteLine("Default Alignment = {0}", pText.isDefaultAlignment());
      Console.WriteLine("Alignment Point = {0}", pText.alignmentPoint().ToString());
      Console.WriteLine("Height = {0}", pText.height());
      Console.WriteLine("Rotation = {0}", toDegreeString(pText.rotation()));
      Console.WriteLine("Horizontal Mode = {0}", pText.horizontalMode().ToString());
      Console.WriteLine("Vertical Mode = {0}", pText.verticalMode().ToString());
      Console.WriteLine("Mirrored in X = {0}", pText.isMirroredInX());
      Console.WriteLine("Mirrored in Y = {0}", pText.isMirroredInY());
      Console.WriteLine("Oblique = {0}", toDegreeString(pText.oblique()));
      OdDbTextStyleTableRecord pRec = OdDbTextStyleTableRecord.cast(pText.textStyle().safeOpenObject());
      Console.WriteLine("Text Style = {0}", pRec.getName());//pText.textStyle().safeOpenObject().isA().name());
      Console.WriteLine("Width Factor = {0}", pText.widthFactor());

      /*********************************************************************/
      /* Dump Bounding Points                                               */
      /**********************************************************************/
      OdGePoint3dArray points = new OdGePoint3dArray();
      pText.getBoundingPoints(points);
      Console.WriteLine("TL Bounding Point = {0}", points[0]);
      Console.WriteLine("TR Bounding Point = {0}", points[1]);
      Console.WriteLine("BL Bounding Point = {0}", points[2]);
      Console.WriteLine("BR Bounding Point = {0}", points[3]);
      Console.WriteLine("Normal = {0}", pText.normal());
      Console.WriteLine("Thickness = {0}", pText.thickness());
      dumpEntityData(pText);
    }
    /************************************************************************/
    /* Dump Attribute data                                                  */
    /************************************************************************/
    static void dumpAttributeData(OdDbAttribute pAttr, int i)
    {
      Console.WriteLine("<{0}> {1}", pAttr.isA().name(), i);
      Console.WriteLine("Handle = {0}", pAttr.getDbHandle());
      Console.WriteLine("Tag = {0}", pAttr.tag());
      Console.WriteLine("Field Length = {0}", pAttr.fieldLength());
      Console.WriteLine("Invisible = {0}", pAttr.isInvisible());
      Console.WriteLine("Preset = {0}", pAttr.isPreset());
      Console.WriteLine("Verifiable = {0}", pAttr.isVerifiable());
      Console.WriteLine("Locked in Position = {0}", pAttr.lockPositionInBlock());
      Console.WriteLine("Constant = {0}", pAttr.isConstant());
      dumpTextData(pAttr);
    }
    /************************************************************************/
    /* Dump Block Reference Data                                             */
    /************************************************************************/
    static void dumpBlockRefData(OdDbBlockReference pBlkRef)
    {
      Console.WriteLine("Position = {0}", pBlkRef.position().ToString());
      Console.WriteLine("Rotation = {0}", toDegreeString(pBlkRef.rotation()));
      Console.WriteLine("Scale Factors = {0}", pBlkRef.scaleFactors().ToString());
      Console.WriteLine("Normal = {0}", pBlkRef.normal().ToString());
      dumpEntityData(pBlkRef);
      /**********************************************************************/
      /* Dump the attributes                                                */
      /**********************************************************************/
      OdDbObjectIterator pIter = (OdDbObjectIterator)pBlkRef.attributeIterator();
      for (int i = 0; !pIter.done(); i++, pIter.step())
      {
        OdDbAttribute pAttr = pIter.entity() as OdDbAttribute;
        if (pAttr != null)
        {
          dumpAttributeData(pAttr, i);
        }
      }
    }
    /************************************************************************/
    /* Dump data common to all OdDbCurves                                   */
    /************************************************************************/
    static void dumpCurveData(OdDbEntity pEnt)
    {
      OdDbCurve pEntity = (OdDbCurve)pEnt;
      OdGePoint3d startPoint = new OdGePoint3d();
      if (OdResult.eOk == pEntity.getStartPoint(startPoint))
      {
        Console.WriteLine("Start Point = {0}", startPoint.ToString());
      }

      OdGePoint3d endPoint = new OdGePoint3d();
      if (OdResult.eOk == pEntity.getEndPoint(endPoint))
      {
        Console.WriteLine("End Point  = {0}", endPoint.ToString());
      }
      Console.WriteLine("Closed = {0}", pEntity.isClosed());
      Console.WriteLine("Periodic = {0}", pEntity.isPeriodic());

      double area;
      if (OdResult.eOk == pEntity.getArea(out area))
      {
        Console.WriteLine("Area = {0}", area);
      }
      dumpEntityData(pEntity);
    }
    static void dumpDimData(OdDbDimension pDim)
    {
      Console.WriteLine("Measurement = {0}", pDim.getMeasurement());
      Console.WriteLine("Dimension Text = {0}", pDim.dimensionText());

      if (pDim.getMeasurement() >= 0.0)
      {
        string formattedMeasurement = "";
        pDim.formatMeasurement(ref formattedMeasurement,
            pDim.getMeasurement(), pDim.dimensionText());
        Console.WriteLine("Formatted Measurement = {0}", formattedMeasurement);
      }
      OdDbBlockTableRecord pBlock = OdDbBlockTableRecord.cast(pDim.dimBlockId().openObject());
      if (pBlock == null)
      {
        Console.WriteLine("Dimension Block = NULL");
      }
      else
      {
        Console.WriteLine("Dimension Block Name = {0}", pBlock.getName());
      }
      Console.WriteLine("Position = {0}", pDim.dimBlockPosition());
      Console.WriteLine("Rotation = {0}", pDim.dimBlockRotation());
      Console.WriteLine("Scale = {0}", pDim.dimBlockScale());
      Console.WriteLine("Text Position = {0}", pDim.textPosition());
      Console.WriteLine("Text Rotation = {0}", toDegreeString(pDim.textRotation()));
      Console.WriteLine("Dimension Style = {0}", pDim.dimensionStyle());
      OdCmColor bgrndTxtColor = new OdCmColor();
      UInt16 bgrndTxtFlags = pDim.getBgrndTxtColor(bgrndTxtColor);
      Console.WriteLine("Background Text Color = {0}", bgrndTxtColor.ToString());
      Console.WriteLine("BackgroundText Flags = {0}", bgrndTxtFlags.ToString());
      Console.WriteLine("Extension Line 1 Linetype = {0}", pDim.getDimExt1Linetype().ToString());
      Console.WriteLine("Extension Line 2 Linetype = {0}", pDim.getDimExt2Linetype().ToString());
      Console.WriteLine("Dim Line Linetype = {0}", pDim.getDimExt2Linetype().ToString());
      Console.WriteLine("Horizontal Rotation = {0}", toDegreeString(pDim.horizontalRotation()));
      Console.WriteLine("Elevation = {0}", pDim.elevation().ToString());
      Console.WriteLine("Normal = {0}", pDim.normal().ToString());
      dumpEntityData(pDim);
    }
    class LineAngularDimensionDumper : OdDbPropertiesOverrule
    {
      public override void list(OdDbEntity pSubject)
      {
        OdDb2LineAngularDimension pDim = (OdDb2LineAngularDimension)pSubject;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Arc Point = {0}", pDim.arcPoint().ToString());
        Console.WriteLine("Extension Line 1 Start = {0}", pDim.xLine1Start().ToString());
        Console.WriteLine("Extension Line 1 End = {0}", pDim.xLine1End().ToString());
        Console.WriteLine("Extension Line 2 Start = {0}", pDim.xLine2Start().ToString());
        Console.WriteLine("Extension Line 2 End = {0}", pDim.xLine2End().ToString());
        dumpDimData(pDim);
      }
    }
    static OdDbPropertiesOverrule m_2LineAngularDimensionDumper = new LineAngularDimensionDumper();
    /************************************************************************/
    /* Dump 2D Vertex data                                                  */
    /************************************************************************/
    static void dump2dVertex(OdDb2dVertex pVertex, int i)
    {
      Console.WriteLine("<{0}> {1}", pVertex.isA().name(), i);
      Console.WriteLine("Handle = {0}", pVertex.getDbHandle());
      Console.WriteLine("Vertex Type = {0}", pVertex.vertexType());
      Console.WriteLine("Position = {0}", pVertex.position());
      Console.WriteLine("Start Width = {0}", pVertex.startWidth());
      Console.WriteLine("End Width = {0}", pVertex.endWidth());
      Console.WriteLine("Bulge = {0}", pVertex.bulge());
      if (pVertex.bulge() != 0)
        Console.WriteLine("Bulge Angle = {0}", toDegreeString(4 * Math.Atan(pVertex.bulge())));
      Console.WriteLine("Tangent Used = {0}", pVertex.isTangentUsed());
      if (pVertex.isTangentUsed())
        Console.WriteLine("Tangent = {0}", pVertex.tangent());
    }

    /************************************************************************/
    /* 2D Polyline Dumper                                                   */
    /************************************************************************/
    class OdDb2dPolyline_Dumper : OdDbPropertiesOverrule
    {
        // as it is stated:
        /** \details
           Determines whether the overrule is applicable and returns True if the overrule can be applied,
           or False, otherwise. It is a pure virtual method that must be implemented in the derived class.

           \param pOverruledSubject [in]  A raw pointer to an object to be overruled.
          */
        // thus isApplcable method is to be obkigatiry implemented if we plan to use list method of OdDb2dPolyline_Dumper class, 
        // otherwise we'll face an exception "Pure virtual method call"
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        OdDb2dPolyline pPolyline = (OdDb2dPolyline)pEnt;
        Console.WriteLine("<{0}> {1}", pPolyline.isA().name(), pPolyline.getDbHandle().ToString());
        Console.WriteLine("Elevation = {0}", pPolyline.elevation().ToString());
        Console.WriteLine("Normal = {0}", pPolyline.normal().ToString());
        Console.WriteLine("Thickness = {0}", pPolyline.thickness());
        /********************************************************************/
        /* Dump the vertices                                                */
        /********************************************************************/
        OdDbObjectIterator pIter = pPolyline.vertexIterator();
        for (int i = 0; !pIter.done(); i++, pIter.step())
        {
          OdDb2dVertex pVertex = pIter.entity() as OdDb2dVertex;
          if (pVertex != null)
          {
            dump2dVertex(pVertex, i);
          }
        }
        dumpCurveData(pPolyline);
      }
    };
    /************************************************************************/
    /* Dump 3D Polyline Vertex data                                         */
    /************************************************************************/
    static void dump3dPolylineVertex(OdDb3dPolylineVertex pVertex, int i)
    {
      Console.WriteLine("<{0}> {1}", pVertex.isA().name(), i.ToString());
      Console.WriteLine("Handle = {0}", pVertex.getDbHandle().ToString());
      Console.WriteLine("Type = {0}", pVertex.vertexType().ToString());
      Console.WriteLine("Position = {0}", pVertex.position().ToString());
    }

    /************************************************************************/
    /* 3D Polyline Dumper                                                   */
    /************************************************************************/
    class OdDb3dPolyline_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDb3dPolyline pPolyline = (OdDb3dPolyline)pEnt;
        Console.WriteLine("<{0}> {1}", pPolyline.isA().name(), pPolyline.getDbHandle().ToString());
        /********************************************************************/
        /* Dump the vertices                                                */
        /********************************************************************/
        OdDbObjectIterator pIter = pPolyline.vertexIterator();
        for (int i = 0; !pIter.done(); i++, pIter.step())
        {
          OdDb3dPolylineVertex pVertex = pIter.entity() as OdDb3dPolylineVertex;
          if (pVertex != null)
          {
            dump3dPolylineVertex(pVertex, i);
          }
        }
        dumpCurveData(pPolyline);
      }
    }

    /************************************************************************/
    /* 3DSolid Dumper                                                       */
    /************************************************************************/
    class OdDb3dSolid_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDb3dSolid pSolid = (OdDb3dSolid)pEnt;
        Console.WriteLine("<{0}> {1}", pSolid.isA().name(), pSolid.getDbHandle().ToString());
        dumpEntityData(pSolid);
      }
    }

    /************************************************************************/
    /* 3 Point Angular Dimension Dumper                                     */
    /************************************************************************/
    class OdDb3PointAngularDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDb3PointAngularDimension pDim = (OdDb3PointAngularDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Arc Point = {0}", pDim.arcPoint().ToString());
        Console.WriteLine("Center Point = {0}", pDim.centerPoint().ToString());
        Console.WriteLine("Extension Line 1 Point = {0}", pDim.xLine1Point().ToString());
        Console.WriteLine("Extension Line 2 Point = {0}", pDim.xLine2Point().ToString());
        dumpDimData(pDim);
      }
    }

    /************************************************************************/
    /* Aligned Dimension Dumper                                             */
    /************************************************************************/
    class OdDbAlignedDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbAlignedDimension pDim = (OdDbAlignedDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Dimension line Point = {0}", pDim.dimLinePoint().ToString());
        Console.WriteLine("Oblique = {0}", toDegreeString(pDim.oblique()));
        Console.WriteLine("Extension Line 1 Point = {0}", pDim.xLine1Point().ToString());
        Console.WriteLine("Extension Line 2 Point = {0}", pDim.xLine2Point().ToString());
        dumpDimData(pDim);
      }
    };
    /************************************************************************/
    /* Arc Dumper                                                           */
    /************************************************************************/
    class OdDbArc_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbArc pArc = (OdDbArc)pEnt;
        Console.WriteLine("<{0}> {1}", pArc.isA().name(), pArc.getDbHandle().ToString());
        Console.WriteLine("Center = {0}", pArc.center().ToString());
        Console.WriteLine("Radius = {0}", pArc.radius().ToString());
        Console.WriteLine("Start Angle = {0}", toDegreeString(pArc.startAngle()));
        Console.WriteLine("End Angle = {0}", toDegreeString(pArc.endAngle()));
        Console.WriteLine("Normal = {0}", pArc.normal().ToString());
        Console.WriteLine("Thickness = {0}", pArc.thickness().ToString());
        dumpCurveData(pArc);
      }
    };

    /************************************************************************/
    /* Arc Aligned Text                                                           */
    /************************************************************************/
    class OdDbArcAlignedText_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbArcAlignedText pText = (OdDbArcAlignedText)pEnt;
        Console.WriteLine("<{0}> {1}", pText.isA().name(), pText.getDbHandle().ToString());
        Console.WriteLine("Center = {0}", pText.center().ToString());
        Console.WriteLine("Radius = {0}", pText.radius().ToString());
        Console.WriteLine("Start Angle = {0}", toDegreeString(pText.startAngle()));
        Console.WriteLine("End Angle = {0}", toDegreeString(pText.endAngle()));
        Console.WriteLine("Normal = {0}", pText.normal().ToString());
        if (!pText.arcId().isNull())
        {
          Console.WriteLine("Associated Arc = {0}", pText.arcId().getHandle().ToString());
        }
        else
        {
          Console.WriteLine("Associated Arc is Null");
        }
        Console.WriteLine("Text String = {0}", pText.textString().ToString());
        Console.WriteLine("Alignment = {0}", pText.alignment().ToString());
        Console.WriteLine("Character Spacing = {0}", pText.charSpacing().ToString());
        Console.WriteLine("Reversed Char Order = {0}", pText.isReversedCharOrder().ToString());
        Console.WriteLine("Offset from Arc = {0}", pText.offsetFromArc().ToString());
        Console.WriteLine("Left Offset = {0}", pText.leftOffset().ToString());
        Console.WriteLine("Right Offset = {0}", pText.rightOffset().ToString());
        Console.WriteLine("Text Position = {0}", pText.textPosition().ToString());
        Console.WriteLine("Text Direction = {0}", pText.textDirection().ToString());
        Console.WriteLine("Text Style = {0}", Program.toString(pText.textStyle()));
        Console.WriteLine("Text Height = {0}", pText.textSize().ToString());
        Console.WriteLine("Width Factor = {0}", pText.xScale().ToString());
        Console.WriteLine("SHX font = {0}", pText.isShxFont().ToString());
        Console.WriteLine("Filename = {0}", shortenPath(pText.fileName()));
        Console.WriteLine("BigFont Filename = {0}", shortenPath(pText.bigFontFileName()));

        String typeface = "";
        bool bold;
        bool italic;
        int charset;
        int pitchAndFamily;
        pText.font(ref typeface, out bold, out italic, out charset, out pitchAndFamily);
        Console.WriteLine("Typeface = {0}", typeface);
        Console.WriteLine("Character Set = {0}", charset);
        Console.WriteLine("Bold = {0}", bold);
        Console.WriteLine("Italic = {0}", italic);
        Console.WriteLine("Font Pitch and Family {0:X}", pitchAndFamily);
        dumpEntityData(pText);
      }
    }
    /************************************************************************/
    /* Arc Dimension Dumper                                                 */
    /************************************************************************/
    class OdDbArcDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbArcDimension pDim = (OdDbArcDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Arc Point", pDim.arcPoint().ToString());
        Console.WriteLine("Center Point", pDim.centerPoint().ToString());
        Console.WriteLine("Arc symbol", toArcSymbolTypeString(pDim.arcSymbolType()));
        Console.WriteLine("Partial", pDim.isPartial().ToString());
        Console.WriteLine("Has leader", pDim.hasLeader().ToString());

        if (pDim.hasLeader())
        {
          Console.WriteLine("Leader 1 Point", pDim.leader1Point().ToString());
          Console.WriteLine("Leader 2 Point", pDim.leader2Point().ToString());
        }
        Console.WriteLine("Extension Line 1 Point", pDim.xLine1Point().ToString());
        Console.WriteLine("Extension Line 2 Point", pDim.xLine2Point().ToString());
        dumpDimData(pDim);
      }
    };

    /************************************************************************/
    /* Block Reference Dumper                                                */
    /************************************************************************/
    class OdDbBlockReference_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbBlockReference pBlkRef = (OdDbBlockReference)pEnt;
        Console.WriteLine("<{0}> {1}", pBlkRef.isA().name(), pBlkRef.getDbHandle().ToString());

        OdDbBlockTableRecord pRecord = pBlkRef.blockTableRecord().openObject() as OdDbBlockTableRecord;
        if (pRecord != null)
        {
          Console.WriteLine("Name = {0}", pRecord.getName());
        }

        dumpBlockRefData(pBlkRef);

        OdDbSpatialFilter pFilt = TD_Db.getFilter(pBlkRef, OdDbSpatialFilter.desc(), OpenMode.kForRead) as OdDbSpatialFilter;

        /********************************************************************/
        /* Dump the Spatial Filter  (Xref Clip)                             */
        /********************************************************************/
        if (pFilt != null)
        {
          Console.WriteLine("<{0}> {1}", pFilt.isA().name(), pFilt.getDbHandle().ToString());
          OdGePoint2dArray points = new OdGePoint2dArray();
          OdGeVector3d normal = new OdGeVector3d();
          double elevation, frontClip, backClip;
          bool enabled;
          pFilt.getDefinition(points, normal, out elevation, out frontClip, out backClip, out enabled);
          Console.WriteLine("Normal = {0}", normal.ToString());
          Console.WriteLine("Elevation = {0}", elevation);
          Console.WriteLine("Front Clip Distance = {0}", frontClip);
          Console.WriteLine("Back Clip Distance = {0}", backClip);
          Console.WriteLine("Enabled = {0}", enabled);
          for (int i = 0; i < points.Count; i++)
          {
            Console.WriteLine("Clip point {0} {1}", i, points[i].ToString());
          }
        }
      }
    }
    /************************************************************************/
    /* Body Dumper                                                        */
    /************************************************************************/
    class OdDbBody_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbBody pBody = (OdDbBody)pEnt;
        Console.WriteLine("<{0}> {1}", pBody.isA().name(), pBody.getDbHandle().ToString());
        dumpEntityData(pBody);
      }
    };

    /************************************************************************/
    /* Circle Dumper                                                        */
    /************************************************************************/
    class OdDbCircle_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbCircle pCircle = (OdDbCircle)pEnt;
        Console.WriteLine("<{0}> {1}", pCircle.isA().name(), pCircle.getDbHandle().ToString());
        Console.WriteLine("Center = {0}", pCircle.center().ToString());
        Console.WriteLine("Radius = {0}", pCircle.radius());
        Console.WriteLine("Diameter = {0}", 2 * pCircle.radius());
        Console.WriteLine("Normal = {0}", pCircle.normal().ToString());
        Console.WriteLine("Thickness = {0}", pCircle.thickness());
        dumpCurveData(pCircle);
      }
    };

    /************************************************************************/
    /* Diametric Dimension Dumper                                           */
    /************************************************************************/
    class OdDbDiametricDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbDiametricDimension pDim = (OdDbDiametricDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Chord Point = {0}", pDim.chordPoint().ToString());
        Console.WriteLine("Far chord Point = {0}", pDim.farChordPoint().ToString());
        Console.WriteLine("Leader Length = {0}", pDim.leaderLength().ToString());
        dumpDimData(pDim);
      }
    };

    /************************************************************************/
    /* Ellipse Dumper                                                       */
    /************************************************************************/
    class OdDbEllipse_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbEllipse pEllipse = (OdDbEllipse)pEnt;
        Console.WriteLine("<{0}> {1}", pEllipse.isA().name(), pEllipse.getDbHandle().ToString());
        Console.WriteLine("Center = {0}", pEllipse.center().ToString());
        Console.WriteLine("Major Axis = {0}", pEllipse.majorAxis().ToString());
        Console.WriteLine("Minor Axis = {0}", pEllipse.minorAxis().ToString());
        Console.WriteLine("Major Radius = {0}", pEllipse.majorAxis().length().ToString());
        Console.WriteLine("Minor Radius = {0}", pEllipse.minorAxis().length().ToString());
        Console.WriteLine("Radius Ratio = {0}", pEllipse.radiusRatio().ToString());
        Console.WriteLine("Start Angle = {0}", toDegreeString(pEllipse.startAngle()));
        Console.WriteLine("End Angle = {0}", toDegreeString(pEllipse.endAngle()));
        Console.WriteLine("Normal = {0}", pEllipse.normal().ToString());
        double startParam;
        pEllipse.getStartParam(out startParam);
        Console.WriteLine("Elevation = {0}", startParam);
        double endParam;
        pEllipse.getEndParam(out endParam);
        Console.WriteLine("Elevation = {0}", endParam);
        dumpCurveData(pEllipse);
      }
    }
    /************************************************************************/
    /* Face Dumper                                                       */
    /************************************************************************/
    class OdDbFace_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbFace pFace = (OdDbFace)pEnt;
        Console.WriteLine("<{0}> {1}", pFace.isA().name(), pFace.getDbHandle().ToString());
        for (UInt16 i = 0; i < 4; i++)
        {
          OdGePoint3d pt = new OdGePoint3d();
          pFace.getVertexAt(i, pt);
          Console.WriteLine("Vertex {0} = {1}", i, pt.ToString());
        }
        for (UInt16 i = 0; i < 4; i++)
        {
          Console.WriteLine("Edge {0} visible = {1}", i, pFace.isEdgeVisibleAt(i));
        }
        dumpEntityData(pFace);
      }
    }

    /************************************************************************/
    /* FCF Dumper                                                           */
    /************************************************************************/
    class OdDbFcf_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbFcf pFcf = (OdDbFcf)pEnt;
        Console.WriteLine("<{0}> {1}", pFcf.isA().name(), pFcf.getDbHandle().ToString());
        Console.WriteLine("Location", pFcf.location().ToString());
        Console.WriteLine("Text", pFcf.text().ToString());
        Console.WriteLine("Dimension Style", pFcf.dimensionStyle().ToString());
        Console.WriteLine("Dimension Gap", pFcf.dimgap().ToString());
        Console.WriteLine("Dimension Scale", pFcf.dimscale().ToString());
        Console.WriteLine("Text Height", pFcf.dimtxt().ToString());
        Console.WriteLine("Frame Color", pFcf.dimclrd().ToString());
        Console.WriteLine("Text Style", pFcf.dimtxsty().ToString());
        Console.WriteLine("Text Color", pFcf.dimclrd().ToString());
        Console.WriteLine("X-Direction", pFcf.direction().ToString());
        Console.WriteLine("Normal", pFcf.normal().ToString());
        dumpEntityData(pFcf);
      }
    }

    /************************************************************************/
    /* Hatch Dumper                                                         */
    /************************************************************************/
    class OdDbHatch_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      /***********************************************************************/
      /* Dump Polyline Loop                                                  */
      /***********************************************************************/
      static void dumpPolylineType(int loopIndex, OdDbHatch pHatch)
      {
        OdGePoint2dArray vertices = new OdGePoint2dArray();
        OdGeDoubleArray bulges = new OdGeDoubleArray();
        pHatch.getLoopAt(loopIndex, vertices, bulges);
        bool hasBulges = (bulges.Count > 0);
        for (int i = 0; i < vertices.Count; i++)
        {
          Console.WriteLine("Vertex {0} =  {1}", i, vertices[i].ToString());
          if (hasBulges)
          {
            Console.WriteLine("Bulge {0} = {1}", i, bulges[i]);
            Console.WriteLine("Bulge angle {0} = {1}", i, toDegreeString(4.0 * Math.Atan(bulges[i])));
          }
        }
      }

      /**********************************************************************/
      /* Dump Circular Arc Edge                                             */
      /**********************************************************************/
      static void dumpCircularArcEdge(OdGeCurve2d pEdge)
      {
        //OdGeCircArc2d pCircArc = (OdGeCircArc2d)pEdge;
        OdGeCircArc2d pCircArc = new OdGeCircArc2d(OdGeCurve2d.getCPtr(pEdge).Handle, false);
        Console.WriteLine("Center = {0}", pCircArc.center().ToString());
        Console.WriteLine("Radius = {0}", pCircArc.radius().ToString());
        Console.WriteLine("Start Angle = {0}", toDegreeString(pCircArc.startAng()));
        Console.WriteLine("End Angle = {0}", toDegreeString(pCircArc.endAng()));
        Console.WriteLine("Clockwise = {0}", pCircArc.isClockWise().ToString());
      }

      /**********************************************************************/
      /* Dump Elliptical Arc Edge                                           */
      /**********************************************************************/
      static void dumpEllipticalArcEdge(OdGeCurve2d pEdge)
      {
        //OdGeEllipArc2d pEllipArc = (OdGeEllipArc2d)pEdge;
        OdGeEllipArc2d pEllipArc = new OdGeEllipArc2d(OdGeCurve2d.getCPtr(pEdge).Handle, false);
        Console.WriteLine("Center = {0}", pEllipArc.center().ToString());
        Console.WriteLine("Major Radius = {0}", pEllipArc.majorRadius().ToString());
        Console.WriteLine("Minor Radius = {0}", pEllipArc.minorRadius().ToString());
        Console.WriteLine("Major Axis = {0}", pEllipArc.majorAxis().ToString());
        Console.WriteLine("Minor Axis = {0}", pEllipArc.minorAxis().ToString());
        Console.WriteLine("Start Angle = {0}", toDegreeString(pEllipArc.startAng()));
        Console.WriteLine("End Angle = {0}", toDegreeString(pEllipArc.endAng()));
        Console.WriteLine("Clockwise = {0}", pEllipArc.isClockWise().ToString());
      }
      /**********************************************************************/
      /* Dump NurbCurve Edge                                           */
      /**********************************************************************/
      static void dumpNurbCurveEdge(OdGeCurve2d pEdge)
      {

        OdGeNurbCurve2d pNurbCurve = (OdGeNurbCurve2d)pEdge;
        int degree;
        bool rational, periodic;
        OdGePoint2dArray ctrlPts = new OdGePoint2dArray();
        OdGeDoubleArray weights = new OdGeDoubleArray();
        OdGeKnotVector knots = new OdGeKnotVector();

        pNurbCurve.getDefinitionData(out degree, out rational, out periodic, knots, ctrlPts, weights);
        Console.WriteLine("Degree = {0}", degree);
        Console.WriteLine("Rational = {0}", rational);
        Console.WriteLine("Periodic = {0}", periodic);

        Console.WriteLine("Number of Control Points = {0}", ctrlPts.Count);
        int i;
        for (i = 0; i < ctrlPts.Count; i++)
        {
          Console.WriteLine("Control Point {0} = {1}", i, ctrlPts[i].ToString());
        }
        Console.WriteLine("Number of Knots = {0}", knots.length().ToString());
        for (i = 0; i < knots.length(); i++)
        {
          Console.WriteLine("Knot {0} = {1}", i, knots[i]);
        }

        if (rational)
        {
          Console.WriteLine("Number of Weights = {0}", weights.Count);
          for (i = 0; i < weights.Count; i++)
          {
            Console.WriteLine("Weight {0} = {1}", i, weights[i]);
          }
        }
      }

      /***********************************************************************/
      /* Dump Edge Loop                                                      */
      /***********************************************************************/
      static void dumpEdgesType(int loopIndex, OdDbHatch pHatch)
      {

        EdgeArray edges = new EdgeArray();
        pHatch.getLoopAt(loopIndex, edges);
        Console.WriteLine("Edges");
        for (int i = 0; i < edges.Count; i++)
        {
          OdGeCurve2d pEdge = edges[i];
          Console.WriteLine("Edge {0} = {1}", i, pEdge.type());
          switch ((OdGe.EntityId)pEdge.type())
          {
            case OdGe.EntityId.kLineSeg2d: break;
            case OdGe.EntityId.kCircArc2d: dumpCircularArcEdge(pEdge); break;
            case OdGe.EntityId.kEllipArc2d: dumpEllipticalArcEdge(pEdge); break;
            case OdGe.EntityId.kNurbCurve2d: dumpNurbCurveEdge(pEdge); break;
            default: break;
          }

          /******************************************************************/
          /* Common Edge Properties                                         */
          /******************************************************************/
          OdGeInterval interval = new OdGeInterval();
          pEdge.getInterval(interval);
          double lower;
          double upper;
          interval.getBounds(out lower, out upper);
          Console.WriteLine("Start Point = {0}", pEdge.evalPoint(lower));
          Console.WriteLine("End Point = {0}", pEdge.evalPoint(upper));
          Console.WriteLine("Closed = {0}", pEdge.isClosed());
        }
      }

      public override void list(OdDbEntity pEnt)
      {
        OdDbHatch pHatch = (OdDbHatch)pEnt;
        Console.WriteLine("<{0}> {1}", pHatch.isA().name(), pHatch.getDbHandle().ToString());
        Console.WriteLine("Hatch Style = {0}", pHatch.hatchStyle().ToString());
        Console.WriteLine("Hatch Object Type = {0}", pHatch.hatchObjectType());
        Console.WriteLine("Is Hatch = {0}", pHatch.isHatch());
        Console.WriteLine("Is Gradient = {0}", !pHatch.isGradient());
        if (pHatch.isHatch())
        {
          /******************************************************************/
          /* Dump Hatch Parameters                                          */
          /******************************************************************/
          Console.WriteLine("Pattern Type = {0}", pHatch.patternType().ToString());
          switch (pHatch.patternType())
          {
            case OdDbHatch.HatchPatternType.kPreDefined:
            case OdDbHatch.HatchPatternType.kCustomDefined:
              Console.WriteLine("Pattern Name = {0}", pHatch.patternName().ToString());
              Console.WriteLine("Solid Fill = {0}", pHatch.isSolidFill().ToString());
              if (!pHatch.isSolidFill())
              {
                Console.WriteLine("Pattern Angle = {0}", toDegreeString(pHatch.patternAngle()));
                Console.WriteLine("Pattern Scale = {0}", pHatch.patternScale().ToString());
              }
              break;
            case OdDbHatch.HatchPatternType.kUserDefined:
              Console.WriteLine("Pattern Angle = {0}", toDegreeString(pHatch.patternAngle()));
              Console.WriteLine("Pattern Double = {0}", pHatch.patternDouble().ToString());
              Console.WriteLine("Pattern Space = {0}", pHatch.patternSpace().ToString());
              break;
          }
        }
        if (pHatch.isGradient())
        {
          /******************************************************************/
          /* Dump Gradient Parameters                                       */
          /******************************************************************/
          Console.WriteLine("Gradient Type = {0}", pHatch.gradientType().ToString());
          Console.WriteLine("Gradient Name = {0}", pHatch.gradientName().ToString());
          Console.WriteLine("Gradient Angle = {0}", toDegreeString(pHatch.gradientAngle()));
          Console.WriteLine("Gradient Shift = {0}", pHatch.gradientShift().ToString());
          Console.WriteLine("Gradient One-Color Mode = {0}", pHatch.getGradientOneColorMode().ToString());
          if (pHatch.getGradientOneColorMode())
          {
            Console.WriteLine("ShadeTintValue = {0}", pHatch.getShadeTintValue().ToString());
          }
          OdCmColorArray colors = new OdCmColorArray();
          OdGeDoubleArray values = new OdGeDoubleArray();
          pHatch.getGradientColors(colors, values);
          for (int i = 0; i < colors.Count; i++)
          {
            Console.WriteLine("Color         {0} = {1}", i, colors[i].ToString());
            Console.WriteLine("Interpolation {0} = {1}", i, values[i].ToString());
          }
        }

        /********************************************************************/
        /* Dump Associated Objects                                          */
        /********************************************************************/
        Console.WriteLine("Associated objects = {0}", pHatch.associative().ToString());
        OdDbObjectIdArray assocIds = new OdDbObjectIdArray();
        pHatch.getAssocObjIds(assocIds);
        for (int i = 0; i < assocIds.Count; i++)
        {
          OdDbEntity pAssoc = assocIds[i].openObject() as OdDbEntity;
          if (pAssoc != null)
            Console.WriteLine("<{0}> {1}", pAssoc.isA().name(), pAssoc.getDbHandle().ToString());
        }

        /********************************************************************/
        /* Dump Seed Points                                                 */
        /********************************************************************/
        Console.WriteLine("Seed points = {0}", pHatch.numSeedPoints().ToString());
        for (uint i = 0; i < pHatch.numSeedPoints(); i++)
        {
          Console.WriteLine("Seed point {0} = {1}", i, pHatch.getSeedPointAt(i));
        }

        /********************************************************************/
        /* Dump Loops                                                       */
        /********************************************************************/
        Console.WriteLine("Loops = {0}", pHatch.numLoops());
        for (int i = 0; i < pHatch.numLoops(); i++)
        {
          Console.WriteLine("Loop {0} = {1}", i, toLooptypeString((OdDbHatch.HatchLoopType)pHatch.loopTypeAt(i)));

          /******************************************************************/
          /* Dump Loop                                                      */
          /******************************************************************/
          if ((pHatch.loopTypeAt(i) & (int)OdDbHatch.HatchLoopType.kPolyline) != 0)
          {
            dumpPolylineType(i, pHatch);
          }
          else
          {
            dumpEdgesType(i, pHatch);
          }
          /******************************************************************/
          /* Dump Associated Objects                                        */
          /******************************************************************/
          if (pHatch.associative())
          {
            Console.WriteLine("Associated objects");
            assocIds.Clear();
            pHatch.getAssocObjIdsAt(i, assocIds);
            for (int j = 0; j < assocIds.Count; j++)
            {
              OdDbEntity pAssoc = assocIds[j].openObject() as OdDbEntity;
              if (pAssoc != null)
                Console.WriteLine("<{0}> {1}", pAssoc.isA().name(), pAssoc.getDbHandle().ToString());
            }
          }
        }

        Console.WriteLine("Elevation = {0}", pHatch.elevation().ToString());
        Console.WriteLine("Normal = {0}", pHatch.normal().ToString());
        dumpEntityData(pHatch);
      }
    }

    /************************************************************************/
    /* Leader Dumper                                                          */
    /************************************************************************/
    class OdDbLeader_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbLeader pLeader = (OdDbLeader)pEnt;
        Console.WriteLine("<{0}> {1}", pLeader.isA().name(), pLeader.getDbHandle().ToString());
        Console.WriteLine("Dimension Style = {0}", pLeader.dimensionStyle().ToString());

        OdDbObject pAnno = pLeader.annotationObjId().openObject();
        Console.WriteLine("Annotation = {0}");
        if (pAnno != null)
        {
          Console.WriteLine("<{0}> {1}", pAnno.isA().name(), pAnno.getDbHandle().ToString());
        }
        Console.WriteLine("Type = {0}", pLeader.annoType().ToString());
        Console.WriteLine("Height = {0}", pLeader.annoHeight().ToString());
        Console.WriteLine("Width = {0}", pLeader.annoWidth().ToString());
        Console.WriteLine("Offset = {0}", pLeader.annotationOffset().ToString());
        Console.WriteLine("X-Direction = {0}", pLeader.annotationXDir().ToString());
        Console.WriteLine("Has Arrowhead = {0}", pLeader.hasArrowHead().ToString());
        Console.WriteLine("Has Hook Line = {0}", pLeader.hasHookLine().ToString());

        if (pLeader.hasHookLine())
        {
          Console.WriteLine("Hook Line on X dir = {0}", pLeader.isHookLineOnXDir().ToString());
        }
        Console.WriteLine("Splined = {0}", pLeader.isSplined().ToString());

        for (int i = 0; i < pLeader.numVertices(); i++)
        {
          Console.WriteLine("Vertex {0} = {1}", i, pLeader.vertexAt(i));
        }
        Console.WriteLine("Normal = {0}", pLeader.normal().ToString());
        dumpCurveData(pLeader);
      }
    }

    /************************************************************************/
    /* Line Dumper                                                          */
    /************************************************************************/
    class OdDbLine_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        OdDbLine pLine = (OdDbLine)pEnt;
        Console.WriteLine("<{0}> {1}", pLine.isA().name(), pLine.getDbHandle().ToString());
        Console.WriteLine("Normal = {0}", pLine.normal().ToString());
        Console.WriteLine("Thickness = {0}", pLine.thickness().ToString());
        dumpEntityData(pLine);
      }
    };
    /************************************************************************/
    /* MInsertBlock Dumper                                                  */
    /************************************************************************/
    class OdDbMInsertBlock_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbMInsertBlock pMInsert = (OdDbMInsertBlock)pEnt;
        Console.WriteLine("<{0}> {1}", pMInsert.isA().name(), pMInsert.getDbHandle().ToString());

        OdDbBlockTableRecord pRecord = (OdDbBlockTableRecord)pMInsert.blockTableRecord().safeOpenObject();
        Console.WriteLine("Name = {0}", pRecord.getName().ToString());
        Console.WriteLine("Rows = {0}", pMInsert.rows().ToString());
        Console.WriteLine("Columns = {0}", pMInsert.columns().ToString());
        Console.WriteLine("Row Spacing = {0}", pMInsert.rowSpacing().ToString());
        Console.WriteLine("Column Spacing = {0}", pMInsert.columnSpacing().ToString());
        dumpBlockRefData(pMInsert);
      }
    };

    /************************************************************************/
    /* Mline Dumper                                                         */
    /************************************************************************/
    class OdDbMline_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbMline pMline = (OdDbMline)pEnt;
        Console.WriteLine("<{0}> {1}", pMline.isA().name(), pMline.getDbHandle().ToString());
        Console.WriteLine("Style = {0}", pMline.style().ToString());
        Console.WriteLine("Closed = {0}", pMline.closedMline().ToString());
        Console.WriteLine("Scale = {0}", pMline.scale().ToString());
        Console.WriteLine("Suppress Start Caps = {0}", pMline.supressStartCaps().ToString());
        Console.WriteLine("Suppress End Caps = {0}", pMline.supressEndCaps().ToString());
        Console.WriteLine("Normal = {0}", pMline.normal().ToString());

        /********************************************************************/
        /* Dump the segment data                                            */
        /********************************************************************/
        for (int i = 0; i < pMline.numVertices(); i++)
        {
          OdMLSegmentArray segments = new OdMLSegmentArray();
          pMline.getParametersAt(i, segments);
          Console.WriteLine("Segment {0}", i);
          Console.WriteLine("Vertex = {0}", pMline.vertexAt(i));
          for (int j = 0; j < segments.Count; j++)
          {
            String rightString = "";
            String leftString = "";
            for (int k = 0; k < segments[j].m_SegParams.Count; k++)
            {
              if (k != 0)
              {
                rightString += ", ";
              }
              rightString += segments[j].m_SegParams[k].ToString();
            }
            if (rightString != "")
            {
              leftString = "SegParams " + j.ToString();
              Console.WriteLine(leftString + rightString);
            }
            rightString = "";
            for (int k = 0; k < segments[j].m_AreaFillParams.Count; k++)
            {
              if (k != 0)
              {
                rightString += ", ";
              }
              rightString += segments[j].m_AreaFillParams[k].ToString();
            }
            if (rightString != "")
            {
              leftString = "FillParams " + j.ToString();
              Console.WriteLine(leftString + rightString);
            }
          }
        }
        dumpEntityData(pMline);
      }
    };
    /************************************************************************/
    /* MText Dumper                                                         */
    /************************************************************************/
    class OdDbMText_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        OdDbMText pMText = (OdDbMText)pEnt;
        Console.WriteLine("<{0}> {1}", pMText.isA().name(), pMText.getDbHandle().ToString());

        Console.WriteLine("Contents = {0}", pMText.contents().ToString());
        Console.WriteLine("Location = {0}", pMText.location().ToString());
        Console.WriteLine("Height = {0}", pMText.textHeight().ToString());
        Console.WriteLine("Rotation = {0}", toDegreeString(pMText.rotation()));
        Console.WriteLine("Text Style = {0}", pMText.textStyle().ToString());
        Console.WriteLine("Attachment = {0}", pMText.attachment().ToString());
        Console.WriteLine("Background Fill On = {0}", pMText.backgroundFillOn().ToString());
        Console.WriteLine("Background Fill Color = {0}", pMText.getBackgroundFillColor().ToString());
        Console.WriteLine("Background Scale Factor = {0}", pMText.getBackgroundScaleFactor().ToString());
        Console.WriteLine("Background Transparency Method = {0}", pMText.getBackgroundTransparency().method().ToString());
        Console.WriteLine("X-Direction = {0}", pMText.direction().ToString());
        Console.WriteLine("Flow Direction = {0}", pMText.flowDirection().ToString());
        Console.WriteLine("Horizontal Mode = {0}", pMText.horizontalMode().ToString());
        Console.WriteLine("Vertical Mode = {0}", pMText.verticalMode().ToString());
        Console.WriteLine("Width = {0}", pMText.width().ToString());
        Console.WriteLine("Actual Height = {0}", pMText.actualHeight().ToString());
        Console.WriteLine("Actual Width = {0}", pMText.actualWidth().ToString());

        OdGePoint3dArray points = new OdGePoint3dArray();
        pMText.getBoundingPoints(points);
        Console.WriteLine("TL Bounding Point = {0}", points[0]);
        Console.WriteLine("TR Bounding Point = {0}", points[1]);
        Console.WriteLine("BL Bounding Point = {0}", points[2]);
        Console.WriteLine("BR Bounding Point = {0}", points[3]);
        Console.WriteLine("Normal = {0}", pMText.normal().ToString());

        /********************************************************************/
        /* Dump Indents and Tabs                                            */
        /********************************************************************/
        OdDbMTextIndents indents = new OdDbMTextIndents();
        pMText.getParagraphsIndent(indents);
        for (int i = 0; i < indents.Count; i++)
        {
          Console.WriteLine("Indent {0}", i);
          Console.WriteLine("First Line = {0}", indents[i].firstLineInd);
          Console.WriteLine("Paragraph = {0}", indents[i].paragraphInd);
          String rightString = "";
          for (int j = 0; j < indents[i].tabs.Count; j++)
          {
            if (j != 0)
            {
              rightString += ", ";
            }
            rightString += indents[i].tabs[j].ToString();
          }
          if (rightString != "")
          {
            Console.WriteLine("Tabs " + rightString);
          }
        }
        dumpEntityData(pMText);
      }
    };

    /************************************************************************/
    /* Ordinate Dimension Dumper                                            */
    /************************************************************************/
    class OdDbOrdinateDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbOrdinateDimension pDim = (OdDbOrdinateDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Defining Point = {0}", pDim.definingPoint().ToString());
        Console.WriteLine("Using x-Axis = {0}", pDim.isUsingXAxis().ToString());
        Console.WriteLine("Using y-Axis = {0}", pDim.isUsingYAxis().ToString());
        Console.WriteLine("Leader End Point = {0}", pDim.leaderEndPoint().ToString());
        Console.WriteLine("Origin = {0}", pDim.origin().ToString());
        dumpDimData(pDim);
      }
    }
    /************************************************************************/
    /* PolyFaceMesh Dumper                                                  */
    /************************************************************************/
    class OdDbPolyFaceMesh_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbPolyFaceMesh pPoly = (OdDbPolyFaceMesh)pEnt;
        Console.WriteLine("<{0}> {1}", pPoly.isA().name(), pPoly.getDbHandle().ToString());
        Console.WriteLine("Number of Vertices = {0}", pPoly.numVertices().ToString());
        Console.WriteLine("Number of Faces = {0}", pPoly.numFaces().ToString());

        /********************************************************************/
        /* dump vertices and faces                                          */
        /********************************************************************/
        OdDbObjectIterator pIter = pPoly.vertexIterator();
        int vertexCount = 0;
        int faceCount = 0;
        for (; !pIter.done(); pIter.step())
        {
          OdDbPolyFaceMeshVertex pVertex = OdDbPolyFaceMeshVertex.cast(pIter.entity());
          if (pVertex != null)
          {
            Console.WriteLine("<{0}> {1}", pVertex.isA().name(), ++vertexCount);
            Console.WriteLine("Handle = {0}", pVertex.getDbHandle().ToString());

            Console.WriteLine("Position = {0}", pVertex.position().ToString());
            dumpEntityData(pVertex);
          }
          else
          {
            OdDbFaceRecord pFace = OdDbFaceRecord.cast(pIter.entity());
            if (pFace != null)
            {
              String face = "{";
              for (int i = 0; i < 4; i++)
              {
                if (i != 0)
                {
                  face += " ";
                }
                face += pFace.getVertexAt(i).ToString();
              }
              face += "}";
              Console.WriteLine("<{0}> {1}", pFace.isA(), ++faceCount);
              Console.WriteLine("Handle = {0}", pFace.getDbHandle().ToString());
              Console.WriteLine("Vertices = {0}", face);
              dumpEntityData(pFace);
            }
            else
            { // Unknown entity type
              Console.WriteLine("Unexpected Entity");
            }
          }
        }
        dumpEntityData(pPoly);
      }

    };

    /************************************************************************/
    /* Ole2Frame                                                            */
    /************************************************************************/
    class OdDbOle2Frame_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbOle2Frame pOle = (OdDbOle2Frame)pEnt;
        Console.WriteLine("<{0}> {1}", pOle.isA().name(), pOle.getDbHandle().ToString());

        OdRectangle3d pos = new OdRectangle3d();
        pOle.position(pos);
        Console.WriteLine("Lower Left = {0}", pos.lowLeft);
        Console.WriteLine("Lower Right = {0}", pos.lowRight);
        Console.WriteLine("Upper Left = {0}", pos.upLeft);
        Console.WriteLine("Upper Right = {0}", pos.upRight);

        OdDbOle2Frame.Type oleType = OdDbOle2Frame.Type.kUnknown;
        try
        {
          oleType = pOle.getType();
          Console.WriteLine("Type = {0}", oleType);
          Console.WriteLine("User Type = {0}", pOle.getUserType().ToString());
        }
        catch (OdError) { } // getUserType may call exception in case if no ole application installed.

        if (oleType == OdDbOle2Frame.Type.kLink)
        {
          Console.WriteLine("Link Name = {0}", pOle.getLinkName().ToString());
          Console.WriteLine("Link Path = {0}", pOle.getLinkPath().ToString());
        }
        Console.WriteLine("Output Quality = {0}", pOle.outputQuality().ToString());
        Console.WriteLine("Auto Output Quality = {0}", pOle.autoOutputQuality().ToString());
        Console.WriteLine("Rotation = {0}", pOle.rotation().ToString());
        Console.WriteLine("WCS Width = {0}", pOle.wcsWidth().ToString());
        Console.WriteLine("WCS Height = {0}", pOle.wcsHeight().ToString());
        Console.WriteLine("Lock Aspect = {0}", pOle.lockAspect().ToString());
        dumpEntityData(pOle);
      }
    }

    /************************************************************************/
    /* Point Dumper                                                         */
    /************************************************************************/
    class OdDbPoint_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbPoint pPoint = (OdDbPoint)pEnt;
        Console.WriteLine("<{0}> {1}", pPoint.isA().name(), pPoint.getDbHandle().ToString());
        Console.WriteLine("Position = {0}", pPoint.position().ToString());
        Console.WriteLine("ECS Rotation = {0}", toDegreeString(pPoint.ecsRotation()));
        Console.WriteLine("Normal = {0}", pPoint.normal().ToString());
        Console.WriteLine("Thickness = {0}", pPoint.thickness().ToString());
        dumpEntityData(pPoint);
      }
    };

    /************************************************************************/
    /* Polygon Mesh Dumper                                                  */
    /************************************************************************/
    class OdDbPolygonMesh_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbPolygonMesh pPoly = (OdDbPolygonMesh)pEnt;
        Console.WriteLine("<{0}> {1}", pPoly.isA().name(), pPoly.getDbHandle().ToString());
        Console.WriteLine("m Size = {0}", pPoly.mSize().ToString());
        Console.WriteLine("m-Closed = {0}", pPoly.isMClosed().ToString());
        Console.WriteLine("m Surface Density = {0}", pPoly.mSurfaceDensity().ToString());
        Console.WriteLine("n Size = {0}", pPoly.nSize().ToString());
        Console.WriteLine("n-Closed = {0}", pPoly.isNClosed().ToString());
        Console.WriteLine("n Surface Density = {0}", pPoly.nSurfaceDensity().ToString());
        /********************************************************************/
        /* dump vertices                                                    */
        /********************************************************************/
        OdDbObjectIterator pIter = pPoly.vertexIterator();

        int vertexCount = 0;
        for (; !pIter.done(); pIter.step())
        {
          OdDbPolygonMeshVertex pVertex = pIter.entity() as OdDbPolygonMeshVertex;
          if (pVertex != null)
          {
            Console.WriteLine("<{0}> {1}", pVertex.isA().name(), vertexCount++);
            Console.WriteLine("Handle = {0}", pVertex.getDbHandle().ToString());
            Console.WriteLine("Position = {0}", pVertex.position().ToString());
            Console.WriteLine("Type = {0}", pVertex.vertexType().ToString());
          }
        }

        dumpEntityData(pPoly);
      }
    };

    /************************************************************************/
    /* Polyline Dumper                                                      */
    /************************************************************************/
    class OdDbPolyline_Dumper : OdDbPropertiesOverrule
    {
        // as it is stated:
        /** \details
           Determines whether the overrule is applicable and returns True if the overrule can be applied,
           or False, otherwise. It is a pure virtual method that must be implemented in the derived class.

           \param pOverruledSubject [in]  A raw pointer to an object to be overruled.
          */
        // thus isApplcable method is to be obkigatiry implemented if we plan to use list method of OdDbPolyline_Dumper class, 
        // otherwise we'll face an exception "Pure virtual method call"
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        OdDbPolyline pPoly = (OdDbPolyline)pEnt;
        Console.WriteLine("<{0}> {1}", pPoly.isA().name(), pPoly.getDbHandle().ToString());
        Console.WriteLine("Has Width = {0}", pPoly.hasWidth().ToString());
        if (!pPoly.hasWidth())
        {
          Console.WriteLine("Constant Width = {0}", pPoly.getConstantWidth().ToString());
        }
        Console.WriteLine("Has Bulges = {0}", pPoly.hasBulges().ToString());
        Console.WriteLine("Elevation = {0}", pPoly.elevation().ToString());
        Console.WriteLine("Normal = {0}", pPoly.normal().ToString());
        Console.WriteLine("Thickness = {0}", pPoly.thickness().ToString());

        /********************************************************************/
        /* dump vertices                                                    */
        /********************************************************************/
        for (uint i = 0; i < (int)pPoly.numVerts(); i++)
        {
          Console.WriteLine("Vertex {0}", i);
          Console.WriteLine("Segment Type = {0}", pPoly.segType(i));
          OdGePoint3d pt = new OdGePoint3d();
          pPoly.getPointAt(i, pt);
          Console.WriteLine("Point = {0}", pt);
          if (pPoly.hasWidth())
          {
            double startWidth;
            double endWidth;
            pPoly.getWidthsAt(i, out startWidth, out endWidth);
            Console.WriteLine("Start Width = {0}", startWidth);
            Console.WriteLine("End Width = {0}", endWidth);
          }
          if (pPoly.hasBulges())
          {
            Console.WriteLine("Bulge = {0}", pPoly.getBulgeAt(i));
            if (pPoly.segType(i) == OdDbPolyline.SegType.kArc)
            {
              Console.WriteLine("Bulge Angle = {0}", toDegreeString(4 * Math.Atan(pPoly.getBulgeAt(i))));
            }
          }
        }
        dumpEntityData(pPoly);
      }
    }

    class OdGiContextDumper : OdGiContext
    {
      OdDbDatabase m_pDb;

      public OdGiContextDumper(OdDbDatabase pDb)
      {
        m_pDb = pDb;
      }

      public override OdRxObject database()
      {
        return m_pDb;
      }

      public override OdGiDrawable openDrawable(OdDbStub s) { return null; }
    }
    /************************************************************************/
    /* Dump the common data and WorldDraw information for all               */
    /* entities without explicit dumpers                                    */
    /************************************************************************/
    class OdDbEntity_Dumper : OdDbPropertiesOverrule
    {
        // as it is stated:
        /** \details
           Determines whether the overrule is applicable and returns True if the overrule can be applied,
           or False, otherwise. It is a pure virtual method that must be implemented in the derived class.

           \param pOverruledSubject [in]  A raw pointer to an object to be overruled.
          */
        // thus isApplcable method is to be obkigatiry implemented if we plan to use list method of OdDbEntity_Dumper class, 
        // otherwise we'll face an exception "Pure virtual method call"
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        Console.WriteLine("<{0}> {1}", pEnt.isA().name(), pEnt.getDbHandle().ToString());
        dumpEntityData(pEnt);
        Console.WriteLine("WorldDraw()");

        /**********************************************************************/
        /* Create an OdGiContext instance for the vectorization               */
        /**********************************************************************/
        OdGiContextDumper ctx = new OdGiContextDumper(pEnt.database());
        /**********************************************************************/
        /* Create an OdGiWorldDraw instance for the vectorization             */
        /**********************************************************************/
        //OdGiWorldDrawDumper wd = new OdGiWorldDrawDumper();
        /**********************************************************************/
        /* Set the context                                                    */
        /**********************************************************************/
        //wd.setContext(&ctx);
        /**********************************************************************/
        /* Call worldDraw()                                                   */
        /**********************************************************************/
        //pEnt.worldDraw(&wd);

      }
    }

    /************************************************************************/
    /* Proxy Entity Dumper                                                  */
    /************************************************************************/
    class OdDbProxyEntity_Dumper : OdDbEntity_Dumper
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        // this will dump proxy entity graphics
        base.list(pEnt);

        OdDbProxyExt proxyEntExt = OdDbProxyExt.cast(pEnt);

        Console.WriteLine("Proxy OriginalClassName = {0}", proxyEntExt.originalClassName(pEnt));
        Console.WriteLine("Proxy ApplicationDescription = {0}", proxyEntExt.applicationDescription(pEnt));
        Console.WriteLine("Proxy OriginalDxfName = {0}", proxyEntExt.originalDxfName(pEnt));

        // If the entity is of type AcAdPart, then dump the associated SAT file.
        OdDbProxyEntity pProxy = (OdDbProxyEntity)pEnt;
        String satString = "";
        if (pProxy.originalClassName() == "AcAdPart" &&
            TD_Db.odGetSatFromProxy(pProxy, ref satString))
        {
          Console.WriteLine("AD_PART SAT file", shortenPath(satString));
        }
      }
    }

    /************************************************************************/
    /* OdDbMPolygon Entity Dumper                                                  */
    /************************************************************************/
    class OdDbMPolygon_Dumper : OdDbEntity_Dumper
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
        {
            // this will dump proxy entity graphics
            base.list(pEnt);

            OdDbMPolygon polyEntExt = OdDbMPolygon.cast(pEnt);

            Console.WriteLine("OdDbMPolygon color index = {0}", polyEntExt.colorIndex());
        }
    }

    /************************************************************************/
    /* Radial Dimension Dumper                                              */
    /************************************************************************/
    class OdDbRadialDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbRadialDimension pDim = (OdDbRadialDimension)pEnt;

        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Center = {0}", pDim.center().ToString());
        Console.WriteLine("Chord Point = {0}", pDim.chordPoint().ToString());
        Console.WriteLine("Leader Length = {0}", pDim.leaderLength().ToString());

        dumpDimData(pDim);
      }
    }
    /************************************************************************/
    /* Dump Raster Image Def                                               */
    /************************************************************************/
    static void dumpRasterImageDef(OdDbRasterImage pImage)
    {
      OdDbObjectId objectId = pImage.imageDefId();
      if (objectId.isNull())
        return;

      OdDbRasterImageDef pDef = objectId.openObject() as OdDbRasterImageDef;
      if (pDef == null)
        return;

      Console.WriteLine("<{0}> {1}", pDef.isA().name(), pDef.getDbHandle().ToString());
      Console.WriteLine("Source Filename = {0}", shortenPath(pDef.sourceFileName()));
      Console.WriteLine("Loaded = {0}", pDef.isLoaded().ToString());
      Console.WriteLine("mm per Pixel = {0}", pDef.resolutionMMPerPixel().ToString());
      Console.WriteLine("Loaded = {0}", pDef.isLoaded().ToString());
      Console.WriteLine("Resolution Units = {0}", pDef.resolutionUnits().ToString());
      Console.WriteLine("Size = {0}", pDef.size().ToString());
    }
    /************************************************************************/
    /* Dump Raster Image Data                                               */
    /************************************************************************/
    static void dumpRasterImageData(OdDbRasterImage pImage)
    {
      Console.WriteLine("Brightness = {0}", pImage.brightness());
      Console.WriteLine("Clipped = {0}", pImage.isClipped());
      Console.WriteLine("Contrast = {0}", pImage.contrast());
      Console.WriteLine("Fade = {0}", pImage.fade());
      Console.WriteLine("kClip = {0}", pImage.isSetDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kClip));
      Console.WriteLine("kShow = {0}", pImage.isSetDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShow));
      Console.WriteLine("kShowUnAligned = {0}", pImage.isSetDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShowUnAligned));
      Console.WriteLine("kTransparent = {0}", pImage.isSetDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kTransparent));
      Console.WriteLine("Scale = {0}", pImage.scale());

      /********************************************************************/
      /* Dump clip boundary                                               */
      /********************************************************************/
      if (pImage.isClipped())
      {
        Console.WriteLine("Clip Boundary Type", pImage.clipBoundaryType().ToString());
        if (pImage.clipBoundaryType() != OdDbRasterImage.ClipBoundaryType.kInvalid)
        {
          OdGePoint2dArray pt = pImage.clipBoundary();
          for (int i = 0; i < pt.Count; i++)
          {
            Console.WriteLine("Clip Point {0} = {1}", i, pt[i]);
          }
        }
      }

      /********************************************************************/
      /* Dump frame                                                       */
      /********************************************************************/
      OdGePoint3dArray vertices = new OdGePoint3dArray();
      pImage.getVertices(vertices);
      for (int i = 0; i < vertices.Count; i++)
      {
        Console.WriteLine("Frame Vertex {0} = {1}", i, vertices[i]);
      }

      /********************************************************************/
      /* Dump orientation                                                 */
      /********************************************************************/
      OdGePoint3d origin = new OdGePoint3d();
      OdGeVector3d u = new OdGeVector3d();
      OdGeVector3d v = new OdGeVector3d();
      pImage.getOrientation(origin, u, v);
      Console.WriteLine("Orientation");
      Console.WriteLine("Origin = {0}", origin);
      Console.WriteLine("uVector = {0}", u);
      Console.WriteLine("vVector = {0}", v);
      dumpRasterImageDef(pImage);
      dumpEntityData(pImage);
    }

    /************************************************************************/
    /* Raster Image Dumper                                                  */
    /************************************************************************/
    class OdDbRasterImage_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbRasterImage pImage = (OdDbRasterImage)pEnt;
        Console.WriteLine("<{0}> {1}", pImage.isA().name(), pImage.getDbHandle().ToString());
        Console.WriteLine("Image size = {0}", pImage.imageSize());
        dumpRasterImageData(pImage);
      }
    };

    /************************************************************************/
    /* Ray Dumper                                                          */
    /************************************************************************/
    class OdDbRay_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbRay pRay = (OdDbRay)pEnt;
        Console.WriteLine("<{0}> {1}", pRay.isA().name(), pRay.getDbHandle().ToString());
        Console.WriteLine("Base Point = {0}", pRay.basePoint());
        Console.WriteLine("Unit Direction = {0}", pRay.unitDir());
        dumpCurveData(pRay);
      }
    }

    /************************************************************************/
    /* Region Dumper                                                        */
    /************************************************************************/
    class OdDbRegion_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbRegion pRegion = (OdDbRegion)pEnt;
        Console.WriteLine("<{0}> {1}", pRegion.isA().name(), pRegion.getDbHandle().ToString());
        dumpEntityData(pRegion);

        //OdWrFileBuf ow("RegionAcisOut.acis");
        //pRegion.acisOut(&ow , kAfTypeASCII);
      }
    };
    /************************************************************************/
    /* Rotated Dimension Dumper                                             */
    /************************************************************************/
    class OdDbRotatedDimension_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbRotatedDimension pDim = (OdDbRotatedDimension)pEnt;
        Console.WriteLine("<{0}> {1}", pDim.isA().name(), pDim.getDbHandle().ToString());
        Console.WriteLine("Dimension Line Point = {0}", pDim.dimLinePoint().ToString());
        Console.WriteLine("Oblique = {0}", toDegreeString(pDim.oblique()));
        Console.WriteLine("Rotation = {0}", toDegreeString(pDim.rotation()));
        Console.WriteLine("Extension Line 1 Point = {0}", pDim.xLine1Point().ToString());
        Console.WriteLine("Extension Line 2 Point = {0}", pDim.xLine2Point().ToString());
        dumpDimData(pDim);
      }
    };
    /************************************************************************/
    /* Shape Dumper                                                          */
    /************************************************************************/
    class OdDbShape_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbShape pShape = (OdDbShape)pEnt;
        Console.WriteLine("<{0}> {1}", pShape.isA().name(), pShape.getDbHandle().ToString());

        if (!pShape.styleId().isNull())
        {
          OdDbTextStyleTableRecord pStyle = (OdDbTextStyleTableRecord)pShape.styleId().safeOpenObject();
          Console.WriteLine("Filename = {0}", shortenPath(pStyle.fileName()));
        }

        Console.WriteLine("Shape Number = {0}", pShape.shapeNumber().ToString());
        Console.WriteLine("Shape Name = {0}", pShape.name().ToString());
        Console.WriteLine("Position = {0}", pShape.position().ToString());
        Console.WriteLine("Size = {0}", pShape.size().ToString());
        Console.WriteLine("Rotation = {0}", toDegreeString(pShape.rotation()));
        Console.WriteLine("Oblique = {0}", toDegreeString(pShape.oblique()));
        Console.WriteLine("Normal = {0}", pShape.normal().ToString());
        Console.WriteLine("Thickness = {0}", pShape.thickness().ToString());
        dumpEntityData(pShape);
      }
    };

    /************************************************************************/
    /* Solid Dumper                                                         */
    /************************************************************************/
    class OdDbSolid_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbSolid pSolid = (OdDbSolid)pEnt;
        Console.WriteLine("<{0}> {1}", pSolid.isA().name(), pSolid.getDbHandle().ToString());

        for (int i = 0; i < 4; i++)
        {
          OdGePoint3d pt = new OdGePoint3d();
          pSolid.getPointAt(i, pt);
          Console.WriteLine("Point {0} = {1}", i, pt);
        }
        dumpEntityData(pSolid);
      }
    };

    /************************************************************************/
    /* Spline Dumper                                                        */
    /************************************************************************/
    class OdDbSpline_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbSpline pSpline = (OdDbSpline)pEnt;
        Console.WriteLine("<{0}> {1}", pSpline.isA().name(), pSpline.getDbHandle().ToString());

        int degree;
        bool rational, closed, periodic;
        OdGePoint3dArray ctrlPts = new OdGePoint3dArray();
        OdGeDoubleArray weights = new OdGeDoubleArray();
        OdGeDoubleArray knots = new OdGeDoubleArray();
        double tol, kTol;
        pSpline.getNurbsData(out degree, out rational, out closed, out periodic, ctrlPts, knots, weights, out tol, out kTol);
        Console.WriteLine("Degree = {0}", degree);
        Console.WriteLine("Rational = {0}", rational);
        Console.WriteLine("Periodic = {0}", periodic);
        Console.WriteLine("Control Point Tolerance = {0:4E}", tol);
        Console.WriteLine("Knot Tolerance = {0:4E}", kTol);

        Console.WriteLine("Number of control points = {0}", ctrlPts.Count);
        for (int i = 0; i < ctrlPts.Count; i++)
        {
          Console.WriteLine("Control Point {0} = {1}", i, ctrlPts[i]);
        }

        Console.WriteLine("Number of Knots = {0}", knots.Count);
        for (int i = 0; i < knots.Count; i++)
        {
          Console.WriteLine("Knot {0} = {1}", i, knots[i]);
        }

        if (rational)
        {
          Console.WriteLine("Number of Weights = {0}", weights.Count);
          for (int i = 0; i < weights.Count; i++)
          {
            Console.WriteLine("Weight {0} = {1}", i, weights[i]);
          }
        }
        dumpCurveData(pEnt);
      }
    };
    /************************************************************************/
    /* Table Dumper                                                         */
    /************************************************************************/
    class OdDbTable_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbTable pTable = (OdDbTable)pEnt;
        Console.WriteLine("<{0}> {1}", pTable.isA().name(), pTable.getDbHandle().ToString());
        Console.WriteLine("Position = {0}", pTable.position().ToString());
        Console.WriteLine("X-Direction = {0}", pTable.direction().ToString());
        Console.WriteLine("Normal = {0}", pTable.normal());
        Console.WriteLine("Height = {0}", pTable.height());
        Console.WriteLine("Width = {0}", pTable.width());
        Console.WriteLine("Rows = {0}", pTable.numRows());
        Console.WriteLine("Columns = {0}", pTable.numColumns());

        OdDbTableStyle pStyle = (OdDbTableStyle)pTable.tableStyle().safeOpenObject();
        Console.WriteLine("Table Style = {0}", pStyle.getName().ToString());
        dumpEntityData(pTable);
      }
    };
    /************************************************************************/
    /* Text Dumper                                                          */
    /************************************************************************/
    class OdDbText_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
      public override void list(OdDbEntity pEnt)
      {
        OdDbText pText = (OdDbText)pEnt;
        Console.WriteLine();
        Console.WriteLine("<{0}> {1}", pText.isA().name(), pText.getDbHandle().ToString());
        dumpTextData(pText);
      }
    }
    /************************************************************************/
    /* Trace Dumper                                                         */
    /************************************************************************/
    class OdDbTrace_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbTrace pTrace = (OdDbTrace)pEnt;
        Console.WriteLine("<{0}> {1}", pTrace.isA().name(), pTrace.getDbHandle().ToString());

        for (int i = 0; i < 4; i++)
        {
          OdGePoint3d pt = new OdGePoint3d();
          pTrace.getPointAt(i, pt);
          Console.WriteLine("Point {0} = {1}", i, pt);
        }
        dumpEntityData(pTrace);
      }
    };
    /************************************************************************/
    /* Viewport Dumper                                                       */
    /************************************************************************/
    class OdDbViewport_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbViewport pVport = (OdDbViewport)pEnt;
        Console.WriteLine("<{0}> {1}", pEnt.isA().name(), pEnt.getDbHandle().ToString());
        Console.WriteLine("Back Clip Distance = {0}", pVport.backClipDistance().ToString());
        Console.WriteLine("Back Clip On = {0}", pVport.isBackClipOn().ToString());
        Console.WriteLine("Center Point = {0}", pVport.centerPoint().ToString());
        Console.WriteLine("Circle sides = {0}", pVport.circleSides().ToString());
        Console.WriteLine("Custom Scale = {0}", pVport.customScale().ToString());
        Console.WriteLine("Elevation = {0}", pVport.elevation().ToString());
        Console.WriteLine("Front Clip at Eye = {0}", pVport.isFrontClipAtEyeOn().ToString());
        Console.WriteLine("Front Clip Distance = {0}", pVport.frontClipDistance().ToString());
        Console.WriteLine("Front Clip On = {0}", pVport.isFrontClipOn().ToString());
        Console.WriteLine("Plot style sheet = {0}", pVport.effectivePlotStyleSheet().ToString());

        OdDbObjectIdArray layerIds = new OdDbObjectIdArray();
        pVport.getFrozenLayerList(layerIds);
        if (layerIds.Count != 0)
        {
          Console.WriteLine("Frozen Layers:");
          for (int i = 0; i < layerIds.Count; i++)
          {
            Console.WriteLine("{0} = {1}", i, layerIds[i].getHandle());
          }
        }
        else
        {
          Console.WriteLine("Frozen Layers = None");
        }

        OdGePoint3d origin = new OdGePoint3d();
        OdGeVector3d xAxis = new OdGeVector3d();
        OdGeVector3d yAxis = new OdGeVector3d();
        pVport.getUcs(origin, xAxis, yAxis);
        Console.WriteLine("UCS origin = {0}", origin);
        Console.WriteLine("UCS x-Axis = {0}", xAxis);
        Console.WriteLine("UCS y-Axis = {0}", yAxis);
        Console.WriteLine("Grid Increment = {0}", pVport.gridIncrement().ToString());
        Console.WriteLine("Grid On = {0}", pVport.isGridOn().ToString());
        Console.WriteLine("Height = {0}", pVport.height().ToString());
        Console.WriteLine("Lens Length = {0}", pVport.lensLength().ToString());
        Console.WriteLine("Locked = {0}", pVport.isLocked().ToString());
        Console.WriteLine("Non-Rectangular Clip = {0}", pVport.isNonRectClipOn().ToString());

        if (!pVport.nonRectClipEntityId().isNull())
        {
          Console.WriteLine("Non-rectangular Clipper = {0}", pVport.nonRectClipEntityId().getHandle().ToString());
        }
        Console.WriteLine("Render Mode = {0}", pVport.renderMode().ToString());
        Console.WriteLine("Remove Hidden Lines = {0}", pVport.hiddenLinesRemoved().ToString());
        Console.WriteLine("Shade Plot = {0}", pVport.shadePlot().ToString());
        Console.WriteLine("Snap Isometric = {0}", pVport.isSnapIsometric().ToString());
        Console.WriteLine("Snap On = {0}", pVport.isSnapOn().ToString());
        Console.WriteLine("Transparent = {0}", pVport.isTransparent().ToString());
        Console.WriteLine("UCS Follow = {0}", pVport.isUcsFollowModeOn().ToString());
        Console.WriteLine("UCS Icon at Origin = {0}", pVport.isUcsIconAtOrigin().ToString());

        OrthographicView orthoUCS;
        Console.WriteLine("UCS Orthographic = {0}", pVport.isUcsOrthographic(out orthoUCS));
        Console.WriteLine("Orthographic UCS = {0}", orthoUCS);
        Console.WriteLine("UCS Saved with VP = {0}", pVport.isUcsSavedWithViewport());

        if (!pVport.ucsName().isNull())
        {
          OdDbUCSTableRecord pUCS = (OdDbUCSTableRecord)pVport.ucsName().safeOpenObject();
          Console.WriteLine("UCS Name = {0}", pUCS.getName().ToString());
        }
        else
        {
          Console.WriteLine("UCS Name = Null");
        }

        Console.WriteLine("View Center = {0}", pVport.viewCenter().ToString());
        Console.WriteLine("View Height = {0}", pVport.viewHeight().ToString());
        Console.WriteLine("View Target = {0}", pVport.viewTarget().ToString());
        Console.WriteLine("Width = {0}", pVport.width().ToString());
        dumpEntityData(pEnt);
      }
    };

    /************************************************************************/
    /* Wipeout Dumper                                                  */
    /************************************************************************/
    class OdDbWipeout_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbWipeout pWipeout = (OdDbWipeout)pEnt;
        Console.WriteLine("<{0}> {1}", pWipeout.isA().name(), pWipeout.getDbHandle().ToString());
        dumpRasterImageData(pWipeout);
      }
    };

    /************************************************************************/
    /* Xline Dumper                                                         */
    /************************************************************************/
    class OdDbXline_Dumper : OdDbPropertiesOverrule
    {
        public override bool isApplicable(OdRxObject pOverruledSubject)
        {
            return true;
        }
        public override void list(OdDbEntity pEnt)
      {
        OdDbXline pXline = (OdDbXline)pEnt;
        Console.WriteLine("<{0}> {1}", pXline.isA().name(), pXline.getDbHandle().ToString());
        Console.WriteLine("Base Point = {0}", pXline.basePoint().ToString());
        Console.WriteLine("Unit Direction = {0}", pXline.unitDir().ToString());
        dumpCurveData(pXline);
      }
    };
    static OdDb2dPolyline_Dumper m_2dPolylineDumper = new OdDb2dPolyline_Dumper();
    static OdDb3PointAngularDimension_Dumper m_3PointAngularDimensionDumper = new OdDb3PointAngularDimension_Dumper();
    static OdDb3dPolyline_Dumper m_3dPolylineDumper = new OdDb3dPolyline_Dumper();
    static OdDb3dSolid_Dumper m_3dSolidDumper = new OdDb3dSolid_Dumper();
    static OdDbAlignedDimension_Dumper m_alignedDimensionDumper = new OdDbAlignedDimension_Dumper();
    static OdDbArcAlignedText_Dumper m_arcAlignedTextDumper = new OdDbArcAlignedText_Dumper();
    static OdDbArcDimension_Dumper m_arcDimensionDumper = new OdDbArcDimension_Dumper();
    static OdDbArc_Dumper m_arcDumper = new OdDbArc_Dumper();
    static OdDbBlockReference_Dumper m_blockReference = new OdDbBlockReference_Dumper();
    static OdDbBody_Dumper m_bodyDumper = new OdDbBody_Dumper();
    static OdDbCircle_Dumper m_circleDumper = new OdDbCircle_Dumper();
    static OdDbCircleGripPointsPE m_circPE = new OdDbCircleGripPointsPE();
    static OdDbDiametricDimension_Dumper m_diametricDimensionDumper = new OdDbDiametricDimension_Dumper();
    static OdDbEllipse_Dumper m_ellipseDumper = new OdDbEllipse_Dumper();
    static OdDbEntity_Dumper m_entityDumper = new OdDbEntity_Dumper();
    static OdDbMPolygon_Dumper m_polyDumper = new OdDbMPolygon_Dumper();
    static OdDbFace_Dumper m_faceDumper = new OdDbFace_Dumper();
    static OdDbFcf_Dumper m_fcfDumper = new OdDbFcf_Dumper();
    static OdDbHatch_Dumper m_hatchDumper = new OdDbHatch_Dumper();
    static OdDbLeader_Dumper m_leaderDumper = new OdDbLeader_Dumper();
    static OdDbLine_Dumper m_lineDumper = new OdDbLine_Dumper();
    static OdDbMInsertBlock_Dumper m_mInsertBlock = new OdDbMInsertBlock_Dumper();
    static OdDbMText_Dumper m_mTextDumper = new OdDbMText_Dumper();
    static OdDbMline_Dumper m_mlineDumper = new OdDbMline_Dumper();
    static OdDbOle2Frame_Dumper m_ole2FrameDumper = new OdDbOle2Frame_Dumper();
    static OdDbOrdinateDimension_Dumper m_ordinateDimensionDumper = new OdDbOrdinateDimension_Dumper();
    static OdDbPoint_Dumper m_pointDumper = new OdDbPoint_Dumper();
    static OdDbPolyFaceMesh_Dumper m_polyFaceMeshDumper = new OdDbPolyFaceMesh_Dumper();
    static OdDbPolygonMesh_Dumper m_polygonMesh = new OdDbPolygonMesh_Dumper();
    static OdDbPolyline_Dumper m_polylineDumper = new OdDbPolyline_Dumper();
    static OdDbProxyEntity_Dumper m_proxyEntityDumper = new OdDbProxyEntity_Dumper();
    static OdDbRadialDimension_Dumper m_radialDimensionDumper = new OdDbRadialDimension_Dumper();
    static OdDbRasterImage_Dumper m_imageDumper = new OdDbRasterImage_Dumper();
    static OdDbRay_Dumper m_rayDumper = new OdDbRay_Dumper();
    static OdDbRegion_Dumper m_regionDumper = new OdDbRegion_Dumper();
    static OdDbRotatedDimension_Dumper m_rotatedDimensionDumper = new OdDbRotatedDimension_Dumper();
    static OdDbShape_Dumper m_shapeDumper = new OdDbShape_Dumper();
    static OdDbSolid_Dumper m_solidDumper = new OdDbSolid_Dumper();
    static OdDbSpline_Dumper m_splineDumper = new OdDbSpline_Dumper();
    static OdDbTable_Dumper m_tableDumper = new OdDbTable_Dumper();
    static OdDbText_Dumper m_textDumper = new OdDbText_Dumper();
    static OdDbTrace_Dumper m_traceDumper = new OdDbTrace_Dumper();
    static OdDbViewport_Dumper m_viewportDumper = new OdDbViewport_Dumper();
    static OdDbWipeout_Dumper m_wipeoutDumper = new OdDbWipeout_Dumper();
    static OdDbXline_Dumper m_xlineDumper = new OdDbXline_Dumper();

    static void addXs()
    {
      OdDbObjectOverrule.addOverrule(OdDbEntity.desc(), m_entityDumper);
      OdDbObjectOverrule.addOverrule(OdDb2LineAngularDimension.desc(), m_2LineAngularDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDb2dPolyline.desc(), m_2dPolylineDumper);
      OdDbObjectOverrule.addOverrule(OdDb3PointAngularDimension.desc(), m_3PointAngularDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDb3dPolyline.desc(), m_3dPolylineDumper);
      OdDbObjectOverrule.addOverrule(OdDb3dSolid.desc(), m_3dSolidDumper);
      OdDbObjectOverrule.addOverrule(OdDbAlignedDimension.desc(), m_alignedDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbArc.desc(), m_arcDumper);
      OdDbObjectOverrule.addOverrule(OdDbArcAlignedText.desc(), m_arcAlignedTextDumper);
      OdDbObjectOverrule.addOverrule(OdDbArcDimension.desc(), m_arcDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbBlockReference.desc(), m_blockReference);
      OdDbObjectOverrule.addOverrule(OdDbBody.desc(), m_bodyDumper);
      //OdRxOverrule.addOverrule(OdDbCircle.desc(), m_circleDumper);
      //OdDbCircle.desc().addX(OdDbGripPointsPE.desc(), m_circPE);
      OdDbObjectOverrule.addOverrule(OdDbCircle.desc(), m_circleDumper);
      Console.WriteLine("m_circPE added");
      //OdDbMPolygon.desc().addX(OdDbMPolygon.desc(), m_polyDumper);
      OdDbObjectOverrule.addOverrule(OdDbMPolygon.desc(), m_polyDumper);
      Console.WriteLine("OdDbMPolygon added");
      OdDbObjectOverrule.addOverrule(OdDbDiametricDimension.desc(), m_diametricDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbEllipse.desc(), m_ellipseDumper);
      OdDbObjectOverrule.addOverrule(OdDbFace.desc(), m_faceDumper);
      OdDbObjectOverrule.addOverrule(OdDbFcf.desc(), m_fcfDumper);
      OdDbObjectOverrule.addOverrule(OdDbHatch.desc(), m_hatchDumper);
      OdDbObjectOverrule.addOverrule(OdDbLeader.desc(), m_leaderDumper);
      OdDbObjectOverrule.addOverrule(OdDbLine.desc(), m_lineDumper);
      //OdDbObjectOverrule.addOverrule(OdDbLine.desc(), Overrule.overrule);
      OdDbObjectOverrule.addOverrule(OdDbMInsertBlock.desc(), m_mInsertBlock);
      OdDbObjectOverrule.addOverrule(OdDbMText.desc(), m_mTextDumper);
      OdDbObjectOverrule.addOverrule(OdDbMline.desc(), m_mlineDumper);
      OdDbObjectOverrule.addOverrule(OdDbOle2Frame.desc(), m_ole2FrameDumper);
      OdDbObjectOverrule.addOverrule(OdDbOrdinateDimension.desc(), m_ordinateDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbPoint.desc(), m_pointDumper);
      OdDbObjectOverrule.addOverrule(OdDbPolyFaceMesh.desc(), m_polyFaceMeshDumper);
      OdDbObjectOverrule.addOverrule(OdDbPolygonMesh.desc(), m_polygonMesh);
      OdDbObjectOverrule.addOverrule(OdDbPolyline.desc(), m_polylineDumper);
      OdDbObjectOverrule.addOverrule(OdDbProxyEntity.desc(), m_proxyEntityDumper);
      OdDbObjectOverrule.addOverrule(OdDbProxyObject.desc(), m_proxyEntityDumper);
      OdDbObjectOverrule.addOverrule(OdDbRadialDimension.desc(), m_radialDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbRasterImage.desc(), m_imageDumper);
      OdDbObjectOverrule.addOverrule(OdDbRay.desc(), m_rayDumper);
      OdDbObjectOverrule.addOverrule(OdDbRegion.desc(), m_regionDumper);
      OdDbObjectOverrule.addOverrule(OdDbRotatedDimension.desc(), m_rotatedDimensionDumper);
      OdDbObjectOverrule.addOverrule(OdDbShape.desc(), m_shapeDumper);
      OdDbObjectOverrule.addOverrule(OdDbSolid.desc(), m_solidDumper);
      OdDbObjectOverrule.addOverrule(OdDbSpline.desc(), m_splineDumper);
      OdDbObjectOverrule.addOverrule(OdDbTable.desc(), m_tableDumper);
      OdDbObjectOverrule.addOverrule(OdDbText.desc(), m_textDumper);
      OdDbObjectOverrule.addOverrule(OdDbTrace.desc(), m_traceDumper);
      OdDbObjectOverrule.addOverrule(OdDbViewport.desc(), m_viewportDumper);
      OdDbObjectOverrule.addOverrule(OdDbWipeout.desc(), m_wipeoutDumper);
      OdDbObjectOverrule.addOverrule(OdDbXline.desc(), m_xlineDumper);
    }
    static void Main(string[] args)
    {
        MemoryManager mMan = MemoryManager.GetMemoryManager();
        MemoryTransaction mStartTrans = mMan.StartTransaction();
        //for (int i = 0; i < 5; i++)
        {
            /**********************************************************************/
            /* Verify the argument count and display an error message as required */
            /**********************************************************************/
            Srv HostApp = new Srv();
            Console.WriteLine("\nOdReadExSwigMgd developed using {0} ver {1}", HostApp.product(), HostApp.versionString());
            OdDbLibraryInfo libInfo = TD_Db.oddbGetLibraryInfo();
            Console.WriteLine("\nOdReadExSwigMgd developed using minor major ver {0} ", HostApp.releaseMajorMinorString());
            Console.WriteLine("\nOdDbLibraryInfo  libVer {0} buildComments {1}", libInfo.getLibVersion(), libInfo.getBuildComments());

            if (args.Length != 1)
            {
                Console.WriteLine("\tusage: OdReadExSwigMgd filename");
                Console.WriteLine("\nPress ENTER to continue...");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine(string.Format("File {0} does not exist", args[0]));
                return;
            }
            /********************************************************************/
            /* Initialize Teigha.                                               */
            /********************************************************************/
            ExSystemServices Serv = new ExSystemServices();
            TD_Db.odInitialize(Serv);

            ////////////////////////////////////////////////////////////
            //OdDbDynBlockReferencePropertyArray
            Globals.odrxDynamicLinker().loadModule("TD_DynBlocks");
            OdDbDynBlockReferencePropertyArray pArr = new OdDbDynBlockReferencePropertyArray();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("OdDbDynBlockReferenceProperty create");
                OdDbDynBlockReferenceProperty prop = new OdDbDynBlockReferenceProperty(new IntPtr(), false);
                Console.WriteLine("OdDbDynBlockReferencePropertyArray Add");
                pArr.Add(prop);
            }
            foreach (OdDbDynBlockReferenceProperty pr in pArr)
            {
                Console.WriteLine("OdDbDynBlockReferenceProperty");
            }
            
            // 2 lines of code just to test key OdFlatMemStream calls
            OdFlatMemStream pStr = OdFlatMemStream.createNew(IntPtr.Zero, 0);
            OdFlatMemStream pStr1 = OdFlatMemStream.cast(pStr);

            ////////////////////////////////////////////////////////////
            // Some PlotStyle test code -- start
            OdDbPlotSettingsValidator pVal = HostApp.plotSettingsValidator();
            int hashcode = pVal.GetHashCode();

            OdRxModule mod = Globals.odrxDynamicLinker().loadApp("PlotStyleServices", false);
            OdPsPlotStyleServices PsServices = new OdPsPlotStyleServices(OdRxModule.getCPtr(mod).Handle, false);

            string psFileName = HostApp.findFile("IndexedColorTest.stb");
            if (psFileName.Length > 0)
            {
              OdStreamBuf pPsFile = TD_Db.odSystemServices().createFile(psFileName);
              OdPsPlotStyleTable pPlotStyleTable = PsServices.loadPlotStyleTable(pPsFile);

              bool bRes = false;
              OdPsPlotStyleData psData = new OdPsPlotStyleData();
              pPlotStyleTable.plotStyleAt("TestStyle").getData(psData);
              bool flag1 = psData.color().isByACI();
              short color_index = psData.color().colorIndex();
              if (color_index == 40)
              {
                bRes = true;
              }
              pPlotStyleTable.plotStyleAt("TestStyle1").getData(psData);
              bool flag2 = psData.color().isByACI();
              short color_index2 = psData.color().colorIndex();
              if (color_index2 == (ushort)OdCmEntityColor.ACIcolorMethod.kACIGreen)
              {
                bRes = true;
              }
            }
	    // -- end --

            /********************************************************************/
            /* Display the Product and Version that created the executable      */
            /********************************************************************/

            try
            {
              OdDbObjectOverrule.setIsOverruling(true); // see OverruleSample from Core wth illustration isApplicable() implementation
                                                    // isApplicable() for overruling feature must be implemented in the derived class.

                addXs();
                Globals.odrxDynamicLinker().loadModule("ExFieldEvaluator");
                /******************************************************************/
                /* Create a database and load the drawing into it.                */
                /*                                                                */
                /* Specified arguments are as followed:                           */
                /*     filename, allowCPConversion, partialLoad, openMode         */
                /******************************************************************/

                //using (OdStreamBuf pStramBuf = Serv.createFile(args[0]))
                {
                    //using (AuditInfo aiAppAudit = new AuditInfo(System.IO.Path.GetDirectoryName(args[0]) + "AuditReport.txt"))
                    AuditInfo aiAppAudit = new AuditInfo(System.IO.Path.GetDirectoryName(args[0]) + "AuditReport.txt");
                    {
                        aiAppAudit.setFixErrors(true);
                        aiAppAudit.setPrintDest(OdAuditInfo.PrintDest.kBoth);

                        //using (OdDbDatabase pDb = HostApp.recoverFile(pStramBuf, aiAppAudit))
                        for (int i = 0; i < 20; i++)
                        {
                            MemoryTransaction mTr = mMan.StartTransaction();
                            //using (OdDbDatabase pDb = HostApp.readFile(args[0], true, false, FileShareMode.kShareDenyNo, ""))
                            OdDbDatabase pDb = HostApp.readFile(args[0], true, false, FileShareMode.kShareDenyNo, "");
                            {
                                if (pDb != null)
                                {
                                    TD_Db.oddbEvaluateFields(pDb, (int)OdDbField.EvalContext.kOpen);
                                    /****************************************************************/
                                    /* Display the File Version                                     */
                                    /****************************************************************/
                                    Console.WriteLine("\nFile Version: {0}", pDb.originalFileVersion());

                                    /****************************************************************/
                                    /* Dump the database                                            */
                                    /****************************************************************/
                                    DbDumper dumper = new DbDumper();
                                    dumper.dump(pDb);
                                }
                            }
                            mMan.StopTransaction(mTr);
                        }
                    }
                }
            }
            /********************************************************************/
            /* Display the error                                                */
            /********************************************************************/
            catch (OdError e)
            {
                Console.WriteLine("\nTeigha Error: {0}", HostApp.getErrorDescription((uint)e.code()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nSystem Error: {0}.\nPress ENTER to continue...\n", ex.Message);
            }
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            mMan.StopTransaction(mStartTrans);
            mMan.StopAll();
            TD_Db.odUninitialize();
            Teigha.Core.Helpers.odUninit();
        }
    }
  }
}
