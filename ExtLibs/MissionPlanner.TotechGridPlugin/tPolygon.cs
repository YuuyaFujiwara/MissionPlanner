using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totech;

namespace totech
{
    class tPolygon :  List<Point2D>
    {
        private double m_ccw_rslt;
        private Point2D m_org_longlat= new Point2D();  //原点経度緯度：X=経度:Y=緯度：度表記（1.5＝１度30分）


        // デフォルトコンストラクタ
        public tPolygon()
        {
            m_ccw_rslt = 0;
        }

        // コピーコンストラクタ
        public tPolygon( tPolygon orig)
            :base(orig)
        {
#if false
            //tPolygon(); // デフォルトコンストラクタ

            reserve(orig.size());
            copy(orig.begin(), orig.end(), back_inserter(*this));

            m_org_longlat = orig.m_org_longlat;

            m_ccw_rslt = 0
#else

            m_org_longlat = orig.m_org_longlat;
            m_ccw_rslt = 0;

#endif 
        }

        // 範囲を指定したコピーコンストラクタ
        public  tPolygon( tPolygon orig, int _begin, int _end )
        {
#if false
            tPolygon(); // デフォルトコンストラクタ

            // 引数CHK
            if (_begin > _end) { int tmp = _begin; _begin = _end; _end = tmp; }
            if (_begin < 0) _begin = 0;
            if (_end >= orig.size()) _end = orig.size() - 1;

            // Vectorのコピー実行
            reserve(_end - _begin + 1);
            copy(orig.begin() + _begin, orig.begin() + _end, back_inserter(*this));

            m_org_longlat = orig.m_org_longlat;
#else

            // 引数CHK
            if (_begin > _end) { int tmp = _begin; _begin = _end; _end = tmp; }
            if (_begin < 0) _begin = 0;
            if (_end >= orig.Count) _end = orig.Count - 1;

            // Vectorのコピー実行
            Capacity = (_end - _begin + 1);
            for (int i = _begin; i <= _end; i++)
                Add(orig[i], orig[i].status );

            m_org_longlat = orig.m_org_longlat;
            m_ccw_rslt = 0;
#endif
        }

#if false
        // これらのコンストラクタでは原点緯度経度を維持できないので使用禁止(private))
        private tPolygon(iterator it1, iterator it2) : std::vector<Point2D>(it1, it2) {}
        private tPolygon(int i, Point2D& p ) : std::vector<Point2D>(i, p) {}
#else
        // これらのコンストラクタでは原点緯度経度を維持できないので使用禁止(private))
        private tPolygon(tPolygon.Enumerator it1, tPolygon.Enumerator it2)
        {
        }
        private tPolygon(int i, Point2D p )
        {
        }
#endif

        // Status付きAdd
        public void Add(Point2D tgt, totech.RouteStatus sts )
        {
            tgt.status = sts;
            Add(tgt);
        }


        // 原点緯度経度を設定する
        public void Set_Org_LngLat( Point2D lnglat )
        {
            m_org_longlat = lnglat.Clone();
           // std::cout << "m_org_longlat = (" << m_org_longlat.x << "," << m_org_longlat.y << ")" << std::endl;
        }

        // 原点緯度経度を設定する
        public void Set_Org_LngLat( tPolygon src )
        {
            m_org_longlat = src.m_org_longlat;
        }

