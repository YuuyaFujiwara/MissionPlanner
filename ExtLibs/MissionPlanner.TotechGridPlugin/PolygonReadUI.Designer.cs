namespace MissionPlanner.TotechGrid
{
    partial class PolygonReadUI
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";



            this.BUT_Close = new MissionPlanner.Controls.MyButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.map = new MissionPlanner.Controls.myGMAP();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BUT_Close
            // 
            this.BUT_Close.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_Close.Location = new System.Drawing.Point(590, 507);
            this.BUT_Close.Name = "BUT_Close";
            this.BUT_Close.Size = new System.Drawing.Size(138, 23);
            this.BUT_Close.TabIndex = 63;
            this.BUT_Close.Text = "閉じる";
            this.BUT_Close.UseVisualStyleBackColor = true;
            this.BUT_Close.Click += new System.EventHandler(this.BUT_Close_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.map);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 518);
            this.panel1.TabIndex = 64;
            // 
            // map
            // 
            this.map.Bearing = 0F;
            this.map.CanDragMap = true;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.EmptyTileColor = System.Drawing.Color.Gray;
            this.map.GrayScaleMode = false;
            this.map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.map.HoldInvalidation = false;
            this.map.LevelsKeepInMemmory = 5;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.MarkersEnabled = true;
            this.map.MaxZoom = 19;
            this.map.MinZoom = 2;
            this.map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.map.Name = "map";
            this.map.NegativeMode = false;
            this.map.PolygonsEnabled = true;
            this.map.RetryLoadTile = 0;
            this.map.RoutesEnabled = true;
            this.map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.map.ShowTileGridLines = false;
            this.map.Size = new System.Drawing.Size(572, 518);
            this.map.TabIndex = 1;
            this.map.Zoom = 3D;
            // 
            // PolygonReadUI
            // 
            this.ClientSize = new System.Drawing.Size(741, 560);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BUT_Close);
            this.Name = "PolygonReadUI";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
    }
}