using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Common.Logging;

using Jenkins.TrayTracker.Utils.Logging;
using DevExpress.XtraEditors;

namespace Jenkins.TrayTracker.Utils
{
    static class ThreadExceptionHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// 
        /// Handles the thread exception.
        /// 
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        public static void HandleException(Exception ex)
        {
            try
            {
                LoggingHelper.LogError(logger, ex);

                // Exit the program after having warned the user
                ShowThreadExceptionDialog(ex);
                Application.Exit();
            }
            catch
            {
                // Fatal error, terminate program
                try
                {
                    XtraMessageBox.Show(JenkinsTrayTrackerResources.FatalError_Message,
                        JenkinsTrayTrackerResources.FatalError_Caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        /// 
        /// Creates and displays the error message.
        /// 
        private static DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            string errorMessage = String.Format(JenkinsTrayTrackerResources.SeriousErrorBoxMessage,
                ex.GetType(), ex.Message);

            return XtraMessageBox.Show(errorMessage,
                JenkinsTrayTrackerResources.ErrorBoxCaption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop);
        }
    }
}
