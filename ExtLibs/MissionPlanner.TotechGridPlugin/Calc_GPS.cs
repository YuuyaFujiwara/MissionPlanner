using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech 
{
    class Calc_GPS
    {



        //Calc_GPS() {
        //}
        //
        //Calc_GPS(const Calc_GPS& orig) {
        //}
        //
        //~Calc_GPS() {
        //}

        //'-----------------------------------------------------------------
        //'GPSログ文字列から位置情報(X=東経,Y=北緯)を得る
        //'
        /*
         * GPSログデータ（PICよりSPIにて受信）から、位置情報(X=東経,Y=北緯)を得る
         * 緯度経度：NMEA形式の実数
         */
        public static Point2D GPS_Log_to_LatLang( String nmea_str ) 
        {
            Point2D latlang_pos = new Point2D(0,0);
            double gnss_time = 0.0;

        #if true
                GPS_Log_to_LatLang(nmea_str, latlang_pos, out gnss_time ); 
            return latlang_pos;
        #else
    
    
            vector<string> str_arry = misc::split( nmea_str, ',' );
    
            string lat_str;         //緯度
            string lng_str;         //経度
            bool   lat_n = true;    //北緯
            bool   lng_e = true;    // 東経
            int    lat_val_index = 0;   //緯度
            int    lat_flg_index = 0;   //経度
            int    lng_val_index = 0;   //北緯
            int    lng_flg_index = 0;   // 東経
    
    
            if( ( str_arry[0] == "$GNGGA" ) || ( str_arry[0] == "$GPGGA" ) )
            {
                // $GPGGA,092813,3854.341800,N,14109.594100,E,1,14,0.80,114.5,M,36.50,M,,*4F
                lat_val_index = 2;   //緯度
                lat_flg_index = 3;   //経度
                lng_val_index = 4;   //北緯
                lng_flg_index = 5;   // 東経
            }
            else
            {
                return retval;  // 演算せず終了
            }

            // 緯度経度取り出し
            retval.y = Convert_dddmmmm_to_degree( str_arry[lat_val_index] );    // 北緯
            retval.x = Convert_dddmmmm_to_degree( str_arry[lng_val_index] );    // 東経
            if( str_arry[lat_flg_index] != "N" ) retval.y *= -1;                // 南緯
            if( str_arry[lng_flg_index] != "E" ) retval.x *= -1;                // 西経

            return retval;
        #endif
        }


        //'-----------------------------------------------------------------
        //'GPSログ文字列から位置情報(X=東経,Y=北緯)を得る
        //'
        /*
            * GPSログデータ(NMEA)から、位置情報(X=東経,Y=北緯)を得る
            * 緯度経度：NMEA形式の実数
            */
        public static bool GPS_Log_to_LatLang( string nmea_str, Point2D latlang_pos, out double gnss_time )
        {
            gnss_time = 0;

            //vector<string> str_arry = misc::split(nmea_str, ',');
            string[] str_arry = nmea_str.Split(',');

            //    string lat_str;         //緯度
            //    string lng_str;         //経度
            //    bool   lat_n = true;    //北緯
            //    bool   lng_e = true;    // 東経
            int lat_val_index = 0;   //緯度
            int lat_flg_index = 0;   //経度
            int lng_val_index = 0;   //北緯
            int lng_flg_index = 0;   // 東経
            int gnsstime_index = 0;


            if ((str_arry[0] == "$GNGGA") || (str_arry[0] == "$GPGGA"))
            {
                // $GPGGA,092813,3854.341800,N,14109.594100,E,1,14,0.80,114.5,M,36.50,M,,*4F
                lat_val_index = 2;   //緯度
                lat_flg_index = 3;   //経度
                lng_val_index = 4;   //北緯
                lng_flg_index = 5;   // 東経
                gnsstime_index = 1; // time
            }
            else
            {
                return false;  // 演算せず終了
            }

            // データチェック
            if (str_arry.Length < 15) return false;

            // 緯度経度取り出し
            latlang_pos.y = Convert_dddmmmm_to_degree(str_arry[lat_val_index]);    // 北緯
            latlang_pos.x = Convert_dddmmmm_to_degree(str_arry[lng_val_index]);    // 東経
            if (str_arry[lat_flg_index] != "N") latlang_pos.y *= -1;                // 南緯
            if (str_arry[lng_flg_index] != "E") latlang_pos.x *= -1;                // 西経

            // gnss time取出し
            //gnss_time = misc::stod(str_arry[gnsstime_index]);
            gnss_time = double.Parse(str_arry[gnsstime_index]);

            return true;
        }




#if false

        //'-----------------------------------------------------------------
        //'GPSログ文字列から位置情報(X=東経,Y=北緯)を得る
        //'
        /*
         * GPSログデータ（PICよりSPIにて受信）から、位置情報(X=東経,Y=北緯)を得る
         * 
         */
        public static Point2D GPS_Log_to_Position(const unsigned short* data)
            {
                double y = Convert_dddmmss_to_degree(data + 5);   // 緯度
                double x = Convert_dddmmss_to_degree(data + 9);   // 経度

                // 南緯・西経の場合は符号を変える
                if (data[4] == 'S') y *= -1;
                if (data[8] == 'W') x *= -1;

                // Debug.Print("x={0:0.000000000}, y={1:0.000000000}", New Object() {x, y})

                //   PointD ret(x,y);
                return new Point2D(x, y);

            }
#endif


        //-----------------------------------------------------------------
        // NMEAの度分表記データから、度に換算した値を得る。
        //
        // 引数：
        //      ary_dddmmss[0]  ：度×100＋分（整数部）
        //      ary_dddmmss[1]　：分 小数部1桁～4桁
        //      ary_dddmmss[2]　：分 小数部5桁～8桁
        //
        public static double Convert_dddmmss_to_degree( short[] ary_dddmmss)
        {
            int degree = ary_dddmmss[0] / 100;  // 度

            // 分
            double min = ary_dddmmss[0] % 100
                        + (double)ary_dddmmss[1] / 10000.0
                        + (double)ary_dddmmss[2] / 100000000.0;

            return degree + min / 60.0;
        }


        //-----------------------------------------------------------------
        // NMEAの度分表記データから、度に換算した値を得る。
        //
        // 引数： ddmm.mmmmmm形式文字列
        //      ary_dddmmss[0]  ：度×100＋分（整数部）
        //      ary_dddmmss[1]　：分 小数部1桁～4桁
        //      ary_dddmmss[2]　：分 小数部5桁～8桁
        //
        public static double Convert_dddmmmm_to_degree( string ddmm_mm )
        {
            // 小数点位置を探す
            int pointpos = ddmm_mm.IndexOf('.');

            if (pointpos < 0) return -1.0;

            // 度と分の取出し
            string dd = ddmm_mm.Substring(0, pointpos - 2);
            string mm_mm = ddmm_mm.Substring(pointpos - 2);

            // 数値化
            double degree = double.Parse(dd);
            double minute = double.Parse(mm_mm);

            // 度に変換
            degree += minute / 60.0;

            return degree;

        }





#if false


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
        //Public Shared Function calcDistHubeny(p1 As PointD, p2 As PointD) As PointD;
        public static Point2D calcDistHubeny( Point2D p1,  Point2D p2 )
            {
                // 定数定義
                const double a = 6378137.0;               // 長半径（赤道半径）

                //#if 0
                //        'bはeの演算に使用するがeが定数化されているため未使用   
                //        const b As Decimal = 6356752.314245          '短半径（極半径）
                //#endif
                const double ee = 0.00669437999019758;    // (第一離心率e)^2
                const double a1e2 = 6335439.32729246;     // a(1-e^2)

                //p1,p2をラジアン化
                double x1 = p1.x * Math.PI / 180.0;
                double y1 = p1.y * Math.PI / 180.0;
                double x2 = p2.x * Math.PI / 180.0;
                double y2 = p2.y * Math.PI / 180.0;

                //
                double avgy = (y1 + y2) / 2.0;        // 平均緯度
                double W = Math.Sqrt(1 - ee * Math.Sin(avgy) * Math.Sin(avgy));
                double M = a1e2 / (W * W * W);            // 子午線曲率半径
                double N = a / W;                     // 卯酉線曲率半径

                //移動距離算出
                double dy = M * (y2 - y1);                    //'北方向移動距離
                double dx = N * Math.Cos(avgy) * (x2 - x1);   //'東方向移動距離

                return new Point2D(dx, dy);
            }
#endif

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
            //Public Shared Function calcDistHubeny(p1 As PointD, p2 As PointD) As PointD;
            public static Point2D calcDistHubeny( Point2D p1, Point2D p2 )
            {
                // 定数定義
                const double a = 6378137.0;               // 長半径（赤道半径）

                //#if 0
                //        'bはeの演算に使用するがeが定数化されているため未使用   
                //        const b As Decimal = 6356752.314245          '短半径（極半径）
                //#endif
                const double ee = 0.00669437999019758;    // (第一離心率e)^2
                const double a1e2 = 6335439.32729246;     // a(1-e^2)

                //p1,p2をラジアン化
                double x1 = p1.x * Math.PI / 180.0;
                double y1 = p1.y * Math.PI / 180.0;
                double x2 = p2.x * Math.PI / 180.0;
                double y2 = p2.y * Math.PI / 180.0;

                //
                double avgy = (y1 + y2) / 2.0;        // 平均緯度
                double W = Math.Sqrt(1 - ee * Math.Sin(avgy) * Math.Sin(avgy));
                double M = a1e2 / (W * W * W);            // 子午線曲率半径
                double N = a / W;                     // 卯酉線曲率半径

                //移動距離算出
                double dy = M * (y2 - y1);                    //'北方向移動距離
                double dx = N * Math.Cos(avgy) * (x2 - x1);   //'東方向移動距離

                return new  Point2D(dx, dy);
            }







    }
}
