using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using log4net;
using MissionPlanner.Plugin;
using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using totech;

namespace MissionPlanner.TotechGrid
{




    public partial class TotechGridUI : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const double rad2deg = (180 / Math.PI);
        const double deg2rad = (1.0 / rad2deg);

        GMapOverlay layerpolygons;
        //GMapPolygon wppoly;
        private GridPlugin plugin;
        List<PointLatLngAlt> grid;          // ルート演算結果

        List<PointLatLngAlt> list = new List<PointLatLngAlt>();         // ポリゴン(圃場形状)


        List<PointLatLngAlt> gnss_list = new List<PointLatLngAlt>();    // GNSSログ



        internal PointLatLng MouseDownStart = new PointLatLng();
        internal PointLatLng MouseDownEnd;
        internal PointLatLngAlt CurrentGMapMarkerStartPos;
        PointLatLng currentMousePosition;
        GMapMarker marker;
        GMapMarker CurrentGMapMarker = null;
        int CurrentGMapMarkerIndex = 0;
        bool isMouseDown = false;
        bool isMouseDraging = false;
        static public Object thisLock = new Object();

        public PluginHost Host2 { get; private set; }

        // ルート開始位置Index
        int _route_start_index = 0;
        private int route_start_index
        {
            get
            {
                if( list==null || list.Count <= 1 )
                {
                    _route_start_index = 0;
                }
                else
                {
                    // ルート開始位置が形状の範囲を超えている場合調整。
                    int polygon_size = list.Count - 1;
                    // 圃場形状サイズ。先頭と最後が一緒なので-1する。
                    while (_route_start_index < 0)
                        _route_start_index += polygon_size;
                    while (_route_start_index >= polygon_size)
                        _route_start_index -= polygon_size;

                }
                return _route_start_index;
            }

            set
            {

                _route_start_index = value;
            }


        }

        // ルート方向
        private bool route_reverse = false;




