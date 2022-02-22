---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will add the ability to list messages in the user's email inbox.

## Get user details

1. Open **./Graph/GraphHelper.cs** and add the following function to the **GraphHelper** class.

    :::code language="csharp" source="../demo/GraphTutorial/Graph/GraphHelper.cs" id="GetInboxSnippet":::

1. Replace the empty `ListInboxAsync` function in **Program.cs** with the following.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="ListInboxSnippet":::

1. Run the app, sign in, and choose option 2 to list your inbox.

```Shell
Please choose one of the following options:
0. Exit
1. Display access token
2. List my inbox
3. Send mail
4. List users (requires app-only)
2
Message: Updates from Ask HR and other communities
  From: Contoso Demo on Yammer
  Status: Read
  Received: 12/30/2021 4:54:54 AM -05:00
Message: Employee Initiative Thoughts
  From: Patti Fernandez
  Status: Read
  Received: 12/28/2021 5:01:10 PM -05:00
Message: Voice Mail (11 seconds)
  From: Alex Wilber
  Status: Unread
  Received: 12/28/2021 5:00:46 PM -05:00
Message: Our Spring Blog Update
  From: Alex Wilber
  Status: Unread
  Received: 12/28/2021 4:49:46 PM -05:00
Message: Atlanta Flight Reservation
  From: Alex Wilber
  Status: Unread
  Received: 12/28/2021 4:35:42 PM -05:00
Message: Atlanta Trip Itinerary - down time
  From: Alex Wilber
  Status: Unread
  Received: 12/28/2021 4:22:04 PM -05:00

...

More messages available? True
```

## Code explained

Consider the code in the `GetInboxAsync` function.

### Accessing well-known mail folders

The function uses the `_userClient.Me.MailFolders["Inbox"].Messages` request builder, which builds a request to the [List messages](https://docs.microsoft.com/graph/api/user-list-messages) API. Because it includes the `MailFolders["Inbox"]` request builder, the API will only return messages in the requested mail folder. In this case, because the inbox is a default, well-known folder inside a user's mailbox, it's accessible via its well-known name. Non-default folders are accessed the same way, by replacing the well-known name with the mail folder's ID property. For details on the available well-known folder names, see [mailFolder resource type](https://docs.microsoft.com/graph/api/resources/mailfolder).

### Accessing a collection

Unlike the `GetUserAsync` function from the previous section, which returns a single object, this method returns a collection of messages. Most APIs in Microsoft Graph that return a collection do not return all available results in a single response. Instead, they use [paging](https://docs.microsoft.com/graph/paging) to return a portion of the results while providing a method for clients to request the next "page".

#### Default page sizes

APIs that use paging implement a default page size. For messages, the default value is 10. Clients can request more (or less) by using the [$top](https://docs.microsoft.com/graph/query-parameters#top-parameter) query parameter. In `GetInboxAsync`, this is accomplished with the `.Top(25)` method.

> [!NOTE]
> The value passed to `.Top()` is an upper-bound, not an explicit number. The API returns a number of messages *up to* the specified value.

#### Getting subsequent pages

If there are more results available on the server, collection responses include an `@odata.nextLink` property with an API URL to access the next page. The .NET client library exposes this as the `NextPageRequest` property on collection page objects. If this property is non-null, there are more results available.

The `NextPageRequest` property exposes a `GetAsync` method which returns the next page.

### Sorting collections

The function uses the `OrderBy` method on the request to request results sorted by the time the message is received (`ReceivedDateTime` property). It includes the `DESC` keyword so that messages received more recently are listed first. This adds the [$orderby query parameter](https://docs.microsoft.com/graph/query-parameters#orderby-parameter) to the API call.
