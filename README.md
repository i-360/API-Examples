API-Examples
============
All API communication requires basic HTTP authorization. In place of a username, supply a user id (GUID). Supply no password.
A colon must follow the user id. The user id and colon together must be base 64 encoded. For example, if the user id is 806da805-67f9-4cf7-bace-4829e94dcb2f, then all HTTP communication should include the following in the header:
**Authorization: Basic ODA2ZGE4MDUtNjdmOS00Y2Y3LWJhY2UtNDgyOWU5NGRjYjJmOg==**
The user id supplied must be for a valid user who has been assigned the Generic API role. To get your user id, go to [User Id] (https://api-platform.i-360.com/1.0/SDK/UserId). Contact your account manager if you do not have a valid user name and password or your user account is not granted the Generic API role.

1. To view our API documentation, please visit https://api-platform.i-360.com/1.0/Help
2. To request a user ID, please email API@i-360.com
3. For more information about i360, please visit http://i-360.com
