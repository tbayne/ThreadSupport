# ThreadSupport

## Introduction
A general purpose .NET library to support threads and blocking queues to feed those threads.

Threads are subclassed from the BaseThread class.  Each thread has it's own message queue, and can access the queue's of other threads by accessing the BaseThread's static queueManager (qm).

## .NET Version: 4.5

## Prerequisites
*  Visual Studion 2015

## NuGet Packages Used
*  NUnit
*  Metrics.NET
*  NLog (Install the NLOG.Config package)

## Third Party Documentation
    * NLog          https://github.com/nlog/nlog/wiki
    * Metrics.NET   https://github.com/etishor/Metrics.NET/wiki
    * NUnit

## Logging
* Logging is enabled via NLOG
* Add an NLog.config file to your project (see documentation)

## Stats
Some basic stats are available via Metrics.Net.

### Configure stats in your main()

    Metric.Config
        .WithHttpEndpoint("http://localhost:1234/metrics/")
        .WithAllCounters();
        //.WithInternalMetrics();
        //.WithReporting(config => config
        //    .WithConsoleReport(TimeSpan.FromSeconds(30))
        //.WithCSVReports(@"c:\temp\reports\", TimeSpan.FromSeconds(10))
        //.WithTextFileReport(@"C:\temp\reports\metrics.txt", TimeSpan.FromSeconds(10))
        //.WithGraphite(new Uri("net.udp://localhost:2003"), TimeSpan.FromSeconds(1))
        //.WithInfluxDb("192.168.1.8", 8086, "admin", "admin", "metrics", TimeSpan.FromSeconds(1))
        //.WithElasticSearch("192.168.1.8", 9200, "metrics", TimeSpan.FromSeconds(1))
        //);

### View stats in your browser at: http://localhost:1234/metrics

### Setting up stats collection via app.config
Added the following to the application config file:

    <appSettings>
        <add key="Metrics.HttpListener.HttpUriPrefix" value="http://localhost:1234/"/>
        <add key="Metrics.GlobalContextName" value="RetailApplicationServer"/>
    </appSettings> 

### Viewing the stats
The stats are available on the localhost where your program is running.  Given the configuration above, you can view you stats when your program is running at:

    http://localhost:1234

### More documentation
More documentation on the stats package (Metrics.NET) can be found here:

    https://github.com/etishor/Metrics.NET/wiki    



