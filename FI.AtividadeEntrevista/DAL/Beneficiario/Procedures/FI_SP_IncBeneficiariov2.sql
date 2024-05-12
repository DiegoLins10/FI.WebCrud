﻿CREATE PROC FI_SP_IncBeneficiarioV2
    @NOME          VARCHAR (MAX) ,
    @CPF           VARCHAR (11) ,
	@IDCLIENTE     BIGINT
AS
BEGIN
	INSERT INTO BENEFICIARIOS (NOME, CPF, IDCLIENTE) 
	VALUES (@NOME, @CPF, @IDCLIENTE)

	SELECT SCOPE_IDENTITY()
END