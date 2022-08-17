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

namespace OdaDgnAppMgd
{
    public partial class Export2STLDlg : Form
    {
        #region private variables
        private String pFileName = String.Empty;
        private UInt64 pHandleId = 0;
        private bool pBinary = true;
        #endregion
        #region public variables
        public String FileName
        {
            get { return pFileName; }
        }
        public UInt64 HandleId
        {
            get { return pHandleId; }
        }
        public bool Binary
        {
            get { return pBinary; }
        }
        #endregion
        public Export2STLDlg()
        {
            InitializeComponent();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            pFileName = StlFilePathString.Text;
            pHandleId = Decimal.ToUInt64(ElementHandle.Value);
            pBinary = BinaryFormat.Checked;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveStlDialog = new SaveFileDialog();
            saveStlDialog.Filter = "STL files|*.stl";
            if (saveStlDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            StlFilePathString.Text = saveStlDialog.FileName;
            Export.Enabled = true;
        }
    }
}
