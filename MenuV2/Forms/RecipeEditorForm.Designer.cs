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
        private System.Windows.Forms.TextBox txtCategory;

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
            this.txtCategory = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(20, 55);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(380, 39);
            this.txtName.TabIndex = 0;
            // 
            // picPhoto
            // 
            this.picPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPhoto.Location = new System.Drawing.Point(633, 4);
            this.picPhoto.Name = "picPhoto";
            this.picPhoto.Size = new System.Drawing.Size(250, 250);
            this.picPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPhoto.TabIndex = 2;
            this.picPhoto.TabStop = false;
            // 
            // btnSelectPhoto
            // 
            this.btnSelectPhoto.Location = new System.Drawing.Point(377, 4);
            this.btnSelectPhoto.Name = "btnSelectPhoto";
            this.btnSelectPhoto.Size = new System.Drawing.Size(250, 40);
            this.btnSelectPhoto.TabIndex = 3;
            this.btnSelectPhoto.Text = "Выбрать фото";
            this.btnSelectPhoto.Click += new System.EventHandler(this.btnSelectPhoto_Click);
            // 
            // txtVideo
            // 
            this.txtVideo.Location = new System.Drawing.Point(20, 135);
            this.txtVideo.Name = "txtVideo";
            this.txtVideo.Size = new System.Drawing.Size(380, 39);
            this.txtVideo.TabIndex = 4;
            // 
            // btnLoadYoutube
            // 
            this.btnLoadYoutube.AutoSize = true;
            this.btnLoadYoutube.Location = new System.Drawing.Point(456, 132);
            this.btnLoadYoutube.Name = "btnLoadYoutube";
            this.btnLoadYoutube.Size = new System.Drawing.Size(131, 42);
            this.btnLoadYoutube.TabIndex = 6;
            this.btnLoadYoutube.Text = "Загрузить";
            this.btnLoadYoutube.Click += new System.EventHandler(this.btnLoadYoutube_Click);
            // 
            // txtInstructions
            // 
            this.txtInstructions.Location = new System.Drawing.Point(26, 312);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstructions.Size = new System.Drawing.Size(380, 439);
            this.txtInstructions.TabIndex = 9;
            // 
            // txtIngredients
            // 
            this.txtIngredients.Location = new System.Drawing.Point(633, 312);
            this.txtIngredients.Multiline = true;
            this.txtIngredients.Name = "txtIngredients";
            this.txtIngredients.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIngredients.Size = new System.Drawing.Size(380, 439);
            this.txtIngredients.TabIndex = 11;
            // 
            // btnParseIngredients
            // 
            this.btnParseIngredients.AutoSize = true;
            this.btnParseIngredients.Location = new System.Drawing.Point(456, 371);
            this.btnParseIngredients.Name = "btnParseIngredients";
            this.btnParseIngredients.Size = new System.Drawing.Size(120, 42);
            this.btnParseIngredients.TabIndex = 13;
            this.btnParseIngredients.Text = "Парсить";
            this.btnParseIngredients.Click += new System.EventHandler(this.btnParseIngredients_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(456, 435);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 40);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(456, 495);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(125, 32);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Название:";
            // 
            // lblVideo
            // 
            this.lblVideo.AutoSize = true;
            this.lblVideo.Location = new System.Drawing.Point(20, 105);
            this.lblVideo.Name = "lblVideo";
            this.lblVideo.Size = new System.Drawing.Size(87, 32);
            this.lblVideo.TabIndex = 5;
            this.lblVideo.Text = "Видео:";
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(20, 266);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(276, 32);
            this.lblInstructions.TabIndex = 10;
            this.lblInstructions.Text = "Способ приготовления:";
            // 
            // lblIngredients
            // 
            this.lblIngredients.AutoSize = true;
            this.lblIngredients.Location = new System.Drawing.Point(641, 266);
            this.lblIngredients.Name = "lblIngredients";
            this.lblIngredients.Size = new System.Drawing.Size(168, 32);
            this.lblIngredients.TabIndex = 12;
            this.lblIngredients.Text = "Ингредиенты:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 185);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(131, 32);
            this.lblCategory.TabIndex = 8;
            this.lblCategory.Text = "Категория:";
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(20, 215);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(380, 39);
            this.txtCategory.TabIndex = 7;
            // 
            // RecipeEditorForm
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1025, 840);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.picPhoto);
            this.Controls.Add(this.btnSelectPhoto);
            this.Controls.Add(this.txtVideo);
            this.Controls.Add(this.lblVideo);
            this.Controls.Add(this.btnLoadYoutube);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtInstructions);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.txtIngredients);
            this.Controls.Add(this.lblIngredients);
            this.Controls.Add(this.btnParseIngredients);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Name = "RecipeEditorForm";
            this.Text = "Редактор рецепта";
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
