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
  partial class BMPExport
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
      this.label1 = new System.Windows.Forms.Label();
      this.colorDialog = new System.Windows.Forms.ColorDialog();
      this.buttonBkgColor = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
      this.checkBoxPlotGeneration = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(209, 14);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(57, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "BitPerPixel";
      // 
      // colorDialog
      // 
      this.colorDialog.Color = System.Drawing.Color.SpringGreen;
      // 
      // buttonBkgColor
      // 
      this.buttonBkgColor.Location = new System.Drawing.Point(148, 79);
      this.buttonBkgColor.Name = "buttonBkgColor";
      this.buttonBkgColor.Size = new System.Drawing.Size(125, 23);
      this.buttonBkgColor.TabIndex = 3;
      this.buttonBkgColor.Text = "Background Color";
      this.buttonBkgColor.UseVisualStyleBackColor = true;
      this.buttonBkgColor.Click += new System.EventHandler(this.buttonBkgColor_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(4, 19);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(67, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Bitmap width";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(4, 47);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(71, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Bitmap height";
      // 
      // numericUpDownWidth
      // 
      this.numericUpDownWidth.Location = new System.Drawing.Point(77, 17);
      this.numericUpDownWidth.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
      this.numericUpDownWidth.Name = "numericUpDownWidth";
      this.numericUpDownWidth.Size = new System.Drawing.Size(65, 20);
      this.numericUpDownWidth.TabIndex = 6;
      this.numericUpDownWidth.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      // 
      // numericUpDownHeight
      // 
      this.numericUpDownHeight.Location = new System.Drawing.Point(77, 43);
      this.numericUpDownHeight.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
      this.numericUpDownHeight.Name = "numericUpDownHeight";
      this.numericUpDownHeight.Size = new System.Drawing.Size(65, 20);
      this.numericUpDownHeight.TabIndex = 7;
      this.numericUpDownHeight.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      // 
      // checkBoxPlotGeneration
      // 
      this.checkBoxPlotGeneration.AutoSize = true;
      this.checkBoxPlotGeneration.Checked = true;
      this.checkBoxPlotGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxPlotGeneration.Location = new System.Drawing.Point(6, 85);
      this.checkBoxPlotGeneration.Name = "checkBoxPlotGeneration";
      this.checkBoxPlotGeneration.Size = new System.Drawing.Size(96, 17);
      this.checkBoxPlotGeneration.TabIndex = 8;
      this.checkBoxPlotGeneration.Text = "PlotGeneration";
      this.checkBoxPlotGeneration.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.comboBox1);
      this.groupBox1.Controls.Add(this.checkBoxPlotGeneration);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.numericUpDownHeight);
      this.groupBox1.Controls.Add(this.buttonBkgColor);
      this.groupBox1.Controls.Add(this.numericUpDownWidth);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Location = new System.Drawing.Point(15, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(279, 115);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Properties";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(56, 133);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 10;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(176, 133);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 11;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // openFileDialog
      // 
      this.openFileDialog.DefaultExt = "txv";
      this.openFileDialog.Filter = "TXV files|*.txv";
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.DefaultExt = "bmp";
      this.saveFileDialog.Filter = "BMP files|*.bmp";
      // 
      // comboBox1
      // 
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16",
            "24",
            "32"});
      this.comboBox1.Location = new System.Drawing.Point(212, 30);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(54, 21);
      this.comboBox1.TabIndex = 9;
      this.comboBox1.Text = "24";
      // 
      // BMPExport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(306, 165);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.groupBox1);
      this.Name = "BMPExport";
      this.Text = "BMPExport";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ColorDialog colorDialog;
    private System.Windows.Forms.Button buttonBkgColor;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown numericUpDownWidth;
    private System.Windows.Forms.NumericUpDown numericUpDownHeight;
    private System.Windows.Forms.CheckBox checkBoxPlotGeneration;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.ComboBox comboBox1;
  }
}