        // 原点緯度経度を取り出す
        public Point2D Get_Org_LngLat()
        {
            return m_org_longlat;
        }


#if false
        未完成
        // 原点位置を調整する
        // 形、位置は変えずに最初の点が(0,0)となるように原点緯度経度を調整する。
        public void OrgLatLng_Trim()
        {
            if (this.Count <= 0)
                return;

            Point2D p1 = new Point2D(this[0]);
            for (int i = 0; i < this.Count; i++)
            {
                this[i] = this[i] - p1;
            }

            // 原点緯度経度を調整する
            Calc_GPS.calcDistHubeny

        }
#endif


#region ファイルアクセス
#if false
        // ファイルに保存する
        //
        // 2018.01.12 Point2D::status対応
        //
        bool SaveToFile( String filename)
        {
            // ファイル出力
            std::ofstream _fout(filename );

        // 緯度経度を出力
        _fout << std::fixed << std::setprecision(8) << m_org_longlat.x << "," << m_org_longlat.y << std::endl;    // setprecision･･･小数点桁数指定

        // polygon頂点を出力
        const_iterator itr;
            for(itr = this->begin(); itr != this->end(); ++itr) 
            {
#if true
            // 2018.01.12 Point2D::status対応
            _fout << std::fixed << std::setprecision(3) << itr->x << "," << itr->y << "," << itr->status << std::endl; // setprecision･･･小数点桁数指定
#else
                _fout  << std::fixed << std::setprecision(3) << itr->x << "," << itr->y << std::endl; // setprecision･･･小数点桁数指定
#endif
        }

        }

//
// ファイルから読み出す
//
// 2018.01.12 Point2D::status対応
//
bool LoadFromFile( const char* filename)
{
    bool flg_lndlat = false;     // 緯度経度を取得したか？

    //ファイルの読み込み
    std::ifstream _fin;
    _fin.open(filename);

    if (_fin.fail())
        return false;

    clear();    // 既存データクリア

    //csvファイルを1行ずつ読み込む
    std::string str;
    while (getline(_fin, str))
    {
        // 行頭が'//'又は'#'の場合は読み飛ばし
        if (str[0] == '/' && str[1] == '/') continue;
        if (str[0] == '#') continue;

        // 1行読み出し
        vector < std::string> str_array = misc::split(str, ',');
        // 値取り出し
        double x, y;
        int status = 0;
        bool rslt = false;
        try
        {
            // 2018.01.12 Point2D::status対応
            if (str_array.size() >= 3)
            {
                status = misc::stoi(str_array.this[2));
            }
            if (str_array.size() >= 2)
            {
                x = misc::stod(str_array.this[0));
                y = misc::stod(str_array.this[1));
                rslt = true;
            }
        }
        catch (...) {

        }
        if (!rslt) continue;

        // 読み込んだデータを保持する
        if (!flg_lndlat)
        {
            // 原点経度緯度設定
            m_org_longlat.x = x;
            m_org_longlat.y = y;
            m_org_longlat.status = status;
            flg_lndlat = true;
        }
        else
        {
            // Polygon頂点
            // 2018.01.12 Point2D::status対応
            push_back(Point2D(x, y, status));
        }
        }
    }
#endif
#endregion ファイルアクセス









            // 点を返す
        public Point2D getPoint(int index)
        {
            return this[index_trim(index)];
        }
        // 線を返す
        public tLine getLine(int index)
        {

            Point2D p1 = this[index_trim(index)];
            Point2D p2 = this[index_trim(index + 1)];
            return new tLine(p1, p2);
        }

        // polygonのサイズを考慮してindexを調整する
        public int index_trim(int val)
        {
            if (val == 0) return 0;

            int size = this.Count;

            if (size == 0)
            {
                return 0;
            }

            while (true)
            {
                if (val < 0)
                    val += size;
                else if (val >= size)
                    val -= size;
                else
                    return val;
            }
        }

        // 多角形が右回りか左回りか調べる
        public bool ccw()
        {
            if (m_ccw_rslt == 0)
            {
                for (int j = 0; j < Count; j++)
                {
                    int i = index_trim(j - 1);
                    int k = index_trim(j + 1);

                    m_ccw_rslt += GeomUtils.ccw(this[i], this[j], this[k]);
                }
            }
            return (m_ccw_rslt > 0) ? true : false;    // ccw:1, cw:-1
        }
        public bool ccw_const()
        {
            double ccw_sum = 0;
            for (int j = 0; j < Count; j++)
            {
                int i = index_trim(j - 1);
                int k = index_trim(j + 1);

                ccw_sum += GeomUtils.ccw(this[i], this[j], this[k]);
                }
            return (ccw_sum > 0) ? true : false;    // ccw:1, cw:-1
        }

