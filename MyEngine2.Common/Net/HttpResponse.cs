namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 响应头
    /// </summary>
    public class HttpResponse : HttpPackage
    {
        /// <summary>
        /// Http 版本 - 默认为 HTTP/1.1
        /// </summary>
        public string Version { get; set; } = "HTTP/1.1";

        /// <summary>
        /// Http 状态码 - 默认为 200
        /// </summary>
        public string StateCode { get; set; } = "200";

        /// <summary>
        /// Http 状态码注释 - 默认为 ""
        /// </summary>
        public string Description { get; set; } = "OK";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="version">Http 版本</param>
        /// <param name="stateCode">Http 状态码</param>
        /// <param name="description">Http 注释</param>
        public HttpResponse(string version, string stateCode, string description)
        {
            Version = version;
            StateCode = stateCode;
            Description = description;
        }

        /// <summary>
        /// 构造函数 - 自动设置 Description
        /// </summary>
        /// <param name="version">Http 版本</param>
        /// <param name="stateCode">Http 状态码</param>
        public HttpResponse(string version, string stateCode)
        {
            Version = version;
            StateCode = stateCode;
            Description = HttpState.Description[stateCode];
        }

        /// <summary>
        /// 将常规属性重置为默认值
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            Version = "HTTP/1.1";
            StateCode = "200";
            Description = "OK";
        }
    }
}