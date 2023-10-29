<h1>Azure Service Bus: Powering Scalability and Reusability in Modern Cloud Solutions</h1>
<br>
In the evolving realm of cloud-native apps, scalable and reusable components are crucial. Azure Service Bus, a fully managed message broker from Microsoft Azure, excels at connecting disparate services like Azure Functions. Leveraging the Azure Service Bus with Azure Functions enhances scalability and decoupling allowing each function to scale based on its own demand. For instance, instead of integrating reCAPTCHA verification in a variety of functions that may require such functionality, centralise it in a single Azure Function for standardized, reduced redundancy.

In this repository:

- **FNReCaptchaVerification Azure Function**: Processes contact form data and reCAPTCHA validation, then forwards to Azure Service Bus.
- **FNQueueFormSubmission Azure Function**: Manages Azure Service Bus queue messages.
- **FNMailSender Azure Function**: Listens to the processed messages in the service bus queue and utilizes SendGrid to dispatch the contact form message to the business email.
- **DatabseSaverWebApi (eexternal web API server hosted on a raspberry pi)**: After successful email send via the SendGrid service, this API extneral web API is called & various form data is saved to an Azure hosted SQL database     

Dive deep into TDD practices with Azure Functions and Service Bus, emphasizing the benefits of modular cloud design. Ideal for enterprises refining their operations and developers prioritizing efficient, maintainable code.

---

Stay tuned for a comprehensive blog post that will delve deep into the Azure services setup and our unique code implementation strategies. Expect it to be published soon!
