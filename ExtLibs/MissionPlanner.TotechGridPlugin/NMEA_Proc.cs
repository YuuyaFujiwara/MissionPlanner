using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech
{
    class NMEA_Proc
    {

        /// 
        /// NMEAファイルを読み込んで、tPolygonで返す
        ///
        /// \param filename
        /// \return 
        public static tPolygon NMEA2Polygon(string filename)
        {
            tPolygon polygon = new tPolygon();

            // 原点座標(緯度経度)
            Point2D org_pos = new Point2D();
            bool org_pos_flag = false;

            //ファイルの読み込み
            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    str = str.Replace("$GP", "$GN"); // $GPGGA->$GNGGA
                    string title = str.Substring(0, 6);
                    if (title == "$GNGGA")
                    {
                        // 緯度経度読み出し
                        Point2D pos = Calc_GPS.GPS_Log_to_LatLang(str);

                        // 緯度経度→始点を原点としたメートル単位に変換
                        if (org_pos_flag)
                        {
                            pos = Calc_GPS.calcDistHubeny(org_pos, pos);
                        }
                        else
                        {
                            org_pos = pos;
                            polygon.Set_Org_LngLat(org_pos);

                            pos = new Point2D(0.0, 0.0);
                            org_pos_flag = true;
                        }

                        // Polygonに保存
                        polygon.Add(pos);
                    }

                }
            }


            return polygon;
        }


    }
}
