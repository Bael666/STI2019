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
            this.label1 = new System.Windows.Forms.Label();
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
            this.Banka.Width = 70;
            // 
            // nakup_korelace
            // 
            this.nakup_korelace.Text = "Nákup - korelace";
            this.nakup_korelace.Width = 94;
            // 
            // prodej_korelace
            // 
            this.prodej_korelace.Text = "Prodej - korelace";
            this.prodej_korelace.Width = 92;
            // 
            // nakup_rozptyl
            // 
            this.nakup_rozptyl.Text = "Nákup - rozptyl";
            this.nakup_rozptyl.Width = 83;
            // 
            // prodej_rozptyl
            // 
            this.prodej_rozptyl.Text = "Prodej - rozptyl";
            this.prodej_rozptyl.Width = 82;
            // 
            // nakup_sazba
            // 
            this.nakup_sazba.Text = "Nákup - sazba";
            this.nakup_sazba.Width = 82;
            // 
            // prodej_sazba
            // 
            this.prodej_sazba.Text = "Prodej - sazba";
            this.prodej_sazba.Width = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(370, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Analýza";
            // 
            // Analyza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView);
            this.Name = "Analyza";
            this.Text = "analyza";
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label label1;
    }
}