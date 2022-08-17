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
using System.Linq;
using System.Text;

using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.Geometry;


namespace CDevGuideExamplesProject
{
  public class BlockEx
  {
    const String path1 = "BlockEx1.dwg";

    public BlockEx(string path)
    {
      using (Database db = new Database(true, true))
      {
        TransactionManager tm = db.TransactionManager;
        using (Transaction ta = tm.StartTransaction())
        {
          using (BlockTable bt = (BlockTable)ta.GetObject(db.BlockTableId, OpenMode.ForWrite))
          {
            // Get the modelspace block
            using (BlockTableRecord modelSpace = (BlockTableRecord)tm.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite))
            {
              // Create a new database to be attached as external reference  
              CreateDatabase(path);
              // Attach the created database as an exernal reference to the current database
              ObjectId xRefId = db.AttachXref(path1, "ExternalBlock");

              // Create a BlockReference object that refer to the external block and add it to the modelspace block
              using (BlockTableRecord xBlock = (BlockTableRecord)xRefId.GetObject(OpenMode.ForWrite))
              {
                // Reload xref
                ObjectIdCollection xrefCollection = new ObjectIdCollection();
                xrefCollection.Add(xBlock.ObjectId);
                db.ReloadXrefs(xrefCollection);
                // Create a block reference and append it to modelspace
                BlockReference xBlockRef = new BlockReference(Point3d.Origin, xRefId);
                modelSpace.AppendEntity(xBlockRef);
              }
            }
          }
          ta.Commit();
        }
        db.SaveAs(path + "BlockEx2.dwg", DwgVersion.Current);
      }
    }

    // Create a new database, add new block definition and block reference objects. This database will be attached as an external reference.  
    static void CreateDatabase(String path)
    {
      Database db = new Database(true, true);
      TransactionManager tm = db.TransactionManager;
      using (Transaction ta = tm.StartTransaction())
      {
        using (BlockTableRecord modelSpace = (BlockTableRecord)ta.GetObject(db.CurrentSpaceId, OpenMode.ForWrite))
        {
          using (BlockTable bt = (BlockTable)ta.GetObject(db.BlockTableId, OpenMode.ForWrite))
          {
            using (BlockTableRecord blk1 = new BlockTableRecord())
            {
              blk1.Name = "Block1";
              bt.Add(blk1);
              blk1.Origin = new Point3d(0, 5, 0);

              // Add some entities to the block definition blk1
              Circle cir1 = new Circle();
              blk1.AppendEntity(cir1);
              cir1.Radius = 3;

              Circle cir2 = new Circle();
              cir2.Thickness = 1;
              blk1.AppendEntity(cir2);
              cir2.Radius = 5;
			  
              Line line = new Line(new Point3d(0, -6, 0), new Point3d(0, 6, 0));
              blk1.AppendEntity(line);

              // Create a block reference for the block definition blk1, set additional properties and explode this block reference
              using (BlockReference blockRef = new BlockReference(Point3d.Origin, blk1.ObjectId))
              {
                modelSpace.AppendEntity(blockRef);
                blockRef.ScaleFactors = new Scale3d(2, 2, 2);
                blockRef.Rotation = 0.7;
                blockRef.Position = new Point3d(5, 0, 0);
                try
                {
                  blockRef.ExplodeToOwnerSpace();
                }
                catch (Teigha.Runtime.Exception ex)
                {                  
                  System.Console.WriteLine("The block cannot be exploded");
                  System.Console.WriteLine("{0}", ex.Message);
                }
              }
            }
          }
        }
        ta.Commit();
      }
      db.SaveAs(path + path1, DwgVersion.Current);
      db.Dispose();
    }
  }
}
