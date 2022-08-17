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
using System.Collections;
//using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Reflection;

using Teigha.Core;
using Teigha.TG;

namespace ExDgnDumpSwigMgd
{
    class OdRxObject_Dumper : OdRxObject
    {
        public const int eOk = 0;
        public OdRxObject_Dumper()
            : base(new IntPtr(), true)
        {
        }
        public OdRxObject_Dumper(OdRxObject pObject) : base(OdRxObject.getCPtr(pObject).Handle, true)
        {
        }
        public virtual void dump( OdRxObject pObject){}
        public virtual String getClassName() { return String.Empty; }               //each pObject class has to define its own name

        public static UInt32 TestCtr = 0;
        //some useful methods for the other dump()'s
        public void startDescription( OdRxObject pObject )
        {
          writeShift();
          m_object = pObject;
          //write it name
          String className;
          {
              OdRxObject_Dumper dumper = Program.GetProperType(pObject);

              //OdRxObject_Dumper dumper = (OdRxObject_Dumper)pObject;

            className = dumper.getClassName();
            Program.DumpStream.WriteLine(className);
          }

          //add the path
          {
            Program.DumpStream.WriteLine(" Path: */");

            int i, j = m_nestedTypes.Count;

            for( i = 0; i < j; i++ )
            {
                Program.DumpStream.WriteLine(m_nestedTypes[ i ].ToString());
            }
          }

          //final actions
          Program.DumpStream.WriteLine();
          m_nestedTypes.Add( className );
          m_nesting++;

          //dump specific information
          writeLinkagesInfo( m_object );
          writeElementInfo( m_object );
          writeGraphicsElementInfo( m_object );
        }


        public void finishDescription()
        {
          m_nesting--;
         
          writeShift();
          Program.DumpStream.WriteLine(m_nestedTypes[m_nestedTypes.Count - 1]);

          m_nestedTypes.RemoveAt(m_nestedTypes.Count - 1);
        }


        public void describeSubElements( OdDgElementIterator iterator )
        {
          int           counter = 0;
          for( ; !iterator.done(); iterator.step(), counter++ )
          {
            writeShift();
            Program.DumpStream.WriteLine(string.Format("Sub element {0}:\n", counter));
            describeSubElement( iterator.item().openObject( OpenMode.kForRead ) );
            Program.DumpStream.Flush();
          }
        }

        public void describeSubElement( OdRxObject someObject )
        {
            OdRxObject_Dumper pDumper = Program.GetProperType(someObject);// (OdRxObject_Dumper)someObject;
            {
                if (null != pDumper)
                {
                    pDumper.dump(someObject);
                }
                else
                {
                    m_nesting++;
                    Program.DumpStream.WriteLine("Object does not have its dumper\n");
                    writeElementInfo(someObject);
                    m_nesting--;
                }
            }
        }

        public void writeShift()
        {
          int i;

          for( i = 0; i < m_nesting; i++ )
          {
            Program.DumpStream.WriteLine("  ");
          }
        }

        public void writeElementInfo( OdRxObject pObject )
        {
          OdDgElement element = OdDgElement.cast( pObject );
          if( null == element )
          {
            return;
          }

          writeShift();
          Program.DumpStream.WriteLine("Common information for DGN elements:\n");
          m_nesting++;

          writeFieldValue( "ID", element.elementId().getHandle().ascii() );
          writeFieldValue( "Type", element.getElementType().ToString() );
          if (element.isDBRO())
          {
            writeFieldValue("Is Locked", element.getLockedFlag());
          }

          m_nesting--;
        }

        public void writeGraphicsElementInfoLS3(OdRxObject pObject)
        {
            //OdDgGraphicsElement element = new OdDgGraphicsElement(OdRxObject.getCPtr(pObject).Handle, false);
            OdDgGraphicsElement element = OdDgGraphicsElement.cast(pObject);
            if (null == element)
            {
                return;
            }

            writeShift();
            Program.DumpStream.WriteLine("Common information for Graphics Elements:\n");

            m_nesting++;

            UInt32 level = element.getLevelEntryId();
            OdDgElementId id = element.database().getLevelTable().getAt(level);
            writeShift();
            if (id.isNull())
            {
                Program.DumpStream.WriteLine("level id is incorrect\n");
            }
            else
            {
                Program.DumpStream.WriteLine("level id is correct\n");
            }

            writeFieldValue("Level ID", element.getLevelEntryId());

            UInt32 uColorIndex = element.getColorIndex();

            if ((uColorIndex != (UInt32)ColorIndexConstants.kColorByLevel) & (uColorIndex != (UInt32)ColorIndexConstants.kColorByCell))
            {
                writeFieldValue("Color", element.getColor());
            }

            writeFieldValue("Color index", uColorIndex);

            switch (uColorIndex)
            {
                case (UInt32)ColorIndexConstants.kColorByLevel: Program.DumpStream.WriteLine("Color is defined by level\n"); break;
                case (UInt32)ColorIndexConstants.kColorByCell: Program.DumpStream.WriteLine("Color is defined by cell\n"); break;
            }

            writeFieldValue("Graphics group", element.getGraphicsGroupEntryId());
            writeFieldValue("Class", element.getClass());
            writeFieldValue("Line style", element.getLineStyleEntryId());
            if (element.getLineStyleEntryId() == (int)LineStyleConstants.kLineStyleByLevel)
            {
                Program.DumpStream.WriteLine("Element has LineStyleByLevel.\n");
            }
            writeFieldValue("Line weight", element.getLineWeight());
            if (element.getLineWeight() == (uint)LineWeightConstants.kLineWeightByLevel)
            {
                Program.DumpStream.WriteLine("Element has WeightByLevel.\n");
            }

            writeFieldValue("Is Invisible", element.getInvisibleFlag());
            writeFieldValue("Is 3D Format Element", element.get3dFormatFlag());
            writeFieldValue("View Independent", element.getViewIndependentFlag());
            writeFieldValue("Non Planar", element.getNonPlanarFlag());
            writeFieldValue("Not Snappable", element.getNotSnappableFlag());
            writeFieldValue("Hbit", element.getHbitFlag());

            //show the extents: recalculated one  saved one
            {
                OdGeExtents3d recalculatedExtent = new OdGeExtents3d();
                OdGeExtents3d savedExtent = new OdGeExtents3d();

                //recalculated
                {
                    if (element.getGeomExtents(recalculatedExtent) == eOk)
                    {
                        writeFieldValue("Result of getGeomExtents()", recalculatedExtent);
                    }
                    else
                    {
                        writeFieldValue("Result of getGeomExtents()", "invalid value");
                    }
                }
                //saved
                OdDgGraphicsElementPE pElementPE = OdDgGraphicsElementPE.cast(element);//(OdRxObject.getCPtr(element).Handle, true);
                //OdDgGraphicsElementPEPtr pElementPE = OdDgGraphicsElementPEPtr(OdRxObjectPtr(element));
                if (null != pElementPE)
                {
                    if (pElementPE.getRange(element, savedExtent) == eOk)
                    {
                        writeFieldValue("Saved extents", savedExtent);
                    }
                    else
                    {
                        writeFieldValue("Saved extents", "invalid value");
                    }
                }
            }

            m_nesting--;
        }

        public void writeGraphicsElementInfo( OdRxObject pObject )
        {
          OdDgGraphicsElement element = OdDgGraphicsElement.cast(pObject);
          if (null == element)
          {
              return;
          }
          else
          {
              ///////// test bug 16516 ////////////
              OdGiDrawable pDr = OdGiDrawable.cast(pObject);
              OdGeExtents3d extents = new OdGeExtents3d();
              //System.Windows.Forms.MessageBox.Show("before getgeomextents test");
              OdResult res = pDr.getGeomExtents(extents);
              /*if (OdResult.eOk != res)
              {
                  System.Windows.Forms.MessageBox.Show("after getgeomextents test");
              }*/
              /////////////////////////////////////
          }

          writeShift();
          Program.DumpStream.WriteLine("Common information for Graphics Elements:\n");
            
          m_nesting++;

          UInt32 level = element.getLevelEntryId();
          OdDgElementId id = element.database().getLevelTable().getAt( level );
          writeShift();
          if (id.isNull())
          {
            Program.DumpStream.WriteLine("level id is incorrect\n" );
          }
          else
          {
            Program.DumpStream.WriteLine("level id is correct\n" );
          }

          writeFieldValue( "Level ID", element.getLevelEntryId() );

          UInt32 uColorIndex = element.getColorIndex();

          if( (uColorIndex != (UInt32) ColorIndexConstants.kColorByLevel) & (uColorIndex != (UInt32) ColorIndexConstants.kColorByCell) )
          {
            writeFieldValue( "Color", element.getColor());
          }

          writeFieldValue( "Color index", uColorIndex );

          switch( uColorIndex )
          {
            case (UInt32)ColorIndexConstants.kColorByLevel : Program.DumpStream.WriteLine("Color is defined by level\n" ); break;
            case (UInt32)ColorIndexConstants.kColorByCell: Program.DumpStream.WriteLine("Color is defined by cell\n"); break;
          }

          writeFieldValue( "Graphics group", element.getGraphicsGroupEntryId() );
          writeFieldValue( "Class", element.getClass() );
          writeFieldValue( "Line style", element.getLineStyleEntryId() );
          if (element.getLineStyleEntryId() == (int)LineStyleConstants.kLineStyleByLevel)
          {
              Program.DumpStream.WriteLine("Element has LineStyleByLevel.\n");
          }
          writeFieldValue( "Line weight", element.getLineWeight() );
          if (element.getLineWeight() == (uint)LineWeightConstants.kLineWeightByLevel)
          {
              Program.DumpStream.WriteLine("Element has WeightByLevel.\n");
          }

          writeFieldValue( "Is Invisible", element.getInvisibleFlag() );
          writeFieldValue( "Is 3D Format Element", element.get3dFormatFlag() );
          writeFieldValue( "View Independent", element.getViewIndependentFlag() );
          writeFieldValue( "Non Planar", element.getNonPlanarFlag() );
          writeFieldValue( "Not Snappable", element.getNotSnappableFlag() );
          writeFieldValue( "Hbit", element.getHbitFlag() );

          //show the extents: recalculated one  saved one
          {
            OdGeExtents3d recalculatedExtent = new OdGeExtents3d();
            OdGeExtents3d savedExtent = new OdGeExtents3d();

            //recalculated
            {
              if( element.getGeomExtents( recalculatedExtent ) == eOk )
              {
                writeFieldValue( "Result of getGeomExtents()", recalculatedExtent );
              }
              else
              {
                writeFieldValue( "Result of getGeomExtents()", "invalid value" );
              }
            }
            //saved
            OdDgGraphicsElementPE pElementPE = OdDgGraphicsElementPE.cast(element);//(OdRxObject.getCPtr(element).Handle, true);
            //OdDgGraphicsElementPEPtr pElementPE = OdDgGraphicsElementPEPtr(OdRxObjectPtr(element));
            if( null != pElementPE )
            {
              if( pElementPE.getRange( element, savedExtent ) == eOk )
              {
                writeFieldValue( "Saved extents", savedExtent );
              }
              else
              {
                writeFieldValue( "Saved extents", "invalid value" );
              }
            }
          }

          m_nesting--;
        }


        public void writeLinkagesInfo( OdRxObject pObject )
        {
          OdDgElement element = OdDgElement.cast( pObject );

          if( null == element )
          {
            return;
          }

          //take all linkages
          OdRxObjectPtrArray linkages = new OdRxObjectPtrArray();
          element.getLinkages( linkages );
          int linkagesNumber = linkages.Count;

          if( linkagesNumber > 0 )
          {
            writeShift();
            Program.DumpStream.WriteLine(string.Format("> Attribute Linkages (%{0} items)\n", linkagesNumber));
            m_nesting++;

            for ( int i = 0; i < linkagesNumber; ++i)
            {
                OdDgAttributeLinkage pLinkage = (OdDgAttributeLinkage)linkages[i];
        	 
              OdBinaryData data = new OdBinaryData();
              pLinkage.getData(data);
              writeShift(); 
              Program.DumpStream.WriteLine(string.Format("Primary ID = 0x%{0}, data size = %{1}", pLinkage.getPrimaryId(), data.Count ));

              //additionary info depending on the type
              switch( pLinkage.getPrimaryId() )
              {
                case (ushort)OdDgPatternLinkage.PrimaryIds.kHatch:
                {
                  OdDgPatternLinkage pPatternLinkage = OdDgPatternLinkage.cast( pLinkage );
                  if ( null != pPatternLinkage )
                  {
                    String namedType;
                    switch( pPatternLinkage.getType() )
                    {
                    case OdDgPatternLinkage.PatternType.kLinearPattern : namedType = "LinearPattern"; break;
                    case OdDgPatternLinkage.PatternType.kCrossPattern : namedType = "CrossPattern"; break;
                    case OdDgPatternLinkage.PatternType.kSymbolPattern : namedType = "SymbolPattern"; break;
                    case OdDgPatternLinkage.PatternType.kDWGPattern : namedType = "DWGPattern"; break;
                    default : namedType = "Unknown"; break;
                    }
                    Program.DumpStream.WriteLine(string.Format(" ( Pattern type = {0}", namedType));
                    if( pPatternLinkage.getUseOffsetFlag() )
                    {
                      OdGePoint3d offset = new OdGePoint3d();
                      pPatternLinkage.getOffset( offset );
                      //Program.DumpStream.WriteLine("; offset = (%g %g %g)", offset.x, offset.y, offset.z );
                      Program.DumpStream.WriteLine(string.Format("; offset = (%g %g %g)", offset.x, offset.y, offset.z));
                    }
                    Program.DumpStream.WriteLine(")");
                  }
                }
                break;
                case (ushort)OdDgPatternLinkage.PrimaryIds.kThickness:
                {
                  OdDgThicknessLinkage pThicknessLinkage = OdDgThicknessLinkage.cast( pLinkage );

                  if ( null != pThicknessLinkage )
                  {
                      Program.DumpStream.WriteLine(string.Format(" ( Thickness Linkage, thickness = {0} )", pThicknessLinkage.getThickness()));
                  }
                }
                break;
                case (ushort)OdDgPatternLinkage.PrimaryIds.kString:
                {
                  OdDgStringLinkage pStrLinkage = OdDgStringLinkage.cast( pLinkage );

                  if ( null != pStrLinkage )
                  {
                      Program.DumpStream.WriteLine(string.Format(" ( String Linkage, ID = {0}; value = {1})", pStrLinkage.getStringId(), pStrLinkage.getString()));
                  }
                }
                break;
                case (ushort)OdDgPatternLinkage.PrimaryIds.kDMRS:
                {
                  OdDgDMRSLinkage DMRSLinkage = OdDgDMRSLinkage.cast( pLinkage );
                  if( null != DMRSLinkage )
                  {
                      Program.DumpStream.WriteLine(string.Format(" ( DMRS Linkage, tableId = {0}, MSLink = {1}, type = {2} )", DMRSLinkage.getTableId(), DMRSLinkage.getMSLink(), DMRSLinkage.getType()));
                  }        
                }
                break;
              case 0x56D5:
                {
                  OdDgProxyLinkage linkage = OdDgProxyLinkage.cast( pLinkage );
                  if( null != linkage )
                  {
                    OdBinaryData d_data = new OdBinaryData();
                    linkage.getData( d_data );
                    Int32 ii = d_data.Count;
                    Int32 j = d_data.Count;
                    Program.DumpStream.WriteLine(" ( Proxy linkage )\n" );
                    m_nesting++;
                    writeFieldValue( "Size of the proxy linkage", j );
                    for( ii = 0; ii < j; ii++ )
                    {
                      if( 0 == ( ii % 16 ) )
                      {
                        if( ii > 0 )
                        {
                            Program.DumpStream.WriteLine("\n");
                        }
                        writeShift();
                        //Program.DumpStream.WriteLine("%.4X: ", i );
                        Program.DumpStream.WriteLine(ii.ToString());
                      }
                      //Program.DumpStream.WriteLine("%.2X ", data[ i ] );
                      Program.DumpStream.WriteLine(d_data[ ii ].ToString());
                    }
                    m_nesting--;
                  }
                }
                break;
              case (ushort)OdDgPatternLinkage.PrimaryIds.kFRAMME: // DB Linkage - FRAMME tag data signature
              case (ushort)OdDgPatternLinkage.PrimaryIds.kBSI: // DB Linkage - secondary id link (BSI radix 50)
              case (ushort)OdDgPatternLinkage.PrimaryIds.kXBASE: // DB Linkage - XBase (DBase)
              case (ushort)OdDgPatternLinkage.PrimaryIds.kINFORMIX: // DB Linkage - Informix
              case (ushort)OdDgPatternLinkage.PrimaryIds.kINGRES: // DB Linkage - INGRES
              case (ushort)OdDgPatternLinkage.PrimaryIds.kSYBASE: // DB Linkage - Sybase
              case (ushort)OdDgPatternLinkage.PrimaryIds.kODBC: // DB Linkage - ODBC
              case (ushort)OdDgPatternLinkage.PrimaryIds.kOLEDB: // DB Linkage - OLEDB
              case (ushort)OdDgPatternLinkage.PrimaryIds.kORACLE: // DB Linkage - Oracle
              case (ushort)OdDgPatternLinkage.PrimaryIds.kRIS: // DB Linkage - RIS
                {
                  OdDgDBLinkage dbLinkage = OdDgDBLinkage.cast( pLinkage );
                  if( null != dbLinkage )
                  {
                    String namedType;

                    switch( dbLinkage.getDBType() )
                    {
                        case OdDgDBLinkage.DBType.kBSI: namedType = "BSI"; break;
                        case OdDgDBLinkage.DBType.kFRAMME: namedType = "FRAMME"; break;
                        case OdDgDBLinkage.DBType.kInformix: namedType = "Informix"; break;
                        case OdDgDBLinkage.DBType.kIngres: namedType = "Ingres"; break;
                        case OdDgDBLinkage.DBType.kODBC: namedType = "ODBC"; break;
                        case OdDgDBLinkage.DBType.kOLEDB: namedType = "OLE DB"; break;
                        case OdDgDBLinkage.DBType.kOracle: namedType = "Oracle"; break;
                        case OdDgDBLinkage.DBType.kRIS: namedType = "RIS"; break;
                        case OdDgDBLinkage.DBType.kSybase: namedType = "Sybase"; break;
                        case OdDgDBLinkage.DBType.kXbase: namedType = "xBase"; break;
                    default : namedType = "Unknown"; break;
                    }

                      Program.DumpStream.WriteLine(string.Format(" ( DB Linkage, tableId = {0}, MSLink = {1}, type = {2} )", dbLinkage.getTableEntityId(), dbLinkage.getMSLink(), namedType));
                  }
                }
                break;
              case (ushort)OdDgPatternLinkage.PrimaryIds.kDimension: // Dimension Linkage
                {
                  OdDgDimensionLinkage dimLinkage = OdDgDimensionLinkage.cast( pLinkage );
                  if( null != dimLinkage )
                  {
                    String sDimType;
                    switch( dimLinkage.getType() )
                    {
                      case OdDgDimensionLinkage.DimensionSubType.kOverall : sDimType = "Overall"; break;
                      case OdDgDimensionLinkage.DimensionSubType.kSegment: sDimType = "Segment"; break;
                      case OdDgDimensionLinkage.DimensionSubType.kPoint: sDimType = "Point"; break;
                      case OdDgDimensionLinkage.DimensionSubType.kSegmentFlags: sDimType = "SegmentFlags"; break;
                      case OdDgDimensionLinkage.DimensionSubType.kDimensionInfo: sDimType = "DimensionInfo"; break;
                      default: sDimType = "Unkown"; break;
                    }
                    Program.DumpStream.WriteLine(" ( Dimension Linkage, type = {0} )", sDimType);
                    switch( dimLinkage.getType() )
                    {
                        case OdDgDimensionLinkage.DimensionSubType.kOverall: 
                      break;
                        case OdDgDimensionLinkage.DimensionSubType.kSegment: 
                      break;
                        case OdDgDimensionLinkage.DimensionSubType.kPoint: 
                      break;
                        case OdDgDimensionLinkage.DimensionSubType.kSegmentFlags: 
                      break;
                        case OdDgDimensionLinkage.DimensionSubType.kDimensionInfo: 
                    {
                        OdDgDimensionInfoLinkage pDimInfoLinkage = (OdDgDimensionInfoLinkage)pLinkage;

                       if( pDimInfoLinkage.getUseAnnotationScale() )
                          writeFieldValue( "  Annotation Scale", pDimInfoLinkage.getAnnotationScale() );

                       if( pDimInfoLinkage.getUseDatumValue() )
                       {
                         double dDatumValue = pDimInfoLinkage.getDatumValue();

                         OdDgDatabase pDb = element.database();
                         OdDgElementId idModel = new OdDgElementId();

                         if( null != pDb )
                           idModel = pDb.getActiveModelId();

                         if( null != idModel )
                         {
                           OdDgModel pModel = OdDgModel.cast( idModel.openObject() );

                           if( null != pModel )
                           {
                             dDatumValue = pModel.convertUORsToWorkingUnits( dDatumValue );
                           }
                         }
                         else
                         {
                           dDatumValue /= 10000000000; // Storage units default factor
                         }

                         writeFieldValue( "  Datum Value", dDatumValue );
                       }

                       if( pDimInfoLinkage.getUseRetainFractionalAccuracy() )
                       {
                         writeFieldValue( "  Detriment in reverse direction", pDimInfoLinkage.getUseDecrimentInReverceDirection() );
                         writeFieldValue( "  Primary retain fractional accuracy", pDimInfoLinkage.getPrimaryRetainFractionalAccuracy() );
                         writeFieldValue( "  Secondary retain fractional accuracy", pDimInfoLinkage.getSecondaryRetainFractionalAccuracy() );
                         writeFieldValue( "  Primary alt format retain fractional accuracy", pDimInfoLinkage.getPrimaryAltFormatRetainFractionalAccuracy() );
                         writeFieldValue( "  Secondary alt format retain fractional accuracy", pDimInfoLinkage.getSecondaryAltFormatRetainFractionalAccuracy() );
                         writeFieldValue( "  Primary tolerance retain fractional accuracy", pDimInfoLinkage.getPrimaryTolerRetainFractionalAccuracy() );
                         writeFieldValue( "  Secondary tolerance retain fractional accuracy", pDimInfoLinkage.getSecondaryTolerRetainFractionalAccuracy() );
                         writeFieldValue( "  Label line mode", pDimInfoLinkage.getLabelLineDimensionMode());
                       }

                       if( pDimInfoLinkage.getUseFitOptionsFlag() )
                       {
                         writeFieldValue( "  Suppress unfit terminators", pDimInfoLinkage.getUseSuppressUnfitTerm() );
                         writeFieldValue( "  Use inline leader length", pDimInfoLinkage.getUseInlineLeaderLength() );
                         writeFieldValue( "  Text above optimal fit", pDimInfoLinkage.getUseTextAboveOptimalFit() );
                         writeFieldValue( "  Narrow font optimal fit", pDimInfoLinkage.getUseNarrowFontOptimalFit() );
                         writeFieldValue( "  Use Min Leader Terminator Length", pDimInfoLinkage.getUseMinLeaderTermLength() );
                         writeFieldValue( "  Use auto mode for dimension leader", pDimInfoLinkage.getUseAutoLeaderMode() );
                         writeFieldValue( "  Fit Options ", pDimInfoLinkage.getFitOptions() );
                       }

                       if( pDimInfoLinkage.getUseTextLocation() )
                       {
                         writeFieldValue( "  Free location of text", pDimInfoLinkage.getUseFreeLocationText() ); 
                         writeFieldValue( "  Note spline fit", pDimInfoLinkage.getUseNoteSplineFit() );
                         writeFieldValue( "  Text location ", pDimInfoLinkage.getTextLocation() );
                       }

                       if( pDimInfoLinkage.getUseInlineLeaderLengthValue() )
                       {
                         double dLengthValue = pDimInfoLinkage.getInlineLeaderLength();

                         OdDgDatabase pDb = element.database();
                         OdDgElementId idModel = new OdDgElementId();

                         if( null != pDb )
                           idModel = pDb.getActiveModelId();

                         if( null != idModel )
                         {
                           OdDgModel pModel = OdDgModel.cast( idModel.openObject() );

                           if( null != pModel )
                           {
                             dLengthValue = pModel.convertUORsToWorkingUnits( dLengthValue );
                           }
                         }
                         else
                         {
                           dLengthValue /= 10000000000; // Storage units default factor
                         }

                         writeFieldValue( "  Inline leader length value", dLengthValue );
                       }

                       if( pDimInfoLinkage.getUseInlineTextLift() )
                         writeFieldValue( "  Inline text lift", pDimInfoLinkage.getInlineTextLift() );

                       if( pDimInfoLinkage.getUseNoteFrameScale() )
                         writeFieldValue( "  Note frame scale", pDimInfoLinkage.getUseNoteFrameScale() );

                       if( pDimInfoLinkage.getUseNoteLeaderLength() )
                         writeFieldValue( "  Note leader length", pDimInfoLinkage.getNoteLeaderLength() );

                       if( pDimInfoLinkage.getUseNoteLeftMargin() )
                         writeFieldValue( "  Note left margin", pDimInfoLinkage.getUseNoteLeftMargin() );

                       if( pDimInfoLinkage.getUseNoteLowerMargin() )
                         writeFieldValue( "  Note lower margin", pDimInfoLinkage.getUseNoteLowerMargin() );

                       if( pDimInfoLinkage.getUsePrimaryToleranceAccuracy() )
                         writeFieldValue( "  Primary tolerance accuracy", pDimInfoLinkage.getPrimaryToleranceAccuracy() );

                       if( pDimInfoLinkage.getUseSecondaryToleranceAccuracy() )
                         writeFieldValue( "  Secondary tolerance accuracy", pDimInfoLinkage.getSecondaryToleranceAccuracy() );

                       if( pDimInfoLinkage.getUseStackedFractionScale() )
                         writeFieldValue( "  Stacked fraction scale", pDimInfoLinkage.getStackedFractionScale() );
                     } break;
                    default:
                      break;
                    }
                  }
                  break;
                }
              case (ushort)OdDgPatternLinkage.PrimaryIds.kFilterMember:
              {
                  OdDgFilterMemberLinkage pFilterLinkage = (OdDgFilterMemberLinkage)pLinkage;
                writeFieldValue( "  Member Id", pFilterLinkage.getMemberId() );
                writeFieldValue( "  Member Type", pFilterLinkage.getMemberType() );
                writeFieldValue( "  Name String", pFilterLinkage.getNameString() );
                writeFieldValue( "  Expression String", pFilterLinkage.getExpressionString() );
              } break;
              case (ushort)OdDgPatternLinkage.PrimaryIds.kDependency: 
                {
                  OdDgDependencyLinkage dependencyLinkage = OdDgDependencyLinkage.cast( pLinkage );
                  if( null != dependencyLinkage )
                  {
                    Program.DumpStream.WriteLine(string.Format("( Root type = {0}; App ID = {1}; App Value = {2} )",
                      dependencyLinkage.getRootDataType(),
                      dependencyLinkage.getAppValue(),
                      dependencyLinkage.getAppValue() ));

                    //some additional information
                    m_nesting++;
                    switch( dependencyLinkage.getRootDataType() ) 
                    {
                    case OdDgDependencyLinkage.RootDataType.kElementId :
                      OdDgDepLinkageElementId elementIdLinkage = OdDgDepLinkageElementId.cast( dependencyLinkage );
                      if( null != elementIdLinkage )
                      {
                          UInt32 j = elementIdLinkage.getCount();
                          UInt32 ii = elementIdLinkage.getCount();
                          Program.DumpStream.WriteLine("\n");
                        m_nesting++;
                        writeShift();
                        Program.DumpStream.WriteLine(string.Format("Number of IDs: {0}; They are:", j ) );
                        for( ii = 0; ii < j; ii++ )
                        {
                          Program.DumpStream.WriteLine(string.Format(" {0}.8I64X", elementIdLinkage.getAt( ii ) ));
                        }
                        m_nesting--;
                      }
                      break;
                    case OdDgDependencyLinkage.RootDataType.kAssocPoint_I:
                      {
                          OdDgDepLinkageAssocPointI pDepLinkageAssocPt = OdDgDepLinkageAssocPointI.cast(dependencyLinkage);
                          var idCount = pDepLinkageAssocPt.getCount();

                          for (UInt32 j = 0; j < idCount; ++j)
                          {                              
                              /*tmp#18875*/
                              OdDgAssocPointIData rootData = pDepLinkageAssocPt.getAt(j);
                              Program.DumpStream.WriteLine("\n");
                              m_nesting++;
                              writeShift();
                              Program.DumpStream.WriteLine(string.Format("Root Number {0}", j));
                              Program.DumpStream.WriteLine(string.Format("PointData {0}", rootData.getPointData().ToString()));
                              Program.DumpStream.WriteLine(string.Format("   Int 1 {0}", rootData.m_iParam1));
                              Program.DumpStream.WriteLine(string.Format("   Int 2 {0}", rootData.m_iParam2));
                              m_nesting--;
                          }
                      } break;
                    }
                    m_nesting--;
                  }
                }
                break;
              }
              Program.DumpStream.WriteLine("\n" );
            }

            m_nesting--;
            writeShift();
            Program.DumpStream.WriteLine("< Attribute Linkages\n" );
          }
        }


