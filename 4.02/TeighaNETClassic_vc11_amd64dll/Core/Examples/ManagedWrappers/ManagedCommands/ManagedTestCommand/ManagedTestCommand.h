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

// ManagedTestCommand.h

#pragma once

using namespace System;
using namespace System::Collections;
using namespace Teigha::Runtime;
using namespace Teigha::Geometry;
using namespace Teigha::DatabaseServices;

/*
To test the command:
 Launch OdaMfcApp C++ sample, 
 Create new file (or open existing)
 Load TD_Mgd_*.dll, using Tools/Load App dialog,
 Invoke command prompt via Shift+C
 Type 'NETLOAD', and browse to ManagedTestCommand.tx
 Type AddSomeText at the command prompt
 (To debug managed code debugger type should be set 
   to Mixed or Managed only, in project properties/Debugging tab)
*/
namespace ManagedTestCommand 
{
	public ref class Class1
	{
  public:
    [CommandMethod("MANAGED_COMMANDS","AddSomeText",CommandFlags::Modal)]
    void run()
    {
      Database^ db = HostApplicationServices::WorkingDatabase;
      Transaction^ tr = db->TransactionManager->StartTransaction();
      try
      {
        DBText text;
        text.TextString = "Created using Teigha®.NET for .dwg files";
        text.Height = 0.2;
        text.Position = Point3d(0,0,0);
        BlockTable^ bt = (BlockTable^)db->BlockTableId.GetObject(OpenMode::ForRead);
        BlockTableRecord^ ms = (BlockTableRecord^)bt[BlockTableRecord::ModelSpace].GetObject(OpenMode::ForWrite);
        ms->AppendEntity(%text);
        tr->Commit();
      }
      finally
      {
        delete tr;
      }
    }
	};
}
