-- 1

SELECT CustomerID, CustomerFirstName, CustomerLastName
FROM dbo.Customers
WHERE CustomerID IN (
    SELECT CustomerID
    FROM loan.Loans
)
AND CustomerID IN (
    SELECT CustomerID
    FROM dbo.Deposits
);

-- 2

-- INNER JOIN
SELECT l.CustomerID, l.LoanID, d.DepositID
FROM loan.Loans l
INNER JOIN dbo.Deposits d
  ON l.CustomerID = d.CustomerID;
-- only customers who have both a loan and a deposit 
-- records multiply if a customer has multiple loans or deposits

-- LEFT JOIN
SELECT l.CustomerID, l.LoanID, d.DepositID
FROM loan.Loans l
LEFT JOIN dbo.Deposits d
  ON l.CustomerID = d.CustomerID;
-- all loans are shown
-- if a customer doesn’t have a deposit, DepositID will be NULL
-- useful to see loans even without deposits

-- RIGHT JOIN
SELECT d.CustomerID, l.LoanID, d.DepositID
FROM loan.Loans l
RIGHT JOIN dbo.Deposits d
  ON l.CustomerID = d.CustomerID;
-- all deposits are shown
-- if a customer doesn’t have a loan, LoanID will be NULL
-- useful to see deposits even without loans

-- FULL OUTER JOIN
SELECT l.CustomerID, l.LoanID, d.DepositID
FROM loan.Loans l
FULL OUTER JOIN dbo.Deposits d
  ON l.CustomerID = d.CustomerID;
-- if a loan eixsts without a deposit d.CustomerID is NULL
-- if a deposit exists without a loan l.CustomerID iS NULL
-- so here any deposit-only row will show NULL for CustomerID

-- 3

SELECT 
    c.CustomerFirstName,
    c.CustomerLastName,
    l.LoanID,
    la.AccountID,
    a.AccountNumber,
    p.ProductName
FROM loan.Loans l
JOIN loan.LoanAccounts la ON l.LoanID = la.LoanID
JOIN dbo.Accounts a ON la.AccountID = a.AccountID
JOIN dbo.Customers c ON l.CustomerID = c.CustomerID
JOIN loan.Products p ON l.ProductID = p.ProductID;

-- 4

SELECT 
    l.LoanID, l.Amount AS LoanAmount,
    d.DepositID, d.Amount AS DepositAmount
FROM loan.Loans l
JOIN dbo.Deposits d
  ON l.CustomerID = d.CustomerID
  AND d.Amount < l.Amount;
-- the join multiplies rows whenever the deposit is smaller than the loan
-- customers with multiple deposits smaller than their loan produce multiple result rows.

-- 5

ALTER TABLE dbo.Accounts
ADD CONSTRAINT FK_Accounts_Customers
FOREIGN KEY (CustomerID)
REFERENCES dbo.Customers(CustomerID);

ALTER TABLE dbo.Accounts
ADD CONSTRAINT FK_Accounts_AccountStatusTypes
FOREIGN KEY (AccountStatusTypeID)
REFERENCES dbo.AccountStatusTypes(AccountStatusTypeID);

ALTER TABLE dbo.Deposits
ADD CONSTRAINT FK_Deposits_Customers
FOREIGN KEY (CustomerID)
REFERENCES dbo.Customers(CustomerID);

ALTER TABLE loan.Loans
ADD CONSTRAINT FK_Loans_Products
FOREIGN KEY (ProductID)
REFERENCES loan.Products(ProductID);

ALTER TABLE loan.Loans
ADD CONSTRAINT FK_Loans_Customers
FOREIGN KEY (CustomerID)
REFERENCES dbo.Customers(CustomerID);

ALTER TABLE loan.LoanAccounts
ADD CONSTRAINT FK_LoanAccounts_Loans
FOREIGN KEY (LoanID)
REFERENCES loan.Loans(LoanID);

ALTER TABLE loan.LoanAccounts
ADD CONSTRAINT FK_LoanAccounts_Accounts
FOREIGN KEY (AccountID)
REFERENCES dbo.Accounts(AccountID);

