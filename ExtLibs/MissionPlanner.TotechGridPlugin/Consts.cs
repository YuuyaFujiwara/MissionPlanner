using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Consts
    {
        // 圃場形状一覧
        //public const string FieldShape_Format = "{0}.shapes";
        public const string FieldShape_Path = "./Shapes";
        //public const string FieldShape_Filter = "*.shapes";
        public const string FieldShape_Ext = "shapes";

        // ルートファイル一覧
        public const string RouteFile_Path = "./Routes/tmp";
        public const string RouteFile_Filter = "*.routes";

        // ルートファイル名書式(String.Format)
        public const string RouteFile_Format = "{0:0.000000}-{1:0.000000}.routes";

        // GNSSログ(NMEA)ファイル
        public const string GnssLog_Path = "./GNSS";
        public const string GnssLog_Filter = "*.nmea";

    }
}
