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
using System.IO;
using Teigha.Core;
using Teigha.TG;
using System.Diagnostics;
using System.Reflection;

namespace ExDgnCreate
{
  class OdExDgnFiller : IDisposable
  {
    OdDgModel m_pModel3d;
    OdDgModel m_pModel2d;
    OdDgSheetModel m_pSheetModel;
    public void Dispose()
    {
      /*if (m_pModel3d != null)
      {
        m_pModel3d.Dispose();
        m_pModel3d = null;
      }
      if (m_pModel2d != null)
      {
        m_pModel2d.Dispose();
        m_pModel2d = null;
      }
      if (m_pSheetModel != null)
      {
        m_pSheetModel.Dispose();
        m_pSheetModel = null;
      }*/

    }
    void createEntityBoxes3d()
    {
      /**********************************************************************/
      /* Create a 2D polyline for each box                                  */
      /**********************************************************************/
      // TODO: set color & level
      for (int j = 0; j < EntityBoxes.VER_BOXES; j++)
      {
        for (int i = 0; i < EntityBoxes.HOR_BOXES; i++)
        {
          if (!EntityBoxes.isBox(j, i))
            break;

          double wCurBox = EntityBoxes.getWidth(j, i);
          OdGePoint3d currentPoint = EntityBoxes.getBox(j, i);

          OdDgLineString3d pLineString = OdDgLineString3d.createObject();

          OdGePoint3d pos = new OdGePoint3d(currentPoint.x, currentPoint.y, 0);
          pLineString.addVertex(pos);

          pos.x += wCurBox;
          pLineString.addVertex(pos);

          pos.y -= EntityBoxes.getHeight();
          pLineString.addVertex(pos);

          pos.x -= wCurBox;
          pLineString.addVertex(pos);

          pos.x = currentPoint.x;
          pos.y = currentPoint.y;
          pLineString.addVertex(pos);

          pLineString.setColorIndex((uint)OdCmEntityColor.ACIcolorMethod.kACIBlue);

          m_pModel3d.addElement(pLineString);
        }
      }
    }

