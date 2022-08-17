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
using Teigha;
using Teigha.DatabaseServices;
using Microsoft.Win32;

namespace OdaMgdMViewApp
{
  class HostAppServ : HostApplicationServices
  {
    public HostAppServ()
    {
    }
       
    public String FindConfigPath(String configType)
    {
      String subkey = GetRegistryAcadProfilesKey();
      if (subkey.Length > 0)
      {
        subkey += String.Format("\\General");
        String searchPath;
        if( GetRegistryString( Registry.CurrentUser, subkey, configType, out searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    private String FindConfigFile(String configType, String file)
    {
      String searchPath = FindConfigPath( configType );
      if (searchPath.Length > 0)
      {
        searchPath = String.Format("{0}\\{1}", searchPath, file);
        if (System.IO.File.Exists(searchPath))
          return searchPath;
      }
      return String.Format("");
    }

    public override String FindFile(String file, Database db, FindFileHint hint)
    {
      String sFile = this.FindFileEx(file, db, hint);
      if (sFile.Length > 0)
        return sFile;

      String strFileName = file;
      String ext;
      if (strFileName.Length>3)
        ext = strFileName.Substring(strFileName.Length - 4, 4).ToUpper();
      else
        ext = file.ToUpper();
      if ( ext == String.Format(".PC3") )
        return FindConfigFile( String.Format("PrinterConfigDir"), file);
      if ( ext == String.Format(".STB") || ext == String.Format(".CTB"))
        return FindConfigFile( String.Format("PrinterStyleSheetDir"), file);
      if ( ext == String.Format(".PMP"))
        return FindConfigFile( String.Format("PrinterDescDir"), file);

      switch (hint)
      {
        case FindFileHint.FontFile:
        case FindFileHint.CompiledShapeFile:
        case FindFileHint.TrueTypeFontFile:
        case FindFileHint.PatternFile:
        case FindFileHint.FontMapFile:
        case FindFileHint.TextureMapFile:
          break;
        default:
          return sFile;
      }

      if ( hint != FindFileHint.TextureMapFile && ext != String.Format(".SHX") && ext != String.Format(".PAT") && ext != String.Format(".TTF") && ext != String.Format(".TTC"))
      {
        strFileName += String.Format(".shx");
      }
      else if (hint == FindFileHint.TextureMapFile)
      {
        strFileName.Replace(String.Format("/"), String.Format("\\"));
        int last = strFileName.LastIndexOf("\\");
        if (last == -1)
          strFileName = "";
        else
          strFileName = strFileName.Substring(0, last);
      }


      sFile = (hint != FindFileHint.TextureMapFile) ? GetRegistryACADFromProfile() : GetRegistryAVEMAPSFromProfile();
      while (sFile.Length>0)
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

      if (hint == FindFileHint.TextureMapFile)
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

    public override bool GetPassword(String fileName, PasswordOptions options, out String pass)
    {
      //PasswordDlg pwdDlg = new PasswordDlg();
      /*pwdDlg.TextFileName.Text = fileName;
      if (pwdDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        pass = pwdDlg.password.Text;
        return true;
      }*/
      pass = String.Format("");
      return false;
    }

    public override String FontMapFileName
    {
      get
      {
        String subkey = GetRegistryAcadProfilesKey();
        if (subkey.Length > 0)
        {
          subkey += String.Format("\\Editor Configuration");
          String fontMapFile;
          if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("FontMappingFile"), out fontMapFile))
            return fontMapFile;
        }
        return String.Format("");
      }
    }

    bool GetRegistryString( RegistryKey rKey, String subkey, String name, out String value)
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
      if ( subkey.Length > 0 )
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
      String subkey     = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
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
      String subkey     = String.Format("SOFTWARE\\Autodesk\\AutoCAD");
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

    String GetRegistryACADFromProfile()
    {
      String subkey = GetRegistryAcadProfilesKey();
      if ( subkey.Length > 0 )
      {
        subkey += String.Format("\\General");
        // get the value for the ACAD entry in the registry
        String tmp;
        if (GetRegistryString(Registry.CurrentUser, subkey, String.Format("ACAD"), out tmp))
          return tmp;
      }
      return String.Format("");
    }
  };
}
