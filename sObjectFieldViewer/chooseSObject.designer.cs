namespace sObjectFieldViewer
{
    partial class chooseSObject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(chooseSObject));
            this.chooseSOBject_btn = new System.Windows.Forms.Button();
            this.sObject_lbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // chooseSOBject_btn
            // 
            this.chooseSOBject_btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.chooseSOBject_btn.Location = new System.Drawing.Point(79, 256);
            this.chooseSOBject_btn.Name = "chooseSOBject_btn";
            this.chooseSOBject_btn.Size = new System.Drawing.Size(110, 23);
            this.chooseSOBject_btn.TabIndex = 1;
            this.chooseSOBject_btn.Text = "Choose sObject";
            this.chooseSOBject_btn.UseVisualStyleBackColor = true;
            this.chooseSOBject_btn.Click += new System.EventHandler(this.chooseSOBject_btn_Click);
            // 
            // sObject_lbox
            // 
            this.sObject_lbox.FormattingEnabled = true;
            this.sObject_lbox.Location = new System.Drawing.Point(12, 12);
            this.sObject_lbox.Name = "sObject_lbox";
            this.sObject_lbox.Size = new System.Drawing.Size(260, 238);
            this.sObject_lbox.TabIndex = 2;
            // 
            // chooseSObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 286);
            this.Controls.Add(this.sObject_lbox);
            this.Controls.Add(this.chooseSOBject_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "chooseSObject";
            this.Text = "Choose Object";
            this.Load += new System.EventHandler(this.chooseSObject_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button chooseSOBject_btn;
        private System.Windows.Forms.ListBox sObject_lbox;
    }
}