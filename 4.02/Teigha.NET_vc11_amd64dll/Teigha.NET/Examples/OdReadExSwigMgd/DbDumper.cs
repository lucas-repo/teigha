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

namespace OdReadEx
{
  class DbDumper
  {
    /************************************************************************/
    /* Dump the Header Variables                                            */
    /************************************************************************/
    void dumpHeader(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      Console.WriteLine();
      Console.WriteLine("Filename: = {0}", Program.shortenPath(pDb.getFilename()));
      Console.WriteLine("File DWG Version: = {0}", (pDb.originalFileVersion()));

      Console.WriteLine();
      Console.WriteLine("Header Variables:");

      Console.WriteLine();
      OdDbDate Date = pDb.getTDCREATE();
      Console.WriteLine("TDCREATE: = {0}/{1}/{2} {3}:{4}:{5}", Date.year(), Date.month(), Date.day(), Date.hour(), Date.minute(), Date.second());
      Date = pDb.getTDUPDATE();
      Console.WriteLine("TDUPDATE: = {0}/{1}/{2} {3}:{4}:{5}", Date.year(), Date.month(), Date.day(), Date.hour(), Date.minute(), Date.second());

      Console.WriteLine();
      Console.WriteLine("ANGBASE = {0}", (pDb.getANGBASE()));
      Console.WriteLine("ANGDIR = {0}", (pDb.getANGDIR()));
      Console.WriteLine("ATTMODE = {0}", (pDb.getATTMODE()));
      Console.WriteLine("AUNITS = {0}", (pDb.getAUNITS()));
      Console.WriteLine("AUPREC = {0}", (pDb.getAUPREC()));
      Console.WriteLine("CECOLOR = {0}", (pDb.getCECOLOR()));
      Console.WriteLine("CELTSCALE = {0}", (pDb.getCELTSCALE()));
      Console.WriteLine("CHAMFERA = {0}", (pDb.getCHAMFERA()));
      Console.WriteLine("CHAMFERB = {0}", (pDb.getCHAMFERB()));
      Console.WriteLine("CHAMFERC = {0}", (pDb.getCHAMFERC()));
      Console.WriteLine("CHAMFERD = {0}", (pDb.getCHAMFERD()));
      Console.WriteLine("CMLJUST = {0}", (pDb.getCMLJUST()));
      Console.WriteLine("CMLSCALE = {0}", (pDb.getCMLSCALE()));
      Console.WriteLine("DIMADEC = {0}", (pDb.dimadec()));
      Console.WriteLine("DIMALT = {0}", (pDb.dimalt()));
      Console.WriteLine("DIMALTD = {0}", (pDb.dimaltd()));
      Console.WriteLine("DIMALTF = {0}", (pDb.dimaltf()));
      Console.WriteLine("DIMALTRND = {0}", (pDb.dimaltrnd()));
      Console.WriteLine("DIMALTTD = {0}", (pDb.dimalttd()));
      Console.WriteLine("DIMALTTZ = {0}", (pDb.dimalttz()));
      Console.WriteLine("DIMALTU = {0}", (pDb.dimaltu()));
      Console.WriteLine("DIMALTZ = {0}", (pDb.dimaltz()));
      Console.WriteLine("DIMAPOST = {0}", (pDb.dimapost()));
      Console.WriteLine("DIMASZ = {0}", (pDb.dimasz()));
      Console.WriteLine("DIMATFIT = {0}", (pDb.dimatfit()));
      Console.WriteLine("DIMAUNIT = {0}", (pDb.dimaunit()));
      Console.WriteLine("DIMAZIN = {0}", (pDb.dimazin()));
      Console.WriteLine("DIMBLK = {0}", Program.toString(pDb.dimblk()));
      Console.WriteLine("DIMBLK1 = {0}", Program.toString(pDb.dimblk1()));
      Console.WriteLine("DIMBLK2 = {0}", Program.toString(pDb.dimblk2()));
      Console.WriteLine("DIMCEN = {0}", (pDb.dimcen()));
      Console.WriteLine("DIMCLRD = {0}", (pDb.dimclrd()));
      Console.WriteLine("DIMCLRE = {0}", (pDb.dimclre()));
      Console.WriteLine("DIMCLRT = {0}", (pDb.dimclrt()));
      Console.WriteLine("DIMDEC = {0}", (pDb.dimdec()));
      Console.WriteLine("DIMDLE = {0}", (pDb.dimdle()));
      Console.WriteLine("DIMDLI = {0}", (pDb.dimdli()));
      Console.WriteLine("DIMDSEP = {0}", (pDb.dimdsep()));
      Console.WriteLine("DIMEXE = {0}", (pDb.dimexe()));
      Console.WriteLine("DIMEXO = {0}", (pDb.dimexo()));
      Console.WriteLine("DIMFRAC = {0}", (pDb.dimfrac()));
      Console.WriteLine("DIMGAP = {0}", (pDb.dimgap()));
      Console.WriteLine("DIMJUST = {0}", (pDb.dimjust()));
      Console.WriteLine("DIMLDRBLK = {0}", Program.toString(pDb.dimldrblk()));
      Console.WriteLine("DIMLFAC = {0}", (pDb.dimlfac()));
      Console.WriteLine("DIMLIM = {0}", (pDb.dimlim()));
      Console.WriteLine("DIMLUNIT = {0}", (pDb.dimlunit()));
      Console.WriteLine("DIMLWD = {0}", (pDb.dimlwd()));
      Console.WriteLine("DIMLWE = {0}", (pDb.dimlwe()));
      Console.WriteLine("DIMPOST = {0}", (pDb.dimpost()));
      Console.WriteLine("DIMRND = {0}", (pDb.dimrnd()));
      Console.WriteLine("DIMSAH = {0}", (pDb.dimsah()));
      Console.WriteLine("DIMSCALE = {0}", (pDb.dimscale()));
      Console.WriteLine("DIMSD1 = {0}", (pDb.dimsd1()));
      Console.WriteLine("DIMSD2 = {0}", (pDb.dimsd2()));
      Console.WriteLine("DIMSE1 = {0}", (pDb.dimse1()));
      Console.WriteLine("DIMSE2 = {0}", (pDb.dimse2()));
      Console.WriteLine("DIMSOXD = {0}", (pDb.dimsoxd()));
      Console.WriteLine("DIMTAD = {0}", (pDb.dimtad()));
      Console.WriteLine("DIMTDEC = {0}", (pDb.dimtdec()));
      Console.WriteLine("DIMTFAC = {0}", (pDb.dimtfac()));
      Console.WriteLine("DIMTIH = {0}", (pDb.dimtih()));
      Console.WriteLine("DIMTIX = {0}", (pDb.dimtix()));
      Console.WriteLine("DIMTM = {0}", (pDb.dimtm()));
      Console.WriteLine("DIMTOFL = {0}", (pDb.dimtofl()));
      Console.WriteLine("DIMTOH = {0}", (pDb.dimtoh()));
      Console.WriteLine("DIMTOL = {0}", (pDb.dimtol()));
      Console.WriteLine("DIMTOLJ = {0}", (pDb.dimtolj()));
      Console.WriteLine("DIMTP = {0}", (pDb.dimtp()));
      Console.WriteLine("DIMTSZ = {0}", (pDb.dimtsz()));
      Console.WriteLine("DIMTVP = {0}", (pDb.dimtvp()));
      Console.WriteLine("DIMTXSTY = {0}", Program.toString(pDb.dimtxsty()));
      Console.WriteLine("DIMTXT = {0}", (pDb.dimtxt()));
      Console.WriteLine("DIMTZIN = {0}", (pDb.dimtzin()));
      Console.WriteLine("DIMUPT = {0}", (pDb.dimupt()));
      Console.WriteLine("DIMZIN = {0}", (pDb.dimzin()));
      Console.WriteLine("DISPSILH = {0}", (pDb.getDISPSILH()));
      Console.WriteLine("DRAWORDERCTL = {0}", (pDb.getDRAWORDERCTL()));
      Console.WriteLine("ELEVATION = {0}", (pDb.getELEVATION()));
      Console.WriteLine("EXTMAX = {0}", (pDb.getEXTMAX()));
      Console.WriteLine("EXTMIN = {0}", (pDb.getEXTMIN()));
      Console.WriteLine("FACETRES = {0}", (pDb.getFACETRES()));
      Console.WriteLine("FILLETRAD = {0}", (pDb.getFILLETRAD()));
      Console.WriteLine("FILLMODE = {0}", (pDb.getFILLMODE()));
      Console.WriteLine("INSBASE = {0}", (pDb.getINSBASE()));
      Console.WriteLine("ISOLINES = {0}", (pDb.getISOLINES()));
      Console.WriteLine("LIMCHECK = {0}", (pDb.getLIMCHECK()));
      Console.WriteLine("LIMMAX = {0}", (pDb.getLIMMAX()));
      Console.WriteLine("LIMMIN = {0}", (pDb.getLIMMIN()));
      Console.WriteLine("LTSCALE = {0}", (pDb.getLTSCALE()));
      Console.WriteLine("LUNITS = {0}", (pDb.getLUNITS()));
      Console.WriteLine("LUPREC = {0}", (pDb.getLUPREC()));
      Console.WriteLine("MAXACTVP = {0}", (pDb.getMAXACTVP()));
      Console.WriteLine("MIRRTEXT = {0}", (pDb.getMIRRTEXT()));
      Console.WriteLine("ORTHOMODE = {0}", (pDb.getORTHOMODE()));
      Console.WriteLine("PDMODE = {0}", (pDb.getPDMODE()));
      Console.WriteLine("PDSIZE = {0}", (pDb.getPDSIZE()));
      Console.WriteLine("PELEVATION = {0}", (pDb.getPELEVATION()));
      Console.WriteLine("PELLIPSE = {0}", (pDb.getPELLIPSE()));
      Console.WriteLine("PEXTMAX = {0}", (pDb.getPEXTMAX()));
      Console.WriteLine("PEXTMIN = {0}", (pDb.getPEXTMIN()));
      Console.WriteLine("PINSBASE = {0}", (pDb.getPINSBASE()));
      Console.WriteLine("PLIMCHECK = {0}", (pDb.getPLIMCHECK()));
      Console.WriteLine("PLIMMAX = {0}", (pDb.getPLIMMAX()));
      Console.WriteLine("PLIMMIN = {0}", (pDb.getPLIMMIN()));
      Console.WriteLine("PLINEGEN = {0}", (pDb.getPLINEGEN()));
      Console.WriteLine("PLINEWID = {0}", (pDb.getPLINEWID()));
      Console.WriteLine("PROXYGRAPHICS = {0}", (pDb.getPROXYGRAPHICS()));
      Console.WriteLine("PSLTSCALE = {0}", (pDb.getPSLTSCALE()));
      Console.WriteLine("PUCSNAME = {0}", Program.toString(pDb.getPUCSNAME()));
      Console.WriteLine("PUCSORG = {0}", (pDb.getPUCSORG()));
      Console.WriteLine("PUCSXDIR = {0}", (pDb.getPUCSXDIR()));
      Console.WriteLine("PUCSYDIR = {0}", (pDb.getPUCSYDIR()));
      Console.WriteLine("QTEXTMODE = {0}", (pDb.getQTEXTMODE()));
      Console.WriteLine("REGENMODE = {0}", (pDb.getREGENMODE()));
      Console.WriteLine("SHADEDGE = {0}", (pDb.getSHADEDGE()));
      Console.WriteLine("SHADEDIF = {0}", (pDb.getSHADEDIF()));
      Console.WriteLine("SKETCHINC = {0}", (pDb.getSKETCHINC()));
      Console.WriteLine("SKPOLY = {0}", (pDb.getSKPOLY()));
      Console.WriteLine("SPLFRAME = {0}", (pDb.getSPLFRAME()));
      Console.WriteLine("SPLINESEGS = {0}", (pDb.getSPLINESEGS()));
      Console.WriteLine("SPLINETYPE = {0}", (pDb.getSPLINETYPE()));
      Console.WriteLine("SURFTAB1 = {0}", (pDb.getSURFTAB1()));
      Console.WriteLine("SURFTAB2 = {0}", (pDb.getSURFTAB2()));
      Console.WriteLine("SURFTYPE = {0}", (pDb.getSURFTYPE()));
      Console.WriteLine("SURFU = {0}", (pDb.getSURFU()));
      Console.WriteLine("SURFV = {0}", (pDb.getSURFV()));
      Console.WriteLine("TEXTQLTY = {0}", (pDb.getTEXTQLTY()));
      Console.WriteLine("TEXTSIZE = {0}", (pDb.getTEXTSIZE()));
      Console.WriteLine("THICKNESS = {0}", (pDb.getTHICKNESS()));
      Console.WriteLine("TILEMODE = {0}", (pDb.getTILEMODE()));
      Console.WriteLine("TRACEWID = {0}", (pDb.getTRACEWID()));
      Console.WriteLine("TREEDEPTH = {0}", (pDb.getTREEDEPTH()));
      Console.WriteLine("UCSNAME = {0}", Program.toString(pDb.getUCSNAME()));
      Console.WriteLine("UCSORG = {0}", (pDb.getUCSORG()));
      Console.WriteLine("UCSXDIR = {0}", (pDb.getUCSXDIR()));
      Console.WriteLine("UCSYDIR = {0}", (pDb.getUCSYDIR()));
      Console.WriteLine("UNITMODE = {0}", (pDb.getUNITMODE()));
      Console.WriteLine("USERI1 = {0}", (pDb.getUSERI1()));
      Console.WriteLine("USERI2 = {0}", (pDb.getUSERI2()));
      Console.WriteLine("USERI3 = {0}", (pDb.getUSERI3()));
      Console.WriteLine("USERI4 = {0}", (pDb.getUSERI4()));
      Console.WriteLine("USERI5 = {0}", (pDb.getUSERI5()));
      Console.WriteLine("USERR1 = {0}", (pDb.getUSERR1()));
      Console.WriteLine("USERR2 = {0}", (pDb.getUSERR2()));
      Console.WriteLine("USERR3 = {0}", (pDb.getUSERR3()));
      Console.WriteLine("USERR4 = {0}", (pDb.getUSERR4()));
      Console.WriteLine("USERR5 = {0}", (pDb.getUSERR5()));
      Console.WriteLine("USRTIMER = {0}", (pDb.getUSRTIMER()));
      Console.WriteLine("VISRETAIN = {0}", (pDb.getVISRETAIN()));
      Console.WriteLine("WORLDVIEW = {0}", (pDb.getWORLDVIEW()));
      MemoryManager.GetMemoryManager().StopTransaction(mTr);

    }

