using System;

namespace Objectia
{
    public class ObjectiaClient
    {
        private static string _apiKey;
        private static int _timeout = 30;

        private ObjectiaClient() { }

        /// <summary>
        /// Initialize the client with apikey and timeout
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="timeout">Connection timeout</param>
        public static void Init(string apiKey, int timeout = 30)
        {
            SetApiKey(apiKey);
            SetTimeout(timeout);
        }


        public static string GetApiKey()
        {
            return _apiKey;
        }


        public static int GetTimeout()
        {
            return _timeout;
        }


        /// <summary>
        /// Set the api key
        /// </summary>
        /// <param name="apiKey">API key</param>
        public static void SetApiKey(string apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentException("No API key provided");
                //throw new AuthenticationException ... ???
            }
            _apiKey = apiKey;
        }

        /// <summary>
        /// Set timeout
        /// </summary>
        /// <param name="timeout">Connection timeout</param>
        public static void SetTimeout(int timeout)
        {
            _timeout = timeout;
        }

        public static string Version
        {
            get
            {
                return Constants.VERSION;
            }
        }
    }
}
