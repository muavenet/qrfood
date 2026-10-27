using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace HemenTanitim.Helpers
{
    public class Capture_Video
    {
        /// <summary>
        /// created by ~kad1r
        /// send files without mappath
        /// like "/video/videolink1.avi"
        /// function puts mappath itself
        /// download ffmpeg.exe - http://www.speedyshare.com/mRbUN/ffmpeg.rar
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string generateThumb(string file, string klasor_adi)
        {
            string thumb = "";

            try
            {
                FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(file));
                string filename = Path.GetFileNameWithoutExtension(fi.Name);
                Random random = new Random();
                int rand = random.Next(1, 9999999);
                string newfilename = "/Content/uploads/Videolar/" + klasor_adi + "/" + filename + "___(" + rand.ToString() + ").jpg";


                var processInfo = new ProcessStartInfo();
                processInfo.FileName = "\"" + HttpContext.Current.Server.MapPath("/Content/uploads/Videolar/ffmpeg.exe") + "\"";
                processInfo.Arguments = string.Format("-ss {0} -i {1} -f image2 -vframes 1 -y {2}", 5, "\"" + HttpContext.Current.Server.MapPath(file) + "\"",
                                        "\"" + HttpContext.Current.Server.MapPath(newfilename) + "\"");
                processInfo.CreateNoWindow = false;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardOutput = false;


                using (var process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                    process.WaitForExit();
                    thumb = newfilename;
                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return thumb;
        }
    }
}