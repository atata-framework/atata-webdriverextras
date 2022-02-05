using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Atata.WebDriverSetup;
using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private Process _coreRunProcess;

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            try
            {
                PingTestApp();
            }
            catch (WebException)
            {
                RunTestApp();
            }

            DriverSetup.AutoSetUp(BrowserNames.Chrome);
        }

        private static WebResponse PingTestApp() =>
            WebRequest.CreateHttp(UITestFixture.BaseUrl).GetResponse();

        private void RunTestApp()
        {
            _coreRunProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c dotnet run",
                    WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\Atata.WebDriverExtras.TestApp")
                }
            };

            _coreRunProcess.Start();

            Thread.Sleep(5000);

            var testAppWait = new SafeWait<SetUpFixture>(this)
            {
                Timeout = TimeSpan.FromSeconds(40),
                PollingInterval = TimeSpan.FromSeconds(1)
            };

            testAppWait.IgnoreExceptionTypes(typeof(WebException));

            testAppWait.Until(x => PingTestApp());
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            if (_coreRunProcess != null)
            {
                _coreRunProcess.Kill(true);
                _coreRunProcess.Dispose();
            }
        }
    }
}