    void createEntityBoxes2d(OdDgModel pModel)
    {
      /**********************************************************************/
      /* Create a 2D polyline for each box                                  */
      /**********************************************************************/
      // TODO: set color & level
      for (int j = 0; j < EntityBoxes.VER_BOXES; j++)
      {
        for (int i = 0; i < EntityBoxes.HOR_BOXES; i++)
        {
          if (!EntityBoxes.isBox(j, i))
            break;

          double wCurBox = EntityBoxes.getWidth(j, i);
          OdGePoint3d currentPoint = EntityBoxes.getBox(j, i);

          OdDgLineString2d pLineString = OdDgLineString2d.createObject();

          OdGePoint2d pos = new OdGePoint2d(currentPoint.x, currentPoint.y);
          pLineString.addVertex(pos);

          pos.x += wCurBox;
          pLineString.addVertex(pos);

          pos.y -= EntityBoxes.getHeight();
          pLineString.addVertex(pos);

          pos.x -= wCurBox;
          pLineString.addVertex(pos);

          pos.x = currentPoint.x;
          pos.y = currentPoint.y;
          pLineString.addVertex(pos);

          pLineString.setColorIndex((uint)OdCmEntityColor.ACIcolorMethod.kACIBlue);

          pModel.addElement(pLineString);
        }
      }
    }
    public void fillDatabase(OdDgDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      /********************************************************************/
      /* Fill the summary information                                     */
      /********************************************************************/
      fillSummaryInfo(pDb);

      /********************************************************************/
      /* Fill the model                                                   */
      /********************************************************************/

      m_pModel3d = (OdDgModel)pDb.getActiveModelId().safeOpenObject(OpenMode.kForWrite);

      //initialize measure units
      {
        OdDgModel.UnitDescription description = new OdDgModel.UnitDescription();
        OdDgModel.fillUnitDescriptor(OdDgModel.UnitMeasure.kMeters, description);
        m_pModel3d.setMasterUnit(description);
        OdDgModel.fillUnitDescriptor(OdDgModel.UnitMeasure.kMillimeters, description);
        m_pModel3d.setSubUnit(description);

        m_pModel3d.setWorkingUnit(OdDgModel.WorkingUnit.kWuMasterUnit);
      }
      // create additional 2d Model
      OdDgModelTable pModelTable = pDb.getModelTable();
      m_pModel2d = OdDgModel.createObject();
      m_pModel2d.setModelIs3dFlag(false);
      m_pModel2d.setName("Model for 2d elements");
      m_pModel2d.setWorkingUnit(OdDgModel.WorkingUnit.kWuMasterUnit);
      pModelTable.add(m_pModel2d);
      // create additional 2d Sheet Model
      m_pSheetModel = OdDgSheetModel.createObject();

      m_pSheetModel.setModelIs3dFlag(false);                        // Model 2d or 3d.
      m_pSheetModel.setName("Sheet Model");                         // Model name.
      m_pSheetModel.setDescription("Sheet Model example");          // Model description
      m_pSheetModel.setType(OdDgModel.Type.kSheetModel);               // Model type ( sheet or design ).
      m_pSheetModel.setBackground(0x00FFFFFF);            // Model background color.
      m_pSheetModel.setUseBackgroundColorFlag(true);                // Use background color value or not.
      m_pSheetModel.setWorkingUnit(OdDgModel.WorkingUnit.kWuMasterUnit);
      pModelTable.add(m_pSheetModel);

      // Sample page properties for sheet model.
      double dPaperWidth = 16.5;
      double dPaperHeight = 13.5;
      double dPaperXOfsset = -1.0;
      double dPaperYOfsset = -1.0;
      double dMarginOffset = 0.25;
      UInt32 uPageNumber = 17;
      double dAnotationScale = 2.0;

      m_pSheetModel.setDrawBorderFlag(true);
      m_pSheetModel.setDrawMarginsFlag(true);
      m_pSheetModel.setSheetRotation(0);
      m_pSheetModel.setSheetOffset(new OdGePoint2d(dPaperXOfsset, dPaperYOfsset));
      m_pSheetModel.setSheetNumber(uPageNumber);
      m_pSheetModel.setSheetAnnotationScale(dAnotationScale);
      m_pSheetModel.setPaper(OdDgSheetModel.PaperType.kCustom);
      m_pSheetModel.setSheetHeight(dPaperHeight);
      m_pSheetModel.setSheetWidth(dPaperWidth);
      m_pSheetModel.setSheetLeftMargin(dMarginOffset);
      m_pSheetModel.setSheetRightMargin(dMarginOffset);
      m_pSheetModel.setSheetTopMargin(dMarginOffset);
      m_pSheetModel.setSheetBottomMargin(dMarginOffset);

      fillModels();

      m_pModel3d.fitToView();
      m_pModel2d.fitToView();
      m_pSheetModel.fitToView();
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    void fillSummaryInfo(OdDgDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      // Summary Information
      OdDgSummaryInformation pSi = TG_Db.oddgGetSummaryInformation(pDb);
      pSi.setTitle("Title");
      pSi.setSubject("Subject");
      pSi.setAuthor("Author");
      pSi.setKeywords("Keywords");
      pSi.setTemplate("Template");
      pSi.setLastSavedBy("LastSavedBy");
      pSi.setRevisionNumber("1");
      pSi.setApplicationName("ExDgnCreateSwigMgd v" + Assembly.GetExecutingAssembly().GetName().Version);
      //pSi.setComments("Comments"); // PIDSI_COMMENTS == 6, VT_LPWSTR == 31
      pSi.setProperty(6, OdDgPropertyValue.createObject(31, "Comments"));

      // Document Summary Information
      OdDgDocumentSummaryInformation pDsi = TG_Db.oddgGetDocumentSummaryInformation(pDb);
      pDsi.setManager("Manager");
      pDsi.setCompany("Open Design Alliance");
      //pDsi.setCategory("Category"); // PIDDSI_CATEGORY == 2, VT_LPWSTR == 31
      pDsi.setProperty(2, OdDgPropertyValue.createObject(31, "Category"));
      //pDsi.setCustomProperty("CustomProperty1", OdDgPropertyValue.createObject(
      //  (UInt16)OdDgPropertyValue.CustomType.kCustomText, "Value 1")); // such string causes ODA_ASSERT
      pDsi.setCustomProperty("CustomProperty2", OdDgPropertyValue.createObject(
        (UInt16)OdDgPropertyValue.CustomType.kCustomNumber, (Int32)255));
      // (64-bit value representing the number of 100-nanosecond intervals since January 1, 1601)
      pDsi.setCustomProperty("CustomProperty3", OdDgPropertyValue.createObject(
        (UInt16)OdDgPropertyValue.CustomType.kCustomDate, (Int64)129459024000000000));  // 30.03.2011
      pDsi.setCustomProperty("CustomProperty4", OdDgPropertyValue.createObject(
        (UInt16)OdDgPropertyValue.CustomType.kCustomBool, (System.Boolean)true));
      //OdRxDictionary pPropDic = pDsi.getCustomProperties();
      //pPropDic.putAt("CustomProperty5", OdDgPropertyValue.createObject(OdDgPropertyValue.kCustomText, OdVariant(OdAnsiString("Value 5"))));
      //pDsi.setCustomProperties(pPropDic);
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    void fillModels()
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* Create table record elements                                       */
      /**********************************************************************/

      addLevelTableRecord();
      addLevelFilterTableRecord();
      addFontTableRecord();
      addDimStyleTableRecord();
      addLineStyleTableRecord();
      addMultilineStyleTableRecord();
      addRegAppTableRecord();
      addTextStyleTableRecord();
      addColorTableRecord();

      MemoryTransaction mTr1 = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* Create boxes for elements                                          */
      /**********************************************************************/
      createEntityBoxes3d();
      createEntityBoxes2d(m_pModel2d);
      createEntityBoxes2d(m_pSheetModel);
      MemoryTransaction mTr2 = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* Add ellipse                                                        */
      /**********************************************************************/
      addEllipse(0, 4);

      /**********************************************************************/
      /* Add ellipse                                                        */
      /**********************************************************************/
      addArc(0, 5);

      /**********************************************************************/
      /* Add cone                                                           */
      /**********************************************************************/
      addCone(6, 4);

      /**********************************************************************/
      /* Add lines                                                          */
      /**********************************************************************/
      addLines(0, 0);

      /**********************************************************************/
      /* Add lines with line style                                                           */
      /**********************************************************************/
      addLineStyleLines(2, 0);

      /**********************************************************************/
      /* Add text                                                           */
      /**********************************************************************/
      addText(2, 1);

      /**********************************************************************/
      /* Add text node                                                      */
      /**********************************************************************/
      addTextNode(1, 1);

      /**********************************************************************/
      /* Add shape                                                          */
      /**********************************************************************/
      addShape(4, 4);

      /**********************************************************************/
      /* Add shape with hole                                                         */
      /**********************************************************************/
      addShapeWithHole(1, 2);

      /**********************************************************************/
      /* Add curve                                                          */
      /**********************************************************************/
      addCurve(6, 5);

      /**********************************************************************/
      /* Add point string                                                   */
      /**********************************************************************/
      addPointString(6, 7);

      /**********************************************************************/
      /* Add dimension                                                      */
      /**********************************************************************/
      addDimension(0, 9);

      /**********************************************************************/
      /* Add complex string                                                 */
      /**********************************************************************/
      addComplexString(0, 1);

      /**********************************************************************/
      /* Multiline                                                          */
      /**********************************************************************/
      addMultiline(0, 6);

      /**********************************************************************/
      /* Complex shape                                                      */
      /**********************************************************************/
      addComplexShape(3, 4);

      /**********************************************************************/
      /* B-spline curve                                                     */
      /**********************************************************************/
      addBSplineCurve(3, 3);

      MemoryTransaction mTr3 = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* B-spline surface                                                   */
      /**********************************************************************/
      addBSplineSurface(4, 1);

      /**********************************************************************/
      /* Surface                                                            */
      /**********************************************************************/
      addSurface(2, 3);

      /**********************************************************************/
      /* Solid                                                              */
      /**********************************************************************/
      addSolid(1, 5);
      addLights(4, 1);

      /**********************************************************************/
      /* Raster                                                             */
      /**********************************************************************/
      addRaster(6, 8);

      /**********************************************************************/
      /* Raster attach                                                      */
      /**********************************************************************/
      addRasterAttach(6, 9);
      addRasterAttachBmp(5, 6);

      /**********************************************************************/
      /* Tag element                                                        */
      /**********************************************************************/
      addTagElement(6, 10);

      MemoryTransaction mTr4 = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* Mesh                                                               */
      /**********************************************************************/
      addMesh(4, 2);
      addColorMesh(5, 1);
      addNormAndTexMesh(4, 5);

      /**********************************************************************/
      /* Reference attach                                                   */
      /**********************************************************************/
      addAttach(5, 7);

      /**********************************************************************/
      /* Shared cell                                                        */
      /**********************************************************************/
      addSharedCells(1, 6);

      /**********************************************************************/
      /* Cell                                                               */
      /**********************************************************************/
      addCell(2, 4);

      /**********************************************************************/
      /* Smart Solid                                                        */
      /**********************************************************************/
      addSmartSolid(2, 5);

      /**********************************************************************/
      /* Patterns                                                           */
      /**********************************************************************/
      addPatterns(5, 0);

      /**********************************************************************/
      /* DB Linkages                                                        */
      /**********************************************************************/
      addDBLinkages(4, 0);

      /**********************************************************************/
      /* Something with true colors                                                         */
      /**********************************************************************/
      addTrueColorShapes(0, 2);

      /**********************************************************************/
      /* Something with a fill color                                                          */
      /**********************************************************************/
      addFilledShape(0, 3);

      /**********************************************************************/
      /* Add _ELEMENT_                                                      */
      /**********************************************************************/
      MemoryManager.GetMemoryManager().StopTransaction(mTr4);
      MemoryManager.GetMemoryManager().StopTransaction(mTr3);
      MemoryManager.GetMemoryManager().StopTransaction(mTr2);
      MemoryManager.GetMemoryManager().StopTransaction(mTr1);
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    void addLevelTableRecord()
    {
      OdCmEntityColor color = new OdCmEntityColor();
      color.setRGB(0, 128, 0);
      //uint rgbColor;
      uint iColor = OdDgColorTable.getColorIndexByRGB(m_pModel3d.database(), color.color());
      color.setColorIndex((short)iColor);

      OdDgLevelTableRecord pLevel1 = OdDgLevelTableRecord.createObject();
      pLevel1.setName("ExDgnCreate level 1");
      //pLevel1.setElementColorIndex(6);
      pLevel1.setElementColorIndex(iColor);
      pLevel1.setElementLineWeight(10);
      pLevel1.setIsDisplayedFlag(false);
      pLevel1.setIsPlotFlag(false);       // won't be displayed by Print Preview

      OdDgLevelTableRecord pLevel2 = OdDgLevelTableRecord.createObject();
      pLevel2.setName("level 2");
      pLevel2.setElementColorIndex(4);
      pLevel2.setElementLineWeight(0);
      pLevel2.setIsDisplayedFlag(true);
      pLevel2.setIsPlotFlag(false);

      OdDgLevelTableRecord pLevel3 = OdDgLevelTableRecord.createObject();
      pLevel3.setName("Custom name");
      pLevel3.setElementColorIndex(2);
      pLevel3.setElementLineWeight(0);
      pLevel3.setIsDisplayedFlag(true);
      pLevel3.setIsPlotFlag(false);

      OdDgLevelTable pLevelTable = m_pModel3d.database().getLevelTable(OpenMode.kForWrite);
      pLevelTable.add(pLevel1);
      pLevelTable.add(pLevel2);
      pLevelTable.add(pLevel3);
    }

    void addLevelFilterTableRecord()
    {
      OdDgLevelNameAndGroupFilter pFilter0 = (OdDgLevelNameAndGroupFilter)OdDgLevelNameAndGroupFilter.createObject();
      pFilter0.setName("All Levels");

      OdDgLevelNameAndGroupFilter pFilter1 = (OdDgLevelNameAndGroupFilter)OdDgLevelNameAndGroupFilter.createObject();

      pFilter1.setName("Layer Name Filter");
      pFilter1.setFilterType(OdDgLevelFilterTableRecord.OdDgLevelFilterType.kUser);

      OdDgLevelNameAndGroupFilter pFilter2 = (OdDgLevelNameAndGroupFilter)OdDgLevelNameAndGroupFilter.createObject();

      pFilter2.setName("Layer Group Filter");
      pFilter2.setFilterType(OdDgLevelFilterTableRecord.OdDgLevelFilterType.kUser);

      OdDgLevelFilterTable pFilterTable = m_pModel3d.database().getLevelFilterTable(OpenMode.kForWrite);
      pFilterTable.add(pFilter0);
      pFilterTable.add(pFilter1);
      pFilterTable.add(pFilter2);

      pFilter1.setNameFilterExpression("level");

      OdDgLevelTable pLevelTable = m_pModel3d.database().getLevelTable(OpenMode.kForWrite);

      OdDgElementIterator pLevelIter = pLevelTable.createIterator();

      int iNumber = 0;

      for (; !pLevelIter.done(); pLevelIter.step())
      {
        OdDgLevelTableRecord pLevel = (OdDgLevelTableRecord)pLevelIter.item().openObject(OpenMode.kForRead);

        if (iNumber > 1)
        {
          pFilter2.addLevelToGroup(pLevel);
        }
        iNumber++;
      }
    }

    void addFontTableRecord()
    {
      OdDgFontTableRecord pFont = OdDgFontTableRecord.createObject();
      pFont.setName("GothicE");
      pFont.setType(OdFontType.kFontTypeTrueType);

      OdDgFontTable pFontTable = m_pModel3d.database().getFontTable(OpenMode.kForWrite);
      pFontTable.add(pFont);

      pFont = OdDgFontTableRecord.createObject();
      pFont.setName("Verdana");
      pFont.setType(OdFontType.kFontTypeTrueType);

      pFontTable.add(pFont);

      pFont = OdDgFontTableRecord.createObject();
      pFont.setName("Italic");
      pFont.setType(OdFontType.kFontTypeTrueType);

      pFontTable.add(pFont);

      pFont = OdDgFontTableRecord.createObject();
      pFont.setName("Arial");
      pFont.setType(OdFontType.kFontTypeTrueType);

      pFontTable.add(pFont);

      pFont = OdDgFontTableRecord.createObject();
      pFont.setName("ROMANS");
      pFont.setType(OdFontType.kFontTypeShx);

      pFontTable.add(pFont);

      // there is no necessity to add RSC (MicroStation Recource file) font
      try
      {
        pFont = OdDgFontTableRecord.createObject();
        pFont.setName("ENGINEERING");
        pFont.setType(OdFontType.kFontTypeRsc);

        pFontTable.add(pFont);
      }
      catch (OdError err)
      {
        Debug.Assert(err.code() == OdResult.eAlreadyInDb);
      }
    }

    void addDimStyleTableRecord()
    {
      OdDgDimStyleTable pTbl = m_pModel3d.database().getDimStyleTable(OpenMode.kForWrite);

      OdDgDimStyleTableRecord pRecDefault = OdDgDimStyleTableRecord.createObject();
      pRecDefault.setName("");
      pRecDefault.setDescription("");
      pRecDefault.setTextHeightOverrideFlag(true);
      pRecDefault.setTextHeight(2);
      pRecDefault.setTextWidthOverrideFlag(true);
      pRecDefault.setTextWidth(2);

      pTbl.add(pRecDefault);

      OdDgDimStyleTableRecord pRec = OdDgDimStyleTableRecord.createObject();
      pRec.setName("DimStyle1");
      pRec.setDescription("Dimension Style 1");
      pRec.setTerminatorColorOverrideFlag(true);
      pRec.setTerminatorColor(4);
      pRec.setTerminatorLineStyleOverrideFlag(true);
      pRec.setTerminatorLineStyleId(2);

      pTbl.add(pRec);
    }

    void addLineStyleTableRecord()
    {
      OdDgLineStyleDefTable pLineStyleDefTable = m_pModel3d.database().getLineStyleDefTable(OpenMode.kForWrite);

      double dScaleToUORs = m_pModel3d.convertWorkingUnitsToUORs(1.0);

      // Create shape for line style

      OdDgCellHeader2d pSmileyCell = OdDgCellHeader2d.createObject();

      pLineStyleDefTable.addSymbol(pSmileyCell);

      OdDgEllipse2d pHeadEllipse = OdDgEllipse2d.createObject();
      pHeadEllipse.setOrigin(new OdGePoint2d(0, 0));
      pHeadEllipse.setPrimaryAxis(1.5);
      pHeadEllipse.setSecondaryAxis(1.5);

      OdDgEllipse2d pEyeLeftEllipse = OdDgEllipse2d.createObject();
      pEyeLeftEllipse.setOrigin(new OdGePoint2d(-0.75, 0.5));
      pEyeLeftEllipse.setPrimaryAxis(0.25);
      pEyeLeftEllipse.setSecondaryAxis(0.25);

      OdDgEllipse2d pEyeRightEllipse = OdDgEllipse2d.createObject();
      pEyeRightEllipse.setOrigin(new OdGePoint2d(0.75, 0.5));
      pEyeRightEllipse.setPrimaryAxis(0.25);
      pEyeRightEllipse.setSecondaryAxis(0.25);

      OdDgLineString2d pNoseLine = OdDgLineString2d.createObject();

      pNoseLine.addVertex(new OdGePoint2d(-0.25, 0.25));
      pNoseLine.addVertex(new OdGePoint2d(-0.25, -0.5));
      pNoseLine.addVertex(new OdGePoint2d(0.25, -0.5));

      OdDgArc2d pMouthArc = OdDgArc2d.createObject();

      pMouthArc.setOrigin(new OdGePoint2d(0, 0));
      pMouthArc.setPrimaryAxis(1.0);
      pMouthArc.setSecondaryAxis(1.0);
      pMouthArc.setStartAngle(5.0 * Globals.OdaPI / 4.0);
      pMouthArc.setSweepAngle(Globals.OdaPI2);

      pSmileyCell.add(pHeadEllipse);
      pSmileyCell.add(pEyeLeftEllipse);
      pSmileyCell.add(pEyeRightEllipse);
      pSmileyCell.add(pNoseLine);
      pSmileyCell.add(pMouthArc);
      pSmileyCell.setName("Linestyle Definition Cell");

      pSmileyCell.setOrigin(new OdGePoint2d(0, 0));
      pSmileyCell.setTransformation(OdGeMatrix2d.scaling(dScaleToUORs));

      // Create point symbol resource

      OdDgLineStyleDefTableRecord pSmileySymbolDef = OdDgLineStyleDefTableRecord.createObject(LineStyleType.kLsTypePointSymbol);

      OdDgPointSymbolResource pRes = (OdDgPointSymbolResource)pSmileySymbolDef.getResource();
      pRes.setDependedCellHeaderHandle(pSmileyCell.elementId().getHandle().ToUInt64());
      pRes.setDescription("Smiley cell");

      pLineStyleDefTable.add(pSmileySymbolDef);

      // Create stroke resource

      OdDgLineStyleDefTableRecord pSmileyStrokeDef = OdDgLineStyleDefTableRecord.createObject(LineStyleType.kLsTypeLineCode);

      OdDgLineCodeResource pStrokeRes = (OdDgLineCodeResource)pSmileyStrokeDef.getResource();

      OdDgLineCodeResourceStrokeData dashData = new OdDgLineCodeResourceStrokeData();

      dashData.setDashFlag(true);
      dashData.setLength(2.0 * dScaleToUORs);
      dashData.setWidthMode(OdDgLineCodeResourceStrokeData.OdLsStrokeWidthMode.kLsNoWidth);

      pStrokeRes.addStroke(dashData);

      dashData.setDashFlag(false);
      dashData.setLength(4.0 * dScaleToUORs);

      pStrokeRes.addStroke(dashData);

      dashData.setDashFlag(true);
      dashData.setLength(2.0 * dScaleToUORs);

      pStrokeRes.addStroke(dashData);

      pStrokeRes.setDescription("Smiley stroke");

      pLineStyleDefTable.add(pSmileyStrokeDef);

      // Create line point resource

      OdDgLineStyleDefTableRecord pSmileyLinePtDef = OdDgLineStyleDefTableRecord.createObject(LineStyleType.kLsTypeLinePoint);

      OdDgLinePointResource pLinePtRes = (OdDgLinePointResource)pSmileyLinePtDef.getResource();

      pLinePtRes.setBasePatternType(OdDgLineStyleResource.OdLsResourceType.kLsLineCodeRes);
      pLinePtRes.setBasePatternHandleId(pSmileyStrokeDef.elementId().getHandle().ToUInt64());

      OdDgLinePointResourceSymInfo symInfo = new OdDgLinePointResourceSymInfo();

      symInfo.setPointSymbolHandleId(pSmileySymbolDef.elementId().getHandle().ToUInt64());
      symInfo.setSymbolPosOnStroke(OdDgLinePointResourceSymInfo.OdLsSymbolPosOnStroke.kLsAtCenterOfStroke);
      symInfo.setSymbolType(OdDgLineStyleResource.OdLsResourceType.kLsPointSymbolRes);
      symInfo.setSymbolStrokeNo(1);

      pLinePtRes.addSymbol(symInfo);

      pLinePtRes.setDescription("Smiley symbol");

      pLineStyleDefTable.add(pSmileyLinePtDef);

      // Create compound resource

      OdDgLineStyleDefTableRecord pSmileyCompoundDef = OdDgLineStyleDefTableRecord.createObject(LineStyleType.kLsTypeCompound);

      OdDgCompoundLineStyleResource pCompoundRes = (OdDgCompoundLineStyleResource)pSmileyCompoundDef.getResource();

      pCompoundRes.setDescription("Smiley compound");

      OdDgCompoundLineStyleComponentInfo infoComp = new OdDgCompoundLineStyleComponentInfo();

      infoComp.setComponentType(OdDgLineStyleResource.OdLsResourceType.kLsLineCodeRes);
      infoComp.setComponentHandleId(pSmileyStrokeDef.elementId().getHandle().ToUInt64());
      infoComp.setComponentOffset(0.0);

      pCompoundRes.addComponent(infoComp);

      infoComp.setComponentType(OdDgLineStyleResource.OdLsResourceType.kLsLinePointRes);
      infoComp.setComponentHandleId(pSmileyLinePtDef.elementId().getHandle().ToUInt64());

      pCompoundRes.addComponent(infoComp);

      pSmileyCompoundDef.setName("{ Smiley }");

      pLineStyleDefTable.add(pSmileyCompoundDef);

      // Create line style

      OdDgLineStyleTableRecord pLineStyle1 = OdDgLineStyleTableRecord.createObject();
      pLineStyle1.setName("{ Smiley }");
      pLineStyle1.setType(LineStyleType.kLsTypeCompound);
      pLineStyle1.setRefersToElementFlag(true);
      pLineStyle1.setRefersToId(pSmileyCompoundDef.elementId());
      pLineStyle1.setUnitsType(LineStyleUnitsType.kLsUORS);

      OdDgLineStyleTable pLSTbl = m_pModel3d.database().getLineStyleTable(OpenMode.kForWrite);

      if (pLSTbl != null)
      {
        pLSTbl.add(pLineStyle1);
      }
    }

    void addMultilineStyleTableRecord()
    {
      OdDgMultilineStyleTableRecord pMLStyle = OdDgMultilineStyleTableRecord.createObject();
      pMLStyle.setName("Multiline Style 1");
      pMLStyle.setUseFillColorFlag(true);
      pMLStyle.setFillColorIndex(135);

      double dScaleWorkingUnitsToUORs = m_pSheetModel.convertWorkingUnitsToUORs(1);
      // Offset of lines into multiline style
      double dOffset1 = 0.5;
      double dOffset2 = -0.5;
      double dOffset3 = -1;
      double dOffset4 = 1.5;
      double dOffset5 = -1.5;

      OdDgMultilineProfile profile = new OdDgMultilineProfile();
      OdDgMultilineSymbology mlSymb = new OdDgMultilineSymbology();
      mlSymb.setColorIndex(3);
      mlSymb.setLineStyleEntryId(3);
      mlSymb.setUseColorFlag(true);
      mlSymb.setUseStyleFlag(true);
      profile.setDistance(dOffset1 * dScaleWorkingUnitsToUORs);
      profile.setSymbology(mlSymb);
      pMLStyle.addProfile(profile);
      profile.setDistance(dOffset2 * dScaleWorkingUnitsToUORs);
      pMLStyle.addProfile(profile);
      profile.setDistance(1 * dScaleWorkingUnitsToUORs);
      mlSymb.setColorIndex(1);          // MKU 23/12/09 - ('warning' issue)
      mlSymb.setLineStyleEntryId(4);
      profile.setSymbology(mlSymb);
      pMLStyle.addProfile(profile);
      profile.setDistance(dOffset3 * dScaleWorkingUnitsToUORs);
      pMLStyle.addProfile(profile);
      profile.setDistance(dOffset4 * dScaleWorkingUnitsToUORs);
      mlSymb.setColorIndex(5);
      mlSymb.setUseStyleFlag(false);
      profile.setSymbology(mlSymb);
      pMLStyle.addProfile(profile);
      profile.setDistance(dOffset5 * dScaleWorkingUnitsToUORs);
      pMLStyle.addProfile(profile);
      OdDgMultilineSymbology mlStartCap = new OdDgMultilineSymbology();
      mlStartCap.setUseColorFlag(true);
      mlStartCap.setColorIndex(3);
      mlStartCap.setCapOutArcFlag(true);
      pMLStyle.setOriginCap(mlStartCap);
      OdDgMultilineSymbology mlEndCap = mlStartCap;
      mlEndCap.setColorIndex(2);
      mlEndCap.setCapInArcFlag(true);
      mlEndCap.setCapLineFlag(true);
      pMLStyle.setEndCap(mlEndCap);
      pMLStyle.setOriginCapAngle(90.0);
      pMLStyle.setEndCapAngle(90.0);
      OdDgMultilineSymbology mlMiddleCap = new OdDgMultilineSymbology();
      mlMiddleCap.setUseColorFlag(true);
      mlMiddleCap.setUseStyleFlag(true);
      mlMiddleCap.setColorIndex(8);
      mlMiddleCap.setLineStyleEntryId(2);
      mlMiddleCap.setCapLineFlag(true);
      pMLStyle.setMiddleCap(mlMiddleCap);

      OdDgMultilineStyleTable pTbl = m_pModel3d.database().getMultilineStyleTable(OpenMode.kForWrite);
      pTbl.add(pMLStyle);
    }

    void addRegAppTableRecord()
    {

    }

    void addTextStyleTableRecord()
    {
      // Set Default Text Style
      OdDgTextStyleTable pTbl = m_pModel3d.database().getTextStyleTable(OpenMode.kForWrite);
      OdDgTextStyleTableRecord pRec = pTbl.getDefaultData();
      pRec.setFontEntryId(41);   // set 'Architectural' font
      pRec.setTextWidth(m_pModel3d.convertWorkingUnitsToUORs(0.27));
      pRec.setTextHeight(m_pModel3d.convertWorkingUnitsToUORs(0.27));
      pTbl.setDefaultData(pRec);

      OdDgTextStyleTable pTextStyleTable = m_pModel3d.database().getTextStyleTable();
      OdDgFontTable pFontTable = m_pModel3d.database().getFontTable();

      // Indemnify the code against some font absence
      UInt32 nAlternateFontNumber = pFontTable.getFont(m_pModel3d.database().appServices().getAlternateFontName()).getNumber();

      OdDgTextStyleTableRecord pTextStyle = OdDgTextStyleTableRecord.createObject();
      pTextStyle.setName("TextStyle1");
      pTextStyle.setTextHeight(m_pModel3d.convertWorkingUnitsToUORs(0.14));
      pTextStyle.setTextWidth(m_pModel3d.convertWorkingUnitsToUORs(0.14));
      //pTextStyle.setItalicsFlag(true);
      pTextStyle.setSlant(Globals.OdaPI / 6);
      pTextStyle.setColorIndex(2);
      pTextStyle.setFontEntryId((pFontTable.getFont("Verdana") != null) ?
        pFontTable.getFont("Verdana").getNumber() : nAlternateFontNumber);
      pTextStyleTable.add(pTextStyle);

      pTextStyle = OdDgTextStyleTableRecord.createObject();
      pTextStyle.setName("TextStyle2");
      pTextStyle.setTextHeight(m_pModel3d.convertWorkingUnitsToUORs(0.23));
      pTextStyle.setTextWidth(m_pModel3d.convertWorkingUnitsToUORs(0.23));
      pTextStyle.setColorIndex(7);
      pTextStyle.setFontEntryId((pFontTable.getFont("ENGINEERING") != null) ?
        pFontTable.getFont("ENGINEERING").getNumber() : nAlternateFontNumber);
      pTextStyleTable.add(pTextStyle);

      pTextStyle = OdDgTextStyleTableRecord.createObject();
      pTextStyle.setName("TextStyle3");
      OdDgElementId id = pTextStyleTable.getAt("TextStyle1");
      if (!id.isNull())
      {
        OdDgTextStyleTableRecord pParentTextStyle = (OdDgTextStyleTableRecord)id.safeOpenObject();
        pTextStyle.setParentTextStyleEntryId(pParentTextStyle.getEntryId());
      }
      pTextStyle.setItalicsFlag(false);
      pTextStyle.setColorIndex(4);
      pTextStyle.setFontEntryId((pFontTable.getFont("Arial") != null) ?
        pFontTable.getFont("Arial").getNumber() : nAlternateFontNumber);
      pTextStyleTable.add(pTextStyle);
    }

    void addColorTableRecord()
    {
      OdDgColorTable pColorTbl = m_pModel3d.database().getColorTable(OpenMode.kForWrite);
      if (pColorTbl != null)
      {
        pColorTbl.Palette = OdDgColorTable.DefaultPalette;
      }
    }

    void addEllipse(int boxRow, int boxCol)
    {
      // adds 2d ellipse
      //
      double minorRadius = EntityBoxes.getHeight() / 8;
      double majorRadius = EntityBoxes.getWidth(boxRow, boxCol) / 8;
      OdGePoint3d boxCenter = EntityBoxes.getBoxCenter(boxRow, boxCol);

      OdDgEllipse2d pEllipse = OdDgEllipse2d.createObject();
      pEllipse.setOrigin(new OdGePoint2d(boxCenter.x + majorRadius, boxCenter.y * 1.0));
      pEllipse.setPrimaryAxis(majorRadius);
      pEllipse.setSecondaryAxis(minorRadius);
      pEllipse.setRotationAngle(Globals.OdaPI4);
      m_pModel2d.addElement(pEllipse);

      // adds 3d ellipse
      //
      minorRadius = EntityBoxes.getHeight() / 8;
      majorRadius = EntityBoxes.getWidth(boxRow, boxCol) / 8;

      OdDgEllipse3d pEllipse3d = OdDgEllipse3d.createObject();
      pEllipse3d.setOrigin(new OdGePoint3d(boxCenter.x - majorRadius, boxCenter.y * 1.0, boxCenter.z));
      pEllipse3d.setPrimaryAxis(majorRadius);
      pEllipse3d.setSecondaryAxis(minorRadius);
      OdGeMatrix3d matrix = new OdGeMatrix3d();
      OdGeQuaternion quat = new OdGeQuaternion();
      pEllipse3d.setRotation(quat.set(matrix.setToRotation(Globals.OdaPI4 / 2, OdGeVector3d.kZAxis)));

      m_pModel3d.addElement(pEllipse3d);
    }


    void addArc(int boxRow, int boxCol)
    {
      // adds 2d arc
      //
      double minorRadius = EntityBoxes.getHeight() / 8;
      double majorRadius = EntityBoxes.getWidth(boxRow, boxCol) / 8;
      OdGePoint3d boxCenter = EntityBoxes.getBoxCenter(boxRow, boxCol);

      OdDgArc2d pArc = OdDgArc2d.createObject();
      pArc.setOrigin(new OdGePoint2d(boxCenter.x + majorRadius, boxCenter.y / 1));
      pArc.setPrimaryAxis(majorRadius);
      pArc.setSecondaryAxis(minorRadius);
      pArc.setRotationAngle(Globals.OdaPI4 / 4);
      pArc.setStartAngle(-Globals.OdaPI4 / 1.0);
      pArc.setSweepAngle(Globals.OdaPI * 1.5);
      m_pModel2d.addElement(pArc);

      // adds 3d arc
      //
      minorRadius = EntityBoxes.getHeight() / 4;
      majorRadius = EntityBoxes.getWidth(boxRow, boxCol) / 4;

      OdDgArc3d pArc3d = OdDgArc3d.createObject();
      pArc3d.setOrigin(new OdGePoint3d(boxCenter.x - majorRadius, boxCenter.y, boxCenter.z));
      pArc3d.setPrimaryAxis(majorRadius);
      pArc3d.setSecondaryAxis(minorRadius);
      pArc3d.setStartAngle(Globals.OdaPI * .1);
      pArc3d.setSweepAngle(Globals.OdaPI * .4);
      m_pModel3d.addElement(pArc3d);
    }

    void addCone(int boxRow, int boxCol)
    {
      // adds Cone
      //
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxCol);
      OdGePoint3d boxCenter = EntityBoxes.getBoxCenter(boxRow, boxCol);

      OdDgCone pCone = OdDgCone.createObject();
      /*
      Is solid = yes
      Radius 1 = 0.323791
      Radius 2 = 0.725507
      Center 1 = ( 2.9033; 1.20283; 0 )
      Center 2 = ( 2.35958; 1.91472; 0 )
      Rotation = ( -0.22654; 0.669835; -0.669835; -0.22654 )
      */

      pCone.setCenter1(new OdGePoint3d(boxCenter.x + (major / 4), boxCenter.y + (minor / 8), boxCenter.z));
      pCone.setCenter2(new OdGePoint3d(boxCenter.x - (major / 5), boxCenter.y - (minor / 8), boxCenter.z));
      pCone.setRadius1(EntityBoxes.getWidth(boxRow, boxCol) / 10);
      pCone.setRadius2(EntityBoxes.getWidth(boxRow, boxCol) / 5);

      {
        OdGeMatrix3d matrix = new OdGeMatrix3d();
        OdGeQuaternion quaternion = new OdGeQuaternion();

        matrix.setToRotation(Globals.OdaPI4 / 2, OdGeVector3d.kZAxis);
        quaternion.set(matrix);

        pCone.setRotation(quaternion);
      }

      pCone.setSolid();

      m_pModel3d.addElement(pCone);

    }


    void addLines(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d boxCenter3d = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      OdGePoint2d boxCenter2d = new OdGePoint2d(boxCenter3d.x, boxCenter3d.y);

      //Line2d
      {
        OdDgLine2d pLine2d;

        pLine2d = OdDgLine2d.createObject();
        pLine2d.setStartPoint(boxCenter2d + new OdGeVector2d(-major / 3.0, -minor / 3.0));
        pLine2d.setEndPoint(boxCenter2d + new OdGeVector2d(major / 3.0, -minor / 3.0));
        pLine2d.setLineStyleEntryId(4);
        m_pModel2d.addElement(pLine2d);

        pLine2d = OdDgLine2d.createObject();
        pLine2d.setStartPoint(boxCenter2d + new OdGeVector2d(-major / 3.0, +minor / 3.0));
        pLine2d.setEndPoint(boxCenter2d + new OdGeVector2d(major / 3.0, +minor / 3.0));
        m_pModel2d.addElement(pLine2d);
      }

      //Line3d
      {
        OdGeVector3d pseudoX = new OdGeVector3d(major / 3.0, minor / 3.0, 0.0);
        OdGeVector3d pseudoY = new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0);
        OdGeVector3d stepZ = new OdGeVector3d(0.0, 0.0, minor / 5.0);
        for (double i = 0; i < 2.0; i += 0.1)
        {
          double angle = i * Globals.OdaPI;
          OdDgLine3d pLine3d = OdDgLine3d.createObject();

          pLine3d.setStartPoint(boxCenter3d + (pseudoX * Math.Cos(angle))
            + pseudoY * Math.Sin(angle) + stepZ * i);
          pLine3d.setEndPoint(boxCenter3d + pseudoX * Math.Cos(angle) + pseudoY * Math.Sin(angle) * 0.1);
          m_pModel3d.addElement(pLine3d);
        }
      }
    }


