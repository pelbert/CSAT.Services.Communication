SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
/* Migration Script */
create PROCEDURE [dbo].[bp_FamilyTreeMatches_Ydna_Select_FTV]
    @KitNum NVARCHAR(20)
AS
BEGIN
    DECLARE @UserY12Pref BIT = (
                                   SELECT Y12MatchIndex
                                   FROM   dbo.KitPreferences
                                   WHERE  Kitnum = @KitNum
                               );
    DECLARE @UserY25Pref BIT = (
                                   SELECT Y25MatchIndex
                                   FROM   dbo.KitPreferences
                                   WHERE  Kitnum = @KitNum
                               );
    DECLARE @UserY37Pref BIT = (
                                   SELECT Y37MatchIndex
                                   FROM   dbo.KitPreferences
                                   WHERE  Kitnum = @KitNum
                               );
    DECLARE @UserY67Pref BIT = (
                                   SELECT Y67MatchIndex
                                   FROM   dbo.KitPreferences
                                   WHERE  Kitnum = @KitNum
                               );
    DECLARE @UserY111Pref BIT = (
                                    SELECT Y111MatchIndex
                                    FROM   dbo.KitPreferences
                                    WHERE  Kitnum = @KitNum
                                );


    DECLARE @ResultId INT = (
                                SELECT S.ResultId
                                FROM   Results.Summary S WITH (NOLOCK)
                                WHERE  S.KitNum = @KitNum
                            );

    DECLARE @Kits TABLE
        (
            ResultId INT,
            KitNum NVARCHAR(20),
            TestType VARCHAR(10),
            Mutations INT
        );

    DECLARE @matches TABLE
        (
            ResultId INT,
            MatchResultId INT PRIMARY KEY,
            KitNum NVARCHAR(20),
            MatchKitNum NVARCHAR(20),
            Y12Distance INT,
            Y25Distance INT,
            Y37Distance INT,
            Y67Distance INT,
            Y111Distance INT
        );

    INSERT INTO @Kits
    SELECT FYCM.ResultId,
           FYCM.KitNum,
           '12',
           FYCM.Mutations
    FROM   Results.fnc_ydna_cached_matching(@KitNum, 12, -1) [FYCM]
    WHERE  FYCM.MatchInd = 'Y'
           AND @UserY12Pref = 1;

    INSERT INTO @Kits
    SELECT FYCM.ResultId,
           FYCM.KitNum,
           '25',
           FYCM.Mutations
    FROM   Results.fnc_ydna_cached_matching(@KitNum, 25, -1) [FYCM]
    WHERE  FYCM.MatchInd = 'Y'
           AND @UserY25Pref = 1;

    INSERT INTO @Kits
    SELECT FYCM.ResultId,
           FYCM.KitNum,
           '37',
           FYCM.Mutations
    FROM   Results.fnc_ydna_cached_matching(@KitNum, 37, -1) [FYCM]
    WHERE  FYCM.MatchInd = 'Y'
           AND @UserY37Pref = 1;

    INSERT INTO @Kits
    SELECT FYCM.ResultId,
           FYCM.KitNum,
           '67',
           FYCM.Mutations
    FROM   Results.fnc_ydna_cached_matching(@KitNum, 67, -1) [FYCM]
    WHERE  FYCM.MatchInd = 'Y'
           AND @UserY67Pref = 1;

    INSERT INTO @Kits
    SELECT FYCM.ResultId,
           FYCM.KitNum,
           '111',
           FYCM.Mutations
    FROM   Results.fnc_ydna_cached_matching(@KitNum, 111, -1) [FYCM]
    WHERE  FYCM.MatchInd = 'Y'
           AND @UserY111Pref = 1;

    INSERT INTO @matches (
                             ResultId,
                             MatchResultId,
                             KitNum,
                             MatchKitNum
                         )
    SELECT DISTINCT @ResultId,
           K.ResultId,
           @KitNum,
           K.KitNum
    FROM   @Kits K
    EXCEPT
    SELECT M.ResultId,
           M.MatchResultId,
           M.KitNum,
           M.MatchKitNum
    FROM   @matches M;

    UPDATE YM
    SET    YM.Y12Distance = K.Mutations
    FROM   @matches YM
           INNER JOIN @Kits K ON K.ResultId = YM.MatchResultId
    WHERE  K.TestType = '12';

    UPDATE YM
    SET    YM.Y25Distance = K.Mutations
    FROM   @matches YM
           INNER JOIN @Kits K ON K.ResultId = YM.MatchResultId
    WHERE  K.TestType = '25';

    UPDATE YM
    SET    YM.Y37Distance = K.Mutations
    FROM   @matches YM
           INNER JOIN @Kits K ON K.ResultId = YM.MatchResultId
    WHERE  K.TestType = '37';

    UPDATE YM
    SET    YM.Y67Distance = K.Mutations
    FROM   @matches YM
           INNER JOIN @Kits K ON K.ResultId = YM.MatchResultId
    WHERE  K.TestType = '67';

    UPDATE YM
    SET    YM.Y111Distance = K.Mutations
    FROM   @matches YM
           INNER JOIN @Kits K ON K.ResultId = YM.MatchResultId
    WHERE  K.TestType = '111';

    SELECT M.ResultId,
           M.MatchResultId,
           M.KitNum,
           M.MatchKitNum,
		   (
					select 'social-photos/' + convert(varchar(55),se.ProfilePhotoId) from [Storage].[Files] sf
					inner join [Social].[Photos] sp on sf.id = sp.FileId 
					inner join social.entities  se on se.ProfilePhotoId = sp.id 
					inner join users.users u on u.id = se.id 
					inner join aspnetusers aspu on aspu.id = u.MembershipId 
					where UserName = M.MatchKitNum
			   ) as ProfileImageUrl,
           MatchUserId = UK.Id,
           C.ContactId,
           C.FirstName,
           C.MiddleName,
           C.LastName,
           FullName = dbo.fnc_format_fullname(
                                                 C.Prefix,
                                                 C.FirstName,
                                                 C.MiddleName,
                                                 C.LastName,
                                                 C.Suffix
                                             ),
           K.Female,
           M.Y12Distance,
           M.Y25Distance,
           M.Y37Distance,
           M.Y67Distance,
           M.Y111Distance,
           YTestName = CASE WHEN Y111Distance IS NOT NULL THEN 'Y-111'
                            WHEN Y67Distance IS NOT NULL THEN 'Y-67'
                            WHEN Y37Distance IS NOT NULL THEN 'Y-37'
                            WHEN Y25Distance IS NOT NULL THEN 'Y-25'
                            WHEN Y12Distance IS NOT NULL THEN 'Y-12'
                            ELSE NULL
                       END,
           YDistance = CASE WHEN Y111Distance IS NOT NULL THEN Y111Distance
                            WHEN Y67Distance IS NOT NULL THEN Y67Distance
                            WHEN Y37Distance IS NOT NULL THEN Y37Distance
                            WHEN Y25Distance IS NOT NULL THEN Y25Distance
                            WHEN Y12Distance IS NOT NULL THEN Y12Distance
                            ELSE NULL
                       END,
           FamilyTreePeopleId = FTP.Id,
           IsAlreadyInFamilyTree = CASE WHEN EXISTS (
                                                        SELECT *
                                                        FROM   FamilyTreePeople
                                                        WHERE  KitNum = M.KitNum
                                                               AND LinkedContactId = C.ContactId
                                                    ) THEN CONVERT(BIT, 1)
                                        ELSE CONVERT(BIT, 0)
                                   END
    FROM   @matches M
           INNER JOIN dbo.Contacts C WITH (NOLOCK) ON C.KitNum = M.MatchKitNum
                                                      AND C.Active = 1
           INNER JOIN dbo.Kits K WITH (NOLOCK) ON K.KitNum = C.KitNum
           INNER JOIN dbo.KitPreferences kp WITH (NOLOCK) ON K.KitNum = kp.KitNum
           INNER JOIN Users.UserKits UK WITH (NOLOCK) ON UK.KitNum = C.KitNum
           LEFT JOIN dbo.FamilyTreePeople FTP WITH (NOLOCK) ON FTP.KitNum = M.KitNum
                                                               AND FTP.LinkedContactId = C.ContactId;


END;