        // コンストラクタ
        public TotechGridUI(GridPlugin plugin)
        {
            this.plugin = plugin;

            InitializeComponent();

            map.MapProvider = plugin.Host.FDMapType;

            layerpolygons = new GMapOverlay( "polygons");
            map.Overlays.Add(layerpolygons);


            // set and angle that is good
            list = new List<PointLatLngAlt>();
            plugin.Host.FPDrawnPolygon.Points.ForEach(x => { list.Add(x); });
#if false
            if ( list.Count > 2 )
            {
                NUM_angle.Value = (decimal)((getAngleOfLongestSide(list) + 360) % 360);
            }
#endif 
            // 開始地点




            // Map Events
            map.OnMarkerEnter += new MarkerEnter(map_OnMarkerEnter);
            map.OnMarkerLeave += new MarkerLeave(map_OnMarkerLeave);
            map.MouseUp += new MouseEventHandler(map_MouseUp);
            map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map_MouseDown);
            map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.map_MouseMove);
        }

        void savesettings()
        {
            plugin.Host.config["totechgrid_margin"] = NUM_margin.Value.ToString();          // 畦からの距離
            plugin.Host.config["totechgrid_distpitch"] = NUM_Distance.Value.ToString();     // 折返し間隔
            plugin.Host.config["totechgrid_devidestep"] = NUM_divide_step.Value.ToString(); //分割ステップ
            plugin.Host.config["totechgrid_uavradius"] = NUM_uav_radius.Value.ToString();   // 旋回半径
            plugin.Host.config["totechgrid_zigzag"] = RBT_Zigzag.Checked.ToString();        // ジグザグ

        }

        void loadsettings()
        {
//            if (plugin.Host.config.ContainsKey("totechgrid"))
//            {
                
                loadsetting("totechgrid_margin", NUM_margin);
                loadsetting("totechgrid_distpitch", NUM_Distance);
                loadsetting("totechgrid_devidestep", NUM_divide_step);            
                loadsetting("totechgrid_uavradius", NUM_uav_radius);
                loadsetting("totechgrid_zigzag", RBT_Zigzag);
                RBT_Spiral.Checked = !RBT_Zigzag.Checked;

        }
        //        }

        void loadsetting(string key, Control item)
        {
            if (plugin.Host.config.ContainsKey(key))
            {
                if (item is NumericUpDown)
                {
                    ((NumericUpDown)item).Value = decimal.Parse(plugin.Host.config[key].ToString());
                }
                else if (item is ComboBox)
                {
                    ((ComboBox)item).Text = plugin.Host.config[key].ToString();
                }
                else if (item is CheckBox)
                {
                    ((CheckBox)item).Checked = bool.Parse(plugin.Host.config[key].ToString());
                }
                else if (item is RadioButton)
                {
                    ((RadioButton)item).Checked = bool.Parse(plugin.Host.config[key].ToString());
                }
            }
        }


        private void map_OnMarkerLeave(GMapMarker item)
        {
            if (!isMouseDown)
            {
                if (item is GMapMarker)
                {
                    // when you click the context menu this triggers and causes problems
                    CurrentGMapMarker = null;
                }

            }
        }

        private void map_OnMarkerEnter(GMapMarker item)
        {
            if (!isMouseDown)
            {
                if (item is GMapMarker)
                {
                    CurrentGMapMarker = item as GMapMarker;
                    CurrentGMapMarkerStartPos = CurrentGMapMarker.Position;
                }
            }
        }

        private void map_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDownEnd = map.FromLocalToLatLng(e.X, e.Y);

            // Console.WriteLine("MainMap MU");

            if (e.Button == MouseButtons.Right) // ignore right clicks
            {
                return;
            }

            if (isMouseDown) // mouse down on some other object and dragged to here.
            {
                if (e.Button == MouseButtons.Left)
                {
                    isMouseDown = false;
                }
                if (!isMouseDraging)
                {
                    if (CurrentGMapMarker != null)
                    {
                        // Redraw polygon
                        //AddDrawPolygon();
                    }
                }
            }
            isMouseDraging = false;
            CurrentGMapMarker = null;
            CurrentGMapMarkerIndex = 0;
            CurrentGMapMarkerStartPos = null;
        }

        private void map_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownStart = map.FromLocalToLatLng(e.X, e.Y);

            if (e.Button == MouseButtons.Left && Control.ModifierKeys != Keys.Alt)
            {
                isMouseDown = true;
                isMouseDraging = false;

                if (CurrentGMapMarkerStartPos != null)
                    CurrentGMapMarkerIndex = list.FindIndex(c => c.ToString() == CurrentGMapMarkerStartPos.ToString());
            }
        }

        private void map_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng point = map.FromLocalToLatLng(e.X, e.Y);
            currentMousePosition = point;

            if (MouseDownStart == point)
                return;

            if (!isMouseDown)
            {
                // update mouse pos display
                //SetMouseDisplay(point.Lat, point.Lng, 0);
            }

            //draging
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                isMouseDraging = true;

                if (CurrentGMapMarker != null)
                {
                    if (CurrentGMapMarkerIndex == -1)
                    {
                        isMouseDraging = false;
                        return;
                    }

                    PointLatLng pnew = map.FromLocalToLatLng(e.X, e.Y);

                    CurrentGMapMarker.Position = pnew;

                    list[CurrentGMapMarkerIndex] = new PointLatLngAlt(pnew);
                    Common_ValueChanged_Event(sender, e);
                }
                else // left click pan
                {
                    double latdif = MouseDownStart.Lat - point.Lat;
                    double lngdif = MouseDownStart.Lng - point.Lng;

                    try
                    {
                        lock (thisLock)
                        {
                            map.Position = new PointLatLng(map.Position.Lat + latdif, map.Position.Lng + lngdif);
                        }
                    }
                    catch { }
                }
            }
        }

        // 圃場形状（枠線）の描画（layerpolygonsへの追加）
        // 
        void AddDrawPolygon()
        {
            AddDrawPolygon(list, "poly", Color.Red, 2);

            // マーカーの描画
            foreach (var item in list)
            {
                layerpolygons.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.red));
            }
        }

        // ルート(waypoints)の描画（layerpolygonsへの追加）

        // 圃場形状またはルートの描画（描画バッファに格納する。)
        void AddDrawPolygon(List<PointLatLngAlt> tgt, string name, Color draw_colow, int line_width )
        {
            // GMapPolygon型に変換
            // 一度List<PointLatLng>を作成してからGMapPolygonを作成する
            List<PointLatLng> work = new List<PointLatLng>();
            tgt.ForEach(x => { work.Add(x); });
            var poly = new GMapPolygon(work, name);

            poly.Stroke = new Pen(draw_colow, line_width);
            poly.Fill = Brushes.Transparent;

            layerpolygons.Polygons.Add(poly);

#if false
            // マーカーの描画
            foreach (var item in tgt)
            {
                layerpolygons.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.red));
            }
