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
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Teigha.Core;
using Teigha.TG;
using Teigha.TD;

namespace DwgViewer
{
  public partial class Form1 : Form
  {
    OdDbDatabase TDDatabase = null;
    OdDgDatabase TGDatabase = null;

    List<UserControl1> Views = new List<UserControl1>();
    UserControl1 viewCtrl = null;

    ExSystemServices Serv = null;
    //ExHostAppServices HostApp = null;
    CustomServices HostApp = null;
    OdDgHostAppServices TgHostApp = null;
    OdRxSystemServices TgSysSrv = null;

    public Form1()
    {
      InitializeComponent();
      Serv = new ExSystemServices();
      //HostApp = new ExHostAppServices();
      HostApp = new CustomServices();
      TD_Db.odInitialize(Serv);
      Environment.SetEnvironmentVariable("DDPLOTSTYLEPATHS", HostApp.FindConfigPath(String.Format("PrinterStyleSheetDir")));

      TgSysSrv = new OdExDgnSystemServices();
      Teigha.Core.Globals.odrxInitialize(TgSysSrv);
      Teigha.Core.Globals.odgsInitialize();
      Teigha.Core.Globals.odrxDynamicLinker().loadModule("TG_Db");

      TgHostApp = new OdExDgnHostAppServices();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      openFileDialog1.Filter = "DWG Files (*.dwg)|*.dwg";
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        DeleteViews();

        CreateViews(true, openFileDialog1.FileName);
      }
    }

    private void CreateViews(bool DWG, string fName)
    {
      if (DWG)
      {
        //OdDbDatabase td_db = HostApp.readFile(fName);
        TDDatabase = HostApp.readFile(fName);
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.ColumnCount = 1;
        viewCtrl = new UserControl1();
        viewCtrl.TDDatabase = TDDatabase;//td_db;
        viewCtrl.ResetDevice(DWG);
        viewCtrl.Dock = DockStyle.Fill;
        viewCtrl.Margin = new Padding(1);
        tableLayoutPanel1.Controls.Add(viewCtrl);
      }
      else
      {
        OdDgDatabase tg_db = TgHostApp.readFile(fName);
        OdDgViewGroup pViewGroup = (OdDgViewGroup)tg_db.getActiveViewGroupId().openObject();
        if (pViewGroup == null)
        {
          pViewGroup = (OdDgViewGroup)TGDatabase.recommendActiveViewGroupId().openObject();
          if (pViewGroup == null)
            return;
        }
        OdDgElementIterator pIt = pViewGroup.createIterator();
        for (; !pIt.done(); pIt.step())
        {
          OdDgView pView = OdDgView.cast(pIt.item().openObject());
          if (pView != null && pView.getVisibleFlag())
          {
            viewCtrl = new UserControl1();
            viewCtrl.TGDatabase = tg_db;
            viewCtrl.TGVectorizedModelId = tg_db.getActiveModelId();
            if (viewCtrl.TGVectorizedModelId.isNull())
            {
              viewCtrl.TGVectorizedModelId = tg_db.getDefaultModelId();
              tg_db.setActiveModelId(viewCtrl.TGVectorizedModelId);
            }
            viewCtrl.TGVectorizedViewId = pIt.item();
            viewCtrl.ResetDevice(DWG);
            Views.Add(viewCtrl);
          }
        }
        tableLayoutPanel1.RowCount = (int)Math.Round(Math.Sqrt(Views.Count));
        tableLayoutPanel1.ColumnCount = (int)Math.Round(Views.Count / (float)tableLayoutPanel1.RowCount);
        foreach (UserControl1 view in Views)
        {
          view.Dock = DockStyle.Fill;
          view.Margin = new Padding(1);
          tableLayoutPanel1.Controls.Add(view);
        }
      }
    }

    private void DeleteViews()
    {
      foreach (UserControl1 view in Views)
      {
        view.DeleteContext();
      }

      tableLayoutPanel1.Controls.Clear();

      Views.Clear();
    }

