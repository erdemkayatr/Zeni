namespace Zeni.Infra.Logging
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceLogAttribute : Attribute
    {
        public readonly bool _requestLog;
        public readonly bool _responseLog;
        /// <summary>
        /// Default Values are true
        /// </summary>
        /// <param name="requestLog"></param>
        /// <param name="responseLog"></param>
        public ServiceLogAttribute(bool requestLog = true, bool responseLog = true)
        {
            _requestLog = requestLog;
            _responseLog = responseLog;
        }



    }
}
