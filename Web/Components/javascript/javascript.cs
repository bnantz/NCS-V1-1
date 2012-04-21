using System;
using System.Text;

namespace Cornerstone
{
   public sealed class AspComponents
   {
      private AspComponents(){}

      public static string PopupWindow(System.Uri url, int width, int height)
      {
         return "<script language='javascript'> {window_handle = window.open('" + url.ToString() + " ','mywindow','width=" + width + ",height=" + height + ",resizable=yes, scrollbars=yes, location=yes, toolbar=yes, status=yes'); window_handle.focus(); return false;}</script>";
      }

      public static string Alert(string message)
      {
         return "<script language='javascript'> { alert ('" + message + "');}</script>";
      }

      public static string Confirm(string message)
      {
         return "<script language='javascript'> { confirm ('" + message + "');}</script>";
      }

      public static string CloseWindow()
      {
         return "<script language='javascript'> { window.close();}</script>";
      }
   }
}