        public void writeFieldValue( String name, String value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}\n", value));
        }


        public void writeFieldValue( String name, UInt16 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}\n", value));
        }


        public void writeFieldValueHex( String name, UInt16 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0:X}\n", value ));
        }


        public void writeFieldValue( String name, UInt32 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}\n", value));
        }

        public void writeFieldValue(String name, OdDgLevelFilterTable.OdDgFilterMemberType value)
        {
          String strValue;

          switch( value )
          {
              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeShort:
            {
              strValue = ("kTypeShort");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeInt:
            {
              strValue = ("kTypeInt");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeBool:
            {
              strValue = ("kTypeBool");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeChar:
            {
              strValue = ("kTypeChar");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeWChar:
            {
              strValue = ("kTypeWChar");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeDouble:
            {
              strValue = ("kTypeDouble");
            } break;

              case OdDgLevelFilterTable.OdDgFilterMemberType.kMemberTypeTime:
            {
              strValue = ("kTypeTime");
            } break;

            default:
            {
              strValue = ("kTypeNull");
            } break;
          }

          writeFieldValue(name, strValue);
        }

        public void writeFieldValueHex( String name, UInt32 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0:X}\n", value ));
        }


        public void writeFieldValue( String name, double value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0:F}\n", value ));
        }


        public void writeFieldValue( String name, OdGePoint2d value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}; {1}\n", value.x, value.y ));
        }


        public void writeFieldValue( String name, OdGePoint3d value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}; {1}; {2}\n", value.x, value.y, value.z ));
        }


        public void writeFieldValue( String name, OdGeVector3d value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}; {1}; {2}\n", value.x, value.y, value.z ));
        }


        public void writeFieldValue( String name, OdCmEntityColor value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("R: {0} G: {1} B: {2}\n",
            value.red(), value.green(), value.blue() ) );
        }


        public void writeFieldValue( String name, OdDgLineStyleInfo value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("Modifiers: {0:X} Scale: {1} Shift: {2} Start width: {3} End width: {4}\n",
            value.getModifiers(), value.getScale(), value.getShift(), value.getStartWidth(), value.getEndWidth() ));
        }


        public void writeFieldValue( String name, UInt64 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}\n", value ));
        }


        public void writeFieldValue( String name, OdDgLightColor value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("R: {0} G: {1} B: {2} Intensity: {3}\n",
                    value.getRed(), value.getGreen(), value.getBlue(), value.getIntensityScale() ));
        }


        public void writeFieldValue( String name, bool value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(value ? "true\n" : "false\n");
        }


        public void writeFieldValue( String name, OdAngleCoordinate value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}° {1}' {2}\"\n",
                    value.getDegrees(), value.getMinutes(), value.getSeconds()) );
        }


        public void writeFieldValue( String name, Int16 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}\n", value ) );
        }


        public void writeFieldValue( String name, Int32 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}I32i\n", value ) );
        }

        public void writeFieldValue( String name, Int64 value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}iI64\n", value ) );
        }


        public void writeFieldValue( String name, OdDgModel.WorkingUnit value )
        {
          writeFieldName( name );
          
          switch( value )
          {
          case OdDgModel.WorkingUnit.kWuUnitOfResolution : Program.DumpStream.WriteLine("unit of resolution\n" ); break;
          case OdDgModel.WorkingUnit.kWuStorageUnit : Program.DumpStream.WriteLine("storage unit\n" ); break;
          case OdDgModel.WorkingUnit.kWuWorldUnit : Program.DumpStream.WriteLine("world unit\n" ); break;
          case OdDgModel.WorkingUnit.kWuMasterUnit : Program.DumpStream.WriteLine("master unit\n" ); break;
          case OdDgModel.WorkingUnit.kWuSubUnit: Program.DumpStream.WriteLine("sub unit\n"); break;
          }
        }


        public void writeFieldValue( String name, TextJustification value )
        {
          writeFieldName( name );

          switch( value )
          {
          case TextJustification.kLeftTop          : Program.DumpStream.WriteLine("left top\n" ); break;
          case TextJustification.kLeftCenter       : Program.DumpStream.WriteLine("left center\n" ); break;
          case TextJustification.kLeftBottom       : Program.DumpStream.WriteLine("left bottom\n" ); break;
          case TextJustification.kLeftMarginTop    : Program.DumpStream.WriteLine("left margin top\n" ); break;
          case TextJustification.kLeftMarginCenter : Program.DumpStream.WriteLine("left margin center\n" ); break;
          case TextJustification.kLeftMarginBottom : Program.DumpStream.WriteLine("left margin bottom\n" ); break;
          case TextJustification.kCenterTop        : Program.DumpStream.WriteLine("center top\n" ); break;
          case TextJustification.kCenterCenter     : Program.DumpStream.WriteLine("center center\n" ); break;
          case TextJustification.kCenterBottom     : Program.DumpStream.WriteLine("center bottom\n" ); break;
          case TextJustification.kRightMarginTop   : Program.DumpStream.WriteLine("right margin top\n" ); break;
          case TextJustification.kRightMarginCenter : Program.DumpStream.WriteLine("right margin center\n" ); break;
          case TextJustification.kRightMarginBottom : Program.DumpStream.WriteLine("right margin bottom\n" ); break;
          case TextJustification.kRightTop          : Program.DumpStream.WriteLine("right top\n" ); break;
          case TextJustification.kRightCenter       : Program.DumpStream.WriteLine("right center\n" ); break;
          case TextJustification.kRightBottom       : Program.DumpStream.WriteLine("right bottom\n" ); break;
          }
        }


        public void writeFieldValue( String name, OdGeQuaternion value )
        {
          writeFieldName( name );

          Program.DumpStream.WriteLine(string.Format("{0}; {1}; {2}; {3}\n", value.w, value.x, value.y, value.z) );
        }

        public void writeFieldValue( String name, OdDgGraphicsElement.Class value )
        {
          writeFieldName( name );

          switch( value )
          {
              case OdDgGraphicsElement.Class.kClassPrimary: Program.DumpStream.WriteLine("Primary\n"); break;
          case OdDgGraphicsElement.Class.kClassPatternComponent : Program.DumpStream.WriteLine("Pattern component\n" ); break;
          case OdDgGraphicsElement.Class.kClassConstruction     : Program.DumpStream.WriteLine("Construction\n" ); break;
          case OdDgGraphicsElement.Class.kClassDimension        : Program.DumpStream.WriteLine("Dimension\n" ); break;
          case OdDgGraphicsElement.Class.kClassPrimaryRule      : Program.DumpStream.WriteLine("Primary rule\n" ); break;
          case OdDgGraphicsElement.Class.kClassLinearPatterned  : Program.DumpStream.WriteLine("Linear patterned\n" ); break;
          case OdDgGraphicsElement.Class.kClassConstructionRule: Program.DumpStream.WriteLine("Construction rule\n"); break;
          }
        }


        public void writeFieldValue( String name, OdGeMatrix2d value )
        {
          writeFieldName( name );
          Program.DumpStream.WriteLine(string.Format("{0}; {1}; {2}; {3}\n", value[0,0], value[1,0], value[0,1], value[1,1]));
        }


        public void writeFieldValue( String name, OdDgDimension.ToolType value )
        {
          writeFieldName( name );
          OdDgDimension pDim = OdDgDimension.cast( m_object );
          switch( value )
          {
          case OdDgDimension.ToolType.kToolTypeInvalid             : Program.DumpStream.WriteLine("Invalid\n" ); break;
          case OdDgDimension.ToolType.kToolTypeSizeArrow           : Program.DumpStream.WriteLine("Size arrow\n" ); break;
          case OdDgDimension.ToolType.kToolTypeSizeStroke         : Program.DumpStream.WriteLine("Size stroke\n" ); break;
          case OdDgDimension.ToolType.kToolTypeLocateSingle       : Program.DumpStream.WriteLine("Locate single\n" ); break;
          case OdDgDimension.ToolType.kToolTypeLocateStacked      : Program.DumpStream.WriteLine("Locate stacked\n" ); break;
          case OdDgDimension.ToolType.kToolTypeAngleSize          : Program.DumpStream.WriteLine("Angle size\n" ); break;
          case OdDgDimension.ToolType.kToolTypeArcSize            : Program.DumpStream.WriteLine("Arc size\n" ); break;
          case OdDgDimension.ToolType.kToolTypeAngleLocation      : Program.DumpStream.WriteLine("Angle location\n" ); break;
          case OdDgDimension.ToolType.kToolTypeArcLocation        : Program.DumpStream.WriteLine("Arc location\n" ); break;
          case OdDgDimension.ToolType.kToolTypeAngleLines         : Program.DumpStream.WriteLine("Angle lines\n" ); break;
          case OdDgDimension.ToolType.kToolTypeAngleAxis          : Program.DumpStream.WriteLine("Angle axis\n" ); break;
          case OdDgDimension.ToolType.kToolTypeRadius             : Program.DumpStream.WriteLine("Radius\n" ); break;
          case OdDgDimension.ToolType.kToolTypeDiameter           : Program.DumpStream.WriteLine("Diameter\n" ); break;
          case OdDgDimension.ToolType.kToolTypeDiameterPara       : Program.DumpStream.WriteLine("Diameter para\n" ); break;
          case OdDgDimension.ToolType.kToolTypeDiameterPerp       : Program.DumpStream.WriteLine("Diameter perp\n" ); break;
          case OdDgDimension.ToolType.kToolTypeCustomLinear       : Program.DumpStream.WriteLine("Custom linear\n" ); break;
          case OdDgDimension.ToolType.kToolTypeOrdinate           : 
          {
            Program.DumpStream.WriteLine("Ordinate\n" ); 

            OdDgDimOrdinate pOrdinateDim = OdDgDimOrdinate.cast(pDim);

            if( null != pOrdinateDim )
            {
              m_nesting++;

              if(pOrdinateDim.getArcSymbolFlag())
                Program.DumpStream.WriteLine("Tool Arc Symbol Flag set\n" );

              Program.DumpStream.WriteLine(string.Format("Tool Datum Value: {0}\n", pOrdinateDim.getDatumValue() ));

              if( pOrdinateDim.getDecrementInReverseDirectionFlag() )
                Program.DumpStream.WriteLine("Tool DecrementInReverse Direction Flag set\n" );

              if( pOrdinateDim.getFreeLocationOfTxtFlag() )
                Program.DumpStream.WriteLine("Tool FreeLocationOfTxt Flag set\n" );

              if( pOrdinateDim.getStackExtLinesFlag() )
                Program.DumpStream.WriteLine("Tool StackExtLines Flag set\n" );

              m_nesting--;
            }
          } break;
          case OdDgDimension.ToolType.kToolTypeRadiusExtended     : Program.DumpStream.WriteLine("Radius extended\n" ); break;
          case OdDgDimension.ToolType.kToolTypeDiameterExtended   : Program.DumpStream.WriteLine("Diameter extended\n" ); break;
          case OdDgDimension.ToolType.kToolTypeCenter              : Program.DumpStream.WriteLine("Center\n" ); break;
          }
        }


        public void writeFieldValue( String name, OdDgDimPoint value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "  Point", value.getPoint() );
          writeFieldValue( "  Offset to dimension line", value.getOffsetToDimLine() );
          writeFieldValue( "  Offset Y", value.getOffsetY() );

          switch( value.getJustification() )
          {
            case OdDgDimTextInfo.TextAlignment.kLeftText:
            {
              writeFieldValue("  Text alignment", "kLeftText" );
            } break;

            case OdDgDimTextInfo.TextAlignment.kCenterLeftText:
            {
              writeFieldValue("  Text alignment", ("kCenterLeftText") );
            } break;

            case OdDgDimTextInfo.TextAlignment.kCenterRightText:
            {
              writeFieldValue("  Text alignment", ("kCenterRightText") );
            } break;

            case OdDgDimTextInfo.TextAlignment.kRightText:
            {
              writeFieldValue("  Text alignment", ("kRightText") );
            } break;

            case OdDgDimTextInfo.TextAlignment.kManualText:
            {
              writeFieldValue("  Text alignment", ("kManualText") );
            } break;
          }

          writeFieldName("  Flags:");
          writeFieldValue( "    Associative", value.getAssociativeFlag() );
          writeFieldValue( "    Relative", value.getRelative() != 0 );
          writeFieldValue( "    WitnessControlLocal", value.getWitnessControlLocalFlag() );
          writeFieldValue( "    NoWitnessLine", value.getNoWitnessLineFlag() );
          writeFieldValue( "    UseAltSymbology", value.getUseAltSymbologyFlag() );

          if( value.getPrimaryTextFlag() )
          {
            writeFieldValue( "  Primary text", value.getPrimaryText() );
          }

          if( value.getPrimaryTopToleranceTextFlag() )
          {
            writeFieldValue( "  Primary Top text", value.getPrimaryTopToleranceText() );
          }

          if( value.getPrimaryBottomToleranceTextFlag() )
          {
            writeFieldValue( "  Primary Bottom text", value.getPrimaryBottomToleranceText() );
          }

          if( value.getSecondaryTextFlag() )
          {
            writeFieldValue( "  Secondary text", value.getSecondaryText() );
          }

          if( value.getSecondaryTopToleranceTextFlag() )
          {
            writeFieldValue( "  Secondary Top text", value.getSecondaryTopToleranceText() );
          }

          if( value.getSecondaryBottomToleranceTextFlag() )
          {
            writeFieldValue( "  Secondary Bottom text", value.getSecondaryBottomToleranceText() );
          }

          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgDimTextInfo value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "  Width", value.getWidth() );
          writeFieldValue( "  Height", value.getHeight() );
          writeFieldValue( "  Font ID", value.getFontEntryId() );
          writeFieldValue( "  Color", value.getColorIndex() );
          writeFieldValue( "  Weight", value.getWeight() );

          switch( value.getStackedFractionType() )
          {
              case OdDgDimTextInfo.StackedFractionType.kFractionFromText:
            {
              writeFieldValue( "  Stacked Fraction Type", ("kFractionFromText") );
            } break;

              case OdDgDimTextInfo.StackedFractionType.kFractionHorizontal:
            {
              writeFieldValue( "  Stacked Fraction Type", ("kFractionHorizontal") );
            } break;

            case OdDgDimTextInfo.StackedFractionType.kFractionDiagonal :
            {
              writeFieldValue( "  Stacked Fraction Type", ("kFractionDiagonal") );
            } break;
          }

          switch( value.getStackFractAlignment() )
          {
            case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentTop :
            {
              writeFieldValue( "  Stacked Fraction Alignment", ("kFractAlignmentTop") );
            } break;

            case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentCenter :
            {
              writeFieldValue( "  Stacked Fraction Alignment", ("kFractAlignmentCenter") );
            } break;

            case OdDgDimTextInfo.StackFractionAlignment.kFractAlignmentBottom :
            {
              writeFieldValue( "  Stacked Fraction Alignment", ("kFractAlignmentBottom") );
            } break;
          }

          writeFieldName("  Text Flags:");
          writeFieldValue( "    Use text color", value.getUseColorFlag() );
          writeFieldValue( "    Use weight ", value.getUseWeightFlag() );
          writeFieldValue( "    Show primary master units ", !value.getPrimaryNoMasterUnitsFlag());
          writeFieldValue( "    Has primary alt format ", !value.getHasAltFormatFlag() );
          writeFieldValue( "    Show secondary master units ", !value.getSecNoMasterUnitsFlag());
          writeFieldValue( "    Has secondary alt format ", !value.getHasSecAltFormatFlag() );

          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgDimTextFormat value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "  Primary accuracy", value.getPrimaryAccuracy() );
          writeFieldValue( "  Secondary accuracy", value.getSecondaryAccuracy() );

          switch( value.getAngleMode() )
          {
              case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_D:
            {
              writeFieldValue( "  Angle display mode", ("kAngle_D") );
            } break;

            case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_DM:
            {
              writeFieldValue( "  Angle display mode", ("kAngle_DM") );
            } break;

            case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_DMS:
            {
              writeFieldValue( "  Angle display mode", ("kAngle_DMS") );
            } break;

            case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_C:
            {
              writeFieldValue( "  Angle display mode", ("kAngle_C") );
            } break;

            case OdDgDimTextFormat.AngleValueDisplayMode.kAngle_Radian:
            {
              writeFieldValue( "  Angle display mode", ("kAngle_Radian") );
            } break;
          }

          writeFieldName("  Text Format Flags:");
          writeFieldValue( "    AngleMeasure", value.getAngleMeasureFlag() );
          writeFieldValue( "    AngleFormat", value.getAngleFormatFlag() );
          writeFieldValue( "    PrimarySubUnits", value.getPrimarySubUnitsFlag() );
          writeFieldValue( "    PrimaryLabels", value.getPrimaryLabelsFlag() );
          writeFieldValue( "    PrimaryDelimiter", value.getPrimaryDelimiterFlag() );
          writeFieldValue( "    DecimalComma", value.getDecimalCommaFlag() );
          writeFieldValue( "    SuperScriptLSD", value.getSuperScriptLSDFlag() );
          writeFieldValue( "    RoundLSD", value.getRoundLSDFlag() );
          writeFieldValue( "    OmitLeadDelimiter", value.getOmitLeadDelimiterFlag() );
          writeFieldValue( "    LocalFileUnits", value.getLocalFileUnitsFlag() );
          writeFieldValue( "    UnusedDeprecated", value.getUnusedDeprecatedFlag() );
          writeFieldValue( "    ThousandSeparator", value.getThousandSepFlag() );
          writeFieldValue( "    MetricSpace", value.getMetricSpaceFlag() );
          writeFieldValue( "    SecondarySubUnits", value.getSecondarySubUnitsFlag() );
          writeFieldValue( "    SecondaryLabels", value.getSecondaryLabelsFlag() );
          writeFieldValue( "    SecondaryDelimiter", value.getSecondaryDelimiterFlag() );
          writeFieldValue( "    Radians", value.getRadiansFlag() );
          writeFieldValue( "    Show primary master units if zero", value.getPriAllowZeroMastFlag() );
          writeFieldValue( "    Show secondary master units if zero", value.getSecAllowZeroMastFlag() );
          writeFieldValue( "    Show primary sub units if zero", !value.getPriSubForbidZeroMastFlag() );
          writeFieldValue( "    Show secondary sub units if zero", !value.getSecSubForbidZeroMastFlag() );
          writeFieldValue( "    HideAngleSeconds", !value.getHideAngleSecondsFlag() );
          writeFieldValue( "    SkipNonStackedFractionSpace", !value.getSkipNonStackedFractionSpaceFlag() );
          
          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgDimTextFormat.Accuracy value )
        {
          writeFieldName( name );

          switch( value )
          {
          case OdDgDimTextFormat.Accuracy.kAccuracyNone              : Program.DumpStream.WriteLine("1 digit\n" ); break;

          case OdDgDimTextFormat.Accuracy.kDecimal1              : Program.DumpStream.WriteLine("Decimal, 2 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal2             : Program.DumpStream.WriteLine("Decimal, 3 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal3       : Program.DumpStream.WriteLine("Decimal, 4 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal4       : Program.DumpStream.WriteLine("Decimal, 5 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal5       : Program.DumpStream.WriteLine("Decimal, 6 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal6       : Program.DumpStream.WriteLine("Decimal, 7 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal7       : Program.DumpStream.WriteLine("Decimal, 8 digit\n" ); break;
          case OdDgDimTextFormat.Accuracy.kDecimal8       : Program.DumpStream.WriteLine("Decimal, 9 digit\n" ); break;

          case OdDgDimTextFormat.Accuracy.kFractional2          : Program.DumpStream.WriteLine("Fractional, 2-th\n" ); break;
          case OdDgDimTextFormat.Accuracy.kFractional4          : Program.DumpStream.WriteLine("Fractional, 4-th\n" ); break;
          case OdDgDimTextFormat.Accuracy.kFractional8          : Program.DumpStream.WriteLine("Fractional, 8-th\n" ); break;
          case OdDgDimTextFormat.Accuracy.kFractional16         : Program.DumpStream.WriteLine("Fractional, 16-th\n" ); break;
          case OdDgDimTextFormat.Accuracy.kFractional32         : Program.DumpStream.WriteLine("Fractional, 32-th\n" ); break;
          case OdDgDimTextFormat.Accuracy.kFractional64         : Program.DumpStream.WriteLine("Fractional, 64-th\n" ); break;

          case OdDgDimTextFormat.Accuracy.kExponential1 : Program.DumpStream.WriteLine("Exponential, 1 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential2 : Program.DumpStream.WriteLine("Exponential, 2 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential3 : Program.DumpStream.WriteLine("Exponential, 3 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential4 : Program.DumpStream.WriteLine("Exponential, 4 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential5 : Program.DumpStream.WriteLine("Exponential, 5 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential6 : Program.DumpStream.WriteLine("Exponential, 6 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential7 : Program.DumpStream.WriteLine("Exponential, 7 digit for mantissa\n" ); break;
          case OdDgDimTextFormat.Accuracy.kExponential8 : Program.DumpStream.WriteLine("Exponential, 8 digit for mantissa\n" ); break;
          }
        }


        public void writeFieldValue( String name, OdDgDimGeometry value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "  Witness line offset", value.getWitnessLineOffset() );
          writeFieldValue( "  Witness line extend", value.getWitnessLineExtend() );
          writeFieldValue( "  Text lift", value.getTextLift() );
          writeFieldValue( "  Text margin", value.getTextMargin() );
          writeFieldValue( "  Terminator width", value.getTerminatorWidth() );
          writeFieldValue( "  Terminator height", value.getTerminatorHeight() );
          writeFieldValue( "  Stack offset", value.getStackOffset() );
          writeFieldValue( "  Center size", value.getCenterSize() );

          if( value.getUseMargin() )
            writeFieldValue( "  Min leader", value.getMargin() );
          else
            writeFieldValue( "  Min leader", value.getTerminatorWidth()*2.0 );
          
          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgDimOption value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeShift();
          if(null != value)
          {
            switch( value.getType() )
            {
              case OdDgDimOption.Type.kNone                             : 
              {
                writeFieldValue( "  Type", (("kNone")) ); 
              } break;

              case OdDgDimOption.Type.kTolerance                        :
              {
                  OdDgDimOptionTolerance pTolerOptions = (OdDgDimOptionTolerance)value;
                writeFieldValue("", pTolerOptions);
              } break;

              case OdDgDimOption.Type.kTerminators                      :
              {
                  OdDgDimOptionTerminators pTermOptions = (OdDgDimOptionTerminators)value;
                writeFieldValue("", pTermOptions);
              } break;

              case OdDgDimOption.Type.kPrefixSymbol                     :
              {
                  OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
                writeFieldValue("  Type", (("kPrefixSymbol")) );
                writeFieldValue("", pSymbolOptions );
              } break;

              case OdDgDimOption.Type.kSuffixSymbol                     :
              {
                  OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
                writeFieldValue("  Type", (("kSuffixSymbol")) );
                writeFieldValue("", pSymbolOptions );
              } break;

              case OdDgDimOption.Type.kDiameterSymbol                   :
              {
                  OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
                writeFieldValue("  Type", (("kDiameterSymbol")) );
                writeFieldValue("", pSymbolOptions );
              } break;

              case OdDgDimOption.Type.kPrefixSuffix                     :
              {
                  OdDgDimOptionPrefixSuffix pPrefixSuffixOptions = (OdDgDimOptionPrefixSuffix)value;
                writeFieldValue("", pPrefixSuffixOptions);
              } break;

              case OdDgDimOption.Type.kPrimaryUnits                     :
              {
                  OdDgDimOptionUnits pUnitsOptions = (OdDgDimOptionUnits)value;
                writeFieldValue("  Type", (("kPrimaryUnits")) );
                writeFieldValue("", pUnitsOptions );
              } break;

              case OdDgDimOption.Type.kSecondaryUnits                   :
              {
                  OdDgDimOptionUnits pUnitsOptions = (OdDgDimOptionUnits)value;
                writeFieldValue("  Type", (("kSecondaryUnits")) );
                writeFieldValue("", pUnitsOptions );
              } break;

              case OdDgDimOption.Type.kTerminatorSymbology              :
              {
                  OdDgDimOptionTerminatorSymbology pTermSymbolOptions = (OdDgDimOptionTerminatorSymbology)value;
                writeFieldValue("", pTermSymbolOptions );
              } break;

              case OdDgDimOption.Type.kView                             :
              {
                  OdDgDimOptionView pViewOptions = (OdDgDimOptionView)value;
                writeFieldValue("  Type", (("kView")) );

                if( null != pViewOptions )
                  writeFieldValue("  Rotation", pViewOptions.getQuaternion() );
              } break;

              case OdDgDimOption.Type.kAlternatePrimaryUnits            :
              {
                  OdDgDimOptionAltFormat pAltOptions = (OdDgDimOptionAltFormat)value;
                writeFieldValue("  Type", (("kAlternativePrimaryUnits")) );
                writeFieldValue("", pAltOptions );
              } break;

              case OdDgDimOption.Type.kOffset                           :
              {
                  OdDgDimOptionOffset pOffsetOptions = (OdDgDimOptionOffset)value;
                writeFieldValue("", pOffsetOptions );
              } break;

              case OdDgDimOption.Type.kAlternateSecondaryUnits          :
              {
                  OdDgDimOptionAltFormat pAltOptions = (OdDgDimOptionAltFormat)value;
                writeFieldValue("  Type", (("kAlternativeSecondaryUnits")) );
                writeFieldValue("", pAltOptions );
              } break;

              case OdDgDimOption.Type.kAlternatePrefixSymbol            :
              {
                  OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
                writeFieldValue("  Type", (("kAlternatePrefixSymbol")) );
                writeFieldValue("", pSymbolOptions );
              } break;

              case OdDgDimOption.Type.kAlternateSuffixSymbol            :
              {
                  OdDgDimOptionSymbol pSymbolOptions = (OdDgDimOptionSymbol)value;
                writeFieldValue("  Type", (("kAlternateSuffixSymbol")) );
                writeFieldValue("", pSymbolOptions );
              } break;

              case OdDgDimOption.Type.kProxyCell                        :
              {
                  OdDgDimOptionProxyCell pCellOptions = (OdDgDimOptionProxyCell)value;
                writeFieldValue("", pCellOptions );
              } break;
            }
          }
          else
          {
            writeFieldValue( "  Type", (("[value unknown]")) );
          }

          m_nesting--;
        }

        public void writeFieldValue( String name, OdDgDimLabelLine.LabelLineDimensionMode value )
        {
          String val;

          switch( value )
          {
          case OdDgDimLabelLine.LabelLineDimensionMode.kAngleLength :
            val = ("Angle/Length"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAbove :
            val = ("Length above"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kAngleAbove :
            val = ("Angle above"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kLengthBelow :
            val = ("Length below"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kAngleBelow :
            val = ("Angle below"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAngleAbove :
            val = ("Length Angle above"); break;
          case OdDgDimLabelLine.LabelLineDimensionMode.kLengthAngleBelow :
            val = ("Length Angle below"); break;
          default :
            val = ("Length/Angle"); break;
          }

          writeFieldValue( name, val );
        }

        public void writeFieldValue( String name, OdDgDimTextInfo.FitOptions value )
        {
          String strVal = String.Empty;

          switch( value )
          {
          case OdDgDimTextInfo.FitOptions.kTermMovesFirst: strVal = "kTermMovesFirst"; break;
          case OdDgDimTextInfo.FitOptions.kTermReversed: strVal = "kTermReversed"; break;
          case OdDgDimTextInfo.FitOptions.kTermInside: strVal = "kTermInside"; break;
          case OdDgDimTextInfo.FitOptions.kTermOutside: strVal = "kTermOutside"; break;
          case OdDgDimTextInfo.FitOptions.kTextInside: strVal = "kTextInside"; break;
          case OdDgDimTextInfo.FitOptions.kTextMovesFirst: strVal = "kTextMovesFirst"; break;
          case OdDgDimTextInfo.FitOptions.kBothMoves: strVal = "kBothMoves"; break;
          case OdDgDimTextInfo.FitOptions.kSmallestMoves: strVal = "kSmallestMoves"; break;
          }

          writeFieldValue( name, strVal );
        }

        public void writeFieldValue( String name, OdDgDimTextInfo.TextLocation value )
        {
          String strVal;

          switch( value )
          {
          case OdDgDimTextInfo.TextLocation.kTextInline: strVal = "kTextInline"; break;
          case OdDgDimTextInfo.TextLocation.kTextAbove: strVal  = "kTextAbove"; break;
          case OdDgDimTextInfo.TextLocation.kTextOutside: strVal = "kTextOutside"; break;
          default: strVal = "kTextTopLeft"; break;
          }

          writeFieldValue( name, strVal );
        }

        public void writeFieldValue( String Val, OdDgDimOptionTolerance value )
        {
          writeFieldValue("  Type", (("kTolerance")) );

          if( null == value )
            return;

          writeFieldValue("  Upper value", value.getToleranceUpper() );
          writeFieldValue("  Lower value", value.getToleranceLower() );
          writeFieldValue("  Stack if equal", value.getStackEqualFlag() );
          writeFieldValue("  Show sign for zero", value.getShowSignForZeroFlag() );
          writeFieldValue("  Left margin", value.getToleranceHorizSep() );
          writeFieldValue("  Separator margin", value.getToleranceVertSep() );
          writeFieldValue("  Font entry Id", value.getFontEntryId() );
          writeFieldValue("  Text Width", value.getToleranceTextWidth() );
          writeFieldValue("  Text Height", value.getToleranceTextHeight() );

          if (0 != value.getTolerancePlusMinusSymbol())
            writeFieldValue("  Plus/Minus symbol", value.getTolerancePlusMinusSymbol() );

          if (0 != value.getTolerancePrefixSymbol())
            writeFieldValue("  Prefix symbol", value.getTolerancePrefixSymbol() );

          if (0 != value.getToleranceSuffixSymbol())
            writeFieldValue("  Suffix symbol", value.getToleranceSuffixSymbol() );

          writeFieldValue("  Stack align", value.getStackAlign() );
        }

        public void writeFieldValue( String Val, OdDgDimOptionTerminators value )
        {
          writeFieldValue("  Type", (("kTerminators")) );

          if( null == value )
            return;

          if (value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault)
          {
            writeFieldValue("  Arrow style", (("kTermDefault")) );
          }
          else if( value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol )
          {
            writeFieldValue("  Arrow style", (("kTermSymbol")) );
            writeFieldValue("  Arrow Font entry Id", value.getArrowFontID() );
            writeFieldValue("  Arrow Symbol code", value.getArrowSymbol() );
          }
          else if( value.getArrowTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell )
          {
            writeFieldValue("  Arrow style", (("kTermCell")) );
            writeFieldValue("  Arrow Cell Id", value.getArrowCellID() );
          }
          else
          {
            writeFieldValue("  Arrow style", (("kTermScaledCell")) );
            writeFieldValue("  Arrow Cell Id", value.getArrowCellID() );
            writeFieldValue("  Arrow Cell scale", value.getSharedCellScale() );
          }

          if( value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault )
          {
            writeFieldValue("  Stroke style", (("kTermDefault")) );
          }
          else if( value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol )
          {
            writeFieldValue("  Stroke style", (("kTermSymbol")) );
            writeFieldValue("  Stroke Font entry Id", value.getStrokeFontID() );
            writeFieldValue("  Stroke Symbol code", value.getStrokeSymbol() );
          }
          else if( value.getStrokeTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell )
          {
            writeFieldValue("  Stroke style", (("kTermCell")) );
            writeFieldValue("  Stroke Cell Id", value.getStrokeCellID() );
          }
          else
          {
            writeFieldValue("  Stroke style", (("kTermScaledCell")) );
            writeFieldValue("  Stroke Cell Id", value.getStrokeCellID() );
            writeFieldValue("  Stroke Cell scale", value.getSharedCellScale() );
          }

          if( value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault )
          {
            writeFieldValue("  Origin style", (("kTermDefault")) );
          }
          else if( value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol )
          {
            writeFieldValue("  Origin style", (("kTermSymbol")) );
            writeFieldValue("  Origin Font entry Id", value.getOriginFontID() );
            writeFieldValue("  Origin Symbol code", value.getOriginSymbol() );
          }
          else if( value.getOriginTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell )
          {
            writeFieldValue("  Origin style", (("kTermCell")) );
            writeFieldValue("  Origin Cell Id", value.getOriginCellID() );
          }
          else
          {
            writeFieldValue("  Origin style", (("kTermScaledCell")) );
            writeFieldValue("  Origin Cell Id", value.getOriginCellID() );
            writeFieldValue("  Origin Cell scale", value.getSharedCellScale() );
          }

          if( value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermDefault )
          {
            writeFieldValue("  Dot style", (("kTermDefault")) );
          }
          else if( value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermSymbol )
          {
            writeFieldValue("  Dot style", (("kTermSymbol")) );
            writeFieldValue("  Dot Font entry Id", value.getDotFontID() );
            writeFieldValue("  Dot Symbol code", value.getDotSymbol() );
          }
          else if( value.getDotTermStyle() == OdDgDimOptionTerminators.TerminatorStyle.kTermCell )
          {
            writeFieldValue("  Dot style", (("kTermCell")) );
            writeFieldValue("  Dot Cell Id", value.getDotCellID() );
          }
          else
          {
            writeFieldValue("  Dot style", (("kTermScaledCell")) );
            writeFieldValue("  Dot Cell Id", value.getDotCellID() );
            writeFieldValue("  Dot Cell scale", value.getSharedCellScale() );
          }
        }

        public void writeFieldValue( String Val, OdDgDimOptionTerminatorSymbology value )
        {
          writeFieldValue("  Type", "kTerminatorSymbology" );

          if( null == value )
            return;

          writeFieldValue("  Use Line type", value.getStyleFlag());
          writeFieldValue("  Use Line weight", value.getWeightFlag());
          writeFieldValue("  Use Color", value.getColorFlag());

          if( value.getStyleFlag() )
            writeFieldValue("  Line Type entry Id", value.getStyle());

          if( value.getWeightFlag() )
            writeFieldValue("  Line Weight", value.getWeight());

          if( value.getColorFlag() )
            writeFieldValue("  Color", value.getColor());
        }

        public void writeFieldValue( String Val, OdDgDimOptionSymbol value )
        {
          if( null == value )
            return;

          writeFieldValue("  Font entry Id", value.getSymbolFont() );
          writeFieldValue("  Symbol code", value.getSymbolChar() );
        }

        public void writeFieldValue( String Val, OdDgDimOptionPrefixSuffix value )
        {
          writeFieldValue("  Type", "kPrefixSuffix");

          if( null == value )
            return;

          if (0 != value.getMainPrefix())
          {
            writeFieldValue("  Main prefix", value.getMainPrefix() );
          }

          if (0 != value.getMainSuffix())
          {
            writeFieldValue("  Main suffix", value.getMainSuffix() );
          }

          if (0 != value.getUpperPrefix())
          {
            writeFieldValue("  Upper prefix", value.getUpperPrefix() );
          }

          if (0 != value.getUpperSuffix())
          {
            writeFieldValue("  Upper suffix", value.getUpperSuffix() );
          }

          if (0 != value.getLowerPrefix())
          {
            writeFieldValue("  Lower prefix", value.getLowerPrefix() );
          }

          if( 0 != value.getLowerSuffix() )
          {
            writeFieldValue("  Lower suffix", value.getLowerSuffix() );
          }
        }

        public void writeFieldValue( String name, OdDgModel.UnitBase value )
        {
          if( value == OdDgModel.UnitBase.kNone )
          {
            writeFieldValue( name, (("kNone")) );
          }
          else if( value == OdDgModel.UnitBase.kMeter )
          {
            writeFieldValue( name, (("kMeter")) );
          }
          else
          {
            writeFieldValue( name, (("Unknown")) );
          }
        }

        public void writeFieldValue( String name, OdDgModel.UnitSystem value )
        {
          if( value == OdDgModel.UnitSystem.kCustom )
          {
            writeFieldValue( name, (("kCustom")) );
          }
          else if( value == OdDgModel.UnitSystem.kMetric )
          {
            writeFieldValue( name, (("kMetric")) );
          }
          else if( value == OdDgModel.UnitSystem.kEnglish )
          {
            writeFieldValue( name, (("kEnglish")) );
          }
          else
          {
            writeFieldValue( name, (("Unknown")) );
          }
        }

        public void writeFieldValue( String Val, OdDgDimOptionUnits value )
        {
          if( null == value )
            return;

          //SWIGTYPE_p_OdDgModel__UnitDescription descr;
          OdDgModel.UnitDescription descr = new OdDgModel.UnitDescription();
          value.getMasterUnit( descr );
          writeFieldName( "  Master units:" );
          writeFieldValue( "    Unit base", descr.m_base );
          writeFieldValue( "    Unit system", descr.m_system );
          writeFieldValue( "    Denominator", descr.m_denominator );
          writeFieldValue( "    Numerator", descr.m_numerator );
          writeFieldValue( "    Name", descr.m_name );
          value.getSubUnit( descr );
          writeFieldName( "  Sub units:" );
          writeFieldValue( "    Unit base", descr.m_base );
          writeFieldValue( "    Unit system", descr.m_system );
          writeFieldValue( "    Denominator", descr.m_denominator );
          writeFieldValue( "    Numerator", descr.m_numerator );
          writeFieldValue( "    Name", descr.m_name );
        }

        public void writeFieldValue( String Val, OdDgDimOptionAltFormat value )
        {
          if( null == value )
            return;

          writeFieldValue("  Accuracy", value.getAccuracy() );
          writeFieldValue("  Show sub units", value.getSubUnits() );
          writeFieldValue("  Show unit labels", value.getLabel() );
          writeFieldValue("  Show delimiter", value.getDelimiter() );
          writeFieldValue("  Show sub units only", value.getNoMasterUnits() );
          writeFieldValue("  Allow zero master units", value.getAllowZeroMasterUnits() );

          if( value.getMoreThanThreshold() )
          {
            if( value.getEqualToThreshold() )
            {
              writeFieldValue("  Condition", ((">=")) );
            }
            else
            {
              writeFieldValue("  Condition", ((">")) );
            }
          }
          else
          {
            if( value.getEqualToThreshold() )
            {
              writeFieldValue("  Condition", (("<=")) );
            }
            else
            {
              writeFieldValue("  Condition", (("<")) );
            }
          }

          writeFieldValue("  Threshold", value.getThreshold() );
        }

        public void writeFieldValue( String name, OdDgDimOptionOffset.ChainType value )
        {
          String strValue = String.Empty;

          switch( value )
          {
            case OdDgDimOptionOffset.ChainType.kNone:
            {
              strValue = "kNone";
            } break;

            case OdDgDimOptionOffset.ChainType.kLine:
            {
              strValue = "kLine";
            } break;

            case OdDgDimOptionOffset.ChainType.kArc:
            {
              strValue = "kArc";
            } break;

            case OdDgDimOptionOffset.ChainType.kBSpline:
            {
              strValue = "kBSpline";
            } break;
          }

          writeFieldValue(name, strValue);
        }

        public void writeFieldValue( String name, OdDgDimOptionOffset.LeaderAlignment value )
        {
          String strValue = String.Empty;

          switch( value )
          {
            case OdDgDimOptionOffset.LeaderAlignment.kAutoAlignment:
            {
              strValue = ("kAutoAlignment");
            } break;

            case OdDgDimOptionOffset.LeaderAlignment.kLeftAlignment:
            {
              strValue = ("kLeftAlignment");
            } break;

            case OdDgDimOptionOffset.LeaderAlignment.kRightAlignment:
            {
              strValue = ("kRightAlignment");
            } break;
          }

          writeFieldValue(name, strValue);
        }


        public void writeFieldValue( String Val, OdDgDimOptionOffset value )
        {
          writeFieldValue("  Type", (("kOffset")) );

          if( null == value )
            return;

          writeFieldValue("  Terminator", value.getTerminator() );
          writeFieldValue("  Chain type", value.getChainType() );
          writeFieldValue("  Elbow", value.getElbowFlag() );
          writeFieldValue("  Alignment", value.getAlignment() );
          writeFieldValue("  No dock on dim line", value.getNoDockOnDimLineFlag() );
        }

        public void writeFieldValue( String Val, OdDgDimOptionProxyCell value )
        {
          writeFieldValue("  Type", (("kProxyCell")) );

          if( null == value )
            return;

          writeFieldValue("  Origin", value.getOrigin() );
          writeFieldValue("  Rotation Matrix", value.getRotScale() );
          writeFieldValue("  Check Sum", value.getCheckSum() );
        }


        public void writeFieldValue( String name, OdDgMultilineSymbology value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "Style", value.getLineStyleEntryId() );
          writeFieldValue( "Weight", value.getLineWeight() );
          writeFieldValue( "Color", value.getColorIndex() );
          writeFieldValue( "Use style", value.getUseStyleFlag() );
          writeFieldValue( "Use weight", value.getUseWeightFlag() );
          writeFieldValue( "Use color", value.getUseColorFlag() );
          writeFieldValue( "Use class", value.getUseClassFlag() );
          writeFieldValue( "Inside arc", value.getCapInArcFlag() );
          writeFieldValue( "Outside arc", value.getCapOutArcFlag() );
          writeFieldValue( "Cap line", value.getCapLineFlag() );
          writeFieldValue( "Custom style", value.getCustomStyleFlag() );
          writeFieldValue( "Cap color from segment", value.getCapColorFromSegmentFlag() );
          writeFieldValue( "Construction class", value.getConstructionClassFlag() );

          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgMultilinePoint value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          {
            OdGePoint3d point = new OdGePoint3d();

            value.getPoint( point );
            writeFieldValue( "Point", point );
          }

          {
            UInt32            i, j = value.getBreaksCount();
            OdDgMultilineBreak  break_ = new OdDgMultilineBreak();
            String                fieldName = String.Empty;

            writeShift();
            Program.DumpStream.WriteLine(string.Format("Number of breaks: {0}\n", j  ));

            for( i = 0; i < j; i++ )
            {
                //Program.DumpStream.WriteLine(string.Format("{0} Break {1}", fieldName, i));
              //sprintf( fieldName, "Break %d", i );
                fieldName = string.Format("Break {0}", i);
              value.getBreak( i, break_ );
              writeFieldValue( fieldName, break_ );
            }
          }

          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgMultilineBreak value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "Lines mask", value.getLinesMask() );
          writeFieldValue( "Offset", value.getOffset() );
          writeFieldValue( "Length", value.getLength() );

          {
            String flagValue = String.Empty;

            switch( value.getFlags() )
            {
            case OdDgMultilineBreak.Flags.kStandardByDistance  : flagValue = "Standard by distance"; break;
            case OdDgMultilineBreak.Flags.kFromJoint           : flagValue = "from joint"; break;
            case OdDgMultilineBreak.Flags.kToJoint: flagValue = "to joing"; break;
            }

            writeFieldValue( "Flag", flagValue );
          }

          m_nesting--;
        }


        public void writeFieldValue( String name, OdDgMultilineProfile value )
        {
          writeShift();
          Program.DumpStream.WriteLine(name );

          m_nesting++;

          writeFieldValue( "Distance", value.getDistance() );

          {
            OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

            value.getSymbology( symbology );
            writeFieldValue( "Symbology", symbology );
          }

          m_nesting--;
        }


        public void writeFieldValue( String name, LineSpacingType value )
        {
          writeFieldName( name );

          switch( value )
          {
          case LineSpacingType.kExact      : Program.DumpStream.WriteLine("Exact\n" ); break;
          case LineSpacingType.kAutomatic  : Program.DumpStream.WriteLine("Automatic\n" ); break;
          case LineSpacingType.kFromLineTop: Program.DumpStream.WriteLine("FromLineTop\n" ); break;
          case LineSpacingType.kAtLeast    : Program.DumpStream.WriteLine("AtLeast\n" ); break;
          }
        }

        public void writeFieldValue( String name, OdDgRaster.RasterFormat value )
        {
          writeFieldName( name );

          switch( value )
          {
          case OdDgRaster.RasterFormat.kBitmap    : Program.DumpStream.WriteLine("Bitmap raster\n" ); break;
          case OdDgRaster.RasterFormat.kByteData  : Program.DumpStream.WriteLine("Byte data raster\n" ); break;
          case OdDgRaster.RasterFormat.kBinaryRLE : Program.DumpStream.WriteLine("Binary RLE raster\n" ); break;
          case OdDgRaster.RasterFormat.kByteRLE   : Program.DumpStream.WriteLine("Byte RLE raster\n" ); break;
          }
        }

        public void writeFieldValue( String name, OdDgTagDefinition.Type value )
        {
          writeFieldName( name );

          switch( value )
          {
          case OdDgTagDefinition.Type.kChar    : Program.DumpStream.WriteLine("Char\n" ); break;
          case OdDgTagDefinition.Type.kInt16    : Program.DumpStream.WriteLine("Short int\n" ); break;
          case OdDgTagDefinition.Type.kInt32    : Program.DumpStream.WriteLine("Long int\n" ); break;
          case OdDgTagDefinition.Type.kDouble  : Program.DumpStream.WriteLine("Double\n" ); break;
          case OdDgTagDefinition.Type.kBinary  : Program.DumpStream.WriteLine("Binary\n" ); break;
          }
        }


        public void writeFieldValue( String name, TextDirection value )
        {
          writeFieldName( name );

          switch( value )
          {
          case TextDirection.kHorizontal  : Program.DumpStream.WriteLine("Horizontal\n" ); break;
          case TextDirection.kVertical    : Program.DumpStream.WriteLine("Vertical\n" ); break;
          case TextDirection.kJapanese    : Program.DumpStream.WriteLine("Japanese\n" ); break;
          case TextDirection.kRightToLeft : Program.DumpStream.WriteLine("Right-to-left\n" ); break;
          }
        }


        public void writeFieldValue( String name, OdGeMatrix3d value )
        {
          writeFieldName( name );

          Program.DumpStream.WriteLine(string.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}; {8}\n",
                    value.GetItem(0, 0), value.GetItem(1, 0), value.GetItem(2, 0),
                    value.GetItem(0, 1), value.GetItem(1, 1), value.GetItem(2, 1),
                    value.GetItem(0, 2), value.GetItem(1, 2), value.GetItem(2, 2)));
        }


        public void writeFieldValue( String name, OdGsDCRect value )
        {
          writeFieldName( name );

          Program.DumpStream.WriteLine(string.Format("( {0:X}; {1:X} ) - ( {2:X}; {3:X} )\n", value.m_min.x, value.m_min.y, value.m_max.x, value.m_max.y));
        }


        public void writeFieldValue( String name, OdDgElementId value )
        {
          writeFieldName( name );

          Program.DumpStream.WriteLine(string.Format("{0:X}\n", value.getHandle()) );
        }

        public void writeFieldValue( String name, Object val )
        {
          switch (val.GetType().ToString())
          {
            case "System.Bool":
            writeFieldValue( name, (bool)val );
            break;
          case "System.Byte":
            writeFieldValue( name, (byte)val );
            break;
          case "System.Int16":
            writeFieldValue( name, (System.Int16)val );
            break;
          case "System.Int32":
            writeFieldValue( name, (Int32)val);
            break;
          case "System.Int64":
            writeFieldValue(name, (Int64)val);
            break;
          case "System.Double":
            writeFieldValue(name, (Double)val);
            break;
          case "System.String":
            writeFieldValue(name, (System.String)val);
            break;
          }
        }

        public void writeFieldValue( String name, OdGeExtents2d value )
        {
          writeFieldName( name );

          OdGePoint2d min = value.minPoint(), max = value.maxPoint();

          Program.DumpStream.WriteLine(string.Format("Low point: {0}; {1};  High point: {2}; {3}\n",
            min.x, min.y,
            max.x, max.y
            ));
        }

        public void writeFieldValue( String name, OdGeExtents3d value )
        {
          writeFieldName( name );

          OdGePoint3d min = value.minPoint(), max = value.maxPoint();

          Program.DumpStream.WriteLine("Low point: {0}; {1}; {2};  High point: {3}; {4}; {5}\n",
            min.x, min.y, min.z,
            max.x, max.y, max.z
            );
        }

        public void writeFieldName( String fieldName )
        {
          writeShift();
          Program.DumpStream.WriteLine(fieldName );
        }

        public void writeFieldValue( String name, OdDgDimTool.TerminatorType iType )
        {
          switch( iType )
          {
            case OdDgDimTool.TerminatorType.kTtNone:
            {
              writeFieldValue(name, (("kTtNone")) );
            } break;

            case OdDgDimTool.TerminatorType.kTtArrow:
            {
              writeFieldValue(name, (("kTtArrow")) );
            } break;

            case OdDgDimTool.TerminatorType.kTtStroke:
            {
              writeFieldValue(name, (("kTtStroke")) );
            } break;

            case OdDgDimTool.TerminatorType.kTtCircle:
            {
              writeFieldValue(name, (("kTtCircle")) );
            } break;

            case OdDgDimTool.TerminatorType.kTtFilledCircle:
            {
              writeFieldValue(name, (("kTtFilledCircle")) );
            } break;
          }
        }

        public void writeFieldValue( String name, OdDgDimTool.TextType iType )
        {
          switch( iType )
          {
            case OdDgDimTool.TextType.kStandard:
            {
              writeFieldValue(name, (("kStandard")) );
            } break;

            case OdDgDimTool.TextType.kVertical:
            {
              writeFieldValue(name, (("kVertical")) );
            } break;

            case OdDgDimTool.TextType.kMixed:
            {
              writeFieldValue(name, (("kMixed")) );
            } break;
          }
        }

        public void writeFieldValue( String name, OdDgDimTool.CustomSymbol iSymbol )
        {
          switch( iSymbol )
          {
            case OdDgDimTool.CustomSymbol.kCsNone:
            {
              writeFieldValue(name, (("kCsNone")) );
            } break;

            case OdDgDimTool.CustomSymbol.kCsDiameter:
            {
              writeFieldValue(name, (("kCsDiameter")) );
            } break;

            case OdDgDimTool.CustomSymbol.kCsRadius:
            {
              writeFieldValue(name, (("kCsRadius")) );
            } break;

            case OdDgDimTool.CustomSymbol.kCsSquare:
            {
              writeFieldValue(name, (("kCsSquare")) );
            } break;

            case OdDgDimTool.CustomSymbol.kCsSR:
            {
              writeFieldValue(name, (("kCsSR")) );
            } break;

            case OdDgDimTool.CustomSymbol.kCsSDiameter:
            {
              writeFieldValue(name, (("kCsSDiameter")) );
            } break;
          }
        }

        public void writeFieldValue( String name, OdDgDimTool.Leader iLeader )
        {
          switch( iLeader )
          {
            case OdDgDimTool.Leader.kRadius:
            {
              writeFieldValue(name, (("kRadius")) );
            } break;

            case OdDgDimTool.Leader.kRadiusExt1:
            {
              writeFieldValue(name, (("kRadiusExt1")) );
            } break;

            case OdDgDimTool.Leader.kRadiusExt2:
            {
              writeFieldValue(name, (("kRadiusExt2")) );
            } break;

            case OdDgDimTool.Leader.kDiameter:
            {
              writeFieldValue(name, (("kDiameter")) );
            } break;
          }
        }

        public void  writeFieldValue( String name, OdDgDimension pElement )
        {
          switch( pElement.getDimensionType() )
          {
            case OdDgDimension.ToolType.kToolTypeSizeArrow:
            {
                OdDgDimSizeArrow pDimSize = (OdDgDimSizeArrow)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeSizeStroke:
            {
                OdDgDimSizeStroke pDimSize = (OdDgDimSizeStroke)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeLocateSingle:
            {
                OdDgDimSingleLocation pDimSize = (OdDgDimSingleLocation)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeLocateStacked:
            {
                OdDgDimStackedLocation pDimSize = (OdDgDimStackedLocation)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeCustomLinear:
            {
                OdDgDimCustomLinear pDimSize = (OdDgDimCustomLinear)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeAngleSize:
            {
                OdDgDimAngleSize pDimSize = (OdDgDimAngleSize)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeAngleLines:
            {
                OdDgDimAngleLines pDimSize = (OdDgDimAngleLines)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeAngleLocation:
            {
                OdDgDimAngleLocation pDimSize = (OdDgDimAngleLocation)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeArcLocation:
            {
                OdDgDimArcLocation pDimSize = (OdDgDimArcLocation)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeAngleAxisX:
            {
                OdDgDimAngleAxisX pDimSize = (OdDgDimAngleAxisX)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeAngleAxisY:
            {
                OdDgDimAngleAxisY pDimSize = (OdDgDimAngleAxisY)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeArcSize:
            {
                OdDgDimArcSize pDimSize = (OdDgDimArcSize)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeRadius:
            {
                OdDgDimRadius pDimSize = (OdDgDimRadius)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeRadiusExtended:
            {
                OdDgDimRadiusExtended pDimSize = (OdDgDimRadiusExtended)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeDiameter:
            {
                OdDgDimDiameter pDimSize = (OdDgDimDiameter)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeDiameterExtended:
            {
                OdDgDimDiameterExtended pDimSize = (OdDgDimDiameterExtended)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeDiameterPara:
            {
                OdDgDimDiameterParallel pDimSize = (OdDgDimDiameterParallel)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeDiameterPerp:
            {
                OdDgDimDiameterPerpendicular pDimSize = (OdDgDimDiameterPerpendicular)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeOrdinate:
            {
                OdDgDimOrdinate pDimSize = (OdDgDimOrdinate)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;

            case OdDgDimension.ToolType.kToolTypeCenter:
            {
                OdDgDimCenter pDimSize = (OdDgDimCenter)pElement;

              if( null != pDimSize )
                writeFieldValue( name, pDimSize );
            } break;
          }
        }

        public void writeFieldValue(String name, OdDgDimSizeArrow pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  JointTerminator", pElement.getJointTerminator());
            writeFieldValue("  TextType", pElement.getTextType());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimSizeStroke pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  JointTerminator", pElement.getJointTerminator());
            writeFieldValue("  TextType", pElement.getTextType());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimSingleLocation pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  JointTerminator", pElement.getJointTerminator());
            writeFieldValue("  TextType", pElement.getTextType());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimStackedLocation pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  JointTerminator", pElement.getJointTerminator());
            writeFieldValue("  TextType", pElement.getTextType());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimCustomLinear pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  JointTerminator", pElement.getJointTerminator());
            writeFieldValue("  TextType", pElement.getTextType());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimAngleSize pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimAngleLines pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimDiameterParallel pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimDiameterPerpendicular pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimAngleLocation pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimArcLocation pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimAngleAxisX pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }
        public void writeFieldValue(String name, OdDgDimAngleAxisY pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag());
            writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  FirstTerminator", pElement.getFirstTerminator());
            writeFieldValue("  LeftTerminator", pElement.getLeftTerminator());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
        }

        public void writeFieldValue(String name, OdDgDimRadius pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
            writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
            writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
            writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
            writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
            writeFieldValue("  Leader", pElement.getLeader());
        }
        public void writeFieldValue(String name, OdDgDimRadiusExtended pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
            writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
            writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
            writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
            writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
            writeFieldValue("  Leader", pElement.getLeader());
        }
        public void writeFieldValue(String name, OdDgDimDiameter pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
            writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
            writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
            writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
            writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
            writeFieldValue("  Leader", pElement.getLeader());
        }
        public void writeFieldValue(String name, OdDgDimDiameterExtended pElement)
        {
            writeFieldName(name);
            writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag());
            writeFieldValue("  CenterMarkFlag", pElement.getCenterMarkFlag());
            writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag());
            writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag());
            writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag());
            writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag());
            writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag());
            writeFieldValue("  RightTerminator", pElement.getRightTerminator());
            writeFieldValue("  Prefix", pElement.getPrefix());
            writeFieldValue("  Suffix", pElement.getSuffix());
            writeFieldValue("  Leader", pElement.getLeader());
        }

        public void writeFieldValue( String name, OdDgDimArcSize pElement ) 
        { 
          writeFieldName( name );
          writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag() );
          writeFieldValue("  LeftExtLinesPresentFlag", pElement.getLeftExtLinesPresentFlag() );
          writeFieldValue("  RightExtLinesPresentFlag", pElement.getRightExtLinesPresentFlag() );
          writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag() );
          writeFieldValue("  ChordAlignFlag", pElement.getChordAlignFlag() );
          writeFieldValue("  LeftTerminator", pElement.getLeftTerminator() );
          writeFieldValue("  RightTerminator", pElement.getRightTerminator() );
          writeFieldValue("  Prefix", pElement.getPrefix() );
          writeFieldValue("  Suffix", pElement.getSuffix() );
        }

        public void writeFieldValue( String name, OdDgDimOrdinate pElement ) 
        { 
          writeFieldName( name ); 
          writeFieldValue("  StackExtLinesFlag", pElement.getStackExtLinesFlag() );   
          writeFieldValue("  ArcSymbolFlag", pElement.getArcSymbolFlag() );     
          writeFieldValue("  DecrementInReverseDirectionFlag", pElement.getDecrementInReverseDirectionFlag() );  
          writeFieldValue("  FreeLocationOfTxtFlag", pElement.getFreeLocationOfTxtFlag() );    
          writeFieldValue("  Datum value", pElement.getDatumValue() ); 
        }

        public void writeFieldValue( String name, OdDgDimCenter pElement ) 
        { 
          writeFieldName( name );
          writeFieldValue("  CenterMarkLeftExtendFlag", pElement.getCenterMarkLeftExtendFlag() );
          writeFieldValue("  CenterMarkRightExtendFlag", pElement.getCenterMarkRightExtendFlag() );
          writeFieldValue("  CenterMarkTopExtendFlag", pElement.getCenterMarkTopExtendFlag() );
          writeFieldValue("  CenterMarkBottomExtendFlag", pElement.getCenterMarkBottomExtendFlag() );
        }

        public void writeFieldValue( String  name, OdDgSurface.Type value )
        {
          switch( value )
          {
          case OdDgSurface.Type.kSurfaceProjection              : Program.DumpStream.WriteLine("Projection" ); break;
          case OdDgSurface.Type.kSurfaceBoundedPlane            : Program.DumpStream.WriteLine("Bounded plane" ); break;
          case OdDgSurface.Type.kSurfaceUnboundedPlane          : Program.DumpStream.WriteLine("Unbounded plane" ); break;
          case OdDgSurface.Type.kSurfaceRight_CIRCULAR_Cylinder : Program.DumpStream.WriteLine("Right circular cylinder" ); break;
          case OdDgSurface.Type.kSurfaceRight_CIRCULAR_Cone     : Program.DumpStream.WriteLine("Right circular cone" ); break;
          case OdDgSurface.Type.kSurfaceTabulatedCylinder       : Program.DumpStream.WriteLine("Tabulated cylinder" ); break;
          case OdDgSurface.Type.kSurfaceTabulatedCone           : Program.DumpStream.WriteLine("Tabulated cone" ); break;
          case OdDgSurface.Type.kSurfaceConvolute               : Program.DumpStream.WriteLine("Convolute" ); break;
          case OdDgSurface.Type.kSurfaceRevolution              : Program.DumpStream.WriteLine("Revolution" ); break;
          case OdDgSurface.Type.kSurfaceWarped                  : Program.DumpStream.WriteLine("Warped" ); break;
          }
        }

        public void writeFieldValue( String  name, OdDgSolid.Type value )
        {
          switch( value )
          {
          case OdDgSolid.Type.kSolidProjection : Program.DumpStream.WriteLine("Projection" ); break;
          case OdDgSolid.Type.kSolidRevolution : Program.DumpStream.WriteLine("Revolution" ); break;
          case OdDgSolid.Type.kSolidBoundary   : Program.DumpStream.WriteLine("Boundary" ); break;
          }
        }
        public OdRxObject getObject(){ return m_object; }
        //it is common for all dumpers
        public static int                  m_nesting;

        private OdRxObject m_object;
        //it is common for all dumpers
        private static ArrayList m_nestedTypes = new ArrayList();
    }
class OdDgDatabase_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        
        OdDgDatabase element = (OdDgDatabase)pObject;
        dumpSummaryInfo(element);
        startDescription(element);
        writeFieldValue( "Filename", element.getFilename() );
        writeFieldValue( "Original version", element.getOriginalFileVersion() );
        writeFieldValue( "Major version", element.getMajorVersion() );
        writeFieldValue( "Minor version", element.getMinorVersion() );
        writeFieldValue( "Control", element.getControlFlags() );
        writeFieldValue( "Control 1", element.getControl1Flags() );
        writeFieldValue( "Fbfdcn", element.getLockFlags() );
        writeFieldValue( "Ext locks", element.getExtLockFlags() );
        writeFieldValue( "Active angle", element.getActAngle() );
        writeFieldValue( "Angle round", element.getAngleRnd() );
        writeFieldValue( "X active scale", element.getXActScale() );
        writeFieldValue( "Y active scale", element.getYActScale() );
        writeFieldValue( "Z active scale", element.getZActScale() );
        writeFieldValue( "Round scale", element.getRoundScale() );
        writeFieldValue( "Azumuth", element.getAzimuth() );
        writeFieldValue( "Low", element.getExtMin() );
        writeFieldValue( "High", element.getExtMax() );
        writeFieldValue( "Active level", element.getActiveLevelEntryId() );
        writeFieldValue( "Active line style", element.getActiveLineStyleEntryId() );
        writeFieldValue( "Active line weight", element.getActiveLineWeight() );
        writeFieldValue( "Active color index", element.getActiveColorIndex() );
        writeFieldValue( "Fill color", element.getFillColorIndex() );
        writeFieldValue( "Active props", element.getActiveProps() );
        writeFieldValue( "Line style", element.getLineStyle() );
        writeFieldValue( "Line style scale", element.getLineStyleScale() );
        writeFieldValueHex( "Multiline flags", element.getMultiLineFlags() );
        writeFieldValue( "Text style ID", element.getActiveTextStyleEntryId() );
        writeFieldValue( "Text scale lock", element.getTextScaleLock() );
        writeFieldValue( "Active view group ID", element.getActiveViewGroupId().getHandle().ascii() );
        writeFieldValue( "Active model ID", element.getActiveModelId().getHandle().ascii() );
        writeFieldValue( "Angle format", element.getAngleFormat() );
        writeFieldValue( "Angle readout precision", element.getAngleReadoutPrec() );
        writeFieldValue( "Tentative mode", element.getTentativeMode() );
        writeFieldValue( "Tentative sub-mode", element.getTentativeSubMode() );
        writeFieldValue( "Keypoint divident", element.getKeyPointDividend() );
        writeFieldValue( "Default snap mode", element.getDefaultSnapMode() );
        writeFieldValue( "System class", element.getSystemClass() );
        writeFieldValueHex( "DMRSF flag", element.getDMRSFlag() );
        writeFieldValue( "DMRS linkage generation mode", element.getDMRSLinkageGenerationMode() );
        writeFieldValueHex( "Auto dimenstions flag", element.getAutoDimFlags() );
        writeFieldValueHex( "Associative lock mask", element.getAssocLockMask() );
        writeFieldValue( "Active cell", element.getActiveCell() );
        writeFieldValue( "Active term cell", element.getActiveTermCell() );
        writeFieldValue( "Active term scale", element.getActiveTermScale() );
        writeFieldValue( "Active pattern cell", element.getActivePatternCell() );
        writeFieldValue( "Active pattern scale", element.getActivePatternScale() );
        writeFieldValue( "Active pattern angle", element.getActivePatternAngle() );
        writeFieldValue( "Active pattern angle 2", element.getActivePatternAngle2() );
        writeFieldValue( "Active pattern row spacing", element.getActivePatternRowSpacing() );
        writeFieldValue( "Active pattern column spacing", element.getActivePatternColumnSpacing() );
        writeFieldValue( "Pattern tolerance", element.getPatternTolerance() );
        writeFieldValue( "Active point type", element.getActivePointType() );
        writeFieldValue( "Active point symbol", element.getActivePointSymbol() );
        writeFieldValue( "Active point cell", element.getActivePointCell() );
        writeFieldValue( "Area pattern angle", element.getAreaPatternAngle() );
        writeFieldValue( "Area pattern row spacing", element.getAreaPatternRowSpacing() );
        writeFieldValue( "Area pattern column spacing", element.getAreaPatternColumnSpacing() );
        writeFieldValue( "Reserved cell", element.getReservedCell() );
        writeFieldValue( "Z range 2D low", element.getZRange2dLow() );
        writeFieldValue( "Z range 2D high", element.getZRange2dHigh() );
        writeFieldValue( "Stream delta", element.getStreamDelta() );
        writeFieldValue( "Stream tolerance", element.getStreamTolerance() );
        writeFieldValue( "Angle tolerance", element.getAngleTolerance() );
        writeFieldValue( "Area tolerance", element.getAreaTolerance() );
        writeFieldValue( "Highlight color", element.getHighlightColorIndex() );
        writeFieldValue( "XOR color", element.getXorColorIndex() );
        writeFieldValue( "Axis lock angle", element.getAxisLockAngle() );
        writeFieldValue( "Axis lock origin", element.getAxisLockOrigin() );
        writeFieldValue( "Chamfer distance 1", element.getChamferDist1() );
        writeFieldValue( "Chamfer distance 2", element.getChamferDist2() );
        writeFieldValue( "Autochain tolerance", element.getAutochainTolerance() );
        writeFieldValue( "Consline distance", element.getConslineDistance() );
        writeFieldValue( "Arc radius", element.getArcRadius() );
        writeFieldValue( "Arc length", element.getArcLength() );
        writeFieldValue( "Cone radius 1", element.getConeRadius1() );
        writeFieldValue( "Cone radius 2", element.getConeRadius2() );
        writeFieldValue( "Polygon radius", element.getPolygonRadius() );
        writeFieldValue( "Surrev angle", element.getSurrevAngle() );
        writeFieldValue( "Extend distance", element.getExtendDistance() );
        writeFieldValue( "Fillet radius", element.getFilletRadius() );
        writeFieldValue( "Coppar distance", element.getCopparDistance() );
        writeFieldValue( "Array row distance", element.getArrayRowDistance() );
        writeFieldValue( "Array column distance", element.getArrayColumnDistance() );
        writeFieldValue( "Array fill angle", element.getArrayFillAngle() );
        writeFieldValue( "Point distance", element.getPointDistance() );
        writeFieldValue( "Polygon edges", element.getPolygonEdges() );
        writeFieldValue( "Points between", element.getPointsBetween() );
        writeFieldValue( "Array num of items", element.getArrayNumItems() );
        writeFieldValue( "Array num of rows", element.getArrayNumRows() );
        writeFieldValue( "Array num of columns", element.getArrayNumCols() );
        writeFieldValue( "Array rotate", element.getArrayRotate() );
        writeFieldValue( "B-spline order", element.getBSplineOrder() );
        writeFieldValue( "Display attribute type", element.getDispAttrType() );
        //  writeFieldValueHex( "Render flags", element.getRenderFlags() );
        writeFieldValue( "Latitude", element.getLatitude() );
        writeFieldValue( "Longitude", element.getLongitude() );
        writeFieldValue( "Solar time", element.getSolarTime() );
        writeFieldValue( "Solar year", element.getSolarYear() );
        writeFieldValue( "GMT offset", element.getGMTOffset() );
        writeFieldValue( "Solar direction", element.getSolarDirection() );
        writeFieldValue( "Solar vector override", element.getSolarVectorOverride() );
        writeFieldValue( "Solar intensity", element.getSolarIntensity() );
        writeFieldValue( "Ambient intensity", element.getAmbientIntensity() );
        writeFieldValue( "Flash intensity", element.getFlashIntensity() );
        writeFieldValue( "Near depth density", element.getNearDepthDensity() );
        writeFieldValue( "Far depth density", element.getFarDepthDensity() );
        writeFieldValue( "Near depth distance", element.getNearDepthDistance() );
        writeFieldValue( "Haze color", element.getHazeColor() );
        writeFieldValue( "Shadow tolerance", element.getShadowTolerance() );
        writeFieldValue( "Stroke tolerance", element.getStrokeTolerance() );
        writeFieldValue( "Max polygon size", element.getMaxPolygonSize() );
        writeFieldValue( "Arc minimum", element.getArcMinimum() );
        writeFieldValue( "Exact Hline accuracy", element.getExactHLineAccuracy() );
        writeFieldValue( "Exact Hline tolerance", element.getExactHLineTolerance() );
        writeFieldValue( "Selection Highlight override", element.getSelectionHighlightOverride() );
        writeFieldValue( "Selection Highlight color", element.getSelectionHighlightColor() );
        writeFieldValue( "Cell filename", element.getCellFileName() );
        writeFieldValue( "Background file", element.getBackgroundFile() );
        //writeFieldValue( "Default model is 3D", element.getDefaultModelIs3D() );
        //writeFieldValue( "Version", element.getVersion() );
        //writeFieldValue( "Sub version", element.getSubVersion() );
        //writeFieldValue( "Format", element.getFormat() );
        //writeFieldValue( "Highest model ID", element.getHighestModelID() );
        writeFieldValue( "Handseed", element.getHandseed().ToString() );
        writeFieldValue( "Last saved time", element.getLastSaveTime() );
        //writeFieldValue( "Next graphics group", element.getNextGraphicGroup() );
        //writeFieldValue( "Next text node", element.getNextTextNode() );
        //writeFieldValue( "Original format", element.getOriginalFormat() );
        //writeFieldValue( "Number of model specific digital signatures", element.getModelSpecificDigitalSignatures() );
        //writeFieldValue( "Number of file-wide digital signatures", element.getFileWideDigitalSignatures() );
        //writeFieldValue( "Primary application ID", element.getPrimaryApplicationID() );
        writeFieldValue( "Is persistent", element.isPersistent() );

        OdDgElementId modelId = element.getDefaultModelId();
        if (!modelId.isNull())
        {
            OdDgModel pModel = OdDgModel.cast(modelId.safeOpenObject());
            if (null != pModel)
            {
              writeFieldValue( "Default Model Name", pModel.getName() );
            }
        }

        //get all models
        {
            //writeShift();
            //Program.DumpStream.WriteLine("Number of models in the database: %I32u\n", number );

            OdDgModelTable pModelTable = element.getModelTable();
            if (null != pModelTable)
            {
              OdDgElementIterator pIter = pModelTable.createIterator();
              for ( ; !pIter.done(); pIter.step() )
              {
                  OdDgModel pModel = OdDgModel.cast( pIter.item().openObject() );
                if ( null != pModel )
                {
                  writeShift();
                    Program.DumpStream.WriteLine("Model #{0}\n", pModel.getEntryId());
                    OdDgModel_Dumper pModelDumper = (OdDgModel_Dumper)Program.GetProperType(pModel);//new OdDgModel_Dumper();//(OdDgModel_Dumper)pModel;

                  pModelDumper.dump( pModel );
                }
              }
            }
        }

        //look through the tables
        {
        OdRxObject                   curObject = null;
        OdRxObject_Dumper dumper;
            
        //table of levels
        curObject = element.getLevelTable();
        if(null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of level filters
        curObject = element.getLevelFilterTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of fonts
        curObject = element.getFontTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        OdDgTextStyleTable pTextStyleTbl = element.getTextStyleTable();
        if (null != pTextStyleTbl)
        {
          //Program.DumpStream.WriteLine(">>>>> Dumps Default Text Style\n" );
            Program.DumpStream.WriteLine(">>>>> Dumps Default Text Style\n");
          curObject = pTextStyleTbl.getDefaultData();
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of text styles
        curObject = element.getTextStyleTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of dimension styles
        curObject = element.getDimStyleTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of multiline styles
        curObject = element.getMultilineStyleTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of line styles
        curObject = element.getLineStyleTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of registered applications
        curObject = element.getRegAppTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of views
        curObject = element.getViewGroupTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of named views
        curObject = element.getNamedViewTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of shared cell definitions
        curObject = element.getSharedCellDefinitionTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of tag set definitions
        curObject = element.getTagDefinitionSetTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }

        //table of colors
        curObject = element.getColorTable();
        if( null != curObject )
        {
          dumper = Program.GetProperType(curObject);
          dumper.dump( curObject );
        }
        else
        {
          writeShift();
          Program.DumpStream.WriteLine("No color table\n");
        }
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    
    public override String getClassName()
    {
        return "OdDgDatabase";
    }

    private void dumpSummaryInfo(OdDgDatabase pDb)
    { 
        Program.DumpStream.WriteLine("> Summary Information\n");
  m_nesting++;

  // Summary Information
  OdDgSummaryInformation pSi = TG_Db.oddgGetSummaryInformation(pDb);
  writeFieldValue( "Title", pSi.getTitle() );
  writeFieldValue( "Subject", pSi.getSubject() );
  writeFieldValue( "Author", pSi.getAuthor() );
  writeFieldValue( "Keywords", pSi.getKeywords() );
  //writeFieldValue( "Comments", pSi.getComments() ); // PIDSI_COMMENTS == 6
  OdDgPropertyValue pPropComments = pSi.getProperty( 6 );
  if (null != pPropComments)
  {
    writeFieldValue( "Comments", pPropComments.getValue() );
  }
  writeFieldValue( "Template", pSi.getTemplate() );
  writeFieldValue( "LastSavedBy", pSi.getLastSavedBy() );
  writeFieldValue( "RevisionNumber", pSi.getRevisionNumber() );
  writeFieldValue( "ApplicationName", pSi.getApplicationName() );
  String timeStr = "";
  OdTimeStamp ts = pSi.getTotalEditingTime();
  if ( ts != new OdTimeStamp(OdTimeStamp.InitialValue.kInitZero) )
      ts.strftime("%H:%M:%S", ref timeStr);
  else
    timeStr = "0";
  writeFieldValue( "TotalEditingTime", timeStr);
  ts = pSi.getLastPrintedTime();
  if ( ts != new OdTimeStamp(OdTimeStamp.InitialValue.kInitZero) )
    ts.strftime("%H:%M:%S", ref timeStr);
  else
    timeStr = "0";
  writeFieldValue( "LastPrintedTime", timeStr);
  timeStr = pSi.getCreatedTime().ToString();
  writeFieldValue( "CreatedTime", timeStr);
  timeStr = pSi.getLastSavedTime().ToString();
  writeFieldValue( "LastSavedTime", timeStr);
  writeFieldValue( "Security", pSi.getSecurity() );
  OdBinaryData dibData = new OdBinaryData();
  pSi.getThumbnailBitmap(dibData);
  if ( 0 != dibData.Count )
  {
    writeFieldValue( "ThumbnailBitmap Size", dibData.Count);
  }

  // Document Summary Information
  OdDgDocumentSummaryInformation pDsi = TG_Db.oddgGetDocumentSummaryInformation(pDb);
  writeFieldValue( "Category", pDsi.getCategory() );
  writeFieldValue( "Manager", pDsi.getManager() );
  //writeFieldValue( "Company", pDSi.getCompany() ); // PIDDSI_COMPANY == 0x0000000F
  OdDgPropertyValue pPropCompany = pDsi.getProperty( 0x0F );
  writeFieldValue( "Company", pPropCompany.getValue() );

  // UserDefined Properties
  OdRxDictionary pPropDic = pDsi.getCustomProperties();
  if ( null != pPropDic )
  {
    OdRxDictionaryIterator pIt = pPropDic.newIterator();
    for ( ; !pIt.done(); pIt.next() )
    {
      String propName = pIt.getKey();
      OdDgPropertyValue pPropValue = OdDgPropertyValue.cast(pIt.getObject()); //(OdDgPropertyValue)pIt.getObject();
      writeFieldValue( propName, pPropValue.getValue().ToString() );
    }
  }
  m_nesting--;
    }
};


class OdDgModel_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgModel element = (OdDgModel)pObject;
        startDescription(element);
        //writeFieldValue("OdDgModel", "OdDgModel");
        
          //storage unit
          {
            OdDgModel.StorageUnitDescription description = new OdDgModel.StorageUnitDescription();
            element.getStorageUnit(description );
            writeShift();
            Program.DumpStream.WriteLine("Storage unit:\n" );
            m_nesting++;

            writeFieldValue( "Base", description.m_base );
            writeFieldValue( "System", description.m_system );
            writeFieldValue( "Numerator", description.m_numerator );
            writeFieldValue( "Denominator", description.m_denominator );
            writeFieldValue( "Uors per Storage", description.m_uorPerStorageUnit );

            m_nesting--;
          }

          //master unit
          {
            OdDgModel.UnitDescription description = new OdDgModel.UnitDescription();
            element.getMasterUnit( description );

            writeShift();
            Program.DumpStream.WriteLine("Master unit:\n" );
            m_nesting++;

            writeFieldValue( "Base", description.m_base );
            writeFieldValue( "System", description.m_system );
            writeFieldValue( "Numerator", description.m_numerator );
            writeFieldValue( "Denominator", description.m_denominator );
            writeFieldValue( "Name", description.m_name );

            m_nesting--;
          }

          //sub unit
          {
            OdDgModel.UnitDescription description = new OdDgModel.UnitDescription();
            element.getSubUnit( description );

            writeShift();
            Program.DumpStream.WriteLine("Sub unit:\n" );
            m_nesting++;

            writeFieldValue( "Base", description.m_base );
            writeFieldValue( "System", description.m_system );
            writeFieldValue( "Numerator", description.m_numerator );
            writeFieldValue( "Denominator", description.m_denominator );
            writeFieldValue( "Name", description.m_name );

            m_nesting--;
          }

          writeFieldValue( "Working unit", element.getWorkingUnit() );
          writeFieldValue( "Global Origin", element.getGlobalOrigin() );

          {
            OdGeExtents3d extent = new OdGeExtents3d();

            if( element.getGeomExtents( extent ) == eOk )
            {
              writeFieldValue( "Extent", extent );
            }
            else
            {
              writeFieldValue( "Extent", "invalid value" );
            }
          }

          //describes all graphics elements
          writeShift();
          Program.DumpStream.WriteLine("All graphics sub-elements:\n" );
          describeSubElements( element.createGraphicsElementsIterator() );
          //describes all non-graphics elements
          writeShift();
          Program.DumpStream.WriteLine("All control sub-elements:\n" );
          describeSubElements( element.createControlElementsIterator() );
        
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgModel";
    }
}

class OdDgSheetModel_Dumper : OdDgModel_Dumper
{
  public override String getClassName()
  {
    return "OdDgSheetModel";
  }
}
class OdDgLine2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLine2d element = OdDgLine2d.cast(pObject);
        startDescription(element);
        OdGePoint2d point = new OdGePoint2d();
        point = element.getStartPoint();
        writeFieldValue("Vertex 1", point);
        point = element.getEndPoint();
        writeFieldValue("Vertex 2", point);
        // check getColor()
        // TODO: check after adding getColor to OdDgLine2d
        UInt32 color = element.getColor();
        writeFieldValue("Color", color);
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLine2d";
    }
}

class OdDgLine3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLine3d element = (OdDgLine3d)pObject;
        OdGePoint3d point = new OdGePoint3d();
        startDescription(element);
        point = element.getStartPoint();
        writeFieldValue("Vertex 1", point);
        point = element.getEndPoint();
        writeFieldValue("Vertex 2", point);
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLine3d";
    }
}

class OdDgLineString2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLineString2d element = (OdDgLineString2d)pObject;
        uint i, j = element.getVerticesCount();
        OdGePoint2d point = new OdGePoint2d();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine("Number of vertices: {0}\n", j);

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLineString2d";
    }
}

class OdDgLineString3d_Dumper : OdRxObject_Dumper
{
    public void writeLinkagesInfoLS3(OdRxObject pObject)
    {
        OdDgElement element = OdDgElement.cast(pObject);

        if (null == element)
        {
            return;
        }

        //take all linkages
        OdRxObjectPtrArray linkages = new OdRxObjectPtrArray();
        element.getLinkages(linkages);
        int linkagesNumber = linkages.Count;

        if (linkagesNumber > 0)
        {
            writeShift();
            Program.DumpStream.WriteLine(string.Format("> Attribute Linkages (%{0} items)\n", linkagesNumber));
            m_nesting++;

            for (int i = 0; i < linkagesNumber; ++i)
            {
                OdDgAttributeLinkage pLinkage = (OdDgAttributeLinkage)linkages[i];

                OdBinaryData data = new OdBinaryData();
                pLinkage.getData(data);
                writeShift();
                Program.DumpStream.WriteLine(string.Format("Primary ID = 0x%{0}, data size = %{1}", pLinkage.getPrimaryId(), data.Count));

                //additionary info depending on the type
                switch (pLinkage.getPrimaryId())
                {
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kHatch:
                        {
                            OdDgPatternLinkage pPatternLinkage = OdDgPatternLinkage.cast(pLinkage);
                            if (null != pPatternLinkage)
                            {
                                String namedType;
                                switch (pPatternLinkage.getType())
                                {
                                    case OdDgPatternLinkage.PatternType.kLinearPattern: namedType = "LinearPattern"; break;
                                    case OdDgPatternLinkage.PatternType.kCrossPattern: namedType = "CrossPattern"; break;
                                    case OdDgPatternLinkage.PatternType.kSymbolPattern: namedType = "SymbolPattern"; break;
                                    case OdDgPatternLinkage.PatternType.kDWGPattern: namedType = "DWGPattern"; break;
                                    default: namedType = "Unknown"; break;
                                }
                                Program.DumpStream.WriteLine(string.Format(" ( Pattern type = {0}", namedType));
                                if (pPatternLinkage.getUseOffsetFlag())
                                {
                                    OdGePoint3d offset = new OdGePoint3d();
                                    pPatternLinkage.getOffset(offset);
                                    //Program.DumpStream.WriteLine("; offset = (%g %g %g)", offset.x, offset.y, offset.z );
                                    Program.DumpStream.WriteLine(string.Format("; offset = (%g %g %g)", offset.x, offset.y, offset.z));
                                }
                                Program.DumpStream.WriteLine(")");
                            }
                        }
                        break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kThickness:
                        {
                            OdDgThicknessLinkage pThicknessLinkage = OdDgThicknessLinkage.cast(pLinkage);

                            if (null != pThicknessLinkage)
                            {
                                Program.DumpStream.WriteLine(string.Format(" ( Thickness Linkage, thickness = {0} )", pThicknessLinkage.getThickness()));
                            }
                        }
                        break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kString:
                        {
                            OdDgStringLinkage pStrLinkage = OdDgStringLinkage.cast(pLinkage);

                            if (null != pStrLinkage)
                            {
                                Program.DumpStream.WriteLine(string.Format(" ( String Linkage, ID = {0}; value = {1})", pStrLinkage.getStringId(), pStrLinkage.getString()));
                            }
                        }
                        break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kDMRS:
                        {
                            OdDgDMRSLinkage DMRSLinkage = OdDgDMRSLinkage.cast(pLinkage);
                            if (null != DMRSLinkage)
                            {
                                Program.DumpStream.WriteLine(string.Format(" ( DMRS Linkage, tableId = {0}, MSLink = {1}, type = {2} )", DMRSLinkage.getTableId(), DMRSLinkage.getMSLink(), DMRSLinkage.getType()));
                            }
                        }
                        break;
                    case 0x56D5:
                        {
                            OdDgProxyLinkage linkage = OdDgProxyLinkage.cast(pLinkage);
                            if (null != linkage)
                            {
                                OdBinaryData d_data = new OdBinaryData();
                                linkage.getData(d_data);
                                Int32 ii = d_data.Count;
                                Int32 j = d_data.Count;
                                Program.DumpStream.WriteLine(" ( Proxy linkage )\n");
                                m_nesting++;
                                writeFieldValue("Size of the proxy linkage", j);
                                for (ii = 0; ii < j; ii++)
                                {
                                    if (0 == (ii % 16))
                                    {
                                        if (ii > 0)
                                        {
                                            Program.DumpStream.WriteLine("\n");
                                        }
                                        writeShift();
                                        //Program.DumpStream.WriteLine("%.4X: ", i );
                                        Program.DumpStream.WriteLine(ii.ToString());
                                    }
                                    //Program.DumpStream.WriteLine("%.2X ", data[ i ] );
                                    Program.DumpStream.WriteLine(d_data[ii].ToString());
                                }
                                m_nesting--;
                            }
                        }
                        break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kFRAMME: // DB Linkage - FRAMME tag data signature
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kBSI: // DB Linkage - secondary id link (BSI radix 50)
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kXBASE: // DB Linkage - XBase (DBase)
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kINFORMIX: // DB Linkage - Informix
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kINGRES: // DB Linkage - INGRES
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kSYBASE: // DB Linkage - Sybase
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kODBC: // DB Linkage - ODBC
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kOLEDB: // DB Linkage - OLEDB
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kORACLE: // DB Linkage - Oracle
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kRIS: // DB Linkage - RIS
                        {
                            OdDgDBLinkage dbLinkage = OdDgDBLinkage.cast(pLinkage);
                            if (null != dbLinkage)
                            {
                                String namedType;

                                switch (dbLinkage.getDBType())
                                {
                                    case OdDgDBLinkage.DBType.kBSI: namedType = "BSI"; break;
                                    case OdDgDBLinkage.DBType.kFRAMME: namedType = "FRAMME"; break;
                                    case OdDgDBLinkage.DBType.kInformix: namedType = "Informix"; break;
                                    case OdDgDBLinkage.DBType.kIngres: namedType = "Ingres"; break;
                                    case OdDgDBLinkage.DBType.kODBC: namedType = "ODBC"; break;
                                    case OdDgDBLinkage.DBType.kOLEDB: namedType = "OLE DB"; break;
                                    case OdDgDBLinkage.DBType.kOracle: namedType = "Oracle"; break;
                                    case OdDgDBLinkage.DBType.kRIS: namedType = "RIS"; break;
                                    case OdDgDBLinkage.DBType.kSybase: namedType = "Sybase"; break;
                                    case OdDgDBLinkage.DBType.kXbase: namedType = "xBase"; break;
                                    default: namedType = "Unknown"; break;
                                }

                                Program.DumpStream.WriteLine(string.Format(" ( DB Linkage, tableId = {0}, MSLink = {1}, type = {2} )", dbLinkage.getTableEntityId(), dbLinkage.getMSLink(), namedType));
                            }
                        }
                        break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kDimension: // Dimension Linkage
                        {
                            OdDgDimensionLinkage dimLinkage = OdDgDimensionLinkage.cast(pLinkage);
                            if (null != dimLinkage)
                            {
                                String sDimType;
                                switch (dimLinkage.getType())
                                {
                                    case OdDgDimensionLinkage.DimensionSubType.kOverall: sDimType = "Overall"; break;
                                    case OdDgDimensionLinkage.DimensionSubType.kSegment: sDimType = "Segment"; break;
                                    case OdDgDimensionLinkage.DimensionSubType.kPoint: sDimType = "Point"; break;
                                    case OdDgDimensionLinkage.DimensionSubType.kSegmentFlags: sDimType = "SegmentFlags"; break;
                                    case OdDgDimensionLinkage.DimensionSubType.kDimensionInfo: sDimType = "DimensionInfo"; break;
                                    default: sDimType = "Unkown"; break;
                                }
                                Program.DumpStream.WriteLine(" ( Dimension Linkage, type = {0} )", sDimType);
                                switch (dimLinkage.getType())
                                {
                                    case OdDgDimensionLinkage.DimensionSubType.kOverall:
                                        break;
                                    case OdDgDimensionLinkage.DimensionSubType.kSegment:
                                        break;
                                    case OdDgDimensionLinkage.DimensionSubType.kPoint:
                                        break;
                                    case OdDgDimensionLinkage.DimensionSubType.kSegmentFlags:
                                        break;
                                    case OdDgDimensionLinkage.DimensionSubType.kDimensionInfo:
                                        {
                                            OdDgDimensionInfoLinkage pDimInfoLinkage = (OdDgDimensionInfoLinkage)pLinkage;

                                            if (pDimInfoLinkage.getUseAnnotationScale())
                                                writeFieldValue("  Annotation Scale", pDimInfoLinkage.getAnnotationScale());

                                            if (pDimInfoLinkage.getUseDatumValue())
                                            {
                                                double dDatumValue = pDimInfoLinkage.getDatumValue();

                                                OdDgDatabase pDb = element.database();
                                                OdDgElementId idModel = new OdDgElementId();

                                                if (null != pDb)
                                                    idModel = pDb.getActiveModelId();

                                                if (null != idModel)
                                                {
                                                    OdDgModel pModel = OdDgModel.cast(idModel.openObject());

                                                    if (null != pModel)
                                                    {
                                                        dDatumValue = pModel.convertUORsToWorkingUnits(dDatumValue);
                                                    }
                                                }
                                                else
                                                {
                                                    dDatumValue /= 10000000000; // Storage units default factor
                                                }

                                                writeFieldValue("  Datum Value", dDatumValue);
                                            }

                                            if (pDimInfoLinkage.getUseRetainFractionalAccuracy())
                                            {
                                                writeFieldValue("  Detriment in reverse direction", pDimInfoLinkage.getUseDecrimentInReverceDirection());
                                                writeFieldValue("  Primary retain fractional accuracy", pDimInfoLinkage.getPrimaryRetainFractionalAccuracy());
                                                writeFieldValue("  Secondary retain fractional accuracy", pDimInfoLinkage.getSecondaryRetainFractionalAccuracy());
                                                writeFieldValue("  Primary alt format retain fractional accuracy", pDimInfoLinkage.getPrimaryAltFormatRetainFractionalAccuracy());
                                                writeFieldValue("  Secondary alt format retain fractional accuracy", pDimInfoLinkage.getSecondaryAltFormatRetainFractionalAccuracy());
                                                writeFieldValue("  Primary tolerance retain fractional accuracy", pDimInfoLinkage.getPrimaryTolerRetainFractionalAccuracy());
                                                writeFieldValue("  Secondary tolerance retain fractional accuracy", pDimInfoLinkage.getSecondaryTolerRetainFractionalAccuracy());
                                                writeFieldValue("  Label line mode", pDimInfoLinkage.getLabelLineDimensionMode());
                                            }

                                            if (pDimInfoLinkage.getUseFitOptionsFlag())
                                            {
                                                writeFieldValue("  Suppress unfit terminators", pDimInfoLinkage.getUseSuppressUnfitTerm());
                                                writeFieldValue("  Use inline leader length", pDimInfoLinkage.getUseInlineLeaderLength());
                                                writeFieldValue("  Text above optimal fit", pDimInfoLinkage.getUseTextAboveOptimalFit());
                                                writeFieldValue("  Narrow font optimal fit", pDimInfoLinkage.getUseNarrowFontOptimalFit());
                                                writeFieldValue("  Use Min Leader Terminator Length", pDimInfoLinkage.getUseMinLeaderTermLength());
                                                writeFieldValue("  Use auto mode for dimension leader", pDimInfoLinkage.getUseAutoLeaderMode());
                                                writeFieldValue("  Fit Options ", pDimInfoLinkage.getFitOptions());
                                            }

                                            if (pDimInfoLinkage.getUseTextLocation())
                                            {
                                                writeFieldValue("  Free location of text", pDimInfoLinkage.getUseFreeLocationText());
                                                writeFieldValue("  Note spline fit", pDimInfoLinkage.getUseNoteSplineFit());
                                                writeFieldValue("  Text location ", pDimInfoLinkage.getTextLocation());
                                            }

                                            if (pDimInfoLinkage.getUseInlineLeaderLengthValue())
                                            {
                                                double dLengthValue = pDimInfoLinkage.getInlineLeaderLength();

                                                OdDgDatabase pDb = element.database();
                                                OdDgElementId idModel = new OdDgElementId();

                                                if (null != pDb)
                                                    idModel = pDb.getActiveModelId();

                                                if (null != idModel)
                                                {
                                                    OdDgModel pModel = OdDgModel.cast(idModel.openObject());

                                                    if (null != pModel)
                                                    {
                                                        dLengthValue = pModel.convertUORsToWorkingUnits(dLengthValue);
                                                    }
                                                }
                                                else
                                                {
                                                    dLengthValue /= 10000000000; // Storage units default factor
                                                }

                                                writeFieldValue("  Inline leader length value", dLengthValue);
                                            }

                                            if (pDimInfoLinkage.getUseInlineTextLift())
                                                writeFieldValue("  Inline text lift", pDimInfoLinkage.getInlineTextLift());

                                            if (pDimInfoLinkage.getUseNoteFrameScale())
                                                writeFieldValue("  Note frame scale", pDimInfoLinkage.getUseNoteFrameScale());

                                            if (pDimInfoLinkage.getUseNoteLeaderLength())
                                                writeFieldValue("  Note leader length", pDimInfoLinkage.getNoteLeaderLength());

                                            if (pDimInfoLinkage.getUseNoteLeftMargin())
                                                writeFieldValue("  Note left margin", pDimInfoLinkage.getUseNoteLeftMargin());

                                            if (pDimInfoLinkage.getUseNoteLowerMargin())
                                                writeFieldValue("  Note lower margin", pDimInfoLinkage.getUseNoteLowerMargin());

                                            if (pDimInfoLinkage.getUsePrimaryToleranceAccuracy())
                                                writeFieldValue("  Primary tolerance accuracy", pDimInfoLinkage.getPrimaryToleranceAccuracy());

                                            if (pDimInfoLinkage.getUseSecondaryToleranceAccuracy())
                                                writeFieldValue("  Secondary tolerance accuracy", pDimInfoLinkage.getSecondaryToleranceAccuracy());

                                            if (pDimInfoLinkage.getUseStackedFractionScale())
                                                writeFieldValue("  Stacked fraction scale", pDimInfoLinkage.getStackedFractionScale());
                                        } break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kFilterMember:
                        {
                            OdDgFilterMemberLinkage pFilterLinkage = (OdDgFilterMemberLinkage)pLinkage;
                            writeFieldValue("  Member Id", pFilterLinkage.getMemberId());
                            writeFieldValue("  Member Type", pFilterLinkage.getMemberType());
                            writeFieldValue("  Name String", pFilterLinkage.getNameString());
                            writeFieldValue("  Expression String", pFilterLinkage.getExpressionString());
                        } break;
                    case (ushort)OdDgPatternLinkage.PrimaryIds.kDependency:
                        {
                            OdDgDependencyLinkage dependencyLinkage = OdDgDependencyLinkage.cast(pLinkage);
                            if (null != dependencyLinkage)
                            {
                                Program.DumpStream.WriteLine(string.Format("( Root type = {0}; App ID = {1}; App Value = {2} )",
                                  dependencyLinkage.getRootDataType(),
                                  dependencyLinkage.getAppValue(),
                                  dependencyLinkage.getAppValue()));

                                //some additional information
                                m_nesting++;
                                switch (dependencyLinkage.getRootDataType())
                                {
                                    case OdDgDependencyLinkage.RootDataType.kElementId:
                                        OdDgDepLinkageElementId elementIdLinkage = OdDgDepLinkageElementId.cast(dependencyLinkage);
                                        if (null != elementIdLinkage)
                                        {
                                            UInt32 j = elementIdLinkage.getCount();
                                            UInt32 ii = elementIdLinkage.getCount();
                                            Program.DumpStream.WriteLine("\n");
                                            m_nesting++;
                                            writeShift();
                                            Program.DumpStream.WriteLine(string.Format("Number of IDs: {0}; They are:", j));
                                            for (ii = 0; ii < j; ii++)
                                            {
                                                Program.DumpStream.WriteLine(string.Format(" {0}.8I64X", elementIdLinkage.getAt(ii)));
                                            }
                                            m_nesting--;
                                        }
                                        break;
                                }
                                m_nesting--;
                            }
                        }
                        break;
                }
                Program.DumpStream.WriteLine("\n");
            }

            m_nesting--;
            writeShift();
            Program.DumpStream.WriteLine("< Attribute Linkages\n");
        }
    }
    public void writeElementInfoLS3(OdRxObject pObject)
    {
        OdDgElement element = OdDgElement.cast(pObject);
        if (null == element)
        {
            return;
        }

        writeShift();
        Program.DumpStream.WriteLine("Common information for DGN elements:\n");
        m_nesting++;

        writeFieldValue("ID", element.elementId().getHandle().ascii());
        writeFieldValue("Type", element.getElementType().ToString());
        writeFieldValue("Is Locked", element.getLockedFlag());

        m_nesting--;
    }


    public void startDescriptionLS3(OdRxObject pObject)
    {
        writeShift();
        //OdRxObject m_object = pObject;
        //write it name
        String className;
        {
            OdRxObject_Dumper dumper = Program.GetProperType(pObject);

            //OdRxObject_Dumper dumper = (OdRxObject_Dumper)pObject;

            className = dumper.getClassName();
            Program.DumpStream.WriteLine(className);
        }

        //add the path
        //{
        //    Program.DumpStream.WriteLine(" Path: */");

        //    int i, j = m_nestedTypes.Count;

        //    for (i = 0; i < j; i++)
        //    {
        //        Program.DumpStream.WriteLine(m_nestedTypes[i].ToString());
        //    }
        //}

        //final actions
        Program.DumpStream.WriteLine();
        //m_nestedTypes.Add(className);
        m_nesting++;

        //dump specific information
        writeLinkagesInfo(pObject);//(m_object);
        //writeLinkagesInfoLS3(pObject);
        writeElementInfo(pObject);//(m_object); // - linkagesInfo + ElementInfo = failure
        //writeElementInfoLS3(pObject);//(m_object); // - linkagesInfo + ElementInfo = failure
        //writeGraphicsElementInfoLS3(pObject);//(m_object);
        writeGraphicsElementInfo(pObject);//(m_object); // - failure
    }

    public void finishDescriptionLS3()
    {
        m_nesting--;

        writeShift();
        //Program.DumpStream.WriteLine(m_nestedTypes[m_nestedTypes.Count - 1]);

        //m_nestedTypes.RemoveAt(m_nestedTypes.Count - 1);
    }

    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLineString3d element = (OdDgLineString3d)pObject;
        uint i, j = element.getVerticesCount();
        OdGePoint3d point = new OdGePoint3d();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine("Number of vertices: {0}\n", j);

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLineString3d";
    }
}
class OdDgText2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgText2d element = (OdDgText2d)pObject;
        startDescription(element);
        writeFieldValue("Text", element.getText());
        writeFieldValue("Font ID", element.getFontEntryId());
        // Gets Font name
        OdDgFontTable pFontTable = element.database().getFontTable();
        OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
        if (null != pFont)
        {
            writeFieldValue("Font Name", pFont.getName());
        }

        writeFieldValue("Justification", element.getJustification());
        //writeFieldValue( "Length", element.getLength() );
        //writeFieldValue( "Height", element.getHeight() );
        writeFieldValue("Length multiplier", element.getLengthMultiplier());
        writeFieldValue("Height multiplier", element.getHeightMultiplier());
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.getOrigin());
        writeFieldValue("TextColor", element.getColorIndex());
        writeFieldValue("Slant", element.getSlant());
        writeFieldValue("ItalicsOverrideFlag", element.getItalicsOverrideFlag());
        writeFieldValue("SlantFlag", element.getSlantFlag());
        writeFieldValue("SlantOverrideFlag", element.getSlantOverrideFlag());

        writeFieldValue("UnderlineOverrideFlag", element.getUnderlineOverrideFlag());
        writeFieldValue("UnderlineFlag", element.getUnderlineFlag());

        writeFieldValue("HeightOverrideFlag", element.getHeightOverrideFlag());

        writeFieldValue("TextStyle bit", element.getTextStyleFlag());
        writeFieldValue("TextStyle ID", element.getTextStyleEntryId());

        OdDgTextStyleTable pTextStyleTable = element.database().getTextStyleTable();
        if (null != pTextStyleTable)
        {
            OdDgElementId id = pTextStyleTable.getAt(element.getTextStyleEntryId());
            if (!id.isNull())
            {
                OdDgTextStyleTableRecord pTextStyle = OdDgTextStyleTableRecord.cast(id.safeOpenObject());
                writeFieldValue("TextStyle Name", pTextStyle.getName());
                writeFieldValue("Height from TextStyle", pTextStyle.getTextHeight());
            }
        }
        writeFieldValue("StyleOverridesFlag", element.getStyleOverridesFlag());

        Int16 nCount = element.getTextEditFieldCount();
        writeFieldValue("The number of enter data fields in the text element is ", nCount);
        writeShift();
        for (Int16 i = 0; i < nCount; i++)
        {
            OdDgTextEditField textEditField = element.getTextEditFieldAt(i);

            writeFieldValue("StartPosition", element.getHeight());
            writeFieldValue("Length", element.getRotation());
            writeFieldValue("Justification", element.getOrigin());
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgText2d";
    }
}
class OdDgText3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgText3d element = (OdDgText3d)pObject;
        startDescription(element);
        writeFieldValue("Text", element.getText());
        writeFieldValue("Font ID", element.getFontEntryId());
        // Gets Font name
        OdDgFontTable pFontTable = element.database().getFontTable();
        OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
        if (null != pFont)
        {
            writeFieldValue("Font Name", pFont.getName());
            writeFieldValue("Font number", pFont.getNumber());
            writeFieldValue("Font type", pFont.getType());
        }

        writeFieldValue("Justification", element.getJustification());
        //writeFieldValue( "Length", element.getLength() );
        //writeFieldValue( "Height", element.getHeight() );
        writeFieldValue("Length multiplier", element.getLengthMultiplier());
        writeFieldValue("Height multiplier", element.getHeightMultiplier());
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.getOrigin());
        writeFieldValue("TextColor", element.getColorIndex());
        writeFieldValue("Slant", element.getSlant());
        writeFieldValue("ItalicsOverrideFlag", element.getItalicsOverrideFlag());
        writeFieldValue("SlantFlag", element.getSlantFlag());
        writeFieldValue("SlantOverrideFlag", element.getSlantOverrideFlag());
        writeFieldValue("SuperscriptFlag", element.getSuperscriptFlag());
        writeFieldValue("SuperscriptOverrideFlag", element.getSuperscriptOverrideFlag());


        writeFieldValue("HeightOverrideFlag", element.getHeightOverrideFlag());

        writeFieldValue("UnderlineOverrideFlag", element.getUnderlineOverrideFlag());
        writeFieldValue("UnderlineFlag", element.getUnderlineFlag());

        writeFieldValue("TextStyle bit", element.getTextStyleFlag());
        writeFieldValue("TextStyle ID", element.getTextStyleEntryId());

        OdDgTextStyleTable pTextStyleTable = element.database().getTextStyleTable();
        if (null != pTextStyleTable)
        {
            OdDgElementId id = pTextStyleTable.getAt(element.getTextStyleEntryId());
            if (!id.isNull())
            {
                OdDgTextStyleTableRecord pTextStyle = OdDgTextStyleTableRecord.cast(id.safeOpenObject());
                writeFieldValue("TextStyle Name", pTextStyle.getName());
                writeFieldValue("Height from TextStyle", pTextStyle.getTextHeight());
            }
        }
        writeFieldValue("StyleOverridesFlag", element.getStyleOverridesFlag());

        Int16 nCount = element.getTextEditFieldCount();
        writeFieldValue("The number of enter data fields in the text element is ", nCount);
        writeShift();
        for (Int16 i = 0; i < nCount; i++)
        {
            OdDgTextEditField textEditField = element.getTextEditFieldAt(i);

            writeFieldValue("StartPosition", element.getHeight());
            writeFieldValue("Length", element.getRotation());
            writeFieldValue("Justification", element.getOrigin());
        } finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgText3d";
    }
}
class OdDgTextNode2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTextNode2d element = (OdDgTextNode2d)pObject;
        startDescription(element);
        writeFieldValue("Line spacing", element.getLineSpacing());
        writeFieldValue("Font ID", element.getFontEntryId());
        writeFieldValue("Max length", element.getMaxLength());
        writeFieldValue("Justification", element.getJustification());
        writeFieldValue("Length", element.getLengthMultiplier());
        writeFieldValue("Height", element.getHeightMultiplier());
        /*
        {
          TextAttributes attributes;

          element.getTextAttributes( attributes );
          writeFieldValue( "Text attributes", attributes );
        }
        */
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.getOrigin());

        describeSubElements(element.createIterator());

        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTextNode2d";
    }
}
class OdDgTextNode3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTextNode3d element = (OdDgTextNode3d)pObject;
        startDescription(element);
        writeFieldValue("Line spacing", element.getLineSpacing());
        writeFieldValue("Font ID", element.getFontEntryId());
        writeFieldValue("Max length", element.getMaxLength());
        writeFieldValue("Justification", element.getJustification());
        writeFieldValue("Length", element.getLengthMultiplier());
        writeFieldValue("Height", element.getHeightMultiplier());
        /*
        {
          TextAttributes attributes;

          element.getTextAttributes( attributes );
          writeFieldValue( "Text attributes", attributes );
        }
        */
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.getOrigin());

        describeSubElements(element.createIterator()); finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTextNode3d";
    }
}
class OdDgShape2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgShape2d element = (OdDgShape2d)pObject;
        OdGePoint2d point = new OdGePoint2d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgShape2d";
    }
}
class OdDgShape3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgShape3d element = (OdDgShape3d)pObject;
        OdGePoint3d point = new OdGePoint3d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgShape3d";
    }
}
class OdDgCurve2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgCurve2d element = (OdDgCurve2d)pObject;
        OdGePoint2d point = new OdGePoint2d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgCurve2d";
    }
}
class OdDgCurve3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgCurve3d element = (OdDgCurve3d)pObject;
        OdGePoint3d point = new OdGePoint3d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Vertex {0}", i), point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgCurve3d";
    }
}
class OdDgEllipse2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgEllipse2d element = (OdDgEllipse2d)pObject;
        startDescription(element);
        writeFieldValue("Primary axis", element.getPrimaryAxis());
        writeFieldValue("Secondary axis", element.getSecondaryAxis());
        writeFieldValue("Rotation", element.getRotationAngle());
        {
            OdGePoint2d point = new OdGePoint2d();

            point = element.getOrigin();
            writeFieldValue("Origin", point);
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgEllipse2d";
    }
}
class OdDgEllipse3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgEllipse3d element = (OdDgEllipse3d)pObject;
        startDescription(element);
        writeFieldValue("Primary axis", element.getPrimaryAxis());
        writeFieldValue("Secondary axis", element.getSecondaryAxis());
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.origin());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgEllipse3d";
    }
}
class OdDgArc2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgArc2d element = (OdDgArc2d)pObject;
        startDescription(element);
        writeFieldValue("Primary axis", element.getPrimaryAxis());
        writeFieldValue("Secondary axis", element.getSecondaryAxis());
        writeFieldValue("Rotation", element.getRotationAngle());
        {
            OdGePoint2d point;

            point = element.getOrigin();
            writeFieldValue("Origin", point);
        }
        writeFieldValue("Start angle", element.getStartAngle());
        writeFieldValue("Sweep angle", element.getSweepAngle());

        OdGeEllipArc2d pArc = element.getEllipArc();
        writeFieldValue("isClockWise", pArc.isClockWise());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgArc2d";
    }
}
class OdDgArc3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgArc3d element = (OdDgArc3d)pObject;
        startDescription(element);
        writeFieldValue("Primary axis", element.getPrimaryAxis());
        writeFieldValue("Secondary axis", element.getSecondaryAxis());
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Origin", element.getOrigin());
        writeFieldValue("Start angle", element.getStartAngle());
        writeFieldValue("Sweep angle", element.getSweepAngle());

        OdGeEllipArc3d pArc = element.getEllipArc();
        writeFieldValue("Start angle (OdGeEllipArc3d)", pArc.startAng());
        writeFieldValue("End angle (OdGeEllipArc3d)", pArc.endAng());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgArc3d";
    }
}
class OdDgCone_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgCone element = (OdDgCone)pObject;
        startDescription(element);
        {
            OdGePoint3d point = element.getCenter1();
            writeFieldValue("Center 1", point);
            point = element.getCenter2();
            writeFieldValue("Center 2", point);
        }
        writeFieldValue("Radius 1", element.getRadius1());
        writeFieldValue("Radius 2", element.getRadius2());
        writeFieldValue("Rotation", element.getRotation());
        writeFieldValue("Is solid", element.isSolid());
        writeFieldValue("Hole", element.getHoleFlag());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgCone";
    }
}
class OdDgComplexString_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgComplexString element = (OdDgComplexString)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgComplexString";
    }
}
class OdDgComplexShape_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgComplexShape element = (OdDgComplexShape)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgComplexShape";
    }
}
class OdDgPointString2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgPointString2d element = (OdDgPointString2d)pObject;
        OdGePoint2d point = new OdGePoint2d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        writeFieldValue("Continuous", element.getContinuousFlag());

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Number of vertices: {0}\n", i), point);

            writeFieldValue(string.Format("Rotation {0}", i), element.getRotationAt(i));
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgPointString2d";
    }
}
class OdDgPointString3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgPointString3d element = (OdDgPointString3d)pObject;
        OdGePoint3d point = new OdGePoint3d();
        uint i, j = element.getVerticesCount();
        startDescription(element);
        writeShift();
        Program.DumpStream.WriteLine(string.Format("Number of vertices: {0}\n", j));

        writeFieldValue("Continuous", element.getContinuousFlag());

        for (i = 0; i < j; i++)
        {
            point = element.getVertexAt(i);
            writeFieldValue(string.Format("Number of vertices: {0}\n", i), point);

            writeFieldValue(string.Format("Rotation {0}", i), element.getRotationAt(i));
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgPointString3d";
    }
}
class OdDgDimension_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgDimension element = (OdDgDimension)pObject;
        startDescription(element);
        writeFieldValue("Tool type", element.getDimensionType());
        writeFieldValue("Scale", element.getScaleFactor());
        writeFieldValueHex("Flags", element.getFlags());
    writeShift();
    switch(element.getAlignment())
    {
    case OdDgDimension.PlacementAlignment.kPaArbitrary:
      Program.DumpStream.WriteLine("Placement Alignment: kPaArbitrary\n" );
      break;
    case OdDgDimension.PlacementAlignment.kPaDrawing:
      Program.DumpStream.WriteLine("Placement Alignment: kPaDrawing\n" );
      break;
    case OdDgDimension.PlacementAlignment.kPaTrue:
      Program.DumpStream.WriteLine("Placement Alignment: kPaTrue\n" );
      break;
    case OdDgDimension.PlacementAlignment.kPaView:
      Program.DumpStream.WriteLine("Placement Alignment: kPaView\n" );
      writeShift();
      Program.DumpStream.WriteLine("Alignment View = %d\n", element.getAlignmentView() );
      break;
    };
    writeShift();
    switch(element.getArrowHead())
    {
    case OdDgDimension.TerminatorArrowHeadType.kOpened:
      Program.DumpStream.WriteLine("Arrow Head Type: kOpened\n" );
      break;
    case OdDgDimension.TerminatorArrowHeadType.kClosed:
      Program.DumpStream.WriteLine("Arrow Head Type: kClosed\n" );
      break;
    case OdDgDimension.TerminatorArrowHeadType.kFilled:
      Program.DumpStream.WriteLine("Arrow Head Type: kFilled\n" );
      break;
    };
    writeShift();
    if(element.Is3D())
    {
      OdGeQuaternion q = new OdGeQuaternion();
      element.getRotation(q);
      Program.DumpStream.WriteLine(string.Format("Quaternion: [ {0} {1} {2} {3} ]\n", q.w, q.x, q.y, q.z ));
    }
    else
    {
      OdGeMatrix2d q = new OdGeMatrix2d();
      element.getRotation(q);
      Program.DumpStream.WriteLine(string.Format("Matrix2d: [[ {0} {1} ] [ {2} {3} ]]\n", q.GetItem(0, 0), q.GetItem(0, 1), q.GetItem(1, 0), q.GetItem(1, 1)));
    }

    writeShift();
    Program.DumpStream.WriteLine("Flags:\n" );
    m_nesting++;

        bool CurFlag;
        CurFlag = element.getBoxTextFlag();
  if(CurFlag)
    {
      writeShift();
      Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag ));
    }
    CurFlag = element.getCapsuleTextFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getCentesimalFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getCrossCenterFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getDualFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getEmbedFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getHorizontalFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getJoinerFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getLeadingZero2Flag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getLeadingZerosFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getNoAutoTextLiftFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getNoLevelSymbFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getRelDimLineFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getRelStatFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getStackFractFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getTextHeapPadFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getThousandCommaFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getToleranceFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getTolmodeFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getTrailingZerosFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getTrailingZeros2Flag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }
    CurFlag = element.getUnderlineTextFlag();
    if (CurFlag)
    {
        writeShift();
        Program.DumpStream.WriteLine(string.Format("{0}\n", CurFlag));
    }

    UInt32      i, j = element.getPointsCount();
    OdDgDimPoint  point = new OdDgDimPoint();

    writeShift();
    Program.DumpStream.WriteLine(string.Format("Number of points: {0}\n", j ) );

    for( i = 0; i < j; i++ )
    {
      point = element.getPoint( i );
      writeFieldValue(string.Format("Point {0}\n", i), point);
    }

