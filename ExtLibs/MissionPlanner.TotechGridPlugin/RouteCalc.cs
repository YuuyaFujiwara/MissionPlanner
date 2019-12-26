using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech
{
    enum RouteType
    {
        ZIGZAG,     // ジグザク
        SPIRAL      // 渦巻き
    };


    enum RouteStatus
    {
        UNDEFINED = 0,      // 未定義         
        START = 10,         // 開始地点
        BEGIN = 11,         // 開始アプローチ
        STRAIGHT = 20,      // 籾播き直線（往路）
        STRAIGHT2 = 21,     // 籾播き直線（復路）
        R_TURN = 31,        // 右旋回
        L_TURN = 32,        // 左旋回
        RETURN = 900,       // 籾播き終了し帰還中
        END = 999,          // 籾播き終了
        OUTOFRANGE = 9999,  // 範囲外
    };



    /*
     * ルート演算クラス
     */
    class RouteCalc
    {
        // 定数の定義    
        private double DEFAULT_EDGE_MARGIN = 2.0;   // 圃場形状（畦）からのマージン
        private double DEFAULT_ROUTE_PITCH = 3.0;   // ルートの間隔
                                                    //private double DEFAULT_MOMIMAKI_R = 4.0;   // 籾播き半径
        private double DEFAULT_HOVER_R = 2.0;   // ホバー旋回半径
        private double DEFAULT_ROUTE_STEP = 0.5;   // ルート分割ピッチ





        // メンバ変数
        public RouteType route_type;        // ルート演算タイプ 0:Zigzag_cw, 1:zigzag_ccw, 2:spiral_cw, 3:spiral_ccw;
        public double edge_margin;       // 圃場形状（畦）からのマージン
        public double route_pitch;       // ルートの間隔
                                    //double m_momimaki_r;      // 籾播き半径
        public double hover_r;         // ホバー旋回半径
        public double split_step;           // ルート分割ピッチ




        /// デフォルトコンストラクタ
        ///
        public RouteCalc()
        {
            // メンバデフォルト値設定
            route_type = RouteType.ZIGZAG;                       // zigzag
            edge_margin = DEFAULT_EDGE_MARGIN;    // 圃場形状（畦）からのマージン
            route_pitch = DEFAULT_ROUTE_PITCH;    // ルートの間隔
                                                    //  m_momimaki_r    = DEFAULT_MOMIMAKI_R;      // 籾播き半径    
            hover_r = DEFAULT_HOVER_R;            // ホバー旋回半径
            split_step = DEFAULT_ROUTE_STEP;            // ルート分割ピッチ
        }

        /// コンストラクタ
        /// \param margin   圃場形状外周からのマージン
        /// \param pitch    ルート間隔
        /// \param hover_r  ホバー旋回最小半径
        /// \param step     ルート分割ステップ[m]
        public RouteCalc(RouteType type, double margin, double pitch, double hover_r, double step)
        {
            route_type = type;     // ルート演算タイプ 0:Zigzag_cw, 1:zigzag_ccw, 2:spiral_cw, 3:spiral_ccw;
            edge_margin = margin;   // 圃場形状（畦）からのマージン
            route_pitch = pitch;    // ルートの間隔
            this.hover_r = hover_r;  // ホバー旋回半径
            split_step = step;     // ルート分割ピッチ
        }




        // ルート演算を行う
        // ルート演算プレゼン画像作成用
        // ピッチとか荒くしているので注意
        //
        public bool RouteCalc_Exec(tPolygon src_polygon, tPolygon rsltbuff)
        {
            bool rslt = false;

            // 作業用Polygon作成
            tPolygon src_pol = new tPolygon(src_polygon);

#if false
            // route_typeに従い処理を分ける
            // ccwの場合、反転
            if (route_type == 1 || route_type == 2)
            {
                src_pol.RotateForward();
                src_pol.Reverse();
            }
#endif

            // route_typeに従い処理を分ける
            // zigzagか、spiralか
            if (route_type == RouteType.SPIRAL )
            {
                // 渦巻(Spiral)
                rslt = PolygonFill_Spiral(src_pol, rsltbuff, edge_margin, route_pitch, hover_r, split_step);    // 渦巻

            }
            else if( route_type == RouteType.ZIGZAG )
            {
                // ジグザグ
                rslt = PolygonFill_Zigzag(src_pol, rsltbuff, edge_margin, route_pitch, hover_r, split_step);  //ジグザク

            }
            else
            {
                // Unknown Route Type
                throw new NotImplementedException();
            }

            // 原点緯度経度をコピー
            rsltbuff.Set_Org_LngLat(src_polygon);

            return rslt;
        }




        // 角に内接する円を描く
        // 始点と終点は半径ゼロの円
        //
        void CreateCircles2(tPolygon polygon, List<tCircle> circles, double radius)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                double r = radius; //get_minimum_R();   // 円の半径＝ホバーの最小旋回半径

                Point2D p;        // 内接円の中心
                bool ccw = false;

                if (i == 0 || i == (polygon.Count - 1))
                {   // 始点又は終点
                    r = 0;
                    p = polygon[i].Clone();
                    ccw = false;
                }
                else
                {
                    // 角をなす３点
                    Point2D p1 = polygon[i - 1];
                    Point2D p2 = polygon[i];
                    Point2D p3 = polygon[i + 1];

                    // 角に内接して半径Rの円の中心
                    p = GeomUtils.inner_circle_point(p1, p2, p3, r);

                    // 回転方向
                    ccw = (bool)(GeomUtils.ccw(p1, p2, p3) > 0);
                }

                // 円を作成
                tCircle tmp_circle = new tCircle(p.x, p.y, r);
                // 回転方向を決める
                tmp_circle.ccw = ccw;

                // 円をコレクション
                circles.Add(tmp_circle);
            }
        }


        // 直線を分割する。
        // 分割した点を格納した配列を返す。
        // 配列に始点は含まれない。　終点は含まれる。
        //
        // 2018.01.12 Point2D::status対応
        //
        private List<Point2D> LineSplit(tLine line, double D, totech.RouteStatus status)
        {
            List<Point2D> ary = new List<Point2D>();
            LineSplit(ary, line, D, status);
            return ary;
        }


        // 直線を分割する。
        // 分割した点を格納した配列を返す。
        // 配列に始点は含まれない。　終点は含まれる。
        //
        // 2018.01.12 Point2D::status対応
        private void LineSplit(List<Point2D> ary, tLine line, double D, totech.RouteStatus status)
        {
            if (line.is_zero()) return;

            Point2D p = line.get_p1();

            // status設定
            p.status = status;

            // pからpeに向かい長さＤのベクトル
            Point2D vd = line.ToVector().normalized() * D;

            int num = (int)(line.length() / D);

            for (int i = 0; i < num; i++)
            {
                // p1をvdだけ移動
                p += vd;

                // 配列に追加
                ary.Add(p);
            }

            //終点を追加
            p = line.get_p2();
            p.status = status;
            ary.Add(p);
        }


        // 円を分割する。
        // 分割した点を格納した配列を返す。
        // 配列に始点は含まれない。　終点は含まれる。
        //
        // 2018.01.12 Point2D::status対応
        //
        List<Point2D> CircleSplit(tCircle circle, double D, totech.RouteStatus status)
        {
            List<Point2D> ary = new List<Point2D>();
            CircleSplit(ary, circle, D, status);
            return ary;
        }

        // 円を分割する。
        // 分割した点を格納した配列を返す。
        // 配列に始点は含まれない。　終点は含まれる。
        //
        // 2018.01.12 Point2D::status対応
        //
        void CircleSplit(List<Point2D> ary, tCircle circle, double D, totech.RouteStatus status)
        {
            Point2D ps = circle.ps;
            Point2D pe = circle.pe;

            // 2018.01.12 Point2D::status対応
            ps.status = status;
            pe.status = status;

            // 円が小さい場合終点のみ
            if (circle.r < D)
            {
                // 終点が始点と同じなら足さな
                if (ps != pe && !pe.is_zero())
                    ary.Add(pe);
                return;
            }


            // 移動のためのベクトルを決める
            // サイズ１で回転させたい角度を持つ。
            double th = D / circle.r;       // 回転角度
            th *= (circle.ccw ? 1 : -1);   // cwなら逆回転
            Point2D vd = Point2D.fromPolar(1.0, th);

            // 円の中心
            Point2D pc = new Point2D(circle.x, circle.y);

            // 円の中心を原点として、終点
            Point2D pex = new Point2D(pc, pe);

            // 円の中心を原点として、操作したい点
            Point2D p1 = new Point2D(pc, ps);
            p1.status = status;     // 2018.01.12 Point2D::status対応

            // 原点
            Point2D p0 = new Point2D(0, 0);

            while (true)
            {
                // p1をvdだけ移動した点
                Point2D p = p1 * vd;    // 複素数的掛け算

                // peを超えていないかチェック
                // p->(原点)->peのccwを調べ、円のccwと比較
                double tmpccw = GeomUtils.ccw(p, p0, pex) * (circle.ccw ? 1 : -1);
                if (tmpccw > 0) break;  // 超えた

                // 配列に追加
                ary.Add(p + pc);
                p1 = p;
            }
            //終点を追加
            ary.Add(pe);
        }









        //
        // 多角形を小さくする。
        // target：多角形の頂点のPoint2Dの配列
        //      ※ targetの各座標が直接操作される。
        //      　 targetの要素数が減る場合がある。（頂点の数が減った場合
        //
        // distance : 移動距離(負数の場合、多角形が大きくなる)
        //
        bool PolygonToReduce(List<Point2D> target, double distance)
        {
            /*
             * 線を移動させて交点を求める方式
             */

            /*
             * 完ぺきではない。
             * 形状が複雑でdistanceが大きい場合（複数の点を同時に削除する場合など）
             * 結果が異常になる。
             * 
             * → とりあえずホバーの実用上 問題になる事は少ないので保留。
             */


            int target_size = target.Count;

            // 多角形が右回りか左回りか調べる
            double ccw_sum = 0;
            for (int j = 0; j < target_size; j++)
            {
                int i = j - 1; if (i < 0) i = target_size - 1;
                int k = j + 1; if (k >= target_size) k = 0;

                //ccw_sum += GeomUtils.normalized_ccw( target[i], target[j], target[k] );
                ccw_sum += GeomUtils.ccw(target[i], target[j], target[k]);
            }
            int ccw_sign = (ccw_sum > 0) ? 1 : -1;    // ccw:1, cw:-1

            // これ以降、「線を左に移動する」処理を行う。
            // 右回りの場合、distanceの符号を反転する。（左に -distance移動)
            distance *= ccw_sign;

            for (int i = 0; i < target_size; i++)
            {
                // 各点を指すindex
                int i0 = i;
                int i1 = i + 1; if (i1 >= target_size) i1 -= target_size;
                int i2 = i + 2; if (i2 >= target_size) i2 -= target_size;
                int i3 = i + 3; if (i3 >= target_size) i3 -= target_size;


                // 線の移動ベクトル
                Point2D V = new Point2D(target[i1], target[i2]) * (new Point2D(0, 1));
                V = V.normalized() * distance;              // 移動量調整

                // 操作対象の線を移動する
                tLine new_line = new tLine(target[i1] + V, target[i2] + V);

                // 移動によりp0, p3が消えた(p1,p2がp0,p3を越えて移動した)場合の処理
                // 交差チェック対象の線分を変更する
                tLine line0 = new tLine(target[i1], target[i0]);
                bool del_p1 = !GeomUtils.line_cross_check(new_line, line0);
                if (del_p1)
                {
                    int tmpi = i0 - 1; if (tmpi < 0) tmpi += target_size;
                    line0.x1 = target[tmpi].x;
                    line0.y1 = target[tmpi].y;
                }
                tLine line3 = new tLine(target[i2], target[i3]);
                bool del_p2 = !GeomUtils.line_cross_check(new_line, line3);
                if (del_p2)
                {
                    int tmpi = i3 + 1; if (tmpi >= target_size) tmpi -= target_size;
                    line3.x1 = target[tmpi].x;
                    line3.y1 = target[tmpi].y;
                }

                // 点を削除した結果、三角形が成り立たないならfalse
                int tmpnum = target_size;
                if (del_p1) tmpnum--;
                if (del_p2) tmpnum--;
                if (tmpnum < 3) return false;

                // 新たな線と、前後の線の交点を求める
                Point2D p1 = GeomUtils.cross_point(new_line, line0);
                Point2D p2 = GeomUtils.cross_point(new_line, line3);

                // 配列に戻す
                target[del_p1 ? i0 : i1] = p1;
                target[del_p2 ? i3 : i2] = p2;

                // 消えた点を配列から削除する。
                // 末尾から削除
                if (i3 > i0)
                {
                    if (del_p2)
                        target.RemoveAt(i2);
                    if (del_p1)
                        target.RemoveAt(i1);
                }
                else
                {
                    if (del_p1)
                        target.RemoveAt(i1);
                    if (del_p2)
                        target.RemoveAt(i2);
                }

                // 要素を削除した場合に、forの添え字を調整する
                if (del_p1 && (i0 <= i)) i--;
                if (del_p2 && (i3 <= i)) i--;
                target_size = target.Count;

            }
            return true;
        }



        //
        // 多角形を塗りつぶす（渦巻き）
        // src_polygon：多角形の頂点のPoint2Dの配列
        // rsltbuff：演算結果を返すバッファ
        //           描く曲線(短い直線)を表すPoint2Dの配列
        // area_r  ：籾播き半径。
        // radius  ：ホバーの旋回半径
        // pitch   ：曲線を分割するピッチ
        //
        // polybonの辺に沿って渦巻きを描く。
        // 
        // 2018.01.12 播きルートstatus追加
        //
        //bool PolygonFill_SpiralOld( tPolygon& src_polygon, tPolygon& rsltbuff, double area_r, double radius, double pitch, RouteCalc_Draw& drawer )
        bool PolygonFill_Spiral(tPolygon src_polygon, tPolygon rsltbuff, double margin, double pitch, double radius, double step)
        {
            // polygonを小さくする。
            // 土手に籾播きしないよう、area_r分内側を走る。
            tPolygon polygon = src_polygon.ReducedPolygon((margin));


            // 基線
            tLine line = polygon.getLine(0);
            // 開始点(田んぼの角)
            Point2D p01 = src_polygon[0];
            // 開始点から基線に至る内接円
            tCircle circle0 = tCircle.TangentCircle(p01, line.get_p1(), line.get_p2(), radius);
            // 線を分割してバッファに格納
            LineSplit(rsltbuff, (new tLine(p01, circle0.ps)), step, RouteStatus.BEGIN);
            CircleSplit(rsltbuff, circle0, step, RouteStatus.BEGIN);
            rsltbuff[0].status = RouteStatus.START;     // 始点


            int index = 0;
            int count = 0;
            Point2D p1;
            p1 = circle0.pe;    //
            while (true)
            {
                // 角をなす点を得る
                if (p1.is_zero()) p1 = polygon.getPoint(index);
                Point2D p2 = polygon.getPoint(index + 1);
                Point2D p3 = polygon.getPoint(index + 2);

                // 内接する円を得る
                tCircle circle = tCircle.TangentCircle(p1, p2, p3, radius);



                // 線を分割してバッファに格納
                LineSplit(rsltbuff, (new tLine(p1, circle.ps)), step, RouteStatus.STRAIGHT);
                // 円を分割してバッファに格納
                CircleSplit(rsltbuff, circle, step, (circle.ccw ? RouteStatus.L_TURN : RouteStatus.R_TURN));
                p1 = circle.pe;

                // polygonの、通過した線を内側に移動する。
                polygon.LineMove(index, pitch, polygon.ccw());

                index = polygon.index_trim(index + 1);
                count++;

                //　仮の終了条件
                if (count > 6)
                    break;

                // 終了条件　
                if (false)
                    break;

            }

            // ルート演算成否
            return (bool)(rsltbuff.Count > 1);

        }



        //
        // 多角形を塗りつぶす（ジグザグ）
        // src_polygon：多角形の頂点のPoint2Dの配列
        // rsltbuff：演算結果を返すバッファ
        //           描く曲線(短い直線)を表すPoint2Dの配列
        // area_r  ：籾播き半径。
        // margin  : 圃場形状外周からの間隔
        // pitch   : ルートの感覚
        // radius  ：ホバーの旋回半径
        // step    ：曲線を分割するピッチ
        //
        // polybonの最初の辺に平行にジグザグを描く
        //bool PolygonFill_Zigzag( tPolygon& src_polygon, tPolygon& rsltbuff, double momimaki_r, double radius, double pitch )
        private bool PolygonFill_Zigzag(tPolygon src_polygon, tPolygon rsltbuff, double margin, double pitch, double radius, double step)
        {


            // polygonを小さくする。
            // 土手に籾播きしないよう、内側を走る。
            // 
            tPolygon polygon = src_polygon.ReducedPolygon((margin));

            // 往復を管理するフラグ
            bool reverse_flag = false;

            int index = 0;  // ０番目の線を基線にする。

            // 基線が短すぎる場合別の線を基線とする
            for (index = 0; index < polygon.Count; index++)
            {
                if (polygon.getLine(index).length() > radius * 1.5)
                    break;
            }
            if (index >= polygon.Count) index = 0;

            // ジグザクの基線
            tLine line = polygon.getLine(index);
            // 開始点(田んぼの角)
            Point2D p01 = src_polygon[0];
            // 開始点から基線に至る内接円
            tCircle circle0 = tCircle.TangentCircle(p01, line.get_p1(), line.get_p2(), radius);
#if true
            // 線を分割せずにバッファに格納
            p01.status = RouteStatus.START;     // 開始地点
            rsltbuff.Add(p01);
            rsltbuff.Add(circle0.ps);

#else
            // 線を分割してバッファに格納
            LineSplit(rsltbuff, (new tLine(p01, circle0.ps)), step, RouteStatus.BEGIN);
#endif
            CircleSplit(rsltbuff, circle0, step, RouteStatus.BEGIN);
            rsltbuff[0].status = RouteStatus.START;     // 開始地点
            line = line.Expanded(radius * 2); // 長くする
            line.set_p1(circle0.pe);      // 円の終点を次の線の始点にする。

            //    int num_count = 0;

            int count = 0;
            while (true)
            {
                // 線の移動方向を決める。
                bool move_left = (polygon.ccw() != reverse_flag); // xor
                                                                  // 線の移動ベクトル
                Point2D V = line.ToVector() * (new Point2D(0, move_left ? 1 : -1));  // 左or右へ移動
                                                                                     // 旋回半径分だけ移動するベクトル
                V = V.normalized() * radius;

                // 籾播き直線部が往路か復路か
                totech.RouteStatus route_sts_straight = (reverse_flag ? RouteStatus.STRAIGHT2 : RouteStatus.STRAIGHT);

                // ( 旋回半径分だけシフトした線と)衝突する線を探す
                //        tLine col_line = polygon.Search_CollisionLine( line + V );    // シフトした線
                //        tLine col_line = polygon.Search_CollisionLine( line  );         // シフトしてない
                tLine col_line = polygon.Search_CollisionLine(line + V * 0.1);         // ちょっとだけシフト
                if (col_line.is_zero()) break; // 交点が無ければ終了        
                if (reverse_flag)
                    col_line = col_line.Reversed();

                // 交点
                Point2D xp1 = GeomUtils.cross_point(line, col_line);

                // 角をなす点を得る
                Point2D p1 = line.get_p1();
                Point2D p2 = xp1;
                Point2D p3 = col_line.get_p2();

                // 内接する円を得る
                tCircle circle1 = tCircle.TangentCircle(p1, p2, p3, radius);

                // 接点がp1より逆戻りしていないか？
                bool circle1_is_valid = true;
                if (GeomUtils.dot(xp1, p1, circle1.ps) < 0) // xp1->p1->psの順に並んでいる
                    circle1_is_valid = false;

                // 内接円がポリゴンに内包されるかチェックする。
                if (!src_polygon.is_inner_point(circle1.get_cp()))
                    circle1_is_valid = false;

                // 内接円が有効でない場合、交点まで進んで終了
                if (!circle1_is_valid)
                {
                    // 交点を再計算
                    col_line = polygon.Search_CollisionLine(line);
                    if (col_line.is_zero()) break;
                    xp1 = GeomUtils.cross_point(line, col_line);

                    if (src_polygon.is_inner_point(xp1))
                    {
#if true
                        // 線を分割せずにバッファに格納
                        xp1.status = route_sts_straight;
                        rsltbuff.Add(xp1);
#else

                        // 線を分割してバッファに格納
                        LineSplit(rsltbuff, (new tLine(p1, xp1)), step, route_sts_straight);
#endif
                    }
                    break;
                }


                // 円と直線の接点
                Point2D tp1 = new  Point2D(circle1.ps);

#if true
                // 線を分割せずにバッファに格納
                tp1.status = route_sts_straight;
                rsltbuff.Add(tp1);
#else
                // 線を分割してバッファに格納
                LineSplit(rsltbuff, (new tLine(p1, tp1)), step, route_sts_straight);
#endif


                /*
                 * 以下、似たような処理を行う。
                 * 
                 */

                // 線を平行移動させるベクトル
                //V = V.normalized() * 2 * area_r;    // 移動量
                V = V.normalized() * pitch;         // 移動量


                // 線を移動する
                //        line = tLine( line.get_p1() + V, line.get_p2() + V );
                line += V;
                // 線を伸ばす
                line = line.Expanded(radius * 2);

                // 衝突する線を探す
                col_line = polygon.Search_CollisionLine(line);
                if (col_line.is_zero())
                {
                    // ↓注意：この処理を行うとぎりぎりまで直進して籾播きを行うので
                    //　　　 ：籾播き後 開始点まで帰ってこれないかもしれない。

                    // 交点が無ければ、前に検索した交点まで線を引いて終了
#if true
                    // 線を分割せずにバッファに格納
                    xp1.status = route_sts_straight;
                    rsltbuff.Add(xp1);
#else
                    // 線を分割してバッファに格納
                    LineSplit(rsltbuff, (new tLine(tp1, xp1)), step, route_sts_straight);
#endif
                    break;

                }
                if (reverse_flag)
                    col_line = col_line.Reversed();
                // 交点
                Point2D xp2 = GeomUtils.cross_point(line, col_line);

                // 角をなす点を得る

                Point2D pp1 = xp1;
                Point2D pp2 = xp2;
                Point2D pp3 = line.get_p1();       // まだ線を反転していないのでp1

                // 内接する円を得る
                tCircle circle2 = tCircle.TangentCircle(pp1, pp2, pp3, radius);

                //Point2D tp2 = circle2.pe;

                // 円と円の接線
                tLine tang_line = GeomUtils.TangentLine(circle1, circle2);
                if (!tang_line.is_zero())
                {
                    circle1.pe = tang_line.get_p1();
                    circle2.ps = tang_line.get_p2();
                }
                else
                {
                    circle2.ps = circle1.pe;
                }

                // 右回りor左回り
                totech.RouteStatus sts = (circle1.ccw ? RouteStatus.L_TURN : RouteStatus.R_TURN);


                if (!polygon.is_outer_point(circle1.get_cp()))
                {
                    CircleSplit(rsltbuff, circle1, step, sts);
                    if (!tang_line.is_zero())
                    {
#if true
                        // 線を分割せずにバッファに格納
                        tang_line.get_p2().status = sts;
                        rsltbuff.Add(tang_line.get_p2());
#else
                        // 線を分割してバッファに格納
                        LineSplit(rsltbuff, tang_line, step, sts);
#endif
                    }
                    CircleSplit(rsltbuff, circle2, step, sts);
                }
                else
                {
                    if (tang_line.length() > radius)   // 接線が短い場合はひかない
                    {
                        tang_line.x2 = xp2.x;
                        tang_line.y2 = xp2.y;
                        // circle2がPolygonに含まれない場合、交点までの線を引いて終了

                        CircleSplit(rsltbuff, circle1, step, sts);
#if true
                        // 線を分割せずにバッファに格納
                        tang_line.get_p2().status = sts;
                        rsltbuff.Add(tang_line.get_p2());
#else
                        // 線を分割してバッファに格納
                        LineSplit(rsltbuff, tang_line, step, sts);
#endif
                    }
                    break;
                }

                // 線を反転
                line = new tLine(circle2.pe, line.get_p1());
                // フラグも反転
                reverse_flag = !reverse_flag;

                count++;

            }

            // ルート演算成否
            return (bool)( rsltbuff.Count>1);

        }
    }

}



// end of text