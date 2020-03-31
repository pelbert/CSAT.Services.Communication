USE [FTDNA]
GO
/****** Object:  StoredProcedure [dbo].[bp_myftdna_matching_profile_select_set]    Script Date: 2/28/2018 1:43:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[bp_genetic_info_FTV]
    @kitnums NVARCHAR(MAX),
    @ownerkit NVARCHAR(20)
AS
BEGIN

--GET INFO FOR OWNERKIT AND UNION WITH MATCH RESULTS SO WE HAVE AN ITEM TO PULL ORIGIN AND HAPPLOGROUP INFO FOR SINCE THERE WILL BE NO MATCHES FOR OWNER 
--KITNUMS HARDCODED FOR NOW TO ENABLE MOCK DATA FILE TO BE BUILT. 
select top(1) kitnum, ResultId, resultid as resultid2, shorthandhaplogroup as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, kitnum as matchkitnum, null as isXMatch, 
null as familyFinderRelationship, null as isFamilyFinderMatch,null as MtDistance, 0 as isMtdnaMatch  from     [Results].[Summary] rs where kitnum =@ownerkit
union
 SELECT FTM.kitnum,resultid, resultid2, yHaplo as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, FTM.matchkitnum as matchkitnum,isXMatch,LinkedRelationshipName as familyFinderRelationship, ffback as isFamilyFinderMatch,FTM.MtDistance,
  CASE WHEN FTM.MtDistance IS NOT NULL THEN  1 else 0
  end as isMtdnaMatch
  FROM [FTDNA].[dbo].[vw_FamilyTreeMatches] as FTM
FULL OUTER JOIN vw_FFSummary as FFSum ON FTM.KitNum = FFSum.Kitnum and  FTM.matchkitnum = FFSum.MatchKitNum
where FTM.kitnum = @ownerkit and FTM.matchkitnum in   ('9','10','1071','117354','123','123056','128','128777','153785','172519','173700','175133','175759','181082','1820','186684','187621','187697','188517','192589','192590','192591','192592','192593','192594','19282','194823','19736','19886','199610','201712','201716','204063','205586','210446','210813','216215','217725','221741','225812','235497','23996','265404','265406','28768','29659','29736','299807','305610','314967','324323','324910','324912','324914','325428','326690','326870','331535','331558','338','348635','353476','355008','355010','362057','362085','362090','362091','362510','362859','363424','370793','370807','370810','374598','388021','388022','388472','388657','398396','4065','418905','422028','430225','436494','439310','442108','465726','4686','471588','474369','489613','49617','49999','503299','519885','522128','528109','547702','549167','569341','61286','645146','645153','645170','6656','729275','735198','76210','843','9','9141','9201','94615','B150079','B1711','B2849','B40950','B7003','N114427','N13911','N21510','N23339','N89021')

--BELOW ARE FOR MOCK DATA TO GIVE US TWO ADDITIONAL TREES OF DATA. 
union
select top(1) kitnum, ResultId, resultid as resultid2, shorthandhaplogroup as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, kitnum as matchkitnum, null as isXMatch, 
null as familyFinderRelationship, null as isFamilyFinderMatch,null as MtDistance, 0 as isMtdnaMatch  from     [Results].[Summary] rs where kitnum ='843'
union
--FOR MOCK
select top(1) kitnum, ResultId, resultid as resultid2, shorthandhaplogroup as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, kitnum as matchkitnum, null as isXMatch, 
null as familyFinderRelationship, null as isFamilyFinderMatch,null as MtDistance, 0 as isMtdnaMatch  from     [Results].[Summary] rs where kitnum ='305610'
union
 SELECT FTM.kitnum,resultid, resultid2, yHaplo as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, FTM.matchkitnum as matchkitnum,isXMatch,LinkedRelationshipName as familyFinderRelationship, ffback as isFamilyFinderMatch,FTM.MtDistance,
  CASE WHEN FTM.MtDistance IS NOT NULL THEN  1 else 0
  end as isMtdnaMatch
  FROM [FTDNA].[dbo].[vw_FamilyTreeMatches] as FTM
FULL OUTER JOIN vw_FFSummary as FFSum ON FTM.KitNum = FFSum.Kitnum and  FTM.matchkitnum = FFSum.MatchKitNum
 where FTM.kitnum = '843' and FTM.matchkitnum in   ('10','1071','117354','123','123056','128','128777','153785','172519','173700','175133','175759','181082','1820','186684','187621','187697','188517','192589','192590','192591','192592','192593','192594','19282','194823','19736','19886','199610','201712','201716','204063','205586','210446','210813','216215','217725','221741','225812','235497','23996','265404','265406','28768','29659','29736','299807','305610','314967','324323','324910','324912','324914','325428','326690','326870','331535','331558','338','348635','353476','355008','355010','362057','362085','362090','362091','362510','362859','363424','370793','370807','370810','374598','388021','388022','388472','388657','398396','4065','418905','422028','430225','436494','439310','442108','465726','4686','471588','474369','489613','49617','49999','503299','519885','522128','528109','547702','549167','569341','61286','645146','645153','645170','6656','729275','735198','76210','843','9','9141','9201','94615','B150079','B1711','B2849','B40950','B7003','N114427','N13911','N21510','N23339')
union

 SELECT FTM.kitnum,resultid, resultid2, yHaplo as yDnaHaplogroup, mtHaplo as mtDnaHaplogroup, FTM.matchkitnum as matchkitnum,isXMatch,LinkedRelationshipName as familyFinderRelationship, ffback as isFamilyFinderMatch,FTM.MtDistance,
  CASE WHEN FTM.MtDistance IS NOT NULL THEN  1 else 0
  end as isMtdnaMatch
  FROM [FTDNA].[dbo].[vw_FamilyTreeMatches] as FTM
FULL OUTER JOIN vw_FFSummary as FFSum ON FTM.KitNum = FFSum.Kitnum and  FTM.matchkitnum = FFSum.MatchKitNum
--where FTM.kitnum = '9' and FTM.matchkitnum in   ('843','108897','113888','100016','100121','128777','269348','9')  
where FTM.kitnum = '305610' and FTM.matchkitnum in   ('10','1071','117354','123','123056','128','128777','153785','172519','173700','175133','175759','181082','1820','186684','187621','187697','188517','192589','192590','192591','192592','192593','192594','19282','194823','19736','19886','199610','201712','201716','204063','205586','210446','210813','216215','217725','221741','225812','235497','23996','265404','265406','28768','29659','29736','299807','305610','314967','324323','324910','324912','324914','325428','326690','326870','331535','331558','338','348635','353476','355008','355010','362057','362085','362090','362091','362510','362859','363424','370793','370807','370810','374598','388021','388022','388472','388657','398396','4065','418905','422028','430225','436494','439310','442108','465726','4686','471588','474369','489613','49617','49999','503299','519885','522128','528109','547702','549167','569341','61286','645146','645153','645170','6656','729275','735198','76210','843','9','9141','9201','94615','B150079','B1711','B2849','B40950','B7003','N114427','N13911','N21510','N23339','N89021')

END