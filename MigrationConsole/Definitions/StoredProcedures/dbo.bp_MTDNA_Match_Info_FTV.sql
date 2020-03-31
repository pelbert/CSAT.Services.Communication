SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create PROCEDURE [dbo].[bp_MTDNA_Match_Info_FTV]
    @kitnum NVARCHAR(15),
    @filtertest NVARCHAR(5),
	 @filtername NVARCHAR(55),
	  @offset int,
	  @pagesize int
AS
BEGIN
	declare @totalMatches  int;
	if (@pagesize= 0) Begin 
	set @pagesize = 10000 
	End 
	if(@filtertest ='FMS') 
		BEGIN
			if (@filtername is not null) 
			begin
			 set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum  
			and kp.FMSMatchIndex=1 and ftm.MtDistance is not null and mttestname = 'FMS' and fullname LIKE '%' + @filtername + '%'   )

			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum  
			and kp.FMSMatchIndex=1 and ftm.MtDistance is not null and mttestname = 'FMS' and fullname LIKE '%' + @filtername + '%'  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end
			else 
			begin
			select @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.FMSMatchIndex=1 and ftm.MtDistance is not null and mttestname = 'FMS' )

			select  @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.FMSMatchIndex=1 and ftm.MtDistance is not null and mttestname = 'FMS'  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end 
		END
	ELSE if(@filtertest ='HVR1') 
		BEGIN
			if (@filtername is not null) 
			begin
			set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR1MatchIndex=1 and   ftm.MtDistance is not null  and mttestname = 'HVR1'  and fullname LIKE '%' + @filtername + '%' )

			select  @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR1MatchIndex=1 and   ftm.MtDistance is not null  and mttestname = 'HVR1' and fullname LIKE '%' + @filtername + '%'  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end 
			else 
			begin
			set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR1MatchIndex=1 and   ftm.MtDistance is not null  and mttestname = 'HVR1' )

			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR1MatchIndex=1 and   ftm.MtDistance is not null  and mttestname = 'HVR1'  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end
		END
	ELSE if(@filtertest ='HVR2') 
		BEGIN
			if (@filtername is not null) 
			begin
			set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and ftm.MtDistance is not null and mttestname = 'HVR2' and fullname LIKE '%' + @filtername + '%')

			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and ftm.MtDistance is not null and mttestname = 'HVR2' and fullname LIKE '%' + @filtername + '%' order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end 
			ELSE 
			begin
			set @totalMatches = (select count(*)  from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and ftm.MtDistance is not null and mttestname = 'HVR2')

			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and ftm.MtDistance is not null and mttestname = 'HVR2'  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end 
		END
	ELSE
		BEGIN
		if (@filtername is not null) 
			begin
			set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and HVR1MatchIndex=1 and FMSMatchIndex=1 and ftm.MtDistance is not null  and fullname LIKE '%' + @filtername + '%')


			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and HVR1MatchIndex=1 and FMSMatchIndex=1 and ftm.MtDistance is not null  and fullname LIKE '%' + @filtername + '%' order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end
			ELSE 
			begin
			set @totalMatches = (select count(*) from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and HVR1MatchIndex=1 and FMSMatchIndex=1 and ftm.MtDistance is not null )

			select @totalMatches as totalmatches,* from vw_FamilyTreeMatches ftm, kitpreferences kp where 
			ftm.kitnum = @kitnum and ftm.MatchKitNum = kp.Kitnum 
			and kp.HVR2MatchIndex=1 and HVR1MatchIndex=1 and FMSMatchIndex=1 and ftm.MtDistance is not null  order by ftm.MtDistance asc offset @offset rows fetch next @pagesize rows only;
			end
		END
END
