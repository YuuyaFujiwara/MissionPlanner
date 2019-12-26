


using System;
using System.Collections.Generic;

namespace totech
{


    /*
     * ユーティリティクラス
     */
    class GeomUtils
    {

        // 3点のなす内積
        public static double dot(Point2D p1, Point2D p2, Point2D p3)
        {
#if true
            return (p1.x - p2.x) * (p3.x - p2.x) + (p1.y - p2.y) * (p3.y - p2.y);
#else
            Point2D v1(p2, p1 );
            Point2D v2(p2, p3 );
            return v1.dot(v2);
#endif
        }


        // 平面幾何における外積(スカラ値)
        public static double cross(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }
        public static double cross(Point2D v1, Point2D v2)
        {
            return v1.x * v2.y - v2.x * v1.y;
        }
        public static double cross(tLine l1, tLine l2 )
        {
            return cross(l1.ToVector(), l2.ToVector());
        }

        // (x1, y1) -> (x2, y2) -> (x3, y3) と進む道のりが反時計回りの場合は正の値、
        // 時計回りの場合は負の値、一直線上の場合はゼロを返す
        public static double ccw(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            return cross(x2 - x1, y2 - y1, x3 - x2, y3 - y2);
        }

        public static double ccw(Point2D p1, Point2D p2, Point2D p3)
        {
            return ccw(p1.x, p1.y, p2.x, p2.y, p3.x, p3.y);
        }

        /// ベクトルv1基準でベクトルv2の向きを調べる
        /// \param v1 ベクトル
        /// \param v2 ベクトル
        /// \return 反時計回りの場合は正の値、時計回りの場合は負の値、一直線上の場合はゼロを返す
        public static double ccw(Point2D v1, Point2D v2)
        {
            return ccw(-v1, new Point2D(0, 0), v2);
        }

        // 単位ベクトル化してccw
        public static double normalized_ccw(Point2D p1, Point2D p2, Point2D p3)
        {
            return ccw(p1.normalized(), p2.normalized(), p3.normalized());
        }

        // 与えられた３点の角度を２分するベクトル
        public static Point2D half_angle(Point2D p1, Point2D p2, Point2D p3)
        {
            Point2D v1 = new Point2D(p2, p1 );
            Point2D v2 = new Point2D(p2, p3 );

            // ２つのベクトルを正規化（長さ＝１）して加えると角度を２分するベクトルになる。
            // （ただし、正規化はされていない）
            return v1.normalized() + v2.normalized();

            // 3点が直線に並んでいた場合・・・
            // v1とv2が同じ方向の場合、問題なし。
            // v1とv2が逆方向の場合、長さゼロのベクトルが返る。

        }

        //
        // 角に内接して半径Rする円の中心
        //
        public static Point2D inner_circle_point(Point2D p1, Point2D p2, Point2D p3, double R)
        {
            // 角をなす2本のベクトル（長さ＝１）
            Point2D v1 = new Point2D(p2, p1).normalized();
            Point2D v2 = new Point2D(p2, p3).normalized();

            // 角度を半分するベクトル
            // ２つのベクトルを正規化（長さ＝１）して加えると角度を２分するベクトルになる。
            // （ただし、正規化はされていない）
            Point2D v = v1 + v2;

            Point2D v_diff = v1 - v2;

            // 内接円の中心までの距離
            double d = 0;
            if (!v.is_zero() && !v_diff.is_zero())
                d = 2 * R / v.norm() / v_diff.norm();

            // 内接円の中心
            return p2 + v * d;
        }


        // 直線と線分が交差するか調べる。
        // ※line1は無限長の直線、line2は(x1,y1)-(x2,y2)の線分であるので注意。
        // 戻り値：交差する=true
        // http://www5d.biglobe.ne.jp/~tomoya03/shtml/algorithm/Intersection.htm
        public static bool line_cross_check(tLine line1, tLine line2 )
        {
            Point2D p1 = new Point2D(line1.x1, line1.y1);
            Point2D p2 = new Point2D(line1.x2, line1.y2);
            Point2D p3 = new Point2D(line2.x1, line2.y1);
            Point2D p4 = new Point2D(line2.x2, line2.y2);

            bool rslt = (((p1.x - p2.x) * (p3.y - p1.y) + (p1.y - p2.y) * (p1.x - p3.x)) *
                          ((p1.x - p2.x) * (p4.y - p1.y) + (p1.y - p2.y) * (p1.x - p4.x)) <= 0.0);

            return rslt;
        }


