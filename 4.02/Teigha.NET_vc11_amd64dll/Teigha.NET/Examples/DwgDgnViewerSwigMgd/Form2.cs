using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DwgViewer
{
    public partial class FormPrompt : Form
    {
        public FormPrompt()
        {
            InitializeComponent();
        }
        public void setPrompt(String val)
        {
            labelPrompt.Text = val;
        }
        public String getValue()
        {
            return inputValue.Text;
        }
    }
}
