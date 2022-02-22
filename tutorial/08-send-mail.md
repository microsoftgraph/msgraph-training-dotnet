---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will add the ability to send an email message as the authenticated user.

## Get user details

1. Open **./Graph/GraphHelper.cs** and add the following function to the **GraphHelper** class.

    :::code language="csharp" source="../demo/GraphTutorial/GraphHelper.cs" id="SendMailSnippet":::

1. Replace the empty `SendMailAsync` function in **Program.cs** with the following.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="SendMailSnippet":::

1. Run the app, sign in, and choose option 3 to send an email to yourself.

```Shell
Please choose one of the following options:
0. Exit
1. Display access token
2. List my inbox
3. Send mail
4. List users (requires app-only)
5. Make a Graph call
3
Mail sent.
```

## Code explained

Consider the code in the `SendMailAsync` function.

### Sending mail

The function uses the `_userClient.Me.SendMail` request builder, which builds a request to the [Send mail](https://docs.microsoft.com/graph/api/user-sendmail) API. The request builder takes a `Message` object representing the message to send.

### Creating objects

Unlike the previous calls to Microsoft Graph that only read data, this call creates data. To do this with the client library you create an instance of the class representing the data (in this case, `Microsoft.Graph.Message`) using the `new` keyword, set the desired properties, then send it in the API call. Because the call is sending data, the `PostAsync` method is used instead of `GetAsync`.
