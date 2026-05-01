namespace MenuV2.Forms
{
    partial class RecipeEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox picPhoto;
        private System.Windows.Forms.Button btnSelectPhoto;
        private System.Windows.Forms.TextBox txtVideo;
        private System.Windows.Forms.Button btnLoadYoutube;
        private System.Windows.Forms.TextBox txtInstructions;
        private System.Windows.Forms.TextBox txtIngredients;
        private System.Windows.Forms.Button btnParseIngredients;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblVideo;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblIngredients;
        private System.Windows.Forms.Label lblCategory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.picPhoto = new System.Windows.Forms.PictureBox();
            this.btnSelectPhoto = new System.Windows.Forms.Button();
            this.txtVideo = new System.Windows.Forms.TextBox();
            this.btnLoadYoutube = new System.Windows.Forms.Button();
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.txtIngredients = new System.Windows.Forms.TextBox();
            this.btnParseIngredients = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblVideo = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblIngredients = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnAddCategory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtName.Location = new System.Drawing.Point(20, 46);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(350, 34);
            this.txtName.TabIndex = 0;
            // 
            // picPhoto
            // 
            this.picPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPhoto.Location = new System.Drawing.Point(766, 12);
            this.picPhoto.Name = "picPhoto";
            this.picPhoto.Size = new System.Drawing.Size(200, 197);
            this.picPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPhoto.TabIndex = 2;
            this.picPhoto.TabStop = false;
            // 
            // btnSelectPhoto
            // 
            this.btnSelectPhoto.AutoSize = true;
            this.btnSelectPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectPhoto.Location = new System.Drawing.Point(411, 41);
            this.btnSelectPhoto.Name = "btnSelectPhoto";
            this.btnSelectPhoto.Size = new System.Drawing.Size(188, 39);
            this.btnSelectPhoto.TabIndex = 3;
            this.btnSelectPhoto.Text = "Выбрать фото";
            this.btnSelectPhoto.Click += new System.EventHandler(this.btnSelectPhoto_Click);
            // 
            // txtVideo
            // 
            this.txtVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtVideo.Location = new System.Drawing.Point(20, 109);
            this.txtVideo.Name = "txtVideo";
            this.txtVideo.Size = new System.Drawing.Size(350, 34);
            this.txtVideo.TabIndex = 4;
            // 
            // btnLoadYoutube
            // 
            this.btnLoadYoutube.AutoSize = true;
            this.btnLoadYoutube.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoadYoutube.Location = new System.Drawing.Point(411, 109);
            this.btnLoadYoutube.Name = "btnLoadYoutube";
            this.btnLoadYoutube.Size = new System.Drawing.Size(139, 39);
            this.btnLoadYoutube.TabIndex = 6;
            this.btnLoadYoutube.Text = "Загрузить";
            this.btnLoadYoutube.Click += new System.EventHandler(this.btnLoadYoutube_Click);
            // 
            // txtInstructions
            // 
            this.txtInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInstructions.Location = new System.Drawing.Point(410, 254);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstructions.Size = new System.Drawing.Size(350, 405);
            this.txtInstructions.TabIndex = 9;
            // 
            // txtIngredients
            // 
            this.txtIngredients.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtIngredients.Location = new System.Drawing.Point(20, 254);
            this.txtIngredients.Multiline = true;
            this.txtIngredients.Name = "txtIngredients";
            this.txtIngredients.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIngredients.Size = new System.Drawing.Size(350, 408);
            this.txtIngredients.TabIndex = 11;
            // 
            // btnParseIngredients
            // 
            this.btnParseIngredients.AutoSize = true;
            this.btnParseIngredients.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnParseIngredients.Location = new System.Drawing.Point(769, 254);
            this.btnParseIngredients.Name = "btnParseIngredients";
            this.btnParseIngredients.Size = new System.Drawing.Size(117, 39);
            this.btnParseIngredients.TabIndex = 13;
            this.btnParseIngredients.Text = "Парсить";
            this.btnParseIngredients.Click += new System.EventHandler(this.btnParseIngredients_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave.Location = new System.Drawing.Point(766, 312);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(143, 39);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(769, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 39);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblName.Location = new System.Drawing.Point(15, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(128, 29);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Название:";
            // 
            // lblVideo
            // 
            this.lblVideo.AutoSize = true;
            this.lblVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVideo.Location = new System.Drawing.Point(15, 80);
            this.lblVideo.Name = "lblVideo";
            this.lblVideo.Size = new System.Drawing.Size(91, 29);
            this.lblVideo.TabIndex = 5;
            this.lblVideo.Text = "Видео:";
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblInstructions.Location = new System.Drawing.Point(406, 215);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(151, 29);
            this.lblInstructions.TabIndex = 10;
            this.lblInstructions.Text = "Инструкция:";
            // 
            // lblIngredients
            // 
            this.lblIngredients.AutoSize = true;
            this.lblIngredients.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblIngredients.Location = new System.Drawing.Point(15, 215);
            this.lblIngredients.Name = "lblIngredients";
            this.lblIngredients.Size = new System.Drawing.Size(172, 29);
            this.lblIngredients.TabIndex = 12;
            this.lblIngredients.Text = "Ингредиенты:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCategory.Location = new System.Drawing.Point(15, 143);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(140, 29);
            this.lblCategory.TabIndex = 8;
            this.lblCategory.Text = "Категория:";
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Location = new System.Drawing.Point(820, 470);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(75, 23);
            this.webView.TabIndex = 16;
            this.webView.Visible = false;
            this.webView.ZoomFactor = 1D;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(20, 172);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(350, 37);
            this.cmbCategory.TabIndex = 17;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.AutoSize = true;
            this.btnAddCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddCategory.Location = new System.Drawing.Point(411, 172);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(123, 39);
            this.btnAddCategory.TabIndex = 18;
            this.btnAddCategory.Text = "+";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // RecipeEditorForm
            // 
            this.ClientSize = new System.Drawing.Size(992, 718);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.webView);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.picPhoto);
            this.Controls.Add(this.btnSelectPhoto);
            this.Controls.Add(this.txtVideo);
            this.Controls.Add(this.lblVideo);
            this.Controls.Add(this.btnLoadYoutube);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtInstructions);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.txtIngredients);
            this.Controls.Add(this.lblIngredients);
            this.Controls.Add(this.btnParseIngredients);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "RecipeEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор рецепта";
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnAddCategory;
    }
}
