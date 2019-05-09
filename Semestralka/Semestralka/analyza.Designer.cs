namespace Semestralka {
    partial class Analyza {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.listView = new System.Windows.Forms.ListView();
            this.Banka = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nakup_korelace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.prodej_korelace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nakup_rozptyl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.prodej_rozptyl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nakup_sazba = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.prodej_sazba = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Banka,
            this.nakup_korelace,
            this.prodej_korelace,
            this.nakup_rozptyl,
            this.prodej_rozptyl,
            this.nakup_sazba,
            this.prodej_sazba});
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.Location = new System.Drawing.Point(0, 56);
            this.listView.Name = "listView";
            this.listView.Scrollable = false;
            this.listView.Size = new System.Drawing.Size(800, 175);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // Banka
            // 
            this.Banka.Text = "Banka";
            // 
            // nakup_korelace
            // 
            this.nakup_korelace.Text = "Nákup - korelace";
            // 
            // prodej_korelace
            // 
            this.prodej_korelace.Text = "Prodej - korelace";
            // 
            // nakup_rozptyl
            // 
            this.nakup_rozptyl.Text = "Nákup - rozptyl";
            // 
            // prodej_rozptyl
            // 
            this.prodej_rozptyl.Text = "Prodej - rozptyl";
            // 
            // nakup_sazba
            // 
            this.nakup_sazba.Text = "Nákup - sazba";
            // 
            // prodej_sazba
            // 
            this.prodej_sazba.Text = "Prodej - sazba";
            this.prodej_sazba.Width = 80;
            // 
            // Analyza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listView);
            this.Name = "Analyza";
            this.Text = "analyza";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader Banka;
        private System.Windows.Forms.ColumnHeader nakup_korelace;
        private System.Windows.Forms.ColumnHeader prodej_korelace;
        private System.Windows.Forms.ColumnHeader nakup_rozptyl;
        private System.Windows.Forms.ColumnHeader prodej_rozptyl;
        private System.Windows.Forms.ColumnHeader nakup_sazba;
        private System.Windows.Forms.ColumnHeader prodej_sazba;
    }
}