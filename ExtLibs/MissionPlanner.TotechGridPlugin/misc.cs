using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totech
{
    class Misc
    {
        public static string GetFullPath( string filename)
        {
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string str2;
            string str4;

            try
            {
                str2 = System.IO.Path.GetDirectoryName(appPath);
                Uri u1 = new Uri(appPath);
                Uri u2 = new Uri(u1, filename);

                str4 = u2.LocalPath;
            }
            finally
            {
                // MessageBox.Show(filename & vbCrLf & vbCrLf & appPath & vbCrLf & vbCrLf & str4)

            }
            return str4;
        }



        /// <summary>
        /// ファイル名に必要なフォルダを作成する
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateDirectory_of_Filename(string filename )
        {
            string pathname = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(filename));
            if (!System.IO.Directory.Exists(pathname))
            {
                string grandparents_path = System.IO.Path.GetDirectoryName(pathname);
                if (!System.IO.Directory.Exists(grandparents_path))
                    CreateDirectory_of_Filename(pathname);  // 再帰

                System.IO.Directory.CreateDirectory(pathname);

            }
        }

    }







}