        // 直線の交点を求める
        // http://www.sousakuba.com/Programming/gs_two_lines_intersect.html
        public static Point2D cross_point(tLine line1, tLine line2 )
        {
            //            // 単位ベクトル
            //            Vector2D n1 = Vector2D( line1.x1, line1.y1, line1.x2, line1.y2 ).normalized();
            //            Vector2D n2 = Vector2D( line2.x1, line2.y1, line2.x2, line2.y2 ).normalized();
            Point2D n1 = line1.ToVector().normalized();
            Point2D n2 = line2.ToVector().normalized();

            Point2D AC = new Point2D(line2.x1 - line1.x1, line2.y1 - line1.y1);

            // 
            double dot = n1.dot(n2);
            double work = 1 - dot * dot;
            if (Math.Abs(work) < 1e-10) return new Point2D();  // 平行

            double d1 = (n1.dot(AC) - dot * n2.dot(AC)) / work;
         //   double d2 = (dot * n1.dot(AC) - n2.dot(AC)) / work;

            Point2D p1 = new Point2D(line1.x1, line1.y1) + n1 * d1;
         //   Point2D p2 = Point2D(line2.x1, line2.y1) + n2 * d2;   // p1と同じになるはず。

            return p1;
        }

        /// 2本の直線を無限長にした際の交点の角度を求める。
        /// \param line1
        /// \param line2
        /// \return 角度(radian))
        public static double cross_angle(tLine line1, tLine line2 )
        {
            Point2D v1 = line1.ToVector();
            Point2D v2 = line2.ToVector();

            return Point2D.AngleOf2Vector(v1, v2);
        }









        // 円と接線の交点
        public static Point2D cross_point(tCircle circle, tLine line )
        {
            // 直線を ax + by + c = 0のa,b,cに変換
            double dx = line.x2 - line.x1;
            double dy = line.y2 - line.y1;
            double a = dy;
            double b = -dx;
            double c = dx * line.y1 - dy * line.x1;

            double sq = 1.0 / (a * a + b * b);
            double factor = -c * sq;
            double xo = a * factor;
            double yo = b * factor;
            double root1 = Math.Sqrt(sq);
            double f = b * root1;
            double g = -a * root1;

            double fsq = f * f;
            double gsq = g * g;
            double fgsq = fsq + gsq;

            if (fgsq < 1e-10) return new Point2D(); // error check

            double xjo = circle.x - xo;
            double yjo = circle.y - yo;
            double fygx = f * yjo - g * xjo;
            double sq2 = circle.r * circle.r * fgsq - fygx * fygx;

            if (sq2 < -1e-10)
            {  /* 交点なし */
                return new Point2D();
            }

            double fxgy = f * xjo + g * yjo;

            if (sq2 < 1e-10)
            {  /* 直線と円は接する。*/
                double t = fxgy / fgsq;
                double px = xo + f * t;
                double py = yo + g * t;
                return new Point2D(px, py);
            }
            else
            {
                double root3 = Math.Sqrt(sq2);
                double fginv = 1.0 / fgsq;
                double t1 = (fxgy - root3) * fginv;
                double t2 = (fxgy + root3) * fginv;
                double x1 = xo + f * t1;
                double y1 = yo + g * t1;
                double x2 = xo + f * t2;
                double y2 = yo + g * t2;
                // 2点得られた場合は、(x1,y1)を返す
                return new Point2D(x1, y1);
            }  /* end if */

        }

        // 点と円の接線の、円との接点を返す
        //
        // 共通接線は２本あるが、どれを得るかはmodeで決める。
        //　mode = 0 :
        //       = 1 :
        public static Point2D TangentPoint(tCircle c1, Point2D p2, int mode)
        {
            // 円の中心
            Point2D p1 = new Point2D(c1.x, c1.y);
            // 円から点へのベクトル
            Point2D dv = new Point2D(p1, p2);
            double l = dv.norm();     // 中心の距離

            double sin = c1.r / l;
            double sinsq = sin * sin;

            // 近すぎる場合接線無し
            if (sinsq > (1 - 1e-10)) return new Point2D();

            double cos = Math.Sqrt(Math.Max(1 - sinsq, 0.0));
            if (mode == 0 || mode == 2) cos *= -1;    // modeにより符号反転(接線の２本目)

            // 中心→接点へのベクトル
            Point2D n = dv * (new Point2D(sin, cos)) / l;
            // 接点
            Point2D p = p1 + n * c1.r;

            return p;

        }


        // 点と円の接線を得る
        //
        // 共通接線は２本あるが、どれを得るかはmodeで決める。
        //　mode = 0 :
        //       = 1 :
        public static tLine TangentLine(tCircle c1, Point2D p2, int mode)
        {
            return new tLine(TangentPoint(c1, p2, mode), p2);
        }


