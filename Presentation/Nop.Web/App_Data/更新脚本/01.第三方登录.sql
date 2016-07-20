if  not exists (select Name From SysColumns Where ID = OBJECT_ID('Customer') And Name = 'RegisterSourceId')
--向客户表注册来源字段
ALTER TABLE Customer ADD  RegisterSourceId int default(0) not null;
GO

--插入第三方登录的默认密码
INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Value]
           ,[StoreId])
     VALUES
           ('customersettings.thirdpartydefaultpassword','12345678',0)
GO