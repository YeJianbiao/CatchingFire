/**
* 命名空间: Utility.Logger 
* 类    名： LogHelper
* 创 建 人：lenovo
* 创建时间：2018/8/18 21:29:04
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.IO;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4net.config", Watch = true)]
namespace Utility.Logger
{
    public class LogHelper
    {

        private static log4net.ILog logDebug ;
        private static log4net.ILog logInfo ;
        private static log4net.ILog logWarn;
        private static log4net.ILog logError ;
        private static log4net.ILog logFatal ;

        private static void Init()
        {
            logDebug = log4net.LogManager.GetLogger("logDebug");
            logInfo = log4net.LogManager.GetLogger("logInfo");
            logWarn = log4net.LogManager.GetLogger("logWarn");
            logError = log4net.LogManager.GetLogger("logError");
            logFatal = log4net.LogManager.GetLogger("logFatal");
        }

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
            Init();
        }

        public static void SetConfig(string filePath)
        {
            FileInfo configFile = new FileInfo(filePath);
            log4net.Config.XmlConfigurator.Configure(configFile);
            Init();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
            Init();
        }

        public static void Info(string info)
        {
            if (logInfo.IsInfoEnabled)
            {
                logInfo.Info(info);
            }
        }

        public static void Debug(string info)
        {
            if (logDebug.IsErrorEnabled)
            {
                logDebug.Debug(info);
            }
        }

        public static void Warn(string info)
        {
            if (logWarn.IsWarnEnabled)
            {
                logWarn.Warn(info);
            }
        }

        public static void Error(string info, Exception se)
        {
            if (logError.IsErrorEnabled)
            {
                logError.Error(info, se);
            }
        }

        public static void Fatal(string info, Exception se)
        {
            if (logFatal.IsFatalEnabled)
            {
                logFatal.Fatal(info, se);
            }
        }


    }
}
