namespace DataModel.Constants
{
    public static class ApiCallManager
    {
        public const string HEALTH_CHECK = "HealthCheck";

        public const string LIVE_SESSION = "LiveSession";
        public const string ALL_LIVE_SESSIONS = "AllLiveSessions";
        public const string POST_NEW_SESSION = "PostNewSession";
        public const string CHANGE_SESSION_TO_LIVE = "ChangeSessionToLive";
        public const string CHANGE_SESSION_TO_OFFLINE = "ChangeSessionToOffline";
        public const string CHANGE_SESSION_NAME = "ChangeSessionName";
        public const string CHANGE_SESSION_DATE = "ChangeSessionDate";
        public const string DELETE_SESSION = "DeleteSession";
        public const string ACTIVE_SESSION_SENSORS = "ActiveSessionSensors";

        public const string GET_PACKAGE_BY_ID = "GetPackageById";
        public const string GET_PACKAGES_AFTER = "GetPackagesAfter";
        public const string GET_ALL_PACKAGES = "GetAllPackages";

        public const string GET_ALL_SENSORS = "GetAllSensors";

        public const string HTTP_CLIENT_HEADER_TYPE = "application/json";
    }
}
