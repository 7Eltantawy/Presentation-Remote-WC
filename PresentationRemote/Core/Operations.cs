using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationRemote.Core
{
    public static class Operations
    {
        public static string GetPassword(this string txt)
        {
            try
            {
                return txt.Split(',')[0];
            }
            catch (Exception)
            {

                return "";
            }

        }
        public static string GetMsg(this string txt)
        {
            try
            {
                return txt.Split(',')[1];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
