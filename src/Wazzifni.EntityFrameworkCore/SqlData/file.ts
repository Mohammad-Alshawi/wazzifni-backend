USE [WazzifniDb]
GO
SET IDENTITY_INSERT [dbo].[SpokenLanguages] ON 
GO
INSERT [dbo].[SpokenLanguages] ([Id], [Name], [DisplayName], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (1, N'Arabic', N'العربية', 0, CAST(N'2025-02-03T15:30:48.6540493' AS DateTime2), 7, CAST(N'2025-02-03T15:40:00.8144140' AS DateTime2), 7, 0, NULL, NULL)
GO
INSERT [dbo].[SpokenLanguages] ([Id], [Name], [DisplayName], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, NULL, NULL, 1, CAST(N'2025-02-03T15:37:17.4495224' AS DateTime2), 7, CAST(N'2025-02-06T19:46:28.2915570' AS DateTime2), NULL, 1, NULL, CAST(N'2025-02-06T19:46:41.4261167' AS DateTime2))
GO
INSERT [dbo].[SpokenLanguages] ([Id], [Name], [DisplayName], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, N'Kurdish', N'الكردية', 1, CAST(N'2025-02-06T19:47:14.3785033' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SpokenLanguages] ([Id], [Name], [DisplayName], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, N'English', N'الانكليزية', 1, CAST(N'2025-02-06T19:47:33.8173224' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SpokenLanguages] ([Id], [Name], [DisplayName], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (6, N'Arabic', N'العربية', 1, CAST(N'2025-02-06T19:48:22.0917496' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SpokenLanguages] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 
GO
INSERT [dbo].[Countries] ([Id], [IsActive], [DialCode], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, 1, N'+964', CAST(N'2025-01-22T15:10:45.0666667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 
GO
INSERT [dbo].[Cities] ([Id], [CountryId], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (1, 2, 1, CAST(N'2025-01-22T15:15:23.5933333' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Cities] ([Id], [CountryId], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, 2, 1, CAST(N'2025-01-22T15:15:23.5933333' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Cities] ([Id], [CountryId], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, 2, 1, CAST(N'2025-01-22T15:15:23.5933333' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpUsers] ON 
GO
INSERT [dbo].[AbpUsers] ([Id], [AccessFailedCount], [AuthenticationSource], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [EmailAddress], [EmailConfirmationCode], [IsActive], [IsDeleted], [IsEmailConfirmed], [IsLockoutEnabled], [IsPhoneNumberConfirmed], [IsTwoFactorEnabled], [LastModificationTime], [LastModifierUserId], [LockoutEndDateUtc], [Name], [NormalizedEmailAddress], [NormalizedUserName], [Password], [PasswordResetCode], [PhoneNumber], [SecurityStamp], [Surname], [TenantId], [UserName], [DialCode], [FcmToken], [RegistrationFullName], [Type], [CompanyId], [InvitationCode], [ProfileId]) VALUES (1, 0, NULL, N'c7dc4e70-47cb-4197-8012-31b7fa2fdacf', CAST(N'2025-01-28T10:39:30.7902489' AS DateTime2), NULL, NULL, NULL, N'admin@aspnetboilerplate.com', NULL, 1, 0, 1, 0, 0, 0, NULL, NULL, NULL, N'admin', N'ADMIN@ASPNETBOILERPLATE.COM', N'ADMIN', N'AQAAAAIAAYagAAAAEDsLggUwHHykn9hb0jVJQjb5+uCGGZQgetE+e2sdrzp4kiDSX3eTUVcPsqk36T1Chg==', NULL, NULL, N'08b89fdb-c9ca-c25c-29d5-3a17be06babc', N'admin', NULL, N'admin', NULL, NULL, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[AbpUsers] ([Id], [AccessFailedCount], [AuthenticationSource], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [EmailAddress], [EmailConfirmationCode], [IsActive], [IsDeleted], [IsEmailConfirmed], [IsLockoutEnabled], [IsPhoneNumberConfirmed], [IsTwoFactorEnabled], [LastModificationTime], [LastModifierUserId], [LockoutEndDateUtc], [Name], [NormalizedEmailAddress], [NormalizedUserName], [Password], [PasswordResetCode], [PhoneNumber], [SecurityStamp], [Surname], [TenantId], [UserName], [DialCode], [FcmToken], [RegistrationFullName], [Type], [CompanyId], [InvitationCode], [ProfileId]) VALUES (48, 0, NULL, N'976c15ad-21d3-48eb-b5d2-44c221a281fb', CAST(N'2025-02-24T11:37:03.3475967' AS DateTime2), NULL, NULL, NULL, N'تيست67501@EntityFrameWorkCore.net', NULL, 1, 0, 1, 1, 1, 0, CAST(N'2025-02-24T11:37:45.1508628' AS DateTime2), NULL, NULL, N'', N'تيست67501@ENTITYFRAMEWORKCORE.NET', N'0945718880C', N'AQAAAAIAAYagAAAAECjFVzaDhCtGMsIMmlf6SPd+1SODBScGQ99qMydGyJh8ZoIRuWuuBUAI+SetRnIcOQ==', NULL, N'0945718880', N'FUFTOZZHZOUPOX5WRKIJJ2FB4RBXXVHU', N'', 1, N'0945718880C', N'+964', NULL, N'تيست', 3, NULL, NULL, NULL)
GO
INSERT [dbo].[AbpUsers] ([Id], [AccessFailedCount], [AuthenticationSource], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [EmailAddress], [EmailConfirmationCode], [IsActive], [IsDeleted], [IsEmailConfirmed], [IsLockoutEnabled], [IsPhoneNumberConfirmed], [IsTwoFactorEnabled], [LastModificationTime], [LastModifierUserId], [LockoutEndDateUtc], [Name], [NormalizedEmailAddress], [NormalizedUserName], [Password], [PasswordResetCode], [PhoneNumber], [SecurityStamp], [Surname], [TenantId], [UserName], [DialCode], [FcmToken], [RegistrationFullName], [Type], [CompanyId], [InvitationCode], [ProfileId]) VALUES (49, 0, NULL, N'05570980-e0a4-4817-a6c9-f16e389ef049', CAST(N'2025-02-24T11:39:21.9869823' AS DateTime2), 48, NULL, NULL, N'تتتتت8911@EntityFrameWorkCore.net', NULL, 1, 0, 1, 1, 1, 0, CAST(N'2025-02-24T11:40:22.4069492' AS DateTime2), 49, NULL, N'', N'تتتتت8911@ENTITYFRAMEWORKCORE.NET', N'0935716664', N'AQAAAAIAAYagAAAAEGTJwuGEaZh3Pc05CGDL8bcokqEnS0Lkckukl42LbEEmIpZr1Pm5Ri4U5GNEP6nlMg==', NULL, N'0935716664', N'NCEUNOGPGDLXW7JMVV2IMULT4PM4PNZI', N'', 1, N'0935716664', N'+964', NULL, N'تتتتت', 2, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[AbpUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[Profiles] ON 
GO
INSERT [dbo].[Profiles] ([Id], [UserId], [CityId], [About], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (28, 49, 2, NULL, CAST(N'2025-02-24T11:39:22.1370731' AS DateTime2), 48, CAST(N'2025-02-24T11:40:10.1624804' AS DateTime2), 49, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Profiles] OFF
GO
SET IDENTITY_INSERT [dbo].[SpokenLanguageValue] ON 
GO
INSERT [dbo].[SpokenLanguageValue] ([Id], [ProfileId], [SpokenLanguageId], [OralLevel], [WritingLevel], [IsNative], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (12, 28, 4, 5, 3, 1, CAST(N'2025-02-24T11:40:10.0662043' AS DateTime2), 49, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SpokenLanguageValue] OFF
GO
SET IDENTITY_INSERT [dbo].[Companies] ON 
GO
INSERT [dbo].[Companies] ([Id], [CityId], [UserId], [IsActive], [Status], [JobType], [DateOfEstablishment], [ReasonRefuse], [NumberOfEmployees], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime], [WebSite]) VALUES (11, 2, 48, 0, 1, N'ظظظظ', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2), NULL, 22, CAST(N'2025-02-24T11:37:33.6314913' AS DateTime2), 48, NULL, NULL, 0, NULL, NULL, N'غغععغغ')
GO
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkPosts] ON 
GO
INSERT [dbo].[WorkPosts] ([Id], [CompanyId], [Status], [Title], [Description], [WorkEngagement], [WorkLevel], [EducationLevel], [MinSalary], [MaxSalary], [ExperienceYearsCount], [RequiredEmployeesCount], [ApplicantsCount], [WorkAvailbility], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime], [IsClosed], [Slug], [WorkPlace]) VALUES (19, 11, 2, N'غغغغ', N'ةةةة', 2, 2, 2, CAST(100.00000 AS Decimal(20, 5)), CAST(1570.00000 AS Decimal(20, 5)), 1, 1, 0, 1, CAST(N'2025-02-24T11:38:00.0651662' AS DateTime2), 48, NULL, NULL, 0, NULL, NULL, 0, N'000001', 2)
GO
SET IDENTITY_INSERT [dbo].[WorkPosts] OFF
GO
SET IDENTITY_INSERT [dbo].[CountryTranslations] ON 
GO
INSERT [dbo].[CountryTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, N'العراق', 2, N'ar', CAST(N'2025-01-22T15:12:07.5400000' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CountryTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, N'Iraq', 2, N'en', CAST(N'2025-01-22T15:12:07.5400000' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CountryTranslations] OFF
GO
SET IDENTITY_INSERT [dbo].[CityTranslations] ON 
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, N'بغداد', 1, N'ar', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, N'Baghdad', 1, N'en', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, N'أربيل', 2, N'ar', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (5, N'Erbil', 2, N'en', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (6, N'البصرة', 3, N'ar', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[CityTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (7, N'Basra', 3, N'en', CAST(N'2025-01-22T15:16:23.5166667' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CityTranslations] OFF
GO
SET IDENTITY_INSERT [dbo].[CompanyTranslations] ON 
GO
INSERT [dbo].[CompanyTranslations] ([Id], [Name], [About], [Address], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (15, N'تيست', N'لللل', N'ةةةةةة', 11, N'ar', CAST(N'2025-02-24T11:37:33.6314426' AS DateTime2), 48, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CompanyTranslations] OFF
GO
SET IDENTITY_INSERT [dbo].[Educations] ON 
GO
INSERT [dbo].[Educations] ([Id], [Level], [InstitutionName], [FieldOfStudy], [StartDate], [EndDate], [IsCurrentlyStudying], [Description], [ProfileId], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (19, 3, N'ععععع', N'ززظظ', CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), 1, N'غغغغ', 28, CAST(N'2025-02-24T11:40:10.0659627' AS DateTime2), 49, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Educations] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkExperiences] ON 
GO
INSERT [dbo].[WorkExperiences] ([Id], [JobTitle], [CompanyName], [StartDate], [EndDate], [IsCurrentJob], [Description], [ProfileId], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (18, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, N'اااا', 28, CAST(N'2025-02-24T11:40:10.0657946' AS DateTime2), 49, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[WorkExperiences] OFF
GO
SET IDENTITY_INSERT [dbo].[Skills] ON 
GO
INSERT [dbo].[Skills] ([Id], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, 1, CAST(N'2025-02-03T15:44:43.8849409' AS DateTime2), 7, CAST(N'2025-02-05T14:25:30.2357708' AS DateTime2), 8, 1, NULL, CAST(N'2025-02-06T10:39:10.5356488' AS DateTime2))
GO
INSERT [dbo].[Skills] ([Id], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, 1, CAST(N'2025-02-06T10:40:16.6331246' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Skills] ([Id], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, 1, CAST(N'2025-02-06T10:40:43.1481459' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Skills] ([Id], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (5, 1, CAST(N'2025-02-06T19:32:06.4557076' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Skills] ([Id], [IsActive], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (6, 1, CAST(N'2025-02-06T19:32:28.0418061' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Skills] OFF
GO
SET IDENTITY_INSERT [dbo].[SkillTranslations] ON 
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, N'skill1', 2, N'en', CAST(N'2025-02-03T15:44:43.8854312' AS DateTime2), 7, NULL, NULL, 1, 7, CAST(N'2025-02-03T15:47:10.3876667' AS DateTime2))
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, N'مهارة1', 2, N'ar', CAST(N'2025-02-03T15:44:43.8860848' AS DateTime2), 7, NULL, NULL, 1, 7, CAST(N'2025-02-03T15:47:10.6644443' AS DateTime2))
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (5, N'skill1', 2, N'en', CAST(N'2025-02-03T15:47:09.5385317' AS DateTime2), 7, CAST(N'2025-02-05T14:25:30.2360346' AS DateTime2), 8, 1, NULL, CAST(N'2025-02-06T10:39:10.5284275' AS DateTime2))
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (6, N'مهارة1', 2, N'ar', CAST(N'2025-02-03T15:47:09.5385392' AS DateTime2), 7, CAST(N'2025-02-05T14:25:30.2360570' AS DateTime2), 8, 1, NULL, CAST(N'2025-02-06T10:39:10.5299750' AS DateTime2))
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (7, N'مsadaة1', 2, N'ku', CAST(N'2025-02-03T15:47:09.5385400' AS DateTime2), 7, CAST(N'2025-02-05T14:25:30.2362121' AS DateTime2), 8, 1, NULL, CAST(N'2025-02-06T10:39:10.5308203' AS DateTime2))
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (8, N'قائد محترف', 3, N'ar', CAST(N'2025-02-06T10:40:16.6332012' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (9, N'Leader', 3, N'en', CAST(N'2025-02-06T10:40:16.6332221' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (10, N'عمل جماعي', 4, N'ar', CAST(N'2025-02-06T10:40:43.1481463' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (11, N'Team work', 4, N'en', CAST(N'2025-02-06T10:40:43.1481469' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (12, N'سريع الانجاز', 5, N'ar', CAST(N'2025-02-06T19:32:06.4557666' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (13, N'Fast work', 5, N'en', CAST(N'2025-02-06T19:32:06.4557741' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (14, N'شخصية طريفة', 6, N'ar', CAST(N'2025-02-06T19:32:28.0418065' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[SkillTranslations] ([Id], [Name], [CoreId], [Language], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (15, N'Funny', 6, N'en', CAST(N'2025-02-06T19:32:28.0418072' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SkillTranslations] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpRoles] ON 
GO
INSERT [dbo].[AbpRoles] ([Id], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [IsDefault], [IsDeleted], [IsStatic], [LastModificationTime], [LastModifierUserId], [Name], [NormalizedName], [TenantId], [Description]) VALUES (1, N'e5c1754c-8935-4c4f-8339-18ed45631ea2', CAST(N'2025-01-28T10:39:24.9414756' AS DateTime2), NULL, NULL, NULL, N'Admin', 1, 0, 1, NULL, NULL, N'Admin', N'ADMIN', NULL, NULL)
GO
INSERT [dbo].[AbpRoles] ([Id], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [IsDefault], [IsDeleted], [IsStatic], [LastModificationTime], [LastModifierUserId], [Name], [NormalizedName], [TenantId], [Description]) VALUES (2, N'07d573f2-a569-4d56-8595-48e4465a1c6d', CAST(N'2025-01-28T10:39:43.1322893' AS DateTime2), NULL, NULL, NULL, N'Admin', 0, 0, 1, NULL, NULL, N'Admin', N'ADMIN', 1, NULL)
GO
INSERT [dbo].[AbpRoles] ([Id], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [IsDefault], [IsDeleted], [IsStatic], [LastModificationTime], [LastModifierUserId], [Name], [NormalizedName], [TenantId], [Description]) VALUES (3, N'2bae5a02-724d-4782-b867-c9b1a77b246e', CAST(N'2025-01-28T10:39:44.7641690' AS DateTime2), NULL, NULL, NULL, N'BasicUser', 0, 0, 1, NULL, NULL, N'BasicUser', N'BASICUSER', 1, NULL)
GO
INSERT [dbo].[AbpRoles] ([Id], [ConcurrencyStamp], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [IsDefault], [IsDeleted], [IsStatic], [LastModificationTime], [LastModifierUserId], [Name], [NormalizedName], [TenantId], [Description]) VALUES (4, N'52960dac-9fbc-4a0f-9af9-8504f35a38f0', CAST(N'2025-01-28T10:39:45.7379731' AS DateTime2), NULL, NULL, NULL, N'CompanyUser', 0, 0, 1, NULL, NULL, N'CompanyUser', N'COMPANYUSER', 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[AbpRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpPermissions] ON 
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (1, CAST(N'2025-01-28T10:39:26.6884768' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (2, CAST(N'2025-01-28T10:39:27.0034989' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.List', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (3, CAST(N'2025-01-28T10:39:27.0707938' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Create', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (4, CAST(N'2025-01-28T10:39:27.1125053' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (5, CAST(N'2025-01-28T10:39:27.1500873' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (6, CAST(N'2025-01-28T10:39:27.1869356' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Activation', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (7, CAST(N'2025-01-28T10:39:27.2226922' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (8, CAST(N'2025-01-28T10:39:27.2584043' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Read', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (9, CAST(N'2025-01-28T10:39:27.3393769' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (10, CAST(N'2025-01-28T10:39:27.3758063' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (11, CAST(N'2025-01-28T10:39:27.4118408' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (12, CAST(N'2025-01-28T10:39:27.4452847' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Create', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (13, CAST(N'2025-01-28T10:39:27.4833787' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (14, CAST(N'2025-01-28T10:39:27.5295701' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (15, CAST(N'2025-01-28T10:39:27.5742120' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.List', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (16, CAST(N'2025-01-28T10:39:27.6660772' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.List.Permission', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (17, CAST(N'2025-01-28T10:39:27.7085848' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Get', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (18, CAST(N'2025-01-28T10:39:27.7464922' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Tenants', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (19, CAST(N'2025-01-28T10:39:46.7561471' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (20, CAST(N'2025-01-28T10:39:46.7861178' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.List', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (21, CAST(N'2025-01-28T10:39:46.8155630' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Create', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (22, CAST(N'2025-01-28T10:39:46.8631387' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (23, CAST(N'2025-01-28T10:39:46.8977673' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (24, CAST(N'2025-01-28T10:39:46.9261785' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users.Activation', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (25, CAST(N'2025-01-28T10:39:46.9544844' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (26, CAST(N'2025-01-28T10:39:46.9772589' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Read', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (27, CAST(N'2025-01-28T10:39:47.0088433' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (28, CAST(N'2025-01-28T10:39:47.0352356' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Accounts.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (29, CAST(N'2025-01-28T10:39:47.0684695' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (30, CAST(N'2025-01-28T10:39:47.1005899' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Create', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (31, CAST(N'2025-01-28T10:39:47.1366987' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (32, CAST(N'2025-01-28T10:39:47.1737357' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (33, CAST(N'2025-01-28T10:39:47.2073832' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.List', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (34, CAST(N'2025-01-28T10:39:47.2492977' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.List.Permission', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (35, CAST(N'2025-01-28T10:39:47.2807927' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Roles.Get', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (36, CAST(N'2025-01-28T10:39:49.1699686' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users', 1, 3, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (37, CAST(N'2025-01-28T10:39:50.2649491' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Pages.Users', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (38, CAST(N'2025-02-13T17:37:08.5348485' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (39, CAST(N'2025-02-13T17:37:08.8689939' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Read', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (40, CAST(N'2025-02-13T17:37:08.8772104' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Create', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (41, CAST(N'2025-02-13T17:37:08.8774842' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (42, CAST(N'2025-02-13T17:37:08.8778042' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (43, CAST(N'2025-02-13T17:37:08.8779781' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (44, CAST(N'2025-02-13T17:37:08.8782962' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Read', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (45, CAST(N'2025-02-13T17:37:08.8784693' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Create', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (46, CAST(N'2025-02-13T17:37:08.8787276' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (47, CAST(N'2025-02-13T17:37:08.8790216' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (48, CAST(N'2025-02-13T17:37:12.9641112' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (49, CAST(N'2025-02-13T17:37:12.9645667' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Read', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (50, CAST(N'2025-02-13T17:37:12.9647273' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Create', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (51, CAST(N'2025-02-13T17:37:12.9648885' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (52, CAST(N'2025-02-13T17:37:12.9650435' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (53, CAST(N'2025-02-13T17:37:12.9657083' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (54, CAST(N'2025-02-13T17:37:12.9660094' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Read', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (55, CAST(N'2025-02-13T17:37:12.9663036' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Create', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (56, CAST(N'2025-02-13T17:37:12.9666218' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (57, CAST(N'2025-02-13T17:37:12.9669495' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (58, CAST(N'2025-02-13T17:37:13.6872689' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (59, CAST(N'2025-02-13T17:37:13.6876522' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Read', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (60, CAST(N'2025-02-13T17:37:13.6878214' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Create', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (61, CAST(N'2025-02-13T17:37:13.6882803' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'Companies.Update', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (62, CAST(N'2025-02-13T17:37:13.6884914' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (63, CAST(N'2025-02-13T17:37:13.6886822' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Read', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (64, CAST(N'2025-02-13T17:37:13.6888309' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Create', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (65, CAST(N'2025-02-13T17:37:13.6889656' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkPosts.Update', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (66, CAST(N'2025-02-16T18:39:16.7988483' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (67, CAST(N'2025-02-16T18:39:17.1556097' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Approve', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (68, CAST(N'2025-02-16T18:39:17.1631549' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Create', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (69, CAST(N'2025-02-16T18:39:17.1634512' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Reject', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (70, CAST(N'2025-02-16T18:39:17.1637647' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Delete', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (71, CAST(N'2025-02-16T18:39:17.1640219' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Update', NULL, 1, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (72, CAST(N'2025-02-16T18:39:21.0822952' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (73, CAST(N'2025-02-16T18:39:21.0832730' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Approve', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (74, CAST(N'2025-02-16T18:39:21.0836003' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Create', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (75, CAST(N'2025-02-16T18:39:21.0837897' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Reject', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (76, CAST(N'2025-02-16T18:39:21.0839573' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Delete', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (77, CAST(N'2025-02-16T18:39:21.0841157' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Update', 1, 2, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (78, CAST(N'2025-02-16T18:39:21.6112262' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications', 1, 3, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (79, CAST(N'2025-02-16T18:39:21.6116807' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Create', 1, 3, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (80, CAST(N'2025-02-16T18:39:21.6119059' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Delete', 1, 3, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (81, CAST(N'2025-02-16T18:39:21.6120934' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Update', 1, 3, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (82, CAST(N'2025-02-16T18:39:22.0910626' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (83, CAST(N'2025-02-16T18:39:22.0915225' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Approve', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (84, CAST(N'2025-02-16T18:39:22.0916934' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Reject', 1, 4, NULL)
GO
INSERT [dbo].[AbpPermissions] ([Id], [CreationTime], [CreatorUserId], [Discriminator], [IsGranted], [Name], [TenantId], [RoleId], [UserId]) VALUES (85, CAST(N'2025-02-16T18:39:22.0918444' AS DateTime2), NULL, N'RolePermissionSetting', 1, N'WorkApplications.Delete', 1, 4, NULL)
GO
SET IDENTITY_INSERT [dbo].[AbpPermissions] OFF
GO
SET IDENTITY_INSERT [dbo].[UserVerficationCodes] ON 
GO
INSERT [dbo].[UserVerficationCodes] ([Id], [UserId], [VerficationCode], [ConfirmationCodeType], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (47, 48, NULL, 2, CAST(N'2025-02-24T11:37:03.8569573' AS DateTime2), NULL, CAST(N'2025-02-24T11:37:45.1297899' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[UserVerficationCodes] ([Id], [UserId], [VerficationCode], [ConfirmationCodeType], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (48, 49, NULL, 2, CAST(N'2025-02-24T11:39:22.1309328' AS DateTime2), 48, CAST(N'2025-02-24T11:40:22.4006327' AS DateTime2), 49, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserVerficationCodes] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpUserRoles] ON 
GO
INSERT [dbo].[AbpUserRoles] ([Id], [CreationTime], [CreatorUserId], [RoleId], [TenantId], [UserId]) VALUES (1, CAST(N'2025-01-28T10:39:35.7935883' AS DateTime2), NULL, 1, NULL, 1)
GO
INSERT [dbo].[AbpUserRoles] ([Id], [CreationTime], [CreatorUserId], [RoleId], [TenantId], [UserId]) VALUES (48, CAST(N'2025-02-24T11:37:03.3555222' AS DateTime2), NULL, 4, 1, 48)
GO
INSERT [dbo].[AbpUserRoles] ([Id], [CreationTime], [CreatorUserId], [RoleId], [TenantId], [UserId]) VALUES (49, CAST(N'2025-02-24T11:39:21.9916989' AS DateTime2), 48, 3, 1, 49)
GO
SET IDENTITY_INSERT [dbo].[AbpUserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpSettings] ON 
GO
INSERT [dbo].[AbpSettings] ([Id], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [UserId], [Value]) VALUES (1, CAST(N'2025-01-28T10:39:37.4929916' AS DateTime2), NULL, NULL, NULL, N'Abp.Net.Mail.DefaultFromAddress', 1, NULL, N'admin@mydomain.com')
GO
INSERT [dbo].[AbpSettings] ([Id], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [UserId], [Value]) VALUES (2, CAST(N'2025-01-28T10:39:38.8204342' AS DateTime2), NULL, NULL, NULL, N'Abp.Net.Mail.DefaultFromDisplayName', 1, NULL, N'mydomain.com mailer')
GO
INSERT [dbo].[AbpSettings] ([Id], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [UserId], [Value]) VALUES (3, CAST(N'2025-01-28T10:39:39.8596316' AS DateTime2), NULL, NULL, NULL, N'Abp.Localization.DefaultLanguageName', 1, NULL, N'en')
GO
SET IDENTITY_INSERT [dbo].[AbpSettings] OFF
GO
SET IDENTITY_INSERT [dbo].[AbpLanguages] ON 
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (1, CAST(N'2025-01-28T10:39:06.5577871' AS DateTime2), NULL, NULL, NULL, N'English', N'famfamfam-flags us', 0, NULL, NULL, N'en', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (2, CAST(N'2025-01-28T10:39:06.5593206' AS DateTime2), NULL, NULL, NULL, N'العربية', N'famfamfam-flags sa', 0, NULL, NULL, N'ar', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (3, CAST(N'2025-01-28T10:39:06.5593255' AS DateTime2), NULL, NULL, NULL, N'German', N'famfamfam-flags de', 0, NULL, NULL, N'de', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (4, CAST(N'2025-01-28T10:39:06.5593266' AS DateTime2), NULL, NULL, NULL, N'Italiano', N'famfamfam-flags it', 0, NULL, NULL, N'it', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (5, CAST(N'2025-01-28T10:39:06.5593279' AS DateTime2), NULL, NULL, NULL, N'فارسی', N'famfamfam-flags ir', 0, NULL, NULL, N'fa', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (6, CAST(N'2025-01-28T10:39:06.5593319' AS DateTime2), NULL, NULL, NULL, N'Français', N'famfamfam-flags fr', 0, NULL, NULL, N'fr', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (7, CAST(N'2025-01-28T10:39:06.5593328' AS DateTime2), NULL, NULL, NULL, N'Português', N'famfamfam-flags br', 0, NULL, NULL, N'pt-BR', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (8, CAST(N'2025-01-28T10:39:06.5593336' AS DateTime2), NULL, NULL, NULL, N'Türkçe', N'famfamfam-flags tr', 0, NULL, NULL, N'tr', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (9, CAST(N'2025-01-28T10:39:06.5593344' AS DateTime2), NULL, NULL, NULL, N'Русский', N'famfamfam-flags ru', 0, NULL, NULL, N'ru', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (10, CAST(N'2025-01-28T10:39:06.5593785' AS DateTime2), NULL, NULL, NULL, N'简体中文', N'famfamfam-flags cn', 0, NULL, NULL, N'zh-Hans', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (11, CAST(N'2025-01-28T10:39:06.5593804' AS DateTime2), NULL, NULL, NULL, N'Español México', N'famfamfam-flags mx', 0, NULL, NULL, N'es-MX', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (12, CAST(N'2025-01-28T10:39:06.5593812' AS DateTime2), NULL, NULL, NULL, N'Nederlands', N'famfamfam-flags nl', 0, NULL, NULL, N'nl', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (13, CAST(N'2025-01-28T10:39:06.5593820' AS DateTime2), NULL, NULL, NULL, N'日本語', N'famfamfam-flags jp', 0, NULL, NULL, N'ja', 1, 0)
GO
INSERT [dbo].[AbpLanguages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [DisplayName], [Icon], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [TenantId], [IsDisabled]) VALUES (14, CAST(N'2025-02-05T17:48:02.6161482' AS DateTime2), NULL, NULL, NULL, N'Kurdî', N'famfamfam-flag-icon-ku', 0, NULL, NULL, N'ku', 1, 0)
GO
SET IDENTITY_INSERT [dbo].[AbpLanguages] OFF
GO
SET IDENTITY_INSERT [dbo].[RegisterdPhoneNumbers] ON 
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (1, N'+963', N'930431582', NULL, 1, CAST(N'2025-01-15T18:06:57.6338682' AS DateTime2), NULL, CAST(N'2025-01-15T18:45:29.0887594' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (2, N'+963', N'945643986', NULL, 1, CAST(N'2025-01-27T19:01:34.2697632' AS DateTime2), NULL, CAST(N'2025-01-27T19:02:46.5442077' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (3, N'+963', N'976984563', NULL, 1, CAST(N'2025-01-29T16:46:58.6995638' AS DateTime2), NULL, CAST(N'2025-01-29T17:10:47.2357996' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (4, N'+961', N'87639827', NULL, 1, CAST(N'2025-01-30T18:44:21.4669799' AS DateTime2), NULL, CAST(N'2025-01-30T18:47:10.6942865' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (5, N'+963', N'930431583', NULL, 1, CAST(N'2025-02-03T18:32:44.4294836' AS DateTime2), NULL, CAST(N'2025-02-03T18:33:24.8006190' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (6, N'+964', N'5555', N'249617', 0, CAST(N'2025-02-03T22:15:57.4507968' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (7, N'+964', N'2222', NULL, 1, CAST(N'2025-02-03T22:22:30.6714170' AS DateTime2), NULL, CAST(N'2025-02-03T22:49:33.0938878' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (8, N'+964', N'1111', N'709328', 0, CAST(N'2025-02-03T22:22:55.8650961' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (9, N'+964', N'22222', N'767709', 0, CAST(N'2025-02-03T22:24:36.0274357' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (10, N'+964', N'555555', NULL, 1, CAST(N'2025-02-03T22:37:46.5512246' AS DateTime2), NULL, CAST(N'2025-02-03T22:45:36.3110683' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (11, N'+964', N'5555555', NULL, 1, CAST(N'2025-02-03T22:51:00.6016088' AS DateTime2), NULL, CAST(N'2025-02-03T22:51:03.2719756' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (12, N'+964', N'3333333', NULL, 1, CAST(N'2025-02-03T22:58:31.9007302' AS DateTime2), NULL, CAST(N'2025-02-03T22:58:33.5252421' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (13, N'+964', N'111111', NULL, 1, CAST(N'2025-02-03T23:03:26.1611106' AS DateTime2), NULL, CAST(N'2025-02-03T23:03:27.6277945' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (14, N'+964', N'2255', NULL, 1, CAST(N'2025-02-03T23:17:34.8251385' AS DateTime2), NULL, CAST(N'2025-02-03T23:17:36.1868984' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (15, N'+964', N'555', N'550776', 0, CAST(N'2025-02-05T12:32:14.3204398' AS DateTime2), NULL, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (16, N'+964', N'00', NULL, 1, CAST(N'2025-02-05T12:32:49.6630346' AS DateTime2), NULL, CAST(N'2025-02-05T12:33:09.0740650' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (17, N'+964', N'5555555555', N'533242', 0, CAST(N'2025-02-05T13:39:00.5485183' AS DateTime2), NULL, CAST(N'2025-02-05T13:41:39.9526039' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (18, N'964', N'1234567890', NULL, 1, CAST(N'2025-02-05T13:45:56.7870220' AS DateTime2), NULL, CAST(N'2025-02-05T13:45:58.3617568' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (19, N'964', N'1234567899', NULL, 1, CAST(N'2025-02-05T13:49:07.8191012' AS DateTime2), NULL, CAST(N'2025-02-05T13:49:08.9138312' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (20, N'964', N'1234567898', NULL, 1, CAST(N'2025-02-05T13:52:16.2716300' AS DateTime2), NULL, CAST(N'2025-02-05T13:52:17.6337902' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (21, N'+963', N'324', N'047843', 0, CAST(N'2025-02-05T17:49:42.4354113' AS DateTime2), 8, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (22, N'964', N'0945718880', NULL, 1, CAST(N'2025-02-05T16:59:24.9599553' AS DateTime2), NULL, CAST(N'2025-02-05T16:59:26.6710916' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (23, N'964', N'0935716664', NULL, 1, CAST(N'2025-02-05T17:02:18.0060987' AS DateTime2), NULL, CAST(N'2025-02-05T17:02:19.3890301' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (24, N'964', N'0952616934', NULL, 1, CAST(N'2025-02-05T17:20:37.5995523' AS DateTime2), NULL, CAST(N'2025-02-05T17:20:38.9761977' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (25, N'964', N'0000000000', NULL, 1, CAST(N'2025-02-05T17:29:36.6865885' AS DateTime2), NULL, CAST(N'2025-02-05T17:29:37.9076488' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (26, N'964', N'0987654321', NULL, 1, CAST(N'2025-02-06T18:07:14.8390400' AS DateTime2), NULL, CAST(N'2025-02-06T18:07:16.4586256' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (27, N'964', N'0987654322', NULL, 1, CAST(N'2025-02-06T18:11:32.8557598' AS DateTime2), NULL, CAST(N'2025-02-06T18:11:34.4597963' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (28, N'964', N'1231231231', NULL, 1, CAST(N'2025-02-06T18:16:29.4313453' AS DateTime2), NULL, CAST(N'2025-02-06T18:16:31.0402027' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (29, N'964', N'3333333333', NULL, 1, CAST(N'2025-02-06T18:21:42.9769211' AS DateTime2), NULL, CAST(N'2025-02-06T18:21:44.4777940' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (30, N'964', N'5555555555', NULL, 1, CAST(N'2025-02-06T18:38:36.0346171' AS DateTime2), NULL, CAST(N'2025-02-06T18:38:37.9247980' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (31, N'964', N'3698521470', NULL, 1, CAST(N'2025-02-06T18:41:06.9721905' AS DateTime2), NULL, CAST(N'2025-02-06T18:41:08.8424749' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (32, N'964', N'1472583690', NULL, 1, CAST(N'2025-02-06T18:44:17.4313903' AS DateTime2), NULL, CAST(N'2025-02-06T18:44:18.8305900' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (33, N'964', N'6655555555', NULL, 1, CAST(N'2025-02-06T19:11:38.8017666' AS DateTime2), NULL, CAST(N'2025-02-06T19:11:40.6502104' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (34, N'964', N'5555555599', NULL, 1, CAST(N'2025-02-06T19:50:25.3630273' AS DateTime2), NULL, CAST(N'2025-02-06T19:50:26.6123072' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (35, N'964', N'7818049186', NULL, 1, CAST(N'2025-02-06T20:57:24.9868768' AS DateTime2), NULL, CAST(N'2025-02-06T20:57:28.5433247' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (36, N'964', N'7807211011', NULL, 1, CAST(N'2025-02-07T17:13:07.0275063' AS DateTime2), NULL, CAST(N'2025-02-24T16:12:35.3162537' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (37, N'+964', N'1234567890', NULL, 1, CAST(N'2025-02-13T16:19:36.4583122' AS DateTime2), NULL, CAST(N'2025-02-22T10:07:40.7443496' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (38, N'+964', N'1234567899', NULL, 1, CAST(N'2025-02-13T16:23:44.5965876' AS DateTime2), NULL, CAST(N'2025-02-20T21:32:59.1544738' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (39, N'+964', N'0987654321', NULL, 1, CAST(N'2025-02-19T12:18:31.7622980' AS DateTime2), NULL, CAST(N'2025-02-19T12:18:33.8923325' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (40, N'+964', N'1212121212', NULL, 1, CAST(N'2025-02-19T17:16:12.6083401' AS DateTime2), NULL, CAST(N'2025-02-19T17:16:13.8797388' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (41, N'+964', N'2222222222', NULL, 1, CAST(N'2025-02-20T21:40:45.5654090' AS DateTime2), NULL, CAST(N'2025-02-20T21:40:46.7249126' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (42, N'+964', N'1122334455', NULL, 1, CAST(N'2025-02-20T21:44:36.9756386' AS DateTime2), NULL, CAST(N'2025-02-20T21:44:47.3778950' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (43, N'+964', N'2211334455', NULL, 1, CAST(N'2025-02-20T21:55:48.6542082' AS DateTime2), NULL, CAST(N'2025-02-20T21:55:49.9931283' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (44, N'+964', N'3366996633', NULL, 1, CAST(N'2025-02-20T22:14:34.8320208' AS DateTime2), NULL, CAST(N'2025-02-20T22:14:36.1003860' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (45, N'+964', N'9988776655', NULL, 1, CAST(N'2025-02-20T22:18:24.9438780' AS DateTime2), NULL, CAST(N'2025-02-20T22:18:25.9572413' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (46, N'+964', N'1144778855', NULL, 1, CAST(N'2025-02-20T22:37:25.3465929' AS DateTime2), NULL, CAST(N'2025-02-20T22:37:26.6896242' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (47, N'+964', N'9955112233', NULL, 1, CAST(N'2025-02-21T09:45:04.1724492' AS DateTime2), NULL, CAST(N'2025-02-21T09:45:05.7344942' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (48, N'+964', N'0945718880', NULL, 1, CAST(N'2025-02-21T11:42:31.4420801' AS DateTime2), NULL, CAST(N'2025-02-24T11:36:48.7336779' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (49, N'+964', N'0935716664', NULL, 1, CAST(N'2025-02-21T12:48:22.0984961' AS DateTime2), NULL, CAST(N'2025-02-24T11:39:15.5738326' AS DateTime2), 48, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (50, N'+964', N'0952616934', NULL, 1, CAST(N'2025-02-21T16:11:45.0220121' AS DateTime2), NULL, CAST(N'2025-02-21T16:11:46.7671769' AS DateTime2), NULL, 0, NULL, NULL)
GO
INSERT [dbo].[RegisterdPhoneNumbers] ([Id], [DialCode], [PhoneNumber], [VerficationCode], [IsVerified], [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId], [IsDeleted], [DeleterUserId], [DeletionTime]) VALUES (51, N'+964', N'0987456321', NULL, 1, CAST(N'2025-02-21T16:18:05.8174817' AS DateTime2), 43, CAST(N'2025-02-21T16:18:07.2869838' AS DateTime2), 43, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[RegisterdPhoneNumbers] OFF
GO
