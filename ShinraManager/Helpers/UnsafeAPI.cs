using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShinraManager.Helpers
{
    public static class UnsafeAPI
    {
        /// <summary>
        /// Check if window minimized
        /// </summary>
        /// <param name="hWnd">ptr to window</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// Open window if its minimized
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool OpenIcon(IntPtr hWnd);

        /// <summary>
        /// Find window by caption
        /// </summary>
        /// <param name="cls">null</param>
        /// <param name="win">caption</param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string cls, string win);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);



        /// <summary>
        /// Activate window
        /// </summary>
        /// <param name="hWnd">window pointer to active</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
