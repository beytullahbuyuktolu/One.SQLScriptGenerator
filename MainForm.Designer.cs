namespace SQLScriptGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Required designer variable.
        /// </summary>

        // Control field declarations
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblDatabases;
        private System.Windows.Forms.CheckedListBox clbDatabases;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.CheckedListBox clbTables;
        private System.Windows.Forms.Label lblScriptType;
        private System.Windows.Forms.ComboBox cboScriptType;
        private System.Windows.Forms.Label lblSourceCompanyCode;
        private System.Windows.Forms.TextBox txtSourceCompanyCode;
        private System.Windows.Forms.Label lblTargetCompanyCode;
        private System.Windows.Forms.TextBox txtTargetCompanyCode;
        private System.Windows.Forms.Button btnGenerateScript;
        private System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.Button btnSaveScript;

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
            lblConnectionStatus = new Label();
            clbDatabases = new CheckedListBox();
            clbTables = new CheckedListBox();
            cboScriptType = new ComboBox();
            lblScriptType = new Label();
            lblSourceCompanyCode = new Label();
            txtSourceCompanyCode = new TextBox();
            lblTargetCompanyCode = new Label();
            txtTargetCompanyCode = new TextBox();
            btnGenerateScript = new Button();
            txtScript = new TextBox();
            btnCopyToClipboard = new Button();
            btnSaveScript = new Button();
            lblDatabases = new Label();
            lblTables = new Label();
            lblScript = new Label();
            SuspendLayout();
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Location = new Point(12, 9);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(126, 20);
            lblConnectionStatus.TabIndex = 0;
            lblConnectionStatus.Text = "Connection status";
            // 
            // clbDatabases
            // 
            clbDatabases.FormattingEnabled = true;
            clbDatabases.Location = new Point(15, 58);
            clbDatabases.Name = "clbDatabases";
            clbDatabases.Size = new Size(270, 136);
            clbDatabases.TabIndex = 2;
            clbDatabases.ItemCheck += clbDatabases_ItemCheck;
            // 
            // clbTables
            // 
            clbTables.FormattingEnabled = true;
            clbTables.Location = new Point(300, 58);
            clbTables.Name = "clbTables";
            clbTables.Size = new Size(270, 136);
            clbTables.TabIndex = 4;
            clbTables.ItemCheck += clbTables_ItemCheck;
            // 
            // cboScriptType
            // 
            cboScriptType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboScriptType.FormattingEnabled = true;
            cboScriptType.Location = new Point(15, 227);
            cboScriptType.Name = "cboScriptType";
            cboScriptType.Size = new Size(200, 28);
            cboScriptType.TabIndex = 6;
            cboScriptType.SelectedIndexChanged += cboScriptType_SelectedIndexChanged;
            // 
            // lblScriptType
            // 
            lblScriptType.AutoSize = true;
            lblScriptType.Location = new Point(19, 202);
            lblScriptType.Name = "lblScriptType";
            lblScriptType.Size = new Size(85, 20);
            lblScriptType.TabIndex = 5;
            lblScriptType.Text = "Script Type:";
            // 
            // lblSourceCompanyCode
            // 
            lblSourceCompanyCode.AutoSize = true;
            lblSourceCompanyCode.Location = new Point(227, 203);
            lblSourceCompanyCode.Name = "lblSourceCompanyCode";
            lblSourceCompanyCode.Size = new Size(163, 20);
            lblSourceCompanyCode.TabIndex = 7;
            lblSourceCompanyCode.Text = "Source Company Code:";
            lblSourceCompanyCode.Visible = false;
            // 
            // txtSourceCompanyCode
            // 
            txtSourceCompanyCode.Location = new Point(224, 227);
            txtSourceCompanyCode.Name = "txtSourceCompanyCode";
            txtSourceCompanyCode.Size = new Size(100, 27);
            txtSourceCompanyCode.TabIndex = 8;
            txtSourceCompanyCode.Visible = false;
            // 
            // lblTargetCompanyCode
            // 
            lblTargetCompanyCode.AutoSize = true;
            lblTargetCompanyCode.Location = new Point(346, 256);
            lblTargetCompanyCode.Name = "lblTargetCompanyCode";
            lblTargetCompanyCode.Size = new Size(159, 20);
            lblTargetCompanyCode.TabIndex = 9;
            lblTargetCompanyCode.Text = "Target Company Code:";
            lblTargetCompanyCode.Visible = false;
            // 
            // txtTargetCompanyCode
            // 
            txtTargetCompanyCode.Location = new Point(346, 227);
            txtTargetCompanyCode.Name = "txtTargetCompanyCode";
            txtTargetCompanyCode.Size = new Size(100, 27);
            txtTargetCompanyCode.TabIndex = 10;
            txtTargetCompanyCode.Visible = false;
            // 
            // btnGenerateScript
            // 
            btnGenerateScript.Location = new Point(452, 227);
            btnGenerateScript.Name = "btnGenerateScript";
            btnGenerateScript.Size = new Size(118, 28);
            btnGenerateScript.TabIndex = 11;
            btnGenerateScript.Text = "Generate Script";
            btnGenerateScript.UseVisualStyleBackColor = true;
            btnGenerateScript.Click += btnGenerateScript_Click;
            // 
            // txtScript
            // 
            txtScript.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtScript.Location = new Point(15, 290);
            txtScript.Multiline = true;
            txtScript.Name = "txtScript";
            txtScript.ScrollBars = ScrollBars.Both;
            txtScript.Size = new Size(555, 194);
            txtScript.TabIndex = 13;
            txtScript.WordWrap = false;
            // 
            // btnCopyToClipboard
            // 
            btnCopyToClipboard.Location = new Point(15, 492);
            btnCopyToClipboard.Name = "btnCopyToClipboard";
            btnCopyToClipboard.Size = new Size(118, 37);
            btnCopyToClipboard.TabIndex = 14;
            btnCopyToClipboard.Text = "Copy to Clipboard";
            btnCopyToClipboard.UseVisualStyleBackColor = true;
            btnCopyToClipboard.Click += btnCopyToClipboard_Click;
            // 
            // btnSaveScript
            // 
            btnSaveScript.Location = new Point(141, 491);
            btnSaveScript.Name = "btnSaveScript";
            btnSaveScript.Size = new Size(118, 37);
            btnSaveScript.TabIndex = 15;
            btnSaveScript.Text = "Save Script";
            btnSaveScript.UseVisualStyleBackColor = true;
            btnSaveScript.Click += btnSaveScript_Click;
            // 
            // lblDatabases
            // 
            lblDatabases.AutoSize = true;
            lblDatabases.Location = new Point(12, 32);
            lblDatabases.Name = "lblDatabases";
            lblDatabases.Size = new Size(147, 20);
            lblDatabases.TabIndex = 1;
            lblDatabases.Text = "Databases on Server:";
            // 
            // lblTables
            // 
            lblTables.AutoSize = true;
            lblTables.Location = new Point(300, 32);
            lblTables.Name = "lblTables";
            lblTables.Size = new Size(136, 20);
            lblTables.TabIndex = 3;
            lblTables.Text = "Tables in Database:";
            // 
            // lblScript
            // 
            lblScript.AutoSize = true;
            lblScript.Location = new Point(18, 263);
            lblScript.Name = "lblScript";
            lblScript.Size = new Size(123, 20);
            lblScript.TabIndex = 12;
            lblScript.Text = "Generated Script:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(644, 559);
            Controls.Add(btnSaveScript);
            Controls.Add(btnCopyToClipboard);
            Controls.Add(txtScript);
            Controls.Add(lblScript);
            Controls.Add(btnGenerateScript);
            Controls.Add(txtTargetCompanyCode);
            Controls.Add(lblTargetCompanyCode);
            Controls.Add(txtSourceCompanyCode);
            Controls.Add(lblSourceCompanyCode);
            Controls.Add(cboScriptType);
            Controls.Add(lblScriptType);
            Controls.Add(clbTables);
            Controls.Add(lblTables);
            Controls.Add(clbDatabases);
            Controls.Add(lblDatabases);
            Controls.Add(lblConnectionStatus);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}