{
    OdDgDimTextInfo textInfo = new OdDgDimTextInfo();

    element.getDimTextInfo( textInfo );
    writeFieldValue( "Text info", textInfo );
  }

  //text format
  {
    OdDgDimTextFormat textFormat = new OdDgDimTextFormat();

    element.getDimTextFormat( textFormat );
    writeFieldValue( "Text format", textFormat );
  }

  //geometry
  {
    OdDgDimGeometry geometry = new OdDgDimGeometry();

    element.getDimGeometry( geometry );
    writeFieldValue( "Geometry", geometry );
  }

  // Symbology
  {
    Int32 altLineStyle = element.getAltLineStyleEntryId();
    UInt32 altLineWeight = element.getAltLineWeight();
    UInt32 altColorIndex = element.getAltColorIndex();

    writeFieldValue( "Alternative LineStyle", altLineStyle );
    writeFieldValue( "Alternative LineWeight", altLineWeight );
    writeFieldValue( "Alternative ColorIndex", altColorIndex );
  }

  // tools

  writeFieldValue("Tools:", element );

  //options
  
    UInt32 iOptionsCount = 0;
    if( element.getOption(OdDgDimOption.Type.kTolerance) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kTerminators) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kPrefixSymbol) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kSuffixSymbol) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kDiameterSymbol) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kPrefixSuffix) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kPrimaryUnits) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kSecondaryUnits) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kTerminatorSymbology) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kView) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kAlternatePrimaryUnits) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kOffset) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kAlternateSecondaryUnits) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kAlternatePrefixSymbol) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kAlternateSuffixSymbol) != null ) iOptionsCount++;      
      if( element.getOption(OdDgDimOption.Type.kProxyCell) != null ) iOptionsCount++;      

      writeShift();
      writeFieldValue( "Number of options", iOptionsCount );
      OdDgDimOption pDimOptions = new OdDgDimOption(IntPtr.Zero, true);
    iOptionsCount = 0;
      
      pDimOptions = element.getOption(OdDgDimOption.Type.kTolerance);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kTerminators);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kPrefixSymbol);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kSuffixSymbol);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kDiameterSymbol);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kPrefixSuffix);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kPrimaryUnits);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kSecondaryUnits);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kTerminatorSymbology);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kView);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kAlternatePrimaryUnits);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kOffset);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kAlternateSecondaryUnits);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kAlternatePrefixSymbol);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kAlternateSuffixSymbol);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }
      pDimOptions = element.getOption(OdDgDimOption.Type.kProxyCell);
    if(null != pDimOptions)                                
    {                                                          
    writeShift();                                            
    writeFieldValue( string.Format("Dimension option {0}", iOptionsCount), pDimOptions ); 
    iOptionsCount++; 
    }

      finishDescription();
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgDimension";
    }
}
class OdDgMultiline_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMultiline element = (OdDgMultiline)pObject;
        startDescription(element);
  writeFieldValue( "Origin cap angle", element.getOriginCapAngle() );
  writeFieldValue( "End cap angle", element.getEndCapAngle() );
  {
    OdGeVector3d vector;

    vector = element.getZVector();
    writeFieldValue( "Z vector", vector );
  }
  writeFieldValue( "Is closed", element.getClosedFlag() );

  //symbologies
  {
    OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

    element.getOriginCap( symbology );
    writeFieldValue( "Origin cap", symbology );
    element.getMiddleCap( symbology );
    writeFieldValue( "Joint cap", symbology );
    element.getEndCap( symbology );
    writeFieldValue( "End cap", symbology );
  }

  //points
  {
    UInt32            j = element.getPointsCount();
    OdDgMultilinePoint  point = new OdDgMultilinePoint();

    writeShift();
    Program.DumpStream.WriteLine(string.Format("Number of points: {0}\n", j ) );

    for(int i = 0; i < j; i++ )
    {
      element.getPoint( i, point );
      writeFieldValue( string.Format("Point {0}", i), point );
    }
  }

  //profiles
  {
    UInt32              j = element.getProfilesCount();
    OdDgMultilineProfile  profile = new OdDgMultilineProfile();

    writeShift();
    Program.DumpStream.WriteLine(string.Format("Number of profiles: %d\n", j ) );

    for(int i = 0; i < j; i++ )
    {
      element.getProfile( i, profile );
      writeFieldValue(string.Format("Profile {0}", i), profile);
    }
  }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMultiline";
    }
}
class OdDgBSplineCurve3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgBSplineCurve3d element = (OdDgBSplineCurve3d)pObject;

        startDescription( element );

        //simple fields
        writeFieldValue( "Order", element.getOrder() );
        writeFieldValue( "Display curve", element.getCurveDisplayFlag() );
        writeFieldValue( "Display polygon", element.getPolygonDisplayFlag() );
        writeFieldValue( "Closed", element.getClosedFlag() );

        if( element.hasFitData() )
        {
            writeFieldValue( "Construction type", "kDefinedByFitPoints" );
            writeFieldValue( "Natural tangents", element.getNaturalTangentsFlag() );
            writeFieldValue( "Chord length tangents", element.getChordLenTangentsFlag() );
            writeFieldValue( "Collinear tangents", element.getColinearTangentsFlag() );

            OdGePoint3dArray fitPoints = new OdGePoint3dArray();
            UInt32 uOrder = 0;
            bool bTangentExists = false;
            OdGeVector3d vrStartTangent = new OdGeVector3d();
            OdGeVector3d vrEndTangent = new OdGeVector3d();
            bool bUniformKnots = false;
            element.getFitData( fitPoints, out uOrder, out bTangentExists, vrStartTangent, vrEndTangent, out bUniformKnots );

            OdGeKnotVector vrKnots = element.getKnots();

            writeFieldValue("Num Fit Points", fitPoints.Count );

            for( int i = 0; i < fitPoints.Count; i++ )
            {
                String strFitPtsName = "  Point " + i.ToString();
                writeFieldValue( strFitPtsName, fitPoints[i] );
            }

            writeFieldValue("Start Tangent", vrStartTangent );
            writeFieldValue("End Tangent", vrEndTangent );
            writeFieldValue("Uniform Knots Flag", bUniformKnots );

            writeFieldValue("Num Knots", vrKnots.length());

            for( int j = 0; j < vrKnots.length(); j++ )
            {
                String strKnotName = "  Knot " + j.ToString();
                writeFieldValue( strKnotName, vrKnots[j] );
            }
        }
        else
        {
            writeFieldValue( "Construction type", "kDefinedByNurbsData" );
            writeFieldValue( "Rational", element.isRational() );

            OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
            OdGeKnotVector   vrKnots = new OdGeKnotVector();
            OdGeDoubleArray  arrWeights = new OdGeDoubleArray();
            UInt32 uOrder = 0;
            bool bClosed = false;
            bool bRational = false;

            element.getNurbsData( out uOrder, out bRational, out bClosed, arrCtrlPts, vrKnots, arrWeights );

            writeFieldValue("Num Control Points", arrCtrlPts.Count);

            for( int i = 0; i < arrCtrlPts.Count; i++ )
            {
                String strCtrlPtsName = "  Point " + i.ToString();
                writeFieldValue( strCtrlPtsName, arrCtrlPts[i] );
            }

            writeFieldValue("Num Knots", vrKnots.length() );

            for( int j = 0; j < vrKnots.length(); j++ )
            {
                String strKnotName = "  Knot " + j.ToString();
                writeFieldValue( strKnotName, vrKnots[j] );
            }

            if( bRational )
            {
                writeFieldValue("Num Weights", arrWeights.Count );

                for( int k = 0; k < arrWeights.Count; k++ )
                {
                    String strWeightName = "  Weight " + k.ToString();
                    writeFieldValue( strWeightName, arrWeights[k] );
                }
            }
        }

        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgBSplineCurve3d";
    }
}
class OdDgBSplineCurve2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgBSplineCurve2d element = (OdDgBSplineCurve2d)pObject;
        startDescription( element );
        writeFieldValue( "Order", element.getOrder() );
        writeFieldValue( "Display curve", element.getCurveDisplayFlag() );
        writeFieldValue( "Display polygon", element.getPolygonDisplayFlag() );
        writeFieldValue( "Closed", element.getClosedFlag() );

        if( element.hasFitData() )
        {
            writeFieldValue( "Construction type", "kDefinedByFitPoints" );
            writeFieldValue( "Natural tangents", element.getNaturalTangentsFlag() );
            writeFieldValue( "Chord length tangents", element.getChordLenTangentsFlag() );
            writeFieldValue( "Collinear tangents", element.getColinearTangentsFlag() );

            OdGePoint2dArray fitPoints = new OdGePoint2dArray();
            UInt32 uOrder = 0;
            bool bTangentExists = false;
            OdGeVector2d vrStartTangent = new OdGeVector2d();
            OdGeVector2d vrEndTangent = new OdGeVector2d();
            bool bUniformKnots = false;
            element.getFitData( fitPoints, out uOrder, out bTangentExists, vrStartTangent, vrEndTangent, out bUniformKnots );

            OdGeKnotVector vrKnots = element.getKnots();

            writeFieldValue("Num Fit Points", fitPoints.Count );

            for( int i = 0; i < fitPoints.Count; i++ )
            {
                string strFitPtsName = "  Point " + i.ToString();
                writeFieldValue( strFitPtsName, fitPoints[i] );
            }

            writeFieldValue("Start Tangent", vrStartTangent );
            writeFieldValue("End Tangent", vrEndTangent );
            writeFieldValue("Uniform Knots Flag", bUniformKnots );

            writeFieldValue("Num Knots", vrKnots.length());

            for( int j = 0; j < vrKnots.length(); j++ )
            {
                String strKnotName = "  Knot " + j.ToString();
                writeFieldValue( strKnotName, vrKnots[j] );
            }
        }
        else
        {
            writeFieldValue( "Construction type", "kDefinedByNurbsData" );
            writeFieldValue( "Rational", element.isRational() );

            OdGePoint2dArray arrCtrlPts = new OdGePoint2dArray();
            OdGeKnotVector   vrKnots = new OdGeKnotVector();
            OdGeDoubleArray  arrWeights = new OdGeDoubleArray();
            UInt32 uOrder = 0;
            bool bClosed = false;
            bool bRational = false;

            element.getNurbsData( out uOrder, out bRational, out bClosed, arrCtrlPts, vrKnots, arrWeights );

            writeFieldValue("Num Control Points", arrCtrlPts.Count);

            for( int i = 0; i < arrCtrlPts.Count; i++ )
            {
                String strCtrlPtsName = "  Point %d" + i.ToString();
                writeFieldValue( strCtrlPtsName, arrCtrlPts[i] );
            }

            writeFieldValue("Num Knots", vrKnots.length() );

            for( int j = 0; j < vrKnots.length(); j++ )
            {
                String strKnotName = "  Knot %d" + j.ToString();
                writeFieldValue( strKnotName, vrKnots[j] );
            }

            if( bRational )
            {
                writeFieldValue("Num Weights", arrWeights.Count );

                for( int k = 0; k < arrWeights.Count; k++ )
                {
                    String strWeightName = "  Weight %d" + k.ToString();
                    writeFieldValue( strWeightName, arrWeights[k] );
                }
            }
        }

        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgBSplineCurve2d";
    }
}
class OdDgSurface_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgSurface element = (OdDgSurface)pObject;
        startDescription(element);
        writeFieldValue("Type of surface", element.getType());

        //extract the structure
        OdDg3dObjectHelper helper = new OdDg3dObjectHelper(element);
        EntireObject structure = new EntireObject();
        helper.extractInformation(structure);

        Int32 currentBoundaryIndex = 0, boundariesAtAll = structure.Count;
        writeFieldValue("Number of boundaries", boundariesAtAll);

        m_nesting++;

        //iterate through boundaries
        for (; currentBoundaryIndex < boundariesAtAll; currentBoundaryIndex++)
        {
            OdDgGraphicsElementPtrArray boundaries = structure[currentBoundaryIndex].m_boundary, rules = structure[currentBoundaryIndex].m_rules;

            Int32 currentIndex, topIndex;

            writeShift();
            Program.DumpStream.WriteLine(string.Format("Boundary # {0}\n", currentBoundaryIndex));
            m_nesting++;
            topIndex = boundaries.Count;
            for (currentIndex = 0; currentIndex < topIndex; currentIndex++)
            {
                describeSubElement(boundaries[currentIndex]);
            }
            m_nesting--;

            writeShift();
            Program.DumpStream.WriteLine("Rules\n");
            m_nesting++;
            topIndex = rules.Count;
            for (currentIndex = 0; currentIndex < topIndex; currentIndex++)
            {
                describeSubElement(rules[currentIndex]);
            }
            m_nesting--;
        }

        m_nesting--;
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgSurface";
    }
}
class OdDgSolid_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgSolid element = (OdDgSolid)pObject;
        startDescription(element);
  writeFieldValue( "Type of solid", element.getType() );

  //extract the structure
  OdDg3dObjectHelper helper = new OdDg3dObjectHelper ( element );
  EntireObject structure = new EntireObject();
  helper.extractInformation( structure );
  
  Int32 currentBoundaryIndex = 0, boundariesAtAll = structure.Count;
  writeFieldValue( "Number of boundaries", boundariesAtAll );

  m_nesting++;

  //iterate through boundaries
  for( ; currentBoundaryIndex < boundariesAtAll; currentBoundaryIndex++ )
  {
    OdDgGraphicsElementPtrArray boundaries = structure[ currentBoundaryIndex ].m_boundary, rules = structure[ currentBoundaryIndex ].m_rules;

    Int32 currentIndex, topIndex;

    writeShift();
    Program.DumpStream.WriteLine(string.Format("Boundary # {0}\n", currentBoundaryIndex ));
    m_nesting++;
    topIndex = boundaries.Count;
    for( currentIndex = 0; currentIndex < topIndex; currentIndex++ )
    {
      describeSubElement( boundaries[ currentIndex ]);
    }
    m_nesting--;

    writeShift();
    Program.DumpStream.WriteLine("Rules\n");
    m_nesting++;
    topIndex = rules.Count;
    for( currentIndex = 0; currentIndex < topIndex; currentIndex++ )
    {
      describeSubElement( rules[ currentIndex ] );
    }
    m_nesting--;
  }

  m_nesting--;
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgSolid";
    }
}
class OdDgRasterAttachmentHeader_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRasterAttachmentHeader element = (OdDgRasterAttachmentHeader)pObject;
        startDescription(element);
  {
    OdGeExtents3d extents = new OdGeExtents3d();
    element.getGeomExtents( extents );
    writeFieldValue( "The result of getGeomExtents()", extents );
  }
  {
    OdGePoint3d origin = new OdGePoint3d();
    OdGeVector3d u = new OdGeVector3d();
    OdGeVector3d v = new OdGeVector3d();    //these values are logged later with OdDgRasterAttachmentComponentGeo object

    element.getOrientation( origin, u, v );
    writeFieldValue( "Origin", origin );
  }
  writeFieldValue( "Extent", element.getExtent() );
  writeFieldValue( "Display gamma", element.getDisplayGamma() );
  writeFieldValue( "Plot gamma", element.getPlotGamma() );
  writeFieldValue( "Apply rotation", element.getApplyRotationFlag() );
  writeFieldValue( "Clipping", element.getClippingFlag() );
  writeFieldValue( "Plot", element.getPlotFlag() );
  writeFieldValue( "Invert", element.getInvertFlag() );
  {
    for(int i = 1; i <= 8; i++ )
    {
      writeFieldValue( string.Format("View {0}", i), element.getViewFlag( i ) );
    }
  }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRasterAttachmentHeader";
    }
}
class OdDgRasterHeader2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRasterHeader2d element = (OdDgRasterHeader2d)pObject;
        startDescription(element);
        writeFieldValue("Right justified", element.getRightJustifiedFlag());
        writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
        writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
        writeFieldValue("Color", element.getColorFlag());
        writeFieldValue("Transparent", element.getTransparentFlag());
        writeFieldValue("Positive", element.getPositiveFlag());
        writeFieldValue("Raster format", element.getFormat());
        writeFieldValue("Foreground", element.getForeground());
        writeFieldValue("Background", element.getBackground());
        writeFieldValue("X extent", element.getXExtent());
        writeFieldValue("Y extent", element.getYExtent());
        writeFieldValue("Scale", element.getScale());
        {
            OdGePoint3d origin = element.getOrigin();
            writeFieldValue("Origin", origin);
        }

        //all parts
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRasterHeader2d";
    }
}
class OdDgRasterHeader3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRasterHeader3d element = (OdDgRasterHeader3d)pObject;
        startDescription(element);
        writeFieldValue("Right justified", element.getRightJustifiedFlag());
        writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
        writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
        writeFieldValue("Color", element.getColorFlag());
        writeFieldValue("Transparent", element.getTransparentFlag());
        writeFieldValue("Positive", element.getPositiveFlag());
        writeFieldValue("Raster format", element.getFormat());
        writeFieldValue("Foreground", element.getForeground());
        writeFieldValue("Background", element.getBackground());
        writeFieldValue("X extent", element.getXExtent());
        writeFieldValue("Y extent", element.getYExtent());
        writeFieldValue("Scale", element.getScale());
        {
            OdGePoint3d origin = element.getOrigin();
            writeFieldValue("Origin", origin);
        }

        //all parts
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRasterHeader3d";
    }
}
class OdDgRasterComponent_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRasterComponent element = (OdDgRasterComponent)pObject;
        startDescription(element);
        writeFieldValue("Right justified", element.getRightJustifiedFlag());
        writeFieldValue("Lower justified", element.getLowerJustifiedFlag());
        writeFieldValue("Horizontal data", element.getHorizontalDataFlag());
        writeFieldValue("Color", element.getColorFlag());
        writeFieldValue("Transparent", element.getTransparentFlag());
        writeFieldValue("Positive", element.getPositiveFlag());
        writeFieldValue("Raster format", element.getFormat());
        writeFieldValue("Foreground", element.getForeground());
        writeFieldValue("Background", element.getBackground());
        writeFieldValue("Offset X", element.getOffsetX());
        writeFieldValue("Offset Y", element.getOffsetY());
        writeFieldValue("Number of pixels", element.getNumberOfPixels());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRasterComponent";
    }
}
class OdDgTagElement_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTagElement element = (OdDgTagElement)pObject;
        startDescription(element);
  writeFieldValue( "Origin", element.getOrigin() );
  writeFieldValue( "Offset", element.getOffset() );
  writeFieldValue( "Rotation (3d)", element.getRotation3d() );
  writeFieldValue( "Associated", element.getAssociatedFlag() );
  writeFieldValue( "Offset is used", element.getOffsetUsedFlag() );
  {
    OdDgElementId setId = new OdDgElementId();
    ushort      definitionId;

    element.getTagDefinitionId( setId, out definitionId );
    writeFieldValue( "Id of the set", setId );
    writeFieldValue( "Entry Id of the definition", definitionId );
    OdDgTagDefinitionSet pTagSet =  OdDgTagDefinitionSet.cast(setId.safeOpenObject());
    if (null != pTagSet)
    {
      writeFieldValue( "TagDefinitionsSet Name", pTagSet.getName() );
      OdDgTagDefinition pTagDefinition = pTagSet.getByEntryId( definitionId );
      if (null != pTagDefinition)
      {
        writeFieldValue( "TagDefinition Name", pTagDefinition.getName());
        writeFieldValue( "TagDefinition Type", pTagDefinition.getType());
      }
    }

  }
  writeFieldValue( "Id of the associated element", element.getAssociationId() );
  writeFieldValue( "Freeze group", element.getFreezeGroup() );

  switch ( element.getDataType() )
  {
  case OdDgTagDefinition.Type.kChar:
    writeFieldValue( "Type", "char"  );
    writeFieldValue( "Value", element.getString() );
    break;
  case OdDgTagDefinition.Type.kInt16:
    writeFieldValue( "Type",  "int16"  );
    writeFieldValue( "Value", element.getInt16() );
    break;
  case OdDgTagDefinition.Type.kInt32:
    writeFieldValue( "Type",  "int32"  );
    writeFieldValue( "Value", element.getInt32() );
    break;
  case OdDgTagDefinition.Type.kDouble:
    writeFieldValue( "Type",  "int16"  );
    writeFieldValue( "Value", element.getDouble() );
    break;
  case OdDgTagDefinition.Type.kBinary:
    writeFieldValue( "Type",  "int16"  );
    writeFieldValue( "Size", element.getBinarySize() );
    break;
  default:
    //ODA_FAIL();
    break;
  }

  writeFieldValue( "Use interChar spacing", element.getUseInterCharSpacingFlag() );
  writeFieldValue( "Fixed width spacing", element.getFixedWidthSpacingFlag() );
  writeFieldValue( "Underlined", element.getUnderlineFlag() );
  writeFieldValue( "Use slant", element.getUseSlantFlag() );
  writeFieldValue( "Vertical", element.getVerticalFlag() );
  writeFieldValue( "Right-to-left", element.getRightToLeftFlag() );
  writeFieldValue( "Reverse MLine", element.getReverseMlineFlag() );
  writeFieldValue( "Kerning", element.getKerningFlag() );
  writeFieldValue( "Use codepage", element.getUseCodepageFlag() );
  writeFieldValue( "Use SHX big font", element.getUseShxBigFontFlag() );
  writeFieldValue( "Subscript", element.getSubscriptFlag() );
  writeFieldValue( "Superscript", element.getSuperscriptFlag() );
  writeFieldValue( "Use text style", element.getUseTextStyleFlag() );
  writeFieldValue( "Overlined", element.getOverlineFlag() );
  writeFieldValue( "Bold", element.getBoldFlag() );
  writeFieldValue( "Full justification", element.getFullJustificationFlag() );
  writeFieldValue( "ACAD interChar spacing", element.getAcadInterCharSpacingFlag() );
  writeFieldValue( "Backwards", element.getBackwardsFlag() );
  writeFieldValue( "Upside down", element.getUpsideDownFlag() );
  writeFieldValue( "ACAD fitted text", element.getAcadFittedTextFlag() );

  writeFieldValue( "Slant", element.getSlant() );
  writeFieldValue( "Character spacing", element.getCharacterSpacing() );
  writeFieldValue( "Underline spacing", element.getUnderlineSpacing() );
  writeFieldValue( "Length multiplier", element.getLengthMultiplier() );
  writeFieldValue( "Height multiplier", element.getHeightMultiplier() );

  writeFieldValue( "Text style ID", element.getTextStyleEntryId() );
  writeFieldValue( "SHX big font", element.getShxBigFont() );
  writeFieldValue( "Font ID", element.getFontEntryId() );
  writeFieldValue( "Justification", element.getJustification() );
  writeFieldValue( "Codepage", element.getCodepage() );
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTagElement";
    }
}
class OdDgCellHeader2d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgCellHeader2d element = (OdDgCellHeader2d)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgCellHeader2d";
    }
}
class OdDgCellHeader3d_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgCellHeader3d element = (OdDgCellHeader3d)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgCellHeader3d";
    }
}
class OdDgBSplineSurface_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgBSplineSurface element = (OdDgBSplineSurface)pObject;

        startDescription( element );

        byte uOrderU = 0;
        byte uOrderV = 0;
        bool  bRational  = false;
        bool  bClosedInU = false;
        bool  bClosedInV = false;
        int   nCtrlPtsInU = 0;
        int   nCtrlPtsInV = 0;
        OdGePoint3dArray arrCtrlPts = new OdGePoint3dArray();
        OdGeDoubleArray  arrWeights = new OdGeDoubleArray();
        OdGeKnotVector   arrKnotsU = new OdGeKnotVector();
        OdGeKnotVector   arrKnotsV = new OdGeKnotVector();
        uint nRulesU = 0;
        uint nRulesV = 0;

        element.get( out uOrderU, out uOrderV, out bRational, out bClosedInU, out bClosedInV, out nCtrlPtsInU, out nCtrlPtsInV, arrCtrlPts,
                    arrWeights, arrKnotsU, arrKnotsV );
        element.getNumberOfSpansInU( out nRulesU );
        element.getNumberOfSpansInV( out nRulesV );

        //simple fields
        writeFieldValue( "Order U",  uOrderU );
        writeFieldValue( "Order V",  uOrderV );
        writeFieldValue( "Closed U", bClosedInU );
        writeFieldValue( "Closed V", bClosedInV );
        writeFieldValue( "Display curve", element.getSurfaceDisplayFlag() );
        writeFieldValue( "Display polygon", element.getControlNetDisplayFlag() );
        writeFieldValue( "Rational", bRational );
        writeFieldValue( "Number of rules U", nRulesU );
        writeFieldValue( "Number of rules V", nRulesV );
        writeFieldValue( "Number of poles U", nCtrlPtsInU );
        writeFieldValue( "Number of poles V", nCtrlPtsInV );
        writeFieldValue( "Number of knots U", arrKnotsU.length() );
        writeFieldValue( "Number of knots V", arrKnotsV.length() );
        writeFieldValue( "Number of boundaries", element.getBoundariesCount() );
        writeFieldValue( "Hole", element.getHoleFlag() );

        for (uint nBoundariesCount = element.getBoundariesCount(), i = 0;
            i < nBoundariesCount;
            i++
            )
        {
            writeShift();
            //fwprintf( DumpStream, L"Boundary %d:\n", i );
            writeFieldValue("Boundary ", i.ToString());
            OdGePoint2dArray arrBoundaryPts = new OdGePoint2dArray();
            element.getBoundary(i, arrBoundaryPts);
            writeFieldValue( "Number of boundary vertices", arrBoundaryPts.Count );
            for (int BoundaryVerticesCount = arrBoundaryPts.Count, j = 0;
                    j < BoundaryVerticesCount;
                    j++
                    )
            {
                writeFieldValue( "Vertex", arrBoundaryPts[j] );
            }
        }

        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgBSplineSurface";
    }
}
class OdDgLevelTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLevelTable element = (OdDgLevelTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLevelTable";
    }
}
class OdDgLevelFilterTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLevelFilterTable element = (OdDgLevelFilterTable)pObject;
        startDescription(element);
        writeFieldValue("Active Filter ID", element.getActiveFilterId());

        writeShift();

        for (UInt32 i = 0; i < element.getFilterMemberCount(); i++)
        {
            OdDgLevelFilterTable.OdDgFilterMemberType iMemberType;
            String strMemberName = String.Empty;
            if (element.getFilterMember(i, ref strMemberName, out iMemberType))
            {
                writeFieldValue(string.Format("  {0}", strMemberName), iMemberType);
            }
        }

        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLevelFilterTable";
    }
}
class OdDgLevelTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLevelTableRecord pLevel = (OdDgLevelTableRecord)pObject;
        startDescription(pLevel);
  writeFieldValue( "Level Name ", pLevel.getName() );
  writeFieldValue( "Level Number", pLevel.getNumber() );

  writeFieldValue( "Entry ID", pLevel.getEntryId() );
  writeFieldValue( "Code", pLevel.getNumber() );

  writeFieldValue( "Element Color (byLevel)", pLevel.getElementColorIndex() );
  writeFieldValue( "Element LineStyle (byLevel)", pLevel.getElementLineStyleEntryId() );
  writeFieldValue( "Element LineWeight (byLevel)", pLevel.getElementLineWeight() );

  writeFieldValue( "Override Color", pLevel.getOverrideColorIndex() );
  writeFieldValue( "Override LineStyle", pLevel.getOverrideLineStyleEntryId() );
  writeFieldValue( "Override LineWeight", pLevel.getOverrideLineWeight() );

  writeFieldValue( "Use override color", pLevel.getUseOverrideColorFlag() );
  writeFieldValue( "Use override line style", pLevel.getUseOverrideLineStyleFlag() );
  writeFieldValue( "Use override line weight", pLevel.getUseOverrideLineWeightFlag() );

  writeFieldValue( "Displayed", pLevel.getIsDisplayedFlag() );
  writeFieldValue( "Can be Plotted", pLevel.getIsPlotFlag() );
  writeFieldValue( "Derived from a library level ", pLevel.getIsExternalFlag() );
  writeFieldValue( "Can be snapped ", pLevel.getIsSnapFlag() );
  writeFieldValue( "ReadOnly", pLevel.getIsReadOnlyFlag() );
  writeFieldValue( "Hidden", pLevel.getIsHiddenFlag() );
  writeFieldValue( "Frozen", pLevel.getIsFrozenFlag() );
  writeFieldValue( "CustomStyleFromMaster", pLevel.getIsCustomStyleFromMasterFlag() );
  writeFieldValue( "Displayed", pLevel.getIsDisplayedFlag() );

  //writeFieldValueHex( "Element access flags", pLevel.getElementAccessFlags() );
  OdDgLevelTableRecord.ElementAccess elementAccess = pLevel.getElementAccess();
  writeShift();
  Program.DumpStream.WriteLine("Element access" );
  switch( elementAccess )
  {
  case OdDgLevelTableRecord.ElementAccess.kAccessAll:      Program.DumpStream.WriteLine("All\n" ); break;
  case OdDgLevelTableRecord.ElementAccess.kAccessLocked:   Program.DumpStream.WriteLine("Locked\n" ); break;
  case OdDgLevelTableRecord.ElementAccess.kAccessReadOnly: Program.DumpStream.WriteLine("ReadOnly\n" ); break;
  case OdDgLevelTableRecord.ElementAccess.kAccessViewOnly: Program.DumpStream.WriteLine("ViewOnly\n"); break;
  }

  writeFieldValue( "Description", pLevel.getDescription() );
  writeFieldValue( "Priority", pLevel.getPriority() );
  writeFieldValue( "Transparency", pLevel.getTransparency() );
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLevelTableRecord";
    }
}
class OdDgLevelFilterTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLevelFilterTableRecord pFilter = (OdDgLevelFilterTableRecord)pObject;
        startDescription(pFilter);
        writeFieldValue("Filter Name ", pFilter.getName());
        writeFieldValue("Entry ID", pFilter.getEntryId());
        writeFieldValue("Parent ID", pFilter.getParentId());
        writeFieldValue("Filter Type", pFilter.getFilterType());
        writeFieldValue("Compose Or Flag", pFilter.getComposeOrFlag());

        writeShift();

        for (UInt32 i = 0; i < pFilter.getFilterMemberCount(); i++)
        {
            String strMemberName = pFilter.getFilterMemberName(i);
            String strMemberExpression = pFilter.getFilterMemberExpression(i);

            if (String.Empty != strMemberExpression)
            {
                strMemberName = "  " + strMemberName;

                writeFieldValue(strMemberName, strMemberExpression);
//                bAddData = true;
            }
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLevelFilterTableRecord";
    }
}
class OdDgFontTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgFontTable element = (OdDgFontTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgFontTable";
    }
}
class OdDgFontTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgFontTableRecord pFont = (OdDgFontTableRecord)pObject;
        startDescription(pFont);
        writeFieldValue("Font name", pFont.getName());
        writeFieldValue("Font number", pFont.getNumber());
        writeFieldValue("Entry ID", pFont.getNumber());
        writeFieldValue("Alternate Font name", pFont.getAlternateName());
        writeFieldValue("Font type", pFont.getType());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgFontTableRecord";
    }
}
class OdDgTextStyleTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTextStyleTable element = (OdDgTextStyleTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTextStyleTable";
    }
}
class OdDgTextStyleTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTextStyleTableRecord element = (OdDgTextStyleTableRecord)pObject;
        startDescription(element);
  writeFieldValue( "Text Style Name", element.getName() );
  writeFieldValue( "Entry ID", element.getEntryId() );

  writeFieldValue( "Font number", element.getFontEntryId() );
  // Gets Font name
  OdDgFontTable pFontTable = element.database().getFontTable();
  OdDgFontTableRecord pFont = pFontTable.getFont(element.getFontEntryId());
  if (null != pFont)
  {
    writeFieldValue( "Font Name", pFont.getName() );
  }

  writeFieldValue( "Justification", element.getJustification() );
  writeFieldValue( "Line length", element.getLineLength() );
  writeFieldValue( "Line offset", element.getLineOffset() );
  writeFieldValue( "Line spacing", element.getLineSpacing() );
  writeFieldValue( "Font number", element.getFontEntryId() );
  writeFieldValue( "Interchar spacing", element.getInterCharacter() );
  writeFieldValue( "Parent text style ID", element.getParentTextStyleEntryId() );
  writeFieldValue( "ShxBigFontId", element.getShxBigFontId() );
  writeFieldValue( "Slant", element.getSlant() );
  writeFieldValue( "Direction", element.getTextDirection() );
  writeFieldValue( "Height", element.getTextHeight() );
  writeFieldValue( "Width", element.getTextWidth() );
  writeFieldValue( "Node justification", element.getTextNodeJustification() );
  //writeFieldValue( "Is from table", element.getIsFromTableFlag() );

  writeFieldValue( "Underline", element.getUnderlineFlag() );
  writeFieldValue( "Overline", element.getOverlineFlag() );
  writeFieldValue( "Italic", element.getItalicsFlag() );
  writeFieldValue( "Bold", element.getBoldFlag() );
  writeFieldValue( "Superscript", element.getSuperscriptFlag() );
  writeFieldValue( "Subscript", element.getSubscriptFlag() );
  writeFieldValue( "Background flag", element.getBackgroundFlag() );
  writeFieldValue( "OverlineStyle", element.getOverlineStyleFlag() );
  writeFieldValue( "UnderlineStyle", element.getUnderlineStyleFlag() );
  writeFieldValue( "Fixed spacing", element.getFixedSpacingFlag() );
  writeFieldValue( "Fractions", element.getFractionsFlag() );
  writeFieldValue( "Color flag", element.getColorFlag() );
  writeFieldValue( "AcadIntercharSpacingFlag", element.getAcadIntercharSpacingFlag() );
  writeFieldValue( "FullJustificationFlag", element.getFullJustificationFlag() );
  writeFieldValue( "AcadShapeFileFlag", element.getAcadShapeFileFlag() );

  writeFieldValue( "Background border", element.getBackgroundBorder() );
  writeFieldValue( "Background fill color index", element.getBackgroundFillColorIndex() );
  writeFieldValue( "Background line color index", element.getBackgroundColorIndex() );
  writeFieldValue( "Background line style entry id", element.getBackgroundLineStyleEntryId() );
  writeFieldValue( "Background line weight", element.getBackgroundLineWeight() );
  writeFieldValue( "Overline line color", element.getOverlineColorIndex() );
  writeFieldValue( "Overline line style ID", element.getOverlineLineStyleEntryId() );
  writeFieldValue( "Overline line weight", element.getOverlineLineWeight() );
  writeFieldValue( "Overline offset", element.getOverlineOffset() );
  writeFieldValue( "Underline line color", element.getUnderlineColorIndex() );
  writeFieldValue( "Underline line style ID", element.getUnderlineLineStyleEntryId() );
  writeFieldValue( "Underline line weight", element.getUnderlineLineWeight() );
  writeFieldValue( "Underline offset", element.getUnderlineOffset() );
  writeFieldValue( "LineSpacinType offset", element.getLineSpacingType() );

  writeFieldValue( "Color index", element.getColorIndex() );
  if (element.getColorIndex() == (uint)ColorIndexConstants.kColorByLevel)
  {
    Program.DumpStream.WriteLine("TextStyle has ColorByLevel.\n" );
  }

  writeFieldValue( "ColorFlagOverrideFlag", element.getColorFlagOverrideFlag() );
  writeFieldValue( "TextColorOverrideFlag", element.getColorOverrideFlag() );
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTextStyleTableRecord";
    }
}
class OdDgDimStyleTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgDimStyleTable element = (OdDgDimStyleTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgDimStyleTable";
    }
}
class OdDgDimStyleTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgDimStyleTableRecord element = (OdDgDimStyleTableRecord)pObject;
        startDescription(element);
        writeFieldValue("Show dimension line", element.getShowDimLineFlag());
        writeFieldValue("Show ext line", element.getShowExtLineFlag());
        writeFieldValue("Show tolerance", element.getShowToleranceFlag());
        writeFieldValue("Main prefix", element.getMainPrefix());
        writeFieldValue("Main suffix", element.getMainSuffix());
        writeFieldValue("Tolerance prefix", element.getTolerancePrefix());
        writeFieldValue("Tolerance suffix", element.getToleranceSuffix());
        writeFieldValue("Upper prefix", element.getUpperPrefix());
        writeFieldValue("Upper suffix", element.getUpperSuffix());
        writeFieldValue("Lower prefix", element.getLowerPrefix());
        writeFieldValue("Lower suffix", element.getLowerSuffix());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgDimStyleTableRecord";
    }
}
class OdDgMultilineStyleTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMultilineStyleTable element = (OdDgMultilineStyleTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMultilineStyleTable";
    }
}
class OdDgMultilineStyleTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMultilineStyleTableRecord element = (OdDgMultilineStyleTableRecord)pObject;
        startDescription(element);
  writeFieldValue( "Uses fill color", element.getUseFillColorFlag() );
  writeFieldValue( "Fill color", element.getFillColorIndex() );
  writeFieldValue( "Origin cap angle", element.getOriginCapAngle() );
  writeFieldValue( "End cap angle", element.getEndCapAngle() );
  {
    OdDgMultilineSymbology symbology = new OdDgMultilineSymbology();

    element.getOriginCap( symbology );
    writeFieldValue( "Origin cap", symbology );
    element.getMiddleCap( symbology );
    writeFieldValue( "End cap", symbology );
    element.getEndCap( symbology );
    writeFieldValue( "Middle cap", symbology );
  }

  {
    OdDgMultilineProfile  profile = new OdDgMultilineProfile();
    uint                   i, j = element.getProfilesCount();

    writeShift();
    Program.DumpStream.WriteLine(string.Format("Number of profiles: {0}\n", j ) );

    for( i = 0; i < j; i++ )
    {
      element.getProfile( i, profile );
      writeFieldValue(string.Format("Profile {0}", i), profile);
    }
  }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMultilineStyleTableRecord";
    }
}
class OdDgLineStyleTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLineStyleTable element = (OdDgLineStyleTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLineStyleTable";
    }
}
class OdDgLineStyleTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgLineStyleTableRecord element = (OdDgLineStyleTableRecord)pObject;
        startDescription(element);
        writeFieldValue("LineStyle name", element.getName());
        writeFieldValue("EntryId", element.getEntryId());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgLineStyleTableRecord";
    }
}
class OdDgRegAppTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRegAppTable element = (OdDgRegAppTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRegAppTable";
    }
}
class OdDgRegAppTableRecord_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgRegAppTableRecord element = (OdDgRegAppTableRecord)pObject;
        startDescription(element);
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgRegAppTableRecord";
    }
}
class OdDgViewGroupTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgViewGroupTable element = (OdDgViewGroupTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgViewGroupTable";
    }
}
class OdDgViewGroup_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgViewGroup element = (OdDgViewGroup)pObject;
        startDescription(element);
        writeFieldValue("Name", element.getName());

        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgViewGroup";
    }
}
class OdDgView_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgView element = (OdDgView)pObject;
        startDescription(element);
  writeFieldValue( "Model ID", element.getModelId() );
 
  writeFieldValue( "Is named", element.isNamed() );
  writeFieldValue( "Name", element.getName() );

  writeFieldValue( "Index", element.getIndex() );

  writeFieldValue( "View rectangle", element.getViewRectangle() );

  writeFieldValue( "Is visible", element.getVisibleFlag() );
  writeFieldValue( "Fast curves", element.getFastCurveFlag() );
  writeFieldValue( "Hide texts", element.getHideTextsFlag() );
  writeFieldValue( "High quality fonts", element.getHighQualityFontsFlag() );
  writeFieldValue( "Show line weights", element.getShowLineWeightsFlag() );
  writeFieldValue( "Show patterns", element.getShowPatternsFlag() );
  writeFieldValue( "Show text nodes", element.getShowTextNodesFlag() );
  writeFieldValue( "Show data fields", element.getShowDataFieldsFlag() );
  writeFieldValue( "Show grid", element.getShowGridFlag() );
  writeFieldValue( "Show level symbology", element.getShowLevelSymbologyFlag() );
  writeFieldValue( "Show construction", element.getShowConstructionFlag() );
  writeFieldValue( "Show dimensions", element.getShowDimensionsFlag() );
  writeFieldValue( "Fast cells", element.getFastCellsFlag() );
  writeFieldValue( "Show fills", element.getShowFillsFlag() );
  writeFieldValue( "Show axis triad", element.getShowAxisTriadFlag() );
  writeFieldValue( "Show background", element.getShowBackgroundFlag() );
  writeFieldValue( "Show boundary", element.getShowBoundaryFlag() );

  if( element.getUseCameraFlag() )
  {
    writeShift();
    Program.DumpStream.WriteLine("Specific parameters for perspective camera:\n" );
    m_nesting++;

    OdGeMatrix3d rotation = new OdGeMatrix3d();
    OdGePoint3d position = new OdGePoint3d();
    OdGeExtents2d rectangle = new OdGeExtents2d();
    element.getCameraRotation( rotation );
    element.getCameraPosition( position );
    element.getCameraVisibleRectangle( rectangle );

    writeFieldValue( "Position", position );
    writeFieldValue( "Rotation", rotation );
    writeFieldValue( "Focal length", element.getCameraFocalLength() );
    writeFieldValue( "Visible rectangle", rectangle );
    writeFieldValue( "Front clipping", element.getCameraFrontClippingDistance() );
    writeFieldValue( "Back clipping", element.getCameraBackClippingDistance() );

    m_nesting--;
  }
  else
  {
    writeShift();
    Program.DumpStream.WriteLine("Specific parameters for orthography:\n" );
    m_nesting++;

    OdGeMatrix3d rotation = new OdGeMatrix3d();
    OdGeExtents3d box = new OdGeExtents3d();
    element.getOrthographyRotation( rotation );
    element.getOrthographyVisibleBox( box );

    writeFieldValue( "Rotation", rotation );
    writeFieldValue( "Visible box", box );

    m_nesting--;
  }

  OdDgElementIterator pIter = element.createIterator();
  for(pIter.start(); !pIter.done(); pIter.step())
  {
    OdDgElement SubElement = OdDgElement.cast(pIter.item().openObject());
    if(SubElement.isKindOf(OdDgLevelMask.desc()))
    {
      OdDgLevelMask levelmask = OdDgLevelMask.cast(SubElement);
      String  sLevelMask;
      UInt32 maxLevelEntryId = levelmask.getMaxLevelEntryId();

      OdDgElementIterator pIt = OdDgElementIterator.cast(element.database().getLevelTable().createIterator());
      for( ; !pIt.done(); pIt.step() )
      {
          OdDgLevelTableRecord pLevel = OdDgLevelTableRecord.cast(pIt.item().safeOpenObject());
        UInt32 levelEntryId = pLevel.getEntryId();
        sLevelMask = string.Format("LevelMask {0}", levelEntryId);
        bool levelIsVisible = true;
        if ( levelEntryId <= maxLevelEntryId || levelEntryId == 64 )
        {
          levelIsVisible = levelmask.getLevelIsVisible( levelEntryId );
          writeFieldValue( sLevelMask, levelIsVisible );
        }
        else
        {
          writeFieldValue( sLevelMask, "Unexpected" );
        }
      }
    }
  }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgView";
    }
}
class OdDgNamedViewTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgNamedViewTable element = (OdDgNamedViewTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgNamedViewTable";
    }
}
class OdDgSharedCellDefinitionTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgSharedCellDefinitionTable element = (OdDgSharedCellDefinitionTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgSharedCellDefinitionTable";
    }
}
class OdDgSharedCellDefinition_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgSharedCellDefinition element = (OdDgSharedCellDefinition)pObject;
        startDescription(element);
        writeFieldValue("Name", element.getName());
        writeFieldValue("Description", element.getDescription());

        writeFieldValue("Origin", element.getOrigin());

        describeSubElements(element.createIterator());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgSharedCellDefinition";
    }
}
class OdDgSharedCellReference_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgSharedCellReference element = (OdDgSharedCellReference)pObject;
        startDescription(element);
        writeFieldValue("Name of definition", element.getDefinitionName());

        writeFieldValue("Transformation", element.getTransformation());
        writeFieldValue("Origin", element.getOrigin());

        writeFieldValueHex("Class map", element.getClassMap());

        writeFieldValue("Override level", element.getLevelOverrideFlag());
        writeFieldValue("Override relative", element.getRelativeOverrideFlag());
        writeFieldValue("Override class", element.getClassOverrideFlag());
        writeFieldValue("Override color", element.getColorOverrideFlag());
        writeFieldValue("Override weight", element.getWeightOverrideFlag());
        writeFieldValue("Override style", element.getStyleOverrideFlag());
        writeFieldValue("Override associative point", element.getAssociativePointOverrideFlag());

        writeFieldValue("ColorIndex", element.getColorIndex());
        writeFieldValue("LineWeight", element.getLineWeight());
        writeFieldValue("LevelEntryId", element.getLevelEntryId());
        UInt32 level = element.getLevelEntryId();
        OdDgElementId idLevel = element.database().getLevelTable().getAt(level);
        if (idLevel.isNull())
        {
            OdDgLevelTableRecord pLevel = OdDgLevelTableRecord.cast(idLevel.safeOpenObject());
            writeFieldValue("Level Name ", pLevel.getName());
        } 
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgSharedCellReference";
    }
}
class OdDgTagDefinitionSetTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTagDefinitionSetTable element = (OdDgTagDefinitionSetTable)pObject;
        startDescription(element);
        describeSubElements(element.createIterator());  
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTagDefinitionSetTable";
    }
}
class OdDgTagDefinitionSet_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTagDefinitionSet pTagDefinitionsSet = (OdDgTagDefinitionSet)pObject;
        startDescription(pTagDefinitionsSet);
  writeFieldValue( "Name", pTagDefinitionsSet.getName() );
  {
    UInt32                        i, j = pTagDefinitionsSet.getCount();
    OdRxObject                   CurObject = null;
    OdRxObject_Dumper dumper = new OdRxObject_Dumper();

    writeFieldValue( "Number of definitions", j );
    for( i = 0; i < j; i++ )
    {
      CurObject = pTagDefinitionsSet.getByIndex( i );

      //MUshakov, 26/05/2010.
      //At the moment, tag definition sets can wrap some information for SmartSolid objects.
      //In this case, they collect OdDgTagDefinitionDgnStore elements.
      //The latters have no dumpers, so let us avoid it without exceptions 'no dumper'.
      dumper = Program.GetProperType(CurObject);
      if(null != dumper )
      {
        dumper.dump( CurObject );
      }
      else
      {
        m_nesting++;
        Program.DumpStream.WriteLine("Object does not have its dumper\n" );
        m_nesting--;
      }
    }
  }        finishDescription();
  MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTagDefinitionSet";
    }
}
class OdDgTagDefinition_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgTagDefinition element = (OdDgTagDefinition)pObject;
        startDescription(element);
  writeFieldValue( "Type", element.getType() );
  writeFieldValue( "ID", element.getEntryId() );
  writeFieldValue( "Name", element.getName() );
  writeFieldValue( "Prompt", element.getPrompt() );
  switch( element.getType() )
  {
  case OdDgTagDefinition.Type.kChar:
    writeFieldValue( "Default char value", element.getString() );
    break;
  case OdDgTagDefinition.Type.kInt16 :
    writeFieldValue( "Default int16 value", element.getInt16() );
    break;
  case OdDgTagDefinition.Type.kInt32 :
    writeFieldValue( "Default int32 value", element.getInt32() );
    break;
  case OdDgTagDefinition.Type.kDouble:
    writeFieldValue( "Default double value", element.getDouble() );
    break;
  case OdDgTagDefinition.Type.kBinary:
    writeFieldValue( "Default binary data (size)", element.getBinarySize() );
    break;
  default:
    //ODA_FAIL();
    break;
  }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgTagDefinition";
    }
}
class OdDgColorTable_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgColorTable element = (OdDgColorTable)pObject;
        startDescription(element);
        for (int i = 0; i < 256; i++)
        {
            uint color = element.lookupRGB((uint)i);
            Int32 red = (int)color & 0xff;// ODGETRED(color);
            Int32 green = (int)(color >> 8) & 0xff;
            Int32 blue = (int)(color >> 16) & 0xff;

            //sprintf(name, "Color %i - %i, %i, %i", i, red, green, blue);
            writeFieldValue(string.Format("Color {0} - {1}, {2}, {3}", i, red, green, blue), element.lookupRGB((uint)i) /*OdUInt32( i )*/ );
        }
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgColorTable";
    }
}
class OdDgReferenceHeader_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgReferenceAttachmentHeader element = (OdDgReferenceAttachmentHeader)pObject;
        startDescription(element);
  writeFieldValue( "The full reference path", element.getFullFileName() );
  writeFieldValue( "The base file name", element.getFileName() );
  writeFieldValue( "ModelName", element.getModelName() );
  writeFieldValue( "LogicalName", element.getLogicalName() );
  writeFieldValue( "MasterOrigin", element.getMasterOrigin() );
  writeFieldValue( "ReferenceOrigin", element.getReferenceOrigin() );
  writeFieldValue( "Transformation", element.getTransformation() );

  writeFieldValue( "Description", element.getDescription() );
  writeFieldValue( "FileNumber", element.getFileNumber() );
  writeFieldValue( "Priority", element.getPriority() );
  writeFieldValue( "BaseNestDepth", element.getBaseNestDepth() );
  writeFieldValue( "NestDepth", element.getNestDepth() );
  writeFieldValue( "Scale", element.getScale() );
  //writeFieldValue( "Entire Scale", element.getEntireScale() );
  writeFieldValue( "Foreign Unit ", (UInt16)(element.getForeignUnits()));
  writeFieldValue( "ZFront", element.getZFront() );
  writeFieldValue( "ZBack", element.getZBack() );
  writeFieldValue( "CameraPosition", element.getCameraPosition() );
  writeFieldValue( "CameraFocalLength", element.getCameraFocalLength() );

  writeFieldValue( "ClipPointsCount", element.getClipPointsCount() );
  for (UInt32 i = 0, nCount = element.getClipPointsCount(); i < nCount; i++)
  {
    writeShift();
    writeFieldValue( string.Format("ClipPoint {0}", i), element.getClipPoint( i ) );
  }

  writeFieldValue( "CoincidentFlag", element.getCoincidentFlag() );
  writeFieldValue( "SnapLockFlag", element.getSnapLockFlag() );
  writeFieldValue( "LocateLockFlag", element.getLocateLockFlag() );
  writeFieldValue( "CompletePathInV7Flag", element.getCompletePathInV7Flag() );
  writeFieldValue( "AnonymousFlag", element.getAnonymousFlag() );
  writeFieldValue( "InactiveFlag", element.getInactiveFlag() );
  writeFieldValue( "MissingFileFlag", element.getMissingFileFlag() );
  writeFieldValue( "LevelOverride", element.getLevelOverride() );
  writeFieldValue( "DontDetachOnAllFlag", element.getDontDetachOnAllFlag() );
  writeFieldValue( "MetadataOnlyFlag", element.getMetadataOnlyFlag() );
  writeFieldValue( "DisplayFlag", element.getDisplayFlag() );
  writeFieldValue( "LineStyleScaleFlag", element.getLineStyleScaleFlag() );
  writeFieldValue( "HiddenLineFlag", element.getHiddenLineFlag() );
  writeFieldValue( "DisplayHiddenLinesFlag", element.getDisplayHiddenLinesFlag() );
  writeFieldValue( "RotateClippingFlag", element.getRotateClippingFlag() );
  writeFieldValue( "ExtendedRefFlag", element.getExtendedRefFlag() );
  writeFieldValue( "ClipBackFlag", element.getClipBackFlag() );
  writeFieldValue( "ClipFrontFlag", element.getClipFrontFlag() );
  writeFieldValue( "CameraOnFlag", element.getCameraOnFlag() );
  writeFieldValue( "TrueScaleFlag", element.getTrueScaleFlag() );
  writeFieldValue( "DisplayBoundaryFlag", element.getDisplayBoundaryFlag() );
  writeFieldValue( "LibraryRefFlag", element.getLibraryRefFlag() );
  writeFieldValue( "DisplayRasterRefsFlag", element.getDisplayRasterRefsFlag() );
  writeFieldValue( "UseAlternateFileFlag", element.getUseAlternateFileFlag() );
  writeFieldValue( "UseLightsFlag", element.getUseLightsFlag() );
  writeFieldValue( "DoNotDisplayAsNestedFlag", element.getDoNotDisplayAsNestedFlag() );
  writeFieldValue( "ColorTableUsage", element.getColorTableUsage() );
  writeFieldValue( "ViewportFlag", element.getViewportFlag() );
  writeFieldValue( "ScaleByStorageUnitsFlag", element.getScaleByStorageUnitsFlag() );
  writeFieldValue( "PrintColorAdjustmentFlag", element.getPrintColorAdjustmentFlag() );
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgReferenceAttachmentHeader";
    }
}
class OdDgMeshFaceLoops_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshFaceLoops element = (OdDgMeshFaceLoops)pObject;
        startDescription(element);
        writeFieldValue("Number of faces", element.getFacesNumber());
        writeFieldValue("Number of points", element.getPointsNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshFaceLoops";
    }
}
class OdDgMeshPointCloud_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshPointCloud element = (OdDgMeshPointCloud)pObject;
        startDescription(element);
        writeFieldValue("Number of points", element.getPointsNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshPointCloud";
    }
}
class OdDgMeshTriangleList_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshTriangleList element = (OdDgMeshTriangleList)pObject;
        startDescription(element);
        writeFieldValue("Number of triangles", element.getTrianglesNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshTriangleList";
    }
}
class OdDgMeshQuadList_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshQuadList element = (OdDgMeshQuadList)pObject;
        startDescription(element);
        writeFieldValue("Number of quads", element.getQuadsNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshQuadList";
    }
}
class OdDgMeshTriangleGrid_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshTriangleGrid element = (OdDgMeshTriangleGrid)pObject;
        startDescription(element);
        writeFieldValue("Number of rows", element.getRowsNumber());
        writeFieldValue("Size of row", element.getColumnsNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshTriangleGrid";
    }
}
class OdDgMeshQuadGrid_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgMeshQuadGrid element = (OdDgMeshQuadGrid)pObject;
        startDescription(element);
        writeFieldValue("Number of rows", element.getRowsNumber());
        writeFieldValue("Size of row", element.getColumnsNumber());
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgMeshQuadGrid";
    }
}
class OdDgProxyElement_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgProxyElement element = (OdDgProxyElement)pObject;
        startDescription(element);
        OdBinaryData data = new OdBinaryData();
        element.getData(data);

        Int32 i, j = data.Count;
        writeFieldValue("Data size", j);
        for (i = 0; i < j; i++)
        {
            if (0 == (i % 16))
            {
                if (0 != i)
                {
                    Program.DumpStream.WriteLine("\n");
                }
                writeShift();
                Program.DumpStream.WriteLine(string.Format("{0}: ", i));
            }
            Program.DumpStream.WriteLine(string.Format("{0} ", data[i]));
        }
        Program.DumpStream.WriteLine("\n");
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgProxyElement";
    }
}
class OdDgApplicationData_Dumper : OdRxObject_Dumper
{
    public override void dump(OdRxObject pObject)
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        OdDgApplicationData element = (OdDgApplicationData)pObject;
        startDescription(element);
        writeFieldValue("Signature", element.getSignature());

