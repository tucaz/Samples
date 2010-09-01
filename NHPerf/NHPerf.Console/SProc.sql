CREATE PROCEDURE SPS_ALL_Products AS

SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P 

GO

CREATE PROCEDURE SPS_One_Product 
(@ProductID INT)
AS

SELECT P.Id, P.Description, P.Name, P.Price FROM dbo.Product P 
WHERE P.Id = @ProductID;
