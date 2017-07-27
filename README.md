# Todo-App
## Zweck
Diese Anwendung ist im Rahmen der Bachelorarbeit zur Erreichung des akamedischen Grades "Bachelor of Science" im Fachbereich "Informatik" der Wilhelm Büchner Hochschule entstanden. Sie dient dazu, die Plattform "ASP.NET Core 2.0" an einem praktischen Beispiel zu veranschaulichen und mit der Plattform "ASP.NET" zu vergleichen.

## Szenario
Die Anwendung stellt eine einfache Todo-Liste dar. Ein User kann sich anmelden und registrieren, um daraufhin beliebig viele Todo-Listen mit beliebig vielen Einträgen pflegen zu können. Abgeschlossene Einträge können als solche gekennzeichnet werden.

## Installation und Inbetriebnahme der ASP.NET Core Variante
1. Download und Installation des .NET Core 2.0 SDK unter https://www.microsoft.com/net/core/preview
    * Zur Kontrolle kann nach der Installation im Terminal der Befehl `dotnet --version` aufgerufen werden. Als Ergebnis müsste etwas auftauchen wie "2.0.0-preview2-006497"
2. Download des GitHub-Repositories via `git clone https://github.com/alexnitter/todo` oder über die "Download"-Schaltfläche auf der Website
3. In das Verzeichnis todo/publish wechseln und dort die Datei appsettings.json wie folgt anpassen
    * "Connectionstring": "[Pfad zur todo.db-Datei]/todo.db"
    * "LogFileDirectory": "[Pfad zu einem beschreibbaren Verzeichnis]"
4. in einem Terminal in das Verzeichnis "publish" wechseln und mit folgendem Befehl die Anwendung starten: `dotnet AlexNitter.Todo.Web.Core.dll`
5. Daraufhin wird der in .NET Core integrierte mini-Webserver "Kestrel" gestartet. In der Konsole wird die URL inklusive Portnummer angegeben (üblicherweise http://localhost:5000), über den die Anwendung angesprochen werden kann. Nun kann die Anwendung im Browser aufgerufen werden. Um Kestrel zu beenden wird ins Terminal gewechselt und die Tastenkombination strg+c gedrückt
