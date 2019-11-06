using System;

namespace Objectia
{
    public class ObjectiaClient 
    {
        private static string _apiKey;
        private static int _timeout = 30;

        private static RestClient _restClient = null;

        private ObjectiaClient() {}

        /// <summary>
        /// Initialize the client with apikey and timeout
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="timeout">Connection timeout</param>
        public static void Init(string apiKey, int timeout=30)
        {
            SetApiKey(apiKey);
            SetTimeout(timeout);
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

            if (apiKey != _apiKey)
            {
                Invalidate();
            }

            _apiKey = apiKey;
        }

        /// <summary>
        /// Set timeout
        /// </summary>
        /// <param name="timeout">Connection timeout</param>
        public static void SetTimeout(int timeout)
        {
            if (timeout != _timeout)
            {
                Invalidate();
            }
            _timeout = timeout;
        }

        /// <summary>
        /// Get the rest client
        /// </summary>
        /// <returns>The rest client</returns>
        public static RestClient GetRestClient()
        {
            if (_restClient != null)
            {
                return _restClient;
            }

            if (_apiKey == null)
            {
                ///throw new AuthenticationException (
                throw new ArgumentException(
                    "RestClient was used before ApiKey was set, please call ObjectiaClient.init()"
                );
            }

            _restClient = new RestClient(_apiKey, _timeout);
            return _restClient;
        }

         /// <summary>
        /// Set the rest client
        /// </summary>
        /// <param name="restClient">Rest Client to use</param>
        public static void SetRestClient(RestClient restClient)
        {
            _restClient = restClient;
        }

        /// <summary>
        /// Clear out the Rest Client
        /// </summary>
        public static void Invalidate()
        {
            _restClient = null;
        }
    }
}
