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
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Teigha.Colors;
using Teigha.GraphicsInterface;
using Geometry = Teigha.Geometry;
using GraphicsInterface = Teigha.GraphicsInterface;
using DatabaseServices = Teigha.DatabaseServices;
using DbNurbSurface = Teigha.DatabaseServices.NurbSurface;
using System.IO;

namespace OdWriteExMgd
{
  public partial class DbFiller
  {
    static double OdaToRadian(double dAngle)
    {
      return dAngle * Math.PI / 180;
    }
    /************************************************************************/
    /* Populate the Database                                                */
    /*                                                                      */
    /* PaperSpace Viewports                                                 */
    /* Linetypes with embedded shapes, and custom linetypes                 */
    /*                                                                      */
    /************************************************************************/
    public void fillDatabase(Database pDb)
    {
      Console.WriteLine("\n\nPopulating the database...");

      /**********************************************************************/
      /* Set Creation and Last Update times                                 */
      /**********************************************************************/
      //DateTime date = new DateTime(2009, 1, 1, 12, 0, 0, 0);
      //date.ToUniversalTime();
      //pDb.Tducreate = date.ToUniversalTime();
      //pDb.Tduupdate = date.ToLocalTime();

      /**********************************************************************/
      /* Add some Registered Applications                                   */
      /**********************************************************************/
      addRegApp(pDb, "ODA");
      addRegApp(pDb, "AVE_FINISH"); // for materials

      /**********************************************************************/
      /* Add an SHX text style                                              */
      /**********************************************************************/
      ObjectId shxTextStyleId = addStyle(pDb, "OdaShxStyle", 0.0, 1.0, 0.2, 0.0, "txt");

      /**********************************************************************/
      /* Add a TTF text style                                               */
      /**********************************************************************/
      ObjectId ttfStyleId = addStyle(pDb, "OdaTtfStyle", 0.0, 1.0, 0.2, 0.0,
          "VERDANA.TTF", false, "Verdana", false, false, 0, 34);

      /**********************************************************************/
      /* Add a Shape file style for complex linetypes                       */
      /**********************************************************************/
      ObjectId shapeStyleId = addStyle(pDb, "", 0.0, 1.0, 0.2, 0.0, "ltypeshp.shx", true);

      /**********************************************************************/
      /* Add some linetypes                                                 */
      /**********************************************************************/
      addLinetypes(pDb, shapeStyleId, shxTextStyleId);

      /**********************************************************************/
      /* Add a Layer                                                        */
      /**********************************************************************/
      ObjectId odaLayer1Id = addLayer(pDb, "Oda Layer 1", 1/*TODO*/, "CONTINUOUS");

      /**********************************************************************/
      /* Add a block definition                                             */
      /**********************************************************************/
      ObjectId odaBlock1Id = addBlockDef(pDb, "OdaBlock1", 1, 2);

      /**********************************************************************/
      /* Add a DimensionStyle                                               */
      /**********************************************************************/
      ObjectId odaDimStyleId = addDimStyle(pDb, "OdaDimStyle");

      /**********************************************************************/
      /* Add an MLine style                                                 */
      /**********************************************************************/
      ObjectId odaMLineStyleId = addMLineStyle(pDb, "OdaStandard", "ODA Standard Style");

      /**********************************************************************/
      /* Add a Material                                                     */
      /**********************************************************************/
      ObjectId odaMaterialId = addMaterial(pDb, "OdaMateria", "ODA Defined Materia");

      /**********************************************************************/
      /* Add a PaperSpace Viewport                                          */
      /**********************************************************************/
      addPsViewport(pDb, odaLayer1Id);

      /**********************************************************************/
      /* Add ModelSpace Entity Boxes                                        */
      /**********************************************************************/
      using (BlockTable blockTable = (BlockTable)pDb.BlockTableId.GetObject(OpenMode.ForRead))
      {
        ObjectId modelSpaceId = blockTable[BlockTableRecord.ModelSpace];

        createEntityBoxes(modelSpaceId, odaLayer1Id);

        /**********************************************************************/
        /* Add some lines                                                     */
        /**********************************************************************/
        addLines(modelSpaceId, 0, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a 2D (heavy) polyline                                          */
        /**********************************************************************/
        add2dPolyline(modelSpaceId, 0, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a PolyFace Mesh                                                */
        /**********************************************************************/
        addPolyFaceMesh(modelSpaceId, 0, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a PolygonMesh                                                  */
        /**********************************************************************/
        addPolygonMesh(modelSpaceId, 0, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some curves                                                    */
        /**********************************************************************/
        addCurves(modelSpaceId, 0, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Tolerance                                                    */
        /**********************************************************************/
        addTolerance(modelSpaceId, 0, 5, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some Leaders                                                   */
        /**********************************************************************/
        addLeaders(modelSpaceId, 0, 6, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Aligned Dimension                                           */
        /**********************************************************************/
        addAlignedDimension(modelSpaceId, 0, 7, odaLayer1Id, ttfStyleId, odaDimStyleId);

        /**********************************************************************/
        /* Add a MultiLine                                                    */
        /**********************************************************************/
        addMLine(modelSpaceId, 0, 8, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Arc Dimension                                               */
        /**********************************************************************/
        addArcDimension(modelSpaceId, 0, 9, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a 3D Polyline                                                  */
        /**********************************************************************/
        add3dPolyline(modelSpaceId, 1, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add MText                                                          */
        /**********************************************************************/
        addMText(modelSpaceId, 1, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Block Reference                                                */
        /**********************************************************************/
        addBlockRef(modelSpaceId, 1, 2, odaLayer1Id, ttfStyleId, odaBlock1Id);

        /**********************************************************************/
        /* Add Radial Dimension                                               */
        /**********************************************************************/
        addRadialDimension(modelSpaceId, 1, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add 3D Face                                                        */
        /**********************************************************************/
        add3dFace(modelSpaceId, 1, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Hatches                                                        */
        /**********************************************************************/
        addHatches(modelSpaceId, 2, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some text entities to ModelSpace                               */
        /**********************************************************************/
        addTextEnts(modelSpaceId, 2, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Solid                                                          */
        /**********************************************************************/
        addSolid(modelSpaceId, 2, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Associative Rotated Dimension                               */
        /**********************************************************************/
        addDimAssoc(modelSpaceId, 2, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Ray                                                         */
        /**********************************************************************/
        addRay(modelSpaceId, 3, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a 3 Point Angular Dimension                                    */
        /**********************************************************************/
        add3PointAngularDimension(modelSpaceId, 3, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Ordinate Dimensions                                            */
        /**********************************************************************/
        addOrdinateDimensions(modelSpaceId, 3, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Spline                                                       */
        /**********************************************************************/
        addSpline(modelSpaceId, 3, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some Traces                                                    */
        /**********************************************************************/
        addTraces(modelSpaceId, 3, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Polyline                                                     */
        /**********************************************************************/
        addPolyline(modelSpaceId, 3, 5, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Wipeout                                                      */
        /**********************************************************************/
        addWipeout(modelSpaceId, 3, 7, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a RadialDimensionLarge                                         */
        /**********************************************************************/
        addRadialDimensionLarge(modelSpaceId, 3, 8, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a 2 Line Angular Dimension                                     */
        /**********************************************************************/
        add2LineAngularDimension(modelSpaceId, 3, 9, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an ACIS Solid                                                  */
        /**********************************************************************/
        addACIS(modelSpaceId, 1, 5, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Image                                                       */
        /**********************************************************************/
        addImage(modelSpaceId, 4, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add an Xref                                                        */
        /**********************************************************************/
        addXRef(modelSpaceId, 4, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Table                                                        */
        /**********************************************************************/
        addTable(modelSpaceId, odaBlock1Id, 4, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Diametric Dimension                                          */
        /**********************************************************************/
        addDiametricDimension(modelSpaceId, 4, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Shape                                                        */
        /**********************************************************************/
        addShape(modelSpaceId, 4, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a MInsert                                                      */
        /**********************************************************************/
        addMInsert(modelSpaceId, 4, 5, odaLayer1Id, ttfStyleId, odaBlock1Id);

        /**********************************************************************/
        /* Add an Xline                                                       */
        /**********************************************************************/
        addXline(modelSpaceId, 4, 6, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add custom objects                                                 */
        /**********************************************************************/
        addCustomObjects(pDb);

        /**********************************************************************/
        /* Add Text with Field                                                */
        /**********************************************************************/
        addTextWithField(modelSpaceId, 5, 0, odaLayer1Id, shxTextStyleId, ttfStyleId);

        /**********************************************************************/
        /* Add OLE object                                                     */
        /**********************************************************************/
        addOLE2FrameFromFile(modelSpaceId, 5, 1, "OdWriteEx.xls", odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Box                                                            */
        /**********************************************************************/
        addBox(modelSpaceId, 5, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Frustum                                                        */
        /**********************************************************************/
        addFrustum(modelSpaceId, 5, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Sphere                                                         */
        /**********************************************************************/
        addSphere(modelSpaceId, 5, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Torus                                                          */
        /**********************************************************************/
        addTorus(modelSpaceId, 5, 5, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Wedge                                                          */
        /**********************************************************************/
        addWedge(modelSpaceId, 5, 6, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Region                                                         */
        /**********************************************************************/
        addRegion(modelSpaceId, 5, 7, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Extrusion                                                      */
        /**********************************************************************/
        //addExtrusion(modelSpaceId, 6, 0, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Revolution                                                     */
        /**********************************************************************/
        //addSolRev(modelSpaceId, 6, 1, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Helix                                                          */
        /**********************************************************************/
        addHelix(modelSpaceId, 6, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Dwf Underlay                                                   */
        /**********************************************************************/
        addDwfUnderlay(modelSpaceId, 6, 3, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some MLeaders                                                  */
        /**********************************************************************/
        addMLeaders(modelSpaceId, 6, 4, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add some Lights                                                    */
        /**********************************************************************/
        addLights(modelSpaceId, 6, 6, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add Pdf Underlay                                                   */
        /**********************************************************************/
        addPdfUnderlay(modelSpaceId, 6, 7, odaLayer1Id, ttfStyleId);


        /**********************************************************************/
        /* Add some Surfaces                                                  */
        /**********************************************************************/
        addTrimmedNURBSurface(modelSpaceId, 7, 2, odaLayer1Id, ttfStyleId);

        /**********************************************************************/
        /* Add a Layout                                                       */
        /**********************************************************************/
        addLayout(pDb);

        // If preview bitmap is already available it can be specified to avoid wasting
        // time on generating it by DD
        if (File.Exists("preview.bmp"))
        {
          System.Drawing.Bitmap bmp = new System.Drawing.Bitmap("preview.bmp");
          pDb.ThumbnailBitmap = bmp;
        }
      }
    }

    void addCustomObjects(Database pDb)
    {
      //Open the main dictionary
      using (DBDictionary pMain = (DBDictionary)pDb.NamedObjectsDictionaryId.GetObject(OpenMode.ForWrite),
        // Create the new dictionary.
      pOdtDic = new DBDictionary())
      {
        // Add new dictionary to the main dictionary.
        ObjectId dicId = pMain.SetAt("Teigha_OBJECTS", pOdtDic);

        // Create a new xrecord object.
        using (Xrecord pXRec = new Xrecord())
        {
          // Add the xrecord the owning dictionary.
          ObjectId xrId = pOdtDic.SetAt("PROPERTIES_1", pXRec);

          ResultBuffer pRb = new ResultBuffer();
          pRb.Add(new TypedValue(1000, "Sample XRecord Data"));
          pRb.Add(new TypedValue(40, 3.14159));
          pRb.Add(new TypedValue(70, (short)312));

          pXRec.Data = pRb;

          pRb.Dispose();
        }
      }
    } //end addCustomObjects

    /************************************************************************/
    /* Add a Layer to the specified database                                */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    ObjectId addLayer(Database pDb, string name, short color, string linetype)
    {
      /**********************************************************************/
      /* Open the layer table                                               */
      /**********************************************************************/
      using (LayerTable pLayers = (LayerTable)pDb.LayerTableId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Create a layer table record                                        */
        /**********************************************************************/
        using (LayerTableRecord pLayer = new LayerTableRecord())
        {
          /**********************************************************************/
          /* Layer must have a name before adding it to the table.              */
          /**********************************************************************/
          pLayer.Name = name;

          /**********************************************************************/
          /* Set the Color.                                                     */
          /**********************************************************************/
          pLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, color);

          /**********************************************************************/
          /* Set the Linetype.                                                  */
          /**********************************************************************/
          using (LinetypeTable pLinetypes = (LinetypeTable)pDb.LinetypeTableId.GetObject(OpenMode.ForWrite))
          {
            ObjectId linetypeId = pLinetypes[linetype];
            pLayer.LinetypeObjectId = linetypeId;
          }

          /**********************************************************************/
          /* Add the record to the table.                                       */
          /**********************************************************************/
          return pLayers.Add(pLayer);
        }
      }
    }

    /************************************************************************/
    /* Add a Registered Application to the specified database               */
    /************************************************************************/
    bool addRegApp(Database pDb, string name)
    {
      return false;// pDb.newRegApp(name);
    }

    /************************************************************************/
    /* Add a Text Style to the specified database                           */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, false);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, isShapeFile, string.Empty);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile, string ttFaceName)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, isShapeFile, ttFaceName, false);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile, string ttFaceName, bool bold)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, isShapeFile, ttFaceName, bold,
                      false);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile, string ttFaceName, bool bold,
                      bool italic)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, isShapeFile, ttFaceName, bold,
                      italic, 0);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile, string ttFaceName, bool bold,
                      bool italic, int charset)
    {
      return addStyle(pDb, styleName, textSize,
                      xScale, priorSize, obliquing,
                      fileName, isShapeFile, ttFaceName, bold,
                      italic, charset, 0);
    }

    ObjectId addStyle(Database pDb, string styleName, double textSize,
                      double xScale, double priorSize, double obliquing,
                      string fileName, bool isShapeFile, string ttFaceName, bool bold,
                      bool italic, int charset, int pitchAndFamily)
    {
      ObjectId styleId = ObjectId.Null;

      using (TextStyleTable pStyles = (TextStyleTable)pDb.TextStyleTableId.GetObject(OpenMode.ForWrite))
      {
        using (TextStyleTableRecord pStyle = new TextStyleTableRecord())
        {
          // Name must be set before a table object is added to a table.  The
          // isShapeFile flag must also be set (if true) before adding the object
          // to the database.
          pStyle.Name = styleName;
          pStyle.IsShapeFile = isShapeFile;

          // Add the object to the table.
          styleId = pStyles.Add(pStyle);

          // Set the remaining properties.
          pStyle.TextSize = textSize;
          pStyle.XScale = xScale;
          pStyle.PriorSize = priorSize;
          pStyle.ObliquingAngle = obliquing;
          pStyle.FileName = fileName;
          if (isShapeFile)
            pStyle.PriorSize = 22.45;

          if (!string.IsNullOrEmpty(ttFaceName))
            pStyle.Font = new FontDescriptor(ttFaceName, bold, italic, charset, pitchAndFamily);

        }
      }
      return styleId;
    }

    /************************************************************************/
    /* Add a Linetype to the specified database                             */
    /*                                                                      */
    /* The symbol table and symbol table record are implicitly closed when  */
    /* this function returns.                                               */
    /************************************************************************/
    ObjectId addLinetype(Database pDb, string name, string comments)
    {
      /**********************************************************************/
      /* Open the Linetype table                                            */
      /**********************************************************************/
      using (LinetypeTable pLinetypes = (LinetypeTable)pDb.LinetypeTableId.GetObject(OpenMode.ForWrite))
      {
        using (LinetypeTableRecord pLinetype = new LinetypeTableRecord())
        {
          /**********************************************************************/
          /* Linetype must have a name before adding it to the table.           */
          /**********************************************************************/
          pLinetype.Name = name;

          /**********************************************************************/
          /* Add the record to the table.                                       */
          /**********************************************************************/
          ObjectId linetypeId = pLinetypes.Add(pLinetype);

          /**********************************************************************/
          /* Add the Comments.                                                  */
          /**********************************************************************/
          pLinetype.Comments = comments;

          return linetypeId;
        }
      }
    }

    /************************************************************************/
    /* Add Several linetypes to the specified database                      */
    /************************************************************************/
    void addLinetypes(Database pDb, ObjectId shapeStyleId, ObjectId txtStyleId)
    {
      /**********************************************************************/
      /* Continuous linetype                                                */
      /**********************************************************************/
      addLinetype(pDb, "Continuous2", "Solid Line");

      /**********************************************************************/
      /* Hidden linetype                                                    */
      /* This is not the standard Hidden linetype, but is used by examples  */
      /**********************************************************************/
      ObjectId ltId = addLinetype(pDb, "Hidden", "- - - - - - - - - - - - - - - - - - - - -");
      using (LinetypeTableRecord pLt = (LinetypeTableRecord)ltId.GetObject(OpenMode.ForWrite))
      {
        pLt.NumDashes = 2;
        pLt.PatternLength = 0.1875;
        pLt.SetDashLengthAt(0, 0.125);
        pLt.SetDashLengthAt(1, -0.0625);
      }

      /**********************************************************************/
      /* Linetype with text                                                 */
      /**********************************************************************/
      ltId = addLinetype(pDb, "HW_ODA", "__ HW __ OD __ HW __ OD __");
      using (LinetypeTableRecord pLt = (LinetypeTableRecord)ltId.GetObject(OpenMode.ForWrite))
      {
        pLt.NumDashes = 6;
        pLt.PatternLength = 1.8;
        pLt.SetDashLengthAt(0, 0.5);
        pLt.SetDashLengthAt(1, -0.2);
        pLt.SetDashLengthAt(2, -0.2);
        pLt.SetDashLengthAt(3, 0.5);
        pLt.SetDashLengthAt(4, -0.2);
        pLt.SetDashLengthAt(5, -0.2);

        pLt.SetShapeStyleAt(1, txtStyleId);
        pLt.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
        pLt.SetTextAt(1, "HW");
        pLt.SetShapeScaleAt(1, 0.5);

        pLt.SetShapeStyleAt(4, txtStyleId);
        pLt.SetShapeOffsetAt(4, new Vector2d(-0.1, -0.05));
        pLt.SetTextAt(4, "OD");
        pLt.SetShapeScaleAt(4, 0.5);
      }

      /**********************************************************************/
      /* ZIGZAG linetype                                                    */
      /**********************************************************************/
      ltId = addLinetype(pDb, "ZigZag", "/\\/\\/\\/\\/\\/\\/\\/\\");
      using (LinetypeTableRecord pLt = (LinetypeTableRecord)ltId.GetObject(OpenMode.ForWrite))
      {
        pLt.NumDashes = 4;
        pLt.PatternLength = 0.8001;
        pLt.SetDashLengthAt(0, 0.0001);
        pLt.SetDashLengthAt(1, -0.2);
        pLt.SetDashLengthAt(2, -0.4);
        pLt.SetDashLengthAt(3, -0.2);

        pLt.SetShapeStyleAt(1, shapeStyleId);
        pLt.SetShapeOffsetAt(1, new Vector2d(-0.2, 0.0));
        pLt.SetShapeNumberAt(1, 131); //ZIG shape
        pLt.SetShapeScaleAt(1, 0.2);

        pLt.SetShapeStyleAt(2, shapeStyleId);
        pLt.SetShapeOffsetAt(2, new Vector2d(0.2, 0.0));
        pLt.SetShapeNumberAt(2, 131); //ZIG shape
        pLt.SetShapeScaleAt(2, 0.2);
        pLt.SetShapeRotationAt(2, 3.1415926);
      }
    }

    //    /************************************************************************/
    //    /* Add a block definition to the specified database                     */
    //    /*                                                                      */
    //    /* Note that the BlockTable and BlockTableRecord are implicitly closed  */
    //    /* when before this function returns.                                   */
    //    /************************************************************************/
    //    ObjectId addBlock(Database pDb, 
    //                                    string name)
    //    {
    //      ObjectId            id;
    //      BlockTable       pTable  = pDb.getBlockTableId().GetObject(OpenMode.ForWrite);
    //      BlockTableRecord pRecord = BlockTableRecord();

    //      /**********************************************************************/
    //      /* Block must have a name before adding it to the table.              */
    //      /**********************************************************************/
    //      pRecord.setName(name);

    //      /**********************************************************************/
    //      /* Add the record to the table.                                       */
    //      /**********************************************************************/
    //      id = pTable.add(pRecord);
    //      return id;
    //    }

    /************************************************************************/
    /* Add a block reference to the specified BlockTableRecord              */
    /************************************************************************/
    ObjectId addInsert(BlockTableRecord bBTR, ObjectId btrId, double xscale, double yscale)
    {
      ObjectId brefId;
      /**********************************************************************/
      /* Add the block reference to the BlockTableRecord                    */
      /**********************************************************************/
      using (BlockReference pBlkRef = new BlockReference(Point3d.Origin, bBTR.ObjectId))
      {
        using (Database pDb = bBTR.Database)
          pBlkRef.SetDatabaseDefaults(pDb);
        brefId = bBTR.AppendEntity(pBlkRef);

        /**********************************************************************/
        /* Set some properties                                                */
        /**********************************************************************/
        pBlkRef.BlockTableRecord = btrId;
        pBlkRef.ScaleFactors = new Scale3d(xscale, yscale, 1.0);
        return brefId;
      }
    }

    /************************************************************************/
    /* Add a text entity with the specified attributes to the specified     */
    /* BlockTableRecord                                                     */
    /************************************************************************/
    ObjectId addTextEnt(BlockTableRecord bBTR, Point3d position, Point3d ap,
                        string str, double height, TextHorizontalMode hMode, TextVerticalMode vMode,
                        ObjectId layerId, ObjectId styleId)
    {
      return addTextEnt(bBTR, position, ap, str, height, hMode, vMode, layerId, styleId, null);
    }

    ObjectId addTextEnt(BlockTableRecord bBTR, Point3d position, Point3d ap,
                        string str, double height, TextHorizontalMode hMode, TextVerticalMode vMode,
                        ObjectId layerId, ObjectId styleId, Group pGroup)
    {
      /**********************************************************************/
      /* Create the text object                                             */
      /**********************************************************************/
      using (DBText pText = new DBText())
      {
        using (Database pDb = bBTR.Database)
          pText.SetDatabaseDefaults(pDb);
        ObjectId textId = bBTR.AppendEntity(pText);

        // Make the text annotative
        pText.Annotative = AnnotativeStates.True;

        /**********************************************************************/
        /* Add the text to the specified group                                */
        /**********************************************************************/
        if (pGroup != null)
          pGroup.Append(textId);

        /**********************************************************************/
        /* Set some properties                                                */
        /**********************************************************************/
        pText.Position = position;
        pText.AlignmentPoint = ap;
        pText.Height = height;
        pText.WidthFactor = 1.0;
        pText.TextString = str;
        pText.HorizontalMode = hMode;
        pText.VerticalMode = vMode;

        /**********************************************************************/
        /* Set the text to the specified style                                */
        /**********************************************************************/
        if (!styleId.IsNull)
          pText.TextStyleId = styleId;
        /**********************************************************************/
        /* Set the text to the specified layer                                */
        /**********************************************************************/
        if (!layerId.IsNull)
          pText.SetLayerId(layerId, false);

        return textId;
      }
    }

    /************************************************************************/
    /* Add a point entity with the specified attributes to the specified    */
    /* BlockTableRecord                                                     */
    /************************************************************************/
    ObjectId addPointEnt(BlockTableRecord bBTR, Point3d point, ObjectId layerId, Group pGroup)
    {
      /**********************************************************************/
      /* Create the point object                                             */
      /**********************************************************************/
      using (DBPoint pPoint = new DBPoint())
      {
        using (Database pDb = bBTR.Database)
          pPoint.SetDatabaseDefaults(pDb);
        ObjectId pointId = bBTR.AppendEntity(pPoint);

        /**********************************************************************/
        /* Set some properties                                                */
        /**********************************************************************/
        pPoint.Position = point;

        /**********************************************************************/
        /* Add the point to the specified group                               */
        /**********************************************************************/
        if (pGroup != null)
        {
          pGroup.Append(pointId);
        }
        /**********************************************************************/
        /* Set the point to the specified layer                               */
        /**********************************************************************/
        if (!layerId.IsNull)
        {
          pPoint.LayerId = layerId;
        }
        return pointId;
      }
    }

    /************************************************************************/
    /* Add some text entities to the specified BlockTableRecord             */
    /*                                                                      */
    /* The newly created entities are placed in a group                     */
    /************************************************************************/
    void addTextEnts(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        // We want to place all text items into a newly created group, so
        // open the group dictionary here.

        /**********************************************************************/
        /* Open the Group Dictionary                                          */
        /**********************************************************************/
        using (DBDictionary pGroupDic = (DBDictionary)btrId.Database.GroupDictionaryId.GetObject(OpenMode.ForWrite))
        {
          /**********************************************************************/
          /* Create a new Group                                                 */
          /**********************************************************************/
          Group pGroup = new Group();

          /**********************************************************************/
          /* Add it to the Group Dictionary                                     */
          /**********************************************************************/
          pGroupDic.SetAt("OdaGroup", pGroup);

          /**********************************************************************/
          /* Set some properties                                                 */
          /**********************************************************************/
          pGroup.Name = "OdaGroup";
          pGroup.Selectable = true;

          /**********************************************************************/
          /* Get the Lower-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          double dx = w / 16.0;
          double dy = h / 12.0;

          double textHeight = m_EntityBoxes.getHeight() / 12.0;

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "TEXT",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Add the text entities, and add them to the group                   */
          /*                                                                    */
          /* Show the relevant positions and alignment points                   */
          /**********************************************************************/
          Point3d position = point + new Vector3d(dx, dy * 9.0, 0.0);
          addPointEnt(bBTR, position, layerId, pGroup);
          addTextEnt(bBTR, position, position, "Left Text",
            textHeight, TextHorizontalMode.TextLeft, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          Point3d alignmentPoint = point + new Vector3d(w / 2.0, dy * 9.0, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Center Text",
            textHeight, TextHorizontalMode.TextCenter, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w - dx, dy * 9.0, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Right Text",
            textHeight, TextHorizontalMode.TextRight, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w / 2.0, dy * 8.0, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Middle Text",
            textHeight, TextHorizontalMode.TextMid, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          position = point + new Vector3d(dx, dy * 1, 0.0);
          alignmentPoint = point + new Vector3d(w - dx, dy, 0.0);
          addPointEnt(bBTR, position, layerId, pGroup);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, position, alignmentPoint, "Aligned Text",
            textHeight, TextHorizontalMode.TextAlign, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          position = point + new Vector3d(dx, dy * 5.5, 0.0);
          alignmentPoint = point + new Vector3d(w - dx, dy * 5.5, 0.0);
          addPointEnt(bBTR, position, layerId, pGroup);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, position, alignmentPoint, "Fit Text",
            textHeight, TextHorizontalMode.TextFit, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);


          /**********************************************************************/
          /* Start a new box                                                    */
          /**********************************************************************/
          point = m_EntityBoxes.getBox(boxRow, boxCol + 1);

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "TEXT",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;
          textHeight = h / 16.0;
          pGroup.Dispose();

          /**********************************************************************/
          /* Create a new anonymous Group                                       */
          /**********************************************************************/
          pGroup = new Group();

          /**********************************************************************/
          /* Add it to the Group Dictionary                                     */
          /**********************************************************************/
          pGroupDic.SetAt("*", pGroup);

          /**********************************************************************/
          /* Set some properties                                                 */
          /**********************************************************************/
          pGroup.Name = "*";
          pGroup.SetAnonymous();
          pGroup.Selectable = true;

          /**********************************************************************/
          /* Add the text entities, and add them to the group                   */
          /*                                                                    */
          /* Show the relevant positions and alignment points                   */
          /**********************************************************************/
          alignmentPoint = point + new Vector3d(dx, dy * 9.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Top Left",
            textHeight, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w / 2.0, dy * 9.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Top Center",
            textHeight, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w - dx, dy * 9.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Top Right",
            textHeight, TextHorizontalMode.TextRight, TextVerticalMode.TextTop, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(dx, dy * 7.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Middle Left",
            textHeight, TextHorizontalMode.TextLeft, TextVerticalMode.TextVerticalMid, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w / 2.0, dy * 7.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Middle Center",
            textHeight, TextHorizontalMode.TextCenter, TextVerticalMode.TextVerticalMid, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w - dx, dy * 7.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Middle Right",
            textHeight, TextHorizontalMode.TextRight, TextVerticalMode.TextVerticalMid, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(dx, dy * 5.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Baseline Left",
            textHeight, TextHorizontalMode.TextLeft, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w / 2.0, dy * 5.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Baseline Center",
            textHeight, TextHorizontalMode.TextCenter, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w - dx, dy * 5.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Baseline Right",
            textHeight, TextHorizontalMode.TextRight, TextVerticalMode.TextBase, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(dx, dy * 3.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Bottom Left",
            textHeight, TextHorizontalMode.TextLeft, TextVerticalMode.TextBottom, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w / 2.0, dy * 3.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Bottom Center",
            textHeight, TextHorizontalMode.TextCenter, TextVerticalMode.TextBottom, ObjectId.Null, styleId, pGroup);

          alignmentPoint = point + new Vector3d(w - dx, dy * 3.5, 0.0);
          addPointEnt(bBTR, alignmentPoint, layerId, pGroup);
          addTextEnt(bBTR, alignmentPoint, alignmentPoint, "Bottom Right",
            textHeight, TextHorizontalMode.TextRight, TextVerticalMode.TextBottom, ObjectId.Null, styleId, pGroup);
          pGroup.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Append a PolygonMesh vertex to the specified PolygonMesh             */
    /************************************************************************/
    void appendPgMeshVertex(PolygonMesh pPgMesh, Point3d pos)
    {
      /**********************************************************************/
      /* Append a Vertex to the PolyFaceMesh                                */
      /**********************************************************************/
      using (PolygonMeshVertex pVertex = new PolygonMeshVertex())
      {
        pPgMesh.AppendVertex(pVertex);

        /**********************************************************************/
        /* Set the properties                                                 */
        /**********************************************************************/
        pVertex.Position = pos;
      }
    }

    /************************************************************************/
    /* Append a PolyFaceMesh vertex to the specified PolyFaceMesh           */
    /************************************************************************/
    void appendPfMeshVertex(PolyFaceMesh pMesh, double x, double y, double z)
    {
      /**********************************************************************/
      /* Append a MeshVertex to the PolyFaceMesh                            */
      /**********************************************************************/
      using (PolyFaceMeshVertex pVertex = new PolyFaceMeshVertex())
      {
        pMesh.AppendVertex(pVertex);

        /**********************************************************************/
        /* Set the properties                                                 */
        /**********************************************************************/
        pVertex.Position = new Point3d(x, y, z);
      }
    }

    /************************************************************************/
    /* Append a FaceRecord to the specified PolyFaceMesh                    */
    /************************************************************************/
    void appendFaceRecord(PolyFaceMesh pMesh, short i1, short i2, short i3, short i4)
    {
      /**********************************************************************/
      /* Append a FaceRecord to the PolyFaceMesh                            */
      /**********************************************************************/
      using (FaceRecord pFr = new FaceRecord())
      {
        pMesh.AppendFaceRecord(pFr);

        /**********************************************************************/
        /* Set the properties                                                 */
        /**********************************************************************/
        pFr.SetVertexAt(0, i1);
        pFr.SetVertexAt(1, i2);
        pFr.SetVertexAt(2, i3);
        pFr.SetVertexAt(3, i4);
      }
    }

    /************************************************************************/
    /* Add an MLine Style to the specified database                         */
    /************************************************************************/
    ObjectId addMLineStyle(Database pDb, string name, string desc)
    {
      /**********************************************************************/
      /* Open the MLineStyle dictionary                                     */
      /**********************************************************************/
      using (DBDictionary pMLDic = (DBDictionary)pDb.MLStyleDictionaryId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Create an Mline Style                                              */
        /**********************************************************************/
        using (MlineStyle pStyle = new MlineStyle())
        {
          /**********************************************************************/
          /* Set some parameters                                                */
          /**********************************************************************/
          pStyle.Name = name;
          pStyle.Description = desc;
          pStyle.StartAngle = OdaToRadian(105.0);
          pStyle.EndAngle = OdaToRadian(75.0);
          pStyle.ShowMiters = true;
          pStyle.StartSquareCap = true;
          pStyle.EndSquareCap = true;

          /**********************************************************************/
          /* Get the object ID of the desired linetype                          */
          /**********************************************************************/
          using (LinetypeTable pLtTable = (LinetypeTable)pDb.LinetypeTableId.GetObject(OpenMode.ForRead))
          {
            ObjectId linetypeId = pLtTable["Hidden"];

            /**********************************************************************/
            /* Add some elements                                                  */
            /**********************************************************************/
            pStyle.Elements.Add(new MlineStyleElement(0.1, Color.FromRgb(255, 0, 0), linetypeId), true);
            pStyle.Elements.Add(new MlineStyleElement(0.0, Color.FromRgb(0, 255, 0), linetypeId), true);
          }
          /**********************************************************************/
          /* Update the MLine dictionary                                        */
          /**********************************************************************/
          return pMLDic.SetAt(name, pStyle);
        }
      }
    }

    /************************************************************************/
    /* Add a Material to the specified database                             */
    /************************************************************************/
    ObjectId addMaterial(Database pDb, string name, string desc)
    {
      /**********************************************************************/
      /* Open the Material dictionary                                     */
      /**********************************************************************/
      using (DBDictionary pMatDic = (DBDictionary)pDb.MaterialDictionaryId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Create a Material                                                  */
        /**********************************************************************/
        using (Material pMaterial = new Material())
        {
          /**********************************************************************/
          /* Set some parameters                                                */
          /**********************************************************************/
          pMaterial.Name = name;
          pMaterial.Description = desc;

          MaterialMap materialMap = new MaterialMap(Source.File, new MaterialTexture(), 0, null);
          using (MaterialColor materialColor = new MaterialColor(Method.Override, 0.75, new EntityColor(192, 32, 255)))
          {
            pMaterial.Ambient = materialColor;
            pMaterial.Bump = materialMap;
            pMaterial.Diffuse = new MaterialDiffuseComponent(materialColor, materialMap);
          }
          pMaterial.Opacity = new MaterialOpacityComponent(1.0, materialMap);
          pMaterial.Reflection = materialMap;
          pMaterial.Refraction = new MaterialRefractionComponent(1.0, materialMap);
          pMaterial.Translucence = 0.0;
          pMaterial.SelfIllumination = 0.0;
          pMaterial.Reflectivity = 0.0;
          pMaterial.Mode = Mode.Realistic;
          pMaterial.ChannelFlags = ChannelFlags.None;
          pMaterial.IlluminationModel = IlluminationModel.BlinnShader;

          using (MaterialColor materialColor = new MaterialColor(Method.Override, 1.0, new EntityColor(255, 255, 255)))
          {
            pMaterial.Specular = new MaterialSpecularComponent(materialColor, materialMap, 0.67);
          }
          /**********************************************************************/
          /* Update the Material dictionary                                        */
          /**********************************************************************/
          return pMatDic.SetAt(name, pMaterial);
        }
      }
    }

    /************************************************************************/
    /* Add some lines to the specified BlockTableRecord                     */
    /************************************************************************/
    void addLines(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Get the origin and size of the box                                 */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        //  double      w     = m_EntityBoxes.getWidth(boxRow, boxCol);
        //  double      h     = m_EntityBoxes.getHeight();

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "LINEs", m_textSize,
                    TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Get the center of the box                                          */
        /**********************************************************************/
        point = m_EntityBoxes.getBoxCenter(0, 0);

        /**********************************************************************/
        /* Add the lines that describe a 12 pointed star                      */
        /**********************************************************************/
        Vector3d toStart = Vector3d.XAxis;
        using (Database pDb = bBTR.Database)
        {
          for (int i = 0; i < 12; i++)
          {
            Line pLine = new Line();
            pLine.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pLine);
            pLine.StartPoint = point + toStart;
            pLine.EndPoint = point + toStart.RotateBy(OdaToRadian(160.0), Vector3d.ZAxis);
            pLine.Dispose();
          }
        }
      }
    }

    /************************************************************************/
    /* Add a 2D (heavy) polyline to the specified BlockTableRecord          */
    /************************************************************************/
    void add2dPolyline(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "2D POLYLINE", m_textSize,
            TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Add a 2dPolyline to the database                                   */
          /**********************************************************************/
          using (Polyline2d pPline = new Polyline2d())
          {
            pPline.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pPline);

            /**********************************************************************/
            /* Add the vertices                                                   */
            /**********************************************************************/
            Point3d pos = point;
            pos = pos + new Vector3d(w / 8, h / 8 - h, 0);

            double[,] width = new double[2, 4]
            {
              {0.0, w/12, w/4, 0.0},
              {w/4, w/12, 0.0, 0.0}
            };

            for (int i = 0; i < 4; i++)
            {
              Vertex2d pVertex = new Vertex2d();
              pVertex.SetDatabaseDefaults(pDb);
              pPline.AppendVertex(pVertex);
              pVertex.Position = pos;
              pos = pos + new Vector3d(w / 4.0, h / 4.0, 0);
              pVertex.StartWidth = width[0, i];
              pVertex.EndWidth = width[1, i];
              pVertex.Dispose();
            }
          }
        }
      }
    }

    /************************************************************************/
    /* Add a 3D polyline to the specified BlockTableRecord                  */
    /************************************************************************/
    void add3dPolyline(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          //  double      h     = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "3D POLYLINE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Add a 3dPolyline to the database                                   */
          /**********************************************************************/
          Polyline3d pPline = new Polyline3d();
          pPline.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pPline);

          /**********************************************************************/
          /* Add the vertices                                                   */
          /**********************************************************************/
          Point3d pos = point;
          Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);

          double radius = w * 3.0 / 8.0;
          double height = 0.0;
          double theta = 0.0;

          int turns = 4;
          int segs = 16;
          int points = segs * turns;

          double deltaR = radius / points;
          double deltaTheta = 2 * Math.PI / segs;
          double deltaH = 2 * radius / points;

          Vector3d vec = new Vector3d(radius, 0, 0);

          for (int i = 0; i < points; i++)
          {
            using (PolylineVertex3d pVertex = new PolylineVertex3d())
            {
              pVertex.SetDatabaseDefaults(pDb);
              pPline.AppendVertex(pVertex);
              pVertex.Position = center + vec;
            }
            radius -= deltaR;
            height += deltaH;
            theta += deltaTheta;
            vec = new Vector3d(radius, 0, height).RotateBy(theta, Vector3d.ZAxis);
          }
          pPline.Dispose();
        }
      }
    }
    /************************************************************************/
    /* Add MText to the specified BlockTableRecord                          */
    /************************************************************************/
    void addMText(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Get the origin and size of the box                                 */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "MTEXT",
          m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Add MText to the database                                          */
        /**********************************************************************/
        using (MText pMText = new MText())
        {
          using (Database pDb = bBTR.Database)
            pMText.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pMText);

          /**********************************************************************/
          /* Set some properties                                                */
          /**********************************************************************/
          pMText.Location = point + new Vector3d(w / 8.0, -h * 2.0 / 8.0, 0);
          pMText.TextHeight = 0.4;
          pMText.Attachment = AttachmentPoint.TopLeft;
          pMText.Contents = "Sample {\\C1;MTEXT} created by {\\C5;OdWriteEx}";
          pMText.Width = w * 6.0 / 8.0;
          pMText.TextStyleId = styleId;
        }
      }
    }

    /************************************************************************/
    /* Add a Block Reference to the specified BlockTableRecord              */
    /************************************************************************/
    void addBlockRef(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId, ObjectId insertId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "INSERT",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Insert the Block                                                   */
          /**********************************************************************/
          ObjectId bklRefId = addInsert(bBTR, insertId, 1.0, 1.0);

          /**********************************************************************/
          /* Open the insert                                                    */
          /**********************************************************************/
          using (BlockReference pBlkRef = (BlockReference)bklRefId.GetObject(OpenMode.ForWrite))
          {
            /**********************************************************************/
            /* Create a transformation matrix for the block and attributes        */
            /**********************************************************************/
            Point3d insPoint = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            Matrix3d blkXfm = Matrix3d.Displacement(insPoint.GetAsVector());
            pBlkRef.TransformBy(blkXfm);

            /**********************************************************************/
            /* Scan the block definition for non-constant attribute definitions   */
            /* and use them as templates for attributes                           */
            /**********************************************************************/
            using (BlockTableRecord pBlockDef = (BlockTableRecord)insertId.GetObject(OpenMode.ForRead))
            {
              foreach (ObjectId idEnt in pBlockDef)
              {
                using (DBObject obj = idEnt.GetObject(OpenMode.ForRead))
                {
                  if (obj is AttributeDefinition)
                  {
                    AttributeDefinition pAttDef = (AttributeDefinition)obj;
                    if (pAttDef != null && !pAttDef.Constant)
                    {
                      using (AttributeReference pAtt = new AttributeReference())
                      {
                        pAtt.SetDatabaseDefaults(pDb);
                        pBlkRef.AttributeCollection.AppendAttribute(pAtt);
                        pAtt.SetPropertiesFrom(pAttDef);
                        pAtt.AlignmentPoint = pAttDef.AlignmentPoint;
                        pAtt.Height = pAttDef.Height;
                        pAtt.HorizontalMode = pAttDef.HorizontalMode;
                        pAtt.Normal = pAttDef.Normal;
                        pAtt.Oblique = pAttDef.Oblique;
                        pAtt.Position = pAttDef.Position;
                        pAtt.Rotation = pAttDef.Rotation;
                        pAtt.TextString = pAttDef.TextString;
                        pAtt.TextStyleId = pAttDef.TextStyleId;
                        pAtt.WidthFactor = pAttDef.WidthFactor;

                        /******************************************************************/
                        /* Specify a new value for the attribute                          */
                        /******************************************************************/
                        pAtt.TextString = "The Value";

                        /******************************************************************/
                        /* Transform it as the block was transformed                      */
                        /******************************************************************/
                        pAtt.TransformBy(blkXfm);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    /************************************************************************/
    /* Add a MInsert to the specified BlockTableRecord                      */
    /************************************************************************/
    void addMInsert(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId, ObjectId insertId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "MInsert",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Add MInsert to the database                                        */
          /**********************************************************************/
          MInsertBlock pMInsert = new MInsertBlock();
          pMInsert.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pMInsert);

          /**********************************************************************/
          /* Set some Parameters                                                */
          /**********************************************************************/
          pMInsert.BlockTableRecord = insertId;
          Point3d insPnt = point + new Vector3d(w * 2.0 / 8.0, h * 2.0 / 8.0, 0.0);
          pMInsert.Position = insPnt;
          pMInsert.ScaleFactors = new Scale3d(2.0 / 8.0);
          pMInsert.Rows = 2;
          pMInsert.Columns = 3;
          pMInsert.RowSpacing = h * 4.0 / 8.0;
          pMInsert.ColumnSpacing = w * 2.0 / 8.0;

          pMInsert.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a PolyFaceMesh to the specified BlockTableRecord                 */
    /************************************************************************/
    void addPolyFaceMesh(ObjectId btrId, int boxRow, int boxCol,
                         ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "PolyFaceMesh",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Add a PolyFaceMesh to the database                                 */
          /**********************************************************************/
          using (PolyFaceMesh pMesh = new PolyFaceMesh())
          {
            pMesh.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pMesh);

            /**********************************************************************/
            /* Add the faces and vertices that define a pup tent                  */
            /**********************************************************************/
            double dx = w * 3.0 / 8.0;
            double dy = h * 3.0 / 8.0;
            double dz = dy;

            Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);

            appendPfMeshVertex(pMesh, center.X + dx, center.Y + dy, 0);
            appendPfMeshVertex(pMesh, center.X + 0, center.Y + dy, center.Z + dz);
            appendPfMeshVertex(pMesh, center.X - dx, center.Y + dy, 0);
            appendPfMeshVertex(pMesh, center.X - dx, center.Y - dy, 0);
            appendPfMeshVertex(pMesh, center.X + 0, center.Y - dy, center.Z + dz);
            appendPfMeshVertex(pMesh, center.X + dx, center.Y - dy, 0);

            appendFaceRecord(pMesh, 1, 2, 5, 6);
            appendFaceRecord(pMesh, 2, 3, 4, 5);
            appendFaceRecord(pMesh, 6, 5, 4, 0);
            appendFaceRecord(pMesh, 3, 2, 1, 0);
          }
        }
      }
    }

    /************************************************************************/
    /* Add PolygonMesh to the specified BlockTableRecord                    */
    /************************************************************************/
    void addPolygonMesh(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Get the origin and size of the box                                 */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();

        /**********************************************************************/
        /* Add a label                                                        */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "PolygonMesh", m_textSize,
          TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Add a PolygonMesh to the database                                 */
        /**********************************************************************/
        using (PolygonMesh pMesh = new PolygonMesh())
        {
          using (Database pDb = bBTR.Database)
            pMesh.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pMesh);

          /**********************************************************************/
          /* Define the size of the mesh                                        */
          /**********************************************************************/
          short mSize = 16, nSize = 4;
          pMesh.MSize = mSize;
          pMesh.NSize = nSize;

          /**********************************************************************/
          /* Define a profile                                                   */
          /**********************************************************************/
          double dx = w * 3.0 / 8.0;
          double dy = h * 3.0 / 8.0;

          Vector3d[] vectors = { new Vector3d(0,  -dy, 0),
                                 new Vector3d(dx, -dy, 0),
                                 new Vector3d(dx,  dy, 0),
                                 new Vector3d(0,   dy, 0)};

          Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);

          /**********************************************************************/
          /* Append the vertices to the mesh                                    */
          /**********************************************************************/
          for (int i = 0; i < mSize; i++)
          {
            for (int j = 0; j < nSize; j++)
            {
              appendPgMeshVertex(pMesh, center + vectors[j]);
              vectors[j] = vectors[j].RotateBy(OdaToRadian(360.0 / mSize), Vector3d.YAxis);
            }
          }
          pMesh.MakeMClosed();
        }
      }
    }

    /************************************************************************/
    /* Add some curves to the specified BlockTableRecord                    */
    /************************************************************************/
    void addCurves(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          center -= new Vector3d(w * 2.5 / 8.0, 0, 0);
          double textY = point.Y - m_textSize / 2.0;

          /**********************************************************************/
          /* Create a Circle                                                    */
          /**********************************************************************/
          using (Circle pCircle = new Circle())
          {
            pCircle.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pCircle);

            pCircle.Center = center;
            pCircle.Radius = w * 1.0 / 8.0;

            /**********************************************************************/
            /* Add a Hyperlink to the Circle                                      */
            /**********************************************************************/
            HyperLinkCollection urls = pCircle.Hyperlinks;
            HyperLink hl = new HyperLink();
            hl.Name = "http://forum.opendesign.com/forumdisplay.php?s=&forumid=17";
            hl.Description = "Open Design Alliance Forum > Teigha, C++ version";
            urls.Add(hl);

            /**********************************************************************/
            /* Add a label                                                        */
            /**********************************************************************/

            addTextEnt(bBTR, new Point3d(center.X, textY, 0), new Point3d(center.X, textY, 0),
              "CIRCLE", m_textSize, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, layerId, styleId);

            addTextEnt(bBTR, new Point3d(center.X, textY - 1.6 * m_textSize, 0), new Point3d(center.X, textY - 1.6 * m_textSize, 0),
              "w/Hyperlink", m_textSize, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, layerId, styleId);
            hl.Dispose();
          }

          using (Circle myCircle = new Circle(center, new Vector3d(0, 0, 1), w * 1.0 / 4.0))
          {
            myCircle.SetDatabaseDefaults(pDb);
            myCircle.ColorIndex = 3;
            bBTR.AppendEntity(myCircle);
          }

          /**********************************************************************/
          /* Create an Arc                                                      */
          /**********************************************************************/
          using (Arc pArc = new Arc())
          {
            pArc.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pArc);

            pArc.Radius = w * 1.0 / 8.0;

            center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);

            center += Vector3d.YAxis * pArc.Radius / 2.0;

            pArc.Center = center;
            pArc.StartAngle = OdaToRadian(0.0);
            pArc.EndAngle = OdaToRadian(180.0);

            /**********************************************************************/
            /* Add a label                                                        */
            /**********************************************************************/
            addTextEnt(bBTR, new Point3d(center.X, textY, 0), new Point3d(center.X, textY, 0),
              "ARC", m_textSize, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, layerId, styleId);
          }

          /**********************************************************************/
          /* Add an Ellipse                                                     */
          /**********************************************************************/
          using (Ellipse pEllipse = new Ellipse())
          {
            pEllipse.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pEllipse);

            double majorRadius = w * 1.0 / 8.0;
            double radiusRatio = 0.25;

            center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            center += new Vector3d(w * 2.5 / 8.0, majorRadius, 0);

            Vector3d majorAxis = new Vector3d(majorRadius, 0.0, 0.0);
            majorAxis = majorAxis.RotateBy(OdaToRadian(30.0), Vector3d.ZAxis);

            pEllipse.Set(center, Vector3d.ZAxis, majorAxis, radiusRatio, 0, 2 * Math.PI);

            /**********************************************************************/
            /* Add a label                                                        */
            /**********************************************************************/
            addTextEnt(bBTR, new Point3d(center.X, textY, 0), new Point3d(center.X, textY, 0),
              "ELLIPSE", m_textSize, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, layerId, styleId);
          }

          /**********************************************************************/
          /* Add a Point                                                        */
          /**********************************************************************/
          using (DBPoint pPoint = new DBPoint())
          {
            pPoint.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pPoint);

            center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            center -= Vector3d.YAxis * h * 1.0 / 8.0;

            pPoint.Position = center;

            /**********************************************************************/
            /* Add a label                                                        */
            /**********************************************************************/
            center += Vector3d.YAxis * h * 1.0 / 8.0;
            addTextEnt(bBTR, center, center, "POINT", m_textSize, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, layerId, styleId);

            /**********************************************************************/
            /* Set the point display mode so we can see it                        */
            /**********************************************************************/
            pDb.Pdmode = 3;
            pDb.Pdsize = 0.1;
          }
        }
      }
    }
    /************************************************************************/
    /* Add a tolerance to the specified BlockTableRecord                    */
    /************************************************************************/
    void addTolerance(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
              "TOLERANCE", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Add a Frame Control Feature (Tolerance) to the database            */
          /**********************************************************************/
          using (FeatureControlFrame pTol = new FeatureControlFrame())
          {
            pTol.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pTol);

            /**********************************************************************/
            /* Set the properties                                                 */
            /**********************************************************************/
            point += Vector3d.XAxis * w / 6.0;
            point -= Vector3d.YAxis * h / 4.0;
            pTol.Location = point;
            pTol.Text = "{\\Fgdt;r}%%v{\\Fgdt;n}3.2{\\Fgdt;m}%%v%%v%%v%%v";
          }
        }
      }
    }

    /************************************************************************/
    /* Add some leaders the specified BlockTableRecord                      */
    /************************************************************************/
    void addLeaders(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "LEADERs",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Define an annotation block -- A circle with radius 0.5             */
          /**********************************************************************/
          using (BlockTable pBlocks = (BlockTable)pDb.BlockTableId.GetObject(OpenMode.ForWrite))
          {
            using (BlockTableRecord pAnnoBlock = new BlockTableRecord())
            {
              pAnnoBlock.Name = "AnnoBlock";
              ObjectId annoBlockId = pBlocks.Add(pAnnoBlock);

              using (Circle pCircle = new Circle())
              {
                pCircle.SetDatabaseDefaults(pDb);
                pAnnoBlock.AppendEntity(pCircle);
                Point3d center = new Point3d(0.5, 0, 0);
                pCircle.Center = center;
                pCircle.Radius = 0.5;
              }

              /**********************************************************************/
              /* Add a leader with database defaults to the database                */
              /**********************************************************************/
              Leader pLeader = new Leader();
              pLeader.SetDatabaseDefaults(pDb);
              bBTR.AppendEntity(pLeader);
              /**********************************************************************/
              /* Add the vertices                                                   */
              /**********************************************************************/
              point += new Vector3d(w * 1.0 / 8.0, -(h * 3.0 / 8.0), 0);
              pLeader.AppendVertex(point);

              point += new Vector3d(w * 2.0 / 8.0, h * 1.0 / 8.0, 0);
              pLeader.AppendVertex(point);

              /**********************************************************************/
              /* Insert the annotation                                              */
              /**********************************************************************/
              BlockReference pBlkRef = new BlockReference(point, bBTR.ObjectId);
              pBlkRef.BlockTableRecord = annoBlockId;
              pBlkRef.ScaleFactors = new Scale3d(0.375, 0.375, 0.375);
              bBTR.AppendEntity(pBlkRef);

              /**********************************************************************/
              /* Attach the Block Reference as annotation to the Leader             */
              /**********************************************************************/
              pLeader.Annotation = pBlkRef.ObjectId;
              pLeader.Dispose();

              /**********************************************************************/
              /* Add a leader with database defaults to the database                */
              /**********************************************************************/
              pLeader = new Leader();
              pLeader.SetDatabaseDefaults(pDb);
              bBTR.AppendEntity(pLeader);

              /**********************************************************************/
              /* Add the vertices                                                   */
              /**********************************************************************/
              point = m_EntityBoxes.getBox(boxRow, boxCol);
              point += new Vector3d(w * 1.0 / 8.0, -(h * 5.0 / 8.0), 0);
              pLeader.AppendVertex(point);

              point += new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0);
              pLeader.AppendVertex(point);

              point += Vector3d.XAxis * w * 1.0 / 8;
              /**********************************************************************/
              /* Set the arrowhead                                                  */
              /**********************************************************************/
              // TODO: pLeader.Dimldrblk = "DOT";

              /**********************************************************************/
              /* Create MText at a 30? angle                                        */
              /**********************************************************************/
              MText pMText = new MText();
              pMText.SetDatabaseDefaults(pDb);
              ObjectId mTextId = bBTR.AppendEntity(pMText);
              double textHeight = 0.15;
              double textWidth = 1.0;
              pMText.Location = point;
              pMText.Rotation = OdaToRadian(10.0);
              pMText.TextHeight = textHeight;
              pMText.Width = textWidth;
              pMText.Attachment = AttachmentPoint.MiddleLeft;
              pMText.Contents = "MText";
              pMText.TextStyleId = styleId;

              /**********************************************************************/
              /* Set a background color                                             */
              /**********************************************************************/
              Color cBackground = Color.FromRgb(255, 255, 0);
              pMText.BackgroundFillColor = cBackground;
              pMText.BackgroundFill = true;
              pMText.BackgroundScaleFactor = 2;

              /**********************************************************************/
              /* Attach the MText as annotation to the Leader                       */
              /**********************************************************************/
              pLeader.Annotation = mTextId;
              pLeader.Dispose();

              /**********************************************************************/
              /* Add a leader with database defaults to the database                */
              /**********************************************************************/
              pLeader = new Leader();
              bBTR.AppendEntity(pLeader);
              pLeader.SetDatabaseDefaults(pDb);

              /**********************************************************************/
              /* Add the vertices                                                   */
              /**********************************************************************/
              point = m_EntityBoxes.getBox(boxRow, boxCol);
              point += new Vector3d(w * 1.0 / 8.0, -(h * 7.0 / 8.0), 0);
              pLeader.AppendVertex(point);

              point += new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0);
              pLeader.AppendVertex(point);

              /**********************************************************************/
              /* Create a Frame Control Feature (Tolerance)                         */
              /**********************************************************************/
              FeatureControlFrame pTol = new FeatureControlFrame();
              pTol.SetDatabaseDefaults(pDb);
              pTol.Location = point;
              pTol.Text = "{\\Fgdt;r}%%v{\\Fgdt;n}3.2{\\Fgdt;m}%%v%%v%%v%%v";

              /**********************************************************************/
              /* Attach the FCF as annotation to the Leader                         */
              /**********************************************************************/
              pLeader.Annotation = bBTR.AppendEntity(pTol);

              pLeader.Dispose();
              pTol.Dispose();
              pMText.Dispose();
              pBlkRef.Dispose();
            }
          }
        }
      }
    }


    /************************************************************************/
    /* Add some MLeaders the specified BlockTableRecord                     */
    /************************************************************************/
    void addMLeaders(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      int llIndex;

      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the origin and size of the box                                 */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();

          /**********************************************************************/
          /* Add a label                                                        */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "MLeaders",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Add a MLeader with database defaults to the database               */
          /**********************************************************************/
          MLeader pMLeader = new MLeader();
          pMLeader.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pMLeader);

          /**********************************************************************/
          /* Add the vertices                                                   */
          /**********************************************************************/
          MText pMText = new MText();
          pMText.SetDatabaseDefaults(pDb);
          pMLeader.EnableFrameText = true;
          pMText.Contents = "MText";

          double textHeight = 0.15;
          double textWidth = 1.0;

          point += new Vector3d(w * 3.0 / 8.0, -h * 1.0 / 6.0, 0);
          pMText.Location = point;
          //  pMText.setRotation(OdaToRadian(10.0));
          pMText.TextHeight = textHeight;
          pMText.Width = textWidth;
          pMText.Attachment = AttachmentPoint.MiddleLeft;
          pMText.TextStyleId = styleId;
          pMLeader.MText = pMText;
          pMLeader.DoglegLength = 0.18;

          point -= new Vector3d(w * 2.0 / 8.0, h * 1.0 / 8.0, 0);
          llIndex = pMLeader.AddLeaderLine(point);

          point += Vector3d.XAxis * w * 1.0 / 8.0;
          //  point.y -= h * 3.0 / 8.0;
          llIndex = pMLeader.AddLeaderLine(point);
          point += new Vector3d(w * 1.0 / 6.0, -h * 1.0 / 8.0, 0);
          pMLeader.AddFirstVertex(llIndex, point);

          point += new Vector3d(w * 3.0 / 8.0, -h * 1.0 / 8.0, 0);
          llIndex = pMLeader.AddLeaderLine(point);

          pMText.Dispose();
          pMLeader.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Block Definition to the specified database                     */
    /************************************************************************/
    ObjectId addBlockDef(Database pDb, string name, int boxRow, int boxCol)
    {
      /**********************************************************************/
      /* Open the block table                                               */
      /**********************************************************************/
      using (BlockTable pBlocks = (BlockTable)pDb.BlockTableId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Create a BlockTableRecord                                          */
        /**********************************************************************/
        using (BlockTableRecord bBTR = new BlockTableRecord())
        {
          /**********************************************************************/
          /* Block must have a name before adding it to the table.              */
          /**********************************************************************/
          bBTR.Name = name;

          /**********************************************************************/
          /* Add the record to the table.                                       */
          /**********************************************************************/
          ObjectId btrId = pBlocks.Add(bBTR);
          //  double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add a Circle                                                       */
          /**********************************************************************/
          Point3d center = new Point3d(-(w * 2.5 / 8.0), 0, 0);

          using (Circle pCircle = new Circle(center, Vector3d.ZAxis, w * 1.0 / 8.0))
          {
            pCircle.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pCircle);
          }

          /**********************************************************************/
          /* Add an Arc                                                         */
          /**********************************************************************/
          using (Arc pArc = new Arc())
          {
            pArc.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pArc);

            pArc.Radius = w * 1.0 / 8.0;
            center = new Point3d(0, -pArc.Radius / 2.0, 0);

            pArc.Center = center;
            pArc.StartAngle = 0;
            pArc.EndAngle = Math.PI;
          }

          /**********************************************************************/
          /* Add an Ellipse                                                     */
          /**********************************************************************/
          using (Ellipse pEllipse = new Ellipse())
          {
            pEllipse.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pEllipse);

            center = new Point3d(w * 2.5 / 8.0, 0, 0);

            double majorRadius = w * 1.0 / 8.0;
            Vector3d majorAxis = new Vector3d(majorRadius, 0.0, 0.0);
            majorAxis = majorAxis.RotateBy(Math.PI / 6, Vector3d.ZAxis);

            double radiusRatio = 0.25;

            pEllipse.Set(center, Vector3d.ZAxis, majorAxis, radiusRatio, 0, 2 * Math.PI);
          }

          /**********************************************************************/
          /* Add an Attdef                                                      */
          /**********************************************************************/
          using (AttributeDefinition pAttDef = new AttributeDefinition())
          {
            pAttDef.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pAttDef);

            pAttDef.Prompt = "Enter ODT_ATT: ";
            pAttDef.Tag = "Oda_ATT";
            pAttDef.HorizontalMode = TextHorizontalMode.TextCenter;
            pAttDef.Height = 0.1;
            pAttDef.TextString = "Default";
          }
          /**********************************************************************/
          /* Return the ObjectId of the BlockTableRecord                        */
          /**********************************************************************/
          return btrId;
        }
      }
    }

    //    /************************************************************************/
    //    /* Append an XData Pair to the specified ResBuf                         */
    //    /************************************************************************/
    //    OdResBufPtr appendXDataPair(OdResBufPtr pCurr, 
    //                                          int code)
    //    {
    //      pCurr.setNext(OdResBuf::newRb(code));
    //      return pCurr.next();
    //    }

    void addExtendedData(ObjectId id)
    {
      DBObject obj = id.GetObject(OpenMode.ForWrite);
      ResultBuffer rb = new ResultBuffer(
        new TypedValue(1001, "ODA"),
        new TypedValue(1000, "Extended Data for ODA app"),
        new TypedValue(1000, "Double"));

      obj.XData = rb;
    }

    // Adds an external reference called "XRefBlock") to the passed in database,
    // which references the file "xref.dwg").

    /************************************************************************/
    /* Add an XRef to the specified BlockTableRecord                        */
    /************************************************************************/
    void addXRef(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        Database pDb = btrId.Database;

        /**********************************************************************/
        /* Get the Upper-left corner of the box and its size                  */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);

        /**********************************************************************/
        /* Add the label                                                      */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "XREF INSERT",
          m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Get the lower-left corner of the box                               */
        /**********************************************************************/
        point -= Vector3d.YAxis * h;

        /**********************************************************************/
        /* Create a BlockTableRecord                                          */
        /**********************************************************************/
        ObjectId objIdXRef = pDb.AttachXref("OdWriteEx XRef.dwg", "XRefBlock");

        /**********************************************************************/
        /* Insert the Xref                                                    */
        /**********************************************************************/
        ObjectId xRefId = addInsert(bBTR, objIdXRef, 1.0, 1.0);
        /**********************************************************************/
        /* Open the insert                                                    */
        /**********************************************************************/
        using (BlockReference pXRefIns = (BlockReference)xRefId.GetObject(OpenMode.ForWrite))
        {

          /**********************************************************************/
          /* Set the insertion point                                            */
          /**********************************************************************/
          pXRefIns.Position = point;

          /**********************************************************************/
          /* Move\Scale XREF to presentation rectangle                          */
          /**********************************************************************/
          Extents3d extents = pXRefIns.GeometricExtents;
          Point3d maxPt = extents.MaxPoint;
          Point3d minPt = extents.MinPoint;
          if ((maxPt.X >= minPt.X) && (maxPt.Y >= minPt.Y) && (maxPt.Z >= minPt.Z))
          {
            double dScale = Math.Min(w / (maxPt.X - minPt.X), h * (7.0 / 8.0) / (maxPt.Y - minPt.Y));
            pXRefIns.ScaleFactors = new Scale3d(dScale, dScale, 1);
            pXRefIns.Position = point - dScale * (minPt - point.GetAsVector()).GetAsVector();
          }
        }
      }
    }

    /************************************************************************/
    /* Add a layout                                                         */
    /************************************************************************/
    void addLayout(Database pDb)
    {
      /********************************************************************/
      /* Create a new Layout                                              */
      /********************************************************************/
      LayoutManager lm = LayoutManager.Current;
      ObjectId layoutId = lm.CreateLayout("ODA Layout");
      using (Layout pLayout = (Layout)layoutId.GetObject(OpenMode.ForRead))
      {
        /********************************************************************/
        /* Make it current, creating the overall PaperSpace viewport        */
        /********************************************************************/
        lm.CurrentLayout = String.Format("ODA Layout");

        /********************************************************************/
        /* Open the overall viewport for this layout                        */
        /********************************************************************/
        Teigha.DatabaseServices.Viewport pOverallViewport = (Teigha.DatabaseServices.Viewport)pDb.PaperSpaceVportId.GetObject(OpenMode.ForRead);

        /********************************************************************/
        /* Get some useful parameters                                       */
        /********************************************************************/
        Point3d centerPoint = pOverallViewport.CenterPoint;

        /********************************************************************/
        /* Note:                                                            */
        /* If a viewport is an overall viewport,                            */
        /* the values returned by width() and height() must be divided by a */
        /* factor of 1.058, and the parameters of setWidth and setHeight()  */
        /* must be multiplied a like factor.                                */
        /********************************************************************/
        const double margin = 0.25;
        double overallWidth = pOverallViewport.Width / 1.058 - 2 * margin;
        double overallHeight = pOverallViewport.Height / 1.058 - 2 * margin;
        Vector3d vecTmp = new Vector3d(0.5 * overallWidth, 0.5 * overallHeight, 0.0);
        Point3d overallLLCorner = centerPoint - vecTmp;

        /********************************************************************/
        /* Open the PaperSpace BlockTableRecord for this layout             */
        /********************************************************************/
        using (BlockTableRecord pPS = (BlockTableRecord)pLayout.BlockTableRecordId.GetObject(OpenMode.ForWrite))
        {
          /********************************************************************/
          /* Create a new viewport, and append it to PaperSpace               */
          /********************************************************************/
          Teigha.DatabaseServices.Viewport pViewport = new Teigha.DatabaseServices.Viewport();
          pViewport.SetDatabaseDefaults(pDb);
          pPS.AppendEntity(pViewport);

          /********************************************************************/
          /* Set some parameters                                              */
          /*                                                                  */
          /* This viewport occupies the upper half of the overall viewport,   */
          /* and displays all objects in model space                          */
          /********************************************************************/

          pViewport.Width = overallWidth;
          pViewport.Height = overallHeight * 0.5;
          Vector3d vecTmp2 = new Vector3d(0.0, 0.5 * pViewport.Height, 0.0);
          pViewport.CenterPoint = centerPoint + vecTmp2;
          pViewport.ViewCenter = pOverallViewport.ViewCenter;
          //pViewport.zoomExtents();

          /********************************************************************/
          /* Create viewports for each of the entities that have been         */
          /* pushBacked onto m_layoutEntities                                 */
          /********************************************************************/

          if (m_layoutEntities.Count > 0)
          {
            double widthFactor = 1.0 / m_layoutEntities.Count;
            int i = 0;
            foreach (ObjectId layId in m_layoutEntities)
            {
              i++;
              Entity pEnt = (Entity)layId.GetObject(OpenMode.ForRead);
              Extents3d entityExtents = pEnt.GeometricExtents;

              /**************************************************************/
              /* Create a new viewport, and append it to PaperSpace         */
              /**************************************************************/
              using (Teigha.DatabaseServices.Viewport pViewportN = new Teigha.DatabaseServices.Viewport())
              {
                pViewportN.SetDatabaseDefaults(pDb);
                pPS.AppendEntity(pViewportN);

                /**************************************************************/
                /* The viewports are tiled along the bottom of the overall    */
                /* viewport                                                   */
                /**************************************************************/
                pViewportN.Width = overallWidth * widthFactor;
                pViewportN.Height = overallHeight * 0.5;
                Vector3d vecTmpN = new Vector3d((i + 0.5) * pViewportN.Width, 0.5 * pViewportN.Height, 0.0);
                pViewportN.CenterPoint = overallLLCorner + vecTmpN;

                /**************************************************************/
                /* The target of the viewport is the midpoint of the entity   */
                /* extents                                                    */
                /**************************************************************/
                Point3d minPt = entityExtents.MinPoint;
                Point3d maxPt = entityExtents.MaxPoint;
                pViewportN.ViewTarget = new Point3d((minPt.X + maxPt.X) / 2.0,
                                                    (minPt.Y + maxPt.Y) / 2.0,
                                                    (minPt.Z + maxPt.Z) / 2.0);

                /**************************************************************/
                /* The viewHeight is the larger of the height as defined by   */
                /* the entityExtents, and the height required to display the  */
                /* width of the entityExtents                                 */
                /**************************************************************/
                double viewHeight = Math.Abs(maxPt.Y - minPt.Y);
                double viewWidth = Math.Abs(maxPt.X - minPt.X);
                viewHeight = Math.Max(viewHeight, viewWidth * pViewportN.Height / pViewportN.Width);
                pViewportN.ViewHeight = viewHeight * 1.05;
              }
              pEnt.Dispose();
            }
          }
          pViewport.Dispose();
        }
        pDb.TileMode = true;
        pOverallViewport.Dispose();
      }
    }

    /************************************************************************/
    /* Add entity boxes to specified BlockTableRecord                       */
    /************************************************************************/
    void createEntityBoxes(ObjectId btrId, ObjectId layerId)
    {
      using (Database pDb = btrId.Database)
      {
        /**********************************************************************/
        /* Open the BlockTableRecord                                          */
        /**********************************************************************/
        using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
        {
          Point3d currentPoint;
          /**********************************************************************/
          /* Create a 2D polyline for each box                                  */
          /**********************************************************************/
          for (int j = 0; j < EntityBoxes.VER_BOXES; j++)
          {
            for (int i = 0; i < EntityBoxes.HOR_BOXES; i++)
            {
              if (!m_EntityBoxes.isBox(j, i))
                break;

              double wCurBox = m_EntityBoxes.getWidth(j, i);
              currentPoint = m_EntityBoxes.getBox(j, i);

              using (Polyline2d pPline = new Polyline2d())
              {
                pPline.SetDatabaseDefaults(pDb);

                bBTR.AppendEntity(pPline);

                Vertex2d pVertex = new Vertex2d();
                pVertex.SetDatabaseDefaults(pDb);
                pPline.AppendVertex(pVertex);
                Point3d pos = currentPoint;
                pVertex.Position = pos;
                pVertex.Dispose();

                pVertex = new Vertex2d();
                pPline.AppendVertex(pVertex);
                pos = pos + wCurBox * Vector3d.XAxis;
                pVertex.Position = pos;
                pVertex.Dispose();

                pVertex = new Vertex2d();
                pPline.AppendVertex(pVertex);
                pos = pos - m_EntityBoxes.getHeight() * Vector3d.YAxis;
                pVertex.Position = pos;
                pVertex.Dispose();

                pVertex = new Vertex2d();
                pPline.AppendVertex(pVertex);
                pos = pos - wCurBox * Vector3d.XAxis;
                pVertex.Position = pos;
                pVertex.Dispose();

                pPline.Closed = true;

                pPline.ColorIndex = 5;
                pPline.LayerId = layerId;
              }
            }
          }
        }
        /**********************************************************************/
        /* 'Zoom' the box array by resizing the active tiled MS viewport      */
        /**********************************************************************/
        using (ViewportTableRecord vPortRec = (ViewportTableRecord)pDb.CurrentViewportTableRecordId.GetObject(OpenMode.ForWrite))
        {
          Point3d center = m_EntityBoxes.getArrayCenter();
          vPortRec.CenterPoint = new Point2d(center.X, center.Y);

          Vector3d size = m_EntityBoxes.getArraySize();
          vPortRec.Height = 1.05 * Math.Abs(size.Y);
          vPortRec.Width = 1.05 * Math.Abs(size.X);
          vPortRec.CircleSides = 20000;
        }
      }
    }

    /************************************************************************/
    /* Add a PaperSpace viewport to the specified database                  */
    /************************************************************************/
    void addPsViewport(Database pDb, ObjectId layerId)
    {
      /**********************************************************************/
      /* Enable PaperSpace                                                  */
      /*                                                                    */
      /* NOTE: This is required to cause Teigha to automatically create  */
      /* the overall viewport. If not called before opening PaperSpace      */
      /* BlockTableRecord,   the first viewport created IS the the overall  */
      /* viewport.                                                          */
      /**********************************************************************/
      pDb.TileMode = false;

      /**********************************************************************/
      /* Open PaperSpace                                                    */
      /**********************************************************************/
      using (BlockTable blTable = (BlockTable)pDb.BlockTableId.GetObject(OpenMode.ForRead))
      {
        using (BlockTableRecord pPs = (BlockTableRecord)blTable[BlockTableRecord.PaperSpace].GetObject(OpenMode.ForWrite))
        {
          /**********************************************************************/
          /* Disable PaperSpace                                                 */
          /**********************************************************************/
          pDb.TileMode = true;

          /**********************************************************************/
          /* Create the viewport                                                */
          /**********************************************************************/
          using (Teigha.DatabaseServices.Viewport pVp = new Teigha.DatabaseServices.Viewport())
          {
            pVp.SetDatabaseDefaults(pDb);
            /**********************************************************************/
            /* Add it to PaperSpace                                               */
            /**********************************************************************/
            pPs.AppendEntity(pVp);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/
            pVp.CenterPoint = new Point3d(5.25, 4.0, 0);
            pVp.Width = 10.0;
            pVp.Height = 7.5;
            pVp.ViewTarget = Point3d.Origin;
            pVp.ViewDirection = Vector3d.ZAxis;
            pVp.ViewHeight = 8.0;
            pVp.LensLength = 50.0;
            pVp.ViewCenter = new Point2d(5.25, 4.0);
            pVp.SnapIncrement = new Vector2d(0.25, 0.25);
            pVp.GridIncrement = new Vector2d(0.25, 0.25);
            pVp.CircleSides = 20000;

            /**********************************************************************/
            /* Freeze a layer in this viewport                                    */
            /**********************************************************************/
            ObjectId[] layers = new ObjectId[] { layerId };
            pVp.FreezeLayersInViewport(layers.GetEnumerator());

            /**********************************************************************/
            /* Add a circle to this PaperSpace Layout                             */
            /**********************************************************************/
            Circle pCircle = new Circle();
            pCircle.SetDatabaseDefaults(pDb);
            pPs.AppendEntity(pCircle);
            pCircle.Radius = 1.0;
            pCircle.Center = new Point3d(1.0, 1.0, 0.0);
            pCircle.SetLayerId(layerId, false);

            /**********************************************************************/
            /* Disable PaperSpace                                                 */
            /**********************************************************************/
            pDb.TileMode = true;
            pCircle.Dispose();
          }
        }
      }
    }

    /************************************************************************/
    /* Add a dimension style to the specified database                      */
    /************************************************************************/
    ObjectId addDimStyle(Database pDb, string dimStyleName)
    {
      /**********************************************************************/
      /* Create the DimStyle                                                */
      /**********************************************************************/
      using (DimStyleTableRecord pDimStyle = new DimStyleTableRecord())
      {
        /**********************************************************************/
        /* Set the name                                                       */
        /**********************************************************************/
        pDimStyle.Name = dimStyleName;
        /**********************************************************************/
        /* Open the DimStyleTable                                             */
        /**********************************************************************/
        using (DimStyleTable pTable = (DimStyleTable)pDb.DimStyleTableId.GetObject(OpenMode.ForWrite))
        {
          /**********************************************************************/
          /* Add the DimStyle                                                   */
          /**********************************************************************/
          ObjectId dimStyleId = pTable.Add(pDimStyle);
          /**********************************************************************/
          /* Set some properties                                                */
          /**********************************************************************/
          using (TextStyleTable textStyleTbl = (TextStyleTable)pDb.TextStyleTableId.GetObject(OpenMode.ForRead))
          {
            pDimStyle.Dimtxsty = textStyleTbl["Standard"];
            pDimStyle.Dimsah = true;
            pDimStyle.Dimblk1s = String.Format("_OBLIQUE");
            pDimStyle.Dimblk2s = String.Format("_DOT");
          }
          return dimStyleId;
        }
      }
    }


    /**********************************************************************/
    /* Add an Associative Rotated Dimension                                           */
    /**********************************************************************/
    void addDimAssoc(ObjectId btrId,
                     int boxRow,
                     int boxCol,
                     ObjectId layerId,
                     ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d ulPoint = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, ulPoint + m_textOffset, ulPoint + m_textOffset, "Associative",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, ulPoint + m_textOffset + m_textLine, ulPoint + m_textOffset + m_textLine, "Dimension",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          Point3d llPoint = new Point3d(ulPoint.X, ulPoint.Y - h, ulPoint.Z);

          /**********************************************************************/
          /* Create a line to be dimensioned                                    */
          /**********************************************************************/
          Point3d line1Pt = new Point3d(llPoint.X + w * 1.0 / 8.0, llPoint.Y + h * 2.0 / 8.0, 0);
          Point3d line2Pt = new Point3d(line1Pt.X + 3.75, llPoint.Y + h * 7.0 / 8.0, 0);

          Line pLine = new Line(line1Pt, line2Pt);
          pLine.SetDatabaseDefaults(pDb);
          ObjectId lineId = bBTR.AppendEntity(pLine);

          /**********************************************************************/
          /* Create a rotated dimension and dimension the ends of the line      */
          /**********************************************************************/
          RotatedDimension pDimension = new RotatedDimension(0, pLine.StartPoint,
            pLine.EndPoint, new Point3d(llPoint.X + w / 2.0, llPoint.Y + h * 1.0 / 8.0, 0), String.Format("RotatedDimension"), ObjectId.Null);
          pDimension.SetDatabaseDefaults(pDb);
          ObjectId dimensionId = bBTR.AppendEntity(pDimension);

          Point3d dimLinePt = new Point3d(ulPoint.X + w / 2.0, ulPoint.Y + h * 1.0 / 8.0, 0);
          pDimension.XLine1Point = pLine.StartPoint;
          pDimension.XLine2Point = pLine.EndPoint;
          pDimension.UsingDefaultTextPosition = true;
          pDimension.CreateExtensionDictionary();

          /**********************************************************************/
          /* Create an associative dimension                                    */
          /**********************************************************************/
          DimAssoc pDimAssoc = new DimAssoc();

          /**********************************************************************/
          /* Associate the associative dimension with the rotated dimension by  */
          /* adding it to the extension dictionary of the rotated dimension     */
          /**********************************************************************/
          DBDictionary pDict = (DBDictionary)pDimension.ExtensionDictionary.GetObject(OpenMode.ForWrite);
          ObjectId dimAssId = pDict.SetAt("ACAD_DIMASSOC", pDimAssoc);

          /**********************************************************************/
          /* Associate the rotated dimension with the associative dimension     */
          /**********************************************************************/
          pDimAssoc.DimObjId = dimensionId;
          pDimAssoc.RotatedType = RotatedDimType.Unknown;

          /**********************************************************************/
          /* Attach the line to the associative dimension                       */
          /**********************************************************************/
          OsnapPointRef pointRef = new OsnapPointRef();
          pointRef.Point = pLine.StartPoint;
          pointRef.OsnapType = ObjectSnapModes.ModeStartpoint;
          pointRef.NearPointParam = 0.0;

          XrefFullSubentityPath xrefPath = pointRef.MainEntity();
          xrefPath.AppendObjectId(lineId);
          xrefPath.SubentId = new SubentityId(SubentityType.Vertex, new IntPtr(0));
          pDimAssoc.SetPointRef(DimAssocPointType.Xline1Point, pointRef);

          pointRef = new OsnapPointRef();
          pointRef.Point = pLine.EndPoint;
          pointRef.OsnapType = ObjectSnapModes.ModeEnd;
          pointRef.NearPointParam = 1.0;

          xrefPath = pointRef.MainEntity();
          xrefPath.AppendObjectId(lineId);
          xrefPath.SubentId = new SubentityId(SubentityType.Edge, new IntPtr(0));
          pDimAssoc.SetPointRef(DimAssocPointType.Xline2Point, pointRef);

          /**********************************************************************/
          /* Add Persistent reactors from the rotated dimension and the line    */
          /* to the associative dimension                                       */
          /**********************************************************************/
          pDimension.AddPersistentReactor(dimAssId);
          pLine.AddPersistentReactor(dimAssId);

          pDimAssoc.Dispose();
          pDimension.Dispose();
          pLine.Dispose();
        }
      }
    }


    /************************************************************************/
    /* Add an Aligned Dimension to the specified BlockTableRecord           */
    /************************************************************************/
    void addAlignedDimension(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId, ObjectId dimStyleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Get the Upper-left corner of the box and its size                  */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);

        /**********************************************************************/
        /* Add the labels                                                     */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Aligned", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
          "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Get the lower-left corner of the box                               */
        /**********************************************************************/
        point -= Vector3d.YAxis * h;

        /**********************************************************************/
        /* Create a line to be dimensioned                                    */
        /**********************************************************************/
        Point3d line1Pt = new Point3d(point.X + w * 0.5 / 8.0, point.Y + h * 1.5 / 8.0, 0);
        Point3d line2Pt = line1Pt + new Vector3d(1.5, 2.0, 0.0);

        Line pLine = new Line();
        using (Database pDb = bBTR.Database)
        {
          pLine.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pLine);
          pLine.StartPoint = line1Pt;
          pLine.EndPoint = line2Pt;

          /**********************************************************************/
          /* Create an aligned dimension and dimension the ends of the line     */
          /**********************************************************************/
          using (AlignedDimension pDimension = new AlignedDimension())
          {
            pDimension.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pDimension);

            Point3d dimLinePt = new Point3d(point.X + w * 3.5 / 8.0, point.Y + h * 2.0 / 8.0, 0);

            pDimension.DimensionStyle = dimStyleId;
            pDimension.XLine1Point = pLine.StartPoint;
            pDimension.XLine2Point = pLine.EndPoint;
            pDimension.DimLinePoint = dimLinePt;
            pDimension.UsingDefaultTextPosition = true;

            // TODO
            //pDimension.JogSymbolHeight(1.5);
          }
        }
        pLine.Dispose();
      }
    }

    /************************************************************************/
    /* Add a Radial Dimension to the specified BlockTableRecord             */
    /************************************************************************/
    void addRadialDimension(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          //  double w    = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Radial",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a circle to be dimensioned                                    */
          /**********************************************************************/
          Circle pCircle = new Circle();
          pCircle.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pCircle);
          pCircle.Center = point + new Vector3d(0.625, h * 3.0 / 8.0, 0);
          pCircle.Radius = 0.5;

          /**********************************************************************/
          /* Create a Radial Dimension                                         */
          /**********************************************************************/
          using (RadialDimension pDimension = new RadialDimension())
          {
            pDimension.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pDimension);

            pDimension.Center = pCircle.Center;
            Vector3d chordVector = new Vector3d(pCircle.Radius, 0.0, 0.0);
            chordVector = chordVector.RotateBy(OdaToRadian(75.0), Vector3d.ZAxis);
            pDimension.ChordPoint = pDimension.Center + chordVector;
            pDimension.LeaderLength = 0.125;
            pDimension.UsingDefaultTextPosition = true;
          }
          pCircle.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Diametric Dimension to the specified BlockTableRecord             */
    /************************************************************************/
    void addDiametricDimension(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          //  double w    = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Diametric",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a circle to be dimensioned                                    */
          /**********************************************************************/
          Circle pCircle = new Circle();
          pCircle.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pCircle);
          pCircle.Center = point + new Vector3d(0.625, h * 3.0 / 8.0, 0);
          pCircle.Radius = 0.5;

          /**********************************************************************/
          /* Create a Diametric Dimension                                       */
          /**********************************************************************/
          using (DiametricDimension pDimension = new DiametricDimension())
          {
            pDimension.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pDimension);

            Vector3d chordVector = new Vector3d(pCircle.Radius, 0.0, 0.0);
            chordVector = chordVector.RotateBy(OdaToRadian(75.0), Vector3d.ZAxis);

            pDimension.ChordPoint = pCircle.Center + chordVector;
            pDimension.FarChordPoint = pCircle.Center - chordVector;
            pDimension.LeaderLength = 0.125;
            pDimension.UsingDefaultTextPosition = true;
          }
          pCircle.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Shape to the specified BlockTableRecord                        */
    /************************************************************************/
    void addShape(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Shape",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the center of the box                                          */
          /**********************************************************************/
          Point3d pCenter = m_EntityBoxes.getBoxCenter(boxRow, boxCol);

          /**********************************************************************/
          /* Create a Shape                                                     */
          /**********************************************************************/
          using (Shape pShape = new Shape())
          {
            pShape.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pShape);
            double size = w * 3.0 / 8.0;
            pShape.Size = size;
            pShape.Position = pCenter + new Vector3d(0.0, -size, 0.0);
            pShape.Rotation = OdaToRadian(90.0);
            pShape.Name = "CIRC1";
          }
        }
      }
    }

