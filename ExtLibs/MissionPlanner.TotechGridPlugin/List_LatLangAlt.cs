using GMap.NET;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner
{
    class List_LatLangAlt : List<PointLatLngAlt>, IFormattable
    {
        // コンストラクタ
        // ファイルから圃場形状（MPのポリゴン）を読み込む
        // original : FlightPlanner.cs::loadPolygonToolStripMenuItem_Click()
        public List_LatLangAlt(string filename)
        {
            LoadFromFile(filename);
        }

        public string Title { get; private set; }




        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFromFile(string filename)
        {
            // 現在のバッファクリア
            Title = "";
            this.Clear();

            // ファイルから読み込み
            using (StreamReader sr = new StreamReader(filename))
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

                        Add(new PointLatLng(
                            double.Parse(items[0], CultureInfo.InvariantCulture),
                            double.Parse(items[1], CultureInfo.InvariantCulture)));

                    }
                }
            }

            // タイトル設定
            Title = System.IO.Path.GetFileNameWithoutExtension(filename);

        }




        // 圃場形状（MPのポリゴン）をファイルに書き込む
        // 
        // original : FlightPlanner.cs::savePolygonToolStripMenuItem_Click()
        public void SaveToFile( string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("#saved by TotechGrid in MissionPlanner " + Application.ProductVersion);

                if (Count > 0)
                {
                    foreach (var pll in this)
                    {
                        sw.WriteLine(pll.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll.Lng.ToString(CultureInfo.InvariantCulture));
                    }

                    PointLatLng pll2 = this[0];

                    sw.WriteLine(pll2.Lat.ToString(CultureInfo.InvariantCulture) + " " + pll2.Lng.ToString(CultureInfo.InvariantCulture));
                }

                sw.Close();
            }
        }

        // stringに変換
        // ComboBoxに表示するタイトル等
        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return Title;
        }
    }
}
