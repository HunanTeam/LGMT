if  not exists (select Name From SysColumns Where ID = OBJECT_ID('PhoneVerificationCode') And Name = 'VerificationType')
--发送验证码的表扩展及相关配置
ALTER TABLE PhoneVerificationCode ADD  VerificationType int default(0) not null;
GO

-- 验证码有效时间（秒钟）
INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Value]
           ,[StoreId])
     VALUES
           ('PhoneVerificationSettings.EffectiveTime','180',0)
GO
--重复获取验证码间隔时间（秒钟）
INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Value]
           ,[StoreId])
     VALUES
           ('PhoneVerificationSettings.AgainGetSpacingTime','60',0)