        // 多角形の面責を計算する
        // 
        public double area()
        {
            // 全頂点をつなぐ面積を返す
            return area(0, Count - 1);
        }

        // 多角形の面責を計算する
        // 開始/終了の頂点の番号を指定
        public double area(int index1, int index2)
        {
            // 面積積算
            Point2D org = new Point2D(0.0, 0.0);
            double area_sum = 0;
            int i = index1;
            while(true)
            {

                int j = i + 1;
                if ((j >= Count)) j = 0;
                if (i == index2) j = index1;

                area_sum += GeomUtils.ccw(this[i], org, this[j]);

                if (i == index2) break; // loop end

                i++;
                if (i >= Count) i = 0;

            }
            area_sum /= 2.0;
            // 絶対値
            area_sum = Math.Abs(area_sum);

            return area_sum;

        }






        // 指定された線を移動する。
        // index    : 対象の線を指定 
        // distance : 移動距離
        // move_left: 左に移動の場合true
        //
        public void LineMove(int index, double distance, bool move_left)
        {
            // 右に移動の場合distanceの符号反転
            distance *= (move_left ? 1 : -1);

            // 各点へのindex
            int i0 = index_trim(index - 1);
            int i1 = index_trim(index + 0);
            int i2 = index_trim(index + 1);
            int i3 = index_trim(index + 2);
            // 各点
            Point2D p0 = this[i0];
            Point2D p1 = this[i1];
            Point2D p2 = this[i2];
            Point2D p3 = this[i3];

            // 線の移動ベクトル
            Point2D V = (new Point2D(p1, p2)) * (new Point2D(0, 1));
            V = V.normalized() * distance;              // 移動量調整

            // 操作対象の線を移動する
            tLine new_line = new tLine(p1 + V, p2 + V);

            // 線を移動したことで前後の線が消える場合がある。
            // 別途処理が必要。

            // 新たな線と、前後の線の交点を求める
            tLine line0 = new tLine(p0, p1 );
            tLine line3 = new tLine(p2, p3 );
            Point2D xp1 = GeomUtils.cross_point(new_line, line0);
            Point2D xp2 = GeomUtils.cross_point(new_line, line3);

            // 配列に戻す
            this[i1] = xp1;
            this[i2] = xp2;

        }


