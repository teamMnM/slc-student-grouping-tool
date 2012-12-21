using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Helper
{
    public class ExceptionHelper
    {
        /// <summary>
        /// Log an exception to Elmah
        /// </summary>
        /// <param name="e">The exception to log</param>
        public static void LogCaughtException(Exception e)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        }

        public static void LogAMessageAsAnException(string message)
        {
            var ex = new Exception(message, new Exception());
            LogCaughtException(ex);
        }
    }
}