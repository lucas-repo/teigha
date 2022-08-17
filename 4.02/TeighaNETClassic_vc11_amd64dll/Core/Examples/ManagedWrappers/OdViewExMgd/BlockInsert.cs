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
using System.Text;
using System.Windows.Forms;

using Teigha;
using Teigha.DatabaseServices;
using Teigha.Geometry;

namespace OdViewExMgd
{
  public partial class BlockInsert : Form
  {
    BlockTable blTable;
    public BlockInsert(BlockTable bt)
    {
      InitializeComponent();
      blTable = bt;

      foreach (ObjectId id in bt)
      {
        using(BlockTableRecord pRec = (BlockTableRecord)id.GetObject(OpenMode.ForRead))
        {
          String name = pRec.Name;
          if (name.IndexOf("*") != 0)
          {
            comboBoxBlocks.Items.Add(name);
            if (null == comboBoxBlocks.SelectedItem)
            {
              comboBoxBlocks.SelectedItem = name;
            }
          }
        }
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      ObjectId objId = blTable[comboBoxBlocks.SelectedItem.ToString()];
      using(Database db = blTable.Database)
      {
        using (BlockTableRecord btr = (BlockTableRecord)db.CurrentSpaceId.GetObject(OpenMode.ForWrite))
        {
          Point3d ptIns = new Point3d(Convert.ToDouble(textBoxInsPointX.Text), Convert.ToDouble(textBoxInsPointY.Text), Convert.ToDouble(textBoxInsPointZ.Text));
          Matrix3d blkXfm = Matrix3d.Displacement(ptIns.GetAsVector());

          BlockReference bref_ins = new BlockReference(Point3d.Origin, objId);
          using (BlockReference BRef = (BlockReference)btr.AppendEntity(bref_ins).GetObject(OpenMode.ForWrite))
          {
            BRef.ScaleFactors = new Scale3d(Convert.ToDouble(textBoxScaleX.Text), Convert.ToDouble(textBoxScaleY.Text), Convert.ToDouble(textBoxScaleZ.Text));
          }
          bref_ins.Dispose();
        }
      }
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}