        // ２つの円の共通接線を得る
        //
        // 共通接線は最大４本あるが、どれを得るかは各円のcw/ccwで決める。
        public static tLine TangentLine(tCircle c1, tCircle c2 )
        {
            // c1とc2の回転方向から接線のmodeを決める
            int mode = c1.ccw && c2.ccw ? 0   // ccw→ccw : 外接線１
                     : !c1.ccw && !c2.ccw ? 1   // cw→cw   : 外接線２
                     : c1.ccw && !c2.ccw ? 2   // ccw→cw  : 内接線１
                     : !c1.ccw && c2.ccw ? 3   // cw→ccw  : 内接線２
                     : -1;                      // impossible

            return TangentLine(c1, c2, mode);
        }



        // ２つの円の共通接線を得る
        //
        // 共通接線は最大４本あるが、どれを得るかはmodeで決める。
        //　mode = 0 : 共通外接線1
        //       = 1 : 共通外接線2
        //       = 2 : 共通内接線1
        //       = 3 : 共通内接線3
        public static tLine TangentLine(tCircle c1, tCircle c2, int mode)
        {
            // 円の中心
            Point2D p1 = new Point2D(c1.x, c1.y);
            Point2D p2 = new Point2D(c2.x, c2.y);
            // 円１から円２へのベクトル
            Point2D dv = new Point2D(p1, p2);
            double l = dv.norm();     // 中心の距離

            // 近すぎる場合共通接線無し
            if (l < 1e-10) return new tLine();

            // c2の半径
            double c2r = c2.r;
            if (mode == 2 || mode == 3)
                c2r *= -1;  // 内接線なら負号反転            

            double sin = (c1.r - c2r) / l;
            double sinsq = sin * sin;

            // 近すぎる場合共通接線無し
            if (sinsq > 1 - 1e-10) return new tLine();

            double cos = Math.Sqrt(Math.Max(1 - sinsq, 0.0));
            if (mode == 0 || mode == 2) cos *= -1;    // modeにより符号反転(内or外接線の２本目)

            // 中心→接点へのベクトル
            Point2D n = dv * (new Point2D(sin, cos)) / l;
            // 接点
            Point2D lp1 = p1 + n * c1.r;
            Point2D lp2 = p2 + n * c2r;

            return new tLine(lp1.x, lp1.y, lp2.x, lp2.y);
        }





#if false
        //"findMaximumDistance" used as part of implementation for RDP algorithm.

        // 始点－終点を結んだ直線から最大距離の点を返す
        /// \return 最大距離の点のIndexと距離値のpair
        //public static std::pair<int, double> findMaximumDistance(tPolygon Points)
        public static KeyValuePair<int, double> findMaximumDistance(tPolygon Points)
        {
            Point2D firstpoint = Points[0];
            Point2D lastpoint = Points[Points.Count - 1];
            int index = 0;      //index to be returned
            double Mdist = -1;     //the Maximum distance to be returned

            //distance calculation
            Point2D p = lastpoint - firstpoint;
            double p_norm = p.norm();
            for (int i = 1; i < Points.Count - 1; i++)
            {
                //traverse through second point to second last point
                Point2D pp = Points[i] - firstpoint;

                double Dist;
                if (p.is_zero())
                {
                    // 始点と終点が同じ場合、始点からの距離
                    // Dist = Math.Abs(Point2D.distance(firstpoint, p));   C++版もpからの距離になっているので注意。
                    Dist = Math.Abs(pp.norm());

                }
                else
                {
                    // 始点と終点を結ぶ直線からの距離
                    // double Dist = Math.Abs(pp * p) / p.norm(); //formula for point-to-line distance　※ここ注意
                    Dist = Math.Abs(cross(pp, p)) / p.norm();
                }

                if (Dist > Mdist)
                {
                    Mdist = Dist;
                    index = i;
                }
            }
            //return std::make_pair(index, Mdist);
            return new KeyValuePair<int, double>(index, Mdist);
        }
#else
        //"findMaximumDistance" used as part of implementation for RDP algorithm.

