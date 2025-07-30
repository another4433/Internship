--Use for referencing only

CREATE TABLE Customer (
    Name TEXT,
    ID VARCHAR(9) PRIMARY KEY,
    DOB DATE,
    Email TEXT,
    Phone TEXT
);

CREATE TABLE RestaurantUser (
    pID INT PRIMARY KEY,
    Password TEXT,
    customerID VARCHAR(9),
    FOREIGN KEY (customerID) REFERENCES Customer(ID) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Item (
    qID INT PRIMARY KEY,
    Name TEXT,
    Category TEXT
);

CREATE TABLE RestaurantOrder (
    orderID INT PRIMARY KEY,
    Quantity INT,
    userID INT,
    FOREIGN KEY (userID) REFERENCES RestaurantUser(pID) ON DELETE SET DEFAULT ON UPDATE CASCADE
);

CREATE TABLE ItemMaster (
    masterID INT PRIMARY KEY,
    childID INT,
    toOrder INT,
    FOREIGN KEY (childID) REFERENCES Item(qID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (toOrder) REFERENCES RestaurantOrder(orderID) ON DELETE SET DEFAULT ON UPDATE CASCADE
);

CREATE TABLE OrderLine (
    userID INT,
    toOrder INT,
    historyDate DATE,
    FOREIGN KEY (userID) REFERENCES RestaurantUser(pID) ON DELETE SET DEFAULT ON UPDATE CASCADE,
    FOREIGN KEY (toOrder) REFERENCES RestaurantOrder(orderID) ON DELETE CASCADE ON UPDATE CASCADE
);

SELECT * FROM Customer;
SELECT * FROM RestaurantUser;
SELECT * FROM Item;
SELECT * FROM ItemMaster;
SELECT * FROM RestaurantOrder;
SELECT * FROM OrderLine;

SELECT * 
FROM Item I, ItemMaster M, RestaurantOrder O 
WHERE I.qID = M.childID AND M.toOrder = O.orderID;

SELECT *
FROM Customer AS C, RestaurantUser AS U 
WHERE C.ID = U.customerID;

SELECT *
FROM RestaurantUser U, OrderLine L, RestaurantOrder O 
WHERE U.pID = L.userID AND L.toOrder = O.orderID;

SELECT * 
FROM RestaurantOrder AS O, OrderLine AS L 
WHERE O.orderID = L.toOrder;

SELECT Name FROM Item WHERE Name LIKE 'chicken' AND Category LIKE 'fast food';