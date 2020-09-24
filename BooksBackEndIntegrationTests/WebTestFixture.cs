using BooksBackEnd;
using BooksBackEnd.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooksBackEndIntegrationTests
{
    public class WebTestFixture:WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var systemTimeDescript = services.SingleOrDefault(d => d.ServiceType == typeof(ISystemTime));
                services.Remove(systemTimeDescript);
                services.AddTransient<ISystemTime, FakeSystemTime>();

            });
        }
    }
    public class FakeSystemTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            return new DateTime(1998, 02, 05, 23, 59, 00);
        }
    }
}
