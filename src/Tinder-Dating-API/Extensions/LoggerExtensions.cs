﻿using Serilog;
using System.IO;
using System.Runtime.CompilerServices;
using Tinder_Dating_API.Models.Constants;

namespace Tinder_Dating_API.Extensions
{
    public static class LoggerExtensions
    {
        public static ILogger Here(
            this ILogger logger,
            [CallerMemberName] string member = "",
            [CallerFilePath] string caller = ""
        )
        {
            var callerType = Path.GetFileNameWithoutExtension(caller);
            return logger.ForContext(LoggerConstants.MemberName, member)
                         .ForContext(LoggerConstants.CallerType, callerType);
        }

        public static void MethoEnterd(this ILogger logger)
        {
            logger.Debug(LoggerConstants.MethodEntered);
        }

        public static void MethodExited(this ILogger logger, object withResult = null)
        {
            logger.Debug(LoggerConstants.MethodExited);
            if (withResult != null) logger.Debug("{@MethodExited} with result {@result}", LoggerConstants.MethodExited, withResult);
        }
    }
}