#endif
        }

        double getDistanceOfRoute(List<PointLatLngAlt> tgt)
        {
            // GMapPolygon型に変換
            // 一度List<PointLatLng>を作成してからGMapPolygonを作成する
            List<PointLatLng> work = new List<PointLatLng>();
            tgt.ForEach(x => { work.Add(x); });
            var poly = new GMapPolygon(work, "");

            return poly.Distance; 
        }




        double getAngleOfLongestSide(List<PointLatLngAlt> list)
        {
            double angle = 0;
            double maxdist = 0;
            PointLatLngAlt last = list[list.Count - 1];
            foreach (var item in list)
            {
                 if (item.GetDistance(last) > maxdist) 
                 {
                     angle = item.GetBearing(last);
                     maxdist = item.GetDistance(last);
                 }
                 last = item;
            }

            return (angle + 360) % 360;
        }


        // コントロールが操作された際にリルート演算
        private void Common_ValueChanged_Event(object sender, EventArgs e)
        {
            // ルート再描画実行
            Do_ReRoot();
        }

        //
        // ルート再描画実行
        private void Do_ReRoot()
        {
            Host2 = plugin.Host;

            // ルート演算実行
            grid = CreateTotechGrid(list);

            // 形状/ルート 再描画
            ReDraw();

        }


        // 形状/ルート 再描画
        private void ReDraw()
        {
            Host2 = plugin.Host;

            map.HoldInvalidation = true;

            layerpolygons.Polygons.Clear();
            layerpolygons.Markers.Clear();

            // 圃場形状の描画
            AddDrawPolygon();


#if false
            // マーカーの描画
            int strips = 0;
            int a = 1;
            PointLatLngAlt prevpoint = grid[0];
            foreach (var item in grid)
            {
                if (item.Tag == "M")
                {
                    if (CHK_internals.Checked)
                    {
                        layerpolygons.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.green) { ToolTipText = a.ToString(), ToolTipMode = MarkerTooltipMode.OnMouseOver });
                        a++;
                    }
                }
                else
                {
                    if (item.Tag == "S" || item.Tag == "E")
                    {
                        strips++;
                        if (chk_markers.Checked)
                            layerpolygons.Markers.Add(new GMarkerGoogle(item, GMarkerGoogleType.green)
                            {
                                ToolTipText = a.ToString(),
                                ToolTipMode = MarkerTooltipMode.OnMouseOver
                            });

                        a++;
                    }
                }
                prevpoint = item;
            }
