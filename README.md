# running the project
run the backend server in c# .net, run the frontend project in angular node version 20 or higher

# using deployed version of the project
frontend: https://stock-collection.netlify.app/#/
backend swagger: https://stocksapi-b6u3.onrender.com/swagger

# running backend
set the StocksApi as startup project, 
add user secrets with CONNECTION_STRING for mongodb.
build and run the project

# running frontend
go to the client-angular folder, run npm install command and then run "npm start" / "ng serve" command
there is an option to use configurations like local, development and production which changes the backend server url.

# versions
npm @9.8.1 or higher
node @18.16.0 or higher
angular @16.1.0 or higher