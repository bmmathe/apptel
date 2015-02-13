using System;
using System.Configuration;

namespace AppTel.Client
{
    public class AppError
    {
        private string _applicationName;

        /// <summary>
        /// The application name that will be logged in AppTel.  This property can be dynamically set by adding an appSetting called AppTel.ApplicationName to your configuration file.
        /// </summary>
        public string ApplicationName
        {
            get { return _applicationName ?? (_applicationName = ConfigurationManager.AppSettings["AppTel.ApplicationName"]); }
            set { _applicationName = value; }
        }

        public Exception Exception { get; set; }
    }
}
