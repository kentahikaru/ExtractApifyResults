using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExtractApifyResults
{
    public class ExtractApifyResultsConfiguration
    {
        public string premenna {get;set;}
    }
}