        OdBinaryData data = new OdBinaryData();
        element.getData(data);

        Int32 i, j = data.Count;
        writeFieldValue("Data size", j);
        for (i = 0; i < j; i++)
        {
            if (0 == (i % 16))
            {
                if (0 != i)
                {
                    Program.DumpStream.WriteLine("\n");
                }
                writeShift();
                Program.DumpStream.WriteLine(string.Format("{0}: ", i));
            }
            Program.DumpStream.WriteLine(string.Format("{0} ", data[i]));
        }
        Program.DumpStream.WriteLine("\n");
        finishDescription();
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override String getClassName()
    {
        return "OdDgApplicationData";
    }
}
class OdDgLineGripPointsPE : OdDgGripPointsPE
{
    public override OdResult getGripPoints(OdDgElement ent, OdGePoint3dArray gripPoints)
    {
        Console.WriteLine("OdDgLineGripPointsPE getGripPoints call");
        return base.getGripPoints(ent, gripPoints);
    }
    public override OdResult moveGripPointsAt(OdDgElement ent, OdIntArray indices, OdGeVector3d offset)
    {
        Console.WriteLine("OdDgLineGripPointsPE moveGripPointsAt call");
        return base.moveGripPointsAt(ent, indices, offset);
    }
    public override OdResult getStretchPoints(OdDgElement ent, OdGePoint3dArray stretchPoints)
    {
        Console.WriteLine("OdDgLineGripPointsPE getStretchPoints call");
        return base.getStretchPoints(ent, stretchPoints);
    }
    public override OdResult moveStretchPointsAt(OdDgElement ent, OdIntArray indices, OdGeVector3d offset)
    {
        Console.WriteLine("OdDgLineGripPointsPE moveStretchPointsAt call");
        return base.moveStretchPointsAt(ent, indices, offset);
    }
    public override OdResult getOsnapPoints(OdDgElement ent, OdDgElement.OsnapMode osnapMode, IntPtr gsSelectionMark, OdGePoint3d pickPoint, OdGePoint3d lastPoint, OdGeMatrix3d viewXform, OdGePoint3dArray snapPoints)
    {
        Console.WriteLine("OdDgLineGripPointsPE getOsnapPoints call");
        return base.getOsnapPoints(ent, osnapMode, gsSelectionMark, pickPoint, lastPoint, viewXform, snapPoints);
    }
};

