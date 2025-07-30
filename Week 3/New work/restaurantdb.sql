--Write your SQL code here
--Do not execute your SQL code
--The SQL code in here is just for reference to be used in MySQL program

CREATE TABLE Date (
    day INT, 
    month INT,
    year INT,
    dateString VARCHAR(12) PRIMARY KEY
);

CREATE TABLE Login (
    uID INT PRIMARY KEY,
    Email TEXT,
    Password TEXT
);

CREATE TABLE Person (
    Name TEXT,
    ID VARCHAR(9) PRIMARY KEY,
    Email TEXT,
    Phone TEXT,
    DOB VARCHAR(12),
    FOREIGN KEY (DOB) REFERENCES Date(dateString) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE RestaurantUser (
    Name TEXT,
    pID VARCHAR(9) PRIMARY KEY,
    Email TEXT,
    Phone TEXT,
    Password TEXT,
    rDOB VARCHAR(12),
    FOREIGN KEY (rDOB) REFERENCES Date(dateString) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Customer (
    Name TEXT,
    cID VARCHAR(9) PRIMARY KEY,
    Email TEXT,
    Phone TEXT,
    Reservation TEXT,
    PASSWORD TEXT,
    mDOB VARCHAR(12),
    FOREIGN KEY (mDOB) REFERENCES Date(dateString) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Cashier (
    Name TEXT,
    bID VARCHAR(9) PRIMARY KEY,
    Email TEXT,
    Phone TEXT,
    Salary FLOAT,
    TheStart VARCHAR(6),
    TheEnd VARCHAR(6),
    Position TEXT,
    Office TEXT,
    Task TEXT,
    PASSWORD TEXT,
    kDOB VARCHAR(12),
    FOREIGN KEY (kDOB) REFERENCES Date(dateString) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE RestaurantOrder (
    Food Text,
    Drink Text,
    Price FLOAT,
    orderID INT PRIMARY KEY,
    gOrder VARCHAR(9),
    rOrder VARCHAR(9),
    FOREIGN KEY (gOrder) REFERENCES Customer(cID) ON DELETE SET NULL ON UPDATE CASCADE,
    FOREIGN KEY (rOrder) REFERENCES Casher(bID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE Employee (
    Name TEXT,
    gID VARCHAR(9) PRIMARY KEY,
    Email TEXT,
    Phone TEXT,
    Salary FLOAT,
    TheStart VARCHAR(6),
    TheEnd VARCHAR(6),
    Position TEXT,
    Office TEXT,
    Task TEXT,
    PASSWORD TEXT,
    dateOfBirth VARCHAR(12),
    FOREIGN KEY (dateOfBirth) REFERENCES Date(dateString) ON DELETE SET NULL ON UPDATE CASCADE
);

SELECT * FROM Date;
SELECT * FROM Login;
SELECT * FROM Person;
SELECT * FROM RestaurantUser;
SELECT * FROM Customer;
SELECT * FROM Cashier;
SELECT * FROM RestaurantOrder;
SELECT * FROM Employee;

SELECT * 
FROM Customer CU, RestaurantOrder O, Cashier Ca 
WHERE CU.cID = O.gOrder AND O.rOrder = Ca.bID;

SELECT *
FROM Customer C, RestaurantOrder O 
WHERE C.cID = O.gOrder;

SELECT * 
FROM Casher AS C, RestaurantOrder AS O 
WHERE C.bID = rOrder;