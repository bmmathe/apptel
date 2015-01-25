# apptel
Light-weight application telemetry built in .Net.  Your web sites, services, and apps send usage data to a database which is then aggregated and viewed in the apptel web site in the form of a dashboard with drill-downs.  Monitor your apps by glancing at health monitors and usage graphs.  Setup alerts based on thresholds.

AppTel provides two ways of monitoring, pings and pulses.
A ping is initiated by the AppTel service targetting a specified endpoint on a service or a specified web page on a web site.
A pulse is initiated by the web service or web site telling AppTel that an endpoint was called.

Pings can be scheduled using the cron standard. Pings are managed by a separate Windows service.

Pulses are collected by the AppTel web service.  The AppTel client library provides an easy way to send the pulse to the AppTel web service.  You can use these pulses to determine how many times your web service is called in a given time period.

You can also log application errors and exceptions using AppTel.  The AppTel web service provides an endpoint to log exceptions for your application.

The AppTel web site contains a dashboard which shows applications that may be in trouble.  You can set thresholds on ping and pulse frequency to determine when an application should show on the dashboard.  There is also an alert feature so that if an application ping or pulse threshold is met you get a notification.  For example if you set a ping to hit your service endpoint every minute and it fails 5 times in a row you can get notified and/or the application will show on the dashboard with a trouble status.  Likewise you can set thresholds for pulses.  If your service endpoint hasn't recorded a pulse in 1 hour you can have AppTel alert you and/or the application can show on the dashboard.
