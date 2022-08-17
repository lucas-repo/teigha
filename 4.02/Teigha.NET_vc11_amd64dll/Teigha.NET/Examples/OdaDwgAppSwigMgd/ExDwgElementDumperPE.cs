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
using Teigha.Core;
using Teigha.TD;
using System.Collections.Generic;
using System.Diagnostics;

//----------------------------------------------------------
//
// OdDbDumper
//
//----------------------------------------------------------
class OdDbDumper
{
    public OdDbDumper()
    { 
    }
}

abstract class OdExDwgDumper
{
    public abstract void dumpFieldName(string fieldName);
    public abstract void dumpFieldValue(string fieldName);

    // output for different types
    public void writeFieldValue(string name, string value)
    {
        dumpFieldName(name);
        dumpFieldValue(value);
    }
    public void writeFieldValue(string name, UInt16 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValueHex(string name, UInt16 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString("X"));
    }
    public void writeFieldValue(string name, UInt32 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValueHex(string name, UInt32 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString("X"));
    }
    public void writeFieldValue(string name, double value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, OdGePoint2d value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, OdGePoint3d value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, OdGeVector3d value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, OdCmEntityColor value)
    {
        dumpFieldName(name);
        dumpFieldValue(String.Format("R: {0} G: {1} B: {2}", value.red(), value.green(), value.blue()));

    }
    //  public void writeFieldValue(string name, OdDbLineStyleInfo value)
    //  {
    //    dumpFieldName(name);
    //    dumpFieldValue(String.Format("Modifiers: {0,8:X} Scale: {1} Shift: {2} Start width: {3} End width: {4}",
    //      value.getModifiers(), value.getScale(), value.getShift(), value.getStartWidth(), value.getEndWidth()));
    //  }
    public void writeFieldValue(string name, UInt64 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValueHex(string name, UInt64 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString("X"));
    }
    public void writeFieldValue(string name, byte value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValueHex(string name, byte value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString("X"));
    }
    //  public void writeFieldValue(string name, OdDgLightColor value)
    //  {
    //    dumpFieldName(name);
    //    dumpFieldValue(String.Format("R: {0} G: {1} B: {2} Intensity: {3}",
    //      value.getRed(), value.getGreen(), value.getBlue(), value.getIntensityScale()));
    //  }
    public void writeFieldValue(string name, bool value)
    {
        dumpFieldName(name);
        dumpFieldValue(value ? "true" : "false");
    }
    //  public void writeFieldValue(string name, OdAngleCoordinate value)
    //  {
    //    dumpFieldName(name);
    //    dumpFieldValue(String.Format("{0}° {1}' {2}\"",
    //      value.getDegrees(), value.getMinutes(), value.getSeconds()));
    //  }
    public void writeFieldValue(string name, Int16 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, Int32 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    public void writeFieldValue(string name, Int64 value)
    {
        dumpFieldName(name);
        dumpFieldValue(value.ToString());
    }
    /*  public void writeFieldValue(string name, OdDgModel.WorkingUnit value)
      {
        dumpFieldName(name);

        String val = "";
        switch (value)
        {
          case OdDgModel.WorkingUnit.kWuUnitOfResolution: val = "unit of resolution"; break;
          case OdDgModel.WorkingUnit.kWuStorageUnit: val = "storage unit"; break;
          case OdDgModel.WorkingUnit.kWuWorldUnit: val = "world unit"; break;
          case OdDgModel.WorkingUnit.kWuMasterUnit: val = "master unit"; break;
          case OdDgModel.WorkingUnit.kWuSubUnit: val = "sub unit"; break;
        }
        dumpFieldValue(val);
      }
     */
    /*
  public void writeFieldValue(string name, TextJustification value)
  {
    dumpFieldName(name);

    String val = "";
    switch (value)
    {
      case TextJustification.kLeftTop: val = "left top"; break;
      case TextJustification.kLeftCenter: val = "left center"; break;
      case TextJustification.kLeftBottom: val = "left bottom"; break;
      case TextJustification.kLeftMarginTop: val = "left margin top"; break;
      case TextJustification.kLeftMarginCenter: val = "left margin center"; break;
      case TextJustification.kLeftMarginBottom: val = "left margin bottom"; break;
      case TextJustification.kCenterTop: val = "center top"; break;
      case TextJustification.kCenterCenter: val = "center center"; break;
      case TextJustification.kCenterBottom: val = "center bottom"; break;
      case TextJustification.kRightMarginTop: val = "right margin top"; break;
      case TextJustification.kRightMarginCenter: val = "right margin center"; break;
      case TextJustification.kRightMarginBottom: val = "right margin bottom"; break;
      case TextJustification.kRightTop: val = "right top"; break;
      case TextJustification.kRightCenter: val = "right center"; break;
      case TextJustification.kRightBottom: val = "right bottom"; break;
    }
    dumpFieldValue(val);
  }
     */
    public void writeFieldValue(string name, OdGeQuaternion value)
    {
        dumpFieldName(name);
        dumpFieldValue(String.Format("{0}; {1}; {2}; {3}", value.w, value.x, value.y, value.z));
    }
    //public void writeFieldValue( string name, TextAttributes value );
    /*public void writeFieldValue(string name, OdDgGraphicsElement.Class value)
    {
      dumpFieldName(name);

      String val = "";
      switch (value)
      {
        case OdDgGraphicsElement.Class.kClassPrimary: val = "Primary"; break;
        case OdDgGraphicsElement.Class.kClassPatternComponent: val = "Pattern component"; break;
        case OdDgGraphicsElement.Class.kClassConstruction: val = "Construction"; break;
        case OdDgGraphicsElement.Class.kClassDimension: val = "Dimension"; break;
        case OdDgGraphicsElement.Class.kClassPrimaryRule: val = "Primary rule"; break;
        case OdDgGraphicsElement.Class.kClassLinearPatterned: val = "Linear patterned"; break;
        case OdDgGraphicsElement.Class.kClassConstructionRule: val = "Construction rule"; break;
      }
      dumpFieldValue(val);
    }
     */
    public void writeFieldValue(string name, OdGeMatrix2d value)
    {
        dumpFieldName(name);
        dumpFieldValue(String.Format("{0}; {1}; {2}; {3}", value[0, 0], value[1, 0], value[0, 1], value[1, 1]));
    }

    //public void writeFieldValue( string name, OdDgDimension.ToolType value );
    /*public void writeFieldValue(string name, OdDgDimPoint value)
    {
      dumpFieldName(name + ":");
      writeFieldValue("  Point", value.getPoint());
      writeFieldValue("  Offset to dimension line", value.getOffsetToDimLine());
      writeFieldValue("  Offset Y", value.getOffsetY());
      writeFieldValue("  Text alignment", value.getJustification());
      dumpFieldName("  Flags:");
      writeFieldValue("    Associative", value.getAssociativeFlag());
      writeFieldValue("    Relative", value.getRelative() != 0);
      writeFieldValue("    WitnessControlLocal", value.getWitnessControlLocalFlag());
      writeFieldValue("    NoWitnessLine", value.getNoWitnessLineFlag());
      writeFieldValue("    UseAltSymbology", value.getUseAltSymbologyFlag());

      if (value.getPrimaryTextFlag())
      {
        writeFieldValue("  Primary text", value.getPrimaryText());
      }

      if (value.getPrimaryTopToleranceTextFlag())
      {
        writeFieldValue("  Primary Top text", value.getPrimaryTopToleranceText());
      }

      if (value.getPrimaryBottomToleranceTextFlag())
      {
        writeFieldValue("  Primary Bottom text", value.getPrimaryBottomToleranceText());
      }

      if (value.getSecondaryTextFlag())
      {
        writeFieldValue("  Secondary text", value.getSecondaryText());
      }

      if (value.getSecondaryTopToleranceTextFlag())
      {
        writeFieldValue("  Secondary Top text", value.getSecondaryTopToleranceText());
      }

      if (value.getSecondaryBottomToleranceTextFlag())
      {
        writeFieldValue("  Secondary Bottom text", value.getSecondaryBottomToleranceText());
      }
    }
     * */
}


///
class OdDbRxObjectDumperPE
{
    public virtual void dump(OdRxObject pObj, OdExDwgDumper pDumper)
  {

  }
    static Dictionary<string, OdDbRxObjectDumperPE> dic = new Dictionary<string, OdDbRxObjectDumperPE>();
    public static OdDbRxObjectDumperPE getDumper(OdRxClass c)
  {
    for (OdRxClass pc = c; pc != null; pc = pc.myParent())
    {
      string name = pc.name();
      OdDbRxObjectDumperPE res;
      if (dic.TryGetValue(name, out res))
        return res;
    }
    return null;
  }
    public static void registerDumper(string name, OdDbRxObjectDumperPE d)
  {
    dic[name] = d;
  }
}
class OdDbObjectDumperPE : OdDbRxObjectDumperPE
{
    public virtual OdDbObjectIterator createIterator(OdDbObject pElm, bool atBeginning, bool skipDeleted)
    {
        return null;
    }
}