        // 指定された線を移動する。
        // index    : 対象の線を指定 
        // distance : 移動距離
        // move_left: 左に移動の場合true
        //
        // 2018.01.22 見直してLineMove2作成
        //
#if false
        void LineMove2( int index, int distance, bool move_left )
        {
            int target_size = this->size();
            bool this_is_ccw = this->ccw();     // このポリゴンは左回りか？

            // 右に移動の場合distanceの符号反転
            distance *= ( move_left ? 1 : -1 );

        
        
            // xxxxx  Polygonから「左回りの線の配列」を作成する。
            // Polygonから「線の配列」を作成する。
            std::vector<tLine> lines(target_size);
            for( int i1 = 0; i < target_size; i1++ )
            {
                int i2 = index_trim( i1 + 1 );
                lines[i1] = tLine( this[i1), this[i2) );
                i1 = i2;
            }

            // 対象の線を移動する
            tLine& line = lines.this[index);// 線への参照
            Point2D V = line.ToVector() * Point2D( 0, 1);   // 線の移動ベクトル
            V = V.normalized() * distance;                  // 移動量調整
            line += V;        // 線を移動
            if( line.is_zero() ) return;

#if false
            // 線の頂点を調整する
            for( int i = 0; i < target_size; i++ )
            {
                int j = ( i < (target_size-1) ? i + 1 : 0 );
                // 線への参照
                tLine& line1 = lines.this[i];
                tLine& line2 = lines.this[j];
                // 交点を求める
                Point2D xp = GeomUtils.cross_point( line1, line2 );
                // 先の頂点を調整
                line1.set_p2( xp );
                line2.set_p1( xp );
            }
#endif

            // 線同士の交点を求める。
            // 縮小後に使用されなくなる線はクリアする。
            bool changed = false;
            while(true)
            {
    SEARCH_START:
                changed = false;
            
                target_size = lines.size();
                for( int i = target_size - 1; i >=0; i-- )
                {
                    if( i == index ) continue;  //同じ線
                    tLine& line2 = lines.this[i];
                    if( line2.is_zero() ) continue;
                
                    // 線分が交差するか調べる
                    bool cross_flag = GeomUtils.line_seg_cross_check( line, line2 );
                    if( cross_flag )
                    {
                        // 交点
                        Point2D xp = GeomUtils.cross_point( line, line2 );
                        // 交点のなす角
                        double angl_ccw = GeomUtils.ccw( line.ToVector(), line2.ToVector() )>0 ? true : false;
                        if( angl_ccw == this->ccw() )
                        {
                            // 新たな交点をセット
                            line.set_p2(xp);
                            line2.set_p1(xp);

                            // 不要な線を削除
                            lines.erase( lines.begin()+index+1, lines.begin()+i );                        
                        
                        
                        
                        }
                    
                        double ccw = GeomUtils.ccw( line1.get_p1(), xp, line2.get_p2() );
                        if( ccw > 0 )
                        {   // 交点が左回り

                            // 新たな交点をセット
                            line1.set_p2(xp);
                            line2.set_p1(xp);

                            // 不要な線を削除
                            lines.erase( lines.begin()+i+1, lines.begin()+j );
                        }
                        else
                        {   // 交点が右回り

                            // 新たな交点をセット
                            line1.set_p1(xp);
                            line2.set_p2(xp);
                            // 不要な線を削除
                            if( j < target_size-1 ) lines.erase( lines.begin()+j+1, lines.end() );
                            if( i > 0             ) lines.erase( lines.begin()    , lines.begin()+i );
                        }
                        goto SEARCH_START; // 最初からやり直し
                    }
                        
                        
                        
                        
                        
                        
                        
                        }
                    }// for(j)
                }// for(i)
            
                // スキャン完了したらwhileを抜ける
                break;
            }// while
                      
            // 新しいPolygonを構築する。
            tPolygon new_polygon;
            for( int i = 0; i < target_size; i++ )
            {
                if( !lines[i].is_zero() )
                {
                    new_polygon.push_back( lines[i].get_p1() );
                }
            }
            // 回転方向を合わせる
            if( !this->ccw_const() )
            {
                new_polygon.RotateForward();
                new_polygon.Reverse();
            }
        
            //緯度経度を合わせる
            new_polygon.m_org_longlat = m_org_longlat;

            return new_polygon;
        
        }

    