    /************************************************************************/
    /* Dump a Symbol Table Record                                           */
    /************************************************************************/
    void dumpSymbolTableRecord(OdDbSymbolTableRecord pRecord)
    {
      Console.WriteLine("Xref dependent = {0}", (pRecord.isDependent()));
      if (pRecord.isDependent())
      {
        Console.WriteLine("Resolved = {0}", (pRecord.isResolved()));
      }
    }
    /************************************************************************/
    /* Dump the Layouts                                                     */
    /************************************************************************/
    void dumpLayouts(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

        OdDbDictionary pLayoutDict = (OdDbDictionary)pDb.getLayoutDictionaryId().safeOpenObject();
        OdDbDictionaryIterator pIter = pLayoutDict.newIterator();
        for (; !pIter.done(); pIter.next())
        {
            OdDbObjectId id = pIter.objectId();
            OdDbLayout pEntry = OdDbLayout.cast(id.safeOpenObject());
            if (pEntry == null)
            {
                continue;
            }
            Console.WriteLine();
            Console.WriteLine((pEntry.isA().name()));
            Console.WriteLine("Name = {0}", (pEntry.getLayoutName()));
            OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)pEntry.getBlockTableRecordId().safeOpenObject();
            if (pBlock != null)
            {
                Console.WriteLine((pBlock.isA().name()));
                Console.WriteLine("Name = {0}", (pBlock.getName()));
                OdGeExtents3d extents = new OdGeExtents3d();
                if (OdResult.eOk == pBlock.getGeomExtents(extents))
                {
                    Console.WriteLine("Min Extents = {0}", (extents.minPoint()));
                    Console.WriteLine("Max Extents = {0}", (extents.maxPoint()));
                }
                else
                {
                    Console.WriteLine("   Incorrect extents");
                }
            }
        }
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    /************************************************************************/
    /* Dump the LayerTable                                                  */
    /************************************************************************/
    void dumpLayers(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the LayerTable                               */
      /**********************************************************************/
      OdDbLayerTable pTable = (OdDbLayerTable)pDb.getLayerTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the LayerTable                                        */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /********************************************************************/
        /* Open the LayerTableRecord for Reading                            */
        /********************************************************************/
        OdDbLayerTableRecord pRecord = (OdDbLayerTableRecord)pIter.getRecordId().safeOpenObject();

        /********************************************************************/
        /* Dump the LayerTableRecord                                        */
        /********************************************************************/
        Console.WriteLine();
        Console.WriteLine("<{0}>", pRecord.isA().name());
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("In Use = {0}", (pRecord.isInUse()));
        Console.WriteLine("On = {0}", (!pRecord.isOff()));
        Console.WriteLine("Frozen = {0}", (pRecord.isFrozen()));
        Console.WriteLine("Locked = {0}", (pRecord.isLocked()));
        Console.WriteLine("Color = {0}", (pRecord.color()));
        Console.WriteLine("Linetype = {0}", Program.toString(pRecord.linetypeObjectId()));
        Console.WriteLine("Lineweight = {0}", (pRecord.lineWeight()));
        Console.WriteLine("Plotstyle = {0}", (pRecord.plotStyleName()));
        Console.WriteLine("Plottable = {0}", (pRecord.isPlottable()));
        Console.WriteLine("New VP Freeze = {0}", (pRecord.VPDFLT()));
        dumpSymbolTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the LinetypeTable                                               */
    /************************************************************************/
    void dumpLinetypes(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the LinetypeTable                            */
      /**********************************************************************/
      OdDbLinetypeTable pTable = (OdDbLinetypeTable)pDb.getLinetypeTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the LinetypeTable                                     */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the LinetypeTableRecord for Reading                          */
        /*********************************************************************/
        OdDbLinetypeTableRecord pRecord = (OdDbLinetypeTableRecord)pIter.getRecordId().safeOpenObject();

        /********************************************************************/
        /* Dump the LinetypeTableRecord                                      */
        /********************************************************************/
        Console.WriteLine();
        Console.WriteLine(pRecord.isA().name());
        /********************************************************************/
        /* Dump the first line of record as in ACAD.LIN                     */
        /********************************************************************/
        String buffer = "*" + pRecord.getName();
        if (pRecord.comments() != "")
        {
          buffer += "," + pRecord.comments();
        }
        Console.WriteLine(buffer);

        /********************************************************************/
        /* Dump the second line of record as in ACAD.LIN                    */
        /********************************************************************/
        if (pRecord.numDashes() != 0)
        {
          buffer = (pRecord.isScaledToFit() ? "S" : "A");
          for (int i = 0; i < pRecord.numDashes(); i++)
          {
            buffer += "," + (pRecord.dashLengthAt(i));
            UInt16 shapeNumber = pRecord.shapeNumberAt(i);
            String text = pRecord.textAt(i);

            /**************************************************************/
            /* Dump the Complex Line                                      */
            /**************************************************************/
            if (shapeNumber != 0 || text != "")
            {
              OdDbTextStyleTableRecord pTextStyle = pRecord.shapeStyleAt(i).openObject() as OdDbTextStyleTableRecord;
              if (shapeNumber != 0)
              {
                buffer += ",[" + shapeNumber.ToString() + ",";
                if (pTextStyle != null)
                  buffer += pTextStyle.fileName();
                else
                  buffer += "NULL style";
              }
              else
              {
                buffer += ",[" + (text) + ",";
                if (pTextStyle != null)
                  buffer += pTextStyle.getName();
                else
                  buffer += "NULL style";
              }
              if (pRecord.shapeScaleAt(i) != 0)
              {
                buffer += ",S" + (pRecord.shapeScaleAt(i));
              }
              if (pRecord.shapeRotationAt(i) != 0)
              {
                buffer += ",R" + Program.toDegreeString(pRecord.shapeRotationAt(i));
              }
              if (pRecord.shapeOffsetAt(i).x != 0)
              {
                buffer += ",X" + (pRecord.shapeOffsetAt(i).x);
              }
              if (pRecord.shapeOffsetAt(i).y != 0)
              {
                buffer += ",Y" + (pRecord.shapeOffsetAt(i).y);
              }
              buffer += "]";
            }
          }
          Console.WriteLine(buffer);
        }
        dumpSymbolTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    /************************************************************************/
    /* Dump the TextStyleTable                                              */
    /************************************************************************/
    void dumpTextStyles(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the TextStyleTable                            */
      /**********************************************************************/
      OdDbTextStyleTable pTable = (OdDbTextStyleTable)pDb.getTextStyleTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the TextStyleTable                                    */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the TextStyleTableRecord for Reading                         */
        /*********************************************************************/
        OdDbTextStyleTableRecord pRecord = (OdDbTextStyleTableRecord)pIter.getRecordId().safeOpenObject();

        /*********************************************************************/
        /* Dump the TextStyleTableRecord                                      */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("Shape File = {0}", (pRecord.isShapeFile()));
        Console.WriteLine("Text Height = {0}", (pRecord.textSize()));
        Console.WriteLine("Width Factor = {0}", (pRecord.xScale()));
        Console.WriteLine("Obliquing Angle = {0}", Program.toDegreeString(pRecord.obliquingAngle()));
        Console.WriteLine("Backwards = {0}", (pRecord.isBackwards()));
        Console.WriteLine("Vertical = {0}", (pRecord.isVertical()));
        Console.WriteLine("Upside Down = {0}", (pRecord.isUpsideDown()));
        Console.WriteLine("Filename = {0}", Program.shortenPath(pRecord.fileName()));
        Console.WriteLine("BigFont Filename = {0}", Program.shortenPath(pRecord.bigFontFileName()));

        String typeface = "";
        bool bold;
        bool italic;
        int charset;
        int pitchAndFamily;
        pRecord.font(ref typeface, out bold, out italic, out charset, out pitchAndFamily);
        Console.WriteLine("Typeface = {0}", (typeface));
        Console.WriteLine("Character Set = {0}", (charset));
        Console.WriteLine("Bold = {0}", (bold));
        Console.WriteLine("Italic = {0}", (italic));
        Console.WriteLine("Font Pitch & Family = {0:X}", pitchAndFamily);
        dumpSymbolTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    /************************************************************************/
    /* Dump the DimStyleTable                                               */
    /************************************************************************/
    void dumpDimStyles(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the DimStyleTable                            */
      /**********************************************************************/
      OdDbDimStyleTable pTable = (OdDbDimStyleTable)pDb.getDimStyleTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the DimStyleTable                                    */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the DimStyleTableRecord for Reading                         */
        /*********************************************************************/
        OdDbDimStyleTableRecord pRecord = (OdDbDimStyleTableRecord)pIter.getRecordId().safeOpenObject();

        /*********************************************************************/
        /* Dump the DimStyleTableRecord                                      */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("Arc Symbol = {0}", Program.toArcSymbolTypeString(pRecord.getArcSymbolType()));

        OdCmColor bgrndTxtColor = new OdCmColor();
        UInt16 bgrndTxtFlags = pRecord.getBgrndTxtColor(bgrndTxtColor);
        Console.WriteLine("Background Text Color = {0}", (bgrndTxtColor));
        Console.WriteLine("BackgroundText Flags = {0}", (bgrndTxtFlags));
        Console.WriteLine("Extension Line 1 Linetype = {0}", Program.toString(pRecord.getDimExt1Linetype()));
        Console.WriteLine("Extension Line 2 Linetype = {0}", Program.toString(pRecord.getDimExt2Linetype()));
        Console.WriteLine("Dimension Line Linetype = {0}", Program.toString(pRecord.getDimExt2Linetype()));
        Console.WriteLine("Extension Line Fixed Len = {0}", (pRecord.getExtLineFixLen()));
        Console.WriteLine("Extension Line Fixed Len Enable = {0}", (pRecord.getExtLineFixLenEnable()));
        Console.WriteLine("Jog Angle = {0}", Program.toDegreeString(pRecord.getJogAngle()));
        Console.WriteLine("Modified For Recompute = {0}", (pRecord.isModifiedForRecompute()));
        Console.WriteLine("DIMADEC = {0}", (pRecord.dimadec()));
        Console.WriteLine("DIMALT = {0}", (pRecord.dimalt()));
        Console.WriteLine("DIMALTD = {0}", (pRecord.dimaltd()));
        Console.WriteLine("DIMALTF = {0}", (pRecord.dimaltf()));
        Console.WriteLine("DIMALTRND = {0}", (pRecord.dimaltrnd()));
        Console.WriteLine("DIMALTTD = {0}", (pRecord.dimalttd()));
        Console.WriteLine("DIMALTTZ = {0}", (pRecord.dimalttz()));
        Console.WriteLine("DIMALTU = {0}", (pRecord.dimaltu()));
        Console.WriteLine("DIMALTZ = {0}", (pRecord.dimaltz()));
        Console.WriteLine("DIMAPOST = {0}", (pRecord.dimapost()));
        Console.WriteLine("DIMASZ = {0}", (pRecord.dimasz()));
        Console.WriteLine("DIMATFIT = {0}", (pRecord.dimatfit()));
        Console.WriteLine("DIMAUNIT = {0}", (pRecord.dimaunit()));
        Console.WriteLine("DIMAZIN = {0}", (pRecord.dimazin()));
        Console.WriteLine("DIMBLK = {0}", Program.toString(pRecord.dimblk()));
        Console.WriteLine("DIMBLK1 = {0}", Program.toString(pRecord.dimblk1()));
        Console.WriteLine("DIMBLK2 = {0}", Program.toString(pRecord.dimblk2()));
        Console.WriteLine("DIMCEN = {0}", (pRecord.dimcen()));
        Console.WriteLine("DIMCLRD = {0}", (pRecord.dimclrd()));
        Console.WriteLine("DIMCLRE = {0}", (pRecord.dimclre()));
        Console.WriteLine("DIMCLRT = {0}", (pRecord.dimclrt()));
        Console.WriteLine("DIMDEC = {0}", (pRecord.dimdec()));
        Console.WriteLine("DIMDLE = {0}", (pRecord.dimdle()));
        Console.WriteLine("DIMDLI = {0}", (pRecord.dimdli()));
        Console.WriteLine("DIMDSEP = {0}", (pRecord.dimdsep()));
        Console.WriteLine("DIMEXE = {0}", (pRecord.dimexe()));
        Console.WriteLine("DIMEXO = {0}", (pRecord.dimexo()));
        Console.WriteLine("DIMFRAC = {0}", (pRecord.dimfrac()));
        Console.WriteLine("DIMGAP = {0}", (pRecord.dimgap()));
        Console.WriteLine("DIMJUST = {0}", (pRecord.dimjust()));
        Console.WriteLine("DIMLDRBLK = {0}", Program.toString(pRecord.dimldrblk()));
        Console.WriteLine("DIMLFAC = {0}", (pRecord.dimlfac()));
        Console.WriteLine("DIMLIM = {0}", (pRecord.dimlim()));
        Console.WriteLine("DIMLUNIT = {0}", (pRecord.dimlunit()));
        Console.WriteLine("DIMLWD = {0}", (pRecord.dimlwd()));
        Console.WriteLine("DIMLWE = {0}", (pRecord.dimlwe()));
        Console.WriteLine("DIMPOST = {0}", (pRecord.dimpost()));
        Console.WriteLine("DIMRND = {0}", (pRecord.dimrnd()));
        Console.WriteLine("DIMSAH = {0}", (pRecord.dimsah()));
        Console.WriteLine("DIMSCALE = {0}", (pRecord.dimscale()));
        Console.WriteLine("DIMSD1 = {0}", (pRecord.dimsd1()));
        Console.WriteLine("DIMSD2 = {0}", (pRecord.dimsd2()));
        Console.WriteLine("DIMSE1 = {0}", (pRecord.dimse1()));
        Console.WriteLine("DIMSE2 = {0}", (pRecord.dimse2()));
        Console.WriteLine("DIMSOXD = {0}", (pRecord.dimsoxd()));
        Console.WriteLine("DIMTAD = {0}", (pRecord.dimtad()));
        Console.WriteLine("DIMTDEC = {0}", (pRecord.dimtdec()));
        Console.WriteLine("DIMTFAC = {0}", (pRecord.dimtfac()));
        Console.WriteLine("DIMTIH = {0}", (pRecord.dimtih()));
        Console.WriteLine("DIMTIX = {0}", (pRecord.dimtix()));
        Console.WriteLine("DIMTM = {0}", (pRecord.dimtm()));
        Console.WriteLine("DIMTOFL = {0}", (pRecord.dimtofl()));
        Console.WriteLine("DIMTOH = {0}", (pRecord.dimtoh()));
        Console.WriteLine("DIMTOL = {0}", (pRecord.dimtol()));
        Console.WriteLine("DIMTOLJ = {0}", (pRecord.dimtolj()));
        Console.WriteLine("DIMTP = {0}", (pRecord.dimtp()));
        Console.WriteLine("DIMTSZ = {0}", (pRecord.dimtsz()));
        Console.WriteLine("DIMTVP = {0}", (pRecord.dimtvp()));
        Console.WriteLine("DIMTXSTY = {0}", Program.toString(pRecord.dimtxsty()));
        Console.WriteLine("DIMTXT = {0}", pRecord.dimtxt());
        Console.WriteLine("DIMTZIN = {0}", (pRecord.dimtzin()));
        Console.WriteLine("DIMUPT = {0}", (pRecord.dimupt()));
        Console.WriteLine("DIMZIN = {0}", (pRecord.dimzin()));
        dumpSymbolTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the RegAppTable                                              */
    /************************************************************************/
    void dumpRegApps(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the RegAppTable                            */
      /**********************************************************************/
      OdDbRegAppTable pTable = (OdDbRegAppTable)pDb.getRegAppTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the RegAppTable                                    */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the RegAppTableRecord for Reading                         */
        /*********************************************************************/
        OdDbRegAppTableRecord pRecord = (OdDbRegAppTableRecord)pIter.getRecordId().safeOpenObject();

        /*********************************************************************/
        /* Dump the RegAppTableRecord                                      */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the AbstractViewTableRecord                                     */
    /*************************************************************************/
    void dumpAbstractViewTableRecord(OdDbAbstractViewTableRecord pView)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /*********************************************************************/
      /* Dump the AbstractViewTableRecord                                  */
      /*********************************************************************/
      Console.WriteLine("Back Clip Dist = {0}", (pView.backClipDistance()));
      Console.WriteLine("Back Clip Enabled = {0}", (pView.backClipEnabled()));
      Console.WriteLine("Front Clip Dist = {0}", (pView.frontClipDistance()));
      Console.WriteLine("Front Clip Enabled = {0}", (pView.frontClipEnabled()));
      Console.WriteLine("Front Clip at Eye = {0}", (pView.frontClipAtEye()));
      Console.WriteLine("Elevation = {0}", (pView.elevation()));
      Console.WriteLine("Height = {0}", (pView.height()));
      Console.WriteLine("Width = {0}", (pView.width()));
      Console.WriteLine("Lens Length = {0}", (pView.lensLength()));
      Console.WriteLine("Render Mode = {0}", (pView.renderMode()));
      Console.WriteLine("Perspective = {0}", (pView.perspectiveEnabled()));
      Console.WriteLine("UCS Name = {0}", Program.toString(pView.ucsName()));

      OrthographicView orthoUCS;
      Console.WriteLine("UCS Orthographic = {0}", (pView.isUcsOrthographic(out orthoUCS)));
      Console.WriteLine("Orthographic UCS = {0}", (orthoUCS));
      OdGePoint3d origin = new OdGePoint3d();
      OdGeVector3d xAxis = new OdGeVector3d();
      OdGeVector3d yAxis = new OdGeVector3d();
      pView.getUcs(origin, xAxis, yAxis);
      Console.WriteLine("UCS Origin = {0}", (origin));
      Console.WriteLine("UCS x-Axis = {0}", (xAxis));
      Console.WriteLine("UCS y-Axis = {0}", (yAxis));
      Console.WriteLine("Target = {0}", (pView.target()));
      Console.WriteLine("View Direction = {0}", (pView.viewDirection()));
      Console.WriteLine("Twist Angle = {0}", Program.toDegreeString(pView.viewTwist()));
      dumpSymbolTableRecord(pView);
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the ViewportTable                                              */
    /************************************************************************/
    void dumpViewports(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the ViewportTable                            */
      /**********************************************************************/
      OdDbViewportTable pTable = (OdDbViewportTable)pDb.getViewportTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine((pTable.isA().name()));

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the ViewportTable                                    */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the ViewportTableRecord for Reading                          */
        /*********************************************************************/
        OdDbViewportTableRecord pRecord = (OdDbViewportTableRecord)pIter.getRecordId().safeOpenObject();

        /*********************************************************************/
        /* Dump the ViewportTableRecord                                      */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("Circle Sides = {0}", (pRecord.circleSides()));
        Console.WriteLine("Fast Zooms Enabled = {0}", (pRecord.fastZoomsEnabled()));
        Console.WriteLine("Grid Enabled = {0}", (pRecord.gridEnabled()));
        Console.WriteLine("Grid Increments = {0}", (pRecord.gridIncrements()));
        Console.WriteLine("Icon at Origin = {0}", (pRecord.iconAtOrigin()));
        Console.WriteLine("Icon Enabled = {0}", (pRecord.iconEnabled()));
        Console.WriteLine("Iso snap Enabled = {0}", (pRecord.isometricSnapEnabled()));
        Console.WriteLine("Iso Snap Pair = {0}", (pRecord.snapPair()));
        Console.WriteLine("UCS Saved w/Vport = {0}", (pRecord.isUcsSavedWithViewport()));
        Console.WriteLine("UCS follow = {0}", (pRecord.ucsFollowMode()));
        Console.WriteLine("Lower-Left Corner = {0}", (pRecord.lowerLeftCorner()));
        Console.WriteLine("Upper-Right Corner = {0}", (pRecord.upperRightCorner()));
        Console.WriteLine("Snap Angle = {0}", Program.toDegreeString(pRecord.snapAngle()));
        Console.WriteLine("Snap Base = {0}", (pRecord.snapBase()));
        Console.WriteLine("Snap Enabled = {0}", (pRecord.snapEnabled()));
        Console.WriteLine("Snap Increments = {0}", (pRecord.snapIncrements()));
        dumpAbstractViewTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the ViewTable                                                   */
    /************************************************************************/
    void dumpViews(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the ViewTable                                */
      /**********************************************************************/
      OdDbViewTable pTable = (OdDbViewTable)pDb.getViewTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine((pTable.isA().name()));

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the ViewTable                                         */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /*********************************************************************/
        /* Open the ViewTableRecord for Reading                              */
        /*********************************************************************/
        OdDbViewTableRecord pRecord = (OdDbViewTableRecord)pIter.getRecordId().safeOpenObject();

        /*********************************************************************/
        /* Dump the ViewTableRecord                                          */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("Category Name = {0}", (pRecord.getCategoryName()));
        Console.WriteLine("Layer State = {0}", (pRecord.getLayerState()));

        String layoutName = "";
        if (!pRecord.getLayout().isNull())
        {
          OdDbLayout pLayout = (OdDbLayout)pRecord.getLayout().safeOpenObject();
          layoutName = pLayout.getLayoutName();
        }
        Console.WriteLine("Layout Name = {0}", (layoutName));
        Console.WriteLine("PaperSpace View = {0}", (pRecord.isPaperspaceView()));
        Console.WriteLine("Associated UCS = {0}", (pRecord.isUcsAssociatedToView()));
        Console.WriteLine("PaperSpace View = {0}", (pRecord.isViewAssociatedToViewport()));
        dumpAbstractViewTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    /************************************************************************/
    /* Dump the MlineStyle Dictionary                                       */
    /************************************************************************/
    void dumpMLineStyles(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      OdDbDictionary pDictionary = (OdDbDictionary)pDb.getMLStyleDictionaryId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine((pDictionary.isA().name()));

      /**********************************************************************/
      /* Get a SmartPointer to a new DictionaryIterator                     */
      /**********************************************************************/
      OdDbDictionaryIterator pIter = pDictionary.newIterator();

      /**********************************************************************/
      /* Step through the MlineStyle dictionary                             */
      /**********************************************************************/
      for (; !pIter.done(); pIter.next())
      {
        OdDbObjectId id = pIter.objectId();
        OdDbMlineStyle pEntry = OdDbMlineStyle.cast(id.safeOpenObject());
        if (pEntry == null)
          continue;

        /*********************************************************************/
        /* Dump the MLineStyle dictionary entry                              */
        /*********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pEntry.isA().name()));
        Console.WriteLine("Name = {0}", (pEntry.name()));
        Console.WriteLine("Description = {0}", (pEntry.description()));
        Console.WriteLine("Start Angle = {0}", Program.toDegreeString(pEntry.startAngle()));
        Console.WriteLine("End Angle = {0}", Program.toDegreeString(pEntry.endAngle()));
        Console.WriteLine("Start Inner Arcs = {0}", (pEntry.startInnerArcs()));
        Console.WriteLine("End Inner Arcs = {0}", (pEntry.endInnerArcs()));
        Console.WriteLine("Start Round Cap = {0}", (pEntry.startRoundCap()));
        Console.WriteLine("End Round Cap = {0}", (pEntry.endRoundCap()));
        Console.WriteLine("Start Square Cap = {0}", (pEntry.startRoundCap()));
        Console.WriteLine("End Square Cap = {0}", (pEntry.endRoundCap()));
        Console.WriteLine("Show Miters = {0}", (pEntry.showMiters()));
        /*********************************************************************/
        /* Dump the elements                                                 */
        /*********************************************************************/
        if (pEntry.numElements() != 0)
        {
          Console.WriteLine("Elements:");
        }
        for (int i = 0; i < pEntry.numElements(); i++)
        {
          double offset;
          OdCmColor color = new OdCmColor();
          OdDbObjectId linetypeId = new OdDbObjectId();
          pEntry.getElementAt(i, out offset, color, linetypeId);
          Console.WriteLine("Index = {0}", (i));
          Console.WriteLine("Offset = {0}", (offset));
          Console.WriteLine("Color = {0}", (color));
          Console.WriteLine("Linetype = {0}", Program.toString(linetypeId));
        }
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    /************************************************************************/
    /* Dump the UCSTable                                                    */
    /************************************************************************/
    void dumpUCSTable(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the UCSTable                               */
      /**********************************************************************/
      OdDbUCSTable pTable = (OdDbUCSTable)pDb.getUCSTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the UCSTable                                          */
      /**********************************************************************/
      for (pIter.start(); !pIter.done(); pIter.step())
      {
        /********************************************************************/
        /* Open the UCSTableRecord for Reading                            */
        /********************************************************************/
        OdDbUCSTableRecord pRecord = (OdDbUCSTableRecord)pIter.getRecordId().safeOpenObject();

        /********************************************************************/
        /* Dump the UCSTableRecord                                        */
        /********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pRecord.isA().name()));
        Console.WriteLine("Name = {0}", (pRecord.getName()));
        Console.WriteLine("UCS Origin = {0}", (pRecord.origin()));
        Console.WriteLine("UCS x-Axis = {0}", (pRecord.xAxis()));
        Console.WriteLine("UCS y-Axis = {0}", (pRecord.yAxis()));
        dumpSymbolTableRecord(pRecord);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the Entity                                                      */
    /************************************************************************/
    void dumpEntity(OdDbObjectId id)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the Entity                                   */
      /**********************************************************************/
      OdDbEntity pEnt = OdDbEntity.cast(id.openObject());
      if (pEnt == null)
        return;

      //////// Grip points tes ///////////
      OdDbCircle pCirc = OdDbCircle.cast(pEnt);
      if (null != pCirc)
      {
          Console.WriteLine("dumpEntity Circle");
          OdGePoint3dArray s_pts = new OdGePoint3dArray();
          pCirc.getStretchPoints(s_pts);
      }
      ////////////////////////////////////

      /////////////// Get osnap points ///////////////
      OdDbLine pLine = OdDbLine.cast(pEnt);
      if (pLine != null)
      {
          OdGePoint3dArray s_pts = new OdGePoint3dArray();
          // .getOsnapPoints() call illustrates overruling and use of isApplicable method
          // an overridden method getOsnapPoints (Overrule class) will be called for an OdDbLine object successfully,
          // as it is properly implemeneted and has an isApplicable method overriden
          pLine.getOsnapPoints(OsnapMode.kOsModeApint, new IntPtr(1), new OdGePoint3d(0, 0, 0), new OdGePoint3d(1, 1, 1), new OdGeMatrix3d(), s_pts);
      }
      ////////////////////////////////////////////////

      ///////////////// OdDbMPolygon test ////////////
      OdDbMPolygon mpolygon = OdDbMPolygon.cast(pEnt);
      if (mpolygon != null)
      {
          Console.WriteLine("OdDbMPolygon fields");
          // test DbPolygon.h declared methods
          // to call hatch file should be open with write privileges
          /*OdDbHatch hatch = mpolygon.hatch();
          OdGePoint2dArray vertices = new OdGePoint2dArray();
          OdGeDoubleArray bulges = new OdGeDoubleArray();
          for (int loopIndex = 0; loopIndex < hatch.numLoops(); loopIndex++)
          {
              hatch.getLoopAt(loopIndex, vertices, bulges);
              bool hasBulges = (bulges.Count > 0);
              for (int i = 0; i < (int)vertices.Count; i++)
              {
                  Console.WriteLine(string.Format("Vertex %d ", i) + vertices[i]);
                  if (hasBulges)
                  {
                      Console.WriteLine(string.Format("Bulge %d ", i) + bulges[i].ToString());
                      Console.WriteLine(string.Format("Bulge angle %d ", i) + (4 * Math.Atan(bulges[i])).ToString());
                  }
              }
          }*/
          Console.WriteLine("Elevation " + mpolygon.elevation().ToString());
          Console.WriteLine("Normal " + mpolygon.normal().ToString());
          Console.WriteLine("Pattern name " + mpolygon.patternName());

          Console.WriteLine(string.Format("Pattern angle {0}", mpolygon.patternAngle()));
          Console.WriteLine(string.Format("Pattern space {0}", mpolygon.patternSpace()));
          Console.WriteLine(string.Format("Pattern scale {0}", mpolygon.patternScale()));
          int numDefinitions = mpolygon.numPatternDefinitions();

          double angle = 0;
          double baseX = 0;
          double baseY = 0;
          double offsetX = 0;
          double offsetY = 0;
          OdGeDoubleArray dashes = new OdGeDoubleArray();
          for (int i = 0; i < numDefinitions; i++)
          {
              mpolygon.getPatternDefinitionAt(i, out angle, out baseX, out baseY, out offsetX, out offsetY, dashes);
              Console.WriteLine(string.Format("Definition {0}", i));
              Console.WriteLine(string.Format("angle {0}", angle));
              Console.WriteLine(string.Format("baseX {0}", baseX));
              Console.WriteLine(string.Format("baseY {0}", baseY));
              Console.WriteLine(string.Format("offsetX {0}", offsetX));
              Console.WriteLine(string.Format("offsetY {0}", offsetY));
              Console.WriteLine(string.Format("dashes number {0}", dashes.Count));
          }
          double ar = 0 ;
          mpolygon.getArea(out ar);
          Console.WriteLine(string.Format("Area {0}", ar));
          Console.WriteLine("Offset vector " + mpolygon.getOffsetVector().ToString());
      }
      ////////////////////////////////////////////////

      /**********************************************************************/
      /* Dump the entity                                                    */
      /**********************************************************************/
      Console.WriteLine();
        // .list() call illustrates overruling and use of isApplicable method
        // for entities, which have an isApplicable method implemented in their _Dumper class,
        // pEnt.list() call will be performed successfully
        // for entities, which do not have an isApplicable method implemented in their _Dumper class,
        // pEnt.list() will generate an exception "pure virtual method call"
      try
      {
          pEnt.list();
      }
      catch (Exception)
      {
          Console.WriteLine("Problem object is: " + pEnt.isA().name());
      }

      /**********************************************************************/
      /* Dump the Xdata                                                     */
      /**********************************************************************/
      dumpXdata(pEnt.xData());

      /**********************************************************************/
      /* Dump the Extension Dictionary                                      */
      /**********************************************************************/
      if (!pEnt.extensionDictionary().isNull())
      {
        dumpObject(pEnt.extensionDictionary(), "ACAD_XDICTIONARY");
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the BlockTable                                                  */
    /************************************************************************/
    void dumpBlocks(OdDbDatabase pDb)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the BlockTable                               */
      /**********************************************************************/
      OdDbBlockTable pTable = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject();

      /**********************************************************************/
      /* Dump the Description                                               */
      /**********************************************************************/
      Console.WriteLine();
      Console.WriteLine(pTable.isA().name());

      /**********************************************************************/
      /* Get a SmartPointer to a new SymbolTableIterator                    */
      /**********************************************************************/
      OdDbSymbolTableIterator pBlkIter = pTable.newIterator();

      /**********************************************************************/
      /* Step through the BlockTable                                        */
      /**********************************************************************/
      for (pBlkIter.start(); !pBlkIter.done(); pBlkIter.step())
      {
        /********************************************************************/
        /* Open the BlockTableRecord for Reading                            */
        /********************************************************************/
        OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)pBlkIter.getRecordId().safeOpenObject();

        /********************************************************************/
        /* Dump the BlockTableRecord                                        */
        /********************************************************************/
        Console.WriteLine();
        Console.WriteLine((pBlock.isA().name()));
        Console.WriteLine("Name = {0}", (pBlock.getName()));
        Console.WriteLine("Anonymous = {0}", (pBlock.isAnonymous()));
        Console.WriteLine("Comments = {0}", (pBlock.comments()));
        Console.WriteLine("Origin = {0}", (pBlock.origin()));
        Console.WriteLine("Block Insert Units = {0}", (pBlock.blockInsertUnits()));
        Console.WriteLine("Block Scaling = {0}", (pBlock.blockScaling()));
        Console.WriteLine("Explodable = {0}", (pBlock.explodable()));

        OdGeExtents3d extents = new OdGeExtents3d();
        if (OdResult.eOk == pBlock.getGeomExtents(extents))
        {
          Console.WriteLine("Min Extents = {0}", (extents.minPoint()));
          Console.WriteLine("Max Extents = {0}", (extents.maxPoint()));
        }
        Console.WriteLine("Layout = {0}", (pBlock.isLayout()));
        Console.WriteLine("Has Attribute Definitions = {0}", (pBlock.hasAttributeDefinitions()));
        Console.WriteLine("Xref Status = {0}", (pBlock.xrefStatus()));
        if (pBlock.xrefStatus() != XrefStatus.kXrfNotAnXref)
        {
          Console.WriteLine("Xref Path = {0}", (pBlock.pathName()));
          Console.WriteLine("From Xref Attach = {0}", (pBlock.isFromExternalReference()));
          Console.WriteLine("From Xref Overlay = {0}", (pBlock.isFromOverlayReference()));
          Console.WriteLine("Xref Unloaded = {0}", (pBlock.isUnloaded()));
        }
        /********************************************************************/
        /* Get a SmartPointer to a new ObjectIterator                       */
        /********************************************************************/
        OdDbObjectIterator pEntIter = pBlock.newIterator();

        /********************************************************************/
        /* Step through the BlockTableRecord                                */
        /********************************************************************/
        for (; !pEntIter.done(); pEntIter.step())
        {
          /********************************************************************/
          /* Dump the Entity                                                  */
          /********************************************************************/
          dumpEntity(pEntIter.objectId());
        }
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump Xdata                                                           */
    /************************************************************************/
    void dumpXdata(OdResBuf resbuf)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      if (resbuf == null)
        return;

      Console.WriteLine("Xdata:");

      /**********************************************************************/
      /* Step through the ResBuf chain                                      */
      /**********************************************************************/
      for (; resbuf != null; resbuf = resbuf.next())
      {
        int code = resbuf.restype();

        /********************************************************************/
        /* Convert resbuf.ResVal to a string                               */
        /********************************************************************/
        String rightString = "???";
        switch (OdDxfCode._getType(code))
        {
          case OdDxfCode.Type.Name:
          case OdDxfCode.Type.String:
          case OdDxfCode.Type.LayerName:
            rightString = (resbuf.getString());
            break;

          case OdDxfCode.Type.Bool:
            rightString = resbuf.getBool().ToString();
            break;

          case OdDxfCode.Type.Integer8:
            rightString = resbuf.getInt8().ToString();
            break;

          case OdDxfCode.Type.Integer16:
            rightString = (resbuf.getInt16().ToString());
            break;

          case OdDxfCode.Type.Integer32:
            rightString = resbuf.getInt32().ToString();
            break;

          case OdDxfCode.Type.Double:
            rightString = (resbuf.getDouble().ToString());
            break;

          case OdDxfCode.Type.Angle:
            rightString = Program.toDegreeString(resbuf.getDouble());
            break;

          case OdDxfCode.Type.Point:
            {
              rightString = resbuf.getPoint3d().ToString();
            }
            break;

          case OdDxfCode.Type.BinaryChunk:
            rightString = "<Binary Data>";
            break;

          case OdDxfCode.Type.ObjectId:
          case OdDxfCode.Type.SoftPointerId:
          case OdDxfCode.Type.HardPointerId:
          case OdDxfCode.Type.SoftOwnershipId:
          case OdDxfCode.Type.HardOwnershipId:
          case OdDxfCode.Type.Handle:
            rightString = resbuf.getHandle().ToString();
            break;

          case OdDxfCode.Type.Unknown:
          default:
            rightString = "Unknown";
            break;
        }
        Console.WriteLine(code.ToString() + " " + rightString);
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the Xref the full path to the Osnap entity                      */
    /************************************************************************/
    void dumpXrefFullSubentPath(OdDbXrefFullSubentPath subEntPath)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      Console.WriteLine("Subentity Index = {0}", ((int)subEntPath.subentId().index()));
      Console.WriteLine("Subentity Type = {0}", (subEntPath.subentId().type()));
      for (int j = 0; j < subEntPath.objectIds().Count; j++)
      {
        OdDbEntity pEnt = subEntPath.objectIds()[j].openObject() as OdDbEntity;
        if (pEnt != null)
        {
          Console.WriteLine("<{0}> {1}", pEnt.isA(), pEnt.getDbHandle());
        }
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump Object Snap Point Reference for an Associative Dimension        */
    /************************************************************************/
    void dumpOsnapPointRef(OdDbOsnapPointRef pRef, int index)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      Console.WriteLine("<{0}> {1}", (pRef.isA()), (index));
      Console.WriteLine("Osnap Mode = {0}", (pRef.osnapType()));
      Console.WriteLine("Near Osnap = {0}", (pRef.nearPointParam()));
      Console.WriteLine("Osnap Point = {0}", (pRef.point()));

      Console.WriteLine("Main Entity");
      dumpXrefFullSubentPath(pRef.mainEntity());

      Console.WriteLine("Intersect Entity");
      dumpXrefFullSubentPath(pRef.intersectEntity());

      if (pRef.lastPointRef() != null)
      {
        Console.WriteLine("Last Point Referenced");
        dumpOsnapPointRef(pRef.lastPointRef(), index + 1);
      }
      else
      {
        Console.WriteLine("Last Point Referenced = {0}", "Null");
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump an Associative dimension                                        */
    /************************************************************************/
    void dumpDimAssoc(OdDbObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      OdDbDimAssoc pDimAssoc = (OdDbDimAssoc)pObject;
      Console.WriteLine("Associative = {0}",
           ((OdDbDimAssoc.AssocFlags)pDimAssoc.assocFlag()));
      Console.WriteLine("TransSpatial = {0}", (pDimAssoc.isTransSpatial()));
      Console.WriteLine("Rotated Type = {0}", (pDimAssoc.rotatedDimType()));

      for (int i = 0; i < OdDbDimAssoc.kMaxPointRefs; i++)
      {
        OdDbOsnapPointRef pRef = pDimAssoc.pointRef(i);
        if (pRef != null)
        {
          dumpOsnapPointRef(pRef, i);
        }
        else
        {
          break;
        }
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /* Dump the object.                                                      */
    /*                                                                      */
    /* Dictionary objects are recursively dumped.                           */
    /* XRecord objects are dumped.                                          */
    /* DimAssoc objects are dumped.                                         */
    /************************************************************************/
    void dumpObject(OdDbObjectId id, String itemName)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();

      /**********************************************************************/
      /* Get a SmartPointer to the object                                   */
      /**********************************************************************/
      OdDbObject pObject = id.safeOpenObject();

      /**********************************************************************/
      /* Dump the item name and class name                                  */
      /**********************************************************************/
      if (pObject.isKindOf(OdDbDictionary.desc()))
      {
        Console.WriteLine();
      }
      Console.WriteLine("{0} {1}", itemName, pObject.isA().name());

      /**********************************************************************/
      /* Dispatch                                                           */
      /**********************************************************************/
      if (pObject.isKindOf(OdDbDictionary.desc()))
      {
        /********************************************************************/
        /* Dump the dictionary                                               */
        /********************************************************************/
        OdDbDictionary pDic = (OdDbDictionary)pObject;

        /********************************************************************/
        /* Get a SmartPointer to a new DictionaryIterator                   */
        /********************************************************************/
        OdDbDictionaryIterator pIter = pDic.newIterator();

        /********************************************************************/
        /* Step through the Dictionary                                      */
        /********************************************************************/
        for (; !pIter.done(); pIter.next())
        {
          /******************************************************************/
          /* Dump the Dictionary object                                     */
          /******************************************************************/
          // Dump each item in the dictionary.
          dumpObject(pIter.objectId(), pIter.name());
        }
      }
      else if (pObject.isKindOf(OdDbXrecord.desc()))
      {
        /********************************************************************/
        /* Dump an Xrecord                                                  */
        /********************************************************************/
        OdDbXrecord pXRec = (OdDbXrecord)pObject;
        dumpXdata(pXRec.rbChain());
      }
      else if (pObject.isKindOf(OdDbDimAssoc.desc()))
      {
        /********************************************************************/
        /* Dump an Associative dimension                                    */
        /********************************************************************/
        dumpDimAssoc(pObject);
      }
      else if (pObject.isKindOf(OdDbProxyObject.desc()))
      {
        OdDbProxyExt proxyEntExt = OdDbProxyExt.cast(pObject);

        Console.WriteLine("Proxy OriginalClassName = {0}", (proxyEntExt.originalClassName(pObject)));
        Console.WriteLine("Proxy ApplicationDescription = {0}", (proxyEntExt.applicationDescription(pObject)));
        Console.WriteLine("Proxy OriginalDxfName = {0}", proxyEntExt.originalDxfName(pObject));
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    /************************************************************************/
    /**** Auxiliary method for bug fix tests                            *****/
    /************************************************************************/
    void dumpAuxiliary(OdDbDatabase pDb)
    {
        //////////// test for bug 20654 ////////////
        String strBuf = "ISOLATED_OBJECTS_IDBUFFER";

        bool bCreate = true;
        OpenMode mode = bCreate ? OpenMode.kForWrite : OpenMode.kForRead;
        if (bCreate)
        {
            pDb.createExtensionDictionary();
        }

        OdDbIdBuffer pIdBuf = OdDbIdBuffer.createObject();

        OdDbDictionary pExDict = OdDbDictionary.cast(pDb.extensionDictionary().openObject(mode));
        if (null != pExDict)
        {
            pIdBuf = OdDbIdBuffer.cast(pExDict.getAt(strBuf, OpenMode.kForWrite));
            if ((null == pIdBuf) && bCreate)
            {
                pIdBuf = OdDbIdBuffer.createObject();
                pExDict.setAt(strBuf, pIdBuf);
            }
        }
        int numIds = pIdBuf.numIds();
        Console.WriteLine("OdDbIdBuffer numIds: {0} ", numIds);
        ////////////////////////////////////////////
    }
    /************************************************************************/
    /* Dump the database                                                    */
    /************************************************************************/
    public void dump(OdDbDatabase pDb)
    {
      MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      dumpHeader(pDb);
      dumpLayouts(pDb);
      dumpLayers(pDb);
      dumpLinetypes(pDb);
      dumpTextStyles(pDb);
      dumpDimStyles(pDb);
      dumpRegApps(pDb);
      dumpViewports(pDb);
      dumpViews(pDb);
      dumpMLineStyles(pDb);
      dumpUCSTable(pDb);
      dumpObject(pDb.getNamedObjectsDictionaryId(), "Named Objects Dictionary");
      dumpBlocks(pDb);
      // auxiliary dump method for bug tests
      dumpAuxiliary(pDb);

      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
  }
}
