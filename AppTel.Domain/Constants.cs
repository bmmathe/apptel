using System.Configuration;

namespace AppTel.Domain
{
    public static class Constants
    {
        private static string _databaseName;
        public static string DatabaseName
        {
            get
            {
                if (string.IsNullOrEmpty(_databaseName))
                {
                    var configuredDatabaseName = ConfigurationManager.AppSettings["AppTel_DatabaseName"];
                    _databaseName = string.IsNullOrEmpty(configuredDatabaseName) ? "apptel" : configuredDatabaseName;
                }

                return _databaseName;
            }
        }

        private static string _collectionName;
        public static string CollectionName
        {
            get
            {
                if (string.IsNullOrEmpty(_collectionName))
                {
                    var configuredCollectionName = ConfigurationManager.AppSettings["AppTel_CollectionName"];
                    _collectionName = string.IsNullOrEmpty(configuredCollectionName) ? "applications" : configuredCollectionName;
                }

                return _collectionName;
                
            }
        }
    }
}