#endif 


            // ルートの描画
            AddDrawPolygon(grid, "Grid", Color.Yellow, 4);

            // ルートの道のり(Distance)を表示
            double distance = getDistanceOfRoute(grid);
            Console.WriteLine("Poly Dist " + distance);
            lbl_area.Text = CalcPolygonArea(list).ToString("#") + " ㎡";
            lbl_distance.Text = distance.ToString("0.##") + " km";


            //    lbl_strips.Text = ((int)(strips / 2)).ToString();
            //    lbl_distbetweenlines.Text = NUM_Distance.Value.ToString("0.##") + " m";

            if (tabControl1.SelectedIndex == 2)
            {
                // GNSSログ（圃場形状の元）の表示
                if (gnss_list.Count > 3)
                {
                    AddDrawPolygon(gnss_list, "gnss", Color.Orange, 1);
                }
            }


            // map.HoldInvalidation = false;    //
            if ( list.Count >  0 )
                map.ZoomAndCenterMarkers("polygons");
            else
                map.ZoomAndCenterRoutes("polygons");

            map.HoldInvalidation = false;    //
            map.Refresh();

        }        





        double CalcPolygonArea(List<PointLatLngAlt> polygon)
        {
            // should be a closed polygon
            // coords are in lat long
            // need utm to calc area

            if (polygon.Count == 0)
            {
//                CustomMessageBox.Show("Please define a polygon!");
                return 0;
            }

            // close the polygon
            if (polygon[0] != polygon[polygon.Count - 1])
                polygon.Add(polygon[0]); // make a full loop

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            IGeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int)((polygon[0].Lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone, polygon[0].Lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double prod1 = 0;
            double prod2 = 0;

            for (int a = 0; a < (polygon.Count - 1); a++)
            {
                double[] pll1 = { polygon[a].Lng, polygon[a].Lat };
                double[] pll2 = { polygon[a + 1].Lng, polygon[a + 1].Lat };

                double[] p1 = trans.MathTransform.Transform(pll1);
                double[] p2 = trans.MathTransform.Transform(pll2);

                prod1 += p1[0] * p2[1];
                prod2 += p1[1] * p2[0];
            }

            double answer = (prod1 - prod2) / 2;

            if (polygon[0] == polygon[polygon.Count - 1])
                polygon.RemoveAt(polygon.Count - 1); // unmake a full loop

            return Math.Abs( answer);
        }





        private void TotechGridUI_Resize(object sender, EventArgs e)
        {
            map.ZoomAndCenterMarkers("polygons");
        }

        private void TotechGridUI_Load(object sender, EventArgs e)
        {
            loadsettings();

            // 圃場形状一覧更新
            Update_FieldShapes();

            // コンボボックス選択
            if(CMB_fieldshapes.Items.Count>0)
            {
                CMB_fieldshapes.SelectedIndex = 0;
            }


            // ルート一覧更新
            Update_RouteWaypoints();
        }


        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private List<PointLatLngAlt> CreateTotechGrid(List<PointLatLngAlt> polygon)
        {
            if (polygon.Count == 0)
                return new List<PointLatLngAlt>();

            // List<PointLatLngAlt> ⇒ totech::tPolygon
            totech.tPolygon src_totech_polygon = MpPointList_To_TotechPolygon(polygon);

            // 開始地点調整
            src_totech_polygon.RotateForward(route_start_index);
            if (route_reverse)
            {
                src_totech_polygon.RotateForward();
                src_totech_polygon.Reverse();
            }


            // ルート演算オブジェクト
            RouteCalc RouteCalc_Obj = new RouteCalc();
            // パラメータ設定
            // RouteCalc_Obj.route_type = // ルート演算タイプ 0:Zigzag_cw, 1:zigzag_ccw, 2:spiral_cw, 3:spiral_ccw;
            RouteCalc_Obj.edge_margin = (double)NUM_margin.Value;       // 圃場形状（畦）からのマージン
            RouteCalc_Obj.hover_r = (double)NUM_uav_radius.Value;       // ホバー旋回半径
            RouteCalc_Obj.route_pitch = (double)NUM_Distance.Value;     // ルート間隔
            RouteCalc_Obj.split_step = (double)NUM_divide_step.Value;   // 分割ステップ

            // Route Type
            if ( RBT_Spiral.Checked )
                RouteCalc_Obj.route_type = RouteType.SPIRAL;
            else if( RBT_Zigzag.Checked )
                RouteCalc_Obj.route_type = RouteType.ZIGZAG;
            else
            {
                // エラー
            }

            // ルート演算実行
            totech.tPolygon dest_totech_polygon = new tPolygon();
            bool calc_rslt = RouteCalc_Obj.RouteCalc_Exec(src_totech_polygon, dest_totech_polygon);

            // totech::tPolygon ⇒ List<PointLatLngAlt>
            List<PointLatLngAlt> rslt = TotechPolygon_To_MpPointList(dest_totech_polygon);

            return rslt;

        }

        /// <summary>
        /// totech::tPolygon形式からMissionPlanner標準のポイントリストに変換する
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static List<PointLatLngAlt> TotechPolygon_To_MpPointList(totech.tPolygon src)
        {
            // バッファの用意
            List<PointLatLngAlt> dest = new List<PointLatLngAlt>();
            dest.Capacity = src.Count;

            if (src.Count <= 0)
                return dest;

            // 原点
            Point2D src_org = src.Get_Org_LngLat();
            PointLatLngAlt org_point = new PointLatLngAlt(src_org.y, src_org.x);

            for (int i = 0; i < src.Count; i++)
            {
                // 原点基準で点を作ってOffsetする
                PointLatLngAlt tmp_point = new PointLatLngAlt(org_point);
                dest.Add(tmp_point.gps_offset(src[i].x, src[i].y));
            }


            return dest;

        }

        /// <summary>
        /// MissionPlanner標準のポイントリストからtotech::tPolygon形式に変換する
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static totech.tPolygon MpPointList_To_TotechPolygon(List<PointLatLngAlt> src)
        {

            // バッファの用意
            tPolygon dest = new tPolygon();
            dest.Capacity = src.Count;

            if (src.Count <= 0)
                return dest;

            // 原点
            PointLatLngAlt org_point = src[0];


            for (int i = 0; i < src.Count; i++)
            {
                PointLatLngAlt src_item = src[i];
                // 原点からのXY距離
                Point2D tmpPoint = calcDistHubeny(org_point, src[i]);
                dest.Add(tmpPoint);
            }

            // 始点と終点が同じなら、終点を削除
            if (dest[0] == dest[dest.Count - 1])
                dest.RemoveAt(dest.Count - 1);


            // 原点を設定する
            dest.Set_Org_LngLat(new Point2D(org_point.Lng, org_point.Lat));


            return dest;

        }


        //'-----------------------------------------------------------------
        //'2点の位置間の移動距離を求める。
        //'ヒュベニの公式を用いて算出する。
        //'演算で使用する各定数はWGS84 (GPS)系
        //'
        //'引数：p1 as PointD    'X=東経, Y=北緯：度単位
        //'　　　P2 as pointD    ' 　〃
        //'Return: PointF
        //'   X(東が＋）、Y（北が＋）のメートル単位
        //'
        //' Decimal型使用したいが、C言語でDecimalを使う手段が不明
        //  演算精度に不安あり
        //'
        private static Point2D calcDistHubeny(PointLatLngAlt p1, PointLatLngAlt p2)
        {
            // PointLatLngAlt ⇒ Point2D変換
            Point2D new_p1 = PointLatLngAlt_To_Point2D(p1);
            Point2D new_p2 = PointLatLngAlt_To_Point2D(p2);

            return Calc_GPS.calcDistHubeny(new_p1, new_p2);
        }

        // PointLatLngAlt ⇒ Point2D変換
        // 緯度がy, 経度がxに入る。
        private static Point2D PointLatLngAlt_To_Point2D(PointLatLngAlt src)
        {
            return new Point2D(src.Lng, src.Lat);
        }


#region "圃場形状（ポリゴン）ファイル取扱い"
        // 圃場形状（ポリゴン）一覧
        List<List<PointLatLngAlt>> fieldShapes = new List<List<PointLatLngAlt>>();
        // 圃場形状（ポリゴン）一覧を更新する。
        private　void Update_FieldShapes()
        {
            fieldShapes.Clear();
            CMB_fieldshapes.Items.Clear();


            // (MP)Polygonファイル一覧 取得
            string search_path = Misc.GetFullPath(Consts.FieldShape_Path);
            string search_filter = Consts.FieldShape_Filter;
            try
            {
                IEnumerable<string> files = System.IO.Directory.EnumerateFiles(search_path, search_filter, System.IO.SearchOption.AllDirectories);
                foreach (string fname in files)
                {
                    List_LatLangAlt readbuff = new List_LatLangAlt(fname);
                    fieldShapes.Add(readbuff);
                    CMB_fieldshapes.Items.Add(readbuff.Title);
                }
            }
            catch { }

        }


        // ルートまたは圃場形状（MPのポリゴン）をファイルに書き込む
        // 
        // original : FlightPlanner.cs::savePolygonToolStripMenuItem_Click()
        private void Save_Polygon_to_file(List<PointLatLngAlt> src_data, string filename)
        {
            Misc.CreateDirectory_of_Filename(filename);

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("#saved by TotechGrid in MissionPlanner " + Application.ProductVersion);

                if (src_data.Count > 0)
                {
                    foreach (var pll in src_data)
                    {
                        sw.WriteLine(pll.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll.Lng.ToString(CultureInfo.InvariantCulture));
                    }

                    PointLatLng pll2 = src_data[0];

                    sw.WriteLine(pll2.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll2.Lng.ToString(CultureInfo.InvariantCulture));
                }

                sw.Close();
            }
        }



