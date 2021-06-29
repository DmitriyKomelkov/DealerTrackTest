# DealerTrackTest
code challenge

1. This is Asp.Net Core 3.1 Web Api
2. Data base developed by code first approach, for create data base on your pc please edit in appsettings.json ConnectionStrings section DealerTrackDbContext field.
3. I was asked develop only back end part of test, cause I am not React developer and I use Angular for front end apps,  so I tested it by Postman for 
IIS hosted model. There are two endpoints: GET https://localhost:44381/Deal (get all deal records), POST https://localhost:44381/Deal (needs Content-Type: text/csv header
and csv file in body of request) - parse data from file and insert it into data base. If in file exist new Customer, Dealership, or Vehicle they insert in data base too. 
4. I didn't include in app some parts, like full logging system, swagger, and I included unit tests only for parse csv file, cause it is test


