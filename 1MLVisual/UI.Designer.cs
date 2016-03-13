namespace _1MLVisual
{
    partial class UI
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
            this.bmpViewerControl1 = new Lib1MLAI.BmpViewerControl();
            this.bmpViewerControl2 = new Lib1MLAI.BmpViewerControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bmpViewerControl1
            // 
            this.bmpViewerControl1.BackColor = System.Drawing.SystemColors.Control;
            this.bmpViewerControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bmpViewerControl1.Location = new System.Drawing.Point(0, 0);
            this.bmpViewerControl1.Name = "bmpViewerControl1";
            this.bmpViewerControl1.Size = new System.Drawing.Size(461, 461);
            this.bmpViewerControl1.TabIndex = 0;
            // 
            // bmpViewerControl2
            // 
            this.bmpViewerControl2.BackColor = System.Drawing.SystemColors.Control;
            this.bmpViewerControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.bmpViewerControl2.Location = new System.Drawing.Point(461, 0);
            this.bmpViewerControl2.Name = "bmpViewerControl2";
            this.bmpViewerControl2.Size = new System.Drawing.Size(276, 100);
            this.bmpViewerControl2.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Black;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(461, 133);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(276, 328);
            this.listBox1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.bmpViewerControl2);
            this.panel1.Controls.Add(this.bmpViewerControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(737, 461);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(461, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(276, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(461, 123);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(276, 10);
            this.progressBar1.TabIndex = 3;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 461);
            this.Controls.Add(this.panel1);
            this.Name = "UI";
            this.Text = "UI";
            this.Load += new System.EventHandler(this.UI_Load);
            this.Resize += new System.EventHandler(this.UI_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Lib1MLAI.BmpViewerControl bmpViewerControl1;
        public Lib1MLAI.BmpViewerControl bmpViewerControl2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ProgressBar progressBar1;
    }
}