ALTER TABLE dbo.OverDrafts
ADD CONSTRAINT FK_OverDrafts_Accounts
FOREIGN KEY (AccountID)
REFERENCES dbo.Accounts(AccountID);

ALTER TABLE dbo.Transactions
ADD CONSTRAINT FK_Transactions_DebitAccount
FOREIGN KEY (DebitAccountID)
REFERENCES dbo.Accounts(AccountID);

ALTER TABLE dbo.Transactions
ADD CONSTRAINT FK_Transactions_CreditAccount
FOREIGN KEY (CreditAccountID)
REFERENCES dbo.Accounts(AccountID);

-- 6

-- Customers >> Accounts თითოეულ მომხმარებელს შეიძლება ჰქონდეს რამდენიმე ექაუნთი, თითოეულო ექაუნთი ეკუთვნის ერთ მომხმარებელს
-- Customers >> Loans ერთ მომხმარებელს შეიძლება ჰქონდეს რამდენიმე სესხი. თითოეული სესხი ეკუთვნის ერთ მომხმარებელს
-- Loans >> LoanAccouns თითოეული LoanAccounts უკავშირდება მხოლოდ ერთ სესხს 
-- Customer >> Deposits თითოეული მომხმ. შეიძლება კავშირდებოდეს რამდენიმე დეპოზიტთან. თით დეპოზიტი დაკავშირებულია 1 მომხმ.
-- Accounts >> Overdrafts ერთ ექაუნთს შეიძლება ჰქონდეს რამდენიმე ოვერდრაფტი. ერთი ოვერდრაფტი დაკავშირებულია 1 ექაუნთტან.
-- Transactions >> Accounts ტრანზაქცია დაკავრებულია დებიტ და კრედიტ ექაუნთთან. თითო ტრანზაქცია დაკავშირებულია ორ(debit-credit) ექაუნთთან
-- TransactionTypes >> Transactions ერთ ტიპი შეიძ₾ება დაკავშირებული იყოს რამდენიმე ტრანზაქციასთან. ტრანზაქცია მხოლოდ ერთ ტიპთან

-- 7

SELECT 
    t.TransactionID,
    t.Amount,
    t.TransactionDate,
    dc.CustomerFirstName AS DebitFirstName,
    dc.CustomerLastName AS DebitLastName,
    cc.CustomerFirstName AS CreditFirstName,
    cc.CustomerLastName AS CreditLastName
FROM dbo.Transactions t
JOIN dbo.Accounts da ON t.DebitAccountID = da.AccountID
JOIN dbo.Customers dc ON da.CustomerID = dc.CustomerID
JOIN dbo.Accounts ca ON t.CreditAccountID = ca.AccountID
JOIN dbo.Customers cc ON ca.CustomerID = cc.CustomerID;

-- 8

SELECT * FROM dbo.Deposits db
WHERE db.CustomerID = 115; -- 2 rows

SELECT * FROM loan.Loans l
WHERE l.CustomerID = 115; -- 9 rows

SELECT * FROM loan.Loans l
JOIN dbo.Deposits d ON l.CustomerID = d.CustomerID
WHERE l.CustomerID = 115; -- must print 18 rows
-- bcs cust 115 may have multiple loans and multiple deposits
-- INNER JOIN multiplies every loan with every deposit -> 9 * 2 = 18

-- 9

SELECT CustomerID, CustomerFirstName, CustomerLastName
FROM dbo.Customers
WHERE CustomerID IN (
    SELECT CustomerID
    FROM loan.Loans
)
AND CustomerID NOT IN (
    SELECT CustomerID
    FROM dbo.Deposits
)
UNION
SELECT CustomerID, CustomerFirstName, CustomerLastName
FROM dbo.Customers
WHERE CustomerID IN (
    SELECT CustomerID
    FROM dbo.Deposits
)
AND CustomerID NOT IN (
    SELECT CustomerID
    FROM loan.Loans
);