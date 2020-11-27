using System;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using TreeHouse.Database;

namespace TreeHouse.Services
{
    public class DbService
    {
        private readonly IConfiguration _configuration;
        private readonly IJSRuntime _jsRuntime;

        public DbService(IConfiguration configuration,IJSRuntime jsRuntime)
        {
            _configuration = configuration;
            _jsRuntime = jsRuntime;
        }

        public TreeHouseContext CreateConnection()
        {
            var db = _configuration["db"];
            _jsRuntime.InvokeAsync<string>("console.log", $"Using {db} as db path");
            return new TreeHouseContext(_configuration["db"]);
        }
    }
}
