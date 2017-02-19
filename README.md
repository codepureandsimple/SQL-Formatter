# SQL-Formatter
I hate commas in front of columns names. I really hate it.

## Remove Columns and Modify Casing
Will turn this:

`SELECT [InvoiceID]
      ,[CustomerID]
      ,[BillToCustomerID]
      ,[OrderID]
      ,[DeliveryMethodID]
      ,[ContactPersonID]
      ,[AccountsPersonID]
      ,[SalespersonPersonID]
      ,[PackedByPersonID]
      ,[InvoiceDate]
  FROM [Sales].[Invoices]`
  
  ...into this...
  
`select [InvoiceID],
      [CustomerID],
      [BillToCustomerID],
      [OrderID],
      [DeliveryMethodID],
      [ContactPersonID],
      [AccountsPersonID],
      [SalespersonPersonID],
      [PackedByPersonID],
      [InvoiceDate]
  from [Sales].[Invoices]`  

## Future Plans

* Fix identation 
* Fix spacing for function calls, etc