        // 始点－終点を結んだ直線から最大距離の点を返す
        /// \return 最大距離の点のIndexと距離値のpair
        //public static std::pair<int, double> findMaximumDistance(tPolygon Points)
        public static KeyValuePair<int, double> findMaximumDistance(tPolygon Points, int start_index, int end_index)
        {
            Point2D firstpoint = Points[start_index];
            Point2D lastpoint = Points[end_index];
            int index = 0;      //index to be returned
            double Mdist = -1;     //the Maximum distance to be returned

            //distance calculation
            Point2D p = lastpoint - firstpoint;
            double p_norm = p.norm();
            for (int i = start_index + 1; i <= end_index; i++)
            {
                //traverse through second point to second last point
                Point2D pp = Points[i] - firstpoint;

                double Dist;
                if (p.is_zero())
                {
                    // 始点と終点が同じ場合、始点からの距離
                    // Dist = Math.Abs(Point2D.distance(firstpoint, p));   C++版もpからの距離になっているので注意。
                    Dist = Math.Abs(pp.norm());

                }
                else
                {
                    // 始点と終点を結ぶ直線からの距離
                    // double Dist = Math.Abs(pp * p) / p.norm(); //formula for point-to-line distance　※ここ注意
                    Dist = Math.Abs(cross(pp, p)) / p.norm();
                }

                if (Dist > Mdist)
                {
                    Mdist = Dist;
                    index = i;
                }
            }
            //return std::make_pair(index, Mdist);
            return new KeyValuePair<int, double>(index, Mdist);
        }
#endif



#if false
       //"simplifyWithRDP" returns the simplified path with a Point vector. The function takes in the paths to be simplified and a customerized thresholds for the simplication.
        // ポリゴンを単純化する
        //  Ramer–Douglas–Peucker アルゴリズム
        // 2018.01.25 始点終点スムーズアルゴリズム見直し
        public static tPolygon simplifyWithRDP(tPolygon Points, double epsilon)
        {
            if (Points.Count < 3)
            {  //base case 1
                return Points;
            }

            // 最後に始点と同じ点を追加する。
            //（ 始点と終点をつなぐ）
            tPolygon tmpPolygon = new tPolygon( Points);
            tmpPolygon.Add( new Point2D( tmpPolygon[0] ));

            tmpPolygon = simplifyWithRDP_sub(tmpPolygon, epsilon);

            if (tmpPolygon.Count < 3)
            {  //base case 1
                return tmpPolygon;
            }

            // 始点と終点をスムーズにする。（2018.01.25Ver2）
            //１）全ポイントをサーチ
            //２）前後の点となす三角形を想定し、内接円を求める
            //３）内接円の半径がepsilon未満であれば点を削除
            //※ある点を削除することで、すでにサーチ済みの点の内接円が変化するが、気にしない（妥協する）
            for (int i = tmpPolygon.Count - 1; i >= 0; i--)
            {
                Point2D p0 = tmpPolygon[tmpPolygon.index_trim(i - 1)];
                Point2D p1 = tmpPolygon[i];
                Point2D p2 = tmpPolygon[tmpPolygon.index_trim(i + 1)];

                // 内接円を得る
                tCircle inner_circle = tCircle.TangentCircle(p0, p1, p2);
                if (inner_circle.r < epsilon)
                {
                    // tmpPolygon.erase(tmpPolygon.begin() + i);
                    tmpPolygon.RemoveAt(i);
                    if (tmpPolygon.Count <= 3)
                        break;
                }
            }


            return tmpPolygon;

        }
#else

