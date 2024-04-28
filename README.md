# GestorTask

## CONFIGURACION BASE DE DATOS  CONEXION (Crear la base datos)

Se utilizo Oracle Sql Developer para la interfaz de la base de datos.
Se utilizo la imagen doctorkirk/oracle-19c en docker para la base de datos de oracle.

Se corrio este comando en el cmd window para configurar y crear contenedor en docker.
docker run --name oracle-19c -p 1521:1521 -e ORACLE_SID=orlc -e ORACLE_PWD=Jon@dt06 -e ORACLE_MEN=1500 doctorkirk/oracle-19c

Se creo la base de datos y la conexion con los siguientes parametros.

1. NOMBRE: GestorTask
2. USERNAME: jonathan
3. PASSWORD: jonadt
4. HOSTNAME: localHost
5. ROLE: default
6. CONNECTIONTYPE: basic
7. PORT: 1521
8. SID: orlc

## Este es el script de la base de datos para generar las tablas... el archivo script esta en el repositorio.

--------------------------------------------------------
--  File created - Saturday-April-27-2024   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Sequence TASK_SEQ
--------------------------------------------------------

   CREATE SEQUENCE  "JONATHAN"."TASK_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 7 NOCACHE  NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Sequence USER_SEQ
--------------------------------------------------------

   CREATE SEQUENCE  "JONATHAN"."USER_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 4 NOCACHE  NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Table TASK
--------------------------------------------------------

  CREATE TABLE "JONATHAN"."TASK" 
   (	"ID" NUMBER(*,0), 
	"NAME" VARCHAR2(50 BYTE), 
	"DESCRIPTION" VARCHAR2(150 BYTE), 
	"DATE_INIT" DATE, 
	"DATE_END" DATE, 
	"DATE_REGISTER" DATE, 
	"REGISTER_BY" NUMBER(*,0), 
	"STATUS" CHAR(1 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table USERS
--------------------------------------------------------

  CREATE TABLE "JONATHAN"."USERS" 
   (	"ID" NUMBER(*,0), 
	"USERNAME" VARCHAR2(25 BYTE), 
	"PASSWORD" VARCHAR2(50 BYTE), 
	"SALT" VARCHAR2(50 BYTE), 
	"DATE_REGISTER" DATE, 
	"STATUS" CHAR(1 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;

--------------------------------------------------------
--  DDL for Index PK_TASK
--------------------------------------------------------

  CREATE UNIQUE INDEX "JONATHAN"."PK_TASK" ON "JONATHAN"."TASK" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index PK_USER
--------------------------------------------------------

  CREATE UNIQUE INDEX "JONATHAN"."PK_USER" ON "JONATHAN"."USERS" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Trigger AL_INSERTAR_TAREA
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "JONATHAN"."AL_INSERTAR_TAREA" before insert on task for each row
begin
    select task_seq.nextval into :new.id from dual;
end;
/
ALTER TRIGGER "JONATHAN"."AL_INSERTAR_TAREA" ENABLE;
--------------------------------------------------------
--  DDL for Trigger AL_INSERTAR_USUARIO
--------------------------------------------------------

  CREATE OR REPLACE EDITIONABLE TRIGGER "JONATHAN"."AL_INSERTAR_USUARIO" before insert on users for each row
begin
    select user_seq.nextval into :new.id from dual;
end;
/
ALTER TRIGGER "JONATHAN"."AL_INSERTAR_USUARIO" ENABLE;
--------------------------------------------------------
--  Constraints for Table USERS
--------------------------------------------------------

  ALTER TABLE "JONATHAN"."USERS" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" MODIFY ("USERNAME" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" MODIFY ("PASSWORD" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" MODIFY ("SALT" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" MODIFY ("DATE_REGISTER" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" MODIFY ("STATUS" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."USERS" ADD CONSTRAINT "PK_USER" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table TASK
--------------------------------------------------------

  ALTER TABLE "JONATHAN"."TASK" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" MODIFY ("NAME" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" MODIFY ("DESCRIPTION" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" MODIFY ("DATE_REGISTER" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" MODIFY ("REGISTER_BY" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" MODIFY ("STATUS" NOT NULL ENABLE);
  ALTER TABLE "JONATHAN"."TASK" ADD CONSTRAINT "PK_TASK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS"  ENABLE;
