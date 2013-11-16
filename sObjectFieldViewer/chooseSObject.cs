using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sObjectFieldViewer
{
    public partial class chooseSObject : Form
    {
        private string sObject;
        private ArrayList sObjects;

        public chooseSObject()
        {
            InitializeComponent();
            sObject = null;
        }

        public ArrayList getSObjects()
        {
            return sObjects;
        }
        public void setSObjects(ArrayList _sObjects)
        {
            sObjects = _sObjects;
        }

        public string getSObject()
        {
            return sObject;
        }
        public void setSObject(string sObject_str)
        {
            sObject = sObject_str;
        }

        private void chooseSObject_Load(object sender, EventArgs e)
        {
            foreach (string strSObject in sObjects)
            {
                sObject_lbox.Items.Add(strSObject);
            }
        }

        private void chooseSOBject_btn_Click(object sender, EventArgs e)
        {
            setSObject(sObject_lbox.SelectedItem.ToString());
        }
    }
}
