using NUnit.Framework;
using SunLine.Community.Services;

namespace SunLine.Community.NUnitTests.Services
{
    public class BaseTest
    {
        private static bool _dbWasInitialized;

        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            if (!_dbWasInitialized)
            {
                DatabaseHelper.ClearTemplateDatabase();
                DatabaseSetup.Init(true);
                UnityConfig.Register();

                DatabaseHelper.CreateTemplateData();
                _dbWasInitialized = true;
            }
        }

        [TestFixtureTearDown]
        public virtual void TeadDown()
        {
        }
    }
}

