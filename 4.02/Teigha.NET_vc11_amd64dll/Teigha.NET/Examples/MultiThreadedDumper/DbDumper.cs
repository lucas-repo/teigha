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
using System.Threading;
using System.IO;

namespace MultiThreadedDumper
{
    class DbDumper
    {
        private StreamWriter dumpFile = null;
        public DbDumper(StreamWriter value)
        {
            dumpFile = value;
        }
        /************************************************************************/
        /* Dump the Header Variables                                            */
        /************************************************************************/
        void dumpHeader(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            dumpFile.WriteLine();
            dumpFile.WriteLine("Filename: = {0}", shortenPath(pDb.getFilename()));
            dumpFile.WriteLine("File DWG Version: = {0}", (pDb.originalFileVersion()));

            dumpFile.WriteLine();
            dumpFile.WriteLine("Header Variables:");

            dumpFile.WriteLine();
            OdDbDate Date = pDb.getTDCREATE();
            dumpFile.WriteLine("TDCREATE: = {0}/{1}/{2} {3}:{4}:{5}", Date.year(), Date.month(), Date.day(), Date.hour(), Date.minute(), Date.second());
            Date = pDb.getTDUPDATE();
            dumpFile.WriteLine("TDUPDATE: = {0}/{1}/{2} {3}:{4}:{5}", Date.year(), Date.month(), Date.day(), Date.hour(), Date.minute(), Date.second());

            dumpFile.WriteLine();
            dumpFile.WriteLine("ANGBASE = {0}", (pDb.getANGBASE()));
            dumpFile.WriteLine("ANGDIR = {0}", (pDb.getANGDIR()));
            dumpFile.WriteLine("ATTMODE = {0}", (pDb.getATTMODE()));
            dumpFile.WriteLine("AUNITS = {0}", (pDb.getAUNITS()));
            dumpFile.WriteLine("AUPREC = {0}", (pDb.getAUPREC()));
            dumpFile.WriteLine("CECOLOR = {0}", (pDb.getCECOLOR()));
            dumpFile.WriteLine("CELTSCALE = {0}", (pDb.getCELTSCALE()));
            dumpFile.WriteLine("CHAMFERA = {0}", (pDb.getCHAMFERA()));
            dumpFile.WriteLine("CHAMFERB = {0}", (pDb.getCHAMFERB()));
            dumpFile.WriteLine("CHAMFERC = {0}", (pDb.getCHAMFERC()));
            dumpFile.WriteLine("CHAMFERD = {0}", (pDb.getCHAMFERD()));
            dumpFile.WriteLine("CMLJUST = {0}", (pDb.getCMLJUST()));
            dumpFile.WriteLine("CMLSCALE = {0}", (pDb.getCMLSCALE()));
            dumpFile.WriteLine("DIMADEC = {0}", (pDb.dimadec()));
            dumpFile.WriteLine("DIMALT = {0}", (pDb.dimalt()));
            dumpFile.WriteLine("DIMALTD = {0}", (pDb.dimaltd()));
            dumpFile.WriteLine("DIMALTF = {0}", (pDb.dimaltf()));
            dumpFile.WriteLine("DIMALTRND = {0}", (pDb.dimaltrnd()));
            dumpFile.WriteLine("DIMALTTD = {0}", (pDb.dimalttd()));
            dumpFile.WriteLine("DIMALTTZ = {0}", (pDb.dimalttz()));
            dumpFile.WriteLine("DIMALTU = {0}", (pDb.dimaltu()));
            dumpFile.WriteLine("DIMALTZ = {0}", (pDb.dimaltz()));
            dumpFile.WriteLine("DIMAPOST = {0}", (pDb.dimapost()));
            dumpFile.WriteLine("DIMASZ = {0}", (pDb.dimasz()));
            dumpFile.WriteLine("DIMATFIT = {0}", (pDb.dimatfit()));
            dumpFile.WriteLine("DIMAUNIT = {0}", (pDb.dimaunit()));
            dumpFile.WriteLine("DIMAZIN = {0}", (pDb.dimazin()));
            dumpFile.WriteLine("DIMBLK = {0}", toString(pDb.dimblk()));
            dumpFile.WriteLine("DIMBLK1 = {0}", toString(pDb.dimblk1()));
            dumpFile.WriteLine("DIMBLK2 = {0}", toString(pDb.dimblk2()));
            dumpFile.WriteLine("DIMCEN = {0}", (pDb.dimcen()));
            dumpFile.WriteLine("DIMCLRD = {0}", (pDb.dimclrd()));
            dumpFile.WriteLine("DIMCLRE = {0}", (pDb.dimclre()));
            dumpFile.WriteLine("DIMCLRT = {0}", (pDb.dimclrt()));
            dumpFile.WriteLine("DIMDEC = {0}", (pDb.dimdec()));
            dumpFile.WriteLine("DIMDLE = {0}", (pDb.dimdle()));
            dumpFile.WriteLine("DIMDLI = {0}", (pDb.dimdli()));
            dumpFile.WriteLine("DIMDSEP = {0}", (pDb.dimdsep()));
            dumpFile.WriteLine("DIMEXE = {0}", (pDb.dimexe()));
            dumpFile.WriteLine("DIMEXO = {0}", (pDb.dimexo()));
            dumpFile.WriteLine("DIMFRAC = {0}", (pDb.dimfrac()));
            dumpFile.WriteLine("DIMGAP = {0}", (pDb.dimgap()));
            dumpFile.WriteLine("DIMJUST = {0}", (pDb.dimjust()));
            dumpFile.WriteLine("DIMLDRBLK = {0}", toString(pDb.dimldrblk()));
            dumpFile.WriteLine("DIMLFAC = {0}", (pDb.dimlfac()));
            dumpFile.WriteLine("DIMLIM = {0}", (pDb.dimlim()));
            dumpFile.WriteLine("DIMLUNIT = {0}", (pDb.dimlunit()));
            dumpFile.WriteLine("DIMLWD = {0}", (pDb.dimlwd()));
            dumpFile.WriteLine("DIMLWE = {0}", (pDb.dimlwe()));
            dumpFile.WriteLine("DIMPOST = {0}", (pDb.dimpost()));
            dumpFile.WriteLine("DIMRND = {0}", (pDb.dimrnd()));
            dumpFile.WriteLine("DIMSAH = {0}", (pDb.dimsah()));
            dumpFile.WriteLine("DIMSCALE = {0}", (pDb.dimscale()));
            dumpFile.WriteLine("DIMSD1 = {0}", (pDb.dimsd1()));
            dumpFile.WriteLine("DIMSD2 = {0}", (pDb.dimsd2()));
            dumpFile.WriteLine("DIMSE1 = {0}", (pDb.dimse1()));
            dumpFile.WriteLine("DIMSE2 = {0}", (pDb.dimse2()));
            dumpFile.WriteLine("DIMSOXD = {0}", (pDb.dimsoxd()));
            dumpFile.WriteLine("DIMTAD = {0}", (pDb.dimtad()));
            dumpFile.WriteLine("DIMTDEC = {0}", (pDb.dimtdec()));
            dumpFile.WriteLine("DIMTFAC = {0}", (pDb.dimtfac()));
            dumpFile.WriteLine("DIMTIH = {0}", (pDb.dimtih()));
            dumpFile.WriteLine("DIMTIX = {0}", (pDb.dimtix()));
            dumpFile.WriteLine("DIMTM = {0}", (pDb.dimtm()));
            dumpFile.WriteLine("DIMTOFL = {0}", (pDb.dimtofl()));
            dumpFile.WriteLine("DIMTOH = {0}", (pDb.dimtoh()));
            dumpFile.WriteLine("DIMTOL = {0}", (pDb.dimtol()));
            dumpFile.WriteLine("DIMTOLJ = {0}", (pDb.dimtolj()));
            dumpFile.WriteLine("DIMTP = {0}", (pDb.dimtp()));
            dumpFile.WriteLine("DIMTSZ = {0}", (pDb.dimtsz()));
            dumpFile.WriteLine("DIMTVP = {0}", (pDb.dimtvp()));
            dumpFile.WriteLine("DIMTXSTY = {0}", toString(pDb.dimtxsty()));
            dumpFile.WriteLine("DIMTXT = {0}", (pDb.dimtxt()));
            dumpFile.WriteLine("DIMTZIN = {0}", (pDb.dimtzin()));
            dumpFile.WriteLine("DIMUPT = {0}", (pDb.dimupt()));
            dumpFile.WriteLine("DIMZIN = {0}", (pDb.dimzin()));
            dumpFile.WriteLine("DISPSILH = {0}", (pDb.getDISPSILH()));
            dumpFile.WriteLine("DRAWORDERCTL = {0}", (pDb.getDRAWORDERCTL()));
            dumpFile.WriteLine("ELEVATION = {0}", (pDb.getELEVATION()));
            dumpFile.WriteLine("EXTMAX = {0}", (pDb.getEXTMAX()));
            dumpFile.WriteLine("EXTMIN = {0}", (pDb.getEXTMIN()));
            dumpFile.WriteLine("FACETRES = {0}", (pDb.getFACETRES()));
            dumpFile.WriteLine("FILLETRAD = {0}", (pDb.getFILLETRAD()));
            dumpFile.WriteLine("FILLMODE = {0}", (pDb.getFILLMODE()));
            dumpFile.WriteLine("INSBASE = {0}", (pDb.getINSBASE()));
            dumpFile.WriteLine("ISOLINES = {0}", (pDb.getISOLINES()));
            dumpFile.WriteLine("LIMCHECK = {0}", (pDb.getLIMCHECK()));
            dumpFile.WriteLine("LIMMAX = {0}", (pDb.getLIMMAX()));
            dumpFile.WriteLine("LIMMIN = {0}", (pDb.getLIMMIN()));
            dumpFile.WriteLine("LTSCALE = {0}", (pDb.getLTSCALE()));
            dumpFile.WriteLine("LUNITS = {0}", (pDb.getLUNITS()));
            dumpFile.WriteLine("LUPREC = {0}", (pDb.getLUPREC()));
            dumpFile.WriteLine("MAXACTVP = {0}", (pDb.getMAXACTVP()));
            dumpFile.WriteLine("MIRRTEXT = {0}", (pDb.getMIRRTEXT()));
            dumpFile.WriteLine("ORTHOMODE = {0}", (pDb.getORTHOMODE()));
            dumpFile.WriteLine("PDMODE = {0}", (pDb.getPDMODE()));
            dumpFile.WriteLine("PDSIZE = {0}", (pDb.getPDSIZE()));
            dumpFile.WriteLine("PELEVATION = {0}", (pDb.getPELEVATION()));
            dumpFile.WriteLine("PELLIPSE = {0}", (pDb.getPELLIPSE()));
            dumpFile.WriteLine("PEXTMAX = {0}", (pDb.getPEXTMAX()));
            dumpFile.WriteLine("PEXTMIN = {0}", (pDb.getPEXTMIN()));
            dumpFile.WriteLine("PINSBASE = {0}", (pDb.getPINSBASE()));
            dumpFile.WriteLine("PLIMCHECK = {0}", (pDb.getPLIMCHECK()));
            dumpFile.WriteLine("PLIMMAX = {0}", (pDb.getPLIMMAX()));
            dumpFile.WriteLine("PLIMMIN = {0}", (pDb.getPLIMMIN()));
            dumpFile.WriteLine("PLINEGEN = {0}", (pDb.getPLINEGEN()));
            dumpFile.WriteLine("PLINEWID = {0}", (pDb.getPLINEWID()));
            dumpFile.WriteLine("PROXYGRAPHICS = {0}", (pDb.getPROXYGRAPHICS()));
            dumpFile.WriteLine("PSLTSCALE = {0}", (pDb.getPSLTSCALE()));
            dumpFile.WriteLine("PUCSNAME = {0}", toString(pDb.getPUCSNAME()));
            dumpFile.WriteLine("PUCSORG = {0}", (pDb.getPUCSORG()));
            dumpFile.WriteLine("PUCSXDIR = {0}", (pDb.getPUCSXDIR()));
            dumpFile.WriteLine("PUCSYDIR = {0}", (pDb.getPUCSYDIR()));
            dumpFile.WriteLine("QTEXTMODE = {0}", (pDb.getQTEXTMODE()));
            dumpFile.WriteLine("REGENMODE = {0}", (pDb.getREGENMODE()));
            dumpFile.WriteLine("SHADEDGE = {0}", (pDb.getSHADEDGE()));
            dumpFile.WriteLine("SHADEDIF = {0}", (pDb.getSHADEDIF()));
            dumpFile.WriteLine("SKETCHINC = {0}", (pDb.getSKETCHINC()));
            dumpFile.WriteLine("SKPOLY = {0}", (pDb.getSKPOLY()));
            dumpFile.WriteLine("SPLFRAME = {0}", (pDb.getSPLFRAME()));
            dumpFile.WriteLine("SPLINESEGS = {0}", (pDb.getSPLINESEGS()));
            dumpFile.WriteLine("SPLINETYPE = {0}", (pDb.getSPLINETYPE()));
            dumpFile.WriteLine("SURFTAB1 = {0}", (pDb.getSURFTAB1()));
            dumpFile.WriteLine("SURFTAB2 = {0}", (pDb.getSURFTAB2()));
            dumpFile.WriteLine("SURFTYPE = {0}", (pDb.getSURFTYPE()));
            dumpFile.WriteLine("SURFU = {0}", (pDb.getSURFU()));
            dumpFile.WriteLine("SURFV = {0}", (pDb.getSURFV()));
            dumpFile.WriteLine("TEXTQLTY = {0}", (pDb.getTEXTQLTY()));
            dumpFile.WriteLine("TEXTSIZE = {0}", (pDb.getTEXTSIZE()));
            dumpFile.WriteLine("THICKNESS = {0}", (pDb.getTHICKNESS()));
            dumpFile.WriteLine("TILEMODE = {0}", (pDb.getTILEMODE()));
            dumpFile.WriteLine("TRACEWID = {0}", (pDb.getTRACEWID()));
            dumpFile.WriteLine("TREEDEPTH = {0}", (pDb.getTREEDEPTH()));
            dumpFile.WriteLine("UCSNAME = {0}", toString(pDb.getUCSNAME()));
            dumpFile.WriteLine("UCSORG = {0}", (pDb.getUCSORG()));
            dumpFile.WriteLine("UCSXDIR = {0}", (pDb.getUCSXDIR()));
            dumpFile.WriteLine("UCSYDIR = {0}", (pDb.getUCSYDIR()));
            dumpFile.WriteLine("UNITMODE = {0}", (pDb.getUNITMODE()));
            dumpFile.WriteLine("USERI1 = {0}", (pDb.getUSERI1()));
            dumpFile.WriteLine("USERI2 = {0}", (pDb.getUSERI2()));
            dumpFile.WriteLine("USERI3 = {0}", (pDb.getUSERI3()));
            dumpFile.WriteLine("USERI4 = {0}", (pDb.getUSERI4()));
            dumpFile.WriteLine("USERI5 = {0}", (pDb.getUSERI5()));
            dumpFile.WriteLine("USERR1 = {0}", (pDb.getUSERR1()));
            dumpFile.WriteLine("USERR2 = {0}", (pDb.getUSERR2()));
            dumpFile.WriteLine("USERR3 = {0}", (pDb.getUSERR3()));
            dumpFile.WriteLine("USERR4 = {0}", (pDb.getUSERR4()));
            dumpFile.WriteLine("USERR5 = {0}", (pDb.getUSERR5()));
            dumpFile.WriteLine("USRTIMER = {0}", (pDb.getUSRTIMER()));
            dumpFile.WriteLine("VISRETAIN = {0}", (pDb.getVISRETAIN()));
            dumpFile.WriteLine("WORLDVIEW = {0}", (pDb.getWORLDVIEW()));
            MemoryManager.GetMemoryManager().StopTransaction(mTr);

        }