    void addText(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d boxOrigin3d = EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(major / 2, minor / 2, 0.0);

      OdDgElementId idLevel = m_pModel3d.database().getLevelTable().getAt("ExDgnCreate level 1");

      //3D with default Font
      {
        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        OdDgText3d pText3d = OdDgText3d.createObject();

        pText3d.setJustification(TextJustification.kCenterCenter);
        pText3d.setText("Default font");
        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 2 + minor / 8, 0));
        pText3d.setRotation(rotation);
        pText3d.setHeightMultiplier(0.5);
        pText3d.setLengthMultiplier(0.5);

        //if (!idLevel.isNull())
        //{
        //  pText3d.setLevelId( idLevel );
        //}

        m_pModel3d.addElement(pText3d);
      }
      //3D with Font
      {
        OdDgText3d pText3d = OdDgText3d.createObject();
        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        pText3d.setJustification(TextJustification.kCenterCenter);
        pText3d.setText("ARIAL font");

        OdDgFontTable pFontTable = m_pModel3d.database().getFontTable();
        OdDgFontTableRecord pFont = pFontTable.getFont("Arial");
        if (pFont != null)
        {
          pText3d.setFontEntryId(pFont.getNumber());
        }

        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 8, 0));
        pText3d.setRotation(rotation);
        pText3d.setHeightMultiplier(0.75);
        pText3d.setLengthMultiplier(0.75);
        pText3d.setColorIndex(3);

        //if (!idLevel.isNull())
        //{
        //  pText3d.setLevelId( idLevel );
        //}

        m_pModel3d.addElement(pText3d);
      }

      //3D
      boxColumn++;
      major = EntityBoxes.getWidth(boxRow, boxColumn);
      boxOrigin3d = EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(major / 2, minor / 2, 0.0);
      {
        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        OdDgText3d pText3d = OdDgText3d.createObject();
        pText3d.setJustification(TextJustification.kCenterCenter);
        pText3d.setText("Verdana font");

        OdDgFontTable pFontTable = m_pModel3d.database().getFontTable();
        OdDgFontTableRecord pFont = pFontTable.getFont("Verdana");
        if (pFont != null)
        {
          pText3d.setFontEntryId(pFont.getNumber());
        }

        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 2 + minor / 8, 0));
        pText3d.setRotation(rotation);
        pText3d.setHeightMultiplier(0.8);
        pText3d.setLengthMultiplier(0.8);

        m_pModel3d.addElement(pText3d);
      }

      {
        OdDgText3d pText3d = OdDgText3d.createObject();
        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        pText3d.setJustification(TextJustification.kCenterCenter);
        pText3d.setText("Verdana (TextStyle1)");

        //OdDgFontTable pFontTable = m_pModel3d.database().getFontTable();
        //OdDgFontTableRecord pFont = pFontTable.getFont("Arial");
        //if (!pFont.isNull())
        //{
        //  pText3d.setFontEntryId(pFont.getNumber());
        //}
        OdDgTextStyleTable pTextStyleTable = m_pModel3d.database().getTextStyleTable();
        OdDgElementId id = pTextStyleTable.getAt("TextStyle1");

        // Firstly to add a text element to model;
        //  Next to apply TextStyle properties.
        m_pModel3d.addElement(pText3d);

        //if (!id.isNull())
        //{
        //  OdDgTextStyleTableRecord pTextStyle = id.safeOpenObject();
        //  pText3d.setTextStyleEntryId( pTextStyle.getEntryId() );
        //}
        if (!id.isNull())
        {
          pText3d.applyTextStyle(id);
        }

        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 8, 0));
        pText3d.setRotation(rotation);
        pText3d.setHeightMultiplier(0.35);
        pText3d.setLengthMultiplier(0.35);
        pText3d.setColorIndex(6);

        //m_pModel3d.addElement( pText3d );
      }

      // Apply TextStyle
      {
        OdDgText3d pText3d = OdDgText3d.createObject();
        m_pModel3d.addElement(pText3d);

        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        pText3d.setJustification(TextJustification.kCenterCenter);
        pText3d.setText("TextStyle2");

        OdDgTextStyleTable pTextStyleTable = m_pModel3d.database().getTextStyleTable();
        OdDgElementId id = pTextStyleTable.getAt("TextStyle2");
        if (!id.isNull())
        {
          // applyTextStyle() should be called after adding element to model
          pText3d.applyTextStyle(id);
        }
        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 6, 0));
        pText3d.setRotation(rotation);
      }

      {
        OdDgText3d pText3d = OdDgText3d.createObject();
        m_pModel3d.addElement(pText3d);

        OdDgTextStyleTable pTextStyleTable = m_pModel3d.database().getTextStyleTable();
        OdDgElementId id = pTextStyleTable.getAt("TextStyle3");
        if (!id.isNull())
        {
          // applyTextStyle() should be called after adding element to model
          pText3d.applyTextStyle(id);
        }

        OdGeQuaternion rotation = new OdGeQuaternion(-1.0, 0.0, 0.0, 0.0);

        pText3d.setText("Apply TextStyle3");

        pText3d.setOrigin(new OdGePoint3d(boxOrigin3d.x + major / 20, boxOrigin3d.y + minor / 3, 0));
        pText3d.setRotation(rotation);
        //pText3d.setColorIndex(9);
        pText3d.setColorIndex(8);
      }

    }


    void addTextNode(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d boxOrigin = EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(minor, 0, 0.0);

      OdDgTextNode3d pTextNode = OdDgTextNode3d.createObject();

      //set parameters
      pTextNode.setLineSpacing(1.2);
      pTextNode.setOrigin(boxOrigin);
      pTextNode.setRotation(new OdGeQuaternion(1.0, 0.0, 0.0, 0.0));
      pTextNode.setTextNodeNumber(1);
      pTextNode.setColorIndex(4);
      pTextNode.setLengthMultiplier((major / 16));
      pTextNode.setHeightMultiplier(minor / 8);

      OdDgTextStyleTable pTextStyleTable = m_pModel3d.database().getTextStyleTable();
      OdDgElementId id = pTextStyleTable.getAt("TextStyle1");
      if (!id.isNull())
      {
        OdDgTextStyleTableRecord pTextStyle = (OdDgTextStyleTableRecord)id.safeOpenObject();
        pTextNode.setTextStyleEntryId(pTextStyle.getEntryId());
      }

      m_pModel3d.addElement(pTextNode);

      //add strings
      {
        OdDgText3d pText = OdDgText3d.createObject();
        pText.setText("First line");
        pText.setLengthMultiplier((major / 12));
        pText.setHeightMultiplier(minor / 6);

        pText.setOrigin(boxOrigin);
        pText.setRotation(new OdGeQuaternion(1.0, 0.0, 0.0, 0.0));
        pTextNode.add(pText);

        pText = OdDgText3d.createObject();
        pText.setText("Second line");
        pText.setLengthMultiplier((major / 12));
        pText.setHeightMultiplier(minor / 8);

        pText.setOrigin(boxOrigin + new OdGeVector3d(0.0, -1.0, 0.0));
        pText.setRotation(new OdGeQuaternion(1.0, 0.0, 0.0, 0.0));

        pTextNode.add(pText);
      }
    }


    void addShape(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgShape3d pShape = OdDgShape3d.createObject();

      pShape.addVertex(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major / 3.0, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major / 3.0, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0));

      m_pModel3d.addElement(pShape);
    }

    void addShapeWithHole(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight() / 2.0;
      double major = EntityBoxes.getWidth(boxRow, boxColumn) / 2.0;

      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgCellHeader3d pHoleCell = OdDgCellHeader3d.createObject();

      OdDgFillColorLinkage pFillColorLinkage = OdDgFillColorLinkage.createObject();

      m_pModel3d.addElement(pHoleCell);

      OdDgShape3d pShape = OdDgShape3d.createObject();

      pShape.addVertex(center + new OdGeVector3d(-major / 1.1, -minor / 1.1, 0.0));
      pShape.addVertex(center + new OdGeVector3d(-major / 1.1, minor / 1.1, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major / 1.1, minor / 1.1, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major / 1.1, -minor / 1.1, 0.0));
      pShape.addVertex(center + new OdGeVector3d(-major / 1.1, -minor / 1.1, 0.0));

      pShape.setHbitFlag(false);

      pFillColorLinkage.setColorIndex(4);
      pShape.addLinkage((UInt16)OdDgAttributeLinkage.PrimaryIds.kFillStyle, pFillColorLinkage);

      pHoleCell.add(pShape);

      OdDgShape3d pHole = OdDgShape3d.createObject();

      pHole.addVertex(center + new OdGeVector3d(-major / 2.0, -minor / 2.0, 0.0));
      pHole.addVertex(center + new OdGeVector3d(-major / 2.0, minor / 2.0, 0.0));
      pHole.addVertex(center + new OdGeVector3d(major / 2.0, minor / 2.0, 0.0));
      pHole.addVertex(center + new OdGeVector3d(major / 2.0, -minor / 2.0, 0.0));
      pHole.addVertex(center + new OdGeVector3d(-major / 2.0, -minor / 2.0, 0.0));

      pHole.setHbitFlag(true);

      pHoleCell.add(pHole);
    }

    void addCurve(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgCurve3d pCurve = OdDgCurve3d.createObject();

      pCurve.addVertex(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(major / 3.0, minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(major / 3.0, -minor / 3.0, 0.0));

      pCurve.addVertex(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(major / 3.0, minor / 3.0, 0.0));
      pCurve.addVertex(center + new OdGeVector3d(major / 3.0, -minor / 3.0, 0.0));

      m_pModel3d.addElement(pCurve);
    }


    void addPointString(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      OdGeQuaternion rotation = new OdGeQuaternion(1.0, 0.0, 0.0, 0.0);

      OdDgPointString3d pPointString = OdDgPointString3d.createObject();

      pPointString.addVertex(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0), rotation);
      pPointString.addVertex(center + new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0), rotation);
      pPointString.addVertex(center + new OdGeVector3d(major / 3.0, minor / 3.0, 0.0), rotation);
      pPointString.addVertex(center + new OdGeVector3d(major / 3.0, -minor / 3.0, 0.0), rotation);

      m_pModel3d.addElement(pPointString);
    }


    void addDimension(int boxRow, int boxCol)
    {
      OdDgDimStyleTable pTbl = m_pModel3d.database().getDimStyleTable(OpenMode.kForWrite);
      OdDgElementId idDimStyle = null;

      if (pTbl != null)
        idDimStyle = pTbl.getAt("DimStyle1");

      OdDgDimStyleTableRecord pDimStyle = null;

      if (!idDimStyle.isNull())
      {
        pDimStyle = (OdDgDimStyleTableRecord)idDimStyle.openObject(OpenMode.kForWrite);
        pDimStyle.setExtensionLineColorOverrideFlag(true);
        pDimStyle.setExtensionLineColor(3);
        pDimStyle.setTextHeightOverrideFlag(true);
        pDimStyle.setTextHeight(2);
        pDimStyle.setTextWidthOverrideFlag(true);
        pDimStyle.setTextWidth(2);
      }

      // adds Dimensions
      //
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxCol);
      OdGePoint3d boxCenter = EntityBoxes.getBoxCenter(boxRow, boxCol);

      // Creates SIZE ARROW dimension
      //
      OdDgDimSizeArrow pDim = OdDgDimSizeArrow.createObject();
      OdDgDimPoint ptDim = new OdDgDimPoint();

      m_pModel3d.addElement(pDim);

      // Point 1
      ptDim.setPoint(new OdGePoint3d(boxCenter.x - major / 4.0, boxCenter.y, 0));
      ptDim.setOffsetToDimLine(minor / 5);
      ptDim.setPrimaryText("The Point 1");
      pDim.addPoint(ptDim);

      // Point 2
      ptDim.setPoint(new OdGePoint3d(boxCenter.x + major / 5.5, boxCenter.y, 0));
      ptDim.setOffsetToDimLine(-minor / 70);
      ptDim.setPrimaryText("The Point 2");
      pDim.addPoint(ptDim);

      if (pDimStyle != null)
      {
        pDim.applyDimensionStyle(pDimStyle.getEntryId());
      }

      // Text Format
      OdDgDimTextFormat format = new OdDgDimTextFormat();
      pDim.getDimTextFormat(format);
      format.setPrimaryAccuracy(OdDgDimTextFormat.Accuracy.kDecimal4);
      format.setSecondaryAccuracy(OdDgDimTextFormat.Accuracy.kDecimal4);
      format.setAngleMeasureFlag(true);
      pDim.setDimTextFormat(format);

      pDim.setRotation(new OdGeQuaternion(1.0, 0.0, 0.0, 0.0));

      // Geometry
      OdDgDimGeometry geom = new OdDgDimGeometry();
      geom.setDefaultValues();
      pDim.setDimGeometry(geom);

      // Text Info
      OdDgDimTextInfo info = new OdDgDimTextInfo();
      info.setDefaultValues();
      info.setFontEntryId(0);
      pDim.setDimTextInfo(info);
    }

    void addComplexString(int boxRow, int boxColumn)
    {
      //3d
      {
        double height = EntityBoxes.getHeight();
        double width = EntityBoxes.getWidth(boxRow, boxColumn);
        OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

        OdDgComplexString pComplexString = OdDgComplexString.createObject();
        m_pModel3d.addElement(pComplexString);

        OdDgLineString3d pLineString;
        OdGePoint3d pos = new OdGePoint3d();

        pLineString = OdDgLineString3d.createObject();
        pos.set(center.x - width * 0.25, center.y + height * 0.125, 0.0);
        pLineString.addVertex(pos);
        pos.y += height * 0.25;
        pLineString.addVertex(pos);
        pos.x += width * 0.5;
        pLineString.addVertex(pos);
        pos.y -= height * 0.25;
        pLineString.addVertex(pos);

        pComplexString.add(pLineString);

        pLineString = OdDgLineString3d.createObject();
        pos.set(center.x - width * 0.25, center.y - height * 0.125, 0.0);
        pLineString.addVertex(pos);
        pos.y -= height * 0.25;
        pLineString.addVertex(pos);
        pos.x += width * 0.5;
        pLineString.addVertex(pos);
        pos.y += height * 0.25;
        pLineString.addVertex(pos);

        pComplexString.add(pLineString);
      }

      //2d
      {
        double height = EntityBoxes.getHeight();
        double width = EntityBoxes.getWidth(boxRow, boxColumn);
        OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

        OdDgComplexString pComplexString = OdDgComplexString.createObject();
        m_pModel2d.addElement(pComplexString);

        OdDgLine2d line;
        OdDgArc2d arc;

        line = OdDgLine2d.createObject();
        line.setStartPoint(new OdGePoint2d(center.x + width * .45, center.y + height * .25));
        line.setEndPoint(new OdGePoint2d(center.x + width * .25, center.y));
        pComplexString.add(line);

        arc = OdDgArc2d.createObject();
        arc.setOrigin((new OdGePoint2d(center.x, center.y)));
        arc.setPrimaryAxis(width * .25);
        arc.setSecondaryAxis(width * .4);
        arc.setStartAngle(0.0);
        arc.setSweepAngle(6.0);
        pComplexString.add(arc);
      }
    }


    void addMultiline(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgMultiline multiline = OdDgMultiline.createObject();

      //add some profiles
      {
        OdDgMultilineProfile profile = new OdDgMultilineProfile();
        OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

        symbology.setLineWeight(1);
        symbology.setColorIndex(34);
        symbology.setUseColorFlag(true);
        profile.setSymbology(symbology);
        profile.setDistance(0.0);
        multiline.addProfile(profile);

        symbology.setColorIndex(53);
        profile.setSymbology(symbology);
        profile.setDistance(.1);
        multiline.addProfile(profile);

        symbology.setColorIndex(150);
        symbology.setUseStyleFlag(true);
        symbology.setLineStyleEntryId(3);
        profile.setSymbology(symbology);
        profile.setDistance(-.1);
        multiline.addProfile(profile);
      }

      //define caps
      {
        OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

        multiline.setOriginCapAngle(90.0);
        multiline.setEndCapAngle(90.0);

        symbology.setCapLineFlag(true);
        multiline.setOriginCap(symbology);
        multiline.setEndCap(symbology);
      }

      //add some points
      {
        OdDgMultilinePoint point = new OdDgMultilinePoint();
        point.setPoint(center + new OdGeVector3d(-major / 3.0, -minor / 3.0, 0.0));
        multiline.addPoint(point);
        point.setPoint(center + new OdGeVector3d(-major / 3.0, minor / 3.0, 0.0));
        multiline.addPoint(point);
        point.setPoint(center + new OdGeVector3d(major / 3.0, minor / 3.0, 0.0));
        multiline.addPoint(point);
      }
      multiline.setLineWeight(4);

      m_pModel3d.addElement(multiline);
    }


    void addComplexShape(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      //create and add to the model
      OdDgComplexShape complexShape = OdDgComplexShape.createObject();
      m_pModel3d.addElement(complexShape);

      //add some elements
      {
        OdDgCurve3d curve = OdDgCurve3d.createObject();
        curve.addVertex(center + new OdGeVector3d(-3.0 * major / 8.0, 0.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(-2.0 * major / 8.0, 2.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(-1.0 * major / 8.0, 1.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(0.0 * major / 8.0, 2.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(1.0 * major / 8.0, 1.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(2.0 * major / 8.0, 2.0 * minor / 5.0, 0.0));
        curve.addVertex(center + new OdGeVector3d(3.0 * major / 8.0, 0.0 * minor / 5.0, 0.0));

        OdDgLineString3d lineString = OdDgLineString3d.createObject();
        lineString.addVertex(center + new OdGeVector3d(3.0 * major / 8.0, 0.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(2.0 * major / 8.0, -2.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(1.0 * major / 8.0, -1.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(0.0 * major / 8.0, -2.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(-1.0 * major / 8.0, -1.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(-2.0 * major / 8.0, -2.0 * minor / 5.0, 0.0));
        lineString.addVertex(center + new OdGeVector3d(-3.0 * major / 8.0, 0.0 * minor / 5.0, 0.0));

        complexShape.add(curve);
        complexShape.add(lineString);

      }
    }


    void addBSplineCurve(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight() / 6.0;
      double major = EntityBoxes.getWidth(boxRow, boxColumn) / 6.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgBSplineCurve3d curve = OdDgBSplineCurve3d.createObject();
      m_pModel3d.addElement(curve);

        OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major,  2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * major,  2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * major,  2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major,  2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major,  1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major, -2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * major, -2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * major, -2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major, -2.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major, -1.0 * minor, 0.0 ) );
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * major,  1.0 * minor, 0.0 ) );

        OdGeKnotVector vrKnots = new OdGeKnotVector();
        OdGeDoubleArray arrWeights = new OdGeDoubleArray();

        curve.setNurbsData( 4, false, true, arrCtrlPts, vrKnots, arrWeights );
    }


    void addBSplineSurface(int boxRow, int boxColumn)
    {
      double sx = EntityBoxes.getHeight() / 4.0;
      double sy = EntityBoxes.getWidth(boxRow, boxColumn) / 8.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgBSplineSurface surface = OdDgBSplineSurface.createObject();

      m_pModel3d.addElement(surface);
        OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
        OdGeDoubleArray  arrEmptyWeights = new OdGeDoubleArray();
        OdGeKnotVector   arrEmptyKnots = new OdGeKnotVector(); // to set uniform knots.

        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * sx, -2.0 * sy, 0.0 ) ); // 0,0
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx, -1.0 * sy, 0.0 ) ); // 1,0
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx, -1.0 * sy, 0.0 ) ); // 2,0
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * sx, -2.0 * sy, 0.0 ) ); // 3,0
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx, -1.0 * sy, 0.0 ) ); // 0,1
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx, -1.0 * sy, 2.0 ) ); // 1,1
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx, -1.0 * sy, 2.0 ) ); // 2,1
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx, -1.0 * sy, 0.0 ) ); // 3,1
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx,  1.0 * sy, 0.0 ) ); // 0,2
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx,  1.0 * sy, 2.0 ) ); // 1,2
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx,  1.0 * sy, 2.0 ) ); // 2,2
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx,  1.0 * sy, 0.0 ) ); // 3,2
        arrCtrlPts.Add( center + new OdGeVector3d( -2.0 * sx,  2.0 * sy, 0.0 ) ); // 0,3
        arrCtrlPts.Add( center + new OdGeVector3d( -1.0 * sx,  1.0 * sy, 0.0 ) ); // 1,3
        arrCtrlPts.Add( center + new OdGeVector3d(  1.0 * sx,  1.0 * sy, 0.0 ) ); // 2,3
        arrCtrlPts.Add( center + new OdGeVector3d(  2.0 * sx,  2.0 * sy, 0.0 ) ); // 3,3

        surface.set( 4, 4, false, false, false, 4, 4, arrCtrlPts, arrEmptyWeights, arrEmptyKnots, arrEmptyKnots );
        surface.setNumberOfSpansInU(10);
        surface.setNumberOfSpansInV(10);
    }


    void addSurface(int boxRow, int boxColumn)
    {
      double sx = EntityBoxes.getWidth(boxRow, boxColumn) / 3.0;
      double sy = EntityBoxes.getHeight() / 3.0;
      double sz = sx * 2.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgSurface surface = OdDgSurface.createObject();
      OdDg3dObjectHelper helper = new OdDg3dObjectHelper(surface);

      m_pModel3d.addElement(surface);

      //make some surface
      {
        OdDgEllipse3d ellipse;
        OdDgLine3d line;
        const double bias = .8;

        //boundary #1
        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(sx);
        ellipse.setSecondaryAxis(sy);
        ellipse.setOrigin(center);
        helper.addToBoundary(ellipse);

        //boundary #2
        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(sx * bias);
        ellipse.setSecondaryAxis(sy * bias);
        ellipse.setOrigin(center + new OdGeVector3d(0.0, 0.0, sz));
        helper.addToBoundary(ellipse);

        //rule #1
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(-sx, 0.0, 0.0));
        line.setEndPoint(center + new OdGeVector3d(-sx * bias, 0.0, sz));
        helper.addToRule(line);

        //rule #2
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(0.0, sy, 0.0));
        line.setEndPoint(center + new OdGeVector3d(0.0, sy * bias, sz));
        helper.addToRule(line);

        //rule #3
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(sx, 0.0, 0.0));
        line.setEndPoint(center + new OdGeVector3d(sx * bias, 0.0, sz));
        helper.addToRule(line);

        //rule #4
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(0.0, -sy, 0.0));
        line.setEndPoint(center + new OdGeVector3d(0.0, -sy * bias, sz));
        helper.addToRule(line);
      }
    }

    void addSolid(int boxRow, int boxColumn)
    {
      double sx = EntityBoxes.getHeight() / 4.0;
      double sy = EntityBoxes.getWidth(boxRow, boxColumn) / 2.0;
      double sz = sx * 3.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgSolid solid = OdDgSolid.createObject();
      OdDg3dObjectHelper helper = new OdDg3dObjectHelper(solid);

      m_pModel3d.addElement(solid);

      //make some solid
      {
        OdDgShape3d shape;
        OdDgLine3d line;

        //boundary #1
        shape = OdDgShape3d.createObject();
        shape.addVertex(center + new OdGeVector3d(sx, sy, 0.0) * .8);
        shape.addVertex(center + new OdGeVector3d(sx, -sy, 0.0) * .8);
        shape.addVertex(center + new OdGeVector3d(-sx, -sy, 0.0) * .8);
        shape.addVertex(center + new OdGeVector3d(-sx, sy, 0.0) * .8);
        shape.addVertex(center + new OdGeVector3d(sx, sy, 0.0) * .8);
        helper.addToBoundary(shape);

        //boundary #2
        shape = OdDgShape3d.createObject();
        shape.addVertex(center + new OdGeVector3d(sx, sy, sz) * .3);
        shape.addVertex(center + new OdGeVector3d(sx, -sy, sz) * .3);
        shape.addVertex(center + new OdGeVector3d(-sx, -sy, sz) * .3);
        shape.addVertex(center + new OdGeVector3d(-sx, sy, sz) * .3);
        shape.addVertex(center + new OdGeVector3d(sx, sy, sz) * .3);
        helper.addToBoundary(shape);

        //rule #1
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(sx, sy, 0.0) * .8);
        line.setEndPoint(center + new OdGeVector3d(sx, sy, sz) * .3);
        helper.addToRule(line);

        //rule #2
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(sx, -sy, 0.0) * .8);
        line.setEndPoint(center + new OdGeVector3d(sx, -sy, sz) * .3);
        helper.addToRule(line);

        //rule #3
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(-sx, -sy, 0.0) * .8);
        line.setEndPoint(center + new OdGeVector3d(-sx, -sy, sz) * .3);
        helper.addToRule(line);

        //rule #4
        line = OdDgLine3d.createObject();
        line.setStartPoint(center + new OdGeVector3d(-sx, sy, 0.0) * .8);
        line.setEndPoint(center + new OdGeVector3d(-sx, sy, sz) * .3);
        helper.addToRule(line);
      }
    }

    void addLights(int boxRow, int boxColumn)
    {
      double sy = EntityBoxes.getHeight();
      double sx = EntityBoxes.getWidth(boxRow, boxColumn);
      double sz = sy;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      // Add point light

      OdDgLightPoint pPointLight = OdDgLightPoint.createObject();

      m_pModel3d.addElement(pPointLight);

      OdGePoint3d ptPointLightOrigin = center;

      ptPointLightOrigin.x -= sx / 4.0;
      ptPointLightOrigin.z += sz / 4.0;

      pPointLight.setOrigin(ptPointLightOrigin);

      pPointLight.setDefaultValues(true, 4.0);
      pPointLight.setColorRed(1.0);
      pPointLight.setColorGreen(0.0);
      pPointLight.setColorBlue(0.0);
      pPointLight.setIntensity(2.0);

      // Add distant light

      OdDgLightDistant pDistantLight = OdDgLightDistant.createObject();

      m_pModel3d.addElement(pDistantLight);

      OdGePoint3d ptDistantLightOrigin = center;

      ptDistantLightOrigin.x += sx / 4.0;
      ptDistantLightOrigin.z += sz / 4.0;

      pDistantLight.setOrigin(ptDistantLightOrigin);

      OdGeMatrix3d matDistantDirection = OdGeMatrix3d.rotation(Globals.OdaPI, OdGeVector3d.kZAxis);

      pDistantLight.setTransformation(matDistantDirection);

      pDistantLight.setDefaultValues(true, 4.0);
      pDistantLight.setColorRed(0.0);
      pDistantLight.setColorGreen(1.0);
      pDistantLight.setColorBlue(0.0);
      pDistantLight.setIntensity(2.0);

      // Add spot light

      OdDgLightSpot pSpotLight = OdDgLightSpot.createObject();

      m_pModel3d.addElement(pSpotLight);

      OdGePoint3d ptSpotLightOrigin = center;

      ptSpotLightOrigin.y += sy / 3.0;
      ptSpotLightOrigin.z += sz / 4.0;

      pSpotLight.setOrigin(ptSpotLightOrigin);

      OdGeMatrix3d matSpotDirection = new OdGeMatrix3d();
      matSpotDirection.setToRotation(3.0 * Globals.OdaPI / 2.0, OdGeVector3d.kZAxis);

      pSpotLight.setTransformation(matSpotDirection);

      pSpotLight.setDefaultValues(true, 1.0);
      pSpotLight.setColorRed(0.0);
      pSpotLight.setColorGreen(0.0);
      pSpotLight.setColorBlue(1.0);
      pSpotLight.setIntensity(2.0);
      pSpotLight.setConeAngle(Globals.OdaPI / 6.0);
      pSpotLight.setDeltaAngle(Globals.OdaPI / 32.0);

      // Add area light

      OdDgLightArea pAreaLight = OdDgLightArea.createObject();

      m_pModel3d.addElement(pAreaLight);

      OdGePoint3d ptAreaLightOrigin = center;

      ptAreaLightOrigin.y -= sy / 3.0;
      ptAreaLightOrigin.z += sz / 4.0;

      pAreaLight.setOrigin(ptAreaLightOrigin);

      OdGeMatrix3d matAreaDirection = OdGeMatrix3d.rotation(-3.0 * Globals.OdaPI / 4.0, OdGeVector3d.kXAxis);

      pAreaLight.setTransformation(matAreaDirection);

      pAreaLight.setDefaultValues(true, 1.0);
      pAreaLight.setColorRed(0.0);
      pAreaLight.setColorGreen(1.0);
      pAreaLight.setColorBlue(1.0);
      pAreaLight.setIntensity(2.0);

      OdDgShape3d pShapeArea = OdDgShape3d.createObject();

      pAreaLight.add(pShapeArea);

      OdGePoint3d ptCur = ptAreaLightOrigin;
      ptCur.x -= sx / 32;
      ptCur.y += sx / 32;
      ptCur.z += sx / 32;
      pShapeArea.addVertex(ptCur);
      ptCur.x += sx / 16;
      pShapeArea.addVertex(ptCur);
      ptCur.z -= sx / 16;
      ptCur.y -= sx / 16;
      pShapeArea.addVertex(ptCur);
      ptCur.x -= sx / 16;
      pShapeArea.addVertex(ptCur);
      ptCur.z += sx / 16;
      ptCur.y += sx / 16;
      pShapeArea.addVertex(ptCur);

      // Add sky opening light

      OdDgLightOpenSky pSkyLight = OdDgLightOpenSky.createObject();

      m_pModel3d.addElement(pSkyLight);

      OdGePoint3d ptSkyLightOrigin = center;

      ptSkyLightOrigin.z += sz / 2.0;

      pSkyLight.setOrigin(ptSkyLightOrigin);

      OdGeMatrix3d matSkyDirection = OdGeMatrix3d.rotation(Globals.OdaPI, OdGeVector3d.kXAxis);

      pSkyLight.setTransformation(matSkyDirection);
      pSkyLight.setDefaultValues(true, 1.0);
      pSkyLight.setMinSamples(4);
      pSkyLight.setMaxSamples(16);

      OdDgShape3d pShapeSky = OdDgShape3d.createObject();

      pSkyLight.add(pShapeSky);

      ptCur = ptSkyLightOrigin;
      ptCur.x -= sx / 32;
      ptCur.y -= sx / 32;
      pShapeSky.addVertex(ptCur);
      ptCur.x += sx / 16;
      pShapeSky.addVertex(ptCur);
      ptCur.y += sx / 16;
      pShapeSky.addVertex(ptCur);
      ptCur.x -= sx / 16;
      pShapeSky.addVertex(ptCur);
      ptCur.y -= sx / 16;
      pShapeSky.addVertex(ptCur);
    }

    void addRaster(int boxRow, int boxColumn)
    {
      OdDgRasterHeader3d raster = OdDgRasterHeader3d.createObject();
      double sx = EntityBoxes.getHeight() / 2.0;
      double sy = EntityBoxes.getWidth(boxRow, boxColumn) / 2.0;

      raster.setXExtent(8);
      raster.setYExtent(8);
      raster.setFormat(OdDgRaster.RasterFormat.kBitmap);
      raster.setHorizontalDataFlag(true);
      raster.setScale(sy / 8);
      raster.setOrigin(EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(sx / 3.0, 0.0, 0.0));
      raster.setForeground(4);

      m_pModel3d.addElement(raster);

      //add rows
      {
        OdDgRasterComponent component;
        byte b = 0;

        for (UInt16 i = 0; i < 8; i++)       // MKU 23/12/09 - ('warning' issue)
        {
          component = OdDgRasterComponent.createObject();
          component.setFormat(OdDgRaster.RasterFormat.kBitmap);
          component.setOffsetX(0);
          component.setOffsetY(i);
          component.setNumberOfPixels(8);

          switch (i)
          {
            case 0: b = 0x8E; break;
            case 1: b = 0xB3; break;
            case 2: b = 0xC3; break;
            case 3: b = 0xE6; break;
            case 4: b = 0x58; break;
            case 5: b = 0x4C; break;
            case 6: b = 0x87; break;
            case 7: b = 0x83; break;
          }

          component.setData(new byte[] { b });
          raster.add(component);
        }
      }
    }

    void addRasterAttach(int boxRow, int boxColumn)
    {
      {
        OdDgRasterAttachmentHeader raster = OdDgRasterAttachmentHeader.createObject();
        double sx = EntityBoxes.getHeight() / 2.0;
        double rotation = .1;
        double affinity = .03;  //some angles to demonstration

        m_pModel3d.addElement(raster);

        raster.setFilename("attach.jpg");
        raster.setExtent(new OdGePoint2d(1.0, 1.0));
        raster.setOrientation(EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(sx / 3.0, 0.0, 0.0),
                                new OdGeVector3d(Math.Cos(rotation), Math.Sin(rotation), 0.0) * .01,
                                new OdGeVector3d(-Math.Sin(rotation + affinity), Math.Cos(rotation + affinity), 0.0) * .01);

        raster.setApplyRotationFlag(true);

        OdDgRasterFrame rasterFrame = OdDgRasterFrame.createObject();
        m_pModel3d.addElement(rasterFrame);

        OdDgElementId idLevel = m_pModel3d.database().getLevelTable().getAt("level 2");
        if (!idLevel.isNull())
        {
          rasterFrame.setLevelId(idLevel);
        }
        OdDgElementId elementId = raster.elementId();
        rasterFrame.setRasterReferenceId(elementId);
        rasterFrame.setPrintableFlag(true);
      }

      // 2d
      {
        OdDgRasterAttachmentHeader raster2d = OdDgRasterAttachmentHeader.createObject();
        double sx2 = EntityBoxes.getHeight() / 2.0;
        double rotation = 0.1;
        double affinity = 0.03;  //some angles to demonstration

        m_pModel2d.addElement(raster2d);

        raster2d.setFilename("attach.jpg");
        raster2d.setExtent(new OdGePoint2d(1.0, 1.0));
        raster2d.setOrientation(EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(sx2 / 3.0, 0.0, 0.0),
                                  new OdGeVector3d(Math.Cos(rotation), Math.Sin(rotation), 0.0) * .01,
                                  new OdGeVector3d(-Math.Sin(rotation + affinity), Math.Cos(rotation + affinity), 0.0) * .01);

        //raster2d.setApplyRotationFlag( true );
      }
    }

    void addRasterAttachBmp(int boxRow, int boxColumn)
    {
      OdDgRasterAttachmentHeader raster = OdDgRasterAttachmentHeader.createObject();
      double sx = EntityBoxes.getHeight() / 2.0;
      double rotation = Globals.OdaPI2, affinity = -.1;  //some angles to demonstration

      m_pModel3d.addElement(raster);

      raster.setFilename("Example.bmp");
      raster.setExtent(new OdGePoint2d(1.0, 1.0));

      //apply some rotation made via U & V vectors
      raster.setOrientation(EntityBoxes.getBoxCenter(boxRow, boxColumn) - new OdGeVector3d(0.0, sx * .7, 0.0),
                              new OdGeVector3d(Math.Cos(rotation), Math.Sin(rotation), 0.0) * .008,
                              new OdGeVector3d(-Math.Sin(rotation + affinity), Math.Cos(rotation + affinity), 0.0) * .008);

      raster.setApplyRotationFlag(true);
    }

    void addTagElement(int boxRow, int boxColumn)
    {
      OdDgText3d text = OdDgText3d.createObject();
      double sx = EntityBoxes.getHeight() / 2.0,
                        sy = EntityBoxes.getWidth(boxRow, boxColumn) / 2.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      //make the text
      OdDgElementId textId;
      {
        OdGeQuaternion rotation = new OdGeQuaternion(0.707, 0.0, 0.0, -0.707);

        text.setJustification(TextJustification.kCenterCenter);
        text.setText("text with tag");
        text.setOrigin(center - new OdGeVector3d(0.0, sy, 0.0));
        text.setRotation(rotation);
        text.setHeightMultiplier(sy / 3.0);
        text.setLengthMultiplier((sx * 2.0 / 20));

        text.setColorIndex(15);

        textId = m_pModel3d.addElement(text);
      }

      //create new definition set with new tag definition inside
      OdDgElementId setId;
      {
        OdDgTagDefinitionSet definitionSet = OdDgTagDefinitionSet.createObject();
        definitionSet.setName("Set of definitions");

        OdDgTagDefinitionSetTable tagTable = m_pModel3d.database().getTagDefinitionSetTable(OpenMode.kForWrite);
        tagTable.add(definitionSet);
        setId = definitionSet.elementId();

        //add definition for CHAR
        OdDgTagDefinition pDefinition = definitionSet.addDefinition(
          OdDgTagDefinition.Type.kChar, "Name for CHAR");
        pDefinition.setPrompt("Prompt for CHAR");
        pDefinition.setString("Have a nice day!");

        OdDgElementId id = tagTable.getAt("Set of definitions");
        OdDgTagDefinitionSet pTagDefinitionsSetTmp = (OdDgTagDefinitionSet)id.safeOpenObject();
        if (pTagDefinitionsSetTmp != null)
        {
          String sName = pTagDefinitionsSetTmp.getName();
        }
      }

      //create new tag
      {
        OdDgTagElement tag = OdDgTagElement.createObject();
        m_pModel3d.addElement(tag);

        //bind to the parent element
        tag.setAssociationId(textId);

        //bind to the tag definition
        tag.setTagDefinitionId(setId, 0);

        //set some properties
        //tag.setSizeMultiplier( new OdGePoint2d( 10.0, 10.0 ) );
        tag.setLengthMultiplier(0.06);
        tag.setHeightMultiplier(0.06);
        tag.setAssociatedFlag(true);
        tag.setOffsetUsedFlag(true);
        tag.setOffset(new OdGeVector3d(.1, 2.0, 0.0));
        tag.setOrigin(text.getOrigin());
      }
    }

    // note that the meshes should be closed before writing, to rebuild internal structure
    // "using" clause takes care of that
    void addMesh(int boxRow, int boxColumn)
    {
      //take the transformation to transpose the rectangle { -100 < x < 100; -100 < y < 100 } to the given box
      OdGeMatrix3d toBox = new OdGeMatrix3d();
      {
        //scale the axises
        toBox.setToIdentity();
        toBox[0, 0] = EntityBoxes.getWidth(boxRow, boxColumn) / 200.0;
        toBox[1, 1] = EntityBoxes.getHeight() / 200.0;

        //shift to the center
        toBox.setTranslation(EntityBoxes.getBoxCenter(boxRow, boxColumn).asVector());
      }
      //face loops
      using (OdDgMeshFaceLoops mesh = OdDgMeshFaceLoops.createObject())
      {
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(36);

        mesh.setPointsNumber(8);
        mesh.setPoint(0, new OdGePoint3d(-70.0, 20.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, new OdGePoint3d(-70.0, 80.0, 0.0).transformBy(toBox));
        mesh.setPoint(2, new OdGePoint3d(-60.0, 40.0, 0.0).transformBy(toBox));
        mesh.setPoint(3, new OdGePoint3d(-60.0, 60.0, 0.0).transformBy(toBox));
        mesh.setPoint(4, new OdGePoint3d(-50.0, 40.0, 0.0).transformBy(toBox));
        mesh.setPoint(5, new OdGePoint3d(-50.0, 60.0, 0.0).transformBy(toBox));
        mesh.setPoint(6, new OdGePoint3d(-40.0, 20.0, 0.0).transformBy(toBox));
        mesh.setPoint(7, new OdGePoint3d(-40.0, 80.0, 0.0).transformBy(toBox));

        VerticesArray vertices = VerticesArray.Repeat(new OdDgMeshFaceLoops.FacePoint(), 4);
        mesh.setFacesNumber(4);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 3; vertices[3].m_pointIndex = 2;
        vertices[0].m_impliesVisibleEdge = true;
        vertices[1].m_impliesVisibleEdge = false;
        vertices[2].m_impliesVisibleEdge = true;
        vertices[3].m_impliesVisibleEdge = false;
        mesh.setFace(0, vertices);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 2; vertices[2].m_pointIndex = 4; vertices[3].m_pointIndex = 6;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[3].m_impliesVisibleEdge = true;
        mesh.setFace(1, vertices);
        vertices[0].m_pointIndex = 3; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 7; vertices[3].m_pointIndex = 5;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[3].m_impliesVisibleEdge = true;
        mesh.setFace(2, vertices);
        vertices[0].m_pointIndex = 6; vertices[1].m_pointIndex = 4; vertices[2].m_pointIndex = 5; vertices[3].m_pointIndex = 7;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[3].m_impliesVisibleEdge = true;
        mesh.setFace(3, vertices);
      }

      //point cloud
      using (OdDgMeshPointCloud mesh = OdDgMeshPointCloud.createObject())
      {
        m_pModel3d.addElement(mesh);

        mesh.setPointsNumber(4);
        mesh.setPoint(0, new OdGePoint3d(40.0, 20.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, new OdGePoint3d(50.0, 40.0, 0.0).transformBy(toBox));
        mesh.setPoint(2, new OdGePoint3d(60.0, 60.0, 0.0).transformBy(toBox));
        mesh.setPoint(3, new OdGePoint3d(70.0, 80.0, 0.0).transformBy(toBox));
      }

      //triangle list

      using (OdDgMeshTriangleList mesh = OdDgMeshTriangleList.createObject())
      {
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(39);

        OdGePoint3d[] triangle = new OdGePoint3d[3];
        mesh.setTrianglesNumber(3);

        triangle[0] = new OdGePoint3d(-14.0, 20.0, 0.0).transformBy(toBox);
        triangle[1] = new OdGePoint3d(-6.0, 70.0, 0.0).transformBy(toBox);
        triangle[2] = new OdGePoint3d(2.0, 20.0, 0.0).transformBy(toBox);
        mesh.setTriangle(0, triangle);
        triangle[0] = new OdGePoint3d(6.0, 20.0, 0.0).transformBy(toBox);
        triangle[1] = new OdGePoint3d(-2.0, 70.0, 0.0).transformBy(toBox);
        triangle[2] = new OdGePoint3d(14.0, 70.0, 0.0).transformBy(toBox);
        mesh.setTriangle(1, triangle);
        triangle[0] = new OdGePoint3d(10.0, 20.0, 0.0).transformBy(toBox);
        triangle[1] = new OdGePoint3d(14.0, 40.0, 0.0).transformBy(toBox);
        triangle[2] = new OdGePoint3d(18.0, 20.0, 0.0).transformBy(toBox);
        mesh.setTriangle(2, triangle);
      }

      //quad list
      using (OdDgMeshQuadList mesh = OdDgMeshQuadList.createObject())
      {
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(10);

        OdGePoint3d[] quad = new OdGePoint3d[4];
        mesh.setQuadsNumber(3);

        quad[0] = new OdGePoint3d(-40.0, -50.0, 0.0).transformBy(toBox);
        quad[1] = new OdGePoint3d(-50.5, -80.0, 0.0).transformBy(toBox);
        quad[2] = new OdGePoint3d(-43.5, -50.0, 0.0).transformBy(toBox);
        quad[3] = new OdGePoint3d(-50.5, -20.0, 0.0).transformBy(toBox);
        mesh.setQuad(0, quad);
        quad[0] = new OdGePoint3d(-54.0, -20.0, 0.0).transformBy(toBox);
        quad[1] = new OdGePoint3d(-47.0, -50.0, 0.0).transformBy(toBox);
        quad[2] = new OdGePoint3d(-54.0, -80.0, 0.0).transformBy(toBox);
        quad[3] = new OdGePoint3d(-61.0, -50.0, 0.0).transformBy(toBox);
        mesh.setQuad(1, quad);
        quad[0] = new OdGePoint3d(-57.5, -80.0, 0.0).transformBy(toBox);
        quad[1] = new OdGePoint3d(-64.5, -50.0, 0.0).transformBy(toBox);
        quad[2] = new OdGePoint3d(-57.5, -20.0, 0.0).transformBy(toBox);
        quad[3] = new OdGePoint3d(-68.0, -50.0, 0.0).transformBy(toBox);
        mesh.setQuad(2, quad);
      }

      //triangle grid
      using (OdDgMeshTriangleGrid mesh = OdDgMeshTriangleGrid.createObject())
      {
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(40);

        mesh.setGridSize(2, 3);
        mesh.setPoint(0, 0, new OdGePoint3d(-15.0, -50.0, 0.0).transformBy(toBox));
        mesh.setPoint(0, 1, new OdGePoint3d(0.0, -30.0, 0.0).transformBy(toBox));
        mesh.setPoint(0, 2, new OdGePoint3d(15.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 0, new OdGePoint3d(-15.0, -70.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 1, new OdGePoint3d(-10.0, -75.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 2, new OdGePoint3d(-5.0, -80.0, 0.0).transformBy(toBox));
      }

      //quad grid
      using (OdDgMeshQuadGrid mesh = OdDgMeshQuadGrid.createObject())
      {
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(90);

        mesh.setGridSize(2, 4);
        mesh.setPoint(0, 0, new OdGePoint3d(40.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(0, 1, new OdGePoint3d(50.0, -30.0, 0.0).transformBy(toBox));
        mesh.setPoint(0, 2, new OdGePoint3d(60.0, -30.0, 0.0).transformBy(toBox));
        mesh.setPoint(0, 3, new OdGePoint3d(70.0, -40.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 0, new OdGePoint3d(40.0, -90.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 1, new OdGePoint3d(50.0, -70.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 2, new OdGePoint3d(60.0, -70.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, 3, new OdGePoint3d(70.0, -60.0, 0.0).transformBy(toBox));
      }
    }

    void addColorMesh(int boxRow, int boxColumn)
    {
      //take the transformation to transpose the rectangle { -100 < x < 100; -100 < y < 100 } to the given box
      OdGeMatrix3d toBox = new OdGeMatrix3d();
      {
        //scale the axises
        toBox.setToIdentity();
        toBox[0, 0] = EntityBoxes.getWidth(boxRow, boxColumn) / 200.0;
        toBox[1, 1] = EntityBoxes.getHeight() / 200.0;

        //shift to the center
        toBox.setTranslation(EntityBoxes.getBoxCenter(boxRow, boxColumn).asVector());
      }

      //face loops
      {
        OdDgMeshFaceLoops mesh = OdDgMeshFaceLoops.createObject();
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(10);

        mesh.setPointsNumber(8);
        mesh.setPoint(0, new OdGePoint3d(-80.0, -80.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, new OdGePoint3d(-80.0, 80.0, 0.0).transformBy(toBox));
        mesh.setPoint(2, new OdGePoint3d(-50.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(3, new OdGePoint3d(-50.0, 10.0, 0.0).transformBy(toBox));
        mesh.setPoint(4, new OdGePoint3d(-40.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(5, new OdGePoint3d(-40.0, 10.0, 0.0).transformBy(toBox));
        mesh.setPoint(6, new OdGePoint3d(-10.0, -80.0, 0.0).transformBy(toBox));
        mesh.setPoint(7, new OdGePoint3d(-10.0, 80.0, 0.0).transformBy(toBox));

        VerticesArray vertices = VerticesArray.Repeat(new OdDgMeshFaceLoops.FacePoint(), 4);

        mesh.setUseColorTableIndexesFlag(true);

        mesh.setFacesNumber(4);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 3; vertices[3].m_pointIndex = 2;
        vertices[0].m_impliesVisibleEdge = true;
        vertices[0].m_colorIndex = 2;
        vertices[1].m_impliesVisibleEdge = false;
        vertices[1].m_colorIndex = 2;
        vertices[2].m_impliesVisibleEdge = true;
        vertices[2].m_colorIndex = 2;
        vertices[3].m_impliesVisibleEdge = false;
        vertices[3].m_colorIndex = 2;
        mesh.setFace(0, vertices);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 2; vertices[2].m_pointIndex = 4; vertices[3].m_pointIndex = 6;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_colorIndex = 3;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_colorIndex = 3;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_colorIndex = 3;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_colorIndex = 3;
        mesh.setFace(1, vertices);
        vertices[0].m_pointIndex = 3; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 7; vertices[3].m_pointIndex = 5;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_colorIndex = 5;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_colorIndex = 5;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_colorIndex = 5;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_colorIndex = 5;
        mesh.setFace(2, vertices);
        vertices[0].m_pointIndex = 6; vertices[1].m_pointIndex = 4; vertices[2].m_pointIndex = 5; vertices[3].m_pointIndex = 7;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_colorIndex = 8;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_colorIndex = 8;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_colorIndex = 8;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_colorIndex = 8;
        mesh.setFace(3, vertices);

        OdDgMeshFaceLoops meshDColor = OdDgMeshFaceLoops.createObject();
        m_pModel3d.addElement(meshDColor);
        meshDColor.setColorIndex(25);

        meshDColor.setPointsNumber(8);
        meshDColor.setPoint(0, new OdGePoint3d(10.0, -80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(1, new OdGePoint3d(10.0, 80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(2, new OdGePoint3d(40.0, -10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(3, new OdGePoint3d(40.0, 10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(4, new OdGePoint3d(50.0, -10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(5, new OdGePoint3d(50.0, 10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(6, new OdGePoint3d(80.0, -80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(7, new OdGePoint3d(80.0, 80.0, 0.0).transformBy(toBox));

        meshDColor.setUseDoubleColorsFlag(true);

        vertices[0].m_colorIndex = -1;
        vertices[1].m_colorIndex = -1;
        vertices[2].m_colorIndex = -1;
        vertices[3].m_colorIndex = -1;

        meshDColor.setFacesNumber(4);

        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 3; vertices[3].m_pointIndex = 2;

        vertices[0].m_impliesVisibleEdge = true;
        vertices[0].m_dColorRGB = new double[] { 1.0, 1.0, 0.0 };
        vertices[1].m_impliesVisibleEdge = false;
        vertices[1].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[2].m_impliesVisibleEdge = true;
        vertices[2].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[3].m_impliesVisibleEdge = false;
        vertices[3].m_dColorRGB = vertices[0].m_dColorRGB;

        meshDColor.setFace(0, vertices);

        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 2; vertices[2].m_pointIndex = 4; vertices[3].m_pointIndex = 6;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_dColorRGB = new double[] { 1.0, 0.0, 1.0 };
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_dColorRGB = vertices[0].m_dColorRGB;

        meshDColor.setFace(1, vertices);

        vertices[0].m_pointIndex = 3;
        vertices[1].m_pointIndex = 1;
        vertices[2].m_pointIndex = 7;
        vertices[3].m_pointIndex = 5;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_dColorRGB = new double[] { 0.0, 0.5, 1.0 };
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_dColorRGB = vertices[0].m_dColorRGB;

        meshDColor.setFace(2, vertices);

        vertices[0].m_pointIndex = 6;
        vertices[1].m_pointIndex = 4;
        vertices[2].m_pointIndex = 5;
        vertices[3].m_pointIndex = 7;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_dColorRGB = new double[] { 0.5, 1.0, 0.3 };
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_dColorRGB = vertices[0].m_dColorRGB;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_dColorRGB = vertices[0].m_dColorRGB;
        meshDColor.setFace(3, vertices);
      }
    }

    void addNormAndTexMesh(int boxRow, int boxColumn)
    {
      //take the transformation to transpose the rectangle { -100 < x < 100; -100 < y < 100 } to the given box
      OdGeMatrix3d toBox = OdGeMatrix3d.kIdentity;
      {
        //scale the axises
        toBox[0, 0] = EntityBoxes.getWidth(boxRow, boxColumn) / 200.0;
        toBox[1, 1] = EntityBoxes.getHeight() / 200.0;

        //shift to the center
        toBox.setTranslation(EntityBoxes.getBoxCenter(boxRow, boxColumn).asVector());
      }

      //face loops
      {
        OdDgMeshFaceLoops mesh = OdDgMeshFaceLoops.createObject();
        m_pModel3d.addElement(mesh);
        mesh.setColorIndex(10);

        mesh.setPointsNumber(8);
        mesh.setPoint(0, new OdGePoint3d(-80.0, -80.0, 0.0).transformBy(toBox));
        mesh.setPoint(1, new OdGePoint3d(-80.0, 80.0, 0.0).transformBy(toBox));
        mesh.setPoint(2, new OdGePoint3d(-50.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(3, new OdGePoint3d(-50.0, 10.0, 0.0).transformBy(toBox));
        mesh.setPoint(4, new OdGePoint3d(-40.0, -10.0, 0.0).transformBy(toBox));
        mesh.setPoint(5, new OdGePoint3d(-40.0, 10.0, 0.0).transformBy(toBox));
        mesh.setPoint(6, new OdGePoint3d(-10.0, -80.0, 0.0).transformBy(toBox));
        mesh.setPoint(7, new OdGePoint3d(-10.0, 80.0, 0.0).transformBy(toBox));

        VerticesArray vertices = VerticesArray.Repeat(new OdDgMeshFaceLoops.FacePoint(), 4);
        mesh.setUseNormalsFlag(true);

        mesh.setFacesNumber(4);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 3; vertices[3].m_pointIndex = 2;
        vertices[0].m_impliesVisibleEdge = true;
        vertices[0].m_vrNormal = -OdGeVector3d.kXAxis;
        vertices[1].m_impliesVisibleEdge = false;
        vertices[1].m_vrNormal = -OdGeVector3d.kXAxis;
        vertices[2].m_impliesVisibleEdge = true;
        vertices[2].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[3].m_impliesVisibleEdge = false;
        vertices[3].m_vrNormal = OdGeVector3d.kZAxis;
        mesh.setFace(0, vertices);
        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 2; vertices[2].m_pointIndex = 4; vertices[3].m_pointIndex = 6;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_vrNormal = -OdGeVector3d.kYAxis;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_vrNormal = -OdGeVector3d.kYAxis;
        mesh.setFace(1, vertices);
        vertices[0].m_pointIndex = 3; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 7; vertices[3].m_pointIndex = 5;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_vrNormal = OdGeVector3d.kXAxis;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_vrNormal = OdGeVector3d.kXAxis;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_vrNormal = OdGeVector3d.kZAxis;
        mesh.setFace(2, vertices);
        vertices[0].m_pointIndex = 6; vertices[1].m_pointIndex = 4; vertices[2].m_pointIndex = 5; vertices[3].m_pointIndex = 7;
        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_vrNormal = OdGeVector3d.kYAxis;
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_vrNormal = OdGeVector3d.kZAxis;
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_vrNormal = OdGeVector3d.kYAxis;
        mesh.setFace(3, vertices);

        OdDgMeshFaceLoops meshDColor = OdDgMeshFaceLoops.createObject();
        m_pModel3d.addElement(meshDColor);
        meshDColor.setColorIndex(25);

        meshDColor.setPointsNumber(8);
        meshDColor.setPoint(0, new OdGePoint3d(10.0, -80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(1, new OdGePoint3d(10.0, 80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(2, new OdGePoint3d(40.0, -10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(3, new OdGePoint3d(40.0, 10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(4, new OdGePoint3d(50.0, -10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(5, new OdGePoint3d(50.0, 10.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(6, new OdGePoint3d(80.0, -80.0, 0.0).transformBy(toBox));
        meshDColor.setPoint(7, new OdGePoint3d(80.0, 80.0, 0.0).transformBy(toBox));

        meshDColor.setUseTextureCoordinatesFlag(true);

        meshDColor.setFacesNumber(4);

        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 3; vertices[3].m_pointIndex = 2;

        vertices[0].m_impliesVisibleEdge = true;
        vertices[0].m_texCoords = new OdGePoint2d(0.0, 0.0);
        vertices[1].m_impliesVisibleEdge = false;
        vertices[1].m_texCoords = new OdGePoint2d(0.0, 1.0);
        vertices[2].m_impliesVisibleEdge = true;
        vertices[2].m_texCoords = new OdGePoint2d(1.0, 1.0);
        vertices[3].m_impliesVisibleEdge = false;
        vertices[3].m_texCoords = new OdGePoint2d(1.0, 0.0);

        meshDColor.setFace(0, vertices);

        vertices[0].m_pointIndex = 0; vertices[1].m_pointIndex = 2; vertices[2].m_pointIndex = 4; vertices[3].m_pointIndex = 6;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_texCoords = new OdGePoint2d(0.0, 0.0);
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_texCoords = new OdGePoint2d(1.0, 0.0);
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_texCoords = new OdGePoint2d(1.0, 1.0);
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_texCoords = new OdGePoint2d(0.0, 1.0);

        meshDColor.setFace(1, vertices);

        vertices[0].m_pointIndex = 3; vertices[1].m_pointIndex = 1; vertices[2].m_pointIndex = 7; vertices[3].m_pointIndex = 5;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_texCoords = new OdGePoint2d(1.0, 0.0);
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_texCoords = new OdGePoint2d(0.0, 0.0);
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_texCoords = new OdGePoint2d(0.0, 1.0);
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_texCoords = new OdGePoint2d(1.0, 1.0);

        meshDColor.setFace(2, vertices);

        vertices[0].m_pointIndex = 6; vertices[1].m_pointIndex = 4; vertices[2].m_pointIndex = 5; vertices[3].m_pointIndex = 7;

        vertices[0].m_impliesVisibleEdge = false;
        vertices[0].m_texCoords = new OdGePoint2d(0.0, 0.0);
        vertices[1].m_impliesVisibleEdge = true;
        vertices[1].m_texCoords = new OdGePoint2d(1.0, 0.0);
        vertices[2].m_impliesVisibleEdge = false;
        vertices[2].m_texCoords = new OdGePoint2d(1.0, 1.0);
        vertices[3].m_impliesVisibleEdge = true;
        vertices[3].m_texCoords = new OdGePoint2d(0.0, 1.0);

        meshDColor.setFace(3, vertices);

        // Create material with texture for mesh

        OdDgMaterialTable pMatTable = m_pModel3d.database().getMaterialTable(OpenMode.kForWrite);

        OdDgMaterialTableRecord pMat = OdDgMaterialTableRecord.createObject();
        OdGiMaterialColor matColor = new OdGiMaterialColor();
        matColor.setColor(new OdDgCmEntityColor(255, 255, 255));
        matColor.setMethod(OdGiMaterialColor.Method.kOverride);
        matColor.setFactor(1.0);

        OdDgMaterialMap texture = new OdDgMaterialMap();
        OdGiMaterialMap matTexture = new OdGiMaterialMap();
        matTexture.setSourceFileName("attach.jpg");
        matTexture.setBlendFactor(1.0);
        OdGiMapper map = matTexture.mapper();
        map.setTransform(OdGeMatrix3d.scaling(2));
        texture.setGiMaterialMap(matTexture);
        texture.setMappingType(OdDgMaterialMap.MappingType.kElevationDrape);
        texture.setTextureScaleMode(OdDgMaterialMap.ScaleMode.kMasterUnitMode);

        pMat.setDiffuse(matColor, texture);
        pMat.setName("Mesh material");

        pMatTable.add(pMat);

        OdDgInternalMaterialLinkage pMatLinkage = OdDgInternalMaterialLinkage.createObject();
        pMatLinkage.setMaterialTableRecordId((UInt64)(pMat.elementId().getHandle()));

        meshDColor.addLinkage(pMatLinkage.getPrimaryId(), pMatLinkage);
      }
    }

    void addAttach(int boxRow, int boxColumn)
    {
      // MKU 23/12/09 - ('warning' issue) //double            sx = EntityBoxes.getWidth( boxRow, boxColumn ) / 2.0;
      double sy = EntityBoxes.getHeight() / 2.0;
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      {
        OdDgReferenceAttachmentHeader attach = OdDgReferenceAttachmentHeader.createObject();

        //create new element
        m_pModel3d.addElement(attach);
        attach.setFileName("attach.dgn");
        attach.setDisplayFlag(true);
        attach.setSnapLockFlag(true);
        attach.setLocateLockFlag(true);
        attach.setTrueScaleFlag(true);
        attach.setScale(.5);
        attach.setTransformation(OdGeMatrix3d.rotation(1.0, new OdGeVector3d(0.0, 0.0, 1.0)));
        attach.setMasterOrigin(center - new OdGePoint3d(0.0, sy * .5, 0.0).asVector());

        //define as viewable at all levels
        {
          OdDgLevelMask levelMask = OdDgLevelMask.createObject();
          int i;

          for (i = 1; i <= 64; i++)
          {
            levelMask.setLevelIsVisible(1, true);
          }

          attach.add(levelMask);
        }

      }
      {
        OdDgReferenceAttachmentHeader attach = OdDgReferenceAttachmentHeader.createObject();

        //create new element
        m_pModel3d.addElement(attach);
        attach.setFileName("attach1.dgn");
        attach.setDisplayFlag(true);
        attach.setSnapLockFlag(true);
        attach.setLocateLockFlag(true);
        attach.setScale(0.1);
        attach.setTrueScaleFlag(true);

        //define as viewable at all levels
        {
          OdDgLevelMask levelMask = OdDgLevelMask.createObject();
          int i;

          for (i = 1; i <= 64; i++)
          {
            levelMask.setLevelIsVisible(1, true);
          }

          attach.add(levelMask);
        }
      }
    }

    void addStringLinkage(OdDgElement pElm)
    {
      if (pElm == null)
        return;

      UInt32 stringId = 5;
      pElm.setStringLinkage(stringId, "ExDgnCreate Example String");
    }


    void addSharedCells(int boxRow, int boxColumn)
    {
      OdDgSharedCellDefinitionTable table = m_pModel3d.database().getSharedCellDefinitionTable(OpenMode.kForWrite);

      //create a definition
      {
        OdDgSharedCellDefinition definition = OdDgSharedCellDefinition.createObject();
        definition.setName("Named definition");
        table.add(definition);

        OdDgEllipse3d ellipse;

        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(1.0);
        ellipse.setSecondaryAxis(1.0);
        ellipse.setLineWeight(3);
        definition.add(ellipse);

        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(.8);
        ellipse.setSecondaryAxis(.8);
        ellipse.setColorIndex(7);
        ellipse.setLineWeight((UInt32)LineWeightConstants.kLineWeightByCell);                        // MKU 23/12/09 - ('warning' issue)
        definition.add(ellipse);

        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(.2);
        ellipse.setSecondaryAxis(.2);
        ellipse.setOrigin(new OdGePoint3d(.4, 0.0, 0.0));
        ellipse.setColorIndex((UInt32)ColorIndexConstants.kColorByCell);                             // MKU 23/12/09 - ('warning' issue)
        definition.add(ellipse);

        ellipse = OdDgEllipse3d.createObject();
        ellipse.setPrimaryAxis(.2);
        ellipse.setSecondaryAxis(.2);
        ellipse.setOrigin(new OdGePoint3d(-.4, 0.0, 0.0));
        definition.add(ellipse);
      }

      double sx = EntityBoxes.getWidth(boxRow, boxColumn), sy = EntityBoxes.getHeight();
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      //create references
      {
        ;

        OdDgSharedCellReference reference = OdDgSharedCellReference.createObject();
        reference.setDefinitionName("Named definition");
        OdGeMatrix3d transformation = OdGeMatrix3d.rotation(70.0, OdGeVector3d.kZAxis);
        transformation *= OdGeMatrix3d.scaling(sx * 0.1);
        reference.setTransformation(transformation);
        reference.setOrigin(center + new OdGeVector3d(-0.2 * sx, -0.1 * sy, 0.0));
        reference.setColorIndex((UInt32)ColorIndexConstants.kColorByLevel);
        reference.setColorOverrideFlag(true);
        m_pModel3d.addElement(reference);

        reference = OdDgSharedCellReference.createObject();
        reference.setDefinitionName("Named definition");
        transformation.setToRotation(5.0, OdGeVector3d.kZAxis);
        transformation *= OdGeMatrix3d.scaling(sx * 0.03);
        reference.setTransformation(transformation);
        reference.setOrigin(center + new OdGeVector3d(0.0 * sx, 0.2 * sy, 0.0));
        reference.setColorIndex(4);
        reference.setColorOverrideFlag(true);
        reference.setLineWeight(5);
        m_pModel3d.addElement(reference);

        reference = OdDgSharedCellReference.createObject();
        reference.setDefinitionName("Named definition");
        transformation.setToRotation(67.0, OdGeVector3d.kZAxis);
        transformation *= OdGeMatrix3d.scaling(sx * 0.07);
        reference.setTransformation(transformation);
        reference.setOrigin(center + new OdGeVector3d(0.2 * sx, 0.1 * sy, 0.0));
        reference.setColorIndex(5);
        reference.setColorOverrideFlag(true);

        reference.setLineWeight((UInt32)LineWeightConstants.kLineWeightByLevel);
        reference.setWeightOverrideFlag(true);

        OdDgElementId idLevel = m_pModel3d.database().getLevelTable().getAt("ExDgnCreate level 1");
        if (!idLevel.isNull())
        {
          reference.setLevelId(idLevel);
          reference.setLevelOverrideFlag(true);
        }

        m_pModel3d.addElement(reference);
      }
    }


    void addCell(int boxRow, int boxColumn)
    {
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      OdGeVector3d offset = center.asVector();

      //2d
      {
        OdGeVector2d offset2d = new OdGeVector2d(offset.x, offset.y);

        //create a cell
        OdDgCellHeader2d cell = OdDgCellHeader2d.createObject();
        m_pModel2d.addElement(cell);

        //add all elements
        OdDgArc2d arc;
        OdDgLine2d line;

        arc = OdDgArc2d.createObject();
        arc.setPrimaryAxis(0.8865);
        arc.setSecondaryAxis(0.8865);
        arc.setStartAngle(1.3);
        arc.setSweepAngle(5.7);
        arc.setOrigin(new OdGePoint2d(-0.3046, 0.5490) + offset2d);
        arc.setColorIndex(2);
        cell.add(arc);

        line = OdDgLine2d.createObject();
        line.setStartPoint(new OdGePoint2d(-.3, .3) + offset2d);
        line.setEndPoint(new OdGePoint2d(.8, -0.8) + offset2d);
        line.setColorIndex(10);
        cell.add(line);

        line = OdDgLine2d.createObject();
        line.setStartPoint(new OdGePoint2d(-.2, 0.4) + offset2d);
        line.setEndPoint(new OdGePoint2d(.9, -.7) + offset2d);
        line.setColorIndex(10);
        cell.add(line);

        //origin
        cell.setOrigin(new OdGePoint2d(center.x, center.y));

        //rotate its range
        cell.setTransformation(OdGeMatrix2d.rotation(Globals.OdaPI / 6.0));
      }
      //3d
      {
        //create a cell
        OdDgCellHeader3d cell = OdDgCellHeader3d.createObject();
        m_pModel3d.addElement(cell);

        //add all elements
        OdDgArc3d arc;
        OdDgLine3d line;

        arc = OdDgArc3d.createObject();
        arc.setPrimaryAxis(0.8865);
        arc.setSecondaryAxis(0.8865);
        arc.setStartAngle(0.0);
        arc.setSweepAngle(4.0);
        arc.setOrigin(new OdGePoint3d(-0.3046, 0.5490, 0.0000) + offset);
        arc.setColorIndex(2);
        cell.add(arc);

        arc = OdDgArc3d.createObject();
        arc.setPrimaryAxis(0.7865);
        arc.setSecondaryAxis(0.7865);
        arc.setStartAngle(-1.0);
        arc.setSweepAngle(4.0);
        arc.setOrigin(new OdGePoint3d(-0.3046, 0.5490, 0.0000) + offset);
        arc.setColorIndex(2);
        cell.add(arc);

        line = OdDgLine3d.createObject();
        line.setStartPoint(new OdGePoint3d(-1.3020, -0.4469, 0.0000) + offset);
        line.setEndPoint(new OdGePoint3d(-0.6720, 0.2865, 0.0000) + offset);
        line.setColorIndex(10);
        cell.add(line);

        line = OdDgLine3d.createObject();
        line.setStartPoint(new OdGePoint3d(-0.6720, 0.2865, 0.0000) + offset);
        line.setEndPoint(new OdGePoint3d(-1.5190, -0.1796, 0.0000) + offset);
        line.setColorIndex(10);
        cell.add(line);

        line = OdDgLine3d.createObject();
        line.setStartPoint(new OdGePoint3d(-1.5190, -0.1796, 0.0000) + offset);
        line.setEndPoint(new OdGePoint3d(-1.3020, -0.4469, 0.0000) + offset);
        line.setColorIndex(10);
        cell.add(line);

        //origin
        cell.setOrigin(center);

        //rotate its range
        cell.setTransformation(OdGeMatrix3d.rotation(Globals.OdaPI4, new OdGeVector3d(0.0, 0.0, 1.0)));
      }
    }

    void addSmartSolid(int boxRow, int boxColumn)
    {
      byte[] pX_B = {
      0x50, 0x53, 0x00, 0x00, 0x00, 0x33, 0x3A, 0x20, 0x54, 0x52, 0x41, 0x4E, 0x53, 0x4D, 0x49, 0x54 , 0x20, 0x46, 0x49, 0x4C, 0x45, 0x20, 0x63, 0x72, 0x65, 0x61, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79
    , 0x20, 0x6D, 0x6F, 0x64, 0x65, 0x6C, 0x6C, 0x65, 0x72, 0x20, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F , 0x6E, 0x20, 0x31, 0x38, 0x30, 0x30, 0x31, 0x39, 0x37, 0x00, 0x00, 0x00, 0x11, 0x53, 0x43, 0x48
    , 0x5F, 0x31, 0x32, 0x30, 0x30, 0x30, 0x30, 0x30, 0x5F, 0x31, 0x32, 0x30, 0x30, 0x36, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x00, 0x00, 0x17, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01
    , 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x40, 0x8F, 0x40, 0x00, 0x00, 0x00, 0x00, 0x01, 0x3E, 0x45 , 0x79, 0x8E, 0xE2, 0x30, 0x8C, 0x3A, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x01, 0x00, 0x01, 0x01
    , 0x01, 0x00, 0x05, 0x00, 0x06, 0x00, 0x07, 0x00, 0x01, 0x00, 0x08, 0x00, 0x09, 0x00, 0x01, 0x00 , 0x46, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00
    , 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x04, 0x00, 0x0A, 0x00 , 0x0A, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x0D, 0x00, 0x05, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01
    , 0x00, 0x02, 0x00, 0x01, 0x00, 0x0B, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x33 , 0x00, 0x06, 0x00, 0x00, 0x00, 0x12, 0x00, 0x01, 0x00, 0x0D, 0x00, 0x0E, 0x00, 0x01, 0x00, 0x01
    , 0x2B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x1F, 0x00, 0x07, 0x00, 0x00, 0x00, 0x10, 0x00, 0x01, 0x00, 0x09, 0x00, 0x0F, 0x00
    , 0x01, 0x00, 0x01, 0x2B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0xF0, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x02
    , 0x00, 0x0C, 0x00, 0x01, 0x00, 0x10, 0x56, 0x00, 0x10, 0x00, 0x09, 0x00, 0x00, 0x00, 0x0C, 0x00 , 0x11, 0xC2, 0xBC, 0x92, 0x8F, 0x99, 0x6E, 0x00, 0x00, 0x00, 0x12, 0x00, 0x01, 0x00, 0x13, 0x00
    , 0x07, 0x00, 0x01, 0x00, 0x01, 0x00, 0x02, 0x00, 0x51, 0x00, 0x00, 0x00, 0x01, 0x00, 0x11, 0x00 , 0x00, 0x00, 0x16, 0x00, 0x14, 0x00, 0x09, 0x00, 0x01, 0x00, 0x01, 0x00, 0x15, 0x00, 0x16, 0x00
    , 0x17, 0x00, 0x11, 0x00, 0x12, 0x00, 0x01, 0x00, 0x18, 0x00, 0x12, 0x00, 0x12, 0x00, 0x01, 0x00 , 0x19, 0x00, 0x09, 0x00, 0x01, 0x00, 0x01, 0x2B, 0x00, 0x10, 0x00, 0x13, 0x00, 0x00, 0x00, 0x06
    , 0x00, 0x16, 0xC2, 0xBC, 0x92, 0x8F, 0x99, 0x6E, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x09, 0x00, 0x01 , 0x00, 0x0F, 0x00, 0x01, 0x00, 0x01, 0x00, 0x02, 0x00, 0x51, 0x00, 0x00, 0x00, 0x01, 0x00, 0x16
    , 0x00, 0x00, 0x00, 0x17, 0x00, 0x14, 0x00, 0x13, 0x00, 0x01, 0x00, 0x01, 0x00, 0x11, 0x00, 0x01 , 0x00, 0x1B, 0x00, 0x11, 0x00, 0x1A, 0x00, 0x01, 0x00, 0x1C, 0x00, 0x1A, 0x00, 0x1A, 0x00, 0x01
    , 0x00, 0x1D, 0x00, 0x13, 0x00, 0x01, 0x00, 0x01, 0x2B, 0x00, 0x1F, 0x00, 0x0F, 0x00, 0x00, 0x00 , 0x0A, 0x00, 0x01, 0x00, 0x13, 0x00, 0x01, 0x00, 0x07, 0x00, 0x01, 0x2B, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0xF0, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x1C , 0x00, 0x00, 0x00, 0x07, 0x00, 0x01, 0x00, 0x1A, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x11, 0x00, 0x1D
    , 0x00, 0x01, 0x00, 0x1E, 0x00, 0x1D, 0x00, 0x1D, 0x00, 0x01, 0x00, 0x1A, 0x00, 0x13, 0x00, 0x01 , 0x00, 0x01, 0x2D, 0x00, 0x0F, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x00, 0x1D, 0x00
    , 0x1F, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x1F, 0x00, 0x00, 0x00, 0x09, 0x00, 0x20, 0xC2, 0xBC, 0x92 , 0x8F, 0x99, 0x6E, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x0B, 0x00, 0x1E, 0x00, 0x05, 0x00, 0x21, 0x2B
    , 0x00, 0x01, 0x00, 0x01, 0x00, 0x0D, 0x00, 0x0B, 0x00, 0x10, 0x00, 0x51, 0x00, 0x00, 0x00, 0x01 , 0x00, 0x20, 0x00, 0x00, 0x00, 0x14, 0x00, 0x14, 0x00, 0x1F, 0x00, 0x01, 0x00, 0x01, 0x00, 0x22
    , 0x00, 0x15, 0x00, 0x23, 0x00, 0x0E, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x02, 0x00, 0x15, 0xC2, 0xBC , 0x92, 0x8F, 0x99, 0x6E, 0x00, 0x00, 0x00, 0x01, 0x00, 0x1F, 0x00, 0x18, 0x00, 0x05, 0x00, 0x06
    , 0x2B, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x1F, 0x00, 0x10, 0x00, 0x0E, 0x00, 0x0B, 0x00 , 0x00, 0x00, 0x0F, 0x00, 0x22, 0xC2, 0xBC, 0x92, 0x8F, 0x99, 0x6E, 0x00, 0x00, 0x00, 0x1F, 0x00
    , 0x01, 0x00, 0x24, 0x00, 0x05, 0x00, 0x0E, 0x2B, 0x00, 0x01, 0x00, 0x01, 0x00, 0x1F, 0x00, 0x01 , 0x00, 0x10, 0x00, 0x32, 0x00, 0x21, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x01, 0x00, 0x1F, 0x00, 0x01
    , 0x00, 0x0E, 0x00, 0x01, 0x2B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0xF0, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00 , 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00
    , 0x01, 0x00, 0x08, 0x00, 0x0B, 0x00, 0x32, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x11, 0x00, 0x01, 0x00 , 0x0B, 0x00, 0x21, 0x00, 0x06, 0x00, 0x01, 0x2B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    , 0x00, 0x51, 0x00, 0x00, 0x00, 0x01, 0x00, 0x22, 0x00, 0x00, 0x00, 0x13, 0x00, 0x14, 0x00, 0x0B , 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x20, 0x00, 0x25, 0x00, 0x0F, 0x00, 0x24, 0x00, 0x00
    , 0x00, 0x0E, 0x00, 0x01, 0x00, 0x19, 0x00, 0x0B, 0x00, 0x01, 0x00, 0x11, 0x00, 0x19, 0x00, 0x01 , 0x00, 0x24, 0x00, 0x19, 0x00, 0x19, 0x00, 0x01, 0x00, 0x12, 0x00, 0x09, 0x00, 0x01, 0x00, 0x01
    , 0x2D, 0x00, 0x50, 0x00, 0x00, 0x00, 0x01, 0x00, 0x14, 0x00, 0x26, 0x00, 0x27, 0x00, 0x00, 0x23 , 0x28, 0x00, 0x00, 0x00, 0x00, 0x03, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01
    , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x52, 0x00, 0x00, 0x00, 0x02, 0x00, 0x25 , 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x4F, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x27
    , 0x42, 0x53, 0x49, 0x5F, 0x45, 0x6E, 0x74, 0x69, 0x74, 0x79, 0x49, 0x64, 0x00, 0x51, 0x00, 0x00 , 0x00, 0x01, 0x00, 0x15, 0x00, 0x00, 0x00, 0x15, 0x00, 0x14, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x01
    , 0x00, 0x20, 0x00, 0x11, 0x00, 0x28, 0x00, 0x0F, 0x00, 0x18, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x01 , 0x00, 0x12, 0x00, 0x0D, 0x00, 0x1C, 0x00, 0x52, 0x00, 0x00, 0x00, 0x02, 0x00, 0x28, 0x00, 0x00
    , 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x52, 0x00, 0x00, 0x00, 0x02, 0x00, 0x23, 0x00, 0x00 , 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x52, 0x00, 0x00, 0x00, 0x02, 0x00, 0x1B, 0x00, 0x00
    , 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x52, 0x00, 0x00, 0x00, 0x02, 0x00, 0x17, 0x00, 0x00 , 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x13, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x05, 0x00, 0x01
    , 0x00, 0x02, 0x00, 0x01, 0x00, 0x08, 0x00, 0x05, 0x53, 0x00, 0x4A, 0x00, 0x00, 0x00, 0x14, 0x00 , 0x0A, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x16, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00
    , 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00 , 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00
    , 0x01, 0x00, 0x01 };

      double sx = EntityBoxes.getWidth(boxRow, boxColumn);
      // MKU 23/12/09 - ('warning' issue) //double sx = EntityBoxes.getWidth( boxRow, boxColumn ), sy = EntityBoxes.getHeight();
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      center.x += 2 * sx;

      OdMemoryStream pBuf = OdMemoryStream.createNew();
      pBuf.putBytes(pX_B);
      pBuf.rewind();

      OdDgCellHeader3d cell = OdDgCellHeader3d.createObject();

      // pCell must be DBRO entity.

      m_pModel3d.addElement(cell);

      //rotate and scale the cell
      {
        OdGeMatrix3d transformation = OdGeMatrix3d.scaling(.001);
        transformation *= OdGeMatrix3d.rotation(10.0, new OdGeVector3d(1.0, 1.0, 1.0));
        cell.setTransformation(transformation);
      }

      //origin
      cell.setOrigin(center);
      using (OdDgBRepEntityPE pe = OdDgBRepEntityPE.cast(cell))
        pe.fillSmartSolid(cell, pBuf);
    }

    void addPatterns(int boxRow, int boxColumn)
    {
      double sx = EntityBoxes.getWidth(boxRow, boxColumn), sy = EntityBoxes.getHeight();
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);
      OdGeVector3d offset = center.asVector();

      OdDgEllipse3d ellipse;

      //linear hatch
      {
        ellipse = OdDgEllipse3d.createObject();
        ellipse.setOrigin(center + new OdGeVector3d(sx * -0.3, sy * 0.1, 0.0));
        ellipse.setPrimaryAxis(sy / 5.0);
        ellipse.setSecondaryAxis(sy / 5.0);
        m_pModel3d.addElement(ellipse);

        OdDgLinearPatternLinkage linkage = (OdDgLinearPatternLinkage)OdDgLinearPatternLinkage.createObject();
        linkage.setSpace(m_pModel3d.convertWorkingUnitsToUORs(0.2));
        linkage.setAngle(0.45);
        linkage.setLineColorIndex(4);

        ellipse.addLinkage(linkage.getPrimaryId(), linkage);
      }

      //cross hatch
      {
        ellipse = OdDgEllipse3d.createObject();
        ellipse.setOrigin(center + new OdGeVector3d(0.0, sy * -0.1, 0.0));
        ellipse.setPrimaryAxis(sy / 4.0);
        ellipse.setSecondaryAxis(sy / 4.0);
        m_pModel3d.addElement(ellipse);

        OdDgCrossPatternLinkage linkage = (OdDgCrossPatternLinkage)OdDgCrossPatternLinkage.createObject();
        linkage.setSpace1(m_pModel3d.convertWorkingUnitsToUORs(.1));
        linkage.setAngle1(.1);
        linkage.setSpace2(m_pModel3d.convertWorkingUnitsToUORs(.3));
        linkage.setAngle2(.6);
        linkage.setLineColorIndex(6);

        ellipse.addLinkage(linkage.getPrimaryId(), linkage);
      }

      //symbol hatch
      {
        ellipse = OdDgEllipse3d.createObject();
        ellipse.setOrigin(center + new OdGeVector3d(sx * .3, sy * .2, 0.0));
        ellipse.setPrimaryAxis(sy / 3.5);
        ellipse.setSecondaryAxis(sy / 3.5);
        m_pModel3d.addElement(ellipse);

        OdDgSymbolPatternLinkage linkage = (OdDgSymbolPatternLinkage)OdDgSymbolPatternLinkage.createObject();
        linkage.setSpace1(m_pModel3d.convertWorkingUnitsToUORs(.1));
        linkage.setAngle1(0.0);
        linkage.setSpace2(m_pModel3d.convertWorkingUnitsToUORs(.1));
        linkage.setAngle2(.5);
        linkage.setScale(m_pModel3d.convertWorkingUnitsToUORs(.1));

        ellipse.addLinkage(linkage.getPrimaryId(), linkage);

        //find the shared cell definition and link to it
        {
          OdDgSharedCellDefinitionTable table = m_pModel3d.database().getSharedCellDefinitionTable(OpenMode.kForRead);

          OdDgDepLinkageElementId dependency = OdDgDepLinkageElementId.createObject();
          dependency.add((UInt64)table.getAt("Named definition").getHandle());
          dependency.setAppId(0x2714);

          ellipse.addLinkage(dependency.getPrimaryId(), dependency);
        }
      }
    }


    void addDBLinkages(int boxRow, int boxColumn)
    {
      //geometry stuff
      const int samplesNumber = 11; //10 for DB linkages + DMRS linkage
      double sx = EntityBoxes.getWidth(boxRow, boxColumn), sy = EntityBoxes.getHeight();
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      //
      //add all linkages
      //

      //text to create
      OdDgText3d text;

      //linkage to create (non-DRMS)
      OdDgDBLinkage linkage = null;
      String linkageName = "";

      UInt16 sampleIndex;                                                           // MKU 23/12/09 - ('warning' issue)

      for (sampleIndex = 0; sampleIndex < samplesNumber; sampleIndex++)
      {
        //create the bearing text
        {
          text = OdDgText3d.createObject();

          text.setOrigin(center + new OdGeVector3d(sx * (sampleIndex - samplesNumber / 2.0 + .7) / samplesNumber, sy * -.45, 0.0));
          text.setJustification(TextJustification.kCenterCenter);
          text.setRotation(new OdGeQuaternion(.707, 0.0, 0.0, -.707));
          text.setLengthMultiplier(sy / 16.0);
          text.setHeightMultiplier(sx / samplesNumber / 2.0);
          text.setColorIndex(2);

          m_pModel3d.addElement(text);
        }

        //add the DMRS linkage
        if (sampleIndex == 0)
        {
          OdDgDMRSLinkage linkage1 = OdDgDMRSLinkage.createObject();

          linkage1.setTableId(1);
          linkage1.setMSLink(0x1234);

          text.addLinkage(linkage1.getPrimaryId(), linkage1);
          text.setText("DMRS");
          continue;
        }

        //detect the linkage to create and the name to assign
        switch (sampleIndex)
        {
          case 1:
            linkageName = "BSI";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kBSI);
            break;
          case 2:
            linkageName = "FRAMME";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kFRAMME);
            break;
          case 3:
            linkageName = "Informix";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kInformix);
            break;
          case 4:
            linkageName = "Ingres";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kIngres);
            break;
          case 5:
            linkageName = "ODBC";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kODBC);
            break;
          case 6:
            linkageName = "OLE DB";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kOLEDB);
            break;
          case 7:
            linkageName = "Oracle";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kOracle);
            break;
          case 8:
            linkageName = "RIS";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kRIS);
            break;
          case 9:
            linkageName = "Sybase";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kSybase);
            break;
          case 10:
            linkageName = "xBase";
            linkage = OdDgDBLinkage.createObject(OdDgDBLinkage.DBType.kXbase);
            break;
        }

        linkage.setTableEntityId(sampleIndex);
        linkage.setMSLink((UInt32)(sampleIndex + samplesNumber));

        text.setText(linkageName);
        text.addLinkage(linkage.getPrimaryId(), linkage);
      }
    }

    void addTrueColorShapes(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgDatabase database = m_pModel3d.database();

      //first of all, the pink one
      OdDgShape3d pShape = OdDgShape3d.createObject();
      pShape.addVertex(center + new OdGeVector3d(major * -3.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -3.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -3.0 / 9, -minor / 3.0, 0.0));
      m_pModel3d.addElement(pShape);
      pShape.setColor((0xFF9696)); //note: an element should be added to the database to use true colors (because they are managed by the owning database only)

      //and other fancy colors...
      pShape = OdDgShape3d.createObject();
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 9, -minor / 3.0, 0.0));
      m_pModel3d.addElement(pShape);
      pShape.setColor(0x64FA96); //note: an element should be added to the database to use true colors (because they are managed by the owning database only)

      pShape = OdDgShape3d.createObject();
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 3.0 / 9, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 3.0 / 9, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 9, -minor / 3.0, 0.0));
      m_pModel3d.addElement(pShape);
      pShape.setColor(0x6450FF); //note: an element should be added to the database to use true colors (because they are managed by the owning database only)
    }

    void addFilledShape(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d center = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgShape3d pShape = OdDgShape3d.createObject();
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 3, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 3, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 3, minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * 1.0 / 3, -minor / 3.0, 0.0));
      pShape.addVertex(center + new OdGeVector3d(major * -1.0 / 3, -minor / 3.0, 0.0));
      m_pModel3d.addElement(pShape);

      //its boundary
      pShape.setColor(0xFF9696);
      pShape.setLineWeight(3);

      //its inside
      {
        OdDgFillColorLinkage fillColor = OdDgFillColorLinkage.createObject();
        fillColor.setColorIndex(OdDgColorTable.getColorIndexByRGB(pShape.database(), 0x6E64B9)); //any simple index 0...255 could be here
        pShape.addLinkage(fillColor.getPrimaryId(), fillColor);
      }
    }

    void addLineStyleLines(int boxRow, int boxColumn)
    {
      double minor = EntityBoxes.getHeight();
      double major = EntityBoxes.getWidth(boxRow, boxColumn);
      OdGePoint3d boxCenter3d = EntityBoxes.getBoxCenter(boxRow, boxColumn);

      OdDgLineStyleTable pLineStyleTable = m_pModel3d.database().getLineStyleTable(OpenMode.kForRead);

      OdDgElementId idLineStyle = pLineStyleTable.getAt("{ Smiley }");

      OdDgLineStyleTableRecord pSmileyLineStyle = (OdDgLineStyleTableRecord)idLineStyle.openObject(OpenMode.kForRead);

      OdDgLine3d pLine3d;

      pLine3d = OdDgLine3d.createObject();

      pLine3d.setStartPoint(new OdGePoint3d(boxCenter3d.x - major * 3 / 8, boxCenter3d.y - minor * 3.0 / 10.0, 0));
      pLine3d.setEndPoint(new OdGePoint3d(boxCenter3d.x + major * 3 / 8, boxCenter3d.y - minor * 3.0 / 10.0, 0));
      pLine3d.setLineStyleEntryId((Int32)pSmileyLineStyle.getEntryId());
      m_pModel3d.addElement(pLine3d);

      OdDgLineStyleModificationLinkage pScaleLinkage = OdDgLineStyleModificationLinkage.createObject();

      pScaleLinkage.setLineStyleScale(0.2);

      pLine3d.addLinkage(pScaleLinkage.getPrimaryId(), pScaleLinkage);

      pLine3d = OdDgLine3d.createObject();

      pLine3d.setStartPoint(new OdGePoint3d(boxCenter3d.x - major * 3 / 8, boxCenter3d.y - minor * 1.0 / 10.0, 0));
      pLine3d.setEndPoint(new OdGePoint3d(boxCenter3d.x + major * 3 / 8, boxCenter3d.y - minor * 1.0 / 10.0, 0));
      pLine3d.setLineStyleEntryId((Int32)pSmileyLineStyle.getEntryId());
      m_pModel3d.addElement(pLine3d);

      pScaleLinkage = OdDgLineStyleModificationLinkage.createObject();

      pScaleLinkage.setLineStyleScale(0.05);

      pLine3d.addLinkage(pScaleLinkage.getPrimaryId(), pScaleLinkage);
    }
  }
}
