# ServiceOrchestrator

Steps to execute:
1. Start the Account Service, Inventory and Payment Service by running the <b>dotnet <i>ServiceName</i>.dll</b> from appropriate bin folders.
2. Start the External Service which starts as triggering point for the sample workflow.
3. Observe the Conlose logs
4. To change workflow, modify the <b>handlers.json</b> file to map the events to appropriate tasks.
