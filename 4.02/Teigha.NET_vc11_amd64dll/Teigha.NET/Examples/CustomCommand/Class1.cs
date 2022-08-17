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
using System.Windows.Forms;

using Teigha.Core;
using Teigha.TD;
using Teigha.TG;
namespace CustomCommand
{
  public class CustomModule
  {
    public static void initApp()
    {
      CustomCommand nCommand = new CustomCommand();
      OdEdCommandStack pStack = Globals.odedRegCmds();
      pStack.addCommand(nCommand);
    }
    public static void uninitApp()
    {
      OdEdCommandStack pStack = Globals.odedRegCmds();
      OdEdCommand pCmd = pStack.lookupCmd("custom");
      if (null != pCmd)
      {
        pStack.removeCmd("MESSAGE", "custom");
      }
    }
  }
  public class CustomCommandClass : OdRxClass
  {
    public override string name()
    {
      return "CustomCommand";
    }
    public override OdRxClass myParent()
    {
      return OdEdCommand.desc();
    }
  }
  public class CustomCommand : OdEdCommand
  {
    public override string groupName()
    {
      return "MESSAGE";
    }
    public override string globalName()
    {
      return "custom";
    }
    public override void execute(OdEdCommandContext pCommandContext)
    {
      OdRxClass desc = pCommandContext.baseDatabase().isA();
      string name = desc.name();
      string companyName = string.Empty;
      if ("OdDgDatabase" == name)
      {
        OdDgDatabase pDat = OdDgDatabase.cast(pCommandContext.baseDatabase());
        companyName = pDat.appServices().companyName();
      }
      else
      {
        OdDbDatabase pDat = OdDbDatabase.cast(pCommandContext.baseDatabase());
        companyName = pDat.appServices().companyName();
      }
      MessageBox.Show("Custom message: company name: " + companyName);
    }
    public override Teigha.Core.OdRxClass isA()
    {
      return new CustomCommandClass();
    }
  }
}
