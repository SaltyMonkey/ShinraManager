using System;
using System.Management;

namespace ShinraManager.Autorun
{
    internal class WMIProcessWatcher : IDisposable
    {
        private string _processName;

        private ManagementEventWatcher createWatcher;
        private ManagementEventWatcher deleteWatcher;
        private bool createWatcherStarted;
        private bool deleteWatcherStarted;

        public WMIProcessWatcher(string processName)
        {
            _processName = processName;
        }

        //Can be "quota violation" and memory leak - must dispose all
        public void Dispose()
        {
            if (createWatcherStarted)
            {
                createWatcher.Stop();
                createWatcher.Dispose();
                createWatcher = null;
            }
            if (deleteWatcherStarted)
            {
                deleteWatcher.Stop();
                deleteWatcher.Dispose();
                deleteWatcher = null;
            }
        }

        public void AddWatchCreateProcessEvent(EventArrivedEventHandler eventHandler)
        {
            WqlEventQuery query = new WqlEventQuery(
                  $"SELECT * FROM Win32_ProcessStartTrace WHERE ProcessName = '{_processName}'");

            createWatcher = new ManagementEventWatcher(query);
            createWatcher.EventArrived += new EventArrivedEventHandler(eventHandler);

            try
            {
                createWatcher.Start();
                createWatcherStarted = true;
            }
            catch (Exception)
            {
                createWatcherStarted = false;
                //TODO: exception handler AddWatchCreateProcessEvent
            }
        }

        public void AddWatchExitProcessEvent(EventArrivedEventHandler eventHandler)
        {
            WqlEventQuery query = new WqlEventQuery(
                  $"SELECT * FROM Win32_ProcessStopTrace WHERE ProcessName = '{_processName}'");

            deleteWatcher = new ManagementEventWatcher(query);
            deleteWatcher.EventArrived += new EventArrivedEventHandler(eventHandler);

            try
            {
                deleteWatcher.Start();
                deleteWatcherStarted = true;
            }
            catch (Exception)
            {
                deleteWatcherStarted = false;
                //TODO: exception handler AddWatchExitProcessEvent
            }
        }

        public void RemoveWatchCreateProcessEvent()
        {
            if (createWatcherStarted)
            {
                try
                {
                    createWatcher.Stop();
                    createWatcherStarted = false;
                }
                catch
                {
                    createWatcherStarted = false;
                }
            }
        }

        public void RemoveWatchExitProcessEvent()
        {
            if (deleteWatcherStarted)
            {
                try
                {
                    deleteWatcher.Stop();
                    deleteWatcherStarted = false;
                }
                catch
                {
                    deleteWatcherStarted = false;
                }
            }
        }
    }
}