#elif false
        void LineMove2( int index, int distance, bool move_left )
        {
            int retnum = index;
        
        
            // 右に移動の場合distanceの符号反転
            distance *= ( move_left ? 1 : -1 );
        
            // 各点へのindex
            int index0 = index_trim( index - 1 );
            int index1 = index_trim( index + 0 );
            int index2 = index_trim( index + 1 );
            int index3 = index_trim( index + 2 );
            // 各点
            Point2D p0 = this[index0);
            Point2D p1 = this[index1);
            Point2D p2 = this[index2);
            Point2D p3 = this[index3);
        
            // 線の移動ベクトル
            Point2D V = Point2D( p1, p2 ) * Point2D( 0, 1);
            V = V.normalized() * distance;              // 移動量調整
        
            // 操作対象の線を移動した別の線を作る）
            tLine new_line = tLine( p1 + V, p2 + V  );        
        
            // 移動した線が元のPolygonと交わるか？
            // 交わらない場合、Polygon消滅を示す-1を返す。（Polygonに変更は加えない）
            tLine xline = Search_CollisionLine(new_line);
            if( xline.is_zero() ) return -1;    // Polygon消滅
        
     

            // 線を移動したことで前後の線が消える場合がある。
            // 隣の線以外と交差する線をサーチ（後方向）
            bool flg1 = false;
            int i1 = index3;
            tLine xLine1;
            while(true)
            {
                int i1next = index_trim( i1 + 1 );
                if( i1 == index0 ) break;   // 見つからない
                xLine1 = tLine( this[i1), this[i1next) );
                flg1 =  GeomUtils.line_cross_check( new_line, xLine1 );
                if( flg1 ) 
                {
                    // 見つかった
                
                    break;   // 見つかった
                }
                i1 = i1next;        // 次の点
            }
            // 隣の線以外と交差する線をサーチ（前方向）
            bool flg2 = false;
            int i2 = index0;
            tLine xLine2;
            while(true)
            {
                int i2next = index_trim( i2 - 1 );
                if( i2next == index3 )      break;   // 見つからない
                if( flg1&&(i2next == i1) )  break;   // 見つからない
                xLine2 = tLine( this[i2), this[i2next) );
                flg2 =  GeomUtils.line_cross_check( new_line, xLine2 );
                if( flg2 ) break;   // 見つかった
                i2 = i2next;        // 次の点
            }
            // 隣以外の線と交差するなら
            if( flg1)
            {
                // 交差する線を得る
            
                // 線の後ろ、index3～i1まで削除
                Remove_Points( index3, i1 );
            }
            if( flg2 )
            {
                // 線の前、i2～index0までを削除
                int delnum = Remove_Points( i2, index0 );
            
                retnum = index - delnum;    // 対象の線のIndexずれる
            
            }
        
            Point2D xp2 = new_line.get_p2();
        
        
            // 新たな線と、前後の線の交点を求める
            tLine line0( p0, p1 );
            tLine line3( p2, p3 );
            Point2D xp1 = GeomUtils.cross_point( new_line, line0  );
            Point2D xp2 = GeomUtils.cross_point( new_line, line3  );        
            // Polugon（点の配列）に反映する
            this[i1) = xp1;
            this[i2) = xp2;   
        
        
        
        
                int Remove_Points( int index1, int index2 );


                        if(  (i1!=j2) && GeomUtils.line_cross_check( l1, l2 ) && GeomUtils.line_cross_check( l2, l1 ) )

        
        
        

        


        }    
#endif
        //
        // 衝突する線を探す。
        public tLine Search_CollisionLine(  tLine src_line )
        {
            for (int i = 0; i < Count; i++)
            {
                tLine target_line = getLine(i);

                // 同じ線で無いこと
                if (target_line != src_line)
                {
                    // 交差すること
                    if (GeomUtils.line_cross_check(src_line, target_line))
                    {
                        // 交点のなす角がPolygonの回転方向と一致すること
                        bool angl_ccw = GeomUtils.cross(src_line, target_line) > 0;
                        if (angl_ccw == this.ccw_const())
                            return target_line; // 衝突する線
                    }
                }
            }

            // 見つからない。空の線を返す
            return new tLine();
        }

        // 形状を維持したまま、点の並びを進める。
        // 今まで2番目の点p1だったところが開始点p0になり、今までのp0が終点になる。
        public void RotateForward()
        {
            // 始点を取り出す
            Point2D p0 = this[0].Clone();
            // 始点を削除
            this.RemoveAt(0);
            // 末尾に加える
            this.Add(p0);
        }


        // 形状を維持したまま、点の並びを進める。
        // 今まで2番目の点p1だったところが開始点p0になり、今までのp0が終点になる。
        public void RotateForward(int num)
        {
            if (num == 0) return;

            // コピー作成
            tPolygon tmp = new tPolygon(this);

            for (int i = 0; i < Count; i++)
            {
                this[i] = tmp[index_trim(i + num)];
            }
        }


            // 形状を維持したまま、点の並びを逆転する。
            // 今までの始点が終点になり、終点が始点になる。
            // ※Reverseでは始点の位置が変わるので注意。
            //   始点を変えずに方向のみ逆転するには、
            //      RotateForward()した後にReverse()を行う。
            //
            // ※C#ではList<Point2D>.Reverse()をそのまま使う。
#if false
            void Reverse()
        {
            std::reverse(this->begin(), this->end());
        }