        /************************************************************************/
        /* Dump a Symbol Table Record                                           */
        /************************************************************************/
        void dumpSymbolTableRecord(OdDbSymbolTableRecord pRecord)
        {
            dumpFile.WriteLine("Xref dependent = {0}", (pRecord.isDependent()));
            if (pRecord.isDependent())
            {
                dumpFile.WriteLine("Resolved = {0}", (pRecord.isResolved()));
            }
        }
        /************************************************************************/
        /* Dump the Layouts                                                     */
        /************************************************************************/
        void dumpLayouts(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pEntry.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pEntry.getLayoutName()));
                OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)pEntry.getBlockTableRecordId().safeOpenObject();
                if (pBlock != null)
                {
                    dumpFile.WriteLine((pBlock.isA().name()));
                    dumpFile.WriteLine("Name = {0}", (pBlock.getName()));
                    /*OdGeExtents3d extents = new OdGeExtents3d();
                    if (OdResult.eOk == pBlock.getGeomExtents(extents))
                    {
                        dumpFile.WriteLine("Min Extents = {0}", (extents.minPoint()));
                        dumpFile.WriteLine("Max Extents = {0}", (extents.maxPoint()));
                    }
                    else
                    {
                        dumpFile.WriteLine("   Incorrect extents");
                    }*/
                }
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }
        /************************************************************************/
        /* Dump the LayerTable                                                  */
        /************************************************************************/
        void dumpLayers(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the LayerTable                               */
            /**********************************************************************/
            OdDbLayerTable pTable = (OdDbLayerTable)pDb.getLayerTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine("<{0}>", pRecord.isA().name());
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("In Use = {0}", (pRecord.isInUse()));
                dumpFile.WriteLine("On = {0}", (!pRecord.isOff()));
                dumpFile.WriteLine("Frozen = {0}", (pRecord.isFrozen()));
                dumpFile.WriteLine("Locked = {0}", (pRecord.isLocked()));
                dumpFile.WriteLine("Color = {0}", (pRecord.color()));
                dumpFile.WriteLine("Linetype = {0}", toString(pRecord.linetypeObjectId()));
                dumpFile.WriteLine("Lineweight = {0}", (pRecord.lineWeight()));
                dumpFile.WriteLine("Plotstyle = {0}", (pRecord.plotStyleName()));
                dumpFile.WriteLine("Plottable = {0}", (pRecord.isPlottable()));
                dumpFile.WriteLine("New VP Freeze = {0}", (pRecord.VPDFLT()));
                dumpSymbolTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the LinetypeTable                                               */
        /************************************************************************/
        void dumpLinetypes(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the LinetypeTable                            */
            /**********************************************************************/
            OdDbLinetypeTable pTable = (OdDbLinetypeTable)pDb.getLinetypeTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine(pRecord.isA().name());
                /********************************************************************/
                /* Dump the first line of record as in ACAD.LIN                     */
                /********************************************************************/
                String buffer = "*" + pRecord.getName();
                if (pRecord.comments() != "")
                {
                    buffer += "," + pRecord.comments();
                }
                dumpFile.WriteLine(buffer);

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
                                buffer += ",R" + toDegreeString(pRecord.shapeRotationAt(i));
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
                    dumpFile.WriteLine(buffer);
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
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the TextStyleTable                            */
            /**********************************************************************/
            OdDbTextStyleTable pTable = (OdDbTextStyleTable)pDb.getTextStyleTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("Shape File = {0}", (pRecord.isShapeFile()));
                dumpFile.WriteLine("Text Height = {0}", (pRecord.textSize()));
                dumpFile.WriteLine("Width Factor = {0}", (pRecord.xScale()));
                dumpFile.WriteLine("Obliquing Angle = {0}", toDegreeString(pRecord.obliquingAngle()));
                dumpFile.WriteLine("Backwards = {0}", (pRecord.isBackwards()));
                dumpFile.WriteLine("Vertical = {0}", (pRecord.isVertical()));
                dumpFile.WriteLine("Upside Down = {0}", (pRecord.isUpsideDown()));
                dumpFile.WriteLine("Filename = {0}", shortenPath(pRecord.fileName()));
                dumpFile.WriteLine("BigFont Filename = {0}", shortenPath(pRecord.bigFontFileName()));

                String typeface = "";
                bool bold;
                bool italic;
                int charset;
                int pitchAndFamily;
                pRecord.font(ref typeface, out bold, out italic, out charset, out pitchAndFamily);
                dumpFile.WriteLine("Typeface = {0}", (typeface));
                dumpFile.WriteLine("Character Set = {0}", (charset));
                dumpFile.WriteLine("Bold = {0}", (bold));
                dumpFile.WriteLine("Italic = {0}", (italic));
                dumpFile.WriteLine("Font Pitch & Family = {0:X}", pitchAndFamily);
                dumpSymbolTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }
        /************************************************************************/
        /* Dump the DimStyleTable                                               */
        /************************************************************************/
        void dumpDimStyles(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the DimStyleTable                            */
            /**********************************************************************/
            OdDbDimStyleTable pTable = (OdDbDimStyleTable)pDb.getDimStyleTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("Arc Symbol = {0}", toArcSymbolTypeString(pRecord.getArcSymbolType()));

                OdCmColor bgrndTxtColor = new OdCmColor();
                UInt16 bgrndTxtFlags = pRecord.getBgrndTxtColor(bgrndTxtColor);
                dumpFile.WriteLine("Background Text Color = {0}", (bgrndTxtColor));
                dumpFile.WriteLine("BackgroundText Flags = {0}", (bgrndTxtFlags));
                dumpFile.WriteLine("Extension Line 1 Linetype = {0}", toString(pRecord.getDimExt1Linetype()));
                dumpFile.WriteLine("Extension Line 2 Linetype = {0}", toString(pRecord.getDimExt2Linetype()));
                dumpFile.WriteLine("Dimension Line Linetype = {0}", toString(pRecord.getDimExt2Linetype()));
                dumpFile.WriteLine("Extension Line Fixed Len = {0}", (pRecord.getExtLineFixLen()));
                dumpFile.WriteLine("Extension Line Fixed Len Enable = {0}", (pRecord.getExtLineFixLenEnable()));
                dumpFile.WriteLine("Jog Angle = {0}", toDegreeString(pRecord.getJogAngle()));
                dumpFile.WriteLine("Modified For Recompute = {0}", (pRecord.isModifiedForRecompute()));
                dumpFile.WriteLine("DIMADEC = {0}", (pRecord.dimadec()));
                dumpFile.WriteLine("DIMALT = {0}", (pRecord.dimalt()));
                dumpFile.WriteLine("DIMALTD = {0}", (pRecord.dimaltd()));
                dumpFile.WriteLine("DIMALTF = {0}", (pRecord.dimaltf()));
                dumpFile.WriteLine("DIMALTRND = {0}", (pRecord.dimaltrnd()));
                dumpFile.WriteLine("DIMALTTD = {0}", (pRecord.dimalttd()));
                dumpFile.WriteLine("DIMALTTZ = {0}", (pRecord.dimalttz()));
                dumpFile.WriteLine("DIMALTU = {0}", (pRecord.dimaltu()));
                dumpFile.WriteLine("DIMALTZ = {0}", (pRecord.dimaltz()));
                dumpFile.WriteLine("DIMAPOST = {0}", (pRecord.dimapost()));
                dumpFile.WriteLine("DIMASZ = {0}", (pRecord.dimasz()));
                dumpFile.WriteLine("DIMATFIT = {0}", (pRecord.dimatfit()));
                dumpFile.WriteLine("DIMAUNIT = {0}", (pRecord.dimaunit()));
                dumpFile.WriteLine("DIMAZIN = {0}", (pRecord.dimazin()));
                dumpFile.WriteLine("DIMBLK = {0}", toString(pRecord.dimblk()));
                dumpFile.WriteLine("DIMBLK1 = {0}", toString(pRecord.dimblk1()));
                dumpFile.WriteLine("DIMBLK2 = {0}", toString(pRecord.dimblk2()));
                dumpFile.WriteLine("DIMCEN = {0}", (pRecord.dimcen()));
                dumpFile.WriteLine("DIMCLRD = {0}", (pRecord.dimclrd()));
                dumpFile.WriteLine("DIMCLRE = {0}", (pRecord.dimclre()));
                dumpFile.WriteLine("DIMCLRT = {0}", (pRecord.dimclrt()));
                dumpFile.WriteLine("DIMDEC = {0}", (pRecord.dimdec()));
                dumpFile.WriteLine("DIMDLE = {0}", (pRecord.dimdle()));
                dumpFile.WriteLine("DIMDLI = {0}", (pRecord.dimdli()));
                dumpFile.WriteLine("DIMDSEP = {0}", (pRecord.dimdsep()));
                dumpFile.WriteLine("DIMEXE = {0}", (pRecord.dimexe()));
                dumpFile.WriteLine("DIMEXO = {0}", (pRecord.dimexo()));
                dumpFile.WriteLine("DIMFRAC = {0}", (pRecord.dimfrac()));
                dumpFile.WriteLine("DIMGAP = {0}", (pRecord.dimgap()));
                dumpFile.WriteLine("DIMJUST = {0}", (pRecord.dimjust()));
                dumpFile.WriteLine("DIMLDRBLK = {0}", toString(pRecord.dimldrblk()));
                dumpFile.WriteLine("DIMLFAC = {0}", (pRecord.dimlfac()));
                dumpFile.WriteLine("DIMLIM = {0}", (pRecord.dimlim()));
                dumpFile.WriteLine("DIMLUNIT = {0}", (pRecord.dimlunit()));
                dumpFile.WriteLine("DIMLWD = {0}", (pRecord.dimlwd()));
                dumpFile.WriteLine("DIMLWE = {0}", (pRecord.dimlwe()));
                dumpFile.WriteLine("DIMPOST = {0}", (pRecord.dimpost()));
                dumpFile.WriteLine("DIMRND = {0}", (pRecord.dimrnd()));
                dumpFile.WriteLine("DIMSAH = {0}", (pRecord.dimsah()));
                dumpFile.WriteLine("DIMSCALE = {0}", (pRecord.dimscale()));
                dumpFile.WriteLine("DIMSD1 = {0}", (pRecord.dimsd1()));
                dumpFile.WriteLine("DIMSD2 = {0}", (pRecord.dimsd2()));
                dumpFile.WriteLine("DIMSE1 = {0}", (pRecord.dimse1()));
                dumpFile.WriteLine("DIMSE2 = {0}", (pRecord.dimse2()));
                dumpFile.WriteLine("DIMSOXD = {0}", (pRecord.dimsoxd()));
                dumpFile.WriteLine("DIMTAD = {0}", (pRecord.dimtad()));
                dumpFile.WriteLine("DIMTDEC = {0}", (pRecord.dimtdec()));
                dumpFile.WriteLine("DIMTFAC = {0}", (pRecord.dimtfac()));
                dumpFile.WriteLine("DIMTIH = {0}", (pRecord.dimtih()));
                dumpFile.WriteLine("DIMTIX = {0}", (pRecord.dimtix()));
                dumpFile.WriteLine("DIMTM = {0}", (pRecord.dimtm()));
                dumpFile.WriteLine("DIMTOFL = {0}", (pRecord.dimtofl()));
                dumpFile.WriteLine("DIMTOH = {0}", (pRecord.dimtoh()));
                dumpFile.WriteLine("DIMTOL = {0}", (pRecord.dimtol()));
                dumpFile.WriteLine("DIMTOLJ = {0}", (pRecord.dimtolj()));
                dumpFile.WriteLine("DIMTP = {0}", (pRecord.dimtp()));
                dumpFile.WriteLine("DIMTSZ = {0}", (pRecord.dimtsz()));
                dumpFile.WriteLine("DIMTVP = {0}", (pRecord.dimtvp()));
                dumpFile.WriteLine("DIMTXSTY = {0}", toString(pRecord.dimtxsty()));
                dumpFile.WriteLine("DIMTXT = {0}", pRecord.dimtxt());
                dumpFile.WriteLine("DIMTZIN = {0}", (pRecord.dimtzin()));
                dumpFile.WriteLine("DIMUPT = {0}", (pRecord.dimupt()));
                dumpFile.WriteLine("DIMZIN = {0}", (pRecord.dimzin()));
                dumpSymbolTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the RegAppTable                                              */
        /************************************************************************/
        void dumpRegApps(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the RegAppTable                            */
            /**********************************************************************/
            OdDbRegAppTable pTable = (OdDbRegAppTable)pDb.getRegAppTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the AbstractViewTableRecord                                     */
        /*************************************************************************/
        void dumpAbstractViewTableRecord(OdDbAbstractViewTableRecord pView)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /*********************************************************************/
            /* Dump the AbstractViewTableRecord                                  */
            /*********************************************************************/
            dumpFile.WriteLine("Back Clip Dist = {0}", (pView.backClipDistance()));
            dumpFile.WriteLine("Back Clip Enabled = {0}", (pView.backClipEnabled()));
            dumpFile.WriteLine("Front Clip Dist = {0}", (pView.frontClipDistance()));
            dumpFile.WriteLine("Front Clip Enabled = {0}", (pView.frontClipEnabled()));
            dumpFile.WriteLine("Front Clip at Eye = {0}", (pView.frontClipAtEye()));
            dumpFile.WriteLine("Elevation = {0}", (pView.elevation()));
            dumpFile.WriteLine("Height = {0}", (pView.height()));
            dumpFile.WriteLine("Width = {0}", (pView.width()));
            dumpFile.WriteLine("Lens Length = {0}", (pView.lensLength()));
            dumpFile.WriteLine("Render Mode = {0}", (pView.renderMode()));
            dumpFile.WriteLine("Perspective = {0}", (pView.perspectiveEnabled()));
            dumpFile.WriteLine("UCS Name = {0}", toString(pView.ucsName()));

            OrthographicView orthoUCS;
            dumpFile.WriteLine("UCS Orthographic = {0}", (pView.isUcsOrthographic(out orthoUCS)));
            dumpFile.WriteLine("Orthographic UCS = {0}", (orthoUCS));
            OdGePoint3d origin = new OdGePoint3d();
            OdGeVector3d xAxis = new OdGeVector3d();
            OdGeVector3d yAxis = new OdGeVector3d();
            pView.getUcs(origin, xAxis, yAxis);
            dumpFile.WriteLine("UCS Origin = {0}", (origin));
            dumpFile.WriteLine("UCS x-Axis = {0}", (xAxis));
            dumpFile.WriteLine("UCS y-Axis = {0}", (yAxis));
            dumpFile.WriteLine("Target = {0}", (pView.target()));
            dumpFile.WriteLine("View Direction = {0}", (pView.viewDirection()));
            dumpFile.WriteLine("Twist Angle = {0}", toDegreeString(pView.viewTwist()));
            dumpSymbolTableRecord(pView);
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the ViewportTable                                              */
        /************************************************************************/
        void dumpViewports(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the ViewportTable                            */
            /**********************************************************************/
            OdDbViewportTable pTable = (OdDbViewportTable)pDb.getViewportTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine((pTable.isA().name()));

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("Circle Sides = {0}", (pRecord.circleSides()));
                dumpFile.WriteLine("Fast Zooms Enabled = {0}", (pRecord.fastZoomsEnabled()));
                dumpFile.WriteLine("Grid Enabled = {0}", (pRecord.gridEnabled()));
                dumpFile.WriteLine("Grid Increments = {0}", (pRecord.gridIncrements()));
                dumpFile.WriteLine("Icon at Origin = {0}", (pRecord.iconAtOrigin()));
                dumpFile.WriteLine("Icon Enabled = {0}", (pRecord.iconEnabled()));
                dumpFile.WriteLine("Iso snap Enabled = {0}", (pRecord.isometricSnapEnabled()));
                dumpFile.WriteLine("Iso Snap Pair = {0}", (pRecord.snapPair()));
                dumpFile.WriteLine("UCS Saved w/Vport = {0}", (pRecord.isUcsSavedWithViewport()));
                dumpFile.WriteLine("UCS follow = {0}", (pRecord.ucsFollowMode()));
                dumpFile.WriteLine("Lower-Left Corner = {0}", (pRecord.lowerLeftCorner()));
                dumpFile.WriteLine("Upper-Right Corner = {0}", (pRecord.upperRightCorner()));
                dumpFile.WriteLine("Snap Angle = {0}", toDegreeString(pRecord.snapAngle()));
                dumpFile.WriteLine("Snap Base = {0}", (pRecord.snapBase()));
                dumpFile.WriteLine("Snap Enabled = {0}", (pRecord.snapEnabled()));
                dumpFile.WriteLine("Snap Increments = {0}", (pRecord.snapIncrements()));
                dumpAbstractViewTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the ViewTable                                                   */
        /************************************************************************/
        void dumpViews(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the ViewTable                                */
            /**********************************************************************/
            OdDbViewTable pTable = (OdDbViewTable)pDb.getViewTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine((pTable.isA().name()));

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("Category Name = {0}", (pRecord.getCategoryName()));
                dumpFile.WriteLine("Layer State = {0}", (pRecord.getLayerState()));

                String layoutName = "";
                if (!pRecord.getLayout().isNull())
                {
                    OdDbLayout pLayout = (OdDbLayout)pRecord.getLayout().safeOpenObject();
                    layoutName = pLayout.getLayoutName();
                }
                dumpFile.WriteLine("Layout Name = {0}", (layoutName));
                dumpFile.WriteLine("PaperSpace View = {0}", (pRecord.isPaperspaceView()));
                dumpFile.WriteLine("Associated UCS = {0}", (pRecord.isUcsAssociatedToView()));
                dumpFile.WriteLine("PaperSpace View = {0}", (pRecord.isViewAssociatedToViewport()));
                dumpAbstractViewTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }
        /************************************************************************/
        /* Dump the MlineStyle Dictionary                                       */
        /************************************************************************/
        void dumpMLineStyles(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            OdDbDictionary pDictionary = (OdDbDictionary)pDb.getMLStyleDictionaryId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine((pDictionary.isA().name()));

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pEntry.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pEntry.name()));
                dumpFile.WriteLine("Description = {0}", (pEntry.description()));
                dumpFile.WriteLine("Start Angle = {0}", toDegreeString(pEntry.startAngle()));
                dumpFile.WriteLine("End Angle = {0}", toDegreeString(pEntry.endAngle()));
                dumpFile.WriteLine("Start Inner Arcs = {0}", (pEntry.startInnerArcs()));
                dumpFile.WriteLine("End Inner Arcs = {0}", (pEntry.endInnerArcs()));
                dumpFile.WriteLine("Start Round Cap = {0}", (pEntry.startRoundCap()));
                dumpFile.WriteLine("End Round Cap = {0}", (pEntry.endRoundCap()));
                dumpFile.WriteLine("Start Square Cap = {0}", (pEntry.startRoundCap()));
                dumpFile.WriteLine("End Square Cap = {0}", (pEntry.endRoundCap()));
                dumpFile.WriteLine("Show Miters = {0}", (pEntry.showMiters()));
                /*********************************************************************/
                /* Dump the elements                                                 */
                /*********************************************************************/
                if (pEntry.numElements() != 0)
                {
                    dumpFile.WriteLine("Elements:");
                }
                for (int i = 0; i < pEntry.numElements(); i++)
                {
                    double offset;
                    OdCmColor color = new OdCmColor();
                    OdDbObjectId linetypeId = new OdDbObjectId();
                    pEntry.getElementAt(i, out offset, color, linetypeId);
                    dumpFile.WriteLine("Index = {0}", (i));
                    dumpFile.WriteLine("Offset = {0}", (offset));
                    dumpFile.WriteLine("Color = {0}", (color));
                    dumpFile.WriteLine("Linetype = {0}", toString(linetypeId));
                }
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }
        /************************************************************************/
        /* Dump the UCSTable                                                    */
        /************************************************************************/
        void dumpUCSTable(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the UCSTable                               */
            /**********************************************************************/
            OdDbUCSTable pTable = (OdDbUCSTable)pDb.getUCSTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pRecord.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pRecord.getName()));
                dumpFile.WriteLine("UCS Origin = {0}", (pRecord.origin()));
                dumpFile.WriteLine("UCS x-Axis = {0}", (pRecord.xAxis()));
                dumpFile.WriteLine("UCS y-Axis = {0}", (pRecord.yAxis()));
                dumpSymbolTableRecord(pRecord);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the Entity                                                      */
        /************************************************************************/
        void dumpEntity(OdDbObjectId id)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

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
                dumpFile.WriteLine("dumpEntity Circle");
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
                dumpFile.WriteLine("OdDbMPolygon fields");
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
                        dumpFile.WriteLine(string.Format("Vertex %d ", i) + vertices[i]);
                        if (hasBulges)
                        {
                            dumpFile.WriteLine(string.Format("Bulge %d ", i) + bulges[i].ToString());
                            dumpFile.WriteLine(string.Format("Bulge angle %d ", i) + (4 * Math.Atan(bulges[i])).ToString());
                        }
                    }
                }*/
                dumpFile.WriteLine("Elevation " + mpolygon.elevation().ToString());
                dumpFile.WriteLine("Normal " + mpolygon.normal().ToString());
                dumpFile.WriteLine("Pattern name " + mpolygon.patternName());

                dumpFile.WriteLine(string.Format("Pattern angle {0}", mpolygon.patternAngle()));
                dumpFile.WriteLine(string.Format("Pattern space {0}", mpolygon.patternSpace()));
                dumpFile.WriteLine(string.Format("Pattern scale {0}", mpolygon.patternScale()));
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
                    dumpFile.WriteLine(string.Format("Definition {0}", i));
                    dumpFile.WriteLine(string.Format("angle {0}", angle));
                    dumpFile.WriteLine(string.Format("baseX {0}", baseX));
                    dumpFile.WriteLine(string.Format("baseY {0}", baseY));
                    dumpFile.WriteLine(string.Format("offsetX {0}", offsetX));
                    dumpFile.WriteLine(string.Format("offsetY {0}", offsetY));
                    dumpFile.WriteLine(string.Format("dashes number {0}", dashes.Count));
                }
                double ar = 0;
                mpolygon.getArea(out ar);
                dumpFile.WriteLine(string.Format("Area {0}", ar));
                dumpFile.WriteLine("Offset vector " + mpolygon.getOffsetVector().ToString());
            }
            ////////////////////////////////////////////////

            /**********************************************************************/
            /* Dump the entity                                                    */
            /**********************************************************************/
            dumpFile.WriteLine();
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
                dumpFile.WriteLine("Problem object is: " + pEnt.isA().name());
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
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the BlockTable                               */
            /**********************************************************************/
            OdDbBlockTable pTable = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject();

            /**********************************************************************/
            /* Dump the Description                                               */
            /**********************************************************************/
            dumpFile.WriteLine();
            dumpFile.WriteLine(pTable.isA().name());

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
                dumpFile.WriteLine();
                dumpFile.WriteLine((pBlock.isA().name()));
                dumpFile.WriteLine("Name = {0}", (pBlock.getName()));
                dumpFile.WriteLine("Anonymous = {0}", (pBlock.isAnonymous()));
                dumpFile.WriteLine("Comments = {0}", (pBlock.comments()));
                dumpFile.WriteLine("Origin = {0}", (pBlock.origin()));
                dumpFile.WriteLine("Block Insert Units = {0}", (pBlock.blockInsertUnits()));
                dumpFile.WriteLine("Block Scaling = {0}", (pBlock.blockScaling()));
                dumpFile.WriteLine("Explodable = {0}", (pBlock.explodable()));

                OdGeExtents3d extents = new OdGeExtents3d();
                /*if (OdResult.eOk == pBlock.getGeomExtents(extents))
                {
                    dumpFile.WriteLine("Min Extents = {0}", (extents.minPoint()));
                    dumpFile.WriteLine("Max Extents = {0}", (extents.maxPoint()));
                }*/
                dumpFile.WriteLine("Layout = {0}", (pBlock.isLayout()));
                dumpFile.WriteLine("Has Attribute Definitions = {0}", (pBlock.hasAttributeDefinitions()));
                dumpFile.WriteLine("Xref Status = {0}", (pBlock.xrefStatus()));
                if (pBlock.xrefStatus() != XrefStatus.kXrfNotAnXref)
                {
                    dumpFile.WriteLine("Xref Path = {0}", (pBlock.pathName()));
                    dumpFile.WriteLine("From Xref Attach = {0}", (pBlock.isFromExternalReference()));
                    dumpFile.WriteLine("From Xref Overlay = {0}", (pBlock.isFromOverlayReference()));
                    dumpFile.WriteLine("Xref Unloaded = {0}", (pBlock.isUnloaded()));
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
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            if (resbuf == null)
                return;

            dumpFile.WriteLine("Xdata:");

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
                        rightString = toDegreeString(resbuf.getDouble());
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
                dumpFile.WriteLine(code.ToString() + " " + rightString);
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump the Xref the full path to the Osnap entity                      */
        /************************************************************************/
        void dumpXrefFullSubentPath(OdDbXrefFullSubentPath subEntPath)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            dumpFile.WriteLine("Subentity Index = {0}", ((int)subEntPath.subentId().index()));
            dumpFile.WriteLine("Subentity Type = {0}", (subEntPath.subentId().type()));
            for (int j = 0; j < subEntPath.objectIds().Count; j++)
            {
                OdDbEntity pEnt = subEntPath.objectIds()[j].openObject() as OdDbEntity;
                if (pEnt != null)
                {
                    dumpFile.WriteLine("<{0}> {1}", pEnt.isA(), pEnt.getDbHandle());
                }
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump Object Snap Point Reference for an Associative Dimension        */
        /************************************************************************/
        void dumpOsnapPointRef(OdDbOsnapPointRef pRef, int index)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            dumpFile.WriteLine("<{0}> {1}", (pRef.isA()), (index));
            dumpFile.WriteLine("Osnap Mode = {0}", (pRef.osnapType()));
            dumpFile.WriteLine("Near Osnap = {0}", (pRef.nearPointParam()));
            dumpFile.WriteLine("Osnap Point = {0}", (pRef.point()));

            dumpFile.WriteLine("Main Entity");
            dumpXrefFullSubentPath(pRef.mainEntity());

            dumpFile.WriteLine("Intersect Entity");
            dumpXrefFullSubentPath(pRef.intersectEntity());

            if (pRef.lastPointRef() != null)
            {
                dumpFile.WriteLine("Last Point Referenced");
                dumpOsnapPointRef(pRef.lastPointRef(), index + 1);
            }
            else
            {
                dumpFile.WriteLine("Last Point Referenced = {0}", "Null");
            }
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }

        /************************************************************************/
        /* Dump an Associative dimension                                        */
        /************************************************************************/
        void dumpDimAssoc(OdDbObject pObject)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            OdDbDimAssoc pDimAssoc = (OdDbDimAssoc)pObject;
            dumpFile.WriteLine("Associative = {0}",
                 ((OdDbDimAssoc.AssocFlags)pDimAssoc.assocFlag()));
            dumpFile.WriteLine("TransSpatial = {0}", (pDimAssoc.isTransSpatial()));
            dumpFile.WriteLine("Rotated Type = {0}", (pDimAssoc.rotatedDimType()));

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
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);

            /**********************************************************************/
            /* Get a SmartPointer to the object                                   */
            /**********************************************************************/
            OdDbObject pObject = id.safeOpenObject();

            /**********************************************************************/
            /* Dump the item name and class name                                  */
            /**********************************************************************/
            if (pObject.isKindOf(OdDbDictionary.desc()))
            {
                dumpFile.WriteLine();
            }
            dumpFile.WriteLine("{0} {1}", itemName, pObject.isA().name());

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

                dumpFile.WriteLine("Proxy OriginalClassName = {0}", (proxyEntExt.originalClassName(pObject)));
                dumpFile.WriteLine("Proxy ApplicationDescription = {0}", (proxyEntExt.applicationDescription(pObject)));
                dumpFile.WriteLine("Proxy OriginalDxfName = {0}", proxyEntExt.originalDxfName(pObject));
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
            dumpFile.WriteLine("OdDbIdBuffer numIds: {0} ", numIds);
            ////////////////////////////////////////////
        }
        /************************************************************************/
        /* Dump the database                                                    */
        /************************************************************************/
        public void dump(OdDbDatabase pDb)
        {
            MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);
            dumpHeader(pDb);
            dumpFile.Flush();
            dumpLayouts(pDb);
            dumpFile.Flush();
            dumpLayers(pDb);
            dumpFile.Flush();
            dumpLinetypes(pDb);
            dumpFile.Flush();
            dumpTextStyles(pDb);
            dumpFile.Flush();
            dumpDimStyles(pDb);
            dumpFile.Flush();
            dumpRegApps(pDb);
            dumpFile.Flush();
            dumpViewports(pDb);
            dumpFile.Flush();
            dumpViews(pDb);
            dumpFile.Flush();
            dumpMLineStyles(pDb);
            dumpFile.Flush();
            dumpUCSTable(pDb);
            dumpFile.Flush();
            dumpObject(pDb.getNamedObjectsDictionaryId(), "Named Objects Dictionary");
            dumpFile.Flush();
            dumpBlocks(pDb);
            dumpFile.Flush();
            // auxiliary dump method for bug tests
            dumpAuxiliary(pDb);
            dumpFile.Flush();

            dumpFile.WriteLine("Dumping finished");
            dumpFile.Close();
            MemoryManager.GetMemoryManager().StopTransaction(mTr);
        }
        /************************************************************************/
        /* Shorten a path with ellipses.                                        */
        /************************************************************************/
        String shortenPath(String path, int maxPath)
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
        String shortenPath(String path)
        {
            return shortenPath(path, 40);
        }
        String toString(OdDbObjectId val)
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
        String toDegreeString(double val)
        {
            return (val * 180.0 / Math.PI).ToString() + "\u00B0";
        }
        String toArcSymbolTypeString(int val)
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
    }
}
