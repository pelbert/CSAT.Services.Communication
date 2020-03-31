USE [FTDNA]
GO
/****** Object:  StoredProcedure [dbo].[bp_FF_FTV]    Script Date: 7/13/2018 8:40:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[bp_FF_FTV]
    @kitnum NVARCHAR(10),
	  @filterGroupId int,
	 @filtername NVARCHAR(55),
	  @offset int,
	  @pagesize int
AS
BEGIN
declare @totalMatches  int;
set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm where kitnum =@kitnum
  and relationshipid is not null)
if (@pagesize= 0) Begin 
	set @pagesize = @totalMatches +100
	End 
	if (@filtername is not null) 
	begin
			if (@filterGroupId >0) 
			Begin
			
				select @totalMatches as totalMatches,  matchkitnum as encryptedFtdnaKitNum, fullname as [name], familyfinderrelationship as relationship, 
				familyfinderrelationshipGroup  as relationshipgroup,familyfinderrelationshipGroupId  as relationshipgroupid ,ffm.buckettype as bucket,CASE WHEN Female = 1 THEN 'F' else 'M' end as gender, (
									select 'social-photos/' + convert(varchar(55),se.ProfilePhotoId) from [Storage].[Files] sf
									inner join [Social].[Photos] sp on sf.id = sp.FileId 
									inner join social.entities  se on se.ProfilePhotoId = sp.id 
									inner join users.users u on u.id = se.id 
									inner join aspnetusers aspu on aspu.id = u.MembershipId 
									where UserName = ftv.MatchKitNum
							   ) as ProfileImageUrl from vw_FamilyTreeMatches ftv, [FAMILYFINDER].[Results].[FFMatchesToResults] ffm  where kitnum =@kitnum 
							   and ftv.matchresultid=ffm.matchresultid and ftv.resultid =ffm.resultid
				  and relationshipid is not null and fullname LIKE '%' + @filtername + '%' and familyfinderrelationshipGroupId =@filterGroupId 
				  order by RelationshipId asc offset @offset rows fetch next @pagesize rows only;
			END
			else 
			Begin 
			select @totalMatches as totalMatches,  matchkitnum as encryptedFtdnaKitNum, fullname as [name], familyfinderrelationship as relationship, 
				familyfinderrelationshipGroup  as relationshipgroup,familyfinderrelationshipGroupId  as relationshipgroupid,ffm.buckettype as bucket, CASE WHEN Female = 1 THEN 'F' else 'M' end as gender,  (
									select 'social-photos/' + convert(varchar(55),se.ProfilePhotoId) from [Storage].[Files] sf
									inner join [Social].[Photos] sp on sf.id = sp.FileId 
									inner join social.entities  se on se.ProfilePhotoId = sp.id 
									inner join users.users u on u.id = se.id 
									inner join aspnetusers aspu on aspu.id = u.MembershipId 
									where UserName = ftv.MatchKitNum
							   ) as ProfileImageUrl from vw_FamilyTreeMatches ftv, [FAMILYFINDER].[Results].[FFMatchesToResults] ffm  where kitnum =@kitnum 
							    and ftv.matchresultid=ffm.matchresultid and ftv.resultid =ffm.resultid
				  and relationshipid is not null and fullname LIKE '%' + @filtername + '%' 
				  order by RelationshipId asc offset @offset rows fetch next @pagesize rows only;
			End
	end
	else 
	begin 
	if (@filterGroupId >0) 
			Begin
			
				select @totalMatches as totalMatches,  matchkitnum as encryptedFtdnaKitNum, fullname as [name], familyfinderrelationship as relationship, 
				familyfinderrelationshipGroup  as relationshipgroup,familyfinderrelationshipGroupId  as relationshipgroupid,ffm.buckettype as bucket, CASE WHEN Female = 1 THEN 'F' else 'M' end as gender,  (
									select 'social-photos/' + convert(varchar(55),se.ProfilePhotoId) from [Storage].[Files] sf
									inner join [Social].[Photos] sp on sf.id = sp.FileId 
									inner join social.entities  se on se.ProfilePhotoId = sp.id 
									inner join users.users u on u.id = se.id 
									inner join aspnetusers aspu on aspu.id = u.MembershipId 
									where UserName = ftv.MatchKitNum
							   ) as ProfileImageUrl from vw_FamilyTreeMatches ftv, [FAMILYFINDER].[Results].[FFMatchesToResults] ffm  where kitnum =@kitnum 
							    and ftv.matchresultid=ffm.matchresultid and ftv.resultid =ffm.resultid
				  and relationshipid is not null  and familyfinderrelationshipGroupId =@filterGroupId 
				  order by RelationshipId asc offset @offset rows fetch next @pagesize rows only;
			END
			else 
			Begin 
			select @totalMatches as totalMatches,  matchkitnum as encryptedFtdnaKitNum, fullname as [name], familyfinderrelationship as relationship, 
				familyfinderrelationshipGroup  as relationshipgroup,familyfinderrelationshipGroupId  as relationshipgroupid,ffm.buckettype as bucket,CASE WHEN Female = 1 THEN 'F' else 'M' end as gender,   (
									select 'social-photos/' + convert(varchar(55),se.ProfilePhotoId) from [Storage].[Files] sf
									inner join [Social].[Photos] sp on sf.id = sp.FileId 
									inner join social.entities  se on se.ProfilePhotoId = sp.id 
									inner join users.users u on u.id = se.id 
									inner join aspnetusers aspu on aspu.id = u.MembershipId 
									where UserName = ftv.MatchKitNum
							   ) as ProfileImageUrl from vw_FamilyTreeMatches ftv, [FAMILYFINDER].[Results].[FFMatchesToResults] ffm  where kitnum =@kitnum 
							    and ftv.matchresultid=ffm.matchresultid and ftv.resultid =ffm.resultid
				  and relationshipid is not null 
				  order by RelationshipId asc offset @offset rows fetch next @pagesize rows only;
			End
	end 
  end