    private void openDGNToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      openFileDialog1.Filter = "DGN Files (*.dgn)|*.dgn";
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        DeleteViews();
        CreateViews(false, openFileDialog1.FileName);
      }
    }

    private void addOrbitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      viewCtrl.AddOrbit(true);
    }

    private void addCommandToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CustomCommand nCommand = new CustomCommand();
      nCommand.SetMessage("new message");
      OdEdCommandStack pStack = Globals.odedRegCmds();
      pStack.addCommand(nCommand);
    }

    private void executeCommandToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // three variants of command call illustrated
        // 1 - SVG export with a ready to use command string
        // 2 - custom command
        // 3 - BMP export with command string based on user input


      ///////// SVG export command with ready to use command string - string created in ExStringIO.create("svgout 1 6 \n\n.png sans-serif 768 1024 Yes Yes\n"); line
      //ExStringIO strIO = ExStringIO.create("svgout 1 6 \n\n.png sans-serif 768 1024 Yes Yes\n");
      //OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, TDDatabase);
      //OdEdCommandStack pStack = Globals.odedRegCmds();
      //OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
      //if (null == pModule)
      //{
      //  MessageBox.Show("TD_SvgExport.tx is missing");
      //  return;
      //}
      //pStack.executeCommand("SVGOUT", pCon);
      //////// SVG export command finish ///////


      ///////////// custom command - before you select Execute command menu item you shold call select Add command
      // Add command menu item handler adds a simple custom command, implemented in class CustomCommand
      // custom command does not require command string, thus String.Empty may be used
      //ExStringIO strIO = ExStringIO.create(String.Empty);
      //OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, TDDatabase);
      //OdEdCommandContext pCont = OdEdCommandContext.cast(pCon);
      //OdEdCommandStack pStack = Globals.odedRegCmds();
      //pStack.executeCommand("custom", pCont);
      //////// Custom command finish ///////

      //////// BMP export command - command string being input parameter by parameter in a special form ////
      /// form for parameter input invoked in getString method od CustomIO class
      OdEdBaseIO strIO = new CustomIO();
      OdDbCommandContext pCon = ExDbCommandContext.createObject(strIO, TDDatabase);
      OdEdCommandStack pStack = Globals.odedRegCmds();
      OdGsModule pModule = OdGsModule.cast(Globals.odrxDynamicLinker().loadModule("TD_RasterExport"));
      try
      {
          pStack.executeCommand("bmpoutbg", pCon);
      }
      catch(Exception ex)
      {
          // we should add the catch block as for instance Cancel button click in Save File Dialog will invoke an exception
          MessageBox.Show("Execute command failed " + ex.Message);
      }
      //////// BMP export command finish ///////
    }

  }

  public class CustomIO : OdEdBaseIO
  {
      public override string getString(string prompt, int options, OdEdStringTracker pTracker)
      {
          //MessageBox.Show("getString 3: " + prompt);
          //return base.getString(prompt, options, pTracker);
          FormPrompt frm = new FormPrompt();
          frm.setPrompt(prompt);
          frm.ShowDialog();
          return frm.getValue();
      }
      public override void putError(string errmsg)
      {
          MessageBox.Show("putError overridden");
          base.putError(errmsg);
      } 
  }
  public class CustomServices : ExHostAppServices
  {
    public override string fileDialog(int flags, string dialogCaption, string defExt, string defFilename, string filter)
    {
      SaveFileDialog dlg = new SaveFileDialog();
      dlg.DefaultExt = defExt;
      dlg.Title = dialogCaption;
      dlg.FileName = defFilename;
      dlg.Filter = filter;
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        return dlg.FileName;
      }
      //throw new OdEdCancel();
      return String.Empty;
    }



    bool GetRegistryString(RegistryKey rKey, String subkey, String name, out String value)
    {
      bool rv = false;
      object objData = null;

      RegistryKey regKey;
      regKey = rKey.OpenSubKey(subkey);
      if (regKey != null)
      {
        objData = regKey.GetValue(name);
        if (objData != null)
        {
          rv = true;
        }
        regKey.Close();
      }
      if (rv)
        value = objData.ToString();
      else
        value = String.Format("");

      rKey.Close();
      return rv;
    }

    String GetRegistryAVEMAPSFromProfile()
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        // get the value for the ACAD entry in the registry
        String tmp;
        if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("AVEMAPS"), out tmp))
          return tmp;
      }
      return String.Format("");
    }

    String GetRegistryAcadProfilesKey()
    {
      String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
      String tmp;

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}\\Profiles", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);
      return subkey;
    }

    String GetRegistryAcadLocation()
    {
      String subkey = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
      String tmp;

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format("CurVer"), out tmp))
        return String.Format("");
      subkey += String.Format("\\{0}", tmp);

      if (!GetRegistryString(Registry.CurrentUser, subkey, String.Format(""), out tmp))
        return String.Format("");
      return tmp;
    }

    public String FindConfigPath(String configType)
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        String searchPath;
        if (GetRegistryString(Registry.CurrentUser, subkey, configType, out searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    private String FindConfigFile(String configType, String file)
    {
      String searchPath = FindConfigPath(configType);
      if (searchPath.Length > 0)
      {
        searchPath = String.Format("{0}\\{1}", searchPath, file);
        if (System.IO.File.Exists(searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    String GetRegistryACADFromProfile()
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        // get the value for the ACAD entry in the registry
        String tmp;
        if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("ACAD"), out tmp))
          return tmp;
      }
      return String.Format("");
    }

    public override String findFile(String fileName, OdRxObject pDb, OdDbBaseHostAppServices.FindFileHint hint)
    {
      String sFile = base.findFile(fileName, pDb, hint);
      if (sFile.Length > 0)
        return sFile;

      String strFileName = fileName;
      String ext;
      if (strFileName.Length > 3)
        ext = strFileName.Substring(strFileName.Length - 4, 4).ToUpper();
      else
        ext = fileName.ToUpper();
      if (ext == String.Format(".PC3"))
        return FindConfigFile(String.Format("PrinterConfigDir"), fileName);
      if (ext == String.Format(".STB") || ext == String.Format(".CTB"))
        return FindConfigFile(String.Format("PrinterStyleSheetDir"), fileName);
      if (ext == String.Format(".PMP"))
        return FindConfigFile(String.Format("PrinterDescDir"), fileName);

      switch (hint)
      {
        case FindFileHint.kFontFile:
        case FindFileHint.kCompiledShapeFile:
        case FindFileHint.kTrueTypeFontFile:
        case FindFileHint.kPatternFile:
        case FindFileHint.kFontMapFile:
        case FindFileHint.kTextureMapFile:
          break;
        default:
          return sFile;
      }

      if (hint != FindFileHint.kTextureMapFile && ext != String.Format(".SHX") && ext != String.Format(".PAT") && ext != String.Format(".TTF") && ext != String.Format(".TTC"))
      {
        strFileName += String.Format(".shx");
      }
      else if (hint == FindFileHint.kTextureMapFile)
      {
        strFileName.Replace(String.Format("/"), String.Format("\\"));
        int last = strFileName.LastIndexOf("\\");
        if (last == -1)
          strFileName = "";
        else
          strFileName = strFileName.Substring(0, last);
      }


      sFile = (hint != FindFileHint.kTextureMapFile) ? GetRegistryACADFromProfile() : GetRegistryAVEMAPSFromProfile();
      while (sFile.Length > 0)
      {
        int nFindStr = sFile.IndexOf(";");
        String sPath;
        if (-1 == nFindStr)
        {
          sPath = sFile;
          sFile = String.Format("");
        }
        else
        {
          sPath = String.Format("{0}\\{1}", sFile.Substring(0, nFindStr), strFileName);
          if (System.IO.File.Exists(sPath))
          {
            return sPath;
          }
          sFile = sFile.Substring(nFindStr + 1, sFile.Length - nFindStr - 1);
        }
      }

      if (hint == FindFileHint.kTextureMapFile)
      {
        return sFile;
      }

      if (sFile.Length <= 0)
      {
        String sAcadLocation = GetRegistryAcadLocation();
        if (sAcadLocation.Length > 0)
        {
          sFile = String.Format("{0}\\Fonts\\{1}", sAcadLocation, strFileName);
          if (System.IO.File.Exists(sFile))
          {
            sFile = String.Format("{0}\\Support\\{1}", sAcadLocation, strFileName);
            if (System.IO.File.Exists(sFile))
            {
              sFile = String.Format("");
            }
          }
        }
      }
      return sFile;
    }

  }
}
