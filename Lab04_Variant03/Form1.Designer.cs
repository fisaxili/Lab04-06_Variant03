namespace Lab04_Variant03
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelLeft = new Panel();
            lblTitle = new Label();
            btnLoad = new Button();
            grpBfs = new GroupBox();
            lblBfsStart = new Label();
            cmbBfsStart = new ComboBox();
            btnBfs = new Button();
            grpDfs = new GroupBox();
            lblDfsStart = new Label();
            cmbDfsStart = new ComboBox();
            btnDfs = new Button();
            grpReach = new GroupBox();
            lblReachFrom = new Label();
            cmbReachFrom = new ComboBox();
            lblReachTo = new Label();
            cmbReachTo = new ComboBox();
            btnReach = new Button();
            btnComponents = new Button();
            btnClear = new Button();
            txtOutput = new RichTextBox();
            lblOutput = new Label();
            panelLeft.SuspendLayout();
            grpBfs.SuspendLayout();
            grpDfs.SuspendLayout();
            grpReach.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BorderStyle = BorderStyle.FixedSingle;
            panelLeft.Controls.Add(lblTitle);
            panelLeft.Controls.Add(btnLoad);
            panelLeft.Controls.Add(grpBfs);
            panelLeft.Controls.Add(grpDfs);
            panelLeft.Controls.Add(grpReach);
            panelLeft.Controls.Add(btnComponents);
            panelLeft.Controls.Add(btnClear);
            panelLeft.Location = new Point(10, 10);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(310, 632);
            panelLeft.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(285, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Дорожная сеть района";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.SteelBlue;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.ForeColor = Color.White;
            btnLoad.Location = new Point(10, 58);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(285, 34);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Загрузить граф из файла";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // grpBfs
            // 
            grpBfs.Controls.Add(lblBfsStart);
            grpBfs.Controls.Add(cmbBfsStart);
            grpBfs.Controls.Add(btnBfs);
            grpBfs.Location = new Point(10, 105);
            grpBfs.Name = "grpBfs";
            grpBfs.Size = new Size(285, 110);
            grpBfs.TabIndex = 2;
            grpBfs.TabStop = false;
            grpBfs.Text = "BFS — обход в ширину";
            // 
            // lblBfsStart
            // 
            lblBfsStart.Location = new Point(8, 25);
            lblBfsStart.Name = "lblBfsStart";
            lblBfsStart.Size = new Size(150, 22);
            lblBfsStart.TabIndex = 0;
            lblBfsStart.Text = "Стартовая вершина:";
            // 
            // cmbBfsStart
            // 
            cmbBfsStart.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBfsStart.Location = new Point(8, 48);
            cmbBfsStart.Name = "cmbBfsStart";
            cmbBfsStart.Size = new Size(265, 27);
            cmbBfsStart.TabIndex = 1;
            // 
            // btnBfs
            // 
            btnBfs.BackColor = Color.DarkSeaGreen;
            btnBfs.FlatStyle = FlatStyle.Flat;
            btnBfs.ForeColor = Color.White;
            btnBfs.Location = new Point(8, 78);
            btnBfs.Name = "btnBfs";
            btnBfs.Size = new Size(265, 26);
            btnBfs.TabIndex = 2;
            btnBfs.Text = "Запустить BFS";
            btnBfs.UseVisualStyleBackColor = false;
            btnBfs.Click += btnBfs_Click;
            // 
            // grpDfs
            // 
            grpDfs.Controls.Add(lblDfsStart);
            grpDfs.Controls.Add(cmbDfsStart);
            grpDfs.Controls.Add(btnDfs);
            grpDfs.Location = new Point(10, 225);
            grpDfs.Name = "grpDfs";
            grpDfs.Size = new Size(285, 110);
            grpDfs.TabIndex = 3;
            grpDfs.TabStop = false;
            grpDfs.Text = "DFS — обход в глубину";
            // 
            // lblDfsStart
            // 
            lblDfsStart.Location = new Point(8, 25);
            lblDfsStart.Name = "lblDfsStart";
            lblDfsStart.Size = new Size(150, 22);
            lblDfsStart.TabIndex = 0;
            lblDfsStart.Text = "Стартовая вершина:";
            // 
            // cmbDfsStart
            // 
            cmbDfsStart.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDfsStart.Location = new Point(8, 48);
            cmbDfsStart.Name = "cmbDfsStart";
            cmbDfsStart.Size = new Size(265, 27);
            cmbDfsStart.TabIndex = 1;
            // 
            // btnDfs
            // 
            btnDfs.BackColor = Color.DarkSeaGreen;
            btnDfs.FlatStyle = FlatStyle.Flat;
            btnDfs.ForeColor = Color.White;
            btnDfs.Location = new Point(8, 78);
            btnDfs.Name = "btnDfs";
            btnDfs.Size = new Size(265, 26);
            btnDfs.TabIndex = 2;
            btnDfs.Text = "Запустить DFS";
            btnDfs.UseVisualStyleBackColor = false;
            btnDfs.Click += btnDfs_Click;
            // 
            // grpReach
            // 
            grpReach.Controls.Add(lblReachFrom);
            grpReach.Controls.Add(cmbReachFrom);
            grpReach.Controls.Add(lblReachTo);
            grpReach.Controls.Add(cmbReachTo);
            grpReach.Controls.Add(btnReach);
            grpReach.Location = new Point(10, 345);
            grpReach.Name = "grpReach";
            grpReach.Size = new Size(285, 165);
            grpReach.TabIndex = 4;
            grpReach.TabStop = false;
            grpReach.Text = "Проверка достижимости (BFS)";
            // 
            // lblReachFrom
            // 
            lblReachFrom.Location = new Point(8, 25);
            lblReachFrom.Name = "lblReachFrom";
            lblReachFrom.Size = new Size(100, 22);
            lblReachFrom.TabIndex = 0;
            lblReachFrom.Text = "Из вершины:";
            // 
            // cmbReachFrom
            // 
            cmbReachFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReachFrom.Location = new Point(8, 48);
            cmbReachFrom.Name = "cmbReachFrom";
            cmbReachFrom.Size = new Size(265, 27);
            cmbReachFrom.TabIndex = 1;
            // 
            // lblReachTo
            // 
            lblReachTo.Location = new Point(8, 78);
            lblReachTo.Name = "lblReachTo";
            lblReachTo.Size = new Size(100, 22);
            lblReachTo.TabIndex = 2;
            lblReachTo.Text = "В вершину:";
            // 
            // cmbReachTo
            // 
            cmbReachTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReachTo.Location = new Point(8, 100);
            cmbReachTo.Name = "cmbReachTo";
            cmbReachTo.Size = new Size(265, 27);
            cmbReachTo.TabIndex = 3;
            // 
            // btnReach
            // 
            btnReach.BackColor = Color.CornflowerBlue;
            btnReach.FlatStyle = FlatStyle.Flat;
            btnReach.ForeColor = Color.White;
            btnReach.Location = new Point(8, 133);
            btnReach.Name = "btnReach";
            btnReach.Size = new Size(265, 26);
            btnReach.TabIndex = 4;
            btnReach.Text = "Проверить достижимость";
            btnReach.UseVisualStyleBackColor = false;
            btnReach.Click += btnReach_Click;
            // 
            // btnComponents
            // 
            btnComponents.BackColor = Color.DarkOrange;
            btnComponents.FlatStyle = FlatStyle.Flat;
            btnComponents.ForeColor = Color.White;
            btnComponents.Location = new Point(10, 528);
            btnComponents.Name = "btnComponents";
            btnComponents.Size = new Size(285, 34);
            btnComponents.TabIndex = 5;
            btnComponents.Text = "Компоненты связности";
            btnComponents.UseVisualStyleBackColor = false;
            btnComponents.Click += btnComponents_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.Gray;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(10, 568);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(285, 34);
            btnClear.TabIndex = 6;
            btnClear.Text = "Очистить вывод";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // txtOutput
            // 
            txtOutput.BackColor = Color.FromArgb(30, 30, 30);
            txtOutput.BorderStyle = BorderStyle.FixedSingle;
            txtOutput.Font = new Font("Consolas", 9.5F);
            txtOutput.ForeColor = Color.LightGreen;
            txtOutput.Location = new Point(330, 35);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtOutput.Size = new Size(655, 590);
            txtOutput.TabIndex = 2;
            txtOutput.Text = "";
            // 
            // lblOutput
            // 
            lblOutput.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            lblOutput.Location = new Point(330, 10);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(120, 22);
            lblOutput.TabIndex = 1;
            lblOutput.Text = "Результаты:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1002, 676);
            Controls.Add(panelLeft);
            Controls.Add(lblOutput);
            Controls.Add(txtOutput);
            Font = new Font("Times New Roman", 10F);
            MinimumSize = new Size(1020, 690);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ЛР №4 — Дорожная сеть района (Вариант 3)";
            panelLeft.ResumeLayout(false);
            grpBfs.ResumeLayout(false);
            grpDfs.ResumeLayout(false);
            grpReach.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        //Fields 
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox grpBfs;
        private System.Windows.Forms.Label lblBfsStart;
        private System.Windows.Forms.ComboBox cmbBfsStart;
        private System.Windows.Forms.Button btnBfs;
        private System.Windows.Forms.GroupBox grpDfs;
        private System.Windows.Forms.Label lblDfsStart;
        private System.Windows.Forms.ComboBox cmbDfsStart;
        private System.Windows.Forms.Button btnDfs;
        private System.Windows.Forms.GroupBox grpReach;
        private System.Windows.Forms.Label lblReachFrom;
        private System.Windows.Forms.ComboBox cmbReachFrom;
        private System.Windows.Forms.Label lblReachTo;
        private System.Windows.Forms.ComboBox cmbReachTo;
        private System.Windows.Forms.Button btnReach;
        private System.Windows.Forms.Button btnComponents;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.RichTextBox txtOutput;
    }
}

