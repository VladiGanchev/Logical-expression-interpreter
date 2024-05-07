namespace WinFormsApp.Project
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            comboBoxTrees = new ComboBox();
            treeView1 = new TreeView();
            SuspendLayout();
            // 
            // comboBoxTrees
            // 
            comboBoxTrees.FormattingEnabled = true;
            comboBoxTrees.Location = new Point(261, 66);
            comboBoxTrees.Name = "comboBoxTrees";
            comboBoxTrees.Size = new Size(151, 28);
            comboBoxTrees.TabIndex = 0;
            comboBoxTrees.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(261, 159);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(151, 121);
            treeView1.TabIndex = 1;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(treeView1);
            Controls.Add(comboBoxTrees);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ComboBox comboBoxTrees;
        private TreeView treeView1;
    }
}