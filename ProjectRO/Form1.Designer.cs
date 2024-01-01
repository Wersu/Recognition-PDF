namespace ProjectRO
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            richTextBoxInput = new RichTextBox();
            richTextBoxOutput = new RichTextBox();
            buttonRecognize = new Button();
            buttonOpen = new Button();
            openFileDialog1 = new OpenFileDialog();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            toolStripComboBox1 = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            outputTypeSelector = new ToolStripComboBox();
            savePathButton = new ToolStripButton();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(103, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenFileToolStrip_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(richTextBoxInput, 0, 0);
            tableLayoutPanel1.Controls.Add(richTextBoxOutput, 1, 0);
            tableLayoutPanel1.Controls.Add(buttonRecognize, 0, 1);
            tableLayoutPanel1.Controls.Add(buttonOpen, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 83F));
            tableLayoutPanel1.Size = new Size(800, 445);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // richTextBoxInput
            // 
            richTextBoxInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            richTextBoxInput.BackColor = SystemColors.ControlLightLight;
            richTextBoxInput.Location = new Point(3, 40);
            richTextBoxInput.Margin = new Padding(3, 40, 3, 3);
            richTextBoxInput.Name = "richTextBoxInput";
            richTextBoxInput.ReadOnly = true;
            richTextBoxInput.Size = new Size(394, 319);
            richTextBoxInput.TabIndex = 2;
            richTextBoxInput.Text = "";
            // 
            // richTextBoxOutput
            // 
            richTextBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            richTextBoxOutput.BackColor = SystemColors.ControlLightLight;
            richTextBoxOutput.Location = new Point(403, 40);
            richTextBoxOutput.Margin = new Padding(3, 40, 3, 3);
            richTextBoxOutput.Name = "richTextBoxOutput";
            richTextBoxOutput.ReadOnly = true;
            richTextBoxOutput.Size = new Size(394, 319);
            richTextBoxOutput.TabIndex = 1;
            richTextBoxOutput.Text = "";
            // 
            // buttonRecognize
            // 
            buttonRecognize.Anchor = AnchorStyles.None;
            buttonRecognize.BackColor = SystemColors.ActiveBorder;
            buttonRecognize.Enabled = false;
            buttonRecognize.Location = new Point(91, 385);
            buttonRecognize.Name = "buttonRecognize";
            buttonRecognize.Size = new Size(217, 37);
            buttonRecognize.TabIndex = 3;
            buttonRecognize.Text = "Recognize";
            buttonRecognize.UseVisualStyleBackColor = false;
            buttonRecognize.Click += ButtonRecognize_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.Anchor = AnchorStyles.None;
            buttonOpen.BackColor = SystemColors.ActiveBorder;
            buttonOpen.Enabled = false;
            buttonOpen.Location = new Point(491, 385);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(217, 37);
            buttonOpen.TabIndex = 4;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = false;
            buttonOpen.Click += ButtonOpen_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "PDF|*.pdf";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripComboBox1, toolStripSeparator1, toolStripLabel2, outputTypeSelector, savePathButton });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Margin = new Padding(0, 5, 0, 5);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 33);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(62, 30);
            toolStripLabel1.Text = "Language:";
            // 
            // toolStripComboBox1
            // 
            toolStripComboBox1.AutoToolTip = true;
            toolStripComboBox1.Items.AddRange(new object[] { "English" });
            toolStripComboBox1.Margin = new Padding(1, 5, 1, 5);
            toolStripComboBox1.Name = "toolStripComboBox1";
            toolStripComboBox1.Size = new Size(121, 23);
            toolStripComboBox1.Tag = "";
            toolStripComboBox1.Text = "English";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 33);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(90, 30);
            toolStripLabel2.Text = "Output file type";
            // 
            // outputTypeSelector
            // 
            outputTypeSelector.Items.AddRange(new object[] { "PDF", "DOCX" });
            outputTypeSelector.Name = "outputTypeSelector";
            outputTypeSelector.Size = new Size(121, 33);
            outputTypeSelector.Text = "PDF";
            outputTypeSelector.SelectedIndexChanged += OutputTypeSelector_Select;
            // 
            // savePathButton
            // 
            savePathButton.Alignment = ToolStripItemAlignment.Right;
            savePathButton.AutoSize = false;
            savePathButton.BackColor = SystemColors.GradientActiveCaption;
            savePathButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            savePathButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            savePathButton.ForeColor = SystemColors.ActiveCaptionText;
            savePathButton.ImageTransparentColor = Color.Magenta;
            savePathButton.Margin = new Padding(0, 1, 10, 2);
            savePathButton.Name = "savePathButton";
            savePathButton.Size = new Size(100, 25);
            savePathButton.Text = "Change folder";
            savePathButton.TextImageRelation = TextImageRelation.Overlay;
            savePathButton.Click += OutputToolStrip_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 469);
            Controls.Add(toolStrip1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "File PDF recognition system";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private RichTextBox richTextBoxOutput;
        private OpenFileDialog openFileDialog1;
        private RichTextBox richTextBoxInput;
        private Button buttonRecognize;
        private Button buttonOpen;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox outputTypeSelector;
        private ToolStripButton savePathButton;
    }
}
