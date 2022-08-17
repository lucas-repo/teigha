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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Teigha;
using Teigha.Runtime;
using Teigha.DatabaseServices;

namespace ASP_example
{
  public partial class _Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      String path = AppDomain.CurrentDomain.BaseDirectory;
      String strPath = Environment.GetEnvironmentVariable("PATH");
      Environment.SetEnvironmentVariable("PATH", path + "Teigha_libs\\" + ";" + strPath);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
      using (Services serv = new Services())
      {
        using (Database pDatabase = new Database(false, false))
        {
          string path = AppDomain.CurrentDomain.BaseDirectory;
          String file = path + @"testFile.dwg";
          pDatabase.ReadDwgFile(file, System.IO.FileShare.Write, true, null);
          TextBox1.Text = "File " + file + " has been opened.";
        }
      }
    }
  }
}
