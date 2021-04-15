# SSE-tests
Figuring out how SSE client/server things work

## To Test SSE

Start up project and browse to Home. Open a browser for the server and a browser per client. Follow the "instructions" on the home page

## To test reporting plugins

Use postman or similar to make api calls:
http://<url:port>/reporting/download/{pluginName}?[<parameterName>=<parameterValue>]

Possible Plugin Names:
1. Excel ( you may need to update the file path to the testexcel.xlsx file)
2. Json

Excel report has no parameters.

Json report has two parameters:
* p
* p2