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
        private Process coreRunProcess;

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            RetrySettings.ThreadBoundary = RetrySettingsThreadBoundary.AsyncLocal;

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
            coreRunProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c dotnet run",
                    WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\Atata.WebDriverExtras.TestApp")
                }
            };

            coreRunProcess.Start();

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
            if (coreRunProcess != null)
            {
                coreRunProcess.Kill(true);
                coreRunProcess.Dispose();
            }
        }
    }
}
