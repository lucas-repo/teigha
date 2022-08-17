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
  partial class PDFExport
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
      this.outputFile = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btBrowse = new System.Windows.Forms.Button();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.checkBoxASCIIHEXencoded = new System.Windows.Forms.CheckBox();
      this.checkBoxMonochrome = new System.Windows.Forms.CheckBox();
      this.textBox_producer = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.textBox_creator = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.textBox_keywords = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.textBox_subject = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.textBox_author = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.textBox_title = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.radioButton_All = new System.Windows.Forms.RadioButton();
      this.radioButton_Active = new System.Windows.Forms.RadioButton();
      this.PapHeight = new System.Windows.Forms.TextBox();
      this.PapWidth = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.UseHidLRAlgorithm = new System.Windows.Forms.CheckBox();
      this.EnableLayerSup_pdfv1_5 = new System.Windows.Forms.CheckBox();
      this.ExportOffLay = new System.Windows.Forms.CheckBox();
      this.EncodedSZ = new System.Windows.Forms.CheckBox();
      this.TTGeometry = new System.Windows.Forms.CheckBox();
      this.SHXTextAsGeometry = new System.Windows.Forms.CheckBox();
      this.ESimpGeometryOpt = new System.Windows.Forms.CheckBox();
      this.ZoomExtents = new System.Windows.Forms.CheckBox();
      this.embedded_fonts = new System.Windows.Forms.CheckBox();
      this.Export = new System.Windows.Forms.Button();
      this.Cancel = new System.Windows.Forms.Button();
      this.label11 = new System.Windows.Forms.Label();
      this.HatchToBMP = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // outputFile
      // 
      this.outputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.outputFile.Location = new System.Drawing.Point(12, 25);
      this.outputFile.Name = "outputFile";
      this.outputFile.Size = new System.Drawing.Size(469, 21);
      this.outputFile.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(58, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Output file:";
      // 
      // btBrowse
      // 
      this.btBrowse.Location = new System.Drawing.Point(487, 23);
      this.btBrowse.Name = "btBrowse";
      this.btBrowse.Size = new System.Drawing.Size(73, 26);
      this.btBrowse.TabIndex = 2;
      this.btBrowse.Text = "Browse";
      this.btBrowse.UseVisualStyleBackColor = true;
      this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.DefaultExt = "pdf";
      this.saveFileDialog.Filter = "PDF files|*.pdf";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.HatchToBMP);
      this.groupBox1.Controls.Add(this.label11);
      this.groupBox1.Controls.Add(this.checkBoxASCIIHEXencoded);
      this.groupBox1.Controls.Add(this.checkBoxMonochrome);
      this.groupBox1.Controls.Add(this.textBox_producer);
      this.groupBox1.Controls.Add(this.label10);
      this.groupBox1.Controls.Add(this.textBox_creator);
      this.groupBox1.Controls.Add(this.label9);
      this.groupBox1.Controls.Add(this.textBox_keywords);
      this.groupBox1.Controls.Add(this.label8);
      this.groupBox1.Controls.Add(this.textBox_subject);
      this.groupBox1.Controls.Add(this.label7);
      this.groupBox1.Controls.Add(this.textBox_author);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.textBox_title);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.radioButton_All);
      this.groupBox1.Controls.Add(this.radioButton_Active);
      this.groupBox1.Controls.Add(this.PapHeight);
      this.groupBox1.Controls.Add(this.PapWidth);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.UseHidLRAlgorithm);
      this.groupBox1.Controls.Add(this.EnableLayerSup_pdfv1_5);
      this.groupBox1.Controls.Add(this.ExportOffLay);
      this.groupBox1.Controls.Add(this.EncodedSZ);
      this.groupBox1.Controls.Add(this.TTGeometry);
      this.groupBox1.Controls.Add(this.SHXTextAsGeometry);
      this.groupBox1.Controls.Add(this.ESimpGeometryOpt);
      this.groupBox1.Controls.Add(this.ZoomExtents);
      this.groupBox1.Controls.Add(this.embedded_fonts);
      this.groupBox1.Location = new System.Drawing.Point(15, 63);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(545, 375);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Properties";
      // 
      // checkBoxASCIIHEXencoded
      // 
      this.checkBoxASCIIHEXencoded.AutoSize = true;
      this.checkBoxASCIIHEXencoded.Checked = true;
      this.checkBoxASCIIHEXencoded.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxASCIIHEXencoded.Location = new System.Drawing.Point(289, 157);
      this.checkBoxASCIIHEXencoded.Name = "checkBoxASCIIHEXencoded";
      this.checkBoxASCIIHEXencoded.Size = new System.Drawing.Size(161, 17);
      this.checkBoxASCIIHEXencoded.TabIndex = 27;
      this.checkBoxASCIIHEXencoded.Text = "ASCIIHEX encoded (x2 size)";
      this.checkBoxASCIIHEXencoded.UseVisualStyleBackColor = true;
      // 
      // checkBoxMonochrome
      // 
      this.checkBoxMonochrome.AutoSize = true;
      this.checkBoxMonochrome.Location = new System.Drawing.Point(289, 180);
      this.checkBoxMonochrome.Name = "checkBoxMonochrome";
      this.checkBoxMonochrome.Size = new System.Drawing.Size(129, 17);
      this.checkBoxMonochrome.TabIndex = 26;
      this.checkBoxMonochrome.Text = "Save as monochrome";
      this.checkBoxMonochrome.UseVisualStyleBackColor = true;
      // 
      // textBox_producer
      // 
      this.textBox_producer.Location = new System.Drawing.Point(67, 346);
      this.textBox_producer.Name = "textBox_producer";
      this.textBox_producer.Size = new System.Drawing.Size(449, 20);
      this.textBox_producer.TabIndex = 25;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(3, 349);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(56, 13);
      this.label10.TabIndex = 24;
      this.label10.Text = "Producer :";
      // 
      // textBox_creator
      // 
      this.textBox_creator.Location = new System.Drawing.Point(67, 320);
      this.textBox_creator.Name = "textBox_creator";
      this.textBox_creator.Size = new System.Drawing.Size(449, 20);
      this.textBox_creator.TabIndex = 23;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(3, 323);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(47, 13);
      this.label9.TabIndex = 22;
      this.label9.Text = "Creator :";
      // 
      // textBox_keywords
      // 
      this.textBox_keywords.Location = new System.Drawing.Point(67, 294);
      this.textBox_keywords.Name = "textBox_keywords";
      this.textBox_keywords.Size = new System.Drawing.Size(449, 20);
      this.textBox_keywords.TabIndex = 21;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(3, 297);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(59, 13);
      this.label8.TabIndex = 20;
      this.label8.Text = "Keywords :";
      // 
      // textBox_subject
      // 
      this.textBox_subject.Location = new System.Drawing.Point(67, 268);
      this.textBox_subject.Name = "textBox_subject";
      this.textBox_subject.Size = new System.Drawing.Size(449, 20);
      this.textBox_subject.TabIndex = 19;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(3, 271);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(49, 13);
      this.label7.TabIndex = 18;
      this.label7.Text = "Subject :";
      // 
      // textBox_author
      // 
      this.textBox_author.Location = new System.Drawing.Point(67, 242);
      this.textBox_author.Name = "textBox_author";
      this.textBox_author.Size = new System.Drawing.Size(449, 20);
      this.textBox_author.TabIndex = 17;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 245);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(44, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "Author :";
      // 
      // textBox_title
      // 
      this.textBox_title.Location = new System.Drawing.Point(67, 216);
      this.textBox_title.Name = "textBox_title";
      this.textBox_title.Size = new System.Drawing.Size(449, 20);
      this.textBox_title.TabIndex = 15;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 219);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(33, 13);
      this.label5.TabIndex = 14;
      this.label5.Text = "Title :";
      // 
      // radioButton_All
      // 
      this.radioButton_All.AutoSize = true;
      this.radioButton_All.Location = new System.Drawing.Point(393, 115);
      this.radioButton_All.Name = "radioButton_All";
      this.radioButton_All.Size = new System.Drawing.Size(36, 17);
      this.radioButton_All.TabIndex = 13;
      this.radioButton_All.Text = "All";
      this.radioButton_All.UseVisualStyleBackColor = true;
      // 
      // radioButton_Active
      // 
      this.radioButton_Active.AutoSize = true;
      this.radioButton_Active.Checked = true;
      this.radioButton_Active.Location = new System.Drawing.Point(393, 97);
      this.radioButton_Active.Name = "radioButton_Active";
      this.radioButton_Active.Size = new System.Drawing.Size(55, 17);
      this.radioButton_Active.TabIndex = 12;
      this.radioButton_Active.TabStop = true;
      this.radioButton_Active.Text = "Active";
      this.radioButton_Active.UseVisualStyleBackColor = true;
      // 
      // PapHeight
      // 
      this.PapHeight.Location = new System.Drawing.Point(393, 43);
      this.PapHeight.Name = "PapHeight";
      this.PapHeight.Size = new System.Drawing.Size(123, 20);
      this.PapHeight.TabIndex = 11;
      // 
      // PapWidth
      // 
      this.PapWidth.Location = new System.Drawing.Point(393, 15);
      this.PapWidth.Name = "PapWidth";
      this.PapWidth.Size = new System.Drawing.Size(123, 20);
      this.PapWidth.TabIndex = 10;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(286, 101);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(94, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Layouts to export :";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(286, 46);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(95, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Paper height, mm :";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(286, 18);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(91, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Paper width, mm :";
      // 
      // UseHidLRAlgorithm
      // 
      this.UseHidLRAlgorithm.AutoSize = true;
      this.UseHidLRAlgorithm.Checked = true;
      this.UseHidLRAlgorithm.CheckState = System.Windows.Forms.CheckState.Checked;
      this.UseHidLRAlgorithm.Location = new System.Drawing.Point(6, 134);
      this.UseHidLRAlgorithm.Name = "UseHidLRAlgorithm";
      this.UseHidLRAlgorithm.Size = new System.Drawing.Size(184, 17);
      this.UseHidLRAlgorithm.TabIndex = 8;
      this.UseHidLRAlgorithm.Text = "Use hidden line removal algorithm";
      this.UseHidLRAlgorithm.UseVisualStyleBackColor = true;
      // 
      // EnableLayerSup_pdfv1_5
      // 
      this.EnableLayerSup_pdfv1_5.AutoSize = true;
      this.EnableLayerSup_pdfv1_5.Location = new System.Drawing.Point(6, 157);
      this.EnableLayerSup_pdfv1_5.Name = "EnableLayerSup_pdfv1_5";
      this.EnableLayerSup_pdfv1_5.Size = new System.Drawing.Size(170, 17);
      this.EnableLayerSup_pdfv1_5.TabIndex = 7;
      this.EnableLayerSup_pdfv1_5.Text = "Enable layer support (pdf v1.5)";
      this.EnableLayerSup_pdfv1_5.UseVisualStyleBackColor = true;
      this.EnableLayerSup_pdfv1_5.CheckedChanged += new System.EventHandler(this.EnableLayerSup_pdfv1_5_CheckedChanged);
      // 
      // ExportOffLay
      // 
      this.ExportOffLay.AutoSize = true;
      this.ExportOffLay.Enabled = false;
      this.ExportOffLay.Location = new System.Drawing.Point(17, 180);
      this.ExportOffLay.Name = "ExportOffLay";
      this.ExportOffLay.Size = new System.Drawing.Size(121, 17);
      this.ExportOffLay.TabIndex = 6;
      this.ExportOffLay.Text = "Export Off layers too";
      this.ExportOffLay.UseVisualStyleBackColor = true;
      // 
      // EncodedSZ
      // 
      this.EncodedSZ.AutoSize = true;
      this.EncodedSZ.Checked = true;
      this.EncodedSZ.CheckState = System.Windows.Forms.CheckState.Checked;
      this.EncodedSZ.Location = new System.Drawing.Point(289, 134);
      this.EncodedSZ.Name = "EncodedSZ";
      this.EncodedSZ.Size = new System.Drawing.Size(122, 17);
      this.EncodedSZ.TabIndex = 5;
      this.EncodedSZ.Text = "Encoded (small size)";
      this.EncodedSZ.UseVisualStyleBackColor = true;
      // 
      // TTGeometry
      // 
      this.TTGeometry.AutoSize = true;
      this.TTGeometry.Checked = true;
      this.TTGeometry.CheckState = System.Windows.Forms.CheckState.Checked;
      this.TTGeometry.Location = new System.Drawing.Point(6, 42);
      this.TTGeometry.Name = "TTGeometry";
      this.TTGeometry.Size = new System.Drawing.Size(131, 17);
      this.TTGeometry.TabIndex = 4;
      this.TTGeometry.Text = "True type as geometry";
      this.TTGeometry.UseVisualStyleBackColor = true;
      // 
      // SHXTextAsGeometry
      // 
      this.SHXTextAsGeometry.AutoSize = true;
      this.SHXTextAsGeometry.Checked = true;
      this.SHXTextAsGeometry.CheckState = System.Windows.Forms.CheckState.Checked;
      this.SHXTextAsGeometry.Location = new System.Drawing.Point(6, 65);
      this.SHXTextAsGeometry.Name = "SHXTextAsGeometry";
      this.SHXTextAsGeometry.Size = new System.Drawing.Size(128, 17);
      this.SHXTextAsGeometry.TabIndex = 3;
      this.SHXTextAsGeometry.Text = "SHX text as geometry";
      this.SHXTextAsGeometry.UseVisualStyleBackColor = true;
      // 
      // ESimpGeometryOpt
      // 
      this.ESimpGeometryOpt.AutoSize = true;
      this.ESimpGeometryOpt.Location = new System.Drawing.Point(6, 88);
      this.ESimpGeometryOpt.Name = "ESimpGeometryOpt";
      this.ESimpGeometryOpt.Size = new System.Drawing.Size(195, 17);
      this.ESimpGeometryOpt.TabIndex = 2;
      this.ESimpGeometryOpt.Text = "Enable simple geometry optimization";
      this.ESimpGeometryOpt.UseVisualStyleBackColor = true;
      // 
      // ZoomExtents
      // 
      this.ZoomExtents.AutoSize = true;
      this.ZoomExtents.Checked = true;
      this.ZoomExtents.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ZoomExtents.Location = new System.Drawing.Point(6, 111);
      this.ZoomExtents.Name = "ZoomExtents";
      this.ZoomExtents.Size = new System.Drawing.Size(131, 17);
      this.ZoomExtents.TabIndex = 1;
      this.ZoomExtents.Text = "Zoom to extents mode";
      this.ZoomExtents.UseVisualStyleBackColor = true;
      // 
      // embedded_fonts
      // 
      this.embedded_fonts.AutoSize = true;
      this.embedded_fonts.Location = new System.Drawing.Point(6, 19);
      this.embedded_fonts.Name = "embedded_fonts";
      this.embedded_fonts.Size = new System.Drawing.Size(103, 17);
      this.embedded_fonts.TabIndex = 0;
      this.embedded_fonts.Text = "Embedded fonts";
      this.embedded_fonts.UseVisualStyleBackColor = true;
      // 
      // Export
      // 
      this.Export.Location = new System.Drawing.Point(96, 442);
      this.Export.Name = "Export";
      this.Export.Size = new System.Drawing.Size(155, 27);
      this.Export.TabIndex = 4;
      this.Export.Text = "Export";
      this.Export.UseVisualStyleBackColor = true;
      this.Export.Click += new System.EventHandler(this.Export_Click);
      // 
      // Cancel
      // 
      this.Cancel.Location = new System.Drawing.Point(326, 442);
      this.Cancel.Name = "Cancel";
      this.Cancel.Size = new System.Drawing.Size(155, 27);
      this.Cancel.TabIndex = 5;
      this.Cancel.Text = "Cancel";
      this.Cancel.UseVisualStyleBackColor = true;
      this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(286, 72);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(100, 13);
      this.label11.TabIndex = 28;
      this.label11.Text = "Hatch to BMP, dpi :";
      // 
      // HatchToBMP
      // 
      this.HatchToBMP.Location = new System.Drawing.Point(393, 69);
      this.HatchToBMP.Name = "HatchToBMP";
      this.HatchToBMP.Size = new System.Drawing.Size(123, 20);
      this.HatchToBMP.TabIndex = 29;
      // 
      // PDFExport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(573, 473);
      this.Controls.Add(this.Cancel);
      this.Controls.Add(this.Export);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btBrowse);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.outputFile);
      this.Name = "PDFExport";
      this.Text = "PDFExport";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox outputFile;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btBrowse;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox TTGeometry;
    private System.Windows.Forms.CheckBox SHXTextAsGeometry;
    private System.Windows.Forms.CheckBox ESimpGeometryOpt;
    private System.Windows.Forms.CheckBox ZoomExtents;
    private System.Windows.Forms.CheckBox embedded_fonts;
    private System.Windows.Forms.CheckBox UseHidLRAlgorithm;
    private System.Windows.Forms.CheckBox EnableLayerSup_pdfv1_5;
    private System.Windows.Forms.CheckBox ExportOffLay;
    private System.Windows.Forms.CheckBox EncodedSZ;
    private System.Windows.Forms.RadioButton radioButton_All;
    private System.Windows.Forms.RadioButton radioButton_Active;
    private System.Windows.Forms.TextBox PapHeight;
    private System.Windows.Forms.TextBox PapWidth;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox_producer;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox textBox_creator;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox textBox_keywords;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox textBox_subject;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBox_author;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBox_title;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button Export;
    private System.Windows.Forms.Button Cancel;
    private System.Windows.Forms.CheckBox checkBoxMonochrome;
    private System.Windows.Forms.CheckBox checkBoxASCIIHEXencoded;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox HatchToBMP;
  }
}