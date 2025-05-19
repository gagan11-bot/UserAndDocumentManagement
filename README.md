User and document management project-
> Invoke APIs in postman with bearer token generate, set token validity for 3 hours..

First started with Auth Controller-
> Add new user with Register Api
> After adding new user Login to that user, if suppose it's admin it will give
  the bearer token when login successfully.
> Copy paste the token to postman to authenticate the use and user other apis
> After all apis done then do the Logout api

Admin Controller-
> Invoke Dashboard api where see the welcome message after login.
> Invoke GetAllUsers Api to see the list of users
> Assing role to user by calling the AssignRole api
> Remove role from user by invoking RemoveRole Api
> View or get all roles by GetUserRoles Api
> If want to delete user then call DeleteUser Api

Document Controller--
> To add new documents invoke Upload Api
> To view list of documents added call GetAll Api
> Only get specific document by id use GetByID Api
> If want to update the existing document, use Update api
> To remove a document call Delete Api

Ingestion Controller--
> To add ingestion, invoke TriggerIngestion Api after added document

Ingestion Management--
> After trigger the ingestion check the status by GetStatus Api
> To cancel the ingestion use CancelIngestion Api
> To view or check the history of Ingestion, call GetIngestionHistory Api

Models or tables for above APIS--
> ApplicationUsers
> IngestionRequestModels
> LoginModels
> RegisterModels
> RoleUpdateModels
> Documents
> IngestionStatuses

Run the migration to add these above mentioned tables to Sql server

For Upload Document API created seprate DTO model to handle upload file

After all Above code pushed code in Git hub
------------------------
> TESTING THROUGH POSTMAN

To Register New User
> Method - Post (Method Type)
https://localhost:7170/api/Auth/register (Url to hit such as localhost, Controller name
and API name)
Body > Raw > JSON
{
  "email": "admin1@gmail.com",
  "password": "Admin123$%^",
  "role": "admin"
}

----------------
Login API
> Method - Post
https://localhost:7170/api/Auth/login
Body > Raw > JSON
{
  "email": "admin1@gmail.com",
  "password": "Admin123$%^"
}
---------------
Upload Document
> Method - Post
https://localhost:7170/api/documents/upload
Authorization > Type > Bearer Token > TokenID
Body > Form-Data
          Value                                           |      Content Type
File >  FileName   (Select File from drop down list)      |     File
Title >  TitleName                                        |     text/plain

--------------
Get Ingestion History API
> Method - Get
https://localhost:7170/api/IngestionManagement/history/1
Authorization > Type > Bearer Token > TokenID
--------------