#if false
        private void savePolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawnpolygon.Points.Count == 0)
            {
                return;
            }


            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = "Polygon (*.poly)|*.poly";
                var result = sf.ShowDialog();
                if (sf.FileName != "" && result == DialogResult.OK)
                {
                    try
                    {
                        StreamWriter sw = new StreamWriter(sf.OpenFile());

                        sw.WriteLine("#saved by Mission Planner " + Application.ProductVersion);

                        if (drawnpolygon.Points.Count > 0)
                        {
                            foreach (var pll in drawnpolygon.Points)
                            {
                                sw.WriteLine(pll.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll.Lng.ToString(CultureInfo.InvariantCulture));
                            }

                            PointLatLng pll2 = drawnpolygon.Points[0];

                            sw.WriteLine(pll2.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll2.Lng.ToString(CultureInfo.InvariantCulture));
                        }

                        sw.Close();
                    }
                    catch
                    {
                        CustomMessageBox.Show("Failed to write fence file");
                    }
                }
            }
        }
#endif

        // ファイルから圃場形状（MPのポリゴン）を読み込む
        // original : FlightPlanner.cs::loadPolygonToolStripMenuItem_Click()
        private List<PointLatLngAlt> Load_FieldShape_from_file( string filename)
        {
            List<PointLatLngAlt> data_buff = new List<PointLatLngAlt>();

            using ( StreamReader sr = new StreamReader(filename  ))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("#"))
                    {
                    }
                    else
                    {
                        string[] items = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (items.Length < 2)
                            continue;

                        data_buff.Add(new PointLatLng(
                            double.Parse(items[0], CultureInfo.InvariantCulture),
                            double.Parse(items[1], CultureInfo.InvariantCulture)));

                    }
                }
            }

            return data_buff;


        }


