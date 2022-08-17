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

using Teigha;
using Teigha.Runtime; // for CommandMethod
using Teigha.DatabaseServices;
using Teigha.Geometry;


namespace Create3dSolids
{
  public class Create3dSolidsClass
	{
    static ObjectId addLayer(Database db, String name)
    {
      TransactionManager tm = db.TransactionManager;
      using (LayerTable layerTable = (LayerTable)tm.GetObject(db.LayerTableId, OpenMode.ForWrite, false))
      {
        using (LayerTableRecord newXRec = new LayerTableRecord())
        {
          newXRec.Name = name;
          return layerTable.Add(newXRec);
        }
      }
    }

    [CommandMethod("MANAGED_COMMANDS", "Create3dSolids", CommandFlags.Modal)]
    public static void run()
    {
      Database db = HostApplicationServices.WorkingDatabase;

      TransactionManager tm = db.TransactionManager;
      using (Transaction ta = tm.StartTransaction())
      {
        using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
        {
          Solid3d solSphere = new Solid3d();
          solSphere.CreateSphere(5);
          btr.AppendEntity(solSphere);
          tm.AddNewlyCreatedDBObject(solSphere, true);
          solSphere.TransformBy(Matrix3d.Displacement(new Vector3d(30, 0, 0)));

          Solid3d solBox = new Solid3d();
          solBox.CreateBox(3, 4, 5);
          btr.AppendEntity(solBox);
          tm.AddNewlyCreatedDBObject(solBox, true);
          solBox.SetLayerId(addLayer(db, "Layer Box"), true);
          solBox.TransformBy(Matrix3d.Displacement(new Vector3d(20, 0, 0)));

          Solid3d solTorus = new Solid3d();
          solTorus.CreateTorus(7, 5);
          btr.AppendEntity(solTorus);
          tm.AddNewlyCreatedDBObject(solTorus, true);
          solTorus.ColorIndex = 5;
          solTorus.SetLayerId(addLayer(db, "Layer Torus"), false);
          solTorus.TransformBy(Matrix3d.Displacement(new Vector3d(10, 0, 0)));

          Solid3d solCone = new Solid3d();
          solCone.CreateFrustum(5, 2, 4, 1);
          btr.AppendEntity(solCone);
          tm.AddNewlyCreatedDBObject(solCone, true);
          solCone.ColorIndex = 7;
          solCone.SetLayerId(addLayer(db, "Layer Cone"), false);
        }
        ta.Commit();        
      }
      db = null;
    }
	}
}
