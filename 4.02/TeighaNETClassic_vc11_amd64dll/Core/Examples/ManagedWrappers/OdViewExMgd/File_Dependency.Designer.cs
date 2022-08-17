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
namespace OdViewExMgd
{
  partial class File_Dependency
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.listView1 = new System.Windows.Forms.ListView();
      this.ColID = new System.Windows.Forms.ColumnHeader();
      this.ColFeature = new System.Windows.Forms.ColumnHeader();
      this.ColFileName = new System.Windows.Forms.ColumnHeader();
      this.ColFoundPath = new System.Windows.Forms.ColumnHeader();
      this.ColFullFileName = new System.Windows.Forms.ColumnHeader();
      this.ColFingerprintGuid = new System.Windows.Forms.ColumnHeader();
      this.ColVersionGuid = new System.Windows.Forms.ColumnHeader();
      this.ColTimeStamp = new System.Windows.Forms.ColumnHeader();
      this.ColFileSize = new System.Windows.Forms.ColumnHeader();
      this.ColAffectsGraphics = new System.Windows.Forms.ColumnHeader();
      this.ColReferenceCount = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColID,
            this.ColFeature,
            this.ColFileName,
            this.ColFoundPath,
            this.ColFullFileName,
            this.ColFingerprintGuid,
            this.ColVersionGuid,
            this.ColTimeStamp,
            this.ColFileSize,
            this.ColAffectsGraphics,
            this.ColReferenceCount});
      this.listView1.Location = new System.Drawing.Point(12, 12);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(493, 438);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // ColID
      // 
      this.ColID.Text = "ID";
      this.ColID.Width = 30;
      // 
      // ColFeature
      // 
      this.ColFeature.Text = "Feature";
      this.ColFeature.Width = 100;
      // 
      // ColFileName
      // 
      this.ColFileName.Text = "File Name";
      this.ColFileName.Width = 100;
      // 
      // ColFoundPath
      // 
      this.ColFoundPath.Text = "Found Path";
      this.ColFoundPath.Width = 150;
      // 
      // ColFullFileName
      // 
      this.ColFullFileName.Text = "Full File Name";
      this.ColFullFileName.Width = 150;
      // 
      // ColFingerprintGuid
      // 
      this.ColFingerprintGuid.Text = "Fingerprint Guid";
      this.ColFingerprintGuid.Width = 100;
      // 
      // ColVersionGuid
      // 
      this.ColVersionGuid.Text = "Version Guid";
      this.ColVersionGuid.Width = 100;
      // 
      // ColTimeStamp
      // 
      this.ColTimeStamp.Text = "Time Stamp";
      this.ColTimeStamp.Width = 100;
      // 
      // ColFileSize
      // 
      this.ColFileSize.Text = "File Size";
      this.ColFileSize.Width = 100;
      // 
      // ColAffectsGraphics
      // 
      this.ColAffectsGraphics.Text = "Affects Graphics";
      this.ColAffectsGraphics.Width = 50;
      // 
      // ColReferenceCount
      // 
      this.ColReferenceCount.Text = "Reference Count";
      this.ColReferenceCount.Width = 50;
      // 
      // File_Dependency
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(517, 462);
      this.Controls.Add(this.listView1);
      this.Name = "File_Dependency";
      this.Text = "File Dependency";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader ColID;
    private System.Windows.Forms.ColumnHeader ColFeature;
    private System.Windows.Forms.ColumnHeader ColFileName;
    private System.Windows.Forms.ColumnHeader ColFoundPath;
    private System.Windows.Forms.ColumnHeader ColFullFileName;
    private System.Windows.Forms.ColumnHeader ColFingerprintGuid;
    private System.Windows.Forms.ColumnHeader ColVersionGuid;
    private System.Windows.Forms.ColumnHeader ColTimeStamp;
    private System.Windows.Forms.ColumnHeader ColFileSize;
    private System.Windows.Forms.ColumnHeader ColAffectsGraphics;
    private System.Windows.Forms.ColumnHeader ColReferenceCount;
  }
}