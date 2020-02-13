namespace MissionPlanner.TotechGrid
{
    partial class TotechGridUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotechGridUI));
            this.map = new MissionPlanner.Controls.myGMAP();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbl_distbetweenlines = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lbl_strips = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.lbl_distance = new System.Windows.Forms.Label();
            this.lbl_area = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabRouteCalc = new System.Windows.Forms.TabPage();
            this.BUT_Test2 = new MissionPlanner.Controls.MyButton();
            this.BUT_Test1 = new MissionPlanner.Controls.MyButton();
            this.BUT_newDialog = new MissionPlanner.Controls.MyButton();
            this.BUT_PolygonRead = new MissionPlanner.Controls.MyButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RBT_Spiral = new System.Windows.Forms.RadioButton();
            this.RBT_Zigzag = new System.Windows.Forms.RadioButton();
            this.BUT_reverse = new MissionPlanner.Controls.MyButton();
            this.BUT_StartNext = new MissionPlanner.Controls.MyButton();
            this.BUT_StartPrev = new MissionPlanner.Controls.MyButton();
            this.label3 = new System.Windows.Forms.Label();
            this.NUM_divide_step = new System.Windows.Forms.NumericUpDown();
            this.NUM_margin = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NUM_Distance = new System.Windows.Forms.NumericUpDown();
            this.NUM_uav_radius = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CHK_internals = new System.Windows.Forms.CheckBox();
            this.chk_grid = new System.Windows.Forms.CheckBox();
            this.chk_markers = new System.Windows.Forms.CheckBox();
            this.chk_boundary = new System.Windows.Forms.CheckBox();
            this.CMB_fieldshapes = new System.Windows.Forms.ComboBox();
            this.BUT_SaveRoute = new MissionPlanner.Controls.MyButton();
            this.BUT_Accept = new MissionPlanner.Controls.MyButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRouteManage = new System.Windows.Forms.TabPage();
            this.BUT_RouteDelete = new MissionPlanner.Controls.MyButton();
            this.BUT_RouteList_Update = new MissionPlanner.Controls.MyButton();
            this.label4 = new System.Windows.Forms.Label();
            this.CMB_RouteList = new System.Windows.Forms.ComboBox();
            this.TabRouteShape = new System.Windows.Forms.TabPage();
            this.BUT_DeleteFieldShape = new MissionPlanner.Controls.MyButton();
            this.BUT_UpdateFieldShapeList = new MissionPlanner.Controls.MyButton();
            this.CMB_NmeaFiles = new System.Windows.Forms.ComboBox();
            this.BUT_ShapeSave = new MissionPlanner.Controls.MyButton();
            this.groupBox5.SuspendLayout();
            this.tabRouteCalc.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_divide_step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_margin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Distance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_uav_radius)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRouteManage.SuspendLayout();
            this.TabRouteShape.SuspendLayout();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Bearing = 0F;
            this.map.CanDragMap = true;
            resources.ApplyResources(this.map, "map");
            this.map.EmptyTileColor = System.Drawing.Color.Gray;
            this.map.GrayScaleMode = false;
            this.map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.map.HoldInvalidation = false;
            this.map.LevelsKeepInMemmory = 5;
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
            this.map.Zoom = 3D;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbl_distbetweenlines);
            this.groupBox5.Controls.Add(this.label25);
            this.groupBox5.Controls.Add(this.lbl_strips);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.lbl_distance);
            this.groupBox5.Controls.Add(this.lbl_area);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.label22);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // lbl_distbetweenlines
            // 
            resources.ApplyResources(this.lbl_distbetweenlines, "lbl_distbetweenlines");
            this.lbl_distbetweenlines.Name = "lbl_distbetweenlines";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // lbl_strips
            // 
            resources.ApplyResources(this.lbl_strips, "lbl_strips");
            this.lbl_strips.Name = "lbl_strips";
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // lbl_distance
            // 
            resources.ApplyResources(this.lbl_distance, "lbl_distance");
            this.lbl_distance.Name = "lbl_distance";
            // 
            // lbl_area
            // 
            resources.ApplyResources(this.lbl_area, "lbl_area");
            this.lbl_area.Name = "lbl_area";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // tabRouteCalc
            // 
            this.tabRouteCalc.Controls.Add(this.BUT_Test2);
            this.tabRouteCalc.Controls.Add(this.BUT_Test1);
            this.tabRouteCalc.Controls.Add(this.BUT_newDialog);
            this.tabRouteCalc.Controls.Add(this.BUT_PolygonRead);
            this.tabRouteCalc.Controls.Add(this.groupBox6);
            this.tabRouteCalc.Controls.Add(this.groupBox4);
            this.tabRouteCalc.Controls.Add(this.CMB_fieldshapes);
            this.tabRouteCalc.Controls.Add(this.BUT_SaveRoute);
            this.tabRouteCalc.Controls.Add(this.BUT_Accept);
            resources.ApplyResources(this.tabRouteCalc, "tabRouteCalc");
            this.tabRouteCalc.Name = "tabRouteCalc";
            this.tabRouteCalc.UseVisualStyleBackColor = true;
            // 
            // BUT_Test2
            // 
            resources.ApplyResources(this.BUT_Test2, "BUT_Test2");
            this.BUT_Test2.Name = "BUT_Test2";
            this.BUT_Test2.UseVisualStyleBackColor = true;
            this.BUT_Test2.Click += new System.EventHandler(this.BUT_Test2_Click);
            // 
            // BUT_Test1
            // 
            resources.ApplyResources(this.BUT_Test1, "BUT_Test1");
            this.BUT_Test1.Name = "BUT_Test1";
            this.BUT_Test1.UseVisualStyleBackColor = true;
            this.BUT_Test1.Click += new System.EventHandler(this.BUT_Test1_Click);
            // 
            // BUT_newDialog
            // 
            resources.ApplyResources(this.BUT_newDialog, "BUT_newDialog");
            this.BUT_newDialog.Name = "BUT_newDialog";
            this.BUT_newDialog.UseVisualStyleBackColor = true;
            this.BUT_newDialog.Click += new System.EventHandler(this.BUT_newDialog_Click);
            // 
            // BUT_PolygonRead
            // 
            resources.ApplyResources(this.BUT_PolygonRead, "BUT_PolygonRead");
            this.BUT_PolygonRead.Name = "BUT_PolygonRead";
            this.BUT_PolygonRead.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox1);
            this.groupBox6.Controls.Add(this.BUT_reverse);
            this.groupBox6.Controls.Add(this.BUT_StartNext);
            this.groupBox6.Controls.Add(this.BUT_StartPrev);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.NUM_divide_step);
            this.groupBox6.Controls.Add(this.NUM_margin);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.NUM_Distance);
            this.groupBox6.Controls.Add(this.NUM_uav_radius);
            this.groupBox6.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBT_Spiral);
            this.groupBox1.Controls.Add(this.RBT_Zigzag);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // RBT_Spiral
            // 
            resources.ApplyResources(this.RBT_Spiral, "RBT_Spiral");
            this.RBT_Spiral.Name = "RBT_Spiral";
            this.RBT_Spiral.TabStop = true;
            this.RBT_Spiral.UseVisualStyleBackColor = true;
            this.RBT_Spiral.Click += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // RBT_Zigzag
            // 
            resources.ApplyResources(this.RBT_Zigzag, "RBT_Zigzag");
            this.RBT_Zigzag.Checked = true;
            this.RBT_Zigzag.Name = "RBT_Zigzag";
            this.RBT_Zigzag.TabStop = true;
            this.RBT_Zigzag.UseVisualStyleBackColor = true;
            this.RBT_Zigzag.Click += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // BUT_reverse
            // 
            resources.ApplyResources(this.BUT_reverse, "BUT_reverse");
            this.BUT_reverse.Name = "BUT_reverse";
            this.BUT_reverse.UseVisualStyleBackColor = true;
            this.BUT_reverse.Click += new System.EventHandler(this.BUT_reverse_Click);
            // 
            // BUT_StartNext
            // 
            resources.ApplyResources(this.BUT_StartNext, "BUT_StartNext");
            this.BUT_StartNext.Name = "BUT_StartNext";
            this.BUT_StartNext.UseVisualStyleBackColor = true;
            this.BUT_StartNext.Click += new System.EventHandler(this.BUT_StartPrevNext_Click);
            // 
            // BUT_StartPrev
            // 
            resources.ApplyResources(this.BUT_StartPrev, "BUT_StartPrev");
            this.BUT_StartPrev.Name = "BUT_StartPrev";
            this.BUT_StartPrev.UseVisualStyleBackColor = true;
            this.BUT_StartPrev.Click += new System.EventHandler(this.BUT_StartPrevNext_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // NUM_divide_step
            // 
            this.NUM_divide_step.DecimalPlaces = 1;
            this.NUM_divide_step.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.NUM_divide_step, "NUM_divide_step");
            this.NUM_divide_step.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUM_divide_step.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_divide_step.Name = "NUM_divide_step";
            this.NUM_divide_step.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUM_divide_step.ValueChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // NUM_margin
            // 
            this.NUM_margin.DecimalPlaces = 1;
            this.NUM_margin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.NUM_margin, "NUM_margin");
            this.NUM_margin.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NUM_margin.Name = "NUM_margin";
            this.NUM_margin.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NUM_margin.ValueChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // NUM_Distance
            // 
            this.NUM_Distance.DecimalPlaces = 1;
            this.NUM_Distance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.NUM_Distance, "NUM_Distance");
            this.NUM_Distance.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NUM_Distance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_Distance.Name = "NUM_Distance";
            this.NUM_Distance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NUM_Distance.ValueChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // NUM_uav_radius
            // 
            this.NUM_uav_radius.DecimalPlaces = 1;
            this.NUM_uav_radius.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.NUM_uav_radius, "NUM_uav_radius");
            this.NUM_uav_radius.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUM_uav_radius.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NUM_uav_radius.Name = "NUM_uav_radius";
            this.NUM_uav_radius.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NUM_uav_radius.ValueChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CHK_internals);
            this.groupBox4.Controls.Add(this.chk_grid);
            this.groupBox4.Controls.Add(this.chk_markers);
            this.groupBox4.Controls.Add(this.chk_boundary);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // CHK_internals
            // 
            resources.ApplyResources(this.CHK_internals, "CHK_internals");
            this.CHK_internals.Checked = true;
            this.CHK_internals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_internals.Name = "CHK_internals";
            this.CHK_internals.UseVisualStyleBackColor = true;
            this.CHK_internals.CheckedChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // chk_grid
            // 
            resources.ApplyResources(this.chk_grid, "chk_grid");
            this.chk_grid.Checked = true;
            this.chk_grid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_grid.Name = "chk_grid";
            this.chk_grid.UseVisualStyleBackColor = true;
            this.chk_grid.CheckedChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // chk_markers
            // 
            resources.ApplyResources(this.chk_markers, "chk_markers");
            this.chk_markers.Checked = true;
            this.chk_markers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_markers.Name = "chk_markers";
            this.chk_markers.UseVisualStyleBackColor = true;
            this.chk_markers.CheckedChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // chk_boundary
            // 
            resources.ApplyResources(this.chk_boundary, "chk_boundary");
            this.chk_boundary.Checked = true;
            this.chk_boundary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_boundary.Name = "chk_boundary";
            this.chk_boundary.UseVisualStyleBackColor = true;
            this.chk_boundary.CheckedChanged += new System.EventHandler(this.Common_ValueChanged_Event);
            // 
            // CMB_fieldshapes
            // 
            this.CMB_fieldshapes.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_fieldshapes, "CMB_fieldshapes");
            this.CMB_fieldshapes.Name = "CMB_fieldshapes";
            this.CMB_fieldshapes.SelectedIndexChanged += new System.EventHandler(this.CMB_fieldshapes_SelectedIndexChanged);
            // 
            // BUT_SaveRoute
            // 
            resources.ApplyResources(this.BUT_SaveRoute, "BUT_SaveRoute");
            this.BUT_SaveRoute.Name = "BUT_SaveRoute";
            this.BUT_SaveRoute.UseVisualStyleBackColor = true;
            this.BUT_SaveRoute.Click += new System.EventHandler(this.BUT_SaveRoute_Click);
            // 
            // BUT_Accept
            // 
            resources.ApplyResources(this.BUT_Accept, "BUT_Accept");
            this.BUT_Accept.Name = "BUT_Accept";
            this.BUT_Accept.UseVisualStyleBackColor = true;
            this.BUT_Accept.Click += new System.EventHandler(this.BUT_Accept_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRouteCalc);
            this.tabControl1.Controls.Add(this.tabRouteManage);
            this.tabControl1.Controls.Add(this.TabRouteShape);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabRouteManage
            // 
            this.tabRouteManage.Controls.Add(this.BUT_RouteDelete);
            this.tabRouteManage.Controls.Add(this.BUT_RouteList_Update);
            this.tabRouteManage.Controls.Add(this.label4);
            this.tabRouteManage.Controls.Add(this.CMB_RouteList);
            resources.ApplyResources(this.tabRouteManage, "tabRouteManage");
            this.tabRouteManage.Name = "tabRouteManage";
            this.tabRouteManage.UseVisualStyleBackColor = true;
            // 
            // BUT_RouteDelete
            // 
            resources.ApplyResources(this.BUT_RouteDelete, "BUT_RouteDelete");
            this.BUT_RouteDelete.Name = "BUT_RouteDelete";
            this.BUT_RouteDelete.UseVisualStyleBackColor = true;
            this.BUT_RouteDelete.Click += new System.EventHandler(this.BUT_RouteDelete_Click);
            // 
            // BUT_RouteList_Update
            // 
            resources.ApplyResources(this.BUT_RouteList_Update, "BUT_RouteList_Update");
            this.BUT_RouteList_Update.Name = "BUT_RouteList_Update";
            this.BUT_RouteList_Update.UseVisualStyleBackColor = true;
            this.BUT_RouteList_Update.Click += new System.EventHandler(this.BUT_RouteList_Update_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CMB_RouteList
            // 
            this.CMB_RouteList.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_RouteList, "CMB_RouteList");
            this.CMB_RouteList.Name = "CMB_RouteList";
            this.CMB_RouteList.SelectedIndexChanged += new System.EventHandler(this.CMB_RouteList_SelectedIndexChanged);
            // 
            // TabRouteShape
            // 
            this.TabRouteShape.Controls.Add(this.BUT_DeleteFieldShape);
            this.TabRouteShape.Controls.Add(this.BUT_UpdateFieldShapeList);
            this.TabRouteShape.Controls.Add(this.CMB_NmeaFiles);
            this.TabRouteShape.Controls.Add(this.BUT_ShapeSave);
            resources.ApplyResources(this.TabRouteShape, "TabRouteShape");
            this.TabRouteShape.Name = "TabRouteShape";
            this.TabRouteShape.UseVisualStyleBackColor = true;
            // 
            // BUT_DeleteFieldShape
            // 
            resources.ApplyResources(this.BUT_DeleteFieldShape, "BUT_DeleteFieldShape");
            this.BUT_DeleteFieldShape.Name = "BUT_DeleteFieldShape";
            this.BUT_DeleteFieldShape.UseVisualStyleBackColor = true;
            this.BUT_DeleteFieldShape.Click += new System.EventHandler(this.BUT_DeleteFieldShape_Click);
            // 
            // BUT_UpdateFieldShapeList
            // 
            resources.ApplyResources(this.BUT_UpdateFieldShapeList, "BUT_UpdateFieldShapeList");
            this.BUT_UpdateFieldShapeList.Name = "BUT_UpdateFieldShapeList";
            this.BUT_UpdateFieldShapeList.UseVisualStyleBackColor = true;
            this.BUT_UpdateFieldShapeList.Click += new System.EventHandler(this.BUT_UpdateFieldShapeList_Click);
            // 
            // CMB_NmeaFiles
            // 
            this.CMB_NmeaFiles.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_NmeaFiles, "CMB_NmeaFiles");
            this.CMB_NmeaFiles.Name = "CMB_NmeaFiles";
            this.CMB_NmeaFiles.SelectedIndexChanged += new System.EventHandler(this.CMB_NmeaFiles_SelectedIndexChanged);
            // 
            // BUT_ShapeSave
            // 
            resources.ApplyResources(this.BUT_ShapeSave, "BUT_ShapeSave");
            this.BUT_ShapeSave.Name = "BUT_ShapeSave";
            this.BUT_ShapeSave.UseVisualStyleBackColor = true;
            this.BUT_ShapeSave.Click += new System.EventHandler(this.BUT_ShapeSave_Click);
            // 
            // TotechGridUI
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.map);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.tabControl1);
            this.Name = "TotechGridUI";
            this.Load += new System.EventHandler(this.TotechGridUI_Load);
            this.Resize += new System.EventHandler(this.TotechGridUI_Resize);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabRouteCalc.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_divide_step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_margin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Distance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_uav_radius)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabRouteManage.ResumeLayout(false);
            this.tabRouteManage.PerformLayout();
            this.TabRouteShape.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.myGMAP map;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lbl_distance;
        private System.Windows.Forms.Label lbl_area;
        private System.Windows.Forms.Label lbl_distbetweenlines;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lbl_strips;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TabPage tabRouteCalc;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown NUM_margin;
        private System.Windows.Forms.Label label6;
        private Controls.MyButton BUT_Accept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUM_Distance;
        private System.Windows.Forms.NumericUpDown NUM_uav_radius;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chk_grid;
        private System.Windows.Forms.CheckBox chk_markers;
        private System.Windows.Forms.CheckBox chk_boundary;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox CHK_internals;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUM_divide_step;
        private Controls.MyButton BUT_PolygonRead;
        private Controls.MyButton BUT_newDialog;
        private System.Windows.Forms.ComboBox CMB_fieldshapes;
        private Controls.MyButton BUT_StartNext;
        private Controls.MyButton BUT_StartPrev;
        private Controls.MyButton BUT_reverse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RBT_Spiral;
        private System.Windows.Forms.RadioButton RBT_Zigzag;
        private System.Windows.Forms.TabPage tabRouteManage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CMB_RouteList;
        private Controls.MyButton BUT_SaveRoute;
        private Controls.MyButton BUT_RouteList_Update;
        private Controls.MyButton BUT_RouteDelete;
        private System.Windows.Forms.TabPage TabRouteShape;
        private Controls.MyButton BUT_DeleteFieldShape;
        private Controls.MyButton BUT_UpdateFieldShapeList;
        private System.Windows.Forms.ComboBox CMB_NmeaFiles;
        private Controls.MyButton BUT_ShapeSave;
        private Controls.MyButton BUT_Test2;
        private Controls.MyButton BUT_Test1;
    }
}