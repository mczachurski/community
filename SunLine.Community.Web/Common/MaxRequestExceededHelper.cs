﻿using System;
using System.Web;

namespace SunLine.Community.Web.Common
{
    public static class MaxRequestExceededHelper
    {
        const int TimedOutExceptionCode = -2147467259;

        public static bool IsMaxRequestExceededException(Exception e)
        {
            Exception main;
            var unhandled = e as HttpUnhandledException;
            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }

            var http = main as HttpException;
            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}