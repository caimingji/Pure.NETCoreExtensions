﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Pure.NetCoreExtensions
{
    public static class LoggerFactoryExtensions
    {
 
        public static IApplicationBuilder UseGlobalLoggerFactory(this IApplicationBuilder app)
        {
            var svr = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            GlobalLoggerFactory.Configure(svr);
            return app;
        }

        public static ILoggerFactory AddFileLogger(this ILoggerFactory factory)
        {
            factory.AddProvider(new FileLoggerProvider());
            return factory;
        }


        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the logger factory.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">The action used to add to the logger factory.</param>
        /// <returns>The same logger factory.</returns>
        public static ILoggerFactory AddIf(
            this ILoggerFactory loggerFactory,
            bool condition,
            Func<ILoggerFactory, ILoggerFactory> action)
        {
            if (condition)
            {
                loggerFactory = action(loggerFactory);
            }

            return loggerFactory;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the logger factory.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the logger factory if the condition is <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the logger factory if the condition is <c>false</c>.</param>
        /// <returns>The same logger factory.</returns>
        public static ILoggerFactory AddIfElse(
            this ILoggerFactory loggerFactory,
            bool condition,
            Func<ILoggerFactory, ILoggerFactory> ifAction,
            Func<ILoggerFactory, ILoggerFactory> elseAction)
        {
            if (condition)
            {
                loggerFactory = ifAction(loggerFactory);
            }
            else
            {
                loggerFactory = elseAction(loggerFactory);
            }

            return loggerFactory;
        }
    }

}