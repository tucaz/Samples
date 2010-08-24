SELECT * FROM TDC2010.dbo.Product
SELECT * FROM TDC2010.dbo.ProductCategory
SELECT * FROM TDC2010.dbo.ProductCategoryToProduct
SELECT * FROM TDC2010.dbo.[User]

--Copy data sample from AdventureWorks
INSERT INTO TDC2010.dbo.Product
SELECT Name, Name as Description, StandardCost FROM AdventureWorks.Production.Product

INSERT INTO TDC2010.dbo.ProductCategory
SELECT Name, Name as Description FROM AdventureWorks.Production.ProductCategory

INSERT INTO TDC2010.dbo.[User]
SELECT FirstName + ' ' + LastName as Name, EmailAddress FROM AdventureWorks.Person.Contact

--Do some weird stuff to populate N-M table
DECLARE @CategoryId INT

DECLARE @TotalRows INT
SET @TotalRows = (SELECT COUNT(*) AS TOTAL FROM TDC2010.dbo.Product)

DECLARE @count INT
SET @count = 1

WHILE @count <= @TotalRows
BEGIN	
	IF @count % 2 = 0
		BEGIN
			SET @CategoryID = 1
		END
	ELSE 
		BEGIN
			IF @count % 3 = 0
				BEGIN
					SET @CategoryID = 2
				END
			ELSE
				BEGIN
					SET @CategoryID = 3
				END
		END
	
	INSERT INTO TDC2010.dbo.ProductCategoryToProduct VALUES (@count, @CategoryId)
	SET @count = @count + 1
END
