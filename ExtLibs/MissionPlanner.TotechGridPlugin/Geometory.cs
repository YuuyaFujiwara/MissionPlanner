using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech
{
    /*
     * ２次元ベクトルクラス
     * 
     * 2018.01.12 int statusメンバを追加。 tPolygonにて籾播きルート演算結果を管理する際、
     * 　　　　　　　　　　　　　　　　　　　籾播き状態の管理用に使用する。
     */
    class Point2D
    {
       public double x;
        public double y;

        //public int status;     // 汎用で使用できる。Point2D内では演算に影響を与えない。
        public totech.RouteStatus status;     // 汎用で使用できる。Point2D内では演算に影響を与えない。


        public Point2D()
        {
            x = 0;
            y = 0;
            status = 0;
        }


        public Point2D( Point2D that )
        {
            x = that.x;
            y = that.y;
            status = that.status;
        }


        public Point2D( double ax, double ay, totech.RouteStatus astatus = totech.RouteStatus.UNDEFINED)
        {
            x = ax;
            y = ay;
            status = astatus;
        }



        // p1からp2へ向かうベクトル
        public Point2D( Point2D p1,  Point2D p2 )
        {
            x = p2.x - p1.x;
            y = p2.y - p1.y;
            status = 0;     // statusは破棄される
        
        }

        // 極座標形式でベクトルを作る
        public static Point2D fromPolar(double _size, double _rad)
        {
            Point2D w = new Point2D();
            w.x = Math.Cos(_rad) * _size;
            w.y = Math.Sin(_rad) * _size;
            w.status = 0;   // statusは破棄される
            return w;
        }

        /* 
         * ※注意！C#ではコピーコンストラクタは作れない。
         * ※ ＝を使用すると参照の代入になるので注意！
        public static Point2D operator=( Point2D v)
        {		//代入演算子の定義
            this->x=v.x;
            this->y=v.y;
            this->status=v.status;
            return *this;
        }
        */

        public Point2D Clone( )
        {
            return new Point2D(this);
        }


#if true
        public static Point2D operator +(Point2D v)  { return new Point2D(v); }                   //+の定義:	+v
        public static Point2D operator -(Point2D v)  { return new Point2D(-1 * v.x, -1 * v.y, v.status); }   //-の定義:	-v
#else
        // コピーしない
        // 引数を変化させるからダメ

        //+の定義:	+v
        public static Point2D operator +(Point2D v)
        {
            return v;
        }
        //-の定義:	-v
        public static Point2D operator -(Point2D v)
        {
            v.x = -1 * v.x;
            v.y = -1 * v.y;
            return v;
            //return new Point2D(-1 * v.x, -1 * v.y, v.status);
        }
#endif
        /*
         * C#では +=, -=は実装不要。+,-が使用される。
        Point2D& Point2D::operator+=(const Point2D& v)                        //+=の定義	v+=
        {
            this->x += v.x;
            this->y += v.y;
            return *this;
        }
        Point2D& Point2D::operator-=(const Point2D& v)                 //-=の定義	v-=
        {
            this->x -= v.x;
            this->y -= v.y;
            return *this;
        }
        */
#if true
        // 注意：+=演算子が使用可能だが、使用しないこと。
        // 左項を変化させるのではなくコピーが作成される。


        public static Point2D operator +(Point2D v, Point2D w)      //vector+vector
        {
            return new Point2D(v.x + w.x, v.y + w.y, v.status);
        }
        public static Point2D operator -(Point2D v, Point2D w)      //vector-vector
        {
            return new Point2D(v.x - w.x, v.y - w.y, v.status);
        }
#else
        // コピーしない
        // 引数を変化させるからダメ
        public static Point2D operator +(Point2D v, Point2D w)      //vector+vector
        {
            v.x = v.x + w.x;
            v.y = v.y + w.y;
            return v;
        }
        public static Point2D operator -(Point2D v, Point2D w)      //vector-vector
        {
            v.x = v.x - w.x;
            v.y = v.y - w.y;
            return v;
        }
#endif

        //内積 vector*vector
        public double dot( Point2D v)
        {
            return this.x * v.x + this.y * v.y;
        }

        // ベクトル × ベクトル → ベクトル
        // 注意：内積でも外積でもない。
        // ベクトルを複素数として積算している。
        public static Point2D operator *( Point2D v, Point2D w) 
        {
            double _x = (v.x * w.x) - ( v.y * w.y);
            double _y = (v.y * w.x) + ( v.x * w.y);
            return new Point2D(_x, _y, v.status );
        }


        /*
         * operator *=は実装不要。 operator *が使用される。
        public static Point2D operator *=(const Point2D& v)                 //*=の定義	v *=
        {
            this->x = (this->x * v.x) - (this->y * v.y);
            this->y = (this->y * v.x) + (this->x * v.y);
            return *this;
        }
        */

        public static Point2D operator *( Point2D v, double k) 	//vector*scalar
        {
            return new Point2D(v.x * k, v.y * k, v.status);
        }
        /*
         * C#ではoperator *=の実装は不要。
        Point2D Point2D::operator*=(double k)   //vector*scalar
            {
            this->x *= k;
            this->y *= k;
            return *this;
        }
        */

        public static Point2D operator /( Point2D v, double k) 	//vector/scalar
        {
            return new Point2D(v.x / k, v.y / k, v.status);
        }

        public static bool operator ==( Point2D v, Point2D w )
        {
            if ((object)v == null && (object)w == null)
                return true;


            if ((object)v == null || (object)w == null)
                return false;

            //※ statusの比較は行わない
            return (bool) ((Math.Abs(v.x-w.x)<1e-10) && (Math.Abs(v.y-w.y)<1e-10) );
        }

        public static bool operator !=( Point2D v, Point2D w )
        {
            return !( v == w );
        }

        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this == (Point2D)obj;

        }



        // ベクトルのサイズ
        public double norm() 
        {
            return Math.Sqrt(x* x + y* y );
        }

        // 正規化（サイズ＝１）したベクトルを返す
        public Point2D normalized()
        {
            if (is_zero()) return new Point2D(0, 0, status);

            double sz = norm();
            return new Point2D(x / sz, y / sz, status);
        }


        // サイズがゼロか
        public bool is_zero()
        {
            return (bool) (Math.Abs(x)<1e-10 && Math.Abs(y)<1e-10);
        }

        //点と点の距離
        public static double distance( Point2D p1,  Point2D p2 )
        {
            return Math.Sqrt((p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y));
        }

        // p1からp2の角度
        public static double angle( Point2D p1,  Point2D p2 )
        {
            return Math.Atan2(p2.y - p1.y, p2.x - p1.x);
        }









    //２つのベクトルABのなす角度θを求める
    public static double AngleOf2Vector( Point2D A, Point2D B )
        {
            //　※ベクトルの長さが0だと答えが出ませんので注意してください。

            //ベクトルAとBの長さを計算する
            double length_A = A.norm();
            double length_B = B.norm();

            //内積とベクトル長さを使ってcosθを求める
            double cos_sita = A.dot(B) / (length_A * length_B);

            //cosθからθを求める
            double sita = Math.Acos(cos_sita);

            return sita;

        }       
    }    // Point2D


    /*
        * 円を扱うクラス
        */
    class tCircle
    {
        public double x;
        public double y;
        public double r;
        public bool ccw;
        public Point2D ps = new Point2D();  // 始点
        public Point2D pe = new Point2D();  // 終点



        public tCircle(double _cx, double _cy, double _r)
        {
            x = _cx;
            y = _cy;
            r = _r;
            ccw = true;
        }


        public tCircle()
            : this(0, 0, 0)
        {
        }

        public tCircle( Point2D _cp, double _r ) 
            : this(_cp.x, _cp.y, _r)
        {
        }

        public tCircle(tCircle src)
            : this(src.x, src.y, src.r)
        {
            ccw = src.ccw;
            ps = src.ps.Clone();
            pe = src.pe.Clone();
        }

        //中心点を返す
        public Point2D get_cp()
        {
            return new Point2D(x, y);
        }

        // 与えられた3点（p1,p2,p3）がなす角に内接する円を作成する。
        // （角 p2は通らない）
        // psとpeにはそれぞれp1, p3からの直線の接点が設定される。
        public static tCircle TangentCircle(Point2D p1, Point2D p2, Point2D p3, double radius)
        {

            // p1, p3が近すぎる場合、接点より遠くする。
            // ※p2から半径分離せば接点より遠くなる。
            Point2D v1 = new Point2D(p2, p1 );
            Point2D v3 = new Point2D(p2, p3 );
            if(v1.norm() < radius ) p1 = p2 + v1.normalized() * radius;
            if(v3.norm() < radius ) p3 = p2 + v3.normalized() * radius;

                // 内接円の中心
                Point2D cp = GeomUtils.inner_circle_point(p1, p2, p3, radius);       
                if(cp.is_zero() ) return new tCircle();
                // 円を作成する
                tCircle circle = new tCircle(cp, radius );
                //回転方向
                circle.ccw = (bool) (GeomUtils.ccw(p1, p2, p3) > 0 );
    
            // p1との接線との接点
            circle.ps = GeomUtils.TangentPoint(circle, p1, circle.ccw? 1 : 0);
            // p2との接線との接点
            circle.pe = GeomUtils.TangentPoint(circle, p3, circle.ccw? 0 : 1);

 
    
            return circle;
        }




        // 与えられた3点（p1,p2,p3）に内接する円を作成する。
        // （角は通らない）
        // psとpeにはそれぞれp2からの直線の接点が設定される。
        // 参考：http://prognote.web.fc2.com/win_c_cpp/tri_incircle/index.html
        public static tCircle TangentCircle(Point2D p1, Point2D p2, Point2D p3)
        {
            // 頂点に対向する辺のベクトル
            Point2D va = p2 - p3;
            Point2D vb = p3 - p1;
            Point2D vc = p1 - p2;

            // ベクトルのサイズ
            double a = va.norm();
            double b = vb.norm();
            double c = vc.norm();

            // 中心点
            double p = a + b + c;
            Point2D cp = (p1 * a + p2 * b + p3 * c) / p;

            // 半径
            double s = p / 2;
            double radius = Math.Sqrt((s - a) * (s - b) * (s - c) / s);

            // 円を作成する
            tCircle circle = new tCircle(cp, radius );
            //回転方向
            circle.ccw = (bool)(GeomUtils.ccw(p1, p2, p3) > 0);

            // p1->p2との接線との接点
            // デバッグ不十分
            circle.ps = GeomUtils.TangentPoint(circle, p2, circle.ccw ? 0 : 1);
            // p2->p3との接線との接点
            // デバッグ不十分
            circle.pe = GeomUtils.TangentPoint(circle, p2, circle.ccw ? 1 : 0);

            return circle;
        }

    }


    /*
     * 直線(線分)を扱うクラス
     */
    class tLine
    {

        public double x1;
        public double y1;
        public double x2;
        public double y2;

        public tLine(double _x1, double _y1, double _x2, double _y2)
        {
            x1 = _x1;
            y1 = _y1;
            x2 = _x2;
            y2 = _y2;
        }
        public tLine()
            : this(0,0,0,0)
        {
        }
        public tLine(Point2D p1, Point2D p2)
            : this(p1.x, p1.y, p2.x, p2.y)
        {
        }

        public tLine(tLine src)
            : this(src.x1, src.y1, src.x2, src.y2)
        {
        }

        public Point2D get_p1()
        {
            return new Point2D(x1, y1);
        }

        public Point2D get_p2() {
            return new Point2D(x2, y2);
        }

        public void set_p1( Point2D src )
        {
            x1 = src.x;
            y1 = src.y;
        }
        public void set_p2( Point2D src )
        {
            x2 = src.x;
            y2 = src.y;
        }





        public static bool operator ==(tLine v1, tLine v2 ) 
        {
          
            Point2D p1= v1.get_p1();
            Point2D p2 = v1.get_p2();
            Point2D p3 = v2.get_p1();
            Point2D p4 = v2.get_p2();
            
            return((p1==p3)&&(p2==p4) 
                 || (p1==p4)&&(p2==p3)); 
        }

        public static bool operator !=(tLine v1, tLine v2)
        {
            return !( v1 == v2 );
        }

        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this == (tLine)obj;

        }



        public static tLine operator +(tLine v1, Point2D v2)    //tLine+vector
        {
            return new tLine(v1.x1 + v2.x, v1.y1 + v2.y, v1.x2 + v2.x, v1.y2 + v2.y);

        }



        public Point2D ToVector()
        {
            return new Point2D(x2 - x1, y2 - y1);
        }


        // 始点と終点を入れ替える
        public tLine Reversed()
        {
            return new tLine(this.get_p2(), this.get_p1() );
        }


        // 線を伸ばす
        // 始点終点をそれぞれ伸ばす方向にdだけ移動する。
        public tLine Expanded(double d) 
        {
            Point2D v = this.ToVector().normalized() * d;
            return new tLine(this.get_p1() - v, this.get_p2() + v  );
        }

        public double length()
        {
            return Math.Sqrt((x1-x2)* (x1-x2)+(y1-y2)* (y1-y2));
        }
        
            //
        public   bool is_zero()
        {
            return get_p1().is_zero() && get_p2().is_zero();
        }

        // 点が線の左側にあるか調べる
        //
        public bool is_on_line(  Point2D p ) 
        {
            Point2D v1 = new Point2D(p, get_p1() );
            Point2D v2 = new Point2D(p, get_p2() );
            double cross = Math.Abs(GeomUtils.cross(v1, v2));
            if(cross<1e-10   )  // 直線上か？
            {
                if(v1.dot(v2) < 0  )       // v1とv2の角度は90度以上か？
                    return true;
            }
            return false;
        }   


        // 接続されている線か調べる
        public bool is_connected_line(  tLine line )
        {
            Point2D p11 = this.get_p1();
            Point2D p12 = this.get_p2();
            Point2D p21 = line.get_p1();
            Point2D p22 = line.get_p2();
                return (p11==p21) || (p11==p22) || (p12==p21) || (p12==p22);
        }



        // 指定の点が直線の左側にあるか調べる
        public bool is_left_point( Point2D tgt )
        {
            // p1 -> p2 -> tgtの経路が反時計回り（ccw>0)ならtgtは左側
            return (GeomUtils.ccw(get_p1(), get_p2(), tgt ) > 1e-10) ? true : false;
        }



    }
    


}
