-- 1

SELECT *
FROM Accounts
WHERE IsActive = 1
  AND (Currency = 'USD' OR OpenDate >= '2022-01-01')
  AND NOT FilialID = 10;

SELECT AccountNumber, Currency, OpenDate
FROM Accounts
WHERE IsActive = 1
  AND (Currency = 'USD' OR OpenDate >= '2022-01-01')
  AND NOT FilialID = 10;

-- 2

-- Date filter
SELECT *
FROM Customers
WHERE BirthDate < '1990-01-01';

-- Text filter
SELECT *
FROM Customers
WHERE CustomerFirstName = 'Jacob';

-- Number filter
SELECT *
FROM Accounts
WHERE FilialID = 5;

-- Combination
SELECT *
FROM Accounts
WHERE IsActive = 1
  AND Currency = 'USD'
  AND OpenDate BETWEEN '2016-01-01' AND '2016-01-31';

-- 3

-- IN
SELECT *
FROM Accounts
WHERE Currency IN ('USD', 'GPB');

-- BETWEEN
SELECT *
FROM Deposits
WHERE StartDate BETWEEN '2018-01-01' AND '2018-02-10';

-- 4

SELECT *
FROM Customers
WHERE CustomerID IN (
    SELECT CustomerID
    FROM Deposits
);

-- 5

SELECT DISTINCT Currency
FROM Accounts;

-- DISTINCT on multiple colunms

SELECT DISTINCT Currency, AccountTypeID
FROM Accounts; -- takes different pairs

-- 6

-- Firstname tarts with "A"
SELECT *
FROM Customers
WHERE CustomerFirstName LIKE 'A%';

-- Lastname ends with "son"
SELECT *
FROM Customers
WHERE CustomerLastName LIKE '%son';

-- Second letter is "i"
SELECT *
FROM Customers
WHERE CustomerFirstName LIKE '_i%';

-- Name starts with A or B
SELECT *
FROM Customers
WHERE CustomerFirstName LIKE '[AB]%';

-- 7

-- v1
SELECT *
FROM Accounts
WHERE Currency = 'USD' OR Currency = 'EUR';

SELECT *
FROM Accounts
WHERE Currency IN ('USD', 'EUR');

-- v2
SELECT *
FROM Accounts
WHERE NOT IsActive = 0;

SELECT *
FROM Accounts
WHERE IsActive = 1;