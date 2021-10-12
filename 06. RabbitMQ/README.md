# RabbitMQ for beginners

[RabbitMQ](https://www.rabbitmq.com/tutorials/amqp-concepts.html) is a message-queueing software also known as a message broker or queue manager. Simply said; it is software where queues are defined, to which applications connect in order to transfer a message or messages.

![Message Queue](./message-queue-small.png)

A message can include any kind of information. It could, for example, have information about a process or task that should start on another application (which could even be on another server), or it could be just a simple text message. The queue-manager software stores the messages until a receiving application connects and takes a message off the queue. The receiving application then processes the message.

## RabbitMQ Example

A message broker acts as a middleman for various services (e.g. a web application, as in this example). They can be used to reduce loads and delivery times of web application servers by delegating tasks that would normally take up a lot of time or resources to a third party that has no other job.

In this guide, we follow a scenario where a web application allows users to upload information to a website. The site will handle this information, generate a PDF, and email it back to the user. Handling the information, generating the PDF, and sending the email will, in this example case, take several seconds. That is one of the reasons why a message queue will be used to perform the task.

When the user has entered user information into the web interface, the web application will create a "PDF processing" message that includes all of the important information the user needs into a message and place it onto a queue defined in RabbitMQ.

![RabbitMQ Workflow](https://user-images.githubusercontent.com/34960418/136807782-c988644a-6c93-464b-8602-32c5b50518c0.png)

The basic architecture of a message queue is simple - there are client applications called producers that create messages and deliver them to the broker (the message queue). Other applications, called consumers, connect to the queue and subscribe to the messages to be processed. Software may act as a producer, or consumer, or both a consumer and a producer of messages. Messages placed onto the queue are stored until the consumer retrieves them.

## When and why should you use RabbitMQ?

Message queueing allows web servers to respond to requests quickly instead of being forced to perform resource-heavy procedures on the spot that may delay response time. Message queueing is also good when you want to distribute a message to multiple consumers or to balance loads between workers.

The consumer takes a message off the queue and starts processing the PDF. At the same time, the producer is queueing up new messages. The consumer can be on a totally different server than the producer or they can be located on the same server. The request can be created in one programming language and handled in another programming language. The point is, the two applications will only communicate through the messages they are sending to each other, which means the sender and receiver have low coupling.

![RabbitMQ Workflow Extended](https://user-images.githubusercontent.com/34960418/136808215-dd337d45-b6b5-427d-a0b9-2c29ec04fbf1.png)

1. The user sends a PDF creation request to the web application.
2. The web application (the producer) sends a message to RabbitMQ that includes data from the request such as name and email.
3. An exchange accepts the messages from the producer and routes them to correct message queues for PDF creation.
4. The PDF processing worker (the consumer) receives the task message and starts processing the PDF.

## EXCHANGES

Messages are not published directly to a queue; instead, the producer sends messages to an exchange. An exchange is responsible for routing the messages to different queues with the help of bindings and routing keys. A binding is a link between a queue and an exchange.

## Message flow in RabbitMQ

![RabbitMQ Message Flow](https://user-images.githubusercontent.com/34960418/136808837-bbcb5527-3106-4aad-87e7-f444f7f70b9a.png)

1. The producer publishes a message to an exchange. When creating an exchange, the type must be specified. This topic will be covered later on.
2. The exchange receives the message and is now responsible for routing the message. The exchange takes different message attributes into account, such as the routing key, depending on the exchange type.
3. Bindings must be created from the exchange to queues. In this case, there are two bindings to two different queues from the exchange. The exchange routes the message into the queues depending on message attributes.
4. The messages stay in the queue until they are handled by a consumer
5. The consumer handles the message.

## TYPES OF EXCHANGES

![RabbitMQ Exchange Types](https://user-images.githubusercontent.com/34960418/136809527-fcb7b425-4325-4def-8218-866961266deb.png)

- **Direct**: The message is routed to the queues whose binding key exactly matches the routing key of the message. For example, if the queue is bound to the exchange with the binding key pdfprocess, a message published to the exchange with a routing key pdfprocess is routed to that queue.
- **Fanout**: A fanout exchange routes messages to all of the queues bound to it.
- **Topic**: The topic exchange does a wildcard match between the routing key and the routing pattern specified in the binding.
- **Headers**: Headers exchanges use the message header attributes for routing.

## RABBITMQ AND SERVER CONCEPTS

Some important concepts need to be described before we dig deeper into RabbitMQ. The default virtual host, the default user, and the default permissions are used in the examples, so let’s go over the elements and concepts:

- **Producer**: Application that sends the messages.
- **Consumer**: Application that receives the messages.
- **Queue**: Buffer that stores messages.
- **Message**: Information that is sent from the producer to a consumer through RabbitMQ.
- **Connection**: A TCP connection between your application and the RabbitMQ broker.
- **Channel**: A virtual connection inside a connection. When publishing or consuming messages from a queue - it's all done over a channel.
- **Exchange**: Receives messages from producers and pushes them to queues depending on rules defined by the exchange type. To receive messages, a queue needs to be bound to at least one exchange.
- **Binding**: A binding is a link between a queue and an exchange.
- **Routing key**: A key that the exchange looks at to decide how to route the message to queues. Think of the routing key like an address for the message.
- **AMQP**: Advanced Message Queuing Protocol is the protocol used by RabbitMQ for messaging.
- **Users**: It is possible to connect to RabbitMQ with a given username and password. Every user can be assigned permissions such as rights to read, write and configure privileges within the instance. Users can also be assigned permissions for specific virtual hosts.
- **Vhost, virtual host**: Provides a way to segregate applications using the same RabbitMQ instance. Different users can have different permissions to different vhost and queues and exchanges can be created, so they only exist in one vhost.

At the beginning of this article series, we had one producer (the website application) and one consumer (the PDF processing application). If the PDF processing application crashes, or if many PDF requests are coming at the same time, messages would continue to stack up in the queue until the consumer starts again. It would then process all the messages, one by one.

## PUBLISH AND SUBSCRIBE MESSAGES

RabbitMQ uses a protocol called AMQP by default. To be able to communicate with RabbitMQ you need a library that understands the same protocol as RabbitMQ. Download the client library for the programming language that you intend to use for your applications. A client library is an application programming interface (API) for use in writing client applications. A client library has several methods; in this case, to communicate with RabbitMQ. The methods should be used when you connect to the RabbitMQ broker (using the given parameters, hostname, port number, etc.), for example, or when you declare a queue or an exchange. There is a choice of libraries for almost every programming language.

Steps to follow when setting up a connection and publishing a message/consuming a message:

1. Set up/create a connection object. The username, password, connection URL, port, etc., will need to be specified. A TCP connection will be set up between the application and RabbitMQ when the start method is called.
2. Create a channel in the TCP connection, then the connection interface can be used to open a channel through which to send and receive messages.
3. Declare/create a queue. Declaring a queue will cause it to be created if it does not already exist. All queues need to be declared before they can be used.
4. Set up exchanges and bind a queue to an exchange in subscriber/consumer. All exchanges must be declared before they can be used. An exchange accepts messages from a producer application and routes them to message queues. For messages to be routed to queues, queues must be bound to an exchange.
5. In publisher: Publish a message to an exchange. In subscriber/consumer: Consume a message from a queue.
7. Close the channel and the connection.

## Exchanges, routing keys and bindings

Messages are not published directly to a queue. Instead, the producer sends messages to an exchange. Exchanges are message routing agents, defined by the virtual host within RabbitMQ. An exchange is responsible for routing the messages to different queues with the help of header attributes, bindings, and routing keys.

A **binding** is a "link" that you set up to bind a queue to an exchange.

The **routing key** is a message attribute the exchange looks at when deciding how to route the message to queues (depending on exchange type).

Exchanges, connections, and queues can be configured with parameters such as durable, temporary, and auto delete upon creation. Durable exchanges survive server restarts and last until they are explicitly deleted. Temporary exchanges exist until RabbitMQ is shut down. Auto-deleted exchanges are removed once the last bound object is unbound from the exchange.

In RabbitMQ, there are four different types of exchanges that route the message differently using different parameters and bindings setups. Clients can create their own exchanges or use the predefined default exchanges which are created when the server starts for the first time.

### Standard RabbitMQ message flow

![RabbitMQ Message Flow](https://user-images.githubusercontent.com/34960418/136812727-f258bbfc-f425-49bc-8dfc-56814a1c0dd8.png)

1. The producer publishes a message to the exchange.
2. The exchange receives the message and is now responsible for the routing of the message.
3. Binding must be set up between the queue and the exchange. In this case, we have bindings to two different queues from the exchange. The exchange routes the message into the queues.
4. The messages stay in the queue until they are handled by a consumer.
5. The consumer handles the message.

### Exchange types

#### Direct Exchange

A direct exchange delivers messages to queues based on a message routing key. The routing key is a message attribute added to the message header by the producer. Think of the routing key as an "address" that the exchange is using to decide how to route the message. **A message goes to the queue(s) with the binding key that exactly matches the routing key of the message.**

The direct exchange type is useful to distinguish messages published to the same exchange using a simple string identifier.

The default exchange AMQP brokers must provide for the direct exchange is "amq.direct".

Imagine that queue A (create_pdf_queue) in the image below (Direct Exchange Figure) is bound to a direct exchange (pdf_events) with the binding key pdf_create. When a new message with routing key pdf_create arrives at the direct exchange, the exchange routes it to the queue where the binding_key = routing_key, in the case to queue A (create_pdf_queue).

It is perfectly legal to bind multiple queues with the same **binding key**. In that case, the direct exchange will behave like **fanout** and will broadcast the message to all the matching queues.

![Direct Exchange](https://user-images.githubusercontent.com/34960418/136813402-d9e2c25e-0208-4a98-8bbe-f9ea8fe85ba1.png)

SCENARIO 1
- Exchange: pdf_events
- Queue A: create_pdf_queue
- Binding key between exchange (pdf_events) and Queue A (create_pdf_queue): pdf_create

SCENARIO 2
- Exchange: pdf_events
- Queue B: pdf_log_queue
- Binding key between exchange (pdf_events) and Queue B (pdf_log_queue): pdf_log

EXAMPLE
Example: A message with routing key pdf_log is sent to the exchange pdf_events. The messages is routed to pdf_log_queue because the routing key (pdf_log) matches the binding key (pdf_log).

Direct Exchange: A message goes to the queues whose binding key exactly matches the routing key of the message. If the message routing key does not match any binding key, the message is discarded.

#### Default exchange

The default exchange is a pre-declared direct exchange with no name, usually referred by an empty string. When you use default exchange, your message is delivered to the queue with a name equal to the routing key of the message. Every queue is automatically bound to the default exchange with a routing key which is the same as the queue name.

#### Topic Exchange

Topic exchanges route messages to queues based on wildcard matches between the routing key and the routing pattern, which is specified by the queue binding. Messages are routed to one or many queues based on a matching between a message routing key and this pattern.

The routing key must be a list of words, delimited by a period (.). Examples are agreements.us and agreements.eu.stockholm which in this case identifies agreements that are set up for a company with offices in lots of different locations. The routing patterns may contain an asterisk (“*”) to match a word in a specific position of the routing key (e.g., a routing pattern of "agreements.*.*.b.*" only match routing keys where the first word is "agreements" and the fourth word is "b"). A pound symbol (“#”) indicates a match of zero or more words (e.g., a routing pattern of "agreements.eu.berlin.#" matches any routing keys beginning with "agreements.eu.berlin").

The consumers indicate which topics they are interested in (like subscribing to a feed for an individual tag). The consumer creates a queue and sets up a binding with a given routing pattern to the exchange. All messages with a routing key that match the routing pattern are routed to the queue and stay there until the consumer consumes the message.

The default exchange AMQP brokers must provide for the topic exchange is "amq.topic".

![Topic Exchange](https://user-images.githubusercontent.com/34960418/136815106-135fa350-5a8c-4e64-8e10-6240e86ba27f.png)

SCENARIO 1
The image to the right shows an example where consumer A is interested in all the agreements in Berlin.
- Exchange: agreements
- Queue A: berlin_agreements
- Routing pattern between exchange (agreements) and Queue A (berlin_agreements): agreements.eu.berlin.#
- Example of message routing key that matches: agreements.eu.berlin and agreements.eu.berlin.headstore

SCENARIO 2
Consumer B is interested in all the agreements.
- Exchange: agreements
- Queue B: all_agreements
- Routing pattern between exchange (agreements) and Queue B (all_agreements): agreements.#
- Example of message routing key that matches: agreements.eu.berlin and agreements.us
rabbitmq topic exchange

SCENARIO 3
- Consumer C is interested in all agreements for European head stores.
- Exchange: agreements
- Queue C: headstore_agreements
- Routing pattern between exchange (agreements) and Queue C (headstore_agreements): agreements.eu.*.headstore
- Example of message routing keys that will match: agreements.eu.berlin.headstore and agreements.eu.stockholm.headstore

EXAMPLE: A message with routing key agreements.eu.berlin is sent to the exchange agreements. The messages are routed to the queue berlin_agreements because the routing pattern of "agreements.eu.berlin.#" matches the routing keys beginning with "agreements.eu.berlin". The message is also routed to the queue all_agreements because the routing key (agreements.eu.berlin) matches the routing pattern (agreements.#).

Topic Exchange: Messages are routed to one or many queues based on a match between a message routing key and the routing pattern.

#### Fanout Exchange

A fanout exchange copies and routes a received message to all queues that are bound to it regardless of routing keys or pattern matching as with direct and topic exchanges. The keys provided will simply be ignored.

Fanout exchanges can be useful when the same message needs to be sent to one or more queues with consumers who may process the same message in different ways.

The image to the right (Fanout Exchange) shows an example where a message received by the exchange is copied and routed to all three queues bound to the exchange. It could be sport or weather updates that should be sent out to each connected mobile device when something happens, for instance.

The default exchange AMQP brokers must provide for the topic exchange is "amq.fanout".

![Fanout Exchange](https://user-images.githubusercontent.com/34960418/136817684-17e9449d-bf96-4011-9d57-f82272c222ae.png)

SCENARIO 1
- Exchange: sport_news
- Queue A: Mobile client queue A
- Binding: Binding between the exchange (sport_news) and Queue A (Mobile client queue A)

EXAMPLE: A message is sent to the exchange sport_news. The message is routed to all queues (Queue A, Queue B, Queue C) because all queues are bound to the exchange. Provided routing keys are ignored.

Fanout Exchange: The received message is routed to all queues that are bound to the exchange.

#### Headers Exchange

A headers exchange routes messages based on arguments containing headers and optional values. Headers exchanges are very similar to topic exchanges, but route messages based on header values instead of routing keys. A message matches if the value of the header equals the value specified upon binding.

A special argument named "x-match", added in the binding between exchange and queue, specifies if all headers must match or just one. Either any common header between the message and the binding count as a match, or all the headers referenced in the binding need to be present in the message for it to match. The "x-match" property can have two different values: "any" or "all", where "all" is the default value. A value of "all" means all header pairs (key, value) must match, while value of "any" means at least one of the header pairs must match. Headers can be constructed using a wider range of data types, integer or hash for example, instead of a string. The headers exchange type (used with the binding argument "any") is useful for directing messages which contain a subset of known (unordered) criteria.

The default exchange AMQP brokers must provide for the topic exchange is "amq.headers".

![Headers Exchange](https://user-images.githubusercontent.com/34960418/136818353-e3164846-09dc-4622-a307-a3964d029ac1.png)

EXAMPLE
- Exchange: Binding to Queue A with arguments (key = value): format = pdf, type = report, x-match = all
- Exchange: Binding to Queue B with arguments (key = value): format = pdf, type = log, x-match = any
- Exchange: Binding to Queue C with arguments (key = value): format = zip, type = report, x-match = all

SCENARIO 1
Message 1 is published to the exchange with header arguments (key = value): "format = pdf", "type = report".

Message 1 is delivered to Queue A because all key/value pairs match, and Queue B since "format = pdf" is a match (binding rule set to "x-match =any").

SCENARIO 2
Message 2 is published to the exchange with header arguments of (key = value): "format = pdf".

Message 2 is only delivered to Queue B. Because the binding of Queue A requires both "format = pdf" and "type = report" while Queue B is configured to match any key-value pair (x-match = any) as long as either "format = pdf" or "type = log" is present.

SCENARIO 3
Message 3 is published to the exchange with header arguments of (key = value): "format = zip", "type = log".

Message 3 is delivered to Queue B since its binding indicates that it accepts messages with the key-value pair "type = log", it doesn't mind that "format = zip" since "x-match = any".

Queue C doesn't receive any of the messages since its binding is configured to match all of the headers ("x-match = all") with "format = zip", "type = pdf". No message in this example lives up to these criterias.

It's worth noting that in a header exchange, the actual order of the key-value pairs in the message is irrelevant.

Example of Headers Exchange. Routes messages to queues that are bound using arguments (key and value) in the amq.headers attribute.

#### Dead Letter Exchange

If no matching queue can be found for the message, the message is silently dropped. RabbitMQ provides an AMQP extension known as the "Dead Letter Exchange", which provides the functionality to capture messages that are not deliverable.

#### [Exchange to Exchange Bindings](https://www.rabbitmq.com/e2e.html)

RabbitMQ  introduced an exchange.bind method which binds one exchange to another exchange. The binding is semantically identical to exchange-to-queue bindings: unidirectional, binding keys and exchange types operate as normal, but both endpoints (the source and destination) of the binding are exchanges. This allows far richer routing topologies to be created. Note the source and destination fields in the exchange.bind method reflect the flow of messages: from the exchange at the source, and to the exchange at the destination.
