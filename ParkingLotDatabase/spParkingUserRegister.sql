USE [ParkingLot]
GO
/****** Object:  StoredProcedure [dbo].[spParkingUserRegister]    Script Date: 9/22/2020 10:51:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruchika
-- Create date: 21/9/2020
-- Description:	Store procedure for Parking User Register
-- =============================================
ALTER PROCEDURE [dbo].[spParkingUserRegister]
@FirstName varchar(50),
@LastName varchar(50),
@EmailID varchar(50),
@Password varchar(50),
@UserRole varchar(50),
@CreateDate varchar(50)
AS
BEGIN
	SET NOCOUNT ON; 

	INSERT INTO ParkingUser(FirstName,LastName,EmailID,Password,UserRole,CreateDate)
		VALUES(@FirstName,@LastName,@EmailID,@Password,@UserRole,@CreateDate);
END
