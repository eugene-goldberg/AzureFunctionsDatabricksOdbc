This is a test Azure Function project, which aims to achieve the following goals:

1   Implement a C# .NET "Serverless" solution, which is scaled through concurrent invocation of
    multiple Activity Functions (such as InsertData and GetData)
2   Implement an ODBC client activity, which is initiated from an Activity Function and is
    accessing a remote Azure Databricks Sql Warehouse resource
3   Verify a deployment of an Azure Functions app on top of a custom Docker container, 
    which includes Simba Spark ODBC Driver required for accessing Databricks Sql Warehouse
4   Confirm an assumption that having a custom Docker container will not be preventing
    this solution from implementing a fan-out / fan-in pattern and being highly scalable
5   Connfirm an assumption that every Activity Function will be given its own set of resources
    such as CPU and RAM, therefore being able to perform concurrent operations 
    without having to wait ano another function instance to finish or to pause first.

