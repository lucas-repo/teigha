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
using System.Runtime.InteropServices;

namespace OdWriteExMgd
{
  class DbFiller
  {
    OdDbObjectIdArray m_layoutEntities = new OdDbObjectIdArray();
    List<OdGeCurve2d> m_edgeReferences = new List<OdGeCurve2d>(); // keep references from GC
    double m_textSize;
    OdGeVector3d m_textOffset;
    OdGeVector3d m_textLine;
    public DbFiller()
    {
      m_textSize = 0.2;
      m_textOffset = new OdGeVector3d(0.5 * m_textSize, -0.5 * m_textSize, 0);
      m_textLine = new OdGeVector3d(0, -1.6 * m_textSize, 0);
    }

    void addCustomObjects(OdDbDatabase pDb)
    {
      //Open the main dictionary
      OdDbDictionary pMain =
        (OdDbDictionary)pDb.getNamedObjectsDictionaryId().safeOpenObject(OpenMode.kForWrite);

      // Create the new dictionary.
      OdDbDictionary pOdtDic = OdDbDictionary.createObject();

      // Add new dictionary to the main dictionary.
      OdDbObjectId dicId = pMain.setAt("TEIGHA_OBJECTS", pOdtDic);

      // Create a new xrecord object.
      OdDbXrecord pXRec = OdDbXrecord.createObject();

      // Add the xrecord the owning dictionary.
      OdDbObjectId xrId = pOdtDic.setAt("PROPERTIES_1", pXRec);

      OdResBuf pRb, temp;
      temp = pRb = OdResBuf.newRb(1000);
      temp.setString("Sample XRecord Data");

      temp = appendXDataPair(temp, 40);
      temp.setDouble(3.14159);

      temp = appendXDataPair(temp, 70);
      temp.setInt16(312);

      pXRec.setFromRbChain(pRb);
    } //end addCustomObjects

    /************************************************************************/
    /* Add a Layer to the specified database                                */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    OdDbObjectId addLayer(OdDbDatabase pDb, string name, Int16 color, string linetype)
    {
      /**********************************************************************/
      /* Open the layer table                                               */
      /**********************************************************************/
      OdDbLayerTable pLayers = (OdDbLayerTable)pDb.getLayerTableId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a layer table record                                        */
      /**********************************************************************/
      OdDbLayerTableRecord pLayer = OdDbLayerTableRecord.createObject();

      /**********************************************************************/
      /* Layer must have a name before adding it to the table.              */
      /**********************************************************************/
      pLayer.setName(name);

      /**********************************************************************/
      /* Set the Color.                                                     */
      /**********************************************************************/
      pLayer.setColorIndex(color);

      /**********************************************************************/
      /* Set the Linetype.                                                  */
      /**********************************************************************/
      OdDbLinetypeTable pLinetypes = (OdDbLinetypeTable)pDb.getLinetypeTableId().safeOpenObject();
      OdDbObjectId linetypeId = pLinetypes.getAt(linetype);
      pLayer.setLinetypeObjectId(linetypeId);

      /**********************************************************************/
      /* Add the record to the table.                                       */
      /**********************************************************************/
      OdDbObjectId layerId = pLayers.add(pLayer);

      return layerId;
    }

    /************************************************************************/
    /* Add a Registered Application to the specified database               */
    /************************************************************************/
    bool addRegApp(OdDbDatabase pDb, string name)
    {
      return pDb.newRegApp(name);
    }

    /************************************************************************/
    /* Add a Text Style to the specified database                           */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    OdDbObjectId addStyle(OdDbDatabase pDb,
                                    string styleName,
                                    double textSize,
                                    double xScale,
                                    double priorSize,
                                    double obliquing,
                                    string fileName,
                                    bool isShapeFile,
                                    string ttFaceName,
                                    bool bold,
                                    bool italic,
                                    int charset,
                                    int pitchAndFamily)
    {
      OdDbObjectId styleId;

      OdDbTextStyleTable pStyles = (OdDbTextStyleTable)pDb.getTextStyleTableId().safeOpenObject(OpenMode.kForWrite);
      OdDbTextStyleTableRecord pStyle = OdDbTextStyleTableRecord.createObject();

      // Name must be set before a table object is added to a table.  The
      // isShapeFile flag must also be set (if true) before adding the object
      // to the database.
      pStyle.setName(styleName);
      pStyle.setIsShapeFile(isShapeFile);

      // Add the object to the table.
      styleId = pStyles.add(pStyle);

      // Set the remaining properties.
      pStyle.setTextSize(textSize);
      pStyle.setXScale(xScale);
      pStyle.setPriorSize(priorSize);
      pStyle.setObliquingAngle(obliquing);
      pStyle.setFileName(fileName);
      if (isShapeFile)
      {
        pStyle.setPriorSize(22.45);
      }
      if (ttFaceName != "")
      {
        pStyle.setFont(ttFaceName, bold, italic, charset, pitchAndFamily);
      }

      return styleId;
    }

    /************************************************************************/
    /* Add a Linetype to the specified database                             */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    OdDbObjectId addLinetype(OdDbDatabase pDb, string name, string comments)
    {
      /**********************************************************************/
      /* Open the Linetype table                                            */
      /**********************************************************************/
      OdDbLinetypeTable pLinetypes = (OdDbLinetypeTable)pDb.getLinetypeTableId().safeOpenObject(OpenMode.kForWrite);
      OdDbLinetypeTableRecord pLinetype = OdDbLinetypeTableRecord.createObject();

      /**********************************************************************/
      /* Linetype must have a name before adding it to the table.           */
      /**********************************************************************/
      pLinetype.setName(name);

      /**********************************************************************/
      /* Add the record to the table.                                       */
      /**********************************************************************/
      OdDbObjectId linetypeId = pLinetypes.add(pLinetype);

      /**********************************************************************/
      /* Add the Comments.                                                  */
      /**********************************************************************/
      pLinetype.setComments(comments);

      return linetypeId;
    }

    /************************************************************************/
    /* Add Several linetypes to the specified database                      */
    /************************************************************************/
    void addLinetypes(OdDbDatabase pDb, OdDbObjectId shapeStyleId, OdDbObjectId txtStyleId)
    {
      /**********************************************************************/
      /* Continuous linetype                                                */
      /**********************************************************************/
      addLinetype(pDb, "Continuous2", "Solid Line");

      /**********************************************************************/
      /* Hidden linetype                                                    */
      /* This is not the standard Hidden linetype, but is used by examples  */
      /**********************************************************************/
      OdDbObjectId ltId = addLinetype(pDb, "Hidden", "- - - - - - - - - - - - - - - - - - - - -");
      OdDbLinetypeTableRecord pLt = (OdDbLinetypeTableRecord)ltId.safeOpenObject(OpenMode.kForWrite);
      pLt.setNumDashes(2);
      pLt.setPatternLength(0.1875);
      pLt.setDashLengthAt(0, 0.125);
      pLt.setDashLengthAt(1, -0.0625);

      /**********************************************************************/
      /* Linetype with text                                                 */
      /**********************************************************************/
      ltId = addLinetype(pDb, "HW_ODA", "__ HW __ OD __ HW __ OD __");
      pLt = (OdDbLinetypeTableRecord)ltId.safeOpenObject(OpenMode.kForWrite);
      pLt.setNumDashes(6);
      pLt.setPatternLength(1.8);
      pLt.setDashLengthAt(0, 0.5);
      pLt.setDashLengthAt(1, -0.2);
      pLt.setDashLengthAt(2, -0.2);
      pLt.setDashLengthAt(3, 0.5);
      pLt.setDashLengthAt(4, -0.2);
      pLt.setDashLengthAt(5, -0.2);

      pLt.setShapeStyleAt(1, txtStyleId);
      pLt.setShapeOffsetAt(1, new OdGeVector2d(-0.1, -0.05));
      pLt.setTextAt(1, "HW");
      pLt.setShapeScaleAt(1, 0.5);

      pLt.setShapeStyleAt(4, txtStyleId);
      pLt.setShapeOffsetAt(4, new OdGeVector2d(-0.1, -0.05));
      pLt.setTextAt(4, "OD");
      pLt.setShapeScaleAt(4, 0.5);

      /**********************************************************************/
      /* ZIGZAG linetype                                                    */
      /**********************************************************************/
      ltId = addLinetype(pDb, "ZigZag", "/\\/\\/\\/\\/\\/\\/\\/\\");
      pLt = (OdDbLinetypeTableRecord)ltId.safeOpenObject(OpenMode.kForWrite);
      pLt.setNumDashes(4);
      pLt.setPatternLength(0.8001);
      pLt.setDashLengthAt(0, 0.0001);
      pLt.setDashLengthAt(1, -0.2);
      pLt.setDashLengthAt(2, -0.4);
      pLt.setDashLengthAt(3, -0.2);

      pLt.setShapeStyleAt(1, shapeStyleId);
      pLt.setShapeOffsetAt(1, new OdGeVector2d(-0.2, 0.0));
      pLt.setShapeNumberAt(1, 131); //ZIG shape
      pLt.setShapeScaleAt(1, 0.2);

      pLt.setShapeStyleAt(2, shapeStyleId);
      pLt.setShapeOffsetAt(2, new OdGeVector2d(0.2, 0.0));
      pLt.setShapeNumberAt(2, 131); //ZIG shape
      pLt.setShapeScaleAt(2, 0.2);
      pLt.setShapeRotationAt(2, 3.1415926);
    }

    /************************************************************************/
    /* Add a block definition to the specified database                     */
    /*                                                                      */
    /* Note that the BlockTable and BlockTableRecord are implicitly closed  */
    /* when before this function returns.                                   */
    /************************************************************************/
    OdDbObjectId addBlock(OdDbDatabase pDb, string name)
    {
      OdDbBlockTable pTable = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject(OpenMode.kForWrite);
      OdDbBlockTableRecord pRecord = OdDbBlockTableRecord.createObject();

      /**********************************************************************/
      /* Block must have a name before adding it to the table.              */
      /**********************************************************************/
      pRecord.setName(name);

      /**********************************************************************/
      /* Add the record to the table.                                       */
      /**********************************************************************/
      OdDbObjectId id = pTable.add(pRecord);
      return id;
    }

    /************************************************************************/
    /* Add a block reference to the specified BlockTableRecord              */
    /************************************************************************/
    OdDbObjectId addInsert(OdDbBlockTableRecord bBTR, OdDbObjectId btrId, double xscale, double yscale)
    {
      /**********************************************************************/
      /* Add the block reference to the BlockTableRecord                    */
      /**********************************************************************/
      OdDbBlockReference pBlkRef = OdDbBlockReference.createObject();
      pBlkRef.setDatabaseDefaults(bBTR.database());
      OdDbObjectId brefId = bBTR.appendOdDbEntity(pBlkRef);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pBlkRef.setBlockTableRecord(btrId);
      pBlkRef.setScaleFactors(new OdGeScale3d(xscale, yscale, 1.0));
      return brefId;
    }

    /************************************************************************/
    /* Add a text entity with the specified attributes to the specified     */
    /* BlockTableRecord                                                     */
    /************************************************************************/
    OdDbObjectId addTextEnt(OdDbBlockTableRecord bBTR, OdGePoint3d position, OdGePoint3d ap,
       string str, double height, TextHorzMode hMode, TextVertMode vMode, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      return addTextEnt(bBTR, position, ap, str, height, hMode, vMode, layerId, styleId, null);
    }

    OdDbObjectId addTextEnt(OdDbBlockTableRecord bBTR,
                                    OdGePoint3d position,
                                    OdGePoint3d ap,
                                    string str,
                                    double height,
                                    TextHorzMode hMode,
                                    TextVertMode vMode,
                                    OdDbObjectId layerId,
                                    OdDbObjectId styleId,
                                    OdDbGroup pGroup)
    {
      /**********************************************************************/
      /* Create the text object                                             */
      /**********************************************************************/
      OdDbText pText = OdDbText.createObject();
      pText.setDatabaseDefaults(bBTR.database());
      OdDbObjectId textId = bBTR.appendOdDbEntity(pText);

      // Make the text annotative
      OdDbAnnotativeObjectPE.cast(pText).setAnnotative(pText, true);

      /**********************************************************************/
      /* Add the text to the specified group                                */
      /**********************************************************************/
      if (pGroup != null)
      {
        pGroup.append(textId);
      }

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pText.setPosition(position);
      pText.setAlignmentPoint(ap);
      pText.setHeight(height);
      pText.setWidthFactor(1.0);
      pText.setTextString(str);
      pText.setHorizontalMode(hMode);
      pText.setVerticalMode(vMode);

      /**********************************************************************/
      /* Set the text to the specified style                                */
      /**********************************************************************/
      if (!styleId.isNull())
      {
        pText.setTextStyle(styleId);
      }
      /**********************************************************************/
      /* Set the text to the specified layer                                */
      /**********************************************************************/
      if (!layerId.isNull())
      {
        pText.setLayer(layerId, false);
      }

      return textId;
    }

    /************************************************************************/
    /* Add a point entity with the specified attributes to the specified    */
    /* BlockTableRecord                                                     */
    /************************************************************************/
    OdDbObjectId addPointEnt(OdDbBlockTableRecord bBTR, OdGePoint3d point, OdDbObjectId layerId, OdDbGroup pGroup)
    {
      /**********************************************************************/
      /* Create the point object                                             */
      /**********************************************************************/
      OdDbPoint pPoint = OdDbPoint.createObject();
      pPoint.setDatabaseDefaults(bBTR.database());
      OdDbObjectId pointId = bBTR.appendOdDbEntity(pPoint);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pPoint.setPosition(point);

      /**********************************************************************/
      /* Add the point to the specified group                               */
      /**********************************************************************/
      if (pGroup != null)
      {
        pGroup.append(pointId);
      }
      /**********************************************************************/
      /* Set the point to the specified layer                               */
      /**********************************************************************/
      if (!layerId.isNull())
      {
        pPoint.setLayer(layerId, false);
      }
      return pointId;
    }

    /************************************************************************/
    /* Add some text entities to the specified BlockTableRecord             */
    /*                                                                      */
    /* The newly created entities are placed in a group                     */
    /************************************************************************/
    void addTextEnts(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      // We want to place all text items into a newly created group, so
      // open the group dictionary here.

      /**********************************************************************/
      /* Open the Group Dictionary                                          */
      /**********************************************************************/
      OdDbDictionary pGroupDic = (OdDbDictionary)btrId.database().getGroupDictionaryId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a new Group                                                 */
      /**********************************************************************/
      OdDbGroup pGroup = OdDbGroup.createObject();

      /**********************************************************************/
      /* Add it to the Group Dictionary                                     */
      /**********************************************************************/
      pGroupDic.setAt("OdaGroup", pGroup);

      /**********************************************************************/
      /* Set some properties                                                 */
      /**********************************************************************/
      pGroup.setName("OdaGroup");
      pGroup.setSelectable(true);

      /**********************************************************************/
      /* Get the Lower-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      double dx = w / 16.0;
      double dy = h / 12.0;

      double textHeight = EntityBoxes.getHeight() / 12.0;

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "TEXT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Add the text entities, and add them to the group                   */
      /*                                                                    */
      /* Show the relevant positions and alignment points                   */
      /**********************************************************************/
      OdGePoint3d position = point + new OdGeVector3d(dx, dy * 9.0, 0.0);
      addPointEnt(bBTR, position, layerId, pGroup);
      addTextEnt(bBTR, position, position,
        "Left Text", textHeight, TextHorzMode.kTextLeft, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      OdGePoint3d alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 9.0, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Center Text", textHeight, TextHorzMode.kTextCenter, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 9.0, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Right Text", textHeight, TextHorzMode.kTextRight, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 8.0, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Middle Text", textHeight, TextHorzMode.kTextMid, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      position = point + new OdGeVector3d(dx, dy * 1, 0.0);
      alignmentPoint = point + new OdGeVector3d(w - dx, dy, 0.0);
      addPointEnt(bBTR, position, layerId, pGroup);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, position, alignmentPoint,
        "Aligned Text", textHeight, TextHorzMode.kTextAlign, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      position = point + new OdGeVector3d(dx, dy * 5.5, 0.0);
      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 5.5, 0.0);
      addPointEnt(bBTR, position, layerId, pGroup);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, position, alignmentPoint,
        "Fit Text", textHeight, TextHorzMode.kTextFit, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);


      /**********************************************************************/
      /* Start a new box                                                    */
      /**********************************************************************/
      point = EntityBoxes.getBox(boxRow, boxCol + 1);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "TEXT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;
      textHeight = h / 16.0;

      /**********************************************************************/
      /* Create a new anonymous Group                                                 */
      /**********************************************************************/
      pGroup = OdDbGroup.createObject();

      /**********************************************************************/
      /* Add it to the Group Dictionary                                     */
      /**********************************************************************/
      pGroupDic.setAt("*", pGroup);

      /**********************************************************************/
      /* Set some properties                                                 */
      /**********************************************************************/
      pGroup.setName("*");
      pGroup.setAnonymous();
      pGroup.setSelectable(true);

