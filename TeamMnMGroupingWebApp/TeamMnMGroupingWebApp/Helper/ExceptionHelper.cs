using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Helper
{
    public class ExceptionHelper
    {
        public static void LogCaughtException(Exception e)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        }
    }
}