class OdDgDumper : IDisposable
{
    public OdDgDumper()
    { 
        //OdRxObject_Dumper::rxInit();

        OdDgDatabase.desc().addX( OdRxObject_Dumper.desc(), m_databaseDumper );
        OdDgModel.desc().addX( OdRxObject_Dumper.desc(), m_modelDumper );
        
        OdDgLine2d.desc().addX( OdRxObject_Dumper.desc(), m_line2dDumper );
        OdDgLine3d.desc().addX( OdRxObject_Dumper.desc(), m_line3dDumper );
        //////////// test of grip points /////////////////////////////////////
        //OdDgLine2d.desc().addX(OdDgGripPointsPE.desc(), m_gripLine);
        //OdDgLine3d.desc().addX(OdDgGripPointsPE.desc(), m_gripLine);

        OdDgLineString2d.desc().addX( OdRxObject_Dumper.desc(), m_lineString2dDumper );
        OdDgLineString3d.desc().addX( OdRxObject_Dumper.desc(), m_lineString3dDumper );
        OdDgText2d.desc().addX( OdRxObject_Dumper.desc(), m_text2dDumper );
        OdDgText3d.desc().addX( OdRxObject_Dumper.desc(), m_text3dDumper );
        OdDgTextNode2d.desc().addX( OdRxObject_Dumper.desc(), m_textNode2dDumper );
        OdDgTextNode3d.desc().addX( OdRxObject_Dumper.desc(), m_textNode3dDumper );
        OdDgShape2d.desc().addX( OdRxObject_Dumper.desc(), m_shape2dDumper );
        OdDgShape3d.desc().addX( OdRxObject_Dumper.desc(), m_shape3dDumper );
        OdDgCurve2d.desc().addX( OdRxObject_Dumper.desc(), m_curve2dDumper );
        OdDgCurve3d.desc().addX( OdRxObject_Dumper.desc(), m_curve3dDumper );
        OdDgEllipse2d.desc().addX( OdRxObject_Dumper.desc(), m_ellipse2dDumper );
        OdDgEllipse3d.desc().addX( OdRxObject_Dumper.desc(), m_ellipse3dDumper );
        OdDgArc2d.desc().addX( OdRxObject_Dumper.desc(), m_arc2dDumper );
        OdDgArc3d.desc().addX( OdRxObject_Dumper.desc(), m_arc3dDumper );
        OdDgCone.desc().addX( OdRxObject_Dumper.desc(), m_coneDumper );
        OdDgComplexString.desc().addX( OdRxObject_Dumper.desc(), m_complexStringDumper );
        OdDgComplexShape.desc().addX( OdRxObject_Dumper.desc(), m_complexShapeDumper );
        OdDgPointString2d.desc().addX( OdRxObject_Dumper.desc(), m_pointString2dDumper );
        OdDgPointString3d.desc().addX( OdRxObject_Dumper.desc(), m_pointString3dDumper );
        OdDgDimension.desc().addX( OdRxObject_Dumper.desc(), m_dimensionDumper );
        OdDgMultiline.desc().addX( OdRxObject_Dumper.desc(), m_multilineDumper );
        OdDgBSplineCurve2d.desc().addX( OdRxObject_Dumper.desc(), m_bSplineCurve2dDumper );
        OdDgBSplineCurve3d.desc().addX( OdRxObject_Dumper.desc(), m_bSplineCurve3dDumper );
        OdDgSurface.desc().addX( OdRxObject_Dumper.desc(), m_surfaceDumper );
        OdDgSolid.desc().addX( OdRxObject_Dumper.desc(), m_solidDumper );
        OdDgRasterAttachmentHeader.desc().addX( OdRxObject_Dumper.desc(), m_rasterAttachmentHeaderDumper );
        OdDgRasterHeader2d.desc().addX( OdRxObject_Dumper.desc(), m_rasterHeader2dDumper );
        OdDgRasterHeader3d.desc().addX( OdRxObject_Dumper.desc(), m_rasterHeader3dDumper );
        OdDgRasterComponent.desc().addX( OdRxObject_Dumper.desc(), m_rasterComponentDumper );
        OdDgTagElement.desc().addX( OdRxObject_Dumper.desc(), m_tagElementDumper );
        OdDgCellHeader2d.desc().addX( OdRxObject_Dumper.desc(), m_cellHeader2dDumper );
        OdDgCellHeader3d.desc().addX( OdRxObject_Dumper.desc(), m_cellHeader3dDumper );
        OdDgBSplineSurface.desc().addX( OdRxObject_Dumper.desc(), m_bSplineSurfaceDumper );
        OdDgLevelTable.desc().addX( OdRxObject_Dumper.desc(), m_levelTableDumper );
        OdDgLevelTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_levelTableRecordDumper );
        OdDgLevelFilterTable.desc().addX( OdRxObject_Dumper.desc(), m_levelFilterTableDumper );
        OdDgLevelFilterTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_levelFilterTableRecordDumper );
        OdDgFontTable.desc().addX( OdRxObject_Dumper.desc(), m_fontTableDumper );
        OdDgFontTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_fontTableRecordDumper );
        OdDgTextStyleTable.desc().addX( OdRxObject_Dumper.desc(), m_textStyleTableDumper );
        OdDgTextStyleTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_textStyleTableRecordDumper );
        OdDgDimStyleTable.desc().addX( OdRxObject_Dumper.desc(), m_dimStyleTableDumper );
        OdDgDimStyleTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_dimStyleTableRecordDumper );
        OdDgMultilineStyleTable.desc().addX( OdRxObject_Dumper.desc(), m_multilineStyleTableDumper );
        OdDgMultilineStyleTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_multilineStyleTableRecordDumper );
        OdDgLineStyleTable.desc().addX( OdRxObject_Dumper.desc(), m_lineStyleTableDumper );
        OdDgLineStyleTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_lineStyleTableRecordDumper );
        OdDgRegAppTable.desc().addX( OdRxObject_Dumper.desc(), m_regAppTableDumper );
        OdDgRegAppTableRecord.desc().addX( OdRxObject_Dumper.desc(), m_regAppTableRecordDumper );
        OdDgViewGroupTable.desc().addX( OdRxObject_Dumper.desc(), m_viewGroupTableDumper );
        OdDgViewGroup.desc().addX( OdRxObject_Dumper.desc(), m_viewGroupDumper );
        OdDgView.desc().addX( OdRxObject_Dumper.desc(), m_viewDumper );
        OdDgNamedViewTable.desc().addX( OdRxObject_Dumper.desc(), m_namedViewTableDumper );
        OdDgSharedCellDefinitionTable.desc().addX( OdRxObject_Dumper.desc(), m_sharedCellDefinitionTableDumper );
        OdDgSharedCellDefinition.desc().addX( OdRxObject_Dumper.desc(), m_sharedCellDefinitionDumper );
        OdDgSharedCellReference.desc().addX( OdRxObject_Dumper.desc(), m_sharedCellReferenceDumper );
        OdDgTagDefinitionSetTable.desc().addX( OdRxObject_Dumper.desc(), m_tagSetDefinitionTableDumper );
        OdDgTagDefinitionSet.desc().addX( OdRxObject_Dumper.desc(), m_tagSetDefinitionDumper );
        OdDgTagDefinition.desc().addX( OdRxObject_Dumper.desc(), m_tagDefinitionDumper );
        OdDgColorTable.desc().addX( OdRxObject_Dumper.desc(), m_colorTableDumper );
        OdDgReferenceAttachmentHeader.desc().addX( OdRxObject_Dumper.desc(), m_referenceAttachmentDumper );
        OdDgMeshFaceLoops.desc().addX( OdRxObject_Dumper.desc(), m_meshFaceLoopsDumper );
        OdDgMeshPointCloud.desc().addX( OdRxObject_Dumper.desc(), m_meshPointCloudDumper );
        OdDgMeshTriangleList.desc().addX( OdRxObject_Dumper.desc(), m_meshTriangleListDumper );
        OdDgMeshQuadList.desc().addX( OdRxObject_Dumper.desc(), m_meshQuadListDumper );
        OdDgMeshTriangleGrid.desc().addX( OdRxObject_Dumper.desc(), m_meshTriangleGridDumper );
        OdDgMeshQuadGrid.desc().addX( OdRxObject_Dumper.desc(), m_meshQuadGridDumper );
        OdDgProxyElement.desc().addX( OdRxObject_Dumper.desc(), m_proxyDumper );
        OdDgApplicationData.desc().addX( OdRxObject_Dumper.desc(), m_applicationDataDumper );
    }
    public void Dispose() 
    {
        OdDgDatabase.desc().delX( OdRxObject_Dumper.desc());
        OdDgModel.desc().delX( OdRxObject_Dumper.desc());
        OdDgLine2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgLine3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgLineString2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgLineString3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgText2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgText3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgTextNode2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgTextNode3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgShape2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgShape3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgCurve2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgCurve3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgEllipse2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgEllipse3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgArc2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgArc3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgCone.desc().delX( OdRxObject_Dumper.desc());
        OdDgComplexString.desc().delX( OdRxObject_Dumper.desc());
        OdDgComplexShape.desc().delX( OdRxObject_Dumper.desc());
        OdDgPointString2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgPointString3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgDimension.desc().delX( OdRxObject_Dumper.desc());
        OdDgMultiline.desc().delX( OdRxObject_Dumper.desc());
        OdDgBSplineCurve2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgBSplineCurve3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgSurface.desc().delX( OdRxObject_Dumper.desc());
        OdDgSolid.desc().delX( OdRxObject_Dumper.desc());
        OdDgRasterAttachmentHeader.desc().delX( OdRxObject_Dumper.desc());
        OdDgRasterHeader2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgRasterHeader3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgRasterComponent.desc().delX( OdRxObject_Dumper.desc());
        OdDgTagElement.desc().delX( OdRxObject_Dumper.desc());
        OdDgCellHeader2d.desc().delX( OdRxObject_Dumper.desc());
        OdDgCellHeader3d.desc().delX( OdRxObject_Dumper.desc());
        OdDgBSplineSurface.desc().delX( OdRxObject_Dumper.desc());
        OdDgLevelTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgLevelTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgLevelFilterTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgLevelFilterTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgFontTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgFontTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgTextStyleTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgTextStyleTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgDimStyleTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgDimStyleTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgMultilineStyleTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgMultilineStyleTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgLineStyleTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgLineStyleTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgRegAppTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgRegAppTableRecord.desc().delX( OdRxObject_Dumper.desc());
        OdDgViewGroupTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgViewGroup.desc().delX( OdRxObject_Dumper.desc());
        OdDgView.desc().delX( OdRxObject_Dumper.desc());
        OdDgNamedViewTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgSharedCellDefinitionTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgSharedCellDefinition.desc().delX( OdRxObject_Dumper.desc());
        OdDgSharedCellReference.desc().delX( OdRxObject_Dumper.desc());
        OdDgTagDefinitionSetTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgTagDefinitionSet.desc().delX( OdRxObject_Dumper.desc());
        OdDgTagDefinition.desc().delX( OdRxObject_Dumper.desc());
        OdDgColorTable.desc().delX( OdRxObject_Dumper.desc());
        OdDgReferenceAttachmentHeader.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshFaceLoops.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshPointCloud.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshTriangleList.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshQuadList.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshTriangleGrid.desc().delX( OdRxObject_Dumper.desc());
        OdDgMeshQuadGrid.desc().delX( OdRxObject_Dumper.desc());
        OdDgProxyElement.desc().delX( OdRxObject_Dumper.desc());
        OdDgApplicationData.desc().delX( OdRxObject_Dumper.desc());

        //OdRxObject_Dumper::rxUninit();    
    }

    public void rootDump(OdRxObject database)  //root dumper
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
        //OdRxObject_Dumper database_dumper = (OdRxObject_Dumper)database;//OdRxObject_Dumper.cast(database);
        //String str = database.GetType().ToString();
        OdRxObject_Dumper database_dumper = Program.GetProperType(database);
        //String str = ((OdDgDatabase)database)..ToString();
        //run the ordinal method
        database_dumper.dump(database);
        // OdDgDatabase_Dumper ExDgnDump
        MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }

    private OdDgDatabase_Dumper m_databaseDumper = new OdDgDatabase_Dumper();
    private OdDgModel_Dumper m_modelDumper = new OdDgModel_Dumper();
    private OdDgLine2d_Dumper m_line2dDumper = new OdDgLine2d_Dumper();
    private OdDgLine3d_Dumper m_line3dDumper = new OdDgLine3d_Dumper();
    //private OdDgLineGripPointsPE m_gripLine = new OdDgLineGripPointsPE();
    private OdDgLineString2d_Dumper m_lineString2dDumper = new OdDgLineString2d_Dumper();
    private OdDgLineString3d_Dumper m_lineString3dDumper = new OdDgLineString3d_Dumper();
    private OdDgText2d_Dumper m_text2dDumper = new OdDgText2d_Dumper();
    private OdDgText3d_Dumper m_text3dDumper = new OdDgText3d_Dumper();
    private OdDgTextNode2d_Dumper m_textNode2dDumper = new OdDgTextNode2d_Dumper();
    private OdDgTextNode3d_Dumper m_textNode3dDumper = new OdDgTextNode3d_Dumper();
    private OdDgShape2d_Dumper m_shape2dDumper = new OdDgShape2d_Dumper();
    private OdDgShape3d_Dumper m_shape3dDumper = new OdDgShape3d_Dumper();
    private OdDgCurve2d_Dumper m_curve2dDumper = new OdDgCurve2d_Dumper();
    private OdDgCurve3d_Dumper m_curve3dDumper = new OdDgCurve3d_Dumper();
    private OdDgEllipse2d_Dumper m_ellipse2dDumper = new OdDgEllipse2d_Dumper();
    private OdDgEllipse3d_Dumper m_ellipse3dDumper = new OdDgEllipse3d_Dumper();
    private OdDgArc2d_Dumper m_arc2dDumper = new OdDgArc2d_Dumper();
    private OdDgArc3d_Dumper m_arc3dDumper = new OdDgArc3d_Dumper();
    private OdDgCone_Dumper m_coneDumper = new OdDgCone_Dumper();
    private OdDgComplexString_Dumper m_complexStringDumper = new OdDgComplexString_Dumper();
    private OdDgComplexShape_Dumper m_complexShapeDumper = new OdDgComplexShape_Dumper();
    private OdDgPointString2d_Dumper m_pointString2dDumper = new OdDgPointString2d_Dumper();
    private OdDgPointString3d_Dumper m_pointString3dDumper = new OdDgPointString3d_Dumper();
    private OdDgDimension_Dumper m_dimensionDumper = new OdDgDimension_Dumper();
    private OdDgMultiline_Dumper m_multilineDumper = new OdDgMultiline_Dumper();
    private OdDgBSplineCurve2d_Dumper m_bSplineCurve2dDumper = new OdDgBSplineCurve2d_Dumper();
    private OdDgBSplineCurve3d_Dumper m_bSplineCurve3dDumper = new OdDgBSplineCurve3d_Dumper();
    private OdDgSurface_Dumper m_surfaceDumper = new OdDgSurface_Dumper();
    private OdDgSolid_Dumper m_solidDumper = new OdDgSolid_Dumper();
    private OdDgRasterAttachmentHeader_Dumper m_rasterAttachmentHeaderDumper = new OdDgRasterAttachmentHeader_Dumper();
    private OdDgRasterHeader2d_Dumper m_rasterHeader2dDumper = new OdDgRasterHeader2d_Dumper();
    private OdDgRasterHeader3d_Dumper m_rasterHeader3dDumper = new OdDgRasterHeader3d_Dumper();
    private OdDgRasterComponent_Dumper m_rasterComponentDumper = new OdDgRasterComponent_Dumper();
    private OdDgTagElement_Dumper m_tagElementDumper = new OdDgTagElement_Dumper();
    private OdDgCellHeader2d_Dumper m_cellHeader2dDumper = new OdDgCellHeader2d_Dumper();
    private OdDgCellHeader3d_Dumper m_cellHeader3dDumper = new OdDgCellHeader3d_Dumper();
    private OdDgBSplineSurface_Dumper m_bSplineSurfaceDumper = new OdDgBSplineSurface_Dumper();
    private OdDgLevelTable_Dumper m_levelTableDumper = new OdDgLevelTable_Dumper();
    private OdDgLevelTableRecord_Dumper m_levelTableRecordDumper = new OdDgLevelTableRecord_Dumper();
    private OdDgLevelFilterTable_Dumper m_levelFilterTableDumper = new OdDgLevelFilterTable_Dumper();
    private OdDgLevelFilterTableRecord_Dumper m_levelFilterTableRecordDumper = new OdDgLevelFilterTableRecord_Dumper();
    private OdDgFontTable_Dumper m_fontTableDumper = new OdDgFontTable_Dumper();
    private OdDgFontTableRecord_Dumper m_fontTableRecordDumper = new OdDgFontTableRecord_Dumper();
    private OdDgTextStyleTable_Dumper m_textStyleTableDumper = new OdDgTextStyleTable_Dumper();
    private OdDgTextStyleTableRecord_Dumper m_textStyleTableRecordDumper = new OdDgTextStyleTableRecord_Dumper();
    private OdDgDimStyleTable_Dumper m_dimStyleTableDumper = new OdDgDimStyleTable_Dumper();
    private OdDgDimStyleTableRecord_Dumper m_dimStyleTableRecordDumper = new OdDgDimStyleTableRecord_Dumper();
    private OdDgMultilineStyleTable_Dumper m_multilineStyleTableDumper = new OdDgMultilineStyleTable_Dumper();
    private OdDgMultilineStyleTableRecord_Dumper m_multilineStyleTableRecordDumper = new OdDgMultilineStyleTableRecord_Dumper();
    private OdDgLineStyleTable_Dumper m_lineStyleTableDumper = new OdDgLineStyleTable_Dumper();
    private OdDgLineStyleTableRecord_Dumper m_lineStyleTableRecordDumper = new OdDgLineStyleTableRecord_Dumper();
    private OdDgRegAppTable_Dumper m_regAppTableDumper = new OdDgRegAppTable_Dumper();
    private OdDgRegAppTableRecord_Dumper m_regAppTableRecordDumper = new OdDgRegAppTableRecord_Dumper();
    private OdDgViewGroupTable_Dumper m_viewGroupTableDumper = new OdDgViewGroupTable_Dumper();
    private OdDgViewGroup_Dumper m_viewGroupDumper = new OdDgViewGroup_Dumper();
    private OdDgView_Dumper m_viewDumper = new OdDgView_Dumper();
    private OdDgNamedViewTable_Dumper m_namedViewTableDumper = new OdDgNamedViewTable_Dumper();
    private OdDgSharedCellDefinitionTable_Dumper m_sharedCellDefinitionTableDumper = new OdDgSharedCellDefinitionTable_Dumper();
    private OdDgSharedCellDefinition_Dumper m_sharedCellDefinitionDumper = new OdDgSharedCellDefinition_Dumper();
    private OdDgSharedCellReference_Dumper m_sharedCellReferenceDumper = new OdDgSharedCellReference_Dumper();
    private OdDgTagDefinitionSetTable_Dumper m_tagSetDefinitionTableDumper = new OdDgTagDefinitionSetTable_Dumper();
    private OdDgTagDefinitionSet_Dumper m_tagSetDefinitionDumper = new OdDgTagDefinitionSet_Dumper();
    private OdDgTagDefinition_Dumper m_tagDefinitionDumper = new OdDgTagDefinition_Dumper();
    private OdDgColorTable_Dumper m_colorTableDumper = new OdDgColorTable_Dumper();
    private OdDgReferenceHeader_Dumper m_referenceAttachmentDumper = new OdDgReferenceHeader_Dumper();
    private OdDgMeshFaceLoops_Dumper m_meshFaceLoopsDumper = new OdDgMeshFaceLoops_Dumper();
    private OdDgMeshPointCloud_Dumper m_meshPointCloudDumper = new OdDgMeshPointCloud_Dumper();
    private OdDgMeshTriangleList_Dumper m_meshTriangleListDumper = new OdDgMeshTriangleList_Dumper();
    private OdDgMeshQuadList_Dumper m_meshQuadListDumper = new OdDgMeshQuadList_Dumper();
    private OdDgMeshTriangleGrid_Dumper m_meshTriangleGridDumper = new OdDgMeshTriangleGrid_Dumper();
    private OdDgMeshQuadGrid_Dumper m_meshQuadGridDumper = new OdDgMeshQuadGrid_Dumper();
    private OdDgProxyElement_Dumper m_proxyDumper = new OdDgProxyElement_Dumper();
    private OdDgApplicationData_Dumper m_applicationDataDumper = new OdDgApplicationData_Dumper();
}
}

