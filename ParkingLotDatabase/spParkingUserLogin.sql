USE [ParkingLot]
GO
/****** Object:  StoredProcedure [dbo].[spLogin]    Script Date: 9/22/2020 10:49:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruchika
-- Create date: 14/5/2020
-- Description:	Store procedure for employee Register
-- =============================================
ALTER procedure [dbo].[spLogin]

(

        @EmailID  varchar(50),

        @Password varchar(50),

		@UserRole varchar(50)

)

AS
BEGIN

SET NOCOUNT ON
	DECLARE @Status int
	IF EXISTS(SELECT * FROM ParkingUser WHERE [EmailID] = @EmailID  AND [Password] = @Password AND [UserRole] = @UserRole)
		SET @Status = 1

	ELSE
		SET @Status = 0
	SELECT @Status
END