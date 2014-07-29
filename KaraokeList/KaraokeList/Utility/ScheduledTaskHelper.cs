using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Utility
{
    public class ScheduledTaskHelper
    {
        public static void StartPeriodicTask(string taskName)
        {
            //PeriodicTask periodicTask = new PeriodicTask(taskName);
            //periodicTask.Description = "Are presenting a periodic task";
            try
            {
                // Variable for tracking enabled status of background agents for this app.
                bool agentEnabled = true;

                // Obtain a reference to the period task, if one exists
                PeriodicTask periodicTask = ScheduledActionService.Find(taskName) as PeriodicTask;

                // If the task already exists and background agents are enabled for the
                // application, you must remove the task and then add it again to update 
                // the schedule
                if (periodicTask != null)
                {
                    ScheduledActionService.Remove(taskName);
                }

                periodicTask = new PeriodicTask(taskName);

                // The description is required for periodic agents. This is the string that the user
                // will see in the background services Settings page on the device.
                periodicTask.Description = "This task updates every 30 minutes.";         

                ScheduledActionService.Add(periodicTask);
                ScheduledActionService.LaunchForTest(taskName, TimeSpan.FromSeconds(3));
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("exists already"))
                {
                    MessageBox.Show("Since then the background agent success is already running");
                }
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background processes for this application has been prohibited");
                }
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type has already been added."))
                {
                    MessageBox.Show("You open the daemon has exceeded the hardware limitations");
                }
            }
            catch (SchedulerServiceException)
            {

            }
        }
        public static void StopPeriodicTask(string taskName)
        {
            try
            {
                ScheduledActionService.Remove(taskName);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("doesn't exist"))
                {
                    //MessageBox.Show("Since then the background agent success is not running");
                }
            }
            catch (SchedulerServiceException)
            {

            }
        }

        public static void SetData()
        {
            Mutex mutex = new Mutex(false, "QuaTangCuocSongLastLaunchAgentData");
            mutex.WaitOne();
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            setting.Remove("QuaTangCuocSongLastLaunchAgentData");
            setting.Add("QuaTangCuocSongLastLaunchAgentData", DateTime.Now);
            mutex.ReleaseMutex();
        }

    }
}