        //"simplifyWithRDP" returns the simplified path with a Point vector. The function takes in the paths to be simplified and a customerized thresholds for the simplication.
        // ポリゴンを単純化する
        //  Ramer–Douglas–Peucker アルゴリズム
        // 2018.01.25 始点終点スムーズアルゴリズム見直し
        // 2019.01.11
        public static tPolygon simplifyWithRDP2(tPolygon Points, double epsilon)
        {
            if (Points.Count < 3)
            {  //base case 1
                return Points;
            }

            // 最後に始点と同じ点を追加する。
            //（ 始点と終点をつなぐ）
            tPolygon tmpPolygon = new tPolygon(Points);
            tmpPolygon.Add(new Point2D(tmpPolygon[0]));

            simplifyWithRDP_sub2(tmpPolygon, 0, tmpPolygon.Count-1, epsilon);

            if (tmpPolygon.Count < 3)
            {  //base case 1
                return tmpPolygon;
            }

            // 始点と終点をスムーズにする。（2018.01.25Ver2）
            //１）全ポイントをサーチ
            //２）前後の点となす三角形を想定し、内接円を求める
            //３）内接円の半径がepsilon未満であれば点を削除
            //※ある点を削除することで、すでにサーチ済みの点の内接円が変化するが、気にしない（妥協する）
            for (int i = tmpPolygon.Count - 1; i >= 0; i--)
            {
                Point2D p0 = tmpPolygon[tmpPolygon.index_trim(i - 1)];
                Point2D p1 = tmpPolygon[i];
                Point2D p2 = tmpPolygon[tmpPolygon.index_trim(i + 1)];

                // 内接円を得る
                tCircle inner_circle = tCircle.TangentCircle(p0, p1, p2);
                if (inner_circle.r < epsilon)
                {
                    // tmpPolygon.erase(tmpPolygon.begin() + i);
                    tmpPolygon.RemoveAt(i);
                    if (tmpPolygon.Count <= 3)
                        break;
                }
            }


            return tmpPolygon;

        }
#endif



#if false
        //"simplifyWithRDP" returns the simplified path with a Point vector. The function takes in the paths to be simplified and a customerized thresholds for the simplication.
        // ポリゴンを単純化する
        //  Ramer–Douglas–Peucker アルゴリズム
        public static tPolygon simplifyWithRDP_sub(tPolygon Points, double epsilon)
        {
            if (Points.Count < 3)
            {  //base case 1
                return new tPolygon(Points);
            }

            KeyValuePair<int, double> maxDistance =  findMaximumDistance(Points);
            if (maxDistance.Value  >= epsilon)
            {
                int index = maxDistance.Key ;
                tPolygon path1 = new tPolygon(Points, 0, index + 1);                //new path l1 from 0 to index
                tPolygon path2 = new tPolygon(Points, index, Points.Count-1);    // new path l2 from index to last

                tPolygon r1 = simplifyWithRDP_sub(path1, epsilon);
                tPolygon r2 = simplifyWithRDP_sub(path2, epsilon);

                // 不要なバッファの開放
                path1 = null;
                path2 = null;

                //Concat simplified path1 and path2 together
#if false
                tPolygon rs= new tPolygon(r1);  // コピーを作る
#else
                tPolygon rs =  r1;    // コピーを作らない
#endif

#if false
                // C++
                rs.pop_back();
                rs.insert(rs.end(), r2.begin(), r2.end());
#else
                // C#
                rs.RemoveAt(rs.Count-1);
                rs.AddRange(r2);
#endif
                return rs;
            }
            else
            { //base case 2, all points between are to be removed.
                //Points.erase(Points.begin() + 1, Points.end() - 1);
                tPolygon pts = new tPolygon();
                pts.Add(Points[0].Clone());
                pts.Add(Points[Points.Count - 1].Clone());
                return pts;
            }
        }
#else
        //"simplifyWithRDP" returns the simplified path with a Point vector. The function takes in the paths to be simplified and a customerized thresholds for the simplication.
        // ポリゴンを単純化する
        //  Ramer–Douglas–Peucker アルゴリズム
        // 2019.01.11 呼び出したバッファ自体に変更が入る
        // 2019.01.11 引数にIndexを指定するように変更
        // start_index : Points内の開始Index
        // end_index   : Points内の終了Index
        public static void simplifyWithRDP_sub2(tPolygon Points, int start_index, int end_index, double epsilon)
        {
            if(( end_index - start_index) < 2 )
            {  //base case 1
                return;
            }

            // 始点からの最大距離の点を探す
            KeyValuePair<int, double> maxDistance = findMaximumDistance(Points, start_index, end_index);
            // 最大距離が離れているか？
            if (maxDistance.Value >= epsilon)
            {
                // 最大距離の点で2つに分け、それぞれを単純化（再帰）
                int index = maxDistance.Key;
                simplifyWithRDP_sub2(Points, index, end_index, epsilon);        // 後半から処理を行う
                simplifyWithRDP_sub2(Points, start_index, index, epsilon);      // 前半

            }
            else
            {
                //base case 2, all points between are to be removed.
                //Points.erase(Points.begin() + 1, Points.end() - 1);

                // 最初と最後のみ残して削除
                Points.RemoveRange( start_index + 1, end_index - start_index -1);
            }
        }
#endif


        // ポリゴンを単純化する
        //   1) Ramer–Douglas–Peuckerアルゴリズムにより単純化
        //   2) 縮小→拡大により余計な線を除去する
        //
        public static tPolygon SimplifiedPolygon(tPolygon src_polygon, double epsilon)
        {
            tPolygon tmp = src_polygon;
            tmp = simplifyWithRDP2(tmp, epsilon);
            tmp = tmp.ReducedPolygon(epsilon);          // 小さくする
            tmp = tmp.ReducedPolygon(-1 * epsilon);     // 大きくする

#if false
        // for debug
        tmp.SaveToFile("./dbg_tmp.pol");
            tmp0.SaveToFile("./dbg_tmp0.pol");
            tmp1.SaveToFile("./dbg_tmp1.pol");
            tmp2.SaveToFile("./dbg_tmp2.pol");

            /*debug*/
            return tmp1;
#endif

        return tmp;
        }

    }


} // namespace totech


// end of text