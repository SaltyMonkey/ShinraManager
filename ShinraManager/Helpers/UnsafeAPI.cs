using System;
using System.Runtime.InteropServices;

namespace ShinraManager.Helpers.Native
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

        /// <summary>
        /// Activate window
        /// </summary>
        /// <param name="hWnd">window pointer to active</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
    }
}