      /**********************************************************************/
      /* Add the text entities, and add them to the group                   */
      /*                                                                    */
      /* Show the relevant positions and alignment points                   */
      /**********************************************************************/
      alignmentPoint = point + new OdGeVector3d(dx, dy * 9.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Top Left", textHeight, TextHorzMode.kTextLeft, TextVertMode.kTextTop, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 9.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Top Center", textHeight, TextHorzMode.kTextCenter, TextVertMode.kTextTop, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 9.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Top Right", textHeight, TextHorzMode.kTextRight, TextVertMode.kTextTop, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(dx, dy * 7.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Middle Left", textHeight, TextHorzMode.kTextLeft, TextVertMode.kTextVertMid, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 7.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Middle Center", textHeight, TextHorzMode.kTextCenter, TextVertMode.kTextVertMid, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 7.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Middle Right", textHeight, TextHorzMode.kTextRight, TextVertMode.kTextVertMid, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(dx, dy * 5.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Baseline Left", textHeight, TextHorzMode.kTextLeft, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 5.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Baseline Center", textHeight, TextHorzMode.kTextCenter, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 5.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Baseline Right", textHeight, TextHorzMode.kTextRight, TextVertMode.kTextBase, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(dx, dy * 3.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Bottom Left", textHeight, TextHorzMode.kTextLeft, TextVertMode.kTextBottom, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w / 2.0, dy * 3.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Bottom Center", textHeight, TextHorzMode.kTextCenter, TextVertMode.kTextBottom, OdDbObjectId.kNull, styleId, pGroup);

      alignmentPoint = point + new OdGeVector3d(w - dx, dy * 3.5, 0.0);
      addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
      addTextEnt(bBTR, alignmentPoint, alignmentPoint,
        "Bottom Right", textHeight, TextHorzMode.kTextRight, TextVertMode.kTextBottom, OdDbObjectId.kNull, styleId, pGroup);
    }

    /************************************************************************/
    /* Append a PolygonMesh vertex to the specified PolygonMesh             */
    /************************************************************************/
    void appendPgMeshVertex(OdDbPolygonMesh pPgMesh, OdGePoint3d pos)
    {
      /**********************************************************************/
      /* Append a Vertex to the PolyFaceMesh                                */
      /**********************************************************************/
      OdDbPolygonMeshVertex pVertex = OdDbPolygonMeshVertex.createObject();
      pPgMesh.appendVertex(pVertex);

      /**********************************************************************/
      /* Set the properties                                                 */
      /**********************************************************************/
      pVertex.setPosition(pos);
    }

    /************************************************************************/
    /* Append a PolyFaceMesh vertex to the specified PolyFaceMesh           */
    /************************************************************************/
    void appendPfMeshVertex(OdDbPolyFaceMesh pMesh, double x, double y, double z)
    {
      /**********************************************************************/
      /* Append a MeshVertex to the PolyFaceMesh                            */
      /**********************************************************************/
      OdDbPolyFaceMeshVertex pVertex = OdDbPolyFaceMeshVertex.createObject();
      pMesh.appendVertex(pVertex);

      /**********************************************************************/
      /* Set the properties                                                 */
      /**********************************************************************/
      pVertex.setPosition(new OdGePoint3d(x, y, z));
    }

    /************************************************************************/
    /* Append a FaceRecord to the specified PolyFaceMesh                    */
    /************************************************************************/
    void appendFaceRecord(OdDbPolyFaceMesh pMesh, Int16 i1, Int16 i2, Int16 i3, Int16 i4)
    {
      /**********************************************************************/
      /* Append a FaceRecord to the PolyFaceMesh                            */
      /**********************************************************************/
      OdDbFaceRecord pFr = OdDbFaceRecord.createObject();
      pMesh.appendFaceRecord(pFr);

      /**********************************************************************/
      /* Set the properties                                                 */
      /**********************************************************************/
      pFr.setVertexAt(0, i1);
      pFr.setVertexAt(1, i2);
      pFr.setVertexAt(2, i3);
      pFr.setVertexAt(3, i4);
    }
    public static double OdaToRadian(double deg) { return (deg) * Math.PI / 180.0; }
    /************************************************************************/
    /* Add an MLine Style to the specified database                         */
    /************************************************************************/
    OdDbObjectId addMLineStyle(OdDbDatabase pDb, string name, string desc)
    {
      /**********************************************************************/
      /* Open the MLineStyle dictionary                                     */
      /**********************************************************************/
      OdDbDictionary pMLDic = (OdDbDictionary)pDb.getMLStyleDictionaryId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create an Mline Style                                              */
      /**********************************************************************/
      OdDbMlineStyle pStyle = OdDbMlineStyle.createObject();
      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pStyle.setName(name);
      pStyle.setDescription(desc);
      pStyle.setStartAngle(OdaToRadian(105.0));
      pStyle.setEndAngle(OdaToRadian(75.0));
      pStyle.setShowMiters(true);
      pStyle.setStartSquareCap(true);
      pStyle.setEndSquareCap(true);

      /**********************************************************************/
      /* Get the object ID of the desired linetype                          */
      /**********************************************************************/
      OdDbLinetypeTable pLtTable = (OdDbLinetypeTable)pDb.getLinetypeTableId().safeOpenObject();
      OdDbObjectId linetypeId = pLtTable.getAt("Hidden");

      OdCmColor color = new OdCmColor();

      /**********************************************************************/
      /* Add some elements                                                  */
      /**********************************************************************/
      color.setRGB(255, 0, 0);
      pStyle.addElement(0.1, color, linetypeId);
      color.setRGB(0, 255, 0);
      pStyle.addElement(0.0, color, linetypeId);

      /**********************************************************************/
      /* Update the MLine dictionary                                        */
      /**********************************************************************/
      return pMLDic.setAt(name, pStyle);
    }
    /************************************************************************/
    /* Add a Material to the specified database                             */
    /************************************************************************/
    OdDbObjectId addMaterial(OdDbDatabase pDb, string name, string desc)
    {
      /**********************************************************************/
      /* Open the Material dictionary                                     */
      /**********************************************************************/
      OdDbDictionary pMatDic = (OdDbDictionary)pDb.getMaterialDictionaryId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a Material                                                  */
      /**********************************************************************/
      OdDbMaterial pMaterial = OdDbMaterial.createObject();
      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pMaterial.setName(name);
      pMaterial.setDescription(desc);

      OdGiMaterialColor materialColor = new OdGiMaterialColor();
      materialColor.setMethod(OdGiMaterialColor.Method.kOverride);
      materialColor.setFactor(0.75);
      materialColor.setColor(new OdCmEntityColor(192, 32, 255));

      OdGiMaterialMap materialMap = new OdGiMaterialMap();
      materialMap.setBlendFactor(0.0);
      materialMap.setSource(OdGiMaterialMap.Source.kFile);

      pMaterial.setAmbient(materialColor);
      pMaterial.setBump(materialMap);
      pMaterial.setDiffuse(materialColor, materialMap);
      pMaterial.setOpacity(1.0, materialMap);
      pMaterial.setReflection(materialMap);
      pMaterial.setRefraction(1.0, materialMap);
      pMaterial.setTranslucence(0.0);
      pMaterial.setSelfIllumination(0.0);
      pMaterial.setReflectivity(0.0);
      pMaterial.setMode(OdGiMaterialTraits.Mode.kRealistic);
      pMaterial.setChannelFlags(OdGiMaterialTraits.ChannelFlags.kNone);
      pMaterial.setIlluminationModel(OdGiMaterialTraits.IlluminationModel.kBlinnShader);

      materialColor.setFactor(1.0);
      materialColor.setColor(new OdCmEntityColor(255, 255, 255));
      pMaterial.setSpecular(materialColor, materialMap, 0.67);
      /**********************************************************************/
      /* Update the Material dictionary                                        */
      /**********************************************************************/
      return pMatDic.setAt(name, pMaterial);
    }
    /************************************************************************/
    /* Add some lines to the specified BlockTableRecord                     */
    /************************************************************************/
    void addLines(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {

      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "LINEs", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the center of the box                                          */
      /**********************************************************************/
      point = EntityBoxes.getBoxCenter(0, 0);

      /**********************************************************************/
      /* Add the lines that describe a 12 pointed star                      */
      /**********************************************************************/
      OdGeVector3d toStart = new OdGeVector3d(1.0, 0.0, 0.0);

      for (int i = 0; i < 12; i++)
      {
        OdDbLine pLine = OdDbLine.createObject();
        pLine.setDatabaseDefaults(bBTR.database());
        bBTR.appendOdDbEntity(pLine);
        pLine.setStartPoint(point + toStart);
        pLine.setEndPoint(point + toStart.rotateBy(OdaToRadian(160.0), OdGeVector3d.kZAxis));
      }
    }

    /************************************************************************/
    /* Add a 2D (heavy) polyline to the specified BlockTableRecord          */
    /************************************************************************/
    void add2dPolyline(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "2D POLYLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add a 2dPolyline to the database                                   */
      /**********************************************************************/
      OdDb2dPolyline pPline = OdDb2dPolyline.createObject();
      pPline.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pPline);

      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      OdGePoint3d pos = point;
      pos.y -= h;
      pos.x += w / 8;
      pos.y += h / 8;

      double[,] width = 
  {
    {0.0, w/12, w/4, 0.0},
    {w/4, w/12, 0.0, 0.0}
  };

      for (int i = 0; i < 4; i++)
      {
        OdDb2dVertex pVertex = OdDb2dVertex.createObject();
        pVertex.setDatabaseDefaults(bBTR.database());
        pPline.appendVertex(pVertex);
        pVertex.setPosition(pos);
        pos.x += w / 4.0;
        pos.y += h / 4.0;
        pVertex.setStartWidth(width[0, i]);
        pVertex.setEndWidth(width[1, i]);
      }
    }

    /************************************************************************/
    /* Add a 3D polyline to the specified BlockTableRecord                  */
    /************************************************************************/
    void add3dPolyline(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "3D POLYLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add a 3dPolyline to the database                                   */
      /**********************************************************************/
      OdDb3dPolyline pPline = OdDb3dPolyline.createObject();
      pPline.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pPline);

      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      OdGePoint3d pos = point;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);

      double radius = w * 3.0 / 8.0;
      double height = 0.0;
      double theta = 0.0;

      int turns = 4;
      int segs = 16;
      int points = segs * turns;

      double deltaR = radius / points;
      double deltaTheta = Math.PI * 2 / segs;
      double deltaH = 2 * radius / points;

      OdGeVector3d vec = new OdGeVector3d(radius, 0, 0);

      for (int i = 0; i < points; i++)
      {
        OdDb3dPolylineVertex pVertex = OdDb3dPolylineVertex.createObject();
        pVertex.setDatabaseDefaults(bBTR.database());
        pPline.appendVertex(pVertex);
        pVertex.setPosition(center + vec);

        radius -= deltaR;
        height += deltaH;
        theta += deltaTheta;
        vec = new OdGeVector3d(radius, 0, height).rotateBy(theta, OdGeVector3d.kZAxis);
      }
    }
    /************************************************************************/
    /* Add MText to the specified BlockTableRecord                          */
    /************************************************************************/
    void addMText(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "MTEXT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add MText to the database                                          */
      /**********************************************************************/
      OdDbMText pMText = OdDbMText.createObject();
      pMText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pMText);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pMText.setLocation(point + new OdGeVector3d(w / 8.0, -h * 2.0 / 8.0, 0));
      pMText.setTextHeight(0.4);
      pMText.setAttachment(OdDbMText.AttachmentPoint.kTopLeft);
      pMText.setContents("Sample {\\C1;MTEXT} created by {\\C5;OdWriteEx}");
      pMText.setWidth(w * 6.0 / 8.0);
      pMText.setTextStyle(styleId);


