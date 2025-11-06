namespace ResistorsManager
{
    partial class MainForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            btnLoadTxt = new Button();
            btnExportToDb = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 41);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(1429, 469);
            dataGridView.TabIndex = 0;
            // 
            // btnLoadTxt
            // 
            btnLoadTxt.Location = new Point(12, 12);
            btnLoadTxt.Name = "btnLoadTxt";
            btnLoadTxt.Size = new Size(231, 23);
            btnLoadTxt.TabIndex = 1;
            btnLoadTxt.Text = "Загрузить данные из текстового файла";
            btnLoadTxt.UseVisualStyleBackColor = true;
            btnLoadTxt.Click += btnLoadTxt_Click;
            // 
            // btnExportToDb
            // 
            btnExportToDb.Location = new Point(249, 12);
            btnExportToDb.Name = "btnExportToDb";
            btnExportToDb.Size = new Size(247, 23);
            btnExportToDb.TabIndex = 2;
            btnExportToDb.Text = "Экспорт в базу даных";
            btnExportToDb.UseVisualStyleBackColor = true;
            btnExportToDb.Click += btnExportToDb_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1453, 522);
            Controls.Add(btnExportToDb);
            Controls.Add(btnLoadTxt);
            Controls.Add(dataGridView);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
        private Button btnLoadTxt;
        private Button btnExportToDb;
    }
}
