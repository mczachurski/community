# Community
## About
Community is the system very similar to Twitter. His main feature is sharing short messages. However except that introduces a lot of features which significantly distinguish Community. Application source codes are available to the public and anyone can contribute to its development. The system does not display ads and there are no algorithms to affect (in an artificial way) users timelines.

**Planned start date: September 1, 2015.**

![Site](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/screenshot01.png)

## The application built by the community
The basic premise of the project is that the service will be built by the community. For this reason it is very important for us to get immediate feedback from users. At this time, it is possible by:

- GitHub issue tracker: [https://github.com/mczachurski/community-web/issues](https://github.com/mczachurski/community-web/issues)
- Voting platform: uservoice.com (soon we give an address).

**Together we can build the best platform to share of information.**

## Features
The following describes the basic features of the Community:

1. Sharing short statuses.

  User can add short messages called *Minds*. There are messages up to 200 characters (with possibility to include pictures). Messages are shown to people who are observing the user.
  
  ![Minds](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/screenshot02.png)

2. Creating longer texts.

  User can add articles called *Speeches*. They can be created at *markdown* format and operates like normal short *Minds*. Thus users can transmit, quote, comment them etc.
  
  ![Speech](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/screenshot03.png)

3. Transmit.

  All messages can be transmitted by user to users who observe him. It’s very similar to Twitters *retweets*.
  
4. Comments.

  User can add to all messages in the system his own comments. Comments work totally different than in Twitter. They are similar to comments in Google+. All messages are displayed directly under the original message.
  
  ![Speech](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/screenshot04.png)
  
5. Quotes.

  All messages in the system can be quoted. Quotes are similar like in Twitter but user can’t modify original message. Additionally user has 200 chars to write his message (amount of chars from original message do not take into account).
  
  ![Speech](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/screenshot05.png)
  
6. Favorites.

  User has possibility to like every message in the system. Messages like that are displayed in separate page. Additionally amount of favorites is important to determinate very popular *Minds*.
  
7. Automatic display messages from a category.

  User on his profile can add categories and amount of favorites to each category. Thanks to this, messages from observed category can automatically display on user timeline (after achieving proper amount of favorites). There are also messages, which aren’t added by observed users. **This feature is not implemented yet.**
  
8. Mentions.

  Mentions are very similar to those familiar from Twitter. So user can mention every other user from the system. Mentioned user can see these messages on his *Mentions* page.
  
9. Direct messages.

  When at the beginning of the message user mentions other user, this message will appear only in mentioned user timeline. **This feature is not implemented yet.**
  
10. Bubbles.

  By default the system displays new messages at the top of timeline. Adding new comments to messages Community doesn’t move messages to the top of timeline. User can change this behavior for every single message. Thus we can enable bubbling for messages which we want to track (by single click). From this moment message will be moving at the top of timeline after adding comments.
  
11. RSS integration.

  User on his profile can add RSS channels to observe. The system will be track RSS channel and when new article appears system will add new message to the user timeline. Message like that operates like normal *Mind* message. So user can transmit, comment, quote etc. **This feature is not implemented yet.**
  
12. Private messages.

  User can send private messages to user which observes him. Message like that will be visible only for these two users. **This feature is not implemented yet.**
  
13. API.

  The system provides an API. Thanks to this it is possible to create native client application (for iOS, Android, Windows etc). To exclude the abuse, the application must be added to the list of allowed applications (will receive his token access). There will be no restrictions of how many users can use the application (and of course it does not have to be free). **This feature is not implemented yet.**
  
14. and many others…

## Architecture
To ensure the best performance and scalability the system is written with a view to Azure. Also, the test version is hosted in Azure. The following describes the target system architecture. Some items are not implemented yet.

![Architecture](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/architecture.jpg)

## Environments
Number of environments that we have is closely related to the way of work which was adopted in the project. Developers work is organized by gitflow shown below.

![Gitflow](https://raw.githubusercontent.com/mczachurski/community-web/develop/Documents/gitflow.jpg)

List of environments are listed below. Each environment is associated with a respective git branch. Additionally every environment is connected with CI server. Thanks to this we have compilation and unit test status for each branch. Project uses [AppVeyour](http://appveyor.com) CI servers.

Environment | Branch | Status | Address
:-: | :-: | :-: | :-:
Production | master | [![Build status](https://ci.appveyor.com/api/projects/status/hhpnbbeh9sfwgnrj/branch/master?svg=true)](https://ci.appveyor.com/project/marcinczachurski/community-web/branch/master) | unknown
Test | release | - | unknown
Develop | develop | [![Build status](https://ci.appveyor.com/api/projects/status/hhpnbbeh9sfwgnrj/branch/develop?svg=true)](https://ci.appveyor.com/project/marcinczachurski/community-web/branch/develop) | [minds.azurewebsites.net](minds.azurewebsites.net)

All of the environments (except production of course) can be cleaned periodically with the data (including user accounts). If you would like to contribute, please fork and create a pull request on Github. Any help is important to us.

## Developing
To start the system development in the local environment, we must follow a few steps:

- clone the repository (branch develop or own fork)
- create a local database (best MsSQL) 
- create an account on the Bit.ly site (you will need a username and a private key)
- create a configuration file in a web project. These files should not be added to Git (by default they are ignored in .gitignore file). Thanks to this developer can store private data only on own local machine. These files are:
    - PrivateConnectionStrings.config
    
            <?xml version="1.0" encoding="utf-8"?>
            <connectionStrings>
                <add name="DefaultConnection" connectionString="Data Source=localhost;Initial Catalog=DATABASE;Persist Security Info=True;User ID=USER;Password=PASSWORD" providerName="System.Data.SqlClient" />
            </connectionStrings>

    
    - PrivateAppSettings.config
    
            <?xml version="1.0" encoding="utf-8"?>
            <appSettings>
                <add key="BitlyUserName" value="LOGIN" />
                <add key="BitlyPrivateKey" value="PRIVATEKEY" />
            </appSettings>


        They should be placed in the folder where the Web.config file is located.


- create a configuration file for unit tests. Like the web project, also unit tests contain configuration file with connection string to the database. It is best if it’s a different database than the one hooked up to the web, because every time you run tests they recreate the database. File *PrivateConnectionStrings.config* (having the same schema as described above) should be placed in a folder: `SunLine.Community.NUnitTest/bin/Debug/`
- build solution in VS 2013 and run the application
- first run will create database schema and create the first data in the database (mainly dictionary data)
- than we can change settings in *Settings* table (email address, mail server etc.)
- now you can register new account in web application (opened by Visual Studio). Email with confirmation of registration should be sent to address provided during registration. If mail server is not configured, you can change manually information about email confirmation in the database (table *Users*, column *EmailConfirmation*).

## License
For now we are using one component that is not free. So if you want to host Community on your own servers you must buy [Xenon](http://themes.laborator.co/xenon/) theme. In the future we will change Xenon for our own theme.
