USE [ParkingLot]
GO
/****** Object:  StoredProcedure [dbo].[spSpecificParkingUser]    Script Date: 9/25/2020 6:44:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ruchika
-- Create date: 14/5/2020
-- Description:	Store procedure for get specific User details
-- =============================================CREATE PROCEDURE SP_EmpLoyee_Update
ALTER PROCEDURE [dbo].[spSpecificParkingUser]
@ID int
AS 
BEGIN
    Select * from ParkingUser
    WHERE ID = @ID
END
