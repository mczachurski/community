using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;

namespace SunLine.Community.NUnitTests.Controllers
{
    [TestFixture]
    public class CommonControllerTest
    {
        [Test]
        public void all_actions_with_http_post_method_attribute_must_have_anti_forgery_token_attribute()
        {
            Type[] types = GetControllerTypes();
            var errors = new StringBuilder();
            bool wasError = false;

            foreach (Type type in types)
            {
                MethodInfo[] methods = GetMethods(type);
                foreach (MethodInfo methodInfo in methods)
                {
                    var antiFogery = Attribute.GetCustomAttribute(methodInfo, typeof(ValidateAntiForgeryTokenAttribute), false) as ValidateAntiForgeryTokenAttribute;
                    var httpPost = Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute), false) as HttpPostAttribute;

                    if (httpPost != null && antiFogery == null)
                    {
                        errors.AppendFormat("{0}/{1}; ", type.Name, methodInfo.Name);
                        wasError = true;
                    }
                }
            }
                
            Assert.IsFalse(wasError, "All controlers with HttpPost attriute must have anti forgery token validation attribute: " + errors);
        }
            
        [Test]
        public void all_actions_must_have_http_method_attribute()
        {
            Type[] types = GetControllerTypes();
            var errors = new StringBuilder();
            bool wasError = false;

            foreach (Type type in types)
            {
                MethodInfo[] methods = GetMethods(type);
                foreach (MethodInfo methodInfo in methods)
                {
                    var httpGet = Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute), false) as HttpGetAttribute;
                    var httpPost = Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute), false) as HttpPostAttribute;
                    var verbs = Attribute.GetCustomAttribute(methodInfo, typeof(AcceptVerbsAttribute), false) as AcceptVerbsAttribute;

                    if (httpPost == null && httpGet == null && verbs == null)
                    {
                        errors.AppendFormat("{0}/{1}; ", type.Name, methodInfo.Name);
                        wasError = true;
                    }
                }
            }
                
            Assert.IsFalse(wasError, "All actions must have http method attribute: " + errors);
        }

        [Test]
        public void all_controllers_must_have_authorize_attribute()
        {
            Type[] types = GetControllerTypes();
            var errors = new StringBuilder();
            bool wasError = false;

            foreach (Type type in types)
            {
                var authorize = Attribute.GetCustomAttribute(type, typeof(AuthorizeAttribute), false) as AuthorizeAttribute;

                if (authorize == null)
                {
                    errors.AppendFormat("{0}; ", type.Name);
                    wasError = true;
                }
            }
                
            Assert.IsFalse(wasError, "All controllers must have authorize attribute: " + errors);
        }

        private Type[] GetControllerTypes()
        {
            Type[] types = null;

            Assembly ass = Assembly.Load("SunLine.Community.Web");

            if (ass != null)
            {
                types = ass.GetTypes().Where(x => x.BaseType == typeof(Controller) && x.IsPublic).ToArray();
            }

            return types;
        }

        private MethodInfo[] GetMethods(Type t)
        {
            return t.GetMethods().Where(x => x.IsPublic && x.DeclaringType == t).ToArray();
        }
    }
}

