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
  partial class PublishDrawingSheets
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btSetting = new System.Windows.Forms.Button();
      this.btRemove = new System.Windows.Forms.Button();
      this.listView1 = new System.Windows.Forms.ListView();
      this.columnHeaderLayoutName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderSheetName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderDrawingName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderAuthor = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderSubject = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderPreviewName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderComments = new System.Windows.Forms.ColumnHeader();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.radioButtonZipped = new System.Windows.Forms.RadioButton();
      this.radioButtonBinaryDWF = new System.Windows.Forms.RadioButton();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.btBrowse = new System.Windows.Forms.Button();
      this.textBoxPassDWF = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxMultySheetDWFFileName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btPublish = new System.Windows.Forms.Button();
      this.btCancel = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.btSetting);
      this.groupBox1.Controls.Add(this.btRemove);
      this.groupBox1.Controls.Add(this.listView1);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(629, 190);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "List of drawing sheets";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(74, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "List of layouts:";
      // 
      // btSetting
      // 
      this.btSetting.Location = new System.Drawing.Point(548, 64);
      this.btSetting.Name = "btSetting";
      this.btSetting.Size = new System.Drawing.Size(75, 23);
      this.btSetting.TabIndex = 2;
      this.btSetting.Text = "Setting...";
      this.btSetting.UseVisualStyleBackColor = true;
      this.btSetting.Click += new System.EventHandler(this.btSetting_Click);
      // 
      // btRemove
      // 
      this.btRemove.Location = new System.Drawing.Point(548, 35);
      this.btRemove.Name = "btRemove";
      this.btRemove.Size = new System.Drawing.Size(75, 23);
      this.btRemove.TabIndex = 1;
      this.btRemove.Text = "Remove";
      this.btRemove.UseVisualStyleBackColor = true;
      this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLayoutName,
            this.columnHeaderSheetName,
            this.columnHeaderDrawingName,
            this.columnHeaderAuthor,
            this.columnHeaderSubject,
            this.columnHeaderPreviewName,
            this.columnHeaderComments});
      this.listView1.FullRowSelect = true;
      this.listView1.Location = new System.Drawing.Point(10, 38);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(532, 146);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      // 
      // columnHeaderLayoutName
      // 
      this.columnHeaderLayoutName.Text = "Layout Name";
      this.columnHeaderLayoutName.Width = 100;
      // 
      // columnHeaderSheetName
      // 
      this.columnHeaderSheetName.Text = "Sheet Name";
      this.columnHeaderSheetName.Width = 120;
      // 
      // columnHeaderDrawingName
      // 
      this.columnHeaderDrawingName.Text = "Drawing Name";
      this.columnHeaderDrawingName.Width = 230;
      // 
      // columnHeaderAuthor
      // 
      this.columnHeaderAuthor.Text = "Author";
      this.columnHeaderAuthor.Width = 80;
      // 
      // columnHeaderSubject
      // 
      this.columnHeaderSubject.Text = "Subject";
      this.columnHeaderSubject.Width = 80;
      // 
      // columnHeaderPreviewName
      // 
      this.columnHeaderPreviewName.Text = "Preview Name";
      this.columnHeaderPreviewName.Width = 230;
      // 
      // columnHeaderComments
      // 
      this.columnHeaderComments.Text = "Comments";
      this.columnHeaderComments.Width = 80;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.radioButtonZipped);
      this.groupBox2.Controls.Add(this.radioButtonBinaryDWF);
      this.groupBox2.Location = new System.Drawing.Point(12, 217);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(229, 71);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Format";
      // 
      // radioButtonZipped
      // 
      this.radioButtonZipped.AutoSize = true;
      this.radioButtonZipped.Checked = true;
      this.radioButtonZipped.Location = new System.Drawing.Point(10, 42);
      this.radioButtonZipped.Name = "radioButtonZipped";
      this.radioButtonZipped.Size = new System.Drawing.Size(210, 17);
      this.radioButtonZipped.TabIndex = 1;
      this.radioButtonZipped.TabStop = true;
      this.radioButtonZipped.Text = "Zipped Ascii Encoded 2D Stream DWF";
      this.radioButtonZipped.UseVisualStyleBackColor = true;
      // 
      // radioButtonBinaryDWF
      // 
      this.radioButtonBinaryDWF.AutoSize = true;
      this.radioButtonBinaryDWF.Location = new System.Drawing.Point(10, 19);
      this.radioButtonBinaryDWF.Name = "radioButtonBinaryDWF";
      this.radioButtonBinaryDWF.Size = new System.Drawing.Size(82, 17);
      this.radioButtonBinaryDWF.TabIndex = 0;
      this.radioButtonBinaryDWF.Text = "Binary DWF";
      this.radioButtonBinaryDWF.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.btBrowse);
      this.groupBox3.Controls.Add(this.textBoxPassDWF);
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.textBoxMultySheetDWFFileName);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Location = new System.Drawing.Point(17, 300);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(524, 113);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Publish to:";
      // 
      // btBrowse
      // 
      this.btBrowse.Location = new System.Drawing.Point(434, 40);
      this.btBrowse.Name = "btBrowse";
      this.btBrowse.Size = new System.Drawing.Size(75, 23);
      this.btBrowse.TabIndex = 4;
      this.btBrowse.Text = "Browse...";
      this.btBrowse.UseVisualStyleBackColor = true;
      this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
      // 
      // textBoxPassDWF
      // 
      this.textBoxPassDWF.Enabled = false;
      this.textBoxPassDWF.Location = new System.Drawing.Point(14, 79);
      this.textBoxPassDWF.Name = "textBoxPassDWF";
      this.textBoxPassDWF.Size = new System.Drawing.Size(405, 20);
      this.textBoxPassDWF.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(11, 63);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(167, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Password to protect this DWF file:";
      // 
      // textBoxMultySheetDWFFileName
      // 
      this.textBoxMultySheetDWFFileName.Location = new System.Drawing.Point(14, 40);
      this.textBoxMultySheetDWFFileName.Name = "textBoxMultySheetDWFFileName";
      this.textBoxMultySheetDWFFileName.Size = new System.Drawing.Size(405, 20);
      this.textBoxMultySheetDWFFileName.TabIndex = 1;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 24);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(134, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Multi-sheet DWF file name:";
      // 
      // btPublish
      // 
      this.btPublish.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btPublish.Location = new System.Drawing.Point(566, 358);
      this.btPublish.Name = "btPublish";
      this.btPublish.Size = new System.Drawing.Size(75, 23);
      this.btPublish.TabIndex = 5;
      this.btPublish.Text = "Publish";
      this.btPublish.UseVisualStyleBackColor = true;
      this.btPublish.Click += new System.EventHandler(this.btPublish_Click);
      // 
      // btCancel
      // 
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Location = new System.Drawing.Point(566, 390);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(75, 23);
      this.btCancel.TabIndex = 6;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "Deisgn Web Format (*.dwf)|*.dwf";
      // 
      // PublishDrawingSheets
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(653, 419);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btPublish);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "PublishDrawingSheets";
      this.Text = "PublishDrawingSheets";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btSetting;
    private System.Windows.Forms.Button btRemove;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton radioButtonZipped;
    private System.Windows.Forms.RadioButton radioButtonBinaryDWF;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBoxMultySheetDWFFileName;
    private System.Windows.Forms.TextBox textBoxPassDWF;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btBrowse;
    private System.Windows.Forms.Button btPublish;
    private System.Windows.Forms.Button btCancel;
    private System.Windows.Forms.ColumnHeader columnHeaderLayoutName;
    private System.Windows.Forms.ColumnHeader columnHeaderSheetName;
    private System.Windows.Forms.ColumnHeader columnHeaderDrawingName;
    private System.Windows.Forms.ColumnHeader columnHeaderAuthor;
    private System.Windows.Forms.ColumnHeader columnHeaderSubject;
    private System.Windows.Forms.ColumnHeader columnHeaderPreviewName;
    private System.Windows.Forms.ColumnHeader columnHeaderComments;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    public System.Windows.Forms.ListView listView1;
  }
}