      /**********************************************************************/
      /* Add annotation scales                                              */
      /**********************************************************************/
      OdDbAnnotativeObjectPE.cast(pMText).setAnnotative(pMText, true);
      OdDbObjectContextCollection contextCollection = bBTR.database().objectContextManager().contextCollection("ACDB_ANNOTATIONSCALES");
      // OdString cast is necessary to avoid incorrect char pointer interpretation as an OdDbObjectContextId (that is essentially void*)
      OdDbObjectContext scale = contextCollection.getContext("1:2");
      if (scale != null)
        OdDbObjectContextInterface.cast(pMText).addContext(pMText, scale);
    }

    /************************************************************************/
    /* Add a Block Reference to the specified BlockTableRecord              */
    /************************************************************************/
    void addBlockRef(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId, OdDbObjectId insertId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double      w     = EntityBoxes.getWidth(boxRow, boxCol);
      //  double      h     = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "INSERT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Insert the Block                                                   */
      /**********************************************************************/
      OdDbObjectId bklRefId = addInsert(bBTR, insertId, 1.0, 1.0);

      /**********************************************************************/
      /* Open the insert                                                    */
      /**********************************************************************/
      OdDbBlockReference pBlkRef = (OdDbBlockReference)bklRefId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a transformation matrix for the block and attributes        */
      /**********************************************************************/
      OdGePoint3d insPoint = EntityBoxes.getBoxCenter(boxRow, boxCol);
      OdGeMatrix3d blkXfm = new OdGeMatrix3d();
      blkXfm.setTranslation(insPoint.asVector());
      pBlkRef.transformBy(blkXfm);

      /**********************************************************************/
      /* Scan the block definition for non-constant attribute definitions   */
      /* and use them as templates for attributes                           */
      /**********************************************************************/
      OdDbBlockTableRecord pBlockDef = (OdDbBlockTableRecord)insertId.safeOpenObject();
      OdDbObjectIterator pIter = pBlockDef.newIterator();
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        OdDbEntity pEntity = pIter.entity();
        OdDbAttributeDefinition pAttDef = OdDbAttributeDefinition.cast(pEntity);
        if (pAttDef != null && !pAttDef.isConstant())
        {
          OdDbAttribute pAtt = OdDbAttribute.createObject();
          pAtt.setDatabaseDefaults(bBTR.database());
          pBlkRef.appendAttribute(pAtt);
          pAtt.setPropertiesFrom(pAttDef, false);
          pAtt.setAlignmentPoint(pAttDef.alignmentPoint());
          pAtt.setHeight(pAttDef.height());
          pAtt.setHorizontalMode(pAttDef.horizontalMode());
          pAtt.setNormal(pAttDef.normal());
          pAtt.setOblique(pAttDef.oblique());
          pAtt.setPosition(pAttDef.position());
          pAtt.setRotation(pAttDef.rotation());
          pAtt.setTextString(pAttDef.textString());
          pAtt.setTextStyle(pAttDef.textStyle());
          pAtt.setWidthFactor(pAttDef.widthFactor());

          /******************************************************************/
          /* Specify a new value for the attribute                          */
          /******************************************************************/
          pAtt.setTextString("The Value");

          /******************************************************************/
          /* Transform it as the block was transformed                      */
          /******************************************************************/
          pAtt.transformBy(blkXfm);
        }
      }
    }
    /************************************************************************/
    /* Add a MInsert to the specified BlockTableRecord                      */
    /************************************************************************/
    void addMInsert(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId, OdDbObjectId insertId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "MInsert", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Add MInsert to the database                                        */
      /**********************************************************************/
      OdDbMInsertBlock pMInsert = OdDbMInsertBlock.createObject();
      pMInsert.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pMInsert);

      /**********************************************************************/
      /* Set some Parameters                                                */
      /**********************************************************************/
      pMInsert.setBlockTableRecord(insertId);
      OdGePoint3d insPnt = point + new OdGeVector3d(w * 2.0 / 8.0, h * 2.0 / 8.0, 0.0);
      pMInsert.setPosition(insPnt);
      pMInsert.setScaleFactors(new OdGeScale3d(2.0 / 8.0));
      pMInsert.setRows(2);
      pMInsert.setColumns(3);
      pMInsert.setRowSpacing(h * 4.0 / 8.0);
      pMInsert.setColumnSpacing(w * 2.0 / 8.0);
    }

    /************************************************************************/
    /* Add a PolyFaceMesh to the specified BlockTableRecord                 */
    /************************************************************************/
    void addPolyFaceMesh(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "PolyFaceMesh", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add a PolyFaceMesh to the database                                 */
      /**********************************************************************/
      OdDbPolyFaceMesh pMesh = OdDbPolyFaceMesh.createObject();
      pMesh.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pMesh);


      /**********************************************************************/
      /* Add the faces and vertices that define a pup tent                  */
      /**********************************************************************/

      double dx = w * 3.0 / 8.0;
      double dy = h * 3.0 / 8.0;
      double dz = dy;

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);

      appendPfMeshVertex(pMesh, center.x + dx, center.y + dy, 0);
      appendPfMeshVertex(pMesh, center.x + 0, center.y + dy, center.z + dz);
      appendPfMeshVertex(pMesh, center.x - dx, center.y + dy, 0);
      appendPfMeshVertex(pMesh, center.x - dx, center.y - dy, 0);
      appendPfMeshVertex(pMesh, center.x + 0, center.y - dy, center.z + dz);
      appendPfMeshVertex(pMesh, center.x + dx, center.y - dy, 0);

      appendFaceRecord(pMesh, 1, 2, 5, 6);
      appendFaceRecord(pMesh, 2, 3, 4, 5);
      appendFaceRecord(pMesh, 6, 5, 4, 0);
      appendFaceRecord(pMesh, 3, 2, 1, 0);
    }

    /************************************************************************/
    /* Add PolygonMesh to the specified BlockTableRecord                    */
    /************************************************************************/
    void addPolygonMesh(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "PolygonMesh", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add a PolygonMesh to the database                                 */
      /**********************************************************************/
      OdDbPolygonMesh pMesh = OdDbPolygonMesh.createObject();
      pMesh.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pMesh);

      /**********************************************************************/
      /* Define the size of the mesh                                        */
      /**********************************************************************/
      Int16 mSize = 16, nSize = 4;
      pMesh.setMSize(mSize);
      pMesh.setNSize(nSize);


      /**********************************************************************/
      /* Define a profile                                                   */
      /**********************************************************************/
      double dx = w * 3.0 / 8.0;
      double dy = h * 3.0 / 8.0;

      OdGeVector3dArray vectors = new OdGeVector3dArray();
      vectors.Add(new OdGeVector3d(0, -dy, 0));
      vectors.Add(new OdGeVector3d(dx, -dy, 0));
      vectors.Add(new OdGeVector3d(dx, dy, 0));
      vectors.Add(new OdGeVector3d(0, dy, 0));

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);

      /**********************************************************************/
      /* Append the vertices to the mesh                                    */
      /**********************************************************************/
      for (int i = 0; i < mSize; i++)
      {
        for (int j = 0; j < nSize; j++)
        {
          appendPgMeshVertex(pMesh, center + vectors[j]);
          vectors[j].rotateBy(OdaToRadian(360.0 / mSize), OdGeVector3d.kYAxis);
        }
      }
      pMesh.makeMClosed();
    }

    /************************************************************************/
    /* Add some curves to the specified BlockTableRecord                    */
    /************************************************************************/
    void addCurves(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Create a Circle                                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pCircle);

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      center.x -= w * 2.5 / 8.0;
      pCircle.setCenter(center);
      pCircle.setRadius(w * 1.0 / 8.0);

      /**********************************************************************/
      /* Add a Hyperlink to the Circle                                      */
      /**********************************************************************/
      OdDbEntityHyperlinkPE hpe = OdDbEntityHyperlinkPE.cast(pCircle);
      OdDbHyperlinkCollection urls = hpe.getHyperlinkCollection(pCircle);

      urls.addTail("http://forum.opendesign.com/forumdisplay.php?s=&forumid=17",
        "Open Design Alliance Forum > Teigha, C++ version");

      hpe.setHyperlinkCollection(pCircle, urls);

      if (!hpe.hasHyperlink(pCircle))
        throw new OdError("Hyperlinks are broken");

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      double textY = point.y - m_textSize / 2.0;

      addTextEnt(bBTR,
        new OdGePoint3d(center.x, textY, 0), new OdGePoint3d(center.x, textY, 0),
        "CIRCLE", m_textSize, TextHorzMode.kTextCenter, TextVertMode.kTextTop, layerId, styleId);
      addTextEnt(bBTR,
        new OdGePoint3d(center.x, textY - 1.6 * m_textSize, 0), new OdGePoint3d(center.x, textY - 1.6 * m_textSize, 0),
        "w/Hyperlink", m_textSize, TextHorzMode.kTextCenter, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create an Arc                                                      */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pArc);

      pArc.setRadius(w * 1.0 / 8.0);

      center = EntityBoxes.getBoxCenter(boxRow, boxCol);

      center.y += pArc.radius() / 2.0;

      pArc.setCenter(center);
      pArc.setStartAngle(OdaToRadian(0.0));
      pArc.setEndAngle(OdaToRadian(180.0));

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR,
        new OdGePoint3d(center.x, textY, 0), new OdGePoint3d(center.x, textY, 0),
        "ARC", m_textSize, TextHorzMode.kTextCenter, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add an Ellipse                                                     */
      /**********************************************************************/
      OdDbEllipse pEllipse = OdDbEllipse.createObject();
      pEllipse.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pEllipse);

      double majorRadius = w * 1.0 / 8.0;
      double radiusRatio = 0.25;

      center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      center.x += w * 2.5 / 8.0;
      center.y += majorRadius;

      OdGeVector3d majorAxis = new OdGeVector3d(majorRadius, 0.0, 0.0);
      majorAxis.rotateBy(OdaToRadian(30.0), OdGeVector3d.kZAxis);

      pEllipse.set(center, OdGeVector3d.kZAxis, majorAxis, radiusRatio);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR,
        new OdGePoint3d(center.x, textY, 0), new OdGePoint3d(center.x, textY, 0),

        "ELLIPSE", m_textSize, TextHorzMode.kTextCenter, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Add a Point                                                        */
      /**********************************************************************/
      OdDbPoint pPoint = OdDbPoint.createObject();
      pPoint.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pPoint);

      center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      center.y -= h * 1.0 / 8.0;

      pPoint.setPosition(center);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      center.y += h * 1.0 / 8.0;
      addTextEnt(bBTR, center, center,
        "POINT", m_textSize, TextHorzMode.kTextCenter, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Set the point display mode so we can see it                        */
      /**********************************************************************/
      pDb.database().setPDMODE(3);
      pDb.database().setPDSIZE(0.1);
    }
    /************************************************************************/
    /* Add a tolerance to the specified BlockTableRecord                    */
    /************************************************************************/
    void addTolerance(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "TOLERANCE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);


      /**********************************************************************/
      /* Add a Frame Control Feature (Tolerance) to the database            */
      /**********************************************************************/
      OdDbFcf pTol = OdDbFcf.createObject();
      pTol.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pTol);

      /**********************************************************************/
      /* Set the properties                                                 */
      /**********************************************************************/
      pTol.setDatabaseDefaults(pDb);
      point.x += w / 6.0;
      point.y -= h / 4.0;
      pTol.setLocation(point);
      pTol.setText("{\\Fgdt;r}%%v{\\Fgdt;n}3.2{\\Fgdt;m}%%v%%v%%v%%v");
    }

    /************************************************************************/
    /* Add some leaders the specified BlockTableRecord                      */
    /************************************************************************/
    void addLeaders(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {

      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "LEADERs", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Define an annotation block -- A circle with radius 0.5             */
      /**********************************************************************/
      OdDbBlockTable pBlocks = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject(OpenMode.kForWrite);
      OdDbBlockTableRecord pAnnoBlock = OdDbBlockTableRecord.createObject();
      pAnnoBlock.setName("AnnoBlock");
      OdDbObjectId annoBlockId = pBlocks.add(pAnnoBlock);
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());
      pAnnoBlock.appendOdDbEntity(pCircle);
      OdGePoint3d center = new OdGePoint3d(0.5, 0, 0);
      pCircle.setCenter(center);
      pCircle.setRadius(0.5);

      /**********************************************************************/
      /* Add a leader with database defaults to the database                */
      /**********************************************************************/
      OdDbLeader pLeader = OdDbLeader.createObject();
      pLeader.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pLeader);
      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      point.x += w * 1.0 / 8.0;
      point.y -= h * 3.0 / 8.0;
      pLeader.appendVertex(point);
      point.x += w * 2.0 / 8.0;
      point.y += h * 1.0 / 8.0;
      pLeader.appendVertex(point);

      /**********************************************************************/
      /* Insert the annotation                                              */
      /**********************************************************************/
      OdDbBlockReference pBlkRef = OdDbBlockReference.createObject();
      OdDbObjectId blkRefId = bBTR.appendOdDbEntity(pBlkRef);
      pBlkRef.setBlockTableRecord(annoBlockId);
      pBlkRef.setPosition(point);
      pBlkRef.setScaleFactors(new OdGeScale3d(0.375, 0.375, 0.375));

      /**********************************************************************/
      /* Attach the Block Reference as annotation to the Leader             */
      /**********************************************************************/
      pLeader.attachAnnotation(blkRefId);

      /**********************************************************************/
      /* Add a leader with database defaults to the database                */
      /**********************************************************************/
      pLeader = OdDbLeader.createObject();
      pLeader.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pLeader);

      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      point = EntityBoxes.getBox(boxRow, boxCol);
      point.x += w * 1.0 / 8.0;
      point.y -= h * 5.0 / 8.0;
      pLeader.appendVertex(point);
      point.x += w * 1.0 / 8.0;
      point.y += h * 1.0 / 8.0;
      pLeader.appendVertex(point);
      point.x += w * 1.0 / 8;

      /**********************************************************************/
      /* Set the arrowhead                                                  */
      /**********************************************************************/
      pLeader.setDimldrblk("DOT");

      /**********************************************************************/
      /* Create MText at a 30° angle                                        */
      /**********************************************************************/
      OdDbMText pMText = OdDbMText.createObject();
      pMText.setDatabaseDefaults(pDb);
      OdDbObjectId mTextId = bBTR.appendOdDbEntity(pMText);
      const double textHeight = 0.15;
      const double textWidth = 1.0;
      pMText.setLocation(point);
      pMText.setRotation(OdaToRadian(10.0));
      pMText.setTextHeight(textHeight);
      pMText.setWidth(textWidth);
      pMText.setAttachment(OdDbMText.AttachmentPoint.kMiddleLeft);
      pMText.setContents("MText");
      pMText.setTextStyle(styleId);

      /**********************************************************************/
      /* Set a background color                                             */
      /**********************************************************************/
      OdCmColor cBackground = new OdCmColor();
      cBackground.setRGB(255, 255, 0); // Yellow
      pMText.setBackgroundFillColor(cBackground);
      pMText.setBackgroundFill(true);
      pMText.setBackgroundScaleFactor(2.0);

      /**********************************************************************/
      /* Attach the MText as annotation to the Leader                       */
      /**********************************************************************/
      pLeader.attachAnnotation(mTextId);

      /**********************************************************************/
      /* Add a leader with database defaults to the database                */
      /**********************************************************************/
      pLeader = OdDbLeader.createObject();
      bBTR.appendOdDbEntity(pLeader);
      pLeader.setDatabaseDefaults(pDb);


      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      point = EntityBoxes.getBox(boxRow, boxCol);
      point.x += w * 1.0 / 8.0;
      point.y -= h * 7.0 / 8.0;
      pLeader.appendVertex(point);
      point.x += w * 1.0 / 8.0;
      point.y += h * 1.0 / 8.0;
      pLeader.appendVertex(point);

      /**********************************************************************/
      /* Create a Frame Control Feature (Tolerance)                         */
      /**********************************************************************/
      OdDbFcf pTol = OdDbFcf.createObject();
      pTol.setDatabaseDefaults(pDb);
      pTol.setLocation(point);
      pTol.setText("{\\Fgdt;r}%%v{\\Fgdt;n}3.2{\\Fgdt;m}%%v%%v%%v%%v");

      /**********************************************************************/
      /* Attach the FCF as annotation to the Leader                         */
      /**********************************************************************/
      pLeader.attachAnnotation(bBTR.appendOdDbEntity(pTol));
    }


    /************************************************************************/
    /* Add some MLeaders the specified BlockTableRecord                     */
    /************************************************************************/
    void addMLeaders(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      int llIndex;

      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(
          bBTR,
          point + m_textOffset, point + m_textOffset,
          "MLeaders",
          m_textSize,
          TextHorzMode.kTextLeft, TextVertMode.kTextTop,
          layerId, styleId);

      /**********************************************************************/
      /* Add a MLeader with database defaults to the database               */
      /**********************************************************************/
      OdDbMLeader pMLeader = OdDbMLeader.createObject();
      pMLeader.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pMLeader);


      /**********************************************************************/
      /* Add the vertices                                                   */
      /**********************************************************************/
      OdDbMText pMText = OdDbMText.createObject();
      pMText.setDatabaseDefaults(pDb);
      pMLeader.setEnableFrameText(true);
      pMText.setContents("MText");

      double textHeight = 0.15;
      double textWidth = 1.0;

      point.x += w * 3.0 / 8.0;
      point.y -= h * 1.0 / 6.0;
      pMText.setLocation(point);
      pMText.setTextHeight(textHeight);
      pMText.setWidth(textWidth);
      pMText.setAttachment(OdDbMText.AttachmentPoint.kMiddleLeft);
      pMText.setTextStyle(styleId);
      pMLeader.setMText(pMText);
      pMLeader.setDoglegLength(0.18);

      point.x -= w * 2.0 / 8.0;
      point.y -= h * 1.0 / 8.0;
      pMLeader.addLeaderLine(point, out llIndex);
      point.x += w * 1.0 / 8.0;
      pMLeader.addLeaderLine(point, out llIndex);
      point.x += w * 1.0 / 6.0;
      point.y -= h * 1.0 / 8.0;
      pMLeader.addFirstVertex(llIndex, point);

      point.x += w * 3.0 / 8.0;
      point.y -= h * 1.0 / 8.0;
      pMLeader.addLeaderLine(point, out llIndex);

      /**********************************************************************/
      /* Add block with attribute for second MLeader contents               */
      /**********************************************************************/
      OdDbObjectId contentBlockId;
      {
        OdDbBlockTable pBlocks = OdDbBlockTable.cast(pDb.getBlockTableId().openObject(OpenMode.kForWrite));
        OdDbBlockTableRecord pBlock = OdDbBlockTableRecord.createObject();
        pBlock.setName("AnnoBlock2");
        contentBlockId = pBlocks.add(pBlock);
        OdDbCircle pCircle = OdDbCircle.createObject();
        pCircle.setDatabaseDefaults(pDb);
        pBlock.appendOdDbEntity(pCircle);
        pCircle.setCenter(OdGePoint3d.kOrigin);
        pCircle.setRadius(0.5);
        OdDbAttributeDefinition pAttr = OdDbAttributeDefinition.createObject();
        pAttr.setDatabaseDefaults(pDb);
        pBlock.appendOdDbEntity(pAttr);
        pAttr.setPrompt("Enter Attr: ");
        pAttr.setTag("Attr");
        pAttr.setHorizontalMode(TextHorzMode.kTextCenter);
        pAttr.setHeight(0.2);
        pAttr.setTextString("Block");
      }

      /**********************************************************************/
      /* Add block for second MLeader arrow heads                           */
      /**********************************************************************/
      OdDbObjectId arrowBlockId;
      {
        OdDbBlockTable pBlocks = OdDbBlockTable.cast(pDb.getBlockTableId().openObject(OpenMode.kForWrite));
        OdDbBlockTableRecord pBlock = OdDbBlockTableRecord.createObject();
        pBlock.setName("ArrowBlock2");
        pBlock.setComments("Block for MLeader arrow heads.");
        arrowBlockId = pBlocks.add(pBlock);
        OdDbPolyline pPl = OdDbPolyline.createObject();
        pPl.setDatabaseDefaults(pDb);
        pBlock.appendOdDbEntity(pPl);
        pPl.addVertexAt(0, new OdGePoint2d(0.0, -0.5));
        pPl.addVertexAt(1, new OdGePoint2d(0.5, 0.0));
        pPl.addVertexAt(2, new OdGePoint2d(0.0, 0.5));
        pPl.addVertexAt(3, new OdGePoint2d(-0.5, 0.0));
        pPl.addVertexAt(4, new OdGePoint2d(0.0, -0.5));
        pPl.transformBy(OdGeMatrix3d.scaling(2.0));
      }

      /**********************************************************************/
      /* Create style for second MLeader                                    */
      /**********************************************************************/
      pMLeader = OdDbMLeader.createObject();
      OdDbObjectId styleId2;
      {
        OdDbDictionary pMLeaderStyleDic = (OdDbDictionary)pDb.getMLeaderStyleDictionaryId().safeOpenObject(OpenMode.kForWrite);
        OdDbMLeaderStyle pMLeaderStyle = OdDbMLeaderStyle.createObject();
        pMLeaderStyle.setDatabaseDefaults(pDb);
        pMLeaderStyle.setContentType(OdDbMLeaderStyle.ContentType.kBlockContent);
        styleId2 = pMLeaderStyleDic.setAt("BlockMLeaderStyle2", pMLeaderStyle);

        pMLeaderStyle.setBlockId(contentBlockId);
        pMLeaderStyle.setBlockScale(new OdGeScale3d(0.5, 0.5, 1.0));
        pMLeaderStyle.setArrowSymbolId(arrowBlockId);
        pMLeaderStyle.setDoglegLength(0.1);
        pMLeaderStyle.setLandingGap(0.0);
        //pMLeaderStyle.setArrowSize(0.5);
      }

      /**********************************************************************/
      /* Set second MLeader vertices                                        */
      /**********************************************************************/
      pMLeader.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pMLeader);
      pMLeader.setMLeaderStyle(styleId2);
      pMLeader.setBlockPosition(new OdGePoint3d(12.2, 1.0, 0.0));

      pMLeader.addLeaderLine(new OdGePoint3d(12.0, 1.5, 0.0), out llIndex);
      pMLeader.addFirstVertex(llIndex, new OdGePoint3d(12.7, 1.9, 0.0));
      pMLeader.addFirstVertex(llIndex, new OdGePoint3d(12.3, 2.3, 0.0));

      pMLeader.addLeaderLine(new OdGePoint3d(13.0, 1.65, 0.0), out llIndex);
      pMLeader.addFirstVertex(llIndex, new OdGePoint3d(13.0, 2.3, 0.0));

      pMLeader.setArrowSize(pMLeader.arrowSize() * 0.5);
    }

    /************************************************************************/
    /* Add some lights to the specified BlockTableRecord                    */
    /************************************************************************/
    void addLights(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);

      /**********************************************************************/
      /* Create a Light                                                     */
      /**********************************************************************/
      OdDbLight pLight = OdDbLight.createObject();
      pLight.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pLight);

      OdGePoint3d ptLight = EntityBoxes.getBoxCenter(boxRow, boxCol);
      pLight.setPosition(ptLight);
      pLight.setLightType(OdGiDrawable.DrawableType.kPointLight);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Light", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
    }

    /************************************************************************/
    /* Add some SubDMeshes to the specified BlockTableRecord                    */
    /************************************************************************/
    void addSubDMeshes(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Create a SubDMesh                                                     */
      /**********************************************************************/
      OdDbSubDMesh pSubDMesh = OdDbSubDMesh.createObject();
      pSubDMesh.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pSubDMesh);

      OdInt32Array faceArray;
      OdGeExtents3d ext;
      OdGePoint3dArray vertexArray;
      DbSubDMeshData.set(out vertexArray, out faceArray, out ext);
      pSubDMesh.setSubDMesh(vertexArray, faceArray, 0);

      double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
      double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
      OdGeMatrix3d xfm = OdGeMatrix3d.scaling(Math.Min(scaleX, scaleY), ext.center());
      pSubDMesh.transformBy(xfm);
      xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
      pSubDMesh.transformBy(xfm);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "SubDMesh", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
    }

    /************************************************************************/
    /* Add some ExtrudedSurfaces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addExtrudedSurfaces(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      // base curve 
      OdDbEllipse ellipse = OdDbEllipse.createObject();
      ellipse.set(new OdGePoint3d(0.0, 0.0, 0.0), new OdGeVector3d(0.0, 0.0, 1.0), new OdGeVector3d(2.0, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Create extruded surface                                                     */
      /**********************************************************************/
      try
      {
        OdStreamBuf pFile = TD_Db.odSystemServices().createFile("extrudedsurf.sat");
        OdDbExtrudedSurface pExtruded = OdDbExtrudedSurface.createObject();
        OdDbSweepOptions options = new OdDbSweepOptions();
        pExtruded.createExtrudedSurface(ellipse, new OdGeVector3d(0.0, 1.0, 3.0), options, pFile);
        pExtruded.setDatabaseDefaults(bBTR.database());
        bBTR.appendOdDbEntity(pExtruded);
        OdGeExtents3d ext = new OdGeExtents3d();
        pExtruded.getGeomExtents(ext);
        OdGeMatrix3d xfm = new OdGeMatrix3d();
        xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
        pExtruded.transformBy(xfm);
        double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
        double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
        xfm.setToScaling(Math.Min(scaleX, scaleY), EntityBoxes.getBoxCenter(boxRow, boxCol));
        pExtruded.transformBy(xfm);

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR,
          point + m_textOffset, point + m_textOffset,
          "Extruded surf", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
      }
      catch (OdError)
      {
        // just skip entity creation
      }
    }

    /************************************************************************/
    /* Add some RevolvedSurfaces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addRevolvedSurfaces(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      // base curve 
      OdDbEllipse ellipse = OdDbEllipse.createObject();
      ellipse.set(new OdGePoint3d(0.0, 0.0, 0.0), new OdGeVector3d(0.0, 0.0, 1.0), new OdGeVector3d(2.0, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Create revolved surface                                                     */
      /**********************************************************************/
      try
      {
        OdStreamBuf pFile = TD_Db.odSystemServices().createFile("revolvedsurf.sat");
        OdDbRevolvedSurface pRevolved = OdDbRevolvedSurface.createObject();
        OdDbRevolveOptions options = new OdDbRevolveOptions();
        pRevolved.createRevolvedSurface(ellipse, new OdGePoint3d(5, 0, 0), new OdGeVector3d(0, 1, 0), 3.14, 0, options, pFile);
        pRevolved.setDatabaseDefaults(bBTR.database());
        bBTR.appendOdDbEntity(pRevolved);
        OdGeExtents3d ext = new OdGeExtents3d();
        pRevolved.getGeomExtents(ext);
        OdGeMatrix3d xfm = new OdGeMatrix3d();
        xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
        pRevolved.transformBy(xfm);
        double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
        double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
        xfm.setToScaling(Math.Min(scaleX, scaleY), EntityBoxes.getBoxCenter(boxRow, boxCol));
        pRevolved.transformBy(xfm);

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR,
          point + m_textOffset, point + m_textOffset,
          "Revolved surf", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
      }
      catch (OdError)
      {
        // just skip entity creation
      }
    }

    /************************************************************************/
    /* Add some PlaneSurfaces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addPlaneSurfaces(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      // base curve 
      OdDbEllipse ellipse = OdDbEllipse.createObject();
      ellipse.set(new OdGePoint3d(0.0, 0.0, 0.0), new OdGeVector3d(0.0, 0.0, 1.0), new OdGeVector3d(2.0, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Create plane surface                                                     */
      /**********************************************************************/
      OdDbPlaneSurface pPlane = OdDbPlaneSurface.createObject();
      OdRxObjectPtrArray curveSegments = new OdRxObjectPtrArray();
      curveSegments.Add(ellipse);
      OdRxObjectPtrArray regions = new OdRxObjectPtrArray();
      OdDbRegion.createFromCurves(curveSegments, regions);
      pPlane.createFromRegion(OdDbRegion.cast(regions[0]));
      pPlane.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pPlane);
      OdGeExtents3d ext = new OdGeExtents3d();
      pPlane.getGeomExtents(ext);
      OdGeMatrix3d xfm = new OdGeMatrix3d();
      xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
      pPlane.transformBy(xfm);
      double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
      double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
      xfm.setToScaling(Math.Min(scaleX, scaleY), EntityBoxes.getBoxCenter(boxRow, boxCol));
      pPlane.transformBy(xfm);

      /**********************************************************************/
      /* Add a label                                                        */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Plane surf", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
    }

    /************************************************************************/
    /* Add some LoftedSurfaces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addLoftedSurfaces(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      // base curve 
      OdDbEllipse ellipse = OdDbEllipse.createObject();
      ellipse.set(new OdGePoint3d(0.0, 0.0, 0.0), new OdGeVector3d(0.0, 0.0, 1.0), new OdGeVector3d(2.0, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Create lofted surface                                                     */
      /**********************************************************************/
      try
      {
        OdStreamBuf pFile = TD_Db.odSystemServices().createFile("loftedsurf.sat");
        OdDbLoftedSurface pLofted = OdDbLoftedSurface.createObject();
        OdDbEntityPtrArray crossSectionCurves = new OdDbEntityPtrArray();
        crossSectionCurves.Add(ellipse);
        OdGeMatrix3d mat = new OdGeMatrix3d();
        mat.setToScaling(0.5);
        OdDbEntity e;
        ellipse.getTransformedCopy(mat, out e);
        crossSectionCurves.Add(e);
        mat.setToTranslation(new OdGeVector3d(0.0, 0.0, 3.0));
        crossSectionCurves[1].transformBy(mat);
        OdDbEntityPtrArray guideCurves = new OdDbEntityPtrArray();
        OdDbLoftOptions options = new OdDbLoftOptions();
        pLofted.createLoftedSurface(crossSectionCurves, guideCurves, null, options, pFile);
        pLofted.setDatabaseDefaults(bBTR.database());
        bBTR.appendOdDbEntity(pLofted);
        OdGeExtents3d ext = new OdGeExtents3d();
        pLofted.getGeomExtents(ext);
        OdGeMatrix3d xfm = new OdGeMatrix3d();
        xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
        pLofted.transformBy(xfm);
        double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
        double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
        xfm.setToScaling(Math.Min(scaleX, scaleY), EntityBoxes.getBoxCenter(boxRow, boxCol));
        pLofted.transformBy(xfm);

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR,
          point + m_textOffset, point + m_textOffset,
          "Lofted surf", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
      }
      catch (OdError)
      {
        // just skip entity creation
      }
    }

    /************************************************************************/
    /* Add some SweptSurfaces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addSweptSurfaces(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the origin and size of the box                                 */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      // base curve 
      OdDbEllipse ellipse = OdDbEllipse.createObject();
      ellipse.set(new OdGePoint3d(0.0, 0.0, 0.0), new OdGeVector3d(0.0, 0.0, 1.0), new OdGeVector3d(2.0, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Create swept surface                                                     */
      /**********************************************************************/
      try
      {
        OdStreamBuf pFile = TD_Db.odSystemServices().createFile("sweptsurf.sat");
        OdDbSweptSurface pSwept = OdDbSweptSurface.createObject();
        OdDbSweepOptions options = new OdDbSweepOptions();
        OdDb3dPolylineVertex[] aVx = { OdDb3dPolylineVertex.createObject(), OdDb3dPolylineVertex.createObject(), OdDb3dPolylineVertex.createObject() };
        aVx[0].setPosition(new OdGePoint3d(0.0, 0.0, 0.0));
        aVx[1].setPosition(new OdGePoint3d(0.5, 0.0, 2.0));
        aVx[2].setPosition(new OdGePoint3d(-0.5, 0.0, 4.0));
        OdDb3dPolyline poly = OdDb3dPolyline.createObject();
        poly.appendVertex(aVx[0]);
        poly.appendVertex(aVx[1]);
        poly.appendVertex(aVx[2]);
        pSwept.createSweptSurface(ellipse, poly, options, pFile);
        pSwept.setDatabaseDefaults(bBTR.database());
        bBTR.appendOdDbEntity(pSwept);
        OdGeExtents3d ext = new OdGeExtents3d();
        pSwept.getGeomExtents(ext);
        OdGeMatrix3d xfm = new OdGeMatrix3d();
        xfm.setToTranslation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector() - ext.center().asVector());
        pSwept.transformBy(xfm);
        double scaleX = w * 0.7 / (ext.maxPoint().x - ext.minPoint().x);
        double scaleY = h * 0.7 / (ext.maxPoint().y - ext.minPoint().y);
        xfm.setToScaling(Math.Min(scaleX, scaleY), EntityBoxes.getBoxCenter(boxRow, boxCol));
        pSwept.transformBy(xfm);

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR,
          point + m_textOffset, point + m_textOffset,
          "Swept surf", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
      }
      catch (OdError)
      {
        // just skip entity creation
      }
    }

    /************************************************************************/
    /* Add a Block Definition to the specified database                     */
    /************************************************************************/
    OdDbObjectId addBlockDef(OdDbDatabase pDb, string name, int boxRow, int boxCol)
    {
      /**********************************************************************/
      /* Open the block table                                               */
      /**********************************************************************/
      OdDbBlockTable pBlocks = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = OdDbBlockTableRecord.createObject();

      /**********************************************************************/
      /* Block must have a name before adding it to the table.              */
      /**********************************************************************/
      bBTR.setName(name);

      /**********************************************************************/
      /* Add the record to the table.                                       */
      /**********************************************************************/
      OdDbObjectId btrId = pBlocks.add(bBTR);
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add a Circle                                                       */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pCircle);

      OdGePoint3d center = new OdGePoint3d(0, 0, 0);
      center.x -= w * 2.5 / 8.0;

      pCircle.setCenter(center);
      pCircle.setRadius(w * 1.0 / 8.0);

      /**********************************************************************/
      /* Add an Arc                                                         */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pArc);

      pArc.setRadius(w * 1.0 / 8.0);

      center = new OdGePoint3d(0, 0, 0);
      center.y -= pArc.radius() / 2.0;

      pArc.setCenter(center);
      pArc.setStartAngle(OdaToRadian(0.0));
      pArc.setEndAngle(OdaToRadian(180.0));

      /**********************************************************************/
      /* Add an Ellipse                                                     */
      /**********************************************************************/
      OdDbEllipse pEllipse = OdDbEllipse.createObject();
      pEllipse.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pEllipse);

      center = new OdGePoint3d(0, 0, 0);
      center.x += w * 2.5 / 8.0;

      double majorRadius = w * 1.0 / 8.0;
      OdGeVector3d majorAxis = new OdGeVector3d(majorRadius, 0.0, 0.0);
      majorAxis.rotateBy(OdaToRadian(30.0), OdGeVector3d.kZAxis);

      double radiusRatio = 0.25;

      pEllipse.set(center, OdGeVector3d.kZAxis, majorAxis, radiusRatio);

      /**********************************************************************/
      /* Add an Attdef                                                      */
      /**********************************************************************/
      OdDbAttributeDefinition pAttDef = OdDbAttributeDefinition.createObject();
      pAttDef.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pAttDef);

      pAttDef.setPrompt("Enter ODT_ATT: ");
      pAttDef.setTag("Oda_ATT");
      pAttDef.setHorizontalMode(TextHorzMode.kTextCenter);
      pAttDef.setHeight(0.1);
      pAttDef.setTextString("Default");

      /**********************************************************************/
      /* Return the ObjectId of the BlockTableRecord                        */
      /**********************************************************************/
      return btrId;
    }

    /************************************************************************/
    /* Append an XData Pair to the specified ResBuf                         */
    /************************************************************************/
    static OdResBuf appendXDataPair(OdResBuf pCurr, int code)
    {
      pCurr.setNext(OdResBuf.newRb(code));
      return pCurr.next();
    }

    void addExtendedData(OdDbObjectId id)
    {
      OdDbObject pObject = id.safeOpenObject(OpenMode.kForWrite);
      OdResBuf xIter = OdResBuf.newRb(1001);

      OdResBuf temp = xIter;
      temp.setString("ODA");  // Application

      temp = appendXDataPair(temp, 1000);
      temp.setString("Extended Data for ODA app");

      temp = appendXDataPair(temp, 1000);
      temp.setString("Double");

      temp = appendXDataPair(temp, 1041);
      temp.setDouble(5.2);

      pObject.setXData(xIter);
    }

    // Adds an external reference called "XRefBlock" to the passed in database,
    // which references the file "xref.dwg".

    /************************************************************************/
    /* Add an XRef to the specified BlockTableRecord                        */
    /************************************************************************/
    void addXRef(OdDbObjectId btrId, int boxRow, int boxCol, OdDbObjectId layerId, OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "XREF INSERT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Open the block table                                               */
      /**********************************************************************/
      OdDbBlockTable pBlocks = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create a BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord pXRef = OdDbXRefManExt.addNewXRefDefBlock(pDb, "OdWriteEx XRef.dwg", "XRefBlock", false);

      /**********************************************************************/
      /* Insert the Xref                                                    */
      /**********************************************************************/
      OdDbObjectId xRefId = addInsert(bBTR, pXRef.objectId(), 1.0, 1.0);

      /**********************************************************************/
      /* Open the insert                                                    */
      /**********************************************************************/
      OdDbBlockReference pXRefIns = (OdDbBlockReference)xRefId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Set the insertion point                                            */
      /**********************************************************************/
      pXRefIns.setPosition(point);

      /**********************************************************************/
      /* Move\Scale XREF to presentation rectangle                          */
      /**********************************************************************/

      OdGeExtents3d extents = new OdGeExtents3d();
      if (pXRefIns.getGeomExtents(extents) == OdResult.eOk && extents.isValidExtents())
      {
        double dScale = Math.Min(w / (extents.maxPoint().x - extents.minPoint().x), h * (7.0 / 8.0) / (extents.maxPoint().y - extents.minPoint().y));
        pXRefIns.setScaleFactors(new OdGeScale3d(dScale, dScale, 1));
        pXRefIns.setPosition(point - (extents.minPoint() - point.asVector()).asVector() * dScale);
      }
    }


    /************************************************************************/
    /* Populate the Database                                                */
    /*                                                                      */
    /* PaperSpace Viewports                                                 */
    /* Linetypes with embedded shapes, and custom linetypes                 */
    /*                                                                      */
    /************************************************************************/
    public void fillDatabase(OdDbDatabase pDb)
    {
        MemoryManager mMan = MemoryManager.GetMemoryManager();
        MemoryTransaction mStartTr = mMan.StartTransaction();
      Console.WriteLine("\nPopulating the database...");

      /**********************************************************************/
      /* Set Creation and Last Update times                                 */
      /**********************************************************************/
      OdDbDate date = new OdDbDate();

      date.setDate(1, 1, 2006);
      date.setTime(12, 0, 0, 0);
      date.localToUniversal();
      TD_Db.odDbSetTDUCREATE(pDb, date);

      date.getUniversalTime();
      TD_Db.odDbSetTDUUPDATE(pDb, date);

      /**********************************************************************/
      /* Add some Registered Applications                                   */
      /**********************************************************************/
      addRegApp(pDb, "ODA");
      addRegApp(pDb, "AVE_FINISH"); // for materials
      /**********************************************************************/
      /* Add an SHX text style                                              */
      /**********************************************************************/
      OdDbObjectId shxTextStyleId = addStyle(pDb, "OdaShxStyle", 0.0, 1.0, 0.2, 0.0, "txt",
        false, "", false, false, 0, 0);

      /**********************************************************************/
      /* Add a TTF text style                                               */
      /**********************************************************************/
      OdDbObjectId ttfStyleId = addStyle(pDb, "OdaTtfStyle", 0.0, 1.0, 0.2, 0.0,
          "VERDANA.TTF", false, "Verdana", false, false, 0, 34);

      /**********************************************************************/
      /* Add a Shape file style for complex linetypes                       */
      /**********************************************************************/
      OdDbObjectId shapeStyleId = addStyle(pDb, "", 0.0, 1.0, 0.2, 0.0, "ltypeshp.shx",
        true, "", false, false, 0, 0);

      /**********************************************************************/
      /* Add some linetypes                                                 */
      /**********************************************************************/
      addLinetypes(pDb, shapeStyleId, shxTextStyleId);

      /**********************************************************************/
      /* Add a Layer                                                        */
      /**********************************************************************/
      OdDbObjectId odaLayer1Id = addLayer(pDb, "Oda Layer 1", (short)OdCmEntityColor.ACIcolorMethod.kACIRed, "CONTINUOUS");

      /**********************************************************************/
      /* Add a block definition                                             */
      /**********************************************************************/
      OdDbObjectId odaBlock1Id = addBlockDef(pDb, "OdaBlock1", 1, 2);

      /**********************************************************************/
      /* Add a DimensionStyle                                               */
      /**********************************************************************/
      OdDbObjectId odaDimStyleId = addDimStyle(pDb, "OdaDimStyle");

      /**********************************************************************/
      /* Add an MLine style                                                 */
      /**********************************************************************/
      OdDbObjectId odaMLineStyleId = addMLineStyle(pDb, "OdaStandard", "ODA Standard Style");

      /**********************************************************************/
      /* Add a Material                                                     */
      /**********************************************************************/
      OdDbObjectId odaMaterialId = addMaterial(pDb, "OdaMaterial", "ODA Defined Material");

      /**********************************************************************/
      /* Add a PaperSpace Viewport                                          */
      /**********************************************************************/
      MemoryTransaction mTr = mMan.StartTransaction();
      addPsViewport(pDb, odaLayer1Id);

      /**********************************************************************/
      /* Add ModelSpace Entity Boxes                                        */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      createEntityBoxes(pDb.getModelSpaceId(), odaLayer1Id);

      /**********************************************************************/
      /* Add some lines                                                     */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addLines(pDb.getModelSpaceId(), 0, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a 2D (heavy) polyline                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      add2dPolyline(pDb.getModelSpaceId(), 0, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a PolyFace Mesh                                                */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addPolyFaceMesh(pDb.getModelSpaceId(), 0, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a PolygonMesh                                                */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addPolygonMesh(pDb.getModelSpaceId(), 0, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some curves                                                    */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addCurves(pDb.getModelSpaceId(), 0, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Tolerance                                                    */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTolerance(pDb.getModelSpaceId(), 0, 5, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some Leaders                                                   */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addLeaders(pDb.getModelSpaceId(), 0, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Aligned Dimension                                           */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addAlignedDimension(pDb.getModelSpaceId(), 0, 7, odaLayer1Id, ttfStyleId, odaDimStyleId);

      /**********************************************************************/
      /* Add a MultiLine                                                    */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addMLine(pDb.getModelSpaceId(), 0, 8, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Arc Dimension                                               */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addArcDimension(pDb.getModelSpaceId(), 0, 9, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a 3D Polyline                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      add3dPolyline(pDb.getModelSpaceId(), 1, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add MText                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addMText(pDb.getModelSpaceId(), 1, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Block Reference                                                */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addBlockRef(pDb.getModelSpaceId(), 1, 2, odaLayer1Id, ttfStyleId, odaBlock1Id);

      /**********************************************************************/
      /* Add Radial Dimension                                               */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addRadialDimension(pDb.getModelSpaceId(), 1, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add 3D Face                                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      add3dFace(pDb.getModelSpaceId(), 1, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add RText                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addRText(pDb.getModelSpaceId(), 1, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Hatches                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addHatches(pDb.getModelSpaceId(), 2, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some text entities to ModelSpace                               */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTextEnts(pDb.getModelSpaceId(), 2, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Solid                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addSolid(pDb.getModelSpaceId(), 2, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Associative Dimension                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addDimAssoc(pDb.getModelSpaceId(), 2, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Ray                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addRay(pDb.getModelSpaceId(), 3, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a 3 Point Angular Dimension                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      add3PointAngularDimension(pDb.getModelSpaceId(), 3, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Ordinate Dimensions                                            */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addOrdinateDimensions(pDb.getModelSpaceId(), 3, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Spline                                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addSpline(pDb.getModelSpaceId(), 3, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some Traces                                                    */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTraces(pDb.getModelSpaceId(), 3, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Polyline                                                     */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addPolyline(pDb.getModelSpaceId(), 3, 5, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an ArcAlignedText                                              */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addArcText(pDb.getModelSpaceId(), 3, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Wipeout                                                      */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addWipeout(pDb.getModelSpaceId(), 3, 7, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a RadialDimensionLarge                                         */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addRadialDimensionLarge(pDb.getModelSpaceId(), 3, 8, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a 2 Line Angular Dimension                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      add2LineAngularDimension(pDb.getModelSpaceId(), 3, 9, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an ACIS Solid                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addACIS(pDb.getModelSpaceId(), 1, 5, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Image                                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addImage(pDb.getModelSpaceId(), 4, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add an Xref                                                        */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addXRef(pDb.getModelSpaceId(), 4, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Table                                                        */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTable(pDb.getModelSpaceId(), odaBlock1Id, 4, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Diametric Dimension                                               */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addDiametricDimension(pDb.getModelSpaceId(), 4, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Shape                                                        */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addShape(pDb.getModelSpaceId(), 4, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a MInsert                                                      */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addMInsert(pDb.getModelSpaceId(), 4, 5, odaLayer1Id, ttfStyleId, odaBlock1Id);

      /**********************************************************************/
      /* Add an Xline                                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addXline(pDb.getModelSpaceId(), 4, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add custom objects                                                 */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addCustomObjects(pDb);

      /**********************************************************************/
      /* Add Text with Field                                                */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTextWithField(pDb.getModelSpaceId(), 5, 0, odaLayer1Id, shxTextStyleId, ttfStyleId);

      /**********************************************************************/
      /* Add OLE object                                                     */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addOLE2FrameFromFile(pDb.getModelSpaceId(), 5, 1, "OdWriteEx.xls", odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Box                                                            */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addBox(pDb.getModelSpaceId(), 5, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Frustum                                                        */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addFrustum(pDb.getModelSpaceId(), 5, 3, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Sphere                                                         */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addSphere(pDb.getModelSpaceId(), 5, 4, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Torus                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addTorus(pDb.getModelSpaceId(), 5, 5, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Wedge                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addWedge(pDb.getModelSpaceId(), 5, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Region                                                         */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addRegion(pDb.getModelSpaceId(), 5, 7, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Extrusion                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addExtrusion(pDb.getModelSpaceId(), 6, 0, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Revolution                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addSolRev(pDb.getModelSpaceId(), 6, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Helix                                                          */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addHelix(pDb.getModelSpaceId(), 6, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add Underlays                                                   */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addDwfUnderlay(pDb.getModelSpaceId(), 6, 3, odaLayer1Id, ttfStyleId);
      addDgnUnderlay(pDb.getModelSpaceId(), 6, 4, odaLayer1Id, ttfStyleId);
      addPdfUnderlay(pDb.getModelSpaceId(), 6, 5, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some Lights                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addLights(pDb.getModelSpaceId(), 6, 6, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some SubDMeshes                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addSubDMeshes(pDb.getModelSpaceId(), 6, 7, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some Surfaces                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addExtrudedSurfaces(pDb.getModelSpaceId(), 6, 8, odaLayer1Id, ttfStyleId);
      addRevolvedSurfaces(pDb.getModelSpaceId(), 6, 9, odaLayer1Id, ttfStyleId);
      addPlaneSurfaces(pDb.getModelSpaceId(), 6, 10, odaLayer1Id, ttfStyleId);
      addLoftedSurfaces(pDb.getModelSpaceId(), 7, 0, odaLayer1Id, ttfStyleId);
      addSweptSurfaces(pDb.getModelSpaceId(), 7, 1, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add some MLeaders                                                  */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addMLeaders(pDb.getModelSpaceId(), 7, 2, odaLayer1Id, ttfStyleId);

      /**********************************************************************/
      /* Add a Layout                                                       */
      /**********************************************************************/
      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
      addLayout(pDb);


      // If preview bitmap is already available it can be specified to avoid wasting
      // time on generating it by DD
      string pBmpFileName = "preview.bmp";
      unchecked
      {
        if (TD_Db.odSystemServices().accessFile(pBmpFileName, (int)(long)FileAccessMode.kFileRead))
        {
          System.IO.FileStream fs = System.IO.File.OpenRead(pBmpFileName);
          long L = fs.Length;
          byte[] bmp = new byte[L];
          fs.Read(bmp, 0, (int)L);
          // Get length taking care about big-endian
          UInt32 length = bmp[2] + ((uint)(bmp[3]) << 8);
          byte[] bmp1 = new byte[length];
          for (uint i = 0; i < length; ++i)
            bmp1[i] = bmp[14 + i]; // Skip BITMAPFILEHEADER
          //pDb.setThumbnailBitmap(bmp1);
        }
      }

      mMan.StopTransaction(mTr);
      mTr = mMan.StartTransaction();
    //tmpmku  UndoSample(pDb);

      mMan.StopTransaction(mTr);
      mMan.StopTransaction(mStartTr);
    }

    /************************************************************************/
    /* Add a layout                                                         */
    /************************************************************************/
    void addLayout(OdDbDatabase pDb)
    {
      /********************************************************************/
      /* Create a new Layout                                              */
      /********************************************************************/
      OdDbObjectId layoutId = pDb.createLayout("ODA Layout");
      OdDbLayout pLayout = (OdDbLayout)layoutId.safeOpenObject();

      /********************************************************************/
      /* Make it current, creating the overall PaperSpace viewport        */
      /********************************************************************/
      pDb.setCurrentLayout(layoutId);

      /********************************************************************/
      /* Open the overall viewport for this layout                        */
      /********************************************************************/
      OdDbViewport pOverallViewport = (OdDbViewport)pLayout.overallVportId().safeOpenObject();

      /********************************************************************/
      /* Get some useful parameters                                       */
      /********************************************************************/
      OdGePoint3d overallCenter = pOverallViewport.centerPoint();

      /********************************************************************/
      /* Note:                                                            */
      /* If a viewport is an overall viewport,                            */
      /* the values returned by width() and height() must be divided by a */
      /* factor of 1.058, and the parameters of setWidth and setHeight()  */
      /* must be multiplied a like factor.                                */
      /********************************************************************/
      const double margin = 0.25;
      double overallWidth = pOverallViewport.width() / 1.058 - 2 * margin;
      double overallHeight = pOverallViewport.height() / 1.058 - 2 * margin;
      OdGePoint3d overallLLCorner = overallCenter -
        new OdGeVector3d(0.5 * overallWidth, 0.5 * overallHeight, 0.0);

      /********************************************************************/
      /* Open the PaperSpace BlockTableRecord for this layout             */
      /********************************************************************/
      OdDbBlockTableRecord pPS = (OdDbBlockTableRecord)pLayout.getBlockTableRecordId().safeOpenObject(OpenMode.kForWrite);

      /********************************************************************/
      /* Create a new viewport, and append it to PaperSpace               */
      /********************************************************************/
      OdDbViewport pViewport = OdDbViewport.createObject();
      pViewport.setDatabaseDefaults(pDb);
      pPS.appendOdDbEntity(pViewport);

      /********************************************************************/
      /* Set some parameters                                              */
      /*                                                                  */
      /* This viewport occupies the upper half of the overall viewport,   */
      /* and displays all objects in model space                          */
      /********************************************************************/

      pViewport.setWidth(overallWidth);
      pViewport.setHeight(overallHeight * 0.5);
      pViewport.setCenterPoint(overallCenter + new OdGeVector3d(0.0, 0.5 * pViewport.height(), 0.0));
      pViewport.setViewCenter(pOverallViewport.viewCenter());
      pViewport.zoomExtents();

      /********************************************************************/
      /* Create viewports for each of the entities that have been         */
      /* pushBacked onto m_layoutEntities                                 */
      /********************************************************************/
      if (m_layoutEntities.Count != 0)
      {
        double widthFactor = 1.0 / m_layoutEntities.Count;
        for (int i = 0; i < m_layoutEntities.Count; ++i)
        {
          OdDbEntity pEnt = (OdDbEntity)m_layoutEntities[i].safeOpenObject();
          OdGeExtents3d entityExtents = new OdGeExtents3d();
          if (pEnt.getGeomExtents(entityExtents) == OdResult.eOk)
          {
            /**************************************************************/
            /* Create a new viewport, and append it to PaperSpace         */
            /**************************************************************/
            pViewport = OdDbViewport.createObject();
            pViewport.setDatabaseDefaults(pDb);
            pPS.appendOdDbEntity(pViewport);

            /**************************************************************/
            /* The viewports are tiled along the bottom of the overall    */
            /* viewport                                                   */
            /**************************************************************/
            pViewport.setWidth(overallWidth * widthFactor);
            pViewport.setHeight(overallHeight * 0.5);
            pViewport.setCenterPoint(overallLLCorner +
              new OdGeVector3d((i + 0.5) * pViewport.width(), 0.5 * pViewport.height(), 0.0));

            /**************************************************************/
            /* The target of the viewport is the midpoint of the entity   */
            /* extents                                                    */
            /**************************************************************/
            OdGePoint3d minPt = entityExtents.minPoint();
            OdGePoint3d maxPt = entityExtents.maxPoint();
            OdGePoint3d viewTarget = new OdGePoint3d();
            viewTarget.x = (minPt.x + maxPt.x) / 2.0;
            viewTarget.y = (minPt.y + maxPt.y) / 2.0;
            viewTarget.z = (minPt.z + maxPt.z) / 2.0;
            pViewport.setViewTarget(viewTarget);

            /**************************************************************/
            /* The viewHeight is the larger of the height as defined by   */
            /* the entityExtents, and the height required to display the  */
            /* width of the entityExtents                                 */
            /**************************************************************/
            double viewHeight = Math.Abs(maxPt.y - minPt.y);
            double viewWidth = Math.Abs(maxPt.x - minPt.x);
            viewHeight = Math.Max(viewHeight, viewWidth * pViewport.height() / pViewport.width());
            pViewport.setViewHeight(viewHeight * 1.05);
          }
        }
      }
      pDb.setTILEMODE(true); // Disable PaperSpace
    }

    /************************************************************************/
    /* Add entity boxes to specified BlockTableRecord                       */
    /************************************************************************/
    void createEntityBoxes(OdDbObjectId btrId,
                                     OdDbObjectId layerId)
    {
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      OdGePoint3d currentPoint;

      /**********************************************************************/
      /* Create a 2D polyline for each box                                  */
      /**********************************************************************/
      for (int j = 0; j < EntityBoxes.VER_BOXES; j++)
      {
        for (int i = 0; i < EntityBoxes.HOR_BOXES; i++)
        {
          if (!EntityBoxes.isBox(j, i))
            break;

          double wCurBox = EntityBoxes.getWidth(j, i);
          currentPoint = EntityBoxes.getBox(j, i);

          OdDb2dPolyline pPline = OdDb2dPolyline.createObject();
          pPline.setDatabaseDefaults(pDb);

          bBTR.appendOdDbEntity(pPline);

          OdDb2dVertex pVertex;

          pVertex = OdDb2dVertex.createObject();
          pVertex.setDatabaseDefaults(pDb);
          pPline.appendVertex(pVertex);
          OdGePoint3d pos = currentPoint;
          pVertex.setPosition(pos);

          pVertex = OdDb2dVertex.createObject();
          pPline.appendVertex(pVertex);
          pos.x += wCurBox;
          pVertex.setPosition(pos);

          pVertex = OdDb2dVertex.createObject();
          pPline.appendVertex(pVertex);
          pos.y -= EntityBoxes.getHeight();
          pVertex.setPosition(pos);

          pVertex = OdDb2dVertex.createObject();
          pPline.appendVertex(pVertex);
          pos.x -= wCurBox;
          pVertex.setPosition(pos);


          pPline.makeClosed();

          pPline.setColorIndex((UInt16)OdCmEntityColor.ACIcolorMethod.kACIBlue, true);
          pPline.setLayer(layerId, true);
        }
      }
      /**********************************************************************/
      /* 'Zoom' the box array by resizing the active tiled MS viewport      */
      /**********************************************************************/
      OdDbViewportTable pVpTable = (OdDbViewportTable)pDb.getViewportTableId().safeOpenObject(OpenMode.kForWrite);
      OdDbObjectId vpID = pVpTable.getActiveViewportId();
      OdDbViewportTableRecord vPortRec = (OdDbViewportTableRecord)vpID.safeOpenObject(OpenMode.kForWrite);

      OdGePoint3d center = EntityBoxes.getArrayCenter();
      vPortRec.setCenterPoint(center.convert2d());

      OdGeVector3d size = EntityBoxes.getArraySize();
      vPortRec.setHeight(1.05 * Math.Abs(size.y));
      vPortRec.setWidth(1.05 * Math.Abs(size.x));
      vPortRec.setCircleSides(20000);
    }
    /************************************************************************/
    /* Add a PaperSpace viewport to the specified database                  */
    /************************************************************************/
    void addPsViewport(OdDbDatabase pDb,
                                 OdDbObjectId layerId)
    {
      /**********************************************************************/
      /* Enable PaperSpace                                                  */
      /*                                                                    */
      /* NOTE: This is required to cause Teigha to automatically create     */
      /* the overall viewport. If not called before opening PaperSpace      */
      /* BlockTableRecord,   the first viewport created IS the the overall  */
      /* viewport.                                                          */
      /**********************************************************************/
      pDb.setTILEMODE(false);

      /**********************************************************************/
      /* Open PaperSpace                                                    */
      /**********************************************************************/
      OdDbBlockTableRecord pPs = (OdDbBlockTableRecord)pDb.getPaperSpaceId().safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Disable PaperSpace                                                 */
      /**********************************************************************/
      // pDb.setTILEMODE(1);

      /**********************************************************************/
      /* Create the viewport                                                */
      /**********************************************************************/
      OdDbViewport pVp = OdDbViewport.createObject();
      pVp.setDatabaseDefaults(pDb);
      /**********************************************************************/
      /* Add it to PaperSpace                                               */
      /**********************************************************************/
      pPs.appendOdDbEntity(pVp);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pVp.setCenterPoint(new OdGePoint3d(5.25, 4.0, 0));
      pVp.setWidth(10.0);
      pVp.setHeight(7.5);
      pVp.setViewTarget(new OdGePoint3d(0, 0, 0));
      pVp.setViewDirection(new OdGeVector3d(0, 0, 1));
      pVp.setViewHeight(8.0);
      pVp.setLensLength(50.0);
      pVp.setViewCenter(new OdGePoint2d(5.25, 4.0));
      pVp.setSnapIncrement(new OdGeVector2d(0.25, 0.25));
      pVp.setGridIncrement(new OdGeVector2d(0.25, 0.25));
      pVp.setCircleSides((UInt16)(20000));

      /**********************************************************************/
      /* Freeze a layer in this viewport                                    */
      /**********************************************************************/
      OdDbObjectIdArray frozenLayers = new OdDbObjectIdArray();
      frozenLayers.Add(layerId);
      pVp.freezeLayersInViewport(frozenLayers);

      /**********************************************************************/
      /* Add a circle to this PaperSpace Layout                             */
      /**********************************************************************/
      /*OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(pDb);
      pPs.appendOdDbEntity(pCircle);
      pCircle.setRadius(1.0);
      pCircle.setCenter(new OdGePoint3d(1.0, 1.0, 0.0));
      pCircle.setLayer(layerId, false);*/

      /**********************************************************************/
      /* Disable PaperSpace                                                 */
      /**********************************************************************/
      pDb.setTILEMODE(true);
    }

    /************************************************************************/
    /* Add a dimension style to the specified database                      */
    /************************************************************************/
    OdDbObjectId addDimStyle(OdDbDatabase pDb,
                                       string dimStyleName)
    {
      /**********************************************************************/
      /* Create the DimStyle                                                */
      /**********************************************************************/
      OdDbDimStyleTableRecord pDimStyle = OdDbDimStyleTableRecord.createObject();
      /**********************************************************************/
      /* Set the name                                                       */
      /**********************************************************************/
      pDimStyle.setName(dimStyleName);
      /**********************************************************************/
      /* Open the DimStyleTable                                             */
      /**********************************************************************/
      OdDbDimStyleTable pTable = (OdDbDimStyleTable)pDb.getDimStyleTableId().safeOpenObject(OpenMode.kForWrite);
      /**********************************************************************/
      /* Add the DimStyle                                                   */
      /**********************************************************************/
      OdDbObjectId dimStyleId = pTable.add(pDimStyle);
      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pDimStyle.setDimtxsty(new OdDbHardPointerId(pDb.getTextStyleStandardId()));
      pDimStyle.setDimsah(true);
      pDimStyle.setDimblk1("_OBLIQUE");
      pDimStyle.setDimblk2("_DOT");
      return dimStyleId;
    }

    /************************************************************************/
    /* Add an Associative Dimension to the specified BlockTableRecord       */
    /************************************************************************/
    void addDimAssoc(OdDbObjectId btrId,
                               int boxRow,
                               int boxCol,
                               OdDbObjectId layerId,
                               OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Associative", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a line to be dimensioned                                    */
      /**********************************************************************/
      OdGePoint3d line1Pt = new OdGePoint3d();
      line1Pt.x = point.x + w * 1.0 / 8.0;
      line1Pt.y = point.y + h * 2.0 / 8.0;
      OdGePoint3d line2Pt = new OdGePoint3d();
      line2Pt.x = line1Pt.x + 3.75;
      line2Pt.y = point.y + h * 7.0 / 8.0;

      OdDbLine pLine = OdDbLine.createObject();
      pLine.setDatabaseDefaults(pDb);
      OdDbObjectId lineId = bBTR.appendOdDbEntity(pLine);
      pLine.setStartPoint(line1Pt);
      pLine.setEndPoint(line2Pt);

      /**********************************************************************/
      /* Create a rotated dimension and dimension the ends of the line      */
      /**********************************************************************/
      OdDbRotatedDimension pDimension = OdDbRotatedDimension.createObject();
      pDimension.setDatabaseDefaults(pDb);
      OdDbObjectId dimensionId = bBTR.appendOdDbEntity(pDimension);

      OdGePoint3d dimLinePt = new OdGePoint3d();
      dimLinePt.x = point.x + w / 2.0;
      dimLinePt.y = point.y + h * 1.0 / 8.0;
      pDimension.setDatabaseDefaults(pDb);
      pDimension.setXLine1Point(pLine.startPoint());
      pDimension.setXLine2Point(pLine.endPoint());
      pDimension.setDimLinePoint(dimLinePt);
      pDimension.useDefaultTextPosition();
      pDimension.createExtensionDictionary();

      /**********************************************************************/
      /* Create an associative dimension                                    */
      /**********************************************************************/
      OdDbDimAssoc pDimAssoc = OdDbDimAssoc.createObject();

      /**********************************************************************/
      /* Associate the associative dimension with the rotated dimension by  */
      /* adding it to the extension dictionary of the rotated dimension     */
      /**********************************************************************/
      OdDbDictionary pDict = (OdDbDictionary)pDimension.extensionDictionary().safeOpenObject(OpenMode.kForWrite);
      OdDbObjectId dimAssId = pDict.setAt("ACAD_DIMASSOC", pDimAssoc);

      /**********************************************************************/
      /* Associate the rotated dimension with the associative dimension     */
      /**********************************************************************/
      pDimAssoc.setDimObjId(dimensionId);
      pDimAssoc.setRotatedDimType(OdDbDimAssoc.RotatedDimType.kUnknown);

      /**********************************************************************/
      /* Attach the line to the associative dimension                       */
      /**********************************************************************/
      OdDbOsnapPointRef pointRef = OdDbOsnapPointRef.createObject();
      pointRef.setPoint(pLine.startPoint());
      pointRef.setOsnapType(OsnapMode.kOsModeStart);
      pointRef.setNearPointParam(0.0);

      pointRef.mainEntity().objectIds().Add(lineId);
      pointRef.mainEntity().subentId().setType(Teigha.Core.SubentType.kVertexSubentType);

      pDimAssoc.setPointRef((int)OdDbDimAssoc.PointType.kXline1Point, pointRef);

      pointRef = OdDbOsnapPointRef.createObject();
      pointRef.setPoint(pLine.endPoint());
      pointRef.setOsnapType(OsnapMode.kOsModeEnd);
      pointRef.setNearPointParam(1.0);

      pointRef.mainEntity().objectIds().Add(lineId);
      pointRef.mainEntity().subentId().setType(Teigha.Core.SubentType.kEdgeSubentType);

      pDimAssoc.setPointRef((int)OdDbDimAssoc.PointType.kXline2Point, pointRef);

      /**********************************************************************/
      /* Add Persistent reactors from the rotated dimension and the line    */
      /* to the associative dimension                                       */
      /**********************************************************************/
      pDimension.addPersistentReactor(dimAssId);
      pLine.addPersistentReactor(dimAssId);
    }

    /************************************************************************/
    /* Add an Aligned Dimension to the specified BlockTableRecord           */
    /************************************************************************/
    void addAlignedDimension(OdDbObjectId btrId,
                                       int boxRow,
                                       int boxCol,
                                       OdDbObjectId layerId,
                                       OdDbObjectId styleId,
                                       OdDbObjectId dimStyleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Aligned", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a line to be dimensioned                                    */
      /**********************************************************************/
      OdGePoint3d line1Pt = new OdGePoint3d();
      OdGePoint3d line2Pt = new OdGePoint3d();
      line1Pt.x = point.x + w * 0.5 / 8.0;
      line1Pt.y = point.y + h * 1.5 / 8.0;
      line2Pt = line1Pt + new OdGeVector3d(1.5, 2.0, 0.0);

      OdDbLine pLine = OdDbLine.createObject();
      pLine.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pLine);
      pLine.setStartPoint(line1Pt);
      pLine.setEndPoint(line2Pt);

      /**********************************************************************/
      /* Create an aligned dimension and dimension the ends of the line     */
      /**********************************************************************/
      OdDbAlignedDimension pDimension = OdDbAlignedDimension.createObject();
      pDimension.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pDimension);

      OdGePoint3d dimLinePt = new OdGePoint3d();
      dimLinePt.x = point.x + w * 3.5 / 8.0;
      dimLinePt.y = point.y + h * 2.0 / 8.0;

      pDimension.setDimensionStyle(dimStyleId);
      pDimension.setXLine1Point(pLine.startPoint());
      pDimension.setXLine2Point(pLine.endPoint());
      pDimension.setDimLinePoint(dimLinePt);
      pDimension.useDefaultTextPosition();
      pDimension.setJogSymbolHeight(1.5);
    }

    /************************************************************************/
    /* Add a Radial Dimension to the specified BlockTableRecord             */
    /************************************************************************/
    void addRadialDimension(OdDbObjectId btrId,
                                      int boxRow,
                                      int boxCol,
                                      OdDbObjectId layerId,
                                      OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Radial", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a circle to be dimensioned                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pCircle);
      pCircle.setCenter(point + new OdGeVector3d(0.625, h * 3.0 / 8.0, 0));
      pCircle.setRadius(0.5);

      /**********************************************************************/
      /* Create a Radial Dimension                                         */
      /**********************************************************************/
      OdDbRadialDimension pDimension = OdDbRadialDimension.createObject();
      pDimension.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pDimension);
      pDimension.setCenter(pCircle.center());
      OdGeVector3d chordVector = new OdGeVector3d(pCircle.radius(), 0.0, 0.0);
      chordVector.rotateBy(OdaToRadian(75.0), OdGeVector3d.kZAxis);
      pDimension.setChordPoint(pDimension.center() + chordVector);
      pDimension.setLeaderLength(0.125);
      pDimension.useDefaultTextPosition();
    }

    /************************************************************************/
    /* Add a Diametric Dimension to the specified BlockTableRecord             */
    /************************************************************************/
    void addDiametricDimension(OdDbObjectId btrId,
                                         int boxRow,
                                         int boxCol,
                                         OdDbObjectId layerId,
                                         OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Diametric", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a circle to be dimensioned                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pCircle);
      pCircle.setCenter(point + new OdGeVector3d(0.625, h * 3.0 / 8.0, 0));
      pCircle.setRadius(0.5);

      /**********************************************************************/
      /* Create a Diametric Dimension                                       */
      /**********************************************************************/
      OdDbDiametricDimension pDimension = OdDbDiametricDimension.createObject();
      pDimension.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pDimension);

      OdGeVector3d chordVector = new OdGeVector3d(pCircle.radius(), 0.0, 0.0);
      chordVector.rotateBy(OdaToRadian(75.0), OdGeVector3d.kZAxis);

      pDimension.setChordPoint(pCircle.center() + chordVector);
      pDimension.setFarChordPoint(pCircle.center() - chordVector);
      pDimension.setLeaderLength(0.125);
      pDimension.useDefaultTextPosition();
    }
    /************************************************************************/
    /* Add a Shape to the specified BlockTableRecord                        */
    /************************************************************************/
    void addShape(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Shape", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the center of the box                                          */
      /**********************************************************************/
      OdGePoint3d pCenter = EntityBoxes.getBoxCenter(boxRow, boxCol);

      /**********************************************************************/
      /* Create a Shape                                                     */
      /**********************************************************************/
      OdDbShape pShape = OdDbShape.createObject();
      pShape.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pShape);
      double size = w * 3.0 / 8.0;
      pCenter.y -= size;
      pShape.setSize(size);
      pShape.setPosition(pCenter);
      pShape.setRotation(OdaToRadian(90.0));
      pShape.setName("CIRC1");
    }

    /************************************************************************/
    /* Add a 3D Face to the specified BlockTableRecord                      */
    /************************************************************************/
    void add3dFace(OdDbObjectId btrId,
                             int boxRow,
                             int boxCol,
                             OdDbObjectId layerId,
                             OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "3DFACE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a 3D Face                                                   */
      /**********************************************************************/
      OdDbFace pFace = OdDbFace.createObject();
      pFace.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pFace);

      pFace.setVertexAt(0, point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0));
      pFace.setVertexAt(1, point + new OdGeVector3d(w * 7.0 / 8.0, h * 1.0 / 8.0, 0.0));
      pFace.setVertexAt(2, point + new OdGeVector3d(w * 7.0 / 8.0, h * 6.0 / 8.0, 0.0));
      pFace.setVertexAt(3, point + new OdGeVector3d(w * 1.0 / 8.0, h * 6.0 / 8.0, 0.0));
    }

    /************************************************************************/
    /* Add a Solid to the specified BlockTableRecord                          */
    /************************************************************************/
    void addSolid(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "SOLID", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a Solid                                                   */
      /**********************************************************************/
      OdDbSolid pSolid = OdDbSolid.createObject();
      pSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pSolid);

      pSolid.setPointAt(0, point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0));
      pSolid.setPointAt(1, point + new OdGeVector3d(w * 7.0 / 8.0, h * 1.0 / 8.0, 0.0));
      pSolid.setPointAt(2, point + new OdGeVector3d(w * 1.0 / 8.0, h * 6.0 / 8.0, 0.0));
      pSolid.setPointAt(3, point + new OdGeVector3d(w * 7.0 / 8.0, h * 6.0 / 8.0, 0.0));
    }

    /************************************************************************/
    /* Add an ACIS Solid to the specified BlockTableRecord                  */
    /************************************************************************/
    void addACIS(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "3DSOLID", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDbEntityPtrArray entities = new OdDbEntityPtrArray();
      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      /**********************************************************************/
      /* Read the solids in the .sat file                                   */
      /**********************************************************************/
      if (OdDbBody.acisIn("OdWriteEx.sat", entities) == OdResult.eOk)
      {
        /********************************************************************/
        /* Read the solids in the .sat file                                 */
        /********************************************************************/
        addTextEnt(bBTR,
          point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
          "from SAT file", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
        foreach (OdDbEntity p3dSolid in entities)
        {
          /******************************************************************/
          /* Move the solid into the center of the box                      */
          /******************************************************************/
          OdDbObjectId id = bBTR.appendOdDbEntity(p3dSolid);
          p3dSolid.transformBy(xfm);
          p3dSolid.Dispose();
          /******************************************************************/
          /* Each of these entities will later get its own viewport         */
          /******************************************************************/
          m_layoutEntities.Add(id);
        }
      }
      else
      {
        /********************************************************************/
        /* Create a simple solid                                            */
        /********************************************************************/
        OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
        p3dSolid.setDatabaseDefaults(bBTR.database());
        OdDbObjectId id = bBTR.appendOdDbEntity(p3dSolid);

        p3dSolid.createSphere(1.0);
        p3dSolid.transformBy(xfm);

        /********************************************************************/
        /* This entity will later get its own viewport                      */
        /********************************************************************/
        m_layoutEntities.Add(id);
      }
    }
    /************************************************************************/
    /* Add a Box to the specified BlockTableRecord                          */
    /************************************************************************/
    void addBox(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Box", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      p3dSolid.createBox(w * 6.0 / 8.0, h * 6.0 / 8.0, w * 6.0 / 8.0);
      p3dSolid.transformBy(xfm);
    }
    /************************************************************************/
    /* Add a Frustum to the specified BlockTableRecord                      */
    /************************************************************************/
    void addFrustum(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Frustum", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      p3dSolid.createFrustum(w * 6.0 / 8.0, w * 3.0 / 8.0, h * 3.0 / 8.0, w * 1.0 / 8.0);
      p3dSolid.transformBy(xfm);
    }
    /************************************************************************/
    /* Add a Sphere to the specified BlockTableRecord                       */
    /************************************************************************/
    void addSphere(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Sphere", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      p3dSolid.createSphere(w * 3.0 / 8.0);
      p3dSolid.transformBy(xfm);
    }
    /************************************************************************/
    /* Add a Sphere to the specified BlockTableRecord                       */
    /************************************************************************/
    void addTorus(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Torus", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      p3dSolid.createTorus(w * 2.0 / 8.0, w * 1.0 / 8.0);
      p3dSolid.transformBy(xfm);
    }
    /************************************************************************/
    /* Add a Wedge to the specified BlockTableRecord                       */
    /************************************************************************/
    void addWedge(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Wedge", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      OdGeMatrix3d xfm = OdGeMatrix3d.translation(EntityBoxes.getBoxCenter(boxRow, boxCol).asVector());

      p3dSolid.createWedge(w * 6.0 / 8.0, h * 6.0 / 8.0, w * 6.0 / 8.0);
      p3dSolid.transformBy(xfm);
    }
    /************************************************************************/
    /* Add a Region to the specified BlockTableRecord                       */
    /************************************************************************/
    void addRegion(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Region", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create a Circle                                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      pCircle.setCenter(center);
      pCircle.setRadius(w * 3.0 / 8.0);


      /**********************************************************************/
      /* Add it to the array of curves                                      */
      /**********************************************************************/
      OdRxObjectPtrArray curveSegments = new OdRxObjectPtrArray();
      curveSegments.Add(pCircle);

      /**********************************************************************/
      /* Create the region                                                  */
      /**********************************************************************/
      OdRxObjectPtrArray regions = new OdRxObjectPtrArray();
      OdResult res = OdDbRegion.createFromCurves(curveSegments, regions);

      /**********************************************************************/
      /* Append it to the block table record                                */
      /**********************************************************************/
      if (res == OdResult.eOk)
      {
        bBTR.appendOdDbEntity((OdDbEntity)regions[0]);
      }
    }
    /************************************************************************/
    /* Add an Extrusion to the specified BlockTableRecord                   */
    /************************************************************************/
    void addExtrusion(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Extrusion", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      /**********************************************************************/
      /* Create a Circle                                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      pCircle.setCenter(center);
      pCircle.setRadius(w * 3.0 / 8.0);

      /**********************************************************************/
      /* Add it to the array of curves                                      */
      /**********************************************************************/
      OdRxObjectPtrArray curveSegments = new OdRxObjectPtrArray();
      curveSegments.Add(pCircle);

      /**********************************************************************/
      /* Create a region                                                    */
      /**********************************************************************/
      OdRxObjectPtrArray regions = new OdRxObjectPtrArray();
      OdDbRegion.createFromCurves(curveSegments, regions);

      /**********************************************************************/
      /* Extrude the region                                                 */
      /**********************************************************************/
      p3dSolid.extrude((OdDbRegion)regions[0], w * 6.0 / 8.0);

    }
    /************************************************************************/
    /* Add an Solid of Revolution to the specified BlockTableRecord         */
    /************************************************************************/
    void addSolRev(OdDbObjectId btrId,
                           int boxRow,
                           int boxCol,
                           OdDbObjectId layerId,
                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Solid of", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Revolution", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      OdDb3dSolid p3dSolid = OdDb3dSolid.createObject();
      p3dSolid.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(p3dSolid);

      /**********************************************************************/
      /* Create a Circle                                                    */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      pCircle.setCenter(center + new OdGeVector3d(w * 2.5 / 8.0, 0.0, 0.0));
      pCircle.setRadius(w * 1.0 / 8.0);

      /**********************************************************************/
      /* Add it to the array of curves                                      */
      /**********************************************************************/
      OdRxObjectPtrArray curveSegments = new OdRxObjectPtrArray();
      curveSegments.Add(pCircle);

      /**********************************************************************/
      /* Create a region                                                    */
      /**********************************************************************/
      OdRxObjectPtrArray regions = new OdRxObjectPtrArray();
      OdDbRegion.createFromCurves(curveSegments, regions);

      /**********************************************************************/
      /* revolve the region                                                 */
      /**********************************************************************/
      p3dSolid.revolve((OdDbRegion)regions[0], center, new OdGeVector3d(0.0, 1.0, 0.0), Math.PI * 2);
    }

    void addHelix(OdDbObjectId blockId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)blockId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Helix", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create the Helix                                                   */
      /**********************************************************************/
      OdDbHelix pHelix = OdDbHelix.createObject();
      pHelix.setDatabaseDefaults(bBTR.database());

      /**********************************************************************/
      /* Add the Helix to the database                                      */
      /**********************************************************************/
      bBTR.appendOdDbEntity(pHelix);

      /**********************************************************************/
      /* Set the Helix's parameters                                         */
      /**********************************************************************/
      pHelix.setConstrain(OdDbHelix.ConstrainType.kHeight);
      pHelix.setHeight(h);
      pHelix.setAxisPoint(point + new OdGeVector3d(w / 2.0, -h / 2.0, 0.0));
      pHelix.setStartPoint(pHelix.axisPoint() + new OdGeVector3d(w / 6.0, 0.0, 0.0));
      pHelix.setTwist(false);
      pHelix.setTopRadius(w * 3.0 / 8.0);
      pHelix.setTurns(6);

      /**********************************************************************/
      /* Create the Helix geometry (confirm parameters are set)             */
      /**********************************************************************/
      pHelix.createHelix();
    }

    /************************************************************************/
    /* Add an Image to the specified BlockTableRecord                       */
    /************************************************************************/
    void addImage(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Open the Image Dictionary                                          */
      /**********************************************************************/
      OdDbObjectId imageDictId = OdDbRasterImageDef.createImageDictionary(pDb);
      OdDbDictionary pImageDict = (OdDbDictionary)imageDictId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Create an ImageDef object                                          */
      /**********************************************************************/
      OdDbRasterImageDef pImageDef = OdDbRasterImageDef.createObject();
      OdDbObjectId imageDefId = pImageDict.setAt("OdWriteEx", pImageDef);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pImageDef.setSourceFileName("OdWriteEx.jpg");
      // Use next line to set image size manually without loading actual raster file.
      // This method sets "dummy" image instead. It's OK for saving drawing to DXF/DWG.
      // But image will not be rendered/exported to other formats without file saving and opening again
      pImageDef.setImage(OdGiRasterImageDesc.createObject(1024, 650, OdGiRasterImage.Units.kInch));

      // Use next line to set size from the actual raster file.
      // This is also required if you are going to render/export the drawing immediately
      // without saving to DWG and loading again
      //pImageDef.image();    // Force image loading from file (findFile() should be able to locate the image).


      /**********************************************************************/
      /* Create an Image object                                             */
      /**********************************************************************/
      OdDbRasterImage pImage = OdDbRasterImage.createObject();
      pImage.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pImage);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pImage.setImageDefId(imageDefId);
      pImage.setOrientation(point, new OdGeVector3d(w, 0, 0), new OdGeVector3d(0.0, h, 0));
      pImage.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShow, true);
      pImage.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShowUnAligned, true);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      point = EntityBoxes.getBox(boxRow, boxCol);
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "IMAGE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
    }

    /************************************************************************/
    /* Add a Ray to the specified BlockTableRecord                          */
    /************************************************************************/
    void addRay(OdDbObjectId btrId,
                          int boxRow,
                          int boxCol,
                          OdDbObjectId layerId,
                          OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "RAY", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a Ray from the center of the box and passing through        */
      /* the lower-left corner of the box                                   */
      /**********************************************************************/
      OdDbRay pRay = OdDbRay.createObject();
      pRay.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pRay);

      OdGePoint3d basePoint = EntityBoxes.getBoxCenter(boxRow, boxCol);
      OdGeVector3d unitDir = (point - basePoint).normalize();

      pRay.setBasePoint(basePoint);
      pRay.setUnitDir(unitDir);
    }

    /************************************************************************/
    /* Add an Xline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addXline(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "XLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a Ray from the center of the box and passing through        */
      /* the lower-left corner of the box                                   */
      /**********************************************************************/
      OdDbXline pXline = OdDbXline.createObject();
      pXline.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pXline);

      OdGePoint3d basePoint = EntityBoxes.getBoxCenter(boxRow, boxCol);
      OdGeVector3d unitDir = (point - basePoint).normalize();

      pXline.setBasePoint(basePoint);
      pXline.setUnitDir(unitDir);
    }

    /************************************************************************/
    /* Add RText to the specified BlockTableRecord                          */
    /************************************************************************/
    void addRText(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      point += m_textOffset;
      addTextEnt(bBTR, point, point,
        "RTEXT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create RText DIESEL expression with no MText sequences             */
      /**********************************************************************/

      RText pRText = RText.createObject();
      pRText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pRText);

      point += m_textLine + m_textLine;
      point.x += w / 16.0;
      pRText.setHeight(m_textSize);
      pRText.setPoint(point);
      pRText.setRotAngle(0.0);
      pRText.setToExpression(true);
      pRText.enableMTextSequences(false);
      pRText.setStringContents("Expression: 123{\\C5;456}");
      pRText.setTextStyle(styleId);

      /**********************************************************************/
      /* Create RText DIESEL expression with MText sequences                */
      /**********************************************************************/
      pRText = RText.createObject();
      pRText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pRText);

      point += m_textLine;
      pRText.setHeight(m_textSize);
      pRText.setPoint(point);
      pRText.setRotAngle(0.0);
      pRText.setToExpression(true);
      pRText.enableMTextSequences(true);
      pRText.setStringContents("Expression: 123{\\C5;456}");
      pRText.setTextStyle(styleId);

      /**********************************************************************/
      /* Create RText External with no MText sequences                      */
      /**********************************************************************/
      pRText = RText.createObject();
      pRText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pRText);

      point += m_textLine;
      pRText.setHeight(m_textSize);
      pRText.setPoint(point);
      pRText.setRotAngle(0.0);
      pRText.setToExpression(false);
      pRText.enableMTextSequences(false);
      pRText.setStringContents(inCurrentFolder("OdWriteEx.txt"));
      pRText.setTextStyle(styleId);

      /**********************************************************************/
      /* Create RText External with MText sequences                         */
      /**********************************************************************/
      pRText = RText.createObject();
      pRText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pRText);

      point += m_textLine;
      pRText.setHeight(m_textSize);
      pRText.setPoint(point);
      pRText.setRotAngle(0.0);
      pRText.setToExpression(false);
      pRText.enableMTextSequences(true);
      pRText.setStringContents(inCurrentFolder("OdWriteEx.txt"));
      pRText.setTextStyle(styleId);

    }

    /************************************************************************/
    /* Add Hatches to the specified BlockTableRecord                          */
    /************************************************************************/
    void addHatches(OdDbObjectId btrId,
                              int boxRow,
                              int boxCol,
                              OdDbObjectId layerId,
                              OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();
      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      //  double h    = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      double delta = w / 12.0;

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "HATCHs", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create a rectangular Hatch with a circular hole                    */
      /**********************************************************************/
      OdDbHatch pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(pDb);
      OdDbObjectId whiteHatchId = bBTR.appendOdDbEntity(pHatch);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pHatch.setAssociative(false);
      pHatch.setPattern(OdDbHatch.HatchPatternType.kPreDefined, "SOLID");
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);

      /**********************************************************************/
      /* Define the outer loop with an OdGePolyline2d                       */
      /**********************************************************************/
      OdGePoint2dArray vertexPts = OdGePoint2dArray.Repeat(OdGePoint2d.kOrigin, 4);
      OdGeDoubleArray vertexBulges = new OdGeDoubleArray();
      vertexPts[0].set(point.x + delta, point.y - delta);
      vertexPts[1].set(point.x + delta * 5, point.y - delta);
      vertexPts[2].set(point.x + delta * 5, point.y - delta * 5);
      vertexPts[3].set(point.x + delta, point.y - delta * 5);
      pHatch.appendLoop((int)(OdDbHatch.HatchLoopType.kExternal | OdDbHatch.HatchLoopType.kPolyline),
        vertexPts, vertexBulges);


      /**********************************************************************/
      /* Define an inner loop with an array of edges                        */
      /**********************************************************************/
      OdGePoint2d cenPt = new OdGePoint2d(point.x + delta * 3, point.y - delta * 3);
      OdGeCircArc2d cirArc = new OdGeCircArc2d();
      cirArc.setCenter(cenPt);
      cirArc.setRadius(delta);
      cirArc.setAngles(0.0, Math.PI * 2);

      EdgeArray edgePtrs = new EdgeArray();
      edgePtrs.Add(cirArc);
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, edgePtrs);
      GC.SuppressFinalize(cirArc); // NB: appendLoop takes ownership of the edges

      /**********************************************************************/
      /* Create a circular Hatch                                            */
      /**********************************************************************/
      pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(pDb);
      OdDbObjectId redHatchId = bBTR.appendOdDbEntity(pHatch);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pHatch.setAssociative(false);
      pHatch.setPattern(OdDbHatch.HatchPatternType.kPreDefined, "SOLID");
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);
      OdCmColor col = new OdCmColor();
      col.setRGB(255, 0, 0);
      pHatch.setColor(col);

      /**********************************************************************/
      /* Define an outer loop with an array of edges                        */
      /**********************************************************************/
      cirArc = new OdGeCircArc2d();
      cirArc.setCenter(cenPt - new OdGeVector2d(delta, 0.0));
      cirArc.setRadius(delta);
      cirArc.setAngles(0.0, Math.PI * 2);
      edgePtrs.Clear();
      edgePtrs.Add(cirArc);
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, edgePtrs);
      GC.SuppressFinalize(cirArc); // NB: appendLoop takes ownership of the edges

      /**********************************************************************/
      /* Create a circular Hatch                                            */
      /**********************************************************************/
      pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(pDb);
      OdDbObjectId greenHatchId = bBTR.appendOdDbEntity(pHatch);

      pHatch.setAssociative(false);
      pHatch.setPattern(OdDbHatch.HatchPatternType.kPreDefined, "SOLID");
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);
      col.setRGB(0, 255, 0);
      pHatch.setColor(col);

      /**********************************************************************/
      /* Define an outer loop with an array of edges                        */
      /**********************************************************************/
      cirArc = new OdGeCircArc2d();
      cirArc.setCenter(cenPt - new OdGeVector2d(0.0, delta));
      cirArc.setRadius(delta);
      cirArc.setAngles(0.0, Math.PI * 2);
      edgePtrs.Clear();
      edgePtrs.Add(cirArc);
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, edgePtrs);
      GC.SuppressFinalize(cirArc); // NB: appendLoop takes ownership of the edges

      /**********************************************************************/
      /* Use the SortentsTable to manipulate draw order                     */
      /*                                                                    */
      /* The draw order now is white, red, green                            */
      /**********************************************************************/
      OdDbSortentsTable pSET = bBTR.getSortentsTable();

      /**********************************************************************/
      /* Move the green hatch below the red hatch                           */
      /* The draw order now is white, green, red                            */
      /**********************************************************************/
      OdDbObjectIdArray id = new OdDbObjectIdArray();
      id.Add(greenHatchId);
      pSET.moveBelow(id, redHatchId);

      /**********************************************************************/
      /* Create an associative user-defined hatch                           */
      /**********************************************************************/
      pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(pDb);
      OdDbObjectId hatchId = bBTR.appendOdDbEntity(pHatch);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pHatch.setAssociative(true);
      pHatch.setDatabaseDefaults(pDb); // make hatch aware of DB for the next call
      pHatch.setPattern(OdDbHatch.HatchPatternType.kUserDefined, "_USER");
      pHatch.setPatternSpace(0.125);
      pHatch.setPatternAngle(OdaToRadian(30.0));
      pHatch.setPatternDouble(true);
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);

      /**********************************************************************/
      /* Define the loops                                                */
      /**********************************************************************/
      OdDbObjectIdArray loopIds = new OdDbObjectIdArray();
      OdDbEllipse pEllipse = OdDbEllipse.createObject();
      pEllipse.setDatabaseDefaults(pDb);
      loopIds.Add(bBTR.appendOdDbEntity(pEllipse));

      OdGePoint3d centerPt = EntityBoxes.getBoxCenter(boxRow, boxCol);
      centerPt.x += delta;
      centerPt.y += delta * 1.5;
      pEllipse.set(centerPt, OdGeVector3d.kZAxis, new OdGeVector3d(delta, 0.0, 0.0), 0.5);

      /**********************************************************************/
      /* Append the loops to the hatch                                      */
      /**********************************************************************/
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, loopIds);

      /**********************************************************************/
      /* Define a custom hatch pattern "MY_STARS"                           */
      /**********************************************************************/
      OdHatchPattern stars = new OdHatchPattern();
      OdHatchPatternLine line = new OdHatchPatternLine();

      line.m_dLineAngle = 0.0;
      line.m_patternOffset = new OdGeVector2d(0, 0.866);
      line.m_dashes.Add(0.5);
      line.m_dashes.Add(-0.5);
      stars.Add(line);
      line.m_dLineAngle = 1.0472;
      line.m_patternOffset = new OdGeVector2d(0, 0.866);
      stars.Add(line);
      line.m_dLineAngle = 2.0944;
      line.m_basePoint = new OdGePoint2d(0.25, 0.433);
      line.m_patternOffset = new OdGeVector2d(0, 0.866);
      stars.Add(line);

      /**********************************************************************/
      /* Register the pattern                                               */
      /**********************************************************************/
      pDb.appServices().patternManager().appendPattern(OdDbHatch.HatchPatternType.kCustomDefined,
        "MY_STARS", stars);

      /**********************************************************************/
      /* Create an associative custom defined hatch                         */
      /**********************************************************************/
      pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(pDb);
      hatchId = bBTR.appendOdDbEntity(pHatch);

      /**********************************************************************/
      /* Set some properties                                                */
      /**********************************************************************/
      pHatch.setAssociative(true);
      pHatch.setDatabaseDefaults(pDb); // make hatch aware of DB for the next call
      pHatch.setPattern(OdDbHatch.HatchPatternType.kCustomDefined, "MY_STARS");
      pHatch.setPatternScale(0.125);
      pHatch.setPatternAngle(OdaToRadian(30.0));
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);

      /**********************************************************************/
      /* Define the loops                                                */
      /**********************************************************************/
      loopIds.Clear();
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(pDb);
      loopIds.Add(bBTR.appendOdDbEntity(pCircle));

      centerPt = EntityBoxes.getBoxCenter(boxRow, boxCol);
      centerPt.x += delta * 4.0;
      centerPt.y += delta;
      pCircle.setCenter(centerPt);
      pCircle.setRadius(delta * 1.5);

      /**********************************************************************/
      /* Append the loops to the hatch                                      */
      /**********************************************************************/
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, loopIds);

      try
      {
        /********************************************************************/
        /* Create an associative predefined hatch                           */
        /********************************************************************/
        pHatch = OdDbHatch.createObject();
        pHatch.setDatabaseDefaults(pDb);
        hatchId = bBTR.appendOdDbEntity(pHatch);

        /********************************************************************/
        /* Set some properties                                              */
        /********************************************************************/
        point = EntityBoxes.getBoxCenter(boxRow, boxCol);
        // Set the hatch properties.
        pHatch.setAssociative(true);
        pHatch.setDatabaseDefaults(pDb);// make hatch aware of DB for the next call
        pHatch.setPattern(OdDbHatch.HatchPatternType.kPreDefined, "ANGLE");
        pHatch.setPatternScale(0.5);
        pHatch.setPatternAngle(0.5); // near 30 degrees
        pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);


        /********************************************************************/
        /* Define the loops                                                 */
        /********************************************************************/
        loopIds.Clear();
        pCircle = OdDbCircle.createObject();
        pCircle.setDatabaseDefaults(pDb);
        loopIds.Add(bBTR.appendOdDbEntity(pCircle));
        centerPt.x -= delta * 2.0/* delta*3 */;
        centerPt.y -= delta * 2.5;
        pCircle.setCenter(centerPt);
        pCircle.setRadius(delta * 1.5);

        /********************************************************************/
        /* Append the loops to the hatch                                    */
        /********************************************************************/
        pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, loopIds);
      }
      catch (OdError e)
      {
        Console.WriteLine("\n\nException occurred: {0}", e.description());
        Console.WriteLine("\nHatch with predefined pattern \"ANGLE\" was not added.");
        Console.WriteLine("\nMake sure PAT file with pattern definition is available to Teigha.");
        Console.WriteLine("\nPress ENTER to continue...");
      }
    }

    /************************************************************************/
    /* Add an Arc Dimension to the specified BlockTableRecord               */
    /************************************************************************/
    void addArcDimension(OdDbObjectId btrId,
                                   int boxRow,
                                   int boxCol,
                                   OdDbObjectId layerId,
                                   OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Arc", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create an arc to be dimensioned                                    */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pArc);
      OdGePoint3d center = point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
      pArc.setCenter(center);
      pArc.setStartAngle(OdaToRadian(0.0));
      pArc.setEndAngle(OdaToRadian(90.0));
      pArc.setRadius(4.0 / Math.PI);


      /**********************************************************************/
      /* Create an ArcDimension                                             */
      /**********************************************************************/
      OdDbArcDimension pDimension = OdDbArcDimension.createObject();
      pDimension.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pDimension.setCenterPoint(pArc.center());
      pDimension.setArcPoint(pArc.center() + new OdGeVector3d(pArc.radius() + 0.45, 0.0, 0.0));

      OdGePoint3d startPoint = new OdGePoint3d();
      pArc.getStartPoint(startPoint);
      pDimension.setXLine1Point(startPoint);

      OdGePoint3d endPoint = new OdGePoint3d();
      pArc.getEndPoint(endPoint);
      pDimension.setXLine2Point(endPoint);

      pDimension.setArcSymbolType(1);

    }

    /************************************************************************/
    /* Add a 3 Point Angular Dimension to the specified BlockTableRecord    */
    /************************************************************************/
    void add3PointAngularDimension(OdDbObjectId btrId,
                                             int boxRow,
                                             int boxCol,
                                             OdDbObjectId layerId,
                                             OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "3 Point Angular", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create an arc to be dimensioned                                    */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pArc);
      OdGePoint3d center = point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
      pArc.setCenter(center);
      pArc.setStartAngle(OdaToRadian(0.0));
      pArc.setEndAngle(OdaToRadian(90.0));
      pArc.setRadius(w * 3.0 / 8.0);

      /**********************************************************************/
      /* Create 3 point angular dimension                                   */
      /**********************************************************************/
      OdDb3PointAngularDimension pDimension = OdDb3PointAngularDimension.createObject();
      pDimension.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      pDimension.setCenterPoint(pArc.center());
      pDimension.setArcPoint(pArc.center() + new OdGeVector3d(pArc.radius() + 0.45, 0.0, 0.0));

      OdGePoint3d startPoint = new OdGePoint3d();
      pArc.getStartPoint(startPoint);
      pDimension.setXLine1Point(startPoint);

      OdGePoint3d endPoint = new OdGePoint3d();
      pArc.getEndPoint(endPoint);
      pDimension.setXLine2Point(endPoint);
    }
    /************************************************************************/
    /* Add a 2 Line Angular Dimension to the specified BlockTableRecord     */
    /************************************************************************/
    void add2LineAngularDimension(OdDbObjectId btrId,
                                             int boxRow,
                                             int boxCol,
                                             OdDbObjectId layerId,
                                             OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "2 Line Angular", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create the lines to be dimensioned                                 */
      /**********************************************************************/
      OdGePoint3d center = point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
      OdGeVector3d v1 = new OdGeVector3d(w * 1.0 / 8.0, 0.0, 0.0);
      OdGeVector3d v2 = new OdGeVector3d(w * 4.0 / 8.0, 0.0, 0.0);
      OdGeVector3d v3 = v2 + new OdGeVector3d(0.45, 0.0, 0.0);

      OdDbLine pLine1 = OdDbLine.createObject();
      pLine1.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pLine1);
      pLine1.setStartPoint(center + v1);
      pLine1.setEndPoint(center + v2);

      double rot = OdaToRadian(75.0);
      v1.rotateBy(rot, OdGeVector3d.kZAxis);
      v2.rotateBy(rot, OdGeVector3d.kZAxis);

      OdDbLine pLine2 = OdDbLine.createObject();
      pLine2.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pLine2);
      pLine2.setStartPoint(center + v1);
      pLine2.setEndPoint(center + v2);

      /**********************************************************************/
      /* Create 2 Line Angular Dimensionn                                   */
      /**********************************************************************/
      OdDb2LineAngularDimension pDimension = OdDb2LineAngularDimension.createObject();
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/

      v3.rotateBy(rot / 2.0, OdGeVector3d.kZAxis);
      pDimension.setArcPoint(center + v3);

      OdGePoint3d startPoint = new OdGePoint3d();
      pLine1.getStartPoint(startPoint);
      pDimension.setXLine1Start(startPoint);

      OdGePoint3d endPoint = new OdGePoint3d();
      pLine1.getEndPoint(endPoint);
      pDimension.setXLine1End(endPoint);

      //  pDimension.setArcPoint(endPoint + 0.45*(endPoint - startPoint).normalize());

      pLine2.getStartPoint(startPoint);
      pDimension.setXLine2Start(startPoint);

      pLine2.getEndPoint(endPoint);
      pDimension.setXLine2End(endPoint);
    }
    /************************************************************************/
    /* Add a RadialDimensionLarge to the specified BlockTableRecord         */
    /************************************************************************/
    void addRadialDimensionLarge(OdDbObjectId btrId,
                                           int boxRow,
                                           int boxCol,
                                           OdDbObjectId layerId,
                                           OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Radial", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dim Large", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create an arc to be dimensioned                                    */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pArc);

      OdGePoint3d center = point + new OdGeVector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
      pArc.setRadius(2.0);

      pArc.setCenter(center);
      pArc.setStartAngle(OdaToRadian(30.0));
      pArc.setEndAngle(OdaToRadian(90.0));

      /**********************************************************************/
      /* Create RadialDimensionLarge                                        */
      /**********************************************************************/
      OdDbRadialDimensionLarge pDimension = OdDbRadialDimensionLarge.createObject();
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      OdGePoint3d centerPoint, chordPoint, overrideCenter, jogPoint, textPosition;

      // The centerPoint of the dimension is the center of the arc
      centerPoint = pArc.center();

      // The chordPoint of the dimension is the midpoint of the arc
      chordPoint = centerPoint +
        new OdGeVector3d(pArc.radius(), 0.0, 0.0).rotateBy(0.5 * (pArc.startAngle() + pArc.endAngle()), OdGeVector3d.kZAxis);

      // The overrideCenter is just to the right of the actual center
      overrideCenter = centerPoint + new OdGeVector3d(w * 3.0 / 8.0, 0.0, 0.0);

      // The jogPoint is halfway between the overrideCenter and the chordCoint
      jogPoint = overrideCenter + new OdGeVector3d(chordPoint - overrideCenter) * 0.5;

      // The textPosition is along the vector between the centerPoint and the chordPoint.
      textPosition = centerPoint + new OdGeVector3d(chordPoint - centerPoint) * 0.7;

      double jogAngle = OdaToRadian(45.0);

      pDimension.setCenter(centerPoint);
      pDimension.setChordPoint(chordPoint);
      pDimension.setOverrideCenter(overrideCenter);
      pDimension.setJogPoint(jogPoint);
      pDimension.setTextPosition(textPosition);
      pDimension.setJogAngle(jogAngle);

    }
    /************************************************************************/
    /* Add Ordinate Dimensions to the specified BlockTableRecord            */
    /************************************************************************/
    void addOrdinateDimensions(OdDbObjectId btrId,
                                         int boxRow,
                                         int boxCol,
                                         OdDbObjectId layerId,
                                         OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Ordinate", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "Dimension", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      double dx = w / 8.0;
      double dy = h / 8.0;
      /**********************************************************************/
      /* Create a line to be dimensioned                                    */
      /**********************************************************************/
      OdDbLine pLine = OdDbLine.createObject();
      pLine.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pLine);

      OdGePoint3d point1 = point + new OdGeVector3d(dx, dy, 0.0);
      OdGePoint3d point2 = point1 + new OdGeVector3d(0.0, 1.5, 0);
      pLine.setStartPoint(point1);
      pLine.setEndPoint(point2);

      /**********************************************************************/
      /* Create the base ordinate dimension                                 */
      /**********************************************************************/
      OdDbOrdinateDimension pDimension = OdDbOrdinateDimension.createObject();
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/

      OdGePoint3d startPoint = new OdGePoint3d();
      OdGePoint3d endPoint = new OdGePoint3d();
      pLine.getStartPoint(startPoint);
      pLine.getEndPoint(endPoint);

      OdGePoint3d leaderEndPoint = startPoint + new OdGeVector3d(3.0 * dx, 0, 0.0);
      pDimension.setOrigin(startPoint);
      pDimension.setDefiningPoint(startPoint);
      pDimension.setLeaderEndPoint(leaderEndPoint);
      pDimension.useYAxis();

      /**********************************************************************/
      /* Create an ordinate dimension                                       */
      /**********************************************************************/
      pDimension = OdDbOrdinateDimension.createObject();
      bBTR.appendOdDbEntity(pDimension);

      /**********************************************************************/
      /* Use the default dim variables                                      */
      /**********************************************************************/
      pDimension.setDatabaseDefaults(pDb);

      /**********************************************************************/
      /* Set some parameters                                                */
      /**********************************************************************/
      leaderEndPoint = endPoint + new OdGeVector3d(3.0 * dx, -dy, 0.0);

      pDimension.setOrigin(startPoint);
      pDimension.setDefiningPoint(endPoint);
      pDimension.setLeaderEndPoint(leaderEndPoint);
      pDimension.useYAxis();
    }
    /************************************************************************/
    /* Add a Spline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addSpline(OdDbObjectId btrId,
                             int boxRow,
                             int boxCol,
                             OdDbObjectId layerId,
                             OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "SPLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create Spline                                                      */
      /**********************************************************************/
      OdDbSpline pSpline = OdDbSpline.createObject();
      pSpline.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pSpline);

      /**********************************************************************/
      /* Create the fit points                                              */
      /**********************************************************************/

      double dx = w / 8.0;
      double dy = h / 8.0;

      OdGePoint3dArray fitPoints = new OdGePoint3dArray();
      fitPoints.Add(point + new OdGeVector3d(1.0 * dx, 1.0 * dy, 0.0));
      fitPoints.Add(point + new OdGeVector3d(3.0 * dx, 6.0 * dy, 0.0));
      fitPoints.Add(point + new OdGeVector3d(4.0 * dx, 2.0 * dy, 0.0));
      fitPoints.Add(point + new OdGeVector3d(7.0 * dx, 7.0 * dy, 0.0));

      pSpline.setFitData(
        fitPoints,                    // Fit Points
        3,                            // Degree
        0.0,                          // Fit tolerance
        new OdGeVector3d(0.0, 0.0, 0.0),  // startTangent
        new OdGeVector3d(1.0, 0.0, 0.0)); // endTangent
    }
    /************************************************************************/
    /* Add some Traces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addTraces(OdDbObjectId btrId,
                             int boxRow,
                             int boxCol,
                             OdDbObjectId layerId,
                             OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "TRACEs", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a Trace                                                     */
      /**********************************************************************/
      OdDbTrace pTrace = OdDbTrace.createObject();
      pTrace.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pTrace);

      double dx = w / 8.0;
      double dy = h / 8.0;
      pTrace.setPointAt(0, point + new OdGeVector3d(1.0 * dx, 2.0 * dx, 0.0));
      pTrace.setPointAt(1, point + new OdGeVector3d(1.0 * dx, 1.0 * dx, 0.0));
      pTrace.setPointAt(2, point + new OdGeVector3d(6.0 * dx, 2.0 * dx, 0.0));
      pTrace.setPointAt(3, point + new OdGeVector3d(7.0 * dx, 1.0 * dx, 0.0));

      /**********************************************************************/
      /* Create a Trace                                                     */
      /**********************************************************************/
      pTrace = OdDbTrace.createObject();
      pTrace.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pTrace);

      pTrace.setPointAt(0, point + new OdGeVector3d(6.0 * dx, 2.0 * dx, 0.0));
      pTrace.setPointAt(1, point + new OdGeVector3d(7.0 * dx, 1.0 * dx, 0.0));
      pTrace.setPointAt(2, point + new OdGeVector3d(6.0 * dx, 7.0 * dy, 0.0));
      pTrace.setPointAt(3, point + new OdGeVector3d(7.0 * dx, 7.0 * dy, 0.0));

    }
    /************************************************************************/
    /* Add an Mline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addMLine(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);
      OdDbDatabase pDb = btrId.database();

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the labels                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "MLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of MLine                                 */
      /**********************************************************************/
      point.x += w / 10.0;
      point.y -= h / 2;

      /**********************************************************************/
      /* Create an MLine and add it to the database                         */
      /**********************************************************************/
      OdDbMline pMLine = OdDbMline.createObject();
      pMLine.setDatabaseDefaults(pDb);
      bBTR.appendOdDbEntity(pMLine);

      /**********************************************************************/
      /* Open the MLineStyle dictionary, and set the style                  */
      /**********************************************************************/
      OdDbDictionary pMLDic = (OdDbDictionary)pDb.getMLStyleDictionaryId().safeOpenObject();
      pMLine.setStyle(pMLDic.getAt("OdaStandard"));

      /**********************************************************************/
      /* Add some segments                                                  */
      /**********************************************************************/
      point.y -= h / 2.2;
      pMLine.appendSeg(point);

      point.y += h / 3.0;
      pMLine.appendSeg(point);

      point.y += h / 5.0;
      point.x += w / 4.0;
      pMLine.appendSeg(point);

      point.x += w / 4.0;
      pMLine.appendSeg(point);

      point.y += h / 3.0;
      pMLine.appendSeg(point);

      point.x += w / 3;
      pMLine.appendSeg(point);

      point.y -= h / 2;
      pMLine.appendSeg(point);

      point.x -= w / 4;
      point.y -= h / 3;
      pMLine.appendSeg(point);
    }

    /************************************************************************/
    /* Add a Polyline to the specified BlockTableRecord                     */
    /************************************************************************/
    void addPolyline(OdDbObjectId btrId,
                               int boxRow,
                               int boxCol,
                               OdDbObjectId layerId,
                               OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "LWPOLYLINE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a polyline                                                  */
      /**********************************************************************/
      OdDbPolyline pPolyline = OdDbPolyline.createObject();
      pPolyline.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pPolyline);

      /**********************************************************************/
      /* Create the vertices                                                */
      /**********************************************************************/

      double dx = w / 8.0;
      double dy = h / 8.0;

      OdGePoint2d point2d = new OdGePoint2d(point.x + 1.5 * dx, point.y + 3.0 * dy);

      pPolyline.addVertexAt(0, point2d);

      point2d.y -= 0.5 * dy;
      pPolyline.addVertexAt(1, point2d);
      pPolyline.setBulgeAt(1, 1.0);

      point2d.x += 5.0 * dx;
      pPolyline.addVertexAt(2, point2d);

      point2d.y += 4.0 * dy;
      pPolyline.addVertexAt(3, point2d);

      point2d.x -= 1.0 * dx;
      pPolyline.addVertexAt(4, point2d);

      point2d.y -= 4.0 * dy;
      pPolyline.addVertexAt(5, point2d);
      pPolyline.setBulgeAt(5, -1.0);

      point2d.x -= 3.0 * dx;
      pPolyline.addVertexAt(6, point2d);

      point2d.y += 0.5 * dy;
      pPolyline.addVertexAt(7, point2d);

      pPolyline.setClosed(true);
    }

    /************************************************************************/
    /* Add Arc Aligned Text to the specified BlockTableRecord               */
    /************************************************************************/
    void addArcText(OdDbObjectId btrId,
                              int boxRow,
                              int boxCol,
                              OdDbObjectId layerId,
                              OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      //  double w    = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "ARCALIGNED-", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      addTextEnt(bBTR,
        point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
        "TEXT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);


      /**********************************************************************/
      /* Create an arc                                                       */
      /**********************************************************************/
      OdDbArc pArc = OdDbArc.createObject();
      pArc.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pArc);

      //  double dx   = w / 8.0;
      double dy = h / 8.0;

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol) + new OdGeVector3d(0.0, -2.0 * dy, 0);
      pArc.setCenter(center);
      pArc.setRadius(3.0 * dy);
      pArc.setStartAngle(OdaToRadian(45.0));
      pArc.setEndAngle(OdaToRadian(135.0));

      /**********************************************************************/
      /* Create the ArcAlignedText                                          */
      /**********************************************************************/
      OdDbArcAlignedText pArcText = OdDbArcAlignedText.createObject();
      pArcText.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pArcText);

      pArcText.setTextString("ArcAligned");
      pArcText.setArcId(pArc.objectId());
      pArcText.setTextStyle(styleId);
    }

    /************************************************************************/
    /* Add a Wipeout to to the specified BlockTableRecord                   */
    /************************************************************************/
    void addWipeout(OdDbObjectId btrId,
                              int boxRow,
                              int boxCol,
                              OdDbObjectId layerId,
                              OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the lower-left corner and center of the box                    */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                     */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "WIPEOUT", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Get the lower-left corner of the box                               */
      /**********************************************************************/
      point.y -= h;

      /**********************************************************************/
      /* Create a hatch object to be wiped out                              */
      /**********************************************************************/
      OdDbHatch pHatch = OdDbHatch.createObject();
      pHatch.setDatabaseDefaults(bBTR.database());
      OdDbObjectId hatchId = bBTR.appendOdDbEntity(pHatch);

      /**********************************************************************/
      /* Create a hatch object to be wiped out                              */
      /**********************************************************************/
      pHatch.setAssociative(true);
      pHatch.setPattern(OdDbHatch.HatchPatternType.kUserDefined, "_USER");
      pHatch.setPatternSpace(0.125);
      pHatch.setPatternAngle(0.5); // near 30 degrees
      pHatch.setPatternDouble(true); // Cross hatch
      pHatch.setHatchStyle(OdDbHatch.HatchStyle.kNormal);

      /**********************************************************************/
      /* Create an outer loop for the hatch                                 */
      /**********************************************************************/
      OdDbCircle pCircle = OdDbCircle.createObject();
      pCircle.setDatabaseDefaults(bBTR.database());
      OdDbObjectIdArray loopIds = new OdDbObjectIdArray();
      loopIds.Add(bBTR.appendOdDbEntity(pCircle));
      pCircle.setCenter(center);
      pCircle.setRadius(Math.Min(w, h) * 0.4);
      pHatch.appendLoop((int)OdDbHatch.HatchLoopType.kDefault, loopIds);

      /**********************************************************************/
      /* Create the wipeout                                                  */
      /**********************************************************************/
      OdDbWipeout pWipeout = OdDbWipeout.createObject();
      pWipeout.setDatabaseDefaults(bBTR.database());
      bBTR.appendOdDbEntity(pWipeout);

      OdGePoint3dArray boundary = new OdGePoint3dArray();
      boundary.Add(center + new OdGeVector3d(-w * 0.4, -h * 0.4, 0.0));
      boundary.Add(center + new OdGeVector3d(w * 0.4, -h * 0.4, 0.0));
      boundary.Add(center + new OdGeVector3d(0.0, h * 0.4, 0.0));
      boundary.Add(center + new OdGeVector3d(-w * 0.4, -h * 0.4, 0.0));

      pWipeout.setBoundary(boundary);

      pWipeout.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShow, true);
      pWipeout.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kClip, true);
      pWipeout.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kShowUnAligned, true);
      pWipeout.setDisplayOpt(OdDbRasterImage.ImageDisplayOpt.kTransparent, false);
    }

    /************************************************************************/
    /* Add a Table to the specified BlockTableRecord                        */
    /************************************************************************/
    void addTable(OdDbObjectId btrId,
                            OdDbObjectId addedBlockId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord pRecord = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the lower-left corner and center of the box                    */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Create the Table                                                  */
      /**********************************************************************/
      OdDbTable pAcadTable = OdDbTable.createObject();
      OdDbObjectId tableId = pRecord.appendOdDbEntity(pAcadTable);
      /**********************************************************************/
      /* This entity will later get its own viewport                        */
      /**********************************************************************/
      m_layoutEntities.Add(tableId);

      /**********************************************************************/
      /* Set the parameters                                                 */
      /**********************************************************************/
      pAcadTable.setDatabaseDefaults(pRecord.database());
      pAcadTable.setNumColumns(3);
      pAcadTable.setNumRows(4);

      pAcadTable.generateLayout();
      pAcadTable.setColumnWidth(w / pAcadTable.numColumns());
      pAcadTable.setRowHeight(h / pAcadTable.numRows());

      pAcadTable.setPosition(point);
      pAcadTable.setTextStyle(styleId);

      pAcadTable.setTextHeight(0.500 * pAcadTable.rowHeight(0), (uint)RowType.kTitleRow);
      pAcadTable.setTextHeight(0.300 * pAcadTable.rowHeight(1), (uint)RowType.kHeaderRow);
      pAcadTable.setTextHeight(0.250 * pAcadTable.rowHeight(2), (uint)RowType.kDataRow);

      /**********************************************************************/
      /* Set the alignments                                                 */
      /**********************************************************************/
      for (uint row = 1; row < pAcadTable.numRows(); row++)
      {
        for (uint col = 0; col < pAcadTable.numColumns(); col++)
        {
          pAcadTable.setAlignment(row, col, CellAlignment.kMiddleCenter);
        }
      }

      /**********************************************************************/
      /* Define the title row                                               */
      /**********************************************************************/
      pAcadTable.mergeCells(0, 0, 0, pAcadTable.numColumns() - 1);
      pAcadTable.setTextString(0, 0, "Title of TABLE");

      /**********************************************************************/
      /* Define the header row                                              */
      /**********************************************************************/
      pAcadTable.setTextString(1, 0, "Header0");
      pAcadTable.setTextString(1, 1, "Header1");
      pAcadTable.setTextString(1, 2, "Header2");

      /**********************************************************************/
      /* Define the first data row                                          */
      /**********************************************************************/
      pAcadTable.setTextString(2, 0, "Data0");
      pAcadTable.setTextString(2, 1, "Data1");
      pAcadTable.setTextString(2, 2, "Data2");

      /**********************************************************************/
      /* Define the second data row                                         */
      /**********************************************************************/
      pAcadTable.setCellType(3, 0, CellType.kBlockCell);
      pAcadTable.setBlockTableRecordId(3, 0, addedBlockId);
      pAcadTable.setBlockScale(3, 0, 1.0);
      pAcadTable.setAutoScale(3, 0, true);
      pAcadTable.setBlockRotation(3, 0, 0.0);

      pAcadTable.setTextString(3, 1, "<-Block Cell.");

      pAcadTable.setCellType(3, 2, CellType.kBlockCell);
      pAcadTable.setBlockTableRecordId(3, 2, addedBlockId);
      pAcadTable.setAutoScale(3, 2, true);
      pAcadTable.setBlockRotation(3, 2, OdaToRadian(30.0));

      pAcadTable.recomputeTableBlock();

      /**********************************************************************/
      /* Add the label                                                     */
      /**********************************************************************/
      addTextEnt(pRecord,
        point + m_textOffset, point + m_textOffset,
        "ACAD_TABLE", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);
    }

    /************************************************************************/
    /* Add a Text with Field to the specified BlockTableRecord              */
    /************************************************************************/
    void addTextWithField(OdDbObjectId btrId,
                            int boxRow,
                            int boxCol,
                            OdDbObjectId layerId,
                            OdDbObjectId styleId,
                            OdDbObjectId noteStyleId)
    {
      OdDbBlockTableRecord pRecord = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      //  double dx = w/16.0;
      //  double dy = h/12.0;

      OdGePoint3d textPos1 = new OdGePoint3d(point);
      textPos1.x += w / 15.0;
      textPos1.y -= h / 3.0;

      OdGePoint3d textPos2 = new OdGePoint3d(point);
      textPos2.x += w / 15.0;
      textPos2.y -= 2.0 * h / 3.0;

      double textHeight = EntityBoxes.getHeight() / 12.0;

      /**********************************************************************/
      /* Prepare the text entities                                           */
      /**********************************************************************/
      OdDbText pText1 = OdDbText.createObject();
      OdDbObjectId textId = pRecord.appendOdDbEntity(pText1);
      OdDbText pText2 = OdDbText.createObject();
      OdDbObjectId textId2 = pRecord.appendOdDbEntity(pText2);

      pText1.setPosition(textPos1);
      pText1.setHeight(textHeight);
      pText2.setPosition(textPos2);
      pText2.setHeight(textHeight);
      if (!styleId.isNull())
      {
        pText1.setTextStyle(styleId);
        pText2.setTextStyle(styleId);
      }

      /**********************************************************************/
      /* Create field objects                                               */
      /**********************************************************************/
      OdDbField pTextField1 = OdDbField.createObject();
      OdDbField pField1_1 = OdDbField.createObject();

      OdDbField pTextField2 = OdDbField.createObject();
      OdDbField pField2_1 = OdDbField.createObject();
      OdDbField pField2_2 = OdDbField.createObject();

      /**********************************************************************/
      /* Set field objects                                                  */
      /**********************************************************************/
      OdDbObjectId textFldId1 = pText1.setField("TEXT", pTextField1);
      OdDbObjectId fldId1_1 = pTextField1.setField("", pField1_1);

      OdDbObjectId textFldId2 = pText2.setField("TEXT", pTextField2);

      /**********************************************************************/
      /* Set field property                                                 */
      /**********************************************************************/

      pField1_1.setEvaluationOption(OdDbField.EvalOption.kAutomatic);
      String fc1 = "\\AcVar Comments";
      pField1_1.setFieldCode(fc1);

      pTextField1.setEvaluationOption(OdDbField.EvalOption.kAutomatic);
      String fc2 = "%<\\_FldIdx 0>%";
      pTextField1.setFieldCode(fc2, OdDbField.FieldCodeFlag.kTextField | OdDbField.FieldCodeFlag.kPreserveFields);

      /**********************************************************************/
      /* Evaluate field                                                     */
      /**********************************************************************/
      pField1_1.evaluate((int)OdDbField.EvalContext.kDemand);

      OdDbFieldArray fldArray = new OdDbFieldArray();
      fldArray.Add(pField2_1);
      fldArray.Add(pField2_2);

      pTextField2.setEvaluationOption(OdDbField.EvalOption.kAutomatic);
      String fc3 = "Date %<\\_FldIdx 0>% Time %<\\_FldIdx 1>%";
      pTextField2.setFieldCode(fc3, OdDbField.FieldCodeFlag.kTextField, fldArray);

      pField2_1.setEvaluationOption(OdDbField.EvalOption.kAutomatic);
      String fc4 = "\\AcVar Date \\f M/dd/yyyy";
      pField2_1.setFieldCode(fc4);

      pField2_2.setEvaluationOption(OdDbField.EvalOption.kAutomatic);
      String fc5 = "\\AcVar Date \\f h:mm tt";
      pField2_2.setFieldCode(fc5);

      /**********************************************************************/
      /* Evaluate fields                                                    */
      /**********************************************************************/
      pField2_1.evaluate((int)OdDbField.EvalContext.kDemand);
      pField2_2.evaluate((int)OdDbField.EvalContext.kDemand);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(pRecord,
        point + m_textOffset, point + m_textOffset,
        "FIELDS", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, noteStyleId);
    }

    /**********************************************************************/
    /* Save persistent OLE object data to file that can be used later for */
    /* creating OLE2 frame entity on any platform.                        */
    /**********************************************************************/
    void saveOleData(string fileName, OdOleItemHandler pHandler)
    {
      try
      {
        OdStreamBuf pFile = TD_Db.odSystemServices().createFile(fileName,
          FileAccessMode.kFileWrite,
          FileShareMode.kShareDenyReadWrite,
          FileCreationDisposition.kCreateAlways);
        pHandler.getCompoundDocument(pFile);
      }
      catch (OdError) //ignore errors
      {
      }
    }


    /************************************************************************/
    /* Prefix a file name with the Current Directory                        */
    /************************************************************************/
    static String inCurrentFolder(String fileName)
    {
      if (fileName.IndexOfAny(new char[] { '\\', '/' }) == -1)
      {
        String sPath = System.IO.Directory.GetCurrentDirectory();
        sPath.TrimEnd(new char[] { '\\', '/' });
        sPath += '/';
        sPath += fileName;
        return sPath;
      }
      else
      {
        return fileName;
      }
    }

    /************************************************************************/
    /* Add an OLE object to the specified BlockTableRecord                  */
    /************************************************************************/
    void addOLE2FrameFromFile(OdDbObjectId btrId,
                                        int boxRow,
                                        int boxCol,
                                        string fileName,
                                        OdDbObjectId layerId,
                                        OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)btrId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the lower-left corner and center of the box                    */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);
      OdDbOle2Frame pOle2Frame = null;
  /**********************************************************************/
  /* Create an ole2frame entity from arbitrary file using Windows OLE RT*/
  /**********************************************************************/
  // pOle2Frame = OleWrappers.CreateFromFile(inCurrentFolder(fileName));

      /**********************************************************************/
      /* Create an ole2frame entity in a platform-neutral manner            */
      /* Important: open file that is a compound document.                  */
      /* OLE2 frame can't be created from arbitrary file such way.          */
      /**********************************************************************/

      try
      {
        OdStreamBuf pFile =
          TD_Db.odSystemServices().createFile(fileName + ".ole", FileAccessMode.kFileRead,
                                        FileShareMode.kShareDenyReadWrite, FileCreationDisposition.kOpenExisting);

        pOle2Frame = OdDbOle2Frame.createObject();

        OdOleItemHandler pHandler = pOle2Frame.getItemHandler();

        pHandler.setCompoundDocument((uint)pFile.length(), pFile);

        pHandler.setDrawAspect(OdOleItemHandler.DvAspect.kContent);

        pOle2Frame.unhandled_setHimetricSize(6879, 3704);
        ///////////////////////////////////////// Embed Raster Services test start ///////////////////////////
        // if to uncomment the code below xls file in Ole object will be replaced by OdWriteEx.jpg file
        // the key difference of this image embedding is that this image will be exported to, for example, 
        // pdf file without saving the file to disk and reloading it. it is possible to call export procedure
        // just after the filling procedure is finished
        //String filePath = "OdWriteEx.jpg";
        //OdRxRasterServices pRasSvcs = (OdRxRasterServices)Teigha.Core.Globals.odrxDynamicLinker().loadApp("RxRasterServices");
        //if (null == pRasSvcs)
        //{
        //    Console.WriteLine("Failed to load raster services...");
        //}
        //else
        //{
        //    Console.WriteLine("Raster services loaded");
        //    OdGiRasterImage pImage = pRasSvcs.loadRasterImage(filePath);
        //    if (null == pImage)
        //    {
        //        Console.WriteLine("Failed to load raster image from file " + filePath);
        //    }
        //    else
        //    {
        //        if (!pHandler.embedRaster(pImage))
        //        {
        //            Console.WriteLine("embedRaster is unsupported...");
        //        }
        //    }
        //}
        ///////////////////////////////////////// Embed Raster Services test stop ////////////////////////////
      }
      catch (OdError)
      {
        Console.WriteLine("Ole file: {0} not found, no OdDbOle2Frame entity created.\n", fileName);
      }
      if (pOle2Frame != null)
      {
        pOle2Frame.setDatabaseDefaults(pBlock.database());
        pBlock.appendOdDbEntity(pOle2Frame);

        /**********************************************************************/
        /* Add the label                                                      */
        /**********************************************************************/
        addTextEnt(pBlock, point + m_textOffset, point + m_textOffset,
          "OLE2: " + pOle2Frame.getUserType(), m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);


        /**********************************************************************/
        /* Inscribe OLE frame in entity box                                   */
        /**********************************************************************/
        h += m_textOffset.y;
        h -= (m_textSize * 1.5);
        center.y += (m_textOffset.y / 2.0);
        center.y -= (m_textSize * 1.5 / 2.0);

        h *= 0.95;
        w *= 0.95;

        h /= 2.0;
        w /= 2.0;

        double oh = pOle2Frame.unhandled_himetricHeight();
        double ow = pOle2Frame.unhandled_himetricWidth();
        if (oh / ow < h / w)
        {
          h = w * oh / ow;
        }
        else
        {
          w = h * ow / oh;
        }

        OdRectangle3d rect = new OdRectangle3d();
        rect.upLeft.x = rect.lowLeft.x = center.x - w;
        rect.upLeft.y = rect.upRight.y = center.y + h;
        rect.upRight.x = rect.lowRight.x = center.x + w;
        rect.lowLeft.y = rect.lowRight.y = center.y - h;
        pOle2Frame.setPosition(rect);
      }
    }

    void addDwfUnderlay(OdDbObjectId blockId,
                                  int boxRow,
                                  int boxCol,
                                  OdDbObjectId layerId,
                                  OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)blockId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();
      double w = EntityBoxes.getWidth(boxRow, boxCol);

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Dwf reference", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create the Dwf definition                                          */
      /**********************************************************************/
      OdDbDwfDefinition pDwfDef = OdDbDwfDefinition.createObject();
      String itemName = "Unsaved Drawing-Model";
      pDwfDef.setSourceFileName("OdWriteEx.dwf");
      pDwfDef.setItemName(itemName);

      // Post to database
      OdDbObjectId idDef = pDwfDef.postDefinitionToDb(blockId.database(),
                                                          "OdWriteEx - " + itemName);

      /**********************************************************************/
      /* Create the Dwf reference                                           */
      /**********************************************************************/
      OdDbDwfReference pDwfRef = OdDbDwfReference.createObject();
      pDwfRef.setDatabaseDefaults(bBTR.database());

      /**********************************************************************/
      /* Add the Dwf reference to the database                              */
      /**********************************************************************/
      bBTR.appendOdDbEntity(pDwfRef);

      /**********************************************************************/
      /* Set the Dwf reference's parameters                                 */
      /**********************************************************************/
      pDwfRef.setDefinitionId(idDef);
      pDwfRef.setPosition(point + new OdGeVector3d(-w / 4, -h / 2, 0.0));
      pDwfRef.setScaleFactors(new OdGeScale3d(0.001));
    }

    void addDgnUnderlay(OdDbObjectId blockId,
                                  int boxRow,
                                  int boxCol,
                                  OdDbObjectId layerId,
                                  OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)blockId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Dgn reference", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create the Dwf definition                                          */
      /**********************************************************************/
      OdDbDgnDefinition pDgnDef = OdDbDgnDefinition.createObject();
      String itemName = "Model";
      pDgnDef.setSourceFileName("OdWriteEx.dgn");
      pDgnDef.setItemName(itemName);

      // Post to database
      OdDbObjectId idDef = pDgnDef.postDefinitionToDb(blockId.database(),
                                                          "OdWriteEx - " + itemName);

      /**********************************************************************/
      /* Create the Dgn reference                                           */
      /**********************************************************************/
      OdDbDgnReference pDgnRef = OdDbDgnReference.createObject();
      pDgnRef.setDatabaseDefaults(bBTR.database());

      /**********************************************************************/
      /* Add the Dgn reference to the database                              */
      /**********************************************************************/
      bBTR.appendOdDbEntity(pDgnRef);

      /**********************************************************************/
      /* Set the Dgn reference's parameters                                 */
      /**********************************************************************/
      pDgnRef.setDefinitionId(idDef);
      pDgnRef.setPosition(point + new OdGeVector3d(0.0, -h, 0.0));
      pDgnRef.setScaleFactors(new OdGeScale3d(0.0004));
    }

    void addPdfUnderlay(OdDbObjectId blockId,
                                  int boxRow,
                                  int boxCol,
                                  OdDbObjectId layerId,
                                  OdDbObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      OdDbBlockTableRecord bBTR = (OdDbBlockTableRecord)blockId.safeOpenObject(OpenMode.kForWrite);

      /**********************************************************************/
      /* Get the Upper-left corner of the box and its size                  */
      /**********************************************************************/
      OdGePoint3d point = EntityBoxes.getBox(boxRow, boxCol);
      double h = EntityBoxes.getHeight();

      /**********************************************************************/
      /* Add the label                                                      */
      /**********************************************************************/
      addTextEnt(bBTR,
        point + m_textOffset, point + m_textOffset,
        "Pdf reference", m_textSize, TextHorzMode.kTextLeft, TextVertMode.kTextTop, layerId, styleId);

      /**********************************************************************/
      /* Create the Pdf definition                                          */
      /**********************************************************************/
      OdDbPdfDefinition pPdfDef = OdDbPdfDefinition.createObject();
      String itemName = "1";
      pPdfDef.setSourceFileName("OdWriteEx.pdf");
      pPdfDef.setItemName(itemName);

      // Post to database
      OdDbObjectId idDef = pPdfDef.postDefinitionToDb(blockId.database(),
                                                          "OdWriteEx - " + itemName);

      /**********************************************************************/
      /* Create the Pdf reference                                           */
      /**********************************************************************/
      OdDbPdfReference pPdfRef = OdDbPdfReference.createObject();
      pPdfRef.setDatabaseDefaults(bBTR.database());

      /**********************************************************************/
      /* Add the Pdf reference to the database                              */
      /**********************************************************************/
      bBTR.appendOdDbEntity(pPdfRef);

      /**********************************************************************/
      /* Set the Pdf reference's parameters                                 */
      /**********************************************************************/
      pPdfRef.setDefinitionId(idDef);
      pPdfRef.setPosition(point + new OdGeVector3d(0.0, -h, 0.0));
      pPdfRef.setScaleFactors(new OdGeScale3d(0.2));
    }

    void UndoSample(OdDbDatabase pDb)
    {
      MemoryManager mMan = MemoryManager.GetMemoryManager();
      MemoryTransaction mStartTr = mMan.StartTransaction();
      MemoryTransaction mTr = mMan.StartTransaction();
      try
      {
        pDb.disableUndoRecording(false);
        pDb.startUndoRecord(); //First Time
        {

          //b) We have put getBlockTableId().safeOpenObject() call in the undo record scope.
          using (OdDbBlockTableRecord pBlockTableRecord = (OdDbBlockTableRecord)pDb.getModelSpaceId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
          {
            OdDbLine pLine = OdDbLine.createObject();
            OdGePoint3d spoint = new OdGePoint3d();
            spoint.x = 66;
            spoint.y = 86;
            spoint.z = 0;
            OdGePoint3d epoint = new OdGePoint3d();
            epoint.x = 110;
            epoint.y = 86;
            epoint.z = 0;
            pLine.setStartPoint(spoint);
            pLine.setEndPoint(epoint);

            pLine.setDatabaseDefaults(pDb);
            pBlockTableRecord.appendOdDbEntity(pLine);

            // DownGrade Open
            pBlockTableRecord.downgradeOpen();
            pLine.Dispose();
            spoint.Dispose();
            epoint.Dispose();
            pLine = null;
            spoint = null;
            epoint = null;
          }
        }

        mMan.StopTransaction(mTr);
        mTr = mMan.StartTransaction();

        pDb.startUndoRecord();//Second Time
        {
          //b) We have put getBlockTableId().safeOpenObject() call in the undo record scope.
          using (OdDbBlockTableRecord pBlockTableRecord = (OdDbBlockTableRecord)pDb.getModelSpaceId().safeOpenObject(Teigha.TD.OpenMode.kForWrite))
          {
            OdDbLine pLine1 = OdDbLine.createObject();
            OdGePoint3d spoint1 = new OdGePoint3d();
            OdGePoint3d epoint1 = new OdGePoint3d();
            spoint1.x = 14;
            spoint1.y = 96;
            spoint1.z = 0;

            epoint1.x = 47;
            epoint1.y = 125;
            epoint1.z = 0;
            pLine1.setStartPoint(spoint1);
            pLine1.setEndPoint(epoint1);
            pLine1.setDatabaseDefaults(pDb);
            pBlockTableRecord.appendOdDbEntity(pLine1);

            //Downgrade Open
            pBlockTableRecord.downgradeOpen();
            pLine1.Dispose();
            spoint1.Dispose();
            epoint1.Dispose();
            pLine1 = null;
            spoint1 = null;
            epoint1 = null;
          }
        }
        //sharedViewInfo.GetDevice().invalidate();
        //sharedViewInfo.GetDevice().update();
        pDb.undo();
        //sharedViewInfo.GetDevice().invalidate();
        //sharedViewInfo.GetDevice().update();
        pDb.undo();
        //sharedViewInfo.GetDevice().invalidate();
        //sharedViewInfo.GetDevice().update();
        //return CadResult.EOk;
      }
      catch (OdError e)
      {
        //MessageBox.Show(err.description());
        //return CadResult.EOk;
        Console.WriteLine("UndoSample failed.\n");
        Console.WriteLine("\n\nException occurred: {0}", e.description());
      }
      mMan.StopTransaction(mTr);
      mMan.StopTransaction(mStartTr);
    }
  }
}
