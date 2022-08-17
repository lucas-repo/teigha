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

namespace OdaMgdMViewApp
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.partialOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.windowMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
      this.toolStripDrawStyle = new System.Windows.Forms.ToolStrip();
      this.ZoomExtents = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonDolly = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonOrbit = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripWire2d = new System.Windows.Forms.ToolStripButton();
      this.toolStripWire3d = new System.Windows.Forms.ToolStripButton();
      this.toolStripHidden = new System.Windows.Forms.ToolStripButton();
      this.toolStripFlatShaded = new System.Windows.Forms.ToolStripButton();
      this.toolStripGroundedShaded = new System.Windows.Forms.ToolStripButton();
      this.toolStripFlatShadedEd = new System.Windows.Forms.ToolStripButton();
      this.toolStripGroundedShadedEd = new System.Windows.Forms.ToolStripButton();
      this.menuStrip.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.toolStripDrawStyle.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.windowMenuToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.MdiWindowListItem = this.windowMenuToolStripMenuItem;
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(832, 24);
      this.menuStrip.TabIndex = 1;
      this.menuStrip.Text = "menuStrip";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.partialOpenToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.newToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
      this.newToolStripMenuItem.MergeIndex = 0;
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.newToolStripMenuItem.Text = "&File";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.MergeIndex = 0;
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
      this.openToolStripMenuItem.Text = "&Open";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // partialOpenToolStripMenuItem
      // 
      this.partialOpenToolStripMenuItem.MergeIndex = 1;
      this.partialOpenToolStripMenuItem.Name = "partialOpenToolStripMenuItem";
      this.partialOpenToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
      this.partialOpenToolStripMenuItem.Text = "Partial Open";
      this.partialOpenToolStripMenuItem.Click += new System.EventHandler(this.partialOpenToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.MergeIndex = 5;
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
      this.exitToolStripMenuItem.Text = "&Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // windowMenuToolStripMenuItem
      // 
      this.windowMenuToolStripMenuItem.Name = "windowMenuToolStripMenuItem";
      this.windowMenuToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
      this.windowMenuToolStripMenuItem.Text = "&Window";
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
      // 
      // redoToolStripMenuItem
      // 
      this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
      this.redoToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
      // 
      // openFileDialog
      // 
      this.openFileDialog.DefaultExt = "DWG";
      this.openFileDialog.FileName = "openFileDialog";
      this.openFileDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf";
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(35, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButtonOpen
      // 
      this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
      this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Chartreuse;
      this.toolStripButtonOpen.Name = "toolStripButtonOpen";
      this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonOpen.Text = "Open";
      this.toolStripButtonOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // toolStripDrawStyle
      // 
      this.toolStripDrawStyle.AllowDrop = true;
      this.toolStripDrawStyle.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStripDrawStyle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomExtents,
            this.toolStripSeparator4,
            this.toolStripButtonDolly,
            this.toolStripSeparator3,
            this.toolStripButtonOrbit,
            this.toolStripSeparator2,
            this.toolStripWire2d,
            this.toolStripWire3d,
            this.toolStripHidden,
            this.toolStripFlatShaded,
            this.toolStripGroundedShaded,
            this.toolStripFlatShadedEd,
            this.toolStripGroundedShadedEd});
      this.toolStripDrawStyle.Location = new System.Drawing.Point(35, 24);
      this.toolStripDrawStyle.Name = "toolStripDrawStyle";
      this.toolStripDrawStyle.Size = new System.Drawing.Size(290, 25);
      this.toolStripDrawStyle.TabIndex = 4;
      this.toolStripDrawStyle.Text = "toolStrip1";
      // 
      // ZoomExtents
      // 
      this.ZoomExtents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.ZoomExtents.Image = ((System.Drawing.Image)(resources.GetObject("ZoomExtents.Image")));
      this.ZoomExtents.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ZoomExtents.Name = "ZoomExtents";
      this.ZoomExtents.Size = new System.Drawing.Size(23, 22);
      this.ZoomExtents.Text = "ZoomExtents";
      this.ZoomExtents.Click += new System.EventHandler(this.ZoomExtents_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButtonDolly
      // 
      this.toolStripButtonDolly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDolly.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDolly.Image")));
      this.toolStripButtonDolly.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDolly.Name = "toolStripButtonDolly";
      this.toolStripButtonDolly.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonDolly.Text = "Dolly";
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButtonOrbit
      // 
      this.toolStripButtonOrbit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonOrbit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOrbit.Image")));
      this.toolStripButtonOrbit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonOrbit.Name = "toolStripButtonOrbit";
      this.toolStripButtonOrbit.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonOrbit.Text = "Rotate";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripWire2d
      // 
      this.toolStripWire2d.AutoSize = false;
      this.toolStripWire2d.Checked = true;
      this.toolStripWire2d.CheckState = System.Windows.Forms.CheckState.Checked;
      this.toolStripWire2d.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripWire2d.Image = ((System.Drawing.Image)(resources.GetObject("toolStripWire2d.Image")));
      this.toolStripWire2d.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.toolStripWire2d.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripWire2d.Name = "toolStripWire2d";
      this.toolStripWire2d.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.toolStripWire2d.Size = new System.Drawing.Size(22, 22);
      this.toolStripWire2d.Text = "2d Wireframe";
      // 
      // toolStripWire3d
      // 
      this.toolStripWire3d.CheckOnClick = true;
      this.toolStripWire3d.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripWire3d.Image = ((System.Drawing.Image)(resources.GetObject("toolStripWire3d.Image")));
      this.toolStripWire3d.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripWire3d.Name = "toolStripWire3d";
      this.toolStripWire3d.Size = new System.Drawing.Size(23, 22);
      this.toolStripWire3d.Text = "3d Wireframe";
      // 
      // toolStripHidden
      // 
      this.toolStripHidden.AccessibleName = "";
      this.toolStripHidden.CheckOnClick = true;
      this.toolStripHidden.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripHidden.Image = ((System.Drawing.Image)(resources.GetObject("toolStripHidden.Image")));
      this.toolStripHidden.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripHidden.Name = "toolStripHidden";
      this.toolStripHidden.Size = new System.Drawing.Size(23, 22);
      this.toolStripHidden.Text = "Hidden";
      // 
      // toolStripFlatShaded
      // 
      this.toolStripFlatShaded.AccessibleName = "";
      this.toolStripFlatShaded.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripFlatShaded.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFlatShaded.Image")));
      this.toolStripFlatShaded.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripFlatShaded.Name = "toolStripFlatShaded";
      this.toolStripFlatShaded.Size = new System.Drawing.Size(23, 22);
      this.toolStripFlatShaded.Text = "Flat Shaded";
      // 
      // toolStripGroundedShaded
      // 
      this.toolStripGroundedShaded.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripGroundedShaded.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGroundedShaded.Image")));
      this.toolStripGroundedShaded.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripGroundedShaded.Name = "toolStripGroundedShaded";
      this.toolStripGroundedShaded.Size = new System.Drawing.Size(23, 22);
      this.toolStripGroundedShaded.Text = "Gouraud Shadded";
      // 
      // toolStripFlatShadedEd
      // 
      this.toolStripFlatShadedEd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripFlatShadedEd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFlatShadedEd.Image")));
      this.toolStripFlatShadedEd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripFlatShadedEd.Name = "toolStripFlatShadedEd";
      this.toolStripFlatShadedEd.Size = new System.Drawing.Size(23, 22);
      this.toolStripFlatShadedEd.Text = "Flat Shaded with Edges";
      // 
      // toolStripGroundedShadedEd
      // 
      this.toolStripGroundedShadedEd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripGroundedShadedEd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGroundedShadedEd.Image")));
      this.toolStripGroundedShadedEd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripGroundedShadedEd.Name = "toolStripGroundedShadedEd";
      this.toolStripGroundedShadedEd.Size = new System.Drawing.Size(23, 22);
      this.toolStripGroundedShadedEd.Text = "Gouraud Shadded with Edges";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(832, 469);
      this.Controls.Add(this.toolStripDrawStyle);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this.menuStrip;
      this.Name = "MainForm";
      this.Text = "OdaMgdMViewApp";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.toolStripDrawStyle.ResumeLayout(false);
      this.toolStripDrawStyle.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem partialOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
    private System.Windows.Forms.ToolStrip toolStripDrawStyle;
    private System.Windows.Forms.ToolStripButton toolStripButtonDolly;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton toolStripButtonOrbit;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripWire2d;
    private System.Windows.Forms.ToolStripButton toolStripWire3d;
    private System.Windows.Forms.ToolStripButton toolStripHidden;
    private System.Windows.Forms.ToolStripButton toolStripFlatShaded;
    private System.Windows.Forms.ToolStripButton toolStripGroundedShaded;
    private System.Windows.Forms.ToolStripButton toolStripFlatShadedEd;
    private System.Windows.Forms.ToolStripButton toolStripGroundedShadedEd;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton ZoomExtents;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem windowMenuToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
  }
}

