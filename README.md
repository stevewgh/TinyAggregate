# Aggregate
Domain Driven Design (DDD) Aggregate

Aggregate designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

* Actor pattern to apply Domain Events to the Aggregate
* Consistent event handling (first time and replaying)
* Uncommited events allow unit testing of the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores
