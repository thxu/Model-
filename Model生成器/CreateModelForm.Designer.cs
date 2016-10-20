namespace Model生成器
{
    partial class CreateModelForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenXls = new System.Windows.Forms.Button();
            this.CreateModel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labXlsPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpenXls
            // 
            this.btnOpenXls.Location = new System.Drawing.Point(72, 66);
            this.btnOpenXls.Name = "btnOpenXls";
            this.btnOpenXls.Size = new System.Drawing.Size(123, 23);
            this.btnOpenXls.TabIndex = 0;
            this.btnOpenXls.Text = "打开xls文件";
            this.btnOpenXls.UseVisualStyleBackColor = true;
            this.btnOpenXls.Click += new System.EventHandler(this.btnOpenXls_Click);
            // 
            // CreateModel
            // 
            this.CreateModel.Location = new System.Drawing.Point(72, 177);
            this.CreateModel.Name = "CreateModel";
            this.CreateModel.Size = new System.Drawing.Size(123, 23);
            this.CreateModel.TabIndex = 1;
            this.CreateModel.Text = "生成Model";
            this.CreateModel.UseVisualStyleBackColor = true;
            this.CreateModel.Click += new System.EventHandler(this.CreateModel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件路径：";
            // 
            // labXlsPath
            // 
            this.labXlsPath.AutoSize = true;
            this.labXlsPath.Location = new System.Drawing.Point(130, 119);
            this.labXlsPath.Name = "labXlsPath";
            this.labXlsPath.Size = new System.Drawing.Size(0, 12);
            this.labXlsPath.TabIndex = 3;
            // 
            // CreateModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.labXlsPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateModel);
            this.Controls.Add(this.btnOpenXls);
            this.Name = "CreateModelForm";
            this.Text = "Model生成器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenXls;
        private System.Windows.Forms.Button CreateModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labXlsPath;
    }
}