#endif
            // 点がポリゴンに内包されているか調べる
            public bool is_inner_point( Point2D p )
            {
                // ポリゴンを含む矩形に含まれるかチェック
                Point2D minp = new Point2D();    // 戻り値用バッファ
                Point2D maxp = new Point2D();    // 戻り値用バッファ
                MinMax(minp, maxp);
                if (minp.x > p.x || p.x > maxp.x
                 || minp.y > p.y || p.y > maxp.y)
                    return false;

            // 対象の点から始まり、ポリゴンを突き抜ける線（X軸に水平）
            tLine l = new tLine(p.x, p.y, maxp.x+100, p.y );

            // 線がポリゴンの辺と交わる回数を調べる。
            // 内部の点から始めたなら奇数回交わる。
            bool flag_inner = false;
            for (int i = 0; i < this.Count; i++)
            {
                tLine tmpl = this.getLine(i);
                // 始点が点と同じならチェックしない（前の線の終点でチェックされる）
                if (tmpl.y1 == p.y) continue;

                // 2つの線分が交わるかチェック
                // （line_cross_check関数は直線と線分なので2回チェック）
                if (GeomUtils.line_cross_check(l, tmpl))
                {
                    if (GeomUtils.line_cross_check(tmpl, l))
                        flag_inner = !flag_inner;   // 反転
                }
            }

            return flag_inner;
        }

        // 点がポリゴンの外側か調べる
        public bool is_outer_point( Point2D p )
        {
            // 内包されていたらfalse
            if (is_inner_point(p)) return false;

            // 線の上にあるか
            for (int i = 0; i < this.Count; i++)
            {
                tLine line = this.getLine(i);
                if (line.is_on_line(p))
                    return false;
            }
            return true;
        }


        // XとYそれぞれの最大を得る
        public Point2D MaxPoint()
        {
            Point2D  maxp = new Point2D();
            MinMax(null, maxp);
            return maxp;
        }
        // XとYそれぞれの最小を得る
        public Point2D MinPoint()
        {
            Point2D minp = new Point2D();
            MinMax(minp, null);
            return minp;
        }

        // XとYそれぞれの最大最小値を得る
        // Polygonを囲む矩形の対角線の頂点
        public void MinMax(Point2D min_point, Point2D max_point)
        {
            if (this.Count == 0) return;

            Point2D p = this[0];
            double minx = p.x;
            double maxx = p.x;
            double miny = p.y;
            double maxy = p.y;

            for (int i = 1; i < this.Count; i++)
            {
                p = this[i];
                if (minx > p.x) minx = p.x;
                if (maxx < p.x) maxx = p.x;
                if (miny > p.y) miny = p.y;
                if (maxy < p.y) maxy = p.y;
            }
            if( min_point != null )
            {
                min_point.x = minx;
                min_point.y = miny;

            }
            if( max_point != null )
            {
                max_point.x = maxx;
                max_point.y = maxy;

            }
        }


        // 小さくしたポリゴンを返す
        // distance : 小さくする距離(マイナス値だと大きくなる)
        // 各辺をdistance分だけ内側へ移動する。
        // 結果、角が減った場合の処理が完全ではない（２つ以上の角が削除される場合）
        //
        // 2017.05.26 見直し
        // 2018.01.23 LineMove2を参考に見直したほうが良いかも
        //
        public tPolygon ReducedPolygon(double distance)
        {
            int target_size = this.Count;

            // Polygonから「左回りの線の配列」を作成する。
            List<tLine> lines = new List<tLine>(target_size);
            int incl = (this.ccw_const()) ? 1 : -1;
            int i1 = 0;
            for (int i = 0; i < target_size; i++)
            {
                int i2 = index_trim(i1 + incl);
              //lines[i] = new tLine(this[i1], this[i2]);
                lines.Add( new tLine(this[i1], this[i2]));
                i1 = i2;
            }

            // すべての線を内側(左側)に移動する。
            for (int i = 0; i < target_size; i++)
            {
                // 線への参照
                tLine line = lines[i];
                // 線の移動ベクトル
                Point2D V = line.ToVector() * (new Point2D(0, 1));
                V = V.normalized() * distance;              // 移動量調整
                // 線を移動
                lines[i] = line + V;
            }

            // 線の頂点を調整する
            for (int i = 0; i < target_size; i++)
            {
                int j = (i < (target_size - 1) ? i + 1 : 0);
                // 線への参照
                tLine line1 = lines[i];
                tLine line2 = lines[j];
                // 交点を求める
                Point2D xp = GeomUtils.cross_point(line1, line2);
                // 先の頂点を調整
                line1.set_p2(xp);
                line2.set_p1(xp);
            }


            // 線同士の交点を求める。
            // 縮小後に使用されなくなる線はクリアする。
            bool changed = false;
            while (true)
            {
                SEARCH_START:
                changed = false;

                target_size = lines.Count;
                for (int i = 0; i < target_size; i++)
                {
                    tLine line1 = lines[i];
                    if (line1.is_zero()) continue;

                    //    bool tgt_line_effectiv_flag = false;    // tgt_lineが有用か

                    for (int j = i + 2; j < target_size; j++)
                    {
                        // 最初の線と最後の線の交差はチェックしない
                        // （元々角を作る線なので）
                        if (! ((i == 0) && (j == (target_size - 1))))
                        {
                            tLine  line2 = lines[j];

                            // 線が交差するか調べる
                            bool cross_flag = true;
                            if (!GeomUtils.line_cross_check(line1, line2)) cross_flag = false;
                            if (!GeomUtils.line_cross_check(line2, line1)) cross_flag = false;
                            if (cross_flag)
                            {
                                // 交点
                                Point2D xp = GeomUtils.cross_point(line1, line2);
                                // 交点のなす角
#if false
                                double ccw = GeomUtils.ccw( line1.get_p1(), xp, line2.get_p2() );
#else
                                double ccw = GeomUtils.ccw(line1.ToVector(), line2.ToVector());
#endif
                                if (ccw > 0)
                                {   // 交点が左回り

                                    // 新たな交点をセット
                                    line1.set_p2(xp);
                                    line2.set_p1(xp);

                                    // 不要な線を削除
                                    //lines.erase(lines.begin() + i + 1, lines.begin() + j);
                                    lines.RemoveRange( i+1, j - i - 1 );    // vector.eraseとList.RemoveRangeで指定方法違うので注意
                                }
                                else
                                {   // 交点が右回り

                                    // 新たな交点をセット
                                    line1.set_p1(xp);
                                    line2.set_p2(xp);
#if false
                                    // 不要な線を削除
                                    // C++動作確認盤
                                    if (j < target_size - 1) lines.erase(lines.begin() + j + 1, lines.end());
                                    if (i > 0) lines.erase(lines.begin(), lines.begin() + i);
#else
                                        // 不要な線を削除
                                        // C#に移植
                                        if (j < target_size - 1) lines.RemoveRange( j + 1, lines.Count - j - 1);
                                        if (i > 0) lines.RemoveRange(0,  i);
#endif
                                    }
                                    goto SEARCH_START; // 最初からやり直し
                            }
                        }
                    }// for(j)
                }// for(i)

                // スキャン完了したらwhileを抜ける
                break;
            }// while

            // 新しいPolygonを構築する。
            tPolygon new_polygon = new tPolygon();
            for (int i = 0; i < target_size; i++)
            {
                if (!lines[i].is_zero())
                {
                    new_polygon.Add(lines[i].get_p1());
                }
            }
            // 回転方向を合わせる
            if (!this.ccw_const())
            {
                new_polygon.RotateForward();
                new_polygon.Reverse();
            }

            //緯度経度を合わせる
            new_polygon.m_org_longlat = m_org_longlat;

            return new_polygon;

        }


        //
        // ポリゴンから交差を除去する。
        //　（交差で分割して小さいエリアを除去する）
        //
        //
        public void NoCross_Polygon()
        {
            while (true)
            {
                SEARCH_START:
                // 交差する点を探す
                for (int i1 = 0; i1 < Count - 2; i1++)
                {
                    for (int j1 = i1 + 2; j1 < Count - 1; j1++)
                    {
                        int i2 = i1 + 1; //if( i2 >= Count ) i2 = 0;
                        int j2 = j1 + 1; //if( j2 >= Count ) j2 = 0;

                        //       std::cout << "size()=" << Count << ", i1=" << i1 << ", i2=" << i2 << ", j1=" << j1 << ", j2=" << j2 << std::endl;


                        // 交差チェックする線
                        tLine l1 = new tLine(this[i1], this[i2] );
                        tLine l2 = new tLine(this[j1], this[j2] );

                        // 交差チェック
                        if ((i1 != j2) && GeomUtils.line_cross_check(l1, l2) && GeomUtils.line_cross_check(l2, l1))
                        {
                            // 交点でPolygonを分け、面積比較
                            // ※面積計算する際に交点が考慮されていない。2018.01.23 
                            double area_i2j1 = area(i2, j1);
                            double area_j2i1 = area(j2, i1);
#if false // for debug
                            std::cout << "size()=" << Count << std::endl;
                            std::cout << "cross!" << std::endl;
                            std::cout << "   i1=" << i1 << ":( " << (this[i1).x) << ", " << (this[i1).y) << " )" << std::endl;
                            std::cout << "   i2=" << i2 << ":( " << (this[i2).x) << ", " << this[i2).y << " )" << std::endl;
                            std::cout << "   j1=" << j1 << ":( " << this[j1).x << ", " << this[j1).y << " )" << std::endl;
                            std::cout << "   j2=" << j2 << ":( " << this[j2).x << ", " << this[j2).y << " )" << std::endl;
                            std::cout << "  areai2j1 = " << area_i2j1 << std::endl;
                            std::cout << "  areaj2i1 = " << area_j2i1 << std::endl;
                            std::cout << std::endl << std::endl;
#endif

                            // 小さいエリアを削除する
                            if (area_i2j1 < area_j2i1)
                            {
                                // i2～j1を削除
                                //erase(begin() + i2, begin() + j1);    // C++
                                RemoveRange( i2, j1 - i2 );      // C#
                            }
                                else
                            {
#if false
                                // 0～i1, j2～size()を削除
                                // C++
                                erase(begin() + j2, end());
                                erase(begin(), begin() + i1);
#else
                                // 0～i1, j2～size()を削除
                                // C#
                                RemoveRange( j2, Count - j2 );
                                RemoveRange( 0 ,  i1);
#endif
                            }

                            goto SEARCH_START; // 交差点検索やり直し
                        }

                    }


                }

                // スキャン完了したらwhileを抜ける
                break;

            }

            return;
        }


        ///
        /// ポリゴンから指定されたindex間の点を削除する
        ///　Index1からIndex2まで削除する。（Index1とIndex2も削除する。）
        /// Index1 ＞ Index2の場合、Index1～end, begin～Index2を削除する。
        /// Index1 == Index2の場合、1ポイントのみ削除する
        /// 
        /// \param index1　削除開始ポイントIndex
        /// \param index2  削除終了ポイントIndex
        /// \return 削除したポイント数
        public int Remove_Points(int index1, int index2)
        {
            int retnum = -1;

            // Indexのチェック、調整
            index1 = index_trim(index1);
            index2 = index_trim(index2);

            if (index1 > index2)
            {
#if false
                // idnex1～size(), 0～index2を削除
                // C++
                erase(begin() + index1, end());
                erase(begin(), begin() + index2 + 1);
#else
                // idnex1～size(), 0～index2を削除
                // C#
                RemoveRange( index1, Count - 1 );
                RemoveRange( 0,   index2 + 1);
#endif

                  retnum = Count - index1 + index2 + 1;  // 削除したポイント数

            }
            else
            {
                // erase(begin() + index1, begin() + index2 + 1);   //  C++
                RemoveRange( index1,  index2 + 1);      // C#
                retnum = index2 - index1 + 1;  // 削除したポイント数
            }

            return retnum;
        }




    }
}