    /************************************************************************/
    /* Add a 3D Face to the specified BlockTableRecord                      */
    /************************************************************************/
    void add3dFace(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "3DFACE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a 3D Face                                                   */
          /**********************************************************************/
          Face pFace = new Face();
          pFace.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pFace);

          pFace.SetVertexAt(0, point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0));
          pFace.SetVertexAt(1, point + new Vector3d(w * 7.0 / 8.0, h * 1.0 / 8.0, 0.0));
          pFace.SetVertexAt(2, point + new Vector3d(w * 7.0 / 8.0, h * 6.0 / 8.0, 0.0));
          pFace.SetVertexAt(3, point + new Vector3d(w * 1.0 / 8.0, h * 6.0 / 8.0, 0.0));
          pFace.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Solid to the specified BlockTableRecord                          */
    /************************************************************************/
    void addSolid(ObjectId btrId,
                            int boxRow,
                            int boxCol,
                            ObjectId layerId,
                            ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "SOLID",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;


          /**********************************************************************/
          /* Create a Solid                                                   */
          /**********************************************************************/
          Solid pSolid = new Solid();
          pSolid.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pSolid);

          pSolid.SetPointAt(0, point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0));
          pSolid.SetPointAt(1, point + new Vector3d(w * 7.0 / 8.0, h * 1.0 / 8.0, 0.0));
          pSolid.SetPointAt(2, point + new Vector3d(w * 1.0 / 8.0, h * 6.0 / 8.0, 0.0));
          pSolid.SetPointAt(3, point + new Vector3d(w * 7.0 / 8.0, h * 6.0 / 8.0, 0.0));
          pSolid.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add an ACIS Solid to the specified BlockTableRecord                  */
    /************************************************************************/
    void addACIS(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          //  double w    = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "3DSOLID",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

          /**********************************************************************/
          /* Read the solids in the .sat file                                   */
          /**********************************************************************/
          using (DBObjectCollection entities = Body.AcisIn("OdWriteEx.sat"))
          {
            if (entities.Count > 0)
            {
              /********************************************************************/
              /* Read the solids in the .sat file                                 */
              /********************************************************************/
              addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine, "from SAT file",
              m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
              foreach (Entity ent in entities)
              {
                /******************************************************************/
                /* Move the solid into the center of the box                      */
                /******************************************************************/
                ObjectId id = bBTR.AppendEntity(ent);
                Entity p3dSolid = (Entity)id.GetObject(OpenMode.ForWrite);
                p3dSolid.TransformBy(xfm);
                /******************************************************************/
                /* Each of these entities will later get its own viewport         */
                /******************************************************************/
                m_layoutEntities.Add(id);
                p3dSolid.Dispose();
                ent.Dispose();
              }
            }
            else
            {
              /********************************************************************/
              /* Create a simple solid                                            */
              /********************************************************************/
              using (Solid3d p3dSolid = new Solid3d())
              {
                p3dSolid.SetDatabaseDefaults(pDb);
                ObjectId id = bBTR.AppendEntity(p3dSolid);

                p3dSolid.CreateSphere(1.0);
                p3dSolid.TransformBy(xfm);

                /********************************************************************/
                /* This entity will later get its own viewport                      */
                /********************************************************************/
                m_layoutEntities.Add(id);
              }
            }
          }
        }
      }
    }
    /************************************************************************/
    /* Add a Box to the specified BlockTableRecord                          */
    /************************************************************************/
    void addBox(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Box",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

            p3dSolid.CreateBox(w * 6.0 / 8.0, h * 6.0 / 8.0, w * 6.0 / 8.0);
            p3dSolid.TransformBy(xfm);
          }
        }
      }
    }

    /************************************************************************/
    /* Add a Frustum to the specified BlockTableRecord                      */
    /************************************************************************/
    void addFrustum(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Frustum",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

            p3dSolid.CreateFrustum(w * 6.0 / 8.0, w * 3.0 / 8.0, h * 3.0 / 8.0, w * 1.0 / 8.0);
            p3dSolid.TransformBy(xfm);
          }
        }
      }
    }
    /************************************************************************/
    /* Add a Sphere to the specified BlockTableRecord                       */
    /************************************************************************/
    void addSphere(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Sphere",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

            p3dSolid.CreateSphere(w * 3.0 / 8.0);
            p3dSolid.TransformBy(xfm);
          }
        }
      }
    }
    /************************************************************************/
    /* Add a Torus to the specified BlockTableRecord                       */
    /************************************************************************/
    void addTorus(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Torus",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

            p3dSolid.CreateTorus(w * 2.0 / 8.0, w * 1.0 / 8.0);
            p3dSolid.TransformBy(xfm);
          }
        }
      }
    }
    /************************************************************************/
    /* Add a Wedge to the specified BlockTableRecord                       */
    /************************************************************************/
    void addWedge(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Wedge",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            Matrix3d xfm = Matrix3d.Displacement(m_EntityBoxes.getBoxCenter(boxRow, boxCol).GetAsVector());

            p3dSolid.CreateWedge(w * 6.0 / 8.0, h * 6.0 / 8.0, w * 6.0 / 8.0);
            p3dSolid.TransformBy(xfm);
          }
        }
      }
    }

    /************************************************************************/
    /* Add a Region to the specified BlockTableRecord                       */
    /************************************************************************/
    void addRegion(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Region",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Create a Circle                                                    */
          /**********************************************************************/
          using (Circle pCircle = new Circle())
          {
            pCircle.SetDatabaseDefaults(pDb);

            Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            pCircle.Center = center;
            pCircle.Radius = w * 3.0 / 8.0;

            /**********************************************************************/
            /* Add it to the array of curves                                      */
            /**********************************************************************/
            DBObjectCollection curveSegments = new DBObjectCollection();
            curveSegments.Add(pCircle);

            /**********************************************************************/
            /* Create the region                                                  */
            /**********************************************************************/
            using (DBObjectCollection regions = Region.CreateFromCurves(curveSegments))
            {
              foreach (DBObject obj in regions)
              {
                /**********************************************************************/
                /* Append it to the block table record                                */
                /**********************************************************************/
                bBTR.AppendEntity((Entity)obj);
                // 3d solids should be disposed, because they lock modeler module
                obj.Dispose();
              }
            }
          }
        }
      }
    }

    /************************************************************************/
    /* Add an Extrusion to the specified BlockTableRecord                   */
    /************************************************************************/
    void addExtrusion(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Extrusion",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            /**********************************************************************/
            /* Create a Circle                                                    */
            /**********************************************************************/
            using (Circle pCircle = new Circle())
            {
              pCircle.SetDatabaseDefaults(pDb);

              Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
              pCircle.Center = center;
              pCircle.Radius = w * 3.0 / 8.0;

              /**********************************************************************/
              /* Add it to the array of curves                                      */
              /**********************************************************************/
              DBObjectCollection entities = new DBObjectCollection();
              entities.Add(pCircle);
              /**********************************************************************/
              /* Create a region                                                    */
              /**********************************************************************/
              using (DBObjectCollection regions = Region.CreateFromCurves(entities))
              {
                System.Collections.IEnumerator enumerator = regions.GetEnumerator();
                /**********************************************************************/
                /* Extrude the region                                                 */
                /**********************************************************************/

                //It disabled in ModelerGeometry
                /*if (enumerator.MoveNext())
                {
                  p3dSolid.Extrude((Region)enumerator.Current, w * 6.0 / 8.0, 0.0);
                  // 3d solids should be disposed, because they lock modeler module
                  ((Region)enumerator.Current).Dispose();
                }*/
              }
            }
          }
        }
      }
    }
    /************************************************************************/
    /* Add an Solid of Revolution to the specified BlockTableRecord         */
    /************************************************************************/
    void addSolRev(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Solid of",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine, "Revolution",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          using (Solid3d p3dSolid = new Solid3d())
          {
            p3dSolid.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(p3dSolid);

            /**********************************************************************/
            /* Create a Circle                                                    */
            /**********************************************************************/
            using (Circle pCircle = new Circle())
            {
              pCircle.SetDatabaseDefaults(pDb);

              Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
              pCircle.Center = center + new Vector3d(w * 2.5 / 8.0, 0.0, 0.0);
              pCircle.Radius = w * 1.0 / 8.0;

              /**********************************************************************/
              /* Add it to the array of curves                                      */
              /**********************************************************************/
              DBObjectCollection entities = new DBObjectCollection();
              entities.Add(pCircle);
              /**********************************************************************/
              /* Create a region                                                    */
              /**********************************************************************/
              using (DBObjectCollection regions = Region.CreateFromCurves(entities))
              {
                System.Collections.IEnumerator enumerator = regions.GetEnumerator();
                /**********************************************************************/
                /* revolve the region                                                 */
                /**********************************************************************/

                //It disabled in ModelerGeometry
                /*if (enumerator.MoveNext())
                {
                  p3dSolid.Revolve((Region)enumerator.Current, center, new Vector3d(0.0, 1.0, 0.0), 2 * Math.PI);
                  // 3d solids should be disposed, because they lock modeler module
                  ((Region)enumerator.Current).Dispose();
                }*/
              }
            }
          }
        }
      }
    }

    void addHelix(ObjectId blockId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)blockId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Helix",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Create the Helix                                                   */
          /**********************************************************************/
          using (Helix pHelix = new Helix())
          {
            pHelix.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Add the Helix to the database                                      */
            /**********************************************************************/
            bBTR.AppendEntity(pHelix);

            /**********************************************************************/
            /* Set the Helix's parameters                                         */
            /**********************************************************************/
            pHelix.Constrain = ConstrainType.Height;
            pHelix.Height = h;
            pHelix.SetAxisPoint(point + new Vector3d(w / 2.0, -h / 2.0, 0.0), true);
            pHelix.StartPoint = pHelix.GetAxisPoint() + new Vector3d(w / 6.0, 0.0, 0.0);
            pHelix.Twist = false;
            pHelix.TopRadius = w * 3.0 / 8.0;
            pHelix.Turns = 6;

            /**********************************************************************/
            /* Create the Helix geometry (confirm parameters are set)             */
            /**********************************************************************/
            pHelix.CreateHelix();
          }
        }
      }
    }

    /************************************************************************/
    /* Add an Image to the specified BlockTableRecord                       */
    /************************************************************************/
    void addImage(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Open the Image Dictionary                                          */
          /**********************************************************************/
          ObjectId imageDictId = RasterImageDef.CreateImageDictionary(pDb);
          using (DBDictionary pImageDict = (DBDictionary)imageDictId.GetObject(OpenMode.ForWrite))
          {
            /**********************************************************************/
            /* Create an ImageDef object                                          */
            /**********************************************************************/
            using (RasterImageDef pImageDef = new RasterImageDef())
            {
              ObjectId imageDefId = pImageDict.SetAt("OdWriteEx", pImageDef);

              /**********************************************************************/
              /* Set some parameters                                                */
              /**********************************************************************/
              pImageDef.SourceFileName = String.Format("OdWriteEx.jpg");
              using (GIRasterImageDesc img = new GIRasterImageDesc(1024, 650, Units.Inch))
              {
                pImageDef.SetImage(img.UnmanagedObject, (IntPtr)0, true);

                /**********************************************************************/
                /* Create an Image object                                             */
                /**********************************************************************/
                using (RasterImage pImage = new RasterImage())
                {
                  pImage.SetDatabaseDefaults(pDb);
                  bBTR.AppendEntity(pImage);

                  /**********************************************************************/
                  /* Set some parameters                                                */
                  /**********************************************************************/
                  pImage.ImageDefId = imageDefId;
                  pImage.Orientation = new CoordinateSystem3d(point, new Vector3d(w, 0, 0), new Vector3d(0.0, h, 0));
                  pImage.DisplayOptions = ImageDisplayOptions.Show | ImageDisplayOptions.ShowUnaligned;

                  /**********************************************************************/
                  /* Add the label                                                      */
                  /**********************************************************************/
                  point = m_EntityBoxes.getBox(boxRow, boxCol);
                  addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "IMAGE",
                    m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
                }
              }
            }
          }
        }
      }
    }

    /************************************************************************/
    /* Add a Ray to the specified BlockTableRecord                          */
    /************************************************************************/
    void addRay(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          //  double w    = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "RAY",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a Ray from the center of the box and passing through        */
          /* the lower-left corner of the box                                   */
          /**********************************************************************/
          Ray pRay = new Ray();
          pRay.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pRay);

          Point3d basePoint = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          Vector3d unitDir = (point - basePoint).GetNormal();

          pRay.BasePoint = basePoint;
          pRay.UnitDir = unitDir;
          pRay.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add an Xline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addXline(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          //  double w    = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "XLINE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a Ray from the center of the box and passing through        */
          /* the lower-left corner of the box                                   */
          /**********************************************************************/
          Xline pXline = new Xline();
          pXline.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pXline);

          Point3d basePoint = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          Vector3d unitDir = (point - basePoint).GetNormal();

          pXline.BasePoint = basePoint;
          pXline.UnitDir = unitDir;
          pXline.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add Hatches to the specified BlockTableRecord                          */
    /************************************************************************/
    void addHatches(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          //  double h    = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);
          double delta = w / 12.0;

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "HATCHs",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Create a rectangular Hatch with a circular hole                    */
          /**********************************************************************/
          Hatch pHatch = new Hatch();
          pHatch.SetDatabaseDefaults(pDb);
          ObjectId whiteHatchId = bBTR.AppendEntity(pHatch);

          /**********************************************************************/
          /* Set some properties                                                */
          /**********************************************************************/
          pHatch.Associative = false;
          pHatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
          pHatch.HatchStyle = HatchStyle.Normal;

          /**********************************************************************/
          /* Define the outer loop with an OdGePolyline2d                       */
          /**********************************************************************/
          HatchLoop loop = new HatchLoop(HatchLoopTypes.External | HatchLoopTypes.Polyline);
          loop.Polyline.Add(new BulgeVertex(new Point2d(point.X + delta, point.Y - delta), 0));
          loop.Polyline.Add(new BulgeVertex(new Point2d(point.X + delta * 5, point.Y - delta), 0));
          loop.Polyline.Add(new BulgeVertex(new Point2d(point.X + delta * 5, point.Y - delta * 5), 0));
          loop.Polyline.Add(new BulgeVertex(new Point2d(point.X + delta, point.Y - delta * 5), 0));
          pHatch.AppendLoop(loop);

          /**********************************************************************/
          /* Define an inner loop with an array of edges                        */
          /**********************************************************************/
          Point2d cenPt = new Point2d(point.X + delta * 3, point.Y - delta * 3);
          using (CircularArc2d cirArc = new CircularArc2d())
          {
            cirArc.Center = cenPt;
            cirArc.Radius = delta;
            cirArc.SetAngles(0.0, 2 * Math.PI);

            loop = new HatchLoop(HatchLoopTypes.Default);
            loop.Curves.Add(cirArc);
          }
          pHatch.AppendLoop(loop);
          pHatch.Dispose();

          /**********************************************************************/
          /* Create a circular Hatch                                            */
          /**********************************************************************/
          pHatch = new Hatch();
          pHatch.SetDatabaseDefaults(pDb);
          ObjectId redHatchId = bBTR.AppendEntity(pHatch);

          /**********************************************************************/
          /* Set some properties                                                */
          /**********************************************************************/
          pHatch.Associative = false;
          pHatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
          pHatch.HatchStyle = HatchStyle.Normal;
          Color col = Color.FromRgb(255, 0, 0);
          pHatch.Color = col;

          /**********************************************************************/
          /* Define an outer loop with an array of edges                        */
          /**********************************************************************/
          using (CircularArc2d cirArc = new CircularArc2d())
          {
            cirArc.Center = cenPt - new Vector2d(delta, 0.0);
            cirArc.Radius = delta;
            cirArc.SetAngles(0.0, 2 * Math.PI);

            loop = new HatchLoop(HatchLoopTypes.Default);
            loop.Curves.Add(cirArc);
          }
          pHatch.AppendLoop(loop);
          pHatch.Dispose();

          /**********************************************************************/
          /* Create a circular Hatch                                            */
          /**********************************************************************/
          pHatch = new Hatch();
          pHatch.SetDatabaseDefaults(pDb);
          ObjectId greenHatchId = bBTR.AppendEntity(pHatch);

          pHatch.Associative = false;
          pHatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
          pHatch.HatchStyle = HatchStyle.Normal;
          col = Color.FromRgb(0, 255, 0);
          pHatch.Color = col;

          /**********************************************************************/
          /* Define an outer loop with an array of edges                        */
          /**********************************************************************/
          using (CircularArc2d cirArc = new CircularArc2d())
          {
            cirArc.Center = cenPt - new Vector2d(0.0, delta);
            cirArc.Radius = delta;
            cirArc.SetAngles(0.0, 2 * Math.PI);

            loop = new HatchLoop(HatchLoopTypes.Default);
            loop.Curves.Add(cirArc);
          }
          pHatch.AppendLoop(loop);
          pHatch.Dispose();

          /**********************************************************************/
          /* Use the SortentsTable to manipulate draw order                     */
          /*                                                                    */
          /* The draw order now is white, red, green                            */
          /**********************************************************************/
          using (DrawOrderTable pSET = (DrawOrderTable)bBTR.DrawOrderTableId.GetObject(OpenMode.ForWrite))
          {
            /**********************************************************************/
            /* Move the green hatch below the red hatch                           */
            /* The draw order now is white, green, red                            */
            /**********************************************************************/
            ObjectIdCollection id = new ObjectIdCollection();
            id.Add(greenHatchId);
            pSET.MoveBelow(id, redHatchId);

            /**********************************************************************/
            /* Create an associative user-defined hatch                           */
            /**********************************************************************/
            pHatch = new Hatch();
            pHatch.SetDatabaseDefaults(pDb);
            ObjectId hatchId = bBTR.AppendEntity(pHatch);

            /**********************************************************************/
            /* Set some properties                                                */
            /**********************************************************************/
            pHatch.Associative = true;
            pHatch.SetDatabaseDefaults(pDb); // make hatch aware of DB for the next call
            pHatch.SetHatchPattern(HatchPatternType.UserDefined, "_USER");
            pHatch.PatternSpace = 0.12;
            pHatch.PatternAngle = OdaToRadian(30.0);
            pHatch.PatternDouble = true;
            pHatch.HatchStyle = HatchStyle.Normal;

            /**********************************************************************/
            /* Define the loops                                                */
            /**********************************************************************/
            ObjectIdCollection loopIds = new ObjectIdCollection();
            Ellipse pEllipse = new Ellipse();
            pEllipse.SetDatabaseDefaults(pDb);
            loopIds.Add(bBTR.AppendEntity(pEllipse));

            Point3d centerPt = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            centerPt += new Vector3d(delta, delta * 1.5, 0);
            pEllipse.Set(centerPt, Vector3d.ZAxis, new Vector3d(delta, 0.0, 0.0), 0.5, 0, 2 * Math.PI);

            /**********************************************************************/
            /* Append the loops to the hatch                                      */
            /**********************************************************************/
            pHatch.AppendLoop(HatchLoopTypes.Default, loopIds);

            pEllipse.Dispose();
            pHatch.Dispose();

            try
            {
              /********************************************************************/
              /* Create an associative predefined hatch                           */
              /********************************************************************/
              pHatch = new Hatch();
              pHatch.SetDatabaseDefaults(pDb);
              hatchId = bBTR.AppendEntity(pHatch);

              /********************************************************************/
              /* Set some properties                                              */
              /********************************************************************/
              point = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
              // Set the hatch properties.
              pHatch.Associative = true;
              pHatch.SetDatabaseDefaults(pDb);// make hatch aware of DB for the next call
              pHatch.SetHatchPattern(HatchPatternType.PreDefined, "ANGLE");
              pHatch.PatternScale = 0.5;
              pHatch.PatternAngle = 0.5; // near 30 degrees
              pHatch.HatchStyle = HatchStyle.Normal;


              /********************************************************************/
              /* Define the loops                                                 */
              /********************************************************************/
              loopIds.Clear();
              Circle pCircle = new Circle();
              pCircle.SetDatabaseDefaults(pDb);
              loopIds.Add(bBTR.AppendEntity(pCircle));
              centerPt -= new Vector3d(delta * 2.0, delta * 2.5, 0);
              pCircle.Center = centerPt;
              pCircle.Radius = delta * 1.5;

              /********************************************************************/
              /* Append the loops to the hatch                                    */
              /********************************************************************/
              pHatch.AppendLoop(HatchLoopTypes.Default, loopIds);

              pCircle.Dispose();
              pHatch.Dispose();
            }
            catch (Exception e)
            {
              Console.WriteLine("\n\nException occurred: {0}\n", e.Message);
              Console.WriteLine("\nHatch with predefined pattern \"ANGLE\" was not added.\n");
              Console.WriteLine("\nMake sure PAT file with pattern definition is available to Teigha.");
              Console.WriteLine("\n\nPress ENTER to continue...");
              Console.ReadLine();
            }
          }
        }
      }
    }

    /************************************************************************/
    /* Add an Arc Dimension to the specified BlockTableRecord               */
    /************************************************************************/
    void addArcDimension(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Arc",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create an arc to be dimensioned                                    */
          /**********************************************************************/
          Arc pArc = new Arc();
          pArc.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pArc);
          Point3d center = point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
          pArc.Center = center;
          pArc.StartAngle = OdaToRadian(0.0);
          pArc.EndAngle = OdaToRadian(90.0);
          pArc.Radius = 4.0 / Math.PI;


          /**********************************************************************/
          /* Create an ArcDimension                                             */
          /**********************************************************************/
          ArcDimension pDimension = new ArcDimension(pArc.Center, pArc.StartPoint, pArc.EndPoint, pArc.Center + new Vector3d(pArc.Radius + 0.45, 0.0, 0.0), "", ObjectId.Null);
          pDimension.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pDimension);

          /**********************************************************************/
          /* Use the default dim variables                                      */
          /**********************************************************************/
          pDimension.SetDatabaseDefaults(pDb);

          /**********************************************************************/
          /* Set some parameters                                                */
          /**********************************************************************/
          pDimension.ArcSymbolType = 1;

          pDimension.Dispose();
          pArc.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a 3 Point Angular Dimension to the specified BlockTableRecord    */
    /************************************************************************/
    void add3PointAngularDimension(ObjectId btrId,
                                             int boxRow,
                                             int boxCol,
                                             ObjectId layerId,
                                             ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "3 Point Angular",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine, "Dimension",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create an arc to be dimensioned                                    */
          /**********************************************************************/
          Arc pArc = new Arc();
          pArc.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pArc);
          Point3d center = point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
          pArc.Center = center;
          pArc.StartAngle = OdaToRadian(0.0);
          pArc.EndAngle = OdaToRadian(90.0);
          pArc.Radius = w * 3.0 / 8.0;

          /**********************************************************************/
          /* Create 3 point angular dimension                                   */
          /**********************************************************************/
          using (Point3AngularDimension pDimension = new Point3AngularDimension())
          {
            pDimension.SetDatabaseDefaults(pDb);
            bBTR.AppendEntity(pDimension);

            /**********************************************************************/
            /* Use the default dim variables                                      */
            /**********************************************************************/
            pDimension.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/
            pDimension.CenterPoint = pArc.Center;
            pDimension.ArcPoint = pArc.Center + new Vector3d(pArc.Radius + 0.45, 0.0, 0.0);

            pDimension.XLine1Point = pArc.StartPoint;
            pDimension.XLine2Point = pArc.EndPoint;
          }
          pArc.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a 2 Line Angular Dimension to the specified BlockTableRecord     */
    /************************************************************************/
    void add2LineAngularDimension(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "2 Line Angular",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create the lines to be dimensioned                                 */
          /**********************************************************************/
          Point3d center = point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
          Vector3d v1 = new Vector3d(w * 1.0 / 8.0, 0.0, 0.0);
          Vector3d v2 = new Vector3d(w * 4.0 / 8.0, 0.0, 0.0);
          Vector3d v3 = v2 + new Vector3d(0.45, 0.0, 0.0);

          Line pLine1 = new Line();
          pLine1.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pLine1);
          pLine1.StartPoint = center + v1;
          pLine1.EndPoint = center + v2;

          double rot = OdaToRadian(75.0);
          v1 = v1.RotateBy(rot, Vector3d.ZAxis);
          v2 = v2.RotateBy(rot, Vector3d.ZAxis);

          Line pLine2 = new Line();
          pLine2.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pLine2);
          pLine2.StartPoint = center + v1;
          pLine2.EndPoint = center + v2;

          /**********************************************************************/
          /* Create 2 Line Angular Dimensionn                                   */
          /**********************************************************************/
          using (LineAngularDimension2 pDimension = new LineAngularDimension2())
          {
            bBTR.AppendEntity(pDimension);

            /**********************************************************************/
            /* Use the default dim variables                                      */
            /**********************************************************************/
            pDimension.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/

            v3 = v3.RotateBy(rot / 2.0, Vector3d.ZAxis);
            pDimension.ArcPoint = center + v3;

            pDimension.XLine1Start = pLine1.StartPoint;
            pDimension.XLine1End = pLine1.EndPoint;

            //  pDimension.setArcPoint(endPoint + 0.45*(endPoint - startPoint).normalize());

            pDimension.XLine2Start = pLine2.StartPoint;
            pDimension.XLine2End = pLine2.EndPoint;
          }
          pLine2.Dispose();
          pLine1.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a RadialDimensionLarge to the specified BlockTableRecord         */
    /************************************************************************/
    void addRadialDimensionLarge(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Radia",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dim Large", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create an arc to be dimensioned                                    */
          /**********************************************************************/
          Arc pArc = new Arc();
          pArc.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pArc);

          Point3d center = point + new Vector3d(w * 1.0 / 8.0, h * 1.0 / 8.0, 0.0);
          pArc.Radius = 2.0;

          pArc.Center = center;
          pArc.StartAngle = OdaToRadian(30.0);
          pArc.EndAngle = OdaToRadian(90.0);
          /**********************************************************************/
          /* Create RadialDimensionLarge                                        */
          /**********************************************************************/
          using (RadialDimensionLarge pDimension = new RadialDimensionLarge())
          {
            bBTR.AppendEntity(pDimension);

            /**********************************************************************/
            /* Use the default dim variables                                      */
            /**********************************************************************/
            pDimension.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/
            Point3d centerPoint, chordPoint, overrideCenter, jogPoint, textPosition;

            // The centerPoint of the dimension is the center of the arc
            centerPoint = pArc.Center;

            // The chordPoint of the dimension is the midpoint of the arc
            chordPoint = centerPoint + new Vector3d(pArc.Radius, 0.0, 0.0).RotateBy(0.5 * (pArc.StartAngle + pArc.EndAngle), Vector3d.ZAxis);

            // The overrideCenter is just to the right of the actual center
            overrideCenter = centerPoint + new Vector3d(w * 3.0 / 8.0, 0.0, 0.0);

            // The jogPoint is halfway between the overrideCenter and the chordCoint
            jogPoint = overrideCenter + 0.5 * (chordPoint - overrideCenter);

            // The textPosition is along the vector between the centerPoint and the chordPoint.
            textPosition = centerPoint + 0.7 * (chordPoint - centerPoint);

            double jogAngle = OdaToRadian(45.0);

            pDimension.Center = centerPoint;
            pDimension.ChordPoint = chordPoint;
            pDimension.OverrideCenter = overrideCenter;
            pDimension.JogPoint = jogPoint;
            pDimension.TextPosition = textPosition;
            pDimension.JogAngle = jogAngle;
          }
          pArc.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add Ordinate Dimensions to the specified BlockTableRecord            */
    /************************************************************************/
    void addOrdinateDimensions(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {

          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "Ordinate",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          addTextEnt(bBTR, point + m_textOffset + m_textLine, point + m_textOffset + m_textLine,
            "Dimension", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          double dx = w / 8.0;
          double dy = h / 8.0;
          /**********************************************************************/
          /* Create a line to be dimensioned                                    */
          /**********************************************************************/
          Line pLine = new Line();
          pLine.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pLine);

          Point3d point1 = point + new Vector3d(dx, dy, 0.0);
          Point3d point2 = point1 + new Vector3d(0.0, 1.5, 0);
          pLine.StartPoint = point1;
          pLine.EndPoint = point2;

          /**********************************************************************/
          /* Create the base ordinate dimension                                 */
          /**********************************************************************/
          Point3d endPoint, startPoint, leaderEndPoint;
          using (OrdinateDimension pDimension = new OrdinateDimension())
          {
            bBTR.AppendEntity(pDimension);

            /**********************************************************************/
            /* Use the default dim variables                                      */
            /**********************************************************************/
            pDimension.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/

            startPoint = pLine.StartPoint;
            endPoint = pLine.EndPoint;

            leaderEndPoint = startPoint + new Vector3d(3.0 * dx, 0, 0.0);

            pDimension.Origin = startPoint;
            pDimension.DefiningPoint = startPoint;
            pDimension.LeaderEndPoint = leaderEndPoint;
            pDimension.UsingXAxis = false;
          }

          /**********************************************************************/
          /* Create an ordinate dimension                                       */
          /**********************************************************************/
          using (OrdinateDimension pDimension = new OrdinateDimension())
          {
            bBTR.AppendEntity(pDimension);

            /**********************************************************************/
            /* Use the default dim variables                                      */
            /**********************************************************************/
            pDimension.SetDatabaseDefaults(pDb);

            /**********************************************************************/
            /* Set some parameters                                                */
            /**********************************************************************/
            leaderEndPoint = endPoint + new Vector3d(3.0 * dx, -dy, 0.0);

            pDimension.Origin = startPoint;
            pDimension.DefiningPoint = endPoint;
            pDimension.LeaderEndPoint = leaderEndPoint;
            pDimension.UsingXAxis = false;
          }
          pLine.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Spline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addSpline(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "SPLINE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create the fit points                                              */
          /**********************************************************************/

          double dx = w / 8.0;
          double dy = h / 8.0;

          Point3dCollection fitPoints = new Point3dCollection();
          fitPoints.Add(point + new Vector3d(1.0 * dx, 1.0 * dy, 0.0));
          fitPoints.Add(point + new Vector3d(3.0 * dx, 6.0 * dy, 0.0));
          fitPoints.Add(point + new Vector3d(4.0 * dx, 2.0 * dy, 0.0));
          fitPoints.Add(point + new Vector3d(7.0 * dx, 7.0 * dy, 0.0));

          /**********************************************************************/
          /* Create Spline                                                      */
          /**********************************************************************/
          Spline pSpline = new Spline(fitPoints, new Vector3d(0, 0, 0), new Vector3d(1.0, 0.0, 0.0), 3, 0.0);
          pSpline.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pSpline);
          pSpline.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add some Traces to the specified BlockTableRecord                    */
    /************************************************************************/
    void addTraces(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "TRACEs",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a Trace                                                     */
          /**********************************************************************/
          Trace pTrace = new Trace();
          pTrace.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pTrace);

          double dx = w / 8.0;
          double dy = h / 8.0;
          pTrace.SetPointAt(0, point + new Vector3d(1.0 * dx, 2.0 * dx, 0.0));
          pTrace.SetPointAt(1, point + new Vector3d(1.0 * dx, 1.0 * dx, 0.0));
          pTrace.SetPointAt(2, point + new Vector3d(6.0 * dx, 2.0 * dx, 0.0));
          pTrace.SetPointAt(3, point + new Vector3d(7.0 * dx, 1.0 * dx, 0.0));
          pTrace.Dispose();
          /**********************************************************************/
          /* Create a Trace                                                     */
          /**********************************************************************/
          pTrace = new Trace();
          pTrace.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pTrace);

          pTrace.SetPointAt(0, point + new Vector3d(6.0 * dx, 2.0 * dx, 0.0));
          pTrace.SetPointAt(1, point + new Vector3d(7.0 * dx, 1.0 * dx, 0.0));
          pTrace.SetPointAt(2, point + new Vector3d(6.0 * dx, 7.0 * dy, 0.0));
          pTrace.SetPointAt(3, point + new Vector3d(7.0 * dx, 7.0 * dy, 0.0));
          pTrace.Dispose();
        }
      }
    }
    /************************************************************************/
    /* Add an Mline to the specified BlockTableRecord                       */
    /************************************************************************/
    void addMLine(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = btrId.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the labels                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "MLINE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of MLine                                 */
          /**********************************************************************/
          point += new Vector3d(w / 10.0, -h / 2, 0);

          /**********************************************************************/
          /* Create an MLine and add it to the database                         */
          /**********************************************************************/
          Mline pMLine = new Mline();
          pMLine.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pMLine);

          /**********************************************************************/
          /* Open the MLineStyle dictionary, and set the style                  */
          /**********************************************************************/
          DBDictionary pMLDic = (DBDictionary)pDb.MLStyleDictionaryId.GetObject(OpenMode.ForRead);
          pMLine.Style = pMLDic.GetAt("OdaStandard");

          /**********************************************************************/
          /* Add some segments                                                  */
          /**********************************************************************/
          point -= new Vector3d(0, h / 2.2, 0);
          pMLine.AppendSegment(point);

          point += new Vector3d(0, h / 3.0, 0);
          pMLine.AppendSegment(point);

          point += new Vector3d(w / 4.0, h / 5.0, 0);
          pMLine.AppendSegment(point);

          point += new Vector3d(w / 4.0, 0, 0);
          pMLine.AppendSegment(point);

          point += new Vector3d(0, h / 3.0, 0);
          pMLine.AppendSegment(point);

          point += new Vector3d(w / 3, 0, 0);
          pMLine.AppendSegment(point);

          point -= new Vector3d(0, h / 2, 0);
          pMLine.AppendSegment(point);

          point -= new Vector3d(w / 4, h / 3, 0);
          pMLine.AppendSegment(point);

          pMLDic.Dispose();
          pMLine.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Polyline to the specified BlockTableRecord                     */
    /************************************************************************/
    void addPolyline(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "LWPOLYLINE",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a polyline                                                  */
          /**********************************************************************/
          Teigha.DatabaseServices.Polyline pPolyline = new Teigha.DatabaseServices.Polyline();
          pPolyline.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pPolyline);

          /**********************************************************************/
          /* Create the vertices                                                */
          /**********************************************************************/

          double dx = w / 8.0;
          double dy = h / 8.0;

          Point2d point2d = new Point2d(point.X + 1.5 * dx, point.Y + 3.0 * dy);

          pPolyline.AddVertexAt(0, point2d, 0, -1, -1);

          point2d -= new Vector2d(0, 1) * 0.5 * dy;
          pPolyline.AddVertexAt(1, point2d, 1.0, -1, -1);

          point2d += new Vector2d(1, 0) * 5.0 * dx;
          pPolyline.AddVertexAt(2, point2d, 0, -1, -1);

          point2d += new Vector2d(0, 1) * 4.0 * dy;
          pPolyline.AddVertexAt(3, point2d, 0, -1, -1);

          point2d -= new Vector2d(1, 0) * 1.0 * dx;
          pPolyline.AddVertexAt(4, point2d, 0, -1, -1);

          point2d -= new Vector2d(0, 1) * 4.0 * dy;
          pPolyline.AddVertexAt(5, point2d, -1, -1, -1);

          point2d -= new Vector2d(1, 0) * 3.0 * dx;
          pPolyline.AddVertexAt(6, point2d, 0, -1, -1);

          point2d += new Vector2d(0, 1) * 0.5 * dy;
          pPolyline.AddVertexAt(7, point2d, 0, -1, -1);

          pPolyline.Closed = true;

          pPolyline.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Wipeout to to the specified BlockTableRecord                   */
    /************************************************************************/
    void addWipeout(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = bBTR.Database)
        {
          /**********************************************************************/
          /* Get the lower-left corner and center of the box                    */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                     */
          /**********************************************************************/
          addTextEnt(bBTR, point + m_textOffset, point + m_textOffset, "WIPEOUT",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /**********************************************************************/
          /* Get the lower-left corner of the box                               */
          /**********************************************************************/
          point -= Vector3d.YAxis * h;

          /**********************************************************************/
          /* Create a hatch object to be wiped out                              */
          /**********************************************************************/
          Hatch pHatch = new Hatch();
          pHatch.SetDatabaseDefaults(pDb);
          ObjectId hatchId = bBTR.AppendEntity(pHatch);

          /**********************************************************************/
          /* Create a hatch object to be wiped out                              */
          /**********************************************************************/
          pHatch.Associative = true;
          pHatch.SetHatchPattern(HatchPatternType.UserDefined, "_USER");
          pHatch.PatternSpace = 0.125;
          pHatch.PatternAngle = 0.5; // near 30 degrees
          pHatch.PatternDouble = true; // Cross hatch
          pHatch.HatchStyle = HatchStyle.Normal;

          /**********************************************************************/
          /* Create an outer loop for the hatch                                 */
          /**********************************************************************/
          Circle pCircle = new Circle();
          pCircle.SetDatabaseDefaults(pDb);
          ObjectIdCollection loopIds = new ObjectIdCollection();
          loopIds.Add(bBTR.AppendEntity(pCircle));
          pCircle.Center = center;
          pCircle.Radius = Math.Min(w, h) * 0.4;
          pHatch.AppendLoop(HatchLoopTypes.Default, loopIds);

          /**********************************************************************/
          /* Create the wipeout                                                  */
          /**********************************************************************/
          Wipeout pWipeout = new Wipeout();
          pWipeout.SetDatabaseDefaults(pDb);
          bBTR.AppendEntity(pWipeout);

          Point2d center2d = new Point2d(center.X, center.Y);
          Point2dCollection boundary = new Point2dCollection();
          boundary.Add(center2d + new Vector2d(-w * 0.4, -h * 0.4));
          boundary.Add(center2d + new Vector2d(w * 0.4, -h * 0.4));
          boundary.Add(center2d + new Vector2d(0.0, h * 0.4));
          boundary.Add(center2d + new Vector2d(-w * 0.4, -h * 0.4));

          pWipeout.SetClipBoundary(ClipBoundaryType.Poly, boundary);

          pWipeout.DisplayOptions = ImageDisplayOptions.Show
                                  | ImageDisplayOptions.Clip
                                  | ImageDisplayOptions.Transparent;

          pWipeout.Dispose();
          pCircle.Dispose();
          pHatch.Dispose();
        }
      }
    }

    /************************************************************************/
    /* Add a Table to the specified BlockTableRecord                        */
    /************************************************************************/
    void addTable(ObjectId btrId, ObjectId addedBlockId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord pRecord = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        using (Database pDb = pRecord.Database)
        {
          /**********************************************************************/
          /* Get the lower-left corner and center of the box                    */
          /**********************************************************************/
          Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
          Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);


          /**********************************************************************/
          /* Create the Table                                                  */
          /**********************************************************************/
          using (Table pAcadTable = new Table())
          {
            ObjectId tableId = pRecord.AppendEntity(pAcadTable);
            /**********************************************************************/
            /* This entity will later get its own viewport                        */
            /**********************************************************************/
            m_layoutEntities.Add(tableId);

            /**********************************************************************/
            /* Set the parameters                                                 */
            /**********************************************************************/
            pAcadTable.SetDatabaseDefaults(pRecord.Database);
            pAcadTable.SetSize(4, 3);

            pAcadTable.GenerateLayout();
            pAcadTable.SetColumnWidth(w / pAcadTable.Columns.Count);
            pAcadTable.SetRowHeight(h / pAcadTable.Rows.Count);

            pAcadTable.Position = point;
            pAcadTable.SetTextStyle(styleId, (int)(RowType.DataRow | RowType.HeaderRow | RowType.TitleRow));

            pAcadTable.SetTextHeight(0.500 * pAcadTable.RowHeight(0), (int)RowType.TitleRow);
            pAcadTable.SetTextHeight(0.300 * pAcadTable.RowHeight(1), (int)RowType.HeaderRow);
            pAcadTable.SetTextHeight(0.250 * pAcadTable.RowHeight(2), (int)RowType.DataRow);

            /**********************************************************************/
            /* Set the alignments                                                 */
            /**********************************************************************/
            int numRows = pAcadTable.Rows.Count;
            int numColumns = pAcadTable.Columns.Count;

            for (int row = 1; row < numRows; row++)
            {
              for (int col = 0; col < numColumns; col++)
              {
                pAcadTable.SetAlignment(row, col, CellAlignment.MiddleCenter);
              }
            }

            /**********************************************************************/
            /* Define the title row                                               */
            /**********************************************************************/
            CellRange cRange = CellRange.Create(pAcadTable, 0, 0, 0, pAcadTable.Columns.Count - 1);
            pAcadTable.MergeCells(cRange);
            pAcadTable.SetTextString(0, 0, "Title of TABLE");

            /**********************************************************************/
            /* Define the header row                                              */
            /**********************************************************************/
            pAcadTable.SetTextString(1, 0, "Header0");
            pAcadTable.SetTextString(1, 1, "Header1");
            pAcadTable.SetTextString(1, 2, "Header2");

            /**********************************************************************/
            /* Define the first data row                                          */
            /**********************************************************************/
            pAcadTable.SetTextString(2, 0, "Data0");
            pAcadTable.SetTextString(2, 1, "Data1");
            pAcadTable.SetTextString(2, 2, "Data2");

            /**********************************************************************/
            /* Define the second data row                                         */
            /**********************************************************************/
            pAcadTable.SetCellType(3, 0, TableCellType.BlockCell);
            pAcadTable.SetBlockTableRecordId(3, 0, addedBlockId, false);
            pAcadTable.SetBlockScale(3, 0, 1.0);
            pAcadTable.SetAutoScale(3, 0, true);
            pAcadTable.SetBlockRotation(3, 0, 0.0);

            pAcadTable.SetTextString(3, 1, "<-Block Cell.");

            pAcadTable.SetCellType(3, 2, TableCellType.BlockCell);
            pAcadTable.SetBlockTableRecordId(3, 2, addedBlockId, false);
            pAcadTable.SetAutoScale(3, 2, true);
            pAcadTable.SetBlockRotation(3, 2, OdaToRadian(30.0));

            pAcadTable.RecomputeTableBlock(true);

            /**********************************************************************/
            /* Add the label                                                     */
            /**********************************************************************/
            addTextEnt(pRecord, point + m_textOffset, point + m_textOffset, "ACAD_TABLE",
              m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
          }
        }
      }
    }

    /************************************************************************/
    /* Add a Text with Field to the specified BlockTableRecord              */
    /************************************************************************/
    void addTextWithField(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId, ObjectId noteStyleId)
    {
      using (BlockTableRecord pRecord = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);

        Point3d textPos1 = point;
        textPos1 += new Vector3d(w / 15.0, -h / 3.0, 0);

        Point3d textPos2 = point;
        textPos2 += new Vector3d(w / 15.0, -2.0 * h / 3.0, 0);

        double textHeight = m_EntityBoxes.getHeight() / 12.0;

        /**********************************************************************/
        /* Prepare the text entities                                           */
        /**********************************************************************/
        using (DBText pText1 = new DBText())
        {
          ObjectId textId = pRecord.AppendEntity(pText1);
          using (DBText pText2 = new DBText())
          {
            ObjectId textId2 = pRecord.AppendEntity(pText2);

            pText1.Position = textPos1;
            pText1.Height = textHeight;
            pText2.Position = textPos2;
            pText2.Height = textHeight;
            if (!styleId.IsNull)
            {
              pText1.TextStyleId = styleId;
              pText2.TextStyleId = styleId;
            }

            /**********************************************************************/
            /* Create field objects                                               */
            /**********************************************************************/
            Field pField1_1 = new Field();

            Field pTextField2 = new Field();
            Field pField2_1 = new Field();
            Field pField2_2 = new Field();
            using (Field pTextField1 = new Field())
            {
              /**********************************************************************/
              /* Set field objects                                                  */
              /**********************************************************************/
              ObjectId textFldId1 = pText1.SetField("TEXT", pTextField1);
              ObjectId fldId1_1 = pTextField1.SetField("", pField1_1);
              ObjectId textFldId2 = pText2.SetField("TEXT", pTextField2);

              /**********************************************************************/
              /* Set field property                                                 */
              /**********************************************************************/

              pField1_1.EvaluationOption = FieldEvaluationOptions.Automatic;
              string fc1 = "\\AcVar Comments";
              pField1_1.SetFieldCode(fc1);

              pTextField1.EvaluationOption = FieldEvaluationOptions.Automatic;
              string fc2 = "%<\\_FldIdx 0>%";

              FieldCodeWithChildren fcwChd = pTextField2.GetFieldCodeWithChildren();
              fcwChd.FieldCode = fc2;
              pTextField1.SetFieldCodeWithChildren(FieldCodeFlags.TextField | FieldCodeFlags.PreserveFields, fcwChd);

              /**********************************************************************/
              /* Evaluate field                                                     */
              /**********************************************************************/
              pField1_1.Evaluate((int)FieldEvaluationOptions.Automatic, null); // TODO:

              pTextField2.EvaluationOption = FieldEvaluationOptions.Automatic;
              string fc3 = "Date %<\\_FldIdx 0>% Time %<\\_FldIdx 1>%";

              FieldCodeWithChildren fcwChd1 = pTextField2.GetFieldCodeWithChildren();
              fcwChd1.Add(0, pField2_1);
              fcwChd1.Add(1, pField2_2);
              fcwChd1.FieldCode = fc3;
              pTextField2.SetFieldCodeWithChildren(FieldCodeFlags.TextField, fcwChd1);

              pField2_1.EvaluationOption = FieldEvaluationOptions.Automatic;
              string fc4 = "\\AcVar Date \\f M/dd/yyyy";
              pField2_1.SetFieldCode(fc4);

              pField2_2.EvaluationOption = FieldEvaluationOptions.Automatic;
              string fc5 = "\\AcVar Date \\f h:mm tt";
              pField2_2.SetFieldCode(fc5);

              /**********************************************************************/
              /* Evaluate fields                                                    */
              /**********************************************************************/
              pField2_1.Evaluate((int)FieldEvaluationOptions.Automatic, null);
              pField2_2.Evaluate((int)FieldEvaluationOptions.Automatic, null);

              /**********************************************************************/
              /* Add the label                                                      */
              /**********************************************************************/
              addTextEnt(pRecord, point + m_textOffset, point + m_textOffset, "FIELDS",
                m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, noteStyleId);
            }
            pField1_1.Dispose();
            pTextField2.Dispose();
            pField2_1.Dispose();
            pField2_2.Dispose();
          }
        }
      }
    }

    /************************************************************************/
    /* Prefix a file name with the Current Directory                        */
    /************************************************************************/
    string inCurrentFolder(string fileName)
    {
      return Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(fileName);
    }

    /************************************************************************/
    /* Add an OLE object to the specified BlockTableRecord                  */
    /************************************************************************/
    void addOLE2FrameFromFile(ObjectId btrId, int boxRow, int boxCol, String fileName, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord pBlock = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        /**********************************************************************/
        /* Get the lower-left corner and center of the box                    */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        Point3d center = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);

        /**********************************************************************/
        /* Create an ole2frame entity from arbitrary file using Windows OLE RT*/
        /**********************************************************************/
        using (Ole2Frame ole2FrameObj = new Ole2Frame())
        {
          if (ole2FrameObj.CreateFromFile(inCurrentFolder(fileName)))
          {
            ole2FrameObj.SetDatabaseDefaults(pBlock.Database);
            pBlock.AppendEntity(ole2FrameObj);

            /**********************************************************************/
            /* Add the label                                                      */
            /**********************************************************************/
            addTextEnt(pBlock, point + m_textOffset, point + m_textOffset,
              "OLE2: " + ole2FrameObj.UserType, m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

            /**********************************************************************/
            /* Inscribe OLE frame in entity box                                   */
            /**********************************************************************/
            h += m_textOffset.Y - (m_textSize * 1.5);
            center = new Point3d(center.X, center.Y + (m_textOffset.Y / 2.0) - (m_textSize * 1.5 / 2.0), center.Z);

            h *= 0.85;
            w *= 0.85;

            double oh = ole2FrameObj.HimetricWidth;
            double ow = ole2FrameObj.HimetricHeight;
            if (oh / ow < h / w)
            {
              h = w * oh / ow;
            }
            else
            {
              w = h * ow / oh;
            }

            Rectangle3d rect = new Rectangle3d(center + new Vector3d(h, -w, 0), center + new Vector3d(h, w, 0),
                                               center + new Vector3d(-h, -w, 0), center + new Vector3d(-h, w, 0));
            ole2FrameObj.Position3d = rect;
          }
        }
      }
    }

    void addDwfUnderlay(ObjectId blockId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the BlockTableRecord                                          */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)blockId.GetObject(OpenMode.ForWrite))
      {

        /**********************************************************************/
        /* Get the Upper-left corner of the box and its size                  */
        /**********************************************************************/
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();
        double w = m_EntityBoxes.getWidth(boxRow, boxCol);

        /**********************************************************************/
        /* Add the label                                                      */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "Dwf reference", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Create the Dwf definition                                          */
        /**********************************************************************/
        using (DwfDefinition pDwfDef = new DwfDefinition())
        {
          String itemName = "Unsaved Drawing-Mode";
          pDwfDef.SourceFileName = "OdWriteEx.dwf";
          pDwfDef.ItemName = itemName;

          // Post to database
          ObjectId idDef = ObjectId.Null;
          {
            string dictName = UnderlayDefinition.GetDictionaryKey(pDwfDef.GetType());

            Database pDb = blockId.Database;
            using (DBDictionary pDict = (DBDictionary)pDb.NamedObjectsDictionaryId.GetObject(OpenMode.ForWrite))
            {
              ObjectId idDefDict = pDict.GetAt(dictName);
              if (idDefDict.IsNull)
              {
                DBDictionary dictTmp = new DBDictionary();
                idDefDict = pDict.SetAt(dictName, dictTmp);
                dictTmp.Dispose();
              }

              using (DBDictionary pDefs = (DBDictionary)idDefDict.GetObject(OpenMode.ForWrite))
              {
                idDef = pDefs.SetAt("OdWriteEx - " + itemName, pDwfDef);
              }
            }
          }
          /**********************************************************************/
          /* Create the Dwf reference                                           */
          /**********************************************************************/
          using (DwfReference pDwfRef = new DwfReference())
          {
            pDwfRef.SetDatabaseDefaults(bBTR.Database);

            /**********************************************************************/
            /* Add the Dwf reference to the database                              */
            /**********************************************************************/
            bBTR.AppendEntity(pDwfRef);

            /**********************************************************************/
            /* Set the Dwf reference's parameters                                 */
            /**********************************************************************/
            pDwfRef.DefinitionId = idDef;
            pDwfRef.Position = point + new Vector3d(-w / 4, -h / 2, 0.0);
            pDwfRef.ScaleFactors = new Scale3d(0.001);
          }
        }
      }
    }


    /************************************************************************/
    /* Add some lights to the specified BlockTableRecord                    */
    /************************************************************************/
    void addLights(ObjectId btrId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)btrId.GetObject(OpenMode.ForWrite))
      {
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);

        /**********************************************************************/
        /* Add the label                                                      */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "Light", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Create a Light                                                     */
        /**********************************************************************/
        using (Light lt = new Light())
        {
          lt.SetDatabaseDefaults(bBTR.Database);
          bBTR.AppendEntity(lt);

          lt.Position = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
          lt.LightType = DrawableType.PointLight;
        }
      }
    }

    /************************************************************************/
    /* Add some PdfUnderlay to the specified BlockTableRecord                    */
    /************************************************************************/
    void addPdfUnderlay(ObjectId blockId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      /**********************************************************************/
      /* Open the Block Table Record                                        */
      /**********************************************************************/
      using (BlockTableRecord bBTR = (BlockTableRecord)blockId.GetObject(OpenMode.ForWrite))
      {
        Point3d point = m_EntityBoxes.getBox(boxRow, boxCol);
        double h = m_EntityBoxes.getHeight();

        /**********************************************************************/
        /* Add the label                                                      */
        /**********************************************************************/
        addTextEnt(bBTR, point + m_textOffset, point + m_textOffset,
          "Pdf reference", m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

        /**********************************************************************/
        /* Create the Pdf definition                                          */
        /**********************************************************************/
        using (PdfDefinition pPdfDef = new PdfDefinition())
        {
          String itemName = "1";
          pPdfDef.SourceFileName = "OdWriteEx.pdf";
          pPdfDef.ItemName = itemName;

          // Post to database
          ObjectId idDef = ObjectId.Null;
          {
            string dictName = UnderlayDefinition.GetDictionaryKey(pPdfDef.GetType());

            Database pDb = blockId.Database;
            using (DBDictionary pDict = (DBDictionary)pDb.NamedObjectsDictionaryId.GetObject(OpenMode.ForWrite))
            {
              ObjectId idDefDict = pDict.GetAt(dictName);
              if (idDefDict.IsNull)
              {
                DBDictionary dictTmp = new DBDictionary();
                idDefDict = pDict.SetAt(dictName, dictTmp);
                dictTmp.Dispose();
              }

              using (DBDictionary pDefs = (DBDictionary)idDefDict.GetObject(OpenMode.ForWrite))
              {
                idDef = pDefs.SetAt("OdWriteEx - " + itemName, pPdfDef);
              }
            }
          }


          /**********************************************************************/
          /* Create the Pdf reference                                           */
          /**********************************************************************/
          using (PdfReference pPdfRef = new PdfReference())
          {
            pPdfRef.SetDatabaseDefaults(bBTR.Database);

            /**********************************************************************/
            /* Add the Pdf reference to the database                              */
            /**********************************************************************/
            bBTR.AppendEntity(pPdfRef);

            /**********************************************************************/
            /* Set the Pdf reference's parameters                                 */
            /**********************************************************************/
            pPdfRef.DefinitionId = idDef;
            pPdfRef.Position = point + new Vector3d(0, -h, 0.0);
            pPdfRef.ScaleFactors = new Scale3d(0.2);
          }
        }
      }
    }

    /************************************************************************/
    /* Add some lights to the specified BlockTableRecord                    */
    /************************************************************************/
    void addTrimmedNURBSurface(ObjectId blockId, int boxRow, int boxCol, ObjectId layerId, ObjectId styleId)
    {
      using (Database pDb = blockId.Database)
      {
        /**********************************************************************/
        /* Open the BlockTableRecord                                          */
        /**********************************************************************/
        using (BlockTableRecord bBTR = (BlockTableRecord)blockId.GetObject(OpenMode.ForWrite))
        {
          /**********************************************************************/
          /* Get the Upper-left corner of the box and its size                  */
          /**********************************************************************/
          Point3d ptUpperLeft = m_EntityBoxes.getBox(boxRow, boxCol);
          double h = m_EntityBoxes.getHeight();
          double w = m_EntityBoxes.getWidth(boxRow, boxCol);

          /**********************************************************************/
          /* Add the label                                                      */
          /**********************************************************************/
          addTextEnt(bBTR, ptUpperLeft + m_textOffset, ptUpperLeft + m_textOffset, "Trimmed",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);
          addTextEnt(bBTR, ptUpperLeft + m_textOffset + m_textLine, ptUpperLeft + m_textOffset + m_textLine, "NURBS surface",
            m_textSize, TextHorizontalMode.TextLeft, TextVerticalMode.TextTop, layerId, styleId);

          /********************************************************************/
          /* Create NURBS surface, trimming-loop and holes                    */
          /********************************************************************/
          Geometry::NurbCurve3d[][] arrLoops = new Geometry::NurbCurve3d[3][];
          Geometry::NurbCurve2d[][] arrLoopsProj = new Geometry::NurbCurve2d[3][];
          Point3dCollection arrApex = new Point3dCollection();

          Geometry.NurbSurface nsf = fillNurbData(arrLoops, arrLoopsProj);

          /********************************************************************/
          /* Create NURBS Surface                                             */
          /********************************************************************/
          DatabaseServices::NurbSurface nrbSrf = DatabaseServices::NurbSurface.createNurbSurface(
                                                                    nsf, arrLoops, arrApex, arrLoopsProj, true);

          /**********************************************************************/
          /* Add trimmed NURBS surface to database                              */
          /**********************************************************************/
          if (nrbSrf != null)
          {
            Extents3d ext3d = nrbSrf.GeometricExtents;
            Point3d maxPt = ext3d.MaxPoint;
            Point3d minPt = ext3d.MinPoint;
            Point3d center = new Point3d((maxPt.X - minPt.X) / 2, (maxPt.Y - minPt.Y) / 2, (maxPt.Z - minPt.Z) / 2);
            Matrix3d xfm;
            double scaleX = w * 0.7 / (maxPt.X - minPt.X);
            double scaleY = h * 0.7 / (maxPt.Y - minPt.Y);
            double dScale = Math.Min(scaleX, scaleY);
            xfm = Matrix3d.Scaling(dScale, center);
            nrbSrf.TransformBy(xfm);
            Point3d centerBox = m_EntityBoxes.getBoxCenter(boxRow, boxCol);
            xfm = Matrix3d.Displacement(centerBox.GetAsVector() - center.GetAsVector());
            nrbSrf.TransformBy(xfm);

            ObjectId brefId;
            nrbSrf.SetDatabaseDefaults(pDb);
            brefId = bBTR.AppendEntity(nrbSrf);
          }
          else
          {
            return;
          }
          /**********************************************************************/
          /* Add the NURBS to Sortents table                                   */
          /**********************************************************************/
          using (DrawOrderTable pSET = (DrawOrderTable)bBTR.DrawOrderTableId.GetObject(OpenMode.ForWrite))
          {
            ObjectIdCollection arObjId = new ObjectIdCollection();
            arObjId.Add(nrbSrf.Id);
            pSET.MoveToBottom(arObjId);
          }
        }
      }
    }
    static Geometry.NurbSurface fillNurbData(Geometry::NurbCurve3d[][] arrLoops, 
                                             Geometry::NurbCurve2d[][] arrLoopsProj)
    {
      int degreeInU = 3;
      int degreeInV = 3;
      int propsInU = 0x01 | 0x10;
      int propsInV = 0x01 | 0x10;
      int numControlPointsInU = 6;
      int numControlPointsInV = 5;

      Point3d[] arrCPNURBS = {
                        new Point3d(118.79646513774851, 710.42519309280760, -104.06947147193246), new Point3d(129.71270653783793, 710.42519309280760, -103.45170852530278),
                        new Point3d(151.54518933801671, 710.42519309280783, -102.21618263204338), new Point3d(173.37767213819544, 710.42519309280738, -100.98065673878389),
                        new Point3d(184.29391353828487, 710.42519309280749, -100.36289379215422), new Point3d(118.97589960333517, 700.08509830913147, -107.24018598714486),
                        new Point3d(129.89214100342454, 700.08509830913158, -106.62242304051519), new Point3d(151.72462380360335, 700.08509830913158, -105.38689714725582),
                        new Point3d(173.55710660378205, 700.08509830913135, -104.15137125399632), new Point3d(184.47334800387154, 700.08509830913135, -103.53360830736666),
                        new Point3d(119.53892391687886, 677.05994177564548, -117.18916294391204), new Point3d(130.45516531696822, 677.05994177564548, -116.57139999728233),
                        new Point3d(152.28764811714711, 677.05994177564583, -115.33587410402289), new Point3d(174.12013091732578, 677.05994177564514, -114.10034821076349),
                        new Point3d(185.03637231741524, 677.05994177564548, -113.48258526413380), new Point3d(120.29166660874762, 641.82635919357858, -130.49057731559242),
                        new Point3d(131.20790800883688, 641.82635919357847, -129.87281436896274), new Point3d(153.04039080901578, 641.82635919357892, -128.63728847570346),
                        new Point3d(174.87287360919453, 641.82635919357824, -127.40176258244379), new Point3d(185.78911500928388, 641.82635919357847, -126.78399963581420),
                        new Point3d(120.26279260065625, 622.09944102868099, -129.98035629273403), new Point3d(131.17903400074579, 622.09944102868099, -129.36259334610423),
                        new Point3d(153.01151680092445, 622.09944102868121, -128.12706745284484), new Point3d(174.84399960110321, 622.09944102868076, -126.89154155958539),
                        new Point3d(185.76024100119267, 622.09944102868099, -126.27377861295572), new Point3d(119.99830332501648, 614.23302502144543, -125.30667232322712),
                        new Point3d(130.91454472510588, 614.23302502144531, -124.68890937659742), new Point3d(152.74702752528469, 614.23302502144566, -123.45338348333803),
                        new Point3d(174.57951032546342, 614.23302502144509, -122.21785759007857), new Point3d(185.49575172555285, 614.23302502144543, -121.60009464344888) 
                       };
      double[] arrUKnots = {
                       -492.03378698802373, -492.03378698802373, -492.03378698802373, -492.03378698802373, -459.72999406537474, 
                       -427.42620114272574, -395.12240822007675, -395.12240822007675,-395.12240822007675, -395.12240822007675 
                      };
      double[] arrVKnots = { 
                       33.966095100818350, 33.966095100818350, 33.966095100818350, 33.966095100818350, 66.767217280848854, 
                       99.568339460879344, 99.568339460879344, 99.568339460879344, 99.568339460879344
                      };
      int degree = 3;
      bool periodic = false;
      Point3d[] arrTrimmingLoopCPS = {
                        new Point3d(171.39105257865623, 710.42519309280749, -101.09308187743311), new Point3d(168.45153099392516, 710.24630783872203, -101.31446239199624),
                        new Point3d(165.03780278917867, 710.04096849299788, -101.57113258477224), new Point3d(161.72398532880021, 709.75939476917233, -101.84694412623884),
                        new Point3d(158.62351701585555, 709.49594923576046, -102.10499845629354), new Point3d(155.44149648144887, 709.15222118981330, -102.39338712463184),
                        new Point3d(152.35447385300299, 708.65055480832643, -102.73003158400211), new Point3d(150.38336219539920, 708.33023307058954, -102.94498427408541),
                        new Point3d(148.41141827031126, 707.93974094135001, -103.18375232081615), new Point3d(146.48039847751295, 707.45271898872477, -103.45480437761124),
                        new Point3d(144.13328491671780, 706.86075414523918, -103.78426239795768), new Point3d(141.82012788269969, 706.11956074806778, -104.16489125408735),
                        new Point3d(139.61304361208579, 705.17257437144224, -104.61946728961887), new Point3d(138.66086998580863, 704.76402837060652, -104.81557910915096),
                        new Point3d(137.72937437730596, 704.31763725325436, -105.02530892492160), new Point3d(136.82263532321986, 703.82874838217583, -105.25090611949136),
                        new Point3d(135.24707366371524, 702.97924867489098, -105.64290676030878), new Point3d(133.76559536140749, 702.01186710908723, -106.07799761541743),
                        new Point3d(132.38696251928721, 700.90816224447622, -106.56683719584397), new Point3d(131.70643372391345, 700.36334500306566, -106.80814104458086),
                        new Point3d(131.05981186747948, 699.79239626818242, -107.05941147859264), new Point3d(130.44296675212314, 699.19291456340727, -107.32213538566057),
                        new Point3d(129.28179329087706, 698.06442666789189, -107.81669718233775), new Point3d(128.24964411961000, 696.85576350367262, -108.34262743419625),
                        new Point3d(127.32151753376262, 695.55912500349768, -108.90520394152732), new Point3d(126.05986121984823, 693.79652906501735, -109.66994682612655),
                        new Point3d(125.01563346464992, 691.90115433599408, -110.48983073800100), new Point3d(124.14957948895956, 689.96924218601703, -111.32380987168591),
                        new Point3d(123.65279423939776, 688.86106014829500, -111.80219637378208), new Point3d(123.20932251158908, 687.72894527624976, -112.29019657028263),
                        new Point3d(122.81418452632097, 686.58600043445324, -112.78131012630790), new Point3d(122.47566143794299, 685.60681535957838, -113.20205752070777),
                        new Point3d(122.17101616352592, 684.61505389942329, -113.62706576222860), new Point3d(121.89778406590716, 683.61771931493865, -114.05257322832603),
                        new Point3d(121.62165726607044, 682.60981867083331, -114.48258864724671), new Point3d(121.37638429395319, 681.59178730486644, -114.91500498394302),
                        new Point3d(121.15997754403588, 680.56960416837751, -115.34634743972245), new Point3d(120.86413845552143, 679.17222757310367, -115.93601462433720),
                        new Point3d(120.62056970802830, 677.75918096509565, -116.52701050128643), new Point3d(120.42525385172314, 676.34217748369235, -117.11172845453878),
                        new Point3d(120.23991635212870, 674.99756631321168, -117.66657415664328), new Point3d(120.09699741634057, 673.64195832055020, -118.21883336833054),
                        new Point3d(119.99434966581934, 672.28383738625189, -118.76246141734318), new Point3d(119.85794077409756, 670.47902655960377, -119.48489029959472),
                        new Point3d(119.79168261180797, 668.65714060443577, -120.19713266425772), new Point3d(119.79332389758527, 666.83396645506991, -120.88619378767380),
                        new Point3d(119.79530910319737, 664.62875904804514, -121.71964279954052), new Point3d(119.89664217194053, 662.40094421486242, -122.52698599074122),
                        new Point3d(120.09912703714075, 660.17746521580455, -123.28443347256476), new Point3d(120.28042690653920, 658.18661789128555, -123.96263294883188),
                        new Point3d(120.54447002204951, 656.18089228079589, -124.60706271254348), new Point3d(120.89656304790073, 654.18283906082149, -125.19855374401830),
                        new Point3d(121.20394936588089, 652.43848692282756, -125.71494071261493), new Point3d(121.57986563915610, 650.69135249467854, -126.19358416252174),
                        new Point3d(122.03013889813559, 648.95383395726083, -126.62230149889685), new Point3d(122.42327099025523, 647.43681227300658, -126.99661318633548),
                        new Point3d(122.87362810214434, 645.92495388064481, -127.33341142312995), new Point3d(123.38682170467966, 644.42568387024380, -127.62498621590397),
                        new Point3d(123.84762176351190, 643.07947903529430, -127.88679322402041), new Point3d(124.35863352017569, 641.74471369872651, -128.11188268359342),
                        new Point3d(124.92403892983181, 640.42642653552082, -128.29542159947766), new Point3d(125.44323510572396, 639.21587983367658, -128.46396031811577),
                        new Point3d(126.00710924548257, 638.02211894969321, -128.59706736862518), new Point3d(126.61962986827133, 636.84772067768324, -128.69204247356643),
                        new Point3d(127.13876213644848, 635.85237780103671, -128.77253713744247), new Point3d(127.68997195004231, 634.87637161242969, -128.82515955914860),
                        new Point3d(128.27333369543726, 633.92321205932672, -128.84950019737718), new Point3d(128.60120974789712, 633.38749268090874, -128.86318075226399),
                        new Point3d(128.93974898055905, 632.85850251958050, -128.86794979019851), new Point3d(129.28884575606682, 632.33620021597051, -128.86382119837117),
                        new Point3d(130.01186091137623, 631.25445868279905, -128.85527046209782), new Point3d(130.77759282273519, 630.20523200689797, -128.80857131668841),
                        new Point3d(131.58417042161156, 629.18767928479406, -128.72479723914768), new Point3d(131.99893114685352, 628.66443030592370, -128.68171868451674),
                        new Point3d(132.42331356075661, 628.15099375492480, -128.62898596396130), new Point3d(132.85873736495847, 627.64514009941581, -128.56659620468938),
                        new Point3d(133.74288558496079, 626.61798080131609, -128.43991090619554), new Point3d(134.66506683387942, 625.63056270817390, -128.27425408339136),
                        new Point3d(135.62269381216180, 624.68436000296435, -128.07325101896225), new Point3d(136.57010557100278, 623.74825065194068, -127.87439209881700),
                        new Point3d(137.55209383741553, 622.85283869031366, -127.64083200198527), new Point3d(138.56024534776932, 622.00488828210109, -127.37961320614097),
                        new Point3d(139.06370221742117, 621.58143361903274, -127.24916416559562), new Point3d(139.57475859301738, 621.16895472073099, -127.11160309477718),
                        new Point3d(140.09290217628740, 620.76819601938621, -126.96795196086232), new Point3d(142.33390303293766, 619.03489154158922, -126.34665252851133),
                        new Point3d(144.53632952352163, 617.65093287836089, -125.65897328370222), new Point3d(146.59131054119746, 616.60184914379715, -125.05424117190655),
                        new Point3d(146.91512539377837, 616.43653915215691, -124.95895015074981), new Point3d(147.24219466268028, 616.27607385324018, -124.86366837591413),
                        new Point3d(147.57151153542782, 616.12158095477980, -124.76931687774632), new Point3d(148.58918451999781, 615.64415856290759, -124.47774672545505),
                        new Point3d(149.67525815809597, 615.20194517187645, -124.18026618355094), new Point3d(150.75304306365624, 614.86568191081744, -123.93139954747051),
                        new Point3d(151.38799406166038, 614.66758051591012, -123.78478576754652), new Point3d(152.05545784262085, 614.49531328424234, -123.64717116989956),
                        new Point3d(152.71867340881226, 614.38143668850023, -123.54278211023281), new Point3d(153.04428270292334, 614.32552835364572, -123.49153173288384),
                        new Point3d(153.37462233191070, 614.28269459432261, -123.44741350166518), new Point3d(153.70466897901099, 614.25952555332526, -123.41496338982839),
                        new Point3d(154.01290965727731, 614.23788727590431, -123.38465723592479), new Point3d(154.32794135257140, 614.23302023944188, -123.36392832421669),
                        new Point3d(154.63770236996223, 614.25411692127079, -123.35894599242118), new Point3d(154.85651374438024, 614.26901935794763, -123.35542653449467),
                        new Point3d(155.07634610716954, 614.29712209707179, -123.35970757656011), new Point3d(155.29061930026575, 614.34296658298899, -123.37460256439837),
                        new Point3d(155.46014533950282, 614.37923726158442, -123.38638699835495), new Point3d(155.62669110296792, 614.42670084637314, -123.40483831040376),
                        new Point3d(155.78717621272526, 614.48783319215613, -123.43125968045800), new Point3d(155.86074829002084, 614.51585843185603, -123.44337217539803),
                        new Point3d(155.93286444949152, 614.54668859280378, -123.45712731401389), new Point3d(156.00326974748407, 614.58055872976604, -123.47262803464767),
                        new Point3d(156.25308287277991, 614.70073725339989, -123.52762792210322), new Point3d(156.47233983492339, 614.85480688446012, -123.60270268963077),
                        new Point3d(156.65670195093233, 615.04637726458725, -123.69732739350940), new Point3d(156.75827103689323, 615.15191753949784, -123.74945819397063),
                        new Point3d(156.84604464892567, 615.26559221496541, -123.80588423454684), new Point3d(156.92262637487056, 615.38678943753030, -123.86571177389959),
                        new Point3d(157.01270243503129, 615.52934261423616, -123.93608142106430), new Point3d(157.08622577155759, 615.68065225220596, -124.01030602270427),
                        new Point3d(157.14680491829381, 615.83577043124433, -124.08522912559482), new Point3d(157.17994362032164, 615.92062496273252, -124.12621442359682),
                        new Point3d(157.20936168371361, 616.00700837461659, -124.16760487220166), new Point3d(157.23552053087661, 616.09410786712772, -124.20893538833489),
                        new Point3d(157.26338135434762, 616.18687432469153, -124.25295499695058), new Point3d(157.28769101047030, 616.28093816174612, -124.29713709028009),
                        new Point3d(157.30893663767583, 616.37549870948078, -124.34104213135871), new Point3d(157.33123573064620, 616.47474804793831, -124.38712420666809),
                        new Point3d(157.35027482584692, 616.57505648916413, -124.43313941232608), new Point3d(157.36655151508529, 616.67571376462672, -124.47871153477821),
                        new Point3d(157.41300913428583, 616.96301404447581, -124.60878542634391), new Point3d(157.43835730609305, 617.26101768291107, -124.73863543415258),
                        new Point3d(157.45040347712839, 617.55604258863889, -124.86170617655405), new Point3d(157.45852821066529, 617.75502687491212, -124.94471321549355),
                        new Point3d(157.46071431940632, 617.95687149940625, -125.02644085796187), new Point3d(157.45871014610881, 618.15668276952067, -125.10488937974471),
                        new Point3d(157.45613508005547, 618.41341067792405, -125.20568411921614), new Point3d(157.44657009827750, 618.67308816996035, -125.30357653360417),
                        new Point3d(157.43242606088546, 618.93233178170510, -125.39727069884378), new Point3d(157.39718020416473, 619.57834698528882, -125.63074937484481),
                        new Point3d(157.33330336596646, 620.24168853373806, -125.84519911380431), new Point3d(157.26087660347036, 620.90793606983664, -126.03589694938567),
                        new Point3d(157.15277530188413, 621.90235061148837, -126.32052494497132), new Point3d(157.02443116454134, 622.91055488430447, -126.55548172320992),
                        new Point3d(156.90384831348806, 623.93972833587736, -126.74509265718446), new Point3d(156.87222178626243, 624.20966043568876, -126.79482390265791),
                        new Point3d(156.84112223522965, 624.48025950843680, -126.84127717444706), new Point3d(156.81086445133346, 624.75146181428931, -126.88453247253415),
                        new Point3d(156.73625152970720, 625.42022184182031, -126.99119607278878), new Point3d(156.66676005540111, 626.09267595570464, -127.07841941879629),
                        new Point3d(156.60705912706354, 626.76790177026032, -127.14738931257978), new Point3d(156.52060933377652, 627.74566097293678, -127.24726101016704),
                        new Point3d(156.45537134128190, 628.72471063668070, -127.30855257640196), new Point3d(156.42313286732576, 629.70875047723041, -127.33554298220112),
                        new Point3d(156.40491319189996, 630.26488370255788, -127.35079669532476), new Point3d(156.39724767198450, 630.82181779202324, -127.35501397563182),
                        new Point3d(156.40268853705695, 631.37834569613733, -127.34885423341362), new Point3d(156.41644319127437, 632.78526330602585, -127.33328223830770),
                        new Point3d(156.51363369477173, 634.18886319313901, -127.25097500248519), new Point3d(156.73641751451748, 635.55652038945209, -127.11658541050411),
                        new Point3d(156.78871740826605, 635.87758646208374, -127.08503661405406), new Point3d(156.84813174290949, 636.19710550464436, -127.05059091365746),
                        new Point3d(156.91549187858689, 636.51478778921728, -127.01342140971670), new Point3d(157.03661188507854, 637.08601110969937, -126.94658706454616),
                        new Point3d(157.18331811391045, 637.65073461845850, -126.87107132412370), new Point3d(157.36207689396031, 638.21448831670648, -126.78672689320807),
                        new Point3d(157.53700827786326, 638.76617151004336, -126.70418835708435), new Point3d(157.73877556191897, 639.30698566976650, -126.61468588024024),
                        new Point3d(157.96576636094085, 639.83726268394321, -126.51947872270563), new Point3d(158.22023426890888, 640.43172943335571, -126.41274679213483),
                        new Point3d(158.50537100760073, 641.01054528789791, -126.29928355799116), new Point3d(158.81797643012052, 641.57427371793040, -126.18077339707629),
                        new Point3d(159.35120051947877, 642.53584878059723, -125.97862567959859), new Point3d(159.96134400028765, 643.44820195987677, -125.76306156204981),
                        new Point3d(160.63004570259488, 644.31682257052387, -125.54055968508729), new Point3d(160.94185549744583, 644.72185281566101, -125.43680899270794),
                        new Point3d(161.26607846214335, 645.11696572749088, -125.33161817757099), new Point3d(161.60070994981143, 645.50271567845505, -125.22550058154921),
                        new Point3d(162.25551088407965, 646.25754431112648, -125.01785155992970), new Point3d(162.95096144012723, 646.97739939046812, -124.80647479772831),
                        new Point3d(163.67257016579617, 647.66674508168239, -124.59412528907036), new Point3d(164.26880076080951, 648.23631828253519, -124.41867109984814),
                        new Point3d(164.88460857077942, 648.78670810040626, -124.24201746627955), new Point3d(165.51264792242270, 649.32054798454658, -124.06507654710798),
                        new Point3d(167.07687016120107, 650.65015291540681, -123.62437976593675), new Point3d(168.76174683099899, 651.91030002386037, -123.17052798241566),
                        new Point3d(170.38050862849215, 653.08452801057842, -122.72963392543096), new Point3d(171.33479625852814, 653.77675539344750, -122.46971937507003),
                        new Point3d(172.28700232536423, 654.45428301988113, -122.20861002006372), new Point3d(173.21512373296562, 655.12289735904085, -121.94681254680175),
                        new Point3d(174.29595543551869, 655.90152348341076, -121.64193970750253), new Point3d(175.39416790258224, 656.70410719508413, -121.32190573644795),
                        new Point3d(176.44331331553869, 657.52202195661994, -120.99299460051076), new Point3d(176.97885534103838, 657.93953102892738, -120.82510010869495),
                        new Point3d(177.50938290885475, 658.36705107815260, -120.65248352666772), new Point3d(178.02622101338576, 658.80451616685775, -120.47552032125482),
                        new Point3d(179.11656783065203, 659.72741380724665, -120.10219013119406), new Point3d(180.17160783867541, 660.71620446393786, -119.70079279902014),
                        new Point3d(181.10675943763138, 661.78187954178941, -119.26992094153009), new Point3d(181.78674807349324, 662.55677736933876, -118.95661562618547),
                        new Point3d(182.40646855692270, 663.37536690411298, -118.62647499830176), new Point3d(182.93697891900300, 664.24572836277798, -118.27714014015865),
                        new Point3d(183.37460343324543, 664.96370022117867, -117.98896952459643), new Point3d(183.74087878970258, 665.69946216626681, -117.69480449183614),
                        new Point3d(184.04228033866787, 666.47953542982930, -117.38399235591875), new Point3d(184.42697622908787, 667.47518717664093, -116.98728519966636),
                        new Point3d(184.69025013028508, 668.50165524674469, -116.57973813913601), new Point3d(184.85837841045895, 669.54568143738345, -116.16529532987494),
                        new Point3d(184.91000416982612, 669.86626191816390, -116.03803580684740), new Point3d(184.95262907665634, 670.18841013230917, -115.91016665307160),
                        new Point3d(184.98693259658810, 670.51057941113345, -115.78228028529118), new Point3d(185.00490566627255, 670.67937756683318, -115.71527519064264),
                        new Point3d(185.02061631353612, 670.84838765310838, -115.64818394451423), new Point3d(185.03416452153556, 671.01754003804535, -115.58103252554277),
                        new Point3d(185.06197716838585, 671.36478710912195, -115.44317970386857), new Point3d(185.08068603599466, 671.71275228479010, -115.30502580798108),
                        new Point3d(185.09113663439547, 672.06080490123861, -115.16681263667195), new Point3d(185.11385643145701, 672.81747782169441, -114.86633460824312),
                        new Point3d(185.09760704586711, 673.57690127944818, -114.56463960471267), new Point3d(185.05041300854799, 674.33239988954097, -114.26439792781531),
                        new Point3d(184.94157525126803, 676.07471266932248, -113.57198777153845), new Point3d(184.66460355111181, 677.84132516911734, -112.86923729680530),
                        new Point3d(184.29834298946605, 679.54705014108436, -112.19209748272529), new Point3d(184.06706466190056, 680.62414459272429, -111.76451182342113),
                        new Point3d(183.79495750954669, 681.70131166596536, -111.33751750557293), new Point3d(183.49539106932713, 682.76443181652019, -110.91748349182571),
                        new Point3d(183.11018461242872, 684.13147662351651, -110.37737020645972), new Point3d(182.67363471620411, 685.49669004507996, -109.84031656125610),
                        new Point3d(182.20443813538338, 686.84500544317154, -109.31414518017736), new Point3d(181.63462874765455, 688.48244868734014, -108.67514352341240),
                        new Point3d(181.00765550601071, 690.12091754273524, -108.04208212919164), new Point3d(180.34917183846127, 691.73501165802941, -107.42861750084775),
                        new Point3d(179.54802419126688, 693.69880782571670, -106.68224250651609), new Point3d(178.68515707957565, 695.66356561959651, -105.95073894165138),
                        new Point3d(177.79003790927317, 697.60557538303487, -105.25107359684031), new Point3d(177.64844572702714, 697.91276732961705, -105.14039878805454),
                        new Point3d(177.50561510750919, 698.22033610598737, -105.03017788229587), new Point3d(177.36168077614536, 698.52805631295143, -104.92053487974866),
                        new Point3d(176.43058370363141, 700.51866815060555, -104.21126505005586), new Point3d(175.45361888317157, 702.51491899498490, -103.52640405249666),
                        new Point3d(174.45502483537496, 704.49604496970426, -102.88470460698483), new Point3d(173.46105435855151, 706.46799817139220, -102.24597628165571),
                        new Point3d(172.43215325497928, 708.45149428445688, -101.64132333889017), new Point3d(171.39105257865623, 710.42519309280749, -101.09308187743311)
                      };
      double[] arrTrimmingLoopKnots = { -236.98493081629493,-236.98493081629493,-236.98493081629493,-236.98493081629493,-229.09596951511273,-229.09596951511273,-229.09596951511273,-221.71491282481190,
                                  -221.71491282481190,-221.71491282481190,-217.00199405524310,-217.00199405524310,-217.00199405524310,-211.27354135049330,-211.27354135049330,-211.27354135049330,
                                  -208.80218959584477,-208.80218959584477,-208.80218959584477,-204.50793640996184,-204.50793640996184,-204.50793640996184,-202.38818207194362,-202.38818207194362,
                                  -202.38818207194362,-198.39787328876372,-198.39787328876372,-198.39787328876372,-192.97361505983841,-192.97361505983841,-192.97361505983841,-189.86215601273199,
                                  -189.86215601273199,-189.86215601273199,-187.19650308828986,-187.19650308828986,-187.19650308828986,-184.50260944286396,-184.50260944286396,-184.50260944286396,
                                  -180.81991913434913,-180.81991913434913,-180.81991913434913,-177.32537124421535,-177.32537124421535,-177.32537124421535,-172.68145647412737,-172.68145647412737,
                                  -172.68145647412737,-167.06444218498149,-167.06444218498149,-167.06444218498149,-162.03510848132646,-162.03510848132646,-162.03510848132646,-157.64437008109101,
                                  -157.64437008109101,-157.64437008109101,-153.81083091612530,-153.81083091612530,-153.81083091612530,-150.36866978234482,-150.36866978234482,-150.36866978234482,
                                  -147.20782825705709,-147.20782825705709,-147.20782825705709,-144.52890648802301,-144.52890648802301,-144.52890648802301,-143.02322960500248,-143.02322960500248,
                                  -143.02322960500248,-139.90481866053170,-139.90481866053170,-139.90481866053170,-138.30126013530980,-138.30126013530980,-138.30126013530980,-135.04516021136718,
                                  -135.04516021136718,-135.04516021136718,-131.82379382772834,-131.82379382772834,-131.82379382772834,-130.21508817302043,-130.21508817302043,-130.21508817302043,
                                  -123.25734351169213,-123.25734351169213,-123.25734351169213,-122.16097277034498,-122.16097277034498,-122.16097277034498,-118.77290803125899,-118.77290803125899,
                                  -118.77290803125899,-116.77691135670266,-116.77691135670266,-116.77691135670266,-115.79696591907884,-115.79696591907884,-115.79696591907884,-114.88176484368253,
                                  -114.88176484368253,-114.88176484368253,-114.23527805569647,-114.23527805569647,-114.23527805569647,-113.72379856074551,-113.72379856074551,-113.72379856074551,
                                  -113.48931818437572,-113.48931818437572,-113.48931818437572,-112.65733142611103,-112.65733142611103,-112.65733142611103,-112.19897188675517,-112.19897188675517,
                                  -112.19897188675517,-111.65984560130205,-111.65984560130205,-111.65984560130205,-111.36492653605340,-111.36492653605340,-111.36492653605340,-111.05081911519031,
                                  -111.05081911519031,-111.05081911519031,-110.72113665805446,-110.72113665805446,-110.72113665805446,-109.78014295679245,-109.78014295679245,-109.78014295679245,
                                  -109.14547464100642,-109.14547464100642,-109.14547464100642,-108.33001979129594,-108.33001979129594,-108.33001979129594,-106.29796882886819,-106.29796882886819,
                                  -106.29796882886819,-103.26501048091168,-103.26501048091168,-103.26501048091168,-102.46952473042604,-102.46952473042604,-102.46952473042604,-100.50792978482644,
                                  -100.50792978482644,-100.50792978482644,-97.667446692150691,-97.667446692150691,-97.667446692150691,-96.062138696759476,-96.062138696759476,-96.062138696759476,
                                  -92.003876619845599,-92.003876619845599,-92.003876619845599,-91.051174239265677,-91.051174239265677,-91.051174239265677,-89.338123673674502,-89.338123673674502,
                                  -89.338123673674502,-87.661751156190249,-87.661751156190249,-87.661751156190249,-85.782454685419310,-85.782454685419310,-85.782454685419310,-82.576860421623110,
                                  -82.576860421623110,-82.576860421623110,-81.082119663044395,-81.082119663044395,-81.082119663044395,-78.157237572519165,-78.157237572519165,-78.157237572519165,
                                  -75.740548129689799,-75.740548129689799,-75.740548129689799,-69.721435854633455,-69.721435854633455,-69.721435854633455,-66.173066726588019,-66.173066726588019,
                                  -66.173066726588019,-62.040859652662540,-62.040859652662540,-62.040859652662540,-59.931551906734164,-59.931551906734164,-59.931551906734164,-55.481653636375427,
                                  -55.481653636375427,-55.481653636375427,-52.245942701417533,-52.245942701417533,-52.245942701417533,-49.576765005081540,-49.576765005081540,-49.576765005081540,
                                  -46.169942133380090,-46.169942133380090,-46.169942133380090,-45.123837233007890,-45.123837233007890,-45.123837233007890,-44.575738486610717,-44.575738486610717,
                                  -44.575738486610717,-43.450565557963543,-43.450565557963543,-43.450565557963543,-41.004418493692540,-41.004418493692540,-41.004418493692540,-35.363172786065007,
                                  -35.363172786065007,-35.363172786065007,-31.800960499862441,-31.800960499862441,-31.800960499862441,-27.220383408241972,-27.220383408241972,-27.220383408241972,
                                  -21.657564038468543,-21.657564038468543,-21.657564038468543,-14.889530343043702,-14.889530343043702,-14.889530343043702,-13.818945897705657,-13.818945897705657,
                                  -13.818945897705657,-6.8934401237315219,-6.8934401237315219,-6.8934401237315219,-0.0000000000000000,-0.0000000000000000,-0.0000000000000000,-0.0000000000000000 
                                };
      Point2d[] arrTrimmingLoopProj = { new Point2d(-492.03378698802373, 86.644833915424897), new Point2d(-491.84750010104676, 83.697499883208195), new Point2d(-491.63392971904386, 80.274722855656535),
                                  new Point2d(-491.34210884050168, 76.950615469536046), new Point2d(-491.06907588144657, 73.840519713593338), new Point2d(-490.71329854711985, 70.647288097718231),
                                  new Point2d(-490.19735635775714, 67.546176144284800), new Point2d(-489.86791929606170, 65.566068234928665), new Point2d(-489.46726873979469, 63.583783801470673),
                                  new Point2d(-488.97032644037250, 61.640534054658531), new Point2d(-488.36630362597339, 59.278555244412914), new Point2d(-487.61304457317726, 56.947587525365705),
                                  new Point2d(-486.66012971090078, 54.718345051364821), new Point2d(-486.24902606070128, 53.756611999943949), new Point2d(-485.80128658860576, 52.814754506959069),
                                  new Point2d(-485.31304609492980, 51.896717501956331), new Point2d(-484.46467301671402, 50.301524374925307), new Point2d(-483.50439176207567, 48.797829681604789),
                                  new Point2d(-482.41849073737376, 47.393779313871704), new Point2d(-481.88246193140174, 46.700703771054968), new Point2d(-481.32281206713674, 46.040917882920205),
                                  new Point2d(-480.73762189591389, 45.410214039055276), new Point2d(-479.63603694421414, 44.222952363198807), new Point2d(-478.46441106827677, 43.162736526105810),
                                  new Point2d(-477.21705784119519, 42.204306562248902), new Point2d(-475.52145823374275, 40.901457108539084), new Point2d(-473.71524023180041, 39.812573372638113),
                                  new Point2d(-471.88909122177392, 38.900782402517095), new Point2d(-470.84157695921323, 38.377761531585016), new Point2d(-469.77607782118628, 37.907425835812589),
                                  new Point2d(-468.70426687921571, 37.485170764870929), new Point2d(-467.78602372426565, 37.123415894350330), new Point2d(-466.85879482525672, 36.795243978688724),
                                  new Point2d(-465.92860035738119, 36.498406852435977), new Point2d(-464.98855113227637, 36.198424945261728), new Point2d(-464.04132929686267, 35.929111924806257),
                                  new Point2d(-463.09189211750049, 35.688679689460784), new Point2d(-461.79396296394333, 35.359996534331486), new Point2d(-460.48454942496943, 35.083425149095454),
                                  new Point2d(-459.17283330005716, 34.855384281277956), new Point2d(-457.92813053312392, 34.638993635653108), new Point2d(-456.67446704463890, 34.465099931237702),
                                  new Point2d(-455.41817101701992, 34.331900748480983), new Point2d(-453.74867560253892, 34.154891970345766), new Point2d(-452.06284620241325, 34.048497405872936),
                                  new Point2d(-450.37148062404822, 34.011203581958625), new Point2d(-448.32570134251631, 33.966095100818350), new Point2d(-446.25259947585101, 34.021650779201011),
                                  new Point2d(-444.16978446761630, 34.181015815909632), new Point2d(-442.30488415222362, 34.323707272445830), new Point2d(-440.41498931420023, 34.550917812278648),
                                  new Point2d(-438.51525577399434, 34.869028687011621), new Point2d(-436.85673925934447, 35.146747708162970), new Point2d(-435.18252877278496, 35.495019749868888),
                                  new Point2d(-433.50007118762773, 35.920350860235530), new Point2d(-432.03112303855374, 36.291706044953763), new Point2d(-430.55379004360520, 36.722314374687663),
                                  new Point2d(-429.07229204853104, 37.218213976904110), new Point2d(-427.74204482814986, 37.663485634125315), new Point2d(-426.40970949684942, 38.160963348732515),
                                  new Point2d(-425.07828614672098, 38.715095463417910), new Point2d(-423.85567684713783, 39.223939683813086), new Point2d(-422.63675931866771, 39.779392414222734),
                                  new Point2d(-421.42290598164209, 40.385568402089874), new Point2d(-420.39412358209529, 40.899323374229830), new Point2d(-419.37456820922500, 41.446679452977655),
                                  new Point2d(-418.36768905989999, 42.027734045291112), new Point2d(-417.80177680495547, 42.354313371918757), new Point2d(-417.23936383020271, 42.692042352525966),
                                  new Point2d(-416.68038165920802, 43.040814733869361), new Point2d(-415.52267236616444, 43.763158035659657), new Point2d(-414.38376674787287, 44.530305270542314),
                                  new Point2d(-413.26298664691262, 45.340327706630518), new Point2d(-412.68665577910940, 45.756859846281110), new Point2d(-412.11671468214206, 46.183543772338339),
                                  new Point2d(-411.55075725599914, 46.621797080859956), new Point2d(-410.40155446530980, 47.511690740554961), new Point2d(-409.27815732139965, 48.441758594849972),
                                  new Point2d(-408.18361691430368, 49.409212645977391), new Point2d(-407.10075221257057, 50.366346650097100), new Point2d(-406.04646458063962, 51.359962571121692),
                                  new Point2d(-405.03105798924582, 52.381262679694274), new Point2d(-404.52397803283372, 52.891285776774779), new Point2d(-404.02559044395497, 53.409298075234645),
                                  new Point2d(-403.53716102303787, 53.934730353233448), new Point2d(-401.42467563089906, 56.207255234610543), new Point2d(-399.65725976869777, 58.445017881219464),
                                  new Point2d(-398.28759684221194, 60.530884013574521), new Point2d(-398.07177139428291, 60.859565606417632), new Point2d(-397.86127347260913, 61.191495894407254),
                                  new Point2d(-397.65773847579294, 61.525617634466414), new Point2d(-397.02876351333134, 62.558138882402616), new Point2d(-396.43682715777874, 63.659285459200007),
                                  new Point2d(-395.98263150298823, 64.749409820873325), new Point2d(-395.71505308097602, 65.391630312014172), new Point2d(-395.48062358845084, 66.065803187960142),
                                  new Point2d(-395.32523833916713, 66.733857366763672), new Point2d(-395.24895110479912, 67.061842204087881), new Point2d(-395.19036994032319, 67.394146848052628),
                                  new Point2d(-395.15867309736507, 67.725499720359181), new Point2d(-395.12907044435178, 68.034960322375909), new Point2d(-395.12240822007675, 68.350659971535023),
                                  new Point2d(-395.15127328656018, 68.660207668431255), new Point2d(-395.17166321465089, 68.878868355874474), new Point2d(-395.21011770883644, 69.098107666417818),
                                  new Point2d(-395.27272412762767, 69.311196991567257), new Point2d(-395.32225630687577, 69.479786392669183), new Point2d(-395.38702571590647, 69.645023595042701),
                                  new Point2d(-395.47025289834187, 69.803759510809826), new Point2d(-395.50840719632311, 69.876529695421127), new Point2d(-395.55034678446583, 69.947753477314251),
                                  new Point2d(-395.59636835879729, 70.017170503998202), new Point2d(-395.75966279320193, 70.263477030340837), new Point2d(-395.96844769592781, 70.478141959021116),
                                  new Point2d(-396.22624620179369, 70.656863197590425), new Point2d(-396.36827298674126, 70.755324601897627), new Point2d(-396.52078696929198, 70.839769885386119),
                                  new Point2d(-396.68276580037576, 70.912848973887819), new Point2d(-396.87328664269739, 70.998805202885677), new Point2d(-397.07467741312666, 71.068017342392920),
                                  new Point2d(-397.28016475385812, 71.124266503875518), new Point2d(-397.39257280385334, 71.155036567537920), new Point2d(-397.50672628168223, 71.182069044755778),
                                  new Point2d(-397.62153484979308, 71.205850898393592), new Point2d(-397.74381322196911, 71.231180073715706), new Point2d(-397.86747425683512, 71.252954573970769),
                                  new Point2d(-397.99145384765853, 71.271685593592522), new Point2d(-398.12158097431171, 71.291345392003493), new Point2d(-398.25273053543344, 71.307754178498215),
                                  new Point2d(-398.38396705885708, 71.321430006366185), new Point2d(-398.75854792976548, 71.360464136344191), new Point2d(-399.14400656784807, 71.378435189710260),
                                  new Point2d(-399.52260990413828, 71.383508525268184), new Point2d(-399.77796500531701, 71.386930317921681), new Point2d(-400.03563181697785, 71.384495259296969),
                                  new Point2d(-400.28940544373870, 71.378061885239305), new Point2d(-400.61546699356484, 71.369795951766903), new Point2d(-400.94312762633967, 71.354715252191795),
                                  new Point2d(-401.26817443611947, 71.335300016123455), new Point2d(-402.07816616390375, 71.286918736457963), new Point2d(-402.89696012052514, 71.211027361424883),
                                  new Point2d(-403.70729642712081, 71.127941721085534), new Point2d(-404.91677212983905, 71.003931402879019), new Point2d(-406.11684409516977, 70.862517048026547),
                                  new Point2d(-407.31831571614458, 70.731413656603550), new Point2d(-407.63343826134610, 70.697027797072181), new Point2d(-407.94774159489100, 70.663353279973478),
                                  new Point2d(-408.26120697063840, 70.630699873329050), new Point2d(-409.03418385465079, 70.550179565591009), new Point2d(-409.80209663284199, 70.475870913384654),
                                  new Point2d(-410.56468965728794, 70.412368501496658), new Point2d(-411.66896074923301, 70.320413978613828), new Point2d(-412.75705284569682, 70.251817179183703),
                                  new Point2d(-413.83546850806857, 70.218105225549621), new Point2d(-414.44493852731438, 70.199052808427822), new Point2d(-415.05042061581264, 70.191161254148994),
                                  new Point2d(-415.65101102782802, 70.196941457952306), new Point2d(-417.16931985070670, 70.211553969669154), new Point2d(-418.65539760310855, 70.313239639360702),
                                  new Point2d(-420.08217608256047, 70.543260690431495), new Point2d(-420.41712124582790, 70.597259569455417), new Point2d(-420.74926056868441, 70.658525201767574),
                                  new Point2d(-421.07838559480405, 70.727877839148206), new Point2d(-421.67018406580326, 70.852580555454850), new Point2d(-422.25168267816696, 71.003319127089696),
                                  new Point2d(-422.82900263361057, 71.186557875622867), new Point2d(-423.39396161762448, 71.365873307606748), new Point2d(-423.94472344012661, 71.572375238927222),
                                  new Point2d(-424.48219526311254, 71.804382710925722), new Point2d(-425.08472774890640, 72.064474557321304), new Point2d(-425.66811967957864, 72.355566566550550),
                                  new Point2d(-426.23367687731633, 72.674368535179909), new Point2d(-427.19837134522692, 73.218162331326383), new Point2d(-428.10590134405129, 73.839510681455181),
                                  new Point2d(-428.96469196473038, 74.519715698347014), new Point2d(-429.36513857093746, 74.836889388860769), new Point2d(-429.75456658200000, 75.166537787920973),
                                  new Point2d(-430.13375593720684, 75.506630446750236), new Point2d(-430.87574691296436, 76.172117703466682), new Point2d(-431.57942574170829, 76.878400266672060),
                                  new Point2d(-432.25048543093141, 77.610854174197755), new Point2d(-432.80494978753137, 78.216045621542079), new Point2d(-433.33873337738277, 78.840850779859835),
                                  new Point2d(-433.85490559005183, 79.477884172498548), new Point2d(-435.14050664671140, 81.064507362594242), new Point2d(-436.34896947017540, 82.772335512758772),
                                  new Point2d(-437.46922775403880, 84.413422283875079), new Point2d(-438.12963907854112, 85.380870865820384), new Point2d(-438.77384784383548, 86.346308717136964),
                                  new Point2d(-439.40781876767073, 87.287739262345866), new Point2d(-440.14610135372686, 88.384069947835897), new Point2d(-440.90463082557403, 89.498610248727118),
                                  new Point2d(-441.67529256547510, 90.564663444118196), new Point2d(-442.06868107393620, 91.108836139818550), new Point2d(-442.47090454011294, 91.648269189913435),
                                  new Point2d(-442.88187444245591, 92.174280232475951), new Point2d(-443.74887654830587, 93.283978726214869), new Point2d(-444.67512124854403, 94.360012626682249),
                                  new Point2d(-445.67041508177238, 95.318014966726835), new Point2d(-446.39413570278640, 96.014619351605873), new Point2d(-447.15720657310624, 96.652003067308968),
                                  new Point2d(-447.96702700161489, 97.201403658795556), new Point2d(-448.63505782057945, 97.654610955314737), new Point2d(-449.31869259088819, 98.036921759604979),
                                  new Point2d(-450.04261775056290, 98.355402962963183), new Point2d(-450.96660448803846, 98.761898584928375), new Point2d(-451.91779224858959, 99.047778645585467),
                                  new Point2d(-452.88440870865332, 99.239054690240877), new Point2d(-453.18121962419940, 99.297788242049492), new Point2d(-453.47940584972486, 99.347569764295613),
                                  new Point2d(-453.77755549119206, 99.389044165203572), new Point2d(-453.93376873966150, 99.410774363711525), new Point2d(-454.09016278250402, 99.430250621518809),
                                  new Point2d(-454.24667609937177, 99.447571294251446), new Point2d(-454.56797686546543, 99.483128303262745), new Point2d(-454.88988980266288, 99.509613086743258),
                                  new Point2d(-455.21185726844374, 99.527856142251679), new Point2d(-455.91182058177861, 99.567516887315023), new Point2d(-456.61420389908324, 99.568339460879344),
                                  new Point2d(-457.31310097128505, 99.538184701003473), new Point2d(-458.92488064153429, 99.468642516807350), new Point2d(-460.55998406133455, 99.231819210427460),
                                  new Point2d(-462.14262915864981, 98.904402654391703), new Point2d(-463.14200378307572, 98.697652702507895), new Point2d(-464.14302132715864, 98.450105736375846),
                                  new Point2d(-465.13328160944917, 98.174750084493482), new Point2d(-466.40663737810189, 97.820675793091510), new Point2d(-467.68213272492932, 97.415167208628034),
                                  new Point2d(-468.94721767418980, 96.976449234480626), new Point2d(-470.48358283504012, 96.443654174897503), new Point2d(-472.02896622125303, 95.853450948056249),
                                  new Point2d(-473.56223929436237, 95.230680399004200), new Point2d(-475.42770411234267, 94.472983306877666), new Point2d(-477.31036718149926, 93.652825096390231),
                                  new Point2d(-479.19338593980115, 92.798667464049515), new Point2d(-479.49124654210289, 92.663554680011671), new Point2d(-479.79003102849640, 92.527179791163917),
                                  new Point2d(-480.08955190458670, 92.389670301609414), new Point2d(-482.02712307500877, 91.500134900611442), new Point2d(-483.99485182350321, 90.563425904915647),
                                  new Point2d(-485.98068619452027, 89.602683572216563), new Point2d(-487.95732599225141, 88.646389553997182), new Point2d(-489.97842357242297, 87.653295423234880),
                                  new Point2d(-492.03378698802373, 86.644833915424897) 
                                };
//hole1
      Point3d[] arrHole1LoopCPS = { new Point3d(172.17480240767057, 694.83490733890983, -106.65258288604315), new Point3d(172.20351992870573, 693.56637790425407, -107.15271243447712),
                              new Point3d(172.23202127112449, 692.29947819936535, -107.65916764875529), new Point3d(172.25571726730959, 691.02966005407063, -108.17148955312955),
                              new Point3d(172.27820412628202, 689.82463700583969, -108.65766917276423), new Point3d(172.29629623409809, 688.61972212001456, -109.14800870527216),
                              new Point3d(172.30667914620054, 687.38290416709333, -109.65388392911069), new Point3d(172.31407721393782, 686.50164244551365, -110.01433185890625),
                              new Point3d(172.31817325985952, 685.55238224796483, -110.40394803115095), new Point3d(172.31522846855520, 684.67995081821266, -110.76270322175873),
                              new Point3d(172.31259251671943, 683.89901696205811, -111.08383341590634), new Point3d(172.30418809050835, 683.16220534309628, -111.38736047543395),
                              new Point3d(172.27994235446081, 682.40296786003194, -111.70073970669969), new Point3d(172.25625035948212, 681.66107037335155, -112.00696176401375),
                              new Point3d(172.21745637679783, 680.91471386794240, -112.31561676240085), new Point3d(172.15424648843239, 680.17508865605123, -112.62221412909548),
                              new Point3d(172.10511144787307, 679.60015464296043, -112.86054193566947), new Point3d(172.04073051705728, 679.02347051695892, -113.10003722936021),
                              new Point3d(171.95600124338873, 678.45232667799655, -113.33782113746034), new Point3d(171.83595372611981, 677.64310930952149, -113.67472202871055),
                              new Point3d(171.67289885581368, 676.83037455209637, -114.01425640045986), new Point3d(171.44861142108084, 676.04091382350771, -114.34614271077038),
                              new Point3d(171.28534348700600, 675.46623329205750, -114.58773623693764), new Point3d(171.08795529958124, 674.89789897996297, -114.82779805738474),
                              new Point3d(170.84753968483093, 674.34763665812432, -115.06204590336415), new Point3d(170.63897618245588, 673.87027732692241, -115.26525879048056),
                              new Point3d(170.39765139075746, 673.40562522292873, -115.46447757412618), new Point3d(170.11748430687723, 672.96161924812191, -115.65693409052740),
                              new Point3d(169.87387309192357, 672.57554664647535, -115.82427910885329), new Point3d(169.60377320695011, 672.20964643909474, -115.98453877194069),
                              new Point3d(169.30425484163516, 671.86628271369466, -116.13707445356148), new Point3d(169.14676824860049, 671.68574225425380, -116.21727763171857),
                              new Point3d(168.98215569000200, 671.51283433133426, -116.29473646705809), new Point3d(168.80857993477741, 671.34598325748755, -116.37020046644605),
                              new Point3d(168.39593299599508, 670.94932310891829, -116.54960333463708), new Point3d(167.94316949360569, 670.59738072566427, -116.71301377393949),
                              new Point3d(167.45495932518901, 670.28478196502090, -116.86262712594797), new Point3d(166.89790087061021, 669.92809995843356, -117.03333922505169),
                              new Point3d(166.29764318686017, 669.62381444202254, -117.18546005303297), new Point3d(165.67135062234567, 669.37248013876422, -117.31823861654159),
                              new Point3d(165.33337154307554, 669.23684746289155, -117.38989263126909), new Point3d(164.98770775828672, 669.11659354317896, -117.45593433901118),
                              new Point3d(164.63683317925390, 669.01168392145439, -117.51629151748099), new Point3d(163.90116271614582, 668.79172238929834, -117.64284099153861),
                              new Point3d(163.13821110333751, 668.63784066763014, -117.74519500389317), new Point3d(162.36797753413529, 668.55196863133665, -117.82179693855841),
                              new Point3d(161.61060150753639, 668.46753006069457, -117.89712015378342), new Point3d(160.84066317361095, 668.44823484703466, -117.94809531793635),
                              new Point3d(160.07689607795658, 668.49860992033473, -117.97196064198008), new Point3d(159.69776868610913, 668.52361567177786, -117.98380718277998),
                              new Point3d(159.31935670837751, 668.56583964645097, -117.98899532395720), new Point3d(158.94402731220916, 668.62603770776445, -117.98707674153604),
                              new Point3d(158.59397224198696, 668.68218208866153, -117.98528735463745), new Point3d(158.24616571871726, 668.75403099264474, -117.97731372671801),
                              new Point3d(157.90309070386465, 668.84218930613235, -117.96274648450473), new Point3d(157.32785106184144, 668.99000583166332, -117.93832134471533),
                              new Point3d(156.76678217143126, 669.18345606215541, -117.89540134479016), new Point3d(156.22798887990282, 669.42542286183061, -117.83214140217311),
                              new Point3d(155.78667127827850, 669.62361428214774, -117.78032612051757), new Point3d(155.36157671858263, 669.85381775173119, -117.71500021764183),
                              new Point3d(154.95194748435128, 670.12046610452478, -117.63426564275169), new Point3d(154.71390492804036, 670.27542002438895, -117.58734940000453),
                              new Point3d(154.48328084242678, 670.44080670840890, -117.53583324072896), new Point3d(154.26009813455332, 670.61562798098237, -117.48006464181093),
                              new Point3d(153.27179240663366, 671.38977785438794, -117.23310811783146), new Point3d(152.44271902695363, 672.33852741363989, -116.90579877330043),
                              new Point3d(151.76474750528558, 673.35860969313160, -116.53744176026346), new Point3d(151.27190449179506, 674.10014589128343, -116.26966918495108),
                              new Point3d(150.85323865536122, 674.88794642963887, -115.97720844942917), new Point3d(150.50506498909022, 675.69407559418494, -115.67148725201945),
                              new Point3d(150.29256606321428, 676.18607598990116, -115.48489810884024), new Point3d(150.10493128367486, 676.68812436625194, -115.29216472819060),
                              new Point3d(149.94205232967110, 677.19500621752502, -115.09547855697546), new Point3d(149.59197802246257, 678.28444293454197, -114.67274269434188),
                              new Point3d(149.35026904599115, 679.41511541828129, -114.22434186274209), new Point3d(149.22677562957415, 680.53979393070949, -113.77023619911843),
                              new Point3d(149.17024611214762, 681.05461921785570, -113.56236783843906), new Point3d(149.13797842519264, 681.57283417829137, -113.35144144707262),
                              new Point3d(149.13235887613232, 682.08903651848277, -113.13968206105204), new Point3d(149.12963007748888, 682.33969937959989, -113.03685375239185),
                              new Point3d(149.13317947771745, 682.59051932354782, -112.93357014296059), new Point3d(149.14338274215677, 682.84073329061744, -112.83014035719708),
                              new Point3d(149.16275128502286, 683.31570676363674, -112.63380277807194), new Point3d(149.20621188306157, 683.79130300098996, -112.43577876261020),
                              new Point3d(149.27671807853926, 684.25938636371666, -112.23935904995375), new Point3d(149.33454306078249, 684.64328045918785, -112.07826730021988),
                              new Point3d(149.41107531554749, 685.02495882374023, -111.91705911893197), new Point3d(149.50913940951875, 685.40103941605469, -111.75702050834838),
                              new Point3d(149.73273546733040, 686.25854122385829, -111.39211628117393), new Point3d(150.09908084475819, 687.20775732817128, -110.98191816150882),
                              new Point3d(150.74431736275108, 688.15427061224671, -110.55854438545357), new Point3d(151.40272579467793, 689.12010610014590, -110.12652782061532),
                              new Point3d(152.24630784647724, 689.92806911976322, -109.75012635665381), new Point3d(153.23601625838492, 690.60980103442694, -109.41783555210608),
                              new Point3d(154.46956992432482, 691.45949867506897, -109.00367463847107), new Point3d(155.89236402606127, 692.08604027008937, -108.67101268682570),
                              new Point3d(157.34164192086706, 692.56412235907385, -108.39710353535091), new Point3d(157.91309779341651, 692.75263197361369, -108.28910009880067),
                              new Point3d(158.49545966446405, 692.92029873558170, -108.18896356434146), new Point3d(159.08061708565128, 693.07043302396573, -108.09576443122516),
                              new Point3d(160.14950188311775, 693.34467761631242, -107.92552112076189), new Point3d(161.25036084711635, 693.56614384133911, -107.77484675697501),
                              new Point3d(162.34206648205898, 693.75016465291719, -107.63973123321448), new Point3d(164.04236723169848, 694.03677187913343, -107.42929261406232),
                              new Point3d(165.81584940191797, 694.24810033175254, -107.24499749802465), new Point3d(167.52768638020925, 694.42023547442352, -107.07983374696613),
                              new Point3d(168.38748583495368, 694.50669329937273, -106.99687742290928), new Point3d(169.28710000256922, 694.58840854145365, -106.91358155592889),
                              new Point3d(170.11222095153238, 694.66028768416538, -106.83841984156335), new Point3d(170.86823658380391, 694.72614682347410, -106.76955305169446),
                              new Point3d(171.74970795168352, 694.79933744965456, -106.69070881163731), new Point3d(172.17480240767057, 694.83490733890983, -106.65258288604315)
                            };
      double[] arrHole1LoopKnots = {
                              0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 4.0915877957553031, 4.0915877957553031, 4.0915877957553031,
                              7.9743938621322474, 7.9743938621322474, 7.9743938621322474, 10.740984006246478, 10.740984006246478, 10.740984006246478, 13.217423799636272,
                              13.217423799636272, 13.217423799636272, 15.637304929867069, 15.637304929867069, 15.637304929867069, 17.518354881976165, 17.518354881976165,
                              17.518354881976165, 20.183494884153532, 20.183494884153532, 20.183494884153532, 22.123558547077895, 22.123558547077895, 22.123558547077895,
                              23.806587633147508, 23.806587633147508, 23.806587633147508, 25.270017079538260, 25.270017079538260, 25.270017079538260, 26.039487483853328,
                              26.039487483853328, 26.039487483853328, 27.868772952975270, 27.868772952975270, 27.868772952975270, 29.956027587013988, 29.956027587013988,
                              29.956027587013988, 31.082415539522355, 31.082415539522355, 31.082415539522355, 33.444086621381238, 33.444086621381238, 33.444086621381238,
                              35.766334224747098, 35.766334224747098, 35.766334224747098, 36.919077884558376, 36.919077884558376, 36.919077884558376, 37.994196878441947,
                              37.994196878441947, 37.994196878441947, 39.796866960492650, 39.796866960492650, 39.796866960492650, 41.273407313980925, 41.273407313980925,
                              41.273407313980925, 42.131450175350686, 42.131450175350686, 42.131450175350686, 45.931066564865276, 45.931066564865276, 45.931066564865276,
                              48.693150710979630, 48.693150710979630, 48.693150710979630, 50.378918392707185, 50.378918392707185, 50.378918392707185, 54.002124084964223,
                              54.002124084964223, 54.002124084964223, 55.660658404143668, 55.660658404143668, 55.660658404143668, 56.466026619933402, 56.466026619933402,
                              56.466026619933402, 57.994832317684953, 57.994832317684953, 57.994832317684953, 59.248667696645413, 59.248667696645413, 59.248667696645413,
                              62.107539242050144, 62.107539242050144, 62.107539242050144, 65.024772037778206, 65.024772037778206, 65.024772037778206, 68.660755289283685,
                              68.660755289283685, 68.660755289283685, 70.094437531323734, 70.094437531323734, 70.094437531323734, 72.713290334737039, 72.713290334737039,
                              72.713290334737039, 76.792079746750275, 76.792079746750275, 76.792079746750275, 78.840721619159709, 78.840721619159709,78.840721619159709,
                              80.717786166379994, 80.717786166379994, 80.717786166379994, 80.717786166379994 
                             };
      Point2d[] arrHole1LoopProjCPS = { 
                                  new Point2d(-476.52127854012758, 87.113215629119480),new Point2d(-475.30413484605936, 87.113629568173465),new Point2d(-474.09667519462585, 87.113470268918775),
                                  new Point2d(-472.89289257934576, 87.108181827165510),new Point2d(-471.75053546163764, 87.103163239090108),new Point2d(-470.61402074887587, 87.093521881927160),
                                  new Point2d(-469.45220325192355, 87.075305865456798),new Point2d(-468.62438110954008, 87.062326527635022),new Point2d(-467.73534311144908, 87.044402414477148),
                                  new Point2d(-466.92019164236098, 87.021192380836240),new Point2d(-466.19053042384451, 87.000416534787433),new Point2d(-465.50351587400439, 86.974876019939302),
                                  new Point2d(-464.79667791089042, 86.932962845528209),new Point2d(-464.10598320729378, 86.892006913493759),new Point2d(-463.41222535789399, 86.835835655287028),
                                  new Point2d(-462.72553272753339, 86.755403752245314),new Point2d(-462.19174488014016, 86.692881496306967),new Point2d(-461.65682358941012, 86.615071740651672),
                                  new Point2d(-461.12740292634629, 86.517042842109745),new Point2d(-460.37730047131652, 86.378151932498042),new Point2d(-459.62467448009943, 86.196173578028194),
                                  new Point2d(-458.89397397997584, 85.953492596171174),new Point2d(-458.36206740560743, 85.776835252017037),new Point2d(-457.83623462044591, 85.566198702292866),
                                  new Point2d(-457.32719622918938, 85.312931953246235),new Point2d(-456.88559916473048, 85.093219933828664),new Point2d(-456.45581369107327, 84.841024629380186),
                                  new Point2d(-456.04511725392700, 84.550431154508146),new Point2d(-455.68800800096028, 84.297753971697318),new Point2d(-455.34955138201161, 84.019030761118529),
                                  new Point2d(-455.03191083365567, 83.711372475537331),new Point2d(-454.86489560666740, 83.549605916868657),new Point2d(-454.70493267491776, 83.380879833469677),
                                  new Point2d(-454.55056328753574, 83.203317581434035),new Point2d(-454.18357623302182, 82.781193422152683),new Point2d(-453.85790226094997, 82.319920367891470),
                                  new Point2d(-453.56858318014002, 81.824036817824322),new Point2d(-453.23846379618942, 81.258222867354348),new Point2d(-452.95676296486562, 80.650329117329733),
                                  new Point2d(-452.72404198158955, 80.017534928766025),new Point2d(-452.59845399266709, 79.676047244996312),new Point2d(-452.48709163309957, 79.327204231297173),
                                  new Point2d(-452.38993090896281, 78.973479927621980),new Point2d(-452.18621630781206, 78.231834513996915),new Point2d(-452.04366218337805, 77.464318593149855),
                                  new Point2d(-451.96410923972678, 76.690987360791482),new Point2d(-451.88588427683760, 75.930565380462753),new Point2d(-451.86800633854773, 75.158976844166176),
                                  new Point2d(-451.91467615133308, 74.395081414918010),new Point2d(-451.93784264344828, 74.015890319382663),new Point2d(-451.97696059292798, 73.637789699870780),
                                  new Point2d(-452.03272605042827, 73.263168273062945),new Point2d(-452.08473631485339, 72.913773498215264),new Point2d(-452.15129199659970, 72.566973092804076),
                                  new Point2d(-452.23294735283332, 72.225269181807604),new Point2d(-452.36986026066859, 71.652328492658384),new Point2d(-452.54902250228793, 71.094580892069843),
                                  new Point2d(-452.77306315116215, 70.560222528860351),new Point2d(-452.95657152697555, 70.122537511308394),new Point2d(-453.16969054379632, 69.701812980848231),
                                  new Point2d(-453.41649879221615, 69.297399672767924),new Point2d(-453.55992328798663, 69.062388180382058),new Point2d(-453.71298925979437, 68.835043206947617),
                                  new Point2d(-453.87477011267424, 68.615367990201122),new Point2d(-454.59117392859878, 67.642594261173556),new Point2d(-455.46876717218657, 66.833338512183474),
                                  new Point2d(-456.41232509574820, 66.177262469441018),new Point2d(-457.09823285970094, 65.700336103583354),new Point2d(-457.82695252814602, 65.298863321038880),
                                  new Point2d(-458.57297455433115, 64.968519327691453),new Point2d(-459.02829008965580, 64.766902288391137),new Point2d(-459.49303350539765, 64.590456830509623),
                                  new Point2d(-459.96247094321978, 64.438950987833010),new Point2d(-460.97142871083622, 64.113320809044936),new Point2d(-462.01960044011850, 63.897332944016853),
                                  new Point2d(-463.06420385418443, 63.799694123653097),new Point2d(-463.54237450175845, 63.754999632583058),new Point2d(-464.02410841388064, 63.734700996156619),
                                  new Point2d(-464.50447649064301, 63.741054993541333),new Point2d(-464.73773858020644, 63.744140433183844),new Point2d(-464.97126593075421, 63.753519767469271),
                                  new Point2d(-465.20435780887919, 63.769550595861290),new Point2d(-465.64682894741014, 63.799981424027706),new Point2d(-466.09035014390827, 63.854561106600976),
                                  new Point2d(-466.52738333188887, 63.936052538413527),new Point2d(-466.88581192335465, 64.002886957227503),new Point2d(-467.24252411379661, 64.088405343202496),
                                  new Point2d(-467.59437444370747, 64.195355090808860),new Point2d(-468.39662880125906, 64.439211338088754),new Point2d(-469.28674958587834, 64.828148010886807),
                                  new Point2d(-470.17742171586292, 65.496274745870821),new Point2d(-471.08627610353500, 66.178040677652888),new Point2d(-471.84957825891445, 67.041542128356113),
                                  new Point2d(-472.49510167435113, 68.048444223919901),new Point2d(-473.29966973296456, 69.303427768852245),new Point2d(-473.89553117275244, 70.742744693890785),
                                  new Point2d(-474.35081450186323, 72.205183526831576),new Point2d(-474.53033447731218, 72.781828808128978),new Point2d(-474.69015988054730, 73.368918180125831),
                                  new Point2d(-474.83336236567851, 73.958406667969840),new Point2d(-475.09494489676155, 75.035202860111696),new Point2d(-475.30651006873381, 76.142816481159258),
                                  new Point2d(-475.48242308836120, 77.240412308560991),new Point2d(-475.75640265718471, 78.949886866902389),new Point2d(-475.95876837140486, 80.730948814349318),
                                  new Point2d(-476.12369238502657, 82.449383107697628),new Point2d(-476.20652829684786, 83.312496176974719),new Point2d(-476.28485846369625, 84.215379541758139),
                                  new Point2d(-476.35378372980443, 85.043429100758132),new Point2d(-476.41693638482030, 85.802128066874758),new Point2d(-476.48714952015928, 86.686646093668784),
                                  new Point2d(-476.52127854012758, 87.113215629119480) 
                                };
//hole 21
      Point3d[] arrHole21LoopCPS = {
                              new Point3d(133.23513938128082, 678.15600616905385, -115.64990487257738), new Point3d(133.79026357963571, 678.26705924344253, -115.57322872426515),
                              new Point3d(134.35281475217442, 678.37961188012775, -115.49549306334569), new Point3d(134.90813999554351, 678.45792168002527, -115.43212079138094),
                              new Point3d(135.33434736259227, 678.51802378059949, -115.38348311553150), new Point3d(135.76878690008061, 678.55983280077760, -115.34183578269190),
                              new Point3d(136.20210827955540, 678.56400378948922, -115.31561156514864), new Point3d(136.46565574259452, 678.56654059829987, -115.29966191130954), 
                              new Point3d(136.72911074805279, 678.55519681960777, -115.28938220123490), new Point3d(136.99151368682416, 678.52572787536394, -115.28655651659501),
                              new Point3d(137.51589359402331, 678.46683782512218, -115.28090973437463), new Point3d(138.05300774720121, 678.33368989397661, -115.30486460733864),
                              new Point3d(138.61336586705372, 678.06700375943365, -115.38181357562932), new Point3d(138.76627810431208, 677.99422963154291, -115.40281164539790),
                              new Point3d(138.91509832412544, 677.91428049079468, -115.42695436709256), new Point3d(139.05981209160549, 677.82796068257448, -115.45390793664318),
                              new Point3d(139.36374751462876, 677.64666730680563, -115.51051723309095), new Point3d(139.65130658209964, 677.43649861118149, -115.57974222058215),
                              new Point3d(139.91584785450422, 677.20507532899023, -115.65878295263856), new Point3d(140.18957762820997, 676.96561385634880, -115.74056906299735),
                              new Point3d(140.44097946453732, 676.70136898262842, -115.83355664010128), new Point3d(140.66218260722380, 676.41917808140295, -115.93531859661509),
                              new Point3d(140.85668517328546, 676.17104935347879, -116.02479726056627), new Point3d(141.02925881827181, 675.90722636282771, -116.12171626127449),
                              new Point3d(141.17359103987624, 675.63172238960340, -116.22473396990131), new Point3d(141.28412216619429, 675.42073856265836, -116.30362600965967),
                              new Point3d(141.37829905719801, 675.20249272188880, -116.38624895689570), new Point3d(141.45299343691164, 674.97883830169462, -116.47199812375007),
                              new Point3d(141.58813051570115, 674.57420270368482, -116.62713552447232), new Point3d(141.67558417616689, 674.10542574125179, -116.81032062593574),
                              new Point3d(141.64350829626960, 673.56081538861315, -117.02954298985327), new Point3d(141.60812833985617, 672.96010574010631, -117.27134704028255),
                              new Point3d(141.43319879218933, 672.36931415679430, -117.51575257328776), new Point3d(141.15787032067786, 671.82700463182164, -117.74541926482010),
                              new Point3d(140.84849464223075, 671.21763293736637, -118.00348661295689), new Point3d(140.40845080233882, 670.66139163364403, -118.24636075619543),
                              new Point3d(139.87603450724413, 670.20532656727630, -118.45434681304027), new Point3d(139.61346584843508, 669.98041160147022, -118.55691809137994),
                              new Point3d(139.32731220219256, 669.77890628001796, -118.65145358845511), new Point3d(139.02399019879240, 669.60798847286276, -118.73494494862938),
                              new Point3d(138.53007432555225, 669.32967362254840, -118.87089851482709), new Point3d(138.03204977521381, 669.15616051777897, -118.96610372066651),
                              new Point3d(137.46620845125764, 669.02892788587826, -119.04725153349821), new Point3d(136.98884843444444, 668.92159077787301, -119.11571016254790),
                              new Point3d(136.49884967222192, 668.85518903956904, -119.16903265363507), new Point3d(136.02576110604815, 668.79519540085516, -119.21892114496858),
                              new Point3d(135.58463846101446, 668.73925544692918, -119.26543874194152), new Point3d(135.15992090558476, 668.68907747231651, -119.30879741099884),
                              new Point3d(134.77218887513200, 668.61661756298793, -119.35861340906597), new Point3d(134.57552657547021, 668.57986503474001, -119.38388067607093),
                              new Point3d(134.37359705711711, 668.53460869561980, -119.41271147891729), new Point3d(134.18246959143613, 668.47599826189207, -119.44604456725065),
                              new Point3d(133.92847785423794, 668.39811010580013, -119.49034133528345), new Point3d(133.69849495112064, 668.29844439864212, -119.54162267553070),
                              new Point3d(133.50626676054918, 668.16326541891272, -119.60428190241299), new Point3d(133.32117861558723, 668.03310747214346, -119.66461374049518),
                              new Point3d(133.20569023817683, 667.89436038790984, -119.72419477949786), new Point3d(133.12681873527174, 667.76698677906757, -119.77726295045298),
                              new Point3d(133.00042094822982, 667.56286055232908, -119.86230886848450), new Point3d(132.94210565302444, 667.34693786672824, -119.94772751366609),
                              new Point3d(132.91129012656816, 667.17538848675429, -120.01454650964818), new Point3d(132.85709449867505, 666.87368257965090, -120.13206186177348),
                              new Point3d(132.86338186068770, 666.55096993649988, -120.25354828859214), new Point3d(132.90108250247991, 666.24416811904632, -120.36665322365909),
                              new Point3d(132.93906929383289, 665.93503766204890, -120.48061663025200), new Point3d(133.01128575269379, 665.62237336145279, -120.59335989958564),
                              new Point3d(133.10398390614603, 665.32157747349311, -120.69992141151192), new Point3d(133.16917541187547, 665.11003780281965, -120.77486255280296),
                              new Point3d(133.24588339558920, 664.89986836181606, -120.84834743736218), new Point3d(133.33146566559972, 664.69429686513422, -120.91934460967539),
                              new Point3d(133.51062670000766, 664.26394602277196, -121.06797267048880), new Point3d(133.73487497541132, 663.83884532968148, -121.21081029769147),
                              new Point3d(133.99220599950129, 663.44290675464663, -121.34000159664851), new Point3d(134.18357238043788, 663.14846370237353, -121.43607579297728),
                              new Point3d(134.39854936273969, 662.86193915429317, -121.52728986097996), new Point3d(134.63424089350229, 662.59714188224120, -121.60898242284954),
                              new Point3d(134.96722008066482, 662.22304282095786, -121.72439566133349), new Point3d(135.36178038775793, 661.86963059956020, -121.82771116243052),
                              new Point3d(135.80272602227342, 661.61664954394394, -121.89225727291942), new Point3d(135.95390375289071, 661.52991526451456, -121.91438683661180),
                              new Point3d(136.10992991827609, 661.45538972423458, -121.93186344162362), new Point3d(136.27521808868039, 661.39386249979111, -121.94420315233194),
                              new Point3d(136.71927843070961, 661.22856452194037, -121.97735480586093), new Point3d(137.18294607194903, 661.17358057186777, -121.97037057583519),
                              new Point3d(137.65731471734995, 661.18242581857396, -121.94041694604692), new Point3d(137.98754207047511, 661.18858335540915, -121.91956500320170),
                              new Point3d(138.32021109454075, 661.22574824899289, -121.88767920002594), new Point3d(138.64432294054845, 661.27493997704039, -121.85202474688708),
                              new Point3d(139.21412572019850, 661.36142118321197, -121.78934266119454), new Point3d(139.78864479663613, 661.48980344155609, -121.71154370476464),
                              new Point3d(140.35450522315202, 661.57966854166159, -121.64774623488435), new Point3d(140.65473360076859, 661.62734823782830, -121.61389723258509),
                              new Point3d(140.96364077431656, 661.66595388422127, -121.58274611745072), new Point3d(141.26972190456621, 661.67929736282224, -121.56069951138737),
                              new Point3d(141.79282219971165, 661.70210170096868, -121.52302131107069), new Point3d(142.24369013796144, 661.64873713647739, -121.51645547111370),
                              new Point3d(142.65813480282475, 661.50585672018678, -121.54346514190655), new Point3d(142.82356854073544, 661.44882319232363, -121.55424658314062),
                              new Point3d(142.98080763000209, 661.37811727234782, -121.57029507607906), new Point3d(143.13225475564155, 661.29599086604537, -121.59063753204614),
                              new Point3d(143.32275331744989, 661.19268773486317, -121.61622539704534), new Point3d(143.50340718505174, 661.07168414270734, -121.64851320597430),
                              new Point3d(143.67297291373532, 660.93933550031409, -121.68525290193438), new Point3d(143.89075934868859, 660.76934985981052, -121.73244054690562),
                              new Point3d(144.09337008278570, 660.57819498471906, -121.78765124884475), new Point3d(144.27836329378806, 660.37643328994704, -121.84718320365508),
                              new Point3d(144.40035869844556, 660.24337976076765, -121.88644207633362), new Point3d(144.51590100565065, 660.10439721681632, -121.92797114696697),
                              new Point3d(144.62463318229814, 659.96139730344419, -121.97109070309399), new Point3d(144.86588612387936, 659.64411174170345, -122.06676357635071),
                              new Point3d(145.07846980088556, 659.30061731951980, -122.17218629341041), new Point3d(145.25591430357429, 658.94787435905016, -122.28156120266969),
                              new Point3d(145.42130468617029, 658.61909385412548, -122.38350608016488), new Point3d(145.56099703725536, 658.27267917246559, -122.49186219450301),
                              new Point3d(145.66470464443691, 657.92188826287202, -122.60250060772094), new Point3d(145.82224605816805, 657.38900453984218, -122.77057055461319),
                              new Point3d(145.90903232100780, 656.80464087445159, -122.95670328376860), new Point3d(145.84119433634112, 656.25750359147787, -123.13620020437968),
                              new Point3d(145.82098269902795, 656.09448960486964, -123.18967948558252), new Point3d(145.78690853511003, 655.93384590490211, -123.24290609807538),
                              new Point3d(145.73591353654058, 655.77602825256281, -123.29592787084935), new Point3d(145.66079609011396, 655.54355723820197, -123.37403082775732),
                              new Point3d(145.53402602504553, 655.26397521147726, -123.46942089712806), new Point3d(145.29708015331477, 654.96255349775288, -123.57682074277339),
                              new Point3d(145.04988139133440, 654.64808895814133, -123.68886788960269), new Point3d(144.73828416199819, 654.38270555632903, -123.78820547685987),
                              new Point3d(144.40285662083255, 654.15620195242821, -123.87645335110690), new Point3d(144.10974928577841, 653.95827584448614, -123.95356717173966),
                              new Point3d(143.79083907048420, 653.78496643531446, -124.02417688631974), new Point3d(143.46943526003625, 653.63244549248475, -124.08839257610559),
                              new Point3d(143.20728004824099, 653.50804074440475, -124.14077054179593), new Point3d(142.93607734745697, 653.39397475557905, -124.19036212267697),
                              new Point3d(142.66486863111658, 653.28882612186703, -124.23715815709430), new Point3d(142.23561971455990, 653.12240471142218, -124.31122342809236),
                              new Point3d(141.78731703219393, 652.97121953271551, -124.38148676879446), new Point3d(141.35189807381542, 652.83289667950498, -124.44699426851489),
                              new Point3d(141.12487294853278, 652.76077588073110, -124.48114952988973), new Point3d(140.89554466008485, 652.69030583549068, -124.51488465763894),
                              new Point3d(140.66729398300600, 652.62133106675822, -124.54806165587767), new Point3d(140.44282409635858, 652.55349881006111, -124.58068910359989),
                              new Point3d(140.22116149665737, 652.48764234743999, -124.61252292169557), new Point3d(139.99551813487980, 652.42061093082043, -124.64487163233768) 
                             };
      double[] arrHole21LoopKnots = { 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 1.7138769016043693, 1.7138769016043693, 1.7138769016043693, 3.0292627415256024,
                                3.0292627415256024, 3.0292627415256024, 3.8292846593201815, 3.8292846593201815, 3.8292846593201815, 5.4280297838018745, 5.4280297838018745, 5.4280297838018745,
                                5.8643002421557942, 5.8643002421557942, 5.8643002421557942, 6.7805782758310364, 6.7805782758310364, 6.7805782758310364, 7.7286820500727069, 7.7286820500727069,
                                7.7286820500727069, 8.5623438811982400, 8.5623438811982400, 8.5623438811982400, 9.2007708390761120, 9.2007708390761120, 9.2007708390761120, 10.355812951198260,
                                10.355812951198260, 10.355812951198260, 11.629833795315278, 11.629833795315278, 11.629833795315278, 13.061400464903434, 13.061400464903434, 13.061400464903434,
                                13.767397903172359, 13.767397903172359, 13.767397903172359, 14.917012297509237, 14.917012297509237, 14.917012297509237, 15.886860068210208, 15.886860068210208,
                                15.886860068210208, 16.791176605284381, 16.791176605284381, 16.791176605284381, 17.249856713800387, 17.249856713800387, 17.249856713800387, 17.859402585008148,
                                17.859402585008148, 17.859402585008148, 18.446307732664529, 18.446307732664529, 18.446307732664529, 19.386869394796872, 19.386869394796872, 19.386869394796872,
                                21.041046224252220, 21.041046224252220, 21.041046224252220, 22.707778329975337, 22.707778329975337, 22.707778329975337, 23.879935180712401, 23.879935180712401,
                                23.879935180712401, 26.333770932446551, 26.333770932446551, 26.333770932446551, 28.158586532830434, 28.158586532830434, 28.158586532830434, 30.736641049135212,
                                30.736641049135212, 30.736641049135212, 31.620524232627599, 31.620524232627599, 31.620524232627599, 33.995149517761284, 33.995149517761284, 33.995149517761284,
                                35.648222987469794, 35.648222987469794, 35.648222987469794, 38.554398338997338, 38.554398338997338, 38.554398338997338, 40.096326825197764, 40.096326825197764,
                                40.096326825197764, 42.731521113610007, 42.731521113610007, 42.731521113610007, 43.783410734138002, 43.783410734138002, 43.783410734138002, 45.106535613831916,
                                45.106535613831916, 45.106535613831916, 46.805927659521934, 46.805927659521934, 46.805927659521934, 47.926606732890853, 47.926606732890853, 47.926606732890853,
                                50.413148724000663, 50.413148724000663, 50.413148724000663, 52.730775503186244, 52.730775503186244, 52.730775503186244, 56.251464189551399, 56.251464189551399,
                                56.251464189551399, 57.300417562371329, 57.300417562371329, 57.300417562371329, 58.845563184436450, 58.845563184436450, 58.845563184436450, 60.457568838149221,
                                60.457568838149221, 60.457568838149221, 61.866191038967969, 61.866191038967969, 61.866191038967969, 63.015143351886955, 63.015143351886955, 63.015143351886955,
                                64.833619329173587, 64.833619329173587, 64.833619329173587, 65.781763016128323, 65.781763016128323, 65.781763016128323, 66.714201459071887, 66.714201459071887,
                                66.714201459071887, 66.714201459071887 
                              };
      Point2d[] arrHole21LoopProjCPS = {
                                  new Point2d(-460.85276110672538, 47.727400892705766), new Point2d(-460.95567785735221, 48.285970572676241), new Point2d(-461.05999977388183, 48.852015225933755),
                                  new Point2d(-461.13258921411278, 49.410033950429543), new Point2d(-461.18830098764693, 49.838308541169148), new Point2d(-461.22705981742536, 50.274407192110104),
                                  new Point2d(-461.23092645577782, 50.708518053258508), new Point2d(-461.23327815771671, 50.972545681602298), new Point2d(-461.22276181910871, 51.236160643841487),
                                  new Point2d(-461.19544402628395, 51.498304061794620), new Point2d(-461.14085278682063, 52.022165348648954), new Point2d(-461.01741311454958, 52.557068022754414),
                                  new Point2d(-460.77028222604696, 53.112183325100446), new Point2d(-460.70284439349348, 53.263664886285689), new Point2d(-460.62876314233466, 53.410883291525202),
                                  new Point2d(-460.54878670660696, 53.553842989178392), new Point2d(-460.38081601350660, 53.854094423013052), new Point2d(-460.18612513846921, 54.137282865290999),
                                  new Point2d(-459.97179625717706, 54.396935684978182), new Point2d(-459.75002293749264, 54.665607211840474), new Point2d(-459.50534699315716, 54.911353577412626),
                                  new Point2d(-459.24411386838341, 55.126453730846869), new Point2d(-459.01441318469642, 55.315589975906967), new Point2d(-458.77022687779481, 55.482411945334547),
                                  new Point2d(-458.51527125739676, 55.620693023766300), new Point2d(-458.32002357416667, 55.726590121293320), new Point2d(-458.11807900370394, 55.815948309327268),
                                  new Point2d(-457.91114990344846, 55.885678474473373), new Point2d(-457.53677375584169, 56.011834295801314), new Point2d(-457.10312278667146, 56.088798153588435),
                                  new Point2d(-456.59936264384334, 56.044387279571843), new Point2d(-456.04371110900951, 55.995401724319109), new Point2d(-455.49726113096642, 55.806942514789121),
                                  new Point2d(-454.99557514515573, 55.519077519119250), new Point2d(-454.43185050972107, 55.195615045922636), new Point2d(-453.91715927063058, 54.742551574305871),
                                  new Point2d(-453.49504369791032, 54.199234411314492), new Point2d(-453.28687140714328, 53.931289834657228), new Point2d(-453.10033485578481, 53.640251974153415),
                                  new Point2d(-452.94209501434705, 53.332697184293082), new Point2d(-452.68442439174464, 52.831888832441798), new Point2d(-452.52373476307014, 52.329280680836725),
                                  new Point2d(-452.40590110516985, 51.759758342767739), new Point2d(-452.30649324198453, 51.279292916567840), new Point2d(-452.24498918635516, 50.787064138188583),
                                  new Point2d(-452.18941927693578, 50.311912566403620), new Point2d(-452.13760414721150, 49.868866310111912), new Point2d(-452.09112436263013, 49.442377419077168),
                                  new Point2d(-452.02399951787220, 49.052450126590841), new Point2d(-451.98995299785912, 48.854674363903101), new Point2d(-451.94802777710385, 48.651438454076271),
                                  new Point2d(-451.89372749494754, 48.458732958618015), new Point2d(-451.82156715448531, 48.202644157436602), new Point2d(-451.72922672287683, 47.970131203804058),
                                  new Point2d(-451.60396062111784, 47.774669792232565), new Point2d(-451.48334735241747, 47.586468519714401), new Point2d(-451.35475553022286, 47.467798253696117),
                                  new Point2d(-451.23668642384712, 47.386054350845257), new Point2d(-451.04747139415673, 47.255053316961657), new Point2d(-450.84726652820757, 47.192004957523814),
                                  new Point2d(-450.68816921475133, 47.157463332077874), new Point2d(-450.40836290311489, 47.096714570178662), new Point2d(-450.10895253032351, 47.096127811152058),
                                  new Point2d(-449.82416582988066, 47.127377711710480), new Point2d(-449.53721758561869, 47.158864800414605), new Point2d(-449.24684704433616, 47.224595815326019),
                                  new Point2d(-448.96735155336739, 47.311125080541046), new Point2d(-448.77079173828719, 47.371978213844692), new Point2d(-448.57543122392366, 47.444411707830220),
                                  new Point2d(-448.38427072922218, 47.525845869601739), new Point2d(-447.98408840215325, 47.696323103464934), new Point2d(-447.58844673900080, 47.912142717937741),
                                  new Point2d(-447.21963473834199, 48.161763261362694), new Point2d(-446.94536459131388, 48.347395678486066), new Point2d(-446.67828738321384, 48.556875581363315),
                                  new Point2d(-446.43131276682965, 48.787574914835368), new Point2d(-446.08239311431907, 49.113501248674808), new Point2d(-445.75241647281365, 49.501593861553324),
                                  new Point2d(-445.51607540325557, 49.938188204491723), new Point2d(-445.43504612492580, 50.087874099530232), new Point2d(-445.36540432880906, 50.242663580004027),
                                  new Point2d(-445.30790144274670, 50.406990508467324), new Point2d(-445.15341518284026, 50.848468394510462), new Point2d(-445.10198688012827, 51.311789966732533),
                                  new Point2d(-445.11025673447114, 51.787093237752380), new Point2d(-445.11601371722463, 52.117971221873709), new Point2d(-445.15076144643126, 52.451910398166390),
                                  new Point2d(-445.19674548935660, 52.777518997665545), new Point2d(-445.27758744499982, 53.349953135318678), new Point2d(-445.39756611713403, 53.927950150655441),
                                  new Point2d(-445.48152572962056, 54.496511252937211), new Point2d(-445.52607215319244, 54.798172527193415), new Point2d(-445.56213487969939, 55.108346300767259),
                                  new Point2d(-445.57459931936080, 55.415184133667800), new Point2d(-445.59590135800789, 55.939577652540244), new Point2d(-445.54606836715112, 56.390096329227454),
                                  new Point2d(-445.41255997289954, 56.802352874299686), new Point2d(-445.35926747193133, 56.966913181167037), new Point2d(-445.29319152994753, 57.122994337382842),
                                  new Point2d(-445.21642332216993, 57.273050170072217), new Point2d(-445.11986002912300, 57.461798686741531), new Point2d(-445.00672361996612, 57.640339683531501),
                                  new Point2d(-444.88292904037996, 57.807558719022666), new Point2d(-444.72393004710511, 58.022331113649074), new Point2d(-444.54505238301874, 58.221498740822234),
                                  new Point2d(-444.35612866775898, 58.402832833781794), new Point2d(-444.23154125740143, 58.522415200772102), new Point2d(-444.10135325899313, 58.635426510831579),
                                  new Point2d(-443.96734205390084, 58.741548705476845), new Point2d(-443.67000047176572, 58.977010667286244), new Point2d(-443.34780357520788, 59.183298288350521),
                                  new Point2d(-443.01652887649482, 59.354279566062033), new Point2d(-442.70775825619199, 59.513645782027965), new Point2d(-442.38208617677736, 59.646992777814575),
                                  new Point2d(-442.05187687153756, 59.744283561680540), new Point2d(-441.55025850246659, 59.892077229375758), new Point2d(-440.99912762145010, 59.968208212417196),
                                  new Point2d(-440.48194805573348, 59.890336879970256), new Point2d(-440.32785966617695, 59.867135908770351), new Point2d(-440.17591217133224, 59.830108831563813),
                                  new Point2d(-440.02654298963290, 59.776199523406845), new Point2d(-439.80651687297751, 59.696789195316320), new Point2d(-439.54168281120610, 59.564832025527423),
                                  new Point2d(-439.25574823166602, 59.322196487165009), new Point2d(-438.95744097037721, 59.069061860857680), new Point2d(-438.70530352212000, 58.752349740158678),
                                  new Point2d(-438.48992840375200, 58.412471952621239), new Point2d(-438.30172672310755, 58.115475849668549), new Point2d(-438.13676789922766, 57.793085572186691),
                                  new Point2d(-437.99150826770966, 57.468566950290835), new Point2d(-437.87302625776454, 57.203871121277345), new Point2d(-437.76432299854139, 56.930299689041256),
                                  new Point2d(-437.66407243283601, 56.656880201401378), new Point2d(-437.50540331890079, 56.224132241016541), new Point2d(-437.36113708251884, 55.772575764170163),
                                  new Point2d(-437.22906462605692, 55.334151138977866), new Point2d(-437.16020275084986, 55.105558875030653), new Point2d(-437.09289286851691, 54.874690864969722),
                                  new Point2d(-437.02698929831683, 54.644930279603386), new Point2d(-436.96217736819773, 54.418975495095061), new Point2d(-436.89923223665198, 54.195868353736557),
                                  new Point2d(-436.83514295592624, 53.968757717520944) 
                                 };
//hole22
      Point3d[] arrHole22LoopCPS = {
                              new Point3d(139.99551813487980, 652.42061093082043, -124.64487163233768), new Point3d(139.95321538655642, 652.68331372742807, -124.57053241815801),
                              new Point3d(139.90491704513161, 652.98071041207663, -124.48541596129257), new Point3d(139.84649703123412, 653.25270673543230, -124.40745060836568),
                              new Point3d(139.73129311656726, 653.78908184278100, -124.25370340314718), new Point3d(139.58785238932185, 654.18668936401536, -124.14001676140744),
                              new Point3d(139.36300997053993, 654.51424715136955, -124.05168795269189), new Point3d(139.19880046229511, 654.75347293213895, -123.98717863847259),
                              new Point3d(138.99064315670350, 654.95604509334817, -123.93584077268227), new Point3d(138.74793228977290, 655.12323472261312, -123.89724587613880),
                              new Point3d(138.60922560205631, 655.21878182352873, -123.87518930186994), new Point3d(138.46148348212969, 655.30122231789881, -123.85764934394356),
                              new Point3d(138.30756438787657, 655.37180507664459, -123.84414044632808), new Point3d(138.12481680734450, 655.45560772926979, -123.82810138134487),
                              new Point3d(137.93359144157665, 655.52255848552863, -123.81777517586264), new Point3d(137.73912548874262, 655.57485396265884, -123.81224158544083),
                              new Point3d(137.49585598755669, 655.64027361299009, -123.80531927455780), new Point3d(137.24563676466832, 655.68326564105905, -123.80584338949653),
                              new Point3d(136.99491330906494, 655.70796642060509, -123.81219702227034), new Point3d(136.83485636188558, 655.72373491485143, -123.81625305708697),
                              new Point3d(136.67376819689380, 655.73212989513365, -123.82270427618933), new Point3d(136.51312322137778, 655.73401495620544, -123.83119707387162),
                              new Point3d(136.12360719799912, 655.73858566556089, -123.85178956840774), new Point3d(135.74060046297794, 655.70498263419961, -123.88413988788967),
                              new Point3d(135.35645476182620, 655.64357018904866, -123.92533112732508), new Point3d(134.77887688584065, 655.55123420943721, -123.98726374432688),
                              new Point3d(134.19138783519713, 655.39397506869182, -124.07020531259444), new Point3d(133.64249620985609, 655.22237440547076, -124.15510216152597),
                              new Point3d(133.04605982377504, 655.03590976528415, -124.24735273965226), new Point3d(132.45376622969064, 654.81956227124317, -124.34831069686244),
                              new Point3d(131.89096207856647, 654.61288338262261, -124.44407391497401), new Point3d(131.56426147034458, 654.49290893239140, -124.49966323772482),
                              new Point3d(131.31272314839939, 654.40012752781729, -124.54245875799953), new Point3d(131.02905116518966, 654.29969891692394, -124.58933273627868),
                              new Point3d(130.76094050476163, 654.20477949313090, -124.63363536049668), new Point3d(130.47173414690096, 654.10624250201431, -124.68012668907572),
                              new Point3d(130.19496922542584, 654.02304445786967, -124.72114693953770), new Point3d(129.98424493584557, 653.95969882630118, -124.75237908807348),
                              new Point3d(129.76041193099138, 653.89903013944650, -124.78348601571177), new Point3d(129.54311926964647, 653.85414321542578, -124.80940568161144),
                              new Point3d(129.22564174821358, 653.78856074570660, -124.84727585741609), new Point3d(128.93480143704392, 653.75902829203551, -124.87266616278201),
                              new Point3d(128.68770678156088, 653.78846562934848, -124.87773046391864), new Point3d(128.62751921539567, 653.79563600575250, -124.87896403151299),
                              new Point3d(128.56755319287248, 653.80659408254837, -124.87903714798759), new Point3d(128.50906229467725, 653.82247091157365, -124.87753260456749),
                              new Point3d(128.40489990952938, 653.85074485556981, -124.87485326750770), new Point3d(128.30730901817495, 653.89410501780628, -124.86722296208853),
                              new Point3d(128.22121841889526, 653.95776751354788, -124.85272316652794), new Point3d(128.14415165348984, 654.01475704162044, -124.83974320875967),
                              new Point3d(128.08157928018352, 654.08409330877112, -124.82214920074317), new Point3d(128.03147730628780, 654.16394265327915, -124.80056160919035),
                              new Point3d(128.00281041357547, 654.20963012652965, -124.78820981697854), new Point3d(127.97884322942588, 654.25777581664386, -124.77481607730709),
                              new Point3d(127.95880618586108, 654.30738897448384, -124.76072127904098), new Point3d(127.92660187530429, 654.38712915858900, -124.73806757470933),
                              new Point3d(127.90454859321703, 654.47039038411515, -124.71368527173802), new Point3d(127.88887484382791, 654.55491983382421, -124.68846888944198),
                              new Point3d(127.83457929645171, 654.84773890521660, -124.60111663915265), new Point3d(127.84603184403694, 655.21286758095596, -124.48652594367336),
                              new Point3d(127.87573956727142, 655.55205471010936, -124.37763069891213), new Point3d(127.90071497242664, 655.83721072516914, -124.28608201859346),
                              new Point3d(127.94236370155993, 656.14189577507352, -124.18645137908376), new Point3d(127.98442356524497, 656.41959497509754, -124.09460597165678),
                              new Point3d(128.05242274873328, 656.86855788658943, -123.94611731370597), new Point3d(128.13526918138547, 657.33776184367173, -123.78809342651707),
                              new Point3d(128.20677955204278, 657.79535453260632, -123.63243913398938), new Point3d(128.32973664056030, 658.58215320587783, -123.36480244788996),
                              new Point3d(128.41925438192521, 659.33489204796683, -123.10372037420750), new Point3d(128.33484075691067, 659.86508447695485, -122.92610835471244),
                              new Point3d(128.31512407029396, 659.98892275504227, -122.88462310245437), new Point3d(128.28300210976019, 660.11893806076114, -122.84156623351542),
                              new Point3d(128.22944047922545, 660.23453626722676, -122.80458425902145), new Point3d(128.19136245423931, 660.31671732389373, -122.77829304020008),
                              new Point3d(128.14119412094044, 660.39447777265877, -122.75415475145385), new Point3d(128.07410973962513, 660.45843507807842, -122.73573149448217),
                              new Point3d(128.01718684438779, 660.51270456562065, -122.72009886724511), new Point3d(127.96126223539636, 660.54472820086926, -122.71211838466856),
                              new Point3d(127.91647775719491, 660.56509348890745, -122.70756535134004), new Point3d(127.85351015660795, 660.59372736775401, -122.70116372301693),
                              new Point3d(127.78567106855905, 660.61109892256479, -122.69895155819216), new Point3d(127.71714552838516, 660.62086276789432, -122.69942846678133),
                              new Point3d(127.59907201148911, 660.63768644492859, -122.70025020819656), new Point3d(127.47293632067546, 660.63274962333048, -122.70910814716052),
                              new Point3d(127.35515891618148, 660.61901739340396, -122.72055650338632), new Point3d(127.08632844776400, 660.58767316554099, -122.74668772083002),
                              new Point3d(126.77979791667137, 660.50124541529317, -122.79412885452952), new Point3d(126.47610248005311, 660.40040795139794, -122.84631448423113),
                              new Point3d(126.31866561862265, 660.34813342924156, -122.87336771177873), new Point3d(126.15690025056978, 660.29016130577418, -122.90261800872040),
                              new Point3d(126.00304818698321, 660.23367907364309, -122.93087512511244), new Point3d(125.65622600162388, 660.10635357195633, -122.99457394481352),
                              new Point3d(125.30597971128331, 659.97057657421601, -123.06123154385905), new Point3d(124.96668955780338, 659.85539240273397, -123.12004984713177),
                              new Point3d(124.64085977816751, 659.74477783633233, -123.17653470116404), new Point3d(124.31662603951065, 659.65012764749656, -123.22731911739766),
                              new Point3d(124.03150101998791, 659.61291965326234, -123.25619940101832), new Point3d(123.86066780858322, 659.59062641096602, -123.27350307926253),
                              new Point3d(123.71018272848731, 659.58986301537959, -123.28228103685541), new Point3d(123.57159569906526, 659.61534017640417, -123.28139687377191),
                              new Point3d(123.45887125692701, 659.63606288599499, -123.28067770985906), new Point3d(123.35271558019016, 659.67439427536112, -123.27355534382269),
                              new Point3d(123.25599993087793, 659.73420059499949, -123.25850066962700), new Point3d(123.16562438174608, 659.79008636961100, -123.24443288993351),
                              new Point3d(123.08948738515561, 659.86099646823175, -123.22437498042378), new Point3d(123.02535546712622, 659.94322646704302, -123.19967933166178),
                              new Point3d(122.94560924171482, 660.04547714380863, -123.16897099120716), new Point3d(122.88667763156761, 660.16240934090695, -123.13192932170249),
                              new Point3d(122.84250939976712, 660.28178191447705, -123.09307706695893), new Point3d(122.80452302610283, 660.38444690694007, -123.05966263748718),
                              new Point3d(122.77639140243411, 660.49186103158615, -123.02395171152628), new Point3d(122.75541777994185, 660.59830428033968, -122.98807482970554),
                              new Point3d(122.72009663024383, 660.77756267621066, -122.92765547611243), new Point3d(122.70283440824988, 660.96556565087292, -122.86288276967116),
                              new Point3d(122.69613533963960, 661.14695295378579, -122.79955074684973), new Point3d(122.69100251582243, 661.28593183617716, -122.75102577509061),
                              new Point3d(122.69186466902268, 661.42667246863368, -122.70138147791185), new Point3d(122.69672936102974, 661.56535316594420, -122.65208167398310),
                              new Point3d(122.70712273572408, 661.86164334175157, -122.54675304601538), new Point3d(122.73654816487873, 662.16737087571062, -122.43627061110925),
                              new Point3d(122.77396142431624, 662.45944863153852, -122.32953687608064), new Point3d(122.82514742939394, 662.85904742933519, -122.18351181477158),
                              new Point3d(122.89610874486469, 663.27034344865137, -122.03089695746229), new Point3d(122.97241560559931, 663.66418674075749, -121.88316600626159),
                              new Point3d(123.05189795580407, 664.07441971545484, -121.72928727129106), new Point3d(123.14207206332804, 664.49143866821134, -121.57109870776272),
                              new Point3d(123.23494844738904, 664.90161851234257, -121.41413572852278), new Point3d(123.38414887811184, 665.56054822883539, -121.16198396414366),
                              new Point3d(123.53575962726978, 666.18145120876420, -120.92101885829371), new Point3d(123.68384752609086, 666.82040824230887, -120.67114907350776),
                              new Point3d(123.79062085233235, 667.28110468232921, -120.49098966641753), new Point3d(123.89030746414137, 667.72784757754573, -120.31530952597295),
                              new Point3d(123.99171323746913, 668.20009187658297, -120.12861270578929), new Point3d(124.02369653541403, 668.34903734214413, -120.06972868144483),
                              new Point3d(124.05294223803251, 668.48689033680284, -120.01514391038430), new Point3d(124.08551042537215, 668.64139335220943, -119.95385331643469),
                              new Point3d(124.46037352020163, 670.41973851805596, -119.24839244775653), new Point3d(124.76133506882482, 671.94753090189738, -118.62758193353436),
                              new Point3d(125.44843579608484, 673.15840523713041, -118.10648932992788), new Point3d(125.64913380236646, 673.51209437657872, -117.95428130207587),
                              new Point3d(125.87958272940502, 673.83479872248790, -117.81228940337382), new Point3d(126.15612585164340, 674.14573376818203, -117.67209944084460),
                              new Point3d(126.43502163134384, 674.45931405650435, -117.53071682901431), new Point3d(126.74771025012606, 674.74718439379865, -117.39739856272972),
                              new Point3d(127.07952889607382, 675.01224419081393, -117.27196907406494), new Point3d(127.49862582895850, 675.34702268899878, -117.11354785622603),
                              new Point3d(127.95413588897362, 675.64997429147115, -116.96554152025419), new Point3d(128.41968889838049, 675.92550054537651, -116.82785190241643),
                              new Point3d(129.15154749339368, 676.35863329932283, -116.61140108323470), new Point3d(129.94862245494699, 676.74726510988808, -116.40875015618509),
                              new Point3d(130.72576584618511, 677.09626728680291, -116.22305278398370), new Point3d(131.54069423369779, 677.46223807736510, -116.02832673754287),
                              new Point3d(132.37539403365355, 677.80390754350958, -115.84206090487336), new Point3d(133.23513938128082, 678.15600616905419, -115.64990487257728) 
                             };
      double[] arrHole22LoopKnots = { 
                                0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.8288289362914333, 0.8288289362914333, 0.8288289362914333,
                                2.4632745942043455, 2.4632745942043455, 2.4632745942043455, 3.6569617960801062, 3.6569617960801062, 3.6569617960801062, 4.3391413788724522,
                                4.3391413788724522, 4.3391413788724522, 5.1490907152339309, 5.1490907152339309, 5.1490907152339309, 6.1623064898775173, 6.1623064898775173,
                                6.1623064898775173, 6.8091236108889515, 6.8091236108889515, 6.8091236108889515, 8.3774616931745065, 8.3774616931745065, 8.3774616931745065,
                                10.735518481726004, 10.735518481726004, 10.735518481726004, 13.297829153279226, 13.297829153279226, 13.297829153279226, 14.785217711408833,
                                14.785217711408833, 14.785217711408833, 16.191012968737343, 16.191012968737343, 16.191012968737343, 17.261362510568674, 17.261362510568674,
                                17.261362510568674, 18.825206947395120, 18.825206947395120, 18.825206947395120, 19.206129757439630, 19.206129757439630, 19.206129757439630,
                                19.884488778684709, 19.884488778684709, 19.884488778684709, 20.491743647257657, 20.491743647257657, 20.491743647257657, 20.839197226684515,
                                20.839197226684515, 20.839197226684515, 21.397638043551872, 21.397638043551872, 21.397638043551872, 23.332136871895848, 23.332136871895848,
                                23.332136871895848, 24.958477996352141, 24.958477996352141, 24.958477996352141, 27.587822431812331, 27.587822431812331, 27.587822431812331,
                                32.108796571959239, 32.108796571959239, 32.108796571959239, 33.164771011017052, 33.164771011017052, 33.164771011017052, 33.915484174035527,
                                33.915484174035527, 33.915484174035527, 34.552484410065681, 34.552484410065681, 34.552484410065681, 35.448115620855212, 35.448115620855212,
                                35.448115620855212, 36.991340623262076, 36.991340623262076, 36.991340623262076, 40.513798110358465, 40.513798110358465, 40.513798110358465,
                                42.339853376338034, 42.339853376338034, 42.339853376338034, 46.456252358387268, 46.456252358387268, 46.456252358387268, 50.409344894253508,
                                50.409344894253508, 50.409344894253508, 52.777847995147340, 52.777847995147340, 52.777847995147340, 54.704350005215382, 54.704350005215382,
                                54.704350005215382, 56.504562052610382, 56.504562052610382, 56.504562052610382, 58.743074795268107, 58.743074795268107, 58.743074795268107,
                                60.668281624443338, 60.668281624443338, 60.668281624443338, 63.910473984776161, 63.910473984776161, 63.910473984776161, 66.394640536134290,
                                66.394640536134290, 66.394640536134290, 71.702042060003834, 71.702042060003834, 71.702042060003834, 78.963229281711619, 78.963229281711619,
                                78.963229281711619, 86.526588855174950, 86.526588855174950, 86.526588855174950, 98.676679725757026, 98.676679725757026, 98.676679725757026,
                                107.43705533184132, 107.43705533184132, 107.43705533184132, 110.20007063091165, 110.20007063091165, 110.20007063091165, 142.00265317164522,
                                142.00265317164522, 142.00265317164522, 151.29199701499243, 151.29199701499243, 151.29199701499243, 160.66036883753469, 160.66036883753469,
                                160.66036883753469, 172.49290430502236, 172.49290430502236, 172.49290430502236, 191.09388552027028, 191.09388552027028, 191.09388552027028,
                                210.59925325394602, 210.59925325394602, 210.59925325394602, 210.59925325394602
                              };
      Point2d[] arrHole22LoopProjCPS = {
                                  new Point2d(-436.83514295592624, 53.968757717520944), new Point2d(-437.08631522899321, 53.930722768749895), new Point2d(-437.37027781470789, 53.887310727075359),
                                  new Point2d(-437.62963360121063, 53.833389138822383), new Point2d(-438.14108164620865, 53.727056092365075), new Point2d(-438.51906766424310, 53.590267887173816),
                                  new Point2d(-438.83020028059843, 53.370775287918789), new Point2d(-439.05743024106329, 53.210472921566804), new Point2d(-439.24961189486783, 53.005548764994792),
                                  new Point2d(-439.40813865101842, 52.765406256843875), new Point2d(-439.49873501177552, 52.628167358945269), new Point2d(-439.57686804869297, 52.481652270114303),
                                  new Point2d(-439.64374678470443, 52.328742316298317), new Point2d(-439.72315166461516, 52.147192884535244), new Point2d(-439.78656260439595, 51.956856429337741),
                                  new Point2d(-439.83608582515745, 51.763013777265257), new Point2d(-439.89803748834680, 51.520524001594978), new Point2d(-439.93873593344858, 51.270674877185023),
                                  new Point2d(-439.96211874392912, 51.019992953265621), new Point2d(-439.97704587251781, 50.859962519100506), new Point2d(-439.98499216812019, 50.698767184666863),
                                  new Point2d(-439.98677652030165, 50.537898980965302), new Point2d(-439.99110304071593, 50.147841695994991), new Point2d(-439.95929904715757, 49.763618974926345),
                                  new Point2d(-439.90115315385003, 49.377759588329212), new Point2d(-439.81372856090735, 48.797605120189466), new Point2d(-439.66479188890730, 48.206368287906912),
                                  new Point2d(-439.50212825793773, 47.653556748799367), new Point2d(-439.32537477196126, 47.052860907677093), new Point2d(-439.12013516473240, 46.455809266999545),
                                  new Point2d(-438.92387688676195, 45.888493468314508), new Point2d(-438.80995146767350, 45.559173906413463), new Point2d(-438.72179912225954, 45.305619422952311),
                                  new Point2d(-438.62634576895505, 45.019752173934442), new Point2d(-438.53612867629408, 44.749566672843713), new Point2d(-438.44242993356852, 44.458195510037314),
                                  new Point2d(-438.36328771964503, 44.179555029265828), new Point2d(-438.30303013094505, 43.967402719479132), new Point2d(-438.24529968359877, 43.742169709944456),
                                  new Point2d(-438.20257897256045, 43.523759680392672), new Point2d(-438.14016146904078, 43.204649617452610), new Point2d(-438.11204228891961, 42.912839335662348),
                                  new Point2d(-438.14006464732893, 42.665853263293016), new Point2d(-438.14689036174013, 42.605692145912499), new Point2d(-438.15732159876467, 42.545817784658453),
                                  new Point2d(-438.17243368990290, 42.487505330310270), new Point2d(-438.19934576521888, 42.383660723437266), new Point2d(-438.24061579637197, 42.286656846311267),
                                  new Point2d(-438.30118681402490, 42.201523021699231), new Point2d(-438.35540890617995, 42.125312743871710), new Point2d(-438.42136425544174, 42.063834401570126),
                                  new Point2d(-438.49728870586216, 42.015032178472126), new Point2d(-438.54073046883491, 41.987108965375064), new Point2d(-438.58650054667169, 41.963936824155539),
                                  new Point2d(-438.63365477540123, 41.944728155692701), new Point2d(-438.70944287363841, 41.913855241604395), new Point2d(-438.78854997347679, 41.893214807442327),
                                  new Point2d(-438.86883114682684, 41.878990841156167), new Point2d(-439.14693373369533, 41.829717497797532), new Point2d(-439.49326796440448, 41.847626213728724),
                                  new Point2d(-439.81449451399277, 41.883439146292723), new Point2d(-440.08455099591362, 41.913547226051065), new Point2d(-440.37274917173659, 41.960758632035656),
                                  new Point2d(-440.63513171853668, 42.007940644157301), new Point2d(-441.05933183191780, 42.084220927032973), new Point2d(-441.50187557404445, 42.175863489258994),
                                  new Point2d(-441.93274932817457, 42.256054214286642), new Point2d(-442.67360665892949, 42.393936565963472), new Point2d(-443.38011202532488, 42.498062647312004),
                                  new Point2d(-443.87707447965096, 42.423819085234470), new Point2d(-443.99315115531243, 42.406477843858575), new Point2d(-444.11496911946568, 42.376839942866312),
                                  new Point2d(-444.22324201272693, 42.325453384313796), new Point2d(-444.30021535901534, 42.288921661119801), new Point2d(-444.37302840888077, 42.240197300993572),
                                  new Point2d(-444.43290660453687, 42.174261011575467), new Point2d(-444.48371485813487, 42.118312303078866), new Point2d(-444.51368970752497, 42.062927934013665),
                                  new Point2d(-444.53275220441373, 42.018472246454756), new Point2d(-444.55955434074502, 41.955966930094121), new Point2d(-444.57581280329651, 41.888361200266999),
                                  new Point2d(-444.58495106803440, 41.819918180150999), new Point2d(-444.60069683309058, 41.701986850145055), new Point2d(-444.59607636149821, 41.575552173705297),
                                  new Point2d(-444.58322392572569, 41.457316071100671), new Point2d(-444.55388785448292, 41.187438610448325), new Point2d(-444.47299413553736, 40.878717284093312),
                                  new Point2d(-444.37857731098421, 40.572558455602575), new Point2d(-444.32963127251418, 40.413844562520929), new Point2d(-444.27534225122423, 40.250684941242447),
                                  new Point2d(-444.22243913774548, 40.095482098834971), new Point2d(-444.10318189152275, 39.745614910037389), new Point2d(-443.97595686596981, 39.392161912749593),
                                  new Point2d(-443.86798976964639, 39.050090476789741), new Point2d(-443.76430596143041, 38.721589754162324), new Point2d(-443.67554719096836, 38.395004602082878),
                                  new Point2d(-443.64065315207102, 38.108703295599213), new Point2d(-443.61974632062675, 37.937165309568257), new Point2d(-443.61903051467198, 37.786424659659332),
                                  new Point2d(-443.64292313781903, 37.648108971256939), new Point2d(-443.66235700999817, 37.535605233503503), new Point2d(-443.69830446440960, 37.430021553797339),
                                  new Point2d(-443.75437740298554, 37.334311003156110), new Point2d(-443.80677453469355, 37.244874664430945), new Point2d(-443.87324908870181, 37.169992579926280),
                                  new Point2d(-443.95031309014331, 37.107358432397483), new Point2d(-444.04613999535519, 37.029474642223093), new Point2d(-444.15569402155808, 36.972730055118134),
                                  new Point2d(-444.26749170273962, 36.930827562149048), new Point2d(-444.36364199717178, 36.894789810794649), new Point2d(-444.46420986532115, 36.868720820849944),
                                  new Point2d(-444.56383747295428, 36.849807774205182), new Point2d(-444.73161781917548, 36.817956788423807), new Point2d(-444.90749150335682, 36.804381850019475),
                                  new Point2d(-445.07709034401671, 36.801271791203362), new Point2d(-445.20703691112078, 36.798888865323249), new Point2d(-445.33858036910146, 36.802554582570316),
                                  new Point2d(-445.46815092097847, 36.810196980653906), new Point2d(-445.74497734556121, 36.826524901667369), new Point2d(-446.03039384132694, 36.862145668574755),
                                  new Point2d(-446.30287303664795, 36.905529701197480), new Point2d(-446.67565855934635, 36.964884468665048), new Point2d(-447.05897832744540, 37.044355285144604),
                                  new Point2d(-447.42572128362889, 37.128887163192381), new Point2d(-447.80772614921864, 37.216936812591875), new Point2d(-448.19570720962469, 37.315904648474941),
                                  new Point2d(-448.57703355470653, 37.417501197694882), new Point2d(-449.18961182750837, 37.580710059202787), new Point2d(-449.76613210469287, 37.745693333913387),
                                  new Point2d(-450.35890253833981, 37.907662507111134), new Point2d(-450.78629783128383, 38.024444415076502), new Point2d(-451.20048403543302, 38.133897847204956),
                                  new Point2d(-451.63808607568512, 38.245690145535143), new Point2d(-451.77610540116717, 38.280949344968327), new Point2d(-451.90382600687781, 38.313232411087782),
                                  new Point2d(-452.04695092943217, 38.349211538636162), new Point2d(-453.69433310813497, 38.763334896543689), new Point2d(-455.10710776455096, 39.098891951208756),
                                  new Point2d(-456.22714003998829, 39.814337209235383), new Point2d(-456.55429475796939, 40.023314483152973), new Point2d(-456.85279159745841, 40.261417933156487),
                                  new Point2d(-457.14042255164333, 40.545440134639499), new Point2d(-457.43050049169148, 40.831878620954704), new Point2d(-457.69681737924418, 41.151600322381896),
                                  new Point2d(-457.94205777849237, 41.489975769760413), new Point2d(-458.25180378101362, 41.917354140119201), new Point2d(-458.53214765820451, 42.380499020748346),
                                  new Point2d(-458.78715366526114, 42.853087905137635), new Point2d(-459.18802818282069, 43.596007035632887), new Point2d(-459.54782734135591, 44.403258648520172),
                                  new Point2d(-459.87102879104600, 45.189652641678137), new Point2d(-460.20994441357749, 46.014281398763593), new Point2d(-460.52645905123813, 46.858171975914665),
                                  new Point2d(-460.85276110672561, 47.727400892705766) 
                                 };

      /********************************************************************/
      /* Create NURBS surface                                             */
      /********************************************************************/
      Point3dCollection controlPoints = new Point3dCollection();
      DoubleCollection arrWeights = new DoubleCollection();
      for (int i1 = 0; i1 < arrCPNURBS.Length; i1++)
      {
        controlPoints.Add(arrCPNURBS[i1]);
      }
      KnotCollection uKnots = new KnotCollection();
      for (int i2 = 0; i2 < arrUKnots.Length; i2++)
      {
        uKnots.Add(arrUKnots[i2]);
      }
      KnotCollection vKnots = new KnotCollection();
      for (int i3 = 0; i3 < arrVKnots.Length; i3++)
      {
        vKnots.Add(arrVKnots[i3]);
      }
      Geometry.NurbSurface nsf = new Geometry.NurbSurface( degreeInU, degreeInV, propsInU, propsInV, 
                                                           numControlPointsInU, numControlPointsInV, 
                                                           controlPoints, arrWeights, uKnots, vKnots);
      /********************************************************************/
      /* Create trimming-loop curve                                       */
      /********************************************************************/
      Point3dCollection controlPoints2 = new Point3dCollection();
      for (int i2 = 0; i2 < arrTrimmingLoopCPS.Length; i2++)
      {
        controlPoints2.Add(arrTrimmingLoopCPS[i2]);
      }
      KnotCollection vecTrimmingLoopKnots = new KnotCollection();
      for (int i3 = 0; i3 < arrTrimmingLoopKnots.Length; i3++)
      {
        vecTrimmingLoopKnots.Add(arrTrimmingLoopKnots[i3]);
      }
      NurbCurve3d pTrimmigLoopCurve = new NurbCurve3d(degree, vecTrimmingLoopKnots, controlPoints2, arrWeights, periodic);
      /********************************************************************/
      /* Create second(hole1)loop curve                                   */
      /********************************************************************/
      Point3dCollection controlPoints3 = new Point3dCollection();
      for (int i4 = 0; i4 < arrHole1LoopCPS.Length; i4++)
      {
        controlPoints3.Add(arrHole1LoopCPS[i4]);
      }
      KnotCollection vecHole1Knots = new KnotCollection();
      for (int i5 = 0; i5 < arrHole1LoopKnots.Length; i5++)
      {
        vecHole1Knots.Add(arrHole1LoopKnots[i5]);
      }
      NurbCurve3d pHole1LoopCurve = new NurbCurve3d(degree, vecHole1Knots, controlPoints3, arrWeights, periodic);
      /********************************************************************/
      /* Create second(hole2)loop curves                                  */
      /********************************************************************/
      Point3dCollection controlPoints4 = new Point3dCollection();
      for (int i6 = 0; i6 < arrHole21LoopCPS.Length; i6++)
      {
        controlPoints4.Add(arrHole21LoopCPS[i6]);
      }
      KnotCollection vecHole21Knots = new KnotCollection();
      for (int i7 = 0; i7 < arrHole21LoopKnots.Length; i7++)
      {
        vecHole21Knots.Add(arrHole21LoopKnots[i7]);
      }
      NurbCurve3d pHole21LoopCurve = new NurbCurve3d(degree, vecHole21Knots, controlPoints4, arrWeights, periodic);

      Point3dCollection controlPoints5 = new Point3dCollection();
      for (int i8 = 0; i8 < arrHole22LoopCPS.Length; i8++)
      {
        controlPoints5.Add(arrHole22LoopCPS[i8]);
      }
      KnotCollection vecHole22Knots = new KnotCollection();
      for (int i9 = 0; i9 < arrHole22LoopKnots.Length; i9++)
      {
        vecHole22Knots.Add(arrHole22LoopKnots[i9]);
      }
      NurbCurve3d pHole22LoopCurve = new NurbCurve3d(degree, vecHole22Knots, controlPoints5, arrWeights, periodic);
      /********************************************************************/
      /* Create arrays - first is a trimming-loop, next - are holes       */
      /********************************************************************/
      Geometry::NurbCurve3d[] arrCurvesTrimmingLoop = new Geometry::NurbCurve3d[1];
      Geometry::NurbCurve3d[] arrCurvesHole1 = new Geometry::NurbCurve3d[1];
      Geometry::NurbCurve3d[] arrCurvesHole2 = new Geometry::NurbCurve3d[2];
      arrCurvesTrimmingLoop[0] = pTrimmigLoopCurve;
      arrCurvesHole1[0] = pHole1LoopCurve;
      arrCurvesHole2[0] = pHole21LoopCurve;
      arrCurvesHole2[1] = pHole22LoopCurve;
      arrLoops[0] = arrCurvesTrimmingLoop;
      arrLoops[1] = arrCurvesHole1;
      arrLoops[2] = arrCurvesHole2;

      /********************************************************************/
      /* Create BS2_Curves (curves projection in surface UV parameters)   */
      /********************************************************************/
      Point2dCollection controlPoints2d = new Point2dCollection();
      for (int j1 = 0; j1 < arrTrimmingLoopProj.Length; j1++)
      {
        controlPoints2d.Add(arrTrimmingLoopProj[j1]);
      }
      NurbCurve2d pPojTrimmigLoopCurve = new NurbCurve2d(degree, vecTrimmingLoopKnots, controlPoints2d, arrWeights, periodic);
      /********************************************************************/
      /* Create hole1 loop projection                                     */
      /********************************************************************/
      Point2dCollection controlPoints2d1 = new Point2dCollection();
      for (int j2 = 0; j2 < arrHole1LoopProjCPS.Length; j2++)
      {
        controlPoints2d1.Add(arrHole1LoopProjCPS[j2]);
      }
      NurbCurve2d pPojHole1LoopCurve = new NurbCurve2d(degree, vecHole1Knots, controlPoints2d1, arrWeights, periodic);
      /********************************************************************/
      /* Create hole2 loop projection                                     */
      /********************************************************************/
      Point2dCollection controlPoints2d2 = new Point2dCollection();
      for (int j4 = 0; j4 < arrHole21LoopProjCPS.Length; j4++)
      {
        controlPoints2d2.Add(arrHole21LoopProjCPS[j4]);
      }
      NurbCurve2d pPojHole21LoopCurve = new NurbCurve2d(degree, vecHole21Knots, controlPoints2d2, arrWeights, periodic);

      Point2dCollection controlPoints2d3 = new Point2dCollection();
      for (int j3 = 0; j3 < arrHole22LoopProjCPS.Length; j3++)
      {
        controlPoints2d3.Add(arrHole22LoopProjCPS[j3]);
      }
      NurbCurve2d pPojHole22LoopCurve = new NurbCurve2d(degree, vecHole22Knots, controlPoints2d3, arrWeights, periodic);
      /********************************************************************/
      /* Create projection curves on UV parameters of NURBS surface       */
      /********************************************************************/
      Geometry::NurbCurve2d[] arrTrimmingLoopProjCurve2d = new Geometry::NurbCurve2d[1];
      Geometry::NurbCurve2d[] arrCurvesProjHole1 = new Geometry::NurbCurve2d[1];
      Geometry::NurbCurve2d[] arrCurvesProjHole2 = new Geometry::NurbCurve2d[2];
      arrTrimmingLoopProjCurve2d[0] = pPojTrimmigLoopCurve;
      arrCurvesProjHole1[0] = pPojHole1LoopCurve;
      arrCurvesProjHole2[0] = pPojHole21LoopCurve;
      arrCurvesProjHole2[1] = pPojHole22LoopCurve;
      arrLoopsProj[0] = arrTrimmingLoopProjCurve2d;
      arrLoopsProj[1] = arrCurvesProjHole1;
      arrLoopsProj[2] = arrCurvesProjHole2;
      return nsf;
    }
  }
}