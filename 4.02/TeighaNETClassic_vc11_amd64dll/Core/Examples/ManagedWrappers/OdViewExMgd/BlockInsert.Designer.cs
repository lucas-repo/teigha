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
  partial class BlockInsert
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
      this.comboBoxBlocks = new System.Windows.Forms.ComboBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxInsPointX = new System.Windows.Forms.TextBox();
      this.textBoxInsPointY = new System.Windows.Forms.TextBox();
      this.textBoxInsPointZ = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.textBoxScaleZ = new System.Windows.Forms.TextBox();
      this.textBoxScaleY = new System.Windows.Forms.TextBox();
      this.textBoxScaleX = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboBoxBlocks
      // 
      this.comboBoxBlocks.FormattingEnabled = true;
      this.comboBoxBlocks.Location = new System.Drawing.Point(12, 12);
      this.comboBoxBlocks.Name = "comboBoxBlocks";
      this.comboBoxBlocks.Size = new System.Drawing.Size(276, 21);
      this.comboBoxBlocks.TabIndex = 1;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.textBoxInsPointZ);
      this.groupBox1.Controls.Add(this.textBoxInsPointY);
      this.groupBox1.Controls.Add(this.textBoxInsPointX);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Location = new System.Drawing.Point(12, 39);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(135, 118);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Insertion point";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(17, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "X:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(17, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Y:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 74);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(17, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Z:";
      // 
      // textBoxInsPointX
      // 
      this.textBoxInsPointX.Location = new System.Drawing.Point(26, 19);
      this.textBoxInsPointX.Name = "textBoxInsPointX";
      this.textBoxInsPointX.Size = new System.Drawing.Size(100, 20);
      this.textBoxInsPointX.TabIndex = 8;
      this.textBoxInsPointX.Text = "0";
      // 
      // textBoxInsPointY
      // 
      this.textBoxInsPointY.Location = new System.Drawing.Point(26, 45);
      this.textBoxInsPointY.Name = "textBoxInsPointY";
      this.textBoxInsPointY.Size = new System.Drawing.Size(100, 20);
      this.textBoxInsPointY.TabIndex = 9;
      this.textBoxInsPointY.Text = "0";
      // 
      // textBoxInsPointZ
      // 
      this.textBoxInsPointZ.Location = new System.Drawing.Point(26, 71);
      this.textBoxInsPointZ.Name = "textBoxInsPointZ";
      this.textBoxInsPointZ.Size = new System.Drawing.Size(100, 20);
      this.textBoxInsPointZ.TabIndex = 10;
      this.textBoxInsPointZ.Text = "0";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.textBoxScaleZ);
      this.groupBox2.Controls.Add(this.textBoxScaleY);
      this.groupBox2.Controls.Add(this.textBoxScaleX);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.label6);
      this.groupBox2.Location = new System.Drawing.Point(153, 39);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(135, 118);
      this.groupBox2.TabIndex = 11;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Scale";
      // 
      // textBoxScaleZ
      // 
      this.textBoxScaleZ.Location = new System.Drawing.Point(26, 71);
      this.textBoxScaleZ.Name = "textBoxScaleZ";
      this.textBoxScaleZ.Size = new System.Drawing.Size(100, 20);
      this.textBoxScaleZ.TabIndex = 10;
      this.textBoxScaleZ.Text = "1";
      // 
      // textBoxScaleY
      // 
      this.textBoxScaleY.Location = new System.Drawing.Point(26, 45);
      this.textBoxScaleY.Name = "textBoxScaleY";
      this.textBoxScaleY.Size = new System.Drawing.Size(100, 20);
      this.textBoxScaleY.TabIndex = 9;
      this.textBoxScaleY.Text = "1";
      // 
      // textBoxScaleX
      // 
      this.textBoxScaleX.Location = new System.Drawing.Point(26, 19);
      this.textBoxScaleX.Name = "textBoxScaleX";
      this.textBoxScaleX.Size = new System.Drawing.Size(100, 20);
      this.textBoxScaleX.TabIndex = 8;
      this.textBoxScaleX.Text = "1";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 74);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(17, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Z:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 48);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(17, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "Y:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 22);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(17, 13);
      this.label6.TabIndex = 1;
      this.label6.Text = "X:";
      // 
      // buttonOK
      // 
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(132, 163);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 12;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Location = new System.Drawing.Point(213, 163);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 13;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // BlockInsert
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(300, 196);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.comboBoxBlocks);
      this.Name = "BlockInsert";
      this.Text = "BlockInsert";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxBlocks;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxInsPointZ;
    private System.Windows.Forms.TextBox textBoxInsPointY;
    private System.Windows.Forms.TextBox textBoxInsPointX;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TextBox textBoxScaleZ;
    private System.Windows.Forms.TextBox textBoxScaleY;
    private System.Windows.Forms.TextBox textBoxScaleX;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;


  }
}