using System.Web;
using Microsoft.Practices.Unity;

namespace SunLine.Community.Web.Common
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private string _key = string.Empty;

        public override object GetValue()
        {
            return HttpContext.Current.Items[_key];
        }

        public override void SetValue(object newValue)
        {
            _key = newValue.GetType().FullName;
            HttpContext.Current.Items[newValue.GetType().FullName] = newValue;
        }

        public override void RemoveValue()
        {
            var obj = GetValue();
            HttpContext.Current.Items.Remove(obj);
        }
    }
}