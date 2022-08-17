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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Code sample below illustrates field evaluation
// Sample operates with a file FieldTest.dwg (copied to the target forlder on a post-build event)
// in FieldTest.dwg object with id 1F4 references object with id 1EF.
// To make evaluation work you should perform the following steps:
// 1. Add OdaX.SxS.manifest to project references - added
// 2. Ensure that post-build event does not influence application manifest (it doesn't for EvaluationExample) - does not
// 3. Mark Main with [STAThread] attribute - Console application's Main by default is CoInited as MTA (Multi-Threaded Apartment thread). 
//    At the same time the threading model of necessary COM object (implicitly created in evaluation procedure) is single-threaded.
//    To avoid this mismatch we should explicitly mark Main method with [STAThread] attribute - marked
// 4. Load Ecaluator module - done, line 57: OdRxModule evalMod = Globals.odrxDynamicLinker().loadModule("ExFieldEvaluator");
// 5. actually call evaluation - done, line 81: TD_Db.oddbEvaluateFields(db, (int)OdDbField.EvalContext.kDemand);
// If any of the steps above is ignored - fields won't be evaluated
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace EvaluationExample
{
  class Program
  {
    [STAThread]  // step 3
    static void Main(string[] args)
    {
        var systemServices = new ExSystemServices();//initialize Teigha services
        TD_Db.odInitialize(systemServices);
        var hostAppServices = new ExHostAppServices();
        OdRxModule evalMod = Globals.odrxDynamicLinker().loadModule("ExFieldEvaluator");// step 4 - loading field evaluator module
        hostAppServices.disableOutput(true); // disable console output
        var db = hostAppServices.readFile("FieldTest.dwg", true, false, FileShareMode.kShareDenyNo, "");

        var text = (OdDbText)db.getOdDbObjectId(new OdDbHandle("1EF")).safeOpenObject(OpenMode.kForWrite);
        var sourceText = text.textString();  // --> "1EF - initial value"
        Console.WriteLine("1EF sourceText = " + sourceText);
        text.setTextString("222");
        var changedText = text.textString(); // --> "222 - new value"
        Console.WriteLine("1EF changedText = " + changedText);
        text.downgradeOpen();

        Console.WriteLine("Before evaluate");
        var textField1 = (OdDbField)db.getOdDbObjectId(new OdDbHandle("1F4")).safeOpenObject(OpenMode.kForWrite);
        var textFieldValue1 = textField1.getValue();  // --> it should be "1EF begore evaluate"
        Console.WriteLine("1F4 textFieldValue = " + textFieldValue1);

        var child1 = textField1.getChild(0, OpenMode.kForWrite);
        var childValue1 = child1.getValue();    // --> it should be "1EF before evaluate"
        Console.WriteLine("1F4 childValue = " + childValue1);
        child1.downgradeOpen();
        textField1.downgradeOpen();
        
        // evaluate
        TD_Db.oddbEvaluateFields(db, (int)OdDbField.EvalContext.kDemand); // step 5

        Console.WriteLine("After evaluate");
        textField1 = (OdDbField)db.getOdDbObjectId(new OdDbHandle("1F4")).safeOpenObject(OpenMode.kForWrite);
        textFieldValue1 = textField1.getValue();  // --> it should be "222 after evaluate"
        Console.WriteLine("1F4 textFieldValue = " + textFieldValue1);

        child1 = textField1.getChild(0, OpenMode.kForWrite);
        childValue1 = child1.getValue();    // --> it should be "222 after evaluate"
        Console.WriteLine("1F4 childValue = " + childValue1);
        child1.downgradeOpen();
        textField1.downgradeOpen();
    }
  }
}
