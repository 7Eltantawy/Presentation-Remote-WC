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
        public static string GetKey(this string txt)
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

        private static string GetMouseInfo(this string txt)
        {
            try
            {
                return txt.Split(',')[2];
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetMouseX(this string txt)
        {
            try
            {
                return txt.GetMouseInfo().Split('*')[0];
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetMouseY(this string txt)
        {
            try
            {
                return txt.GetMouseInfo().Split('*')[1];
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetMouseClick(this string txt)
        {
            try
            {
                return txt.GetMouseInfo().Split('*')[2];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
