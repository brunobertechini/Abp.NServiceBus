# Abp.NServiceBus
ASP.NET Boilerplate integration with NServiceBus

## Goals

* Provide samples to allow NServiceBus integration into ASP.NET Boilerplate applications
* Create a Abp.NServiceBus package that might become official sometime

## Definitions

* Abp Web will be hosted in Azure AppService
* NServiceBus v6 will be hosted in Azure WebJobs

## NServiceBus 6

* Unobtrusive Messages
* Custom AbpNServiceBusUnitOfWork using Pipeline Behaviors
* Custom AbpNServiceBusSession to handle multitenancy
