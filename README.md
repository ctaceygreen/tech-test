Hi,

This system has been built for a coding test.

The system was required to add new orders and list existing orders, linked to customers.

Notes on the system:
 - Database. The DB.mdf should be included within the repo, however if you actually want to test the project you'll need to ensure that the connection strings defined in Web.config and App.config are pointed to wherever your DB.mdf is downloaded to. In production this would obviously be pointing to a proper production database server using shared connection strings, but seemed like overkill for this!
 - Unit of Work and repository pattern. Instead of simply implementing EF, I wanted to show understanding and therefore built the data access on top of Ado.Net. It uses Repositories and Unit of Works to allow for proper transaction handling and passing of connections between repositories.
 - Existing Repositories. Since the rules stated that the CustomerRepository could not be changed to non-static, I just made them string holders for SQL commands. We don't want to be using actual static repositories or else you can have some nasty clashes with transactions and commits! The proper repositories are the RepositoryUmbrella classes.
 - Dependency Injection. As much as possible, the system uses Autofac to inject repositories/services into required classes. This allows for easier unit testing, and just a better structure of project!
 - Order Processing. Orders can be processed in two ways: 1) Just using the NormalOrderSender which simply calls the OrderService. 2) Using the RabbitOrderSender which uses RabbitMQ to queue a new event to be processed. If you just want to use the normal order sender, please change the Registered type within IoCof OrderSendReceive project from RabbitOrderSender to NormalOrderSender. If you DO want to use RabbitMQ, please ensure it is installed, the service is running on the computer, and that you are running the OrderSendReceive program within this project.
 - Front-end. The front-end is WebApi combined with ReactJS to allow for realtime updates to the UI when you add orders. Ignore the design of the UI, I decided that it didn't need to look pretty for this test, so no validation and the simplest HTML has been used!

With more time, what I would improve:
 - ReactJS. Separate into separate class files for each component.
 - Front-end validation.
 - Make country selectable from a list of countries
 - Make DoB have to be in the past.
 - Clean up RabbitMQ Order Sending and Receiving to make the classes use similar code. I added Rabbit in fairly quickly so it definitely could be structured better!


Enjoy, and any questions please ask away!

Thanks,
Chris