#if false
        
        //private void loadPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Polygon (*.poly)|*.poly";
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    StreamReader sr = new StreamReader(fd.OpenFile());

                    drawnpolygonsoverlay.Markers.Clear();
                    drawnpolygonsoverlay.Polygons.Clear();
                    drawnpolygon.Points.Clear();

                    int a = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("#"))
                        {
                        }
                        else
                        {
                            string[] items = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            if (items.Length < 2)
                                continue;

                            drawnpolygon.Points.Add(new PointLatLng(
                                double.Parse(items[0], CultureInfo.InvariantCulture),
                                double.Parse(items[1], CultureInfo.InvariantCulture)));
                            addpolygonmarkergrid(drawnpolygon.Points.Count.ToString(),
                                double.Parse(items[1], CultureInfo.InvariantCulture),
                                double.Parse(items[0], CultureInfo.InvariantCulture), 0);

                            a++;
                        }
                    }

                    // remove loop close
                    if (drawnpolygon.Points.Count > 1 &&
                        drawnpolygon.Points[0] == drawnpolygon.Points[drawnpolygon.Points.Count - 1])
                    {
                        drawnpolygon.Points.RemoveAt(drawnpolygon.Points.Count - 1);
                    }

                    drawnpolygonsoverlay.Polygons.Add(drawnpolygon);

                    MainMap.UpdatePolygonLocalPosition(drawnpolygon);

                    MainMap.Invalidate();

                    MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);
                }
            }
        }
#endif

        // 圃場形状読み出しボタン
        private void BUT_PolygonRead_Click(object sender, EventArgs e)
        {

        }
        #endregion "圃場形状（ポリゴン）ファイル取扱"

        #region "ルート（Waypoints）ファイル取扱い"
        // ルート（Waypoints）一覧
        List<List<PointLatLngAlt>> RouteWaypoints = new List<List<PointLatLngAlt>>();
        List<string> RouteFileNames = new List<string>();
        // 圃場形状（ポリゴン）一覧を更新する。
        private void Update_RouteWaypoints()
        {
            RouteWaypoints.Clear();
            RouteFileNames.Clear();
            CMB_RouteList.Items.Clear();

            // ルート（Waypoints）ファイル一覧 取得
            string search_path = Misc.GetFullPath(Consts.RouteFile_Path);
            string search_filter = Consts.RouteFile_Filter;
            try
            {
                IEnumerable<string> files = System.IO.Directory.EnumerateFiles(search_path, search_filter, System.IO.SearchOption.AllDirectories);
                foreach (string fname in files)
                {
                    List_LatLangAlt readbuff = new List_LatLangAlt(fname);
                    RouteWaypoints.Add(readbuff);
                    RouteFileNames.Add(fname);
                    CMB_RouteList.Items.Add(readbuff.Title);

                }
            }
            catch { }

        }




        #endregion "ルート（Waypoints）ファイル取扱い"


        #region "圃場形状作成"

        //*****************************************************************
        // 圃場形状を作成する
        //
        //
        private bool make_field_shape(string nmea_filename )
        {   
    
            tPolygon source_polygon;

            // NMEA読み出し
                //cout << "target_polygon = NMEA_Proc::NMEA2Polygon( " << filename << " );" << std::endl;
            source_polygon = NMEA_Proc.NMEA2Polygon(nmea_filename);
            //cout << "NMEA_Proc::NMEA2Polygon()...OK." << std::endl;
    
            if(source_polygon.Count == 0 )
            {
               // cout << "size of source polygon was zero..." << std::endl;
                return false;     // fail
            }


            tPolygon target_polygon = new tPolygon(source_polygon);
            // Polygonの交差を除去する
            target_polygon.NoCross_Polygon();
  



            // 単純化する
        #if false
            // 精度　0.5ｍ
            tPolygon dest_polygon1 = GeomUtils::simplifyWithRDP( target_polygon, 1.0 );
        #else
            // ポリゴンを単純化する
            //   1) Ramer–Douglas–Peuckerアルゴリズムにより単純化
            //   2) 縮小→拡大により余計な線を除去する
                tPolygon dest_polygon1 = GeomUtils.SimplifiedPolygon(target_polygon, 1.0);
#endif

#if false
            // drawerの用意
            std::string drawer_filename = cDirectory::ImgFullPath(SHAPE_SVG_FILENAME);
            RouteCalc_Draw drawer(drawer_filename.c_str() );
            drawer.SetViewBox(dest_polygon1, 2.0 );

            // 描画
        //    drawer.Draw_Porygon( target_polygon, "black" );   // 閉じた形状
            drawer.Draw_Points(source_polygon, "black" );     // 開いた形状

            drawer.Draw_Porygon(target_polygon, "blue" );
        
            drawer.Draw_Porygon(dest_polygon1, "red" );    // 描画
            // 圃場形状出力
            dest_polygon1.SaveToFile(cDirectory::ShapeFullPath(SHAPE_POL_FILENAME).c_str() );

#endif
            // totech::tPolygon ⇒ List<PointLatLngAlt>
            List<PointLatLngAlt> rslt = TotechPolygon_To_MpPointList(dest_polygon1);

            list = rslt;

#if false
            // 圃場形状（MPのポリゴン）をファイルに書き込む
            // 
            // original : FlightPlanner.cs::savePolygonToolStripMenuItem_Click()
            Save_Polygon_to_file(rslt, string filename)
#endif


            // 正常終了
            return true;
        }

#endregion "圃場形状作成"



