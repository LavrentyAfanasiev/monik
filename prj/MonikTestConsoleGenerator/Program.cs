﻿using System;
using System.Configuration;
using System.Linq;
using Gerakul.FastSql;
using Monik.Client.Azure.Sender;
using Monik.Client.Settings;
using MonikTestConsoleGenerator.Metrics;

namespace MonikTestConsoleGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connstr = ConfigurationManager.AppSettings["DBConnectionString"];

            var proto    = new {name = default(string), value = default(string)};
            var settings = SimpleCommand.ExecuteQueryAnonymous(proto, connstr, "select Name, Value from mon.Settings").ToDictionary(p => p.name, p => p.value);

            var asureSender = new AzureSender(settings["OutcomingConnectionString"], settings["OutcomingQueue"]);

            var monikTestGeneratorInstance = new MonikTestGeneratorInstance(asureSender, new ClientSettings()
            {
                AutoKeepAliveEnable = false,
                SourceName          = "TestSource",
                InstanceName        = "TestInstance"
            });

            //var logsSender = new InstancesLogsSender(10000, source, monikTestGeneratorInstance);
            //logsSender.StartSendingLogs();

            var metricSender = new MetricsSender(monikTestGeneratorInstance, TimeSpan.FromSeconds(15));
            metricSender.StartSendingMetrics();
        }
    }
}