create database StudentCrud;
use StudentCrud;

CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50),
    Email VARCHAR(50),
    MobileNumber VARCHAR(20),
    State VARCHAR(30),
    Gender VARCHAR(10),
    Dob DATE
);

Select * From Students;

-- Get all students
CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT * FROM Students
END
GO

-- Get student by Id
CREATE PROCEDURE sp_GetStudentById
    @Id INT
AS
BEGIN
    SELECT * FROM Students WHERE Id=@Id
END
GO

-- Add student
CREATE PROCEDURE sp_AddStudent
    @Name NVARCHAR(50),
    @Email NVARCHAR(50),
    @MobileNumber NVARCHAR(20),
    @State NVARCHAR(30),
    @Gender NVARCHAR(10),
    @Dob DATE
AS
BEGIN
    INSERT INTO Students (Name, Email, MobileNumber, State, Gender, Dob)
    VALUES (@Name, @Email, @MobileNumber, @State, @Gender, @Dob)
END
GO

-- Update student
CREATE PROCEDURE sp_UpdateStudent
    @Id INT,
    @Name NVARCHAR(50),
    @Email NVARCHAR(50),
    @MobileNumber NVARCHAR(20),
    @State NVARCHAR(30),
    @Gender NVARCHAR(10),
    @Dob DATE
AS
BEGIN
    UPDATE Students SET 
        Name=@Name,
        Email=@Email,
        MobileNumber=@MobileNumber,
        State=@State,
        Gender=@Gender,
        Dob=@Dob
    WHERE Id=@Id
END
GO

-- Delete student
CREATE PROCEDURE sp_DeleteStudent
    @Id INT
AS
BEGIN
    DELETE FROM Students WHERE Id=@Id
END
GO

ALTER TABLE Students ADD CONSTRAINT UQ_Email UNIQUE(Email);
ALTER TABLE Students ADD CONSTRAINT UQ_MobileNumber UNIQUE(MobileNumber);

CREATE PROCEDURE sp_IsEmailExists
    @Email NVARCHAR(50),
    @Id INT = NULL
AS
BEGIN
    IF @Id IS NULL
        SELECT COUNT(1) AS CountExists
        FROM Students
        WHERE Email = @Email;
    ELSE
        SELECT COUNT(1) AS CountExists
        FROM Students
        WHERE Email = @Email AND Id != @Id;
END
GO

CREATE PROCEDURE sp_IsMobileExists
    @MobileNumber NVARCHAR(20),
    @Id INT = NULL
AS
BEGIN
    IF @Id IS NULL
        SELECT COUNT(1) AS CountExists
        FROM Students
        WHERE MobileNumber = @MobileNumber;
    ELSE
        SELECT COUNT(1) AS CountExists
        FROM Students
        WHERE MobileNumber = @MobileNumber AND Id != @Id;
END
GO


