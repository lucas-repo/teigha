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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using Teigha.Runtime;

namespace DevGuideExamples
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    void RunAllForAssembly(Assembly asm, String dirName)
    {
      Type[] allTypes = asm.GetExportedTypes();
      object[] args = new object[1];
      args[0] = dirName;
      foreach (Type type in allTypes)
      {
        object tp = type.InvokeMember(null, BindingFlags.CreateInstance, null, type, args);
      }

    }

    protected override void OnStartup(StartupEventArgs e)
    {
      using(Services svcs = new Services())
      {
        base.OnStartup(e);
        if (e.Args.Length > 0)
        {
          RunAllForAssembly(typeof(CDevGuideExamplesProject.MTextEx).Assembly, e.Args[0]);
          RunAllForAssembly(typeof(VBNETDevGuideExamplesProject.MTextEx).Assembly, e.Args[0]);
        }
        else
        {
          new Window1().ShowDialog();
        }
      }
      this.Shutdown();
    }
  }
}