//「完了」ボタン 
private void BUT_Accept_Click(object sender, EventArgs e)
        {
            if (grid != null && grid.Count > 0)
            {
                MainV2.instance.FlightPlanner.quickadd = true;

                PointLatLngAlt lastpnt = PointLatLngAlt.Zero;

                grid.ForEach(plla =>
                {
                    if (plla.Tag == "M")
                    {
                        if (CHK_internals.Checked)
                            plugin.Host.AddWPtoList(MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, plla.Lng, plla.Lat, plla.Alt);
                    }
                    else
                    {
                        if (!(plla.Lat == lastpnt.Lat && plla.Lng == lastpnt.Lng && plla.Alt == lastpnt.Alt))
                            plugin.Host.AddWPtoList(MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, plla.Lng, plla.Lat, plla.Alt);

                        lastpnt = plla;
                    }
                });

                MainV2.instance.FlightPlanner.quickadd = false;

                MainV2.instance.FlightPlanner.writeKML();

                savesettings();

                this.Close();
            }
            else
            {
                CustomMessageBox.Show("Bad Grid", "Error");
            }
        }

        // ダイアログを開くボタン
        private void BUT_newDialog_Click(object sender, EventArgs e)
        {
            // 圃場形状読み出しダイアログ開く
            using (Form polui = new PolygonReadUI(this.plugin))
            {
                MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(polui);
                polui.ShowDialog();
            }

        }

        // 圃場形状コンボボックス選択
        private void CMB_fieldshapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox sndr = (ComboBox)sender;
            // 選択された形状
            list = fieldShapes[sndr.SelectedIndex ];

            // 圃場形状を再描画
            // 未実装

            // ルート再演算
            Do_ReRoot();
        }

        // 開始地点変更
        private void BUT_StartPrevNext_Click(object sender, EventArgs e)
        {
            if (sender == BUT_StartNext)
                route_start_index += 1;
            if (sender == BUT_StartPrev)
                route_start_index -= 1;

            // ルート再演算
            Do_ReRoot();

        }

        // ルート方向変更
        private void BUT_reverse_Click(object sender, EventArgs e)
        {
            route_reverse = ! route_reverse;

            // ルート再演算
            Do_ReRoot();
        }


        // ルートをファイルに保存する
        private void BUT_SaveRoute_Click(object sender, EventArgs e)
        {
            if( grid.Count == 0)
                return;

            // ルートの始点LatLang
            double lat = grid[0].Lat;
            double lng = grid[0].Lng;


            // 開始地点の緯度経度からファイル名を決める
            string fname = string.Format(Consts.RouteFile_Format, lat, lng );
            fname = Misc.GetFullPath( Consts.RouteFile_Path + "/" + fname);

            // 保存実行
            Save_Polygon_to_file(grid, fname);

        }


        // ルート一覧更新ボタン
        private void BUT_RouteList_Update_Click(object sender, EventArgs e)
        {
            // ルート一覧更新
            Update_RouteWaypoints();
        }

        // ルート一覧から選択
        private void CMB_RouteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox sndr = (ComboBox)sender;
            // 選択されたルート
            grid = RouteWaypoints[sndr.SelectedIndex];

            // 圃場形状クリア
            list.Clear();

            // ルート再描画
            ReDraw();

        }

        // 選択されたルートファイルの削除
        private void BUT_RouteDelete_Click(object sender, EventArgs e)
        {
            int index = CMB_RouteList.SelectedIndex;
            if (index < 0) return;

            // ComboBoxで選択されたルートのファイル名
            string fname = RouteFileNames[index];

            // ファイル削除
            System.IO.File.Delete(fname);

            //リストの更新
            Update_RouteWaypoints();

            // 表示の更新
            grid.Clear();
            ReDraw();

        }

        #region "圃場形状UI"

        // NMEAファイル一覧
        List<string> NmeaFileNames = new List<string>();
        // NMEAファイル一覧を更新する。
        private void Update_NmeaFileNames()
        {
            NmeaFileNames.Clear();
            CMB_NmeaFiles.Items.Clear();

            // NMEAファイル一覧 取得
            string search_path = Misc.GetFullPath(Consts.GnssLog_Path);
            string search_filter = Consts.GnssLog_Filter;
            try
            {
                IEnumerable<string> files = System.IO.Directory.EnumerateFiles(search_path, search_filter, System.IO.SearchOption.AllDirectories);
                foreach (string fname in files)
                {
                    NmeaFileNames.Add(fname);
                    CMB_NmeaFiles.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fname));
                }
            }
            catch { }

        }

        // 「更新」ボタンが押された
        private void BUT_UpdateFieldShapeList_Click(object sender, EventArgs e)
        {
            // NMEAファイル一覧を更新する。
            Update_NmeaFileNames();
        }

        // NMEAファイルが選択された
        private void CMB_NmeaFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox sndr = (ComboBox)sender;
            // 選択されたNMEAファイル名
            string filename = NmeaFileNames[sndr.SelectedIndex];

            // NMEAファイル表示設定
            gnss_list = TotechPolygon_To_MpPointList(NMEA_Proc.NMEA2Polygon(filename));


            // NMEAファイルから圃場形状を作成
            make_field_shape(filename);

            // 表示の更新
            ReDraw();
        }


        #endregion "圃場形状UI"


    }
}

