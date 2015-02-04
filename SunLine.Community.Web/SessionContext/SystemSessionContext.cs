namespace SunLine.Community.Web.SessionContext
{
    public class SystemSessionContext
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string ApplicationPath { get; set; }

        public string FullPath
        {
            get
            {
                return string.Format("http://{0}{1}", Host, ApplicationPath);
            }
        }
    }
}
